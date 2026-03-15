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
command_queue = queue.Queue()          # 소켓 명령 큐
active_moves: Dict[int, JointMoveCommand] = {}  # 현재 실행 중인 조인트 이동 명령들
last_ctrl_values: Dict[int, float] = {}  # 마지막으로 설정한 ctrl 값 (UI 변경 감지용)

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
    """viewer_config.txt 파일에서 설정 읽기"""
    config = {
        'FIXED_BASE': True,  # 기본값: 고정
        'USE_PYBULLET': True,
        'USE_MUJOCO': False
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
                    if key in config:
                        config[key] = (value == '1')
            print(f"Config loaded from: {config_file}")
        except Exception as e:
            print(f"Warning: Could not read config file: {e}")
    else:
        print(f"Note: Config file not found, using defaults")
    return config

viewer_config = read_viewer_config()
USE_FIXED_BASE = viewer_config['FIXED_BASE']
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
    global active_moves, last_ctrl_values

    try:
        cmd = command_string.strip().rstrip(';').lower()

        # 특수 명령 처리
        if cmd == 'clear':
            active_moves.clear()
            last_ctrl_values.clear()
            print("Active moves cleared - UI control restored")
            return True
        elif cmd == 'reset':
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
    """조인트를 지정된 각도로 부드럽게 이동시키는 명령 생성

    Args:
        joint_number: 조인트 번호 (예: 11, 12 - URDF의 T11, T12에 해당)
        target_degrees: 목표 각도 (도 단위)
        duration_ms: 이동 시간 (밀리초), 0이면 즉시 이동
    """
    global data, joint_number_to_qpos, active_moves, qpos_to_actuator

    # 조인트 번호로 qpos 인덱스 찾기
    if joint_number not in joint_number_to_qpos:
        print(f"Invalid joint number: {joint_number} (available: {sorted(joint_number_to_qpos.keys())})")
        return False

    qpos_idx = joint_number_to_qpos[joint_number]

    # 목표 위치 (라디안으로 변환)
    target_rad = math.radians(target_degrees)

    # time=0이면 즉시 이동 (보간 없음)
    if duration_ms <= 0:
        # 해당 조인트의 active_move가 있으면 삭제
        if qpos_idx in active_moves:
            del active_moves[qpos_idx]

        # ctrl에 즉시 목표값 설정
        if qpos_idx in qpos_to_actuator:
            ctrl_idx = qpos_to_actuator[qpos_idx]
            if ctrl_idx < len(data.ctrl):
                data.ctrl[ctrl_idx] = target_rad
        return True

    # 현재 위치 가져오기
    current_pos_rad = get_joint_position(qpos_idx)

    # 이동 명령 객체 생성
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

# URDF에서 조인트 정보 파싱
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

        print(f"\nLoading MJCF: {os.path.basename(mjcf_file)}")

        # C#에서 이미 free-floating/fixed 구조로 생성된 MJCF를 그대로 로드
        model = mujoco.MjModel.from_xml_path(mjcf_file)
        data = mujoco.MjData(model)
        if USE_FIXED_BASE:
            print(f"Robot loaded successfully! (MJCF mode, fixed base)")
        else:
            print(f"Robot loaded in FREE-FLOATING mode (gravity active, will fall to ground)")
    else:
        # URDF 모드: MuJoCo 내장 URDF 파서 사용 (actuator 없음, 기구학 전용)
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

# 초기 ctrl 값 설정 (모든 관절을 0도 위치로 유지)
if len(data.ctrl) > 0:
    print(f"\nInitializing ctrl values ({len(data.ctrl)} actuators)...")

    # MuJoCo 표준 초기화 (freejoint 쿼터니언도 올바르게 처리)
    mujoco.mj_resetData(model, data)

    # 모든 actuator의 목표 위치를 0으로 설정
    for ctrl_idx in range(len(data.ctrl)):
        data.ctrl[ctrl_idx] = 0.0

    if USE_FIXED_BASE:
        # 좌표고정 모드: warmup으로 초기 위치 안정화
        print("Warmup phase: stabilizing robot at initial position...")
        for step in range(500):  # 500 스텝 = 1초 (timestep=0.002)
            for ctrl_idx in range(len(data.ctrl)):
                data.ctrl[ctrl_idx] = 0.0
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
