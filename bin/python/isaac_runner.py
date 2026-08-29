# -*- coding: utf-8 -*-
"""
Isaac Sim 5.1 generic URDF runner for MakeUrdf.

Usage:
    python.bat isaac_runner.py <urdf_path>

Features:
- URDF import (Isaac Sim 5.1 API: omni.kit.commands "URDFParseAndImportFile")
- Continuous joint auto-detection (URDF XML parsing — stage.Traverse not reliable
  before world.reset)
- USD DriveAPI velocity drive setup for continuous joints
- TCP socket server on 127.0.0.1:4000 (MakeUrdf 3D->Sim socket compatible):
      "<time_ms>,<delay>,<id1>,<val1>,<id2>,<val2>,...;"
      "reset;" / "clear;"
      "SCENE_ADD,type,x,y,z,rx,ry,rz,sx,sy,sz,color,mass[,friction];"
  Query commands (reply on the SAME connection, then close):
      "GETPOS[,robot_idx];"  -> "POS,<id>,<angle>,...;"   (deg / prismatic=mm, 실제각)
      "GETCOM[,robot_idx];"  -> "COM,cx,cy,cz,footx,footy,footz,total_kg;"  (전신 COM +
                                 지지발(OJW_COM_FOOT, 기본 link_16) world pos — COM 서보용)
      "GETSCENE;"            -> "SCENE,<idx>,x,y,z,rx,ry,rz;" x N  (spawn 물체 실제 pose)
      "CAM_GET,<name>;"      -> "CAM,<name>,<w>,<h>,<len>;" + JPEG bytes
- Camera attach for every <sensor name="cameraN"> in URDF (REP 0119)
- Multi-robot scaffolding: spawn_robot(urdf_path, robot_idx, position) — first call
  loads the user URDF; later calls clone the prim for RL parallel-env style use

This script intentionally avoids tank-specific kinematic hacks. If you need tank
differential drive use the older tank_driver.py.
"""

from __future__ import annotations

import math
import os
import queue
import socket
import sys
import threading
import time
import xml.etree.ElementTree as _ET

# 한글 Windows (cp949) 에서 유니코드 print 실패 방지
try:
    sys.stdout.reconfigure(encoding='utf-8', errors='replace')
    sys.stderr.reconfigure(encoding='utf-8', errors='replace')
except Exception:
    pass

# ----------------------------------------------------------------------------
# Isaac Sim 5.1 boot — must happen before any omni/isaacsim/pxr import
# ----------------------------------------------------------------------------
from isaacsim import SimulationApp  # type: ignore

CONFIG = {
    # OJW_HEADLESS=1: 창 없이 실행 (자동 검증/CI 용)
    "headless": os.environ.get("OJW_HEADLESS", "0").strip() == "1",
    "renderer": "RaytracedLighting",
    "anti_aliasing": 3,
}
simulation_app = SimulationApp(CONFIG)

# --- Late imports (must come after SimulationApp) ---------------------------
import carb  # noqa: E402
import omni  # noqa: E402
import omni.kit.commands  # noqa: E402
import omni.usd  # noqa: E402
from pxr import Gf, Sdf, Usd, UsdGeom, UsdPhysics  # noqa: E402

# URDF importer — Isaac 6.0 에서 API 전면 개편:
#   5.1: 최상위 _urdf (+ kit command "URDFParseAndImportFile" 로 스테이지 임포트)
#   6.0: URDFImporter/URDFImporterConfig (URDF→USD "파일 변환기", _urdf 최상위 제거,
#        UrdfJointTargetType 은 패키지 전체에서 소멸 → joint_target_type="position" 문자열)
# 새 API 를 먼저 시도하고 없으면 5.1 로 — 사용자 컴퓨터가 어느 버전이든 동작한다.
try:
    from isaacsim.asset.importer.urdf import URDFImporter, URDFImporterConfig  # noqa: E402  (Isaac >= 6.0)
    _urdf = None
    ISAAC6 = True
except ImportError:
    from isaacsim.asset.importer.urdf import _urdf  # noqa: E402  (Isaac <= 5.1)
    URDFImporter = None
    URDFImporterConfig = None
    ISAAC6 = False
print("[Init] URDF importer API: %s" % ("Isaac 6.0+ (URDFImporter)" if ISAAC6 else "Isaac 5.1 (_urdf)"),
      flush=True)

from isaacsim.core.api import World  # noqa: E402
from isaacsim.core.prims import SingleArticulation  # noqa: E402
from isaacsim.core.utils.types import ArticulationAction  # noqa: E402

# ----------------------------------------------------------------------------
# Globals
# ----------------------------------------------------------------------------
WHEEL_RADIUS_M = 0.030     # default, used only by helpers — runner itself is agnostic
SOCKET_HOST = "127.0.0.1"
SOCKET_PORT = int(os.environ.get("OJW_SOCKET_PORT", "4000") or 4000)  # 병렬/테스트용 오버라이드

world: World | None = None
articulations: list[SingleArticulation] = []          # [robot_idx] -> articulation
articulation_paths: list[str] = []                    # [robot_idx] -> root prim path
continuous_joint_names: list[set[str]] = []           # [robot_idx] -> set of names
prismatic_joint_names: list[set[str]] = []            # [robot_idx] -> set of prismatic joint names
camera_specs: list[list[dict]] = []                   # [robot_idx] -> [cam dict, ...]
camera_objs: dict[tuple[int, int], object] = {}       # (robot_idx, cam_idx) -> Camera

# world_fixed 카메라 ($scene 패턴, robot 과 분리) — /World/scene_cameras/ 아래 prim
# camera_specs 와 다른 점: 부모가 articulation 이 아닌 World, 다중 robot 환경에서도 단 1세트만 존재.
world_camera_specs: list[dict] = []                   # world_cameras.json 에서 로드된 dict 리스트
world_camera_objs: dict[str, object] = {}             # name -> Camera prim
_world_cameras_loaded: bool = False                   # 첫 robot spawn 때만 1회 로드

joint_pos_targets: dict[tuple[int, str], float] = {}  # (robot_idx, joint_name) -> rad
joint_pos_origin: dict[tuple[int, str], float] = {}   # interpolation origin
joint_pos_t_start: dict[tuple[int, str], float] = {}  # interpolation start time
joint_pos_t_end: dict[tuple[int, str], float] = {}    # interpolation end time
joint_vel_targets: dict[tuple[int, str], float] = {}  # (robot_idx, joint_name) -> rad/s

# Mimic: follower_name -> (leader_name, multiplier, offset)
# Isaac Sim 의 URDFImporter 는 <mimic> 태그를 무시하므로 (또는 SanitizeUrdfForIsaacSim
# 이 제거함) follower 가 leader 와 함께 움직이도록 socket 단에서 직접 적용한다.
# 메타데이터는 sibling 의 robot_stl.urdf (mimic 보존본) 에서 읽음.
mimic_relationships: list[dict[str, dict]] = []       # [robot_idx] -> {follower: {parent, multiplier, offset}}

# ── 상태 회신 (GETPOS / GETSCENE / CAM_GET) ─────────────────────────────────
# 소켓 스레드에서 USD/physics API 를 직접 부르면 stage lock 데드락 (scene 큐와 동일
# 이유). 메인 루프가 매 프레임 스냅샷을 갱신하고 소켓 스레드는 읽기만 한다.
_state_lock = threading.Lock()

# ============================================================================
# actuator-net 타깃셰이핑 (OJW_ACTNET=<npz 경로>) — 실기 서보 응답 재현
#   보간된 명령 타깃을 학습된 실기 응답(데드밴드/지연/히스테리시스)형 궤적으로 변환.
#   입력 6: [err(t..t-3), Δpos(t), Δpos(t-1)] (deg), 출력: Δpos (20ms 스텝).
#   T6(그리퍼)는 mimic 링크 동기화를 위해 셰이핑 제외.
# ============================================================================
_actnet_models: dict = {}
_actnet_state: dict = {}
_ACTNET_DT = 0.02
# ★로봇-모델 일치 게이트: npz 는 특정 로봇(예: 휴머노이드 T1~T17)용 — urdf 폴더에
#   남아 있으면 델타(T1~T3)처럼 이름만 겹치는 다른 로봇의 모터까지 셰이핑해
#   파괴적 타깃(-180° 등)을 만든다 (2026-08-26 실사고). _post_spawn_init 에서
#   모델 관절의 60% 이상이 실제 존재할 때만 활성.
_actnet_gate = {"on": True}

def _actnet_load() -> None:
    path = os.environ.get("OJW_ACTNET", "").strip()
    if not path:
        return
    if not os.path.isfile(path):
        print("[ActNet] 파일 없음: %s" % path, flush=True)
        return
    try:
        import numpy as _np
        z = _np.load(path)
        for nm in sorted(set(k.split("_")[0] for k in z.files)):
            try:
                _actnet_models[nm] = (z[nm + "_W0"], z[nm + "_b0"], z[nm + "_W1"],
                                      z[nm + "_b1"], z[nm + "_W2"], z[nm + "_b2"])
            except Exception:
                pass
        for _sk in os.environ.get("OJW_ACTNET_SKIP", "").split(","):
            _sk = _sk.strip()
            if _sk:
                _actnet_models.pop(_sk, None)   # 예: OMX 그리퍼 T6 (mimic 동기화)
        print("[ActNet] loaded %s: %s" % (path, sorted(_actnet_models.keys())), flush=True)
    except Exception as ex:
        print("[ActNet] load 실패: %s" % ex, flush=True)

_actnet_load()

def _actnet_shape(key, jname: str, ridx: int, cmd_rad: float, dt: float) -> float:
    """보간 명령(rad) → 실기형 위치(rad). 모델 없으면 원값 통과."""
    mdl = _actnet_models.get(jname)
    if mdl is None:
        return cmd_rad
    import numpy as _np
    g_deg = math.degrees(cmd_rad)
    st = _actnet_state.get(key)
    if st is None:
        p0 = g_deg
        try:   # 실제 관절각 스냅샷으로 초기화 (있으면)
            if len(jname) > 1 and jname[0] == 'T' and jname[1:].isdigit():
                mid = int(jname[1:])
                with _state_lock:
                    rows = list(_joint_state_snapshot.get(ridx, []))
                for rid, rval in rows:
                    if rid == mid:
                        p0 = rval
                        break
        except Exception:
            pass
        st = {"g": [g_deg] * 4, "p": [p0] * 4, "acc": 0.0, "tw": time.time()}
        _actnet_state[key] = st
    # 벽시계 기준 누적 — 명령 보간(time.time())/실기 데이터(20ms 실측)와 시간축 일치.
    # 물리 dt(시뮬 시간) 기준이면 시뮬이 실시간보다 느릴 때 응답이 늘어짐.
    _tw = time.time()
    st["acc"] += min(0.1, _tw - st["tw"])
    st["tw"] = _tw
    W0, b0, W1, b1, W2, b2 = mdl
    while st["acc"] >= _ACTNET_DT:
        st["acc"] -= _ACTNET_DT
        g = st["g"]; p = st["p"]
        st["g"] = [g_deg, g[0], g[1], g[2]]
        g = st["g"]
        feats = [g[0] - p[0], g[1] - p[1], g[2] - p[2], g[3] - p[3],
                 p[0] - p[1], p[1] - p[2]]
        if W0.shape[1] >= 7:
            feats.append(p[0] / 90.0)   # 자세(중력) 특징 — 7입력 모델
        x = _np.array(feats, dtype="float32")
        h = _np.tanh(W0.dot(x) + b0)
        h = _np.tanh(W1.dot(h) + b1)
        d = float(W2.dot(h) + b2)
        st["p"] = [p[0] + d, p[0], p[1], p[2]]
    return math.radians(st["p"][0])

_joint_state_snapshot: dict[int, list] = {}   # ridx -> [(motor_id, deg|mm)] — 실제 관절각
_scene_state_snapshot: list = []              # [(idx, (x, y, z, rx, ry, rz))] — spawn 물체 pose
_cam_requests = queue.Queue()                 # (camera_name, threading.Event, result_dict)


# ============================================================================
# URDF helpers
# ============================================================================
def _parse_continuous_joints_from_urdf(urdf_path: str) -> set[str]:
    """Return set of continuous joint names by direct URDF XML parse.

    USD stage.Traverse() before world.reset() returns 0 joints and
    robot.dof_properties['hasLimits'] is unreliable in 5.1 — XML parse is the
    safest path."""
    names: set[str] = set()
    try:
        tree = _ET.parse(urdf_path)
        for j in tree.getroot().findall("joint"):
            if j.get("type", "") == "continuous":
                nm = j.get("name", "")
                if nm:
                    names.add(nm)
    except Exception as ex:
        print("[Init] continuous-joint parse failed: %s" % ex, flush=True)
    return names


def _parse_prismatic_joints_from_urdf(urdf_path: str) -> set[str]:
    """Return set of prismatic joint names by direct URDF XML parse.

    MakeUrdf socket protocol 은 모든 모터 값을 같은 형식으로 전송하지만,
    revolute 는 degree 단위, prismatic 은 **mm 단위** 로 보낸다 (3D viewer 내부).
    Isaac/USD prismatic 타겟은 m 단위이므로 mm → m 변환 (× 0.001) 이 필요하다.
    이 set 에 포함된 조인트는 _handle_command 에서 별도 처리된다."""
    names: set[str] = set()
    try:
        tree = _ET.parse(urdf_path)
        for j in tree.getroot().findall("joint"):
            if j.get("type", "") == "prismatic":
                nm = j.get("name", "")
                if nm:
                    names.add(nm)
    except Exception as ex:
        print("[Init] prismatic-joint parse failed: %s" % ex, flush=True)
    return names


def _parse_mimic_from_urdf(urdf_path: str) -> dict[str, dict]:
    """Return mimic relationships as {follower_name: {'parent', 'multiplier', 'offset'}}.

    Isaac Sim 의 URDFImporter 는 <mimic> 태그를 USD constraint 로 변환하지 않으므로
    socket 단에서 직접 leader→follower 값을 전파해야 한다.
    SanitizeUrdfForIsaacSim 이 isaac_urdf 에서 <mimic> 을 제거하므로, sibling 의
    robot_stl.urdf (mimic 보존본) 을 읽어 메타데이터 추출.
    """
    info: dict[str, dict] = {}
    # urdf_path 이 robot_stl_isaac.urdf 이면 sibling robot_stl.urdf 를 우선 시도.
    candidates = []
    base_dir = os.path.dirname(os.path.abspath(urdf_path))
    candidates.append(os.path.join(base_dir, "robot_stl.urdf"))
    candidates.append(urdf_path)  # fallback: 같은 파일에 mimic 이 남아있을 수도
    for path in candidates:
        if not os.path.exists(path):
            continue
        try:
            tree = _ET.parse(path)
            for j in tree.getroot().findall("joint"):
                m = j.find("mimic")
                if m is None:
                    continue
                jname = j.get("name", "")
                parent = m.get("joint", "")
                if not jname or not parent:
                    continue
                try:
                    mult = float(m.get("multiplier", "1.0"))
                except Exception:
                    mult = 1.0
                try:
                    off = float(m.get("offset", "0.0"))
                except Exception:
                    off = 0.0
                info[jname] = {"parent": parent, "multiplier": mult, "offset": off}
            if info:
                print("[Mimic] parsed %d mimic joints from %s" % (len(info), os.path.basename(path)), flush=True)
                break
        except Exception as ex:
            print("[Mimic] parse failed for %s: %s" % (path, ex), flush=True)
    return info


def _parse_scenes_from_urdf(urdf_path: str) -> list[dict]:
    """Return list of <scene><include .../></scene> entries."""
    items: list[dict] = []
    try:
        tree = _ET.parse(urdf_path)
        for sc in tree.getroot().findall("scene"):
            for inc in sc.findall("include"):
                url = inc.get("url", "")
                if not url:
                    continue
                try:
                    xyz = [float(v) for v in inc.get("xyz", "0 0 0").split()]
                    rpy = [float(v) for v in inc.get("rpy", "0 0 0").split()]
                    scale = float(inc.get("scale", "1.0"))
                except Exception:
                    continue
                color_str = inc.get("color", "")
                color = None
                if color_str:
                    try:
                        cv = int(color_str, 0) if color_str.lower().startswith("0x") else int(color_str)
                        color = (((cv >> 16) & 0xFF) / 255.0,
                                 ((cv >> 8) & 0xFF) / 255.0,
                                 (cv & 0xFF) / 255.0)
                    except Exception:
                        color = None
                items.append({"url": url, "xyz": xyz, "rpy": rpy, "scale": scale, "color": color})
    except Exception as ex:
        print("[Init] scene parse failed: %s" % ex, flush=True)
    return items


def _scene_cube_half_size(url: str, scale: float) -> tuple[float, float, float]:
    u = url.lower()
    if "cube_small" in u or "cube" in u:
        s = 0.02 * scale
    elif "tray" in u:
        return (0.10 * scale, 0.075 * scale, 0.01 * scale)
    else:
        s = 0.02 * scale
    return (s, s, s)


# === Runtime $scene placement (socket SCENE_ADD / SCENE_CLEAR / SCENE_TOOL_SET) ===
#   MakeUrdf Scene 탭에서 socket 으로 명령이 들어옴.
#   _spawn_scene_runtime() 가 dict obj 받아서 USD prim 생성.
#   _clear_scene_runtime() 가 모든 spawn prim 제거.
_scene_active_tool = None       # dict: type, sx, sy, sz, color, mass, zoffset (현재는 미사용; viewer click 확장 시 사용)
_base_state_snapshot: dict = {}   # ridx -> (x,y,z,rx,ry,rz) 로봇 베이스 world pose
_imu_state_snapshot: dict = {}    # ridx -> 몸통(IMU 등가) 링크 world pose
_imu_prim_cache: dict = {}
_imu_dbg_once = False
_IMU_LINK = os.environ.get("OJW_IMU_LINK", "link_2").strip()
_IMU_JOINT = os.environ.get("OJW_IMU_JOINT", "T17").strip()
_com_state_snapshot: dict = {}    # ridx -> (comx,comy,comz,footx,footy,footz,total_kg)
_com_prim_cache: dict = {}        # ridx -> [(prim, mass, com_local Gf.Vec3d), ...]
_com_foot_cache: dict = {}        # ridx -> 지지발 링크 prim
_COM_FOOT = os.environ.get("OJW_COM_FOOT", "link_16").strip()   # 한다리 지지발 (R)
_scene_spawned_paths = []       # 생성된 prim path 누적 (GETSCENE 의 idx = 이 리스트 인덱스)
_scene_material_paths = []      # friction 용 physics material prim (clear 시 함께 제거,
                                #  GETSCENE 인덱스 오염 방지를 위해 spawned 와 분리)
_scene_spawn_counter = [0]      # naming index (mutable container for nonlocal)

# Socket 스레드에서 USD/PXR API 를 직접 호출하면 Isaac Sim 5.1 stage lock 과 충돌해
# 데드락(응답없음)이 발생함. 워커 스레드는 큐에만 넣고, _process_scene_queue() 가
# 메인 스레드(physics step callback) 에서 drain.
_scene_cmd_queue = queue.Queue()  # items: ("add", obj_dict) | ("clear", None)


def _process_scene_queue() -> None:
    """메인 스레드에서 호출 — 큐에 쌓인 scene 명령을 모두 처리."""
    while True:
        try:
            op, payload = _scene_cmd_queue.get_nowait()
            if op == "reset_sim":
                try:
                    joint_pos_targets.clear(); joint_vel_targets.clear()
                    joint_pos_origin.clear(); joint_pos_t_start.clear(); joint_pos_t_end.clear()
                    _actnet_state.clear()
                    world.reset()
                    # world.reset() 은 수동 생성 articulation 의 physics view 를 무효화
                    # → 재초기화 필수 (아니면 GETPOS 빈값/명령 무시 — 조용한 죽음)
                    for _a2 in articulations:
                        if _a2 is None:
                            continue
                        try:
                            _a2.initialize()
                        except Exception as _iex2:
                            print("[Sim] art re-init failed: %s" % _iex2, flush=True)
                    _imu_prim_cache.clear()
                    print("[Sim] RESETSIM - world.reset() + art re-init", flush=True)
                except Exception as _rex:
                    print("[Sim] RESETSIM failed: %s" % _rex, flush=True)
                continue
        except queue.Empty:
            return
        try:
            if op == "add":
                _spawn_scene_runtime(payload)
            elif op == "clear":
                _clear_scene_runtime()
        except Exception as ex:
            print("[Scene] queue process error (%s): %s" % (op, ex), flush=True)


