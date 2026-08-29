import pybullet as p
import pybullet_data
import time
import sys
import math

# 한글 Windows (cp949) 에서 em-dash 등 유니코드 print 실패 방지 — stdout 을 utf-8 로 reconfigure
try:
    sys.stdout.reconfigure(encoding='utf-8', errors='replace')
    sys.stderr.reconfigure(encoding='utf-8', errors='replace')
except Exception:
    pass
import xml.etree.ElementTree as ET
import os
import socket
import threading
import queue
import json as _json  # 트랙 벨트 메타데이터 로드용

# Windows 비블로킹 키보드 입력
if sys.platform == 'win32':
    import msvcrt
from dataclasses import dataclass
from typing import Dict, Optional, List, Tuple
from collections import defaultdict

@dataclass
class JointMoveCommand:
    """조인트 이동 명령을 저장하는 데이터 클래스"""
    joint_id: int       # 조인트 ID
    target_angle: float # 목표 각도 (도 단위)
    duration: float     # 이동 시간 (초 단위)
    start_time: float   # 시작 시간
    start_angle: float  # 시작 각도

# PyBullet 물리 엔진 초기화
physicsClient = p.connect(p.GUI)
p.setAdditionalSearchPath(pybullet_data.getDataPath())
p.setGravity(0, 0, -9.81)  # 중력 설정

# GUI 설정 - 슬라이더는 보이도록 유지
p.configureDebugVisualizer(p.COV_ENABLE_GUI, 1)  # GUI 활성화 (슬라이더 표시)
p.configureDebugVisualizer(p.COV_ENABLE_SHADOWS, 1)  # 그림자 활성화
p.configureDebugVisualizer(p.COV_ENABLE_SEGMENTATION_MARK_PREVIEW, 0)
p.configureDebugVisualizer(p.COV_ENABLE_DEPTH_BUFFER_PREVIEW, 0)
p.configureDebugVisualizer(p.COV_ENABLE_RGB_BUFFER_PREVIEW, 0)

# 카메라 초기 위치 설정
p.resetDebugVisualizerCamera(
    cameraDistance=0.8,     # 카메라 거리
    cameraYaw=45,           # 좌우 회전각
    cameraPitch=-10,        # 상하 회전각
    cameraTargetPosition=[0, 0, 0.1]  # 카메라가 바라보는 지점
)

# 바닥 평면 로드
plane_id = p.loadURDF("plane.urdf")

# URDF 파일 경로 설정 (Release 폴더 기준)
script_dir = os.path.dirname(os.path.abspath(__file__))
release_dir = os.path.join(script_dir, "..", "Release")  # python/../Release

# C#에서 생성한 URDF 파일 사용
urdf_file = os.path.join(release_dir, "robot_stl.urdf")

# 작업 디렉토리를 Release 폴더로 변경 (메시 파일 로딩을 위해)
original_cwd = os.getcwd()
if os.path.exists(release_dir):
    os.chdir(release_dir)
    print(f"Working directory changed to: {os.path.abspath(release_dir)}")

# URDF 파일 존재 확인
if not os.path.exists(urdf_file):
    print(f"ERROR: URDF file not found: {urdf_file}")
    print("Please run the C# application first to generate the URDF file.")
    print("Also make sure STL mesh files are in the Release folder.")
    sys.exit(1)

# URDF에서 참조하는 STL 파일들 확인
def check_mesh_files(urdf_path):
    """URDF 파일에서 참조하는 메시 파일들이 존재하는지 확인"""
    missing_files = []
    try:
        tree = ET.parse(urdf_path)
        root = tree.getroot()
        for mesh in root.findall('.//mesh'):
            filename = mesh.get('filename')
            if filename:
                # package:// 프로토콜 제거
                if filename.startswith('package://'):
                    filename = filename.split('/', 3)[-1] if filename.count('/') >= 3 else filename
                # 절대 경로가 아니면 Release 폴더 기준으로 확인
                if not os.path.isabs(filename):
                    full_path = os.path.join(release_dir, filename)
                else:
                    full_path = filename
                if not os.path.exists(full_path):
                    missing_files.append(filename)
    except Exception as e:
        print(f"Warning: Could not parse URDF for mesh files: {e}")
    return missing_files

missing_meshes = check_mesh_files(urdf_file)
if missing_meshes:
    print(f"\nWARNING: {len(missing_meshes)} mesh file(s) not found:")
    for mf in missing_meshes[:5]:  # 최대 5개만 표시
        print(f"  - {mf}")
    if len(missing_meshes) > 5:
        print(f"  ... and {len(missing_meshes) - 5} more")
    print(f"\nMesh files should be in: {os.path.abspath(release_dir)}")
    print("The robot may not display correctly without these files.")
    print()

####################################################
# 설정 파일 읽기 (C#에서 생성)
####################################################
def read_viewer_config():
    """viewer_config.txt 파일에서 설정 읽기 (bool + float 혼합)."""
    config = {
        'FIXED_BASE': True,
        'USE_PYBULLET': True,
        'USE_MUJOCO': False,
        'SPAWN_Z': 0.4,   # float: 로봇 base 의 초기 z (m). fixed/non-fixed 모두 동일하게 적용.
    }
    config_file = os.path.join(release_dir, "viewer_config.txt")
    if os.path.exists(config_file):
        try:
            with open(config_file, 'r', encoding='utf-8-sig') as f:
                for line in f:
                    line = line.strip()
                    if line.startswith('#') or '=' not in line:
                        continue
                    key, value = line.split('=', 1)
                    key = key.strip()
                    value = value.strip()
                    if key not in config:
                        continue
                    if isinstance(config[key], bool):
                        config[key] = (value == '1')
                    elif isinstance(config[key], float):
                        try:
                            config[key] = float(value)
                        except ValueError:
                            pass
            print(f"Config loaded from: {config_file}")
        except Exception as e:
            print(f"Warning: Could not read config file: {e}")
    else:
        print(f"Note: Config file not found, using defaults")
    return config

viewer_config = read_viewer_config()
USE_FIXED_BASE = viewer_config['FIXED_BASE']
SPAWN_Z = float(viewer_config.get('SPAWN_Z', 0.4))
print(f"Spawn Z: {SPAWN_Z}")
print(f"Fixed Base: {'Enabled' if USE_FIXED_BASE else 'Disabled'}")

print("\n" + "=" * 60)
print(" PyBullet URDF Viewer with Joint Sliders & Socket Control")
print("=" * 60)
print(f"Loading: {urdf_file}")

####################################################
# Scene 객체 로드 — URDF 의 <scene><include .../></scene> 노드 자동 파싱
#   MakeUrdf 의 $scene 라인이 만든 메타데이터를 읽어 객체 자동 배치
#   체크박스 Export $scene OFF 였으면 <scene> 노드 자체가 없어 아무것도 안 함
####################################################
import xml.etree.ElementTree as _ET

def _parse_scene_from_urdf(path):
    """URDF 의 <scene><include .../></scene> 노드를 파싱하여 객체 dict 리스트 반환."""
    items = []
    try:
        tree = _ET.parse(path)
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
                        color = [((cv >> 16) & 0xFF) / 255.0,
                                 ((cv >> 8) & 0xFF) / 255.0,
                                 (cv & 0xFF) / 255.0, 1.0]
                    except Exception:
                        color = None
                items.append({"url": url, "xyz": xyz, "rpy": rpy, "scale": scale, "color": color})
    except Exception as ex:
        print(f"[Scene] parse error: {ex}", flush=True)
    return items


_urdf_for_scene = urdf_to_load if 'urdf_to_load' in dir() else os.path.join(release_dir, "robot_stl.urdf")
scene_items = _parse_scene_from_urdf(_urdf_for_scene)
scene_ids = []
for it in scene_items:
    try:
        orn = p.getQuaternionFromEuler(it["rpy"])
        bid = p.loadURDF(it["url"], it["xyz"], orn, globalScaling=it["scale"])
        scene_ids.append(bid)
        if it["color"] is not None:
            p.changeVisualShape(bid, -1, rgbaColor=it["color"])
    except Exception as ex:
        print(f"[Scene] loadURDF failed for {it['url']}: {ex}", flush=True)
print(f"[Scene] {len(scene_ids)} object(s) loaded from <scene> node", flush=True)
####################################################

# 로봇 링크 색상 팔레트 정의
COLOR_PRESETS = {
    'default': [0.7, 0.7, 0.7, 1.0],     # 회색 (기본)
    'node': [0.3, 0.7, 1.0, 1.0],        # 청록색 (노드용)
    'segbar': [0.2, 0.6, 0.9, 1.0],      # 파란색 (막대용)
    'motor': [1.0, 0.3, 0.3, 1.0],       # 빨간색 (메인 모터)
    'follower': [0.3, 1.0, 0.3, 1.0],    # 초록색 (팔로워)
    'base': [0.5, 0.5, 0.5, 1.0],        # 어두운 회색 (베이스)
}

# 전역 변수들
simulation_running = True               # 시뮬레이션 실행 상태

# ── 자동 종료 (캡쳐 모드용) ─────────────────────────────────────────────
# 환경변수 OJW_AUTO_EXIT_SEC 가 설정되면 그 초만큼 시뮬 후 자동 종료.
# C# 측 btnCaptureAll_Click 가 윈도우 캡쳐 마치고 정리할 때 사용.
_ojw_auto_exit_sec = 0.0
try:
    _ojw_auto_exit_sec = float(os.environ.get('OJW_AUTO_EXIT_SEC', '0') or 0)
except Exception:
    _ojw_auto_exit_sec = 0.0
_ojw_auto_exit_start = time.time() if _ojw_auto_exit_sec > 0 else None
if _ojw_auto_exit_start is not None:
    print(f"[OJW] AUTO_EXIT_SEC={_ojw_auto_exit_sec} (capture mode)", flush=True)
command_queue = queue.Queue()          # 소켓 명령 큐
# === $scene 라이브 적용 ===
#   MakeUrdf Scene 탭의 Apply 버튼이 SCENE_ADD/SCENE_CLEAR 명령을 보냄.
#   Play() 가 prefix 보고 scene_queue 에 enqueue, process_scene_commands() 가 main 루프에서 spawn.
scene_queue = queue.Queue()
scene_spawned_ids = []                 # spawn 된 body 들 (SCENE_CLEAR 시 removeBody)
# === viewer 내 직접 클릭 배치 ===
#   MakeUrdf 의 Scene 탭 라이브러리 버튼 클릭 시 SCENE_TOOL_SET 으로 채워짐.
#   active 인 동안 PyBullet 윈도우의 좌클릭이 ray cast → Z=0 plane → object spawn.
active_scene_tool = None              # dict: type, sx, sy, sz, color, mass, zoffset
scene_last_click_state = 0            # 좌클릭 down/up 트리거 dedup 용
active_moves: Dict[int, JointMoveCommand] = {}  # 현재 실행 중인 조인트 이동 명령들
joint_number_to_id = {}                # 조인트 번호(예: 11, 12) → PyBullet joint ID 매핑

# 로봇 위치 제어 변수들
robot_position_sliders = {}            # 로봇 위치 제어 슬라이더들
initial_robot_position = [0, 0, SPAWN_Z]   # 로봇 초기 위치 (fixed/non-fixed 모두 SPAWN_Z 적용)
current_robot_position = [0, 0, SPAWN_Z]

##########################################
# URDF 파일 파싱 및 조인트 제어 함수들
##########################################

def extract_joint_number(joint_name):
    """조인트 이름에서 번호 추출 (예: T11 -> 11, joint_5 -> 5)"""
    import re
    match = re.search(r'(\d+)', joint_name)
    if match:
        return int(match.group(1))
    return None

##########################################
# 카메라 sensor (REP 0119 <sensor> element) 지원
#   isaac_runner.py 와 동일한 파싱 방식 + PyBullet 의 getCameraImage 로 렌더.
#   cv2.imshow 로 별도 윈도우에 카메라 뷰 표시 (cv2 import 가능할 때만).
##########################################

