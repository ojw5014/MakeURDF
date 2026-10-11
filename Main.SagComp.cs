// ================================================================
// ⑨ 처짐(중력)·주행 보상 — UI 껍데기 (2026-09-28)
//   측정·학습·적용 본체는 전부 DLL: Ojw.C3d.SagComp_Survey / SagComp_SurveyDynamic /
//   SagComp_Verify / SagComp_VerifyDynamic, Ojw.C3d.CSagComp (Fit·FitDynamic·Predict·Compensate·Save·Load)
//   — COjw_12_3D_SagComp.cs. 실물 연결은 DLL 의 CScene_t (교육 실행기와 같은 스트리밍 경로).
//
//   여기서는 ① 설정 파일 읽기 ② 확인 대화 ③ 스레드 띄워 DLL 호출 ④ 결과 파일 저장 ⑤ 자동 로드만 한다.
//
//   사용자 계약: 학습 파일(<urdf폴더>\sagcomp_model.txt)이 있으면 예제 로드 때 자동으로 읽어
//   PlayXyz / PlayXyzPath 가 조용히 보상한다. 없으면 예전과 완전히 같다.
//
//   바닥 보정표 (2026-09-29): 같은 파일의 floorr= / floordz= / flooron=1 세 줄이 있으면 실물 출력에서 끝 높이를 반경별로
//   더 보정한다(14번 손목 브래킷 처짐 — 엔코더 밖). flooron=0 이거나 세 줄이 없으면 관절 모델만으로 예전과 같다.
//   로그 "[처짐보상]" 줄 끝에 "바닥 보정표 n점 …" 이 보이면 켜진 것. 만들기·끄기 자세한 법: docs\바닥보정표_사용법.md.
//   "처짐 학습" 을 다시 해도 FK 기준 보정표는 새 모델로 이어진다(DLL SagComp_Survey). 엔코더 검증 동안만 자동으로 꺼진다.
//   바닥 GP (2026-09-29, 지금 활성): 같은 파일에 gp 로 시작하는 줄이 있으면 반경 표 대신 학습 모델(가우시안 프로세스)을 쓴다 —
//   잰 자세 근처(관절 3° 안)에서만 보정하고, 멀면·두 상태 반경 띠(gpbadr) 안이면 0. flooron=0 이면 둘 다 끈다. 문서 같은 곳 5절.
//   로봇 정의 (10-04): sagcomp.txt 에 관절 줄(func·posids·wrist)이 없으면 로봇 정의(sagarm.txt — 로봇팔 세팅 탭 '로봇 정의' 페이지)의 관절을 쓰고,
//   관절이 정의와 같으면 수직 관절 합(downsum)·이름도 정의 값으로. [예] (측정+학습+검증) 전에 DLL SagDef_CheckSurvey 로 ⑨ 가 쓸 관절을 3D 와 대조 —
//   맞지 않으면 시작하지 않는다 ([아니오] 다시 읽기·취소는 확인하지 않는다). 관절이 정의와 달라도 3D 와 맞으면 된다 (OMX 가 아닌 로봇).
// ================================================================
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenJigWare;

namespace MakeUrdf
{
    public partial class Main
    {
        private const string SAGCOMP_MODEL = "sagcomp_model.txt";
        private const string SAGCOMP_CFG = "sagcomp.txt";