def _bind_friction_material(stage, prim, prim_path: str, friction) -> None:
    """SCENE_ADD 의 optional friction → physics material (static=dynamic=friction)
       생성 후 prim 에 purpose="physics" 바인딩. 자식 collision 에도 상속됨
       (weakerThanDescendants). friction None 이면 no-op (기존 기본값 유지)."""
    if friction is None:
        return
    try:
        from pxr import UsdShade, UsdPhysics
        f = float(friction)
        mat_path = prim_path + "_physmat"
        mat = UsdShade.Material.Define(stage, mat_path)
        pm = UsdPhysics.MaterialAPI.Apply(mat.GetPrim())
        pm.CreateStaticFrictionAttr().Set(f)
        pm.CreateDynamicFrictionAttr().Set(f)
        UsdShade.MaterialBindingAPI.Apply(prim).Bind(
            mat, UsdShade.Tokens.weakerThanDescendants, "physics")
        _scene_material_paths.append(mat_path)
        print("[Scene] friction=%.3f bound -> %s" % (f, prim_path), flush=True)
    except Exception as ex:
        print("[Scene] friction bind 실패: %s" % ex, flush=True)


def _spawn_scene_runtime(obj: dict) -> None:
    """obj dict (PyBullet 와 동일 schema) → Isaac USD prim 생성.
       type: cube/box/sphere/cylinder/asset:tray etc.
       physics: mass>0 면 RigidBody + Mass + Collision API 추가.
       obj["friction"] (optional) 있으면 physics material 바인딩."""
    global _scene_spawned_paths
    try:
        from pxr import UsdGeom, UsdPhysics, Gf
        stage = omni.usd.get_context().get_stage()
        if stage is None:
            print("[Scene] stage 가 None — spawn 불가", flush=True)
            return

        idx = _scene_spawn_counter[0]
        _scene_spawn_counter[0] += 1
        prim_path = "/World/scene_rt_%d_%s" % (idx, obj["type"].replace(":", "_"))

        t = obj["type"]
        sx, sy, sz = float(obj["sx"]), float(obj["sy"]), float(obj["sz"])
        col = int(obj["color"])
        rgb = (((col >> 16) & 0xFF) / 255.0, ((col >> 8) & 0xFF) / 255.0, (col & 0xFF) / 255.0)
        mass = float(obj["mass"])

        # 1) 도형 생성 — type 별 분기

        # asset:can — YCB 005_tomato_soup_can USD reference (실제 텍스처 작은 캔)
        # NVIDIA Omniverse 호스트 자산을 prim 으로 reference. 다운로드 실패/오프라인
        # 시 cylinder 로 graceful fallback.
        # 주의: YCB Axis_Aligned 자산은 Y-up 이 native — Z-up 인 Isaac 스테이지에
        # 그대로 reference 하면 누워서 로딩됨. RotateXYZ(+90°,0,0) 보정 필요.
        if t == "asset:can":
            try:
                # Isaac 5.x 표준 API → fallback chain
                assets_root = None
                try:
                    from isaacsim.storage.native import get_assets_root_path
                    assets_root = get_assets_root_path()
                except Exception:
                    try:
                        from omni.isaac.core.utils.nucleus import get_assets_root_path
                        assets_root = get_assets_root_path()
                    except Exception:
                        pass

                if assets_root:
                    # _Physics 버전: collision/mass 사전 설정됨. 없으면 일반 버전.
                    candidates = [
                        "/Isaac/Props/YCB/Axis_Aligned_Physics/005_tomato_soup_can.usd",
                        "/Isaac/Props/YCB/Axis_Aligned/005_tomato_soup_can.usd",
                    ]
                    can_url = assets_root + candidates[0]

                    # add_reference_to_stage 로 prim 생성 + 외부 USD 참조
                    try:
                        from isaacsim.core.utils.stage import add_reference_to_stage
                    except Exception:
                        from omni.isaac.core.utils.stage import add_reference_to_stage
                    add_reference_to_stage(usd_path=can_url, prim_path=prim_path)

                    # 참조된 prim 의 Xform 으로 위치/회전 적용
                    ref_prim = stage.GetPrimAtPath(prim_path)
                    if ref_prim and ref_prim.IsValid():
                        # YCB Axis_Aligned: native ~66×102mm, Y 가 cylinder 의 height 축
                        # (Y-up). 50×80mm 으로 축소해서 작은 그립용 캔.
                        #   ratio_xy = target_d / native_d
                        #   ratio_z  = target_h / native_h  (rotate 후 Z 축이 height)
                        # USD op order = [Translate, Scale, Rotate] → matrix T*S*R.
                        # 적용 순서: Rotate(Y→Z 정렬) → Scale(Z-up frame 기준) → Translate.
                        # 따라서 obj["z"] = sy/2 = scaled_height/2 → 바닥 정확 안착.
                        native_d = 0.066
                        native_h = 0.102
                        ratio_xy = max(sx / native_d, 0.1) if sx > 0 else 1.0
                        ratio_z  = max(sy / native_h, 0.1) if sy > 0 else 1.0

                        x_xform = UsdGeom.Xformable(ref_prim)
                        x_xform.ClearXformOpOrder()
                        # 1) 가장 outermost — 최종 위치 (월드 좌표)
                        x_xform.AddTranslateOp().Set(
                            Gf.Vec3d(float(obj["x"]), float(obj["y"]), float(obj["z"])))
                        # 2) 중간 — Z-up frame 에서 비등방 scale
                        x_xform.AddScaleOp().Set(Gf.Vec3f(ratio_xy, ratio_xy, ratio_z))
                        # 3) innermost — Y-up → Z-up 보정 (+90° X)
                        #    + 사용자 회전 obj["rx,ry,rz"] 누적 (현재 (0,0,0) 이므로 영향 X).
                        x_xform.AddRotateXYZOp().Set(
                            Gf.Vec3f(90.0 + float(obj["rx"]),
                                     float(obj["ry"]),
                                     float(obj["rz"])))

                        # _Physics 버전은 이미 mass/collision 가 있음. 사용자 mass 가
                        # 명시되면 override.
                        if mass > 0:
                            try:
                                UsdPhysics.RigidBodyAPI.Apply(ref_prim)
                                mass_api = UsdPhysics.MassAPI.Apply(ref_prim)
                                mass_api.GetMassAttr().Set(mass)
                            except Exception:
                                pass

                        _bind_friction_material(stage, ref_prim, prim_path, obj.get("friction"))
                        _scene_spawned_paths.append(prim_path)
                        print("[Scene] +can(YCB mini) @ (%.3f,%.3f,%.3f) "
                              "ratio_xy=%.2f ratio_z=%.2f rot=(90+rx, ry, rz) → %s" %
                              (obj["x"], obj["y"], obj["z"], ratio_xy, ratio_z, can_url),
                              flush=True)
                        return
                    else:
                        print("[Scene] can: ref prim invalid (%s) — fallback to cylinder" %
                              can_url, flush=True)
                else:
                    print("[Scene] can: assets_root 미해결 — cylinder fallback", flush=True)
            except Exception as ex:
                print("[Scene] can asset load 실패 (%s) — cylinder fallback" % ex, flush=True)
            # ↓ fallthrough: cylinder 처리로 위임 (아래 분기에서 처리되도록 t 변경)
            t = "cylinder"
            # cylinder 가 sx=직경, sy=높이 기대 → 그대로 OK
            # 색상은 obj["color"] 그대로 (Main.cs 에서 진홍색)

        # asset:tray 는 받침판 + 4 벽을 Xform 자식으로 묶어서 그릇 모양 생성.
        # (PyBullet 의 tray.urdf 와 시각적으로 유사하게)
        if t == "asset:tray":
            # 외곽 크기: sx × sy × sz (default 0.20 × 0.15 × 0.02)
            wall_h = max(sz * 1.5, 0.02)   # 벽 높이 (받침의 1.5배, 최소 20mm)
            wall_t = max(min(sx, sy) * 0.05, 0.004)  # 벽 두께 (짧은 변의 5%, 최소 4mm)
            base_t = max(sz * 0.5, 0.005)  # 받침 두께

            # 그룹용 Xform 정의 (이게 _scene_spawned_paths 에 등록 → clear 시 children 다 삭제)
            xform_prim = UsdGeom.Xform.Define(stage, prim_path)
            x_xform = UsdGeom.Xformable(xform_prim)
            x_xform.ClearXformOpOrder()
            x_xform.AddTranslateOp().Set(Gf.Vec3d(float(obj["x"]), float(obj["y"]), float(obj["z"])))
            x_xform.AddRotateXYZOp().Set(Gf.Vec3f(float(obj["rx"]), float(obj["ry"]), float(obj["rz"])))

            # 받침 + 4 벽 (각자 unit cube + scale)
            #   piece: (local_offset_xyz, size_xyz)
            half_x = sx / 2.0
            half_y = sy / 2.0
            pieces = [
                ("base",  (0.0,            0.0,            0.0),               (sx,           sy,           base_t)),
                ("wall_n",(0.0,            half_y - wall_t/2.0, wall_h/2.0),   (sx,           wall_t,       wall_h)),
                ("wall_s",(0.0,           -half_y + wall_t/2.0, wall_h/2.0),   (sx,           wall_t,       wall_h)),
                ("wall_e",(half_x - wall_t/2.0,  0.0,           wall_h/2.0),   (wall_t,       sy,           wall_h)),
                ("wall_w",(-half_x + wall_t/2.0, 0.0,           wall_h/2.0),   (wall_t,       sy,           wall_h)),
            ]
            for pname, poff, psize in pieces:
                piece_path = "%s/%s" % (prim_path, pname)
                cube = UsdGeom.Cube.Define(stage, piece_path)
                cube.GetSizeAttr().Set(1.0)
                cx = UsdGeom.Xformable(cube)
                cx.ClearXformOpOrder()
                cx.AddTranslateOp().Set(Gf.Vec3d(*poff))
                cx.AddScaleOp().Set(Gf.Vec3f(*psize))
                try:
                    cube.GetDisplayColorAttr().Set([Gf.Vec3f(*rgb)])
                except Exception:
                    pass
                try:
                    cube_prim = cube.GetPrim()
                    UsdPhysics.CollisionAPI.Apply(cube_prim)
                except Exception:
                    pass

            # tray 자체 physics (mass>0 이면 root Xform 에 RigidBody)
            try:
                xprim = xform_prim.GetPrim()
                if mass > 0:
                    UsdPhysics.RigidBodyAPI.Apply(xprim)
                    mass_api = UsdPhysics.MassAPI.Apply(xprim)
                    mass_api.GetMassAttr().Set(mass)
            except Exception as pex:
                print("[Scene] tray physics apply 실패: %s" % pex, flush=True)

            _bind_friction_material(stage, xform_prim.GetPrim(), prim_path, obj.get("friction"))
            _scene_spawned_paths.append(prim_path)
            print("[Scene] +tray @ (%.3f,%.3f,%.3f) size=(%.3f,%.3f,%.3f) → %s" %
                  (obj["x"], obj["y"], obj["z"], sx, sy, sz, prim_path), flush=True)
            return

        if t in ("cube", "box"):
            prim = UsdGeom.Cube.Define(stage, prim_path)
            prim.GetSizeAttr().Set(1.0)   # unit cube; scale 로 크기 조정
            scale = Gf.Vec3f(sx, sy, sz)
        elif t == "sphere":
            prim = UsdGeom.Sphere.Define(stage, prim_path)
            r = max(sx, 0.001) / 2.0
            prim.GetRadiusAttr().Set(r)
            scale = Gf.Vec3f(1.0, 1.0, 1.0)
        elif t == "cylinder":
            prim = UsdGeom.Cylinder.Define(stage, prim_path)
            r = max(sx, 0.001) / 2.0
            h = max(sy, 0.001)
            prim.GetRadiusAttr().Set(r)
            prim.GetHeightAttr().Set(h)
            prim.GetAxisAttr().Set("Z")
            scale = Gf.Vec3f(1.0, 1.0, 1.0)
        else:
            # asset:* (tray 외) / unknown → 작은 cube 로 fallback
            prim = UsdGeom.Cube.Define(stage, prim_path)
            prim.GetSizeAttr().Set(1.0)
            uniform = max(sx, sy, sz, 0.01)
            scale = Gf.Vec3f(uniform, uniform, uniform)
            if t.startswith("asset:"):
                print("[Scene] asset: 매핑 미구현 — cube 로 대체 (%s)" % t, flush=True)

        # 2) Transform (translate + scale + rotate)
        xform = UsdGeom.Xformable(prim)
        xform.ClearXformOpOrder()
        xform.AddTranslateOp().Set(Gf.Vec3d(float(obj["x"]), float(obj["y"]), float(obj["z"])))
        xform.AddRotateXYZOp().Set(Gf.Vec3f(float(obj["rx"]), float(obj["ry"]), float(obj["rz"])))
        xform.AddScaleOp().Set(scale)

        # 3) 색상
        try:
            prim.GetDisplayColorAttr().Set([Gf.Vec3f(*rgb)])
        except Exception:
            pass

        # 4) Physics — mass>0 면 RigidBody+Mass+Collision, 아니면 Collision 만 (static)
        try:
            primPrim = prim.GetPrim()
            UsdPhysics.CollisionAPI.Apply(primPrim)
            if mass > 0:
                UsdPhysics.RigidBodyAPI.Apply(primPrim)
                mass_api = UsdPhysics.MassAPI.Apply(primPrim)
                mass_api.GetMassAttr().Set(mass)
        except Exception as pex:
            print("[Scene] physics API apply 실패: %s" % pex, flush=True)

        _bind_friction_material(stage, prim.GetPrim(), prim_path, obj.get("friction"))
        _scene_spawned_paths.append(prim_path)
        print("[Scene] +%s @ (%.3f,%.3f,%.3f) mass=%.2f → %s" %
              (t, obj["x"], obj["y"], obj["z"], mass, prim_path), flush=True)
    except Exception as ex:
        print("[Scene] _spawn_scene_runtime error: %s" % ex, flush=True)


def _clear_scene_runtime() -> None:
    """지금까지 _spawn_scene_runtime 으로 만든 prim (+friction material) 모두 제거."""
    global _scene_spawned_paths, _scene_material_paths
    try:
        stage = omni.usd.get_context().get_stage()
        if stage is None:
            return
        n = 0
        for path in _scene_spawned_paths + _scene_material_paths:
            try:
                stage.RemovePrim(path)
                n += 1
            except Exception:
                pass
        _scene_spawned_paths = []
        _scene_material_paths = []
        print("[Scene] cleared %d prim(s)" % n, flush=True)
    except Exception as ex:
        print("[Scene] _clear_scene_runtime error: %s" % ex, flush=True)


def _spawn_scene_objects(urdf_path: str) -> None:
    """Spawn box primitive USD prims for each <scene><include .../></scene>."""
    if _is_usd_path(urdf_path):
        return   # .usda 는 XML 아님 — scene 노드는 URDF 전용 (파싱 시도하면 syntax error)
    items = _parse_scenes_from_urdf(urdf_path)
    if not items:
        return
    stage = omni.usd.get_context().get_stage()
    for idx, it in enumerate(items):
        path = "/World/scene_obj_%d" % idx
        try:
            cube = UsdGeom.Cube.Define(stage, path)
            sx, sy, sz = _scene_cube_half_size(it["url"], it["scale"])
            # UsdGeom.Cube has unit edge (size attribute = full edge of unit cube before scale).
            # Use Xformable scale to control half-extents.
            cube.GetSizeAttr().Set(2.0 * max(sx, sy, sz))  # fallback uniform
            xform = UsdGeom.Xformable(cube)
            xform.AddTranslateOp().Set(Gf.Vec3d(*it["xyz"]))
            # scale per axis (so anisotropic boxes like tray work)
            uniform_half = max(sx, sy, sz)
            if uniform_half > 0:
                xform.AddScaleOp().Set(Gf.Vec3f(sx / uniform_half, sy / uniform_half, sz / uniform_half))
            # color
            if it["color"] is not None:
                try:
                    cube.GetDisplayColorAttr().Set([Gf.Vec3f(*it["color"])])
                except Exception:
                    pass
            print("[Scene] %s @ %s (%.3f,%.3f,%.3f)" % (it["url"], path, *it["xyz"]), flush=True)
        except Exception as ex:
            print("[Scene] spawn failed for %s: %s" % (it["url"], ex), flush=True)


def _parse_world_cameras_json(urdf_path: str) -> list[dict]:
    """world_cameras.json 파싱 (urdf_path 와 같은 폴더). 없으면 빈 리스트.

    JSON 포맷은 COjwUrdfBuilder_t.BuildWorldCamerasJson() 출력과 일치:
      {"cameras": [{"idx", "name", "pose":{x,y,z,roll,pitch,yaw}, "intrinsic":{fov_deg,width,height,near,far}, "update_rate"}, ...]}

    Returns: list of dict (name, pose, hfov_rad, w, h, near, far, rate).
    world camera 는 robot 과 무관 — Isaac 에서 /World/scene_cameras/ 아래 spawn.
    """
    import json as _json
    cams: list[dict] = []
    json_path = os.path.join(os.path.dirname(urdf_path), "world_cameras.json")
    if not os.path.exists(json_path):
        return cams
    try:
        # utf-8-sig — BOM 이 있어도 안전하게 파싱
        with open(json_path, 'r', encoding='utf-8-sig') as f:
            data = _json.load(f)
        for entry in data.get("cameras", []):
            pose = entry.get("pose", {})
            intr = entry.get("intrinsic", {})
            cams.append({
                "name":  entry.get("name", "scene_cam%d" % int(entry.get("idx", 0))),
                "x":     float(pose.get("x", 0.0)),
                "y":     float(pose.get("y", 0.0)),
                "z":     float(pose.get("z", 0.0)),
                "roll":  float(pose.get("roll", 0.0)),
                "pitch": float(pose.get("pitch", 0.0)),
                "yaw":   float(pose.get("yaw", 0.0)),
                "hfov_rad": float(intr.get("fov_deg", 60.0)) * 3.141592653589793 / 180.0,
                "w":     int(intr.get("width", 512)),
                "h":     int(intr.get("height", 512)),
                "near":  float(intr.get("near", 0.05)),
                "far":   float(intr.get("far", 100.0)),
                "rate":  int(entry.get("update_rate", 30)),
            })
        print("[WorldCam] loaded %d from %s" % (len(cams), json_path), flush=True)
    except Exception as ex:
        print("[WorldCam] parse failed (%s): %s" % (json_path, ex), flush=True)
    return cams


def _parse_cameras_from_urdf(urdf_path: str) -> list[dict]:
    """Return list of camera dicts from <sensor> elements (REP 0119)."""
    cams: list[dict] = []
    try:
        tree = _ET.parse(urdf_path)
        for s in tree.getroot().findall("sensor"):
            cam = s.find("camera")
            if cam is None:
                continue
            img = cam.find("image")
            parent = s.find("parent")
            if img is None or parent is None:
                continue
            try:
                hfov = float(img.get("hfov", "1.047"))
                w = int(img.get("width", "640"))
                h = int(img.get("height", "480"))
                near = float(img.get("near", "0.05"))
                far = float(img.get("far", "10.0"))
                rate = int(s.get("update_rate", "30"))
            except Exception:
                continue
            cams.append({
                "name": s.get("name", ""),
                "link": parent.get("link", ""),
                "hfov_rad": hfov,
                "w": w, "h": h,
                "near": near, "far": far,
                "rate": rate,
            })
    except Exception as ex:
        print("[Init] camera parse failed: %s" % ex, flush=True)
    return cams


def _set_joint_to_velocity_drive(stage, joint_path: str) -> None:
    """Switch a joint's drive from position->velocity (stiffness=0, damping=1e5)."""
    prim = stage.GetPrimAtPath(joint_path)
    if not prim or not prim.IsValid():
        return
    drive = UsdPhysics.DriveAPI.Apply(prim, "angular")
    drive.CreateTypeAttr("force")
    drive.GetStiffnessAttr().Set(0.0)
    drive.GetDampingAttr().Set(1e5)


def _tune_prismatic_joint_drive(stage, joint_path: str,
                                stiffness: float = 5e7,
                                damping: float = 5e7,
                                max_force: float = 1e9) -> None:
    """Prismatic joint 의 linear DriveAPI 를 강한 damping 위주로 튜닝.

    revolute 와 prismatic 은 같은 1e9/1e6 stiffness/damping 을 쓰면 단위 의미가
    다르다:
        revolute : N·m/rad  /  N·m·s/rad
        prismatic: N/m      /  N·s/m

    출렁임이 길게 지속 = under-damped. damping 을 stiffness 와 비슷하거나 더 크게
    잡아서 over-damped 영역으로 강제. 1kg~100kg 범위 모두 안정.
        m = 10 kg, k = 5e7 N/m → critical damping = 2·sqrt(5e7·10) ≈ 44721
        damping = 5e7 → 1100× critical → 매우 over-damped, 진동 거의 0

    또한 USD `CreateXAttr(value)` 는 속성이 이미 존재하면 값을 **갱신하지 않음**.
    URDF importer 가 default 로 미리 만들어 둔 stiffness/damping/type 을 강제로
    덮어쓰려면 .Set() 를 명시적으로 호출해야 한다 (이전 버전 버그).
    """
    prim = stage.GetPrimAtPath(joint_path)
    if not prim or not prim.IsValid():
        print("[Tune] prim 무효: %s" % joint_path, flush=True)
        return
    try:
        drive = UsdPhysics.DriveAPI.Apply(prim, "linear")
        # 명시적 Set — Create*Attr 는 이미 있으면 값 무시함.
        drive.CreateTypeAttr().Set("force")
        drive.CreateStiffnessAttr().Set(stiffness)
        drive.CreateDampingAttr().Set(damping)
        drive.CreateMaxForceAttr().Set(max_force)
        # 검증 출력 — 실제로 어떤 값이 박혔는지 확인 가능.
        print("[Tune]   %s → stiff=%g damp=%g maxF=%g type=%s" %
              (joint_path,
               drive.GetStiffnessAttr().Get(),
               drive.GetDampingAttr().Get(),
               drive.GetMaxForceAttr().Get(),
               drive.GetTypeAttr().Get()), flush=True)
    except Exception as ex:
        print("[Tune] prismatic drive set 실패 (%s): %s" % (joint_path, ex), flush=True)