def _parse_cameras_from_urdf(urdf_path):
    """URDF 의 <sensor> elements (REP 0119) 에서 camera 정보 추출.
    Returns: list of dict {name, link, hfov_rad, w, h, near, far, rate, origin_xyz, origin_rpy}.
    원점 (origin xyz/rpy) 는 부모 link 기준 — joint 없이 sensor element 직속.
    MakeUrdf 출력 형식 (camera0 link + camera0_joint + sensor) 와 호환."""
    cams = []
    try:
        tree = ET.parse(urdf_path)
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
                far  = float(img.get("far", "10.0"))
                rate = int(s.get("update_rate", "30"))
            except Exception:
                continue
            # world_fixed 비표준 확장 attribute — MakeUrdf 에서 $camera 의 14번째 필드 = 1 시 emit.
            #   true → robot 의 base_link 위치/자세 추적 X, 초기 world pose 에 고정 (천장 카메라 등).
            world_fixed = (s.get("world_fixed", "0") == "1")
            cams.append({
                "name":     s.get("name", ""),
                "link":     parent.get("link", ""),
                "hfov_rad": hfov,
                "w": w, "h": h,
                "near": near, "far": far,
                "rate": rate,
                "world_fixed": world_fixed,
                "fixed_pos": None,   # 첫 렌더 때 캡쳐
                "fixed_quat": None,
            })
    except Exception as ex:
        print(f"[Camera] parse failed: {ex}", flush=True)
    return cams

def _parse_world_cameras_json(json_path):
    """world_cameras.json (MakeUrdf 가 robot URDF 옆에 출력) 파싱.
    Returns: list of dict (camera_list 와 동일 포맷, world_pose 키로 절대좌표 명시).
    파일 없으면 빈 리스트.

    world_fixed 카메라는 robot URDF 와 무관하게 world 좌표에 고정 — robot 떨어져도 위치 유지.
    PyBullet 다중 env 학습 시 env_origin + offset 으로 확장 가능 (env_origin 인수는 추후).
    """
    import json as _json
    cams = []
    if not json_path or not os.path.exists(json_path):
        return cams
    try:
        # utf-8-sig — BOM 이 있어도 안전하게 파싱
        with open(json_path, 'r', encoding='utf-8-sig') as f:
            data = _json.load(f)
        for entry in data.get("cameras", []):
            pose = entry.get("pose", {})
            intr = entry.get("intrinsic", {})
            cams.append({
                "name":     entry.get("name", f"scene_cam{entry.get('idx', 0)}"),
                "link":     "(world)",
                "hfov_rad": float(intr.get("fov_deg", 60.0)) * math.pi / 180.0,
                "w":        int(intr.get("width", 512)),
                "h":        int(intr.get("height", 512)),
                "near":     float(intr.get("near", 0.05)),
                "far":      float(intr.get("far", 100.0)),
                "rate":     int(entry.get("update_rate", 30)),
                # world_pose: 절대 world 좌표 (또는 env-local). _render_camera_view 가 직접 사용.
                "world_pose": (
                    float(pose.get("x", 0.0)),
                    float(pose.get("y", 0.0)),
                    float(pose.get("z", 0.0)),
                    float(pose.get("roll",  0.0)),
                    float(pose.get("pitch", 0.0)),
                    float(pose.get("yaw",   0.0)),
                ),
            })
        print(f"[Camera] world_cameras.json 로드: {len(cams)}개 (robot 과 분리, world 고정)", flush=True)
    except Exception as ex:
        print(f"[Camera] world_cameras.json 파싱 실패: {ex}", flush=True)
    return cams

def _spawn_world_camera_marker(cam):
    """world camera 위치에 작은 빨간 박스 마커 spawn (시각 식별용, 충돌 없음)."""
    try:
        pose = cam.get("world_pose")
        if not pose:
            return
        x, y, z, roll, pitch, yaw = pose
        # X 방향 가장 김 = 광축, 빨간색 (URDF builder 의 cam_color_world 와 동일 톤)
        vs = p.createVisualShape(p.GEOM_BOX, halfExtents=[0.02, 0.01, 0.01],
                                  rgbaColor=[0.85, 0.1, 0.1, 1.0])
        quat = p.getQuaternionFromEuler([roll, pitch, yaw])
        p.createMultiBody(baseMass=0.0,
                          baseVisualShapeIndex=vs,
                          basePosition=[x, y, z],
                          baseOrientation=quat)
        print(f"[Camera] '{cam['name']}' 마커 spawn @ "
              f"({x:.3f},{y:.3f},{z:.3f})", flush=True)
    except Exception as ex:
        print(f"[Camera] 마커 spawn 실패 ({cam.get('name','')}): {ex}", flush=True)

def _find_link_id_by_name(robot_id, name):
    """링크 이름 → link index (PyBullet getLinkState 호출용).
    base_link 면 -1 반환."""
    if not name or name == "base_link":
        return -1
    try:
        n = p.getNumJoints(robot_id)
        for i in range(n):
            info = p.getJointInfo(robot_id, i)
            link_name = info[12].decode('utf-8')
            if link_name == name:
                return i
    except Exception:
        pass
    return None

def _render_camera_view(robot_id, cam, link_id):
    """카메라 sensor 의 view 를 렌더해 RGB 배열 반환 (np.uint8, [h,w,3]).
    실패 시 None.

    우선순위:
      1. cam["world_pose"] 가 있으면 → 절대 world 좌표로 직접 렌더 ($scene 카메라, robot 무관)
      2. 아니면 robot link_state → 부착 카메라
    """
    try:
        import numpy as _np
        # 1) world camera: pose 가 절대좌표로 명시되어 robot 과 무관
        wp = cam.get("world_pose")
        if wp is not None:
            x, y, z, roll, pitch, yaw = wp
            pos = (x, y, z)
            quat = p.getQuaternionFromEuler([roll, pitch, yaw])
        else:
            # 2) 부착 카메라
            if link_id is None:
                return None
            if link_id < 0:
                pos, quat = p.getBasePositionAndOrientation(robot_id)
            else:
                ls = p.getLinkState(robot_id, link_id, computeForwardKinematics=True)
                pos = ls[0]; quat = ls[1]
        rot = _np.array(p.getMatrixFromQuaternion(quat)).reshape(3, 3)
        # 카메라 +X = 광축 (forward), +Z = up — OpenJigWare convention
        forward = rot.dot([1.0, 0.0, 0.0])
        up      = rot.dot([0.0, 0.0, 1.0])
        target  = [pos[0] + forward[0], pos[1] + forward[1], pos[2] + forward[2]]
        view_mat = p.computeViewMatrix(pos, target, up)
        fov_deg = cam["hfov_rad"] * 180.0 / math.pi
        aspect  = float(cam["w"]) / float(cam["h"])
        proj_mat = p.computeProjectionMatrixFOV(fov_deg, aspect, cam["near"], cam["far"])
        _, _, rgb, _, _ = p.getCameraImage(
            width=cam["w"], height=cam["h"],
            viewMatrix=view_mat, projectionMatrix=proj_mat,
            renderer=p.ER_BULLET_HARDWARE_OPENGL)
        arr = _np.array(rgb, dtype=_np.uint8)
        return arr[:, :, :3] if arr.ndim == 3 else None
    except Exception as ex:
        print(f"[Camera] render failed ({cam.get('name','')}): {ex}", flush=True)
        return None

def parse_link_visual_colors(urdf_path):
    """URDF 파일에서 각 링크의 모든 visual 색상 정보를 추출하여 반환

    Returns:
        dict: {link_name: [[r, g, b, a], ...]} 형태의 색상 정보 (visual별로 여러 색상 가능)
    """
    link_colors = {}

    if not os.path.exists(urdf_path):
        return link_colors

    try:
        tree = ET.parse(urdf_path)
        root = tree.getroot()

        # 모든 link 태그 검색
        for link in root.findall('.//link'):
            link_name = link.get('name')
            if not link_name:
                continue

            colors = []
            # 모든 visual 태그에서 색상 정보 추출
            for visual in link.findall('visual'):
                material = visual.find('material')
                if material is not None:
                    color = material.find('color')
                    if color is not None:
                        rgba_str = color.get('rgba', '0.7 0.7 0.7 1.0')
                        try:
                            rgba = [float(x) for x in rgba_str.split()]
                            if len(rgba) >= 4:
                                colors.append(rgba)
                        except:
                            pass

            if colors:
                link_colors[link_name] = colors

    except Exception as e:
        print(f"Note: Could not parse URDF for link colors: {e}")

    return link_colors

def apply_link_colors_from_urdf(robot_id, link_visual_colors):
    """URDF에서 파싱한 색상을 PyBullet 로봇에 적용

    Args:
        robot_id: PyBullet 로봇 ID
        link_visual_colors: {link_name: [[r, g, b, a], ...]} 형태의 색상 정보
    """
    if not link_visual_colors:
        return

    # PyBullet에서 현재 visual shape 데이터 가져오기
    visual_shapes = p.getVisualShapeData(robot_id)

    # 링크 인덱스별로 visual shape를 그룹화
    link_to_shapes = defaultdict(list)
    for shape in visual_shapes:
        link_idx = shape[1]  # linkIndex
        link_to_shapes[link_idx].append(shape)

    num_joints = p.getNumJoints(robot_id)

    # 링크 이름 -> 인덱스 매핑 생성
    link_name_to_idx = {'base_link': -1}
    for joint_idx in range(num_joints):
        joint_info = p.getJointInfo(robot_id, joint_idx)
        link_name = joint_info[12].decode('utf-8')  # linkName
        link_name_to_idx[link_name] = joint_idx

    applied_count = 0
    # 각 링크에 색상 적용
    for link_name, colors in link_visual_colors.items():
        if link_name not in link_name_to_idx:
            continue

        link_idx = link_name_to_idx[link_name]
        shapes_in_link = link_to_shapes.get(link_idx, [])

        # URDF의 visual 색상 순서대로 적용
        for i, color in enumerate(colors):
            if i < len(shapes_in_link):
                # 특정 visual shape에 색상 적용
                # PyBullet에서는 linkIndex만 지정 가능, 모든 shape가 같은 색으로 변경됨
                # 따라서 첫 번째 색상만 적용 (대부분의 경우 같은 색)
                pass

        # 링크에 첫 번째 visual의 색상 적용
        if colors:
            try:
                p.changeVisualShape(robot_id, link_idx, rgbaColor=colors[0])
                applied_count += 1
            except:
                pass

    print(f"Applied colors to {applied_count} links")

def parse_mimic_joints(urdf_path):
    """URDF 파일에서 mimic joint 정보를 추출하여 반환"""
    mimic_info = {}
    
    if not os.path.exists(urdf_path):
        print(f"Warning: URDF file not found: {urdf_path}")
        print("Using default robot model...")
        return mimic_info
    
    try:
        # XML 파일 파싱
        tree = ET.parse(urdf_path)
        root = tree.getroot()
        
        # 모든 joint 태그 검색
        for joint in root.findall('.//joint'):
            joint_name = joint.get('name')
            mimic = joint.find('mimic')
            if mimic is not None:
                # mimic 관계 정보 추출
                parent_joint = mimic.get('joint')
                multiplier = float(mimic.get('multiplier', '1.0'))
                offset = float(mimic.get('offset', '0.0'))
                
                mimic_info[joint_name] = {
                    'parent': parent_joint,
                    'multiplier': multiplier,
                    'offset': offset
                }
                
    except Exception as e:
        print(f"Note: Could not parse URDF for mimic joints: {e}")
    
    return mimic_info

def get_joint_position(robot_id, joint_id):
    """현재 조인트 위치를 라디안 단위로 반환"""
    try:
        joint_state = p.getJointState(robot_id, joint_id)
        return joint_state[0]  # position 반환
    except:
        return 0.0

def update_slider_for_joint(joint_id, angle_degrees):
    """특정 조인트에 해당하는 슬라이더 값을 업데이트"""
    try:
        joint_index = movable_joints.index(joint_id)
        if joint_index in sliders:
            slider_data = sliders[joint_index]
            # 슬라이더 타입에 따라 값 설정
            if slider_data['type'] == p.JOINT_REVOLUTE:
                # 회전 조인트는 도 단위로 슬라이더에 표시
                p.setUserDebugParameterValue(slider_data['id'], angle_degrees)
            else:
                # 직선 조인트는 미터 단위 (라디안으로 변환)
                p.setUserDebugParameterValue(slider_data['id'], math.radians(angle_degrees))
            
            # joint_info도 업데이트
            if joint_index in joint_info:
                joint_info[joint_index]['current_pos'] = math.radians(angle_degrees)
    except (ValueError, Exception):
        # joint_id가 movable_joints에 없거나 기타 오류 발생 시 무시
        pass

