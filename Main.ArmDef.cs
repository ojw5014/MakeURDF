// ================================================================
// 로봇팔 세팅 탭 — 로봇 정의 페이지 · 실습 카드 · 수직 눈 확인 (2026-10-04)
//   측정·실습이 쓰는 로봇 정의 = ① 공구 끝 (IK 수식 체인의 끝 — 첫 '!' 바로 앞 DH 줄, '!' 가 없으면 체인 끝)
//   ② 꺾는 손목 (공구를 바닥으로 수직으로 꺾는 관절 — 공구 끝 앞 마지막 관절)
//   ③ 바닥으로 수직 (공구축 = 손목 원점 → 공구 끝 이 3D 의 −Z 일 때의 관절 합 피치1 + 피치2 + 손목 = downsum — 미리 정해 두는 값).
//   정본은 <urdf>\sagarm.txt 의 7키 (robot·func·posids·wrist·holdids·tcplen·downsum — DLL CSagArmSetup). 다른 곳은 채워 쓰거나 대조만 한다.
//   확인·계산은 전부 DLL (Ojw.C3d.SagDef_* — COjw_12_3D_SagDef.cs): 칸이 바뀔 때마다 후보 정의를 지금 3D 와 대조하고 (캐시 — 가볍다)
//   맞을 때만 [확인·저장]. 저장은 고급 페이지 입력과 같은 길 — 파일과 맞춤(ArmSetup_SyncCfg) → DLL 키 규칙(ArmSetup_CheckCfg) → sagarm.txt (ArmSetup_SaveCfg),
//   저장하지 못하면 대화창으로 까닭.
//   계획 기구학은 지금 3D 의 수식으로 맞춘다 (DLL SagRun_Prepare — UI 스레드, 3D 컨트롤은 DLL 이 처음 한 번만 만들고 DH 행이 바뀌면 수식만 바꾼다):
//   로봇팔 세팅 탭을 보고 있을 때 3D 모델을 다시 해석하면 MakeAll 끝에서 (ArmDef_ModelChanged), 아니면 탭을 볼 때·작업 전에. 맞추지 못하면 로그에 ★ 까닭.
//   본보기 = <urdf>\sagarm_defs\<이름>.txt (정의 7키만) — 고르면 칸만 채운다 (저장 안 함). 작업은 본보기를 읽지 않는다.
//   3D 표시 (DLL ToolMark_Set — 공구 끝 빨강 · 공구축 초록(수직)/파랑 · 손목 주황): 로봇팔 세팅 탭에서 이 페이지를 보는 동안은 고치는 중인 후보 정의로,
//   탭 안의 다른 페이지에서는 실습 카드의 [3D 표시] 체크와 저장된 정의로, 탭 밖이면 끔. 수직 눈 확인 대화창이 떠 있는 동안은 그 작업의 정의로 늘 켠다.
//   수직 눈 확인: 이 페이지의 [수직 눈 확인] (실습 카드는 기록이 없을 때 [눈 확인하기] 링크) 이 눈 확인만 하는 작업(DLL SagPrac_EyeJob — 펼치기 → 묻기 →
//   기록 → 원위치, 바닥은 누르지 않고 실습 진행은 그대로)을 띄운다 — 실물은 확인창 [예] 뒤에만 (기본 [아니오]), 연결이 없으면 3D 로만 미리 본다 (기록 없음).
//   실물 1단계(DLL SagPrac_ErrorJob)도 처음이면 펼친 자리에서 묻는다. 둘 다 작업 스레드로 묻고 → UI 스레드로 Invoke 해 대화창 (기본 [아니오]).
//   눈 확인 전이면 다른 실물 바닥 작업(③ 측정·⑤ 닿기 시험·실습 2·3단계)은 시작하지 않고 이 버튼을 알린다 (ArmDef_EyeGate — DLL 작업 입구도 거절).
//   UI 스레드는 작업을 기다리지 않으므로 (ArmSetup_Run → Extra_Run 스레드) 서로 멈추지 않는다.
//   컨트롤은 Main.Designer.cs 에 있다 (tabArmSetup → tabArmInner → tabArmDef '로봇 정의', 카드는 tabArmPrac).
// ================================================================
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using OpenJigWare;

namespace MakeUrdf
{
    public partial class Main
    {
        private bool m_bArmDefInit = false;               // 정의 페이지 준비됨 (ArmDef_Init)
        private bool m_bArmDefUiSync = false;             // 정의 칸을 코드로 바꾸는 중 (이벤트 무시)
        private bool m_bArmDefDownEdited = false;         // ③ 을 사용자가 고침 — 구조(① ②·베이스·피치)가 바뀌어도 3D 에서 잰 값으로 다시 채우지 않는다
        private bool m_bArmDefLenEdited = false;          // 공구 끝 거리(고급)를 사용자가 고침 — 같은 규칙
        private bool m_bArmDefCanSave = false;            // 후보 정의가 3D 와 맞고 저장된 정의와 다름 ([확인·저장] — 놀 때만 켠다)
        private Ojw.C3d.CSagArmCfg m_ArmDefCand = null;   // 칸의 후보 정의 (칸을 읽지 못하면 null)
        private Ojw.C3d.CSagDefCheck m_ArmDefChk = null;  // 그 확인 (DLL SagDef_Check)
        private bool m_bArmEyeAsking = false;             // 수직 눈 확인 대화창이 떠 있음 — 3D 표시를 그 작업의 정의로 켠다
        private Ojw.C3d.CSagArmCfg m_ArmEyeCfg = null;    // 그 작업의 정의
        private string m_strArmPrepWhy = "";              // 마지막으로 계획 기구학을 지금 3D 로 맞추지 못한 까닭 (같은 까닭은 로그에 한 번)
        private readonly List<int> m_lstArmDefFuncs = new List<int>();          // cmbDefTcp 항목의 수식 번호
        private readonly List<int> m_lstArmDefWrists = new List<int>();         // cmbDefWrist 항목의 관절 번호
        private readonly List<string> m_lstArmDefPresets = new List<string>();  // cmbDefPreset 항목 [2..] 의 본보기 파일 (전체 경로)
        private const int ARMDEF_PRESET_FIRST = 2;        // cmbDefPreset: [0] 지금 설정 (sagarm.txt) · [1] OMX follower (내장) · [2..] 본보기 파일
        // 수직 눈 확인 안내 (기록이 없을 때 — 실습 카드 툴팁·정의 페이지 눈 확인 줄 툴팁)
        private const string ARMDEF_EYE_HOWTO = "수직 눈 확인 전 — 실물 ③ 측정·⑤ 닿기 시험·실습 2·3단계 전에 이 로봇·정의·모터 영점으로 한 번: '로봇 정의' 페이지의 [수직 눈 확인] (실습 카드의 [눈 확인하기]) — 팔을 바닥 위 옮기는 높이에서 수직 자세로 펼쳐 실물 펜이 수직인지 묻고 기록합니다 (바닥은 누르지 않고 실습 진행은 그대로)";

        // ════════════════════════════════════════════════════════════
        // 실패 안내·실습 카드
        // ════════════════════════════════════════════════════════════

        /// <summary>정의 확인 실패 종류별 안내 — 작업 버튼이 꺼진 까닭 뒤에 (실습 안내 줄·실습 확인창·고급 페이지 맨 아래 줄)</summary>
        private static string ArmDef_KinHint(Ojw.C3d.ESagDefFail kind)
        {
            switch (kind)
            {
                case Ojw.C3d.ESagDefFail.NoModel: return "3D 에 로봇 모델(수식)이 없습니다 — 로봇 모델을 불러온 뒤 이 탭을 다시 여십시오 (OMX follower 는 예제 5·6)";
                case Ojw.C3d.ESagDefFail.Def: return "'로봇 정의' 페이지([정의 바꾸기])에서 고치십시오";
                case Ojw.C3d.ESagDefFail.Model:
                case Ojw.C3d.ESagDefFail.Changed: return "3D 가 바뀌었습니다 — 이 탭을 다시 여십시오";
                case Ojw.C3d.ESagDefFail.NoKin: return "앱을 다시 켜십시오";
                default: return "이 탭을 다시 여십시오";
            }
        }

        /// <summary>까닭 + 종류별 안내 (" — " 로 잇는다). 까닭이 이미 그 안내(또는 안내의 끝 말 — '이 탭을 다시 여십시오' 등)를 담고 있으면 까닭만 —
        /// 같은 말을 두 번 보이지 않는다 (DLL 의 '계획 기구학을 만들지 못했습니다 — 앱을 다시 켜십시오' 같은 글)</summary>
        private static string ArmDef_WhyHint(string why, Ojw.C3d.ESagDefFail kind)
        {
            string h = ArmDef_KinHint(kind);
            if (string.IsNullOrEmpty(why)) return h;
            int k = h.LastIndexOf(" — ", StringComparison.Ordinal);
            string tail = (k >= 0) ? h.Substring(k + 3) : h;
            if (why.IndexOf(h, StringComparison.Ordinal) >= 0 || why.IndexOf(tail, StringComparison.Ordinal) >= 0) return why;
            return why + " — " + h;
        }