# ============================================================================
# Robot spawn
# ============================================================================
def _is_usd_path(path: str) -> bool:
    """확장자로 USD/USDA/USDC 여부 판정."""
    ext = os.path.splitext(path)[1].lower()
    return ext in (".usd", ".usda", ".usdc")


# Isaac 6.0 변환 캐시 — 같은 URDF 는 1회만 변환 (멀티로봇 스폰 대비)
_isaac6_usd_cache: dict = {}
_isaac6_dir_cleaned = [False]     # _isaac6_usd 폴더 청소는 프로세스당 1회
_reload_pending = [False]         # RELOAD 소켓 명령 → 메인 루프가 처리


def _strip_world_weld_for_isaac6(urdf_path: str) -> str:
    """URDF 의 <link name="world"/> + world 를 parent 로 하는 fixed joint 를
    제거한 사본 경로를 반환 (없으면 원본 그대로).

    ★정책(2026-07 확립): fixed base 는 export 에 굽지 않고 러너가 스폰 위치를
    앵커로 author 한다 (_load_usd_as_reference 의 OJW_FIXED_BASE). URDF 에 world
    용접이 남아 있으면 6.0 변환기가 그대로 보존해 로봇이 스폰 좌표(공중)에
    용접 고정된다 — "SPAWN_Z 만큼 허공에 뜬 팔" 의 원인."""
    try:
        tree = _ET.parse(urdf_path)
        root = tree.getroot()
        removed = False
        for link in list(root.findall("link")):
            if link.get("name") == "world":
                root.remove(link)
                removed = True
        for j in list(root.findall("joint")):
            p = j.find("parent")
            if p is not None and p.get("link") == "world":
                root.remove(j)
                removed = True
        if not removed:
            return urdf_path
        out = os.path.join(
            os.path.dirname(urdf_path),
            os.path.splitext(os.path.basename(urdf_path))[0] + "_isaac6.urdf")
        tree.write(out, encoding="utf-8", xml_declaration=True)
        print("[Isaac6] URDF 의 world 용접 제거 사본으로 변환: %s"
              " (fixed base 는 OJW_FIXED_BASE 로 러너가 스폰 앵커에 author)" % out, flush=True)
        return out
    except Exception as ex:
        print("[Isaac6] world weld strip 실패(원본 사용): %s" % ex, flush=True)
        return urdf_path


def _isaac6_convert_urdf(urdf_path: str) -> str:
    """Isaac 6.0 전용: URDF → USD 파일 변환 (캐시).

    5.1 의 kit command 는 현재 스테이지에 임포트했지만, 6.0 URDFImporter 는 파일
    변환기라서 결과 USD 를 _load_usd_as_reference 로 태운다.

    ★안전성: 6.0 의 stage_utils.open_stage 는 standalone Usd.Stage.Open 이라
    USD 컨텍스트(현재 월드)를 건드리지 않는다 — 세션 도중(핫 리로드)에도 호출 안전.

    5.1 ImportConfig → 6.0 매핑:
      convex_decomp=False            → collision_type 기본 "Convex Hull" (동일)
      density=1000.0                 → link_density=1000.0
      default_drive_type=JOINT_DRIVE_POSITION → joint_target_type="position"
      default_drive_strength=1e9     → override_joint_stiffness=1e9
      default_position_drive_damping=1e6 → override_joint_damping=1e6
      fix_base                       → 굽지 않음(None): _load_usd_as_reference 가
                                       OJW_FIXED_BASE 로 스폰 위치 앵커에 author
                                       (원점 용접 폭발 방지 — USD 경로와 동일 정책)
    """
    cached = _isaac6_usd_cache.get(urdf_path)
    if cached and os.path.isfile(cached):
        return cached

    out_dir = os.path.join(os.path.dirname(os.path.abspath(urdf_path)), "_isaac6_usd")
    # 6.0 변환기는 출력 폴더를 unique-path(robot_stl, robot_stl_1, ...)로 늘려가므로
    # 사본이 쌓인다 — 프로세스 시작 후 첫 변환 전에만 통째로 청소.
    # (핫 리로드 재변환은 일부러 청소하지 않는다: 같은 경로를 재사용하면 USD 레이어
    #  캐시가 낡은 내용을 돌려줄 수 있어, unique-path 로 새 폴더를 받는 게 안전)
    if not _isaac6_dir_cleaned[0]:
        _isaac6_dir_cleaned[0] = True
        if os.path.isdir(out_dir):
            import shutil
            shutil.rmtree(out_dir, ignore_errors=True)
    os.makedirs(out_dir, exist_ok=True)

    cfg6 = URDFImporterConfig(
        urdf_path=_strip_world_weld_for_isaac6(urdf_path),
        usd_path=out_dir,
        merge_fixed_joints=False,
        fix_base=None,
        link_density=1000.0,
        joint_target_type="position",
        override_joint_stiffness=1e9,
        override_joint_damping=1e6,
    )
    usd_file = URDFImporter().import_urdf(cfg6)
    print("[Isaac6] URDF->USD converted: %s" % usd_file, flush=True)

    _isaac6_usd_cache[urdf_path] = usd_file
    return usd_file


_ojw_usd_mode = [False]   # USD 네이티브 경로 여부 (폐루프 dt 분기용)


def _load_usd_as_reference(usd_path: str, robot_idx: int = 0,
                           position: tuple[float, float, float] = (0.0, 0.0, 0.0)) -> str:
    """USD 파일을 스테이지에 reference 로 로드하고 articulation root prim path 반환.
    URDFParseAndImportFile 대신 사용하는 USD 네이티브 경로.

    추가 처리 (URDF importer 가 하던 일을 runner 가 대신):
      - PhysxArticulationAPI 보험 적용: self-collision OFF + 솔버 반복수
      - OJW_FIXED_BASE=1 → root fixed joint 를 spawn pose 앵커로 author
        (export 시점엔 spawn 위치를 모르므로 여기서만 가능 — 앵커 불일치 = 폭발 원인)
    """
    stage = omni.usd.get_context().get_stage()
    prim_path = "/World/robot_%d" % robot_idx
    # 핫 리로드: 같은 경로의 USD 를 다시 참조하면 Sdf 레이어 캐시가 낡은 내용을
    # 돌려준다 — 디스크의 새 내용으로 강제 리로드 (없으면 no-op)
    try:
        _lyr = Sdf.Layer.Find(usd_path)
        if _lyr:
            _lyr.Reload()
            print("[Spawn USD] layer reloaded from disk: %s" % usd_path, flush=True)
    except Exception as _ex:
        print("[Spawn USD] layer reload skip: %s" % _ex, flush=True)
    # Xform prim 생성 후 reference 추가
    xform = UsdGeom.Xform.Define(stage, prim_path)
    ref_prim = stage.GetPrimAtPath(prim_path)
    ref_prim.GetReferences().AddReference(usd_path)
    # 위치 설정
    xf = UsdGeom.Xformable(ref_prim)
    xf.ClearXformOpOrder()
    xf.AddTranslateOp().Set(Gf.Vec3d(float(position[0]), float(position[1]), float(position[2])))
    print("[Spawn USD] referenced %s at %s (pos=%s)" % (usd_path, prim_path, position), flush=True)

    # ── PhysxArticulationAPI 보험 (USDA 텍스트 속성이 무시될 경우 대비) ──
    #    OJW_SELF_COLLISION=1: 자기충돌 ON — 실기에서 불가능한 관통 자세(팔이 몸통
    #    파고들기 등)를 시뮬에서도 막음 (하드웨어 검증용. 인접 링크는 PhysX 가 자동 제외)
    try:
        from pxr import PhysxSchema  # type: ignore
        _self_col = os.environ.get("OJW_SELF_COLLISION", "0").strip() == "1"
        art_api = PhysxSchema.PhysxArticulationAPI.Apply(ref_prim)
        art_api.CreateEnabledSelfCollisionsAttr().Set(_self_col)
        art_api.CreateSolverPositionIterationCountAttr().Set(32)
        art_api.CreateSolverVelocityIterationCountAttr().Set(1)
        print("[Spawn USD] PhysxArticulationAPI applied: selfCollisions=%s, posIter=32"
              % _self_col, flush=True)
    except Exception as ex:
        print("[Spawn USD] PhysxArticulationAPI apply failed (무시 가능): %s" % ex, flush=True)

    # ── DriveAPI 보험 — USDA 가 drive:* 속성만 쓰고 API 스키마 적용을 빠뜨렸으면
    #    PhysX 가 드라이브를 무시 (= 토크 오프, 관절 힘없이 풀림 + 명령 무반응).
    #    revolute → angular, prismatic → linear 드라이브를 코드로 강제 적용.
    #    (Create*Attr 는 authored 값 무시하므로 .Set 명시 — 이전 세션 교훈)
    try:
        n_drives = 0
        for prim in Usd.PrimRange(ref_prim):
            tname = prim.GetTypeName()
            if tname == "PhysicsRevoluteJoint":
                dtype = "angular"
            elif tname == "PhysicsPrismaticJoint":
                dtype = "linear"
            else:
                continue
            drive = UsdPhysics.DriveAPI.Apply(prim, dtype)
            drive.CreateTypeAttr().Set("force")
            # authored 값이 있으면 존중, 없으면 URDF 경로와 동일한 강성 적용.
            # OJW_FORCE_DRIVE=1: authored 여부 무관 강제 상향 (IsaacLab 변환 USD 는
            # RL actuator cfg 전제의 약한 게인이 authored 돼 있어 직접 위치제어가 무력)
            _force_drive = os.environ.get("OJW_FORCE_DRIVE", "0").strip() == "1"
            stiff = prim.GetAttribute("drive:%s:physics:stiffness" % dtype)
            if _force_drive or (not stiff or not stiff.Get()):
                drive.CreateStiffnessAttr().Set(1e9)
                drive.CreateDampingAttr().Set(1e6)
                drive.CreateMaxForceAttr().Set(1e6)
                drive.CreateTargetPositionAttr().Set(0.0)
            n_drives += 1
        print("[Spawn USD] DriveAPI ensured on %d joints" % n_drives, flush=True)
    except Exception as ex:
        print("[Spawn USD] DriveAPI ensure failed: %s" % ex, flush=True)

    # ── Fixed base: root fixed joint (URDF importer 의 fix_base 와 동일 방식) ──
    _fix_base = os.environ.get("OJW_FIXED_BASE", "0").strip() == "1"
    if _fix_base:
        # ★플로팅 루트 교정: importer 는 ArticulationRootAPI 를 base_link(리지드바디)에
        #   저작 → PhysX 가 "떠 있는 베이스 + 월드 고정조인트(제약)" 로 해석해
        #   경량 베이스가 반복해석 제약 위에서 미세하게 흔들리며 체인 간 에너지를
        #   주고받는 지속 발진의 원인 (2026-08-25 델타 실측 — 직접 USDA 경로는
        #   루트 API 가 비바디 Xform 이라 동일 조건에서 완전 정지였다).
        #   → 바디에 붙은 루트 API 를 제거하고 래퍼 Xform(robot_N)에 적용해
        #   고정베이스(키네마틱 루트) 아티큘레이션으로 만든다.
        try:
            n_moved = 0
            for prim in Usd.PrimRange(ref_prim):
                if (prim.HasAPI(UsdPhysics.ArticulationRootAPI)
                        and prim.HasAPI(UsdPhysics.RigidBodyAPI)):
                    prim.RemoveAPI(UsdPhysics.ArticulationRootAPI)
                    n_moved += 1
            if n_moved > 0:
                UsdPhysics.ArticulationRootAPI.Apply(ref_prim)
                print("[Spawn USD] ArticulationRootAPI relocated: body prim %d개 -> %s"
                      % (n_moved, prim_path), flush=True)
        except Exception as ex:
            print("[Spawn USD] root API relocate failed: %s" % ex, flush=True)
        try:
            # base 링크 = robot root 의 자식 중 첫 RigidBodyAPI prim
            base_prim = None
            for child in ref_prim.GetChildren():
                if child.HasAPI(UsdPhysics.RigidBodyAPI):
                    base_prim = child
                    break
            if base_prim is None:
                print("[Spawn USD] fixed base: RigidBody 자식을 못 찾음 — skip", flush=True)
            else:
                joint_path = prim_path + "/ojw_root_joint"
                fixed = UsdPhysics.FixedJoint.Define(stage, joint_path)
                fixed.CreateBody1Rel().SetTargets([base_prim.GetPath()])
                # 앵커 = spawn 위치 (base 의 현재 world pose 와 일치 → Play 시 스냅 없음)
                fixed.CreateLocalPos0Attr().Set(Gf.Vec3f(
                    float(position[0]), float(position[1]), float(position[2])))
                fixed.CreateLocalRot0Attr().Set(Gf.Quatf(1.0, 0.0, 0.0, 0.0))
                fixed.CreateLocalPos1Attr().Set(Gf.Vec3f(0.0, 0.0, 0.0))
                fixed.CreateLocalRot1Attr().Set(Gf.Quatf(1.0, 0.0, 0.0, 0.0))
                print("[Spawn USD] fixed base joint authored: %s -> %s @ %s"
                      % (joint_path, base_prim.GetPath(), str(position)), flush=True)
        except Exception as ex:
            print("[Spawn USD] fixed base joint author failed: %s" % ex, flush=True)

    return prim_path


def spawn_robot(urdf_path: str, robot_idx: int = 0,
                position: tuple[float, float, float] = (0.0, 0.0, 0.0)) -> int:
    """Import a URDF (or USD/USDA) as a new articulation. Returns the new robot_idx.

    For multi-robot RL: call repeatedly with different position values. Each
    call duplicates the entire prim subtree under a unique path
    /World/robot_<idx>.

    확장자 감지:
      - .urdf → URDFParseAndImportFile (기존 경로)
      - .usd / .usda / .usdc → USD reference 로 직접 로드 (Method B)
    """
    # global 선언은 함수 최상단에 1회만 — 아래 두 분기 (USD/URDF) 가 모두 사용
    global _world_cameras_loaded

    # === USD 로드 분기 ===
    if _is_usd_path(urdf_path):
        # ★네이티브 USD 모드 표식 — Isaac6 은 URDF 도 변환 후 _load_usd_as_reference 를
        #   타므로 그 함수 안에서 판정하면 안 된다 (폐루프 dt 분기가 URDF 에 오적용된 실사고)
        _ojw_usd_mode[0] = True
        prim_path = _load_usd_as_reference(urdf_path, robot_idx, position)
        articulation_paths.append(prim_path)

        # joint 타입 분류 — stage 의 joint prim 을 직접 순회
        #   prismatic → socket mm→m 변환 분기에 필요
        #   continuous → velocity drive 분기에 필요 (limit 미작성 revolute = continuous)
        cont: set = set()
        prism: set = set()
        try:
            stage = omni.usd.get_context().get_stage()
            root_prim = stage.GetPrimAtPath(prim_path)
            for prim in Usd.PrimRange(root_prim):
                tname = prim.GetTypeName()
                jn = prim.GetName()
                if tname == "PhysicsPrismaticJoint":
                    prism.add(jn)
                elif tname == "PhysicsRevoluteJoint":
                    lower = prim.GetAttribute("physics:lowerLimit")
                    has_limit = bool(lower and lower.HasAuthoredValue())
                    if not has_limit:
                        cont.add(jn)
        except Exception as ex:
            print("[Spawn USD] joint classify failed: %s" % ex, flush=True)
        continuous_joint_names.append(cont)
        prismatic_joint_names.append(prism)
        print("[Spawn USD] continuous=%s prismatic=%s" % (sorted(cont), sorted(prism)), flush=True)

        # mimic — USDA 에는 주석뿐이므로 옆의 robot_stl.urdf (mimic 보존본) 에서 파싱
        mim = {}
        try:
            sibling_urdf = os.path.join(os.path.dirname(urdf_path), "robot_stl.urdf")
            if os.path.exists(sibling_urdf):
                mim = _parse_mimic_from_urdf(sibling_urdf)
                if mim:
                    print("[Spawn USD] mimic from sibling urdf: %s" % list(mim.keys()), flush=True)
        except Exception as ex:
            print("[Spawn USD] mimic parse failed: %s" % ex, flush=True)
        mimic_relationships.append(mim)

        # 카메라 spec — USD 에는 Camera prim 이 이미 있으므로 부착 spec 비움
        camera_specs.append([])
        # world_fixed 카메라 — 신형 USD(2026-08-14+)는 루트 자식 Camera prim 으로
        # 이미 내장돼 있다. 그때 world_cameras.json 까지 읽으면 같은 천장 카메라가
        # 두 번 스폰된다 (scene_cam1 중복 — 게다가 json 쪽은 월드 절대좌표라
        # SPAWN_Z 만큼 어긋나 바닥만 보인다). 루트 직계 Camera 가 있으면 json 생략.
        if not _world_cameras_loaded:
            _has_usd_world_cam = False
            try:
                _rp = omni.usd.get_context().get_stage().GetPrimAtPath(prim_path)
                if _rp and _rp.IsValid():
                    for _child in _rp.GetChildren():
                        if _child.GetTypeName() == "Camera":
                            _has_usd_world_cam = True
                            break
            except Exception:
                pass
            if _has_usd_world_cam:
                print("[Spawn USD] USD 내장 world 카메라 감지 — world_cameras.json 생략 (중복 방지)", flush=True)
            else:
                world_camera_specs.extend(_parse_world_cameras_json(urdf_path))
            _world_cameras_loaded = True
        return len(articulation_paths) - 1

    # === 기존 URDF 로드 경로 ===
    if ISAAC6:
        # Isaac 6.0: kit command "URDFParseAndImportFile" 와 _urdf 가 소멸 —
        # URDF→USD 변환(캐시, 보통 main() 이 World 전에 선변환) 후 USD reference
        # 경로로 태운다. 위치/DriveAPI/fixed-base 보험은 _load_usd_as_reference 가
        # 처리한다 (6.0 변환이 author 한 drive 1e9/1e6 은 authored 로 존중됨).
        print("[Spawn] importing (Isaac6) %s as robot_%d" % (urdf_path, robot_idx), flush=True)
        usd_file = _isaac6_convert_urdf(urdf_path)
        prim_path = _load_usd_as_reference(usd_file, robot_idx, position)
    else:
        import_config = _urdf.ImportConfig()
        import_config.merge_fixed_joints = False
        import_config.convex_decomp = False
        # OJW_FIXED_BASE 환경변수 — MakeUrdf 의 "Fixed Base" 체크박스에서 전달.
        #   1 → 로봇 base 가 world 에 고정 (떨어지지 않음)
        #   0/미설정 → 자유 부유 (중력으로 떨어짐)
        _fix_base_env = os.environ.get("OJW_FIXED_BASE", "0").strip()
        import_config.fix_base = (_fix_base_env == "1")
        print("[Spawn] import_config.fix_base = %s (OJW_FIXED_BASE=%s)" %
              (import_config.fix_base, _fix_base_env), flush=True)
        import_config.make_default_prim = (robot_idx == 0)
        import_config.create_physics_scene = (robot_idx == 0)
        import_config.import_inertia_tensor = True
        import_config.distance_scale = 1.0
        import_config.density = 1000.0
        import_config.default_drive_type = _urdf.UrdfJointTargetType.JOINT_DRIVE_POSITION
        # 관절 hold 강화 — 떨어지는 충격에 덜 흔들리도록 강성/감쇠 100x/10x 증가
        #   이전: 1e7 / 1e5  →  덜렁거리는 현상 (특히 SPAWN_Z 에서 자유낙하 후 floor 충돌 시)
        import_config.default_drive_strength = 1e9
        import_config.default_position_drive_damping = 1e6

        print("[Spawn] importing %s as robot_%d" % (urdf_path, robot_idx), flush=True)
        status, prim_path = omni.kit.commands.execute(
            "URDFParseAndImportFile",
            urdf_path=urdf_path,
            import_config=import_config,
            dest_path="",
            get_articulation_root=True,
        )
        if not status or not prim_path:
            raise RuntimeError("URDF import failed for %s" % urdf_path)

        stage = omni.usd.get_context().get_stage()

        # Reposition root prim — ClearXformOpOrder 로 기존 op 정리 후 단일 TranslateOp 적용
        # (이전 버그: AddTranslateOp 만 호출하면 xformOpOrder 가 갱신 안되어 무효화될 수 있음)
        try:
            prim = stage.GetPrimAtPath(prim_path)
            xform = UsdGeom.Xformable(prim)
            xform.ClearXformOpOrder()
            t_op = xform.AddTranslateOp()
            t_op.Set(Gf.Vec3d(float(position[0]), float(position[1]), float(position[2])))
            print("[Spawn] root xform set to %s" % str(position), flush=True)
        except Exception as ex:
            print("[Spawn] reposition failed: %s" % ex, flush=True)

    articulation_paths.append(prim_path)
    cont = _parse_continuous_joints_from_urdf(urdf_path)
    continuous_joint_names.append(cont)
    print("[Spawn] continuous joints: %s" % sorted(cont), flush=True)

    # prismatic joint 파싱 — socket 값 mm → m 변환 분기에 사용
    prism = _parse_prismatic_joints_from_urdf(urdf_path)
    prismatic_joint_names.append(prism)
    print("[Spawn] prismatic joints: %s" % sorted(prism), flush=True)

    # Mimic 메타데이터 파싱 (Isaac 의 URDFImporter 가 mimic 을 무시하므로 직접 처리)
    mim = _parse_mimic_from_urdf(urdf_path)
    mimic_relationships.append(mim)
    if mim:
        for _fname, _info in mim.items():
            print("[Mimic]   %s -> %s (mult=%.3f, off=%.3f)" %
                  (_fname, _info["parent"], _info["multiplier"], _info["offset"]), flush=True)

    # Configure velocity drive for each continuous joint
    for jname in cont:
        joint_path = "%s/joints/%s" % (prim_path, jname)
        _set_joint_to_velocity_drive(stage, joint_path)

    # Prismatic joint drive 튜닝 — 강한 damping 으로 over-damped 강제.
    # 1) hardcoded path 먼저 시도
    # 2) 그 path 가 무효이면 stage traverse 로 prismatic 찾아 적용 (Isaac 5.1
    #    URDF importer 가 다른 path 를 쓸 수도 있어서 fallback)
    tuned_names: set = set()
    for jname in prism:
        joint_path = "%s/joints/%s" % (prim_path, jname)
        if stage.GetPrimAtPath(joint_path).IsValid():
            _tune_prismatic_joint_drive(stage, joint_path)
            tuned_names.add(jname)

    # Stage traverse fallback — 누락된 prismatic joint 찾기
    missing = prism - tuned_names
    if missing:
        print("[Tune] hardcoded path 누락된 prismatic joint: %s — stage traverse" %
              sorted(missing), flush=True)
        for p in stage.Traverse():
            try:
                if p.IsA(UsdPhysics.PrismaticJoint):
                    name = p.GetName()
                    if name in missing:
                        _tune_prismatic_joint_drive(stage, str(p.GetPath()))
                        tuned_names.add(name)
            except Exception:
                continue

    still_missing = prism - tuned_names
    if still_missing:
        print("[Tune] WARN: prismatic joint drive 적용 실패: %s" %
              sorted(still_missing), flush=True)

    # Camera specs from URDF — attached after world.reset (next step)
    cams = _parse_cameras_from_urdf(urdf_path)
    camera_specs.append(cams)
    print("[Spawn] cameras: %s" % [c["name"] for c in cams], flush=True)

    # world_fixed 카메라는 robot 과 무관 → 첫 robot spawn 때 1회만 로드
    # (global 선언은 함수 최상단에 있음)
    if not _world_cameras_loaded:
        world_camera_specs.extend(_parse_world_cameras_json(urdf_path))
        _world_cameras_loaded = True

    return len(articulation_paths) - 1


