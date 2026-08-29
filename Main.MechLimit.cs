// ================================================================
// ② 관절 리미트 측정 — UI 껍데기
//   측정 본체는 DLL 에 있다 (COjw_42_Sim2Real.cs):
//     Ojw.CSim2Real.TeachLimit()  — 손으로 가르치기 (mode=teach, 기본·권장)
//     Ojw.CSim2Real.MechLimit()   — 옛 구동 스윕   (mode=drive)
//
//   ★2026-08-28 측정 방식을 수동 학습으로 바꿨다.
//     구동 스윕은 "서보가 멈춘 각"과 "기구가 막힌 각"을 끝내 구별하지 못한다.
//     저PWM 이면 중력·마찰이 먼저 이겨 gravity_suspect 가 붙고(힙피치 +20.5도로 나왔으나 실제 +101.7도),
//     PWM 을 올리면 이번엔 기구를 때린다. R10 의 중력보상(아래 관절 접기)도 그 절반의 대책일 뿐이었다.
//     손으로 돌리면 사람이 벽을 직접 느끼므로 그 구별 자체가 필요 없다.
//     구동 스윕은 되돌릴 수 있게 남겨둔다 (mechlimit.txt 의 mode=drive).
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
        private bool m_bTeachMode = true;        // mechlimit.txt 의 mode= (기본 teach)
        private int m_nTeachSec = 40;            //              teachsec=

        private Ojw.CSim2Real.CMechCfg MechLimit_LoadConfig(string strDir)
        {
            var cfg = new Ojw.CSim2Real.CMechCfg();
            cfg.nPwm = 200;                      // 23% — 간섭해도 기구·서보 손상 최소
            m_bTeachMode = true; m_nTeachSec = 40;
            string strPath = Path.Combine(strDir, "mechlimit.txt");
            if (!File.Exists(strPath))
            {
                var sb = new StringBuilder();
                sb.AppendLine("// 관절 리미트 측정 설정 (매달림)");
                sb.AppendLine("// port/baud/ids/idoffset/pwm/step/stepms/errthr/loadthr/cap");
                sb.AppendLine("// fold,<시험관절>,<아래관절>,<비율>  — 중력 보상 (예: fold,6,15,-2.058)");
                sb.AppendLine("//");
                sb.AppendLine("// mode=teach : 토크를 풀고 사람이 손으로 돌린 범위를 한계로 잡는다 (권장)");
                sb.AppendLine("// mode=drive : 옛 구동 스윕 — 서보가 멈춘 각과 기구가 막힌 각을 구별 못 한다");
                sb.AppendLine("// teachsec   : 학습 시간(초). 그동안 손으로 돌리면 된다");
                sb.AppendLine("mode=teach");
                sb.AppendLine("teachsec=40");
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
                    if (s.StartsWith("mode=")) m_bTeachMode = !s.Substring(5).Trim().ToLower().StartsWith("drive");
                    else if (s.StartsWith("teachsec=")) m_nTeachSec = int.Parse(s.Substring(9));
                    else if (s.StartsWith("port=")) cfg.nPort = int.Parse(s.Substring(5));
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

            if (m_bTeachMode) MechLimit_RunTeach(cfg, strDir);
            else MechLimit_RunDrive(cfg, strDir);
        }

        //-------------------------------------------------------------
        // ②-a 수동 학습 — 토크를 풀고 사람이 손으로 (기본)
        //-------------------------------------------------------------
        private void MechLimit_RunTeach(Ojw.CSim2Real.CMechCfg cfg, string strDir)
        {
            string strIds = string.Join(",", Array.ConvertAll(cfg.anIDs,
                                delegate(int x) { return "T" + (x - cfg.nIdOffset); }));

            if (MessageBox.Show(string.Format(
                    "② 관절 리미트 — 손으로 가르치기\r\n\r\n" +
                    "COM{0}, 관절 {1}개, 학습 {2}초\r\n대상: {3}\r\n\r\n" +
                    "· ★로봇을 매달아 주십시오. 대상 축의 토크가 **전부** 풀립니다\r\n" +
                    "· '예'를 누르면 곧바로 힘이 빠집니다 — 로봇을 잡고 계십시오\r\n" +
                    "· {2}초 동안 각 관절을 양쪽 끝 **벽에 닿을 때까지** 돌리십시오\r\n" +
                    "  (덜 밀면 덜 민 그 자리가 한계로 기록됩니다)\r\n" +
                    "· 끝나면 현재 위치를 유지한 채 토크가 복구됩니다 (0도로 튕기지 않습니다)\r\n" +
                    "· 결과: sim2real\\mech_limits.json  (직전 값은 자동 백업)\r\n\r\n" +
                    "진행할까요?",
                    cfg.nPort, cfg.anIDs.Length, m_nTeachSec, strIds),
                "관절 리미트 — 손으로 가르치기", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            S2R_LogFileStart("② 관절 리미트 — 손으로 가르치기");
            S2R_ShowPanel("② 관절 리미트 — 손으로 가르치기");
            m_bMechStop = false;
            var lim = new Dictionary<int, Ojw.CSim2Real.CLimit>();
            int nSec = m_nTeachSec;
            m_thrMech = new Thread(delegate()
            {
                bool bOk = Ojw.CSim2Real.TeachLimit(cfg, strDir,
                    new Ojw.CSim2Real.DLog(S2R_Log),
                    new Ojw.CSim2Real.DIsStopped(delegate() { return m_bMechStop; }),
                    lim, nSec);
                if (!bOk) { S2R_Done("② 관절리미트 — 실패 (연결)"); return; }

                int nNew = 0;
                foreach (var kv in lim) if (kv.Value.q == "hand_taught") nNew++;
                S2R_Log("");
                S2R_Log("=========================================");
                S2R_Log(string.Format("   학습 완료 — 이번 {0}축 / 파일 전체 {1}축", nNew, lim.Count));
                S2R_Log("   이제 전원을 꺼도 됩니다");
                S2R_Log("=========================================");

                // 자기검증 — 좌우 대칭쌍 (① 관절방향 결과가 있어야 URDF 각으로 비교된다)
                var signs = Ojw.CSim2Real.LoadJointMap(Path.Combine(strDir, "joint_map.json"));
                if (signs.Count == 0)
                    S2R_Log("① 관절방향(joint_map.json)이 없어 대칭 검사를 건너뜁니다");
                else
                    Ojw.CSim2Real.MirrorCheck(cfg, lim, signs, 5.0f, new Ojw.CSim2Real.DLog(S2R_Log));

                S2R_Log("");
                S2R_Log("★기록된 범위가 '정말 벽까지' 간 값인지 눈으로 확인하십시오.");
                S2R_Log("  좌우를 나란히 덜 밀면 대칭 검사도 통과합니다 (2026-08-28 팔꿈치 — 좌우 차 1.3도로");
                S2R_Log("  통과했지만 둘 다 40도 모자랐고, 덮어쓰기 전 값이 정답이었습니다).");
                S2R_Log("  틀렸으면 그 축만 ids= 에 남기고 다시 배우면 됩니다 (나머지 축은 보존됩니다).");
                S2R_Log("다음: ③ sim2real 로 모델에 반영하십시오");
                S2R_Done(string.Format("② 관절리미트(손) — 완료 ({0}축)", nNew));
            });
            m_thrMech.IsBackground = true;
            m_thrMech.Start();
        }

        //-------------------------------------------------------------
        // ②-b 구동 스윕 — 옛 방식 (mechlimit.txt 에 mode=drive 일 때만)
        //-------------------------------------------------------------
        private void MechLimit_RunDrive(Ojw.CSim2Real.CMechCfg cfg, string strDir)
        {
            var sbFold = new StringBuilder();
            foreach (var kv in cfg.FoldJoint)
                sbFold.AppendFormat("T{0}(무릎 T{1} x{2:F2}) ", kv.Key - cfg.nIdOffset,
                                    kv.Value - cfg.nIdOffset, cfg.FoldRatio[kv.Key]);

            if (MessageBox.Show(string.Format(
                    "② 관절 리미트 — 구동 스윕 (옛 방식, mode=drive)\r\n\r\n" +
                    "COM{0}, 관절 {1}개, {2:F1}도/스텝, PWM {3}/885 ({4}%)\r\n\r\n" +
                    "· ★이 방식은 '서보가 멈춘 각'과 '기구가 막힌 각'을 구별하지 못합니다.\r\n" +
                    "  mechlimit.txt 의 mode=teach (손으로 가르치기)를 권합니다\r\n" +
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