        /// <summary>실습 카드 (lblPracDef) — 정의 확인 상태 · 꺾는 손목 · 수직 눈 확인 (쉬운 말). 툴팁 = DLL 확인 요약·숫자·눈 확인 시각.
        /// 확인 상태는 탭을 볼 때·3D 모델이 바뀔 때의 기구학 확인 (m_bArmKinOk·m_eArmKinKind), 눈 확인은 실습 상태 파일 (DLL SagPrac_EyeNeeded)</summary>
        private void Prac_DefLine()
        {
            if (IsDisposed) return;
            CultureInfo ci = CultureInfo.InvariantCulture;
            Ojw.C3d.CSagArmCfg arm = (m_ArmCfg != null) ? m_ArmCfg.Arm : null;
            string s, eye = "";
            Color c;
            bool bEyeNeed = true;
            Ojw.C3d.CSagDefCheck chk = null;
            try
            {
                if (m_C3d != null && arm != null)
                {
                    if (m_strArmCfgDir != null) bEyeNeed = m_C3d.SagPrac_EyeNeeded(m_strArmCfgDir, m_ArmConn, arm, out eye);
                    chk = m_C3d.SagDef_Check(arm);
                }
            }
            catch (Exception ex) { eye = ""; chk = null; ArmMotor_Log("실습 카드 오류: " + ex.Message); }
            if (arm != null && m_bArmKinOk)
            {
                s = "로봇 정의 확인됨 · 손목 " + arm.WristID.ToString(ci) + "번 · " + (bEyeNeed ? "수직 눈 확인 전" : "수직 눈 확인 끝");
                c = Color.DarkGreen;
            }
            else if (m_strArmKinWhy == null || m_strArmKinWhy.Length == 0)
            {
                s = "로봇 정의 확인 전 — 이 탭을 다시 여십시오";
                c = SystemColors.GrayText;
            }
            else
            {
                switch (m_eArmKinKind)
                {
                    case Ojw.C3d.ESagDefFail.NoModel: s = "★3D 에 로봇 모델 없음"; break;
                    case Ojw.C3d.ESagDefFail.Model:
                    case Ojw.C3d.ESagDefFail.Changed: s = "★3D 가 바뀜 — 이 탭을 다시 여십시오"; break;
                    case Ojw.C3d.ESagDefFail.NoKin: s = "★계획 기구학 없음 — 앱을 다시 켜십시오"; break;
                    default: s = "★로봇 정의 확인 안 됨 — [정의 바꾸기]"; break;
                }
                c = Color.Firebrick;
            }
            string tip;
            if (!m_bArmKinOk && m_strArmKinWhy != null && m_strArmKinWhy.Length > 0)   // 맞지 않으면 까닭 + 안내 한 줄 (DLL 요약은 같은 까닭이라 빼고 숫자만)
                tip = "★" + ArmDef_WhyHint(m_strArmKinWhy, m_eArmKinKind) + ((chk != null && chk.Detail.Length > 0) ? "\r\n" + chk.Detail : "");
            else tip = (chk != null) ? chk.Summary + ((chk.Detail.Length > 0) ? "\r\n" + chk.Detail : "") : "";
            tip += ((tip.Length > 0) ? "\r\n" : "") + (bEyeNeed ? ARMDEF_EYE_HOWTO : eye + " — 다시 하려면 '로봇 정의' 페이지의 [수직 눈 확인]");
            if (lblPracDef.Text != s) lblPracDef.Text = s;
            if (lblPracDef.ForeColor != c) lblPracDef.ForeColor = c;
            tipArmExp.SetToolTip(lblPracDef, tip);
            bool bLink = bEyeNeed && arm != null && m_bArmKinOk;             // 눈 확인 기록이 없을 때만 (정의가 3D 와 맞을 때) — 정의 페이지의 [수직 눈 확인] 과 같다
            if (lnkPracEye.Visible != bLink) lnkPracEye.Visible = bLink;
            ArmDef_EyeLabel(bEyeNeed, eye);
        }

        /// <summary>정의 페이지의 눈 확인 줄 (lblDefEye — [수직 눈 확인] 옆): 끝 (시각, 초록) / ★확인 전 (빨강). 툴팁 = 하는 곳·하는 일</summary>
        private void ArmDef_EyeLabel(bool bNeed, string eye)
        {
            if (IsDisposed) return;
            string s = bNeed ? "★수직 눈 확인 전 (실물 바닥 작업 전에 한 번)" : ((eye != null && eye.Length > 0) ? eye : "수직 눈 확인 끝");
            ArmSetup_SetLabel(lblDefEye, s, bNeed ? Color.Firebrick : Color.DarkGreen);
            tipArmExp.SetToolTip(lblDefEye, bNeed ? ARMDEF_EYE_HOWTO : s + " — 저장된 정의·지금 모터 영점으로 확인함 (영점·정의를 고치면 다시 필요)");
        }

