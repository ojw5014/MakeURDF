// ================================================================
// ③ sim2real — 측정값을 모델에 반영한다 (UI 껍데기)
//   변환 본체는 DLL: Ojw.CSim2Real.ApplyJointRange()  (COjw_42_Sim2Real.cs)
//
// 왜 이 순서인가 (① 방향 → ② 리미트 → ③ 반영):
//   리미트는 모터각으로 나온다. URDF 로 옮기려면 부호가 있어야 하고,
//   sign < 0 인 축은 lo/hi 가 **뒤바뀐다**. 2026-07-27 에 이 순서를 안 지켜
//   5축(T2/T4/T5/T7/T17)이 부호가 뒤집힌 채 URDF 에 실렸다.
//
// ── 버튼은 Main.Designer.cs 에 있다 ─────────────────────────
//   btnSignCalib(①) / btnMechLimit(②) / btnSim2Real(③) / btnSignClear
//   ⚠Visual Studio 폼 디자이너가 열려 있으면 Designer 를 자기 사본으로 다시 써서
//     밖에서 고친 내용이 사라진다 (2026-07-27 두 버튼이 날아갔다).
//     빌드는 그래도 통과한다 — 핸들러 메서드만 있고 버튼 필드를 참조하는 코드가 없어
//     컴파일 에러가 안 난다. "빌드 성공"이 "동작한다"를 보장하지 않는다.
//     외부에서 Designer 를 고쳤으면 VS 에서 **다시 불러오기**를 하십시오.
// ================================================================
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenJigWare;

namespace MakeUrdf
{
    public partial class Main
    {
        // ── 공용 진행 패널 (①②③ 이 함께 쓴다) ────────────────
        //   ★모달로 만들지 말 것. 진행 중에 3D 를 돌려보고 로그를 읽을 수 있어야 한다.
        private Form m_frmS2R = null;
        private TextBox m_txtS2RLog = null;
        private Button m_btnS2RStop = null;
        // ★완료 후에도 버튼이 "중단"으로 남아 있으면 끝났는지 알 수 없다 (2026-07-31 사용자 지적).
        //   완료 시 버튼을 "닫기"로 바꾸고, 그 상태의 클릭은 중단 요청이 아니라 패널 닫기가 된다.
        private volatile bool m_bS2RDone = false;

        private void S2R_ShowPanel(string strTitle)
        {
            if (InvokeRequired) { Invoke(new Action(delegate() { S2R_ShowPanel(strTitle); })); return; }
            m_bS2RDone = false;
            if (m_frmS2R != null && !m_frmS2R.IsDisposed)
            {
                m_frmS2R.Text = strTitle; m_txtS2RLog.Clear();
                if (m_btnS2RStop != null && !m_btnS2RStop.IsDisposed)
                { m_btnS2RStop.Text = "중단"; m_btnS2RStop.ForeColor = Color.Tomato; }
                m_frmS2R.BringToFront(); return;
            }

            var f = new Form();
            f.Text = strTitle;
            f.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            f.ShowInTaskbar = false; f.TopMost = true;
            f.StartPosition = FormStartPosition.Manual;
            f.ClientSize = new Size(520, 340);
            f.BackColor = Color.FromArgb(45, 45, 48);
            f.Location = new Point(Math.Max(0, Bounds.Right - 560), Bounds.Top + 80);

            var t = new TextBox();
            t.Multiline = true; t.ReadOnly = true; t.ScrollBars = ScrollBars.Vertical;
            t.BackColor = Color.FromArgb(30, 30, 30); t.ForeColor = Color.Gainsboro;
            t.Font = new Font("Consolas", 9F);
            t.SetBounds(8, 8, 504, 288);
            t.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            f.Controls.Add(t);

            var b = new Button();
            b.Text = "중단"; b.SetBounds(412, 302, 100, 30);
            b.FlatStyle = FlatStyle.Flat; b.BackColor = Color.FromArgb(60, 60, 60);
            b.ForeColor = Color.Tomato;
            b.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            b.Click += new EventHandler(delegate(object s, EventArgs e)
            {
                if (m_bS2RDone) { m_frmS2R.Close(); return; }      // 완료 후엔 닫기
                m_bMechStop = true; S2R_Log("중단 요청 — 안전하게 마무리하는 중...");
            });
            f.Controls.Add(b);

            m_frmS2R = f; m_txtS2RLog = t; m_btnS2RStop = b;
            f.Show(this);
        }