def update_mimic_joints_for_parent(parent_id, parent_pos):
    """특정 부모 조인트의 mimic 자식 조인트들을 업데이트"""
    for child_id, mimic_data in mimic_joints.items():
        if mimic_data['parent_id'] == parent_id:
            # mimic 공식: child = offset + multiplier * parent
            child_target = mimic_data['offset'] + mimic_data['multiplier'] * parent_pos
            
            # 조인트 한계값 확인
            child_info = p.getJointInfo(robot, child_id)
            lower_limit = child_info[8]
            upper_limit = child_info[9]
            
            # 한계값 적용
            if lower_limit > -1e10 and upper_limit < 1e10:
                child_target = max(lower_limit, min(upper_limit, child_target))
            
            # Mimic 자식 조인트 위치 설정
            p.setJointMotorControl2(
                robot,
                child_id,
                p.POSITION_CONTROL,
                targetPosition=child_target,
                force=1000
            )

def reset_all_joints():
    """모든 조인트를 초기 위치(0도)로 리셋하고 로봇 위치도 초기화"""
    # 활성 이동 명령 모두 취소
    active_moves.clear()
    
    # 모든 슬라이더를 0으로 리셋
    for joint_idx, slider_data in sliders.items():
        p.setUserDebugParameterValue(slider_data['id'], 0)
    
    # 로봇 위치 슬라이더도 초기값으로 리셋
    if robot_position_sliders:
        p.setUserDebugParameterValue(robot_position_sliders['x']['id'], initial_robot_position[0])
        p.setUserDebugParameterValue(robot_position_sliders['y']['id'], initial_robot_position[1])
        p.setUserDebugParameterValue(robot_position_sliders['z']['id'], initial_robot_position[2])
        
        # 로봇 위치 초기화
        p.resetBasePositionAndOrientation(robot, initial_robot_position, [0, 0, 0, 1])
        current_robot_position[:] = initial_robot_position[:]
    
    # 모든 조인트를 0 위치로 이동
    for idx in joint_info:
        joint_id = joint_info[idx]['id']
        p.setJointMotorControl2(
            robot,
            joint_id,
            p.POSITION_CONTROL,
            targetPosition=0,
            force=1000
        )
        joint_info[idx]['current_pos'] = 0
        
        # 리셋 시에도 mimic 관계 적용
        update_mimic_joints_for_parent(joint_id, 0)
    
    # Mimic 조인트들도 리셋
    for child_id, mimic_data in mimic_joints.items():
        child_target = mimic_data['offset']  # 부모가 0일 때의 자식 위치
        p.setJointMotorControl2(
            robot,
            child_id,
            p.POSITION_CONTROL,
            targetPosition=child_target,
            force=1000
        )

def update_robot_position():
    """로봇의 전체 위치를 슬라이더 값에 따라 업데이트"""
    if not robot_position_sliders:
        return
    
    try:
        # 각 축의 슬라이더 값 읽기
        new_x = p.readUserDebugParameter(robot_position_sliders['x']['id'])
        new_y = p.readUserDebugParameter(robot_position_sliders['y']['id'])
        new_z = p.readUserDebugParameter(robot_position_sliders['z']['id'])
        
        new_position = [new_x, new_y, new_z]
        
        # 위치가 변경되었을 때만 업데이트
        if any(abs(new_position[i] - current_robot_position[i]) > 0.001 for i in range(3)):
            # 로봇 위치 업데이트 (방향은 그대로 유지)
            current_orientation = p.getBasePositionAndOrientation(robot)[1]
            p.resetBasePositionAndOrientation(robot, new_position, current_orientation)
            current_robot_position[:] = new_position[:]
            
    except Exception:
        pass  # 슬라이더 읽기 실패 시 무시

def parse_socket_message(message):
    """소켓 메시지를 파싱하여 조인트 명령으로 변환 (가변형 조인트 수 지원)"""
    try:
        # 세미콜론 제거 후 쉼표로 분리
        values = message.strip().rstrip(';').split(',')
        
        if len(values) < 2:
            return None
        
        # 시간과 딜레이 파싱
        nTime = int(values[0])    # 이동 시간 (밀리초)
        nDelay = int(values[1])   # 딜레이 (현재 미사용)
        
        # 조인트 명령들 파싱
        joint_commands = []
        remaining_values = values[2:]
        
        # 짝수 개의 값이 있어야 함 (조인트ID, 각도 쌍)
        if len(remaining_values) % 2 != 0:
            return None
        
        # 조인트 ID와 각도를 쌍으로 파싱
        for i in range(0, len(remaining_values), 2):
            joint_id = int(remaining_values[i])
            joint_angle = float(remaining_values[i + 1])
            joint_commands.append({
                'joint_id': joint_id,
                'angle': joint_angle
            })
        
        return {
            'time': nTime,
            'delay': nDelay,
            'joint_commands': joint_commands
        }
        
    except (ValueError, IndexError):
        return None

def Play(command_string):
    """명령 문자열을 받아서 실행 큐에 추가하는 함수.
       SCENE_CLEAR / SCENE_ADD prefix 도 처리 (scene_queue 로 라우팅)."""
    try:
        cmd = command_string.strip().rstrip(';').strip()

        # === $scene 명령 라우팅 ===
        global active_scene_tool
        if cmd.startswith('SCENE_TOOL_CLEAR'):
            active_scene_tool = None
            print("[Scene] tool cleared (viewer click placement OFF)")
            return True
        if cmd.startswith('SCENE_TOOL_SET'):
            # SCENE_TOOL_SET, type, sx, sy, sz, color, mass, zoffset
            parts = [s.strip() for s in cmd.split(',')]
            if len(parts) >= 8:
                try:
                    active_scene_tool = {
                        'type':  parts[1],
                        'sx': float(parts[2]), 'sy': float(parts[3]), 'sz': float(parts[4]),
                        'color': int(parts[5], 0) if parts[5].lower().startswith('0x') else int(parts[5]),
                        'mass': float(parts[6]),
                        'zoffset': float(parts[7]),
                    }
                    print(f"[Scene] tool set: {active_scene_tool['type']} (viewer click → spawn)")
                    return True
                except Exception as ex:
                    print(f"SCENE_TOOL_SET parse error: {ex}")
                    return False
            return False
        if cmd.startswith('SCENE_CLEAR'):
            scene_queue.put(('CLEAR',))
            return True
        if cmd.startswith('SCENE_ADD'):
            # SCENE_ADD, type, x, y, z, rx, ry, rz, sx, sy, sz, color, mass
            parts = [s.strip() for s in cmd.split(',')]
            if len(parts) >= 13:
                try:
                    obj = {
                        'type':  parts[1],
                        'x': float(parts[2]), 'y': float(parts[3]), 'z': float(parts[4]),
                        'rx': float(parts[5]), 'ry': float(parts[6]), 'rz': float(parts[7]),
                        'sx': float(parts[8]), 'sy': float(parts[9]), 'sz': float(parts[10]),
                        'color': int(parts[11], 0) if parts[11].lower().startswith('0x') else int(parts[11]),
                        'mass': float(parts[12]),
                    }
                    scene_queue.put(('ADD', obj))
                    return True
                except Exception as ex:
                    print(f"SCENE_ADD parse error: {ex} / {cmd}")
                    return False
            return False

        # 기존 joint 명령 처리
        parsed = parse_socket_message(command_string)
        if parsed:
            command_queue.put(parsed)
            return True
        else:
            return False

    except Exception as e:
        print(f"Play exception: {e}")
        return False


def _spawn_scene_object(obj):
    """obj dict 에서 PyBullet body 하나 생성. body id 반환 (실패 시 None)."""
    try:
        t = obj['type']
        pos = [obj['x'], obj['y'], obj['z']]
        orn = p.getQuaternionFromEuler([
            math.radians(obj['rx']), math.radians(obj['ry']), math.radians(obj['rz'])
        ])
        sx, sy, sz = obj['sx'], obj['sy'], obj['sz']
        col_int = obj['color']
        mass = obj['mass']
        rgba = [
            ((col_int >> 16) & 0xFF) / 255.0,
            ((col_int >> 8) & 0xFF) / 255.0,
            (col_int & 0xFF) / 255.0,
            1.0
        ]

        if t in ('cube', 'box'):
            cs = p.createCollisionShape(p.GEOM_BOX, halfExtents=[sx/2, sy/2, sz/2])
            vs = p.createVisualShape(p.GEOM_BOX, halfExtents=[sx/2, sy/2, sz/2], rgbaColor=rgba)
            return p.createMultiBody(mass, cs, vs, pos, orn)

        if t == 'sphere':
            r = sx / 2 if sx > 0 else 0.04
            cs = p.createCollisionShape(p.GEOM_SPHERE, radius=r)
            vs = p.createVisualShape(p.GEOM_SPHERE, radius=r, rgbaColor=rgba)
            return p.createMultiBody(mass, cs, vs, pos, orn)

        if t == 'cylinder':
            r = sx / 2 if sx > 0 else 0.03
            h = sy if sy > 0 else 0.1
            cs = p.createCollisionShape(p.GEOM_CYLINDER, radius=r, height=h)
            vs = p.createVisualShape(p.GEOM_CYLINDER, radius=r, length=h, rgbaColor=rgba)
            return p.createMultiBody(mass, cs, vs, pos, orn)

        if t.startswith('asset:'):
            # PyBullet 내장 URDF 자산 매핑
            # PyBullet 내장 URDF 의 native 크기가 다 다르므로 scale 로 보정.
            #   tray.urdf  ≈ 600 × 500 × 40mm  → scale 0.3 → ≈ 180 × 150 × 12mm
            #   table.urdf ≈ 적당히 큰 책상      → scale 0.5
            asset_map = {
                'tray':  ('tray/tray.urdf',   0.3),
                'table': ('table/table.urdf', 0.5),
                'mug':   ('urdf/mug.urdf',    1.0),
            }
            name = t[6:]

            # 'can' 같은 PyBullet 내장이 없는 자산은 cylinder 로 우아하게 fallback.
            #   (Isaac Sim 에서는 YCB texture mesh 로 진짜 토마토수프 캔처럼 보이지만,
            #    PyBullet 은 동급 자산이 없어서 색상/사이즈만 일치하는 cylinder.
            #    PyBullet GEOM_CYLINDER 는 Z-axis default 라 자동으로 세워서 spawn.)
            if name == 'can':
                r = sx / 2 if sx > 0 else 0.025
                h = sy if sy > 0 else 0.080
                cs = p.createCollisionShape(p.GEOM_CYLINDER, radius=r, height=h)
                vs = p.createVisualShape(p.GEOM_CYLINDER, radius=r, length=h, rgbaColor=rgba)
                return p.createMultiBody(mass, cs, vs, pos, orn)

            mapping = asset_map.get(name)
            if mapping:
                urdf_path, default_scale = mapping
                try:
                    bid = p.loadURDF(urdf_path, pos, orn,
                                     useFixedBase=(mass <= 0),
                                     globalScaling=default_scale)
                    # 색상 override (visualShapeData 의 각 link)
                    try:
                        for vsi in p.getVisualShapeData(bid):
                            p.changeVisualShape(bid, vsi[1], rgbaColor=rgba)
                    except Exception:
                        pass
                    return bid
                except Exception as ex:
                    print(f"loadURDF {urdf_path} failed: {ex}, falling back to box")
            # fallback: box 로 근사
            cs = p.createCollisionShape(p.GEOM_BOX, halfExtents=[sx/2, sy/2, sz/2])
            vs = p.createVisualShape(p.GEOM_BOX, halfExtents=[sx/2, sy/2, sz/2], rgbaColor=rgba)
            return p.createMultiBody(mass, cs, vs, pos, orn)

        if t.endswith('.urdf'):
            try:
                return p.loadURDF(t, pos, orn, useFixedBase=(mass <= 0), globalScaling=max(sx, 0.01))
            except Exception as ex:
                print(f"loadURDF {t} failed: {ex}")
                return None

        print(f"Unknown scene type: {t}")
        return None
    except Exception as ex:
        print(f"_spawn_scene_object error: {ex}")
        return None