        /// <summary>실물 바닥 작업(③ 측정·⑤ 닿기 시험·실습 2·3단계) 확인창 앞 — 이 로봇·정의·모터 영점으로 수직 눈 확인을 아직 안 했으면 (DLL SagPrac_EyeNeeded)
        /// 하는 곳('로봇 정의' 페이지의 [수직 눈 확인])을 알리고 [예] 면 그 페이지로, false (시작하지 않는다 — DLL 작업 입구도 같은 까닭으로 거절). 했으면 true</summary>
        private bool ArmDef_EyeGate(string strTitle, Ojw.C3d.CSagArmCfg arm)
        {
            string t;
            bool bNeed = true;
            try { bNeed = m_C3d == null || m_strArmCfgDir == null || arm == null || m_C3d.SagPrac_EyeNeeded(m_strArmCfgDir, m_ArmConn, arm, out t); }
            catch { bNeed = true; }
            if (!bNeed) return true;
            string s = strTitle + " — 수직 눈 확인 전입니다 (이 로봇·정의·모터 영점으로 처음 한 번 — 영점·정의를 고쳤으면 다시).\r\n\r\n" +
                       "로봇팔 세팅 탭 '로봇 정의' 페이지의 [수직 눈 확인] 을 누르면, 팔을 바닥 위 옮기는 높이에서 수직 자세로 펼쳐 실물 펜이 바닥에 수직인지 묻습니다.\r\n" +
                       "[예] 로 답하면 기록하고 원위치합니다 (바닥은 누르지 않고 실습 진행은 그대로). 그다음 이 작업을 다시 누르십시오.\r\n\r\n" +
                       "지금 '로봇 정의' 페이지로 갈까요?";
            if (ArmMotor_Ask(s, strTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes && !IsDisposed)
                tabArmInner.SelectedTab = tabArmDef;
            return false;
        }

        // ════════════════════════════════════════════════════════════
        // 수직 눈 확인만 ([수직 눈 확인] · 실습 카드 [눈 확인하기] — DLL SagPrac_EyeJob)
        // ════════════════════════════════════════════════════════════

        /// <summary>[수직 눈 확인] (정의 페이지) — 연결이 있으면 실물 (확인창 [예] 뒤에만), 없으면 3D 로만 미리 보기 (까닭을 묻고)</summary>
        private void btnDefEye_Click(object sender, EventArgs e)
        {
            bool bConn = m_ArmConn != null && m_ArmConn.IsConnected;
            ArmDef_EyeRun(!bConn, true);
        }

        /// <summary>[눈 확인하기] (실습 카드, 기록이 없을 때만 보임) — [3D만] 체크대로 (실물은 확인창 [예] 뒤에만)</summary>
        private void lnkPracEye_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { ArmDef_EyeRun(chkPracDry.Checked, false); }

        /// <summary>수직 눈 확인만 (DLL SagPrac_EyeJob — 정의 확인 → 펼치기 (가운데 점 위, 옮기는 높이, 바닥으로 수직 자세) → 묻기 → [예] 면 기록 → 원위치·토크 시작 상태.
        /// 바닥은 누르지 않고 실습 진행은 그대로). 저장된 정의(sagarm.txt)로 한다 — 정의 페이지에 저장하지 않은 칸이 있으면 알리고 멈춘다.
        /// 놀 때만, 작업 전 공통 확인(Prac_Prepare — 계획 기구학·설정 맞춤·정의와 3D)을 지나야. 실물: 시작 전 경고(ArmSetup_CheckStart) → 확인창 (기본 [아니오]) →
        /// 연결 다시 확인. 3D 만: bAskDry 면 까닭(연결 없음)을 알리고 묻는다. 작업은 실습·고급 페이지와 같은 길 (Prac_Start → ArmSetup_Run 잠금·로그 창·[중단]),
        /// 묻는 대화창은 Prac_EyeAsk, 끝난 뒤 처리는 Prac_After ([아니오] 면 영점 안내, 실패면 까닭) → 카드·이 페이지의 눈 확인 줄</summary>
        private void ArmDef_EyeRun(bool bDry, bool bAskDry)
        {
            const string T = "수직 눈 확인";
            if (ArmSetup_AnyBusy())
            {
                ArmMotor_MsgBox(T + " — 로봇팔이 동작 중입니다 (작업·조그·⑨·3D 모션). 끝난 뒤 다시 누르십시오.", MessageBoxIcon.Information);
                return;
            }
            Ojw.C3d.CSagArmCfg arm;
            string strDir;
            if (!Prac_Prepare(T, out arm, out strDir)) return;
            if (ArmDef_PageShown() && m_ArmDefCand != null && !ArmDef_Same(m_ArmDefCand, m_ArmCfg.Arm))
            {
                ArmMotor_MsgBox(T + " — 이 페이지에 저장하지 않은 칸이 있습니다.\r\n수직 눈 확인은 저장된 정의(" + Ojw.C3d.CSagArmSetup.FILE + ")로 합니다. " +
                                "먼저 [확인·저장] 을 누르거나, 본보기에서 '지금 설정' 을 골라 칸을 되돌린 뒤 다시 누르십시오.", MessageBoxIcon.Information);
                return;
            }
            string strEye;
            bool bNeed = m_C3d.SagPrac_EyeNeeded(strDir, bDry ? null : m_ArmConn, arm, out strEye);
            if (bDry)
            {
                if (bAskDry && !ArmMotor_Confirm(T + " — 연결이 없어 3D 로만 미리 봅니다 (실물은 움직이지 않고 기록하지 않음).\r\n\r\n" +
                                                 "· 3D 팔이 휴지 자세에서 펼쳐 가운데 점 위 옮기는 높이에서 '바닥으로 수직' 자세로 멈춥니다\r\n" +
                                                 "· 실물로 하려면 위 연결 줄에서 먼저 연결한 뒤 다시 누르십시오\r\n\r\n[예] 3D 로 보기   [아니오] 취소", T)) return;
            }
            else
            {
                if (!ArmSetup_CheckStart(T, arm)) return;                // 연결 없음·구성 바뀜이면 알리고 멈춘다 (방향 확인 안 된 축은 계속/취소)
                bool bTq = m_ArmConn != null && m_ArmConn.IsTorqueOn;
                string s = T + " — 로봇팔이 실제로 움직입니다.\r\n\r\n" +
                           "· 휴지 자세에서 펼쳐 가운데 점 위 옮기는 높이에서 '바닥으로 수직' 자세로 멈춥니다 — 바닥은 누르지 않습니다\r\n" +
                           "· 멈추면 '실물 펜이 바닥에 수직입니까?' 를 묻습니다 — 옆과 앞에서 보고 답하십시오 ([예] 면 기록, [아니오] 면 기록하지 않음)\r\n" +
                           "· " + (bNeed ? "이 로봇·정의·모터 영점으로 아직 확인하지 않았습니다" : strEye + " — 다시 확인합니다") + " · 실습 진행(단계·결과)은 바뀌지 않습니다\r\n" +
                           ArmSetup_PortLine(arm) + ArmSetup_PathLine(arm) +
                           (bTq ? "· 토크는 이미 켜져 있음 — 끝나면 휴지 자세로 돌아가 토크 ON 그대로\r\n" : "· 지금 자세(휴지)를 읽어 두고 토크를 켭니다 — 끝나면 그 자세로 돌아가 토크를 끕니다\r\n") +
                           "· 팔 둘레를 비우고 손을 떼십시오. [중단] 은 언제든 — 원위치합니다.\r\n\r\n[예] 시작   [아니오] 취소";
                if (!ArmMotor_Confirm(s, T)) return;
                if (!ArmSetup_CheckConn(T)) return;
            }
            Ojw.C3d.DSagEyeAsk eyeAsk = Prac_EyeAsk(arm);              // 묻는 대화창 (기본 [아니오]) — 묻는 동안 3D 표시를 이 정의로
            Prac_Start(T, bDry, false, delegate(Ojw.CSim2Real.DLog log, Ojw.CSim2Real.DIsStopped stop)
            {
                return m_C3d.SagPrac_EyeJob(m_ArmConn, arm, strDir, bDry, eyeAsk, log, stop);
            });
        }

        /// <summary>계획 기구학을 지금 3D 로 맞춘다 (DLL SagRun_Prepare — UI 스레드, 작업·학습·계획 중이면 DLL 이 바꾸지 않는다).
        /// 맞추지 못하면 로그에 ★ 까닭 (같은 까닭은 한 번 — 정의 확인·작업 입구가 거절하며 같은 까닭을 보인다). 시작 때 준비하지 못했으면 하지 않는다</summary>
        private void ArmDef_Prepare()
        {
            if (!m_bArmPrepared || m_C3d == null || IsDisposed) return;
            string why;
            bool bOk;
            try { bOk = m_C3d.SagRun_Prepare(out why); }
            catch (Exception ex) { bOk = false; why = "★오류: " + ex.Message; }
            if (bOk) { m_strArmPrepWhy = ""; return; }
            if (why == m_strArmPrepWhy) return;
            m_strArmPrepWhy = why;
            ArmMotor_Log("★계획 기구학 — " + why);
        }

        private void chkPracDefShow_CheckedChanged(object sender, EventArgs e) { ArmDef_UpdateMark(); }

        private void btnPracDef_Click(object sender, EventArgs e) { tabArmInner.SelectedTab = tabArmDef; }

        // ════════════════════════════════════════════════════════════
        // 3D 표시·3D 모델이 바뀜
        // ════════════════════════════════════════════════════════════

        /// <summary>로봇팔 세팅 탭을 보고 있나 (바깥 탭)</summary>
        private bool ArmDef_TabShown() { return tabRight.SelectedTab == tabArmSetup; }

        /// <summary>정의 페이지를 보고 있나 (로봇팔 세팅 탭 + 안쪽 탭 '로봇 정의')</summary>
        private bool ArmDef_PageShown() { return ArmDef_TabShown() && tabArmInner.SelectedTab == tabArmDef; }

        /// <summary>3D 표시 (DLL ToolMark_Set — 공구 끝·공구축·꺾는 손목) — 수직 눈 확인 중이면 그 작업의 정의로 켬, 로봇팔 세팅 탭 밖이면 끔,
        /// 정의 페이지를 보는 동안은 후보 정의로 켬 (칸을 읽지 못하면 저장된 정의), 탭 안의 다른 페이지에서는 저장된 정의로 실습 카드 [3D 표시] 체크대로.
        /// 체크·탭(안·바깥)·정의가 바뀔 때마다, sagarm.txt 를 다시 읽은 뒤, 3D 모델이 바뀐 뒤</summary>
        private void ArmDef_UpdateMark()
        {
            if (IsDisposed || m_C3d == null) return;
            try
            {
                Ojw.C3d.CSagArmCfg saved = (m_ArmCfg != null) ? m_ArmCfg.Arm : null;
                Ojw.C3d.CSagArmCfg c;
                bool bShow;
                if (m_bArmEyeAsking) { c = (m_ArmEyeCfg != null) ? m_ArmEyeCfg : saved; bShow = true; }
                else if (!ArmDef_TabShown()) { c = saved; bShow = false; }
                else if (ArmDef_PageShown()) { c = (m_ArmDefCand != null) ? m_ArmDefCand : saved; bShow = true; }
                else { c = saved; bShow = chkPracDefShow.Checked; }
                m_C3d.ToolMark_Set(c, bShow && c != null);
            }
            catch (Exception ex) { ArmMotor_Log("3D 표시 오류: " + ex.Message); }
        }

        /// <summary>바깥 탭을 바꿀 때 (Main.cs tabRight_SelectedIndexChanged 끝) — 3D 표시를 탭에 맞춘다 (밖이면 끔)</summary>
        private void ArmDef_TopTabChanged()
        {
            if (IsDisposed) return;
            ArmDef_UpdateMark();
        }

        /// <summary>MakeAll 끝 (3D 모델을 다시 해석한 뒤) — 로봇팔 세팅 탭을 보고 있으면 계획 기구학을 새 수식으로 (ArmDef_Prepare: 작업·학습·계획 중이면 DLL 이
        /// 바꾸지 않는다, DH 행이 같으면 DLL 이 그대로 둔다) 하고 가벼운 기구학 확인 (정의 확인은 DLL 캐시 — 목록·계획 다시 읽기는 탭을 다시 볼 때), 정의 페이지면
        /// 콤보·확인. 탭 밖이면 계획 기구학은 탭을 볼 때 맞춘다 (만들기 탭의 끌기처럼 MakeAll 이 잦아도 가볍게). 3D 표시는 늘. 잠금 중이면 하지 않는다 (MakeAll 도 미룬다)</summary>
        private void ArmDef_ModelChanged()
        {
            if (m_bArmLocked || !m_bArmPrepared || m_C3d == null || IsDisposed) return;
            try
            {
                if (ArmDef_TabShown())
                {
                    if (!ArmSetup_AnyBusy()) ArmDef_Prepare();
                    if (m_ArmCfg != null && !ArmSetup_AnyBusy())
                    {
                        ArmSetup_CheckKin();
                        ArmSetup_StatusLine();
                        ArmSetup_UpdateEnable();
                    }
                    if (m_bArmDefInit && ArmDef_PageShown()) { ArmDef_RefreshLists(); ArmDef_Validate(); }
                }
                ArmDef_UpdateMark();
            }
            catch (Exception ex) { ArmMotor_Log("로봇 정의 — 3D 모델 바뀜 처리 오류: " + ex.Message); }
        }

        /// <summary>안쪽 탭을 바꿀 때 — 정의 페이지로 오면 계획 기구학을 지금 3D 로 (UI 스레드), 본보기·콤보 다시, 확인. 3D 표시도</summary>
        private void tabArmInner_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsDisposed) return;
            if (m_bArmDefInit && ArmDef_PageShown())
            {
                try
                {
                    ArmDef_Prepare();
                    ArmDef_FillPresets(null);
                    ArmDef_RefreshLists();
                    ArmDef_Validate();
                }
                catch (Exception ex) { ArmMotor_Log("로봇 정의 페이지 갱신 오류: " + ex.Message); }
            }
            ArmDef_UpdateMark();
        }