def _ojw_cam_xform(prim, roll=0.0, pitch=0.0, yaw=0.0, x=0.0, y=0.0, z=0.0) -> None:
    """카메라 prim 로컬 변환 author — OJW 규약(+X 광축, Z 업)을 USD Camera(-Z 광축, +Y 업)로.

    이 보정 없이는 카메라가 링크의 +X(그리퍼 방향)가 아니라 아래(-Z)를 본다 —
    "카메라 영상이 엉뚱한 곳" 의 원인. R_total = R_rpy(Rz·Ry·Rx) · C,
    C 열: X_usd=(0,-1,0), Y_usd=(0,0,1), Z_usd=(-1,0,0)  (rpy 라디안, xyz 미터)
    """
    import math as _m
    cr, sr = _m.cos(roll), _m.sin(roll)
    cp, sp = _m.cos(pitch), _m.sin(pitch)
    cy, sy = _m.cos(yaw), _m.sin(yaw)
    R = [  # URDF extrinsic XYZ (열벡터 규약) — C# BuildUsda 의 UsdRpyToR 과 동일
        [cy * cp, cy * sp * sr - sy * cr, cy * sp * cr + sy * sr],
        [sy * cp, sy * sp * sr + cy * cr, sy * sp * cr - cy * sr],
        [-sp,     cp * sr,                cp * cr],
    ]
    C = [[0, 0, -1], [-1, 0, 0], [0, 1, 0]]
    T = [[sum(R[i][k] * C[k][j] for k in range(3)) for j in range(3)] for i in range(3)]
    # usda matrix4d 는 행벡터 규약: 상단 3x3 = R^T, 4행 = 평행이동
    m = Gf.Matrix4d(
        T[0][0], T[1][0], T[2][0], 0.0,
        T[0][1], T[1][1], T[2][1], 0.0,
        T[0][2], T[1][2], T[2][2], 0.0,
        float(x), float(y), float(z), 1.0)
    xf = UsdGeom.Xformable(prim)
    xf.ClearXformOpOrder()
    xf.AddTransformOp().Set(m)


def _add_cam_marker(stage, marker_path: str, roll=0.0, pitch=0.0, yaw=0.0,
                    x=0.0, y=0.0, z=0.0) -> None:
    """카메라 설치 위치 시각 마커 — 광축(+X) 방향으로 길쭉한 빨간 박스."""
    try:
        import math as _m
        cube = UsdGeom.Cube.Define(stage, marker_path)
        cube.GetSizeAttr().Set(0.04)
        cube.CreateDisplayColorAttr().Set([Gf.Vec3f(1.0, 0.15, 0.15)])
        mx = UsdGeom.Xformable(cube)
        mx.ClearXformOpOrder()
        mx.AddTranslateOp().Set(Gf.Vec3d(float(x), float(y), float(z)))
        rdeg = 180.0 / _m.pi
        # RotateZYX = Rz·Ry·Rx 합성 (URDF rpy 와 동일 순서), 값 성분은 (x,y,z) 각
        mx.AddRotateZYXOp().Set(Gf.Vec3f(
            float(roll * rdeg), float(pitch * rdeg), float(yaw * rdeg)))
        mx.AddScaleOp().Set(Gf.Vec3f(1.6, 0.5, 0.5))   # X 가장 김 = 광축
    except Exception as ex:
        print("[Camera] marker failed (%s): %s" % (marker_path, ex), flush=True)


def _attach_cameras_after_reset() -> None:
    """Attach an Isaac Sim Camera prim under each <sensor>'s parent link."""
    try:
        from isaacsim.sensors.camera import Camera  # type: ignore
    except ImportError:
        try:
            from omni.isaac.sensor import Camera  # type: ignore  # deprecated path
        except ImportError:
            print("[Camera] no Camera module available — skipping", flush=True)
            return

    stage = omni.usd.get_context().get_stage()
    for ridx, cams in enumerate(camera_specs):
        root = articulation_paths[ridx]
        for cam in cams:
            link_path = "%s/%s" % (root, cam["link"])
            # 6.0 변환 USD 등에서 링크가 직계 자식이 아닐 수 있음 — 이름 탐색 폴백
            if not stage.GetPrimAtPath(link_path).IsValid():
                found = None
                rp = stage.GetPrimAtPath(root)
                if rp and rp.IsValid():
                    for p in Usd.PrimRange(rp):
                        if p.GetName() == cam["link"]:
                            found = str(p.GetPath())
                            break
                if found:
                    link_path = found
                else:
                    print("[Camera] link '%s' not found under %s — skip %s" %
                          (cam["link"], root, cam["name"]), flush=True)
                    continue
            prim_path = "%s/Camera" % link_path
            try:
                obj = Camera(
                    prim_path=prim_path,
                    frequency=cam["rate"],
                    resolution=(cam["w"], cam["h"]),
                )
                obj.initialize()
                obj.set_clipping_range(cam["near"], cam["far"])
                # hfov (rad) -> focal length is camera-dependent; left at default.
                # Users can call obj.set_focal_length(...) externally if needed.
                # ★광축 보정: 카메라 링크의 +X 를 보도록 (없으면 -Z = 엉뚱한 방향)
                _ojw_cam_xform(stage.GetPrimAtPath(prim_path))
                # 설치 위치 마커 (링크 자식 — 팔 따라 움직임)
                _add_cam_marker(stage, "%s/Camera_marker" % link_path)
                camera_objs[(ridx, cam["name"])] = obj
                print("[Camera] attached %s @ %s" % (cam["name"], prim_path), flush=True)
            except Exception as ex:
                print("[Camera] attach failed for %s: %s" % (cam["name"], ex), flush=True)


# ── Ojw.CStream 호환 MJPEG/JPEG HTTP 스트리밍 ─────────────────────────
#   OpenJigWare C# 쪽은 Ojw.CStream.Start_MJpeg/Start_Jpeg 로 그대로 수신:
#     m_CStream.Start_MJpeg(ctrl, "127.0.0.1", 8081, w, h)   ← 연속 스트림
#     m_CStream.Start_Jpeg (ctrl, "127.0.0.1", 8081, w, h)   ← 단발 폴링
#   카메라 선택: "http://127.0.0.1:8081/?action=stream&cam=camera0" (풀 URL 오버로드)
#   포트: 환경변수 OJW_MJPEG_PORT (기본 8081, 0 이면 비활성)
_mjpeg_frames: dict = {}
_mjpeg_lock = threading.Lock()


def _encode_jpeg(rgb):
    """RGB ndarray → JPEG bytes (cv2 우선, 없으면 PIL, 둘 다 없으면 None)."""
    import numpy as _np
    a = _np.asarray(rgb)
    if a.dtype != _np.uint8:
        a = _np.clip(a, 0, 255).astype(_np.uint8)
    try:
        import cv2 as _c
        ok, buf = _c.imencode(".jpg", _np.ascontiguousarray(a[:, :, ::-1]))
        if ok:
            return buf.tobytes()
    except Exception:
        pass
    try:
        import io as _io
        from PIL import Image as _Img
        b = _io.BytesIO()
        _Img.fromarray(a).save(b, "JPEG", quality=85)
        return b.getvalue()
    except Exception:
        return None


def _mjpeg_server_thread(port) -> None:
    """GET ...?action=stream[&cam=이름] → multipart MJPEG / 그 외 GET → 단발 JPEG.
    cam 미지정 시 이름순 첫 카메라. 프레임은 메인 루프의 펌프가 채운다."""
    import http.server
    import socketserver
    import urllib.parse

    class _H(http.server.BaseHTTPRequestHandler):
        def log_message(self, *a):
            pass

        def _pick(self, name):
            with _mjpeg_lock:
                if name and name in _mjpeg_frames:
                    return _mjpeg_frames[name]
                for k in sorted(_mjpeg_frames):
                    return _mjpeg_frames[k]
            return None

        def do_GET(self):
            q = urllib.parse.urlparse(self.path)
            name = urllib.parse.parse_qs(q.query).get("cam", [None])[0]
            if "action=stream" in (q.query or ""):
                try:
                    self.send_response(200)
                    self.send_header("Content-Type",
                                     "multipart/x-mixed-replace; boundary=ojwframe")
                    self.end_headers()
                    while True:
                        jpg = self._pick(name)
                        if jpg is not None:
                            self.wfile.write(("--ojwframe\r\nContent-Type: image/jpeg\r\n"
                                              "Content-Length: %d\r\n\r\n" % len(jpg)).encode("ascii"))
                            self.wfile.write(jpg)
                            self.wfile.write(b"\r\n")
                        time.sleep(0.066)
                except Exception:
                    return          # 클라이언트 끊김 = 정상
            else:
                jpg = self._pick(name)
                if jpg is None:
                    self.send_response(503)
                    self.end_headers()
                    return
                try:
                    self.send_response(200)
                    self.send_header("Content-Type", "image/jpeg")
                    self.send_header("Content-Length", str(len(jpg)))
                    self.end_headers()
                    self.wfile.write(jpg)
                except Exception:
                    return

    class _S(socketserver.ThreadingTCPServer):
        allow_reuse_address = True
        daemon_threads = True

    try:
        srv = _S(("0.0.0.0", int(port)), _H)
        print("[MJPEG] Ojw.CStream 호환 스트리밍 시작: http://127.0.0.1:%d/?action=stream"
              " (&cam=<이름> 으로 선택)" % int(port), flush=True)
        srv.serve_forever()
    except Exception as ex:
        print("[MJPEG] server failed: %s" % ex, flush=True)


def _wrap_usd_cameras_after_reset() -> None:
    """USD 로드 경로: 파일에 이미 든 Camera prim 을 Isaac Camera 센서로 감싸
    camera_objs 에 등록 — CAM_GET/MJPEG 가 URDF 경로와 동일하게 동작하게."""
    try:
        from isaacsim.sensors.camera import Camera  # type: ignore
    except ImportError:
        try:
            from omni.isaac.sensor import Camera  # type: ignore
        except ImportError:
            return
    try:
        stage = omni.usd.get_context().get_stage()
    except Exception:
        return
    already = set()
    for o in list(camera_objs.values()) + list(world_camera_objs.values()):
        p = getattr(o, "prim_path", None)
        if p:
            already.add(str(p))
    for ridx, root in enumerate(articulation_paths):
        rp = stage.GetPrimAtPath(root)
        if not rp or not rp.IsValid():
            continue
        for p in Usd.PrimRange(rp):
            if p.GetTypeName() != "Camera":
                continue
            path = str(p.GetPath())
            if path in already:
                continue
            name = p.GetName()
            try:
                obj = Camera(prim_path=path, frequency=30, resolution=(640, 480))
                obj.initialize()
                camera_objs[(ridx, name)] = obj
                print("[Camera] USD 내장 카메라 wrap: %s" % path, flush=True)
            except Exception as ex:
                print("[Camera] USD 카메라 wrap 실패 %s: %s" % (name, ex), flush=True)


_camera_viewport_wins: list = []


def _open_camera_viewports() -> None:
    """카메라별 뷰포트 창 — 텔레옵/영상인식 확인용 실시간 피드.

    대상: ①부착 카메라(camera_objs) ②world 카메라(world_camera_objs)
         ③USD 로드 시 파일에 이미 들어 있는 Camera prim (robot 서브트리 스캔)
    Kit 버전별 API 차이는 try 로 흡수 — 실패해도 시뮬은 계속 돈다.
    """
    try:
        from omni.kit.viewport.utility import create_viewport_window
    except Exception as ex:
        print("[CamView] viewport utility 없음 — 창 생략: %s" % ex, flush=True)
        return

    targets = []          # (label, prim_path)
    seen = set()

    def _add(label, path):
        if path and path not in seen:
            seen.add(path)
            targets.append((label, path))

    for (ridx, name), obj in camera_objs.items():
        _add("cam %s" % name, getattr(obj, "prim_path", None))
    for name, obj in world_camera_objs.items():
        _add("cam %s" % name, getattr(obj, "prim_path", None))
    try:
        stage = omni.usd.get_context().get_stage()
        for root in articulation_paths:
            rp = stage.GetPrimAtPath(root)
            if not rp or not rp.IsValid():
                continue
            for p in Usd.PrimRange(rp):
                if p.GetTypeName() == "Camera":
                    _add("cam %s" % p.GetName(), str(p.GetPath()))
    except Exception as ex:
        print("[CamView] stage 스캔 실패: %s" % ex, flush=True)

    for i, (label, path) in enumerate(targets):
        try:
            try:
                win = create_viewport_window(label, width=440, height=340,
                                             position_x=60 + i * 48, position_y=90 + i * 48)
            except TypeError:
                win = create_viewport_window(label)
            ok = False
            try:
                win.viewport_api.set_active_camera(path)
                ok = True
            except Exception:
                try:
                    win.viewport_api.camera_path = path
                    ok = True
                except Exception as ex2:
                    print("[CamView] camera 지정 실패 %s: %s" % (path, ex2), flush=True)
            _camera_viewport_wins.append(win)   # GC 로 창이 닫히지 않게 참조 유지
            print("[CamView] viewport '%s' -> %s (%s)" %
                  (label, path, "ok" if ok else "fail"), flush=True)
        except Exception as ex:
            print("[CamView] viewport 생성 실패 (%s): %s" % (label, ex), flush=True)
    if not targets:
        print("[CamView] 카메라 없음 — 뷰포트 생략", flush=True)


def _spawn_world_cameras_after_reset() -> None:
    """world_fixed 카메라를 /World/scene_cameras/ 아래 prim 으로 생성.

    robot articulation 트리와 무관 — robot 떨어져도 카메라 위치 변하지 않음 ($scene 패턴).
    좌표/회전은 world 절대값 (또는 다중 env 시 env-local) 으로 적용.

    Isaac Sim Camera convention 변환:
      - OpenJigWare/URDF: 카메라 +X = 광축 forward, +Z = up
      - Isaac Sim Camera: -Z = forward, +Y = up (USD/OpenGL 기본)
      → orientation 에 추가 회전 필요 시 추후 obj.set_local_pose() 등으로 보정.
        지금은 raw RPY 그대로 전달 (LeRobot 호환은 별도 캘리브 단계에서 처리).
    """
    if not world_camera_specs:
        return
    try:
        from isaacsim.sensors.camera import Camera  # type: ignore
    except ImportError:
        try:
            from omni.isaac.sensor import Camera  # type: ignore  # deprecated
        except ImportError:
            print("[WorldCam] no Camera module — skipping world cameras", flush=True)
            return

    # /World/scene_cameras Xform group prim 생성 (없으면)
    stage = omni.usd.get_context().get_stage()
    group_path = "/World/scene_cameras"
    if not stage.GetPrimAtPath(group_path).IsValid():
        UsdGeom.Xform.Define(stage, group_path)

    for cam in world_camera_specs:
        cam_name = cam["name"]
        prim_path = "%s/%s" % (group_path, cam_name)
        try:
            obj = Camera(
                prim_path=prim_path,
                position=(cam["x"], cam["y"], cam["z"]),
                # orientation 은 Camera 클래스 API 가 quaternion 또는 euler 지원 (버전별 다름).
                # 안전하게 prim 직접 회전 적용 — 아래 Xform op 추가
                frequency=cam["rate"],
                resolution=(cam["w"], cam["h"]),
            )
            obj.initialize()
            obj.set_clipping_range(cam["near"], cam["far"])

            # ★pose 를 광축 보정 포함 행렬로 author (이전: raw RPY 만 → 카메라가
            #   OJW +X 광축이 아니라 USD -Z 를 봐서 엉뚱한 방향이 찍혔다)
            try:
                prim = stage.GetPrimAtPath(prim_path)
                _ojw_cam_xform(prim, cam["roll"], cam["pitch"], cam["yaw"],
                               cam["x"], cam["y"], cam["z"])
            except Exception as _ex:
                print("[WorldCam] pose set failed for %s: %s" %
                      (cam_name, _ex), flush=True)

            # 시각 마커 — 같은 위치에 작은 빨간 박스 (X 가장 김 = 광축 +X 방향 식별)
            try:
                marker_path = "%s_marker" % prim_path
                cube = UsdGeom.Cube.Define(stage, marker_path)
                cube.GetSizeAttr().Set(0.04)   # 4cm 기본
                m_xform = UsdGeom.Xformable(cube)
                m_xform.AddTranslateOp().Set(Gf.Vec3d(cam["x"], cam["y"], cam["z"]))
                rdeg = 180.0 / 3.141592653589793
                m_xform.AddRotateXYZOp().Set(Gf.Vec3f(
                    float(cam["roll"]  * rdeg),
                    float(cam["pitch"] * rdeg),
                    float(cam["yaw"]   * rdeg),
                ))
                # 가로 직육면체 모양 (광축 방향 표시) — X 가 가장 김
                m_xform.AddScaleOp().Set(Gf.Vec3f(1.0, 0.5, 0.5))
                cube.GetDisplayColorAttr().Set([Gf.Vec3f(0.85, 0.1, 0.1)])
            except Exception as _ex:
                print("[WorldCam] marker failed for %s: %s" % (cam_name, _ex), flush=True)

            world_camera_objs[cam_name] = obj
            print("[WorldCam] spawn %s @ %s "
                  "pos=(%.3f,%.3f,%.3f) rpy=(%.3f,%.3f,%.3f) %dx%d" %
                  (cam_name, prim_path,
                   cam["x"], cam["y"], cam["z"],
                   cam["roll"], cam["pitch"], cam["yaw"],
                   cam["w"], cam["h"]), flush=True)
        except Exception as ex:
            print("[WorldCam] spawn failed for %s: %s" % (cam_name, ex), flush=True)