def process_viewer_click_placement():
    """PyBullet viewer 의 좌클릭을 받아 active_scene_tool 객체를 Z=0 평면에 spawn.
       active_scene_tool 이 None 이면 작동 안 함 (보통 PyBullet 카메라 조작 정상).
       이미 spawn 된 객체 위에 클릭하면 그 위에 쌓이도록 rayTest 결과의 hit_position 사용.
       없으면 (= ray 가 free space) Z=0 floor 와 교차."""
    global active_scene_tool, scene_spawned_ids
    if active_scene_tool is None:
        return
    try:
        events = p.getMouseEvents()
        if not events:
            return
        cam = p.getDebugVisualizerCamera()
        if cam is None or len(cam) < 12:
            return
        width  = cam[0]
        height = cam[1]
        view_m = cam[2]   # 16-elem flat (column-major)
        proj_m = cam[3]

        import numpy as _np
        view_np = _np.array(view_m, dtype=_np.float64).reshape(4, 4, order='F')
        proj_np = _np.array(proj_m, dtype=_np.float64).reshape(4, 4, order='F')
        try:
            inv_vp = _np.linalg.inv(proj_np @ view_np)
        except _np.linalg.LinAlgError:
            return

        for e in events:
            # event_type=2 (mouse button), button=0 (left), state has KEY_WAS_TRIGGERED bit.
            if len(e) < 5: continue
            if e[0] != 2: continue
            if e[3] != 0: continue
            if not (e[4] & p.KEY_WAS_TRIGGERED): continue

            mx = float(e[1]); my = float(e[2])
            ndc_x = (2.0 * mx / float(width)) - 1.0
            ndc_y = 1.0 - (2.0 * my / float(height))

            near_clip = _np.array([ndc_x, ndc_y, -1.0, 1.0])
            far_clip  = _np.array([ndc_x, ndc_y,  1.0, 1.0])
            near_world = inv_vp @ near_clip; near_world = near_world[:3] / near_world[3]
            far_world  = inv_vp @ far_clip;  far_world  = far_world[:3]  / far_world[3]

            # 1) 우선 기존 객체와 ray test → 그 위에 쌓을 수 있도록 hit_position 사용.
            hit_pos = None
            try:
                rt = p.rayTest(near_world.tolist(), far_world.tolist())
                if rt and rt[0][0] >= 0:
                    hit_pos = list(rt[0][3])
            except Exception:
                pass

            # 2) ray 가 아무것도 안 맞췄으면 Z=0 평면 교차.
            if hit_pos is None:
                dz = far_world[2] - near_world[2]
                if abs(dz) < 1e-9:
                    print("[Scene] viewer click: ray parallel to floor — skip")
                    continue
                t = -near_world[2] / dz
                if t < 0:
                    continue
                hit_pos = (near_world + t * (far_world - near_world)).tolist()

            # 3) active tool 정의로 객체 obj 생성 후 spawn.
            tool = active_scene_tool
            obj = {
                'type':  tool['type'],
                'x': hit_pos[0],
                'y': hit_pos[1],
                'z': hit_pos[2] + tool['zoffset'],
                'rx': 0.0, 'ry': 0.0, 'rz': 0.0,
                'sx': tool['sx'], 'sy': tool['sy'], 'sz': tool['sz'],
                'color': tool['color'],
                'mass': tool['mass'],
            }
            bid = _spawn_scene_object(obj)
            if bid is not None:
                scene_spawned_ids.append(bid)
                print(f"[Scene] viewer-click spawn {tool['type']} @ ({obj['x']:.3f},{obj['y']:.3f},{obj['z']:.3f})")
    except Exception as ex:
        print(f"process_viewer_click_placement error: {ex}")


def process_scene_commands():
    """scene_queue 에서 SCENE_CLEAR / SCENE_ADD 명령 꺼내 처리. 매 frame main 루프에서 호출."""
    global scene_spawned_ids
    try:
        while not scene_queue.empty():
            try:
                cmd = scene_queue.get_nowait()
            except queue.Empty:
                break
            if cmd[0] == 'CLEAR':
                for bid in scene_spawned_ids:
                    try: p.removeBody(bid)
                    except Exception: pass
                scene_spawned_ids = []
                print("[Scene] cleared")
            elif cmd[0] == 'ADD':
                obj = cmd[1]
                bid = _spawn_scene_object(obj)
                if bid is not None:
                    scene_spawned_ids.append(bid)
                    print(f"[Scene] +{obj['type']} @ ({obj['x']:.3f},{obj['y']:.3f},{obj['z']:.3f}) mass={obj['mass']}")
    except Exception as e:
        print(f"process_scene_commands error: {e}")

def WaitMotion(milliseconds=None):
    """
    지정된 시간만큼 대기하거나, 모든 움직임이 완료될 때까지 대기
    
    Args:
        milliseconds: 대기 시간(밀리초). None이면 모든 움직임 완료까지 대기
    """
    if milliseconds is None:
        # 모든 움직임 완료까지 대기
        timeout = 10.0  # 최대 10초 대기
        start_time = time.time()
        
        time.sleep(0.1)  # 명령이 처리될 시간 확보
        
        # 활성 이동이 모두 완료될 때까지 대기
        while len(active_moves) > 0 and (time.time() - start_time) < timeout:
            time.sleep(0.1)
    else:
        # 지정된 시간만큼 대기
        time.sleep(milliseconds / 1000.0)
        
def move_joint_to_angle(robot_id, joint_id, target_degrees, duration_ms, movable_joints):
    """조인트 명령 처리.
       - Revolute/Prismatic: target_degrees 를 목표 각도/거리로 해석, 부드럽게 보간.
       - Continuous (바퀴/트랙): target_degrees 를 RPS (rotations per second) 로 해석, 즉시 속도 적용.
    """

    # 유효한 조인트 인덱스인지 확인
    if joint_id >= len(movable_joints):
        print(f"Invalid joint index: {joint_id} (max: {len(movable_joints)-1})")
        return False

    actual_joint_id = movable_joints[joint_id]

    # 조인트 타입 판별 (continuous = revolute + 한계값 없음)
    joint_info = p.getJointInfo(robot_id, actual_joint_id)
    joint_type = joint_info[2]
    lower_limit = joint_info[8]
    upper_limit = joint_info[9]
    is_continuous = (joint_type == p.JOINT_REVOLUTE and lower_limit >= upper_limit)

    if is_continuous:
        # Continuous: target_degrees 를 RPS 로 해석 (사용자 요청)
        # target_velocity_rad_s = RPS × 2π
        target_vel_rad_s = target_degrees * 2.0 * math.pi
        p.setJointMotorControl2(
            robot_id,
            actual_joint_id,
            p.VELOCITY_CONTROL,
            targetVelocity=target_vel_rad_s,
            force=2000
        )
        # 슬라이더 UI 값도 동기화 (RPS 단위로 표시)
        try:
            if actual_joint_id in movable_joints:
                jidx = movable_joints.index(actual_joint_id)
                if jidx in sliders:
                    # RPS 값을 슬라이더 범위 [-5, +5] 에 클램프
                    rps_clamped = max(-5.0, min(5.0, target_degrees))
                    p.setUserDebugParameterValue(sliders[jidx]['id'], rps_clamped)
                    if jidx in joint_info:
                        joint_info[jidx]['current_pos'] = target_vel_rad_s
        except Exception:
            pass
        return True

    # 현재 위치 가져오기 (라디안 또는 m)
    current_pos_native = get_joint_position(robot_id, actual_joint_id)

    # 단위 분기 — URDF joint type 에 따라 다르게 변환.
    #   revolute: target_degrees 가 deg → rad 변환
    #   prismatic: target_degrees 가 mm → m 변환 (MakeUrdf 내부 단위 = mm)
    is_prismatic = (joint_type == p.JOINT_PRISMATIC)
    if is_prismatic:
        target_native = target_degrees * 0.001    # mm → m
        current_pos_display = current_pos_native * 1000.0   # m → mm (UI/시작값 표시)
    else:
        target_native = math.radians(target_degrees)   # deg → rad
        current_pos_display = math.degrees(current_pos_native)  # rad → deg

    # 조인트 한계값 확인 및 적용 (네이티브 단위로 클램프 — Limit 도 native)
    if lower_limit > -1e10 and upper_limit < 1e10:
        target_native = max(lower_limit, min(upper_limit, target_native))
        if is_prismatic:
            target_degrees = target_native * 1000.0
        else:
            target_degrees = math.degrees(target_native)

    # 이동 명령 객체 생성
    #   target_angle / start_angle 필드 이름은 그대로 두지만 단위는 display 단위
    #   (revolute: deg, prismatic: mm) — update_joint_movements 가 사용.
    move_command = JointMoveCommand(
        joint_id=actual_joint_id,
        target_angle=target_degrees,
        duration=duration_ms / 1000.0,  # 밀리초를 초로 변환
        start_time=time.time(),
        start_angle=current_pos_display
    )
    
    # 활성 이동 목록에 추가 (기존 이동이 있으면 덮어쓰기)
    active_moves[actual_joint_id] = move_command
    
    return True

def update_joint_movements(robot_id):
    """활성 조인트 이동 명령들을 업데이트 (부드러운 보간 이동)"""
    if len(active_moves) == 0: # 움직일 조인트가 없으면 조기 반환
        return  
    
    current_time = time.time()
    completed_moves = []
    
    # 각 활성 이동 명령 처리
    for joint_id, move_cmd in active_moves.items():
        elapsed = current_time - move_cmd.start_time
        progress = min(elapsed / move_cmd.duration, 1.0)  # 0~1 사이로 제한
        
        if progress >= 1.0:
            # 이동 완료
            current_angle = move_cmd.target_angle
            completed_moves.append(joint_id)
        else:
            # 선형 보간으로 중간 위치 계산
            current_angle = move_cmd.start_angle + (move_cmd.target_angle - move_cmd.start_angle) * progress
        
        # 단위 분기 — joint type 에 따라 display 단위(current_angle) 를 native 로 변환
        #   revolute: deg → rad
        #   prismatic: mm → m
        try:
            jt = p.getJointInfo(robot_id, joint_id)[2]
        except Exception:
            jt = p.JOINT_REVOLUTE   # 안전한 default
        if jt == p.JOINT_PRISMATIC:
            target_native = current_angle * 0.001    # mm → m
        else:
            target_native = math.radians(current_angle)   # deg → rad

        p.setJointMotorControl2(
            robot_id,
            joint_id,
            p.POSITION_CONTROL,
            targetPosition=target_native,
            force=1000
        )

        # Mimic 조인트 업데이트 (부모 조인트가 움직일 때 자식들도 함께 움직임)
        update_mimic_joints_for_parent(joint_id, target_native)

        # 해당 슬라이더 값 업데이트 (반복 작업이므로 print 제거)
        update_slider_for_joint(joint_id, current_angle)
    
    # 완료된 이동 제거
    for joint_id in completed_moves:
        del active_moves[joint_id]

def socket_server_thread():
    """소켓 서버를 실행하는 스레드 함수"""
    HOST = '127.0.0.1'  # 로컬호스트
    PORT = 4000         # 포트 번호
    
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    
    try:
        server_socket.bind((HOST, PORT))
        server_socket.listen(1)
        print(f"Socket server listening on {HOST}:{PORT}")
        
        while simulation_running:
            try:
                server_socket.settimeout(1.0)
                client_socket, client_address = server_socket.accept()
                print(f"Client connected: {client_address}")
                
                try:
                    buffer = ""
                    while simulation_running:
                        try:
                            chunk = client_socket.recv(1024).decode('utf-8')
                            if not chunk:
                                break
                            
                            buffer += chunk
                            
                            # 완전한 메시지들 처리 (세미콜론으로 구분)
                            while ';' in buffer:
                                message, buffer = buffer.split(';', 1)
                                message = message.strip()
                                
                                if message:
                                    # 명령 실행 (반복 작업이므로 print 제거)
                                    success = Play(message + ';')
                                    response = "OK" if success else "ERROR"
                                    
                                    try:
                                        if (response != "OK") :
                                            client_socket.send(response.encode('utf-8'))
                                    except:
                                        break
                            
                        except socket.timeout:
                            continue
                        except Exception:
                            break
                        
                except Exception as e:
                    print(f"Client error: {e}")
                finally:
                    client_socket.close()
                    print("Client disconnected")
                    
            except socket.timeout:
                continue
            except Exception as e:
                if simulation_running:
                    print(f"Socket server error: {e}")
                break
                
    except Exception as e:
        print(f"Socket server startup error: {e}")
    finally:
        server_socket.close()
        print("Socket server closed")

def process_socket_commands(robot_id, movable_joints):
    """소켓 명령 큐에서 명령을 가져와 처리"""
    global joint_number_to_id
    try:
        while not command_queue.empty():
            command = command_queue.get_nowait()

            # 각 조인트 명령을 처리
            for joint_cmd in command['joint_commands']:
                joint_number = joint_cmd['joint_id']  # 조인트 번호 (예: 11, 12)
                joint_angle = joint_cmd['angle']

                # 조인트 번호로 PyBullet joint ID 찾기
                if joint_number in joint_number_to_id:
                    pybullet_joint_id = joint_number_to_id[joint_number]

                    # movable_joints에서 인덱스 찾기
                    if pybullet_joint_id in movable_joints:
                        joint_index = movable_joints.index(pybullet_joint_id)
                        move_joint_to_angle(robot_id, joint_index, joint_angle, command['time'], movable_joints)
                    else:
                        print(f"Joint {joint_number} (ID:{pybullet_joint_id}) is not movable")
                else:
                    print(f"Invalid joint number: {joint_number} (available: {sorted(joint_number_to_id.keys())})")

    except queue.Empty:
        pass
    except Exception as e:
        print(f"Exception in process_socket_commands: {e}")

