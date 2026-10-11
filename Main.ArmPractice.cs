// ================================================================
// 로봇팔 세팅 탭 — 처짐 보정 실습 페이지 (2026-10-04)
//   학생용 네 단계: 1 오차 재기 → 2 보정 데이터 만들기 → 3 효과 보기 → 4 적용 (자동 보정).
//   바닥을 살짝 눌러 처음 닿은 높이로 재므로 자도 손 입력도 없다. 처음 쓰는 로봇(모델·데이터셋 없음)도 1단계가 바닥부터 찾는다.
//   절차·계산·판정·파일·그래프 자료는 전부 DLL (Ojw.C3d.SagPrac_* — COjw_12_3D_SagPractice.cs). 여기서는
//   ① 확인창 (실물은 [예] 를 눌러야 — 기본 [아니오]) ② 작업 띄우기 (ArmSetup_Run — 고급 페이지와 같은 잠금·로그 창·중단·Extra_Run 스레드)
//   ③ 버튼 차례 (진행 상태 <urdf>\sagcomp_practice\state.txt — 앱을 다시 켜도 이어서) ④ 그래프 그리기 (DLL SagPrac_ChartData 의 값만) 만 한다.
//   [3D만] 은 실물 없이 가상 로봇으로 같은 순서를 보여 준다 (결과는 실습 폴더 안에만, 적용 안 함).
//   1·3단계 동안은 처짐 쌍둥이(실물이 있을 자세 — 반투명 팔)를 켜고 끝나면 앞 상태로 되돌린다.
//   설정: 옮기는 높이·경유점 등 팔 설정은 고급 페이지와 같은 sagarm.txt (DLL CSagArmSetup), 실습 점·창은 DLL 기본값 (CSagPracCfg).
//   로봇 정의 카드 (한 줄 — 정의 확인 · 꺾는 손목 · 수직 눈 확인, [3D 표시] · [정의 바꾸기] → '로봇 정의' 페이지): Main.ArmDef.cs.
//   수직 눈 확인: 새 로봇·정의·모터 영점의 실물 1단계는 바닥을 누르기 전에 펼친 자리에서 "실물 펜이 수직입니까?" 를 한 번 묻는다
//   (DLL 이 묻고 기록 — 대화창은 Prac_EyeAsk). [아니오] 면 누르지 않고 원위치, 모터 세팅 페이지(영점)로 안내.
//   2·3단계 실물은 지금 정의·모터 영점으로 눈 확인을 했어야 시작한다 (영점·정의를 고쳤으면 다시 — ArmDef_EyeGate, DLL 도 거절).
//   1단계를 다시 하지 않고 눈 확인만 하는 길: 카드의 [눈 확인하기] (기록이 없을 때) · '로봇 정의' 페이지의 [수직 눈 확인] (Main.ArmDef.cs — 진행은 그대로).
//   컨트롤은 Main.Designer.cs 에 있다 (tabArmSetup → tabArmInner → tabArmPrac).
// ================================================================
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using OpenJigWare;

namespace MakeUrdf
{
    public partial class Main
    {
        private Ojw.C3d.CSagPracState m_PracState = null;    // 진행 상태 (마지막으로 읽은 것)
        private Ojw.C3d.CSagPracChart m_PracChart = null;    // 그래프 자료 (DLL)
        private string m_strPracDir = null;                  // 그 둘을 읽은 urdf 폴더
        private int m_nPracTwinSaved = -1;                   // 실습이 쌍둥이 표시를 켰으면 그 전 선택 (끝나면 되돌림), 아니면 −1
        private bool m_bPracInit = false;

        // ════════════════════════════════════════════════════════════
        // 표시
        // ════════════════════════════════════════════════════════════

        /// <summary>ArmSetup_Init 끝에서 한 번 — 진행 상태를 읽고 [3D만] 을 마지막 실습과 같게 (이어서 할 수 있게)</summary>
        private void Prac_Init()
        {
            Prac_Refresh();
            if (m_PracState != null && m_PracState.Step >= 1 && m_PracState.Step < 4) chkPracDry.Checked = m_PracState.Dry;
            m_bPracInit = true;
            Prac_Labels();
        }