# ============================================================================
# 상태 스냅샷 (GETPOS / GETSCENE 회신용) — 메인 루프에서 매 프레임 호출
# ============================================================================
def _matrix_to_euler_xyz_deg(m) -> tuple:
    """Gf.Matrix4d(world transform) → xformOp:rotateXYZ 규약의 XYZ euler(도).
       scale 은 Gf.Transform 으로 제거 후 회전만 분해."""
    tr = Gf.Transform(m)
    rot = tr.GetRotation()
    # rotateXYZ (row-vector: v·Rx·Ry·Rz) → Decompose(X, Y, Z) = (θx, θy, θz)
    ax, ay, az = rot.Decompose(Gf.Vec3d.XAxis(), Gf.Vec3d.YAxis(), Gf.Vec3d.ZAxis())
    return (float(ax), float(ay), float(az))


def _update_state_snapshots() -> None:
    """관절각/씬 pose 스냅샷 갱신. GETPOS 는 '명령 목표각' 이 아니라 articulation 의
       **실제각** 을 회신해야 함 (그리퍼가 물체에 막혀 목표각에 못 간 경우 감지용)."""
    global _scene_state_snapshot
    # 1) 관절 실제각 — 관절명 T<id> → 모터 id. revolute/continuous = deg, prismatic = mm
    #    (소켓 수신 명령과 동일 단위/부호 규약 — _queue_position 의 역변환)
    snap = {}
    for ridx, art in enumerate(articulations):
        if art is None:
            continue
        try:
            names = art.dof_names
            qpos = art.get_joint_positions()
        except Exception:
            continue
        if qpos is None:
            continue
        rows = []
        for i, jname in enumerate(names):
            if not (len(jname) > 1 and jname[0] == 'T' and jname[1:].isdigit()):
                continue   # mimic follower (t16_0 등) 는 모터 id 가 없음 — 제외
            mid = int(jname[1:])
            is_pris = (ridx < len(prismatic_joint_names)
                       and jname in prismatic_joint_names[ridx])
            v = float(qpos[i])
            rows.append((mid, (v * 1000.0) if is_pris else math.degrees(v)))
        rows.sort()
        snap[ridx] = rows
    # 2) 씬 오브젝트 실제 pose (물리 정착 후 위치 — "상자에 들어갔나" 판정용)
    scene_rows = []
    try:
        stage = omni.usd.get_context().get_stage()
        if stage is not None:
            for idx, path in enumerate(_scene_spawned_paths):
                prim = stage.GetPrimAtPath(path)
                if (not prim) or (not prim.IsValid()):
                    continue
                try:
                    m = omni.usd.get_world_transform_matrix(prim)
                    t = m.ExtractTranslation()
                    rx, ry, rz = _matrix_to_euler_xyz_deg(m)
                    scene_rows.append((idx, (float(t[0]), float(t[1]), float(t[2]),
                                             rx, ry, rz)))
                except Exception:
                    continue
    except Exception:
        pass
    # 3) 로봇 베이스 world pose (IMU 등가 신호 — 몸 기울기 동일성 지표용)
    base_rows = {}
    for ridx, art in enumerate(articulations):
        if art is None:
            continue
        try:
            pos, quat = art.get_world_pose()   # quat = (w,x,y,z)
            w, qx, qy, qz = float(quat[0]), float(quat[1]), float(quat[2]), float(quat[3])
            sinr = 2.0 * (w * qx + qy * qz)
            cosr = 1.0 - 2.0 * (qx * qx + qy * qy)
            rx = math.degrees(math.atan2(sinr, cosr))
            sinp = max(-1.0, min(1.0, 2.0 * (w * qy - qz * qx)))
            ry = math.degrees(math.asin(sinp))
            siny = 2.0 * (w * qz + qx * qy)
            cosy = 1.0 - 2.0 * (qy * qy + qz * qz)
            rz = math.degrees(math.atan2(siny, cosy))
            base_rows[ridx] = (float(pos[0]), float(pos[1]), float(pos[2]), rx, ry, rz)
        except Exception:
            continue

    # 4) IMU 등가 링크 world pose (OJW_IMU_LINK, 기본 link_2 = T17 child 몸통 — CM-550 위치)
    imu_rows = {}
    global _imu_dbg_once
    try:
        stage2 = omni.usd.get_context().get_stage()
        for ridx, art in enumerate(articulations):
            if art is None:
                continue
            prim = _imu_prim_cache.get(ridx)
            if prim is None or not prim.IsValid():
                try:
                    rootp = stage2.GetPrimAtPath(art.prim_path)
                    # 아티큘레이션 루트가 base_link 자체면 링크들은 형제 — 부모에서 검색
                    if rootp and rootp.IsValid() and rootp.GetParent() and rootp.GetParent().IsValid():
                        rootp = rootp.GetParent()
                    if rootp and rootp.IsValid():
                        for p in Usd.PrimRange(rootp):
                            if p.GetName() == _IMU_LINK:
                                _imu_prim_cache[ridx] = p
                                prim = p
                                break
                except Exception:
                    prim = None
            if prim is None:
                # 폴백: 조인트(기본 T17)의 body1 관계 → 몸통 링크 (링크명 무관)
                try:
                    for p2 in Usd.PrimRange(rootp):
                        if p2.GetName() == _IMU_JOINT:
                            rel = p2.GetRelationship("physics:body1")
                            tgts = rel.GetTargets() if rel else []
                            if tgts:
                                cand = stage2.GetPrimAtPath(tgts[0])
                                if cand and cand.IsValid():
                                    _imu_prim_cache[ridx] = cand
                                    prim = cand
                                    print("[IMU] link prim = %s (joint %s body1)" % (
                                        tgts[0], _IMU_JOINT), flush=True)
                            break
                except Exception:
                    pass
            if prim is None:
                if not _imu_dbg_once:
                    _imu_dbg_once = True
                    try:
                        print("[IMU] prim not found: root=%s link=%s joint=%s" % (
                            getattr(art, "prim_path", "?"), _IMU_LINK, _IMU_JOINT), flush=True)
                    except Exception as _dex:
                        print("[IMU] debug err: %s" % _dex, flush=True)
                continue
            m2 = omni.usd.get_world_transform_matrix(prim)
            t2 = m2.ExtractTranslation()
            ix, iy, iz = _matrix_to_euler_xyz_deg(m2)
            imu_rows[ridx] = (float(t2[0]), float(t2[1]), float(t2[2]), ix, iy, iz)
    except Exception:
        pass

    # 5) 전신 COM (GETCOM 회신용) — MassAPI 링크 질량가중 평균 + 지지발 world pos
    #    (MuJoCo subtree_com 등가 — 한다리 COM 서보의 피드백 신호)
    com_rows = {}
    try:
        stage3 = omni.usd.get_context().get_stage()
        for ridx, art in enumerate(articulations):
            if art is None:
                continue
            cache = _com_prim_cache.get(ridx)
            if cache is None:
                cache = []
                try:
                    rootp = stage3.GetPrimAtPath(art.prim_path)
                    if rootp and rootp.IsValid() and rootp.GetParent() and rootp.GetParent().IsValid():
                        rootp = rootp.GetParent()
                    for p in Usd.PrimRange(rootp):
                        if not (p.HasAPI(UsdPhysics.MassAPI) and p.HasAPI(UsdPhysics.RigidBodyAPI)):
                            continue
                        a_m = p.GetAttribute("physics:mass")
                        m_val = float(a_m.Get()) if (a_m and a_m.Get()) else 0.0
                        if m_val <= 0.0:
                            continue
                        a_c = p.GetAttribute("physics:centerOfMass")
                        cv = a_c.Get() if a_c else None
                        cl = Gf.Vec3d(float(cv[0]), float(cv[1]), float(cv[2])) if cv is not None else Gf.Vec3d(0, 0, 0)
                        cache.append((p, m_val, cl))
                    _com_prim_cache[ridx] = cache
                    print("[COM] robot %d: %d bodies, total %.3f kg (foot=%s)" % (
                        ridx, len(cache), sum(c[1] for c in cache), _COM_FOOT), flush=True)
                except Exception as _cex:
                    print("[COM] cache build err: %s" % _cex, flush=True)
                    continue
            if not cache:
                continue
            tot = 0.0
            acc = Gf.Vec3d(0, 0, 0)
            for (p, m_val, cl) in cache:
                if not p.IsValid():
                    continue
                X = omni.usd.get_world_transform_matrix(p)
                acc += Gf.Vec3d(X.Transform(cl)) * m_val
                tot += m_val
            if tot <= 0.0:
                continue
            com = acc / tot
            fx = fy = fz = 0.0
            fp = _com_foot_cache.get(ridx)
            if fp is None or not fp.IsValid():
                try:
                    rootp2 = stage3.GetPrimAtPath(art.prim_path)
                    if rootp2 and rootp2.IsValid() and rootp2.GetParent() and rootp2.GetParent().IsValid():
                        rootp2 = rootp2.GetParent()
                    for p3 in Usd.PrimRange(rootp2):
                        if p3.GetName() == _COM_FOOT and p3.HasAPI(UsdPhysics.RigidBodyAPI):
                            _com_foot_cache[ridx] = p3
                            fp = p3
                            break
                except Exception:
                    fp = None
            if fp is not None and fp.IsValid():
                ftr = omni.usd.get_world_transform_matrix(fp).ExtractTranslation()
                fx, fy, fz = float(ftr[0]), float(ftr[1]), float(ftr[2])
            com_rows[ridx] = (float(com[0]), float(com[1]), float(com[2]), fx, fy, fz, tot)
    except Exception:
        pass

    with _state_lock:
        _joint_state_snapshot.clear()
        _joint_state_snapshot.update(snap)
        _scene_state_snapshot = scene_rows
        _base_state_snapshot.clear()
        _base_state_snapshot.update(base_rows)
        _imu_state_snapshot.clear()
        _imu_state_snapshot.update(imu_rows)
        _com_state_snapshot.clear()
        _com_state_snapshot.update(com_rows)


def _serve_cam_requests(rgb_getter) -> None:
    """CAM_GET 요청 큐 처리 — 메인 루프에서 호출 (Camera capture 는 메인 스레드 전용).
       결과는 result dict 에 jpg/w/h 로 넣고 Event set → 소켓 스레드가 send."""
    while True:
        try:
            name, evt, result = _cam_requests.get_nowait()
        except queue.Empty:
            return
        try:
            # world_fixed 카메라 우선, 없으면 부착 카메라에서 이름 검색
            obj = world_camera_objs.get(name)
            if obj is None:
                for (_ridx, cname), cobj in camera_objs.items():
                    if cname == name:
                        obj = cobj
                        break
            rgb = rgb_getter(obj) if obj is not None else None
            if rgb is not None:
                import cv2 as _cv
                import numpy as _np
                bgr = _np.ascontiguousarray(rgb[:, :, ::-1].astype(_np.uint8))
                ok, enc = _cv.imencode(".jpg", bgr, [int(_cv.IMWRITE_JPEG_QUALITY), 85])
                if ok:
                    result["jpg"] = enc.tobytes()
                    result["h"] = int(bgr.shape[0])
                    result["w"] = int(bgr.shape[1])
        except Exception as ex:
            print("[Cam] CAM_GET 처리 실패 (%s): %s" % (name, ex), flush=True)
        finally:
            evt.set()


# ============================================================================
# Socket protocol — MakeUrdf 3D->Sim compatible
# ============================================================================
def _handle_query(conn, cmd: str) -> bool:
    """회신형 명령 처리. 회신형이면 True 반환 (응답은 같은 연결로 send 후 호출측이 close).
         GETPOS[,robot_idx]  → "POS,<id>,<angle>,<id>,<angle>,...;"
                               (revolute/continuous=도, prismatic=mm — 실제 관절각)
         GETSCENE            → "SCENE,<idx>,x,y,z,rx,ry,rz;" × N개 연결 (없으면 "SCENE;")
         CAM_GET,<name>      → "CAM,<name>,<w>,<h>,<jpeg_len>;" 헤더 + JPEG 바이트
                               (실패 시 "CAM,ERR,<name>,0,0,0;")"""
    if not cmd:
        return False
    up = cmd.upper()
    try:
        if up.startswith("RESETSIM"):
            _scene_cmd_queue.put(("reset_sim", None))
            conn.sendall(b"RESET;")
            return True

        if up.startswith("GETIMU"):
            parts = [p.strip() for p in cmd.split(",")]
            ridx = int(parts[1]) if (len(parts) > 1 and parts[1] != "") else 0
            with _state_lock:
                row = _imu_state_snapshot.get(ridx)
            if row is not None:
                conn.sendall(("IMU,%.4f,%.4f,%.4f,%.2f,%.2f,%.2f;" % row).encode("utf-8"))
            else:
                conn.sendall(b"IMU;")
            return True

        if up.startswith("GETBASE"):
            parts = [p.strip() for p in cmd.split(",")]
            ridx = int(parts[1]) if (len(parts) > 1 and parts[1] != "") else 0
            with _state_lock:
                row = _base_state_snapshot.get(ridx)
            if row is not None:
                conn.sendall(("BASE,%.4f,%.4f,%.4f,%.2f,%.2f,%.2f;" % row).encode("utf-8"))
            else:
                conn.sendall(b"BASE;")
            return True

        if up.startswith("GETCOM"):
            parts = [p.strip() for p in cmd.split(",")]
            ridx = int(parts[1]) if (len(parts) > 1 and parts[1] != "") else 0
            with _state_lock:
                row = _com_state_snapshot.get(ridx)
            if row is not None:
                conn.sendall(("COM,%.4f,%.4f,%.4f,%.4f,%.4f,%.4f,%.3f;" % row).encode("utf-8"))
            else:
                conn.sendall(b"COM;")
            return True

        if up.startswith("GETPOS"):
            parts = [p.strip() for p in cmd.split(",")]
            ridx = int(parts[1]) if (len(parts) > 1 and parts[1] != "") else 0
            with _state_lock:
                rows = list(_joint_state_snapshot.get(ridx, []))
            out = ["POS"]
            for mid, val in rows:
                out.append(str(mid))
                out.append("%.3f" % val)
            conn.sendall((",".join(out) + ";").encode("utf-8"))
            return True

        if up.startswith("GETSCENE"):
            with _state_lock:
                rows = list(_scene_state_snapshot)
            if rows:
                msgs = []
                for idx, (x, y, z, rx, ry, rz) in rows:
                    msgs.append("SCENE,%d,%.4f,%.4f,%.4f,%.2f,%.2f,%.2f;"
                                % (idx, x, y, z, rx, ry, rz))
                conn.sendall("".join(msgs).encode("utf-8"))
            else:
                conn.sendall(b"SCENE;")
            return True

        if up.startswith("RELOAD"):
            # Apply DH 핫 리로드 — 실행은 메인 루프(메인 스레드)가 담당
            _reload_pending[0] = True
            conn.sendall(b"RELOAD,OK;")
            return True

        if up.startswith("CAM_GET"):
            parts = [p.strip() for p in cmd.split(",")]
            name = parts[1] if len(parts) > 1 else ""
            evt = threading.Event()
            result = {}
            _cam_requests.put((name, evt, result))
            ok = evt.wait(3.0)   # 메인 루프 1~2 프레임 내 처리됨
            jpg = result.get("jpg") if ok else None
            if jpg:
                header = "CAM,%s,%d,%d,%d;" % (name, result.get("w", 0),
                                               result.get("h", 0), len(jpg))
                conn.sendall(header.encode("utf-8") + jpg)
            else:
                conn.sendall(("CAM,ERR,%s,0,0,0;" % name).encode("utf-8"))
            return True
    except Exception as ex:
        print("[Sock] query error (%s): %s" % (cmd[:40], ex), flush=True)
        return True   # 회신형이었으나 실패 — 일반 명령 파서로 넘기지 않음
    return False


_server_sock = [None]   # 메인 루프 종료 시 명시적으로 닫아 포트 반납 (좀비 하이재킹 방지)


def _socket_thread(host: str, port: int) -> None:
    srv = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    # ★SO_REUSEADDR 금지 (Windows): 이전 인스턴스가 좀비로 남아 포트를 쥐고 있어도
    #   이중 바인드가 성공해 버려 명령/GETPOS 가 좀비에게 가로채이는 사고 실측
    #   (2026-08-25 — simulation_app.close() 행으로 프로세스 잔존 + 데몬 소켓 스레드 생존).
    #   바인드 실패를 명확히 알리는 편이 옳다.
    try:
        srv.bind((host, port))
        srv.listen(8)
    except OSError as ex:
        print("[Sock] bind %s:%d failed: %s" % (host, port, ex), flush=True)
        print("[Sock] 포트 점유 = 이전 Isaac 이 좀비로 남았을 가능성 — kit.exe 종료 후 재실행",
              flush=True)
        return
    _server_sock[0] = srv
    print("[Sock] listening on %s:%d" % (host, port), flush=True)

    while True:
        try:
            conn, addr = srv.accept()
        except OSError:
            break
        buf = b""
        try:
            data = conn.recv(4096)
            if data:
                buf += data
                # protocol: "...;"
                while b";" in buf:
                    msg, _, buf = buf.partition(b";")
                    text = msg.decode("utf-8", "replace").strip()
                    # 회신형 (GETPOS/GETSCENE/CAM_GET) 은 같은 연결로 send 후 close
                    if _handle_query(conn, text) == False:
                        _handle_command(text)
        except Exception as ex:
            print("[Sock] recv error: %s" % ex, flush=True)
        finally:
            conn.close()