        /// <summary>설정 파일 읽기 (없으면 기본값으로 만들어 둔다 — 사용자가 고쳐 쓰는 용도). bIdsInFile = 파일에 관절 줄(func·posids·wrist)이 하나라도 있다
        /// (없으면 부른 쪽이 로봇 정의의 관절로 채운다 — SagComp_DefFill). 새로 만드는 기본 파일에는 관절 줄을 쓰지 않는다 (bIdsInFile = false —
        /// 로봇 정의(sagarm.txt)의 관절을 쓴다. OMX 가 아닌 로봇도 첫 ⑨ 부터 정의대로. 줄을 적어 넣으면 그 값)</summary>
        private Ojw.C3d.CSagSurveyCfg SagComp_LoadCfg(string strDir, out int nPort, out int nBaud, out bool bIdsInFile)
        {
            var cfg = new Ojw.C3d.CSagSurveyCfg();
            nPort = 8; nBaud = 1000000;
            bIdsInFile = false;
            string strPath = Path.Combine(strDir, SAGCOMP_CFG);
            var ci = CultureInfo.InvariantCulture;
            if (!File.Exists(strPath))
            {
                var sb = new StringBuilder();
                sb.AppendLine("// 처짐(중력)·주행 보상 측정 설정. 단위 mm / deg / ms");
                sb.AppendLine("// 관절(func·posids·wrist)은 로봇팔 세팅 탭 '로봇 정의' 페이지(sagarm.txt)의 것을 씀 — 다른 관절로 재려면 func=·posids=·wrist= 줄을 적어 넣는다");
                sb.AppendLine("port=" + nPort);
                sb.AppendLine("baud=" + nBaud);
                sb.AppendLine("pitch=90          // 툴 자세 Ry (90 = 수직 아래)");
                sb.AppendLine("yaw=0             // 베이스 방향들 (쉼표로 여러 개 가능)");
                sb.AppendLine("r=120,280,5       // 뻗는 거리 시작,끝,분할수");
                sb.AppendLine("z=60              // 측정 높이들 (쉼표로 여러 개 = 2차 모델)");
                sb.AppendLine("both=1            // 1 = 멀리 갔다가 되돌아오며 한 번 더 (백래시 지표)");
                sb.AppendLine("movems=2500");
                sb.AppendLine("settlems=800");
                sb.AppendLine("maxdelta=40       // 보상량 안전 상한");
                sb.AppendLine("speeds=4000,2000,1000   // 주행 측정: 같은 직선을 이 시간(ms)들로 왕복 (비우면 주행 측정 생략)");
                sb.AppendLine("sampleticks=4     // 주행 중 엔코더 표본 간격 (틱)");
                File.WriteAllText(strPath, sb.ToString(), Encoding.UTF8);
                return cfg;                                       // 관절 줄 없음 (bIdsInFile = false) — 부른 쪽이 로봇 정의의 관절로 채운다
            }
            foreach (string raw in File.ReadAllLines(strPath))
            {
                string line = raw; int c = line.IndexOf("//"); if (c >= 0) line = line.Substring(0, c);
                line = line.Trim(); if (line.Length == 0) continue;
                int eq = line.IndexOf('='); if (eq < 0) continue;
                string k = line.Substring(0, eq).Trim().ToLowerInvariant(), v = line.Substring(eq + 1).Trim();
                if (k == "func" || k == "posids" || k == "wrist") bIdsInFile = true;   // 관절 줄이 있다 (값을 읽지 못해도 — 그 줄은 기본값)
                try
                {
                    switch (k)
                    {
                        case "port": nPort = int.Parse(v, ci); break;
                        case "baud": nBaud = int.Parse(v, ci); break;
                        case "func": cfg.FuncNumber = int.Parse(v, ci); break;
                        case "posids": cfg.PosIDs = Array.ConvertAll(v.Split(','), s => int.Parse(s.Trim(), ci)); break;
                        case "wrist": cfg.WristID = int.Parse(v, ci); break;
                        case "pitch": cfg.RyDeg = float.Parse(v, ci); break;
                        case "yaw": cfg.YawDegs = Array.ConvertAll(v.Split(','), s => float.Parse(s.Trim(), ci)); break;
                        case "r":
                            {
                                var a = Array.ConvertAll(v.Split(','), s => float.Parse(s.Trim(), ci));
                                if (a.Length >= 2) { cfg.RStart = a[0]; cfg.REnd = a[1]; }
                                if (a.Length >= 3) cfg.RSteps = Math.Max(1, (int)a[2]);
                                break;
                            }
                        case "z": cfg.ZLevels = Array.ConvertAll(v.Split(','), s => float.Parse(s.Trim(), ci)); break;
                        case "both": cfg.BothDirections = v != "0"; break;
                        case "movems": cfg.MoveMs = int.Parse(v, ci); break;
                        case "settlems": cfg.SettleMs = int.Parse(v, ci); break;
                        case "maxdelta": cfg.MaxAbsDelta = float.Parse(v, ci); break;
                        case "speeds": cfg.PathMs = v.Length == 0 ? new int[0] : Array.ConvertAll(v.Split(','), s => int.Parse(s.Trim(), ci)); break;
                        case "sampleticks": cfg.SampleEveryTicks = Math.Max(1, int.Parse(v, ci)); break;
                    }
                }
                catch { }
            }
            return cfg;
        }

        /// <summary>예제 로드·시작 시 — 학습 파일이 있으면 조용히 적용, 없으면 해제. 사용자는 아무것도 안 해도 된다.</summary>
        private void SagComp_AutoLoad()
        {
            if (m_C3d == null) return;
            string strPath = Path.Combine(GetUrdfOutputDir(), SAGCOMP_MODEL);
            if (File.Exists(strPath) && m_C3d.SagComp_Load(strPath))
            {
                Ojw.printf(txtLogHistory, "[처짐보상] {0}\r\n", m_C3d.SagComp_Get().Summary());
                // 모델을 만든 기구학(kinsig)이 지금 3D 와 다르면 한 줄 (막지 않는다 — 다시 학습 권장). 계획 기구학을 준비한 뒤에만 (시작 때는 3D 모델 전)
                string strKin = m_bArmPrepared ? m_C3d.SagDef_ModelKinWarn(m_C3d.SagComp_Get()) : "";
                if (strKin.Length > 0) Ojw.printf(txtLogHistory, "[처짐보상] {0}\r\n", strKin);
            }
            else
            {
                m_C3d.SagComp_Clear();
                m_C3d.SagTwin_SetModel(null);   // 다른 로봇 — 옛 모델로 쌍둥이를 그리지 않게
            }
            SagComp_UiSync();
        }

