// ================================================================
// ⑤ 서보동정 / ⑥ 전도경계 / ⑦ 전원측정 — UI 껍데기
//   측정 본체는 전부 DLL: Ojw.CSim2Real.{ServoIdent, TipBoundary, PowerProfile}
//
//   이 세 버튼으로 sim2real 이 MakeUrdf 하나로 완결된다 (2026-07-31 사용자 지시).
//   그 전에는 속도상한·브레이크어웨이·전도경계·전압이 외부 콘솔 하네스였다.
//
//   자세 요약:  ①②⑤ = 매달림  /  ④⑥⑦ = 기립  /  ③ = 로봇 무관
// ================================================================
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using OpenJigWare;

namespace MakeUrdf
{
    public partial class Main
    {
        private Thread m_thrExtra = null;

        private bool Extra_Busy()
        {
            if ((m_thrExtra != null && m_thrExtra.IsAlive) ||
                (m_thrMech != null && m_thrMech.IsAlive) ||
                (m_thrStand != null && m_thrStand.IsAlive))
            { MessageBox.Show("다른 측정이 진행 중입니다."); return true; }
            return false;
        }

        private void Extra_Run(string strTitle, Func<string> work)
        {
            S2R_LogFileStart(strTitle);
            S2R_ShowPanel(strTitle);
            m_bMechStop = false;
            m_thrExtra = new Thread(delegate()
            {
                string strTail = work();
                S2R_Log("");
                S2R_Log("=========================================");
                S2R_Log(strTail != null ? "   측정 완료 — 이제 전원을 꺼도 됩니다" : "   중단/실패 — 위 로그 확인");
                S2R_Log("=========================================");
                S2R_Done(strTitle + (strTail != null ? " — 완료" : " — 중단"));
            });
            m_thrExtra.IsBackground = true;
            m_thrExtra.Start();
        }

        // ── ? 설명서 — 텍스트 본체는 DLL(GetGuideText) ─────────
        private void btnS2RGuide_Click(object sender, EventArgs e)
        {
            Form frm = new Form();
            frm.Text = "sim2real 측정 순서 — 기초 운용법";
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ClientSize = new System.Drawing.Size(560, 640);
            frm.MinimizeBox = false;
            frm.MaximizeBox = false;

            TextBox txt = new TextBox();
            txt.Multiline = true;
            txt.ReadOnly = true;
            txt.ScrollBars = ScrollBars.Vertical;
            txt.WordWrap = false;
            txt.Dock = DockStyle.Fill;
            txt.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            txt.ForeColor = System.Drawing.Color.Gainsboro;
            txt.Font = new System.Drawing.Font("맑은 고딕", 10F);
            txt.Text = Ojw.CSim2Real.GetGuideText();
            txt.SelectionStart = 0;   // 전체선택 상태로 뜨는 것 방지
            txt.SelectionLength = 0;

            frm.Controls.Add(txt);
            frm.ShowDialog(this);
        }