def _handle_command(cmd: str) -> None:
    if not cmd:
        return
    if cmd == "reset" or cmd == "clear":
        joint_pos_targets.clear()
        joint_vel_targets.clear()
        _actnet_state.clear()
        joint_pos_origin.clear()
        joint_pos_t_start.clear()
        joint_pos_t_end.clear()
        print("[Sock] reset", flush=True)
        return

    # === $scene 명령 라우팅 ===
    #   SCENE_TOOL_SET   : 활성 객체 정의 저장 (viewer click 배치용)
    #   SCENE_TOOL_CLEAR : 활성 객체 정의 해제
    #   SCENE_ADD        : 단일 객체 즉시 spawn
    #   SCENE_CLEAR      : 지금까지 spawn 된 모든 scene 객체 제거
    global _scene_active_tool, _scene_spawned_paths
    parts = [p.strip() for p in cmd.split(",")]
    if cmd.startswith("SCENE_TOOL_CLEAR"):
        _scene_active_tool = None
        print("[Scene] tool cleared", flush=True)
        return
    if cmd.startswith("SCENE_TOOL_SET"):
        # SCENE_TOOL_SET, type, sx, sy, sz, color, mass, zoffset
        if len(parts) >= 8:
            try:
                _scene_active_tool = {
                    "type":  parts[1],
                    "sx": float(parts[2]), "sy": float(parts[3]), "sz": float(parts[4]),
                    "color": int(parts[5], 0) if parts[5].lower().startswith("0x") else int(parts[5]),
                    "mass": float(parts[6]),
                    "zoffset": float(parts[7]),
                }
                print("[Scene] tool set: %s" % _scene_active_tool["type"], flush=True)
            except Exception as ex:
                print("[Scene] SCENE_TOOL_SET parse error: %s" % ex, flush=True)
        return
    if cmd.startswith("SCENE_CLEAR"):
        # 메인 스레드 처리 위해 큐에만 넣음 (직접 stage.RemovePrim 호출 X)
        _scene_cmd_queue.put(("clear", None))
        return
    if cmd.startswith("SCENE_ADD"):
        # SCENE_ADD, type, x, y, z, rx, ry, rz, sx, sy, sz, color, mass [, friction]
        #   friction (optional): 마찰계수 (static=dynamic 동일 적용) — 실기 계측값 기반
        #   마찰 랜덤화용. 생략 시 기존 기본 physics material 유지.
        if len(parts) >= 13:
            try:
                obj = {
                    "type":  parts[1],
                    "x": float(parts[2]), "y": float(parts[3]), "z": float(parts[4]),
                    "rx": float(parts[5]), "ry": float(parts[6]), "rz": float(parts[7]),
                    "sx": float(parts[8]), "sy": float(parts[9]), "sz": float(parts[10]),
                    "color": int(parts[11], 0) if parts[11].lower().startswith("0x") else int(parts[11]),
                    "mass": float(parts[12]),
                }
                if (len(parts) > 13) and (parts[13] != ""):
                    obj["friction"] = float(parts[13])
                # 메인 스레드 처리 위해 큐에만 넣음 (직접 USD API 호출 X — 데드락 회피)
                _scene_cmd_queue.put(("add", obj))
            except Exception as ex:
                print("[Scene] SCENE_ADD parse error: %s" % ex, flush=True)
        return

    if len(parts) < 4:
        return
    try:
        time_ms = int(parts[0])
        _delay = int(parts[1])  # not used here
    except Exception:
        return

    now = time.time()
    # pairs: (id, val)
    i = 2
    while i + 1 < len(parts):
        try:
            motor_id = int(parts[i])
            val = float(parts[i + 1])
        except Exception:
            i += 2
            continue
        # Map motor_id -> joint name. MakeUrdf convention: T<id>
        jname = "T%d" % motor_id
        # Choose robot 0 by default — multi-robot routing is per-protocol upgrade
        ridx = 0

        def _queue_position(target_jname: str, val: float, is_prismatic: bool) -> None:
            """target_jname 에 position target 등록.
               is_prismatic=True 면 val 은 mm — m 단위로 변환 (× 0.001).
               is_prismatic=False 면 val 은 degree — rad 단위로 변환.

               중요: 보간 origin 을 **직전 target** 으로 설정해야 같은 값이 반복 전송
               (Auto-sync) 될 때 jumpback 출렁임이 생기지 않는다.
               (직전 코드는 `.get(key, 0.0)` 라 origin 이 영원히 0.0 → 매 전송마다
                joint 가 0 으로 점프했다가 다시 target 으로 보간되어 진동.)"""
            if is_prismatic:
                target_native = val * 0.001   # mm → m
            else:
                target_native = math.radians(val)   # deg → rad
            key = (ridx, target_jname)
            # origin = 직전 target (Auto-sync 반복 전송 시 jumpback 방지).
            # 직전 target 이 없는 "첫 명령" 은 실제 관절각 스냅샷을 origin 으로 사용
            # → 첫 명령부터 time_ms 보간이 동작 (이전엔 origin=target 이라 즉시 점프).
            prev_target = joint_pos_targets.get(key, None)
            if prev_target is None:
                prev_target = target_native   # 스냅샷 miss 시 기존 동작 (점프) 유지
                try:
                    if (len(target_jname) > 1 and target_jname[0] == 'T'
                            and target_jname[1:].isdigit()):
                        mid = int(target_jname[1:])
                        with _state_lock:
                            rows = list(_joint_state_snapshot.get(ridx, []))
                        for rid, rval in rows:
                            if rid == mid:
                                prev_target = (rval * 0.001) if is_prismatic \
                                    else math.radians(rval)
                                break
                except Exception:
                    pass
            joint_pos_origin[key] = prev_target
            joint_pos_t_start[key] = now
            dur = max(0.0, time_ms / 1000.0)
            joint_pos_t_end[key] = now + dur
            joint_pos_targets[key] = target_native

        # 단위 분기 — URDF 의 joint type 에 따라 다르게 처리.
        #   continuous: velocity (RPS → rad/s)
        #   prismatic:  position (mm → m)
        #   revolute:   position (deg → rad)
        if ridx < len(continuous_joint_names) and jname in continuous_joint_names[ridx]:
            # Continuous joint => velocity (RPS -> rad/s)
            joint_vel_targets[(ridx, jname)] = val * 2.0 * math.pi
        else:
            is_prismatic = (ridx < len(prismatic_joint_names)
                            and jname in prismatic_joint_names[ridx])
            _queue_position(jname, val, is_prismatic)

            # ── Mimic 전파: 이 leader 의 follower 들에 multiplier × value + offset 적용 ──
            #    Isaac 의 URDFImporter 는 <mimic> 을 무시하므로 socket 단에서 직접 처리.
            #    Follower 는 leader 와 같은 단위계 (둘 다 prismatic 이거나 둘 다 revolute)
            #    라고 가정 — URDF mimic 규약상 일반적.
            if ridx < len(mimic_relationships):
                for follower_name, mim_info in mimic_relationships[ridx].items():
                    if mim_info.get("parent") != jname:
                        continue
                    mult = mim_info.get("multiplier", 1.0)
                    follower_is_prismatic = (ridx < len(prismatic_joint_names)
                                             and follower_name in prismatic_joint_names[ridx])
                    if follower_is_prismatic:
                        # URDF mimic offset 은 m 단위 (혹은 rad). mm 단위로 변환 후 합성.
                        off_native = mim_info.get("offset", 0.0) * 1000.0  # m → mm
                        follower_val = mult * val + off_native
                    else:
                        off_native = mim_info.get("offset", 0.0) * 180.0 / math.pi  # rad → deg
                        follower_val = mult * val + off_native
                    _queue_position(follower_name, follower_val, follower_is_prismatic)
        i += 2


# ============================================================================
# 종동(passive) 조인트 퍼펫팅 — delta_passive.json (2026-08-25)
#   MakeUrdf Export 가 폐루프 종동 관절(델타 무릎 β/γ 등)을 무동력 dpass_* 조인트로
#   내보내고, 각 조인트의 각도 수식을 JSON 으로 동봉한다. 여기서는 매 physics step 마다
#   현재 모터각으로 수식을 평가해 종동 조인트 타깃을 갱신 → 시뮬 안에서 T 를 어떻게
#   움직이든(소켓/처짐 포함) 폐루프 형상이 유지된다.
#   각도 규약: q_pass_deg = (motor_deg + eval(formula, motors_deg)) - zero_bake_deg
#   (부호는 조인트 축에 흡수되어 있음 — C# ProcessDhLine_Urdf 참조)
# ============================================================================
_passive_defs = []            # [{joint, formula, motor_id, zero_bake_deg}]
_delta_geo = [55.0, 100.0, 215.0, 24.0]  # deltainit 으로 갱신 (변 길이 규약)


def _delta_fk(m0, m1, m2):
    """COjw_05_Math.Delta_Parallel_ForwardKinematics 포팅 (+z 규약, deg 입력)"""
    e, f, re_, rf = _delta_geo[3], _delta_geo[0], _delta_geo[2], _delta_geo[1]
    d2r = math.pi / 180.0
    t = (f - e) * math.tan(30 * d2r) / 2
    y1 = -(t + rf * math.cos(-m0 * d2r)); z1 = -rf * math.sin(-m0 * d2r)
    y2 = (t + rf * math.cos(-m1 * d2r)) * math.sin(30 * d2r); x2 = y2 * math.tan(60 * d2r); z2 = -rf * math.sin(-m1 * d2r)
    y3 = (t + rf * math.cos(-m2 * d2r)) * math.sin(30 * d2r); x3 = -y3 * math.tan(60 * d2r); z3 = -rf * math.sin(-m2 * d2r)
    dnm = (y2 - y1) * x3 - (y3 - y1) * x2
    w1 = y1 * y1 + z1 * z1; w2 = x2 * x2 + y2 * y2 + z2 * z2; w3 = x3 * x3 + y3 * y3 + z3 * z3
    a1 = (z2 - z1) * (y3 - y1) - (z3 - z1) * (y2 - y1); b1 = -((w2 - w1) * (y3 - y1) - (w3 - w1) * (y2 - y1)) / 2.0
    a2 = -(z2 - z1) * x3 + (z3 - z1) * x2;              b2 = ((w2 - w1) * x3 - (w3 - w1) * x2) / 2.0
    a = a1 * a1 + a2 * a2 + dnm * dnm
    b = 2 * (a1 * b1 + a2 * (b2 - y1 * dnm) - z1 * dnm * dnm)
    c = (b2 - y1 * dnm) ** 2 + b1 * b1 + dnm * dnm * (z1 * z1 - re_ * re_)
    d = b * b - 4.0 * a * c
    if d < 0 or d != d:
        return None
    h = -0.5 * (b + math.sqrt(d)) / a
    return ((a1 * h + b1) / dnm, (a2 * h + b2) / dnm, -h)


def _delta_calc_passive(arm, m0, m1, m2):
    """COjw_05_Math.Delta_Parallel_CalcPassive 포팅 — (beta, gamma) deg 반환"""
    p = _delta_fk(m0, m1, m2)
    if p is None:
        return None
    d2r = math.pi / 180.0
    psi = 30.0 if arm == 1 else (150.0 if arm == 2 else -90.0)
    t = m1 if arm == 1 else (m2 if arm == 2 else m0)
    cp_, sp_ = math.cos(psi * d2r), math.sin(psi * d2r)
    px = p[0] * cp_ + p[1] * sp_; py = -p[0] * sp_ + p[1] * cp_; pz = p[2]
    a_f = 0.5 * 0.57735 * _delta_geo[0]
    a_e = 0.5 * 0.57735 * _delta_geo[3]
    kx = a_f + _delta_geo[1] * math.cos(t * d2r)
    kz = -_delta_geo[1] * math.sin(t * d2r)
    vx = (px + a_e) - kx; vy = py; vz = pz - kz
    vlen = math.sqrt(vx * vx + vy * vy + vz * vz)
    if vlen < 1e-9:
        return None
    gamma = -math.asin(max(-1.0, min(1.0, vy / vlen))) / d2r
    beta = -t - math.atan2(vz, vx) / d2r
    return beta, gamma


_EDH_OPS = ("=", "+", "-", "*", "/")
_EDH_CMDS = ("deltainit", "getdelta") + _EDH_OPS


def _edh_operand(tok, motors):
    tok = tok.strip()
    if not tok:
        return None
    if tok[0] == "t":
        try:
            return float(motors.get(int(tok[1:]), 0.0))
        except (ValueError, TypeError):
            return None
    try:
        return float(tok)
    except ValueError:
        return None


def _edh_eval_formula(formula, motors):
    """C# Edh_EvalDeltaFormula 포팅 — 실패 시 None. motors: {모터ID: deg}"""
    try:
        toks = formula.strip().lower().split(";")
        result = 0.0
        n = 0
        while n < len(toks):
            tok = toks[n].strip()
            if not tok:
                n += 1; continue
            if tok == "deltainit":
                if n + 4 >= len(toks):
                    return None
                vals = [_edh_operand(toks[n + k], motors) for k in range(1, 5)]
                if any(v is None for v in vals):
                    return None
                _delta_geo[0], _delta_geo[1], _delta_geo[2], _delta_geo[3] = vals
                n += 5; continue
            if tok == "getdelta":
                if n + 5 >= len(toks):
                    return None
                vals = [_edh_operand(toks[n + k], motors) for k in range(1, 6)]
                if any(v is None for v in vals):
                    return None
                arm, sel, m0, m1, m2 = int(vals[0]), int(vals[1]), vals[2], vals[3], vals[4]
                if sel <= 1:
                    bg = _delta_calc_passive(arm, m0, m1, m2)
                    if bg is None:
                        return None
                    result = bg[0] if sel == 0 else bg[1]
                else:
                    p = _delta_fk(m0, m1, m2)
                    if p is None:
                        return None
                    result = p[sel - 2]
                n += 6; continue
            if tok in _EDH_OPS:
                nxt = toks[n + 1].strip() if n + 1 < len(toks) else ""
                if nxt in _EDH_CMDS:
                    n += 1; continue  # 피연산자 자리에 명령 → 연산자 무효 (렌더 파서 거동)
                v = _edh_operand(nxt, motors)
                if v is None:
                    return None
                if tok == "=":
                    result = v
                elif tok == "+":
                    result += v
                elif tok == "-":
                    result -= v
                elif tok == "*":
                    result *= v
                else:
                    result = result / v if v != 0 else 0.0
                n += 2; continue
            return None  # 미지원 토큰
        return result
    except Exception:
        return None


_closure_defs = []            # 진짜 물리 폐루프 앵커 (delta_passive.json "closures")


def _load_passive_defs(urdf_dir):
    global _passive_defs, _closure_defs
    _passive_defs = []
    _closure_defs = []
    path = os.path.join(urdf_dir, "delta_passive.json")
    if not os.path.exists(path):
        return
    try:
        import json as _json
        with open(path, "r", encoding="utf-8") as fp:
            data = _json.load(fp)
        _closure_defs = data.get("closures", [])
        if _closure_defs:
            # ★진짜 물리 폐루프 모드 (2026-08-26): 스폰 시 루프 조인트(스패리컬+거리)를 저작하고
            #   dpass 드라이브를 무력화 — 물리가 폐루프를 유지하므로 퍼펫은 끈다
            #   (퍼펫 드라이브 타깃은 구속 솔버와 싸운다).
            print("[Closure] delta_passive.json: closures %d개 — 물리 폐루프 모드 (퍼펫 OFF)"
                  % len(_closure_defs), flush=True)
        else:
            _passive_defs = data.get("passive_joints", [])
            print("[Passive] delta_passive.json: %d joints (퍼펫팅 활성)" % len(_passive_defs), flush=True)
    except Exception as ex:
        print("[Passive] delta_passive.json 로드 실패: %s" % ex, flush=True)


def _author_delta_closures(spawn_z_hint: float = 0.0) -> None:
    """스폰 직후(world.reset 전) 루프 조인트 저작 — URDF 임포트/USDA 양 경로 공용.

    조인트 prim 을 이름으로 찾아 body0/body1 rel 로 링크를 해석하므로 경로 구조 무관:
      · rod_joint 의 body1(child) = 로드 링크, plate_joint 의 body1 = 플레이트 링크
      · knee_joint 의 body0(parent) = 무릎블록(상완) 링크
    저작 내용 (MJCF <connect>/<tendon> equality, USDA ojw_loop/ojw_rod2 와 동일 원리):
      ①로드팁 PhysicsSphericalJoint (점 일치) ②두 번째 로드 PhysicsDistanceJoint (min=max=L)
      — 전부 excludeFromArticulation (PhysX articulation 은 트리만 허용, 루프는 밖에서 닫는다)
    USDA 경로는 export 시 이미 저작되어 있음 (ojw_loop_* 존재) → 스킵.
    dpass 드라이브 stiffness=0 도 여기서 (URDF importer 는 전 관절에 고강성 드라이브를 단다)."""
    if not _closure_defs:
        return
    try:
        from pxr import Usd, UsdPhysics, Sdf, Gf  # type: ignore
        stage = simulation_app.context.get_stage()
        if stage is None:
            return
        roots = [p_ for p_ in articulation_paths] if articulation_paths else []
        if not roots:
            return
        root = roots[0]
        # 이름 → 조인트 prim (root 서브트리 한정)
        joint_prims = {}
        already = False
        for prim in stage.Traverse():
            pp = str(prim.GetPath())
            if not pp.startswith(root):
                continue
            nm = prim.GetName()
            if nm.startswith("ojw_loop_") or nm.startswith("ojw_rod2_"):
                already = True
            if prim.HasRelationship("physics:body1") or prim.HasRelationship("physics:body0"):
                joint_prims[nm] = prim

        def _rel_target(jname, rel):
            pr = joint_prims.get(jname)
            if pr is None:
                return None
            r = pr.GetRelationship(rel)
            if not r:
                return None
            tg = r.GetTargets()
            return str(tg[0]) if tg else None

        # 폐루프 모드: 솔버 반복수 상향 — 루프 구속(스패리컬 5+rod2 6)이 그램급 링크를
        # 지나는 사슬이라 반복 부족이 극자세에서 mm 급 플레이트 이탈로 나타난다.
        # ★USD 경로는 ArticulationRootAPI 가 래퍼 Xform 에 있고 PhysxArticulationAPI(32)는
        #   로봇 prim 에 남는다 — 어느 쪽을 파서가 읽든 걸리도록 루트+기존 보유 prim 전부 설정
        try:
            from pxr import Sdf as _Sdf  # type: ignore
            targets = set()
            root_prim0 = stage.GetPrimAtPath(root)
            if root_prim0 and root_prim0.IsValid():
                targets.add(root_prim0)
            for prim2 in stage.Traverse():
                if not str(prim2.GetPath()).startswith(root):
                    continue
                a_ex = prim2.GetAttribute("physxArticulation:solverPositionIterationCount")
                if a_ex and a_ex.IsValid() and a_ex.HasAuthoredValue():
                    targets.add(prim2)
            for pr2 in targets:
                a_it = pr2.GetAttribute("physxArticulation:solverPositionIterationCount")
                if not (a_it and a_it.IsValid()):
                    a_it = pr2.CreateAttribute("physxArticulation:solverPositionIterationCount",
                                               _Sdf.ValueTypeNames.Int)
                a_it.Set(128)
        except Exception:
            pass

        # dpass 드라이브 무력화 + T 드라이브 완화 (양 경로 공통)
        n_freed = 0
        n_soft = 0
        for nm, pr in joint_prims.items():
            try:
                if nm.startswith("dpass_"):
                    # ★drive 완전 0 — deg 단위 damping 174.53 은 rad 1만 상당의 초과감쇠로
                    #   그램급 링크를 동결시켜 루프 구속과 싸우다 NaN 폭발 (실측 사고)
                    vals = (("drive:angular:physics:stiffness", 0.0),
                            ("drive:angular:physics:damping", 0.0))
                    n_freed += 1
                elif nm.startswith("T") and nm[1:].isdigit():
                    # T 드라이브 = 임포터 패리티(deg 17453293/17453.293)로 통일.
                    #   (러너의 "drive hardening" 1e9 재강화는 폐루프 관절 제외 — 아래 참조.
                    #    플립 분기 도약의 진짜 원인은 free 강체 rod2 의 텔레포트 킥이었고
                    #    트리 편입으로 해결 — 드라이브 완화(deg 8.7)는 오히려 정적 바이어스만 남김)
                    vals = (("drive:angular:physics:stiffness", 17453293.0),
                            ("drive:angular:physics:damping", 17453.293))
                    n_soft += 1
                else:
                    continue
                for attr, val in vals:
                    a = pr.GetAttribute(attr)
                    if a and a.IsValid():
                        a.Set(val)
            except Exception:
                pass
        if already:
            # USDA 저작본: rod2 는 트리 조인트(_a 스패리컬, exclude 없음)로 articulation 에
            # 편입되므로 spawn 텔레포트를 함께 따라간다 — 별도 보정 불필요.
            print("[Closure] 루프 조인트 이미 저작됨 (USDA) — dpass/T 드라이브 조정만 (해제 %d, 완화 %d)"
                  % (n_freed, n_soft), flush=True)
            return

        n_loop = 0
        for idx, clo in enumerate(_closure_defs, start=1):
            plate_path = _rel_target(clo.get("plate_joint") or "", "physics:body1")
            if not plate_path:
                continue
            rj = clo.get("rod_joint") or ""
            if rj:
                rod_path = _rel_target(rj, "physics:body1")
                if rod_path:
                    jp = UsdPhysics.SphericalJoint.Define(stage, Sdf.Path(root + "/ojw_loop_%d" % idx))
                    jp.CreateBody0Rel().SetTargets([Sdf.Path(rod_path)])
                    jp.CreateBody1Rel().SetTargets([Sdf.Path(plate_path)])
                    jp.CreateLocalPos0Attr().Set(Gf.Vec3f(*[float(v) for v in clo["rod_xyz"]]))
                    jp.CreateLocalPos1Attr().Set(Gf.Vec3f(*[float(v) for v in clo["plate_xyz"]]))
                    jp.CreateLocalRot0Attr().Set(Gf.Quatf(1.0))
                    jp.CreateLocalRot1Attr().Set(Gf.Quatf(1.0))
                    jp.CreateExcludeFromArticulationAttr().Set(True)
                    n_loop += 1
            kj = clo.get("knee_joint") or ""
            ax = clo.get("pair_axis", [0, 0, 0])
            alen = math.sqrt(sum(a * a for a in ax))
            if kj and alen > 1e-9:
                knee_parent = _rel_target(kj, "physics:body0")
                if knee_parent:
                    # ★두 번째 로드 = 실제 강체 + 양끝 스패리컬.
                    #   PhysicsDistanceJoint(min=max)는 PhysX 에서 사실상 무력 (플레이트가
                    #   10~17mm 떠돎 — 실측). 스패리컬 루프는 완벽 작동이 검증되었으므로
                    #   질량 1g 무형상 강체 rod2 로 평행사변형을 정직하게 구성한다.
                    from pxr import UsdGeom  # type: ignore
                    w = float(clo.get("pair_halfwidth_m", 0.03))
                    u = [a / alen * w for a in ax]
                    knee = clo.get("knee_xyz", [0, 0, 0])
                    plate = clo.get("plate_xyz", [0, 0, 0])
                    cache = UsdGeom.XformCache(Usd.TimeCode.Default())
                    kp_prim = stage.GetPrimAtPath(Sdf.Path(knee_parent))
                    pl_prim = stage.GetPrimAtPath(Sdf.Path(plate_path))
                    root_prim = stage.GetPrimAtPath(Sdf.Path(root))
                    if not (kp_prim and kp_prim.IsValid() and pl_prim and pl_prim.IsValid()):
                        continue
                    Tk = cache.GetLocalToWorldTransform(kp_prim)
                    Tp = cache.GetLocalToWorldTransform(pl_prim)
                    K2w = Tk.Transform(Gf.Vec3d(knee[0] + u[0], knee[1] + u[1], knee[2] + u[2]))
                    P2w = Tp.Transform(Gf.Vec3d(plate[0] + u[0], plate[1] + u[1], plate[2] + u[2]))
                    # ★rod2 는 아티큘레이션 "트리의 가지" — 무릎→rod2 스패리컬을 exclude 없이
                    #   저작하면 PhysX 파서가 rod2 를 articulation 링크로 편입한다.
                    #   free 강체로 두면 reset 텔레포트(spawn_z)를 못 따라가 첫 스텝에 팔을
                    #   걷어차 팔꿈치 플립 분기로 떨어뜨린다 (실측 사고 — z 보정/후처리 재배치는
                    #   physics view 무효화 등 부작용만 낳고 실패). 트리 편입이 근본 해결.
                    rod2_path = Sdf.Path(root + "/ojw_rod2_%d" % idx)
                    rod2 = UsdGeom.Xform.Define(stage, rod2_path)
                    rod2_prim = rod2.GetPrim()
                    UsdPhysics.RigidBodyAPI.Apply(rod2_prim)
                    mass_api = UsdPhysics.MassAPI.Apply(rod2_prim)
                    mass_api.CreateMassAttr().Set(0.001)
                    mass_api.CreateDiagonalInertiaAttr().Set(Gf.Vec3f(1e-7, 1e-7, 1e-7))
                    Tr = cache.GetLocalToWorldTransform(root_prim)
                    K2_local_root = Tr.GetInverse().Transform(K2w)
                    try:
                        rod2.AddTranslateOp().Set(Gf.Vec3d(K2_local_root))
                    except Exception:
                        rod2_prim.GetAttribute("xformOp:translate").Set(Gf.Vec3d(K2_local_root))
                    # rod2 로컬 = 월드 정렬 (회전 없음) — P2 는 rod2 로컬로 (P2w − K2w)
                    d_vec = Gf.Vec3d(P2w[0] - K2w[0], P2w[1] - K2w[1], P2w[2] - K2w[2])
                    # 무릎→rod2 스패리컬: 트리 조인트 (exclude 없음 — rod2 가 articulation 에 편입)
                    ja = UsdPhysics.SphericalJoint.Define(stage, Sdf.Path(root + "/ojw_rod2_%d_a" % idx))
                    ja.CreateBody0Rel().SetTargets([Sdf.Path(knee_parent)])
                    ja.CreateBody1Rel().SetTargets([rod2_path])
                    ja.CreateLocalPos0Attr().Set(Gf.Vec3f(knee[0] + u[0], knee[1] + u[1], knee[2] + u[2]))
                    ja.CreateLocalPos1Attr().Set(Gf.Vec3f(0.0, 0.0, 0.0))
                    ja.CreateLocalRot0Attr().Set(Gf.Quatf(1.0))
                    ja.CreateLocalRot1Attr().Set(Gf.Quatf(1.0))
                    try:
                        # rod2 자체축 스핀(볼-볼 로드의 자유 회전)이 무감쇠로 계속 돌지 않게 소량 마찰
                        ja.GetPrim().CreateAttribute("physxJoint:jointFriction",
                                                     Sdf.ValueTypeNames.Float).Set(0.0001)
                    except Exception:
                        pass
                    jb = UsdPhysics.SphericalJoint.Define(stage, Sdf.Path(root + "/ojw_rod2_%d_b" % idx))
                    jb.CreateBody0Rel().SetTargets([rod2_path])
                    jb.CreateBody1Rel().SetTargets([Sdf.Path(plate_path)])
                    jb.CreateLocalPos0Attr().Set(Gf.Vec3f(d_vec[0], d_vec[1], d_vec[2]))
                    jb.CreateLocalPos1Attr().Set(Gf.Vec3f(plate[0] + u[0], plate[1] + u[1], plate[2] + u[2]))
                    jb.CreateLocalRot0Attr().Set(Gf.Quatf(1.0))
                    jb.CreateLocalRot1Attr().Set(Gf.Quatf(1.0))
                    jb.CreateExcludeFromArticulationAttr().Set(True)
                    # ★평행 잠금 (안티평행사변형 봉쇄): rod2 는 rod1 과 항상 평행 — 회전 3축 잠금,
                    #   병진 3축 자유 (D6). 없으면 빠른 천이 중 rod2 가 교차 상태로 플립해
                    #   메커니즘이 잼 (T1 이 41° 에서 고착된 실측 사고 — 링키지 특이점 통과 실패).
                    #   rod1 링크 = 무릎(β) child 의 child (γ joint 의 child) — 체인 한 칸 하강으로 해석
                    rod1_path = None
                    kc = _rel_target(kj, "physics:body1")
                    if kc:
                        for nm2 in joint_prims:
                            if _rel_target(nm2, "physics:body0") == kc:
                                rod1_path = _rel_target(nm2, "physics:body1")
                                break
                    if rod1_path and os.environ.get("OJW_ROD2_PLOCK", "1") == "1":
                        jl = UsdPhysics.Joint.Define(stage, Sdf.Path(root + "/ojw_rod2_%d_p" % idx))
                        jl.CreateBody0Rel().SetTargets([Sdf.Path(rod1_path)])
                        jl.CreateBody1Rel().SetTargets([rod2_path])
                        jl.CreateLocalPos0Attr().Set(Gf.Vec3f(0.0, 0.0, 0.0))
                        jl.CreateLocalPos1Attr().Set(Gf.Vec3f(0.0, 0.0, 0.0))
                        jl.CreateLocalRot0Attr().Set(Gf.Quatf(1.0))
                        jl.CreateLocalRot1Attr().Set(Gf.Quatf(1.0))
                        # 일반 PhysicsJoint(D6) = 기본 전 DOF 잠금 → 병진 3축만 해방 (회전은 잠긴 채)
                        for dof in ("transX", "transY", "transZ"):
                            la = UsdPhysics.LimitAPI.Apply(jl.GetPrim(), dof)
                            la.CreateLowAttr().Set(float("-inf"))
                            la.CreateHighAttr().Set(float("inf"))
                        jl.CreateExcludeFromArticulationAttr().Set(True)
                    n_loop += 1
        print("[Closure] 루프 조인트 %d개 저작 완료 + dpass 해제 %d개 + T 드라이브 완화 %d개 (물리 폐루프)"
              % (n_loop, n_freed, n_soft), flush=True)
    except Exception as ex:
        print("[Closure] 루프 조인트 저작 실패: %s" % ex, flush=True)


