// ================================================================
// ④ 기립 사지탈 처짐 — UI 껍데기
//   측정 본체는 DLL: Ojw.CSim2Real.StandDroop()  (COjw_42_Sim2Real.cs)
//
//   ★①②는 매달림, ③은 모델 작업이라 로봇이 필요 없었다.
//     ④는 **처음으로 두 발로 세워두고 하는 측정**이다.
//
//   왜 필요한가:
//     지금까지의 강성 측정은 전부 매달림이었다. 기립은 두 다리와 지면이
//     닫힌 사슬이라 하중 경로가 아예 다르다. 매달림 법칙(0.01454 deg/load)이
//     기립에서도 통하는지 확인하지 않으면, 시뮬 kp 를 기립 자세에 쓰는 근거가 없다.
//     (2026-07-27: compliance.csv 에서 롤 축 3개는 0.86~1.01배로 확인됐다.
//      사지탈은 미확인 — 아라베스크·보행이 정작 요구하는 축들이다.)
//
//   ★안전: 전도경계는 실측 전방 16.5도 / 후방 26.5도다. 기본 최대 lean 은 8도로
//     전방 여유의 절반만 쓴다. 그래도 사람이 옆에서 잡아줄 준비를 해야 한다.
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
        private Thread m_thrStand = null;

        private Ojw.CSim2Real.CStandCfg StandDroop_LoadConfig(string strDir)
        {
            var cfg = new Ojw.CSim2Real.CStandCfg();
            string strPath = Path.Combine(strDir, "standdroop.txt");
            if (!File.Exists(strPath))
            {
                var sb = new StringBuilder();
                sb.AppendLine("// 기립 사지탈 처짐 측정 설정 (두 발로 세워둔 상태)");
                sb.AppendLine("// ★pwm 은 운전값(885)을 유지할 것. 낮추면 토크 포화가 처짐으로 둔갑한다(R10).");
                sb.AppendLine("// ★maxlean 은 전도경계(전방 16.5도)의 절반 이하로.");
                sb.AppendLine("port=" + cfg.nPort);
                sb.AppendLine("baud=" + cfg.nBaud);
                sb.AppendLine("ids=1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17");
                sb.AppendLine("idoffset=0");
                sb.AppendLine("imuid=" + cfg.nImuId);
                sb.AppendLine("pwm=885");
                sb.AppendLine("maxlean=8.0");
                sb.AppendLine("bowmax=4.0");
                sb.AppendLine("bowstep=1.0");
                sb.AppendLine("// squat=1 로 켜면 힙·무릎·발목에 하중 165 까지 실린다(가장 좋은 데이터).");
                sb.AppendLine("// 다만 시뮬에서 10도에 전도했다 — 사람이 반드시 붙잡은 상태에서만 켤 것.");
                sb.AppendLine("squat=0");
                sb.AppendLine("squatmax=4.0");
                sb.AppendLine("pitchabort=20");
                sb.AppendLine("step=2.0");
                sb.AppendLine("movems=900");
                sb.AppendLine("settlems=800");
                sb.AppendLine("loadabort=400");
                sb.AppendLine("gyroabort=25");
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
                    else if (s.StartsWith("imuid=")) cfg.nImuId = int.Parse(s.Substring(6));
                    else if (s.StartsWith("pwm=")) cfg.nPwm = int.Parse(s.Substring(4));
                    else if (s.StartsWith("maxlean=")) cfg.fMaxLean = Ojw.CConvert.StrToFloat(s.Substring(8));
                    else if (s.StartsWith("step=")) cfg.fStep = Ojw.CConvert.StrToFloat(s.Substring(5));
                    else if (s.StartsWith("movems=")) cfg.nMoveMs = int.Parse(s.Substring(7));
                    else if (s.StartsWith("settlems=")) cfg.nSettleMs = int.Parse(s.Substring(9));
                    else if (s.StartsWith("loadabort=")) cfg.nLoadAbort = int.Parse(s.Substring(10));
                    else if (s.StartsWith("gyroabort=")) cfg.fGyroAbort = Ojw.CConvert.StrToFloat(s.Substring(10));
                    else if (s.StartsWith("bowmax=")) cfg.fBowMax = Ojw.CConvert.StrToFloat(s.Substring(7));
                    else if (s.StartsWith("bowstep=")) cfg.fBowStep = Ojw.CConvert.StrToFloat(s.Substring(8));
                    else if (s.StartsWith("squatmax=")) cfg.fSquatMax = Ojw.CConvert.StrToFloat(s.Substring(9));
                    else if (s.StartsWith("pitchabort=")) cfg.fPitchAbort = Ojw.CConvert.StrToFloat(s.Substring(11));
                    else if (s.StartsWith("squat=")) cfg.bSquat = (s.Substring(6).Trim().StartsWith("1"));
                    else if (s.StartsWith("ids="))
                    {
                        string[] t = s.Substring(4).Split(',');
                        var l = new List<int>();
                        foreach (string x in t) if (x.Trim().Length > 0) l.Add(int.Parse(x.Trim()));
                        cfg.anIDs = l.ToArray();
                    }
                }
                catch { }
            }
            return cfg;
        }

        private void btnStandDroop_Click(object sender, EventArgs e)
        {
            if (m_thrStand != null && m_thrStand.IsAlive)
            { MessageBox.Show("기립 처짐 측정이 이미 진행 중입니다."); return; }

            string strDir = Path.Combine(GetUrdfOutputDir(), "sim2real");
            Directory.CreateDirectory(strDir);
            var cfg = StandDroop_LoadConfig(strDir);

            var mech = Ojw.CSim2Real.LoadMechLimits(Path.Combine(strDir, "mech_limits.json"));
            if (mech.Count == 0)
            {
                MessageBox.Show("mech_limits.json 이 없습니다.\r\n\r\n" +
                    "기구한계표가 없으면 명령이 스톱을 밀어 과부하가 납니다(R9).\r\n" +
                    "먼저 ② 관절리미트를 하십시오.", "기립 처짐");
                return;
            }
            int nToolPort, nToolBaud;                                   // 포트·속도 = 로봇팔 세팅 탭 칸 (standdroop 설정 파일의 port/baud 대신 — 탭이 연결 중이면 해제할지 묻는다)
            if (!ArmMotor_ToolPort("④ 기립 처짐", true, out nToolPort, out nToolBaud)) return;
            cfg.nPort = nToolPort; cfg.nBaud = nToolBaud;

            int nSteps = (int)(cfg.fMaxLean / cfg.fStep) * 2;
            if (MessageBox.Show(string.Format(
                    "④ 기립 사지탈 처짐 측정\r\n\r\n" +
                    "★로봇을 **두 발로 지면에 세워** 두십시오. (①②와 반대입니다)\r\n" +
                    "★넘어질 수 있습니다. 옆에서 잡아줄 준비를 하십시오.\r\n\r\n" +
                    "COM{0}, 기구한계 {1}축 적용, PWM {2}/885\r\n" +
                    "패턴 2개를 연달아 잽니다 — 한 패턴은 원하는 축을 다 싣지 못합니다:\r\n" +
                    "  · 평행이동 ±{3:F0}도 ({4:F0}도씩) — 골반만 전후. **발목**에 실립니다\r\n" +
                    "  · 숙임     ±{8:F0}도 (1도씩)   — 몸통이 기움. **힙피치**에 실립니다\r\n" +
                    "    (숙임은 전도 여유가 작아 얕게만 갑니다 — 시뮬 예측 전방 11 / 후방 8도)\r\n" +
                    "  실측 전도경계는 전방 16.5 / 후방 26.5도. 그 절반 이하만 씁니다\r\n\r\n" +
                    "· 먼저 좌우대칭 후보 2개로 어느 쪽이 평행이동인지 판별합니다\r\n" +
                    "  이때 몸통이 전혀 안 움직이면 '서 있지 않다'로 보고 중단합니다\r\n" +
                    "· PWM 은 운전값 그대로입니다 — 낮추면 처짐이 아니라 토크 포화를 재게 됩니다\r\n" +
                    "· 하중 {6} 초과 또는 자이로 {7:F0} 초과면 즉시 후퇴합니다\r\n" +
                    "· 결과: sim2real\\stand_droop.csv\r\n\r\n" +
                    "3분 안팎 걸립니다. 진행할까요?",
                    cfg.nPort, mech.Count, cfg.nPwm, cfg.fMaxLean, cfg.fStep, nSteps,
                    cfg.nLoadAbort, cfg.fGyroAbort, cfg.fBowMax),
                "기립 처짐 측정", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            S2R_LogFileStart("④ 기립 사지탈 처짐");
            S2R_ShowPanel("④ 기립 사지탈 처짐");
            m_bMechStop = false;                    // 중단 버튼을 ②와 공유한다
            m_thrStand = new Thread(delegate()
            {
                string strRep;
                bool bOk = Ojw.CSim2Real.StandDroop(cfg, strDir,
                    new Ojw.CSim2Real.DLog(S2R_Log),
                    new Ojw.CSim2Real.DIsStopped(delegate() { return m_bMechStop; }),
                    out strRep);
                S2R_Log("");
                S2R_Log("=========================================");
                S2R_Log(bOk ? "   측정 완료 — 이제 전원을 꺼도 됩니다"
                            : "   중단됨 — 위 로그에서 사유를 확인하십시오");
                S2R_Log("=========================================");
                S2R_Done(bOk ? "④ 기립처짐 — 완료" : "④ 기립처짐 — 중단");
            });
            m_thrStand.IsBackground = true;
            m_thrStand.Start();
        }
    }
}
