import mujoco
import mujoco.viewer
import numpy as np
import time
import sys
import math
import xml.etree.ElementTree as ET
import os
import socket
import threading
import queue
import json as _json  # 트랙 벨트 메타데이터 로드

# Windows 비블로킹 키보드 입력
if sys.platform == 'win32':
    import msvcrt
from dataclasses import dataclass
from typing import Dict, Optional, List, Tuple
from collections import defaultdict

##########################################
# 로딩 모드 설정
##########################################
# True: MJCF 파일 로드 (robot_stl.xml - C#에서 생성한 완전한 MJCF)
# False: URDF 파일 직접 로드 (robot_stl.urdf - MuJoCo 내장 URDF 파서 사용)
USE_MJCF_MODE = True

@dataclass
class JointMoveCommand:
    """조인트 이동 명령을 저장하는 데이터 클래스"""
    joint_id: int       # 조인트 ID (qpos 인덱스)
    target_angle: float # 목표 각도 (라디안)
    duration: float     # 이동 시간 (초 단위)
    start_time: float   # 시작 시간
    start_angle: float  # 시작 각도 (라디안)

# 전역 변수들
simulation_running = True               # 시뮬레이션 실행 상태

# ── 자동 종료 (캡쳐 모드용) ─────────────────────────────────────────────
# 환경변수 OJW_AUTO_EXIT_SEC 가 설정되면 그 초만큼 시뮬 후 자동 종료.
_ojw_auto_exit_sec = 0.0
try:
    _ojw_auto_exit_sec = float(os.environ.get('OJW_AUTO_EXIT_SEC', '0') or 0)
except Exception:
    _ojw_auto_exit_sec = 0.0
_ojw_auto_exit_start = time.time() if _ojw_auto_exit_sec > 0 else None
if _ojw_auto_exit_start is not None:
    print(f"[OJW] AUTO_EXIT_SEC={_ojw_auto_exit_sec} (capture mode)", flush=True)
command_queue = queue.Queue()          # 소켓 명령 큐
active_moves: Dict[int, JointMoveCommand] = {}  # 현재 실행 중인 조인트 이동 명령들
last_ctrl_values: Dict[int, float] = {}  # 마지막으로 설정한 ctrl 값 (UI 변경 감지용)
# === MuJoCo runtime $scene ===
#   MuJoCo 는 model recompile 없이 새 body 추가가 어려워, 현재는 메시지만 받아 큐에 보관.
#   추후 (Phase 2): MJCF 에 pre-allocated slot pool 추가하거나 MjSpec API 사용해 동적 spawn.
_mj_scene_active_tool = None
_mj_scene_pending: List[dict] = []

# MuJoCo 모델 및 데이터 (전역)
model = None
data = None
viewer = None

# 조인트 정보
joint_info = {}
movable_joints = []  # 제어 가능한 조인트 인덱스 리스트 (qpos 인덱스)
joint_name_to_id = {}
joint_number_to_qpos = {}  # 조인트 번호(예: 11, 12) → qpos 인덱스 매핑
mimic_joints = {}
actuator_to_qpos = {}  # actuator ctrl 인덱스 → qpos 인덱스 매핑
qpos_to_actuator = {}  # qpos 인덱스 → actuator ctrl 인덱스 매핑

# 파일 경로 설정 (Release 폴더 기준)
script_dir = os.path.dirname(os.path.abspath(__file__))
release_dir = os.path.join(script_dir, "..", "Release")  # python/../Release
mjcf_file = os.path.join(release_dir, "robot_stl.xml")    # MuJoCo MJCF 설정 파일
urdf_file = os.path.join(release_dir, "robot_stl.urdf")   # URDF 로봇 모델 (MJCF에서 include)