        /// <summary>진행 상태·그래프 자료를 다시 읽는다 (urdf 폴더 = 고급 페이지가 읽은 폴더 m_strArmCfgDir). 파일만 읽는다 — 3D·실물은 건드리지 않는다</summary>
        private void Prac_Refresh()
        {
            if (IsDisposed) return;
            try
            {
                m_strPracDir = m_strArmCfgDir;
                m_PracState = (m_strPracDir != null) ? Ojw.C3d.SagPrac_LoadState(m_strPracDir) : null;
                m_PracChart = (m_strPracDir != null) ? Ojw.C3d.SagPrac_ChartData(m_strPracDir) : null;
            }
            catch (Exception ex) { m_PracState = null; m_PracChart = null; ArmMotor_Log("실습 페이지 갱신 오류: " + ex.Message); }
            if (m_bPracInit) Prac_Labels();
            picPracChart.Invalidate();
        }

        /// <summary>단계 줄·결과 줄</summary>
        private void Prac_Labels()
        {
            Ojw.C3d.CSagPracState st = m_PracState;
            int n = (st == null) ? 0 : st.Step;
            string[] names = { "1. 오차 재기", "2. 보정 데이터 만들기", "3. 효과 보기", "4. 적용" };
            string s;
            if (n <= 0) s = "다음: 1. 오차 재기 (처음이면 바닥부터 찾습니다)";
            else if (n >= 4) s = "네 단계 끝 — 적용됨: 이제 모든 동작이 자동으로 보정됩니다";
            else s = "1~" + n + " 단계 끝" + (st.Dry ? " (3D만 실습)" : "") + " — 다음: " + names[n];
            if (n >= 3 && st != null && st.Dry) s += "\r\n3D만 실습은 적용하지 않습니다 — 실물로 하려면 [3D만] 을 끄고 1단계부터";
            else if (n >= 1 && st != null && st.Dry != chkPracDry.Checked) s += "\r\n★마지막 실습은 " + (st.Dry ? "3D만" : "실물") + " — 이어서 하려면 [3D만] 을 " + (st.Dry ? "켜십시오" : "끄십시오") + " (아니면 1단계부터)";
            else if (!chkPracDry.Checked && (m_ArmConn == null || !m_ArmConn.IsConnected)) s += "\r\n실물: 위 연결 줄에서 먼저 연결하십시오 (또는 [3D만])";
            if (!m_bArmKinOk)                                        // 단계 버튼이 꺼지는 까닭 — 로봇 정의가 3D 와 맞지 않음 (종류별 안내 ArmDef_KinHint — 까닭이 이미 담고 있으면 한 번만)
                s += "\r\n★버튼이 꺼져 있는 까닭: " + ((m_strArmKinWhy != null && m_strArmKinWhy.Length > 0) ? ArmDef_WhyHint(m_strArmKinWhy, m_eArmKinKind)
                                                                                                             : "로봇 정의를 아직 확인하지 않음 — " + ArmDef_KinHint(Ojw.C3d.ESagDefFail.None));
            ArmSetup_SetLabel(lblPracStep, s, (s.IndexOf('★') >= 0) ? Color.Firebrick : SystemColors.ControlText);
            Prac_DefLine();                                          // 로봇 정의 카드 (Main.ArmDef.cs)
            if (st != null)
            {
                string tip = "";
                foreach (string l in new string[] { st.Line1, st.Line2, st.Line3, st.Line4 }) if (!string.IsNullOrEmpty(l)) tip += (tip.Length > 0 ? "\r\n" : "") + l;
                if (st.Folder.Length > 0) tip += (tip.Length > 0 ? "\r\n" : "") + "실습 폴더: " + st.FolderDir;
                tipArmExp.SetToolTip(lblPracStep, tip);
            }
            Ojw.C3d.CSagPracChart ch = m_PracChart;
            string r = (ch == null || ch.Line == null) ? "" : ch.Line;
            ArmSetup_SetLabel(lblPracResult, r, (ch != null && ch.HasAfter) ? Color.DarkGreen : Color.Firebrick);
        }

        /// <summary>버튼 활성 — ArmSetup_UpdateEnable 끝에서 (같은 잠금 조건). 차례: 2 는 1 뒤, 3 은 2 뒤, 4 는 3 뒤 (같은 종류 — 실물/3D만).
        /// 실물 단계는 연결이 있어야, 모든 단계는 로봇 정의가 3D 와 맞아야 (기구학 확인). [정의 바꾸기] 는 놀 때만, [3D 표시] 는 늘</summary>
        private void Prac_UpdateEnable(bool bIdle, bool bReal, bool bKin, bool bJob)
        {
            if (IsDisposed) return;
            Ojw.C3d.CSagPracState st = m_PracState;
            bool bDry = chkPracDry.Checked;
            bool bGo = bIdle && bKin && (bDry || bReal);
            bool bSame = st != null && st.Step >= 1 && st.Dry == bDry;
            btnPrac1.Enabled = bGo;
            btnPrac2.Enabled = bGo && bSame && st.Step >= 1;
            btnPrac3.Enabled = bGo && bSame && st.Step >= 2;
            btnPrac4.Enabled = bIdle && st != null && st.Step >= 3 && !st.Dry && !st.Applied;
            chkPracDry.Enabled = chkPracPrecise.Enabled = bIdle;
            btnPracDef.Enabled = bIdle;
            if (lnkPracEye.Enabled != bIdle) lnkPracEye.Enabled = bIdle;      // [눈 확인하기] (보임은 Prac_DefLine — 기록이 없을 때만)
            btnPracStop.Enabled = bJob;
            if (m_bPracInit) Prac_Labels();                          // 연결이 바뀌면 안내 줄도
        }

