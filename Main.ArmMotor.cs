// ================================================================
// 로봇팔 세팅 탭 — 연결 줄·모터 세팅 UI 껍데기 (2026-10-02)
//   본체는 DLL Ojw.CArmConn_t / CArmMotorSet_t (COjw_44_ArmMotor.cs).
//   여기서는 ① 표·버튼 상태 ② 확인 대화 ③ 배경 스레드로 DLL 호출 ④ 결과 표시만 한다.
//   수치 계산·검증 규칙·파일 형식·버스 접근은 넣지 않는다 (전부 DLL).
//   컨트롤은 Main.Designer.cs 에 있다 (tabArmSetup → grpArmConn, tabArmInner → tabArmMotor).
//
//   모터 세팅 파일
//   · 저장은 디자이너 파일(.ojw)만 — [저장] 할 때마다 위치를 고른다 (기본 이름 motors_<축서명>.ojw, 처음 폴더 = 마지막으로 쓴 폴더 또는 urdf\armsetup).
//     .ojw 에는 지금 3D 모델·DH 와 표의 모터 정보가 함께 들어간다 (디자이너에서 바로 열 수 있다). 방향 확인 체크는 칸이 없어 남지 않는다.
//   · 축 서명별 마지막 .ojw 경로는 urdf\armsetup\ojw_paths.txt 에 남는다 — 로봇이 바뀌면 그 파일을 저절로 불러온다
//     (없으면 예전 motors_<축서명>.txt 를 읽기만 하고, 그것도 없으면 기본값). txt 는 더 이상 쓰지 않는다.
//
//   스레드
//   · 연결·해제·읽기·토크·조그·영점·(연결 중) 세팅 적용은 버스를 쓰는 블로킹 호출 — ArmMotor_Run 이 배경 스레드에서 부른다.
//   · 연결 객체의 StateChanged·로그는 작업 스레드에서 올 수 있다 — BeginInvoke 로만 화면에 넘긴다 (Invoke 로 기다리지 않는다).
//   · 따라가기(FollowOnce)는 UI 타이머에서 — 표 칸에 아직 커밋 안 된 입력이 있거나 커밋이 줄 서 있거나 이 탭의 확인창이 떠 있으면 건너뛴다.
//     읽기 안에서 DoEvents 를 돌므로 그사이 끼어든 UI 이벤트가 같은 스레드에서 연결 잠금 함수를 부르면 "사용 중: 따라가기" 로 거부된다
//     → 그 안에서 들어온 표 커밋·(버스 안 쓰는) 세팅 적용·창 닫기는 ArmMotor_AfterFollow 로 읽기가 끝난 뒤에 한다.
//   · 창 닫기·예제 바꾸기 때의 연결 해제만 UI 스레드에서 바로 부른다 (따라가기를 먼저 멈춘다 — 토크 끄기·포트 닫기뿐인 짧은 동작).
//   · 3D 모션 표시(MoveJoints_IsMoving)는 DLL 의 대기 루프(Play·PlayXyz)가 진행시킨다 — 그 루프를 도는 스레드(조그·측정 작업·⑨)가 다 끝났는데
//     표시가 남았으면(모션 스텝이 예외로 끝남) ArmMotor_ClearStaleMotion 이 지운다 (그대로 두면 영영 '사용 중' — 연결 해제·창 닫기가 막힌다).
//
//   처짐 보정과 조그 (10-03 다시 봄 — DLL 의 보정 전환이 출력 틱 기준이 됨)
//   · 관절 모델이 켜져 있으면 실물로 보내는 모션 틱마다(SendPose) 보정이 얹히고, 켜거나 끄거나 모델을 바꾸면 1초에 걸쳐 넘어간다.
//     그 1초는 출력 틱이 흐른 시간이다 (틱 하나에 60ms 까지 — 쉬는 동안은 멈춤). 그래서 조그 사이에 쉬어도 다음 조그 첫 틱이 튀지 않고
//     (보정 켬, 3° 조그-2초 쉼-조그 첫 틱 0.2° ≤ 4틱 — 예전 벽시계 전환은 2.4°), 토크를 붙잡은 채 보정을 끄거나 모델을 바꿔도(⑥ 적용)
//     다음 틱은 걸려 있던 보정 그대로 이어진다 (튐 0 — 예전 2.3°). 토크를 붙잡은 서보를 이어받을 때(연결·토크 켜기) 보정이 걸려 있었으면 DLL 이
//     3D = 서보 목표로 두고 보정을 페이드 없이 끈다 (CArmConn_t.ADOPT_COMP_OFF — 다시 켜면 다음 동작의 출력 틱 1초에 걸쳐 들어온다).
//     → 토크 ON 동안 처짐 보정 켬/끔·학습 파일 다시 읽기·⑥ 적용을 막던 임시 막음과 '토크 ON 중 보정 바뀜' 표시는 없앴다.
//   · 남긴 막음 하나: 보정이 켜져 있거나 출력 단에 아직 빠지지 않은 보정이 있으면(SagComp_HasOffsets — 끈 뒤 남은 것, 손목 유지)
//     조그를 하지 않는다 (ArmMotor_CompBlockReason). 보정이 들어오거나 빠지는 1초 동안은 조그의 틱마다 그 일부가 얹혀 고르지 않은 관절도
//     움직인다 (3° 조그 하나에 ID 12 약 1°) — 방향 확인 조그는 고른 관절만 움직여야 한다. 출력 단 상태로 보므로 그 보정이 어디서 왔는지
//     (⑨·작업·3D 만 작업·쌍둥이 계산) 가리지 않는다.
//   · 보정을 끄는 것은 언제든 된다 (⑦). 토크를 켠 뒤 아직 움직이지 않았으면 남는 것이 없어 바로 조그할 수 있고, 이미 걸려 있던 보정은
//     다음 동작의 출력 틱에 걸쳐 빠진다 — 그때까지는 막히고, 토크를 껐다 켜면 지워진다.
// ================================================================
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using OpenJigWare;

namespace MakeUrdf
{
    public partial class Main
    {
        // ── 상태 ──
        private Ojw.CArmConn_t m_ArmConn = null;          // 실물 연결 하나 (이 탭·측정 작업·⑨ 공용)
        private Ojw.CArmMotorSet_t m_ArmEdit = null;      // 표에 묶인 편집본 (지금 로봇)
        private Ojw.CArmMotorSet_t m_ArmSaved = null;     // 저장 기준 (마지막으로 저장·불러온 파일, 파일이 없으면 기본값) — '저장 안 된 수정' 비교용
        private int[] m_anArmAxes = new int[0];           // 지금 EDH 의 모터 축 (MakeAll 마다)
        private string m_strArmFile = null;               // 지금 세트의 파일 (null = 아직 파일 없음)
        private string m_strArmMsg = "";                  // 마지막 알림 한 줄 (lblArmFile 둘째 줄, ★ = 거부·경고)
        private bool m_bArmDirty = false;                 // 저장 안 된 수정 (편집본 ≠ 저장 기준)
        private bool m_bArmUiSync = false;                // 표·체크를 코드로 바꾸는 중 (이벤트 무시)
        private bool m_bArmFollowBusy = false;            // 따라가기 Tick 재진입 막기
        private bool m_bArmModal = false;                 // 이 탭의 확인창·파일 대화가 떠 있음 (따라가기 건너뜀)
        private bool m_bArmCommitPending = false;         // 표 칸 커밋 처리가 줄 서 있음 (그동안 표를 다시 그리지 않고 따라가기도 건너뛴다)
        private readonly List<Action> m_lstArmAfterFollow = new List<Action>();   // 따라가기 읽기(DoEvents) 안에서 들어와 읽기가 끝난 뒤로 미룬 일
        private bool m_bArmApplyPending = false;          // 로봇이 바뀌었는데 연결이 사용 중이라 세팅 적용을 미룸 (비면 다시)
        private int m_nArmFollowTick = 0;                 // 따라가기 — 현재각 열은 3틱에 한 번
        private volatile bool m_bArmMotorRun = false;     // 이 탭 동작(연결·읽기·조그 …)의 배경 스레드가 도는 중
        private volatile bool m_bArmUiQueued = false;     // StateChanged → 화면 갱신이 이미 줄 서 있음
        private Font m_fntArmStateBold = null;            // lblArmState — 토크 ON 굵게
        private Font m_fntArmStateNormal = null;

        // 행 색 (상태 → 색만 앱이 정한다)
        private static readonly Color ARM_ROW_RED = Color.FromArgb(255, 214, 214);       // 무응답·리밋 밖
        private static readonly Color ARM_ROW_YELLOW = Color.FromArgb(255, 246, 196);    // Drive Mode·Homing·기종 다름 등 경고
        private static readonly Color ARM_ROW_BLUE = Color.FromArgb(214, 232, 255);      // 저장 안 된 수정
        private static readonly Color ARM_BTN_REAL = Color.FromArgb(180, 80, 80);        // 실물에 힘이 들어가는 동작
        private static readonly Color ARM_BTN_OFF = Color.FromArgb(110, 110, 110);       // 토크 끄기

        // 표 열 이름 → CArmMotorSet_t.SetField/GetField 키 (UI 대응표만 — 검증·해석은 DLL)
        private static readonly Dictionary<string, string> s_dicArmColKey = new Dictionary<string, string>
        {
            { "colArmName", "name" }, { "colArmDir", "dir" }, { "colArmCenter", "center" }, { "colArmLo", "lo" }, { "colArmHi", "hi" },
            { "colArmMirror", "mirror" }, { "colArmModel", "model" }, { "colArmChecked", "checked" }, { "colArmGroup", "group" },
            { "colArmBusId", "busid" }, { "colArmMechMove", "mechmove" }, { "colArmMechAngle", "mechangle" },
            { "colArmInit", "init" }, { "colArmInit2", "init2" }, { "colArmProto", "proto" },
        };

        // ════════════════════════════════════════════════════════════
        // 초기화·연결 지점 (Main.cs 에서 부른다)
        // ════════════════════════════════════════════════════════════

        /// <summary>Main_Load 에서 (SagComp_AutoLoad 다음) — 연결 객체를 만들고 지금 로봇의 모터 세팅을 불러 표를 채운다</summary>
        private void ArmMotor_Init()
        {
            if (m_C3d == null || m_ArmConn != null) return;
            m_ArmConn = new Ojw.CArmConn_t(m_C3d);
            m_ArmConn.Logger = new Ojw.CSim2Real.DLog(ArmMotor_Log);
            m_ArmConn.StateChanged += new EventHandler(ArmMotor_OnStateChanged);
            m_fntArmStateNormal = lblArmState.Font;
            m_fntArmStateBold = new Font(lblArmState.Font, FontStyle.Bold);
            ArmMotor_AfterRebuild(txtDhParams.Text);
            ArmMotor_UpdateUi();
        }

        /// <summary>MakeAll·URDF 내보내기 직후 (UI 스레드) — DH 를 다시 해석했으니 연결이 리밋을 다시 반영한다 (버스 안 씀, 사용 중이면 DLL 이 미뤘다 반영).
        /// 모터 축 구성이 바뀌었으면(로봇이 바뀜) 그 로봇의 모터 세팅 파일을 불러 적용하고 표를 다시 채운다</summary>
        private void ArmMotor_AfterRebuild(string strDh)
        {
            if (m_ArmConn == null) return;
            if (InvokeRequired) { try { BeginInvoke(new Action<string>(ArmMotor_AfterRebuild), strDh); } catch { } return; }
            int[] anAxes;
            try { anAxes = ExtractMotorIds((strDh == null) ? "" : strDh).ToArray(); }
            catch (Exception ex) { ArmMotor_Log("모터 축 읽기 오류: " + ex.Message); return; }
            try { m_ArmConn.ReapplyAfterRebuild(anAxes); }
            catch (Exception ex) { ArmMotor_Log("DH 리밋 반영 오류: " + ex.Message); }
            if (m_ArmEdit != null && m_ArmEdit.Signature == Ojw.CArmMotorSet_t.SignatureOf(anAxes)) { m_anArmAxes = anAxes; return; }   // 같은 로봇 — 표는 그대로
            ArmMotor_ChangeRobot(anAxes);
        }

        /// <summary>예제(로봇)를 바꾸기 전 (cmbExample) — 연결 중 토크 ON 이면 거부(메시지), 토크 OFF 면 연결을 저절로 해제한다. 동작 중이면 거부</summary>
        private bool ArmMotor_CanChangeRobot()
        {
            if (m_ArmConn == null || !m_ArmConn.IsConnected) return true;
            if (ArmMotor_IsBusy())
            {
                ArmMotor_MsgBox("로봇팔이 동작 중입니다 — 끝난 뒤 예제를 바꾸십시오.", MessageBoxIcon.Warning);
                return false;
            }
            if (m_ArmConn.IsTorqueOn)
            {
                ArmMotor_MsgBox("로봇팔 토크가 켜져 있습니다.\r\n예제(로봇)를 바꾸려면 '로봇팔 세팅' 탭에서 토크를 끄거나 연결을 해제하십시오.", MessageBoxIcon.Warning);
                return false;
            }
            if (m_bArmFollowBusy)
            {
                // 따라가기 읽기(DoEvents) 안에서 들어온 선택 — 이 스레드가 연결 잠금을 쥐고 있어 지금은 해제할 수 없다
                ArmMotor_StopFollow();
                ArmMotor_MsgBox("따라가기가 실물을 읽는 중이라 지금은 연결을 해제할 수 없습니다 — 따라가기를 껐습니다.\r\n예제를 다시 고르십시오.", MessageBoxIcon.Information);
                return false;
            }
            ArmMotor_StopFollow();
            if (!m_ArmConn.Disconnect(false))
            {
                ArmMotor_MsgBox("로봇팔 연결을 해제하지 못했습니다 — Log 탭을 보십시오.", MessageBoxIcon.Warning);
                return false;
            }
            ArmMotor_Log("예제를 바꿔 로봇팔 연결을 해제함 (토크 OFF 그대로)");
            return true;
        }

