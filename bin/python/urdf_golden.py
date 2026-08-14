# -*- coding: utf-8 -*-
"""URDF 골든값 회귀검사 — "합격증 없는 모델은 배포하지 않는다"의 최소 구현.

새 export 가 직전 인증본과 물리적으로 같은 로봇인지 자동 판정한다.
humanoid 프로젝트 제안 §6-4 (sim2real_동정하네스_제안.md) 의 골든값 표를 구현.

사용법:
    python urdf_golden.py emit  robot.urdf  golden.json      # 기준값 발행
    python urdf_golden.py check robot.urdf  golden.json      # 검사 (회귀 시 exit 1)

검사 항목 (의존성 없음 — 표준 라이브러리만):
    관절 수·이름          완전 일치
    관절 축·원점          완전 일치 (1e-6)
    관절 한계             완전 일치 (1e-6)   ★±180° 플레이스홀더 복귀는 별도 치명 경고
    총질량                ±2 %
    영자세 CoM            ±2 mm
    effort/velocity       플레이스홀더(100/2.0) 잔존 탐지

왜 필요한가: 관절 한계가 ±180° 플레이스홀더로 되돌아가면 시뮬은 실물이 못 하는 자세로
자기를 구제할 수 있어 "시뮬만 통과"가 발생한다. humanoid 프로젝트가 이 회귀로 무릎 역굴곡·
발날 딛기 재학습을 2회 치렀고, OpenJigWare 쪽도 같은 URDF 로 같은 함정을 밟았다.
"""
from __future__ import print_function

import io
import json
import math
import os
import sys
import xml.etree.ElementTree as ET

PLACEHOLDER_LIMIT = math.pi          # ±180° = 미측정 플레이스홀더
PLACEHOLDER_EFFORT = 100.0
PLACEHOLDER_VELOCITY = 2.0
TOL_GEOM = 1e-6
TOL_MASS_PCT = 2.0
TOL_COM_MM = 2.0


# ── 회전/변환 유틸 (URDF rpy = 고정축 X-Y-Z) ────────────────────────────────
def rpy_to_mat(r, p, y):
    cr, sr = math.cos(r), math.sin(r)
    cp, sp = math.cos(p), math.sin(p)
    cy, sy = math.cos(y), math.sin(y)
    return [
        [cy * cp, cy * sp * sr - sy * cr, cy * sp * cr + sy * sr],
        [sy * cp, sy * sp * sr + cy * cr, sy * sp * cr - cy * sr],
        [-sp,     cp * sr,                cp * cr],
    ]


def mat_mul(a, b):
    return [[sum(a[i][k] * b[k][j] for k in range(3)) for j in range(3)] for i in range(3)]


def mat_vec(m, v):
    return [sum(m[i][k] * v[k] for k in range(3)) for i in range(3)]


def parse_origin(el):
    xyz, rpy = [0.0, 0.0, 0.0], [0.0, 0.0, 0.0]
    if el is None:
        return xyz, rpy
    o = el.find("origin")
    if o is None:
        return xyz, rpy
    if o.get("xyz"):
        xyz = [float(v) for v in o.get("xyz").split()]
    if o.get("rpy"):
        rpy = [float(v) for v in o.get("rpy").split()]
    return xyz, rpy


