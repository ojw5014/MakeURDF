import pybullet as p
import pybullet_data
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
print(f"Fixed Base: {'Enabled' if USE_FIXED_BASE else 'Disabled'}")

print("\n" + "=" * 60)
print(" PyBullet URDF Viewer with Joint Sliders & Socket Control")
print("=" * 60)
print(f"Loading: {urdf_file}")

####################################################
# 테스트용 큐브들 배치
cube_positions = [
    [0.1, 0.2, 0.05],   # 큐브 1 위치
    [0.2, 0.1, 0.05],   # 큐브 2 위치
    [0.15, 0.3, 0.05]   # 큐브 3 위치
]

# 큐브 색상 팔레트
colors = [
    [1, 0, 0, 1],      # 빨간색
    [0, 1, 0, 1],      # 초록색
    [0, 0, 1, 1],      # 파란색
    [1, 1, 0, 1],      # 노란색
    [1, 0, 1, 1],      # 자홍색
    [0, 1, 1, 1],      # 청록색
    [1, 0.5, 0, 1],    # 주황색
    [0.5, 0, 1, 1],    # 보라색
]

# 큐브들 생성 및 색상 적용
nColorIndex = 0
for pos in cube_positions:
    cube_id = p.loadURDF("cube_small.urdf", pos)
    p.changeVisualShape(cube_id, -1, rgbaColor=colors[nColorIndex])
    nColorIndex += 1

# 바구니 배치 (작업 공간 시각화용)
tray_id = p.loadURDF("tray/tray.urdf", [0.05, -0.2, 0], globalScaling=0.33)
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
command_queue = queue.Queue()          # 소켓 명령 큐
active_moves: Dict[int, JointMoveCommand] = {}  # 현재 실행 중인 조인트 이동 명령들
joint_number_to_id = {}                # 조인트 번호(예: 11, 12) → PyBullet joint ID 매핑

# 로봇 위치 제어 변수들
robot_position_sliders = {}            # 로봇 위치 제어 슬라이더들
initial_robot_position = [0, 0, 0.5]  # 로봇 초기 위치
current_robot_position = [0, 0, 0.5]  # 현재 로봇 위치

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
    """명령 문자열을 받아서 실행 큐에 추가하는 함수"""
    try:
        # 메시지 파싱
        parsed = parse_socket_message(command_string)
        
        if parsed:
            # 명령 큐에 추가
            command_queue.put(parsed)
            return True
        else:
            return False
            
    except Exception as e:
        print(f"Play exception: {e}")
        return False

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
    """조인트를 지정된 각도로 부드럽게 이동시키는 명령 생성"""
    
    # 유효한 조인트 인덱스인지 확인
    if joint_id >= len(movable_joints):
        print(f"Invalid joint index: {joint_id} (max: {len(movable_joints)-1})")
        return False
    
    actual_joint_id = movable_joints[joint_id]
    
    # 현재 위치 가져오기 (라디안 -> 도로 변환)
    current_pos_rad = get_joint_position(robot_id, actual_joint_id)
    current_pos_deg = math.degrees(current_pos_rad)
    
    # 목표 위치 (라디안으로 변환)
    target_rad = math.radians(target_degrees)
    
    # 조인트 한계값 확인 및 적용
    joint_info = p.getJointInfo(robot_id, actual_joint_id)
    lower_limit = joint_info[8]
    upper_limit = joint_info[9]
    
    if lower_limit > -1e10 and upper_limit < 1e10:
        target_rad = max(lower_limit, min(upper_limit, target_rad))
        target_degrees = math.degrees(target_rad)
    
    # 이동 명령 객체 생성
    move_command = JointMoveCommand(
        joint_id=actual_joint_id,
        target_angle=target_degrees,
        duration=duration_ms / 1000.0,  # 밀리초를 초로 변환
        start_time=time.time(),
        start_angle=current_pos_deg
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
        
        # 조인트 위치 설정
        target_rad = math.radians(current_angle)
        p.setJointMotorControl2(
            robot_id,
            joint_id,
            p.POSITION_CONTROL,
            targetPosition=target_rad,
            force=1000
        )
        
        # Mimic 조인트 업데이트 (부모 조인트가 움직일 때 자식들도 함께 움직임)
        update_mimic_joints_for_parent(joint_id, target_rad)
        
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
# 메인 실행 부분
####################################################

# Mimic 정보 파싱
mimic_relationships = parse_mimic_joints(urdf_file)

# URDF에서 링크별 색상 정보 파싱
link_colors = parse_link_visual_colors(urdf_file)
print(f"Parsed {len(link_colors)} link colors from URDF")

# 로봇 URDF 로드 (C#에서 이미 fixed/free-floating 구조로 생성된 파일을 그대로 사용)
if not USE_FIXED_BASE:
    robot = p.loadURDF(urdf_file, [0, 0, 0.4], useFixedBase=False)
else:
    robot = p.loadURDF(urdf_file, initial_robot_position, useFixedBase=True)

# 조인트 정보 수집을 위한 변수들 초기화
num_joints = p.getNumJoints(robot)
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
    if joint_type in [p.JOINT_REVOLUTE, p.JOINT_PRISMATIC] and i not in mimic_joints:
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
            # 바퀴형 연속 회전 조인트 — 속도 제어 (deg/s)
            slider_min = -360.0
            slider_max = 360.0
            slider_label = f"{joint_name} [deg/s]"
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
        
        # 조인트 이동 업데이트 (소켓 명령으로 인한 부드러운 이동)
        update_joint_movements(robot)
        
        # 슬라이더 값 읽기 및 조인트 제어 (수동 제어)
        for joint_idx, slider_data in sliders.items():
            try:
                slider_value = p.readUserDebugParameter(slider_data['id'])
                
                # 조인트 타입에 따른 값 변환
                if slider_data.get('is_continuous', False):
                    # 바퀴: 슬라이더 값은 deg/s → rad/s 변환
                    target_vel = math.radians(slider_value)
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
                                force=1000
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
        
        # 물리 시뮬레이션 한 스텝 실행
        p.stepSimulation()
        time.sleep(1./240.)  # 240 FPS로 제한
        
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
p.disconnect()
print("Program terminated.")
