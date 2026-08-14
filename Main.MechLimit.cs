// ================================================================
// ② 관절 리미트 측정 — UI 껍데기
//   측정 본체는 DLL 에 있다: Ojw.CSim2Real.MechLimit()  (COjw_42_Sim2Real.cs)
//
//   ★R10 — 근위 관절은 중력을 빼고 재야 한다. 매달림 + 저PWM 으로 그냥 재면
//     "그 PWM 으로 팔다리를 들 수 있는 각"이 나온다 (힙피치 +20.5도로 나왔으나 실제 +101.7도).
//     그래서 아래 관절을 접어 끝 질량을 시험 관절 축 아래로 되돌린 뒤 측정한다.
// ================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using OpenJigWare;

namespace MakeUrdf
{
    public partial class Main
    {
        private Thread m_thrMech = null;
        private volatile bool m_bMechStop = false;

        private Ojw.CSim2Real.CMechCfg MechLimit_LoadConfig(string strDir)
        {
            var cfg = new Ojw.CSim2Real.CMechCfg();
            cfg.nPwm = 200;                      // 23% — 간섭해도 기구·서보 손상 최소
            string strPath = Path.Combine(strDir, "mechlimit.txt");
            if (!File.Exists(strPath))
            {
                var sb = new StringBuilder();
                sb.AppendLine("// 관절 리미트 측정 설정 (매달림)");
                sb.AppendLine("// port/baud/ids/idoffset/pwm/step/stepms/errthr/loadthr/cap");
                sb.AppendLine("// fold,<시험관절>,<아래관절>,<비율>  — 중력 보상 (예: fold,6,15,-2.058)");
                sb.AppendLine("port=" + cfg.nPort);
                sb.AppendLine("baud=" + cfg.nBaud);
                sb.AppendLine("ids=1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17");
                sb.AppendLine("idoffset=0");
                sb.AppendLine("pwm=200");
                sb.AppendLine("step=1.0");
                sb.AppendLine("stepms=80");
                sb.AppendLine("errthr=2.5");
                sb.AppendLine("loadthr=350");
                sb.AppendLine("cap=125");
                sb.AppendLine("fold,6,15,-2.058");
                sb.AppendLine("fold,8,16,2.058");
                File.WriteAllText(strPath, sb.ToString(), Encoding.UTF8);
                return cfg;
            }
            bool bFoldCleared = false;
            foreach (string strRaw in File.ReadAllLines(strPath, Encoding.UTF8))
            {
                string s = strRaw.Trim();
                if (s.Length == 0 || s.StartsWith("//")) continue;
                try
                {
                    if (s.StartsWith("port=")) cfg.nPort = int.Parse(s.Substring(5));
                    else if (s.StartsWith("baud=")) cfg.nBaud = int.Parse(s.Substring(5));
                    else if (s.StartsWith("idoffset=")) cfg.nIdOffset = int.Parse(s.Substring(9));
                    else if (s.StartsWith("pwm=")) cfg.nPwm = int.Parse(s.Substring(4));
                    else if (s.StartsWith("step=")) cfg.fStep = Ojw.CConvert.StrToFloat(s.Substring(5));
                    else if (s.StartsWith("stepms=")) cfg.nStepMs = int.Parse(s.Substring(7));
                    else if (s.StartsWith("errthr=")) cfg.fErrThr = Ojw.CConvert.StrToFloat(s.Substring(7));
                    else if (s.StartsWith("loadthr=")) cfg.nLoadThr = int.Parse(s.Substring(8));
                    else if (s.StartsWith("cap=")) cfg.fCap = Ojw.CConvert.StrToFloat(s.Substring(4));
                    else if (s.StartsWith("ids="))
                    {
                        string[] t = s.Substring(4).Split(',');
                        var l = new List<int>();
                        foreach (string x in t) if (x.Trim().Length > 0) l.Add(int.Parse(x.Trim()));
                        cfg.anIDs = l.ToArray();
                    }
                    else if (s.StartsWith("fold,"))
                    {
                        if (!bFoldCleared) { cfg.FoldJoint.Clear(); cfg.FoldRatio.Clear(); bFoldCleared = true; }
                        string[] t = s.Split(',');
                        int a = int.Parse(t[1].Trim());
                        cfg.FoldJoint[a] = int.Parse(t[2].Trim());
                        cfg.FoldRatio[a] = Ojw.CConvert.StrToFloat(t[3].Trim());
                    }
                }
                catch { }
            }
            return cfg;
        }

