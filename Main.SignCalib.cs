// ================================================================
// ① 관절 방향 검사 — UI 껍데기
//   측정 본체는 DLL 에 있다: Ojw.CSim2Real.SignCalib()  (COjw_42_Sim2Real.cs)
//   여기서는 설정 읽기 / 확인 대화 / 방향 판정 패널 / 3D 표시만 담당한다.
//
//   저장된 joint_map.json 을 **적용해서** 움직이므로 재실행이 재측정이 아니라 검증이 된다.
//   결과 스키마는 humanoid sign_calib 산출물과 동일 — 그쪽 러너가 그대로 읽는다.
// ================================================================
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using OpenJigWare;

namespace MakeUrdf
{
    public partial class Main
    {
        private Thread m_thrSignCalib = null;

        // ── 설정 파일 (앱 쪽 관심사) ───────────────────────────
        private Ojw.CSim2Real.CSignCfg SignCalib_LoadConfig(string strDir)
        {
            var cfg = new Ojw.CSim2Real.CSignCfg();
            string strPath = Path.Combine(strDir, "signcalib.txt");
            if (!File.Exists(strPath))
            {
                var sb = new StringBuilder();
                sb.AppendLine("// 관절 방향 검사 설정");
                sb.AppendLine("// port/baud/ids/idoffset/sweep/movems/pwm/home, sweep,<id>,<deg> 로 개별 지정");
                sb.AppendLine("port=" + cfg.nPort);
                sb.AppendLine("baud=" + cfg.nBaud);
                sb.AppendLine("ids=1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17");
                sb.AppendLine("idoffset=0");
                sb.AppendLine("sweep=15");
                sb.AppendLine("movems=450");
                sb.AppendLine("pwm=400");
                sb.AppendLine("home=0");
                File.WriteAllText(strPath, sb.ToString(), Encoding.UTF8);
                return cfg;
            }
            foreach (string strRaw in File.ReadAllLines(strPath, Encoding.UTF8))
            {
                string s = strRaw.Trim();
                if (s.Length == 0 || s.StartsWith("//")) continue;
                try
                {
                    if (s.StartsWith("port=")) cfg.nPort = int.Parse(s.Substring(5));
                    else if (s.StartsWith("baud=")) cfg.nBaud = int.Parse(s.Substring(5));
                    else if (s.StartsWith("idoffset=")) cfg.nIdOffset = int.Parse(s.Substring(9));
                    else if (s.StartsWith("sweep=")) cfg.fSweep = Ojw.CConvert.StrToFloat(s.Substring(6));
                    else if (s.StartsWith("movems=")) cfg.nMoveMs = int.Parse(s.Substring(7));
                    else if (s.StartsWith("pwm=")) cfg.nPwm = int.Parse(s.Substring(4));
                    else if (s.StartsWith("home=")) cfg.bHome = (s.Substring(5).Trim() != "0");
                    else if (s.StartsWith("ids="))
                    {
                        string[] t = s.Substring(4).Split(',');
                        var l = new List<int>();
                        foreach (string x in t) if (x.Trim().Length > 0) l.Add(int.Parse(x.Trim()));
                        cfg.anIDs = l.ToArray();
                    }
                    else if (s.StartsWith("sweep,"))
                    {
                        string[] t = s.Split(',');
                        cfg.Sweep[int.Parse(t[1])] = Ojw.CConvert.StrToFloat(t[2]);
                    }
                }
                catch { }
            }
            return cfg;
        }

        // ── 3D 표시 (워커 -> UI 마샬링) ────────────────────────
        private void SC_Show3d(int nT, float fDeg)
        {
            if (m_C3d == null) return;
            if (InvokeRequired) Invoke(new Action(delegate() { m_C3d.SetData(nT, fDeg); }));
            else m_C3d.SetData(nT, fDeg);
        }

        // ── 방향 판정 패널 (비모달) ────────────────────────────
        //   모달을 쓰면 판정하는 동안 3D 를 돌려볼 수 없다. 방향 판단은 시점을 바꿔가며
        //   봐야 하는 일이라 그게 치명적이다. 그래서 비모달로 띄우고 워커는 이벤트로 기다린다.
        private Ojw.CSim2Real.ESignAns m_eSignAns = Ojw.CSim2Real.ESignAns.Abort;
        private readonly ManualResetEvent m_evSignAns = new ManualResetEvent(false);
        private Form m_frmSignAsk = null;