####################################################
# 트랙 벨트 flow 애니메이션 (robot_stl_tracks.json 기반)
####################################################

# 트랙 flow 애니메이션 활성화 여부. True 면:
#   - URDF 의 정적 트랙 세그먼트 visual 제거
#   - JSON 메타데이터로 동적 세그먼트 body 생성
#   - 매 프레임 drive 모터 각도 따라 세그먼트를 belt path 재배치
# False 면: URDF 의 정적 세그먼트만 표시 (트랙 자체 애니메이션 없음)
ENABLE_TRACK_FLOW = True


def preprocess_urdf_for_track_flow(urdf_path_in, tracks_metas):
    """트랙 flow 활성화 시: URDF 의 정적 트랙 visual + collision 전부 제거.
    → 지면 접촉은 별도 휠 실린더(continuous joint 자식)가 담당. 휠 마찰은 run-time 에서 튜닝.
    """
    if not tracks_metas:
        return urdf_path_in
    track_stls = set()
    for entry in tracks_metas:
        stl = entry.get("segment_stl", "")
        if stl:
            track_stls.add(stl.replace("\\", "/"))
    if not track_stls:
        return urdf_path_in
    try:
        tree = ET.parse(urdf_path_in)
    except Exception as e:
        print(f"[Track] URDF 파싱 실패: {e}")
        return urdf_path_in
    root = tree.getroot()
    removed = 0
    for link in root.findall('link'):
        to_remove = []
        for elem in list(link):
            if elem.tag not in ('visual', 'collision'):
                continue
            mesh = elem.find('.//mesh')
            if mesh is None:
                continue
            fn = (mesh.get('filename') or '').replace('\\', '/')
            if fn in track_stls:
                to_remove.append(elem)
        for elem in to_remove:
            link.remove(elem)
            removed += 1
    if removed == 0:
        return urdf_path_in
    temp_path = urdf_path_in + ".trackanim.urdf"
    try:
        tree.write(temp_path, xml_declaration=True, encoding='utf-8')
        print(f"[Track] 정적 세그먼트 visual+collision {removed}개 제거 → {os.path.basename(temp_path)}")
        return temp_path
    except Exception as e:
        print(f"[Track] URDF 임시 저장 실패: {e}")
        return urdf_path_in


def boost_track_wheel_friction(robot_id, tracks):
    """트랙 구동 휠 + 지면의 마찰 계수 튜닝 — 탱크 추진력 확보용.
    기본 PyBullet friction 낮아서 바퀴가 헛도는 문제 방지.
    """
    for track in tracks:
        jname = track["motor_joint"]
        try:
            if jname not in joint_name_to_id:
                continue
            jid = joint_name_to_id[jname]
            # joint index == child link index (PyBullet convention)
            p.changeDynamics(robot_id, jid,
                             lateralFriction=2.5,
                             rollingFriction=0.001,
                             spinningFriction=0.001,
                             restitution=0.0)
        except Exception as e:
            print(f"[Track] friction 튜닝 실패 ({jname}): {e}")
    # mimic wheels 도 동일하게
    for name in list(mimic_joints.keys() if 'mimic_joints' in globals() else []):
        pass  # mimic_joints 는 dict[int,dict] 형태. 후처리로 set


def boost_mimic_wheels_friction(robot_id):
    """트랙 구동 휠만 지면과 접촉 + 마찰 강화. 다른 링크(chassis, 팔)는 collision 비활성화.
    → chassis STL 이 지면에 끼어서 휠이 헛도는 문제 방지.
    """
    try:
        num_j = p.getNumJoints(robot_id)
        # ── 가드: 실제 'track_T' joint 가 존재할 때만 적용 ──
        # 이전 캡쳐의 stale tracks.json 가 남아있으면 비-트랙 로봇에도
        # 호출되어 모든 link collision 이 꺼짐 → 로봇이 지면 통과.
        has_track = False
        for i in range(num_j):
            name = p.getJointInfo(robot_id, i)[1].decode('utf-8')
            if 'track_T' in name:
                has_track = True
                break
        if not has_track:
            print("[Track] no 'track_T' joint — skip friction tuning (stale tracks.json guard)", flush=True)
            return
        wheel_count = 0
        disabled_count = 0
        # base_link (-1) collision 비활성화
        p.setCollisionFilterGroupMask(robot_id, -1, 0, 0)
        disabled_count += 1
        for i in range(num_j):
            info = p.getJointInfo(robot_id, i)
            name = info[1].decode('utf-8')
            if 'track_T' in name:
                # 트랙 휠: collision 활성 + 매우 강한 마찰 (탱크용)
                # linearDamping/angularDamping 은 0 으로 (휠 회전 자체를 감쇠하면 안 됨)
                p.setCollisionFilterGroupMask(robot_id, i, 1, 0xFFFFFFFF)
                p.changeDynamics(robot_id, i,
                                 lateralFriction=50.0,    # 측면 마찰 초강화 (회전 시 미끄러짐 방지)
                                 rollingFriction=0.0001,  # 굴러가는 저항 최소
                                 spinningFriction=5.0,    # 회전 피벗 미끄러짐 강력 억제
                                 restitution=0.0,
                                 contactStiffness=100000,
                                 contactDamping=5000,
                                 linearDamping=0.0,
                                 angularDamping=0.0)
                wheel_count += 1
            else:
                # 그 외 링크(chassis body, arm): collision 비활성화
                p.setCollisionFilterGroupMask(robot_id, i, 0, 0)
                disabled_count += 1
        print(f"[Track] 휠 {wheel_count}개 마찰 튜닝, 기타 링크 {disabled_count}개 collision 비활성화", flush=True)
        # 지면 마찰도 매우 강하게
        if 'plane_id' in globals() and plane_id is not None:
            p.changeDynamics(plane_id, -1, lateralFriction=20.0, spinningFriction=5.0)
    except Exception as e:
        print(f"[Track] friction 튜닝 실패: {e}", flush=True)


def _preview_tracks_meta(release_dir_local):
    """URDF 로드 전에 track JSON 만 훑어봄 (preprocess 용)."""
    jp = os.path.join(release_dir_local, "robot_stl_tracks.json")
    if not os.path.exists(jp):
        return []
    try:
        # utf-8-sig: C# Encoding.UTF8 이 쓰는 BOM 자동 제거
        with open(jp, 'r', encoding='utf-8-sig') as f:
            return _json.load(f).get("tracks", [])
    except Exception as e:
        print(f"[Track] _preview_tracks_meta 실패: {e}", flush=True)
        return []

def _belt_perimeter(pulleys):
    """2D 풀리 리스트 [[x,y,r], ...] 로부터 벨트 전체 둘레 길이 계산 (mm)."""
    n_cnt = len(pulleys)
    total = 0.0
    for n in range(n_cnt):
        p0 = pulleys[n]
        p1 = pulleys[(n + 1) % n_cnt]
        p2 = pulleys[(n + 2) % n_cnt]
        dx = p1[0] - p0[0]; dy = p1[1] - p0[1]
        length = math.sqrt(dx * dx + dy * dy)
        if length < 1e-6: continue
        dr = p1[2] - p0[2]
        length_out = math.sqrt(max(length * length - dr * dr, 0.0))
        angle_deg = math.degrees(math.atan2(dy, dx))
        dx2 = p2[0] - p1[0]; dy2 = p2[1] - p1[1]
        angle2_deg = math.degrees(math.atan2(dy2, dx2))
        tmp_deg = ((angle_deg - angle2_deg) % 360.0 + 360.0) % 360.0
        R1 = p1[2] if p1[2] > 0 else 1.0
        arc_len = (tmp_deg / 360.0) * 2.0 * math.pi * R1
        total += length_out + arc_len
    return total

def _arc_pos_to_xy_rot(s, pulleys):
    """벨트 아크 길이 s (mm) → 2D 평면 (x_mm, y_mm, rot_deg) 변환."""
    n_cnt = len(pulleys)
    for n in range(n_cnt):
        p0 = pulleys[n]
        p1 = pulleys[(n + 1) % n_cnt]
        p2 = pulleys[(n + 2) % n_cnt]
        dx = p1[0] - p0[0]; dy = p1[1] - p0[1]
        length = math.sqrt(dx * dx + dy * dy)
        if length < 1e-6: continue
        dr = p1[2] - p0[2]
        length_out = math.sqrt(max(length * length - dr * dr, 0.0))
        angle_rad = math.atan2(dy, dx)
        angle_deg = math.degrees(angle_rad)
        angle_gap_rad = math.atan2(dr, length)
        angle_gap_deg = math.degrees(angle_gap_rad)
        # Phase 1 tangent
        if s < length_out:
            cos_a = math.cos(angle_rad); sin_a = math.sin(angle_rad)
            cos_g = math.cos(angle_gap_rad); sin_g = math.sin(angle_gap_rad)
            lx1 = s * cos_g
            ly1 = p0[2] + s * sin_g
            seg_x = p0[0] + lx1 * cos_a - ly1 * sin_a
            seg_y = p0[1] + lx1 * sin_a + ly1 * cos_a
            return (seg_x, seg_y, angle_deg + angle_gap_deg)
        s -= length_out
        # Phase 2 arc around p1
        dx2 = p2[0] - p1[0]; dy2 = p2[1] - p1[1]
        angle2_deg = math.degrees(math.atan2(dy2, dx2))
        tmp_deg = ((angle_deg - angle2_deg) % 360.0 + 360.0) % 360.0
        R1 = p1[2] if p1[2] > 0 else 1.0
        arc_len = (tmp_deg / 360.0) * 2.0 * math.pi * R1
        if s < arc_len:
            angle_diff_deg = s * 360.0 / (2.0 * math.pi * R1)
            rot_deg = angle_deg + 90 - angle_diff_deg
            rot_rad = math.radians(rot_deg)
            seg_x = p1[0] + R1 * math.cos(rot_rad)
            seg_y = p1[1] + R1 * math.sin(rot_rad)
            return (seg_x, seg_y, rot_deg - 90)
        s -= arc_len
    return None

def load_track_belts(release_dir_local):
    """robot_stl_tracks.json 로드 + 각 트랙의 세그먼트 body 생성. 없으면 빈 리스트."""
    json_path = os.path.join(release_dir_local, "robot_stl_tracks.json")
    print(f"[Track] JSON 검색: {json_path}")
    if not os.path.exists(json_path):
        print(f"[Track] JSON 파일 없음 → 트랙 애니메이션 비활성화")
        return []
    try:
        with open(json_path, 'r', encoding='utf-8-sig') as f:
            data = _json.load(f)
    except Exception as e:
        print(f"[Track] JSON 로드 실패: {e}")
        return []

    tracks = []
    for entry in data.get("tracks", []):
        pulleys = entry.get("pulleys_2d_mm", [])
        dist = entry.get("segment_distance_mm", 10.0)
        if len(pulleys) < 2 or dist < 1e-3:
            print(f"[Track] 스킵: pulleys={len(pulleys)}, dist={dist}")
            continue
        perim = _belt_perimeter(pulleys)
        if perim < 1e-3:
            print(f"[Track] 스킵: perimeter={perim}")
            continue
        n_seg = max(1, int(round(perim / dist)))
        seg_dist_adj = perim / n_seg  # 세그먼트 N개가 벨트를 정확히 덮도록

        color = entry.get("color_rgb", [0.7, 0.7, 0.7])
        rgba = [color[0], color[1], color[2], 1.0]
        scale = entry.get("stl_scale", 0.001)
        stl = entry.get("segment_stl", "")

        # STL 파일 존재 확인
        stl_abs = stl if os.path.isabs(stl) else os.path.join(release_dir_local, stl)
        if not os.path.exists(stl_abs):
            # cwd 기준 시도
            stl_cwd = os.path.join(os.getcwd(), stl)
            if not os.path.exists(stl_cwd):
                print(f"[Track] STL 파일 없음: {stl} (cwd={os.getcwd()}, release={release_dir_local})")
                continue
            stl_abs = stl_cwd
        print(f"[Track] STL 로드: {stl_abs}")

        try:
            vshape = p.createVisualShape(
                shapeType=p.GEOM_MESH,
                fileName=stl_abs,
                meshScale=[scale, scale, scale],
                rgbaColor=rgba
            )
            if vshape < 0:
                print(f"[Track] visual shape 생성 실패 (return={vshape}, stl={stl_abs})")
                continue
        except Exception as e:
            print(f"[Track] visual shape 예외 ({stl_abs}): {e}")
            continue

        body_ids = []
        for i in range(n_seg):
            try:
                body = p.createMultiBody(
                    baseMass=0,
                    baseCollisionShapeIndex=-1,
                    baseVisualShapeIndex=vshape,
                    basePosition=[0, 0, -10]  # 초기 위치: 일단 바닥 아래로 숨겨둠
                )
                body_ids.append(body)
            except Exception as e:
                print(f"[Track] body 생성 실패: {e}")
                break

        tracks.append({
            "motor_joint": entry.get("motor_joint", ""),
            "pulleys": pulleys,
            "seg_dist": seg_dist_adj,
            "perim": perim,
            "track_frame_pos": entry.get("track_frame_pos_m", [0, 0, 0]),
            "track_frame_rpy_deg": entry.get("track_frame_rpy_deg", [0, 0, 0]),
            "body_ids": body_ids,
            "n_seg": n_seg,
        })
        print(f"[Track] {entry.get('motor_joint','?')}: {n_seg} segs (perim={perim:.1f}mm, dist={seg_dist_adj:.2f}mm)")
    return tracks