        private void btnMechLimit_Click(object sender, EventArgs e)
        {
            if (m_thrMech != null && m_thrMech.IsAlive)
            { MessageBox.Show("관절 리미트 측정이 이미 진행 중입니다."); return; }

            string strDir = Path.Combine(GetUrdfOutputDir(), "sim2real");
            Directory.CreateDirectory(strDir);
            var cfg = MechLimit_LoadConfig(strDir);

            var sbFold = new StringBuilder();
            foreach (var kv in cfg.FoldJoint)
                sbFold.AppendFormat("T{0}(무릎 T{1} x{2:F2}) ", kv.Key - cfg.nIdOffset,
                                    kv.Value - cfg.nIdOffset, cfg.FoldRatio[kv.Key]);

            if (MessageBox.Show(string.Format(
                    "② 관절 리미트 측정 (매달림)\r\n\r\n" +
                    "COM{0}, 관절 {1}개, {2:F1}도/스텝, PWM {3}/885 ({4}%)\r\n\r\n" +
                    "· ★로봇을 매달아 주십시오. 발이 땅에 닿으면 안 됩니다\r\n" +
                    "· 중력 보상: {5}\r\n" +
                    "  (안 하면 근위 관절이 실제의 1/5 로 좁게 나옵니다)\r\n" +
                    "· 좌우 대칭 관절은 동반 구동해 자기충돌을 걸러냅니다\r\n" +
                    "· 결과: sim2real\\mech_limits.json\r\n\r\n" +
                    "{1}축이면 10분 안팎 걸립니다. 진행할까요?",
                    cfg.nPort, cfg.anIDs.Length, cfg.fStep, cfg.nPwm, cfg.nPwm * 100 / 885,
                    sbFold.ToString()),
                "관절 리미트 측정", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            S2R_LogFileStart("② 관절 리미트 측정");
            S2R_ShowPanel("② 관절 리미트 측정");
            m_bMechStop = false;
            var lim = new Dictionary<int, Ojw.CSim2Real.CLimit>();
            m_thrMech = new Thread(delegate()
            {
                bool bOk = Ojw.CSim2Real.MechLimit(cfg, strDir,
                    new Ojw.CSim2Real.DLog(S2R_Log),
                    new Ojw.CSim2Real.DIsStopped(delegate() { return m_bMechStop; }),
                    lim);
                if (!bOk) return;
                S2R_Log("");
                S2R_Log("");
                S2R_Log("=========================================");
                S2R_Log(string.Format("   측정 완료 — {0}축", lim.Count));
                S2R_Log("   이제 전원을 꺼도 됩니다");
                S2R_Log("=========================================");
                S2R_Log(string.Format("sim2real\\mech_limits.json 저장 ({0}축)", lim.Count));
                int nSus = 0;
                foreach (var kv in lim) if (kv.Value.q == "gravity_suspect") nSus++;
                if (nSus > 0)
                    S2R_Log(string.Format("★{0}축이 gravity_suspect 입니다 — 중력 한계일 수 있어 "
                                        + "$jointrange 에 경고 주석과 함께 실립니다. 아래 관절로 보상해 재측정을 권합니다.", nSus));
                S2R_Log("다음: ③ sim2real 로 모델에 반영하십시오 (로봇은 내려놓아도 됩니다)");
                S2R_Done(string.Format("② 관절리미트 — 완료 ({0}축)", lim.Count));
            });
            m_thrMech.IsBackground = true;
            m_thrMech.Start();
        }
    }
}