        private void SC_BuildAskForm(int nT, int nId, float fSw, int nSign, bool bKnown, int nDone, int nTotal)
        {
            var f = new Form();
            f.Text = string.Format("T{0} 방향 확인  ({1}/{2})", nT, nDone, nTotal);
            f.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            f.ShowInTaskbar = false; f.TopMost = true;
            f.StartPosition = FormStartPosition.Manual;
            f.ClientSize = new Size(342, 212);
            f.BackColor = Color.FromArgb(45, 45, 48);

            var lbl = new Label();
            lbl.ForeColor = Color.White;
            lbl.Font = new Font("맑은 고딕", 9.5F);
            lbl.SetBounds(12, 10, 318, 62);
            lbl.Text = string.Format(
                "T{0} (ID {1}) — 3D 를 {2:+0;-0}도 움직였고,\r\n" +
                "저장된 부호 {3:+0;-0} 를 적용해 실기를 돌렸습니다.\r\n" +
                "같은 방향입니까?  (3D 는 마우스로 돌려볼 수 있습니다)", nT, nId, fSw, nSign);
            f.Controls.Add(lbl);

            var lblSt = new Label();
            lblSt.ForeColor = bKnown ? Color.DeepSkyBlue : Color.Gray;
            lblSt.Font = new Font("맑은 고딕", 9F);
            lblSt.SetBounds(12, 72, 318, 20);
            lblSt.Text = bKnown ? "저장된 값 있음 — 맞으면 [같다] 로 유지하면 됩니다"
                                : "저장된 값 없음 — 기본 +1 로 시도 중";
            f.Controls.Add(lblSt);

            // ★버튼으로 답했는지 표시. Close() 가 FormClosed 를 **동기적으로** 부르므로
            //   이벤트 신호만으로 판단하면 버튼 답이 X 닫기로 오인되어 중단으로 덮어써진다.
            bool bAnswered = false;
            var mk = new Func<string, int, int, int, Color, Ojw.CSim2Real.ESignAns, Button>(
                delegate(string strText, int x, int y, int w, Color c, Ojw.CSim2Real.ESignAns ans)
                {
                    var b = new Button();
                    b.Text = strText; b.SetBounds(x, y, w, 38);
                    b.FlatStyle = FlatStyle.Flat;
                    b.BackColor = Color.FromArgb(60, 60, 60); b.ForeColor = c;
                    b.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
                    b.Click += new EventHandler(delegate(object s, EventArgs e)
                    {
                        m_eSignAns = ans; bAnswered = true;
                        m_evSignAns.Set();      // 신호를 먼저, 닫기는 그 다음
                        f.Close();
                    });
                    f.Controls.Add(b);
                    return b;
                });

            mk("같다 (유지)", 12, 98, 104, Color.Lime, Ojw.CSim2Real.ESignAns.Same);
            mk("반대 (뒤집기)", 122, 98, 104, Color.Orange, Ojw.CSim2Real.ESignAns.Opposite);
            mk("다시 보기", 232, 98, 98, Color.White, Ojw.CSim2Real.ESignAns.Again);
            mk("건너뛰기", 12, 142, 104, Color.Silver, Ojw.CSim2Real.ESignAns.Skip);
            mk("중단 (저장 안 함)", 122, 142, 208, Color.Tomato, Ojw.CSim2Real.ESignAns.Abort);

            f.FormClosed += new FormClosedEventHandler(delegate(object s, FormClosedEventArgs e)
            { if (!bAnswered) { m_eSignAns = Ojw.CSim2Real.ESignAns.Abort; m_evSignAns.Set(); } });

            f.Location = new Point(Math.Max(0, Bounds.Right - f.Width - 24), Bounds.Top + 90);
            m_frmSignAsk = f;
            f.Show(this);
        }

        private Ojw.CSim2Real.ESignAns SC_AskDir(int nT, int nId, float fSw, int nSign,
                                                 bool bKnown, int nDone, int nTotal)
        {
            m_evSignAns.Reset();
            m_eSignAns = Ojw.CSim2Real.ESignAns.Abort;
            Invoke(new Action(delegate() { SC_BuildAskForm(nT, nId, fSw, nSign, bKnown, nDone, nTotal); }));
            m_evSignAns.WaitOne();
            Invoke(new Action(delegate()
            {
                if (m_frmSignAsk != null && !m_frmSignAsk.IsDisposed) m_frmSignAsk.Close();
                m_frmSignAsk = null;
            }));
            return m_eSignAns;
        }