        // ── ⑤ 서보동정 (매달림) ────────────────────────────────
        private void btnServoIdent_Click(object sender, EventArgs e)
        {
            if (Extra_Busy()) return;
            string strDir = Path.Combine(GetUrdfOutputDir(), "sim2real");
            Directory.CreateDirectory(strDir);
            if (MessageBox.Show(
                    "⑤ 서보 동정 (매달림)\r\n\r\n" +
                    "★로봇을 매달아 주십시오 — 발이 땅에 닿으면 안 됩니다.\r\n\r\n" +
                    "· EEPROM 전체 덤프 (읽기 전용)\r\n" +
                    "· 축별 최대속도 — 큰 계단이라 팔다리가 빠르게 움직입니다\r\n" +
                    "· 브레이크어웨이·백래시 (0.088도씩 미는 준정적 시험)\r\n" +
                    "· PWM 은 운전값(885) — 낮추면 측정이 오염됩니다\r\n" +
                    "· 결과: sim2real\\servo_ident.json + eeprom_dump.csv\r\n\r\n" +
                    "15분 안팎 걸립니다. 진행할까요?",
                    "서보 동정", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            Extra_Run("⑤ 서보동정", delegate()
            {
                var cfg = new Ojw.CSim2Real.CIdentCfg();
                string strRep;
                bool bOk = Ojw.CSim2Real.ServoIdent(cfg, strDir,
                    new Ojw.CSim2Real.DLog(S2R_Log),
                    new Ojw.CSim2Real.DIsStopped(delegate() { return m_bMechStop; }), out strRep);
                return bOk ? strRep : null;
            });
        }

        // ── ⑥ 전도경계 (기립) ──────────────────────────────────
        private void btnTipBound_Click(object sender, EventArgs e)
        {
            if (Extra_Busy()) return;
            string strDir = Path.Combine(GetUrdfOutputDir(), "sim2real");
            Directory.CreateDirectory(strDir);
            if (Ojw.CSim2Real.LoadMechLimits(Path.Combine(strDir, "mech_limits.json")).Count == 0)
            { MessageBox.Show("mech_limits.json 이 없습니다 — 먼저 ② 관절리미트를 하십시오 (R9)."); return; }
            if (MessageBox.Show(
                    "⑥ 전도경계 측정 (기립)\r\n\r\n" +
                    "★두 발로 세워 두고, 옆에서 잡아줄 준비를 하십시오.\r\n" +
                    "  전도 '개시'를 감지해 즉시 후퇴합니다 — 실제로 넘기지는 않습니다.\r\n\r\n" +
                    "· 좌우(롤) ±24도 / 전방 20도 / 후방 30도 상한, 0.5도씩\r\n" +
                    "· 결과에서 유효 접지면($footcontact 제안값)을 자동 역산합니다\r\n" +
                    "· 결과: sim2real\\tiptest_pitch.json + tiptest_raw.csv\r\n\r\n" +
                    "10분 안팎 걸립니다. 진행할까요?",
                    "전도경계", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            Extra_Run("⑥ 전도경계", delegate()
            {
                var cfg = new Ojw.CSim2Real.CTipCfg();
                string strRep;
                bool bOk = Ojw.CSim2Real.TipBoundary(cfg, strDir,
                    new Ojw.CSim2Real.DLog(S2R_Log),
                    new Ojw.CSim2Real.DIsStopped(delegate() { return m_bMechStop; }), out strRep);
                return bOk ? strRep : null;
            });
        }

        // ── ⑧ 부하속도 (기립 부하 / 매달림 무부하 대조) — R7 ──
        private void btnLoadVmax_Click(object sender, EventArgs e)
        {
            if (Extra_Busy()) return;
            string strDir = Path.Combine(GetUrdfOutputDir(), "sim2real");
            Directory.CreateDirectory(strDir);
            if (Ojw.CSim2Real.LoadMechLimits(Path.Combine(strDir, "mech_limits.json")).Count == 0)
            { MessageBox.Show("mech_limits.json 이 없습니다 — 먼저 ② 관절리미트를 하십시오 (R9)."); return; }
            DialogResult drPosture = MessageBox.Show(
                "⑧ 부하 vmax (R7) — 자세를 고르십시오.\r\n\r\n" +
                "[예] = 기립 (체중 부하 실속도 — 본측정)\r\n" +
                "       ★두 발로 세우고, 프레임 지지나 사람 보조 필수.\r\n" +
                "       다리 6관절에 큰 스텝(20/40도)이 순서대로 나갑니다.\r\n" +
                "       몸이 크게 출렁이므로 잡아줄 준비를 하십시오.\r\n\r\n" +
                "[아니오] = 매달림 (무부하 대조 — 같은 명령 프로파일)\r\n\r\n" +
                "· 결과: sim2real\\load_vmax_{stand|hang}.json + _raw.csv\r\n" +
                "· 5분 안팎 걸립니다.",
                "부하 vmax", MessageBoxButtons.YesNoCancel);
            if (drPosture == DialogResult.Cancel) return;
            bool bStanding = (drPosture == DialogResult.Yes);
            bool bOneFoot = false;
            if (bStanding)
            {
                bOneFoot = (MessageBox.Show(
                    "한발 지지(전체중) 모드로 할까요? (회신10 요청 — 2점째)\r\n\r\n" +
                    "[예] = 한발 지지: 반대발을 접어 지지다리에 전체중을 싣고\r\n" +
                    "       지지쪽 3관절씩 좌우 교대 측정. ★반드시 잡아줄 것 —\r\n" +
                    "       체중은 지지다리에 실리게, 손은 균형만.\r\n\r\n" +
                    "[아니오] = 차렷 양발 (반체중 — 이미 측정했다면 불필요)",
                    "부하 수준", MessageBoxButtons.YesNo) == DialogResult.Yes);
            }
            Extra_Run(bStanding ? (bOneFoot ? "⑧ 부하속도(한발 전체중)" : "⑧ 부하속도(기립)")
                                : "⑧ 부하속도(매달림 대조)", delegate()
            {
                var cfg = new Ojw.CSim2Real.CLoadVmaxCfg();
                cfg.bStanding = bStanding;
                cfg.bOneFoot = bOneFoot;
                string strRep;
                bool bOk = Ojw.CSim2Real.LoadVmax(cfg, strDir,
                    new Ojw.CSim2Real.DLog(S2R_Log),
                    new Ojw.CSim2Real.DIsStopped(delegate() { return m_bMechStop; }), out strRep);
                return bOk ? strRep : null;
            });
        }

        // ── ⑦ 전원측정 (기립) ──────────────────────────────────
        private void btnPowerProf_Click(object sender, EventArgs e)
        {
            if (Extra_Busy()) return;
            string strDir = Path.Combine(GetUrdfOutputDir(), "sim2real");
            Directory.CreateDirectory(strDir);
            if (MessageBox.Show(
                    "⑦ 전원 프로파일 (기립, 배터리)\r\n\r\n" +
                    "★두 발로 세워 두십시오. 웅크림 펌프(무릎 굽혔다 펴기) 25회로\r\n" +
                    "  보행급 부하를 걸며 버스/다리말단 전압 강하를 잽니다.\r\n" +
                    "· 결과: sim2real\\power_profile.json\r\n\r\n" +
                    "2분 안팎 걸립니다. 진행할까요?",
                    "전원 프로파일", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            Extra_Run("⑦ 전원측정", delegate()
            {
                var cfg = new Ojw.CSim2Real.CStandCfg();
                string strRep;
                bool bOk = Ojw.CSim2Real.PowerProfile(cfg, strDir, 25,
                    new Ojw.CSim2Real.DLog(S2R_Log),
                    new Ojw.CSim2Real.DIsStopped(delegate() { return m_bMechStop; }), out strRep);
                return bOk ? strRep : null;
            });
        }
    }
}