# ── URDF → 골든 지표 ────────────────────────────────────────────────────────
def extract(urdf_path):
    root = ET.parse(urdf_path).getroot()

    links = {}
    for lk in root.findall("link"):
        name = lk.get("name")
        inertial = lk.find("inertial")
        mass = 0.0
        com = [0.0, 0.0, 0.0]
        if inertial is not None:
            m = inertial.find("mass")
            if m is not None and m.get("value"):
                mass = float(m.get("value"))
            com, _ = parse_origin(inertial)
        links[name] = {"mass": mass, "com": com}

    joints = {}
    children = {}
    for jt in root.findall("joint"):
        name = jt.get("name")
        jtype = jt.get("type", "")
        parent = jt.find("parent").get("link") if jt.find("parent") is not None else None
        child = jt.find("child").get("link") if jt.find("child") is not None else None
        xyz, rpy = parse_origin(jt)
        axis = [0.0, 0.0, 1.0]
        ax = jt.find("axis")
        if ax is not None and ax.get("xyz"):
            axis = [float(v) for v in ax.get("xyz").split()]
        lim = jt.find("limit")
        lower = upper = effort = velocity = None
        if lim is not None:
            for k in ("lower", "upper", "effort", "velocity"):
                if lim.get(k) is not None:
                    v = float(lim.get(k))
                    if k == "lower":
                        lower = v
                    elif k == "upper":
                        upper = v
                    elif k == "effort":
                        effort = v
                    else:
                        velocity = v
        joints[name] = {"type": jtype, "parent": parent, "child": child,
                        "xyz": xyz, "rpy": rpy, "axis": axis,
                        "lower": lower, "upper": upper,
                        "effort": effort, "velocity": velocity}
        children[child] = name

    # 영자세 FK (관절각 0) → 링크 world 원점 → 총질량·CoM
    roots = [n for n in links if n not in children]
    pose = {}
    for r in roots:
        pose[r] = ([0.0, 0.0, 0.0], [[1, 0, 0], [0, 1, 0], [0, 0, 1]])
    changed = True
    while changed:
        changed = False
        for jn, j in joints.items():
            if j["parent"] in pose and j["child"] not in pose:
                pp, pR = pose[j["parent"]]
                R = mat_mul(pR, rpy_to_mat(*j["rpy"]))
                t = mat_vec(pR, j["xyz"])
                pose[j["child"]] = ([pp[i] + t[i] for i in range(3)], R)
                changed = True

    total_mass = 0.0
    acc = [0.0, 0.0, 0.0]
    missing = []
    for ln, lk in links.items():
        if lk["mass"] <= 0:
            continue
        if ln not in pose:
            missing.append(ln)
            continue
        p, R = pose[ln]
        c = mat_vec(R, lk["com"])
        for i in range(3):
            acc[i] += lk["mass"] * (p[i] + c[i])
        total_mass += lk["mass"]
    com = [a / total_mass for a in acc] if total_mass > 0 else [0.0, 0.0, 0.0]

    return {
        "joint_names": sorted(joints.keys()),
        "joints": joints,
        "total_mass": total_mass,
        "com": com,
        "unreachable_links": sorted(missing),
        "link_count": len(links),
    }


def to_golden(d):
    """비교용 직렬화 (joints 는 필요한 필드만)"""
    js = {}
    for n, j in d["joints"].items():
        js[n] = {"type": j["type"], "parent": j["parent"], "child": j["child"],
                 "xyz": j["xyz"], "rpy": j["rpy"], "axis": j["axis"],
                 "lower": j["lower"], "upper": j["upper"],
                 "effort": j["effort"], "velocity": j["velocity"]}
    return {"joint_names": d["joint_names"], "joints": js,
            "total_mass": d["total_mass"], "com": d["com"],
            "link_count": d["link_count"]}


# ── 검사 ────────────────────────────────────────────────────────────────────
def near(a, b, tol=TOL_GEOM):
    if a is None and b is None:
        return True
    if a is None or b is None:
        return False
    return abs(a - b) <= tol


def vec_near(a, b, tol=TOL_GEOM):
    return len(a) == len(b) and all(near(a[i], b[i], tol) for i in range(len(a)))