        // ★로그는 파일에도 남긴다.
        //   패널을 닫으면 무슨 일이 있었는지 알 길이 없다. 측정이 실패했을 때
        //   원인을 추측하게 되는 것이 가장 큰 시간 낭비였다 (2026-07-27).
        private string m_strS2RLogPath = null;

        private void S2R_LogFileStart(string strTitle)
        {
            try
            {
                string strDir = Path.Combine(GetUrdfOutputDir(), "sim2real");
                Directory.CreateDirectory(strDir);
                m_strS2RLogPath = Path.Combine(strDir, "sim2real_log.txt");
                File.AppendAllText(m_strS2RLogPath,
                    "\r\n===== " + strTitle + "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " =====\r\n",
                    new UTF8Encoding(false));
            }
            catch { m_strS2RLogPath = null; }
        }

        private void S2R_Log(string strMsg)
        {
            // ★UI 스레드로 넘기는 것이 **먼저**다. 파일 쓰기를 앞에 두면 재귀 호출에서 또 써서
            //   측정 로그가 전부 두 줄씩 남는다 (2026-08-28 무릎 재학습 로그를 읽다 발견 —
            //   그동안의 sim2real_log.txt 가 전부 그랬다).
            if (m_txtS2RLog != null && !m_txtS2RLog.IsDisposed && m_txtS2RLog.InvokeRequired)
            { m_txtS2RLog.Invoke(new Action(delegate() { S2R_Log(strMsg); })); return; }
            if (m_strS2RLogPath != null)
            {
                try { File.AppendAllText(m_strS2RLogPath, strMsg + "\r\n", new UTF8Encoding(false)); }
                catch { }
            }
            if (m_txtS2RLog == null || m_txtS2RLog.IsDisposed) return;
            m_txtS2RLog.AppendText(strMsg + "\r\n");
        }

        /// <summary>끝났음을 창 제목 + 버튼("중단"→"닫기")으로 알린다.
        /// 진행 중인지 끝났는지 알 수 없으면 사용자가 도중에 전원을 끈다(실제로 그랬다).
        /// 버튼이 "중단"으로 남아 있는 것도 같은 혼란을 만든다(2026-07-31 지적).</summary>
        private void S2R_Done(string strTitle)
        {
            if (m_frmS2R == null || m_frmS2R.IsDisposed) return;
            if (m_frmS2R.InvokeRequired)
            { m_frmS2R.Invoke(new Action(delegate() { S2R_Done(strTitle); })); return; }
            m_bS2RDone = true;
            m_frmS2R.Text = strTitle;
            if (m_btnS2RStop != null && !m_btnS2RStop.IsDisposed)
            { m_btnS2RStop.Text = "닫기"; m_btnS2RStop.ForeColor = Color.LightGreen; }
        }