        // "처짐 보정 켬" 체크박스 — 이 앱의 처짐 보정을 켜고 끈다: 3D 모션(PlayXyz)과, 연결돼 있으면 실물로 보내는 관절 명령(SendPose) 둘 다
        //   (모델은 남긴다 → 꺼도 쌍둥이는 처진 자세를 보여 준다)
        private bool m_bSagCompUiSync = false;
        private void SagComp_UiSync()
        {
            if (chkSagCompOn == null || m_C3d == null) return;
            if (chkSagCompOn.InvokeRequired) { chkSagCompOn.BeginInvoke(new Action(SagComp_UiSync)); return; }   // ⑨ 작업 스레드에서 불릴 수 있다
            m_bSagCompUiSync = true;
            var m = m_C3d.SagComp_Get();
            // 로봇팔 작업·조그 중에는 끈다 — 작업은 보정 모델을 바꿔 가며 쓰고(지금 걸린 모델이 작업의 것이다), 켬·끔은 지금 걸린 모델의 플래그다.
            // 로봇팔이 토크를 붙잡고 있는 것만으로는 막지 않는다 (10-03 다시 봄): 보정 전환이 출력 틱 기준이라 바꾼 뒤 다음 틱은 걸려 있던 보정 그대로
            // 이어지고(토크 ON 에서 끔·모델 바꿈 → 첫 틱 튐 0), 새 상태는 다음 동작의 출력 틱 1초에 걸쳐 들어오고 빠진다.
            // 그동안 조그는 Main.ArmMotor.cs 가 출력 단 상태를 보고 막는다 (머리 주석 '처짐 보정과 조그')
            chkSagCompOn.Enabled = (m != null) && !m_bArmLocked;
            chkSagCompOn.Checked = (m != null && m.Enabled);
            m_bSagCompUiSync = false;
        }
        private void chkSagCompOn_CheckedChanged(object sender, EventArgs e)
        {
            if (m_bSagCompUiSync || m_C3d == null) return;
            if (m_C3d.SagComp_Get() == null || m_bArmLocked) { SagComp_UiSync(); return; }   // 로봇팔 작업·조그 중 — 되돌린다
            m_C3d.SagComp_Enable(chkSagCompOn.Checked);       // 1초에 걸쳐 들어오고 빠진다 (SagComp_Switch 크로스페이드)
            Ojw.printf(txtLogHistory, "[처짐보상] {0}\r\n", chkSagCompOn.Checked ? "켬" : "끔 (모델은 유지 — 처짐 쌍둥이는 처진 자세를 보여 준다)");
            if (ArmMotor_TorqueHeld())
                Ojw.printf(txtLogHistory, "[처짐보상] 로봇팔 토크 ON — 지금은 움직이지 않음. 다음 동작의 출력 틱 1초에 걸쳐 {0} (튀지 않음). 그동안 조그는 막힘\r\n",
                           chkSagCompOn.Checked ? "들어온다" : "빠진다");
            ArmMotor_UpdateUi();                              // 로봇팔 세팅 탭 — 조그 막힘 표시
        }