def update_track_belts(tracks, robot_id, joint_name_to_id_local):
    """매 프레임: 모터 각도 읽어 세그먼트들을 belt path 따라 재배치."""
    if not tracks: return
    try:
        base_pos, base_orn = p.getBasePositionAndOrientation(robot_id)
    except:
        return
    for track in tracks:
        jname = track["motor_joint"]
        if jname not in joint_name_to_id_local:
            continue
        jid = joint_name_to_id_local[jname]
        try:
            motor_angle_rad = p.getJointState(robot_id, jid)[0]
        except:
            continue

        R_mm = track["pulleys"][0][2] if track["pulleys"][0][2] > 0 else 30.0
        # 휠 회전 방향과 벨트 flow 방향 일치시키기 위해 음수 (벨트 바닥이 탱크 뒤로 흐르면 탱크 전진)
        belt_offset_mm = -motor_angle_rad * R_mm

        tf_local_pos = track["track_frame_pos"]
        tf_local_orn = p.getQuaternionFromEuler([
            math.radians(track["track_frame_rpy_deg"][0]),
            math.radians(track["track_frame_rpy_deg"][1]),
            math.radians(track["track_frame_rpy_deg"][2]),
        ])
        track_world_pos, track_world_orn = p.multiplyTransforms(
            base_pos, base_orn, tf_local_pos, tf_local_orn
        )

        pulleys = track["pulleys"]
        n_seg = track["n_seg"]
        seg_dist = track["seg_dist"]
        perim = track["perim"]
        body_ids = track["body_ids"]

        for i in range(n_seg):
            s = (i * seg_dist + belt_offset_mm) % perim
            r = _arc_pos_to_xy_rot(s, pulleys)
            if r is None: continue
            seg_x, seg_y, seg_rot_deg = r
            seg_local_pos = [seg_x * 0.001, seg_y * 0.001, 0.0]
            seg_local_orn = p.getQuaternionFromEuler([0, 0, math.radians(seg_rot_deg)])
            sw_pos, sw_orn = p.multiplyTransforms(
                track_world_pos, track_world_orn, seg_local_pos, seg_local_orn
            )
            if i < len(body_ids):
                p.resetBasePositionAndOrientation(body_ids[i], sw_pos, sw_orn)


####################################################
# 메인 실행 부분
####################################################

# Mimic 정보 파싱
mimic_relationships = parse_mimic_joints(urdf_file)

# URDF에서 링크별 색상 정보 파싱
link_colors = parse_link_visual_colors(urdf_file)
print(f"Parsed {len(link_colors)} link colors from URDF")

# 트랙 flow 애니메이션 활성화 시: URDF 에서 정적 트랙 세그먼트 제거한 임시 URDF 로드
urdf_to_load = urdf_file
print(f"[Track] ENABLE_TRACK_FLOW={ENABLE_TRACK_FLOW}, release_dir={release_dir}", flush=True)
if ENABLE_TRACK_FLOW:
    _preview_metas = _preview_tracks_meta(release_dir)
    print(f"[Track] JSON preview: {len(_preview_metas)} tracks", flush=True)
    if _preview_metas:
        urdf_to_load = preprocess_urdf_for_track_flow(urdf_file, _preview_metas)
        print(f"[Track] URDF to load: {urdf_to_load}", flush=True)

# 로봇 URDF 로드 — fixed/non-fixed 모두 동일하게 [0, 0, SPAWN_Z]
robot = p.loadURDF(urdf_to_load, [0, 0, SPAWN_Z], useFixedBase=USE_FIXED_BASE)

# 조인트 정보 수집을 위한 변수들 초기화
num_joints = p.getNumJoints(robot)

# ── 관절 강제 유지: 모든 movable joint 에 POSITION_CONTROL + 큰 force ──
#    중력으로 인한 처짐/꺾임 방지. 사용자가 슬라이더로 조작하면 그 값으로 갱신됨.
HOLD_FORCE = 5000.0
try:
    for _ji in range(num_joints):
        _ji_info = p.getJointInfo(robot, _ji)
        _jt = _ji_info[2]   # jointType: 0=revolute, 1=prismatic, 4=fixed 등
        if _jt == p.JOINT_FIXED:
            continue
        _cur = p.getJointState(robot, _ji)[0]
        _ctrl_mode = p.POSITION_CONTROL if _jt != p.JOINT_PRISMATIC else p.POSITION_CONTROL
        p.setJointMotorControl2(robot, _ji, _ctrl_mode,
                                targetPosition=_cur, force=HOLD_FORCE)
    print(f"[OJW] hold force {HOLD_FORCE} applied to all movable joints", flush=True)
except Exception as _ex:
    print(f"[OJW] joint hold setup failed: {_ex}", flush=True)

##########################################
# 카메라 sensor 셋업
#   - 부착 카메라: URDF <sensor> elements → robot link 따라감
#   - world 카메라: world_cameras.json → 절대좌표 고정, 별도 마커 spawn
##########################################
camera_list = _parse_cameras_from_urdf(urdf_to_load)

# world_cameras.json 도 같이 로드 (있으면) — robot URDF 와 같은 폴더
_world_cams_path = os.path.join(os.path.dirname(urdf_to_load), "world_cameras.json")
_world_cams = _parse_world_cameras_json(_world_cams_path)
for _wc in _world_cams:
    _spawn_world_camera_marker(_wc)   # 시각 마커 (작은 빨간 박스)
camera_list.extend(_world_cams)        # 같은 렌더 루프에서 처리

camera_link_ids = []  # 같은 인덱스에 link_id 저장 (world camera 는 None)
for _cam in camera_list:
    if _cam.get("world_pose") is not None:
        camera_link_ids.append(None)   # world camera 는 link 추적 안 함
        print(f"[Camera] '{_cam['name']}' (world) {_cam['w']}x{_cam['h']} "
              f"fov={_cam['hfov_rad']*180/math.pi:.1f}deg", flush=True)
    else:
        _lid = _find_link_id_by_name(robot, _cam["link"])
        camera_link_ids.append(_lid)
        print(f"[Camera] '{_cam['name']}' parent='{_cam['link']}' "
              f"link_id={_lid} {_cam['w']}x{_cam['h']} "
              f"fov={_cam['hfov_rad']*180/math.pi:.1f}deg", flush=True)

# cv2 import 시도 — 없으면 카메라 뷰 표시 skip (parse 는 유지)
try:
    import cv2 as _cv2
    _cv2_ok = True
    print(f"[Camera] cv2 OK — {len(camera_list)} 카메라 뷰 활성화", flush=True)
except Exception as _ex:
    _cv2_ok = False
    print(f"[Camera] cv2 없음 — 카메라 뷰 비활성. "
          f"`pip install opencv-python` 으로 설치 가능 ({_ex})", flush=True)

# 카메라 update_rate 제한용 (sec 단위 마지막 갱신 시각)
_camera_last_render = [0.0] * len(camera_list)

joint_info = {}
movable_joints = []
joint_name_to_id = {}
mimic_joints = {}
sliders = {}

print(f"Robot loaded successfully!")
print(f"Total joints: {num_joints}")

# URDF에서 파싱한 색상 적용
if link_colors:
    print("Applying link colors from URDF...")
    apply_link_colors_from_urdf(robot, link_colors)

# 로봇 위치 제어 슬라이더 생성
print("\nCreating robot position control sliders...")
robot_position_sliders['x'] = {
    'id': p.addUserDebugParameter("Robot X Position [m]", -2.0, 2.0, initial_robot_position[0]),
    'axis': 0
}
robot_position_sliders['y'] = {
    'id': p.addUserDebugParameter("Robot Y Position [m]", -2.0, 2.0, initial_robot_position[1]),
    'axis': 1
}
robot_position_sliders['z'] = {
    'id': p.addUserDebugParameter("Robot Z Height [m]", 0.0, 1.0, initial_robot_position[2]),
    'axis': 2
}
print("  Robot position sliders created (X, Y, Z)")

# 바닥에 붙이기 버튼 추가
ground_button = p.addUserDebugParameter("== PLACE ON GROUND ==", 0, 1, 0)
print("  Ground placement button created")

# 모든 조인트 정보 수집 (조인트 이름 -> ID 매핑)
for i in range(num_joints):
    info = p.getJointInfo(robot, i)
    joint_name = info[1].decode('utf-8')
    joint_name_to_id[joint_name] = i


# ============================================================================
# 종동(passive) 조인트 퍼펫팅 — delta_passive.json (PyBullet 판, 2026-08-26)
#   Isaac/MuJoCo 와 동일 규약: q_pass = (m + f(m)) - f0, 매 스텝 모터각으로 수식
#   평가 → dpass_* 를 resetJointState 로 키네마틱 구동 (기본 모터는 해제).
#   unwrap(최근접 mod360) + 분기가드(모터 변화로 설명 안 되는 점프 서행) 포함.
# ============================================================================
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
    pt = _delta_fk(m0, m1, m2)
    if pt is None:
        return None
    d2r = math.pi / 180.0
    psi = 30.0 if arm == 1 else (150.0 if arm == 2 else -90.0)
    t = m1 if arm == 1 else (m2 if arm == 2 else m0)
    cp_, sp_ = math.cos(psi * d2r), math.sin(psi * d2r)
    px = pt[0] * cp_ + pt[1] * sp_; py = -pt[0] * sp_ + pt[1] * cp_; pz = pt[2]
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
                    pt = _delta_fk(m0, m1, m2)
                    if pt is None:
                        return None
                    result = pt[sel - 2]
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


_passive_defs_pb = []
_passive_last_pb = {}
_passive_motor_ids_pb = {}
_closure_mode_pb = False
_closure_rods_pb = []      # 두 번째 로드 (거리 스프링): K2(무릎블록)↔P2(플레이트), 길이 L
# ★스프링 게인은 질량에 맞춰 자동 스케일 (240Hz 외부힘은 명시적분 — K ≫ 0.15·m·f² 이면 폭발.
#   1g 링크에 K=1500 을 줬다가 dt·ω=160 대폭발로 관절이 엉킨 실측 사고)
_CLOSURE_ROD_K = 10.0      # N/m — _init 에서 링크 질량 기준 재계산
_CLOSURE_ROD_C = 0.05      # N·s/m


def _link_anchor_to_com_pb(link_idx, anchor):
    """URDF 링크 프레임 앵커 → PyBullet COM(inertial) 프레임 (createConstraint 규약)"""
    dyn = p.getDynamicsInfo(robot, link_idx)
    inv_p_, inv_o_ = p.invertTransform(dyn[3], dyn[4])
    pos, _ = p.multiplyTransforms(inv_p_, inv_o_, anchor, [0, 0, 0, 1])
    return pos