        private void chkPracDry_CheckedChanged(object sender, EventArgs e)
        {
            if (!m_bPracInit) return;
            Prac_Labels();
            ArmSetup_UpdateEnable();
        }

        // ════════════════════════════════════════════════════════════
        // 단계 버튼
        // ════════════════════════════════════════════════════════════

        /// <summary>작업 전 공통 — 계획 기구학을 지금 3D 로 (SagRun_Prepare), sagarm.txt 와 맞추고 (다시 읽었으면 페이지를 새로 — 아니면 로봇 정의 확인만 다시),
        /// 읽지 못함·그대로 쓰지 못한 줄이면 멈춤, 로봇 정의가 3D 와 맞는지 (종류별 안내). arm = 팔 설정 사본 (옮기는 높이·경유점 …), strDir = urdf 폴더</summary>
        private bool Prac_Prepare(string strTitle, out Ojw.C3d.CSagArmCfg arm, out string strDir)
        {
            arm = null; strDir = null;
            ArmDef_Prepare();                                    // 3D 모델이 바뀌었으면 계획 기구학을 지금 수식으로 (UI 스레드 — 작업 중이면 DLL 이 바꾸지 않는다)
            if (ArmSetup_SyncCfg() && !ArmSetup_AnyBusy()) { ArmSetup_RefreshPage(); Prac_Refresh(); ArmSetup_UpdateEnable(); }
            else if (!ArmSetup_AnyBusy())
            {
                bool bWas = m_bArmKinOk;
                ArmSetup_CheckKin();                                 // 정의 확인은 DLL 캐시 — 가볍다 (탭을 본 뒤 3D 가 바뀌었을 수도)
                if (bWas != m_bArmKinOk) { ArmSetup_StatusLine(); ArmSetup_UpdateEnable(); }
            }
            if (m_strArmCfgFail != null) return ArmSetup_CfgFailWarn(strTitle);
            if (ArmSetup_CfgWarn().Length > 0) return ArmSetup_CfgLineWarn(strTitle);
            if (m_ArmCfg == null || m_strArmCfgDir == null || !Directory.Exists(m_strArmCfgDir))
            {
                ArmMotor_MsgBox(strTitle + " — urdf 폴더가 없습니다 (" + m_strArmCfgDir + ").\r\n로봇 모델(예: 예제 OMX follower)을 불러와 내보낸 뒤 다시 누르십시오.", MessageBoxIcon.Information);
                return false;
            }
            if (!m_bArmKinOk)
            {
                string w = (m_strArmKinWhy.Length > 0) ? m_strArmKinWhy : "로봇 정의를 확인하지 못했습니다";
                bool bHint = ArmDef_WhyHint(w, m_eArmKinKind) != w;                   // 까닭이 이미 안내를 담고 있으면 (DLL '… — 앱을 다시 켜십시오') 한 번만
                ArmMotor_MsgBox(strTitle + " — " + w + (bHint ? "\r\n" + ArmDef_KinHint(m_eArmKinKind) + "." : ""), MessageBoxIcon.Information);
                return false;
            }
            arm = m_ArmCfg.Arm.Clone();
            strDir = m_strArmCfgDir;
            return true;
        }