        /// <summary>ArmSetup_OnShown 끝 — 정의 페이지의 콤보 항목을 지금 3D 로 (고른 값·고친 칸은 그대로), 확인, 3D 표시</summary>
        private void ArmDef_OnShown()
        {
            if (!m_bArmDefInit || IsDisposed) return;
            try { ArmDef_FillPresets(null); ArmDef_RefreshLists(); ArmDef_Validate(); }
            catch (Exception ex) { ArmMotor_Log("로봇 정의 페이지 갱신 오류: " + ex.Message); }
            ArmDef_UpdateMark();
        }

        // ════════════════════════════════════════════════════════════
        // 정의 페이지 — 준비·칸 채우기
        // ════════════════════════════════════════════════════════════

        /// <summary>ArmSetup_Init 끝 (Prac_Init 앞) — 본보기 목록, 칸 = 저장된 정의, 확인</summary>
        private void ArmDef_Init()
        {
            try
            {
                ArmDef_FillPresets(null);
                ArmDef_FillUi((m_ArmCfg != null) ? m_ArmCfg.Arm : Ojw.C3d.CSagArmSetup.OmxDef());
            }
            catch (Exception ex) { ArmMotor_Log("로봇 정의 페이지 준비 오류: " + ex.Message); }
            m_bArmDefInit = true;
            ArmDef_Validate();
        }

        /// <summary>정의 c → 칸. old 가 있으면 (sagarm.txt 를 밖에서 고쳐 다시 읽음 — ArmSetup_PutCfgUi) 칸이 아직 old 값 그대로일 때만 바꾼다 (고친 칸은 그대로).
        /// old 가 없으면 전부 바꾸고 본보기 콤보를 '지금 설정' 으로, ③·공구 끝 거리의 '사용자가 고침' 표시를 지운다</summary>
        private void ArmDef_FillUi(Ojw.C3d.CSagArmCfg c, Ojw.C3d.CSagArmCfg old = null)
        {
            if (c == null || IsDisposed) return;
            ArmDef_PutFields(c, old);
            if (old == null)
            {
                m_bArmDefDownEdited = m_bArmDefLenEdited = false;
                if (cmbDefPreset.Items.Count > 0 && cmbDefPreset.SelectedIndex != 0)
                {
                    m_bArmDefUiSync = true;
                    try { cmbDefPreset.SelectedIndex = 0; }
                    finally { m_bArmDefUiSync = false; }
                }
            }
            if (m_bArmDefInit) ArmDef_Validate();
            else ArmDef_UpdateMark();
        }

        /// <summary>칸에 값만 (콤보 항목은 지금 3D 에서 — 3D 에 없는 수식·체인 밖 손목도 '(3D 에 없음)'·'(체인 밖)' 항목으로 보인다 — 확인이 까닭을 말한다)</summary>
        private void ArmDef_PutFields(Ojw.C3d.CSagArmCfg c, Ojw.C3d.CSagArmCfg old)
        {
            int fNow = ArmDef_SelFunc(), wNow = ArmDef_SelWrist();
            int f = (old == null || fNow < 0 || fNow == old.FuncNumber) ? c.FuncNumber : fNow;
            int w = (old == null || wNow < 0 || wNow == old.WristID) ? c.WristID : wNow;
            m_bArmDefUiSync = true;
            try
            {
                ArmDef_SetFuncItems(f);
                ArmDef_SetWristItems(f, w);
                ArmSetup_PutText(txtDefDownSum, ArmDef_Num(c.DownSumDeg), (old != null) ? ArmDef_Num(old.DownSumDeg) : null);
                ArmSetup_PutText(txtDefName, (c.DefName == null) ? "" : c.DefName, (old != null) ? ((old.DefName == null) ? "" : old.DefName) : null);
                ArmSetup_PutText(txtDefBase, ArmDef_Base(c), (old != null) ? ArmDef_Base(old) : null);
                ArmSetup_PutText(txtDefPitch, ArmDef_Pitch(c), (old != null) ? ArmDef_Pitch(old) : null);
                ArmSetup_PutText(txtDefHold, ArmDef_Ints(c.HoldIDs), (old != null) ? ArmDef_Ints(old.HoldIDs) : null);
                ArmSetup_PutText(txtDefTcpLen, ArmDef_Len(c.TcpLenMm), (old != null) ? ArmDef_Len(old.TcpLenMm) : null);
            }
            finally { m_bArmDefUiSync = false; }
        }

        /// <summary>공구 끝·손목 콤보 항목을 지금 3D 에서 다시 (고른 값은 그대로 — 고른 것이 없으면 저장된 정의의 값)</summary>
        private void ArmDef_RefreshLists()
        {
            Ojw.C3d.CSagArmCfg saved = (m_ArmCfg != null) ? m_ArmCfg.Arm : Ojw.C3d.CSagArmSetup.OmxDef();
            int f = ArmDef_SelFunc(), w = ArmDef_SelWrist();
            if (f < 0) f = saved.FuncNumber;
            if (w < 0) w = saved.WristID;
            m_bArmDefUiSync = true;
            try { ArmDef_SetFuncItems(f); ArmDef_SetWristItems(f, w); }
            finally { m_bArmDefUiSync = false; }
        }

        /// <summary>① 콤보 — 3D 에서 글이 있는 수식 슬롯 (DLL SagDef_Formulas — 앱이 합쳐 둔 '전체 수식' 슬롯은 빠진다). f 가 없으면 '(3D 에 없음)' 항목을 더해 고른다</summary>
        private void ArmDef_SetFuncItems(int f)
        {
            int[] a = (m_C3d != null) ? m_C3d.SagDef_Formulas() : new int[0];
            cmbDefTcp.BeginUpdate();
            try
            {
                cmbDefTcp.Items.Clear();
                m_lstArmDefFuncs.Clear();
                foreach (int n in a) { cmbDefTcp.Items.Add("수식 " + n.ToString(CultureInfo.InvariantCulture) + " 끝"); m_lstArmDefFuncs.Add(n); }
                if (f >= 0 && !m_lstArmDefFuncs.Contains(f)) { cmbDefTcp.Items.Add("수식 " + f.ToString(CultureInfo.InvariantCulture) + " 끝 (3D 에 없음)"); m_lstArmDefFuncs.Add(f); }
                cmbDefTcp.SelectedIndex = m_lstArmDefFuncs.IndexOf(f);
            }
            finally { cmbDefTcp.EndUpdate(); }
        }

        /// <summary>② 콤보 — 수식 f 의 공구 끝 체인 관절 (DLL SagDef_ChainIDs — 베이스 쪽부터, '!' 뒤 관절은 없다). w 가 체인 밖이면 '(체인 밖)' 항목을 더해 고른다</summary>
        private void ArmDef_SetWristItems(int f, int w)
        {
            int[] ch = (m_C3d != null && f >= 0) ? m_C3d.SagDef_ChainIDs(f) : new int[0];
            cmbDefWrist.BeginUpdate();
            try
            {
                cmbDefWrist.Items.Clear();
                m_lstArmDefWrists.Clear();
                foreach (int id in ch) { cmbDefWrist.Items.Add(id.ToString(CultureInfo.InvariantCulture) + "번"); m_lstArmDefWrists.Add(id); }
                if (w >= 0 && !m_lstArmDefWrists.Contains(w)) { cmbDefWrist.Items.Add(w.ToString(CultureInfo.InvariantCulture) + "번 (체인 밖)"); m_lstArmDefWrists.Add(w); }
                cmbDefWrist.SelectedIndex = m_lstArmDefWrists.IndexOf(w);
            }
            finally { cmbDefWrist.EndUpdate(); }
        }

        /// <summary>본보기 콤보 — [0] 지금 설정 (sagarm.txt) · [1] OMX follower (내장) · [2..] <urdf>\sagarm_defs\*.txt (DLL CSagArmSetup.DefPresets).
        /// strSelect 가 있으면 그 파일을, 없으면 지금 고른 것을 다시 고른다</summary>
        private void ArmDef_FillPresets(string strSelect)
        {
            int k = cmbDefPreset.SelectedIndex;
            string keep = strSelect;
            if (keep == null && k >= ARMDEF_PRESET_FIRST && k - ARMDEF_PRESET_FIRST < m_lstArmDefPresets.Count) keep = m_lstArmDefPresets[k - ARMDEF_PRESET_FIRST];
            string[] a = (m_strArmCfgDir != null) ? Ojw.C3d.CSagArmSetup.DefPresets(m_strArmCfgDir) : new string[0];
            int nSel = (strSelect == null && k == 1) ? 1 : 0;
            m_bArmDefUiSync = true;
            cmbDefPreset.BeginUpdate();
            try
            {
                cmbDefPreset.Items.Clear();
                m_lstArmDefPresets.Clear();
                cmbDefPreset.Items.Add("지금 설정 (" + Ojw.C3d.CSagArmSetup.FILE + ")");
                cmbDefPreset.Items.Add(Ojw.C3d.CSagArmSetup.DEF_OMX + " (내장)");
                foreach (string p in a)
                {
                    if (keep != null && ArmSetup_SamePath(p, keep)) nSel = cmbDefPreset.Items.Count;
                    cmbDefPreset.Items.Add(Path.GetFileNameWithoutExtension(p));
                    m_lstArmDefPresets.Add(p);
                }
                cmbDefPreset.SelectedIndex = nSel;
            }
            finally { cmbDefPreset.EndUpdate(); m_bArmDefUiSync = false; }
            tipArmExp.SetToolTip(cmbDefPreset, "고르면 칸만 채웁니다 (저장 안 함) — [확인·저장] 을 눌러야 " + Ojw.C3d.CSagArmSetup.FILE + " 에 들어갑니다" +
                                 ((m_strArmCfgDir != null) ? "\r\n본보기 폴더: " + Path.Combine(m_strArmCfgDir, Ojw.C3d.CSagArmSetup.DEF_DIR) : ""));
        }