_passive_dbg = {"announced": False, "fail": 0, "t_print": 0.0}


def _apply_passive_puppet(art, names, positions):
    """이 스텝의 모터 타깃으로 종동 조인트 타깃 계산 — positions 리스트에 기입. 반환: 기입 여부

    ★모터값은 명령(이 스텝에 계산된 positions[i]) 우선, 없을 때만 실측(cur[i]).
      실측을 쓰면 물리 흔들림이 수식→드라이브로 되먹임돼 워크스페이스 경계
      (∂수식/∂모터 폭증) 근처에서 발진한다 (2026-08-25 URDF 경로 실사고).
    ★스텝당 타깃 변화 ±20° 제한 — 경계에서 수식이 잠시 실패했다 복귀할 때
      타깃이 한 스텝에 점프하면 고강성 드라이브가 로봇을 걷어차는 것을 방지.
    """
    if not _passive_defs:
        return False
    try:
        cur = art.get_joint_positions()
        if cur is None:
            return False
    except Exception:
        return False
    motors = {}
    name_idx = {}
    for i, jn in enumerate(names):
        name_idx[jn] = i
        if jn.startswith("T") and jn[1:].isdigit():
            src = positions[i] if positions[i] is not None else float(cur[i])
            motors[int(jn[1:])] = float(src) * 180.0 / math.pi
    if not motors:
        return False
    last = getattr(art, "_ojw_passive_last", None)
    if last is None:
        last = {}
        try:
            art._ojw_passive_last = last
        except Exception:
            pass
    # 이번 스텝 모터 변화량 — 수식 출력 점프가 "모터 명령 점프" 로 설명 가능한지 판정용
    m_prev = getattr(art, "_ojw_passive_mprev", None)
    m_step = 0.0
    if m_prev:
        for k, v in motors.items():
            dv = abs(v - m_prev.get(k, v))
            if dv > m_step:
                m_step = dv
    try:
        art._ojw_passive_mprev = dict(motors)
    except Exception:
        pass
    wrote = False
    dbg_jn = None
    for pdef in _passive_defs:
        jn = pdef.get("joint")
        if jn not in name_idx:
            continue
        f_val = _edh_eval_formula(pdef.get("formula", ""), motors)
        if f_val is None:
            _passive_dbg["fail"] += 1
            continue
        m_deg = motors.get(pdef.get("motor_id", -1), 0.0)
        q_deg = (m_deg + f_val) - float(pdef.get("zero_bake_deg", 0.0))
        prev = last.get(jn)
        if prev is not None:
            # ★각도 연속화(unwrap): atan2 경계에서 수식이 360° 랩 — 렌더/MuJoCo(qpos)는
            #   mod360 이라 안 보이지만 드라이브 타깃은 실제로 한 바퀴 돌다 ±180° 한계에
            #   걸려 "서서히 분해" 됐다 (2026-08-25 실사고, T1≈-99° 에서 +360.3 점프).
            d_un = ((q_deg - prev + 180.0) % 360.0) - 180.0
            # 분기 점프 가드: 모터 변화(m_step)로 설명 안 되는 큰 점프 = FK 팬텀 분기
            #   (|T1|≥130° 등 물리 불가능 접힘 영역에서 117~128° 진짜 점프 실측) —
            #   추종하면 관절이 찢어지므로 1°/스텝 서행 (복귀 시 자기치유).
            #   정상/명령 점프는 20°/스텝 슬루 유지.
            rate = 1.0 if abs(d_un) > (5.0 + 3.0 * m_step) else 20.0
            if d_un > rate:
                d_un = rate
            elif d_un < -rate:
                d_un = -rate
            q_deg = prev + d_un
        last[jn] = q_deg
        positions[name_idx[jn]] = q_deg * math.pi / 180.0
        # 추종 오차 (타깃 vs 실측) — 최대값을 하트비트에 노출.
        # 커지면 = 드라이브 무력(토크 오프) 또는 관절 한계 충돌 — "토크 풀림" 판별용.
        err = abs(q_deg - float(cur[name_idx[jn]]) * 180.0 / math.pi)
        if err > _passive_dbg.get("trk_err", 0.0):
            _passive_dbg["trk_err"] = err
        if dbg_jn is None:
            dbg_jn = jn
        wrote = True
    # 하트비트 진단 — 퍼펫 실동작 여부(타깃 기입)와 물리 추종 여부(실측)를 로그로 남김
    if wrote:
        now = time.time()
        if not _passive_dbg["announced"]:
            _passive_dbg["announced"] = True
            _passive_dbg["t_print"] = now
            print("[Passive] puppet ACTIVE (%d joints, motor src=cmd>measured)"
                  % len(last), flush=True)
        elif now - _passive_dbg["t_print"] >= 5.0:
            _passive_dbg["t_print"] = now
            mtxt = ",".join("T%d=%.1f" % (k, motors[k]) for k in sorted(motors))
            i_dbg = name_idx.get(dbg_jn, -1)
            meas = float(cur[i_dbg]) * 180.0 / math.pi if i_dbg >= 0 else 0.0
            print("[Passive] hb %s | %s: tgt=%.1f meas=%.1f | trk_err=%.1f | eval_fail=%d"
                  % (mtxt, dbg_jn, last.get(dbg_jn, 0.0), meas,
                     _passive_dbg.get("trk_err", 0.0), _passive_dbg["fail"]), flush=True)
            _passive_dbg["fail"] = 0
            _passive_dbg["trk_err"] = 0.0
    return wrote


# ============================================================================
# Physics step callback
# ============================================================================
def _on_physics_step(dt: float) -> None:
    # Scene 명령 (워커 스레드에서 enqueue 된 것) 을 메인 스레드에서 처리
    _process_scene_queue()
    now = time.time()
    for ridx, art in enumerate(articulations):
        if art is None:
            continue
        try:
            names = art.dof_names
            n = len(names)
        except Exception:
            continue

        positions = [None] * n
        velocities = [None] * n
        any_pos = False
        any_vel = False

        for i, jname in enumerate(names):
            # Velocity drive
            key = (ridx, jname)
            if key in joint_vel_targets:
                velocities[i] = joint_vel_targets[key]
                any_vel = True
                continue
            # Position drive interpolation
            if key in joint_pos_targets:
                t_end = joint_pos_t_end.get(key, now)
                t_start = joint_pos_t_start.get(key, now)
                target = joint_pos_targets[key]
                if now >= t_end or t_end <= t_start:
                    cmd = target
                else:
                    frac = (now - t_start) / (t_end - t_start)
                    origin = joint_pos_origin.get(key, target)
                    cmd = origin + (target - origin) * frac
                # actuator-net 타깃셰이핑 (revolute 만 — prismatic 은 원값,
                # 로봇-모델 불일치 시 게이트 OFF)
                if _actnet_models and _actnet_gate["on"] \
                        and not (ridx < len(prismatic_joint_names)
                                 and jname in prismatic_joint_names[ridx]):
                    positions[i] = _actnet_shape(key, jname, ridx, cmd, dt)
                else:
                    positions[i] = cmd
                any_pos = True

        # 종동(passive) 조인트 퍼펫팅 — 현재 모터각 기준 매 스텝 갱신 (타깃 유무와 무관)
        if _apply_passive_puppet(art, names, positions):
            any_pos = True

        if not any_pos and not any_vel:
            continue

        action = ArticulationAction(
            joint_positions=positions if any_pos else None,
            joint_velocities=velocities if any_vel else None,
        )
        try:
            art.apply_action(action)
        except Exception:
            pass


# ============================================================================
# Main
# ============================================================================
def main() -> None:
    global world

    if len(sys.argv) < 2:
        print("usage: isaac_runner.py <urdf_or_usd_path>", flush=True)
        simulation_app.close()
        return

    urdf_path = os.path.abspath(sys.argv[1])   # 변수명은 호환성 위해 유지 (실제 USD 도 가능)
    if not os.path.exists(urdf_path):
        print("[Init] file not found: %s" % urdf_path, flush=True)
        simulation_app.close()
        return
    _load_passive_defs(os.path.dirname(urdf_path))  # 종동 조인트 퍼펫팅 메타 (있을 때만)
    if _is_usd_path(urdf_path):
        print("[Init] load mode = USD (%s)" % urdf_path, flush=True)
    else:
        print("[Init] load mode = URDF (%s)" % urdf_path, flush=True)
        # ★Isaac 6.0: URDF→USD 변환은 컨텍스트 스테이지를 갈아치우므로
        #   World 생성 "전"에 선변환해 둔다 (spawn_robot 은 캐시 히트로 안전).
        if ISAAC6:
            try:
                _isaac6_convert_urdf(urdf_path)
            except Exception as _cex:
                print("[Init] Isaac6 pre-convert failed: %s" % _cex, flush=True)
                simulation_app.close()
                return

    world = World(stage_units_in_meters=1.0)
    _gf = os.environ.get("OJW_GROUND_FRICTION", "").strip()
    if _gf:
        try:
            _gfv = float(_gf)
            world.scene.add_default_ground_plane(static_friction=_gfv, dynamic_friction=_gfv)
            print("[Init] ground friction=%.2f (OJW_GROUND_FRICTION)" % _gfv, flush=True)
        except Exception as _gex:
            print("[Init] ground friction 적용 실패(%s) — 기본값" % _gex, flush=True)
            world.scene.add_default_ground_plane()
    else:
        world.scene.add_default_ground_plane()

    # SPAWN_Z: ①환경변수 OJW_SPAWN_Z (MakeUrdf UI 의 spawn 높이 — 최우선)
    #          ②viewer_config.txt  ③default 0.4
    #   (이전: env 를 안 읽어서 UI 값과 무관하게 낡은 viewer_config 0.4 가 쓰였다)
    _spawn_z = 0.4
    _env_spawn = os.environ.get("OJW_SPAWN_Z", "").strip()
    if _env_spawn:
        try:
            _spawn_z = float(_env_spawn)
            print("[OJW] SPAWN_Z = %s (OJW_SPAWN_Z env)" % _spawn_z, flush=True)
        except Exception:
            _env_spawn = ""
    if not _env_spawn:
        _spawn_z = 0.4
        try:
            _cfg_path = os.path.join(os.path.dirname(os.path.abspath(urdf_path)), "viewer_config.txt")
            if os.path.isfile(_cfg_path):
                with open(_cfg_path, 'r', encoding='utf-8-sig') as _f:
                    for _ln in _f:
                        _ln = _ln.strip()
                        if _ln.startswith('SPAWN_Z='):
                            _spawn_z = float(_ln.split('=', 1)[1].strip())
                            break
            print(f"[OJW] SPAWN_Z = {_spawn_z} (viewer_config)", flush=True)
        except Exception as _ex:
            print(f"[OJW] SPAWN_Z read failed, using default 0.4: {_ex}", flush=True)

    # First robot (extend with spawn_robot(...) calls below for multi-robot RL)
    spawn_robot(urdf_path, robot_idx=0, position=(0.0, 0.0, _spawn_z))
    # Example multi-robot (uncomment for RL parallel envs):
    # spawn_robot(urdf_path, robot_idx=1, position=(1.5, 0.0, 0.0))
    # spawn_robot(urdf_path, robot_idx=2, position=(3.0, 0.0, 0.0))

    # Scene objects from URDF <scene><include .../></scene> nodes
    # (box primitives — known URLs like cube_small / tray sized appropriately)
    _spawn_scene_objects(urdf_path)

    _post_spawn_init(_spawn_z)

    world.add_physics_callback("runner_step", _on_physics_step)

    # Start socket server
    t = threading.Thread(target=_socket_thread, args=(SOCKET_HOST, SOCKET_PORT), daemon=True)
    t.start()

    _run_main_loop(urdf_path, _spawn_z)