        /// <summary>실물 확인창 (기본 [아니오]) — U4 경고(방향 확인 안 된 축·저장 안 된 세팅) 뒤, 연결이 그사이 바뀌었으면 시작 안 함</summary>
        private bool Prac_ConfirmReal(string strTitle, Ojw.C3d.CSagArmCfg arm, string strBody)
        {
            if (!ArmSetup_CheckStart(strTitle, arm)) return false;
            bool bTq = m_ArmConn.IsTorqueOn;
            string s = strTitle + " — 로봇팔이 실제로 움직입니다.\r\n\r\n" + strBody +
                       ArmSetup_PortLine(arm) + ArmSetup_PathLine(arm) +
                       (bTq ? "· 토크는 이미 켜져 있음 — 끝나면 휴지 자세로 돌아가 토크 ON 그대로\r\n" : "· 지금 자세(휴지)를 읽어 두고 토크를 켭니다 — 끝나면 그 자세로 돌아가 토크를 끕니다\r\n") +
                       "· 바닥은 천천히 내려가 살짝 닿으면 멈추고 들어 올립니다 (세게 누르지 않음)\r\n" +
                       "· 팔 둘레를 비우고 손을 떼십시오. [중단] 은 언제든 — 지금 점을 마치고 원위치합니다.\r\n\r\n[예] 시작   [아니오] 취소";
            if (!ArmMotor_Confirm(s, strTitle)) return false;
            return ArmSetup_CheckConn(strTitle);
        }

        private void btnPrac1_Click(object sender, EventArgs e)
        {
            const string T = "1. 오차 재기";
            Ojw.C3d.CSagArmCfg arm; string strDir;
            if (!Prac_Prepare(T, out arm, out strDir)) return;
            bool bDry = chkPracDry.Checked, bPrecise = chkPracPrecise.Checked;
            Ojw.C3d.CSagPracCfg cfg = new Ojw.C3d.CSagPracCfg();
            Ojw.C3d.CSagPracState st = m_PracState;
            if (st != null && st.Step >= 2 &&
                !ArmMotor_Confirm(T + " — 새 실습을 시작합니다.\r\n지금 실습(" + st.Step + "단계까지)의 기록은 그 폴더에 남고, 진행은 1단계부터 다시입니다.\r\n\r\n[예] 새로 시작   [아니오] 취소", T)) return;
            if (!bDry)
            {
                string strEye;
                bool bEye = m_C3d.SagPrac_EyeNeeded(strDir, m_ArmConn, arm, out strEye);   // 이 로봇·정의·모터 영점으로 수직 눈 확인을 했나 (DLL — state.txt)
                string body = "· 보정을 모두 끄고 점마다 바닥을 살짝 눌러, 3D 가 믿는 높이와 실제 닿는 높이의 차를 잽니다\r\n" +
                              "· " + Ojw.C3d.SagPrac_PlanText(cfg, 1, bPrecise) + "\r\n" +
                              "· 바닥 높이를 모르면 가운데 점에서 천천히 내려가 바닥부터 찾습니다\r\n" +
                              (bEye ? "· 처음 쓰는 로봇·정의·영점이라, 바닥을 누르기 전에 수직 자세로 멈춰 실물 펜이 수직인지 묻습니다\r\n" : "");
                if (!Prac_ConfirmReal(T, arm, body)) return;
            }
            Ojw.C3d.DSagEyeAsk eyeAsk = Prac_EyeAsk(arm);              // 수직 눈 확인 대화창 (실물·처음일 때만 DLL 이 부른다 — Main.ArmDef.cs)
            Prac_Start(T, bDry, true, delegate(Ojw.CSim2Real.DLog log, Ojw.CSim2Real.DIsStopped stop)
            {
                return m_C3d.SagPrac_ErrorJob(m_ArmConn, arm, cfg, strDir, bPrecise, bDry, eyeAsk, log, stop);
            });
        }

        private void btnPrac2_Click(object sender, EventArgs e)
        {
            const string T = "2. 보정 데이터 만들기";
            Ojw.C3d.CSagArmCfg arm; string strDir;
            if (!Prac_Prepare(T, out arm, out strDir)) return;
            bool bDry = chkPracDry.Checked, bPrecise = chkPracPrecise.Checked;
            Ojw.C3d.CSagPracCfg cfg = new Ojw.C3d.CSagPracCfg();
            string strKey = ArmSetup_MapKey();
            if (!bDry)
            {
                if (!ArmDef_EyeGate(T, arm)) return;             // 수직 눈 확인 전 (1단계 뒤 영점을 고쳤어도) — 하는 곳을 알리고 시작하지 않는다 (DLL 도 거절)
                string body = "· 먼저 가운데 점에서 바닥이 1단계와 같은지 봅니다 (다르면 멈춤 — 1단계부터)\r\n" +
                              "· 바닥보다 높은 공중에서 점마다 멈춰 관절 처짐을 잽니다 → 관절 모델 (새 파일)\r\n" +
                              "· 바닥을 더 눌러 데이터를 모아 로봇 데이터셋(floor_dataset_fc.csv)에 덧붙이고, 원위치 뒤 학습합니다 → 결과 파일\r\n" +
                              "· " + Ojw.C3d.SagPrac_PlanText(cfg, 2, bPrecise) + "\r\n" +
                              "· 적용 중 보정 파일은 바꾸지 않습니다 (4단계에서)\r\n";
                if (!Prac_ConfirmReal(T, arm, body)) return;
            }
            Prac_Start(T, bDry, false, delegate(Ojw.CSim2Real.DLog log, Ojw.CSim2Real.DIsStopped stop)
            {
                return m_C3d.SagPrac_BuildJob(m_ArmConn, arm, cfg, strDir, bPrecise, bDry, strKey, log, stop);
            });
        }