def _init_passive_puppet_pb():
    global _passive_defs_pb, _closure_mode_pb, _closure_rods_pb
    _passive_defs_pb = []
    _closure_rods_pb = []
    _closure_mode_pb = False
    _passive_last_pb.clear()
    _passive_motor_ids_pb.clear()
    path = os.path.join(release_dir, "delta_passive.json")
    if not os.path.exists(path):
        return
    try:
        import json as _json
        with open(path, "r", encoding="utf-8") as fp:
            data = _json.load(fp)
        closures = data.get("closures", [])
        # 종동 조인트는 어느 모드든 무동력 (자유 관절)
        for d in data.get("passive_joints", []):
            jn = d.get("joint")
            if jn in joint_name_to_id:
                d["_jid"] = joint_name_to_id[jn]
                p.setJointMotorControl2(robot, d["_jid"], p.VELOCITY_CONTROL, force=0)
                if not closures:
                    _passive_defs_pb.append(d)   # 폐루프 정보 없을 때만 퍼펫 폴백
        for _nm, _jid in joint_name_to_id.items():
            if _nm.startswith("T") and _nm[1:].isdigit():
                _passive_motor_ids_pb[int(_nm[1:])] = _jid

        # ★진짜 물리 폐루프 (2026-08-26): 퍼펫 대신 구속으로 폐루프 유지.
        #   ①로드팁 point2point ×2 (MJCF <connect> 대응 — 앵커는 COM 프레임 변환 필수)
        #   ②두 번째 로드 = 거리 스프링 ×3 (MJCF tendon equality 대응 — PyBullet 에 거리
        #     구속이 없어 매 스텝 K2↔P2 스프링-댐퍼 외력으로 근사. 평행사변형 등가)
        n_p2p = 0
        for clo in closures:
            rj = clo.get("rod_joint") or ""
            pj_ = clo.get("plate_joint") or ""
            if rj and rj in joint_name_to_id and pj_ in joint_name_to_id:
                jid_r = joint_name_to_id[rj]
                jid_p = joint_name_to_id[pj_]
                a_r = _link_anchor_to_com_pb(jid_r, clo["rod_xyz"])
                a_p = _link_anchor_to_com_pb(jid_p, clo["plate_xyz"])
                cid = p.createConstraint(robot, jid_r, robot, jid_p,
                                         p.JOINT_POINT2POINT, [0, 0, 0], a_r, a_p)
                p.changeConstraint(cid, maxForce=500)
                n_p2p += 1
            kj = clo.get("knee_joint") or ""
            ax = clo.get("pair_axis", [0, 0, 0])
            alen = math.sqrt(ax[0] ** 2 + ax[1] ** 2 + ax[2] ** 2)
            if kj and kj in joint_name_to_id and pj_ in joint_name_to_id and alen > 1e-9:
                w = float(clo.get("pair_halfwidth_m", 0.03))
                u = [ax[0] / alen * w, ax[1] / alen * w, ax[2] / alen * w]
                knee = clo.get("knee_xyz", [0, 0, 0])
                plate = clo.get("plate_xyz", [0, 0, 0])
                _closure_rods_pb.append({
                    "linkA": p.getJointInfo(robot, joint_name_to_id[kj])[16],  # 무릎 조인트의 parent 링크
                    "posA": [knee[0] + u[0], knee[1] + u[1], knee[2] + u[2]],
                    "linkB": joint_name_to_id[pj_],
                    "posB": [plate[0] + u[0], plate[1] + u[1], plate[2] + u[2]],
                    "L": float(clo.get("rod_len_m", 0.0)),
                })
        if closures:
            _closure_mode_pb = True
            # 스프링 게인 자동 스케일: 관련 링크 최소 질량 기준 (안정 한계의 15%)
            global _CLOSURE_ROD_K, _CLOSURE_ROD_C
            m_ref = None
            for rod in _closure_rods_pb:
                for li in (rod["linkA"], rod["linkB"]):
                    if li >= 0:
                        m_li = p.getDynamicsInfo(robot, li)[0]
                        if m_li > 0 and (m_ref is None or m_li < m_ref):
                            m_ref = m_li
            if m_ref is not None:
                _CLOSURE_ROD_K = max(1.0, min(0.15 * m_ref * 240.0 ** 2, 5000.0))
                _CLOSURE_ROD_C = 0.6 * math.sqrt(_CLOSURE_ROD_K * m_ref)
            print(f"[Closure] PyBullet 물리 폐루프: p2p {n_p2p}개 + rod2 스프링 {len(_closure_rods_pb)}개 "
                  f"(K={_CLOSURE_ROD_K:.1f} C={_CLOSURE_ROD_C:.3f}) — 퍼펫 OFF", flush=True)
        elif _passive_defs_pb:
            print(f"[Passive] delta_passive.json: {len(_passive_defs_pb)} joints (PyBullet 퍼펫팅 활성)", flush=True)
    except Exception as ex:
        print(f"[Passive] load 실패: {ex}", flush=True)


def _link_world_point_pb(link_idx, local_pt):
    """링크 프레임 로컬 점 → 월드 좌표 + 그 점의 월드 속도"""
    st = p.getLinkState(robot, link_idx, computeLinkVelocity=1, computeForwardKinematics=1)
    lw_pos, lw_orn = st[4], st[5]          # URDF 링크 프레임 월드 자세
    com_pos = st[0]
    v_com, w_ang = st[6], st[7]
    pt, _ = p.multiplyTransforms(lw_pos, lw_orn, local_pt, [0, 0, 0, 1])
    r = [pt[0] - com_pos[0], pt[1] - com_pos[1], pt[2] - com_pos[2]]
    v = [v_com[0] + w_ang[1] * r[2] - w_ang[2] * r[1],
         v_com[1] + w_ang[2] * r[0] - w_ang[0] * r[2],
         v_com[2] + w_ang[0] * r[1] - w_ang[1] * r[0]]
    return pt, v


def _apply_closure_rods_pb():
    """매 스텝: 두 번째 로드 거리 스프링-댐퍼 (K2↔P2, 목표 길이 L)"""
    for rod in _closure_rods_pb:
        pa, va = _link_world_point_pb(rod["linkA"], rod["posA"])
        pb_, vb = _link_world_point_pb(rod["linkB"], rod["posB"])
        dx = [pb_[0] - pa[0], pb_[1] - pa[1], pb_[2] - pa[2]]
        dist = math.sqrt(dx[0] ** 2 + dx[1] ** 2 + dx[2] ** 2)
        if dist < 1e-9:
            continue
        u = [dx[0] / dist, dx[1] / dist, dx[2] / dist]
        vrel = ((vb[0] - va[0]) * u[0] + (vb[1] - va[1]) * u[1] + (vb[2] - va[2]) * u[2])
        f = _CLOSURE_ROD_K * (dist - rod["L"]) + _CLOSURE_ROD_C * vrel
        fa = [f * u[0], f * u[1], f * u[2]]          # A 를 B 쪽으로 (늘어나면 당김)
        fb = [-f * u[0], -f * u[1], -f * u[2]]
        p.applyExternalForce(robot, rod["linkA"], fa, pa, p.WORLD_FRAME)
        p.applyExternalForce(robot, rod["linkB"], fb, pb_, p.WORLD_FRAME)


def _apply_passive_puppet_pb():
    """매 스텝: 폐루프 모드면 rod2 스프링, 아니면 퍼펫 (unwrap+분기가드)"""
    if _closure_mode_pb:
        _apply_closure_rods_pb()
        return
    if not _passive_defs_pb:
        return
    motors = {}
    m_step = 0.0
    for _mid, _jid in _passive_motor_ids_pb.items():
        v = p.getJointState(robot, _jid)[0] * 180.0 / math.pi
        pv = _passive_last_pb.get(("m", _mid))
        if pv is not None and abs(v - pv) > m_step:
            m_step = abs(v - pv)
        _passive_last_pb[("m", _mid)] = v
        motors[_mid] = v
    for d in _passive_defs_pb:
        f_val = _edh_eval_formula(d.get("formula", ""), motors)
        if f_val is None:
            continue
        m_deg = motors.get(d.get("motor_id", -1), 0.0)
        q_deg = (m_deg + f_val) - float(d.get("zero_bake_deg", 0.0))
        prev = _passive_last_pb.get(d["_jid"])
        if prev is not None:
            d_un = ((q_deg - prev + 180.0) % 360.0) - 180.0
            rate = 1.0 if abs(d_un) > (5.0 + 3.0 * m_step) else 20.0
            if d_un > rate:
                d_un = rate
            elif d_un < -rate:
                d_un = -rate
            q_deg = prev + d_un
        _passive_last_pb[d["_jid"]] = q_deg
        p.resetJointState(robot, d["_jid"], q_deg * math.pi / 180.0, targetVelocity=0.0)


_init_passive_puppet_pb()

# 트랙 벨트 flow 애니메이션 초기화 (ENABLE_TRACK_FLOW=True + JSON 있을 때만)
print(f"[Track] 동적 세그먼트 초기화 시작 (ENABLE_TRACK_FLOW={ENABLE_TRACK_FLOW})", flush=True)
track_belts = load_track_belts(release_dir) if ENABLE_TRACK_FLOW else []
print(f"[Track] 동적 세그먼트 초기화 완료: {len(track_belts)} 트랙", flush=True)
for _tb in track_belts:
    print(f"[Track]  - {_tb['motor_joint']}: {_tb['n_seg']} 세그먼트, body_ids {len(_tb['body_ids'])}개", flush=True)

# 트랙 구동 휠 + 지면 마찰 튜닝 (탱크 전진 가능하게)
if track_belts:
    boost_mimic_wheels_friction(robot)
    print(f"[Track] 휠/지면 마찰 튜닝 완료 (lateralFriction=2.5, plane=2.0)", flush=True)

# Mimic 관계 설정
if mimic_relationships:
    print(f"\nMimic joints found: {len(mimic_relationships)}")
    for child_name, mimic_data in mimic_relationships.items():
        if child_name in joint_name_to_id and mimic_data['parent'] in joint_name_to_id:
            child_id = joint_name_to_id[child_name]
            parent_id = joint_name_to_id[mimic_data['parent']]
            
            mimic_joints[child_id] = {
                'parent_id': parent_id,
                'parent_name': mimic_data['parent'],
                'multiplier': mimic_data['multiplier'],
                'offset': mimic_data['offset']
            }
            print(f"  {child_name} -> {mimic_data['parent']} (x{mimic_data['multiplier']})")

# 제어 가능한 조인트 찾기 및 슬라이더 생성
print("\nCreating control sliders...")
slider_count = 0

for i in range(num_joints):
    info = p.getJointInfo(robot, i)
    joint_name = info[1].decode('utf-8')
    joint_type = info[2]

    # REVOLUTE(회전) 또는 PRISMATIC(직선) 조인트만 제어 대상
    # PyBullet은 URDF continuous를 REVOLUTE(type=0)로 로드하되 limit=0/-1로 구분
    # Mimic joint는 제외 (부모 조인트에 의해 자동 제어됨)
    # ★종동(dpass_*) 조인트도 제외 — 퍼펫 전용. 숫자 추출 매핑이 dpass_1→1 로
    #   T1 의 소켓 번호를 덮어써 3D 와 동작이 달라지는 실사고 (2026-08-26).
    if joint_type in [p.JOINT_REVOLUTE, p.JOINT_PRISMATIC] and i not in mimic_joints \
            and not joint_name.startswith("dpass"):
        joint_idx = len(movable_joints)
        movable_joints.append(i)

        # 조인트 번호 추출 및 매핑 (예: T11 -> 11)
        joint_num = extract_joint_number(joint_name)
        if joint_num is not None:
            joint_number_to_id[joint_num] = i

        # 조인트 한계값 가져오기
        lower_limit = info[8]
        upper_limit = info[9]

        # continuous joint 판별: PyBullet은 limit 없는 joint를 lower>=upper로 표시
        is_continuous = (joint_type == p.JOINT_REVOLUTE and lower_limit >= upper_limit)

        # 타입별 슬라이더 설정
        if is_continuous:
            # 바퀴형 연속 회전 조인트 — 속도 제어 (RPS: rotations per second, socket 과 단위 통일)
            slider_min = -5.0   # -5 RPS
            slider_max = 5.0    # +5 RPS
            slider_label = f"{joint_name} [RPS]"
            initial_value = 0
        elif joint_type == p.JOINT_REVOLUTE:
            # 일반 회전 조인트 처리
            if lower_limit < -10:
                lower_limit = -3.14  # -180도
            if upper_limit > 10:
                upper_limit = 3.14   # +180도
            # 라디안을 도로 변환하여 슬라이더에 표시
            slider_min = math.degrees(lower_limit)
            slider_max = math.degrees(upper_limit)
            slider_label = f"{joint_name} [deg]"
            initial_value = 0
        else:  # PRISMATIC (직선 조인트)
            if lower_limit < -10:
                lower_limit = -1.0
            if upper_limit > 10:
                upper_limit = 1.0
            slider_min = lower_limit
            slider_max = upper_limit
            slider_label = f"{joint_name} [m]"
            initial_value = 0
        
        # 슬라이더가 유효한 범위를 가지는지 확인
        if slider_max > slider_min:
            # 디버그 슬라이더 생성
            slider_id = p.addUserDebugParameter(
                paramName=slider_label,
                rangeMin=slider_min,
                rangeMax=slider_max,
                startValue=initial_value
            )
            
            # 슬라이더 정보 저장
            sliders[joint_idx] = {
                'id': slider_id,
                'joint_id': i,
                'type': joint_type,
                'name': joint_name,
                'is_continuous': is_continuous
            }

            # 조인트 정보 저장
            type_str = 'CONTINUOUS' if is_continuous else ('REVOLUTE' if joint_type == p.JOINT_REVOLUTE else 'PRISMATIC')
            joint_info[joint_idx] = {
                'id': i,
                'name': joint_name,
                'type': type_str,
                'lower_limit': lower_limit,
                'upper_limit': upper_limit,
                'current_pos': 0
            }
            
            slider_count += 1
            print(f"  [{joint_idx+1}] {joint_name} - Slider created")