        // 칸 글 (정의 → 칸)
        private static string ArmDef_Num(float f) { return f.ToString("0.###", CultureInfo.InvariantCulture); }
        private static string ArmDef_Len(float f) { return float.IsNaN(f) ? "" : ArmDef_Num(f); }
        private static string ArmDef_Base(Ojw.C3d.CSagArmCfg c) { return (c.PosIDs != null && c.PosIDs.Length == 3) ? c.PosIDs[0].ToString(CultureInfo.InvariantCulture) : ""; }
        private static string ArmDef_Pitch(Ojw.C3d.CSagArmCfg c)
        {
            return (c.PosIDs != null && c.PosIDs.Length == 3) ? c.PosIDs[1].ToString(CultureInfo.InvariantCulture) + "," + c.PosIDs[2].ToString(CultureInfo.InvariantCulture) : "";
        }
        private static string ArmDef_Ints(int[] a)
        {
            if (a == null) return "";
            string[] s = new string[a.Length];
            for (int i = 0; i < a.Length; i++) s[i] = a[i].ToString(CultureInfo.InvariantCulture);
            return string.Join(",", s);
        }

        /// <summary>① 에서 고른 수식 번호 (없으면 −1)</summary>
        private int ArmDef_SelFunc()
        {
            int k = cmbDefTcp.SelectedIndex;
            return (k >= 0 && k < m_lstArmDefFuncs.Count) ? m_lstArmDefFuncs[k] : -1;
        }

        /// <summary>② 에서 고른 손목 관절 번호 (없으면 −1)</summary>
        private int ArmDef_SelWrist()
        {
            int k = cmbDefWrist.SelectedIndex;
            return (k >= 0 && k < m_lstArmDefWrists.Count) ? m_lstArmDefWrists[k] : -1;
        }

        // ════════════════════════════════════════════════════════════
        // 정의 페이지 — 칸 읽기·확인
        // ════════════════════════════════════════════════════════════

        /// <summary>칸 → 후보 정의 (저장된 설정의 사본에 정의 7키만 넣는다). 수를 읽지 못하면 false + why (쉬운 말). 값의 범위는 DLL 이 본다
        /// (확인 SagDef_Check, 저장 전 ArmSetup_CheckCfg). bMeasure = ③·공구 끝 거리를 읽지 않고 NaN (3D 에서 재려고)</summary>
        private bool ArmDef_ReadUi(out Ojw.C3d.CSagArmCfg cand, out string why, bool bMeasure = false)
        {
            cand = null; why = "";
            Ojw.C3d.CSagArmCfg c = (m_ArmCfg != null) ? m_ArmCfg.Arm.Clone() : new Ojw.C3d.CSagArmCfg();
            int f = ArmDef_SelFunc(), w = ArmDef_SelWrist();
            if (f < 0) { why = "① 공구 끝 수식을 고르십시오 (3D 에 수식이 없으면 로봇 모델부터)"; return false; }
            if (w < 0) { why = "② 꺾는 손목을 고르십시오"; return false; }
            float d = float.NaN, len = float.NaN;
            if (!bMeasure && !ArmDef_Float(txtDefDownSum.Text, out d)) { why = "③ 바닥으로 수직 관절 합을 숫자로 넣으십시오 (예: 90, 소수점은 '.')"; return false; }
            string n = txtDefName.Text.Trim();
            string wn = Ojw.C3d.CSagArmSetup.DefNameWhy(n);
            if (wn.Length > 0) { why = "고급 — 이름: " + wn; return false; }
            int[] b, p, h;
            if (!ArmDef_ParseInts(txtDefBase.Text, out b) || b.Length != 1) { why = "고급 — 베이스(요) 관절 번호를 정수 하나로 넣으십시오 (예: 11)"; return false; }
            if (!ArmDef_ParseInts(txtDefPitch.Text, out p) || p.Length != 2) { why = "고급 — 피치 관절 두 개를 쉼표로 넣으십시오 (베이스 쪽부터, 예: 12,13)"; return false; }
            if (!ArmDef_ParseInts(txtDefHold.Text, out h)) { why = "고급 — 토크만 관절을 쉼표로 나눈 정수로 넣으십시오 (예: 15,16 — 없으면 비움)"; return false; }
            if (!bMeasure && txtDefTcpLen.Text.Trim().Length > 0 && !ArmDef_Float(txtDefTcpLen.Text, out len))
            { why = "고급 — 공구 끝 거리를 숫자(mm)로 넣거나 비우십시오 (비우면 확인 안 함)"; return false; }
            c.DefName = n;
            c.FuncNumber = f;
            c.PosIDs = new int[] { b[0], p[0], p[1] };
            c.WristID = w;
            c.HoldIDs = h;
            c.TcpLenMm = len;
            c.DownSumDeg = d;
            cand = c;
            return true;
        }

        /// <summary>수 하나 (소수점은 '.', 쉼표·NaN·무한은 안 됨)</summary>
        private static bool ArmDef_Float(string s, out float f)
        {
            return float.TryParse((s == null) ? "" : s.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out f) && !float.IsNaN(f) && !float.IsInfinity(f);
        }

        /// <summary>"12,13" → 정수 배열 (빈칸은 건너뜀 — 비었으면 빈 배열). 정수가 아닌 칸이 있으면 false</summary>
        private static bool ArmDef_ParseInts(string s, out int[] a)
        {
            a = new int[0];
            List<int> l = new List<int>();
            foreach (string t in ((s == null) ? "" : s).Split(','))
            {
                string u = t.Trim();
                if (u.Length == 0) continue;
                int n;
                if (!int.TryParse(u, NumberStyles.Integer, CultureInfo.InvariantCulture, out n)) return false;
                l.Add(n);
            }
            a = l.ToArray();
            return true;
        }

        /// <summary>정의 7키가 같은가 (이름·수식·관절·토크만 관절·공구 끝 거리·수직 관절 합)</summary>
        private static bool ArmDef_Same(Ojw.C3d.CSagArmCfg a, Ojw.C3d.CSagArmCfg b)
        {
            if (a == null || b == null) return a == b;
            return string.Equals((a.DefName ?? "").Trim(), (b.DefName ?? "").Trim(), StringComparison.Ordinal) && a.FuncNumber == b.FuncNumber &&
                   ArmDef_Ints(a.PosIDs) == ArmDef_Ints(b.PosIDs) && a.WristID == b.WristID && ArmDef_Ints(a.HoldIDs) == ArmDef_Ints(b.HoldIDs) &&
                   (a.TcpLenMm == b.TcpLenMm || (float.IsNaN(a.TcpLenMm) && float.IsNaN(b.TcpLenMm))) && a.DownSumDeg == b.DownSumDeg;
        }

        /// <summary>칸이 바뀔 때마다 — 후보 정의를 DLL 로 확인 (SagDef_Check — 캐시라 가볍다) 해 정보 라벨·상태 줄·툴팁·③ 라벨의 관절 번호,
        /// [확인·저장] 켬 조건 (확인됨 && 저장된 정의와 다름 && 놀 때), 3D 표시</summary>
        private void ArmDef_Validate()
        {
            if (!m_bArmDefInit || IsDisposed) return;
            Ojw.C3d.CSagArmCfg cand = null;
            Ojw.C3d.CSagDefCheck chk = null;
            string why = "";
            try
            {
                if (ArmDef_ReadUi(out cand, out why) && m_C3d != null) chk = m_C3d.SagDef_Check(cand);
            }
            catch (Exception ex) { chk = null; why = "★오류: " + ex.Message; }
            m_ArmDefCand = cand;
            m_ArmDefChk = chk;
            bool bDirty = cand != null && m_ArmCfg != null && !ArmDef_Same(cand, m_ArmCfg.Arm);
            m_bArmDefCanSave = chk != null && chk.Ok && bDirty;
            ArmDef_Labels(cand, chk, why, bDirty);
            ArmDef_UpdateEnable(!m_bArmLocked && !m_bArmRun);
            ArmDef_UpdateMark();
        }