        private void btnPrac3_Click(object sender, EventArgs e)
        {
            const string T = "3. 효과 보기";
            Ojw.C3d.CSagArmCfg arm; string strDir;
            if (!Prac_Prepare(T, out arm, out strDir)) return;
            bool bDry = chkPracDry.Checked;
            Ojw.C3d.CSagPracCfg cfg = new Ojw.C3d.CSagPracCfg();
            if (!bDry)
            {
                if (!ArmDef_EyeGate(T, arm)) return;             // 수직 눈 확인 전 (1단계 뒤 영점을 고쳤어도) — 하는 곳을 알리고 시작하지 않는다 (DLL 도 거절)
                Ojw.C3d.CSagPracState st = m_PracState;
                string strMap = "";
                try { if (st != null) strMap = Ojw.C3d.SagArm_MotorMapWarn(m_ArmConn, Ojw.C3d.CSagComp.Load(st.ResultPath)); }
                catch { strMap = ""; }
                string body = "· 2단계 보정을 켜고 1단계와 같은 점을 다시 눌러 오차가 얼마나 줄었는지 봅니다 (먼저 가운데 점 바닥 확인)\r\n" +
                              "· " + Ojw.C3d.SagPrac_PlanText(cfg, 3, false) + "\r\n" +
                              ((st != null && st.ResultPath.Length > 0) ? "· 보정 파일: " + Path.GetFileName(st.ResultPath) + "\r\n" : "") +
                              ((strMap.Length > 0) ? "· " + strMap + "\r\n" : "");
                if (!Prac_ConfirmReal(T, arm, body)) return;
            }
            Prac_Start(T, bDry, true, delegate(Ojw.CSim2Real.DLog log, Ojw.CSim2Real.DIsStopped stop)
            {
                return m_C3d.SagPrac_VerifyJob(m_ArmConn, arm, cfg, strDir, null, bDry, log, stop);
            });
        }

        /// <summary>4. 적용 — UI 스레드 (DLL 규칙: 적용은 작업 중이면 거절). 고급 페이지 ⑥ 과 같은 막음·뒤처리 (ArmSetup_CanSwitchComp·ArmSetup_AfterSwitch)</summary>
        private void btnPrac4_Click(object sender, EventArgs e)
        {
            const string T = "4. 적용";
            if (!ArmSetup_CanSwitchComp(T)) return;
            Ojw.C3d.CSagArmCfg arm; string strDir;
            if (!Prac_Prepare(T, out arm, out strDir)) return;
            Ojw.C3d.CSagPracState st = Ojw.C3d.SagPrac_LoadState(strDir);
            if (st.Step < 3 || st.Dry || string.IsNullOrEmpty(st.ResultPath))
            {
                ArmMotor_MsgBox(T + " — 실물로 3. 효과 보기까지 마친 뒤에 적용합니다.", MessageBoxIcon.Information);
                Prac_Refresh(); ArmSetup_UpdateEnable();
                return;
            }
            string d = ArmSetup_MapDiff(Prac_FileMap(st.ResultPath));
            string s = "이 보정을 적용합니다: " + Path.GetFileName(st.ResultPath) + "\r\n" +
                       ((st.Line3.Length > 0) ? "효과: " + st.Line3 + "\r\n" : "") +
                       ((d.Length > 0) ? "★학습 때와 방향·영점이 다름 (" + d + ") — 다시 학습 권장\r\n" : "") +
                       "적용할 urdf 폴더: " + strDir + "\r\n" +
                       "지금 보정 파일은 sagcomp_backup\\… 로 백업됩니다. 적용하면 1초에 걸쳐 새 보정으로 바뀌고, 앱을 다시 켜도 그 뒤 모든 동작이 자동으로 보정됩니다." +
                       ArmSetup_SwitchNote();
            if (!ArmMotor_Confirm(s, T)) return;
            if (!ArmSetup_CanSwitchComp(T)) return;
            string strBak, rep;
            bool bOk = m_C3d.SagPrac_Apply(strDir, ArmSetup_MapKey(), new Ojw.CSim2Real.DLog(ArmMotor_Log), out strBak, out rep);
            ArmSetup_AfterSwitch(T, bOk, rep);                   // 보정 체크·쌍둥이·목록·조그 막힘 표시 (+ ArmSetup_OnShown → 실습 페이지)
            Prac_Busy((bOk ? "" : "★") + T + (bOk ? " 완료 — 이제 모든 동작이 자동 보정" : " 실패"));
        }
        // 결과 파일의 motormap (없으면 null) — 확인창의 '학습 때와 다름' 비교용
        private static string Prac_FileMap(string strPath)
        {
            try { Ojw.C3d.CSagComp m = Ojw.C3d.CSagComp.Load(strPath); return (m != null) ? m.MotorMap : null; }
            catch { return null; }
        }