if slider_count > 0:
    print(f"\n{slider_count} joint sliders created!")
else:
    print("\nNo controllable joints found!")

# 사용 가능한 조인트 번호 출력
print(f"\nJoint number mapping:")
for jnum in sorted(joint_number_to_id.keys()):
    jid = joint_number_to_id[jnum]
    jname = p.getJointInfo(robot, jid)[1].decode('utf-8')
    print(f"  [{jnum}] {jname}")

# 사용법 안내 출력
print("\n" + "=" * 60)
print("Controls:")
print("  - Robot Position Sliders: Control X, Y, Z position of entire robot")
print("  - Joint Sliders: Drag to control individual joints manually")
print("  - PLACE ON GROUND: Automatically place robot on the floor (Z=0)")
print("-" * 60)
print("  Socket Control:")
print("  - Socket format: 'time,delay,joint1,angle1,joint2,angle2,...;'")
print(f"  - Example: '2000,0,{sorted(joint_number_to_id.keys())[0] if joint_number_to_id else 11},45;'")
print("  - Use Play() function: Play('2000,0,11,45,12,90')")
print("-" * 60)
print("  Python API:")
print("  - test()  : Run test sequence")
print("  - Play('time,delay,joint,angle,...;')")
print("  - WaitMotion() : Wait for all motions to complete")
print("-" * 60)
print("  - Mouse: Rotate/zoom view")
print("  - Press 'q' or close window to exit")
print("=" * 60)

# 소켓 서버 스레드 시작
socket_thread = threading.Thread(target=socket_server_thread, daemon=True)
socket_thread.start()
print("Socket server thread started!")

print("\nSimulation running...")

####################################################
# 테스트 함수
####################################################
def test():
    """테스트 동작 시퀀스 - Play() 함수 사용 예제"""
    print("\n" + "=" * 50)
    print("Running test sequence...")
    print("=" * 50)

    # 사용 가능한 조인트 번호 출력
    if joint_number_to_id:
        available = sorted(joint_number_to_id.keys())
        print(f"Available joint numbers: {available}")

        if len(available) >= 1:
            j1 = available[0]

            # 예제 1: 첫 번째 조인트를 45도로 이동 (2초)
            print(f"\nMoving joint {j1} to 45 degrees...")
            Play(f"2000,0,{j1},45;")
            WaitMotion()

            # 예제 2: 첫 번째 조인트를 -45도로 이동 (2초)
            print(f"Moving joint {j1} to -45 degrees...")
            Play(f"2000,0,{j1},-45;")
            WaitMotion()

            # 예제 3: 첫 번째 조인트를 0도로 복귀 (1초)
            print(f"Moving joint {j1} back to 0 degrees...")
            Play(f"1000,0,{j1},0;")
            WaitMotion()

            if len(available) >= 2:
                j2 = available[1]
                # 예제 4: 두 조인트 동시 이동
                print(f"\nMoving joints {j1} and {j2} simultaneously...")
                Play(f"2000,0,{j1},30,{j2},-30;")
                WaitMotion()

                # 원위치 복귀
                print(f"Returning to zero position...")
                Play(f"1500,0,{j1},0,{j2},0;")
                WaitMotion()
    else:
        print("No joints available for testing!")

    print("\nTest sequence complete!")
    print("=" * 50)

# 시작 시 자동 실행할 명령들 (간결한 모션 프로그래밍 예제)
def startup_sequence_thread():
    """시작 시 자동 명령 순차 실행 - 간결한 모션 프로그래밍 예시"""

    # 시뮬레이션 준비 대기
    time.sleep(1.0)

    # 사용 가능한 조인트 번호 확인
    if joint_number_to_id:
        available = sorted(joint_number_to_id.keys())
        if len(available) >= 1:
            j1 = available[0]

            # 예제 동작 시퀀스
            Play(f"2000,0,{j1},-45")          # 첫 번째 조인트를 -45도로 2초간
            WaitMotion(2000)

            Play(f"2000,0,{j1},45")           # 첫 번째 조인트를 45도로 2초간
            WaitMotion(2000)

            if len(available) >= 3:
                j2, j3 = available[1], available[2]
                Play(f"2000,0,{j1},0,{j2},45,{j3},-45") # 다중 조인트 동시 이동
                WaitMotion(2000)

##########################################################################################
### 아래 주석 해제하면 최초 프로그램 시작 후 동작을 하게 된다. ###################################
##########################################################################################
# startup_thread = threading.Thread(target=startup_sequence_thread, daemon=True)
# startup_thread.start()
##########################################################################################

####################################################
# 메인 시뮬레이션 루프
####################################################
last_ground_value = 0

print("Press 'r' in console to run test(), 'c' to clear moves, 'q' to quit")

try:
    while simulation_running:
        # 자동 종료 (캡쳐 모드)
        if _ojw_auto_exit_start is not None and (time.time() - _ojw_auto_exit_start) >= _ojw_auto_exit_sec:
            print(f"[OJW] AUTO_EXIT reached ({_ojw_auto_exit_sec}s) — exiting", flush=True)
            simulation_running = False
            break

        # PyBullet 연결 상태 확인
        if p.getConnectionInfo()['isConnected'] == 0:
            break

        # Windows 키보드 입력 처리 (비블로킹)
        if sys.platform == 'win32' and msvcrt.kbhit():
            key = msvcrt.getch().decode('utf-8', errors='ignore').lower()
            if key == 'r':
                print("\n[Key 'r'] Running test()...")
                threading.Thread(target=test, daemon=True).start()
            elif key == 'c':
                active_moves.clear()
                print("\n[Key 'c'] Active moves cleared")
            elif key == 'q':
                print("\n[Key 'q'] Quitting...")
                simulation_running = False
                break

        # 로봇 위치 업데이트 (슬라이더 값에 따라)
        update_robot_position()
        
        # 소켓 명령 처리
        process_socket_commands(robot, movable_joints)
        # $scene placement live 적용 (MakeUrdf Scene 탭 Apply 버튼 → 즉시 spawn)
        process_scene_commands()
        # viewer 내 좌클릭 직접 배치 (active_scene_tool 설정된 경우)
        process_viewer_click_placement()

        # 조인트 이동 업데이트 (소켓 명령으로 인한 부드러운 이동)
        update_joint_movements(robot)
        
        # 슬라이더 값 읽기 및 조인트 제어 (수동 제어)
        for joint_idx, slider_data in sliders.items():
            try:
                slider_value = p.readUserDebugParameter(slider_data['id'])
                
                # 조인트 타입에 따른 값 변환
                if slider_data.get('is_continuous', False):
                    # 바퀴: 슬라이더 값은 RPS (rotations per second) → rad/s 변환
                    target_vel = slider_value * 2.0 * math.pi
                    target_pos = target_vel  # 비교용
                elif slider_data['type'] == p.JOINT_REVOLUTE:
                    target_pos = math.radians(slider_value)
                else:
                    target_pos = slider_value

                # 값이 변경되었을 때만 업데이트 (반복 작업이므로 print 제거)
                if abs(target_pos - joint_info[joint_idx]['current_pos']) > 0.001:
                    joint_id = slider_data['joint_id']

                    # 소켓 명령으로 움직이는 중이 아닐 때만 슬라이더 제어 허용
                    if joint_id not in active_moves:
                        if slider_data.get('is_continuous', False):
                            # 바퀴형: 속도 제어
                            p.setJointMotorControl2(
                                robot,
                                joint_id,
                                p.VELOCITY_CONTROL,
                                targetVelocity=target_vel,
                                force=2000
                            )
                        else:
                            # 일반 조인트: 위치 제어
                            p.setJointMotorControl2(
                                robot,
                                joint_id,
                                p.POSITION_CONTROL,
                                targetPosition=target_pos,
                                force=1000
                            )

                        # Mimic 조인트 업데이트 (슬라이더 조작 시에도 적용)
                        update_mimic_joints_for_parent(joint_id, target_pos)

                        joint_info[joint_idx]['current_pos'] = target_pos
            except:
                pass  # 슬라이더 읽기 실패 시 무시
                
        # 바닥에 붙이기 버튼 체크
        try:
            ground_value = p.readUserDebugParameter(ground_button)
            if ground_value > 0.5 and last_ground_value <= 0.5:
                # 로봇의 가장 낮은 점을 계산하여 바닥에 붙이기
                robot_pos, robot_orn = p.getBasePositionAndOrientation(robot)
                
                # 로봇의 AABB (Axis-Aligned Bounding Box) 구하기
                aabb_min, aabb_max = p.getAABB(robot)
                lowest_point = aabb_min[2]  # Z축 최저점
                
                # 바닥(Z=0)에 맞춰 위치 조정
                new_z = robot_pos[2] - lowest_point
                new_position = [robot_pos[0], robot_pos[1], new_z]
                
                # 로봇 위치 업데이트
                p.resetBasePositionAndOrientation(robot, new_position, robot_orn)
                current_robot_position[:] = new_position[:]
                
                # Z축 슬라이더도 업데이트
                p.setUserDebugParameterValue(robot_position_sliders['z']['id'], new_z)
                
                print(f"Robot placed on ground at height: {new_z:.3f}m")
            last_ground_value = ground_value
        except:
            pass
        
        # 트랙 벨트 flow 애니메이션 업데이트 (모터 각도 기반)
        update_track_belts(track_belts, robot, joint_name_to_id)

        # 종동(passive) 조인트 퍼펫팅 — 매 스텝 모터각 기반 갱신 (델타 폐루프)
        _apply_passive_puppet_pb()

        # 물리 시뮬레이션 한 스텝 실행
        p.stepSimulation()
        time.sleep(1./240.)  # 240 FPS로 제한

        # === 카메라 sensor 렌더 (update_rate 제한 적용) ===
        if _cv2_ok and camera_list:
            _now = time.time()
            for _i, _cam in enumerate(camera_list):
                _interval = 1.0 / max(1, _cam["rate"])
                if _now - _camera_last_render[_i] < _interval:
                    continue
                _camera_last_render[_i] = _now
                _img = _render_camera_view(robot, _cam, camera_link_ids[_i])
                if _img is None:
                    continue
                # cv2 는 BGR — pybullet RGB 를 BGR 로 변환
                _bgr = _img[:, :, ::-1]
                _cv2.imshow(_cam["name"] or f"Camera_{_i}", _bgr)
            # waitKey 1ms — 윈도우가 반응성 있게. ESC 누르면 카메라 뷰만 닫기
            _k = _cv2.waitKey(1) & 0xFF
            if _k == 27:  # ESC
                _cv2.destroyAllWindows()

        # 키보드 이벤트 체크 (q 키로 종료)
        keys = p.getKeyboardEvents()
        if ord('q') in keys and keys[ord('q')] & p.KEY_WAS_TRIGGERED:
            simulation_running = False
            break

except KeyboardInterrupt:
    print("\nInterrupted by user")
except Exception as e:
    print(f"\nError during simulation: {e}")

# 시뮬레이션 종료 처리
simulation_running = False
print("\nShutting down...")
try:
    if _cv2_ok:
        _cv2.destroyAllWindows()
except Exception:
    pass
p.disconnect()
print("Program terminated.")