def check(cur, gold):
    fails, warns = [], []

    # 1) 관절 수·이름
    if cur["joint_names"] != gold["joint_names"]:
        only_new = sorted(set(cur["joint_names"]) - set(gold["joint_names"]))
        only_old = sorted(set(gold["joint_names"]) - set(cur["joint_names"]))
        fails.append("관절 집합 불일치 — 추가 %s / 누락 %s" % (only_new or "없음", only_old or "없음"))
        return fails, warns   # 이후 비교 무의미

    for jn in cur["joint_names"]:
        c, g = cur["joints"][jn], gold["joints"][jn]

        # 2) 축·원점 — 다르면 부호맵·영점 캘리가 무효가 된다
        if not vec_near(c["axis"], g["axis"]):
            fails.append("%s 축 변경 %s → %s  ★부호맵/영점 캘리 무효 — 재캘리 필요" % (jn, g["axis"], c["axis"]))
        if not vec_near(c["xyz"], g["xyz"]) or not vec_near(c["rpy"], g["rpy"]):
            fails.append("%s 원점 변경 xyz %s→%s rpy %s→%s" % (jn, g["xyz"], c["xyz"], g["rpy"], c["rpy"]))
        if c["type"] != g["type"]:
            fails.append("%s 타입 변경 %s → %s" % (jn, g["type"], c["type"]))

        # 3) 관절 한계 — ★±180° 플레이스홀더 복귀는 치명
        if not near(c["lower"], g["lower"]) or not near(c["upper"], g["upper"]):
            was_real = (g["lower"] is not None and
                        abs(abs(g["lower"]) - PLACEHOLDER_LIMIT) > 1e-4)
            now_ph = (c["lower"] is not None and
                      abs(abs(c["lower"]) - PLACEHOLDER_LIMIT) < 1e-4 and
                      abs(abs(c["upper"]) - PLACEHOLDER_LIMIT) < 1e-4)
            if was_real and now_ph:
                fails.append("%s ★관절한계가 ±180도 플레이스홀더로 회귀 (%.3f~%.3f → ±pi) "
                             "— 시뮬이 실물 불가능 자세로 자기를 구제하게 됨. $jointrange 확인"
                             % (jn, g["lower"], g["upper"]))
            else:
                fails.append("%s 관절한계 변경 %s~%s → %s~%s"
                             % (jn, g["lower"], g["upper"], c["lower"], c["upper"]))

        # 4) effort/velocity 플레이스홀더 잔존
        if c["type"] in ("revolute", "prismatic"):
            if near(c["effort"], PLACEHOLDER_EFFORT, 1e-3) and near(c["velocity"], PLACEHOLDER_VELOCITY, 1e-3):
                warns.append("%s effort/velocity 가 플레이스홀더(100/2.0) — $servo 기종 지정 누락?" % jn)

    # 5) 총질량 ±2 %
    if gold["total_mass"] > 0:
        dm = abs(cur["total_mass"] - gold["total_mass"]) / gold["total_mass"] * 100.0
        if dm > TOL_MASS_PCT:
            fails.append("총질량 %.4f → %.4f kg (%+.1f %%, 허용 ±%.0f %%)"
                         % (gold["total_mass"], cur["total_mass"], dm, TOL_MASS_PCT))

    # 6) 영자세 CoM ±2 mm
    dc = [(cur["com"][i] - gold["com"][i]) * 1000.0 for i in range(3)]
    dist = math.sqrt(sum(v * v for v in dc))
    if dist > TOL_COM_MM:
        fails.append("영자세 CoM %.1f mm 이동 (dx %.1f dy %.1f dz %.1f, 허용 %.0f mm) "
                     "— 평형 트림각이 바뀌므로 실기 재캘리 필요" % (dist, dc[0], dc[1], dc[2], TOL_COM_MM))

    return fails, warns


def main():
    if len(sys.argv) < 4 or sys.argv[1] not in ("emit", "check"):
        print(__doc__)
        return 2
    mode, urdf, goldf = sys.argv[1], sys.argv[2], sys.argv[3]
    if not os.path.exists(urdf):
        print("URDF 없음: %s" % urdf)
        return 2

    cur = extract(urdf)
    if cur["unreachable_links"]:
        print("경고: FK 로 도달 못한 링크 %d개 (질량 합산 제외): %s"
              % (len(cur["unreachable_links"]), ", ".join(cur["unreachable_links"][:5])))

    if mode == "emit":
        with io.open(goldf, "w", encoding="utf-8") as f:
            f.write(json.dumps(to_golden(cur), indent=1, ensure_ascii=False))
        print("골든값 발행: %s" % goldf)
        print("  관절 %d개 / 링크 %d개 / 총질량 %.4f kg / CoM (%.1f, %.1f, %.1f) mm"
              % (len(cur["joint_names"]), cur["link_count"], cur["total_mass"],
                 cur["com"][0] * 1000, cur["com"][1] * 1000, cur["com"][2] * 1000))
        n_ph = sum(1 for j in cur["joints"].values()
                   if j["lower"] is not None and abs(abs(j["lower"]) - PLACEHOLDER_LIMIT) < 1e-4)
        if n_ph:
            print("  ※ 주의: 관절 %d개가 아직 ±180도 플레이스홀더입니다 — 기구한계 측정 후 $jointrange 로 채우세요." % n_ph)
        return 0

    if not os.path.exists(goldf):
        print("골든값 없음: %s  (먼저 emit 하세요)" % goldf)
        return 2
    with io.open(goldf, encoding="utf-8") as f:
        gold = json.load(f)

    fails, warns = check(cur, gold)
    print("골든값 검사: %s vs %s" % (os.path.basename(urdf), os.path.basename(goldf)))
    print("  관절 %d / 총질량 %.4f kg / CoM (%.1f, %.1f, %.1f) mm"
          % (len(cur["joint_names"]), cur["total_mass"],
             cur["com"][0] * 1000, cur["com"][1] * 1000, cur["com"][2] * 1000))
    for w in warns:
        print("  [경고] %s" % w)
    if fails:
        print("  ★불합격 %d건:" % len(fails))
        for f_ in fails:
            print("    - %s" % f_)
        return 1
    print("  합격 — 직전 인증본과 물리적으로 동일한 로봇입니다.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