        private void btnPracStop_Click(object sender, EventArgs e)
        {
            btnArmStop_Click(sender, e);                         // 같은 중단 (m_bMechStop)
            Prac_Busy("중단 요청함 — 지금 점을 마치고 원위치 중");
        }

        // ════════════════════════════════════════════════════════════
        // 작업 띄우기·결과
        // ════════════════════════════════════════════════════════════

        private delegate Ojw.C3d.CSagPracResult DPracJob(Ojw.CSim2Real.DLog log, Ojw.CSim2Real.DIsStopped stop);

        /// <summary>작업 하나 — ArmSetup_Run (잠금·로그 창·Extra_Run 스레드). bTwin = 처짐 쌍둥이 표시를 켠다 (1·3단계 — 끝나면 앞 상태로).
        /// 작업 스레드는 결과를 지역 변수에만, 화면은 끝난 뒤 UI 스레드에서 (Prac_After)</summary>
        private void Prac_Start(string strTitle, bool bDry, bool bTwin, DPracJob job)
        {
            Ojw.CSim2Real.DLog log = Prac_Log();
            Ojw.CSim2Real.DIsStopped stop = ArmSetup_Stop();
            Ojw.C3d.CSagPracResult res = null;
            if (bTwin) Prac_TwinShow(true);
            Prac_Busy("실행 중: " + strTitle + (bDry ? " (3D만)" : ""));
            bool bRun = ArmSetup_Run(strTitle + (bDry ? " (3D만)" : ""), !bDry, delegate()
            {
                res = job(log, stop);
                return (res != null && res.Ok) ? res.Line : null;
            }, delegate()
            {
                if (bTwin) Prac_TwinShow(false);
                Prac_After(strTitle, res);
            });
            if (!bRun) { if (bTwin) Prac_TwinShow(false); Prac_Busy("대기"); }
        }

        /// <summary>끝난 뒤 (UI 스레드) — 맨 아래 줄·결과 줄, 시작하지 않았거나 실패했으면 그 까닭을 대화창으로, 원위치를 확인하지 못했으면 경고.
        /// 수직 눈 확인에 [아니오] 였으면 그 까닭 대신 영점 맞추는 곳 (모터 세팅 페이지로 갈지 — Prac_EyeNoNotice)</summary>
        private void Prac_After(string strTitle, Ojw.C3d.CSagPracResult res)
        {
            if (res == null) { Prac_Busy("★" + strTitle + " — 결과 없음 (로그 창 확인)"); return; }
            m_strArmLast = res.Ok ? "" : (res.Stopped ? "중단" : "실패");
            string strLine = res.Line;
            Prac_Busy((res.Ok ? "" : "★") + strTitle + " — " + strLine);
            ArmSetup_JobNotice(strTitle, res.Ran, res.Dry, res.Stopped, res.RestOk, res.Why, res.RunDir);
            if (res.EyeAns == "아니오") Prac_EyeNoNotice();
            else if (res.Ran && !res.Ok && !res.Stopped && !string.IsNullOrEmpty(res.Why) && !IsDisposed)
            {
                string s = strTitle + " — 끝까지 하지 못했습니다:\r\n\r\n" + res.Why + "\r\n\r\n자세한 내용은 로그 창·실습 폴더의 기록을 보십시오.";
                try { BeginInvoke(new Action(delegate() { ArmMotor_MsgBox(s, MessageBoxIcon.Warning); })); } catch { }
            }
            Prac_Refresh();                                      // 진행 상태·그래프 (버튼 차례는 이어서 ArmSetup_OnShown → UpdateEnable)
        }