def _post_spawn_init(spawn_z: float) -> None:
    """스폰 직후 공통 초기화 — main() 최초 1회 + RELOAD(핫 리로드) 재사용.
    articulation 생성/default state, world.reset, 관절 hold, 드라이브 강화,
    카메라 attach/wrap/뷰포트까지."""
    # ── Articulation 들을 reset *전* 에 미리 생성 + set_default_state 로 SPAWN_Z 등록 ──
    #    world.reset() 이 articulation 의 default state 를 적용하므로, reset 전에
    #    base pose 를 default 로 박아두면 reset 후에도 자동 유지됨.
    #    (이전 버그: reset 후 set_world_pose 만 호출 → physics view 가 이미 (0,0,0) 캐시한 상태)
    # 델타 폐루프 조인트 저작 — world.reset() 의 physics 파싱 *전* 에 있어야 반영됨
    _author_delta_closures(spawn_z)
    # 폐루프 모드 dt (경로별 실측 최적):
    #   USD 경로 = 1/240 — 1/60 은 dpass_4(팔1 되감기)가 4° 무름, 1/240 에서 1.4° (실측)
    #   URDF 경로 = 1/60 유지 — 1/240 은 오히려 T2 스톨(41° 고착) 유발, 1/60 은 전 자세 정상 (실측)
    if _closure_defs and _ojw_usd_mode[0]:
        try:
            world.get_physics_context().set_physics_dt(1.0 / 240.0)
            print("[Closure] physics dt = 1/240 (USD 경로 폐루프 구속 강성 확보)", flush=True)
        except Exception as _ex:
            print("[Closure] physics dt 설정 실패: %s" % _ex, flush=True)
    import numpy as _np
    _default_pos = _np.array([0.0, 0.0, spawn_z], dtype=_np.float32)
    _default_orn = _np.array([1.0, 0.0, 0.0, 0.0], dtype=_np.float32)  # wxyz identity
    for prim_path in articulation_paths:
        try:
            art = SingleArticulation(prim_path=prim_path, name=os.path.basename(prim_path))
            try:
                art.set_default_state(position=_default_pos, orientation=_default_orn)
                print("[Init] default_state z=%.3f registered for %s" % (spawn_z, prim_path), flush=True)
            except Exception as _ex:
                print("[Init] set_default_state failed for %s: %s" % (prim_path, _ex), flush=True)
            articulations.append(art)
        except Exception as ex:
            print("[Init] pre-reset articulation wrap failed for %s: %s" % (prim_path, ex), flush=True)
            articulations.append(None)

    world.reset()

    # ── Viewport camera 자동 fit (default 가 너무 멀어 robot 이 점으로 보임)
    #    /OmniverseKit_Persp 카메라를 robot 가까이로 옮기고 origin 을 바라보게 회전.
    #    실패해도 무시 — 기능 자체는 fail-soft.
    try:
        from pxr import Gf, UsdGeom  # type: ignore
        _stg = simulation_app.context.get_stage()
        for _cam_path in ("/OmniverseKit_Persp", "/Render/Camera"):
            _cam = _stg.GetPrimAtPath(_cam_path)
            if _cam and _cam.IsValid():
                _xf = UsdGeom.Xformable(_cam)
                # 기존 xformOp 재사용 — Kit persp 카메라는 이미 translate/rotateXYZ/scale 을 갖고 있어
                # ClearXformOpOrder+AddTranslateOp 는 'already exists in xformOpOrder' 에러가 남.
                _ops = {}
                for _op in _xf.GetOrderedXformOps():
                    _ops[_op.GetOpType()] = _op
                _tr = _ops.get(UsdGeom.XformOp.TypeTranslate) or _xf.AddTranslateOp()
                _rot = _ops.get(UsdGeom.XformOp.TypeRotateXYZ) or _xf.AddRotateXYZOp()
                # 1.2 m 거리, 45° isometric (robot 정면+위쪽) — precision(double/float) 양쪽 대응
                try:
                    _tr.Set(Gf.Vec3d(1.0, 1.0, 0.6))
                except Exception:
                    _tr.Set(Gf.Vec3f(1.0, 1.0, 0.6))
                try:
                    _rot.Set(Gf.Vec3d(-25.0, 0.0, 135.0))
                except Exception:
                    _rot.Set(Gf.Vec3f(-25.0, 0.0, 135.0))
                print(f"[Cam] viewport camera moved to (1.0, 1.0, 0.6) for capture", flush=True)
                break
    except Exception as _ex:
        print(f"[Cam] viewport camera setup failed: {_ex}", flush=True)

    # Initialize articulations (이미 생성됨, set_default_state 등록 완료) + SPAWN_Z 보험 + joint hold
    for art in articulations:
        if art is None:
            continue
        try:
            art.initialize()
            try:
                print("[Init] DOFs: %s" % list(art.dof_names), flush=True)
            except Exception:
                pass
            # ── ActNet 적용 게이트 판정 (첫 로봇 기준 1회) ──
            try:
                if _actnet_models and art is articulations[0]:
                    _dof = set(art.dof_names)
                    _mdl = set(_actnet_models.keys())
                    _cov = len(_mdl & _dof) / float(max(1, len(_mdl)))
                    _actnet_gate["on"] = (_cov >= 0.6)
                    print("[ActNet] robot match %.0f%% (%d/%d joints) -> %s"
                          % (_cov * 100.0, len(_mdl & _dof), len(_mdl),
                             "ON" if _actnet_gate["on"] else
                             "OFF (다른 로봇용 모델 — 셰이핑 비활성)"), flush=True)
            except Exception:
                pass
            # ── SPAWN_Z 보험 재적용 (default_state 가 살아남았으면 no-op, 아니면 강제 적용) ──
            try:
                art.set_world_pose(position=_default_pos, orientation=_default_orn)
                print("[Init] base set_world_pose z=%.3f (insurance)" % spawn_z, flush=True)
            except Exception as _ex:
                print("[Init] set_world_pose failed: %s" % _ex, flush=True)
            # ── 관절 강제 유지: 현재 위치 + 속도=0 으로 직접 박고 + target 도 동일하게 설정 ──
            #   set_joint_position_targets 만으로는 떨어지는 충격을 흡수 못함 (target 만 설정).
            #   set_joint_positions 로 실제 qpos 를 박고, velocities=0 으로 잔여 운동 제거.
            try:
                _cur = art.get_joint_positions()
                if _cur is not None:
                    import numpy as _np
                    _zero_vel = _np.zeros_like(_cur)
                    try:
                        art.set_joint_positions(_cur)
                    except Exception:
                        pass
                    try:
                        art.set_joint_velocities(_zero_vel)
                    except Exception:
                        pass
                    # Isaac 5.x: SingleArticulation 에 set_joint_position_targets 없음 → apply_action 사용
                    art.apply_action(ArticulationAction(joint_positions=_cur))
                    print("[Init] hold joint pos+vel+target at current positions", flush=True)
            except Exception as _ex:
                print("[Init] joint hold setup failed: %s" % _ex, flush=True)

            # ── USD DriveAPI 로 각 joint drive 의 stiffness/damping 추가 강화 ──
            #   import_config 의 default 가 모든 joint 에 적용 안 되는 경우가 있어 explicit 적용.
            try:
                from pxr import UsdPhysics  # type: ignore
                _stg = omni.usd.get_context().get_stage()
                _art_path = articulation_paths[articulations.index(art)] if art in articulations else None
                if _art_path:
                    _joints_prim = _stg.GetPrimAtPath(_art_path + "/joints")
                    if _joints_prim and _joints_prim.IsValid():
                        _hardened = 0
                        for _jprim in _joints_prim.GetChildren():
                            # continuous joint (velocity drive) 는 _set_joint_to_velocity_drive 가 이미 처리.
                            # 여기서는 position drive (stiffness>0) 만 강화 — velocity drive 는 손대지 않음.
                            # ★폐루프 모드의 델타 관절은 제외 — _author_delta_closures 가 dpass 해제/
                            #   T 완화(deg 8.73 = rad 500)를 해뒀는데 여기서 1e9 로 되굳히면
                            #   그램급 링크가 폭력 가속돼 팔꿈치 플립 분기로 도약한다 (실측 사고)
                            _jn0 = _jprim.GetName()
                            if _closure_defs and (_jn0.startswith("dpass_")
                                                  or (_jn0.startswith("T") and _jn0[1:].isdigit())):
                                continue

                            # angular drive (revolute / continuous)
                            try:
                                _drv_ang = UsdPhysics.DriveAPI.Get(_jprim, "angular")
                                if _drv_ang:
                                    _s = _drv_ang.GetStiffnessAttr().Get() or 0
                                    _d = _drv_ang.GetDampingAttr().Get() or 0
                                    if _s > 0:    # position drive 만 강화 (velocity drive 는 stiffness=0 유지)
                                        _drv_ang.GetStiffnessAttr().Set(max(_s, 1e9))
                                        _drv_ang.GetDampingAttr().Set(max(_d, 1e6))
                                        _hardened += 1
                            except Exception:
                                pass
                            # linear drive (prismatic)
                            try:
                                _drv_lin = UsdPhysics.DriveAPI.Get(_jprim, "linear")
                                if _drv_lin:
                                    _s = _drv_lin.GetStiffnessAttr().Get() or 0
                                    _d = _drv_lin.GetDampingAttr().Get() or 0
                                    if _s > 0:
                                        _drv_lin.GetStiffnessAttr().Set(max(_s, 1e9))
                                        _drv_lin.GetDampingAttr().Set(max(_d, 1e6))
                                        _hardened += 1
                            except Exception:
                                pass
                        print("[Init] joint drives hardened: %d" % _hardened, flush=True)
            except Exception as _ex:
                print("[Init] drive hardening failed: %s" % _ex, flush=True)
        except Exception as ex:
            print("[Init] articulation init failed: %s" % ex, flush=True)

    _attach_cameras_after_reset()
    _spawn_world_cameras_after_reset()
    _wrap_usd_cameras_after_reset()   # USD 내장 Camera prim → 센서 등록 (CAM_GET/MJPEG 용)
    _open_camera_viewports()   # 카메라별 실시간 피드 창 (텔레옵/영상인식 확인용)


def _despawn_and_reload(urdf_path: str, spawn_z: float) -> None:
    """RELOAD (Apply DH 핫 리로드): 로봇/카메라 prim 제거 → URDF 재변환 → 재스폰.
    6.0 변환은 standalone stage 라 세션 중에도 안전. ★메인 스레드에서만 호출."""
    global _world_cameras_loaded
    print("[Reload] Apply DH 핫 리로드 시작", flush=True)
    _load_passive_defs(os.path.dirname(urdf_path))  # 재-Export 로 갱신된 퍼펫팅 메타 재로드
    try:
        try:
            world.stop()
        except Exception:
            pass
        stage = omni.usd.get_context().get_stage()
        for p in list(articulation_paths):
            try:
                stage.RemovePrim(p)
            except Exception:
                pass
        try:
            stage.RemovePrim("/World/scene_cameras")
        except Exception:
            pass
        for w in _camera_viewport_wins:
            try:
                w.destroy()
            except Exception:
                try:
                    w.visible = False
                except Exception:
                    pass
        _camera_viewport_wins[:] = []
        articulations[:] = []
        articulation_paths[:] = []
        camera_specs[:] = []
        camera_objs.clear()
        world_camera_specs[:] = []
        world_camera_objs.clear()
        continuous_joint_names[:] = []
        prismatic_joint_names[:] = []
        mimic_relationships[:] = []
        for d in (joint_pos_targets, joint_pos_origin, joint_pos_t_start,
                  joint_pos_t_end, joint_vel_targets):
            d.clear()
        # ★actnet 내부 상태도 반드시 청소 — 리로드 전의 격한 이력이 남으면
        #   새 로봇의 타깃 셰이핑이 계속 오염돼 "부서진 영향이 리프레시 후에도
        #   지속" 되는 실사고 (2026-08-26)
        _actnet_state.clear()
        with _mjpeg_lock:
            _mjpeg_frames.clear()
        _world_cameras_loaded = False
        _isaac6_usd_cache.clear()   # URDF 가 바뀌었으니 재변환 (unique-path 로 새 폴더)

        spawn_robot(urdf_path, robot_idx=0, position=(0.0, 0.0, spawn_z))
        _spawn_scene_objects(urdf_path)
        _post_spawn_init(spawn_z)
        print("[Reload] 완료 — 새 EDH/URDF 가 시뮬에 반영됨", flush=True)
    except Exception as ex:
        import traceback
        print("[Reload] 실패: %s\n%s" % (ex, traceback.format_exc()), flush=True)


def _dump_state() -> None:
    """진단용 상태 덤프 (OJW_STATE_DUMP_SEC) — 관절각(deg) + 링크 world 위치.
    조립 이상(조인트 미바인딩/프레임 오해석) 을 수치로 특정하기 위한 것."""
    try:
        if not articulations or articulations[0] is None:
            return
        art = articulations[0]
        names = art.dof_names
        q = art.get_joint_positions()
        if q is not None:
            print("[Dump] q(deg): " + ", ".join(
                "%s=%.2f" % (n, float(v) * 180.0 / math.pi)
                for n, v in zip(names, q)), flush=True)
        stage = omni.usd.get_context().get_stage()
        root = stage.GetPrimAtPath(articulation_paths[0])
        if not root or not root.IsValid():
            return
        cache = UsdGeom.XformCache(Usd.TimeCode.Default())
        for prim in Usd.PrimRange(root):
            if prim.HasAPI(UsdPhysics.RigidBodyAPI):
                t = cache.GetLocalToWorldTransform(prim).ExtractTranslation()
                print("[Dump] %s pos=(%.4f, %.4f, %.4f)"
                      % (prim.GetName(), t[0], t[1], t[2]), flush=True)
    except Exception as ex:
        print("[Dump] failed: %s" % ex, flush=True)


def _run_main_loop(urdf_path: str, _spawn_z: float) -> None:
    """소켓 이후의 서버/메인 루프 (main 에서 분리 — RELOAD 처리 포함)."""
    # Ojw.CStream 호환 MJPEG 스트리밍 서버 (OJW_MJPEG_PORT=0 이면 끔)
    try:
        _mjpeg_port = int(os.environ.get("OJW_MJPEG_PORT", "8081") or 0)
    except Exception:
        _mjpeg_port = 0
    if _mjpeg_port > 0:
        threading.Thread(target=_mjpeg_server_thread, args=(_mjpeg_port,), daemon=True).start()
    _mjpeg_last = [0.0]

    # Main loop
    print("[Init] running. Connect %s:%d to drive joints." % (SOCKET_HOST, SOCKET_PORT), flush=True)

    # ── 자동 종료 (캡쳐 모드용) — 환경변수 OJW_AUTO_EXIT_SEC ─────────────
    try:
        _ojw_auto_exit_sec = float(os.environ.get('OJW_AUTO_EXIT_SEC', '0') or 0)
    except Exception:
        _ojw_auto_exit_sec = 0.0
    _ojw_auto_exit_start = time.time() if _ojw_auto_exit_sec > 0 else None
    if _ojw_auto_exit_start is not None:
        print(f"[OJW] AUTO_EXIT_SEC={_ojw_auto_exit_sec} (capture mode)", flush=True)

    # ── 상태 덤프 주기 (진단용) — 환경변수 OJW_STATE_DUMP_SEC ────────────
    try:
        _ojw_dump_sec = float(os.environ.get('OJW_STATE_DUMP_SEC', '0') or 0)
    except Exception:
        _ojw_dump_sec = 0.0
    _ojw_dump_last = [time.time()]

    # ── cv2 카메라 윈도우 (PyBullet 과 동일한 UX, OPT-IN) ──────────────
    #   기본 OFF — Isaac Sim 의 Omniverse Kit GUI 와 cv2 HighGUI 가 같은 프로세스에서
    #   충돌할 가능성이 있어 사용자가 명시적으로 켤 때만 활성화.
    #   켜려면 환경변수 OJW_ISAAC_CV2=1 설정 (또는 MakeUrdf 측에서 자동 전달).
    # numpy 는 CAM_GET 브리지(_isaac_get_rgb)도 사용 — cv2 창 여부와 무관하게 항상 import
    import numpy as _np_cv
    _cv2_ok = False
    _enable_cv2 = (os.environ.get("OJW_ISAAC_CV2", "0").strip() == "1")
    if _enable_cv2:
        try:
            import cv2 as _cv2  # type: ignore
            _cv2_ok = True
            _n_total = sum(len(cs) for cs in camera_specs) + len(world_camera_specs)
            print(f"[Camera] cv2 ON ({_n_total} cameras) - OJW_ISAAC_CV2=1", flush=True)
        except Exception as _ex:
            print(f"[Camera] cv2 import 실패 - pip install opencv-python ({_ex})", flush=True)
    else:
        print("[Camera] cv2 OFF - Viewport 에서 카메라 직접 선택하세요 "
              "(Window>Viewports>Viewport 2). cv2 활성화: OJW_ISAAC_CV2=1", flush=True)

    # 마지막 캡쳐 시각 — (ridx, name) 또는 ("world", name) 키
    _cam_last_render = {}

    def _isaac_get_rgb(obj):
        """Isaac Camera 객체에서 RGB 배열 가져오기 (버전별 API 차이 대응)."""
        try:
            rgba = obj.get_rgba()
            if rgba is None:
                return None
            arr = _np_cv.asarray(rgba)
            if arr.ndim == 3 and arr.shape[2] >= 3:
                return arr[:, :, :3]
        except Exception:
            pass
        try:
            frame = obj.get_current_frame()
            if frame and "rgba" in frame:
                arr = _np_cv.asarray(frame["rgba"])
                if arr.ndim == 3 and arr.shape[2] >= 3:
                    return arr[:, :, :3]
        except Exception:
            pass
        return None

    # ── 크래시 로그 (DOS 창이 닫혀도 원인 추적 가능하도록) ──────────────
    #   urdf 폴더 옆에 isaac_crash.log 작성. main loop 의 예외를 잡아 stack trace 기록.
    _crash_log_path = os.path.join(os.path.dirname(urdf_path), "isaac_crash.log")

    try:
        while simulation_app.is_running():
            if _ojw_auto_exit_start is not None and (time.time() - _ojw_auto_exit_start) >= _ojw_auto_exit_sec:
                print(f"[OJW] AUTO_EXIT reached ({_ojw_auto_exit_sec}s) - exiting", flush=True)
                break
            world.step(render=True)

            # === RELOAD (Apply DH 핫 리로드) — 메인 스레드에서 처리 ===
            if _reload_pending[0]:
                _reload_pending[0] = False
                _despawn_and_reload(urdf_path, _spawn_z)

            # === 상태 스냅샷 갱신 (GETPOS/GETSCENE 회신용) + CAM_GET 요청 처리 ===
            try:
                _update_state_snapshots()
            except Exception:
                pass
            _serve_cam_requests(_isaac_get_rgb)

            # === 진단 상태 덤프 (OJW_STATE_DUMP_SEC 초마다) ===
            if _ojw_dump_sec > 0 and (time.time() - _ojw_dump_last[0]) >= _ojw_dump_sec:
                _ojw_dump_last[0] = time.time()
                _dump_state()

            # === MJPEG 프레임 펌프 (Ojw.CStream 소비용, ~15fps) ===
            #   카메라 캡처는 메인 스레드 전용이라 여기서 찍어 두고
            #   HTTP 스레드는 최신 JPEG 만 서빙한다.
            if _mjpeg_port > 0:
                _mnow = time.time()
                if _mnow - _mjpeg_last[0] >= 0.066:
                    _mjpeg_last[0] = _mnow
                    _mj_items = list(camera_objs.items()) + \
                                [(("world", _n), _o) for _n, _o in world_camera_objs.items()]
                    for _mj_key, _mj_obj in _mj_items:
                        try:
                            _mj_rgb = _isaac_get_rgb(_mj_obj)
                            if _mj_rgb is None:
                                continue
                            _mj_jpg = _encode_jpeg(_mj_rgb)
                            if _mj_jpg:
                                with _mjpeg_lock:
                                    _mjpeg_frames[_mj_key[1]] = _mj_jpg
                        except Exception:
                            pass

            # === cv2 카메라 윈도우 갱신 (update_rate 제한) ===
            # 전체를 try/except 로 감싸 cv2 문제가 main loop 죽이지 않도록 격리
            if _cv2_ok:
                try:
                    _now = time.time()
                    # 부착 카메라
                    for ridx, cams in enumerate(camera_specs):
                        for cam in cams:
                            key = (ridx, cam["name"])
                            interval = 1.0 / max(1, cam.get("rate", 30))
                            if _now - _cam_last_render.get(key, 0.0) < interval:
                                continue
                            _cam_last_render[key] = _now
                            obj = camera_objs.get((ridx, cam["name"]))
                            if obj is None:
                                continue
                            rgb = _isaac_get_rgb(obj)
                            if rgb is None:
                                continue
                            try:
                                bgr = _np_cv.ascontiguousarray(rgb[:, :, ::-1].astype(_np_cv.uint8))
                                _cv2.imshow(cam["name"] or f"camera_{ridx}_{cam.get('idx',0)}", bgr)
                            except Exception:
                                pass
                    # World 카메라
                    for cam in world_camera_specs:
                        key = ("world", cam["name"])
                        interval = 1.0 / max(1, cam.get("rate", 30))
                        if _now - _cam_last_render.get(key, 0.0) < interval:
                            continue
                        _cam_last_render[key] = _now
                        obj = world_camera_objs.get(cam["name"])
                        if obj is None:
                            continue
                        rgb = _isaac_get_rgb(obj)
                        if rgb is None:
                            continue
                        try:
                            bgr = _np_cv.ascontiguousarray(rgb[:, :, ::-1].astype(_np_cv.uint8))
                            _cv2.imshow(cam["name"], bgr)
                        except Exception:
                            pass
                    # ESC 누르면 cv2 윈도우만 닫음 (Isaac 메인은 계속)
                    _k = _cv2.waitKey(1) & 0xFF
                    if _k == 27:
                        _cv2.destroyAllWindows()
                except Exception as _cv_ex:
                    # cv2 한 회차 실패는 무시하고 main loop 계속
                    print(f"[Camera] cv2 step error (계속 진행): {_cv_ex}", flush=True)
    except Exception as _main_ex:
        import traceback
        _tb = traceback.format_exc()
        print("[FATAL] main loop 예외:", flush=True)
        print(_tb, flush=True)
        # 파일로도 기록 — DOS 창이 즉시 닫혀도 원인 추적 가능
        try:
            with open(_crash_log_path, 'w', encoding='utf-8') as _f:
                _f.write(f"Isaac main loop crash at {time.strftime('%Y-%m-%d %H:%M:%S')}\n\n")
                _f.write(f"Exception: {_main_ex}\n\n")
                _f.write("Traceback:\n")
                _f.write(_tb)
            print(f"[FATAL] crash log written: {_crash_log_path}", flush=True)
        except Exception:
            pass
        # 콘솔이 닫히기 전 잠시 대기 (사용자가 메시지 읽을 수 있도록)
        try:
            time.sleep(10)
        except Exception:
            pass

    try:
        if _cv2_ok:
            _cv2.destroyAllWindows()
    except Exception:
        pass

    # ★소켓 즉시 반납 — simulation_app.close() 가 행으로 멈춰 프로세스가 좀비로 남아도
    #   포트 4000 이 풀려 있어야 다음 인스턴스가 정상 바인드/명령 수신한다.
    try:
        if _server_sock[0] is not None:
            _server_sock[0].close()
            _server_sock[0] = None
            print("[Sock] closed (port released)", flush=True)
    except Exception:
        pass

    simulation_app.close()


if __name__ == "__main__":
    main()