        /// <summary>창 닫기 (Main_FormClosing 맨 앞, m_C3d.DInit 전) — false 면 닫지 않는다.
        /// 저장 안 된 모터 세팅이 있으면 저장 여부, 연결 중이면 토크 3지선다(U1) 뒤 연결 해제 (모션 싱크를 연결 전 것으로 되돌린다).
        /// 따라가기 읽기 안에서 들어온 닫기는 읽기가 끝난 뒤 다시 닫는다. 잠금 소유자가 남았는데 도는 작업이 없으면 그래도 닫을지 묻는다</summary>
        private bool ArmMotor_OnClosing()
        {
            if (m_ArmConn == null) return true;
            if (m_bArmFollowBusy)
            {
                // 따라가기 읽기(DoEvents) 안 — 이 스레드가 연결 잠금을 쥐고 있어 지금은 해제할 수 없다. 읽기가 끝난 뒤 다시 닫는다
                ArmMotor_StopFollow();
                ArmMotor_AfterFollow(delegate() { if (!IsDisposed) Close(); });
                return false;
            }
            ArmMotor_StopFollow();
            if (ArmMotor_IsBusy())
            {
                string strOwner = m_ArmConn.BusyOwner;
                if (strOwner != null && !ArmMotor_HasWorker())
                {
                    // 잠금을 쥔 작업이 이미 끝났다 (잠금을 놓지 못하고 끝남) — 기다려도 풀리지 않는다. 연결 해제도 잠금 때문에 거부된다
                    DialogResult drForce = ArmMotor_Ask("로봇팔 연결이 '" + strOwner + "' 로 잡혀 있지만 도는 작업이 없습니다 (작업이 잠금을 놓지 못하고 끝남).\r\n\r\n" +
                                                        "[예]  그래도 닫기 — 연결을 해제하지 못해 토크는 지금 상태 그대로 남습니다 (켜져 있으면 서보가 자세를 붙잡음)\r\n" +
                                                        "[아니오]  닫지 않음",
                                                        "로봇팔 세팅", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (drForce != DialogResult.Yes) return false;
                    ArmMotor_Log("잠금이 남은 채 창을 닫음 (" + strOwner + ") — 토크 상태 그대로");
                    return true;
                }
                ArmMotor_MsgBox("로봇팔이 동작 중입니다" + ((strOwner != null) ? " (" + strOwner + ")" : "") + " — 끝난 뒤 닫으십시오.", MessageBoxIcon.Warning);
                return false;
            }
            if (m_ArmEdit != null && m_ArmEdit.Motors.Count > 0 && ArmMotor_ComputeDirty())
            {
                DialogResult dr = ArmMotor_Ask("저장 안 된 모터 세팅이 있습니다 (ID " + Ojw.CArmMotorSet_t.RangeText(m_ArmEdit.Axes) + ").\r\n\r\n" +
                                               "[예]  저장하고 닫기 (디자이너 파일 .ojw 위치를 고릅니다 — 그 창에서 취소하면 닫지 않음)\r\n[아니오]  저장하지 않고 닫기\r\n[취소]  닫지 않음",
                                               "로봇팔 세팅", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dr == DialogResult.Cancel) return false;
                if (dr == DialogResult.Yes && !ArmMotor_SaveEdit()) return false;
            }
            if (m_ArmConn.IsConnected)
            {
                bool bTorqueOff = false;
                if (m_ArmConn.IsTorqueOn)
                {
                    DialogResult dr = ArmMotor_AskTorqueOnDisconnect("창 닫기");
                    if (dr == DialogResult.Cancel) return false;
                    bTorqueOff = (dr == DialogResult.Yes);
                }
                bool bOk;
                try { bOk = m_ArmConn.Disconnect(bTorqueOff); }
                catch (Exception ex) { bOk = false; ArmMotor_Log("연결 해제 오류: " + ex.Message); }
                if (!bOk)
                {
                    ArmMotor_MsgBox("로봇팔 연결을 해제하지 못했습니다 — Log 탭을 보십시오. (창을 닫지 않습니다)", MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }

        /// <summary>포트를 쓰는 도구(①②④⑤⑥⑦⑧, 로봇팔 세팅 탭 연결 없이 도는 ⑨)의 포트·속도 — 로봇팔 세팅 탭의 포트·속도 칸이 정본이다
        /// (도구마다 있던 설정 파일의 port/baud 줄과 DLL 기본값 COM8 은 쓰지 않는다). 칸을 읽지 못하거나 그 포트가 이 PC 에 없으면 알리고 false.
        /// bOwnPort = 그 도구가 포트를 따로 연다(①②④⑤~⑧) — 로봇팔 세팅 탭이 연결돼 있으면 같은 포트를 두 곳에서 열 수 없으므로
        ///   토크 OFF 이고 한가하면 연결을 해제하고 진행할지 묻고([예] → 연결 해제, 토크 OFF 그대로), 토크 ON 이거나 탭이 동작 중이면 거부한다
        ///   (토크를 끌지는 사용자가 정한다 — 끄면 팔이 처질 수 있다). 끝난 뒤 로봇팔 세팅 탭에서 다시 [연결] 하면 된다.
        /// bOwnPort = false (⑨) — 연결 중이면 그 연결을 빌리므로 연결 상태는 보지 않는다 (부르는 쪽이 연결이 없을 때만 부른다)</summary>
        private bool ArmMotor_ToolPort(string strWhat, bool bOwnPort, out int nPort, out int nBaud)
        {
            nPort = 0; nBaud = 0;
            if (bOwnPort && Extra_IsBusy())                            // 다른 측정이 같은 포트를 쓰는 중 (①②④ 는 자기 스레드만 보던 틈)
            {
                ArmMotor_MsgBox(strWhat + " — 다른 측정이 진행 중입니다 (같은 포트를 씁니다). 끝난 뒤 다시 누르십시오.", MessageBoxIcon.Warning);
                return false;
            }
            if (bOwnPort && m_ArmConn != null && (m_ArmConn.IsConnected || m_bArmMotorRun))
            {
                string strCom = "COM" + m_ArmConn.Port.ToString(CultureInfo.InvariantCulture);
                if (m_bArmMotorRun || m_bArmFollowBusy || ArmMotor_IsBusy())
                {
                    // 연결하는 동안은 아직 '연결 안 됨' 으로 보이지만 포트는 이미 쓰는 중 — 같은 포트를 두 곳에서 열면 충돌
                    ArmMotor_Ask(strWhat + " — 로봇팔 세팅 탭이 동작 중입니다 (연결하는 중·조그·측정 등).\r\n끝난 뒤 다시 누르십시오.",
                                 "로봇팔 연결 중", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return false;
                }
                if (m_ArmConn.IsTorqueOn)
                {
                    ArmMotor_Ask(strWhat + " — 로봇팔 세팅 탭의 연결(" + strCom + ")이 토크를 붙잡고 있습니다.\r\n" +
                                 "이 측정은 같은 포트를 따로 엽니다. 로봇팔 세팅 탭에서 팔을 받치고 [토크 끄기] 하거나 [연결 해제] 한 뒤 다시 누르십시오.",
                                 "로봇팔 연결 중", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return false;
                }
                if (ArmMotor_Ask(strWhat + " — 로봇팔 세팅 탭이 " + strCom + " 에 연결돼 있습니다 (토크 OFF).\r\n" +
                                 "이 측정은 같은 포트를 따로 엽니다 — 연결을 해제하고 진행할까요?\r\n" +
                                 "(끝난 뒤 로봇팔 세팅 탭에서 다시 [연결] 하면 됩니다)",
                                 "로봇팔 연결 해제", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    return false;
                ArmMotor_StopFollow();
                if (!m_ArmConn.Disconnect(false))
                {
                    ArmMotor_MsgBox(strWhat + " — 로봇팔 연결을 해제하지 못했습니다 — Log 탭을 보십시오.", MessageBoxIcon.Warning);
                    return false;
                }
                ArmMotor_Log(strWhat + " — 포트를 따로 쓰려고 로봇팔 연결을 해제함 (토크 OFF 그대로)");
                ArmMotor_UpdateUi();
            }
            string strBaudWarn;
            if (!Ojw.CArmConn_t.ParsePort(txtArmPort.Text, out nPort))
            {
                ArmMotor_MsgBox(strWhat + " — 로봇팔 세팅 탭의 포트 칸을 읽지 못했습니다 ('" + txtArmPort.Text + "'). 예: COM4", MessageBoxIcon.Warning);
                return false;
            }
            if (!Ojw.CArmConn_t.ParseBaud(txtArmBaud.Text, out nBaud, out strBaudWarn))
            {
                ArmMotor_MsgBox(strWhat + " — 로봇팔 세팅 탭의 통신 속도 칸을 읽지 못했습니다 ('" + txtArmBaud.Text + "'). 예: 1000000", MessageBoxIcon.Warning);
                return false;
            }
            if (strBaudWarn.Length > 0) ArmMotor_Log(strBaudWarn);
            string strTabCom = "COM" + nPort.ToString(CultureInfo.InvariantCulture);
            string[] astrPorts = new string[0];
            try { astrPorts = System.IO.Ports.SerialPort.GetPortNames(); } catch { }
            bool bFound = false;
            foreach (string s in astrPorts)
                if (string.Equals(s.Trim(), strTabCom, StringComparison.OrdinalIgnoreCase)) { bFound = true; break; }
            if (!bFound)
            {
                ArmMotor_MsgBox(strWhat + " — " + strTabCom + " 이 이 PC 에 없습니다 (지금 포트: " + ((astrPorts.Length > 0) ? string.Join(", ", astrPorts) : "없음") + ").\r\n" +
                                "로봇팔 세팅 탭의 포트 칸을 고친 뒤 다시 누르십시오 (이 측정도 그 칸의 포트·속도를 씁니다).", MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool ArmMotor_IsConnected() { return m_ArmConn != null && m_ArmConn.IsConnected; }

        /// <summary>로봇팔 연결이 토크를 붙잡고 있나 (연결 && 토크 ON) — 적용·되돌리기 확인창이 "새 보정은 다음 동작에 걸쳐 들어온다" 를 덧붙일 때</summary>
        private bool ArmMotor_TorqueHeld() { return m_ArmConn != null && m_ArmConn.IsConnected && m_ArmConn.IsTorqueOn; }

        /// <summary>처짐 보정의 관절 모델이 켜져 있나 — 켜져 있으면 실물로 보내는 모션 틱마다(SendPose) 보정이 얹힌다 (관절 모델이 없는 보정은 실물 명령에 얹지 않는다)</summary>
        private bool ArmMotor_CompJointOn()
        {
            Ojw.C3d.CSagComp m = (m_C3d != null) ? m_C3d.SagComp_Get() : null;
            return m != null && m.Enabled && m.HasJoint;
        }

        /// <summary>조그를 막는 처짐 보정 상태의 사유 (없으면 ""). 어느 스레드에서 불러도 된다 (머리 주석 '처짐 보정과 조그').
        /// 보정이 켜져 있으면, 또는 출력 단에 아직 빠지지 않은 보정이 있으면(DLL SagComp_HasOffsets — 끈 뒤 페이드, 손목 유지) 막는다.
        /// 출력 단 상태를 바로 보므로 '토크 ON 중 바뀜' 같은 앱 쪽 추측 표시가 필요 없다 (다 빠지면 저절로 풀리고, 토크를 껐다 켜면 지워진다)</summary>
        private string ArmMotor_CompBlockReason()
        {
            if (ArmMotor_CompJointOn()) return "처짐 보정 켬";
            if (m_C3d != null && m_C3d.SagComp_HasOffsets) return "남은 보정이 아직 빠지는 중";
            return "";
        }

        /// <summary>조그 막힘을 푸는 짧은 절차 (알림 줄·로그)</summary>
        private static string ArmMotor_CompBlockHint(string strWhy)
        {
            return (strWhy == "처짐 보정 켬") ? "⑦ '처짐 보정 켬' 끄기 (남은 보정이 있으면 토크 껐다 켜기)" : "토크 껐다 켜기 (팔을 받치고)";
        }

        /// <summary>조그 막힘 안내 — 방향 확인 조그는 보정 없이</summary>
        private static string ArmMotor_CompBlockText(string strWhy)
        {
            string s = "조그를 하지 않습니다 — " + strWhy + ".\r\n\r\n" +
                       "처짐 보정은 실물로 보내는 모션 틱마다 얹히고, 켜거나 끄거나 바뀐 뒤 출력 틱 1초에 걸쳐 들어오고 빠집니다. 그동안은 조그의 틱마다 " +
                       "그 일부가 얹혀 고르지 않은 관절도 움직입니다 (튀지는 않음). 방향 확인 조그는 고른 관절만 움직여야 하므로 보정 없이 합니다.\r\n\r\n";
            if (strWhy == "처짐 보정 켬")
                s += "'측정·학습·시험' 페이지 ⑦ 의 '처짐 보정 켬' 을 끄십시오.\r\n" +
                     "  · 토크를 켠 뒤 아직 움직이지 않았으면 남는 보정이 없어 바로 조그할 수 있습니다.\r\n" +
                     "  · 보정이 이미 실물에 걸려 있었으면 남은 보정이 빠질 때까지 막힙니다 — 그때는 팔을 받치고 토크를 껐다 켜십시오.\r\n\r\n";
            else
                s += "보정은 꺼져 있지만 실물에 걸려 있던 보정이 아직 빠지지 않았습니다 (다음 동작의 출력 틱에 걸쳐 빠짐).\r\n" +
                     "팔을 손으로 받치고 토크를 껐다 켜면 지워집니다.\r\n\r\n";
            return s + "보정은 조그를 마친 뒤 다시 켜십시오 (토크 ON 이어도 됩니다 — 다음 동작에 걸쳐 들어옵니다).";
        }

        /// <summary>이 탭의 동작·측정 작업·연결 잠금(소유자) 중 하나라도 있거나 그런 작업이 돌리는 3D 모션 중이면 true.
        /// 3D 모션 표시만 남고 돌리는 스레드가 없으면 지우고(ArmMotor_ClearStaleMotion) 바쁘지 않은 것으로 본다 (UI 스레드에서 부른다)</summary>
        private bool ArmMotor_IsBusy()
        {
            if (m_bArmMotorRun || m_bArmRun) return true;
            if (m_ArmConn != null && m_ArmConn.BusyOwner != null) return true;
            return m_C3d != null && m_C3d.MoveJoints_IsMoving() && !ArmMotor_ClearStaleMotion();
        }

        /// <summary>실물·3D 를 쓰는 스레드가 도는가 — 이 탭의 동작(조그·읽기 …), 이 페이지의 작업, 측정 스레드(⑨·⑤~⑧·①②④)</summary>
        private bool ArmMotor_HasWorker()
        {
            return m_bArmMotorRun || m_bArmRun || Extra_IsBusy();
        }

        /// <summary>3D 모션 표시(MoveJoints_IsMoving)가 남았는데 그것을 진행시킬 스레드가 없으면 지운다 (UI 스레드). true = 지웠다.
        /// 이 앱의 3D 모션은 DLL 의 대기 루프(Play·PlayXyz)가 직접 진행시키고, 그 루프는 조그(이 탭 스레드)·측정 작업·⑨(측정 스레드) 안에서만 돈다.
        /// 그 스레드가 다 끝났고 잠금 소유자도 없는데 표시가 남았다면 모션 스텝이 예외로 끝난 것 — 진행시킬 곳이 없어 영영 '모션 중' 이 된다</summary>
        private bool ArmMotor_ClearStaleMotion()
        {
            if (m_C3d == null || InvokeRequired || !m_C3d.MoveJoints_IsMoving()) return false;
            if (ArmMotor_HasWorker() || (m_ArmConn != null && m_ArmConn.BusyOwner != null)) return false;
            try { m_C3d.MoveJoints_Stop(); }
            catch (Exception ex) { ArmMotor_Log("3D 모션 표시 지우기 오류: " + ex.Message); return false; }
            ArmMotor_Log("3D 모션 표시가 남아 있었음 — 진행시키는 작업이 없어(모션 스텝 오류로 끝남) 멈춤 처리 (연결 중이면 다음 동작 전에 3D 를 실물 자세로 다시 맞춘다)");
            return true;
        }

        // ════════════════════════════════════════════════════════════
        // 로봇 바뀜 → 세팅 불러오기·적용
        // ════════════════════════════════════════════════════════════

        /// <summary>출력 폴더(GetUrdfOutputDir) — 출력 폴더 칸 이름을 경로로 쓸 수 없으면(", &lt;, &gt;, | 등) null 과 까닭.
        /// 클릭 처리기·표시 갱신에서 예외가 밖으로 나가 처리되지 않은 예외 창이 뜨지 않게 한다</summary>
        private string ArmMotor_UrdfDir(out string strWhy)
        {
            strWhy = "";
            try { return GetUrdfOutputDir(); }
            catch (Exception ex)
            {
                strWhy = "urdf 폴더 이름을 경로로 쓸 수 없습니다 (" + ex.Message + ") — 출력 폴더 칸을 고치십시오";
                return null;
            }
        }

        /// <summary>로봇(모터 축 구성)이 바뀜 — 저장 안 된 수정은 옛 서명의 .autosave.ojw 로 보관(대화창 없음 — MakeAll 은 입력 중에도 불린다),
        /// 새 서명의 세팅을 불러 적용하고 미러 콤보·표를 다시 채운다. 불러오는 순서: ojw_paths.txt 에 남은 그 서명의 마지막 .ojw →
        /// 예전 motors_&lt;서명&gt;.txt (읽기만) → 기본값. 어느 것을 썼는지 로그에 남긴다</summary>
        private void ArmMotor_ChangeRobot(int[] anAxes)
        {
            string strDirWhy;
            string strDir = ArmMotor_UrdfDir(out strDirWhy);    // 칸 이름을 경로로 쓸 수 없으면 null — 파일 없이 기본값으로 (예외를 밖으로 내지 않는다)
            if (m_ArmEdit != null && m_ArmEdit.Motors.Count > 0 && ArmMotor_ComputeDirty())
            {
                if (strDir == null) ArmMotor_Log("★로봇이 바뀜 — 저장 안 된 모터 세팅을 보관하지 못함: " + strDirWhy);
                else
                {
                    // 3D 는 이미 새 로봇이다 — 보관 파일의 모터 정보만 옛 세팅이다 ([불러오기] 로 그 축만 가져올 수 있다)
                    string strAuto = ArmMotor_AutosavePath(strDir, m_ArmEdit.Axes);
                    string w;
                    if (m_ArmEdit.SaveDesignerFile(m_C3d, strAuto, out w))
                        ArmMotor_Log("로봇이 바뀜 — 저장 안 된 모터 세팅을 보관: " + strAuto + " (모터 정보만 옛 세팅 — 3D 모델은 바뀐 로봇)" +
                                     ((w.Length > 0) ? " / " + w.Replace("\r\n", " / ") : ""));
                    else ArmMotor_Log("★로봇이 바뀜 — 저장 안 된 모터 세팅 보관 실패: " + w.Replace("\r\n", " / "));
                }
            }
            m_anArmAxes = anAxes;
            m_bArmApplyPending = false;
            m_strArmMsg = "";
            string strRobot = "";                               // 표시용 로봇 이름은 저장할 때 넣는다 (예제 이름은 이 호출 뒤에 바뀐다)
            if (anAxes.Length == 0)
            {
                m_ArmEdit = Ojw.CArmMotorSet_t.CreateDefault(anAxes, strRobot);
                m_ArmSaved = m_ArmEdit.Clone();
                m_strArmFile = null;
                ArmMotor_FillGrid(true);
                return;
            }
            string strSig = Ojw.CArmMotorSet_t.SignatureOf(anAxes);
            Ojw.CArmMotorSet_t set = null;
            string strFrom = null;
            // ① 이 서명의 마지막 .ojw (ojw_paths.txt)
            string strOjw = (strDir != null) ? Ojw.CArmMotorSet_t.GetRecentOjw(strDir, strSig) : null;
            if (strOjw != null)
            {
                if (!File.Exists(strOjw)) ArmMotor_Log("마지막 디자이너 파일이 없음 — 다음 순서로: " + strOjw);
                else
                {
                    Ojw.CArmMotorSet_t s = Ojw.CArmMotorSet_t.CreateDefault(anAxes, strRobot);
                    string strWhy;
                    if (s.ImportDesignerFile(strOjw, out strWhy) < 0) ArmMotor_Log("★디자이너 파일을 읽지 못함 — 다음 순서로: " + strWhy.Replace("\r\n", " / "));
                    else
                    {
                        set = s;
                        strFrom = strOjw;
                        ArmMotor_Log("모터 세팅 불러옴 (디자이너 파일): " + strOjw + " — " + strWhy.Replace("\r\n", " / "));
                    }
                }
            }
            // ② 예전 txt (읽기만 — 저장하면 .ojw 로)
            string strTxt = (strDir != null) ? Ojw.CArmMotorSet_t.DefaultPath(strDir, anAxes) : null;
            if (set == null && strTxt != null && File.Exists(strTxt))
            {
                string strWhy;
                set = Ojw.CArmMotorSet_t.Load(strTxt, out strWhy);
                if (set == null) ArmMotor_Log("예전 모터 세팅 파일(txt)을 읽지 못함 — 기본값으로: " + strWhy.Replace("\r\n", " / "));
                else if (strWhy.Length > 0) ArmMotor_Log("예전 모터 세팅 파일(txt) 경고: " + strWhy.Replace("\r\n", " / "));
                if (set != null && set.Signature != strSig)
                {
                    int[] anTaken;
                    set = set.MergeFor(anAxes, out anTaken);      // 파일 안의 축이 이름과 다르면 같은 축만
                }
                if (set != null)
                {
                    strFrom = strTxt;
                    ArmMotor_Log("모터 세팅 불러옴 (예전 txt — 읽기만, 저장하면 .ojw): " + strTxt);
                }
            }
            // ③ 기본값
            if (set == null)
            {
                set = Ojw.CArmMotorSet_t.CreateDefault(anAxes, strRobot);
                if (strDir == null) ArmMotor_Log("★모터 세팅 파일을 찾지 못함 — " + strDirWhy + " — 기본값(방향 정·영점 2048)");
                else ArmMotor_Log("모터 세팅 파일 없음 (ID " + Ojw.CArmMotorSet_t.RangeText(anAxes) + " 의 .ojw 기록·예전 txt 모두 없음) — 기본값(방향 정·영점 2048)");
            }
            m_strArmFile = strFrom;
            m_ArmEdit = set;
            m_ArmSaved = set.Clone();
            ArmMotor_ApplyRobotSet();
            ArmMotor_FillGrid(true);
        }

        /// <summary>편집본을 적용 (로봇이 바뀐 뒤·미뤄 둔 적용). 거부되면: 연결이 사용 중 → 끝난 뒤 다시(m_bArmApplyPending),
        /// 수동 리밋과 EDH $jointrange 가 겹치지 않음 → 기본값을 적용하고 편집본은 그대로 둔다 (표에서 리밋을 고치면 적용된다)</summary>
        private void ArmMotor_ApplyRobotSet()
        {
            if (m_ArmConn == null || m_ArmEdit == null || m_ArmEdit.Motors.Count == 0) return;
            Ojw.CArmMotorSet_t set = m_ArmEdit.Clone();
            ArmMotor_ApplySet(set, "세팅 적용", delegate(bool bOk, string strWhy)
            {
                if (bOk)
                {
                    if (strWhy.Length > 0) ArmMotor_Log("모터 세팅 적용 — 경고: " + strWhy.Replace("\r\n", " / "));
                    ArmMotor_FillGrid(false);
                    return;
                }
                Ojw.CArmMotorSet_t chk = set.Clone();
                chk.ComputeEffective(m_C3d);
                if (chk.HasLimitConflict)
                {
                    ArmMotor_Log("불러온 모터 세팅의 리밋이 EDH $jointrange 와 겹치지 않음 — 기본값을 적용:\r\n" + chk.LimitConflict);
                    Ojw.CArmMotorSet_t def = Ojw.CArmMotorSet_t.CreateDefault(m_anArmAxes, set.strRobot);
                    ArmMotor_ApplySet(def, "기본 세팅 적용", delegate(bool bOk2, string strWhy2)
                    {
                        if (!bOk2) ArmMotor_Log("기본 세팅도 적용하지 못함: " + strWhy2.Replace("\r\n", " / "));
                        ArmMotor_SetMsg("불러온 세팅의 리밋이 EDH 와 겹치지 않아 기본값으로 동작 중 — 표에서 하한·상한을 고치면 적용됩니다", true);
                        ArmMotor_FillGrid(false);
                    });
                    return;
                }
                if (m_bArmMotorRun || m_ArmConn.IsBusy)
                {
                    m_bArmApplyPending = true;                   // 사용 중 — 끝나면 ArmMotor_UpdateUi 가 다시 적용
                    ArmMotor_Log("연결이 사용 중 — 끝난 뒤 모터 세팅을 적용합니다 (" + strWhy.Replace("\r\n", " / ") + ")");
                }
                else
                {
                    ArmMotor_Log("모터 세팅 적용 거부: " + strWhy.Replace("\r\n", " / "));
                    ArmMotor_SetMsg("모터 세팅 적용 거부: " + strWhy.Replace("\r\n", " / "), true);
                }
                ArmMotor_FillGrid(false);
            });
        }

        /// <summary>세트 적용 — 연결된 모터와 축 구성이 같으면 버스를 쓸 수 있어(매핑이 바뀌면 틱을 읽는다) 배경 스레드로, 아니면 바로 (버스 안 씀).
        /// done(적용됨, 사유·경고)은 UI 스레드에서. 따라가기 읽기 안에서 불리면 바로 하는 적용은 읽기가 끝난 뒤에 한다</summary>
        private void ArmMotor_ApplySet(Ojw.CArmMotorSet_t set, string strTitle, Action<bool, string> done)
        {
            Ojw.CScene_t sc = m_ArmConn.Scene;
            bool bBus = sc != null && Ojw.CArmMotorSet_t.SignatureOf(sc.IDs) == set.Signature;
            if (!bBus)
            {
                if (m_bArmFollowBusy)
                {
                    // 따라가기 읽기(DoEvents) 안 — 지금 UI 스레드에서 잠금 함수를 부르면 "사용 중: 따라가기" 로 거부된다
                    ArmMotor_AfterFollow(delegate() { ArmMotor_ApplySet(set, strTitle, done); });
                    return;
                }
                bool b;
                string w;
                try { b = m_ArmConn.ApplyMotors(set, null, out w); }
                catch (Exception ex) { b = false; w = "세팅 적용 오류: " + ex.Message; }
                done(b, (w == null) ? "" : w);
                return;
            }
            bool bOk = false;
            string strWhy = "";
            if (!ArmMotor_Run(strTitle, delegate() { bOk = m_ArmConn.ApplyMotors(set, null, out strWhy); return null; },
                              delegate(string r) { done(bOk, (strWhy == null) ? "" : strWhy); }))
                done(false, "사용 중 — 다른 동작이 끝난 뒤 다시");
        }

        /// <summary>표 편집·불러오기·가져오기 결과(m_ArmEdit)를 적용 — 거부되면 편집본을 prev 로 되돌리고 사유를 알림 줄과 로그에.
        /// 받아들이면 경고(U3 다시 학습 권장 등)를 보이고 onOk</summary>
        private void ArmMotor_ApplyEdit(Ojw.CArmMotorSet_t prev, string strWhat, string strWarn0, Action onOk)
        {
            if (m_ArmConn == null || m_ArmEdit == null) return;
            Ojw.CArmMotorSet_t set = m_ArmEdit.Clone();
            ArmMotor_ApplySet(set, "세팅 적용", delegate(bool bOk, string strWhy)
            {
                if (!bOk)
                {
                    if (prev != null) m_ArmEdit = prev;          // 표는 적용 전 값으로 (연결 쪽 세팅은 그대로)
                    ArmMotor_SetMsg(strWhat + " — 적용 거부: " + strWhy.Replace("\r\n", " / "), true);
                    ArmMotor_Log(strWhat + " — 적용 거부: " + strWhy.Replace("\r\n", " / "));
                    ArmMotor_FillGrid(false);
                    return;
                }
                string strW = ArmMotor_Join(strWarn0, strWhy, "\r\n");
                if (strW.Length > 0)
                {
                    ArmMotor_SetMsg(strWhat + " — 경고: " + strW.Replace("\r\n", " / "), true);
                    ArmMotor_Log(strWhat + " — 경고: " + strW.Replace("\r\n", " / "));
                }
                else ArmMotor_SetMsg(strWhat + " — 적용 (저장 안 됨)", false);
                if (onOk != null) onOk();
                ArmMotor_FillGrid(false);
            });
        }

        // ════════════════════════════════════════════════════════════
        // 상태 갱신
        // ════════════════════════════════════════════════════════════

        /// <summary>연결 객체의 상태 알림 — 작업 스레드에서(잠금을 잡은 채일 수도) 온다. 여기서는 화면 갱신을 UI 로 넘기기만 한다</summary>
        private void ArmMotor_OnStateChanged(object sender, EventArgs e)
        {
            if (m_bArmUiQueued) return;
            m_bArmUiQueued = true;
            try
            {
                if (IsDisposed || !IsHandleCreated) { m_bArmUiQueued = false; return; }
                BeginInvoke(new Action(delegate() { m_bArmUiQueued = false; ArmMotor_UpdateUi(); }));
            }
            catch { m_bArmUiQueued = false; }
        }

        /// <summary>연결 줄·모터 세팅 버튼의 글·색·활성 (명세 표) + 표 상태 열 + 측정 페이지 잠금. StateChanged 마다</summary>
        private void ArmMotor_UpdateUi()
        {
            if (m_ArmConn == null || IsDisposed) return;
            if (InvokeRequired) { try { BeginInvoke(new Action(ArmMotor_UpdateUi)); } catch { } return; }
            bool bConn = m_ArmConn.IsConnected, bTq = m_ArmConn.IsTorqueOn, bMis = m_ArmConn.ConfigMismatch;
            string strOwner = m_ArmConn.BusyOwner;
            bool bBusy = ArmMotor_IsBusy();
            bool bHasMotors = m_ArmEdit != null && m_ArmEdit.Motors.Count > 0;
            int nSel = ArmMotor_SelectedAxis();

            // lblArmState — 연결 안 됨(회색) / 토크 OFF(파랑) / 토크 ON(빨강 굵게) / 사용 중(주황) / 구성 불일치(빨강) / 처짐 보정 때문에 조그 막힘
            string strState;
            Color cState;
            bool bBold = false;
            if (!bConn) { strState = "연결 안 됨"; cState = Color.Gray; }
            else
            {
                strState = "COM" + m_ArmConn.Port.ToString(CultureInfo.InvariantCulture) + " · 토크 " + (bTq ? "ON" : "OFF");
                cState = bTq ? Color.Red : Color.Blue;
                bBold = bTq;
                if (bMis) { strState += "\r\n모터 구성 바뀜 — 다시 연결"; cState = Color.Red; }
                else if (strOwner != null) { strState += "\r\n사용 중: " + strOwner; cState = Color.DarkOrange; }
                else if (bTq && ArmMotor_CompBlockReason().Length > 0) strState += "\r\n조그 막힘 (보정)";   // 사유·절차는 조그를 누르면 보인다
                else if (m_ArmConn.NeedsSync) strState += "\r\n움직이기 전 다시 맞춤";
            }
            lblArmState.Text = strState;
            lblArmState.ForeColor = cState;
            if (m_fntArmStateBold != null) lblArmState.Font = bBold ? m_fntArmStateBold : m_fntArmStateNormal;

            // 버튼 활성 (명세 §3.3 표)
            txtArmPort.Enabled = txtArmBaud.Enabled = !bConn && !bBusy;
            btnArmConnect.Text = bConn ? "연결 해제" : "연결";
            btnArmConnect.Enabled = !bBusy;
            btnArmTorque.Text = bTq ? "토크 끄기" : "토크 켜기";
            btnArmTorque.BackColor = bTq ? ARM_BTN_OFF : ARM_BTN_REAL;
            btnArmTorque.Enabled = bConn && !bBusy && !bMis;
            btnArmRead.Enabled = bConn && !bBusy;
            btnArmJogMinus.Enabled = btnArmJogPlus.Enabled = bConn && bTq && nSel >= 0 && !bBusy && !bMis;
            btnArmSetZero.Enabled = bConn && nSel >= 0 && !bBusy;
            btnArmSave.Enabled = btnArmLoad.Enabled = btnArmImportAuto.Enabled = !bBusy && bHasMotors;

            // 따라가기 — 연결 && 토크 OFF 에서만. 토크가 켜지거나 끊기면 저절로 끄고 타이머를 멈춘다
            bool bFollowOk = bConn && !bTq && !bMis;
            if (!bFollowOk && chkArmFollow.Checked) { m_bArmUiSync = true; chkArmFollow.Checked = false; m_bArmUiSync = false; }
            chkArmFollow.Enabled = bFollowOk;
            tmrArmFollow.Enabled = chkArmFollow.Checked && bFollowOk;

            // 로봇이 바뀌었는데 사용 중이라 미뤄 둔 세팅 적용
            if (m_bArmApplyPending && !bBusy) { m_bArmApplyPending = false; ArmMotor_ApplyRobotSet(); }

            ArmMotor_FillGrid(false);
            ArmSetup_RefreshLock();
            ArmSetup_UpdateEnable();
            SagComp_UiSync();                                   // 처짐 보정 체크 — 로봇팔 토크 ON 동안은 끈다 (Main.SagComp.cs)
        }

        /// <summary>표 채우기 — bRebuild 거나 축이 바뀌었으면 미러·기종 콤보 Items 를 먼저 채운 뒤 행을 다시 만든다 (칸 값이 늘 Items 안 — DataError 방지).
        /// 편집 중인 칸은 건드리지 않는다</summary>
        private void ArmMotor_FillGrid(bool bRebuild)
        {
            if (m_bArmCommitPending) return;
            m_bArmUiSync = true;
            try
            {
                int[] anAxes = (m_ArmEdit != null) ? m_ArmEdit.Axes : new int[0];
                bool bRows = bRebuild || dgvArmMotor.Rows.Count != anAxes.Length;
                for (int i = 0; !bRows && i < anAxes.Length; i++)
                {
                    object t = dgvArmMotor.Rows[i].Tag;
                    if (!(t is int) || (int)t != anAxes[i]) bRows = true;
                }
                if (bRows)
                {
                    if (dgvArmMotor.IsCurrentCellInEditMode) dgvArmMotor.CancelEdit();
                    dgvArmMotor.Rows.Clear();
                    colArmMirror.Items.Clear();
                    colArmMirror.Items.Add("없음");
                    colArmMirror.Items.Add("자기 반전");
                    foreach (int ax in anAxes) colArmMirror.Items.Add(ax.ToString(CultureInfo.InvariantCulture));
                    colArmModel.Items.Clear();
                    colArmModel.Items.Add(Ojw.CArmMotorSet_t.ModelName(0));
                    foreach (int k in Ojw.CArmMotorSet_t.KnownModelKeys)
                    {
                        string strName = Ojw.CArmMotorSet_t.ModelName(k);
                        if (!colArmModel.Items.Contains(strName)) colArmModel.Items.Add(strName);
                    }
                    foreach (int ax in anAxes)
                    {
                        int r = dgvArmMotor.Rows.Add();
                        dgvArmMotor.Rows[r].Tag = ax;
                    }
                }
                Ojw.CArmMotorState_t[] aSt = (m_ArmConn != null) ? m_ArmConn.LastState : null;
                bool bConn = ArmMotor_IsConnected();
                for (int i = 0; i < anAxes.Length; i++) ArmMotor_FillRow(dgvArmMotor.Rows[i], anAxes[i], ArmMotor_StateOf(aSt, anAxes[i]), bConn);
            }
            catch (Exception ex) { ArmMotor_Log("표 채우기 오류: " + ex.Message); }
            finally { m_bArmUiSync = false; }
            m_bArmDirty = ArmMotor_ComputeDirty();
            ArmMotor_UpdateFileLabel();
        }

        private void ArmMotor_FillRow(DataGridViewRow row, int nAxis, Ojw.CArmMotorState_t st, bool bConn)
        {
            ArmMotor_SetCell(row, colArmAxis, nAxis.ToString(CultureInfo.InvariantCulture));
            foreach (DataGridViewColumn col in dgvArmMotor.Columns)
            {
                string strKey;
                if (!s_dicArmColKey.TryGetValue(col.Name, out strKey)) continue;
                ArmMotor_SetCell(row, col, m_ArmEdit.GetField(nAxis, strKey));
            }
            CultureInfo ci = CultureInfo.InvariantCulture;
            ArmMotor_SetCell(row, colArmNowDeg, (st != null && st.bTickOk) ? st.fSim.ToString("0.0", ci) : "");
            ArmMotor_SetCell(row, colArmNowTick, (st != null && st.bTickOk) ? st.nTick.ToString(ci) : "");
            ArmMotor_SetCell(row, colArmRealModel, (st == null) ? "" : (st.bResponded ? st.strModel : "무응답"));
            ArmMotor_SetCell(row, colArmNote, ArmMotor_Join((st != null) ? st.strNote : "", "출처 " + m_ArmEdit.GetField(nAxis, "source"), " · "));
            Color c = Color.Empty;
            if (bConn && st != null && (!st.bResponded || st.bOutside)) c = ARM_ROW_RED;
            else if (bConn && st != null && st.bWarn) c = ARM_ROW_YELLOW;
            else if (ArmMotor_RowDirty(nAxis)) c = ARM_ROW_BLUE;
            if (row.DefaultCellStyle.BackColor != c) row.DefaultCellStyle.BackColor = c;
        }

        /// <summary>칸 하나를 편집본 값으로 (m_bArmUiSync 안에서 — 값을 넣으면 CellValueChanged 가 나지만 무시된다).
        /// 지금 칸이 편집 모드이면: 입력이 아직 커밋되지 않았으면(IsCurrentCellDirty — 글 칸에 치는 중) 그대로 두고, 커밋된 칸은 값을 넣은 뒤
        /// 편집 컨트롤도 그 값으로 다시 그린다 (RefreshEdit). EditOnEnter 라 콤보·체크 칸은 고르는 즉시 커밋돼도 편집 모드로 남고, EndEdit 로도
        /// 편집 모드가 끝나지 않는다 — 거부·취소된 방향·미러·기종·확인 칸이 편집본(그대로) 값으로 돌아가 보이려면 여기서 바꿔야 한다</summary>
        private void ArmMotor_SetCell(DataGridViewRow row, DataGridViewColumn col, string strValue)
        {
            DataGridViewCell cell = row.Cells[col.Index];
            bool bEditing = dgvArmMotor.IsCurrentCellInEditMode && dgvArmMotor.CurrentCell == cell;
            if (bEditing && dgvArmMotor.IsCurrentCellDirty) return;           // 치는 중인 입력은 그대로 (커밋되면 CommitCell 이 처리)
            object o;
            if (cell is DataGridViewCheckBoxCell) o = (strValue == "1");
            else
            {
                o = (strValue == null) ? "" : strValue;
                DataGridViewComboBoxCell cb = cell as DataGridViewComboBoxCell;
                if (cb != null && !cb.Items.Contains(o)) cb.Items.Add(o);    // 목록에 없는 값(기타(n) 등) — 그 칸에만 넣는다
            }
            if (object.Equals(cell.Value, o)) return;
            cell.Value = o;
            if (bEditing)
            {
                try { dgvArmMotor.RefreshEdit(); }                            // 편집 컨트롤(콤보 글·체크 표시)도 새 값으로
                catch (Exception ex) { ArmMotor_Log("표 칸 다시 그리기 오류: " + ex.Message); }
            }
        }

        /// <summary>현재각·현재 틱 열만 (따라가기 — 3틱에 한 번)</summary>
        private void ArmMotor_FillNowColumns()
        {
            if (m_bArmCommitPending || m_ArmConn == null) return;
            Ojw.CArmMotorState_t[] aSt = m_ArmConn.LastState;
            if (aSt == null) return;
            CultureInfo ci = CultureInfo.InvariantCulture;
            m_bArmUiSync = true;
            try
            {
                foreach (DataGridViewRow row in dgvArmMotor.Rows)
                {
                    if (!(row.Tag is int)) continue;
                    Ojw.CArmMotorState_t st = ArmMotor_StateOf(aSt, (int)row.Tag);
                    if (st == null || !st.bTickOk) continue;
                    ArmMotor_SetCell(row, colArmNowDeg, st.fSim.ToString("0.0", ci));
                    ArmMotor_SetCell(row, colArmNowTick, st.nTick.ToString(ci));
                    Color c = st.bOutside ? ARM_ROW_RED : (st.bWarn ? ARM_ROW_YELLOW : (ArmMotor_RowDirty((int)row.Tag) ? ARM_ROW_BLUE : Color.Empty));
                    if (!st.bResponded) c = ARM_ROW_RED;
                    if (row.DefaultCellStyle.BackColor != c) row.DefaultCellStyle.BackColor = c;
                }
            }
            finally { m_bArmUiSync = false; }
        }

        private static Ojw.CArmMotorState_t ArmMotor_StateOf(Ojw.CArmMotorState_t[] aSt, int nAxis)
        {
            if (aSt == null) return null;
            foreach (Ojw.CArmMotorState_t s in aSt) if (s != null && s.nAxis == nAxis) return s;
            return null;
        }

        /// <summary>그 축이 저장 기준과 다른가 (표 키마다 GetField 비교 — 표시 문자열 비교만).
        /// 방향 확인 체크는 보지 않는다 — .ojw 에 칸이 없어 저장해도 남지 않는다</summary>
        private bool ArmMotor_RowDirty(int nAxis)
        {
            if (m_ArmEdit == null || m_ArmSaved == null) return false;
            if (m_ArmSaved.Get(nAxis) == null) return true;
            foreach (string k in Ojw.CArmMotorSet_t.FieldKeys)
                if (k != "checked" && m_ArmEdit.GetField(nAxis, k) != m_ArmSaved.GetField(nAxis, k)) return true;
            return false;
        }

        private bool ArmMotor_ComputeDirty()
        {
            if (m_ArmEdit == null || m_ArmEdit.Motors.Count == 0) return false;
            if (m_ArmSaved == null || m_ArmSaved.Signature != m_ArmEdit.Signature) return true;
            foreach (int ax in m_ArmEdit.Axes) if (ArmMotor_RowDirty(ax)) return true;
            return false;
        }

        private void ArmMotor_UpdateFileLabel()
        {
            string s;
            if (m_ArmEdit == null || m_ArmEdit.Motors.Count == 0) s = "파일: (모터 없음 — 예제를 불러오십시오)";
            else if (m_strArmFile == null)
            {
                string strDirWhy;
                string strDir = ArmMotor_UrdfDir(out strDirWhy);
                s = (strDir != null)
                    ? "파일: (없음 — 기본값. '저장' 하면 디자이너 파일(.ojw) 위치를 고릅니다)"
                    : "파일: (없음 — 기본값. ★" + strDirWhy + ")";
            }
            else
            {
                bool bOldTxt = !string.Equals(Path.GetExtension(m_strArmFile), ".ojw", StringComparison.OrdinalIgnoreCase);
                s = "파일: " + ArmMotor_ShortPath(m_strArmFile) + ArmMotor_SavedText(m_ArmEdit.strSaved) +
                    (bOldTxt ? " · 예전 txt (읽기만 — '저장' 하면 .ojw)" : "");
            }
            if (m_bArmDirty) s += " · 저장 안 된 수정 있음";
            if (m_strArmMsg.Length > 0) s += "\r\n" + m_strArmMsg;
            lblArmFile.Text = s;
            lblArmFile.ForeColor = m_strArmMsg.StartsWith("★") ? Color.Firebrick : SystemColors.ControlText;
        }

        /// <summary>" (저장 hh:mm)" — 오늘이면 시각만, 다른 날이면 날짜까지. 비면 ""</summary>
        private static string ArmMotor_SavedText(string strSaved)
        {
            if (string.IsNullOrEmpty(strSaved)) return "";
            DateTime t;
            if (!DateTime.TryParseExact(strSaved, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out t)) return " (저장 " + strSaved + ")";
            return " (저장 " + ((t.Date == DateTime.Today) ? t.ToString("HH:mm", CultureInfo.InvariantCulture) : strSaved) + ")";
        }

        /// <summary>알림 줄 (lblArmFile 둘째 줄) — bWarn 이면 ★ (빨강)</summary>
        private void ArmMotor_SetMsg(string strMsg, bool bWarn)
        {
            m_strArmMsg = (strMsg == null) ? "" : ((bWarn ? "★" : "") + strMsg);
            ArmMotor_UpdateFileLabel();
        }

        /// <summary>로그 (Log 탭) — 어느 스레드에서 불러도 된다 (작업 스레드면 BeginInvoke 로 넘기고 기다리지 않는다)</summary>
        private void ArmMotor_Log(string strMsg)
        {
            if (strMsg == null) return;
            try
            {
                if (InvokeRequired) { if (!IsDisposed) BeginInvoke(new Action<string>(ArmMotor_Log), strMsg); return; }
                Ojw.printf(txtLogHistory, "[로봇팔] {0}\r\n", strMsg);
            }
            catch { }
        }

        // ════════════════════════════════════════════════════════════
        // 배경 스레드·대화 도우미
        // ════════════════════════════════════════════════════════════

        /// <summary>이 탭의 버스 동작을 배경 스레드(IsBackground)에서 — 시작 전에 버튼을 끄고(잠금 포함), 끝나면 UI 스레드에서 onDone(작업 반환값) 뒤 화면 갱신.
        /// 예외는 로그로 보내고 던지지 않는다. 이미 도는 동작이 있으면 false</summary>
        private bool ArmMotor_Run(string strTitle, Func<string> work, Action<string> onDone)
        {
            if (m_bArmMotorRun || m_ArmConn == null || work == null) return false;
            m_bArmMotorRun = true;
            ArmMotor_UpdateUi();
            Thread thr = new Thread(delegate()
            {
                string strRet = null;
                try { strRet = work(); }
                catch (Exception ex) { strRet = null; ArmMotor_Log(strTitle + " 오류: " + ex.Message); }
                try
                {
                    BeginInvoke(new Action(delegate()
                    {
                        m_bArmMotorRun = false;
                        try { if (onDone != null) onDone(strRet); }
                        catch (Exception ex2) { ArmMotor_Log(strTitle + " 결과 처리 오류: " + ex2.Message); }
                        ArmMotor_UpdateUi();
                    }));
                }
                catch { m_bArmMotorRun = false; }
            });
            thr.IsBackground = true;
            thr.Name = "ArmMotor";
            thr.Start();
            return true;
        }

        private DialogResult ArmMotor_Ask(string strText, string strCaption, MessageBoxButtons btn, MessageBoxIcon icon, MessageBoxDefaultButton def)
        {
            m_bArmModal = true;
            try { return MessageBox.Show(this, strText, strCaption, btn, icon, def); }
            finally { m_bArmModal = false; }
        }

        /// <summary>예/아니오 확인 — 기본 버튼 아니오</summary>
        private bool ArmMotor_Confirm(string strText, string strCaption)
        {
            return ArmMotor_Ask(strText, strCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        private void ArmMotor_MsgBox(string strText, MessageBoxIcon icon)
        {
            ArmMotor_Ask(strText, "로봇팔 세팅", MessageBoxButtons.OK, icon, MessageBoxDefaultButton.Button1);
        }

        /// <summary>U1 — 토크 ON 인데 연결을 끊을 때 (연결 해제 버튼·창 닫기): [예] 토크 끄고 해제 / [아니오] 토크 유지한 채 해제 / [취소]. 기본 버튼 = 취소</summary>
        private DialogResult ArmMotor_AskTorqueOnDisconnect(string strWhy)
        {
            return ArmMotor_Ask(strWhy + " — 로봇팔 토크가 켜져 있습니다. 연결을 해제할 때 토크를 어떻게 할까요?\r\n\r\n" +
                                "[예]  토크 끄고 해제 — 팔이 처지거나 떨어질 수 있습니다. 손으로 받치십시오\r\n" +
                                "[아니오]  토크 유지한 채 해제 — 서보가 마지막 자세를 붙잡습니다\r\n" +
                                "[취소]  해제하지 않음",
                                "로봇팔 연결 해제", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
        }

        private void ArmMotor_StopFollow()
        {
            tmrArmFollow.Enabled = false;
            if (chkArmFollow.Checked) { m_bArmUiSync = true; chkArmFollow.Checked = false; m_bArmUiSync = false; }
        }

        /// <summary>따라가기 읽기(FollowOnce — 버스 읽기 안에서 DoEvents) 안에서 들어온 일을 읽기가 끝난 뒤로 미룬다 (tmrArmFollow_Tick 끝에서 BeginInvoke).
        /// 그 안에서 같은 스레드가 연결 잠금 함수를 부르면 "사용 중: 따라가기" 로 거부되고, 그 자리에서 BeginInvoke 로 다시 넣으면
        /// DoEvents 가 곧바로 다시 꺼내 돈다 (읽기가 끝나지 않는다)</summary>
        private void ArmMotor_AfterFollow(Action a)
        {
            if (a != null) m_lstArmAfterFollow.Add(a);
        }

        private int ArmMotor_SelectedAxis()
        {
            DataGridViewCell c = dgvArmMotor.CurrentCell;
            if (c == null || c.RowIndex < 0) return -1;
            return ArmMotor_RowAxis(c.RowIndex);
        }

        private int ArmMotor_RowAxis(int nRow)
        {
            if (nRow < 0 || nRow >= dgvArmMotor.Rows.Count) return -1;
            object t = dgvArmMotor.Rows[nRow].Tag;
            return (t is int) ? (int)t : -1;
        }

        private static string ArmMotor_Join(string a, string b, string strSep)
        {
            a = (a == null) ? "" : a;
            b = (b == null) ? "" : b;
            if (a.Length == 0) return b;
            if (b.Length == 0) return a;
            return a + strSep + b;
        }

        /// <summary>실행 폴더 아래면 상대 경로로 (표시용)</summary>
        private static string ArmMotor_ShortPath(string strPath)
        {
            if (string.IsNullOrEmpty(strPath)) return "";
            string strBase = Application.StartupPath.TrimEnd('\\') + "\\";
            return strPath.StartsWith(strBase, StringComparison.OrdinalIgnoreCase) ? strPath.Substring(strBase.Length) : strPath;
        }

        /// <summary>로봇이 바뀔 때 저장 안 된 수정을 보관할 곳 — urdf\armsetup\motors_&lt;옛 서명&gt;.autosave.ojw</summary>
        private static string ArmMotor_AutosavePath(string strUrdfDir, int[] anAxes)
        {
            return Path.Combine(Path.Combine(strUrdfDir, "armsetup"), Path.GetFileNameWithoutExtension(Ojw.CArmMotorSet_t.DefaultOjwName(anAxes)) + ".autosave.ojw");
        }

        /// <summary>.ojw 대화의 처음 폴더·파일 이름 — 이 서명의 마지막 .ojw 가 있으면 그 폴더·이름, 없으면 마지막으로 쓴 폴더(없으면 urdf\armsetup) + motors_&lt;서명&gt;.ojw</summary>
        private static void ArmMotor_OjwDialogStart(string strUrdf, int[] anAxes, out string strInitDir, out string strFileName)
        {
            strFileName = Ojw.CArmMotorSet_t.DefaultOjwName(anAxes);
            strInitDir = Path.Combine(strUrdf, "armsetup");
            string strSigPath = Ojw.CArmMotorSet_t.GetRecentOjw(strUrdf, Ojw.CArmMotorSet_t.SignatureOf(anAxes));
            string strLast = Ojw.CArmMotorSet_t.GetRecentOjw(strUrdf, "last");
            try
            {
                if (strSigPath != null && Directory.Exists(Path.GetDirectoryName(strSigPath)))
                {
                    strInitDir = Path.GetDirectoryName(strSigPath);
                    strFileName = Path.GetFileName(strSigPath);
                }
                else if (strLast != null && Directory.Exists(Path.GetDirectoryName(strLast))) strInitDir = Path.GetDirectoryName(strLast);
            }
            catch { }                                           // 기록에 이상한 경로가 있으면 기본값 그대로
        }

        /// <summary>[저장]·창 닫기·로봇 바뀜 전 확인의 저장 — 디자이너 파일(.ojw) 위치를 고르게 하고(매번) DLL SaveDesignerFile 로 쓴다.
        /// 고른 경로는 ojw_paths.txt 에 서명별로 남는다 (다음에 로봇이 바뀌면 저절로 불러옴). 취소·실패면 false</summary>
        private bool ArmMotor_SaveEdit()
        {
            if (m_ArmEdit == null || m_ArmEdit.Motors.Count == 0) return false;
            string strDirWhy;
            string strDir = ArmMotor_UrdfDir(out strDirWhy);
            if (strDir == null)
            {
                ArmMotor_MsgBox("모터 세팅을 저장하지 않았습니다 — " + strDirWhy, MessageBoxIcon.Warning);
                return false;
            }
            string strInitDir, strName, strPath;
            ArmMotor_OjwDialogStart(strDir, m_ArmEdit.Axes, out strInitDir, out strName);
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "모터 세팅 저장 (디자이너 파일) — ID " + Ojw.CArmMotorSet_t.RangeText(m_ArmEdit.Axes);
                dlg.Filter = "디자이너 파일 (*.ojw)|*.ojw";
                dlg.DefaultExt = "ojw";
                dlg.AddExtension = true;
                dlg.OverwritePrompt = true;
                dlg.FileName = strName;
                try { Directory.CreateDirectory(strInitDir); } catch { }
                if (Directory.Exists(strInitDir)) dlg.InitialDirectory = strInitDir;
                m_bArmModal = true;
                try { if (dlg.ShowDialog(this) != DialogResult.OK) return false; }
                finally { m_bArmModal = false; }
                strPath = dlg.FileName;
            }
            if (string.IsNullOrEmpty(m_ArmEdit.strRobot) && !string.IsNullOrEmpty(m_strLastExampleName)) m_ArmEdit.strRobot = m_strLastExampleName;   // 표시용
            string strWhy;
            if (!m_ArmEdit.SaveDesignerFile(m_C3d, strPath, out strWhy))
            {
                ArmMotor_MsgBox("모터 세팅을 저장하지 못했습니다:\r\n" + strWhy, MessageBoxIcon.Error);
                return false;
            }
            string strRecentWhy;
            if (!Ojw.CArmMotorSet_t.SetRecentOjw(strDir, m_ArmEdit.Signature, strPath, out strRecentWhy))
                ArmMotor_Log("★" + strRecentWhy + " — 다음에 로봇이 바뀔 때 이 파일을 저절로 불러오지 못할 수 있음");
            m_ArmSaved = m_ArmEdit.Clone();
            m_strArmFile = strPath;
            ArmMotor_Log("모터 세팅 저장 (디자이너 파일): " + strPath);
            if (strWhy.Length > 0)
            {
                ArmMotor_Log("★" + strWhy.Replace("\r\n", " / "));
                ArmMotor_SetMsg("저장함 — 다시 읽은 값이 다른 칸 있음 (Log 탭)", true);
                ArmMotor_MsgBox("저장했지만 파일에서 다시 읽은 값이 표와 다른 칸이 있습니다:\r\n\r\n" + strWhy, MessageBoxIcon.Warning);
            }
            else ArmMotor_SetMsg("저장함 — " + ArmMotor_ShortPath(strPath), false);
            ArmMotor_FillGrid(false);
            return true;
        }

        // ════════════════════════════════════════════════════════════
        // 연결 줄
        // ════════════════════════════════════════════════════════════

        private void btnArmConnect_Click(object sender, EventArgs e)
        {
            if (m_ArmConn == null) return;
            if (ArmMotor_IsBusy()) { ArmMotor_MsgBox("로봇팔이 사용 중입니다 — 끝난 뒤 다시.", MessageBoxIcon.Information); return; }
            if (!m_ArmConn.IsConnected)
            {
                int nPort, nBaud;
                string strBaudWarn;
                if (!Ojw.CArmConn_t.ParsePort(txtArmPort.Text, out nPort)) { ArmMotor_MsgBox("포트를 읽지 못했습니다 — 예: COM4", MessageBoxIcon.Warning); return; }
                if (!Ojw.CArmConn_t.ParseBaud(txtArmBaud.Text, out nBaud, out strBaudWarn)) { ArmMotor_MsgBox("통신 속도를 읽지 못했습니다 — 예: 1000000", MessageBoxIcon.Warning); return; }
                if (Extra_IsBusy()) { ArmMotor_MsgBox("다른 측정이 진행 중입니다 — 끝난 뒤 연결하십시오 (같은 포트를 쓸 수 있습니다).", MessageBoxIcon.Warning); return; }
                int[] anAxes = (int[])m_anArmAxes.Clone();
                if (anAxes.Length == 0) { ArmMotor_MsgBox("EDH 에 모터가 없습니다 — 예제를 먼저 불러오십시오.", MessageBoxIcon.Warning); return; }
                if (strBaudWarn.Length > 0) ArmMotor_Log(strBaudWarn);
                bool bOk = false;
                string strRep = "";
                ArmMotor_SetMsg("COM" + nPort.ToString(CultureInfo.InvariantCulture) + " 연결 중 …", false);
                ArmMotor_Run("연결", delegate() { bOk = m_ArmConn.Connect(anAxes, nPort, nBaud, out strRep); return null; },
                    delegate(string r)
                    {
                        ArmMotor_Log((bOk ? "연결 보고:\r\n" : "연결 실패: ") + strRep);
                        if (!bOk)
                        {
                            ArmMotor_SetMsg("연결 실패 — " + ArmMotor_FirstLine(strRep), true);
                            ArmMotor_MsgBox("연결하지 못했습니다:\r\n\r\n" + strRep, MessageBoxIcon.Warning);
                            return;
                        }
                        ArmMotor_SetMsg(ArmMotor_FirstLine(strRep), false);
                        ArmMotor_FillGrid(false);
                        if (m_ArmConn.IsTorqueOn)       // 서보가 이미 토크를 붙잡고 있던 경우 — 연결은 그 상태를 이어받는다 (새로 켜지 않음)
                        {
                            string strComp = ArmMotor_CompBlockReason();
                            ArmMotor_MsgBox("서보 토크가 이미 켜져 있던 상태를 이어받았습니다 (새로 켜지 않음).\r\n" +
                                            "따라가기는 토크 OFF 에서만 됩니다. 연결 해제·창 닫기 때 토크를 어떻게 할지 묻습니다.\r\n\r\n" +
                                            ((strComp.Length > 0) ? "★" + strComp + " — 조그(방향 확인)는 막힙니다: " + ArmMotor_CompBlockHint(strComp) + "\r\n\r\n" : "") +
                                            strRep, MessageBoxIcon.Information);
                        }
                    });
            }
            else
            {
                bool bTorqueOff = false;
                if (m_ArmConn.IsTorqueOn)
                {
                    DialogResult dr = ArmMotor_AskTorqueOnDisconnect("연결 해제");
                    if (dr == DialogResult.Cancel) return;
                    bTorqueOff = (dr == DialogResult.Yes);
                }
                ArmMotor_StopFollow();
                bool bOk = false;
                ArmMotor_Run("연결 해제", delegate() { bOk = m_ArmConn.Disconnect(bTorqueOff); return null; },
                    delegate(string r)
                    {
                        if (!bOk) { ArmMotor_SetMsg("연결 해제 거부 — Log 탭 확인", true); return; }
                        ArmMotor_SetMsg("연결 해제" + (bTorqueOff ? " (토크 끔)" : " (토크 그대로)"), false);
                        ArmMotor_FillGrid(false);
                    });
            }
        }

        private void btnArmTorque_Click(object sender, EventArgs e)
        {
            if (m_ArmConn == null || !m_ArmConn.IsConnected || ArmMotor_IsBusy()) return;
            if (!m_ArmConn.IsTorqueOn)
            {
                string strAsk = "토크를 켭니다 — 지금 자세 그대로 힘이 들어갑니다. 움직이지 않습니다.";
                // 처짐 보정이 켜져 있으면 조그(방향 확인)는 막힌다는 것만 알린다 (10-03: 보정을 끄려고 토크 켜기 전에 끌 필요가 없다 —
                // 토크를 켠 뒤 움직이기 전에 ⑦ 에서 끄면 남는 보정이 없다. 예전의 [예] 보정 끄고 켜기 / [아니오] / [취소] 세 갈래는 없앴다)
                if (ArmMotor_CompJointOn())
                    strAsk += "\r\n\r\n★처짐 보정이 켜져 있습니다 ('측정·학습·시험' 페이지 ⑦ '처짐 보정 켬'). 보정이 켜져 있는 동안 조그(방향 확인)는 막힙니다 — " +
                              "조그하려면 토크를 켠 뒤 움직이기 전에 ⑦ 에서 끄십시오 (남는 보정 없이 바로 조그할 수 있습니다).";
                if (!ArmMotor_Confirm(strAsk + "\r\n\r\n켤까요?", "토크 켜기")) return;
                ArmMotor_StopFollow();
                bool bOk = false;
                string strWhy = "";
                ArmMotor_Run("토크 켜기", delegate() { bOk = m_ArmConn.TorqueOn(null, out strWhy); return null; },
                    delegate(string r)
                    {
                        if (!bOk)
                        {
                            ArmMotor_SetMsg("토크를 켜지 못함 — " + ArmMotor_FirstLine(strWhy), true);
                            ArmMotor_MsgBox("토크를 켜지 못했습니다:\r\n\r\n" + strWhy, MessageBoxIcon.Warning);
                            return;
                        }
                        string strComp = ArmMotor_CompBlockReason();   // 보정 켠 채 켰으면 조그 막힘을 알린다
                        if (strWhy.Length > 0)
                        {
                            // U5 — 읽기 실패·리밋 밖은 토크를 막지 않고 경고만 온다. 움직이는 동작은 시작 전에 다시 확인한다
                            ArmMotor_Log("토크 ON — 경고: " + strWhy.Replace("\r\n", " / "));
                            ArmMotor_SetMsg("토크 ON — 경고 있음 (Log 탭)" + ((strComp.Length > 0) ? " · 조그 막힘 (" + strComp + ")" : ""), true);
                            ArmMotor_MsgBox("토크를 켰습니다. 경고:\r\n\r\n" + strWhy +
                                            "\r\n\r\n움직이는 동작(조그·측정·시험)은 시작 전에 3D 와 실물이 맞는지 다시 확인합니다." +
                                            ((strComp.Length > 0) ? "\r\n\r\n★" + strComp + " — 조그(방향 확인)는 막힙니다: " + ArmMotor_CompBlockHint(strComp) : ""),
                                            MessageBoxIcon.Warning);
                        }
                        else if (strComp.Length > 0) ArmMotor_SetMsg("토크 ON — " + strComp + " → 조그 막힘: " + ArmMotor_CompBlockHint(strComp), true);
                        else ArmMotor_SetMsg("토크 ON", false);
                    });
            }
            else
            {
                if (!ArmMotor_Confirm("토크를 끕니다 — 팔이 처지거나 떨어질 수 있으니 손으로 받치십시오.\r\n받친 뒤 [예] 를 누르십시오.", "토크 끄기")) return;
                bool bOk = false;
                ArmMotor_Run("토크 끄기", delegate() { bOk = m_ArmConn.TorqueOff(null); return null; },
                    delegate(string r)
                    {
                        if (!bOk) ArmMotor_SetMsg("토크 끄기 거부 — Log 탭 확인", true);
                        else ArmMotor_SetMsg("토크 OFF", false);
                    });
            }
        }

        // ════════════════════════════════════════════════════════════
        // 모터 세팅 버튼
        // ════════════════════════════════════════════════════════════

        private void btnArmRead_Click(object sender, EventArgs e)
        {
            if (m_ArmConn == null || !m_ArmConn.IsConnected || ArmMotor_IsBusy()) return;
            Ojw.CArmMotorState_t[] aSt = null;
            string strWhy = "";
            ArmMotor_Run("읽기", delegate() { aSt = m_ArmConn.ReadState(null, out strWhy); return null; },
                delegate(string r)
                {
                    if (aSt == null) { ArmMotor_SetMsg("읽기 실패 — " + ArmMotor_FirstLine(strWhy), true); ArmMotor_Log("읽기 실패: " + strWhy); return; }
                    // 표의 기종이 미지정이라 실물 기종으로 채운 축 — 적용된 세트에는 DLL 이 이미 넣었다. 편집본에도 옮긴다 (저장해야 남는다)
                    int nFill = 0;
                    foreach (Ojw.CArmMotorState_t s in aSt)
                    {
                        string w;
                        if (s != null && s.bModelFilled && m_ArmEdit != null &&
                            m_ArmEdit.SetField(s.nAxis, "model", s.nModel.ToString(CultureInfo.InvariantCulture), out w)) nFill++;
                    }
                    string strMsg = "읽기 — " + aSt.Length + "축" + ((strWhy.Length > 0) ? " · " + strWhy.Replace("\r\n", " / ") : "") +
                                    ((nFill > 0) ? " · 기종 " + nFill + "축 채움 (저장 안 됨)" : "");
                    ArmMotor_SetMsg(strMsg, strWhy.Length > 0);
                    ArmMotor_Log(strMsg);
                    foreach (Ojw.CArmMotorState_t s in aSt)
                        if (s != null && s.strNote.Length > 0) ArmMotor_Log("  ID " + s.nAxis + ": " + s.strNote);
                    ArmMotor_FillGrid(false);
                });
        }

        private void btnArmJog_Click(object sender, EventArgs e)
        {
            if (m_ArmConn == null || ArmMotor_IsBusy()) return;
            string strComp = ArmMotor_CompBlockReason();
            if (strComp.Length > 0)
            {
                // 보정이 들어오거나 빠지는 동안은 조그의 모션 틱마다 그 일부가 얹혀 고르지 않은 관절도 움직인다 (튀지는 않는다 — 머리 주석) — 막고 절차를 알린다
                ArmMotor_SetMsg("조그 막힘 (" + strComp + ") — " + ArmMotor_CompBlockHint(strComp), true);
                ArmMotor_Log("조그 막힘 — " + strComp);
                ArmMotor_MsgBox(ArmMotor_CompBlockText(strComp), MessageBoxIcon.Warning);
                return;
            }
            Button b = sender as Button;
            int nSign = (b != null && b.Tag != null && b.Tag.ToString() == "-1") ? -1 : 1;
            int nAxis = ArmMotor_SelectedAxis();
            if (nAxis < 0) { ArmMotor_SetMsg("표에서 움직일 축(행)을 고르십시오", true); return; }
            float fStep;
            if (!float.TryParse(txtArmJogStep.Text.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out fStep) ||
                float.IsNaN(fStep) || fStep <= 0f || fStep > m_ArmConn.JogMaxDeg)
            {
                ArmMotor_MsgBox("조그 각은 0 보다 크고 " + m_ArmConn.JogMaxDeg.ToString("0.#", CultureInfo.InvariantCulture) + "° 이하인 숫자로 넣으십시오 (예: 3).", MessageBoxIcon.Warning);
                return;
            }
            float fDelta = nSign * fStep;
            Ojw.CArmJogResult_t res = null;
            bool bOk = false;
            string strWhy = "";
            ArmMotor_Run("조그", delegate()
                {
                    string strC = ArmMotor_CompBlockReason();          // 누른 뒤 보정 상태가 바뀌었으면 움직이지 않는다
                    if (strC.Length > 0) { strWhy = "조그 막힘 — " + strC; return null; }
                    bOk = m_ArmConn.Jog(nAxis, fDelta, out res, out strWhy);
                    return null;
                },
                delegate(string r)
                {
                    if (!bOk || res == null)
                    {
                        ArmMotor_SetMsg("조그 거부 — " + ArmMotor_FirstLine(strWhy), true);
                        ArmMotor_MsgBox("조그를 하지 않았습니다:\r\n\r\n" + strWhy, MessageBoxIcon.Warning);
                        return;
                    }
                    CultureInfo ci = CultureInfo.InvariantCulture;
                    string strRes = string.Format(ci, "조그 ID {0}: 명령 Δ{1:+0.0;-0.0}° / 실측 Δ{2:+0.0;-0.0}°", nAxis, res.fTo - res.fFrom, res.fRealDelta) +
                                    ((res.strNote.Length > 0) ? " — " + res.strNote : "");
                    ArmMotor_SetMsg(strRes, res.strNote.Length > 0);
                    // 방향이 틀려도 읽기·쓰기가 같은 세팅이라 실측 부호는 명령과 같게 나온다 — 방향은 사람 눈으로만 판정한다.
                    // DLL 이 실측 이상(거의 안 움직임·따라가지 못함·읽기 실패)을 알렸으면 판단하지 말라고 먼저 적는다 (기본 버튼은 늘 취소)
                    string strOdd = (res.strNote.Length > 0) ? "★" + res.strNote + " — 실물이 제대로 움직이지 않았으면 방향을 정하지 말고 [취소]\r\n\r\n" : "";
                    DialogResult dr = ArmMotor_Ask(strOdd + "ID " + nAxis + " 실물이 3D 와 같은 방향으로 움직였습니까?\r\n(" + strRes + ")\r\n\r\n" +
                                                   "[예]  같음 — 이 줄을 '확인' 으로\r\n" +
                                                   "[아니오]  반대 — 이 줄의 방향을 바꿉니다 (실물은 움직이지 않음 — 다시 조그해서 확인)\r\n" +
                                                   "[취소]  아무것도 바꾸지 않음",
                                                   "방향 확인", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dr == DialogResult.Cancel || m_ArmEdit == null) return;
                    Ojw.CArmMotorSet_t prev = m_ArmEdit.Clone();
                    string w;
                    if (dr == DialogResult.Yes)
                    {
                        if (!m_ArmEdit.SetField(nAxis, "checked", "1", out w)) { ArmMotor_SetMsg("확인 표시 거부: " + w, true); return; }
                        ArmMotor_ApplyEdit(prev, "ID " + nAxis + " 방향 확인", w, null);
                    }
                    else
                    {
                        string strFlip = (m_ArmEdit.GetField(nAxis, "dir") == "정") ? "역" : "정";
                        if (!m_ArmEdit.SetField(nAxis, "dir", strFlip, out w)) { ArmMotor_SetMsg("방향 바꾸기 거부: " + w, true); return; }
                        ArmMotor_ApplyEdit(prev, "ID " + nAxis + " 방향 → " + strFlip, w, delegate()
                        {
                            ArmMotor_MsgBox("ID " + nAxis + " 의 방향을 '" + strFlip + "' 으로 바꿨습니다 (실물은 움직이지 않았습니다).\r\n다시 조그해서 같은 방향인지 확인하십시오.", MessageBoxIcon.Information);
                        });
                    }
                });
        }

        private void btnArmSetZero_Click(object sender, EventArgs e)
        {
            if (m_ArmConn == null || !m_ArmConn.IsConnected || ArmMotor_IsBusy()) return;
            int nAxis = ArmMotor_SelectedAxis();
            if (nAxis < 0) { ArmMotor_SetMsg("표에서 영점을 정할 축(행)을 고르십시오", true); return; }
            int nTick = 0, nNew = 0;
            string strWarn = "";
            bool bOk = false;
            ArmMotor_Run("영점 읽기", delegate() { bOk = m_ArmConn.PreviewZero(nAxis, 0f, out nTick, out nNew, out strWarn); return null; },
                delegate(string r)
                {
                    if (!bOk) { ArmMotor_SetMsg("영점을 정할 수 없음 — " + ArmMotor_FirstLine(strWarn), true); ArmMotor_MsgBox("영점을 정할 수 없습니다:\r\n\r\n" + strWarn, MessageBoxIcon.Warning); return; }
                    Ojw.CArmMotorSet_t cur = m_ArmConn.Motors;
                    Ojw.CArmMotor_t m = (cur != null) ? cur.Get(nAxis) : null;
                    if (m == null) return;
                    float fDeg;
                    bool bLarge = Ojw.CArmMotorSet_t.IsLargeZeroChange(m, nNew, out fDeg);
                    CultureInfo ci = CultureInfo.InvariantCulture;
                    string s = string.Format(ci, "ID {0} 의 지금 위치(틱 {1})를 0° 로 정합니다.\r\n영점 {2} → {3} ({4:+0.0;-0.0}°)\r\n\r\n", nAxis, nTick, m.nCenter_Evd, nNew, fDeg) +
                               "3D 의 0° 모양과 실물 관절이 같은 모양입니까?" + (m_ArmConn.IsTorqueOn ? "\r\n토크 ON 이면 처짐만큼 어긋날 수 있습니다." : "");
                    if (m_C3d != null && m_C3d.SagComp_Get() != null) s += "\r\n\r\n★" + Ojw.CArmMotorSet_t.RetrainWarnText;   // U3
                    if (strWarn.Length > 0) s += "\r\n\r\n" + strWarn;
                    if (!ArmMotor_Confirm(s, "현재 위치를 0 으로")) return;
                    if (bLarge && !ArmMotor_Confirm(string.Format(ci, "영점이 {0:0}° 넘게 바뀝니다 ({1:+0.0;-0.0}°).\r\n관절 모양을 한 번 더 확인하십시오 — 정말 바꿀까요?",
                                                                  Ojw.CArmMotorSet_t.ZERO_RECONFIRM_DEG, fDeg), "현재 위치를 0 으로 — 다시 확인")) return;
                    bool bOk2 = false;
                    string strWhy = "";
                    int nExpect = nNew;
                    ArmMotor_Run("영점 적용", delegate() { bOk2 = m_ArmConn.SetZeroHere(nAxis, 0f, nExpect, out strWhy); return null; },
                        delegate(string r2)
                        {
                            if (!bOk2) { ArmMotor_SetMsg("영점을 바꾸지 못함 — " + ArmMotor_FirstLine(strWhy), true); ArmMotor_MsgBox("영점을 바꾸지 못했습니다:\r\n\r\n" + strWhy, MessageBoxIcon.Warning); return; }
                            // 적용된 세트의 영점(다시 읽어 정한 값 — 미리보기와 3틱 안)을 표에 묶인 편집본에도 (저장해야 남는다)
                            Ojw.CArmMotorSet_t applied = m_ArmConn.Motors;
                            Ojw.CArmMotor_t mApplied = (applied != null) ? applied.Get(nAxis) : null;
                            int nCenter = (mApplied != null) ? mApplied.nCenter_Evd : nExpect;
                            string w;
                            if (m_ArmEdit != null) m_ArmEdit.SetField(nAxis, "center", nCenter.ToString(CultureInfo.InvariantCulture), out w);
                            string strMsg = "ID " + nAxis + " 영점 → " + nCenter + " (저장 안 됨)" + ((strWhy.Length > 0) ? " — " + strWhy.Replace("\r\n", " / ") : "");
                            ArmMotor_SetMsg(strMsg, strWhy.Length > 0);
                            ArmMotor_Log(strMsg);
                            ArmMotor_FillGrid(false);
                        });
                });
        }

        private void btnArmSave_Click(object sender, EventArgs e)
        {
            if (ArmMotor_IsBusy()) return;
            ArmMotor_SaveEdit();
        }

        private void btnArmLoad_Click(object sender, EventArgs e)
        {
            if (m_ArmConn == null || ArmMotor_IsBusy() || m_ArmEdit == null || m_ArmEdit.Motors.Count == 0) return;
            string strDirWhy;
            string strUrdf = ArmMotor_UrdfDir(out strDirWhy);
            if (strUrdf == null)
            {
                ArmMotor_MsgBox("모터 세팅 불러오기 — " + strDirWhy, MessageBoxIcon.Warning);
                return;
            }
            string strInitDir, strName, strPath;
            ArmMotor_OjwDialogStart(strUrdf, m_ArmEdit.Axes, out strInitDir, out strName);
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "모터 세팅 불러오기";
                dlg.Filter = "디자이너 파일 (*.ojw)|*.ojw|예전 모터 세팅 (motors_*.txt)|motors_*.txt|모든 파일|*.*";
                if (Directory.Exists(strInitDir)) dlg.InitialDirectory = strInitDir;
                if (File.Exists(Path.Combine(strInitDir, strName))) dlg.FileName = strName;
                m_bArmModal = true;
                try { if (dlg.ShowDialog(this) != DialogResult.OK) return; }
                finally { m_bArmModal = false; }
                strPath = dlg.FileName;
            }
            string strWhy;
            Ojw.CArmMotorSet_t set;
            bool bOjw = string.Equals(Path.GetExtension(strPath), ".ojw", StringComparison.OrdinalIgnoreCase);
            if (bOjw)
            {
                set = m_ArmEdit.Clone();                         // 이 표의 축만 — 서명은 늘 같다
                if (set.ImportDesignerFile(strPath, out strWhy) < 0) { ArmMotor_MsgBox("불러오지 못했습니다:\r\n\r\n" + strWhy, MessageBoxIcon.Warning); return; }
                ArmMotor_Log("디자이너 파일 읽음: " + strWhy.Replace("\r\n", " / "));
            }
            else
            {
                set = Ojw.CArmMotorSet_t.Load(strPath, out strWhy);
                if (set == null) { ArmMotor_MsgBox("불러오지 못했습니다:\r\n\r\n" + strWhy, MessageBoxIcon.Warning); return; }
                if (strWhy.Length > 0) ArmMotor_Log("불러오기 경고: " + strWhy.Replace("\r\n", " / "));
            }
            int[] anAxes = m_ArmEdit.Axes;
            bool bMerged = false;
            if (set.Signature != Ojw.CArmMotorSet_t.SignatureOf(anAxes))
            {
                if (!ArmMotor_Confirm("이 파일은 ID " + Ojw.CArmMotorSet_t.RangeText(set.Axes) + " 로봇 것입니다.\r\n같은 ID(" +
                                      Ojw.CArmMotorSet_t.RangeText(anAxes) + ")만 가져올까요?", "모터 세팅 불러오기")) return;
                int[] anTaken;
                set = set.MergeFor(anAxes, out anTaken);
                if (anTaken.Length == 0) { ArmMotor_MsgBox("겹치는 ID 가 없어 가져올 것이 없습니다.", MessageBoxIcon.Information); return; }
                bMerged = true;
            }
            string strDiff = Ojw.CArmMotorSet_t.DescribeMappingDiff(m_ArmEdit.MappingKey(), set.MappingKey());
            if (!ArmMotor_Confirm("불러온 모터 세팅을 적용합니다.\r\n파일: " + strPath + "\r\n\r\n바뀌는 방향·영점: " +
                                  ((strDiff.Length > 0) ? strDiff : "없음 (리밋·이름 등만)") +
                                  ((bOjw && strWhy.Length > 0) ? "\r\n\r\n" + strWhy : "") +
                                  "\r\n\r\n세팅을 바꾸는 순간 실물은 움직이지 않습니다 (3D 를 새 해석으로 옮김).\r\n적용할까요?", "모터 세팅 불러오기")) return;
            Ojw.CArmMotorSet_t prev = m_ArmEdit;
            string strDefault = Ojw.CArmMotorSet_t.DefaultPath(strUrdf, anAxes);
            // 그 파일 그대로면 저장된 상태: .ojw 는 늘 (이 표의 축만 읽음), 예전 txt 는 이 서명의 기본 경로일 때만
            bool bAsSaved = bOjw || (!bMerged && string.Equals(Path.GetFullPath(strPath), Path.GetFullPath(strDefault), StringComparison.OrdinalIgnoreCase));
            string strLoaded = strPath;
            m_ArmEdit = set;
            ArmMotor_ApplyEdit(prev, "불러오기 " + Path.GetFileName(strPath), "", delegate()
            {
                if (bAsSaved) { m_ArmSaved = set.Clone(); m_strArmFile = strLoaded; }
                if (bOjw)
                {
                    string strRecentWhy;                         // 이 서명의 마지막 .ojw 로 기억 — 다음에 로봇이 바뀌면 이것을 저절로 불러온다
                    if (!Ojw.CArmMotorSet_t.SetRecentOjw(strUrdf, set.Signature, strLoaded, out strRecentWhy)) ArmMotor_Log("★" + strRecentWhy);
                }
                ArmMotor_Log("모터 세팅 불러와 적용: " + strLoaded + ((strDiff.Length > 0) ? " (" + strDiff + ")" : ""));
            });
        }

        private void btnArmImportAuto_Click(object sender, EventArgs e)
        {
            if (m_ArmConn == null || ArmMotor_IsBusy() || m_ArmEdit == null || m_ArmEdit.Motors.Count == 0) return;
            string strDirWhy;
            string strUrdf = ArmMotor_UrdfDir(out strDirWhy);
            if (strUrdf == null)
            {
                ArmMotor_MsgBox("자동 방향 결과 가져오기 — " + strDirWhy, MessageBoxIcon.Warning);
                return;
            }
            string strPath = Ojw.CArmMotorSet_t.FindJointMap(strUrdf);
            if (strPath == null)
            {
                ArmMotor_MsgBox("자동 방향 결과(joint_map.json)가 없습니다 — urdf\\armsetup\\ 또는 urdf\\sim2real\\", MessageBoxIcon.Information);
                return;
            }
            Ojw.CArmMotorSet_t set = m_ArmEdit.Clone();
            string strWhy;
            int n = set.ImportJointMap(strPath, out strWhy);
            if (n < 0) { ArmMotor_MsgBox("가져오지 않았습니다:\r\n\r\n" + strWhy + "\r\n\r\n파일: " + strPath, MessageBoxIcon.Warning); return; }
            string strDiff = Ojw.CArmMotorSet_t.DescribeMappingDiff(m_ArmEdit.MappingKey(), set.MappingKey());
            if (!ArmMotor_Confirm(strWhy + "\r\n파일: " + strPath + "\r\n\r\n바뀌는 방향: " + ((strDiff.Length > 0) ? strDiff : "없음") +
                                  "\r\n\r\n적용할까요? (그 축들은 '자동', 확인 안 됨으로 표시됩니다)", "자동 방향 결과 가져오기")) return;
            Ojw.CArmMotorSet_t prev = m_ArmEdit;
            m_ArmEdit = set;
            ArmMotor_ApplyEdit(prev, "자동 방향 결과 가져오기", "", null);
        }

        private void chkArmFollow_CheckedChanged(object sender, EventArgs e)
        {
            if (m_bArmUiSync || m_ArmConn == null) return;
            bool bOk = m_ArmConn.IsConnected && !m_ArmConn.IsTorqueOn;
            if (chkArmFollow.Checked && !bOk) { m_bArmUiSync = true; chkArmFollow.Checked = false; m_bArmUiSync = false; }
            tmrArmFollow.Enabled = chkArmFollow.Checked;
            m_nArmFollowTick = 0;
            ArmMotor_SetMsg(chkArmFollow.Checked ? "따라가기 켬 — 관절을 손으로 돌려 3D 와 같은 방향인지 보십시오" : "따라가기 끔", false);
        }

        private void chkArmAdvanced_CheckedChanged(object sender, EventArgs e)
        {
            bool b = chkArmAdvanced.Checked;
            colArmGroup.Visible = colArmBusId.Visible = colArmMechMove.Visible = colArmMechAngle.Visible = b;
            colArmInit.Visible = colArmInit2.Visible = colArmProto.Visible = b;
        }

        /// <summary>따라가기 타이머 (100ms) — 재진입 막음, 표 칸에 커밋 안 된 입력이 있거나 커밋이 줄 서 있거나 확인창이 떠 있으면 건너뜀,
        /// 토크가 켜지면 저절로 끔. 칸을 고르기만 한 편집 모드(EditOnEnter)에서는 계속 돈다 — 그사이 들어온 커밋은 읽기가 끝난 뒤에 처리된다</summary>
        private void tmrArmFollow_Tick(object sender, EventArgs e)
        {
            if (m_bArmFollowBusy || m_ArmConn == null) return;
            if (!chkArmFollow.Checked || !m_ArmConn.IsConnected || m_ArmConn.IsTorqueOn) { ArmMotor_StopFollow(); return; }
            if (m_bArmModal || m_bArmMotorRun || m_bArmCommitPending || dgvArmMotor.IsCurrentCellDirty) return;
            m_bArmFollowBusy = true;
            try
            {
                if (m_ArmConn.FollowOnce() && (++m_nArmFollowTick % 3) == 0) ArmMotor_FillNowColumns();
            }
            catch (Exception ex) { ArmMotor_StopFollow(); ArmMotor_Log("따라가기 오류 — 멈춤: " + ex.Message); }
            finally
            {
                m_bArmFollowBusy = false;
                if (m_lstArmAfterFollow.Count > 0)
                {
                    // 읽기 안에서 들어와 미룬 일 — 이제 DoEvents 밖이므로 한 박자 뒤에 차례로
                    Action[] aDo = m_lstArmAfterFollow.ToArray();
                    m_lstArmAfterFollow.Clear();
                    foreach (Action a in aDo)
                    {
                        try { BeginInvoke(a); }
                        catch (Exception ex) { ArmMotor_Log("미룬 처리 오류: " + ex.Message); }
                    }
                }
            }
        }

        // ════════════════════════════════════════════════════════════
        // 표 편집
        // ════════════════════════════════════════════════════════════

        private void dgvArmMotor_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (m_bArmUiSync) return;
            if (m_ArmEdit == null || ArmMotor_IsBusy()) e.Cancel = true;     // 동작 중에는 표를 바꾸지 않는다
        }

        private void dgvArmMotor_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (m_bArmUiSync || !dgvArmMotor.IsCurrentCellDirty) return;
            if (ArmMotor_IsBusy())
            {
                try { dgvArmMotor.CancelEdit(); } catch { }                  // 편집하는 사이 동작이 시작됨 — 이 편집은 버린다
                ArmMotor_SetMsg("동작 중 — 편집을 취소했습니다", true);
            }
        }

        private void dgvArmMotor_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (m_bArmUiSync || !dgvArmMotor.IsCurrentCellDirty || dgvArmMotor.CurrentCell == null) return;
            DataGridViewColumn col = dgvArmMotor.Columns[dgvArmMotor.CurrentCell.ColumnIndex];
            if (col is DataGridViewComboBoxColumn || col is DataGridViewCheckBoxColumn)
                dgvArmMotor.CommitEdit(DataGridViewDataErrorContexts.Commit);   // 콤보·체크는 고르는 즉시 반영
        }

        private void dgvArmMotor_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_bArmUiSync || m_ArmEdit == null || e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string strKey;
            if (!s_dicArmColKey.TryGetValue(dgvArmMotor.Columns[e.ColumnIndex].Name, out strKey)) return;
            int nAxis = ArmMotor_RowAxis(e.RowIndex);
            if (nAxis < 0) return;
            DataGridViewCell cell = dgvArmMotor.Rows[e.RowIndex].Cells[e.ColumnIndex];
            string strVal;
            if (cell is DataGridViewCheckBoxCell) strVal = (cell.Value is bool && (bool)cell.Value) ? "1" : "0";
            else strVal = (cell.Value == null) ? "" : cell.Value.ToString();
            string strHdr = dgvArmMotor.Columns[e.ColumnIndex].HeaderText;
            int nRow = e.RowIndex, nCol = e.ColumnIndex;
            m_bArmCommitPending = true;
            // 칸 이벤트 안에서 확인창을 띄우거나 표를 다시 그리지 않게 한 박자 뒤에 처리한다
            try { BeginInvoke(new Action(delegate() { ArmMotor_CommitCell(nRow, nCol, nAxis, strKey, strHdr, strVal); })); }
            catch { m_bArmCommitPending = false; }
        }

        /// <summary>표 한 칸 커밋: SetField(편집본, DLL 검증) → (영점 45° 넘으면 다시 확인) → ApplyMotors. 거부되면 칸을 되돌리고 사유를 알림 줄·로그에.
        /// nRow·nCol = 커밋된 칸 (따라가기 읽기 뒤로 미룰 때 그대로 넘긴다). 따라가기 읽기 안에서 꺼내지면 읽기가 끝난 뒤에 한다.
        /// 되돌리기는 끝의 FillGrid 가 한다 — 커밋된 채 편집 모드로 남은 콤보·체크 칸도 ArmMotor_SetCell 이 값과 편집 컨트롤을 함께 바꾼다
        /// (거부·바쁨·45° 취소는 여기서, 적용 거부(ApplyMotors)는 ArmMotor_ApplyEdit 의 끝에서)</summary>
        private void ArmMotor_CommitCell(int nRow, int nCol, int nAxis, string strKey, string strHdr, string strVal)
        {
            if (m_bArmFollowBusy)
            {
                // 따라가기 읽기(DoEvents) 안 — 확인창·적용은 읽기가 끝난 뒤에 (m_bArmCommitPending 은 그대로 — 그동안 따라가기도 건너뛴다)
                ArmMotor_AfterFollow(delegate() { ArmMotor_CommitCell(nRow, nCol, nAxis, strKey, strHdr, strVal); });
                return;
            }
            m_bArmCommitPending = false;
            if (m_ArmEdit == null) return;
            string strWhat = "ID " + nAxis + " " + strHdr;
            try
            {
                if (ArmMotor_IsBusy()) { ArmMotor_SetMsg(strWhat + " — 동작 중이라 바꾸지 않음", true); return; }
                if (m_ArmEdit.GetField(nAxis, strKey) == strVal.Trim()) return;   // 그대로
                Ojw.CArmMotorSet_t prev = m_ArmEdit.Clone();
                string strWarn;
                if (!m_ArmEdit.SetField(nAxis, strKey, strVal, out strWarn))
                {
                    ArmMotor_SetMsg(strWhat + " 거부: " + strWarn.Replace("\r\n", " / "), true);
                    ArmMotor_Log(strWhat + " 거부: " + strWarn.Replace("\r\n", " / "));
                    return;
                }
                if (strKey == "center")
                {
                    // U3 — 영점이 45° 넘게 바뀌면 한 번 더 확인 (판정은 DLL)
                    Ojw.CArmMotor_t mOld = prev.Get(nAxis), mNew = m_ArmEdit.Get(nAxis);
                    float fDeg;
                    if (mOld != null && mNew != null && Ojw.CArmMotorSet_t.IsLargeZeroChange(mOld, mNew.nCenter_Evd, out fDeg) &&
                        !ArmMotor_Confirm(string.Format(CultureInfo.InvariantCulture, "ID {0} 영점이 {1:+0.0;-0.0}° 바뀝니다 ({2} → {3}).\r\n관절 모양을 한 번 더 확인하십시오 — 바꿀까요?",
                                                        nAxis, fDeg, mOld.nCenter_Evd, mNew.nCenter_Evd), "영점 바꾸기"))
                    {
                        m_ArmEdit = prev;
                        ArmMotor_SetMsg(strWhat + " — 취소", false);
                        return;
                    }
                }
                ArmMotor_ApplyEdit(prev, strWhat + " = " + strVal.Trim(), strWarn, null);
            }
            finally
            {
                // 표를 편집본으로 — 거부·취소된 칸은 편집본(그대로) 값으로 돌아간다. 콤보·체크 칸은 CommitEdit 뒤에도 편집 모드로 남지만
                // (EditOnEnter — EndEdit 로도 끝나지 않는다) 커밋돼 있으므로 ArmMotor_SetCell 이 값을 바꾸고 편집 컨트롤도 다시 그린다
                ArmMotor_FillGrid(false);
            }
        }

        private void dgvArmMotor_SelectionChanged(object sender, EventArgs e)
        {
            if (m_bArmUiSync || m_C3d == null || m_ArmConn == null) return;
            int nAxis = ArmMotor_SelectedAxis();
            m_C3d.SelectMotor((nAxis >= 0) ? nAxis : -1);       // 3D 에 그 관절 강조
            bool bConn = m_ArmConn.IsConnected, bBusy = ArmMotor_IsBusy();
            btnArmJogMinus.Enabled = btnArmJogPlus.Enabled = bConn && m_ArmConn.IsTorqueOn && nAxis >= 0 && !bBusy && !m_ArmConn.ConfigMismatch;
            btnArmSetZero.Enabled = bConn && nAxis >= 0 && !bBusy;
        }

        private void dgvArmMotor_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ArmMotor_Log("표 값 오류 (행 " + e.RowIndex + ", 열 " + e.ColumnIndex + "): " + ((e.Exception != null) ? e.Exception.Message : ""));
            e.ThrowException = false;
        }

        private static string ArmMotor_FirstLine(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            int n = s.IndexOf('\r');
            return (n >= 0) ? s.Substring(0, n) : s;
        }
    }
}