        /// <summary>작업 로그 — 로그 창(S2R)에, "진행 " 줄은 실습 페이지 아래 줄과 고급 페이지 아래 줄에도 (작업 스레드에서 불린다)</summary>
        private Ojw.CSim2Real.DLog Prac_Log()
        {
            return new Ojw.CSim2Real.DLog(delegate(string s)
            {
                try
                {
                    S2R_Log(s);
                    if (s != null && s.StartsWith("진행 ", StringComparison.Ordinal) && !IsDisposed)
                        BeginInvoke(new Action(delegate() { Prac_Busy(s); ArmSetup_Busy(s); }));
                }
                catch { }
            });
        }

        private void Prac_Busy(string s)
        {
            if (IsDisposed) return;
            ArmSetup_SetLabel(lblPracBusy, s, (s != null && s.StartsWith("★")) ? Color.Firebrick : SystemColors.ControlText);
        }

        /// <summary>처짐 쌍둥이 표시 — 켤 때: 꺼져 있으면 '3D' 로 (앞 선택 기억), 끌 때: 실습이 켰으면 앞 선택으로</summary>
        private void Prac_TwinShow(bool bOn)
        {
            try
            {
                if (bOn)
                {
                    m_nPracTwinSaved = -1;
                    if (cmbSagTwin.SelectedIndex <= 0 && cmbSagTwin.Items.Count >= 2) { m_nPracTwinSaved = cmbSagTwin.SelectedIndex; cmbSagTwin.SelectedIndex = 1; ArmMotor_Log("실습 — 처짐 쌍둥이 표시 켬 (3D)"); }
                }
                else if (m_nPracTwinSaved >= 0)
                {
                    if (cmbSagTwin.SelectedIndex == 1) cmbSagTwin.SelectedIndex = m_nPracTwinSaved;
                    m_nPracTwinSaved = -1;
                }
            }
            catch (Exception ex) { ArmMotor_Log("쌍둥이 표시 오류: " + ex.Message); }
        }

        // ════════════════════════════════════════════════════════════
        // 그래프 — 반경마다 닿은 높이 (빨강 = 보정 끔, 초록 = 보정 켬), 평평한 바닥 기준선 (점선). 값은 DLL SagPrac_ChartData
        // ════════════════════════════════════════════════════════════