        // ── 버튼 ───────────────────────────────────────────────
        private void btnSim2Real_Click(object sender, EventArgs e)
        {
            string strDir = Path.Combine(GetUrdfOutputDir(), "sim2real");
            Directory.CreateDirectory(strDir);

            var signs = Ojw.CSim2Real.LoadJointMap(Path.Combine(strDir, "joint_map.json"));
            var mech = Ojw.CSim2Real.LoadMechLimits(Path.Combine(strDir, "mech_limits.json"));

            var sb = new StringBuilder();
            sb.AppendLine("③ sim2real — 측정값을 모델에 반영합니다");
            sb.AppendLine();
            sb.AppendLine(string.Format("  ① 관절방향   : {0}",
                signs.Count > 0 ? signs.Count + "축 있음" : "★없음 — 먼저 ①을 하십시오"));
            sb.AppendLine(string.Format("  ② 관절리미트 : {0}",
                mech.Count > 0 ? mech.Count + "축 있음" : "★없음 — 먼저 ②를 하십시오"));
            sb.AppendLine();
            sb.AppendLine("진행하면:");
            sb.AppendLine("  · $jointrange 를 DH 텍스트에 자동 기입 (부호 적용)");
            sb.AppendLine("  · URDF / MJCF / USD 내보내기");
            sb.AppendLine();
            sb.AppendLine("진행할까요?");

            if (signs.Count == 0 || mech.Count == 0)
            { MessageBox.Show(sb.ToString(), "sim2real"); return; }
            if (MessageBox.Show(sb.ToString(), "sim2real", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            S2R_LogFileStart("③ sim2real — 모델 반영");
            S2R_ShowPanel("③ sim2real — 모델 반영");
            S2R_Log("── 모델 반영 ──");

            // ★내보내기는 텍스트박스가 아니라 **빌더**(m_Builder.GenerateFullDh())를 읽는다.
            //   텍스트박스만 고치면 화면만 바뀌고 URDF 에는 옛 값이 그대로 나간다
            //   (2026-07-27 실제로 그랬다 — 로그는 "9축 기입"인데 URDF 는 ex6 값이었다).
            //   그래서 빌더에서 읽고, 고친 뒤 빌더에 되돌려 넣는다.
            string strEdh = m_Builder.GenerateFullDh();
            string strRep;
            int n = Ojw.CSim2Real.ApplyJointRange(strDir, ref strEdh, 0, out strRep);
            S2R_Log(strRep);
            if (n == 0) { S2R_Done("③ sim2real — 실패 (반영할 값 없음)"); return; }
            m_Builder.ImportFromText(strEdh);
            if (txtDhParams != null) txtDhParams.Text = strEdh;
            S2R_Log("빌더에 반영 완료 — 내보내기가 이 값을 씁니다");

            S2R_Log("");
            S2R_Log("── 내보내기 ──");
            try
            {
                if (ExportUrdfFiles()) S2R_Log("URDF/MJCF 내보내기 완료 -> " + GetUrdfOutputDir());
                else { S2R_Log("내보내기 실패"); S2R_Done("③ sim2real — 실패 (내보내기)"); return; }
            }
            catch (Exception ex) { S2R_Log("내보내기 예외: " + ex.Message); S2R_Done("③ sim2real — 실패 (예외)"); return; }

            S2R_Log("");
            S2R_Log("── 접촉 보정 ($footcontact) ──");
            S2R_Log("발 STL 메시를 그대로 충돌로 쓰면 지지다각형이 실물보다 넓어져");
            S2R_Log("시뮬이 훨씬 안 넘어진다. 실측 전도각으로 역산한 유효 접지면을");
            S2R_Log("$footcontact 지시자로 EDH 에 넣으십시오 (대상 링크 DH 라인 바로 뒤).");
            S2R_Log("");
            S2R_Log("  $footcontact, 반길이, 반폭, 반두께, x, y, z [, roll, pitch, yaw [, color]]");
            S2R_Log("    치수·오프셋 mm / 회전 deg / 반치수로 준다 (반폭 13 = 실폭 26)");
            S2R_Log("");
            S2R_Log("실측 참고 (PR2 MAX-E3 #2, 2026-07-27):");
            S2R_Log("  실물 전도경계  좌우 19.0 / 앞 16.5 / 뒤 26.5 도");
            S2R_Log("  보정 전 시뮬   좌우 29.0 / 앞뒤 29.0   <- 세 방향 다 낙관적이었다");
            S2R_Log("  보정 후 시뮬   좌우 18.5 / 앞 18.0 / 뒤 27.0");
            S2R_Log("  유효 접지면    80 x 26 mm (메시 100x60 의 0.80 x 0.43), 중심 8mm 뒤");
            S2R_Log("  → $footcontact, 40, 13, 2.5, -8, 0, <발바닥까지의 z>");

            S2R_Log("");
            S2R_Log("=========================================");
            S2R_Log("   ③ sim2real 완료");
            S2R_Log("   URDF/MJCF 는 내보냈습니다. Isaac 용 USD 가 필요하면");
            S2R_Log("   [USD Export] 버튼을 이어서 누르십시오.");
            S2R_Log("=========================================");
            S2R_Done("③ sim2real — 완료");
        }
    }
}