        // ── 저장값 지우기 ──────────────────────────────────────
        private void btnSignClear_Click(object sender, EventArgs e)
        {
            string strDir = Path.Combine(GetUrdfOutputDir(), "sim2real");
            string strPath = Path.Combine(strDir, "joint_map.json");
            string strPart = Path.Combine(strDir, "joint_map.partial.json");
            var cur = Ojw.CSim2Real.LoadJointMap(strPath);
            if (cur.Count == 0 && !File.Exists(strPart))
            { MessageBox.Show("지울 저장값이 없습니다.\r\n" + strPath, "부호 초기화"); return; }

            if (MessageBox.Show(string.Format(
                    "저장된 관절 부호 {0}축을 지웁니다.\r\n\r\n" +
                    "다음 검사는 기준 없이(전부 +1 가정) 처음부터 진행됩니다.\r\n" +
                    "지우기 전 사본을 joint_map.bak.json 으로 남깁니다.\r\n\r\n지울까요?",
                    cur.Count), "부호 초기화", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            try
            {
                if (File.Exists(strPath))
                {
                    File.Copy(strPath, Path.Combine(strDir, "joint_map.bak.json"), true);
                    File.Delete(strPath);
                }
                if (File.Exists(strPart)) File.Delete(strPart);
                MessageBox.Show("지웠습니다. (사본: joint_map.bak.json)", "부호 초기화");
            }
            catch (Exception ex) { MessageBox.Show("지우지 못했습니다:\r\n" + ex.Message, "부호 초기화"); }
        }

        // ── 버튼 ───────────────────────────────────────────────
        private void btnSignCalib_Click(object sender, EventArgs e)
        {
            if (m_thrSignCalib != null && m_thrSignCalib.IsAlive)
            { MessageBox.Show("관절방향 검사가 이미 진행 중입니다."); return; }

            string strDir = Path.Combine(GetUrdfOutputDir(), "sim2real");
            Directory.CreateDirectory(strDir);
            var cfg = SignCalib_LoadConfig(strDir);
            var signs = Ojw.CSim2Real.LoadJointMap(Path.Combine(strDir, "joint_map.json"));

            if (MessageBox.Show(string.Format(
                    "① 관절 방향 검사 (매달림)\r\n\r\n" +
                    "COM{0} @ {1}bps, 관절 {2}개, 스윕 {3:F0}도\r\n\r\n" +
                    "관절마다 3D 와 실기를 움직여 같은 방향인지 확인해 주십시오.\r\n\r\n" +
                    "· ★로봇을 매달아 주십시오 (발이 땅에 닿으면 안 됩니다)\r\n" +
                    "· 현재 자세 기준으로 관절 하나씩만 움직입니다\r\n" +
                    "· 저장된 부호 {4}축을 불러왔습니다 — 적용해 돌려보고 확인만 하면 됩니다\r\n" +
                    "  (기준을 새로 잡으려면 옆의 [지움] 으로 먼저 지우십시오)\r\n" +
                    "· 결과: sim2real\\joint_map.json\r\n\r\n진행할까요?",
                    cfg.nPort, cfg.nBaud, cfg.anIDs.Length, cfg.fSweep, signs.Count),
                "관절 방향 검사", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            S2R_LogFileStart("① 관절 방향 검사");
            S2R_ShowPanel("① 관절 방향 검사");
            var before = new Dictionary<int, int>(signs);
            m_thrSignCalib = new Thread(delegate()
            {
                bool bAborted; int nConfirmed;
                bool bOk = Ojw.CSim2Real.SignCalib(cfg, strDir,
                    new Ojw.CSim2Real.DAskDir(SC_AskDir),
                    new Ojw.CSim2Real.DShow3d(SC_Show3d),
                    new Ojw.CSim2Real.DLog(S2R_Log),
                    signs, out bAborted, out nConfirmed);
                if (!bOk) return;

                string strPart = Path.Combine(strDir, "joint_map.partial.json");
                if (bAborted)
                {
                    try { if (File.Exists(strPart)) File.Delete(strPart); }
                    catch { }
                    S2R_Log("");
                    S2R_Log(string.Format("중단 — 저장하지 않았습니다. 확인한 {0}축의 결과는 버렸고, "
                                        + "기존 joint_map.json 은 그대로입니다.", nConfirmed));
                    S2R_Log("로봇은 시작 자세로 복귀했고 PWM 도 원복했습니다.");
                    return;
                }
                Ojw.CSim2Real.SaveJointMap(Path.Combine(strDir, "joint_map.json"),
                                           cfg.anIDs, cfg.nIdOffset, signs);
                try { if (File.Exists(strPart)) File.Delete(strPart); }
                catch { }

                var lstFlip = new List<int>(); var lstChanged = new List<int>();
                foreach (var kv in signs)
                {
                    if (kv.Value < 0) lstFlip.Add(kv.Key - cfg.nIdOffset);
                    if (before.ContainsKey(kv.Key) && before[kv.Key] != kv.Value)
                        lstChanged.Add(kv.Key - cfg.nIdOffset);
                }
                S2R_Log("");
                S2R_Log(string.Format("joint_map.json 저장 — 전체 {0}축 (이번에 확인 {1}축, 기존값 {2}축)",
                                      signs.Count, nConfirmed, before.Count));
                if (lstChanged.Count > 0)
                    S2R_Log("★기존값에서 바뀐 축: T" + string.Join(", T",
                        lstChanged.ConvertAll(x => x.ToString()).ToArray()));
                else if (before.Count > 0) S2R_Log("기존값과 모두 일치 — 검증 통과");
                if (lstFlip.Count > 0)
                {
                    S2R_Log(string.Format("EDH 의 Dir 이 실물과 반대인 축 {0}개: T{1}",
                        lstFlip.Count, string.Join(", T", lstFlip.ConvertAll(x => x.ToString()).ToArray())));
                    S2R_Log("런타임은 joint_map 의 sign 으로 보정됩니다. 모델 자체를 고치려면");
                    S2R_Log("EDH 해당 줄의 Dir 을 0<->1 로 바꾸십시오 (그러면 sign 이 전부 +1 이 됩니다).");
                }
                S2R_Log("");
                S2R_Log("다음: ② 관절리미트 (매달린 채로 이어서 하십시오)");
            });
            m_thrSignCalib.IsBackground = true;
            m_thrSignCalib.Start();
        }
    }
}