        private void picPracChart_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rc = picPracChart.ClientRectangle;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            CultureInfo ci = CultureInfo.InvariantCulture;
            Ojw.C3d.CSagPracChart ch = m_PracChart;
            using (Font f = new Font("맑은 고딕", 8F))
            using (Font fb = new Font("맑은 고딕", 8.5F, FontStyle.Bold))
            {
                if (ch == null || !ch.HasBefore)
                {
                    TextRenderer.DrawText(g, "1. 오차 재기를 하면 여기에 반경마다 바닥에 닿은 높이가 그려집니다.\r\n바닥은 평평하니 3D 가 맞다면 모두 같은 높이여야 합니다.",
                                          f, rc, Color.DimGray, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
                    return;
                }
                // 범위
                float rMin = float.MaxValue, rMax = float.MinValue, zMin = float.MaxValue, zMax = float.MinValue;
                for (int i = 0; i < ch.R.Length; i++)
                {
                    rMin = Math.Min(rMin, ch.R[i]); rMax = Math.Max(rMax, ch.R[i]);
                    foreach (float v in new float[] { ch.Before[i], ch.After[i] }) if (!float.IsNaN(v)) { zMin = Math.Min(zMin, v); zMax = Math.Max(zMax, v); }
                }
                if (!double.IsNaN(ch.RefZ)) { zMin = Math.Min(zMin, (float)ch.RefZ); zMax = Math.Max(zMax, (float)ch.RefZ); }
                if (zMin > zMax) { zMin = 0; zMax = 1; }
                float zMid = 0.5f * (zMin + zMax), zSpan = Math.Max(10f, (zMax - zMin) * 1.25f);
                zMin = zMid - zSpan / 2; zMax = zMid + zSpan / 2;
                rMin -= 10; rMax += 10;
                Rectangle pa = new Rectangle(rc.Left + 46, rc.Top + 24, Math.Max(10, rc.Width - 58), Math.Max(10, rc.Height - 52));
                Func<float, float> X = r => pa.Left + (r - rMin) / (rMax - rMin) * pa.Width;
                Func<float, float> Y = z => pa.Bottom - (z - zMin) / (zMax - zMin) * pa.Height;
                // 격자·눈금
                using (Pen pg = new Pen(Color.Gainsboro))
                using (Brush bt = new SolidBrush(Color.DimGray))
                {
                    float step = Prac_NiceStep(zMax - zMin);
                    for (float z = (float)Math.Ceiling(zMin / step) * step; z <= zMax; z += step)
                    {
                        g.DrawLine(pg, pa.Left, Y(z), pa.Right, Y(z));
                        g.DrawString(z.ToString("0.#", ci), f, bt, rc.Left + 2, Y(z) - 7);
                    }
                    for (float r = (float)Math.Ceiling(rMin / 30f) * 30f; r <= rMax; r += 30f)
                    {
                        g.DrawLine(pg, X(r), pa.Top, X(r), pa.Bottom);
                        g.DrawString(r.ToString("0", ci), f, bt, X(r) - 10, pa.Bottom + 2);
                    }
                    g.DrawRectangle(Pens.Gray, pa);
                    g.DrawString("닿은 높이 (mm)", f, bt, rc.Left + 2, rc.Top + 4);
                    g.DrawString("반경 (mm)", f, bt, pa.Right - 52, pa.Bottom + 14);
                }
                // 기준선 (평평한 바닥)
                if (!double.IsNaN(ch.RefZ))
                    using (Pen pr = new Pen(Color.DimGray, 1.5f))
                    {
                        pr.DashStyle = DashStyle.Dash;
                        float yr = Y((float)ch.RefZ);
                        g.DrawLine(pr, pa.Left, yr, pa.Right, yr);
                        using (Brush br = new SolidBrush(Color.DimGray)) g.DrawString(ch.RefLabel, f, br, pa.Left + 4, yr + 1);
                    }
                // 전(빨강)·후(초록) — 요 0 은 선으로 잇고, 다른 요는 점만
                Prac_DrawSeries(g, ch, ch.Before, Color.Firebrick, X, Y);
                if (ch.HasAfter) Prac_DrawSeries(g, ch, ch.After, Color.ForestGreen, X, Y);
                // 범례
                using (Brush b1 = new SolidBrush(Color.Firebrick))
                using (Brush b2 = new SolidBrush(Color.ForestGreen))
                {
                    float lx = pa.Left + 110;
                    g.FillEllipse(b1, lx, rc.Top + 7, 8, 8);
                    g.DrawString("보정 끔 (1. 오차 재기)", fb, b1, lx + 11, rc.Top + 3);
                    if (ch.HasAfter)
                    {
                        g.FillEllipse(b2, lx + 170, rc.Top + 7, 8, 8);
                        g.DrawString("보정 켬 (3. 효과 보기)", fb, b2, lx + 181, rc.Top + 3);
                    }
                }
            }
        }
        private static void Prac_DrawSeries(Graphics g, Ojw.C3d.CSagPracChart ch, float[] v, Color c, Func<float, float> X, Func<float, float> Y)
        {
            List<int> line = new List<int>();
            for (int i = 0; i < ch.R.Length; i++) if (Math.Abs(ch.Yaw[i]) < 0.5f && !float.IsNaN(v[i])) line.Add(i);
            line.Sort(delegate(int a, int b) { return ch.R[a].CompareTo(ch.R[b]); });
            using (Pen p = new Pen(c, 2f))
            using (Brush b = new SolidBrush(c))
            {
                for (int k = 1; k < line.Count; k++) g.DrawLine(p, X(ch.R[line[k - 1]]), Y(v[line[k - 1]]), X(ch.R[line[k]]), Y(v[line[k]]));
                for (int i = 0; i < ch.R.Length; i++)
                {
                    if (float.IsNaN(v[i])) continue;
                    float x = X(ch.R[i]), y = Y(v[i]);
                    if (Math.Abs(ch.Yaw[i]) < 0.5f) g.FillEllipse(b, x - 4, y - 4, 8, 8);
                    else g.DrawRectangle(p, x - 3.5f, y - 3.5f, 7, 7);   // 다른 요 (정밀) — 네모
                }
            }
        }
        // 눈금 간격 — 1·2·5 × 10^n 중 5~8 칸이 되는 것
        private static float Prac_NiceStep(float span)
        {
            if (!(span > 0)) return 1f;
            double raw = span / 6.0, p = Math.Pow(10, Math.Floor(Math.Log10(raw)));
            foreach (double m in new double[] { 1, 2, 5, 10 }) if (m * p >= raw) return (float)(m * p);
            return (float)(10 * p);
        }
    }
}