        /// <summary>정보 라벨·상태 줄 (값은 DLL 확인 결과·DLL 함수의 글)</summary>
        private void ArmDef_Labels(Ojw.C3d.CSagArmCfg cand, Ojw.C3d.CSagDefCheck chk, string why, bool bDirty)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            int f = ArmDef_SelFunc(), w = ArmDef_SelWrist();
            // ① 공구 끝 — 손목에서 거리, 툴팁 = 공구 끝 DH 줄 원문
            string strTcp = (m_C3d != null && f >= 0) ? m_C3d.SagDef_TcpLine(f) : "";
            ArmSetup_SetLabel(lblDefTcpInfo, (chk != null && !float.IsNaN(chk.TcpLenMm)) ? string.Format(ci, "손목에서 {0:0.0} mm", chk.TcpLenMm) : "", SystemColors.ControlText);
            string tipTcp = ((strTcp.Length > 0) ? "공구 끝 DH 줄: " + strTcp + "\r\n" : "") + "공구 끝(TCP) = 이 수식 체인의 끝 — 첫 '!' 바로 앞 DH 줄 ('!' 가 없으면 체인 마지막 DH 줄)";
            tipArmExp.SetToolTip(lblDefTcpInfo, tipTcp);
            tipArmExp.SetToolTip(cmbDefTcp, tipTcp);
            // ② 꺾는 손목 — 체인에서 자리, 툴팁 = 체인 차례
            int[] ch = (m_C3d != null && f >= 0) ? m_C3d.SagDef_ChainIDs(f) : new int[0];
            int iw = Array.IndexOf(ch, w);
            string sw;
            Color cw = SystemColors.ControlText;
            if (ch.Length == 0) sw = "";
            else if (iw < 0) { sw = "★공구 끝 체인 밖 관절"; cw = Color.Firebrick; }
            else if (iw < ch.Length - 1) { sw = "★공구 끝 앞 마지막 관절이 아님 (뒤에 " + ch[iw + 1].ToString(ci) + "번)"; cw = Color.Firebrick; }
            else sw = "공구 끝 바로 앞 관절";
            ArmSetup_SetLabel(lblDefWristInfo, sw, cw);
            string[] chs = new string[ch.Length];
            for (int i = 0; i < ch.Length; i++) chs[i] = ch[i].ToString(ci);
            string tipW = ((ch.Length > 0) ? "체인 (베이스 → 공구 끝): " + string.Join(" → ", chs) + "\r\n" : "") + "공구를 바닥으로 수직으로 꺾는 관절 — 공구 끝 앞 마지막 관절";
            tipArmExp.SetToolTip(lblDefWristInfo, tipW);
            tipArmExp.SetToolTip(cmbDefWrist, tipW);
            // ③ 바닥으로 수직 — 관절 합의 관절 번호, 3D 에서 잰 값
            string sSum = (cand != null) ? string.Format(ci, "관절 합 {0}+{1}+{2} =", cand.PosIDs[1], cand.PosIDs[2], cand.WristID) : "관절 합 =";
            if (lblDefDownSum.Text != sSum) lblDefDownSum.Text = sSum;
            ArmSetup_SetLabel(lblDefDownInfo, (chk != null && !float.IsNaN(chk.DownSumMeasDeg)) ? string.Format(ci, "° (3D 에서 잰 값 {0:0.0}°)", chk.DownSumMeasDeg) : "° (3D 에서 잴 수 없음)",
                              SystemColors.ControlText);
            tipArmExp.SetToolTip(lblDefDownInfo, "공구축(손목 원점 → 공구 끝)이 아래(−Z)를 향할 때의 관절 합을 3D 의 자세 다섯 개로 잰 값" +
                                 ((chk != null && !float.IsNaN(chk.SpreadDeg)) ? string.Format(ci, " (흩어짐 {0:0.00}°)", chk.SpreadDeg) : ""));
            // 상태 줄 — 확인됨 (요약) / ★ 첫 실패 까닭 (+ 종류별 안내). 툴팁 = 확인 항목 전부 + 숫자
            string s;
            Color c;
            if (cand == null) { s = "★" + why; c = Color.Firebrick; }
            else if (chk == null) { s = "★로봇 정의를 확인하지 못함 — " + ArmDef_KinHint(Ojw.C3d.ESagDefFail.NoKin); c = Color.Firebrick; }
            else if (chk.Ok)
            {
                s = "정의 확인됨 — " + chk.Summary + (bDirty ? "\r\n바뀐 칸이 있습니다 — [확인·저장] 을 누르면 " + Ojw.C3d.CSagArmSetup.FILE + " 에 들어갑니다" : "");
                c = Color.DarkGreen;
            }
            else
            {
                if (chk.Kind == Ojw.C3d.ESagDefFail.NoModel) s = "★" + ArmDef_KinHint(chk.Kind);   // 까닭과 안내가 같은 말 — 안내만
                else s = "★" + ((chk.Kind != Ojw.C3d.ESagDefFail.Def) ? ArmDef_WhyHint(chk.Why, chk.Kind) : chk.Why + ArmDef_FixHint(chk));
                c = Color.Firebrick;
            }
            ArmSetup_SetLabel(lblDefStatus, s, c);
            if (chk != null) tipArmExp.SetToolTip(lblDefStatus, s + "\r\n\r\n" + string.Join("\r\n", chk.Items.ToArray()) + ((chk.Detail.Length > 0) ? "\r\n" + chk.Detail : ""));
        }

        /// <summary>정의가 3D 와 맞지 않는 첫 까닭이 '정한 값 ≠ 3D 에서 잰 값' (V5 공구 끝 거리·V7 수직 관절 합) 이면 어느 칸을 무엇으로 고치면 되는지 한 줄 (아니면 "").
        /// 3D 에서 잰 값으로 저절로 바꾸지 않는다 — 정의는 미리 정해 두는 값이라 사용자가 보고 고친다 (① ② 를 바꿀 때만 잰 값으로 채운다)</summary>
        private static string ArmDef_FixHint(Ojw.C3d.CSagDefCheck chk)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            string first = "";
            foreach (string it in chk.Items)
                if (it.EndsWith("★" + chk.Why, StringComparison.Ordinal)) { first = it.Substring(0, Math.Min(2, it.Length)); break; }
            if (first == "V5" && !float.IsNaN(chk.TcpLenMm) && chk.TcpLenSpreadMm <= Ojw.C3d.SAGDEF_LEN_SPREAD_MM)
                return string.Format(ci, "\r\n펜 길이를 바꿨다면 고급 보기의 '공구 끝 거리' 를 {0:0.0} 으로 (비우면 거리 확인 안 함)", chk.TcpLenMm);
            if (first == "V7" && !float.IsNaN(chk.DownSumMeasDeg))
                return string.Format(ci, "\r\n3D 모델이 맞다면 ③ 을 {0:0.0} 으로 (실물이 수직이 아니면 고칠 곳은 이 칸이 아니라 모터 영점)", chk.DownSumMeasDeg);
            return "";
        }

        /// <summary>정의 칸 켬·끔 — 작업 중이면 칸·버튼을 모두 끈다 (고급 보기 체크·실습 카드 [3D 표시] 는 잠그지 않는다). ArmSetup_UpdateEnable 이 부른다</summary>
        private void ArmDef_UpdateEnable(bool bIdle)
        {
            if (IsDisposed) return;
            foreach (Control c in new Control[] { cmbDefPreset, btnDefPresetSave, cmbDefTcp, cmbDefWrist, txtDefDownSum, txtDefName, txtDefBase, txtDefPitch, txtDefHold, txtDefTcpLen })
                if (c.Enabled != bIdle) c.Enabled = bIdle;
            bool bSave = bIdle && m_bArmDefCanSave;
            if (btnDefSave.Enabled != bSave) btnDefSave.Enabled = bSave;
            bool bEye = bIdle && m_bArmPrepared && m_bArmKinOk;               // 수직 눈 확인 — 저장된 정의가 3D 와 맞을 때 (DLL 작업 입구도 정의를 먼저 본다)
            if (btnDefEye.Enabled != bEye) btnDefEye.Enabled = bEye;
        }

        // ════════════════════════════════════════════════════════════
        // 정의 페이지 — 칸 이벤트
        // ════════════════════════════════════════════════════════════

        /// <summary>구조가 바뀜 (① 또는 ②) — ① 이면 ② 항목을 새 체인으로 (손목이 체인에 있으면 그대로, 없으면 공구 끝 앞 마지막 관절),
        /// 체인의 관절이 손목 앞에 정확히 3개면 베이스·피치 칸을 그 셋으로, ③·공구 끝 거리를 3D 에서 잰 값으로 (사용자가 고친 칸은 그대로)</summary>
        private void ArmDef_StructChanged(bool bFunc)
        {
            if (m_bArmDefUiSync || !m_bArmDefInit) return;
            int f = ArmDef_SelFunc(), w = ArmDef_SelWrist();
            int[] ch = (m_C3d != null && f >= 0) ? m_C3d.SagDef_ChainIDs(f) : new int[0];
            m_bArmDefUiSync = true;
            try
            {
                if (bFunc)
                {
                    if (Array.IndexOf(ch, w) < 0 && ch.Length > 0) w = ch[ch.Length - 1];
                    ArmDef_SetWristItems(f, w);
                }
                if (Array.IndexOf(ch, w) == 3)
                {
                    string sb = ch[0].ToString(CultureInfo.InvariantCulture), sp = ch[1].ToString(CultureInfo.InvariantCulture) + "," + ch[2].ToString(CultureInfo.InvariantCulture);
                    if (txtDefBase.Text != sb) txtDefBase.Text = sb;
                    if (txtDefPitch.Text != sp) txtDefPitch.Text = sp;
                }
            }
            finally { m_bArmDefUiSync = false; }
            ArmDef_FillMeasured();
            ArmDef_Validate();
        }

        /// <summary>③·공구 끝 거리 칸을 3D 에서 잰 값으로 (0.1 반올림 — DLL SagDef_Check 의 DownSumMeasDeg·TcpLenMm, 정의 값 없이 재므로 V5 거리·V7 은 보지 않는다).
        /// 사용자가 직접 고친 칸은 그대로. 잴 수 없으면 (체인·관절이 맞지 않음) 그대로</summary>
        private void ArmDef_FillMeasured()
        {
            if (m_bArmDefDownEdited && m_bArmDefLenEdited) return;
            Ojw.C3d.CSagArmCfg c;
            string why;
            if (m_C3d == null || !ArmDef_ReadUi(out c, out why, true)) return;
            Ojw.C3d.CSagDefCheck chk = m_C3d.SagDef_Check(c);
            m_bArmDefUiSync = true;
            try
            {
                if (!m_bArmDefDownEdited && !float.IsNaN(chk.DownSumMeasDeg))
                {
                    string s = ((float)Math.Round(chk.DownSumMeasDeg, 1)).ToString("0.#", CultureInfo.InvariantCulture);
                    if (txtDefDownSum.Text != s) txtDefDownSum.Text = s;
                }
                if (!m_bArmDefLenEdited && !float.IsNaN(chk.TcpLenMm))
                {
                    string s = ((float)Math.Round(chk.TcpLenMm, 1)).ToString("0.#", CultureInfo.InvariantCulture);
                    if (txtDefTcpLen.Text != s) txtDefTcpLen.Text = s;
                }
            }
            finally { m_bArmDefUiSync = false; }
        }

        private void cmbDefTcp_SelectedIndexChanged(object sender, EventArgs e) { ArmDef_StructChanged(true); }
        private void cmbDefWrist_SelectedIndexChanged(object sender, EventArgs e) { ArmDef_StructChanged(false); }

        private void txtDefDownSum_TextChanged(object sender, EventArgs e)
        {
            if (m_bArmDefUiSync) return;
            m_bArmDefDownEdited = true;                          // 사용자가 정한 수직 — 구조가 바뀌어도 다시 채우지 않는다
            ArmDef_Validate();
        }

        private void txtDefTcpLen_TextChanged(object sender, EventArgs e)
        {
            if (m_bArmDefUiSync) return;
            m_bArmDefLenEdited = true;
            ArmDef_Validate();
        }

        /// <summary>베이스·피치 칸 (고급) — 구조가 바뀜: ③·공구 끝 거리를 다시 잰다 (사용자가 고친 칸은 그대로)</summary>
        private void txtDefBase_TextChanged(object sender, EventArgs e)
        {
            if (m_bArmDefUiSync || !m_bArmDefInit) return;
            ArmDef_FillMeasured();
            ArmDef_Validate();
        }

        private void txtDefPitch_TextChanged(object sender, EventArgs e)
        {
            if (m_bArmDefUiSync || !m_bArmDefInit) return;
            ArmDef_FillMeasured();
            ArmDef_Validate();
        }

        private void txtDefName_TextChanged(object sender, EventArgs e) { if (!m_bArmDefUiSync) ArmDef_Validate(); }
        private void txtDefHold_TextChanged(object sender, EventArgs e) { if (!m_bArmDefUiSync) ArmDef_Validate(); }

        private void chkDefAdv_CheckedChanged(object sender, EventArgs e) { grpDefAdv.Visible = chkDefAdv.Checked; }

        /// <summary>본보기 고르기 — 칸만 채운다 (저장 안 함): [0] 저장된 정의, [1] 내장 OMX follower, [2..] 본보기 파일 (DLL CSagArmSetup.LoadDefPreset — 정의 7키만)</summary>
        private void cmbDefPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_bArmDefUiSync || !m_bArmDefInit) return;
            int k = cmbDefPreset.SelectedIndex;
            Ojw.C3d.CSagArmCfg c = null;
            string warn = "";
            if (k == 0) c = (m_ArmCfg != null) ? m_ArmCfg.Arm : Ojw.C3d.CSagArmSetup.OmxDef();
            else if (k == 1) c = Ojw.C3d.CSagArmSetup.OmxDef();
            else if (k >= ARMDEF_PRESET_FIRST && k - ARMDEF_PRESET_FIRST < m_lstArmDefPresets.Count)
            {
                string p = m_lstArmDefPresets[k - ARMDEF_PRESET_FIRST];
                c = Ojw.C3d.CSagArmSetup.LoadDefPreset(p, out warn);
                if (c == null) { ArmMotor_MsgBox("본보기를 읽지 못했습니다:\r\n" + p + "\r\n\r\n" + warn, MessageBoxIcon.Warning); return; }
                if (warn != null && warn.Length > 0) ArmMotor_Log("로봇 정의 본보기 " + Path.GetFileName(p) + " — " + warn);
            }
            if (c == null) return;
            ArmDef_PutFields(c, null);
            m_bArmDefDownEdited = m_bArmDefLenEdited = false;
            ArmDef_Validate();
        }

        // ════════════════════════════════════════════════════════════
        // 정의 페이지 — 저장
        // ════════════════════════════════════════════════════════════

        /// <summary>후보 정의의 7키를 설정에 넣는 동작 (DLL 키 규칙 확인·저장 둘 다 이것으로)</summary>
        private static Action<Ojw.C3d.CSagArmSetup> ArmDef_Put(Ojw.C3d.CSagArmCfg d)
        {
            Ojw.C3d.CSagArmCfg v = d.Clone();
            return delegate(Ojw.C3d.CSagArmSetup c)
            {
                c.Arm.DefName = v.DefName;
                c.Arm.FuncNumber = v.FuncNumber;
                c.Arm.PosIDs = (int[])v.PosIDs.Clone();
                c.Arm.WristID = v.WristID;
                c.Arm.HoldIDs = (v.HoldIDs != null) ? (int[])v.HoldIDs.Clone() : new int[0];
                c.Arm.TcpLenMm = v.TcpLenMm;
                c.Arm.DownSumDeg = v.DownSumDeg;
            };
        }

        /// <summary>[확인·저장] — 놀 때만: 파일과 맞춤 (다시 읽었으면·읽지 못함·그대로 쓰지 못한 줄이면 저장하지 않고 알림) → 칸 → DLL 확인 (3D 와 맞아야) →
        /// 실습 기록과 다른 정의면 확인창 (다음은 1단계부터 — DLL 도 막는다) → DLL 키 규칙 (ArmSetup_CheckCfg) → sagarm.txt (ArmSetup_SaveCfg — 실패하면 메모리의
        /// 정의를 앞 값으로 되돌리고 대화창) → 페이지·실습 카드·버튼 다시</summary>
        private void btnDefSave_Click(object sender, EventArgs e)
        {
            const string T = "로봇 정의 저장";
            if (ArmSetup_AnyBusy())
            {
                ArmMotor_MsgBox(T + " — 로봇팔이 동작 중입니다 (작업·조그·⑨·3D 모션). 끝난 뒤 다시 누르십시오.", MessageBoxIcon.Information);
                return;
            }
            bool bReread = ArmSetup_SyncCfg();
            string strPath = (m_strArmCfgDir != null) ? Path.Combine(m_strArmCfgDir, Ojw.C3d.CSagArmSetup.FILE) : Ojw.C3d.CSagArmSetup.FILE;
            if (m_ArmCfg == null || m_strArmCfgDir == null || m_strArmCfgFail != null)
            {
                ArmMotor_MsgBox(T + " — 저장하지 않았습니다: " + Ojw.C3d.CSagArmSetup.FILE + " 를 읽지 못했습니다" + ((m_strArmCfgFail != null) ? " (" + m_strArmCfgFail + ")" : "") +
                                ".\r\n파일을 닫거나 고친 뒤 다시 누르십시오.\r\n" + strPath, MessageBoxIcon.Warning);
                return;
            }
            if (ArmSetup_CfgWarn().Length > 0)
            {
                ArmMotor_MsgBox(T + " — 저장하지 않았습니다: " + Ojw.C3d.CSagArmSetup.FILE + " 에 그대로 쓰지 못한 줄이 있습니다.\r\n\r\n· " +
                                ArmSetup_CfgWarn().Replace(" · ", "\r\n· ") + "\r\n\r\n지금 저장하면 그 줄이 기본값으로 바뀌거나 빠집니다. 메모장으로 그 줄을 고친 뒤 다시 누르십시오:\r\n" + strPath,
                                MessageBoxIcon.Warning);
                return;
            }
            if (bReread)
            {
                ArmSetup_RefreshPage();
                ArmSetup_UpdateEnable();
                ArmDef_Validate();
                ArmMotor_MsgBox(T + " — 저장하지 않았습니다: " + Ojw.C3d.CSagArmSetup.FILE + " 를 다시 읽었습니다 (밖에서 고침·urdf 폴더 바뀜).\r\n" +
                                "이 페이지에서 고친 칸은 그대로, 나머지는 파일 값입니다. 바뀐 값을 확인한 뒤 다시 누르십시오.", MessageBoxIcon.Information);
                return;
            }
            Ojw.C3d.CSagArmCfg cand;
            string why;
            if (!ArmDef_ReadUi(out cand, out why)) { ArmMotor_MsgBox(T + " — " + why, MessageBoxIcon.Warning); return; }
            if (m_C3d == null) return;
            Ojw.C3d.CSagDefCheck chk = m_C3d.SagDef_Check(cand);
            if (!chk.Ok)
            {
                ArmDef_Validate();
                ArmMotor_MsgBox(T + " — 3D 와 맞지 않아 저장하지 않습니다:\r\n\r\n" + chk.Why, MessageBoxIcon.Warning);
                return;
            }
            // 실습 기록과 다른 정의 — 다음은 1. 오차 재기부터 (DLL 이 state.txt 의 정의 서명으로 막는다). 저장된 정의의 사용자 몫(수식·관절·수직 관절 합)이
            // 기록과 같을 때만 묻는다 (한 번 [예] 로 바꾼 뒤 이름만 고치는 저장은 다시 묻지 않는다 — 기록은 이미 1단계부터. 3D 가 바뀌어 기구학 표시만 다른 것은 묻는다)
            Ojw.C3d.CSagPracState st = null;
            try { st = Ojw.C3d.SagPrac_LoadState(m_strArmCfgDir); }
            catch { st = null; }
            if (st != null && st.Step >= 1)
            {
                string strNew = m_C3d.SagDef_Sig(cand), strSaved = m_C3d.SagDef_Sig(m_ArmCfg.Arm);
                string strWas = (st.Def != null && st.Def.Length > 0) ? st.Def : strSaved;
                if (strNew != strWas && ArmDef_SigUser(strSaved) == ArmDef_SigUser(strWas) &&
                    !ArmMotor_Confirm(T + " — 실습 " + st.Step + "단계까지의 기록은 지금 정의로 한 것입니다.\r\n바꾸면 다음은 1. 오차 재기부터입니다 (DLL 이 막습니다).\r\n\r\n[예] 바꾸어 저장   [아니오] 취소", T))
                    return;
            }
            Action<Ojw.C3d.CSagArmSetup> put = ArmDef_Put(cand);
            string w = ArmSetup_CheckCfg(put);
            if (w.Length > 0)
            {
                ArmMotor_MsgBox(T + " — 값이 설정 범위 밖입니다 (sagarm.txt 와 같은 규칙):\r\n\r\n" + w.Replace(" · ", "\r\n") + "\r\n\r\n고친 뒤 다시 누르십시오.", MessageBoxIcon.Warning);
                return;
            }
            Ojw.C3d.CSagArmCfg prev = m_ArmCfg.Arm.Clone();
            Ojw.C3d.CSagArmSetup cfgBefore = m_ArmCfg;
            if (!ArmSetup_SaveCfg(m_strArmCfgDir, put))
            {
                if (object.ReferenceEquals(cfgBefore, m_ArmCfg)) ArmDef_Put(prev)(m_ArmCfg);   // 쓰지 못했으면 메모리의 정의도 앞 값으로 (작업이 파일에 없는 정의를 쓰지 않게)
                ArmDef_Validate();
                ArmMotor_MsgBox(T + " — " + Ojw.C3d.CSagArmSetup.FILE + " 에 저장하지 못했습니다" +
                                ((ArmSetup_CfgWarn().Length > 0) ? " (" + ArmSetup_CfgWarn() + ")" : " (urdf 폴더가 바뀌었거나 파일에 쓰지 못함 — Log 탭)") + ".\r\n" + strPath,
                                MessageBoxIcon.Warning);
                return;
            }
            ArmMotor_Log(T + " — " + chk.Summary + " (" + strPath + ")");
            ArmSetup_RefreshPage();                                  // 기구학 확인·맨 아래 줄 (새 정의로)
            Prac_Refresh();                                          // 실습 카드·안내 줄
            ArmDef_FillUi(m_ArmCfg.Arm);
            ArmSetup_UpdateEnable();
        }

        /// <summary>정의 서명 (DLL SagDef_Sig "f0|p11,12,13|w14|d90|k1A2B3C4D") 의 사용자 몫 — 기구학 표시 '|k…' 를 뺀 앞부분</summary>
        private static string ArmDef_SigUser(string strSig)
        {
            if (strSig == null) return "";
            int k = strSig.IndexOf("|k", StringComparison.Ordinal);
            return (k >= 0) ? strSig.Substring(0, k) : strSig;
        }

        /// <summary>본보기로 저장 — 지금 칸의 정의를 <urdf>\sagarm_defs\<이름>.txt 로 (DLL CSagArmSetup.SaveDefPreset — 정의 7키, 'OMX follower' 이름은 거절).
        /// sagarm.txt 는 바꾸지 않는다. 이름 칸도 그 이름으로 바꾼다 ([확인·저장] 을 누르면 robot 도 그 이름)</summary>
        private void btnDefPresetSave_Click(object sender, EventArgs e)
        {
            const string T = "본보기로 저장";
            if (m_strArmCfgDir == null) { ArmMotor_MsgBox(T + " — urdf 폴더를 아직 읽지 못했습니다 (출력 폴더 칸을 확인하십시오).", MessageBoxIcon.Information); return; }
            Ojw.C3d.CSagArmCfg cand;
            string why;
            if (!ArmDef_ReadUi(out cand, out why)) { ArmMotor_MsgBox(T + " — " + why, MessageBoxIcon.Warning); return; }
            string name = string.Equals(cand.DefName, Ojw.C3d.CSagArmSetup.DEF_OMX, StringComparison.OrdinalIgnoreCase) ? "" : cand.DefName;
            if (Ojw.CInputBox.Show(T, "이름 (예: 내 OMX 펜 80mm)", ref name) != DialogResult.OK) return;
            name = (name == null) ? "" : name.Trim();
            cand.DefName = name;
            string strPath;
            if (!Ojw.C3d.CSagArmSetup.SaveDefPreset(m_strArmCfgDir, cand, out strPath, out why))
            {
                ArmMotor_MsgBox(T + " — 저장하지 못했습니다: " + why, MessageBoxIcon.Warning);
                return;
            }
            bool bOk = m_ArmDefChk != null && m_ArmDefChk.Ok;
            ArmMotor_Log(T + " — " + strPath + (bOk ? "" : " (★지금 3D 와는 맞지 않는 정의)"));
            m_bArmDefUiSync = true;
            try { if (txtDefName.Text != name) txtDefName.Text = name; }
            finally { m_bArmDefUiSync = false; }
            ArmDef_FillPresets(strPath);
            ArmDef_Validate();
        }

        // ════════════════════════════════════════════════════════════
        // 수직 눈 확인 대화창 (DLL 이 묻는다 — 눈 확인만 SagPrac_EyeJob · 실물 1단계 SagPrac_ErrorJob)
        // ════════════════════════════════════════════════════════════

        /// <summary>수직 눈 확인 대리자 — DLL 이 펼친 자리(가운데 점 위, 옮기는 높이, 바닥으로 수직 자세)에서 작업 스레드로 부른다 (눈 확인만·실물 1단계).
        /// UI 스레드로 Invoke 해 대화창 (기본 [아니오]), 묻는 동안 3D 표시를 이 작업의 정의(arm)로 켜고 끝나면 체크 상태로 되돌린다.
        /// 폼이 닫혔거나 예외면 중단. UI 스레드는 작업을 기다리지 않으므로 (Extra_Run 스레드) 서로 멈추지 않는다</summary>
        private Ojw.C3d.DSagEyeAsk Prac_EyeAsk(Ojw.C3d.CSagArmCfg arm)
        {
            Ojw.C3d.CSagArmCfg a = (arm != null) ? arm.Clone() : null;
            return new Ojw.C3d.DSagEyeAsk(delegate(string strPrompt)
            {
                Ojw.C3d.ESagEyeAns ans = Ojw.C3d.ESagEyeAns.Stop;
                try
                {
                    if (IsDisposed || Disposing) return Ojw.C3d.ESagEyeAns.Stop;
                    Invoke(new Action(delegate()
                    {
                        if (IsDisposed) return;
                        m_ArmEyeCfg = a;
                        m_bArmEyeAsking = true;
                        ArmDef_UpdateMark();
                        try
                        {
                            DialogResult dr = ArmMotor_Ask(strPrompt, "수직 눈 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            ans = (dr == DialogResult.Yes) ? Ojw.C3d.ESagEyeAns.Yes : Ojw.C3d.ESagEyeAns.No;
                        }
                        finally
                        {
                            m_bArmEyeAsking = false;
                            m_ArmEyeCfg = null;
                            ArmDef_UpdateMark();
                        }
                    }));
                }
                catch { ans = Ojw.C3d.ESagEyeAns.Stop; }
                return ans;
            });
        }

        /// <summary>수직 눈 확인을 [아니오] (실물 펜이 수직이 아님) 로 마친 뒤 (실물 1단계·눈 확인만) — 영점 맞추는 곳을 알리고 [예] 면 모터 세팅 페이지로
        /// (UI 스레드, 끝난 뒤 처리가 끝난 다음에)</summary>
        private void Prac_EyeNoNotice()
        {
            if (IsDisposed) return;
            Ojw.C3d.CSagArmCfg a = (m_ArmCfg != null) ? m_ArmCfg.Arm : new Ojw.C3d.CSagArmCfg();
            CultureInfo ci = CultureInfo.InvariantCulture;
            string s = "실물 펜이 수직이 아니라고 답하셨습니다 (바닥을 누르지 않고 원위치함).\r\n\r\n" +
                       string.Format(ci, "모터 세팅 페이지에서 손목({0})과 팔 관절({1}·{2})을 3D 와 같은 자세로 놓고 [현재 위치를 0 으로] 로 영점을 맞춘 뒤 " +
                                         "수직 눈 확인을 다시 하십시오 ('로봇 정의' 페이지의 [수직 눈 확인], 또는 실습 1단계).\r\n",
                                     a.WristID, (a.PosIDs != null && a.PosIDs.Length == 3) ? a.PosIDs[1] : 0, (a.PosIDs != null && a.PosIDs.Length == 3) ? a.PosIDs[2] : 0) +
                       "('바닥으로 수직' 관절 합은 3D 와 맞으므로 고칠 곳은 정의가 아니라 모터 영점입니다)\r\n\r\n지금 모터 세팅 페이지로 갈까요?";
            try
            {
                BeginInvoke(new Action(delegate()
                {
                    if (IsDisposed) return;
                    if (ArmMotor_Ask(s, "수직 눈 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        tabArmInner.SelectedTab = tabArmMotor;
                }));
            }
            catch { }
        }
    }
}