        // ── 처짐 쌍둥이 (10-02) — 실물이 있을 자세를 3D 에 반투명 팔로, 시뮬(PyBullet·MuJoCo·Isaac)로도 ──
        //   0 끔 / 1 3D 만 / 2 3D + 시뮬 겹쳐 보기(뷰어가 반투명 팔을 따로 그림) / 3 3D + 시뮬 실물 자세(시뮬 팔 자체를 실물 자세로)
        //   계산은 전부 DLL(C3d.SagTwin_*, CSimBridge.BuildC3dPayload). 보정을 켜면 쌍둥이 ≈ 목표, 끄면 실물처럼 처진다.
        //   시뮬로 보내는 것은 기존 "3D → Sim" / Auto-sync 전송에 실린다.
        private void cmbSagTwin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_C3d == null) return;
            int k = cmbSagTwin.SelectedIndex;
            m_C3d.SagTwin_Show = k >= 1;
            m_C3d.SagTwin_Compute = k >= 2;
            if (k >= 1 && m_C3d.SagTwin_GetModel() == null)
                Ojw.printf(txtLogHistory, "[처짐 쌍둥이] 처짐 모델이 없습니다 — {0} 이 있어야 그릴 수 있습니다\r\n", SAGCOMP_MODEL);
            ArmSetup_TwinUiSync();   // 로봇팔 세팅 탭 ⑦ 표시와 맞춤
        }
        // 시뮬 전송 방식: −1 쌍둥이 없음, 0 겹쳐 보기, 1 실물 자세
        private int SagTwin_SimMode()
        {
            int k = (cmbSagTwin != null) ? cmbSagTwin.SelectedIndex : 0;
            return k == 2 ? 0 : (k == 3 ? 1 : -1);
        }

        // ── ⑨ 처짐 보상 측정 → 학습 → 검증 (정지, 이어서 주행) ────────────────
        //   로봇팔 세팅 탭에서 연결돼 있으면 그 연결(CArmConn_t)을 빌린다 — 포트를 따로 열거나 닫지 않고, 모션 싱크도 연결이 가진 것을 쓴다.
        //   잠금("처짐보상") → 토크(꺼져 있었으면 켬) → 3D·실물 맞춤 확인(실패면 시작 안 함) → 측정 → 끝나면 토크를 시작 상태로, 잠금 놓기.
        //   시작 상태는 서보에서도 읽는다 (주소 64 — 로봇팔 세팅 탭의 작업과 같은 규칙): 연결은 OFF 로 아는데 서보가 토크를 붙잡고 있었으면 끝에서 끄지 않는다.
        //   연결이 없으면 예전대로 sagcomp.txt 의 포트를 직접 연다 — 그 길은 모터 세팅 없이(방향 정·영점 2048, 리밋 자르기 없음) 보내므로, 로봇팔 세팅 탭의
        //   모터 세팅(적용된 세트)이 ⑨ 의 축(위치 3개 + 손목)에서 기본값과 다르면 측정은 시작하지 않는다 (10-04 검토: 탭에서 정한 방향·영점을 무시하고
        //   3D 와 다른 자세로 움직였다 — 영점 18° 면 끝 높이가 38~87mm 어긋나고, 방향 반대면 바닥 아래). 그때는 탭에서 연결한 뒤 ⑨ (연결을 빌리는 길은 세팅대로 보낸다).
        //   결과 파일에는 그때의 모터 매핑 키를 적는다 (motormap — 적용·시험·앱 시작 때 '학습 때와 방향·영점이 다름' 비교). 연결 없이 돈 측정도 그 축들은
        //   기본값과 같다고 확인했으므로 지금 키를 적는다 (다른 축 — 그리퍼 등 — 의 세팅 때문에 다시 학습하라는 경고가 잘못 나오지 않게)
        private const string SAGCOMP_OWNER = "처짐보상";

        /// <summary>연결 없이 도는 ⑨ (예전 방식 — 방향 정·영점 2048·4096틱/360°) 와 로봇팔 세팅 탭의 적용된 모터 세팅이 그 축들에서 다른가 — 다르면 차이 문구
        /// (DLL CArmMotorSet_t.DescribeMappingDiff — "ID 12 영점 2048→2250"), 같거나 적용된 세팅이 없으면 "". 다르면 ⑨ 의 3D 자세와 실물 자세가 달라진다</summary>
        private string SagComp_LegacyMapDiff(int[] anAxes)
        {
            try
            {
                Ojw.CArmMotorSet_t set = (m_ArmConn != null) ? m_ArmConn.Motors : null;
                if (set == null || anAxes == null || anAxes.Length == 0) return "";
                int[] anTaken;
                Ojw.CArmMotorSet_t sub = set.MergeFor(anAxes, out anTaken);      // ⑨ 의 축만 (세트에 없는 축은 기본값)
                return Ojw.CArmMotorSet_t.DescribeMappingDiff(Ojw.CArmMotorSet_t.DefaultMappingKey(anAxes), sub.MappingKey());
            }
            catch (Exception ex) { return "모터 세팅과 비교하지 못함: " + ex.Message; }   // 비교할 수 없으면 다르다고 본다 (예전 방식으로 움직이지 않는다)
        }

        /// <summary>⑨ 설정에 로봇 정의를 잇는다 (UI 스레드) — sagcomp.txt 에 관절 줄(func·posids·wrist)이 없으면 로봇 정의(sagarm.txt — ArmSetup_SyncCfg 로 맞춘 것)의
        /// 관절로 채우고, 관절이 정의와 같으면 수직 관절 합·이름도 정의 값으로 (다르면 NaN 그대로 — DLL 확인이 3D 에서 잰 값을 쓴다).
        /// 반환 = 확인창 한 줄 (정보 — 막지 않는다. 막는 것은 [예] 뒤 SagComp_DefGate)</summary>
        private string SagComp_DefFill(Ojw.C3d.CSagSurveyCfg cfg, bool bIdsInFile, string strDir)
        {
            var ci = CultureInfo.InvariantCulture;
            Ojw.C3d.CSagArmCfg def = null;
            try
            {
                ArmSetup_SyncCfg();                                             // 밖에서 고친 sagarm.txt 를 먼저
                if (m_ArmCfg != null && m_strArmCfgFail == null && ArmSetup_SamePath(strDir, m_strArmCfgDir)) def = m_ArmCfg.Arm;
            }
            catch { def = null; }
            if (def == null) return "· 로봇 정의(" + Ojw.C3d.CSagArmSetup.FILE + ")를 읽지 못함 — sagcomp.txt 의 관절, 수직 관절 합은 3D 에서 잰 값\r\n";
            if (!bIdsInFile) { cfg.FuncNumber = def.FuncNumber; cfg.PosIDs = (int[])def.PosIDs.Clone(); cfg.WristID = def.WristID; }
            bool bSame = cfg.FuncNumber == def.FuncNumber && cfg.WristID == def.WristID && cfg.PosIDs != null && def.PosIDs != null && cfg.PosIDs.Length == def.PosIDs.Length;
            if (bSame) for (int i = 0; i < cfg.PosIDs.Length; i++) if (cfg.PosIDs[i] != def.PosIDs[i]) bSame = false;
            if (bSame) { cfg.DownSumDeg = def.DownSumDeg; cfg.Robot = def.DefName; }
            Ojw.C3d.CSagDefCheck chk = (m_C3d != null) ? m_C3d.SagDef_CheckSurvey(cfg) : null;
            if (bSame) return "· 로봇 정의: " + ((chk == null) ? def.DefName : (chk.Ok ? chk.Summary : "★" + chk.Why)) + "\r\n";
            string strIds = string.Format(ci, "func {0}/posids {1}/wrist {2}", cfg.FuncNumber,
                                          string.Join(",", Array.ConvertAll((cfg.PosIDs != null) ? cfg.PosIDs : new int[0], x => x.ToString(ci))), cfg.WristID);
            return "· sagcomp.txt 의 관절 (정의와 다름: " + strIds + ") — " + ((chk == null) ? "확인 못 함" : (chk.Ok ? "3D 와 맞음" : "★" + chk.Why)) + "\r\n";
        }

        /// <summary>[예] (측정+학습+검증) 직전 — DLL SagDef_CheckSurvey (⑨ 가 쓸 관절의 공구 끝 체인·꺾는 손목·바닥으로 수직, 공구 피치 Ry — 캐시) 가 맞지 않으면
        /// 까닭·고칠 곳을 알리고 false (실물은 움직이지 않는다 — DLL 측정 입구도 같은 확인을 한다). 맞으면 예전 경로 경고(Ry &lt; 90)만 로그에</summary>
        private bool SagComp_DefGate(Ojw.C3d.CSagSurveyCfg cfg, bool bIdsInFile)
        {
            Ojw.C3d.CSagDefCheck chk = (m_C3d != null) ? m_C3d.SagDef_CheckSurvey(cfg) : null;
            if (chk != null && chk.Ok)
            {
                foreach (string it in chk.Items) if (it.StartsWith("Ry ★", StringComparison.Ordinal)) Ojw.printf(txtLogHistory, "[처짐보상] {0}\r\n", it.Substring(3));
                return true;
            }
            string how;
            if (chk == null) how = ArmDef_KinHint(Ojw.C3d.ESagDefFail.NoKin);
            else if (chk.Kind != Ojw.C3d.ESagDefFail.Def) how = (ArmDef_WhyHint(chk.Why, chk.Kind) == chk.Why) ? "" : ArmDef_KinHint(chk.Kind);   // 까닭이 안내를 담고 있으면 한 번만
            else if (bIdsInFile) how = "sagcomp.txt 의 func·posids·wrist 를 3D 에 맞게 고치거나 (세 줄을 지우면 로봇 정의의 관절을 씀), 로봇팔 세팅 탭 '로봇 정의' 페이지에서 정의를 고치십시오";
            else how = "로봇팔 세팅 탭 '로봇 정의' 페이지에서 정의를 고치십시오";
            MessageBox.Show("⑨ 처짐 보상 — 시작하지 않았습니다 (실물은 움직이지 않음).\r\n\r\n로봇 정의 확인 안 됨: " + ((chk != null) ? chk.Why : "계획 기구학 없음") +
                            ((how.Length > 0) ? "\r\n\r\n" + how + "." : ""), "처짐 보상");
            return false;
        }

        private void btnSagComp_Click(object sender, EventArgs e)
        {
            if (Extra_Busy(true)) return;                     // 로봇팔 세팅 탭이 연결 중이어도 된다 (그 연결을 빌린다)
            if (ArmMotor_IsBusy())
            {
                // 로봇팔 세팅 탭이 동작 중(연결하는 중·조그·측정 …)이면 연결 여부와 상관없이 시작하지 않는다 — 연결하는 동안은 아직
                // '연결 안 됨' 으로 보여 예전 경로가 같은 포트를 열려 하고, 그 경로의 끝이 연결의 모션 싱크를 걷어 낼 수 있다
                MessageBox.Show("로봇팔 세팅 탭이 동작 중입니다 (연결·조그·측정 등) — 끝난 뒤 다시.", "처짐 보상");
                return;
            }
            string strDir = GetUrdfOutputDir();
            Directory.CreateDirectory(strDir);
            int nPort, nBaud;
            bool bIdsInFile;
            var cfg = SagComp_LoadCfg(strDir, out nPort, out nBaud, out bIdsInFile);
            string strDefLine = SagComp_DefFill(cfg, bIdsInFile, strDir);   // 관절 줄이 없으면 로봇 정의의 관절로, 정의와 같으면 수직 관절 합도 — 확인창 한 줄 (정보)
            string strModel = Path.Combine(strDir, SAGCOMP_MODEL);
            bool bDyn = cfg.PathMs != null && cfg.PathMs.Length > 0;
            Ojw.CArmConn_t conn = ArmMotor_IsConnected() ? m_ArmConn : null;   // 로봇팔 세팅 탭의 연결 (없으면 null — 예전 경로)
            if (conn == null)
            {
                // 연결 없이 도는 예전 경로도 포트·속도는 로봇팔 세팅 탭 칸 (sagcomp.txt 의 port/baud 대신)
                int nTabPort, nTabBaud;
                if (!ArmMotor_ToolPort("⑨ 처짐보상", false, out nTabPort, out nTabBaud)) return;
                nPort = nTabPort; nBaud = nTabBaud;
            }
            string strIds = string.Join(",", Array.ConvertAll(cfg.PosIDs, x => x.ToString()));
            int[] anAll = new int[cfg.PosIDs.Length + 1];                       // ⑨ 가 쓰는 관절 = 위치 + 손목
            Array.Copy(cfg.PosIDs, anAll, cfg.PosIDs.Length); anAll[anAll.Length - 1] = cfg.WristID;
            string strLink;
            if (conn != null)
            {
                Ojw.CScene_t sc0 = conn.Scene;
                strLink = string.Format("· 로봇팔 세팅 탭의 연결(COM{0}) 사용, ID {1} 토크 ON — 그 탭의 모터 세팅(방향·영점·리밋)으로 보냄\r\n· IK 수식 {2}, 관절 {3} / 손목 {4}\r\n", conn.Port,
                    Ojw.CArmMotorSet_t.RangeText((sc0 != null) ? sc0.IDs : new int[0]), cfg.FuncNumber, strIds, cfg.WristID);
                if (conn.IsTorqueOn)
                    strLink += "· 지금 토크 ON — 끝나도 켜 둡니다 (처짐 보정이 켜져 있거나 남아 있으면 조그는 막힘)\r\n";
            }
            else
            {
                strLink = string.Format("· COM{0} @ {1} (로봇팔 세팅 탭의 포트·속도), IK 수식 {2}, 관절 {3} / 손목 {4} — 로봇팔 세팅 탭 연결 없음: 예전 방식 (방향 정·영점 2048, 리밋 자르기 없음)\r\n",
                                        nPort, nBaud, cfg.FuncNumber, strIds, cfg.WristID);
                string strMap0 = SagComp_LegacyMapDiff(anAll);
                if (strMap0.Length > 0)
                    strLink += "★로봇팔 세팅 탭의 모터 세팅이 기본값과 다름 (" + strMap0 + ") — 이 방식으로는 3D 와 다른 자세로 움직여 [예] 는 하지 않습니다.\r\n" +
                               "   로봇팔 세팅 탭에서 연결한 뒤 다시 누르십시오 (그 연결의 모터 세팅으로 측정)\r\n";
            }

            DialogResult dr = MessageBox.Show(
                "⑨ 처짐(중력)·주행 보상 — 측정·학습·검증\r\n\r\n" +
                "★로봇팔이 실제로 움직입니다. 작업 공간을 비우십시오.\r\n" +
                strLink +
                strDefLine +
                string.Format("· r {0:F0}~{1:F0}mm {2}분할, z {3}, 요 {4}{5}\r\n", cfg.RStart, cfg.REnd, cfg.RSteps,
                    string.Join("/", Array.ConvertAll(cfg.ZLevels, x => x.ToString("F0"))),
                    string.Join("/", Array.ConvertAll(cfg.YawDegs, x => x.ToString("F0"))), cfg.BothDirections ? ", 왕복" : "") +
                "· 정지: 보상 OFF 로 지점마다 PlayXyz → 정착 → 엔코더 → FK 실측 → (r,z) 학습 → 보상 ON 재측정\r\n" +
                (bDyn ? string.Format("· 주행: 같은 직선을 {0} ms 로 왕복(PlayXyzPath), 주행 중 엔코더 표본 → 속도·가속도항 학습 → 재주행 검증\r\n",
                    string.Join("/", Array.ConvertAll(cfg.PathMs, x => x.ToString()))) : "· 주행 측정 없음 (speeds= 비어 있음)\r\n") +
                string.Format("· 결과: {0}\r\n", strModel) +
                "· 설정: " + SAGCOMP_CFG + " (같은 폴더)\r\n\r\n" +
                "[예] 측정+학습+검증   [아니오] 학습 파일 다시 로드만   [취소]",
                "처짐 보상", MessageBoxButtons.YesNoCancel);
            if (dr == DialogResult.Cancel) return;
            if (dr == DialogResult.No)
            {
                // 학습 파일 다시 읽기 = 적용 중 보정 모델 바꿈. 로봇팔이 토크를 붙잡고 있어도 된다 (10-03 — SagComp_UiSync 주석: 다음 틱은 그대로 이어지고
                // 새 보정은 다음 동작에 걸쳐 들어온다). 로봇팔이 동작 중이면 그 동작이 지금 모델로 보내고 있어 바꾸지 않는다
                if (ArmMotor_IsBusy()) { MessageBox.Show("로봇팔 세팅 탭이 동작 중입니다 — 끝난 뒤 다시.", "처짐 보상"); return; }
                SagComp_AutoLoad();
                ArmMotor_UpdateUi();
                return;
            }
            // 확인창이 떠 있는 사이 로봇팔 세팅 탭의 상태가 바뀌었으면 시작하지 않는다 (위에서 고른 경로 — 연결 빌림/직접 연결 — 가 맞지 않게 된다)
            if (ArmMotor_IsBusy() || ArmMotor_IsConnected() != (conn != null))
            {
                MessageBox.Show("로봇팔 세팅 탭의 연결 상태가 바뀌었습니다 — 다시 누르십시오.", "처짐 보상");
                return;
            }
            // 연결 없이 (예전 방식 — 모터 세팅 없이 방향 정·영점 2048 로 보냄): 탭의 모터 세팅이 ⑨ 의 축에서 기본값과 다르면 측정하지 않는다
            if (conn == null)
            {
                string strMap = SagComp_LegacyMapDiff(anAll);
                if (strMap.Length > 0)
                {
                    MessageBox.Show("⑨ 처짐 보상 — 시작하지 않았습니다.\r\n\r\n" +
                                    "로봇팔 세팅 탭의 모터 세팅(방향·영점)이 기본값과 다릅니다 (" + strMap + ").\r\n" +
                                    "연결 없이 도는 ⑨ 는 예전 방식(방향 정·영점 2048, 리밋 자르기 없음)으로 실물에 보내므로 3D 와 다른 자세가 됩니다 (바닥을 누를 수 있음).\r\n\r\n" +
                                    "로봇팔 세팅 탭에서 연결한 뒤 ⑨ 를 누르십시오 — 그 연결의 모터 세팅으로 측정합니다.", "처짐 보상");
                    return;
                }
            }
            if (!SagComp_DefGate(cfg, bIdsInFile)) return;                      // 로봇 정의 확인 — ⑨ 가 쓸 관절이 3D 와 맞나 ([예] 에서만, 움직이기 전)
            string strMapKey = ArmSetup_MapKey();                               // 결과 파일의 motormap (지금 적용된 모터 세팅 — 없으면 null = 기본 매핑)

            Extra_Run("⑨ 처짐보상", delegate()
            {
                var c3d = m_C3d;
                Ojw.CScene_t scene = null;
                bool bWasLoaded = c3d.SagComp_IsLoaded;
                bool bOwned = false, bTorqueWas = true, bTurnedOn = false;   // 연결을 빌렸을 때 — 잠금을 잡았나, 시작할 때 토크가 켜져 있었나, 이 측정이 토크를 켰나
                var stop = new Ojw.CSim2Real.DIsStopped(delegate() { return m_bMechStop; });
                var log = new Ojw.CSim2Real.DLog(S2R_Log);
                try
                {
                    if (conn != null)
                    {
                        // 로봇팔 세팅 탭의 연결을 빌린다 — 잠금 → 토크(꺼져 있었으면 켬, 경고는 로그) → 3D·실물 맞춤 확인 (실패면 시작하지 않는다)
                        scene = conn.Scene;
                        if (scene == null) { S2R_Log("로봇팔 연결이 끊어짐 — 중단"); return null; }
                        foreach (int id in anAll)
                            if (Array.IndexOf(scene.IDs, id) < 0) { S2R_Log("로봇팔 연결에 ID " + id + " 없음 — 중단"); return null; }
                        if (!conn.TryAcquire(SAGCOMP_OWNER)) { S2R_Log("로봇팔 연결 사용 중: " + conn.BusyOwner + " — 중단"); return null; }
                        bOwned = true;
                        bool bConnOn = conn.IsTorqueOn;                  // 연결이 아는 토크 상태 — OFF 면 아래에서 토크를 켠다
                        bTorqueWas = bConnOn;                             // 끝에서 토크를 끄지 않는다 (시작 때 켜져 있었음 — 연결이 알았거나 서보가 붙잡고 있었음)
                        if (!bConnOn)
                        {
                            // 시작 때 토크 상태는 서보에서도 읽는다 (주소 64 — 로봇팔 세팅 탭의 작업과 같은 규칙, DLL SagArm_Open). 연결은 OFF 로 아는데 서보가
                            //   토크를 붙잡고 있으면(연결 때 토크 상태를 못 읽음·다른 프로그램이 켬) 끝에서 끄지 않는다 — 끄면 붙잡혀 있던 팔이 떨어진다.
                            //   못 읽은 축이 있으면 토크를 건드리지 않고 시작하지 않는다
                            string strRd;
                            Dictionary<int, long> dTq = scene.ReadReg(64, 1, out strRd);
                            var lstHeld = new List<int>(); var lstUnk = new List<int>();
                            foreach (int id in scene.IDs)
                            {
                                long v;
                                if (dTq == null || !dTq.TryGetValue(id, out v)) lstUnk.Add(id); else if (v != 0) lstHeld.Add(id);
                            }
                            if (lstUnk.Count > 0)
                            { S2R_Log("서보 토크 상태를 읽지 못함 — ID " + Ojw.CArmMotorSet_t.RangeText(lstUnk.ToArray()) + (string.IsNullOrEmpty(strRd) ? "" : " (" + strRd + ")") + " — 중단 (토크 그대로)"); return null; }
                            if (lstHeld.Count > 0)
                            {
                                bTorqueWas = true;
                                S2R_Log("시작 때 서보 토크: ID " + Ojw.CArmMotorSet_t.RangeText(lstHeld.ToArray()) + " 이 이미 토크를 붙잡고 있었음 (연결은 토크 OFF 로 알고 있었음) — 끝나도 토크를 끄지 않음");
                            }
                        }
                        string strWhy;
                        if (!bConnOn)
                        {
                            if (!conn.TorqueOn(SAGCOMP_OWNER, out strWhy)) { S2R_Log("토크 ON 실패 — " + strWhy + " — 중단"); return null; }
                            bTurnedOn = true;
                            if (strWhy.Length > 0) S2R_Log("토크 ON 경고: " + strWhy.Replace("\r\n", " / "));
                        }
                        if (!conn.EnsureSynced(SAGCOMP_OWNER, out strWhy)) { S2R_Log("3D·실물 맞춤 확인 실패 — " + strWhy + " — 중단"); return null; }
                    }
                    else
                    {
                        int[] anSign = new int[anAll.Length]; for (int i = 0; i < anSign.Length; i++) anSign[i] = 1;
                        scene = new Ojw.CScene_t(c3d, anAll, anSign);      // 모션 싱크 등록 = PlayXyz 가 실물을 끈다
                        scene.open(nPort, nBaud);
                        if (!scene.IsConnected) { S2R_Log("실물 연결 실패 — 중단"); return null; }
                        scene.torqon();
                        if (!scene.IsTorqueOn) { S2R_Log("토크 ON 실패 — 중단"); return null; }
                    }

                    // ① 정지 측정 + 학습
                    string rep1;
                    var model = c3d.SagComp_Survey(scene, cfg, log, stop, out rep1);
                    if (model == null) return null;
                    model.MotorMap = strMapKey;                          // 학습 때 모터 매핑 (아래 두 번 저장 모두) — 다시 학습하면 지금 세팅으로 바뀐다
                    model.Save(strModel);
                    S2R_Log("저장: " + strModel);

                    // ② 정지 검증 (PlayXyz, 보상 ON)
                    string rep2;
                    float rmse = c3d.SagComp_Verify(scene, cfg, model, log, stop, out rep2);

                    // ③ 주행 측정 + 학습 + 검증 (PlayXyzPath)
                    float rmseDyn = -1f;
                    if (bDyn && !m_bMechStop)
                    {
                        string rep3;
                        c3d.SagComp_SurveyDynamic(scene, cfg, model, log, stop, out rep3);
                        model.Save(strModel);                            // 주행항까지 담아 다시 저장
                        File.WriteAllText(Path.ChangeExtension(strModel, ".trace.csv"), c3d.SagComp_GetTraceCsv(), Encoding.UTF8);
                        if ((model.HasDynamic || model.HasJoint) && !m_bMechStop)   // 관절 모델은 주행 중에도 출력 단에서 보정한다
                        {
                            string rep4;
                            rmseDyn = c3d.SagComp_VerifyDynamic(scene, cfg, model, log, stop, out rep4);
                        }
                    }
                    c3d.SagComp_Set(model);                              // 이후 PlayXyz/PlayXyzPath 부터 적용
                    return string.Format("학습 완료 — {0}\r\n정지 검증 잔차 RMSE {1:F2}mm{2}\r\n{3}", model.Summary(), rmse,
                        rmseDyn >= 0 ? string.Format(" / 주행 검증 잔차 RMSE {0:F2}mm", rmseDyn) : "", strModel);
                }
                catch (Exception ex) { S2R_Log("오류: " + ex.Message); return null; }
                finally
                {
                    if (conn != null)
                    {
                        // 빌린 연결 — 토크를 시작 상태로 되돌리고 잠금만 놓는다 (포트를 닫거나 싱크를 지우지 않는다). 끄는 것은 이 측정이 켠 토크만
                        //   (켜기 전에 멈췄으면 — 서보 토크 상태를 읽지 못함 등 — 토크를 건드리지 않는다)
                        if (bOwned && bTurnedOn && !bTorqueWas) { try { conn.TorqueOff(SAGCOMP_OWNER); } catch { } }
                        if (bOwned) { try { conn.Release(SAGCOMP_OWNER); } catch { } }
                    }
                    else if (scene != null)
                    {
                        try { scene.torqoff(); } catch { }
                        // 닫고 실물 싱크 해제 — 이후 3D 만 움직인다. 싱크가 아직 이 장면 것일 때만 이 장면을 만들기 전 것(보통 없음)으로 되돌린다
                        // (그사이 로봇팔 세팅 탭 연결 등 다른 것이 건 싱크는 건드리지 않는다 — 예전에는 무조건 null)
                        try { scene.Release(); } catch { }
                    }
                    if (!c3d.SagComp_IsLoaded && bWasLoaded) SagComp_AutoLoad();
                }
            });                                                       // 끝나면 Extra_AfterRun 이 로봇팔 세팅 탭 상태(잠금·조그 막힘 표시)를 다시 본다
            if (conn != null) ArmMotor_UpdateUi();                    // 연결을 빌린 측정 — 로봇팔 세팅 탭 잠금을 바로 건다 (연결 중 + 측정 스레드)
        }
    }
}