####################################################
# 설정 파일 읽기 (C#에서 생성)
####################################################
def read_viewer_config():
    """viewer_config.txt 파일에서 설정 읽기 (bool + float 혼합)."""
    config = {
        'FIXED_BASE': True,
        'USE_PYBULLET': True,
        'USE_MUJOCO': False,
        'SPAWN_Z': 0.4,
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
print(f"Fixed Base: {'Enabled (weld constraint active)' if USE_FIXED_BASE else 'Disabled'}")

print("\n" + "=" * 60)
print(" MuJoCo MJCF Viewer with Socket Control")
print("=" * 60)
print(f"Loading mode: {'MJCF (complete simulation)' if USE_MJCF_MODE else 'URDF (kinematics only)'}")
print(f"Release dir: {os.path.abspath(release_dir)}")
print(f"MJCF file: {mjcf_file}")
print(f"URDF file: {urdf_file}")

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

def parse_urdf_joints(urdf_path):
    """URDF 파일에서 모든 조인트 정보를 추출하여 반환"""
    joint_names = []  # 제어 가능한 조인트 이름 목록
    mimic_info = {}   # mimic 조인트 정보

    if not os.path.exists(urdf_path):
        print(f"Warning: URDF file not found: {urdf_path}")
        return joint_names, mimic_info

    try:
        tree = ET.parse(urdf_path)
        root = tree.getroot()

        for joint in root.findall('.//joint'):
            joint_name = joint.get('name')
            joint_type = joint.get('type', 'fixed')

            # mimic 정보 추출
            mimic = joint.find('mimic')
            if mimic is not None:
                parent_joint = mimic.get('joint')
                multiplier = float(mimic.get('multiplier', '1.0'))
                offset = float(mimic.get('offset', '0.0'))

                mimic_info[joint_name] = {
                    'parent': parent_joint,
                    'multiplier': multiplier,
                    'offset': offset
                }

            # revolute, continuous, prismatic 타입만 제어 가능
            if joint_type in ['revolute', 'continuous', 'prismatic']:
                # mimic가 아닌 경우만 actuator 대상에 추가
                if joint_name not in mimic_info:
                    joint_names.append(joint_name)

    except Exception as e:
        print(f"Note: Could not parse URDF: {e}")

    return joint_names, mimic_info

def parse_mimic_joints(urdf_path):
    """URDF 파일에서 mimic joint 정보를 추출하여 반환 (하위 호환성)"""
    _, mimic_info = parse_urdf_joints(urdf_path)
    return mimic_info

def get_joint_position(joint_idx):
    """현재 조인트 위치를 라디안 단위로 반환"""
    global data
    try:
        if joint_idx < len(data.qpos):
            return data.qpos[joint_idx]
    except:
        pass
    return 0.0

def update_mimic_joints():
    """Mimic 조인트들을 부모 조인트 값에 따라 업데이트"""
    global data, mimic_joints, qpos_to_actuator

    for child_name, mimic_data in mimic_joints.items():
        try:
            child_idx = mimic_data['child_idx']
            parent_idx = mimic_data['parent_idx']

            if parent_idx < len(data.qpos) and child_idx < len(data.qpos):
                parent_pos = data.qpos[parent_idx]
                child_target = mimic_data['offset'] + mimic_data['multiplier'] * parent_pos

                # Actuator가 있으면 ctrl로 제어
                if child_idx in qpos_to_actuator and len(data.ctrl) > 0:
                    ctrl_idx = qpos_to_actuator[child_idx]
                    if ctrl_idx < len(data.ctrl):
                        data.ctrl[ctrl_idx] = child_target
                else:
                    # fallback: qpos 직접 설정
                    data.qpos[child_idx] = child_target
        except:
            pass

def reset_all_joints():
    """모든 조인트를 초기 위치(0)로 리셋"""
    global data, active_moves, last_ctrl_values

    active_moves.clear()
    last_ctrl_values.clear()

    # mj_resetData: freejoint 쿼터니언 포함 모든 상태 올바르게 초기화
    mujoco.mj_resetData(model, data)

    # ctrl도 0으로 리셋
    for i in range(len(data.ctrl)):
        data.ctrl[i] = 0.0

    print("All joints reset to zero!")

def parse_socket_message(message):
    """소켓 메시지를 파싱하여 조인트 명령으로 변환"""
    try:
        values = message.strip().rstrip(';').split(',')

        if len(values) < 2:
            return None

        nTime = int(values[0])    # 이동 시간 (밀리초)
        nDelay = int(values[1])   # 딜레이 (현재 미사용)

        joint_commands = []
        remaining_values = values[2:]

        if len(remaining_values) % 2 != 0:
            return None

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
    """명령 문자열을 받아서 실행 큐에 추가하는 함수

    특수 명령:
        clear; - 모든 active_moves 삭제 (UI 슬라이더 제어 복원)
        reset; - 모든 조인트를 0으로 리셋
    """
    global active_moves, last_ctrl_values, _mj_scene_active_tool, _mj_scene_pending

    try:
        # SCENE_* 명령은 원본 case 보존 (lower 안 함). cmd_lower 는 기존 조인트 명령용.
        cmd_raw = command_string.strip().rstrip(';').strip()
        cmd_lower = cmd_raw.lower()

        # === $scene 명령 라우팅 ===
        #   MuJoCo 는 runtime 에 새 body 를 추가하기 어려워 (model recompile 필요),
        #   현재는 socket 메시지만 받아서 콘솔/큐에 기록 + 사용자 안내.
        #   Phase 2: pre-allocated slot pool 또는 MjSpec API 로 동적 add 구현 예정.
        if cmd_raw.startswith('SCENE_TOOL_CLEAR'):
            _mj_scene_active_tool = None
            print("[Scene/MuJoCo] tool cleared")
            return True
        if cmd_raw.startswith('SCENE_TOOL_SET'):
            parts = [s.strip() for s in cmd_raw.split(',')]
            if len(parts) >= 8:
                try:
                    _mj_scene_active_tool = {
                        'type':  parts[1],
                        'sx': float(parts[2]), 'sy': float(parts[3]), 'sz': float(parts[4]),
                        'color': int(parts[5], 0) if parts[5].lower().startswith('0x') else int(parts[5]),
                        'mass': float(parts[6]),
                        'zoffset': float(parts[7]),
                    }
                    print(f"[Scene/MuJoCo] tool set: {_mj_scene_active_tool['type']} (viewer click 미지원 — 다음 launch 시 적용)")
                    return True
                except Exception as ex:
                    print(f"[Scene/MuJoCo] SCENE_TOOL_SET parse error: {ex}")
                    return False
            return False
        if cmd_raw.startswith('SCENE_CLEAR'):
            _mj_scene_pending = []
            print("[Scene/MuJoCo] pending scene queue cleared (runtime remove 미지원)")
            return True
        if cmd_raw.startswith('SCENE_ADD'):
            parts = [s.strip() for s in cmd_raw.split(',')]
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
                    _mj_scene_pending.append(obj)
                    print(f"[Scene/MuJoCo] +{obj['type']} pending @ ({obj['x']:.3f},{obj['y']:.3f},{obj['z']:.3f}) — viewer restart 시 적용")
                    return True
                except Exception as ex:
                    print(f"[Scene/MuJoCo] SCENE_ADD parse error: {ex}")
                    return False
            return False

        # 특수 명령 처리 (기존)
        if cmd_lower == 'clear':
            active_moves.clear()
            last_ctrl_values.clear()
            print("Active moves cleared - UI control restored")
            return True
        elif cmd_lower == 'reset':
            reset_all_joints()
            return True

        # 일반 조인트 명령
        parsed = parse_socket_message(command_string)

        if parsed:
            command_queue.put(parsed)
            return True
        else:
            return False

    except Exception as e:
        print(f"Play exception: {e}")
        return False

def WaitMotion(milliseconds=None):
    """지정된 시간만큼 대기하거나, 모든 움직임이 완료될 때까지 대기"""
    if milliseconds is None:
        timeout = 10.0
        start_time = time.time()

        time.sleep(0.1)

        while len(active_moves) > 0 and (time.time() - start_time) < timeout:
            time.sleep(0.1)
    else:
        time.sleep(milliseconds / 1000.0)

def move_joint_to_angle(joint_number, target_degrees, duration_ms):
    """조인트 명령 처리.
       - Position actuator (pos_T*): target_degrees 를 각도(도) 로 해석, 부드럽게 보간
       - Velocity actuator (vel_track_T*): target_degrees 를 RPS 로 해석, 즉시 속도 설정
    """
    global data, model, joint_number_to_qpos, active_moves, qpos_to_actuator

    if joint_number not in joint_number_to_qpos:
        print(f"Invalid joint number: {joint_number} (available: {sorted(joint_number_to_qpos.keys())})")
        return False

    qpos_idx = joint_number_to_qpos[joint_number]

    # actuator 타입 판별 (velocity vs position)
    is_velocity = False
    ctrl_idx = qpos_to_actuator.get(qpos_idx, -1)
    if ctrl_idx >= 0 and ctrl_idx < len(data.ctrl):
        try:
            aname = mujoco.mj_id2name(model, mujoco.mjtObj.mjOBJ_ACTUATOR, ctrl_idx) or ""
            if aname.startswith("vel_"):
                is_velocity = True
        except Exception:
            pass

    if is_velocity:
        # Velocity actuator: target_degrees 는 RPS → rad/s 로 변환, 즉시 ctrl 설정 (보간 없음)
        target_vel_rad_s = target_degrees * 2.0 * math.pi
        if qpos_idx in active_moves:
            del active_moves[qpos_idx]
        if ctrl_idx >= 0 and ctrl_idx < len(data.ctrl):
            data.ctrl[ctrl_idx] = target_vel_rad_s
        return True

    # Position actuator: 도 → 라디안
    target_rad = math.radians(target_degrees)

    # time=0이면 즉시 이동 (보간 없음)
    if duration_ms <= 0:
        if qpos_idx in active_moves:
            del active_moves[qpos_idx]
        if ctrl_idx >= 0 and ctrl_idx < len(data.ctrl):
            data.ctrl[ctrl_idx] = target_rad
        return True

    # 현재 위치 가져오기
    current_pos_rad = get_joint_position(qpos_idx)

    # 이동 명령 객체 생성 (위치 보간)
    move_command = JointMoveCommand(
        joint_id=qpos_idx,
        target_angle=target_rad,
        duration=duration_ms / 1000.0,
        start_time=time.time(),
        start_angle=current_pos_rad
    )

    active_moves[qpos_idx] = move_command

    return True

def update_joint_movements():
    """활성 조인트 이동 명령들을 업데이트 - 물리 시뮬레이션 모드 (actuator ctrl 사용)"""
    global data, active_moves, qpos_to_actuator, last_ctrl_values

    # 활성 이동이 없으면 아무것도 하지 않음 (UI 컨트롤 허용)
    if len(active_moves) == 0:
        return

    current_time = time.time()
    completed_moves = []
    ui_changed_moves = []  # UI에서 변경된 조인트들

    # 활성 이동 명령 처리
    for joint_idx, move_cmd in active_moves.items():
        # UI 변경 감지: 이전에 설정한 ctrl 값과 현재 ctrl 값 비교
        if joint_idx in qpos_to_actuator:
            ctrl_idx = qpos_to_actuator[joint_idx]
            if ctrl_idx < len(data.ctrl) and joint_idx in last_ctrl_values:
                current_ctrl = data.ctrl[ctrl_idx]
                last_ctrl = last_ctrl_values[joint_idx]
                # UI에서 슬라이더를 움직였으면 값이 달라짐 (허용 오차 0.001 rad ≈ 0.06도)
                if abs(current_ctrl - last_ctrl) > 0.001:
                    ui_changed_moves.append(joint_idx)
                    continue  # 이 조인트는 보간 건너뜀

        elapsed = current_time - move_cmd.start_time
        progress = min(elapsed / move_cmd.duration, 1.0)

        if progress >= 1.0:
            current_angle = move_cmd.target_angle
            completed_moves.append(joint_idx)
        else:
            # 선형 보간
            current_angle = move_cmd.start_angle + (move_cmd.target_angle - move_cmd.start_angle) * progress

        # Actuator ctrl 값 업데이트
        if joint_idx in qpos_to_actuator:
            ctrl_idx = qpos_to_actuator[joint_idx]
            if ctrl_idx < len(data.ctrl):
                data.ctrl[ctrl_idx] = current_angle
                last_ctrl_values[joint_idx] = current_angle  # 설정한 값 저장

    # 완료된 이동 제거
    for joint_idx in completed_moves:
        del active_moves[joint_idx]
        if joint_idx in last_ctrl_values:
            del last_ctrl_values[joint_idx]

    # UI에서 변경된 조인트의 active_move 제거 (슬라이더 조작 시 즉시 반응)
    for joint_idx in ui_changed_moves:
        del active_moves[joint_idx]
        if joint_idx in last_ctrl_values:
            del last_ctrl_values[joint_idx]

def update_joint_movements_kinematics():
    """활성 조인트 이동 명령들을 업데이트 - 기구학 모드 (qpos 직접 설정)"""
    global data, active_moves

    if len(active_moves) == 0:
        return

    current_time = time.time()
    completed_moves = []

    for joint_idx, move_cmd in active_moves.items():
        elapsed = current_time - move_cmd.start_time
        progress = min(elapsed / move_cmd.duration, 1.0)

        if progress >= 1.0:
            current_angle = move_cmd.target_angle
            completed_moves.append(joint_idx)
        else:
            # 선형 보간
            current_angle = move_cmd.start_angle + (move_cmd.target_angle - move_cmd.start_angle) * progress

        # qpos 직접 설정 (기구학 모드)
        if joint_idx < len(data.qpos):
            data.qpos[joint_idx] = current_angle

    # 완료된 이동 제거
    for joint_idx in completed_moves:
        del active_moves[joint_idx]

def update_mimic_joints_kinematics():
    """Mimic 조인트들을 부모 조인트 값에 따라 업데이트 - 기구학 모드"""
    global data, mimic_joints

    for child_name, mimic_data in mimic_joints.items():
        try:
            child_idx = mimic_data['child_idx']
            parent_idx = mimic_data['parent_idx']

            if parent_idx < len(data.qpos) and child_idx < len(data.qpos):
                parent_pos = data.qpos[parent_idx]
                child_target = mimic_data['offset'] + mimic_data['multiplier'] * parent_pos
                # qpos 직접 설정 (기구학 모드)
                data.qpos[child_idx] = child_target
        except:
            pass

def socket_server_thread():
    """소켓 서버를 실행하는 스레드 함수"""
    global simulation_running

    HOST = '127.0.0.1'
    PORT = 4000

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

                            while ';' in buffer:
                                message, buffer = buffer.split(';', 1)
                                message = message.strip()

                                if message:
                                    success = Play(message + ';')
                                    response = "OK" if success else "ERROR"

                                    try:
                                        if response != "OK":
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

def process_socket_commands():
    """소켓 명령 큐에서 명령을 가져와 처리"""
    try:
        while not command_queue.empty():
            command = command_queue.get_nowait()

            for joint_cmd in command['joint_commands']:
                joint_number = joint_cmd['joint_id']  # 조인트 번호 (예: 11, 12)
                joint_angle = joint_cmd['angle']

                move_joint_to_angle(joint_number, joint_angle, command['time'])

    except queue.Empty:
        pass
    except Exception as e:
        print(f"Exception in process_socket_commands: {e}")

####################################################
# 테스트 함수
####################################################
def test():
    """테스트 동작 시퀀스 - Play() 함수 사용 예제"""
    print("\n" + "=" * 50)
    print("Running test sequence...")
    print("=" * 50)

    # 사용 가능한 조인트 번호 출력
    if joint_number_to_qpos:
        available = sorted(joint_number_to_qpos.keys())
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

####################################################
# 메인 실행 부분
####################################################

####################################################
# 트랙 벨트 flow 애니메이션 (robot_stl_tracks.json 기반, MuJoCo 버전)
####################################################

# True: MJCF 에서 정적 트랙 geom 제거 + mocap body 추가, 매 프레임 belt flow
# False: MJCF 그대로 (정적 트랙 visual)
ENABLE_TRACK_FLOW = True


def _belt_perimeter(pulleys):
    n_cnt = len(pulleys)
    total = 0.0
    for n in range(n_cnt):
        p0 = pulleys[n]; p1 = pulleys[(n+1)%n_cnt]; p2 = pulleys[(n+2)%n_cnt]
        dx = p1[0]-p0[0]; dy = p1[1]-p0[1]
        length = math.sqrt(dx*dx + dy*dy)
        if length < 1e-6: continue
        dr = p1[2]-p0[2]
        length_out = math.sqrt(max(length*length - dr*dr, 0.0))
        angle_deg = math.degrees(math.atan2(dy, dx))
        dx2 = p2[0]-p1[0]; dy2 = p2[1]-p1[1]
        angle2_deg = math.degrees(math.atan2(dy2, dx2))
        tmp_deg = ((angle_deg - angle2_deg) % 360.0 + 360.0) % 360.0
        R1 = p1[2] if p1[2] > 0 else 1.0
        arc_len = (tmp_deg / 360.0) * 2.0 * math.pi * R1
        total += length_out + arc_len
    return total


def _arc_pos_to_xy_rot(s, pulleys):
    n_cnt = len(pulleys)
    for n in range(n_cnt):
        p0 = pulleys[n]; p1 = pulleys[(n+1)%n_cnt]; p2 = pulleys[(n+2)%n_cnt]
        dx = p1[0]-p0[0]; dy = p1[1]-p0[1]
        length = math.sqrt(dx*dx + dy*dy)
        if length < 1e-6: continue
        dr = p1[2]-p0[2]
        length_out = math.sqrt(max(length*length - dr*dr, 0.0))
        angle_rad = math.atan2(dy, dx)
        angle_deg = math.degrees(angle_rad)
        angle_gap_rad = math.atan2(dr, length)
        angle_gap_deg = math.degrees(angle_gap_rad)
        if s < length_out:
            cos_a = math.cos(angle_rad); sin_a = math.sin(angle_rad)
            cos_g = math.cos(angle_gap_rad); sin_g = math.sin(angle_gap_rad)
            lx1 = s * cos_g
            ly1 = p0[2] + s * sin_g
            seg_x = p0[0] + lx1*cos_a - ly1*sin_a
            seg_y = p0[1] + lx1*sin_a + ly1*cos_a
            return (seg_x, seg_y, angle_deg + angle_gap_deg)
        s -= length_out
        dx2 = p2[0]-p1[0]; dy2 = p2[1]-p1[1]
        angle2_deg = math.degrees(math.atan2(dy2, dx2))
        tmp_deg = ((angle_deg - angle2_deg) % 360.0 + 360.0) % 360.0
        R1 = p1[2] if p1[2] > 0 else 1.0
        arc_len = (tmp_deg / 360.0) * 2.0 * math.pi * R1
        if s < arc_len:
            angle_diff_deg = s * 360.0 / (2.0*math.pi*R1)
            rot_deg = angle_deg + 90 - angle_diff_deg
            rot_rad = math.radians(rot_deg)
            seg_x = p1[0] + R1*math.cos(rot_rad)
            seg_y = p1[1] + R1*math.sin(rot_rad)
            return (seg_x, seg_y, rot_deg - 90)
        s -= arc_len
    return None


def _parse_scene_from_urdf_mj(urdf_path):
    """URDF 의 <scene><include .../></scene> 노드를 파싱 (PyBullet 와 동일 포맷)."""
    items = []
    if not os.path.exists(urdf_path):
        return items
    try:
        import xml.etree.ElementTree as _ET
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
                        color = [((cv >> 16) & 0xFF) / 255.0,
                                 ((cv >> 8) & 0xFF) / 255.0,
                                 (cv & 0xFF) / 255.0, 1.0]
                    except Exception:
                        color = None
                items.append({"url": url, "xyz": xyz, "rpy": rpy, "scale": scale, "color": color})
    except Exception as ex:
        print(f"[Scene] parse error: {ex}", flush=True)
    return items


def _scene_box_size_for_url(url, scale):
    """URL 키워드 매칭으로 box half-size (MuJoCo size = half-extents) 반환."""
    u = url.lower()
    if "cube_small" in u or "cube" in u:
        s = 0.02 * scale          # 4cm cube → half=2cm
    elif "tray" in u:
        return (0.10 * scale, 0.075 * scale, 0.01 * scale)
    else:
        s = 0.02 * scale          # default 4cm cube
    return (s, s, s)


def preprocess_mjcf_for_scene(mjcf_path_in, scene_items):
    """MJCF 의 worldbody 에 scene 객체용 body+geom (box primitive) 추가, 새 파일에 저장."""
    if not scene_items:
        return mjcf_path_in
    if not os.path.exists(mjcf_path_in):
        return mjcf_path_in
    try:
        import xml.etree.ElementTree as ET
        tree = ET.parse(mjcf_path_in)
        root = tree.getroot()
        worldbody = root.find("worldbody")
        if worldbody is None:
            worldbody = ET.SubElement(root, "worldbody")
        for idx, it in enumerate(scene_items):
            sx, sy, sz = _scene_box_size_for_url(it["url"], it["scale"])
            rgba = "0.6 0.6 0.6 1"
            if it["color"]:
                rgba = " ".join(f"{v:.3f}" for v in it["color"])
            pos = " ".join(f"{v:.6f}" for v in it["xyz"])
            euler = " ".join(f"{v:.6f}" for v in it["rpy"])
            body = ET.SubElement(worldbody, "body",
                name=f"scene_{idx}", pos=pos, euler=euler)
            ET.SubElement(body, "geom", type="box",
                size=f"{sx:.6f} {sy:.6f} {sz:.6f}", rgba=rgba)
        out_path = mjcf_path_in + ".scene.xml"
        tree.write(out_path, encoding="utf-8", xml_declaration=True)
        return out_path
    except Exception as ex:
        print(f"[Scene] preprocess error: {ex}", flush=True)
        return mjcf_path_in


def _preview_tracks_meta_mj(release_dir_local):
    jp = os.path.join(release_dir_local, "robot_stl_tracks.json")
    if not os.path.exists(jp):
        return []
    try:
        with open(jp, 'r', encoding='utf-8-sig') as f:
            return _json.load(f).get("tracks", [])
    except Exception as e:
        print(f"[Track] _preview_tracks_meta_mj 실패: {e}", flush=True)
        return []


def preprocess_mjcf_for_track_flow(mjcf_path_in, tracks_metas):
    """MJCF 에서 track mesh 참조하는 geom 전체 제거 + worldbody 아래에 mocap body 추가."""
    if not tracks_metas:
        return mjcf_path_in, []
    # 트랙 mesh 이름 집합 (예: "caterpillar_0" from "stl/caterpillar_0.stl")
    track_mesh_names = set()
    for entry in tracks_metas:
        stl = entry.get("segment_stl", "")
        if stl:
            track_mesh_names.add(os.path.splitext(os.path.basename(stl))[0])
    if not track_mesh_names:
        return mjcf_path_in, []

    try:
        tree = ET.parse(mjcf_path_in)
    except Exception as e:
        print(f"[Track] MJCF 파싱 실패: {e}", flush=True)
        return mjcf_path_in, []
    root = tree.getroot()

    # 1. 정적 트랙 geom 은 그대로 둠 (visual + collision 모두 유지)
    #    → 지면 접촉/구동력 제공
    #    → Mocap body 는 그 위에 overlay (시각 이중이지만 기능상 문제 없음)
    removed_count = 0

    # 1.5. <asset> 에 track mesh 가 이미 등록되어 있는지 확인. 없으면 보충.
    #      휴머노이드 등 URDF 메인 체인에 트랙 mesh 가 빠진 예제에서도 mocap 렌더가 가능하도록.
    asset = root.find('asset')
    if asset is None:
        asset = ET.SubElement(root, 'asset')
    existing_meshes = set()
    for m in asset.findall('mesh'):
        nm = m.get('name')
        if nm:
            existing_meshes.add(nm)
    added_asset = 0
    for entry in tracks_metas:
        stl = entry.get("segment_stl", "")
        if not stl:
            continue
        mesh_name = os.path.splitext(os.path.basename(stl))[0]
        if mesh_name in existing_meshes:
            continue
        scale = float(entry.get("stl_scale", 0.001))
        ET.SubElement(asset, 'mesh', {
            'name': mesh_name,
            'file': stl,
            'scale': "{0:.6f} {0:.6f} {0:.6f}".format(scale),
        })
        existing_meshes.add(mesh_name)
        added_asset += 1

    # 2. worldbody 에 mocap body 추가
    worldbody = root.find('worldbody')
    if worldbody is None:
        print(f"[Track] MJCF worldbody 없음 → mocap body 추가 불가", flush=True)
        return mjcf_path_in, []

    track_summary = []
    for entry in tracks_metas:
        pulleys = entry.get("pulleys_2d_mm", [])
        dist = entry.get("segment_distance_mm", 10.0)
        if len(pulleys) < 2 or dist < 1e-3:
            continue
        perim = _belt_perimeter(pulleys)
        if perim < 1e-3:
            continue
        n_seg = max(1, int(round(perim / dist)))
        seg_dist_adj = perim / n_seg

        stl = entry.get("segment_stl", "")
        mesh_name = os.path.splitext(os.path.basename(stl))[0]
        color = entry.get("color_rgb", [0.7, 0.7, 0.7])
        motor_name = entry.get("motor_joint", "track")

        for i in range(n_seg):
            body = ET.SubElement(worldbody, 'body', {
                'name': f"track_mocap_{motor_name}_{i}",
                'mocap': "true",
                'pos': "0 0 -10",
            })
            ET.SubElement(body, 'geom', {
                'type': "mesh",
                'mesh': mesh_name,
                'rgba': f"{color[0]:.4f} {color[1]:.4f} {color[2]:.4f} 1.0",
                'contype': "0",
                'conaffinity': "0",
            })
        track_summary.append({
            "motor_joint": motor_name,
            "pulleys": pulleys,
            "seg_dist": seg_dist_adj,
            "perim": perim,
            "n_seg": n_seg,
            "track_frame_pos": entry.get("track_frame_pos_m", [0,0,0]),
            "track_frame_rpy_deg": entry.get("track_frame_rpy_deg", [0,0,0]),
        })

    temp_path = mjcf_path_in + ".trackanim.xml"
    try:
        tree.write(temp_path, xml_declaration=True, encoding='utf-8')
        print(f"[Track] MJCF geom {removed_count}개 제거, <asset> mesh {added_asset}개 보충, mocap body {sum(t['n_seg'] for t in track_summary)}개 추가 → {os.path.basename(temp_path)}", flush=True)
        return temp_path, track_summary
    except Exception as e:
        print(f"[Track] MJCF 임시 저장 실패: {e}", flush=True)
        return mjcf_path_in, []


def _rpy_deg_to_quat(rpy_deg):
    """Roll-Pitch-Yaw (deg) → MuJoCo quat (wxyz). Rz·Ry·Rx 규약."""
    r, p, y = [math.radians(a) for a in rpy_deg]
    cr, sr = math.cos(r/2), math.sin(r/2)
    cp, sp = math.cos(p/2), math.sin(p/2)
    cy, sy = math.cos(y/2), math.sin(y/2)
    return np.array([
        cr*cp*cy + sr*sp*sy,
        sr*cp*cy - cr*sp*sy,
        cr*sp*cy + sr*cp*sy,
        cr*cp*sy - sr*sp*cy,
    ])


def _quat_mul(q1, q2):
    w1,x1,y1,z1 = q1; w2,x2,y2,z2 = q2
    return np.array([
        w1*w2 - x1*x2 - y1*y2 - z1*z2,
        w1*x2 + x1*w2 + y1*z2 - z1*y2,
        w1*y2 - x1*z2 + y1*w2 + z1*x2,
        w1*z2 + x1*y2 - y1*x2 + z1*w2,
    ])


def _quat_rotate(q, v):
    w,x,y,z = q
    t = np.array([
        2*(y*v[2] - z*v[1]),
        2*(z*v[0] - x*v[2]),
        2*(x*v[1] - y*v[0])
    ])
    return v + w*t + np.cross([x,y,z], t)


def boost_track_wheels_mujoco(model_local, track_mesh_names=None):
    """트랙 구동계 마찰 튜닝.
       - 정적 트랙 mesh geom (유저가 만든 탱크 벨트 실물 — collision) → 고마찰
       - 휠 실린더 (drive joint 자식) → 고마찰
       - 그 외 chassis/팔 → 저마찰 (얼음)
       - 지면 → 고마찰
    """
    if track_mesh_names is None:
        track_mesh_names = set()

    # track_T* joint 의 body 수집
    track_wheel_bodies = set()
    for i in range(model_local.njnt):
        jname = mujoco.mj_id2name(model_local, mujoco.mjtObj.mjOBJ_JOINT, i)
        if jname and 'track_T' in jname:
            track_wheel_bodies.add(int(model_local.jnt_bodyid[i]))

    static_track_count = 0
    wheel_geom_count = 0
    low_friction_count = 0
    plane_count = 0
    for g in range(model_local.ngeom):
        body_id = int(model_local.geom_bodyid[g])
        body_name = mujoco.mj_id2name(model_local, mujoco.mjtObj.mjOBJ_BODY, body_id) or ""

        # mocap body 의 geom → skip (원래 contype=0)
        if body_name.startswith('track_mocap_'):
            continue

        # 지면 (worldbody 직속 geom)
        if body_id == 0:
            model_local.geom_friction[g] = [2.0, 0.05, 0.01]
            plane_count += 1
            continue

        # 정적 트랙 mesh 감지 (유저 eDH 로 만들어진 탱크 벨트 자체)
        is_static_track = False
        if model_local.geom_type[g] == mujoco.mjtGeom.mjGEOM_MESH:
            mesh_id = int(model_local.geom_dataid[g])
            if mesh_id >= 0:
                mname = mujoco.mj_id2name(model_local, mujoco.mjtObj.mjOBJ_MESH, mesh_id) or ""
                if mname in track_mesh_names:
                    is_static_track = True

        if is_static_track:
            # 탱크 벨트 — 지면 접촉 표면. 적당한 고마찰 (과하면 불안정)
            model_local.geom_friction[g] = [3.0, 0.1, 0.01]
            static_track_count += 1
        elif body_id in track_wheel_bodies:
            # 드라이브 휠 실린더 (트랙 안쪽에서 회전) — 중간 마찰
            model_local.geom_friction[g] = [1.5, 0.05, 0.005]
            wheel_geom_count += 1
        else:
            # chassis / 팔: 낮은 마찰 (걸리지 않게)
            model_local.geom_friction[g] = [0.1, 0.005, 0.0001]
            low_friction_count += 1

    # actuator force range 증강 (velocity actuator 힘 부족 시 구동 안됨)
    try:
        for a in range(model_local.nu):
            aname = mujoco.mj_id2name(model_local, mujoco.mjtObj.mjOBJ_ACTUATOR, a) or ""
            if 'track_T' in aname:
                model_local.actuator_forcerange[a] = [-5000.0, 5000.0]
                model_local.actuator_forcelimited[a] = 0
    except Exception as ex:
        print(f"[Track] actuator force 튜닝 실패: {ex}", flush=True)

    print(f"[Track] MuJoCo 정적 트랙 geom {static_track_count} 고마찰, 휠 {wheel_geom_count} 고마찰, 비휠 {low_friction_count} 얼음, 지면 {plane_count} 강마찰", flush=True)


def apply_kinematic_tank_drive(model_local, data_local, tracks_summary):
    """URDF/MJCF 의 wheel-track-ground 물리 체인 한계 우회:
    양쪽 트랙 모터 속도로부터 차동 구동(differential drive) 계산 → base freejoint 의 linear/yaw 속도 직접 설정.
    물리 시뮬은 정적 트랙으로 지면 지지만 담당.
    """
    if not tracks_summary or len(tracks_summary) < 2: return
    # freejoint 가 있어야 base 속도 조작 가능
    try:
        root_joint_id = mujoco.mj_name2id(model_local, mujoco.mjtObj.mjOBJ_JOINT, "root_freejoint")
        if root_joint_id < 0: return
        root_qvel_idx = int(model_local.jnt_dofadr[root_joint_id])
    except Exception:
        return

    # 양쪽 트랙 모터 각속도 읽기
    wheel_speeds = []
    for track in tracks_summary:
        jname = track["motor_joint"]
        jid = mujoco.mj_name2id(model_local, mujoco.mjtObj.mjOBJ_JOINT, jname)
        if jid < 0:
            wheel_speeds.append(0.0)
            continue
        dof_idx = int(model_local.jnt_dofadr[jid])
        omega = float(data_local.qvel[dof_idx])  # rad/s
        R_mm = track["pulleys"][0][2] if track["pulleys"][0][2] > 0 else 30.0
        v_linear = omega * (R_mm * 0.001)  # m/s (휠 접선 속도)
        wheel_speeds.append(v_linear)

    if len(wheel_speeds) < 2: return

    # 차동 구동: 두 트랙 속도로부터 linear / angular 계산
    v_L = wheel_speeds[0]
    v_R = wheel_speeds[1]
    # 부호 반전: body X 축 방향이 사용자 "전방" 과 반대라 negate
    linear_vel = -(v_L + v_R) / 2.0
    # 트랙 간 거리 (좌우 간격). 2개 트랙의 base 좌표로부터 추정 (Y 차이).
    try:
        dy = tracks_summary[0]["track_frame_pos"][1] - tracks_summary[1]["track_frame_pos"][1]
        wheelbase = abs(dy) if abs(dy) > 1e-3 else 0.144
    except Exception:
        wheelbase = 0.144  # 기본값: 144mm
    angular_vel = (v_R - v_L) / wheelbase

    # 로봇 base 의 world orientation (quat wxyz) 으로 body 전방 벡터 계산
    base_body_id = mujoco.mj_name2id(model_local, mujoco.mjtObj.mjOBJ_BODY, "base_link")
    if base_body_id < 0: base_body_id = 1
    base_quat = data_local.xquat[base_body_id]
    # 로봇 전방 벡터 = quat * (1,0,0) — 기존 xmat 사용이 더 편함
    base_mat = data_local.xmat[base_body_id].reshape(3, 3)
    forward_vec = base_mat[:, 0]  # body X 축의 world 좌표

    # base freejoint qvel[0..2] = linear velocity (world), qvel[3..5] = angular velocity
    data_local.qvel[root_qvel_idx + 0] = linear_vel * forward_vec[0]
    data_local.qvel[root_qvel_idx + 1] = linear_vel * forward_vec[1]
    # Z velocity 는 그대로 (중력/관성 유지)
    data_local.qvel[root_qvel_idx + 5] = angular_vel  # yaw rate (world Z)


def update_track_belts_mujoco(model_local, data_local, tracks_summary):
    """매 프레임: 각 트랙의 모터 각도 읽어 mocap body 위치 갱신."""
    if not tracks_summary: return
    # 로봇 base body (첫 번째 non-world body = id 1 또는 이름 "base_link")
    base_body_id = mujoco.mj_name2id(model_local, mujoco.mjtObj.mjOBJ_BODY, "base_link")
    if base_body_id < 0:
        base_body_id = 1
    base_pos = np.array(data_local.xpos[base_body_id], dtype=float)
    base_quat = np.array(data_local.xquat[base_body_id], dtype=float)  # wxyz

    for track in tracks_summary:
        motor_name = track["motor_joint"]
        jnt_id = mujoco.mj_name2id(model_local, mujoco.mjtObj.mjOBJ_JOINT, motor_name)
        if jnt_id < 0: continue
        qpos_idx = model_local.jnt_qposadr[jnt_id]
        if qpos_idx >= len(data_local.qpos): continue
        motor_angle_rad = float(data_local.qpos[qpos_idx])

        R_mm = track["pulleys"][0][2] if track["pulleys"][0][2] > 0 else 30.0
        belt_offset_mm = -motor_angle_rad * R_mm

        tf_pos = np.array(track["track_frame_pos"], dtype=float)
        tf_quat = _rpy_deg_to_quat(track["track_frame_rpy_deg"])

        # track world = base * track_local
        track_world_pos = base_pos + _quat_rotate(base_quat, tf_pos)
        track_world_quat = _quat_mul(base_quat, tf_quat)

        for i in range(track["n_seg"]):
            s = (i * track["seg_dist"] + belt_offset_mm) % track["perim"]
            r = _arc_pos_to_xy_rot(s, track["pulleys"])
            if r is None: continue
            seg_x, seg_y, seg_rot_deg = r
            seg_local_pos = np.array([seg_x * 0.001, seg_y * 0.001, 0.0])
            seg_local_quat = _rpy_deg_to_quat([0, 0, seg_rot_deg])

            seg_world_pos = track_world_pos + _quat_rotate(track_world_quat, seg_local_pos)
            seg_world_quat = _quat_mul(track_world_quat, seg_local_quat)

            body_name = f"track_mocap_{motor_name}_{i}"
            body_id = mujoco.mj_name2id(model_local, mujoco.mjtObj.mjOBJ_BODY, body_name)
            if body_id < 0: continue
            mocap_id = int(model_local.body_mocapid[body_id])
            if mocap_id < 0: continue
            data_local.mocap_pos[mocap_id] = seg_world_pos
            data_local.mocap_quat[mocap_id] = seg_world_quat


####################################################
# URDF에서 조인트 정보 파싱
####################################################
controllable_joints, mimic_relationships = parse_urdf_joints(urdf_file)
print(f"Controllable joints: {len(controllable_joints)}")
for jname in controllable_joints:
    print(f"  - {jname}")

####################################################
# MuJoCo 모델 로드


# MuJoCo 모델 로드
original_cwd = os.getcwd()
free_floating_joint_qpos_addr = -1   # freejoint qpos 시작 인덱스 (-1이면 없음)

try:
    # 작업 디렉토리를 Release 폴더로 변경 (메시 파일 로딩을 위해)
    os.chdir(release_dir)

    if USE_MJCF_MODE:
        # MJCF 모드: C#에서 생성한 완전한 MJCF 파일 로드 (actuator, 시뮬레이션 설정 포함)
        if not os.path.exists(mjcf_file):
            raise FileNotFoundError(f"MJCF file not found: {mjcf_file}\nC#에서 먼저 URDF/MJCF를 생성해주세요.")

        # === Scene 객체 (<scene><include .../></scene>) 자동 import ===========
        # PyBullet 의 loadURDF 와 달리 MuJoCo 는 외부 URDF 직접 import 제한적
        # → 알려진 URL (cube, tray) 은 적절한 크기로, 미지 URL 은 기본 큐브 (4cm) 로 근사
        try:
            _scene_items_mj = _parse_scene_from_urdf_mj(urdf_file)
            if _scene_items_mj:
                mjcf_file_with_scene = preprocess_mjcf_for_scene(mjcf_file, _scene_items_mj)
                if mjcf_file_with_scene != mjcf_file:
                    mjcf_file = mjcf_file_with_scene
                    print(f"[Scene] {len(_scene_items_mj)} object(s) added to MJCF (box primitives)", flush=True)
        except Exception as _ex:
            print(f"[Scene] preprocess failed: {_ex}", flush=True)

        # 트랙 flow 애니메이션: MJCF 전처리 (정적 geom 제거 + mocap body 추가)
        mjcf_to_load = mjcf_file
        track_belts_mj = []
        if ENABLE_TRACK_FLOW:
            _preview_metas = _preview_tracks_meta_mj(release_dir)
            print(f"[Track] MJCF preview: {len(_preview_metas)} tracks", flush=True)
            if _preview_metas:
                mjcf_to_load, track_belts_mj = preprocess_mjcf_for_track_flow(mjcf_file, _preview_metas)

        print(f"\nLoading MJCF: {os.path.basename(mjcf_to_load)}")

        # C#에서 이미 free-floating/fixed 구조로 생성된 MJCF를 그대로 로드
        model = mujoco.MjModel.from_xml_path(mjcf_to_load)
        data = mujoco.MjData(model)

        # 지면 (plane geom) 색을 진한 파란색 (deep navy) 으로 — checkerboard 텍스처 대신 단일 컬러
        try:
            for _gi in range(model.ngeom):
                if model.geom_type[_gi] == mujoco.mjtGeom.mjGEOM_PLANE:
                    model.geom_rgba[_gi] = [0.08, 0.15, 0.45, 1.0]   # deep navy blue
                    model.geom_matid[_gi] = -1   # material 해제 → rgba 만 적용
        except Exception as _ex:
            print(f"Note: floor color set failed: {_ex}")

        # ── 초기 상태 깨끗하게 reset (qpos / qvel / ctrl / time 등) ──
        #    이 reset 을 SPAWN_Z / joint hold 보다 *앞* 에서 호출해야 그 설정이 살아남음.
        #    (이전 버그: 이 reset 이 SPAWN_Z 적용 *후* 에 호출되어 qpos[2] 가 wipe 되었음)
        mujoco.mj_resetData(model, data)

        # 로봇 base 의 초기 z = SPAWN_Z (fixed/non-fixed 모두 동일)
        #   free joint 가 있으면 qpos[0:3] = pos, qpos[3:7] = quat
        #   MJCF 의 base body pos 가 0 이므로 qpos[2] 만 SPAWN_Z 로 갱신하면 됨.
        try:
            if model.nq >= 3 and model.njnt > 0 and model.jnt_type[0] == mujoco.mjtJoint.mjJNT_FREE:
                data.qpos[2] = SPAWN_Z
                print(f"[OJW] free joint base spawn at z={SPAWN_Z}")
        except Exception as _ex:
            print(f"Note: free joint spawn z set failed: {_ex}")

        # ── 관절 강제 유지: actuator 의 gainprm/biasprm 강화 + ctrl = 현재 qpos ──
        #    BuildMjcf 가 position actuator 를 생성하므로 kp 만 크게 잡으면 강하게 유지.
        try:
            for _ai in range(model.nu):
                _jid = model.actuator_trnid[_ai, 0]
                if _jid < 0:
                    continue
                _qadr = model.jnt_qposadr[_jid]
                if 0 <= _qadr < len(data.qpos):
                    data.ctrl[_ai] = data.qpos[_qadr]
                # position actuator 의 stiffness 강화 (gainprm[0] = kp)
                model.actuator_gainprm[_ai, 0] = max(model.actuator_gainprm[_ai, 0], 500.0)
                # biasprm: [b0, b1, b2] = [setpoint_bias, -kp, -kv]; kp 와 맞춤
                model.actuator_biasprm[_ai, 1] = -max(abs(model.actuator_biasprm[_ai, 1]), 500.0)
            print(f"[OJW] hold gain applied to {model.nu} actuators")
        except Exception as _ex:
            print(f"Note: actuator hold setup failed: {_ex}")

        # SPAWN_Z + ctrl 설정을 mj_forward 로 일관된 상태로 전파
        mujoco.mj_forward(model, data)
        if USE_FIXED_BASE:
            print(f"Robot loaded successfully! (MJCF mode, fixed base)")
        else:
            print(f"Robot loaded in FREE-FLOATING mode (gravity active, will fall to ground)")

        # 트랙 휠 + 정적 트랙 마찰 튜닝
        if track_belts_mj:
            # JSON 으로부터 track mesh 이름 추출 (정적 트랙 geom 식별용)
            _track_mesh_names = set()
            for entry in _preview_metas:
                stl = entry.get("segment_stl", "")
                if stl:
                    _track_mesh_names.add(os.path.splitext(os.path.basename(stl))[0])
            boost_track_wheels_mujoco(model, _track_mesh_names)
    else:
        # URDF 모드: MuJoCo 내장 URDF 파서 사용 (actuator 없음, 기구학 전용)
        track_belts_mj = []  # URDF 모드에서는 트랙 flow 미지원
        if not os.path.exists(urdf_file):
            raise FileNotFoundError(f"URDF file not found: {urdf_file}\nC#에서 먼저 URDF를 생성해주세요.")

        print(f"\nLoading URDF: {os.path.basename(urdf_file)}")
        model = mujoco.MjModel.from_xml_path(urdf_file)
        data = mujoco.MjData(model)
        print(f"Robot loaded successfully! (URDF mode - no actuators, kinematics only)")

        # URDF 모드에서는 물리 시뮬레이션 비활성화 권장
        print("  Note: URDF mode has no actuators. Robot will fall due to gravity.")
        print("  Consider using USE_PHYSICS_SIMULATION = False for kinematics-only mode.")

    os.chdir(original_cwd)  # 원래 디렉토리로 복원

except Exception as e:
    os.chdir(original_cwd)  # 에러 시에도 복원
    print(f"Error loading MJCF: {e}")
    import traceback
    traceback.print_exc()
    sys.exit(1)

# 조인트 정보 수집
num_joints = model.njnt
print(f"Total joints: {num_joints}")
print(f"Total qpos (DOF): {model.nq}")

# 조인트 이름과 인덱스 매핑
for i in range(num_joints):
    joint_name = mujoco.mj_id2name(model, mujoco.mjtObj.mjOBJ_JOINT, i)
    if joint_name:
        joint_name_to_id[joint_name] = i

        # 조인트 타입 확인 (0: free, 1: ball, 2: slide, 3: hinge)
        joint_type = model.jnt_type[i]

        # hinge(revolute) 조인트만 제어 대상
        if joint_type == 3:  # hinge joint
            qpos_idx = model.jnt_qposadr[i]

            # mimic가 아닌 경우만 movable_joints에 추가
            if joint_name not in mimic_relationships:
                movable_joints.append(qpos_idx)

                # 조인트 번호 추출 및 매핑
                joint_num = extract_joint_number(joint_name)
                if joint_num is not None:
                    joint_number_to_qpos[joint_num] = qpos_idx

                joint_info[len(movable_joints)] = {
                    'id': qpos_idx,
                    'name': joint_name,
                    'type': 'hinge',
                    'number': joint_num
                }
                print(f"  [{joint_num if joint_num else '?'}] {joint_name} (qpos idx: {qpos_idx})")

# Mimic 관계 설정
if mimic_relationships:
    print(f"\nMimic joints found: {len(mimic_relationships)}")
    for child_name, mimic_data in mimic_relationships.items():
        if child_name in joint_name_to_id and mimic_data['parent'] in joint_name_to_id:
            child_jnt_id = joint_name_to_id[child_name]
            parent_jnt_id = joint_name_to_id[mimic_data['parent']]

            child_qpos_idx = model.jnt_qposadr[child_jnt_id]
            parent_qpos_idx = model.jnt_qposadr[parent_jnt_id]

            mimic_joints[child_name] = {
                'child_idx': child_qpos_idx,
                'parent_idx': parent_qpos_idx,
                'multiplier': mimic_data['multiplier'],
                'offset': mimic_data['offset']
            }
            print(f"  {child_name} -> {mimic_data['parent']} (x{mimic_data['multiplier']})")

# Actuator와 qpos 인덱스 매핑 설정
num_actuators = model.nu  # actuator 개수
print(f"\nActuators: {num_actuators}")

if num_actuators == 0:
    print("WARNING: No actuators found! Robot will fall due to gravity.")
    print("Check if actuator XML is properly included in MJCF.")
else:
    # Actuator gain을 높여서 응답 속도 개선
    # position actuator의 경우: gainprm[0] = kp, biasprm[1] = -kp
    # 기본값이 낮으면 조인트가 천천히 움직임
    ############################
    # print("Boosting actuator gains for faster response...")
    # for i in range(num_actuators):
    #     # 현재 gain 확인
    #     current_kp = model.actuator_gainprm[i, 0]
    #     if current_kp < 100:  # gain이 낮으면 높임
    #         new_kp = 500.0  # 높은 position gain
    #         model.actuator_gainprm[i, 0] = new_kp
    #         model.actuator_biasprm[i, 1] = -new_kp  # negative feedback
    #         model.actuator_biasprm[i, 2] = 0
    # print(f"  Actuator gains boosted to kp=500 for faster response")
    ############################
    pass

for i in range(num_actuators):
    actuator_name = mujoco.mj_id2name(model, mujoco.mjtObj.mjOBJ_ACTUATOR, i)
    if actuator_name:
        # actuator가 제어하는 joint 찾기
        trnid = model.actuator_trnid[i, 0]  # transmission id (joint id)

        if trnid >= 0 and trnid < model.njnt:
            qpos_idx = model.jnt_qposadr[trnid]
            actuator_to_qpos[i] = qpos_idx
            qpos_to_actuator[qpos_idx] = i
            joint_name = mujoco.mj_id2name(model, mujoco.mjtObj.mjOBJ_JOINT, trnid)
            print(f"  [{i}] {actuator_name} -> joint '{joint_name}' qpos[{qpos_idx}]")

# freejoint 인덱스 찾기 (free-floating 모드)
for _i in range(model.njnt):
    _jname = mujoco.mj_id2name(model, mujoco.mjtObj.mjOBJ_JOINT, _i)
    if _jname == 'root_freejoint':
        if model.jnt_type[_i] == 0:  # 0 = free joint
            free_floating_joint_qpos_addr = int(model.jnt_qposadr[_i])
            print(f"  root_freejoint found at qpos[{free_floating_joint_qpos_addr}]")
        break

# 초기 ctrl 값 설정 — SPAWN_Z 와 joint hold 는 이미 위쪽 MJCF 로드 직후에 적용됨.
# 여기서는 base z / ctrl reset 을 절대로 다시 하면 안 됨 (그러면 SPAWN_Z 가 wipe 됨).
# fixed base 모드일 때만 warmup 으로 초기 안정화.
if len(data.ctrl) > 0:
    print(f"\nActuator count: {len(data.ctrl)} (ctrl 은 위에서 current qpos 로 hold 설정됨)")

    if USE_FIXED_BASE:
        # 좌표고정 모드: warmup 으로 초기 위치 안정화 (ctrl = 현재 hold target 유지)
        print("Warmup phase: stabilizing robot at initial position...")
        for step in range(500):  # 500 스텝 = 1초 (timestep=0.002)
            mujoco.mj_step(model, data)
        print("Warmup complete!")
    else:
        # free-floating 모드: warmup 없이 순기구학만 계산 (중력으로 자연 낙하)
        mujoco.mj_forward(model, data)
        print("Free-floating mode: robot will fall under gravity (no warmup)")

# 사용법 안내 출력
print("\n" + "=" * 60)
print("Controls:")
print("  - MuJoCo Viewer UI: Use the control panel on the right side")
print("    to manually adjust joint angles with sliders")
print("-" * 60)
print("  Socket Control:")
print("  - Socket format: 'time,delay,joint1,angle1,joint2,angle2,...;'")
print("  - Example: '2000,0,11,45,12,-30;' (Joint 11→45°, Joint 12→-30°)")
print("  - Use Play() function: Play('2000,0,11,45,12,90')")
print("-" * 60)
print("  Python API:")
print("  - test()  : Run test sequence")
print("  - Play('time,delay,joint,angle,...;')")
print("  - WaitMotion() : Wait for all motions to complete")
print("=" * 60)

# 소켓 서버 스레드 시작
socket_thread = threading.Thread(target=socket_server_thread, daemon=True)
socket_thread.start()
print("Socket server thread started!")

print("\nSimulation running...")
print("Use the MuJoCo viewer's control panel to adjust joints manually.")

####################################################
# 메인 시뮬레이션 루프
####################################################

# 물리 시뮬레이션 사용 여부 (False: 기구학만 사용, 중력 영향 없음)
USE_PHYSICS_SIMULATION = True

####################################################
# 종동(passive) 조인트 퍼펫팅 — delta_passive.json (2026-08-25)
#   MakeUrdf Export 가 폐루프 종동 관절(델타 무릎 β/γ 등)을 무동력 dpass_* 조인트로
#   내보내고 각도 수식을 JSON 으로 동봉한다. 매 스텝 현재 모터각(T*)으로 수식을 평가해
#   qpos 를 직접 구동 → UI 슬라이더/소켓 어느 쪽으로 T 를 움직여도 폐루프가 유지된다.
#   q_pass_deg = (motor_deg + eval(formula, motors_deg)) - zero_bake_deg
####################################################
_delta_geo = [55.0, 100.0, 215.0, 24.0]  # deltainit 으로 갱신 (변 길이 규약)


def _delta_fk(m0, m1, m2):
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
                    n += 1; continue
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
            return None
        return result
    except Exception:
        return None


_passive_bindings = []   # [(qpos_adr, dof_adr, def)]
_motor_qpos_by_id = {}   # 모터ID → qpos_adr


def _init_passive_puppet():
    global _passive_bindings, _motor_qpos_by_id
    _passive_bindings = []
    _motor_qpos_by_id = {}
    _passive_last_mj.clear()
    path = os.path.join(release_dir, "delta_passive.json")
    if not os.path.exists(path):
        return
    try:
        with open(path, "r", encoding="utf-8") as fp:
            _pj = _json.load(fp)
            defs = _pj.get("passive_joints", [])
            _closures = _pj.get("closures", [])
    except Exception as ex:
        print(f"[Passive] delta_passive.json 로드 실패: {ex}")
        return
    # ★진짜 물리 폐루프 모드 (2026-08-26): MJCF 에 <connect>/<tendon> equality 가 저작되어
    #   있으면 (closures 존재 + MJCF 물리 모드) 물리가 폐루프를 유지한다 — 퍼펫 불필요.
    #   퍼펫 qpos 강제는 구속 솔버와 싸우므로 반드시 꺼야 한다.
    #   URDF/기구학 모드는 equality 가 없으므로 기존 퍼펫 유지.
    if _closures and USE_MJCF_MODE and USE_PHYSICS_SIMULATION:
        n_eq = sum(1 for i in range(model.neq)
                   if model.eq_type[i] in (mujoco.mjtEq.mjEQ_CONNECT, mujoco.mjtEq.mjEQ_TENDON))
        if n_eq > 0:
            print(f"[Closure] 물리 폐루프 모드: equality {n_eq}개 (connect+tendon) — 퍼펫 OFF")
            return
    for i in range(model.njnt):
        jn = mujoco.mj_id2name(model, mujoco.mjtObj.mjOBJ_JOINT, i)
        if jn and jn.startswith("T") and jn[1:].isdigit():
            _motor_qpos_by_id[int(jn[1:])] = int(model.jnt_qposadr[i])
    for pdef in defs:
        jn = pdef.get("joint")
        jid = mujoco.mj_name2id(model, mujoco.mjtObj.mjOBJ_JOINT, jn) if jn else -1
        if jid < 0:
            continue
        _passive_bindings.append((int(model.jnt_qposadr[jid]), int(model.jnt_dofadr[jid]), pdef))
    if _passive_bindings:
        print(f"[Passive] 퍼펫팅 활성: 종동 조인트 {len(_passive_bindings)}개, 모터 {sorted(_motor_qpos_by_id.keys())}")


_passive_last_mj = {}


def _apply_passive_puppet_mj():
    """매 스텝: 현재 모터각으로 종동 조인트 qpos 직접 구동 (+qvel 0 — 드리프트 방지)

    ★unwrap: β 수식의 atan2 경계에서 값이 360° 랩 — qpos 는 시각상 mod360 이지만
      ±π 관절 한계(limited)의 제약력이 스며들 수 있어 최근접 연속 표현으로 유지.
      모터 변화로 설명 안 되는 큰 점프(FK 팬텀 분기)는 1°/스텝 서행 (Isaac 러너와 동일)."""
    if not _passive_bindings:
        return
    motors = {}
    m_step = 0.0
    for mid, adr in _motor_qpos_by_id.items():
        v = float(data.qpos[adr]) * 180.0 / math.pi
        motors[mid] = v
        pv = _passive_last_mj.get(("m", mid))
        if pv is not None and abs(v - pv) > m_step:
            m_step = abs(v - pv)
        _passive_last_mj[("m", mid)] = v
    for qadr, dadr, pdef in _passive_bindings:
        f_val = _edh_eval_formula(pdef.get("formula", ""), motors)
        if f_val is None:
            continue
        m_deg = motors.get(pdef.get("motor_id", -1), 0.0)
        q_deg = (m_deg + f_val) - float(pdef.get("zero_bake_deg", 0.0))
        prev = _passive_last_mj.get(qadr)
        if prev is not None:
            d_un = ((q_deg - prev + 180.0) % 360.0) - 180.0
            rate = 1.0 if abs(d_un) > (5.0 + 3.0 * m_step) else 20.0
            if d_un > rate:
                d_un = rate
            elif d_un < -rate:
                d_un = -rate
            q_deg = prev + d_un
        _passive_last_mj[qadr] = q_deg
        data.qpos[qadr] = q_deg * math.pi / 180.0
        if 0 <= dadr < len(data.qvel):
            data.qvel[dadr] = 0.0


_init_passive_puppet()


def simulation_step():
    """시뮬레이션 스텝 실행"""
    global data, model

    # 소켓 명령 처리
    process_socket_commands()

    # 조인트 이동 업데이트 (active_moves가 있을 때만 ctrl 값 변경)
    update_joint_movements_kinematics() if not USE_PHYSICS_SIMULATION else update_joint_movements()

    # Mimic 조인트 업데이트
    update_mimic_joints_kinematics() if not USE_PHYSICS_SIMULATION else update_mimic_joints()

    if USE_PHYSICS_SIMULATION:
        # 물리 시뮬레이션 스텝 (중력, 충돌 등 적용)
        mujoco.mj_step(model, data)
    else:
        # 기구학 모드: UI 슬라이더(ctrl)를 qpos에 반영
        # active_moves가 없는 조인트만 ctrl → qpos 복사 (UI 슬라이더 제어 허용)
        for qpos_idx, ctrl_idx in qpos_to_actuator.items():
            if qpos_idx not in active_moves:
                if ctrl_idx < len(data.ctrl) and qpos_idx < len(data.qpos):
                    data.qpos[qpos_idx] = data.ctrl[ctrl_idx]

        # 기구학만 계산 (물리 시뮬레이션 없이 위치만 업데이트)
        mujoco.mj_forward(model, data)

    # 종동(passive) 조인트 퍼펫팅 — 스텝 후 최종 자세를 수식으로 강제 (폐루프 유지)
    _apply_passive_puppet_mj()

    # 트랙 벨트 flow 애니메이션 (mocap body 위치 갱신)
    if 'track_belts_mj' in globals() and track_belts_mj:
        update_track_belts_mujoco(model, data, track_belts_mj)
        # URDF 한계 우회: 차동 구동 → base 속도 직접 설정
        apply_kinematic_tank_drive(model, data, track_belts_mj)

# MuJoCo 뷰어 시작 (인터랙티브 모드)
try:
    with mujoco.viewer.launch_passive(model, data) as viewer:
        # 카메라 초기 위치 설정
        viewer.cam.distance = 1.0
        viewer.cam.azimuth = 45
        viewer.cam.elevation = -20
        viewer.cam.lookat[:] = [0, 0, 0.1]

        # 렌더링 옵션 설정
        viewer.opt.flags[mujoco.mjtVisFlag.mjVIS_LIGHT] = True      # 조명 표시
        viewer.opt.flags[mujoco.mjtVisFlag.mjVIS_TEXTURE] = True    # 텍스처 표시

        # 배경색 설정 (흰색/밝은 회색)
        try:
            if hasattr(viewer, 'scn'):
                viewer.scn.flags[mujoco.mjtRndFlag.mjRND_SKYBOX] = True
                viewer.scn.flags[mujoco.mjtRndFlag.mjRND_FOG] = False

            if hasattr(model, 'vis'):
                model.vis.rgba.fog[:] = [1.0, 1.0, 1.0, 1.0]
                model.vis.rgba.haze[:] = [1.0, 1.0, 1.0, 1.0]
                model.vis.headlight.ambient[:] = [0.8, 0.8, 0.8]
                model.vis.headlight.diffuse[:] = [1.0, 1.0, 1.0]
                model.vis.headlight.specular[:] = [0.3, 0.3, 0.3]
                model.vis.headlight.active = 1

            print("Background set to white/light gray")
        except Exception as ex:
            print(f"Note: Could not configure background: {ex}")

        print("Press 'r' in console to run test(), 'c' to clear moves, 'q' to quit")

        while viewer.is_running() and simulation_running:
            step_start = time.time()

            # 자동 종료 (캡쳐 모드)
            if _ojw_auto_exit_start is not None and (step_start - _ojw_auto_exit_start) >= _ojw_auto_exit_sec:
                print(f"[OJW] AUTO_EXIT reached ({_ojw_auto_exit_sec}s) — exiting", flush=True)
                simulation_running = False
                break

            # Windows 키보드 입력 처리 (비블로킹)
            if sys.platform == 'win32' and msvcrt.kbhit():
                key = msvcrt.getch().decode('utf-8', errors='ignore').lower()
                if key == 'r':
                    print("\n[Key 'r'] Running test()...")
                    threading.Thread(target=test, daemon=True).start()
                elif key == 'c':
                    active_moves.clear()
                    last_ctrl_values.clear()
                    print("\n[Key 'c'] Active moves cleared")
                elif key == 'q':
                    print("\n[Key 'q'] Quitting...")
                    break

            # 시뮬레이션 스텝 실행
            simulation_step()

            # 뷰어 동기화
            viewer.sync()

            # 실시간 시뮬레이션을 위한 타이밍 조절
            time_until_next_step = model.opt.timestep - (time.time() - step_start)
            if time_until_next_step > 0:
                time.sleep(time_until_next_step)

except KeyboardInterrupt:
    print("\nInterrupted by user")
except Exception as e:
    print(f"\nError during simulation: {e}")
    import traceback
    traceback.print_exc()

# 시뮬레이션 종료 처리
simulation_running = False
print("\nShutting down...")
print("Program terminated.")
