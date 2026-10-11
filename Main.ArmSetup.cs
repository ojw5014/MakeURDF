// ================================================================
// 로봇팔 세팅 탭 — 고급 페이지 (측정·학습·시험, 2026-10-03)
//   측정 계획 → 바닥 측정 → 학습(결과 파일) → 결과 파일 닿기 시험(보정 끔↔켬) → 적용 을 한 페이지에서.
//   학생용 네 단계(오차 재기 → 보정 데이터 → 효과 보기 → 적용)는 같은 탭의 '처짐 보정 실습' 페이지 (Main.ArmPractice.cs) — 같은 잠금·작업 띄우기를 쓴다.
//   로봇 정의(공구 끝·꺾는 손목·바닥으로 수직 — sagarm.txt 맨 앞 7키)는 같은 탭의 '로봇 정의' 페이지 (Main.ArmDef.cs) — 확인은 DLL SagDef_Check.
//   자로 재는 틈 재기(SagRun_GapJob)와 질문 줄은 화면에서 뺐다 (바닥 첫닿음으로 재므로 손 입력이 없다 — DLL 함수는 그대로).
//   절차·계산·판정·파일은 전부 DLL (Ojw.C3d.SagRun_* / SagArm_*, 설정 = Ojw.C3d.CSagArmSetup — <urdf>\sagarm.txt).
//   실물은 같은 탭의 연결(Ojw.CArmConn_t, Main.ArmMotor.cs)을 빌려 쓴다 — 이 파일은 포트를 열지 않는다.
//   여기서는 ① 설정 읽기·쓰기 (값 확인도 DLL 의 설정 해석으로) ② 확인 대화 ③ 작업 스레드 띄우기(ArmSetup_Run) ④ 결과를 그룹 라벨·로그에
//   ⑤ 잠금(ArmSetup_Lock — 잠금 중 DH 다시 해석은 미뤘다가 풀릴 때) ⑥ 창 닫기·탭 전환 막기만 한다.
//
//   스레드
//   · 계획 만들기·계획 확인·바닥 측정·닿기 시험·학습은 블로킹 — ArmSetup_Run → Extra_Run 스레드. 실물 작업은 늘 ArmSetup_Run(.., bRobot: true, ..).
//     DLL 의 로그·중단 대리자는 그 스레드에서 불린다 → 화면에는 BeginInvoke 로만 넘긴다 (로그 창 S2R_Log 는 UI 로 동기 Invoke — UI 는 작업을 기다리지 않는다).
//     작업 스레드는 결과를 지역 변수에 남기기만 하고, 컨트롤·설정은 끝난 뒤 UI 스레드(after)에서 바꾼다. 경로·설정은 UI 스레드에서 미리 잡아 넘긴다
//     (GetUrdfOutputDir 는 TextBox 를 읽는다).
//   · 적용·되돌리기·쌍둥이 모델 고르기는 UI 스레드 (DLL 규칙 — 작업 중이면 DLL 도 거절한다).
//   · 계획 기구학(SagRun_Prepare — 지금 3D 의 수식으로 맞추는 OpenGL 컨트롤, DLL 이 처음 한 번만 만든다)은 UI 스레드에서: 시작 때, 탭을 볼 때,
//     이 탭을 보는 동안 3D 모델을 다시 해석한 뒤 (MakeAll 끝 — ArmDef_ModelChanged), 작업 버튼마다 (ArmDef_Prepare — 작업·학습·계획 중이면 DLL 이 바꾸지 않는다,
//     바뀐 3D 는 작업 입구의 정의 확인이 거절한다. 맞추지 못하면 로그에 ★ 까닭).
//   · 탭을 볼 때 다시 읽기(ArmSetup_OnShown)는 작업·조그·⑨·3D 모션 가운데 하나라도 돌면 하지 않는다 (ArmSetup_AnyBusy).
//     그 안의 기구학 확인(SagArm_CheckKinematics — 로봇 정의 확인 포함)은 관절값 사본 FK 라 3D 관절값을 바꾸지 않는다 — 작업이 실물로 보내는 3D 와 섞이지 않는다.
//   · 3D 만 작업(미리보기·3D 닿기)이 끝난 뒤 출력 단에 남는 보정은 DLL 이 시작 때 상태로 되돌린다. 앱은 그것을 따로 기억하지 않는다 —
//     조그 막음은 출력 단 상태(SagComp_HasOffsets)를 바로 본다 (Main.ArmMotor.cs 머리 주석 '처짐 보정과 조그').
//   설정 (sagarm.txt — 사용자가 메모장으로도 고친다: 계획 세트·창·틈 재기·옮기는 높이·경유점 … 은 파일에만 있다)
//   · DLL 의 Save 는 파일을 통째로 다시 쓴다 → 쓰기 전에는 늘 파일과 먼저 맞춘다 (ArmSetup_SyncCfg — 시각·크기가 바뀌었거나 앞 읽기가 실패했으면
//     다시 읽고, 이 페이지의 칸은 여기서 고치지 않은 것만 새 값으로). 작업이 끝난 뒤 after() 의 저장(계획·결과 파일 칸)도 같은 길이다.
//   · 작업 버튼은 그때 다시 읽었으면 시작하지 않고 바뀐 값을 보인 뒤 다시 누르게 한다 — 작업은 늘 지금 파일 값으로.
//     있는 파일을 읽지 못하면(잠김 등 '★오류') 저장도 작업도 하지 않고 다음에 다시 읽는다 (기본값·옛 값으로 실물을 움직이지 않는다).
//   · 파일에 그대로 쓰지 못한 줄이 있어도(LoadWarn — 읽지 못한 줄·범위 밖 값) 저장도 작업도 하지 않는다: 저장하면 DLL 이 그 줄을 기본값으로
//     바꾸거나 빼고(읽지 못한 계획 세트 planX) 다시 쓴다. 그 줄을 알리고 사용자가 고치게 한다 — 고치면 다음에 다시 읽는다.
//   측정 계획 CSV 도 같은 규칙 — 행을 읽은 뒤 파일이 바뀌었으면(시각·크기) 탭을 볼 때·미리보기·측정 버튼에서 다시 읽고, 버튼은 그때 시작하지 않는다.
//     읽지 못했어도 그 두 버튼은 켜 둔다 — 파일을 닫거나 고친 뒤 다시 누르면 다시 읽는다 (sagarm.txt 를 읽지 못했을 때의 작업 버튼과 같다).
//   ⑥ 적용·되돌리기·[ … ] 는 목록을 읽은 urdf 폴더에만 — 출력 폴더 칸이 그사이 바뀌었으면 그 폴더로 다시 읽고 멈춘다 (ArmSetup_ListDirCheck).
//   컨트롤은 Main.Designer.cs 에 있다 (tabArmSetup → tabArmInner → tabArmExp '고급').
// ================================================================
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private bool m_bArmRun = false;                   // 이 페이지에서 띄운 작업이 도는 중
        private bool m_bArmLocked = false;                // ArmSetup_Lock 이 건 잠금 (작업·조그가 돌거나 연결이 사용 중)
        private bool m_bArmExpUiSync = false;             // 이 페이지 컨트롤을 코드로 바꾸는 중 (이벤트 무시)
        private volatile string m_strArmLast = "";        // 작업 스레드가 남긴 짧은 결과 (끝난 뒤 UI 스레드에서 lblArmBusy 로)
        private readonly Dictionary<Control, bool> m_dicArmLockSaved = new Dictionary<Control, bool>();   // 잠그기 전 Enabled
        private bool m_bArmDhReadOnlySaved = false;       // 잠그기 전 DH 글상자 ReadOnly
        private bool m_bArmPreviewPending = false;        // 잠금 중 미룬 UpdatePreview — 풀릴 때 한 번
        private string m_strArmRebuildDh = null;          // 잠금 중 미룬 MakeAll 의 마지막 DH (null = 없음) — 풀릴 때 한 번

        // ── 설정·목록·표시 상태 ──
        private Ojw.C3d.CSagArmSetup m_ArmCfg = null;     // sagarm.txt (DLL 이 읽고 쓴다) — 이 페이지 칸과 마지막으로 맞춘 값 (다시 읽을 때 고친 칸을 가리는 기준)
        private string m_strArmCfgDir = null;             // m_ArmCfg 를 읽은 urdf 폴더
        private string m_strArmCfgStamp = null;           // 그때 sagarm.txt 의 시각·크기 — 다르면 밖에서 고친 것 (다시 읽는다)
        private string m_strArmCfgFail = null;            // 마지막 읽기가 실패한 까닭 ('★오류 …') — 있으면 저장·작업을 하지 않고 다음에 다시 읽어 본다
        private string m_strArmCfgWarnShown = null;       // 마지막으로 로그에 알린 LoadWarn (같은 경고를 되풀이하지 않게)
        private bool m_bArmPrepared = false;              // 계획 기구학 준비됨 (SagRun_Prepare — 지금 3D 의 수식 글로 만든다)
        private bool m_bArmKinOk = false;                 // 로봇 정의가 지금 3D 와 맞음 (SagArm_CheckKinematics — 정의 확인 SagDef_Check 포함)
        private string m_strArmKinWhy = "";
        private Ojw.C3d.ESagDefFail m_eArmKinKind = Ojw.C3d.ESagDefFail.None;   // 맞지 않은 종류 — 안내 문구 ArmDef_KinHint (Main.ArmDef.cs)
        private List<Ojw.C3d.CFloorPlanRow> m_lstArmPlan = null;   // 지금 계획 (null = 없음)
        private string m_strArmPlanPath = null;           // 그 계획 파일 (전체 경로)
        private string m_strArmPlanWhy = "";              // 계획 파일을 읽지 못한 까닭 (읽었으면 건너뛴 행 — DLL SagRun_PlanParse 의 why)
        private string m_strArmPlanNote = "";             // 계획 라벨 끝줄 — 마지막 3D 미리보기 결과
        private string m_strArmPlanStamp = null;          // 그 행을 읽거나 만들 때 계획 파일의 시각·크기 — 다르면 밖에서 고친 것 (다시 읽는다)
        private readonly List<Ojw.C3d.CSagFileInfo> m_lstArmResults = new List<Ojw.C3d.CSagFileInfo>();   // cmbArmResult 항목 순서
        private readonly List<Ojw.C3d.CSagFileInfo> m_lstArmBackups = new List<Ojw.C3d.CSagFileInfo>();   // cmbArmBackup 항목 순서
        private string m_strArmListDir = null;            // 결과·백업 목록을 읽은 urdf 폴더 — ⑥ 적용·되돌리기는 이 폴더에만 (ArmSetup_ListDirCheck)
        private string m_strArmResultSel = null;          // 고른 결과 파일 (전체 경로) — 목록을 다시 읽어도 유지
        private string m_strArmBrowsed = null;            // [ … ] 로 고른 결과 폴더 밖의 파일 (목록 끝에 '다른 파일')
        private string m_strArmTestFor = null;            // lblArmTest 가 보이는 파일 (그 파일의 작업 결과 줄이면 목록을 다시 읽어도 그대로)
        private string m_strArmMeasureLine = null;        // 마지막 측정 결과 줄 (lblArmMeasure 아래줄)
        private int m_nArmDataRows = -1;                  // 학습 데이터셋 행 수 (없으면 -1)
        private string m_strArmTwinInfo = null;           // 마지막 쌍둥이 모델 고르기(SagRun_TwinUse) 문구
        private Ojw.C3d.CSagComp m_ArmTwinModel = null;   // 그때 쌍둥이 모델 (그 뒤 적용·작업이 바꿨는지 보려고)
        private string m_strArmLastRunDir = null;         // 마지막 작업 폴더 (폴더 열기)
        private bool m_bArmBusyWarn = false;              // lblArmBusy 가 탭 상태 경고를 보이는 중

        // ════════════════════════════════════════════════════════════
        // 초기화·표시
        // ════════════════════════════════════════════════════════════

        /// <summary>Main_Load 끝 (InitPlaybackTab 다음, ArmMotor_Init 뒤) — 계획 기구학 준비(UI 스레드), sagarm.txt 읽기·LoadWarn 알림,
        /// 콤보 기본값, 로봇 정의와 3D 확인·정의 페이지 (ArmDef_Init), 적용 중 모델의 모터 매핑 확인 (C10 — 다르면 로그에), 실습 페이지 (Prac_Init)</summary>
        private void ArmSetup_Init()
        {
            m_bArmExpUiSync = true;
            try { if (cmbArmTwinSrc.Items.Count > 0) cmbArmTwinSrc.SelectedIndex = 0; }
            finally { m_bArmExpUiSync = false; }
            // 계획 기구학 — 지금 3D 의 수식 글로 만드는 OpenGL 컨트롤이라 UI(STA) 스레드에서 만든다 (작업 스레드에서는 만들지 않는다 — 계획·시험·학습이 "계획 기구학을 만들지 못했습니다" 로 거절)
            string strPrepWhy = "";
            try { m_bArmPrepared = m_C3d != null && m_C3d.SagRun_Prepare(out strPrepWhy); }
            catch (Exception ex) { m_bArmPrepared = false; strPrepWhy = ex.Message; ArmMotor_Log("계획 기구학 준비 오류: " + ex.Message); }
            if (!m_bArmPrepared) ArmMotor_Log("★측정·학습·시험 페이지 — 계획 기구학을 준비하지 못함: 계획·학습·시험은 거절됩니다" +
                                              ((strPrepWhy != null && strPrepWhy.Length > 0) ? " (" + strPrepWhy + ")" : ""));
            ArmSetup_LoadCfg(true);
            ArmSetup_PushCfg();
            string strCfgWarn = ArmSetup_CfgWarn();
            if (strCfgWarn.Length > 0)
            {
                ArmSetup_Busy("★" + Ojw.C3d.CSagArmSetup.FILE + ": " + strCfgWarn);   // 로그에도 (ArmSetup_LoadCfg) — 읽지 못함('★오류')·그대로 쓰지 못한 줄이면 저장·작업을 하지 않는다
                m_bArmBusyWarn = true;
            }
            ArmSetup_TwinUiSync();
            ArmSetup_CheckKin();                                     // 로봇 정의와 3D — 실습 카드·버튼 (탭을 볼 때마다 다시)
            ArmDef_Init();                                           // 로봇 정의 페이지 — 본보기·칸·확인 (Main.ArmDef.cs)
            Prac_Init();                                             // 처짐 보정 실습 페이지 — 진행 상태·그래프 (Main.ArmPractice.cs)
            // 앱이 시작하며 읽은 적용 중 모델 (SagComp_AutoLoad) — 학습 때와 모터 방향·영점이 다르면 알린다 (C10)
            try
            {
                string strWarn = Ojw.C3d.SagArm_MotorMapWarn(m_ArmConn, (m_C3d != null) ? m_C3d.SagComp_Get() : null);
                if (strWarn.Length > 0) ArmMotor_Log("적용 중 처짐 모델 " + SAGCOMP_MODEL + " — " + strWarn);
            }
            catch (Exception ex) { ArmMotor_Log("매핑 확인 오류: " + ex.Message); }
            ArmSetup_UpdateEnable();
        }

        /// <summary>탭을 고를 때·작업이 끝날 때 — sagarm.txt 와 맞추고(ArmSetup_SyncCfg — 밖에서 고쳤으면 다시 읽어 이 페이지에서 고치지 않은 칸만 새 값으로)
        /// 계획·결과/백업 목록·데이터셋·적용 중 모델·기구학 확인을 다시 읽어 라벨·목록을 채운다.
        /// 작업·조그·⑨·3D 모션 가운데 하나라도 돌면 버튼 상태만 고친다 (이 페이지의 작업만 보면 조그·⑨ 가 실물로 보내는 사이에도 돈다)</summary>
        private void ArmSetup_OnShown()
        {
            if (IsDisposed) return;
            if (ArmSetup_AnyBusy()) { ArmSetup_UpdateEnable(); return; }
            ArmDef_Prepare();                                        // 계획 기구학을 지금 3D 로 (탭 밖에서 바뀐 3D 모델 — 맞추지 못하면 로그에 ★ 까닭)
            ArmSetup_SyncCfg();                                      // 폴더가 바뀌었거나 밖에서 고친 sagarm.txt
            ArmSetup_RefreshPage();
            Prac_Refresh();                                          // 실습 페이지 — 진행 상태·그래프 (그 폴더의 sagcomp_practice)
            ArmDef_OnShown();                                        // 로봇 정의 페이지 — 콤보를 지금 3D 로·확인·3D 표시 (Main.ArmDef.cs)
            ArmSetup_UpdateEnable();
        }

        /// <summary>지금 설정으로 계획·결과/백업 목록·데이터셋·적용 중 모델·기구학 확인·라벨을 다시 채운다. 3D 관절값은 바꾸지 않는다 (기구학 확인은
        /// 관절값 사본 FK) — 그래도 부르는 쪽이 ArmSetup_AnyBusy 를 먼저 본다</summary>
        private void ArmSetup_RefreshPage()
        {
            try
            {
                ArmSetup_RefreshPlan();
                ArmSetup_FillResults();
                ArmSetup_FillBackups();
                ArmSetup_RefreshData();
                ArmSetup_RefreshActive();
                // 로봇 정의가 3D 와 맞나 — 관절값 사본 FK (3D 관절값을 바꾸지 않는다). 아니면 미리보기·측정·시험 버튼을 끄고 아래 줄에 까닭·안내
                ArmSetup_CheckKin();
                ArmSetup_TestLabel(false);
                ArmSetup_MeasureLabel();
                ArmSetup_TwinUiSync();
                ArmSetup_StatusLine();
            }
            catch (Exception ex) { ArmMotor_Log("측정·학습·시험 페이지 갱신 오류: " + ex.Message); }
        }

        /// <summary>로봇 정의와 3D 확인 (DLL SagArm_CheckKinematics — 정의 확인 SagDef_Check(캐시) + 3D 와 계획 기구학의 공구 끝, 관절값 사본 FK 라 3D 관절값을
        /// 바꾸지 않는다). 결과 = m_bArmKinOk · m_strArmKinWhy · m_eArmKinKind (실패 종류 — 안내 ArmDef_KinHint). 버튼·안내 줄은 부른 쪽이 고친다</summary>
        private void ArmSetup_CheckKin()
        {
            string why = "";
            Ojw.C3d.ESagDefFail kind = Ojw.C3d.ESagDefFail.None;
            try { m_bArmKinOk = m_bArmPrepared && m_C3d != null && m_ArmCfg != null && m_C3d.SagArm_CheckKinematics(m_ArmCfg.Arm, true, out why, out kind); }
            catch (Exception ex) { m_bArmKinOk = false; why = "★오류: " + ex.Message; kind = Ojw.C3d.ESagDefFail.Def; }
            if (m_bArmKinOk) { m_strArmKinWhy = ""; m_eArmKinKind = Ojw.C3d.ESagDefFail.None; }
            else if (!m_bArmPrepared || m_C3d == null) { m_strArmKinWhy = "계획 기구학을 만들지 못했습니다"; m_eArmKinKind = Ojw.C3d.ESagDefFail.NoKin; }   // 안내(앱을 다시 켜기)는 ArmDef_KinHint
            else
            {
                m_strArmKinWhy = (why != null && why.Length > 0) ? why : "로봇 정의를 확인하지 못함";
                m_eArmKinKind = (kind == Ojw.C3d.ESagDefFail.None) ? Ojw.C3d.ESagDefFail.Def : kind;
            }
        }

        /// <summary>작업·조그·⑨·3D 모션 가운데 하나라도 도는가 (UI 스레드) — 이 페이지의 작업, 잠금, 이 탭의 동작(조그·읽기 …), 측정 스레드(⑨·⑤~⑧·①②④),
        /// DLL 작업 표시, 연결 잠금 소유자, 3D 모션. 다시 읽기·적용·되돌리기·쌍둥이 바꾸기를 하지 않는 조건</summary>
        private bool ArmSetup_AnyBusy()
        {
            return m_bArmRun || m_bArmLocked || m_bArmMotorRun || Extra_IsBusy() || Ojw.C3d.SagRun_IsBusy ||
                   (m_ArmConn != null && m_ArmConn.BusyOwner != null) || (m_C3d != null && m_C3d.MoveJoints_IsMoving());
        }

        /// <summary>이 페이지 버튼 활성 — 잠금·연결·기구학·계획·결과 파일·데이터셋. 작업 중에는 중단만. 실습 페이지도 같은 조건으로 (Prac_UpdateEnable)</summary>
        private void ArmSetup_UpdateEnable()
        {
            if (IsDisposed) return;
            bool bIdle = !m_bArmLocked && !m_bArmRun;
            bool bReal = m_ArmConn != null && m_ArmConn.IsConnected && !m_ArmConn.ConfigMismatch;   // 실물 작업은 연결이 있어야
            bool bKin = m_bArmPrepared && m_bArmKinOk;
            // ② 미리보기·③ 측정 — 측정할 행이 있거나 계획 파일이 정해져 있으면 누를 수 있다. 누르면 먼저 그 파일을 다시 본다 (ArmSetup_PlanCheck —
            // 고쳤으면 다시 읽고 멈춤, 읽지 못하면 까닭). 파일을 읽지 못했거나 행이 없어도 끄지 않는다: 파일을 닫거나 고친 뒤 '다시 누르십시오' 를
            // 따를 수 있게 (sagarm.txt 를 읽지 못해도 작업 버튼을 켜 두는 것과 같은 규칙 — 행이 없으면 처리기가 알린다)
            bool bPlan = ArmSetup_PlanRows() > 0 || m_strArmPlanPath != null;
            Ojw.C3d.CSagFileInfo fi = ArmSetup_SelectedResult();
            bool bFc = fi != null && fi.NFc > 0;                                                    // 첫닿음 모델이 있는 결과 파일 (닿기 시험)
            btnArmPlanMake.Enabled = btnArmPlanLoad.Enabled = bIdle && m_bArmPrepared;
            btnArmPlanDry.Enabled = bIdle && bKin && bPlan;
            chkArmHold.Enabled = bIdle;
            btnArmMeasure.Enabled = bIdle && bKin && bPlan && bReal;
            chkArmUseHold.Enabled = txtArmRef.Enabled = bIdle;
            btnArmLearn.Enabled = bIdle && m_bArmPrepared && m_nArmDataRows > 0;
            cmbArmResult.Enabled = btnArmResultBrowse.Enabled = bIdle;
            txtArmTestR.Enabled = txtArmTestYaw.Enabled = txtArmTestPitch.Enabled = txtArmTestRep.Enabled = chkArmTestHold.Enabled = bIdle;
            btnArmTestDry.Enabled = bIdle && bKin && bFc;
            btnArmTest.Enabled = bIdle && bKin && bFc && bReal;
            btnArmApply.Enabled = bIdle && fi != null && !fi.IsActive && !fi.SameAsActive;
            cmbArmBackup.Enabled = bIdle;
            btnArmRollback.Enabled = bIdle && cmbArmBackup.SelectedIndex >= 0 && cmbArmBackup.SelectedIndex < m_lstArmBackups.Count;
            cmbArmTwinSrc.Enabled = bIdle;
            // 중단 — 이 페이지의 작업, 또는 연결을 빌린 작업(⑨ 등)이 도는 동안
            bool bJob = m_bArmRun || (m_ArmConn != null && m_ArmConn.BusyOwner != null && Extra_IsBusy());
            btnArmStop.Enabled = bJob;
            ArmDef_UpdateEnable(bIdle);                              // 로봇 정의 페이지 칸·[확인·저장] (Main.ArmDef.cs)
            Prac_UpdateEnable(bIdle, bReal, bKin, bJob);
        }

        /// <summary>쌍둥이 표시(cmbSagTwin — 이 탭 ⑦ 로 옮김)와 쌍둥이 모델을 lblArmTwin 에 (cmbSagTwin 처리기 끝에서도 부른다)</summary>
        private void ArmSetup_TwinUiSync()
        {
            if (IsDisposed) return;
            if (InvokeRequired) { try { BeginInvoke(new Action(ArmSetup_TwinUiSync)); } catch { } return; }
            int k = (cmbSagTwin != null) ? cmbSagTwin.SelectedIndex : 0;
            Ojw.C3d.CSagComp tw = (m_C3d != null) ? m_C3d.SagTwin_GetModel() : null;
            string strModel;
            if (tw == null) strModel = "쌍둥이 모델 없음 (처짐 모델이 없음)";
            else if (m_strArmTwinInfo != null && object.ReferenceEquals(tw, m_ArmTwinModel)) strModel = m_strArmTwinInfo;
            else strModel = "쌍둥이 = " + (object.ReferenceEquals(tw, m_C3d.SagComp_Get()) ? "적용 중 모델" : "다른 모델") + " (" + Ojw.C3d.SagRun_ModelBrief(tw) + ")";
            string s = ((k <= 0) ? "표시 끔 — 켜면 실물이 있을 자세를 반투명 팔로" : "표시: " + cmbSagTwin.Text) + "\r\n" + strModel;
            ArmSetup_SetLabel(lblArmTwin, s, SystemColors.ControlText);
        }

        // ════════════════════════════════════════════════════════════
        // 설정 (sagarm.txt — DLL CSagArmSetup)
        // ════════════════════════════════════════════════════════════

        /// <summary>sagarm.txt 를 (다시) 읽는다 — bForce, urdf 폴더가 바뀜, 밖에서 고침(파일 시각·크기 — ArmSetup_FileStamp), 앞 읽기가 실패함 가운데 하나면.
        /// 반환 = m_ArmCfg 를 새 설정으로 바꿨다 (이 페이지 칸과 맞출 것 — ArmSetup_SyncCfg, 시작 때는 ArmSetup_PushCfg).
        /// 파일이 없으면 DLL 이 기본값으로 만든다. urdf 폴더가 아직 없으면 만들지 않고 기본값만 (예제를 내보내면 생긴다).
        /// 있는 파일을 읽지 못하면(LoadWarn '★오류' — 잠김 등) m_strArmCfgFail 에 남겨 저장·작업을 막고 다음에 부를 때 다시 읽는다: 같은 폴더면 앞 설정
        /// (이 페이지 칸과 맞춘 값)을 그대로 두고 false, 처음·다른 폴더면 보일 것이 있게 기본값 설정을 쓴다 (DLL 도 그 객체로는 파일을 덮어쓰지 않는다).
        /// LoadWarn 은 로그에 (같은 경고는 한 번)</summary>
        private bool ArmSetup_LoadCfg(bool bForce)
        {
            string strDir, strPath;
            try
            {
                strDir = GetUrdfOutputDir();
                strPath = Path.Combine(strDir, Ojw.C3d.CSagArmSetup.FILE);
            }
            catch (Exception ex)
            {
                // 출력 폴더 칸에 경로로 쓸 수 없는 글자 — 어느 폴더의 설정인지 모르므로 저장·작업을 막는다 (칸을 고치면 다음에 다시 읽는다)
                ArmSetup_CfgFail("★오류: urdf 폴더 이름을 경로로 쓸 수 없음 — " + ex.Message);
                if (m_ArmCfg != null) return false;
                m_ArmCfg = new Ojw.C3d.CSagArmSetup();            // 처음이면 보일 것이 있게 기본값 (저장·작업은 막힌다)
                return true;
            }
            bool bSameDir = m_ArmCfg != null && ArmSetup_SamePath(strDir, m_strArmCfgDir);
            if (!bForce && bSameDir && m_strArmCfgFail == null && ArmSetup_FileStamp(strPath) == m_strArmCfgStamp) return false;
            Ojw.C3d.CSagArmSetup cfg = Directory.Exists(strDir) ? Ojw.C3d.CSagArmSetup.Load(strDir) : new Ojw.C3d.CSagArmSetup();
            string w = (cfg.LoadWarn == null) ? "" : cfg.LoadWarn;
            if (w.StartsWith("★오류"))
            {
                ArmSetup_CfgFail(w);
                if (bSameDir) return false;                   // 앞 설정 그대로 — 이 페이지 칸·메모리를 기본값으로 바꾸지 않는다
            }
            else
            {
                if (w.Length > 0 && w != m_strArmCfgWarnShown) ArmMotor_Log("★" + Ojw.C3d.CSagArmSetup.FILE + " — " + w);
                m_strArmCfgWarnShown = w;
                m_strArmCfgFail = null;
            }
            if (!ArmSetup_SamePath(strDir, m_strArmCfgDir))
            {
                // 다른 urdf 폴더 — 그 폴더의 계획·결과 파일로 (앞 폴더의 고른 파일·작업 결과 줄은 버린다)
                m_lstArmPlan = null; m_strArmPlanPath = null; m_strArmPlanNote = "";
                m_strArmBrowsed = null; m_strArmTestFor = null; m_strArmMeasureLine = null; m_strArmLastRunDir = null;
            }
            m_ArmCfg = cfg;
            m_strArmCfgDir = strDir;
            m_strArmCfgStamp = ArmSetup_FileStamp(strPath);   // DLL 이 기본값으로 만들었으면 그 파일의
            return true;
        }

        /// <summary>읽기 실패를 남긴다 — 저장·작업을 막고 다음에 다시 읽는다. 로그는 같은 까닭이면 한 번</summary>
        private void ArmSetup_CfgFail(string strWhy)
        {
            if (strWhy != m_strArmCfgWarnShown)
                ArmMotor_Log(Ojw.C3d.CSagArmSetup.FILE + " — " + strWhy + " (설정 저장·작업을 하지 않음 — 파일을 닫거나 고치면 다시 읽음)");
            m_strArmCfgWarnShown = strWhy;
            m_strArmCfgFail = strWhy;
        }

        /// <summary>파일 시각·크기 ("" = 없음) — 밖에서 고쳤는지 보는 표 (시각만 보면 시각 단위가 거친 디스크에서 같은 단위 안에 고친 것을 놓친다)</summary>
        private static string ArmSetup_FileStamp(string strPath)
        {
            try
            {
                FileInfo f = new FileInfo(strPath);
                return f.Exists ? f.LastWriteTimeUtc.Ticks.ToString(CultureInfo.InvariantCulture) + "/" + f.Length.ToString(CultureInfo.InvariantCulture) : "";
            }
            catch { return "?"; }
        }

        /// <summary>설정 경고 — 읽지 못함('★오류 …')이 먼저, 아니면 읽을 때의 경고 (범위 밖 값 — 그 칸은 기본값). 없으면 ""</summary>
        private string ArmSetup_CfgWarn()
        {
            if (m_strArmCfgFail != null) return m_strArmCfgFail;
            return (m_ArmCfg != null && m_ArmCfg.LoadWarn != null) ? m_ArmCfg.LoadWarn : "";
        }

        /// <summary>설정을 저장해도 되나 — 마지막 읽기가 실패했으면(있는 파일을 읽지 못함 — '★오류') 저장하지 않는다: 메모리의 설정이 그 파일의 값이 아니다
        /// (사용자의 설정 파일이 기본값·옛 값으로 바뀌지 않게. 처음부터 읽지 못한 기본값 설정은 DLL 의 Save 도 거절).
        /// 파일에 그대로 쓰지 못한 줄이 있어도(LoadWarn — 읽지 못한 줄·범위 밖 값) 저장하지 않는다: DLL 의 Save 는 파일을 통째로 다시 써 그 줄을 기본값으로
        /// 바꾸거나 뺀다 (읽지 못한 계획 세트 planX 는 통째로 빠진다). 사용자가 고치면 다음에 다시 읽는다</summary>
        private bool ArmSetup_CanSaveCfg()
        {
            return m_ArmCfg != null && ArmSetup_CfgWarn().Length == 0;
        }

        /// <summary>sagarm.txt 와 맞춘다 (탭을 볼 때·작업 버튼마다·설정을 쓰기 전 — UI 스레드). 다시 읽었으면(ArmSetup_LoadCfg) 이 페이지 칸에:
        /// 처음·다른 폴더면 전부 (ArmSetup_PushCfg), 같은 폴더면 여기서 고치지 않은 칸만 (앞 설정과 같은 글·체크 — 고친 칸은 누를 때 저장된다).
        /// 고른 결과 파일은 같은 폴더면 이 페이지의 것 그대로 (콤보가 가진다) — 다만 앞 설정이 처음에 읽지 못해 쓴 기본값이었으면 파일의 result 로.
        /// 읽기 경고(읽지 못함·그대로 쓰지 못한 줄)가 생기거나 없어졌으면 맨 아래 줄도 고친다. 반환 = 새 설정을 읽었다</summary>
        private bool ArmSetup_SyncCfg()
        {
            try
            {
                Ojw.C3d.CSagArmSetup old = m_ArmCfg;
                string strOldDir = m_strArmCfgDir, strOldWarn = ArmSetup_CfgWarn();
                bool bOldDefault = old != null && old.LoadWarn != null && old.LoadWarn.StartsWith("★오류");   // 앞 설정 = 처음(다른 폴더)에 읽지 못해 쓴 기본값
                bool bNew = ArmSetup_LoadCfg(false);
                if (bNew)
                {
                    if (old == null || !ArmSetup_SamePath(strOldDir, m_strArmCfgDir)) ArmSetup_PushCfg();
                    else
                    {
                        ArmSetup_PutCfgUi(m_ArmCfg, old);
                        if (bOldDefault) ArmSetup_ResultSelFromCfg();   // 그때 고른 결과 파일은 기본값(적용 중 파일)이었다 — 이제 읽은 파일의 result 로
                        ArmMotor_Log(Ojw.C3d.CSagArmSetup.FILE + " 다시 읽음 (밖에서 고쳤거나 앞에서 읽지 못함) — 이 페이지에서 고친 칸은 그대로, 나머지는 파일 값");
                    }
                }
                if (ArmSetup_CfgWarn() != strOldWarn) ArmSetup_StatusLine();   // 맨 아래 줄을 지금 경고 상태로 (고친 뒤 지난 경고를 남기지 않는다)
                return bNew;
            }
            catch (Exception ex) { ArmMotor_Log(Ojw.C3d.CSagArmSetup.FILE + " 다시 읽기 오류: " + ex.Message); return false; }
        }

        /// <summary>sagarm.txt 에 쓴다 — 먼저 파일과 맞추고(ArmSetup_SyncCfg: DLL 의 Save 는 파일을 통째로 다시 쓴다 — 밖에서 고친 값(계획 세트·창·틈 재기·
        /// 옮기는 높이·경유점 …)을 옛 값으로 덮어쓰지 않게) change 로 바꿀 칸만 바꾼 뒤. strDir = 그 칸이 가리키는 urdf 폴더 — 그사이 폴더가 바뀌었으면
        /// 쓰지 않는다. 읽지 못한 설정·그대로 쓰지 못한 줄이 있는 파일이면(ArmSetup_CanSaveCfg) 메모리만 바꾸고 쓰지 않는다 (이 페이지는 바꾼 값으로 보인다). 반환 = 썼다</summary>
        private bool ArmSetup_SaveCfg(string strDir, Action<Ojw.C3d.CSagArmSetup> change)
        {
            ArmSetup_SyncCfg();
            if (m_ArmCfg == null || m_strArmCfgDir == null) return false;
            if (strDir != null && !ArmSetup_SamePath(strDir, m_strArmCfgDir))
            {
                ArmMotor_Log("★urdf 폴더가 바뀌어 " + Ojw.C3d.CSagArmSetup.FILE + " 에 쓰지 않음 (" + strDir + ")");
                return false;
            }
            if (change != null) change(m_ArmCfg);
            if (!ArmSetup_CanSaveCfg())
            {
                ArmMotor_Log("★" + Ojw.C3d.CSagArmSetup.FILE + ((m_strArmCfgFail != null) ? " 를 읽지 못해" : " 에 그대로 쓰지 못한 줄이 있어 (저장하면 그 줄이 기본값으로 바뀌거나 빠짐)") +
                             " 설정을 저장하지 않음 (" + ArmSetup_CfgWarn() + ") — 파일을 닫거나 고치면 탭을 다시 볼 때·작업 버튼을 누를 때 다시 읽습니다");
                return false;
            }
            bool bOk = false;
            try { bOk = m_ArmCfg.Save(m_strArmCfgDir); }
            catch (Exception ex) { ArmMotor_Log("설정 저장 오류: " + ex.Message); }
            if (!bOk) ArmMotor_Log("★" + Ojw.C3d.CSagArmSetup.FILE + " 저장 실패 (" + m_strArmCfgDir + ")");
            m_strArmCfgStamp = ArmSetup_FileStamp(Path.Combine(m_strArmCfgDir, Ojw.C3d.CSagArmSetup.FILE));
            return bOk;
        }

        /// <summary>설정 → 이 페이지 칸 전부 (시작·다른 폴더). 고른 결과 파일은 설정의 result 로</summary>
        private void ArmSetup_PushCfg()
        {
            Ojw.C3d.CSagArmSetup c = m_ArmCfg;
            if (c == null) return;
            ArmSetup_PutCfgUi(c, null);
            ArmSetup_ResultSelFromCfg();
        }

        /// <summary>고른 결과 파일 ← 설정의 result (비었으면 null — 목록을 채울 때 적용 중 파일을 고른다)</summary>
        private void ArmSetup_ResultSelFromCfg()
        {
            string strRes = (m_ArmCfg.ResultFile == null) ? "" : m_ArmCfg.ResultFile.Trim();
            m_strArmResultSel = (strRes.Length > 0) ? Ojw.C3d.SagRun_Resolve(m_strArmCfgDir, strRes, "") : null;
        }

        /// <summary>설정 c → 이 페이지 칸 (손목 유지·학습 칸·닿기 시험 칸)과 로봇 정의 페이지 칸 (ArmDef_FillUi — 3D 표시도). old 가 있으면 칸이 아직 old 값 그대로일 때만
        /// 바꾼다 — 밖에서 고친 sagarm.txt 를 다시 읽어도 이 페이지에서 고친 칸(누르기 전이라 아직 저장 안 함)은 지우지 않는다</summary>
        private void ArmSetup_PutCfgUi(Ojw.C3d.CSagArmSetup c, Ojw.C3d.CSagArmSetup old)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            m_bArmExpUiSync = true;
            try
            {
                if (old == null || chkArmHold.Checked == old.Measure.WristHold) chkArmHold.Checked = c.Measure.WristHold;
                if (old == null || chkArmUseHold.Checked == old.Learn.UseHold) chkArmUseHold.Checked = c.Learn.UseHold;
                if (old == null || chkArmTestHold.Checked == old.Test.WristHold) chkArmTestHold.Checked = c.Test.WristHold;
                ArmSetup_PutText(txtArmRef, ArmSetup_RefText(c.Learn.RefMm), (old != null) ? ArmSetup_RefText(old.Learn.RefMm) : null);
                ArmSetup_PutText(txtArmTestR, ArmSetup_Join(c.Test.Radii), (old != null) ? ArmSetup_Join(old.Test.Radii) : null);
                ArmSetup_PutText(txtArmTestYaw, ArmSetup_Join(c.Test.Yaws), (old != null) ? ArmSetup_Join(old.Test.Yaws) : null);
                ArmSetup_PutText(txtArmTestPitch, c.Test.Pitch.ToString("0.###", ci), (old != null) ? old.Test.Pitch.ToString("0.###", ci) : null);
                ArmSetup_PutText(txtArmTestRep, c.Test.Repeats.ToString(ci), (old != null) ? old.Test.Repeats.ToString(ci) : null);
            }
            finally { m_bArmExpUiSync = false; }
            ArmDef_FillUi(c.Arm, (old != null) ? old.Arm : null);   // 로봇 정의 페이지 — 같은 규칙 (고친 칸은 그대로), 다시 읽은 정의로 3D 표시
        }

        /// <summary>글 칸에 값 — strOld 가 있으면 칸 글이 그 값과 같을 때만 (고친 글은 그대로)</summary>
        private static void ArmSetup_PutText(TextBox t, string strNew, string strOld)
        {
            if (strOld != null && !ArmSetup_SameText(t.Text, strOld)) return;
            if (t.Text != strNew) t.Text = strNew;
        }

        /// <summary>칸 글이 그 값과 같은가 — 글이 같거나, 둘 다 수(쉼표 목록)로 읽혀 수가 같으면 ("255, 235" = "255,235")</summary>
        private static bool ArmSetup_SameText(string a, string b)
        {
            a = (a == null) ? "" : a.Trim();
            b = (b == null) ? "" : b.Trim();
            if (a == b) return true;
            float[] fa, fb;
            if (!ArmSetup_Floats(a, out fa) || !ArmSetup_Floats(b, out fb) || fa.Length != fb.Length) return false;
            for (int i = 0; i < fa.Length; i++) if (fa[i] != fb[i]) return false;
            return true;
        }

        /// <summary>기준 높이 칸 글 (NaN = 빈칸 — 학습 평균)</summary>
        private static string ArmSetup_RefText(float f)
        {
            return float.IsNaN(f) ? "" : f.ToString("0.###", CultureInfo.InvariantCulture);
        }

        /// <summary>이 페이지의 입력 → 설정 → 확인 → sagarm.txt. 작업 버튼마다 시작 전에 (UI 스레드). false = 시작하지 않음 (까닭은 대화창).
        /// 먼저 파일과 맞춘다 (ArmSetup_SyncCfg). 그때 다시 읽었으면(밖에서 고침·urdf 폴더 바뀜·앞 읽기 실패 뒤) 페이지를 새 설정으로 채우고 시작하지 않는다 —
        /// 확인창에 다 나오지 않는 값(창·틈 재기 기준 …)도 바뀌었을 수 있어 바로 움직이지 않고 다시 누르게. 있는 파일을 읽지 못했으면 시작하지 않는다
        /// (기본값·옛 값으로 실물을 움직이지 않게). 파일에 그대로 쓰지 못한 줄이 있으면(LoadWarn) 그 줄을 알리고 시작하지 않는다 — 시작 때·탭을 볼 때
        /// 읽은 경고도 (ArmSetup_CfgLineWarn — 저장하면 그 줄이 기본값으로 바뀌거나 빠진다).
        /// 수 읽기만 여기서 하고, 값의 범위는 DLL 의 설정 해석(sagarm.txt 규칙)에 맡긴다 (ArmSetup_CheckCfg —
        /// 범위 밖이면 시작하지 않는다). 맞으면 이 페이지 칸만 바꿔 쓴다 (ArmSetup_SaveCfg — 파일의 다른 칸은 방금 읽은 그대로).
        /// 시험 점이 닿는 자리인지(피치 ≤ 90·IK·창)는 시작 전에 DLL 이 다시 본다 (SagRun_TestPoints·SagRun_GapPlan — 확인창에 쓸 점 수와 뺀 까닭)</summary>
        private bool ArmSetup_ReadUi(string strWhat)
        {
            ArmDef_Prepare();                                        // 3D 모델이 바뀌었으면 계획 기구학을 지금 수식으로 (UI 스레드 — 작업 중이면 DLL 이 바꾸지 않는다)
            if (ArmSetup_SyncCfg())
            {
                if (ArmSetup_AnyBusy()) { ArmSetup_RefreshPlan(); ArmSetup_MeasureLabel(); }   // 바쁜 동안은 계획·측정 줄만 (나머지는 탭을 다시 볼 때)
                else ArmSetup_RefreshPage();
                ArmSetup_UpdateEnable();
                if (ArmSetup_CfgWarn().Length == 0)                    // 읽지 못함·그대로 쓰지 못한 줄은 아래 대화창이 알린다
                {
                    ArmMotor_MsgBox(strWhat + " — 시작하지 않았습니다: " + Ojw.C3d.CSagArmSetup.FILE + " 를 다시 읽었습니다 (밖에서 고침·urdf 폴더 바뀜·앞에서 읽지 못함).\r\n" +
                                    "이 페이지에서 고친 칸은 그대로, 나머지는 파일 값입니다 (옮기는 높이·경유점·창·틈 재기 … 도).\r\n\r\n" +
                                    "바뀐 값을 확인한 뒤 다시 누르십시오.", MessageBoxIcon.Information);
                    return false;
                }
            }
            if (m_strArmCfgFail != null) return ArmSetup_CfgFailWarn(strWhat);
            if (ArmSetup_CfgWarn().Length > 0) return ArmSetup_CfgLineWarn(strWhat);   // 그대로 쓰지 못한 줄 — 저장하면 그 줄이 바뀌거나 빠진다
            // 결과·백업 목록을 다른 urdf 폴더에서 읽었으면(바쁜 동안 폴더가 바뀌어 설정만 다시 읽은 경우) 그 목록의 파일로 이 폴더에 시험·계획하지 않는다 —
            // ⑥ 과 같은 규칙: 바쁘면 알리기만, 아니면 이 폴더로 다시 읽고 멈춘다 (바뀐 목록을 본 뒤 다시 누르게)
            if (m_strArmListDir != null && !ArmSetup_SamePath(m_strArmListDir, m_strArmCfgDir)) return ArmSetup_ListDirCheck(strWhat);
            CultureInfo ci = CultureInfo.InvariantCulture;
            float[] afR, afYaw, afPitch, afRef;
            int nRep;
            if (!ArmSetup_Floats(txtArmTestR.Text, out afR))
                return ArmSetup_InputWarn(strWhat, "닿기 시험 반경을 읽지 못했습니다 — 쉼표로 나눈 숫자(mm, 예: 255,235,175). 소수점은 '.'", txtArmTestR);
            if (!ArmSetup_Floats(txtArmTestYaw.Text, out afYaw))
                return ArmSetup_InputWarn(strWhat, "닿기 시험 요를 읽지 못했습니다 — 쉼표로 나눈 숫자(°, 예: 0 또는 0,15,-15)", txtArmTestYaw);
            if (!ArmSetup_Floats(txtArmTestPitch.Text, out afPitch) || afPitch.Length != 1)
                return ArmSetup_InputWarn(strWhat, "공구 피치 Ry 를 숫자 하나로 넣으십시오 (90 = 수직 아래, 90 이하)", txtArmTestPitch);
            if (!int.TryParse(txtArmTestRep.Text.Trim(), NumberStyles.Integer, ci, out nRep))
                return ArmSetup_InputWarn(strWhat, "반복을 정수로 넣으십시오 (예: 2)", txtArmTestRep);
            float fRef = float.NaN;
            if (txtArmRef.Text.Trim().Length > 0)
            {
                if (!ArmSetup_Floats(txtArmRef.Text, out afRef) || afRef.Length != 1)
                    return ArmSetup_InputWarn(strWhat, "기준 높이(mm)를 숫자 하나로 넣거나 비우십시오 (비우면 학습 평균)", txtArmRef);
                fRef = afRef[0];
            }
            bool bTestHold = chkArmTestHold.Checked, bMeasHold = chkArmHold.Checked, bUseHold = chkArmUseHold.Checked;
            string strSel = m_strArmResultSel, strDir = m_strArmCfgDir;
            // 이 페이지가 가진 칸만 — 확인(기본 설정에)·저장(방금 읽은 설정에) 둘 다 이것으로
            Action<Ojw.C3d.CSagArmSetup> put = delegate(Ojw.C3d.CSagArmSetup c)
            {
                c.Test.Radii = (float[])afR.Clone();
                c.Test.Yaws = (float[])afYaw.Clone();
                c.Test.Pitch = afPitch[0];
                c.Test.Repeats = nRep;
                c.Test.WristHold = bTestHold;
                c.Measure.WristHold = bMeasHold;
                c.Learn.UseHold = bUseHold;
                c.Learn.RefMm = fRef;
                if (strSel != null) c.ResultFile = Ojw.C3d.SagRun_Relative(strDir, strSel);
            };
            string w = ArmSetup_CheckCfg(put);
            if (w.Length > 0)
            {
                // 범위 밖 값 — 설정·파일은 그대로 (컨트롤 글도 그대로 두어 고치게), 시작하지 않는다
                ArmMotor_MsgBox(strWhat + " — 입력 값이 설정 범위 밖입니다 (sagarm.txt 와 같은 규칙):\r\n\r\n" + w.Replace(" · ", "\r\n") + "\r\n\r\n고친 뒤 다시 누르십시오.", MessageBoxIcon.Warning);
                return false;
            }
            if (!ArmSetup_SaveCfg(strDir, put) && ArmSetup_CfgWarn().Length > 0)   // 쓰기 직전에 다시 읽으려다 못 읽음·그대로 쓰지 못한 줄이 생김
                return (m_strArmCfgFail != null) ? ArmSetup_CfgFailWarn(strWhat) : ArmSetup_CfgLineWarn(strWhat);
            return true;
        }

        /// <summary>읽지 못한 sagarm.txt — 시작하지 않는다는 대화창 (반환 false)</summary>
        private bool ArmSetup_CfgFailWarn(string strWhat)
        {
            ArmMotor_MsgBox(strWhat + " — 시작하지 않았습니다: " + Ojw.C3d.CSagArmSetup.FILE + " 를 읽지 못했습니다 (기본값·옛 값으로 작업하지 않습니다).\r\n\r\n" +
                            m_strArmCfgFail + "\r\n\r\n파일을 닫거나 고친 뒤 다시 누르십시오.", MessageBoxIcon.Warning);
            return false;
        }

        /// <summary>sagarm.txt 에 그대로 쓰지 못한 줄(LoadWarn — 읽지 못한 줄·범위 밖 값)이 있다 — 그 줄들을 알리고 시작하지 않는다는 대화창 (반환 false).
        /// 저장하면 DLL 이 파일을 통째로 다시 써 그 줄을 기본값으로 바꾸거나 빼므로 저장도 작업도 하지 않고 사용자가 파일을 고치게 한다 (고치면 다시 읽는다)</summary>
        private bool ArmSetup_CfgLineWarn(string strWhat)
        {
            string strPath = (m_strArmCfgDir != null) ? Path.Combine(m_strArmCfgDir, Ojw.C3d.CSagArmSetup.FILE) : Ojw.C3d.CSagArmSetup.FILE;
            ArmMotor_MsgBox(strWhat + " — 시작하지 않았습니다: " + Ojw.C3d.CSagArmSetup.FILE + " 에 그대로 쓰지 못한 줄이 있습니다.\r\n\r\n· " +
                            ArmSetup_CfgWarn().Replace(" · ", "\r\n· ") + "\r\n\r\n" +
                            "지금 저장하면 그 줄이 기본값으로 바뀌거나 빠집니다 (읽지 못한 계획 세트 planX 는 '기본값 유지' 로 나와도 통째로 빠짐) — " +
                            "그래서 설정 저장도 작업도 하지 않습니다.\r\n메모장으로 그 줄을 고친 뒤 다시 누르십시오 (고치면 다시 읽습니다):\r\n" + strPath, MessageBoxIcon.Warning);
            return false;
        }

        /// <summary>이 페이지 값의 범위 확인 — DLL 의 설정 해석으로: 기본 설정에 이 페이지 칸만 넣어(put — 이 칸들의 규칙은 서로 묶이지 않는다) 임시 폴더에
        /// 저장했다 다시 읽어 LoadWarn 을 돌려준다 (없으면 ""). 지금 설정(m_ArmCfg)과 사용자의 sagarm.txt 는 건드리지 않는다.
        /// 저장·읽기를 못 하면 "" (작업 함수들이 값을 다시 거른다)</summary>
        private string ArmSetup_CheckCfg(Action<Ojw.C3d.CSagArmSetup> put)
        {
            try
            {
                string strTmp = Path.Combine(Path.GetTempPath(), "MakeUrdf_sagarm_check");
                Directory.CreateDirectory(strTmp);
                Ojw.C3d.CSagArmSetup c = new Ojw.C3d.CSagArmSetup();
                put(c);
                if (!c.Save(strTmp)) return "";
                Ojw.C3d.CSagArmSetup chk = Ojw.C3d.CSagArmSetup.Load(strTmp);
                string w = (chk.LoadWarn == null) ? "" : chk.LoadWarn;
                return w.StartsWith("★오류") ? "" : w;
            }
            catch { return ""; }
        }

        private bool ArmSetup_InputWarn(string strWhat, string strMsg, Control ctl)
        {
            ArmMotor_MsgBox(strWhat + " — " + strMsg, MessageBoxIcon.Warning);
            if (ctl != null && ctl.Enabled) ctl.Focus();
            return false;
        }

        /// <summary>"255,235,175" → 수 배열 (쉼표로 나눔, 소수점은 '.'). 비었거나 수가 아닌 칸이 있으면 false</summary>
        private static bool ArmSetup_Floats(string s, out float[] a)
        {
            a = null;
            List<float> l = new List<float>();
            foreach (string t in ((s == null) ? "" : s).Split(','))
            {
                string u = t.Trim();
                if (u.Length == 0) continue;
                float f;
                if (!float.TryParse(u, NumberStyles.Float, CultureInfo.InvariantCulture, out f) || float.IsNaN(f) || float.IsInfinity(f)) return false;
                l.Add(f);
            }
            if (l.Count == 0) return false;
            a = l.ToArray();
            return true;
        }

        /// <summary>수 배열 → "255,235,175" (sSep 로 잇는다)</summary>
        private static string ArmSetup_Join(float[] a) { return ArmSetup_Join(a, ",", "0.###"); }
        private static string ArmSetup_Join(float[] a, string sSep, string sFmt)
        {
            if (a == null) return "";
            string[] s = new string[a.Length];
            for (int i = 0; i < a.Length; i++) s[i] = a[i].ToString(sFmt, CultureInfo.InvariantCulture);
            return string.Join(sSep, s);
        }

        private static bool ArmSetup_SamePath(string a, string b)
        {
            if (a == null || b == null) return a == b;
            try { return string.Equals(Path.GetFullPath(a), Path.GetFullPath(b), StringComparison.OrdinalIgnoreCase); }
            catch { return string.Equals(a, b, StringComparison.OrdinalIgnoreCase); }
        }

        // ════════════════════════════════════════════════════════════
        // 라벨 (그룹마다 결과 한 줄 — 잘려도 마우스를 올리면 전부)
        // ════════════════════════════════════════════════════════════

        private void ArmSetup_SetLabel(Label lbl, string s, Color c)
        {
            if (s == null) s = "";
            if (lbl.Text != s) lbl.Text = s;
            if (lbl.ForeColor != c) lbl.ForeColor = c;
            tipArmExp.SetToolTip(lbl, s);
        }

        /// <summary>페이지 맨 아래 줄 (lblArmBusy) — ★ 로 시작하면 빨강</summary>
        private void ArmSetup_Busy(string s)
        {
            if (IsDisposed) return;
            m_bArmBusyWarn = false;
            ArmSetup_SetLabel(lblArmBusy, s, (s != null && s.StartsWith("★")) ? Color.Firebrick : SystemColors.ControlText);
        }

        /// <summary>맨 아래 줄에 탭 상태 경고 (sagarm.txt 읽지 못함·그대로 쓰지 못한 줄 — 둘 다 저장·작업을 막아 먼저, 계획 기구학 없음·로봇 정의가 3D 와 맞지 않음 — 종류별 안내) —
        /// 없으면 이전 경고만 지운다 (작업 결과 줄은 그대로)</summary>
        private void ArmSetup_StatusLine()
        {
            string s = "";
            if (m_strArmCfgFail != null) s = "★" + Ojw.C3d.CSagArmSetup.FILE + ": " + m_strArmCfgFail;        // 읽지 못함 — 저장·작업이 막힌다 (먼저 보인다)
            else if (ArmSetup_CfgWarn().Length > 0) s = "★" + Ojw.C3d.CSagArmSetup.FILE + ": " + ArmSetup_CfgWarn();    // 그대로 쓰지 못한 줄 — 고칠 때까지 저장·작업이 막힌다
            else if (!m_bArmPrepared) s = "★계획 기구학 없음 — 계획·학습·시험을 할 수 없음 (앱을 다시 켜십시오)";
            else if (!m_bArmKinOk && m_strArmKinWhy.Length > 0) s = "★" + ArmDef_WhyHint(m_strArmKinWhy, m_eArmKinKind) + " (미리보기·측정·시험 막힘)";   // 확인 전(까닭 없음)이면 쓰지 않는다 — 안내는 한 번만
            if (s.Length > 0) { ArmSetup_Busy(s); m_bArmBusyWarn = true; }
            else if (m_bArmBusyWarn) ArmSetup_Busy("대기");
        }

        /// <summary>계획을 (다시) 읽는다 (DLL SagRun_PlanParse) — 설정의 plan 이 가리키는 파일이 지금 계획과 다르거나, 그 파일이 지금 행을 읽은(만든) 뒤
        /// 바뀌었거나(시각·크기 — 메모장·엑셀로 고침), 앞에서 읽지 못했을 때. 반환 = 파일을 다시 읽었다 (미리보기·측정 버튼은 그때 시작하지 않는다 — ArmSetup_PlanCheck)</summary>
        private bool ArmSetup_RefreshPlan()
        {
            bool bRead = false;
            string strPlan = (m_ArmCfg.PlanFile == null) ? "" : m_ArmCfg.PlanFile.Trim();
            string strWant = (strPlan.Length > 0) ? Ojw.C3d.SagRun_Resolve(m_strArmCfgDir, strPlan, "") : null;
            if (strWant == null) { m_lstArmPlan = null; m_strArmPlanPath = null; m_strArmPlanWhy = ""; m_strArmPlanNote = ""; }
            else if (m_lstArmPlan == null || !ArmSetup_SamePath(strWant, m_strArmPlanPath) || ArmSetup_FileStamp(strWant) != m_strArmPlanStamp)
            {
                bRead = true;
                m_strArmPlanPath = strWant; m_lstArmPlan = null; m_strArmPlanWhy = ""; m_strArmPlanNote = "";
                m_strArmPlanStamp = ArmSetup_FileStamp(strWant);      // 읽기 전에 — 읽는 사이 고치면 다음에 다시 읽는다
                try
                {
                    if (!File.Exists(strWant)) m_strArmPlanWhy = "파일 없음";
                    else
                    {
                        string why;
                        m_lstArmPlan = Ojw.C3d.SagRun_PlanParse(File.ReadAllText(strWant), out why);
                        m_strArmPlanWhy = why;
                    }
                }
                catch (Exception ex) { m_lstArmPlan = null; m_strArmPlanWhy = ex.Message; }
            }
            ArmSetup_PlanLabel();
            return bRead;
        }

        /// <summary>계획에서 측정할 행 수 (IK 확인 통과 행)</summary>
        private int ArmSetup_PlanRows()
        {
            int n = 0;
            if (m_lstArmPlan != null) foreach (Ojw.C3d.CFloorPlanRow r in m_lstArmPlan) if (r != null && r.IkOk) n++;
            return n;
        }

        /// <summary>미리보기·측정 버튼 — 계획 파일과 맞춘다 (ArmSetup_RefreshPlan). 지금 행을 읽은(만든) 뒤 파일이 바뀌었거나 앞에서 읽지 못해 다시 읽었으면 ② 라벨을
        /// 새 행으로 고치고 시작하지 않는다 — 점 수·시간이 바뀌었을 수 있어 본 뒤 다시 누르게 (sagarm.txt 다시 읽기와 같은 규칙). 계획 파일을 읽지 못하면(없음·잠김·형식)
        /// 까닭과 함께 시작하지 않는다 (메모리의 옛 행으로 누르지 않는다). 그래도 두 버튼은 켜 둔다 (ArmSetup_UpdateEnable) — 고친 뒤 다시 누르면 여기서 다시 읽는다.
        /// true = 지금 행이 파일 그대로</summary>
        private bool ArmSetup_PlanCheck(string strWhat)
        {
            if (!ArmSetup_RefreshPlan()) return true;
            ArmSetup_UpdateEnable();
            if (m_lstArmPlan == null)
                ArmMotor_MsgBox(strWhat + " — 시작하지 않았습니다: 계획 파일을 읽지 못했습니다 (" + m_strArmPlanWhy + ").\r\n" + m_strArmPlanPath +
                                "\r\n\r\n파일을 닫거나(엑셀 등) 고친 뒤 다시 누르십시오. 새로 만들려면 [계획 만들기].", MessageBoxIcon.Warning);
            else
            {
                bool bNone = ArmSetup_PlanRows() == 0;          // 읽었지만 측정할 행이 없다 (모두 ik=0·창 없음 …) — 다시 눌러도 시작하지 않는다
                ArmMotor_MsgBox(strWhat + " — 시작하지 않았습니다: 계획 파일을 다시 읽었습니다 (밖에서 고쳤거나 앞에서 읽지 못함).\r\n\r\n" + lblArmPlan.Text +
                                ((m_strArmPlanWhy.Length > 0) ? "\r\n★" + m_strArmPlanWhy + " (그 행은 측정하지 않음)" : "") +   // 형식이 틀린 행 — DLL 이 건너뛴다
                                (bNone ? "\r\n\r\n측정할 행이 없습니다. 계획 파일을 고친 뒤 다시 누르거나 [계획 만들기]."
                                       : "\r\n\r\n바뀐 점 수·시간을 확인한 뒤 다시 누르십시오."), bNone ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
            }
            return false;
        }

        /// <summary>② 라벨 — "90점 (제외 3) · 8줄 · 약 33분 / plan_….csv / 마지막 3D 미리보기"</summary>
        private void ArmSetup_PlanLabel()
        {
            string s;
            Color c = SystemColors.ControlText;
            if (m_lstArmPlan == null)
            {
                if (m_strArmPlanPath == null) s = "계획 없음 — [계획 만들기] (첫닿음 모델 예측으로 점마다 하강 창)";
                else { s = "★계획을 읽지 못함 — " + m_strArmPlanWhy + "\r\n" + Path.GetFileName(m_strArmPlanPath); c = Color.Firebrick; }
            }
            else
            {
                int nOk = 0, nBad = 0;
                List<string> lines = new List<string>();
                foreach (Ojw.C3d.CFloorPlanRow r in m_lstArmPlan)
                {
                    if (r == null) continue;
                    if (!r.IkOk) { nBad++; continue; }
                    nOk++;
                    if (!lines.Contains(r.LineKey)) lines.Add(r.LineKey);
                }
                double sec = (m_ArmCfg != null) ? Ojw.C3d.SagRun_EstimateSec(m_lstArmPlan, m_ArmCfg.Measure.Probe, m_ArmCfg.Arm) : 0;
                s = string.Format(CultureInfo.InvariantCulture, "{0}점{1} · {2}줄 · 약 {3:0}분", nOk, (nBad > 0) ? " (제외 " + nBad + ")" : "", lines.Count, sec / 60.0) +
                    "\r\n" + Path.GetFileName(m_strArmPlanPath) + ((m_strArmPlanNote.Length > 0) ? "\r\n" + m_strArmPlanNote : "");
                if (nOk == 0) { s = "★측정할 행 없음 — " + s; c = Color.Firebrick; }
                else if (m_strArmPlanNote.StartsWith("★")) c = Color.Firebrick;
            }
            ArmSetup_SetLabel(lblArmPlan, s, c);
        }

        /// <summary>결과 파일 목록 (DLL SagRun_ListResults — [0] = 적용 중 파일, 이어서 결과 폴더 최신순) + [ … ] 로 고른 다른 파일.
        /// 고르는 것: 기억한 선택 → 설정의 result → 첫 항목 (적용 중 파일)</summary>
        private void ArmSetup_FillResults()
        {
            List<Ojw.C3d.CSagFileInfo> lst = Ojw.C3d.SagRun_ListResults(m_strArmCfgDir);
            m_strArmListDir = m_strArmCfgDir;                       // ⑥ 적용은 이 폴더에만 (ArmSetup_ListDirCheck)
            if (m_strArmBrowsed != null && File.Exists(m_strArmBrowsed))
            {
                bool bIn = false;
                foreach (Ojw.C3d.CSagFileInfo f in lst) if (ArmSetup_SamePath(f.Path, m_strArmBrowsed)) bIn = true;
                if (!bIn) lst.Add(ArmSetup_OtherFileInfo(m_strArmBrowsed));
            }
            int nSel = -1;
            if (m_strArmResultSel != null)
                for (int i = 0; i < lst.Count && nSel < 0; i++) if (ArmSetup_SamePath(lst[i].Path, m_strArmResultSel)) nSel = i;
            if (nSel < 0 && lst.Count > 0) nSel = 0;
            m_bArmExpUiSync = true;
            cmbArmResult.BeginUpdate();
            try
            {
                cmbArmResult.Items.Clear();
                foreach (Ojw.C3d.CSagFileInfo f in lst) cmbArmResult.Items.Add(f.Display);
                m_lstArmResults.Clear();
                m_lstArmResults.AddRange(lst);
                cmbArmResult.SelectedIndex = nSel;
            }
            finally { cmbArmResult.EndUpdate(); m_bArmExpUiSync = false; }
            m_strArmResultSel = (nSel >= 0) ? lst[nSel].Path : null;
            Ojw.C3d.CSagFileInfo sel = ArmSetup_SelectedResult();
            tipArmExp.SetToolTip(cmbArmResult, (sel != null) ? sel.Path + "\r\n" + sel.Display : "결과 파일 없음 — ④ 학습");
        }

        /// <summary>결과 폴더 밖에서 고른 파일의 목록 항목 — 모델 요약은 DLL (SagRun_ModelBrief), 시험 기록 요약은 목록 함수만 만든다</summary>
        private static Ojw.C3d.CSagFileInfo ArmSetup_OtherFileInfo(string strPath)
        {
            Ojw.C3d.CSagFileInfo fi = new Ojw.C3d.CSagFileInfo();
            fi.Path = strPath;
            fi.Name = Path.GetFileName(strPath);
            Ojw.C3d.CSagComp m = null;
            try { m = Ojw.C3d.CSagComp.Load(strPath); } catch { m = null; }
            fi.Brief = Ojw.C3d.SagRun_ModelBrief(m);
            if (m != null) { fi.MotorMap = m.MotorMap; fi.NFc = m.HasFc ? m.Fc.Count : 0; }
            fi.TestState = "목록 밖 파일 — 시험 기록 요약 없음";
            fi.Display = "다른 파일: " + fi.Name + " · " + fi.Brief;
            return fi;
        }

        private Ojw.C3d.CSagFileInfo ArmSetup_SelectedResult()
        {
            int k = cmbArmResult.SelectedIndex;
            return (k >= 0 && k < m_lstArmResults.Count) ? m_lstArmResults[k] : null;
        }

        /// <summary>백업 목록 (DLL SagRun_ListBackups — 최신순)</summary>
        private void ArmSetup_FillBackups()
        {
            List<Ojw.C3d.CSagFileInfo> lst = Ojw.C3d.SagRun_ListBackups(m_strArmCfgDir);
            m_strArmListDir = m_strArmCfgDir;                       // ⑥ 되돌리기는 이 폴더에만 (ArmSetup_ListDirCheck)
            m_bArmExpUiSync = true;
            cmbArmBackup.BeginUpdate();
            try
            {
                cmbArmBackup.Items.Clear();
                foreach (Ojw.C3d.CSagFileInfo f in lst) cmbArmBackup.Items.Add(f.Display);
                m_lstArmBackups.Clear();
                m_lstArmBackups.AddRange(lst);
                cmbArmBackup.SelectedIndex = (lst.Count > 0) ? 0 : -1;
            }
            finally { cmbArmBackup.EndUpdate(); m_bArmExpUiSync = false; }
            tipArmExp.SetToolTip(cmbArmBackup, (lst.Count > 0) ? "백업 " + lst.Count + "개 — sagcomp_backup (적용·되돌리기 때마다 이전 파일)" : "백업 없음 — 적용하면 이전 파일이 여기로");
        }

        /// <summary>④ 데이터셋 줄 (DLL SagRun_DatasetInfo — 학습이 읽을 파일)</summary>
        private void ArmSetup_RefreshData()
        {
            string strDs = ArmSetup_DatasetPath();
            string strInfo;
            m_nArmDataRows = Ojw.C3d.SagRun_DatasetInfo(strDs, out strInfo);
            ArmSetup_SetLabel(lblArmDataInfo, strInfo, (m_nArmDataRows > 0) ? SystemColors.ControlText : Color.Firebrick);
        }

        private string ArmSetup_DatasetPath()
        {
            return Ojw.C3d.SagRun_Resolve(m_strArmCfgDir, (m_ArmCfg != null) ? m_ArmCfg.Learn.DatasetPath : null, Ojw.C3d.SAGRUN_FC_DATASET);
        }

        /// <summary>⑥ 라벨 — 적용 중 파일 (목록 [0]) 요약·시험 기록·모터 매핑 차이 (C10). 전체 모델 요약은 마우스를 올리면</summary>
        private void ArmSetup_RefreshActive()
        {
            Ojw.C3d.CSagFileInfo fa = null;
            foreach (Ojw.C3d.CSagFileInfo f in m_lstArmResults) if (f.IsActive) { fa = f; break; }
            Ojw.C3d.CSagComp m = (m_C3d != null) ? m_C3d.SagComp_Get() : null;
            string s;
            Color c = Color.DarkBlue;
            if (fa == null) s = "적용 중인 처짐 모델 파일 없음 (" + SAGCOMP_MODEL + ")" + ((m != null) ? " — 메모리: " + Ojw.C3d.SagRun_ModelBrief(m) : "");
            else
            {
                s = "적용 중: " + fa.Name + " · " + fa.Brief + ((m != null && !m.Enabled) ? " (보정 끔)" : "") + " · 시험: " + fa.TestState;
                string d = ArmSetup_MapDiff(fa.MotorMap);
                if (d.Length > 0) { s += "\r\n★학습 때와 방향·영점이 다름 (" + d + ") — 다시 학습 권장"; c = Color.Firebrick; }
            }
            ArmSetup_SetLabel(lblArmActive, s, c);
            if (m != null) tipArmExp.SetToolTip(lblArmActive, s + "\r\n\r\n" + m.Summary());
        }

        /// <summary>⑤ 라벨 — 고른 결과 파일 요약·시험 기록 (bForce 가 아니면 같은 파일의 작업 결과 줄은 그대로 둔다)</summary>
        private void ArmSetup_TestLabel(bool bForce)
        {
            Ojw.C3d.CSagFileInfo fi = ArmSetup_SelectedResult();
            string strPath = (fi != null) ? fi.Path : null;
            if (!bForce && m_strArmTestFor != null && ArmSetup_SamePath(strPath, m_strArmTestFor)) return;
            m_strArmTestFor = strPath;
            string s;
            Color c = Color.DarkGreen;
            if (fi == null) s = "결과 파일 없음 — ④ 학습으로 만들거나 [ … ] 로 고르십시오";
            else
            {
                s = "고른 파일: " + fi.Name + " · " + fi.Brief + " · 시험: " + fi.TestState;
                if (fi.NFc <= 0) { s += "\r\n★첫닿음 모델이 없는 파일 — 닿기 시험은 ④ 학습 결과 파일로"; c = Color.Firebrick; }
                else
                {
                    string d = ArmSetup_MapDiff(fi.MotorMap);
                    if (d.Length > 0) { s += "\r\n★학습 때와 방향·영점이 다름 (" + d + ") — 다시 학습 권장"; c = Color.Firebrick; }
                }
            }
            ArmSetup_SetLabel(lblArmTest, s, c);
        }

        /// <summary>③ 라벨 — 데이터가 쌓일 곳 (손목 유지 켬/끔 → hold 칸) + 마지막 측정 결과 줄</summary>
        private void ArmSetup_MeasureLabel()
        {
            string strDs = Ojw.C3d.SagRun_Resolve(m_strArmCfgDir, (m_ArmCfg != null) ? m_ArmCfg.Measure.DatasetPath : null, Ojw.C3d.SAGRUN_FC_DATASET);
            string s = "손목 유지 " + (chkArmHold.Checked ? "켬" : "끔") + " → " + Path.GetFileName(strDs) + " 에 덧붙임 (hold=" + (chkArmHold.Checked ? "1" : "0") + ")";
            Color c = SystemColors.ControlText;
            if (m_strArmMeasureLine != null)
            {
                s = m_strArmMeasureLine + "\r\n" + s;
                if (m_strArmMeasureLine.IndexOf('★') >= 0) c = Color.Firebrick;
            }
            ArmSetup_SetLabel(lblArmMeasure, s, c);
        }

        /// <summary>파일의 모터 매핑 키와 지금 연결(적용된 세트)의 차이 문구 (DLL SagRun_MotorMapDiff — 같거나 비교할 수 없으면 "")</summary>
        private string ArmSetup_MapDiff(string strFileKey)
        {
            string strNow = ArmSetup_MapKey();
            return (strNow == null) ? "" : Ojw.C3d.SagRun_MotorMapDiff(strFileKey, strNow);
        }

        /// <summary>지금 적용된 모터 세트의 매핑 키 (학습 결과에 적는다 — C10). 세트가 없으면 null (DLL 은 motormap 줄을 쓰지 않는다 = 기본 매핑)</summary>
        private string ArmSetup_MapKey()
        {
            Ojw.CArmMotorSet_t set = (m_ArmConn != null) ? m_ArmConn.Motors : null;
            return (set != null) ? set.MappingKey() : null;
        }

        /// <summary>시험 기록 요약에 실물 판정(합격·불합격·판정 불가)이 있나 — 적용 확인창 문구용</summary>
        private static bool ArmSetup_HasRealVerdict(string strTestState)
        {
            return strTestState != null && (strTestState.IndexOf("합격") >= 0 || strTestState.IndexOf("판정 불가") >= 0);
        }

        // ════════════════════════════════════════════════════════════
        // 잠금 — 작업·조그가 돌거나 연결이 사용 중일 때
        // ════════════════════════════════════════════════════════════

        /// <summary>잠금이 필요한지 다시 보고 바뀌었으면 건다·푼다 (작업 시작·끝, 연결 StateChanged 마다, 측정 스레드가 끝날 때 — Extra_AfterRun).
        /// 잠그는 때: 이 페이지의 작업 · 이 탭의 동작(연결·조그 …) · 연결 잠금 소유자 · 연결 중인데 측정 스레드(⑨ 가 연결을 빌림)나 3D 모션이 돌 때.
        /// 연결 없이 도는 예전 측정(포트를 직접 여는 ⑨ 등)은 잠그지 않는다 (예전 동작 그대로 — 그 3D 모션은 이 탭의 연결과 상관없다)</summary>
        private void ArmSetup_RefreshLock()
        {
            if (IsDisposed) return;
            ArmMotor_ClearStaleMotion();                                                  // 진행시킬 곳 없이 남은 3D 모션 표시면 지운다
            bool bConn = m_ArmConn != null && m_ArmConn.IsConnected;
            bool bNeed = m_bArmRun || m_bArmMotorRun ||
                         (m_ArmConn != null && m_ArmConn.BusyOwner != null) ||
                         (bConn && (Extra_IsBusy() || (m_C3d != null && m_C3d.MoveJoints_IsMoving())));
            if (bNeed != m_bArmLocked) ArmSetup_Lock(bNeed);
        }

        /// <summary>작업 중 잠금. 작업·조그가 실물로 보낼 때(SendPose)는 움직이지 않는 축도 지금 3D 값을 보낸다 — 그동안 3D 를 바꾸는 조작
        /// (Control 탭 관절, 마우스 관절 끌기, 재생, 예제·DH 편집, 내보내기·보기 설정의 DH 다시 해석)이 실물로 나가지 않게 막는다.
        /// DH 다시 해석(MakeAll·UpdatePreview·내보내기)은 잠금 중 어디서 불려도 하지 않고 미뤘다가 풀릴 때 한 번 한다 —
        /// 다시 해석하는 동안 작업 스레드가 쓰는 기구학이 비었다 채워진다 (3D 클릭·끌어 놓기·면 맞춤 모드 등 버튼 밖의 길도 같은 곳에서 막힌다).
        /// 다른 탭은 이 탭과 Log 탭만 고를 수 있다 (tabRight_Selecting). 풀면 잠그기 전 상태로 되돌린다</summary>
        private void ArmSetup_Lock(bool bRun)
        {
            if (bRun == m_bArmLocked) return;
            m_bArmLocked = bRun;                                                          // 먼저 — 아래 모드 끄기의 3D 다시 그리기도 미뤄진다
            if (bRun)
            {
                if (m_bPlaying) btnPause_Click(this, EventArgs.Empty);                   // 재생 타이머가 3D 각을 바꾸면 다음 스텝이 그 값을 실물로 보낸다
                if (!rdMouseCam.Checked) rdMouseCam.Checked = true;                       // 마우스 관절 끌기 끔 (카메라)
                if (m_C3d != null) m_C3d.SetMouseMode(0);
                m_eMode = InteractionMode.Control;                                        // 3D 마우스로 DH 를 바꾸는 만들기 모드 끔 (만들기 탭으로 돌아가면 되살린다)
                if (m_bDragging) CancelDrag();                                            // 끌던 부품은 원래 자리로
                m_bPickWaitForResult = false;                                             // 직전 3D 클릭의 '허공 클릭 → 선택 해제'(IK 선택 번호를 잠깐 -1 로) 대기 끔
                m_eCurrentAction = Ojw.BuildAction.None;
                if (m_bFaceSnapMode) ExitFaceSnapMode("로봇팔 작업 중 — Face Snap 끔");   // 면 맞춤·축 정렬 모드는 3D 위 마우스만으로 DH 를 바꾼다
                if (m_bMergeMode) ExitMergeMode();
                if (m_eSceneMode != SceneToolMode.None) CancelSceneMode();
                m_dicArmLockSaved.Clear();
                foreach (Control c in ArmSetup_LockControls())
                {
                    if (m_dicArmLockSaved.ContainsKey(c)) continue;
                    m_dicArmLockSaved[c] = c.Enabled;
                    c.Enabled = false;
                }
                m_bArmDhReadOnlySaved = txtDhParams.ReadOnly;
                txtDhParams.ReadOnly = true;                                              // DH 편집 (다시 해석하면 3D 리밋이 바뀐다)
            }
            else
            {
                foreach (KeyValuePair<Control, bool> kv in m_dicArmLockSaved)
                    if (!kv.Key.IsDisposed) kv.Key.Enabled = kv.Value || kv.Key.Enabled;   // 잠근 사이 다른 곳이 켠 것은 켠 채로
                m_dicArmLockSaved.Clear();
                txtDhParams.ReadOnly = m_bArmDhReadOnlySaved;
                if (tabRight.SelectedTab == tabCreate) rbMode();                          // 만들기 탭에 있었으면 그 탭의 마우스 모드로
                if (m_bArmPreviewPending || m_strArmRebuildDh != null)
                {
                    try { BeginInvoke(new Action(ArmSetup_ReplayRebuild)); }              // 미룬 3D 다시 만들기 — 이 호출(상태 갱신)이 끝난 뒤에
                    catch { }
                }
            }
            SagComp_UiSync();                                                             // 처짐 보정 체크도 (작업 중엔 끔)
            ArmSetup_UpdateEnable();
        }

        /// <summary>잠금 중 미룬 3D 다시 만들기를 한 번 (잠금이 풀린 뒤). 미리보기(UpdatePreview — 빌더에서 다시 만든다)가 미뤄졌으면 그것을,
        /// 아니면 마지막으로 미룬 MakeAll 의 DH 로. 그사이 다시 잠겼으면 다음에 풀릴 때 한다</summary>
        private void ArmSetup_ReplayRebuild()
        {
            if (m_bArmLocked || IsDisposed) return;
            bool bPreview = m_bArmPreviewPending;
            string strDh = m_strArmRebuildDh;
            m_bArmPreviewPending = false;
            m_strArmRebuildDh = null;
            try
            {
                if (bPreview) UpdatePreview(null);
                else if (strDh != null) MakeAll(strDh);
            }
            catch (Exception ex) { ArmMotor_Log("미룬 3D 다시 그리기 오류: " + ex.Message); }
        }

        /// <summary>잠글 컨트롤 — 예제·출력 폴더·마우스 관절·DH 편집·내보내기·보기 설정(MakeAll 을 다시 부른다) + 이 탭과 Log 탭을 뺀 탭 페이지</summary>
        private List<Control> ArmSetup_LockControls()
        {
            List<Control> lst = new List<Control>(new Control[] {
                cmbExample, txtUrdfFolder, rdMouseJoint, btnApplyDh, pnDhEdit,
                btnExportUrdf, btnViewMjcf, btnViewUsd, btnIsaacUsd, btnUsdExport, btnPyBullet, btnMuJoCo, btnIsaac,
                btnCaptureAll, btnExportConfig, btnSim2Real,
                cmbViewMode, chkShowArrow, txtArrowSize, chkShowBall, txtBallSize, chkShowSphere, txtSphereSize,
                txtSkeletonSize, txtAxisSize });
            foreach (TabPage p in tabRight.TabPages)
                if (p != tabArmSetup && p != tabHistory) lst.Add(p);
            return lst;
        }

        /// <summary>잠금 중에는 이 탭과 Log 탭만 고를 수 있다</summary>
        private void tabRight_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (m_bArmLocked && e.TabPage != tabArmSetup && e.TabPage != tabHistory) e.Cancel = true;
        }

        /// <summary>창 닫기 (Main_FormClosing 맨 앞) — 측정 작업이 돌면 닫지 않고 중단을 요청한다.
        /// 작업 스레드를 기다리지 않는다 (S2R_Log 가 UI 로 동기 Invoke 라 기다리면 서로 멈춘다)</summary>
        private bool ArmSetup_BlockClose(FormClosingEventArgs e)
        {
            if (!m_bArmRun && !Extra_IsBusy()) return false;
            e.Cancel = true;
            m_bMechStop = true;
            try { S2R_Log("창 닫기 요청 — 작업을 멈추는 중. 원위치·토크 끄기가 끝난 뒤에 닫으십시오"); } catch { }
            MessageBox.Show(this, "로봇팔 작업을 멈추는 중입니다.\r\n원위치·토크 끄기가 끝나면(로그 창 '닫기') 다시 닫으십시오.", "로봇팔 세팅",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return true;
        }

        // ════════════════════════════════════════════════════════════
        // 작업 띄우기
        // ════════════════════════════════════════════════════════════

        /// <summary>작업 하나를 Extra_Run 스레드로 — 잠그고, 로그 창을 띄우고, 끝나면(UI 스레드) 잠금을 풀고 after() → 목록 다시 읽기.
        /// work 는 작업 스레드에서 돈다: 결과는 지역 변수(·m_strArmLast)에 남기기만 하고 컨트롤·설정은 건드리지 않는다. 경로·설정은 UI 스레드에서 미리 잡아 넘긴다.
        /// after() 가 설정 칸을 쓰면(계획·결과 파일) ArmSetup_SaveCfg 가 먼저 sagarm.txt 와 맞춘다 — 작업 중 밖에서 고친 값을 옛 값으로 덮어쓰지 않는다.
        /// work 의 반환 = 로그 창 끝 줄 (null = 중단/실패). bRobot = 실물 작업 (완료 문구만 다르다 — 원위치·토크 결과는 작업 로그의 '원위치 —' 줄).
        /// 실물 작업이 처짐 보정 모델을 바꿔 가며 써도 끝에 남는 것은 DLL 이 정리하고(시작 때 토크 ON 이면 보정 전환을 끝까지 붙잡아 마친다),
        /// 조그 막음은 출력 단 상태를 바로 본다 — 여기서 따로 표시하지 않는다. 다른 작업이 돌면 false</summary>
        private bool ArmSetup_Run(string strTitle, bool bRobot, Func<string> work, Action after)
        {
            if (work == null) return false;
            if (m_bArmRun || m_bArmMotorRun) { ArmMotor_MsgBox("로봇팔 작업이 이미 진행 중입니다.", MessageBoxIcon.Information); return false; }
            if (Extra_Busy(true)) return false;                  // 로봇팔 연결은 빌려 쓰므로 연결 중이어도 된다
            m_bArmRun = true;
            m_strArmLast = "";
            ArmSetup_RefreshLock();
            ArmSetup_Busy("실행 중: " + strTitle);
            Extra_Run(strTitle, work, delegate(string strTail)
            {
                m_bArmRun = false;
                ArmSetup_RefreshLock();
                try { if (after != null) after(); }
                catch (Exception ex) { ArmMotor_Log(strTitle + " 결과 처리 오류: " + ex.Message); }
                ArmSetup_OnShown();
                SagComp_UiSync();
                ArmMotor_UpdateUi();
                string strLast = m_strArmLast;
                ArmSetup_Busy("대기 — 마지막: " + strTitle + ((strTail != null) ? " 완료" : " 중단/실패") +
                              ((strLast != null && strLast.Length > 0) ? " · " + strLast : ""));
            }, bRobot ? "   작업 끝 — 원위치·토크는 위 '원위치 —' 줄" : "   작업 완료");
            return true;
        }

        /// <summary>실물 작업 시작 전 확인 (U4) — 연결이 없거나 구성이 바뀌었으면 거부, 작업이 쓰는 축 가운데 방향 확인 안 된 축(DLL SagArm_UncheckedAxes)·
        /// 저장 안 된 모터 세팅이 있으면 경고만(계속/취소). 확인창(실물이 움직입니다) 앞에서 부른다</summary>
        private bool ArmSetup_CheckStart(string strTitle, Ojw.C3d.CSagArmCfg arm)
        {
            if (m_ArmConn == null || !m_ArmConn.IsConnected)
            { ArmMotor_MsgBox(strTitle + " — 로봇팔 세팅 탭에서 먼저 연결하십시오.", MessageBoxIcon.Information); return false; }
            if (m_ArmConn.ConfigMismatch)
            { ArmMotor_MsgBox(strTitle + " — 모터 구성이 연결 뒤 바뀌었습니다. 다시 연결하십시오.", MessageBoxIcon.Information); return false; }
            int[] anUnchecked = Ojw.C3d.SagArm_UncheckedAxes(m_ArmConn, arm);
            bool bDirty = ArmMotor_ComputeDirty();
            if (anUnchecked.Length == 0 && !bDirty) return true;
            string s = strTitle + " — 시작 전에 확인하십시오\r\n\r\n";
            if (anUnchecked.Length > 0) s += "· 방향 확인 안 된 축: ID " + Ojw.CArmMotorSet_t.RangeText(anUnchecked) + " ('모터 세팅' 표의 '확인' — 조그로 방향을 본 뒤)\r\n";
            if (bDirty) s += "· 저장 안 된 모터 세팅이 있습니다 ('모터 세팅' 탭의 '저장')\r\n";
            s += "\r\n그래도 시작할까요?";
            return ArmMotor_Confirm(s, "로봇팔 세팅");
        }

        /// <summary>⑥ 적용·되돌리기 시작 전 — 지금 무언가 실물·3D 로 보내는 중이면(작업·조그·⑨·3D 모션 — ArmSetup_AnyBusy) 거부한다: 적용은 적용 중 처짐
        /// 모델을 바꿔 끼우는데, 도는 동작이 그 모델로 보내고 있다 (작업은 DLL 도 거절). true = 바꿔도 된다.
        /// 로봇팔이 토크를 붙잡고 있는 것만으로는 막지 않는다 (10-03 다시 봄): 보정 전환이 출력 틱 기준이라 바꾼 뒤 다음 틱은 걸려 있던 보정 그대로 이어지고
        /// (토크를 붙잡은 채 모델 바꿈 → 첫 틱 튐 0), 새 보정은 다음 동작의 출력 틱 1초에 걸쳐 들어온다. 보정이 켜져 있는 동안 조그는 따로 막힌다
        /// (Main.ArmMotor.cs ArmMotor_CompBlockReason). 그 사실은 확인창에 적는다 (ArmSetup_SwitchNote)</summary>
        private bool ArmSetup_CanSwitchComp(string strTitle)
        {
            if (!ArmSetup_AnyBusy()) return true;
            ArmMotor_MsgBox(strTitle + " — 로봇팔이 동작 중입니다 (작업·조그·⑨·3D 모션). 끝난 뒤 다시 누르십시오.\r\n" +
                                  "(도는 동작이 지금 처짐 모델로 보내고 있어 바꾸지 않습니다)", MessageBoxIcon.Warning);
            return false;
        }

        /// <summary>적용·되돌리기 확인창 끝 줄 — 토크를 붙잡고 있으면 새 보정이 어떻게 들어오는지</summary>
        private string ArmSetup_SwitchNote()
        {
            return ArmMotor_TorqueHeld()
                ? "\r\n\r\n· 로봇팔 토크 ON — 지금은 움직이지 않습니다. 새 보정은 다음 동작의 출력 틱 1초에 걸쳐 들어옵니다 (튀지 않음). " +
                  "보정이 켜져 있는 동안 조그(방향 확인)는 막힙니다."
                : "";
        }

        /// <summary>⑥ 적용·되돌리기·[ … ] 전 — 결과·백업 목록을 읽은 urdf 폴더(m_strArmListDir)가 지금 출력 폴더 칸·설정의 폴더와 같은가. 그 폴더에만 한다
        /// (다른 폴더의 적용 중 모델을 건드리지 않게 — 처리기는 출력 폴더 칸을 다시 읽지 않고 m_strArmCfgDir 로). 칸 이름을 경로로 쓸 수 없으면 알리고 false.
        /// 다르면 지금 폴더와 맞춰(ArmSetup_SyncCfg) 페이지를 다시 채우고 false — 작업 버튼과 같은 규칙 (바뀐 목록을 본 뒤 다시 누르게).
        /// 다른 동작이 돌면 다시 채우지 않고 알리기만 (탭을 다시 볼 때 채운다). true = 같은 폴더</summary>
        private bool ArmSetup_ListDirCheck(string strWhat)
        {
            string strNow;
            try { strNow = GetUrdfOutputDir(); }
            catch (Exception ex)
            {
                ArmMotor_MsgBox(strWhat + " — 하지 않았습니다: urdf 폴더 이름을 경로로 쓸 수 없습니다 (" + ex.Message + ").\r\n출력 폴더 칸을 고친 뒤 다시 누르십시오.", MessageBoxIcon.Warning);
                return false;
            }
            if (m_strArmListDir != null && ArmSetup_SamePath(strNow, m_strArmListDir) && ArmSetup_SamePath(m_strArmListDir, m_strArmCfgDir)) return true;
            if (ArmSetup_AnyBusy())
            {
                ArmMotor_MsgBox(strWhat + " — 하지 않았습니다: urdf 폴더가 바뀌었습니다 (" + strNow + ").\r\n동작이 끝난 뒤 탭을 다시 열어 목록을 새로 읽고 다시 누르십시오.", MessageBoxIcon.Information);
                return false;
            }
            ArmSetup_SyncCfg();
            ArmSetup_RefreshPage();
            ArmSetup_UpdateEnable();
            ArmMotor_MsgBox(strWhat + " — 하지 않았습니다: 목록을 읽은 urdf 폴더가 지금 출력 폴더와 달라 이 페이지(결과·백업 목록·설정)를 지금 폴더로 다시 읽었습니다.\r\n" +
                            m_strArmCfgDir + "\r\n\r\n목록을 확인한 뒤 다시 누르십시오.", MessageBoxIcon.Information);
            return false;
        }

        /// <summary>작업 로그 대리자 — 로그 창(S2R)·로그 파일에, "진행 " 으로 시작하는 줄은 페이지 아래 lblArmBusy 에도 (작업 스레드에서 불린다)</summary>
        private Ojw.CSim2Real.DLog ArmSetup_Log()
        {
            return new Ojw.CSim2Real.DLog(delegate(string s)
            {
                try
                {
                    S2R_Log(s);
                    if (s != null && s.StartsWith("진행 ", StringComparison.Ordinal) && !IsDisposed)
                        BeginInvoke(new Action(delegate() { ArmSetup_Busy(s); }));
                }
                catch { }
            });
        }

        /// <summary>작업 중단 확인 대리자 — 이 페이지·로그 창의 [중단] 은 같은 m_bMechStop</summary>
        private Ojw.CSim2Real.DIsStopped ArmSetup_Stop()
        {
            return new Ojw.CSim2Real.DIsStopped(delegate() { return m_bMechStop; });
        }

        private void btnArmStop_Click(object sender, EventArgs e)
        {
            m_bMechStop = true;
            try { S2R_Log("중단 요청 — 지금 점을 마치고 원위치로 돌아가 토크를 시작 상태로 돌립니다..."); } catch { }
            ArmSetup_Busy("중단 요청함 — 원위치 중");
        }

        /// <summary>작업이 남긴 경고를 대화창으로 — 시작하지 않은 까닭, 원위치를 확인하지 못함 (실물).
        /// bRan = 작업이 손잡이를 열고 돌았다 (DLL 이 작업 보고서를 쓴 경우 — 결과의 ReportPath. 시작하지 않은 결과는 보고서 없이 Line 만).
        /// 대화창은 끝난 뒤 처리(목록 다시 읽기·잠금 풀기)가 다 끝난 다음에 뜬다 (BeginInvoke)</summary>
        private void ArmSetup_JobNotice(string strTitle, bool bRan, bool bDry, bool bStopped, bool bRestOk, string strWhy, string strRunDir)
        {
            if (!string.IsNullOrEmpty(strRunDir)) m_strArmLastRunDir = strRunDir;
            string s = null;
            if (!bRan) { if (!bStopped && !string.IsNullOrEmpty(strWhy)) s = strTitle + " — 시작하지 않았습니다:\r\n\r\n" + strWhy; }
            else if (!bDry && !bRestOk) s = strTitle + " — ★휴지 자세로 돌아왔는지 확인하지 못했습니다 (차가 크거나 읽기 실패).\r\n팔과 로그 창의 '원위치 —' 줄을 확인하십시오.";
            if (s == null || IsDisposed) return;
            try { BeginInvoke(new Action(delegate() { ArmMotor_MsgBox(s, MessageBoxIcon.Warning); })); }
            catch { }
        }

        // ════════════════════════════════════════════════════════════
        // ② 측정 계획
        // ════════════════════════════════════════════════════════════

        /// <summary>계획 만들기 — 첫닿음 모델(⑤ 에서 고른 결과 파일 → 첫닿음 모델이 없으면 적용 중 파일)로 점마다 하강 창 (DLL SagRun_MakePlan — IK 확인 포함),
        /// sagcomp_runs\plan_….csv 로 저장 (SagRun_SavePlan). 작업 스레드 (계획 93행 ≈ 0.5초)</summary>
        private void btnArmPlanMake_Click(object sender, EventArgs e)
        {
            const string T = "② 계획 만들기";
            if (!ArmSetup_ReadUi(T)) return;
            string strDir = m_strArmCfgDir;
            Ojw.C3d.CSagArmSetup cfg = m_ArmCfg;
            Ojw.C3d.CSagFileInfo fi = ArmSetup_SelectedResult();
            string strSel = (fi != null) ? fi.Path : null;
            string strActive = Ojw.C3d.SagRun_ActivePath(strDir);
            float fRetract = cfg.Measure.Probe.RetractAboveKneeMm;
            Ojw.CSim2Real.DLog log = ArmSetup_Log();
            List<Ojw.C3d.CFloorPlanRow> plan = null;
            string strPath = null, strStamp = null;
            ArmSetup_Run(T, false, delegate()
            {
                // 첫닿음 모델: 고른 결과 파일 → 없으면 적용 중 파일 (DLL 이 모델을 보고 거절·보고한다)
                Ojw.C3d.CSagComp m = null;
                string strFrom = null;
                foreach (string p in new string[] { strSel, strActive })
                {
                    if (string.IsNullOrEmpty(p) || !File.Exists(p)) continue;
                    Ojw.C3d.CSagComp mm = null;
                    try { mm = Ojw.C3d.CSagComp.Load(p); }
                    catch (Exception ex) { log("★읽지 못함 (다음 후보로): " + p + " — " + ex.Message); continue; }   // 다른 프로그램이 잡고 있으면 적용 중 파일로
                    if (mm != null && mm.HasFc) { m = mm; strFrom = p; break; }
                }
                log("첫닿음 모델: " + ((strFrom != null) ? strFrom : "없음"));
                string rep;
                List<Ojw.C3d.CFloorPlanRow> lst = m_C3d.SagRun_MakePlan(m, cfg.Plan, cfg.Arm, fRetract, out rep);
                log(rep);
                if (lst == null || lst.Count == 0) { m_strArmLast = "계획 없음"; return null; }
                string p2 = Ojw.C3d.SagRun_SavePlan(lst, strDir);
                if (p2 == null) { log("★계획 파일을 저장하지 못함 — " + Ojw.C3d.SagRun_RunsDir(strDir)); return null; }
                log("계획 파일: " + p2);
                plan = lst; strPath = p2; strStamp = ArmSetup_FileStamp(p2);   // 저장한 그때의 시각·크기 — 그 뒤 고치면 다시 읽는다
                return Path.GetFileName(p2);
            }, delegate()
            {
                if (plan == null || strPath == null) return;
                m_lstArmPlan = plan; m_strArmPlanPath = strPath; m_strArmPlanStamp = strStamp; m_strArmPlanWhy = ""; m_strArmPlanNote = "";
                string strRel = Ojw.C3d.SagRun_Relative(strDir, strPath);
                ArmSetup_SaveCfg(strDir, delegate(Ojw.C3d.CSagArmSetup c) { c.PlanFile = strRel; });   // 작업 중 밖에서 고친 sagarm.txt 를 먼저 읽고 plan 칸만
                m_strArmLast = ArmSetup_PlanRows() + "점";
            });
        }

        /// <summary>계획 불러오기 — 고른 CSV 를 읽고(DLL SagRun_PlanParse) 측정 작업과 같이 다시 확인한다 (DLL 6인자 SagRun_CheckPlan — 예측이 없는 예전 계획 행은 빼고,
        /// 첫닿음은 적용 중 파일의 첫닿음 모델(없으면 측정 모델 파일의 것)과 행에 적힌 계획을 만든 모델로 다시 예측해 창 시작·하강 한도를 본다.
        /// 다시 예측할 모델이 없으면 창이 있는 행을 뺀다. 측정 작업은 적용 중 파일·측정 모델 파일 둘 다로 보므로 두 파일이 다르면 작업이 더 뺄 수 있다
        /// — 작업이 다시 확인해 그 수를 알린다). 모델 읽기·확인은 작업 스레드</summary>
        private void btnArmPlanLoad_Click(object sender, EventArgs e)
        {
            const string T = "② 계획 불러오기";
            if (!ArmSetup_ReadUi(T)) return;
            string strDir = m_strArmCfgDir;
            string strPath;
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "측정 계획 불러오기";
                dlg.Filter = "측정 계획 (plan_*.csv)|plan_*.csv|CSV (*.csv)|*.csv|모든 파일|*.*";
                string strRuns = Ojw.C3d.SagRun_RunsDir(strDir);
                if (Directory.Exists(strRuns)) dlg.InitialDirectory = strRuns;
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                strPath = dlg.FileName;
            }
            string why, strStamp = ArmSetup_FileStamp(strPath);   // 읽기 전에 — 그 뒤 고치면 탭을 볼 때·미리보기·측정 버튼에서 다시 읽는다
            List<Ojw.C3d.CFloorPlanRow> plan;
            try { plan = Ojw.C3d.SagRun_PlanParse(File.ReadAllText(strPath), out why); }
            catch (Exception ex) { plan = null; why = ex.Message; }
            if (plan == null) { ArmMotor_MsgBox(T + " — 읽지 못했습니다:\r\n\r\n" + why + "\r\n\r\n" + strPath, MessageBoxIcon.Warning); return; }
            Ojw.C3d.CSagArmSetup cfg = m_ArmCfg;
            float fRetract = cfg.Measure.Probe.RetractAboveKneeMm;
            string strActive = Ojw.C3d.SagRun_ActivePath(strDir);
            string strCollect = Ojw.C3d.SagRun_Resolve(strDir, cfg.Measure.CollectModelPath, Ojw.C3d.SAGRUN_ACTIVE);
            Ojw.CSim2Real.DLog log = ArmSetup_Log();
            int nOk = -1;
            ArmSetup_Run(T, false, delegate()
            {
                log("계획 파일: " + strPath + ((why.Length > 0) ? " (" + why + ")" : ""));
                // 측정 작업이 첫닿음을 다시 예측할 모델 — 적용 중 파일 → 측정 모델 파일 (첫닿음 모델이 있는 첫 파일, DLL 이 행에 적힌 계획을 만든 모델도 쓴다)
                Ojw.C3d.CSagComp mFc = null;
                foreach (string p in new string[] { strActive, strCollect })
                {
                    if (string.IsNullOrEmpty(p) || !File.Exists(p)) continue;
                    Ojw.C3d.CSagComp mm = null;
                    try { mm = Ojw.C3d.CSagComp.Load(p); }
                    catch (Exception ex) { log("★읽지 못함 (다음 후보로): " + p + " — " + ex.Message); continue; }   // 다른 프로그램이 잡고 있으면 다음 후보
                    if (mm != null && mm.HasFc) { mFc = mm; break; }
                }
                string rep;
                nOk = m_C3d.SagRun_CheckPlan(plan, cfg.Arm, fRetract, true, mFc, out rep);
                log(rep);
                return string.Format(CultureInfo.InvariantCulture, "측정할 행 {0}/{1}", nOk, plan.Count);
            }, delegate()
            {
                if (nOk < 0) return;
                m_lstArmPlan = plan; m_strArmPlanPath = strPath; m_strArmPlanStamp = strStamp; m_strArmPlanWhy = why; m_strArmPlanNote = "";
                string strRel = Ojw.C3d.SagRun_Relative(strDir, strPath);
                ArmSetup_SaveCfg(strDir, delegate(Ojw.C3d.CSagArmSetup c) { c.PlanFile = strRel; });   // 밖에서 고친 sagarm.txt 를 먼저 읽고 plan 칸만
                m_strArmLast = nOk + "점";
            });
        }

        /// <summary>3D 미리보기 — 측정 작업을 3D 만으로 (DLL SagRun_MeasureJob bDry: 실물은 움직이지 않고 데이터셋도 건드리지 않는다). 확인창 없음</summary>
        private void btnArmPlanDry_Click(object sender, EventArgs e)
        {
            const string T = "② 3D 미리보기";
            if (!ArmSetup_ReadUi(T)) return;
            if (!ArmSetup_PlanCheck(T)) return;                 // 계획 파일이 바뀌었으면 다시 읽고 멈춤 (메모리의 옛 행으로 걷지 않는다)
            if (ArmSetup_PlanRows() == 0) { ArmMotor_MsgBox(T + " — 측정할 행이 없습니다. 계획 파일을 고친 뒤 다시 누르거나 [계획 만들기].", MessageBoxIcon.Information); return; }
            string strDir = m_strArmCfgDir;
            Ojw.C3d.CSagArmSetup cfg = m_ArmCfg;
            List<Ojw.C3d.CFloorPlanRow> plan = m_lstArmPlan;
            Ojw.CSim2Real.DLog log = ArmSetup_Log();
            Ojw.CSim2Real.DIsStopped stop = ArmSetup_Stop();
            Ojw.C3d.CSagRunResult res = null;
            ArmSetup_Run(T, false, delegate()
            {
                res = m_C3d.SagRun_MeasureJob(m_ArmConn, cfg.Arm, plan, cfg.Measure, strDir, true, log, stop);
                return (res != null && res.Ok) ? res.Line : null;
            }, delegate()
            {
                if (res == null) return;
                m_strArmPlanNote = (res.ReportPath != null) ? res.Line : "★3D 미리보기 — 시작하지 않음 (까닭은 로그 창)";
                m_strArmLast = res.Stopped ? "중단" : res.RowsDone + "/" + res.RowsPlanned + "점";
                ArmSetup_PlanLabel();
                ArmSetup_JobNotice(T, res.ReportPath != null, true, res.Stopped, res.RestOk, res.Why, res.RunDir);
            });
        }

        // ════════════════════════════════════════════════════════════
        // ③ 바닥 측정 (실물)
        // ════════════════════════════════════════════════════════════

        private void btnArmMeasure_Click(object sender, EventArgs e)
        {
            const string T = "③ 바닥 측정";
            if (!ArmSetup_ReadUi(T)) return;
            if (!ArmSetup_PlanCheck(T)) return;                 // 계획 파일이 바뀌었으면 다시 읽고 멈춤
            if (ArmSetup_PlanRows() == 0) { ArmMotor_MsgBox(T + " — 측정할 행이 없습니다. 계획 파일을 고친 뒤 다시 누르거나 ② [계획 만들기].", MessageBoxIcon.Information); return; }
            Ojw.C3d.CSagArmSetup cfg = m_ArmCfg;
            if (!ArmDef_EyeGate(T, cfg.Arm)) return;            // 수직 눈 확인 전이면 하는 곳을 알리고 시작하지 않는다 (DLL 작업 입구도 거절)
            if (!ArmSetup_CheckStart(T, cfg.Arm)) return;
            if (!ArmSetup_PlanCheck(T)) return;                 // 확인창 바로 앞에 한 번 더 (위 확인이 떠 있던 사이 고쳤을 수도) — 확인창은 지금 파일의 행으로
            string strDir = m_strArmCfgDir;
            List<Ojw.C3d.CFloorPlanRow> plan = m_lstArmPlan;    // 확인창에 보인 그 행으로 작업한다
            if (!ArmMotor_Confirm(ArmSetup_MeasureText(cfg, strDir), T)) return;
            if (!ArmSetup_CheckConn(T)) return;
            Ojw.CSim2Real.DLog log = ArmSetup_Log();
            Ojw.CSim2Real.DIsStopped stop = ArmSetup_Stop();
            Ojw.C3d.CSagRunResult res = null;
            ArmSetup_Run(T, true, delegate()
            {
                res = m_C3d.SagRun_MeasureJob(m_ArmConn, cfg.Arm, plan, cfg.Measure, strDir, false, log, stop);
                return (res != null && res.Ok) ? res.Line : null;
            }, delegate()
            {
                if (res == null) return;
                m_strArmMeasureLine = (res.Ok || res.Stopped || res.Line.StartsWith("★")) ? res.Line : "★" + res.Line;
                m_strArmLast = res.Stopped ? "중단" : res.RowsDone + "/" + res.RowsPlanned + "점";
                ArmSetup_MeasureLabel();
                ArmSetup_JobNotice(T, res.ReportPath != null, false, res.Stopped, res.RestOk, res.Why, res.RunDir);
            });
        }

        /// <summary>③ 확인창 (명세 3.5 를 첫닿음 계획에 맞춤) — 값은 설정·계획·DLL 에서 (예상 시간 SagRun_EstimateSec, 데이터셋 SagRun_DatasetInfo,
        /// 측정 모델의 매핑 차이 SagArm_MotorMapWarn)</summary>
        private string ArmSetup_MeasureText(Ojw.C3d.CSagArmSetup cfg, string strDir)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            Ojw.C3d.CSagArmCfg a = cfg.Arm;
            int nRows = 0;
            List<string> lines = new List<string>();
            foreach (Ojw.C3d.CFloorPlanRow r in m_lstArmPlan) if (r != null && r.IkOk) { nRows++; if (!lines.Contains(r.LineKey)) lines.Add(r.LineKey); }
            double sec = Ojw.C3d.SagRun_EstimateSec(m_lstArmPlan, cfg.Measure.Probe, a);
            string strDs;
            Ojw.C3d.SagRun_DatasetInfo(Ojw.C3d.SagRun_Resolve(strDir, cfg.Measure.DatasetPath, Ojw.C3d.SAGRUN_FC_DATASET), out strDs);
            string strMap = "";
            try { strMap = Ojw.C3d.SagArm_MotorMapWarn(m_ArmConn, Ojw.C3d.CSagComp.Load(Ojw.C3d.SagRun_Resolve(strDir, cfg.Measure.CollectModelPath, Ojw.C3d.SAGRUN_ACTIVE))); }
            catch { strMap = ""; }
            bool bTq = m_ArmConn.IsTorqueOn;
            return "③ 바닥 측정 — 로봇팔이 실제로 움직입니다.\r\n\r\n" +
                   ArmSetup_PortLine(a) +
                   (bTq ? "· 토크는 이미 켜져 있음 — 끝나면 휴지 자세로 돌아가 토크 ON 그대로 붙잡습니다\r\n"
                        : "· 시작: 지금 자세(휴지)를 읽어 두고 토크를 켭니다 — 끝나면 그 자세로 돌아가 토크를 끕니다\r\n") +
                   string.Format(ci, "· 순서: 펼치기(({0:0.#},{1:0.#},{2:0.#}) 경유 {3:0.#}초) → 줄마다 옮기는 높이(최고 z={4:0.#})로 들어 옮김\r\n" +
                                     "         → 점마다 천천히({5:0.#}mm/s) 내려가 바닥을 누르고 들어 올림 → 원위치(({6:0.#},{7:0.#},{8:0.#}) 경유, 관절 {9:0.#}초) → 토크 {10}\r\n",
                                 a.UnfoldVia[0], a.UnfoldVia[1], a.UnfoldVia[2], a.UnfoldViaMs / 1000.0, a.SafeZ, cfg.Measure.Probe.PressDescentMmPerSec,
                                 a.RestoreVia[0], a.RestoreVia[1], a.RestoreVia[2], a.RestoreJointMs / 1000.0, bTq ? "ON 그대로" : "끔") +
                   string.Format(ci, "· {0}점 ({1}줄), 약 {2:0}분, 손목 수직 유지 {3} → 데이터: {4} 에 덧붙임\r\n",
                                 nRows, lines.Count, sec / 60.0, cfg.Measure.WristHold ? "켬" : "끔", strDs) +
                   "· 계획: " + Path.GetFileName(m_strArmPlanPath) + " (첫닿음 모델 예측 창 — 시작 = 예측 + 위, 한도 = 예측 − 아래)" +
                   ((m_strArmPlanWhy.Length > 0) ? " ★" + m_strArmPlanWhy : "") + "\r\n" +
                   ((strMap.Length > 0) ? "· " + strMap + "\r\n" : "") +
                   "· 바닥 위 작업 공간을 비우십시오. [중단] 은 언제든 — 지금 점을 마치고 원위치합니다.\r\n\r\n" +
                   "[예] 시작   [아니오] 취소";
        }

        /// <summary>확인창의 연결 줄 — "COM4 @ 1000000, ID 11~14 이동, 15~16 토크만 켜서 고정"</summary>
        private string ArmSetup_PortLine(Ojw.C3d.CSagArmCfg a)
        {
            return string.Format(CultureInfo.InvariantCulture, "· COM{0} @ {1}, ID {2} 이동{3}\r\n", m_ArmConn.Port, m_ArmConn.Baud,
                                 Ojw.CArmMotorSet_t.RangeText(a.MoveIDs),
                                 (a.HoldIDs != null && a.HoldIDs.Length > 0) ? ", " + Ojw.CArmMotorSet_t.RangeText(a.HoldIDs) + " 토크만 켜서 고정" : "");
        }

        /// <summary>확인창의 경로 줄 (닿기 시험 — 측정 확인창은 순서 줄에 이미 있다) — 펼치기·원위치 경유점과 옮기는 높이
        /// (sagarm.txt 의 unfoldvia·safez·restorevia, 지금 설정 값)</summary>
        private static string ArmSetup_PathLine(Ojw.C3d.CSagArmCfg a)
        {
            return string.Format(CultureInfo.InvariantCulture,
                                 "· 경로: 펼치기(({0:0.#},{1:0.#},{2:0.#}) 경유 {3:0.#}초) → 점 사이는 옮기는 높이(최고 z={4:0.#})로 들어 옮김 → 원위치(({5:0.#},{6:0.#},{7:0.#}) 경유, 관절 {8:0.#}초)\r\n",
                                 a.UnfoldVia[0], a.UnfoldVia[1], a.UnfoldVia[2], a.UnfoldViaMs / 1000.0, a.SafeZ,
                                 a.RestoreVia[0], a.RestoreVia[1], a.RestoreVia[2], a.RestoreJointMs / 1000.0);
        }

        /// <summary>확인창이 떠 있는 사이 연결이 끊기거나 다른 동작이 시작됐으면 시작하지 않는다</summary>
        private bool ArmSetup_CheckConn(string strTitle)
        {
            if (m_ArmConn != null && m_ArmConn.IsConnected && !m_ArmConn.ConfigMismatch && !ArmSetup_AnyBusy()) return true;
            ArmMotor_MsgBox(strTitle + " — 확인하는 사이 로봇팔 연결 상태가 바뀌었거나 다른 동작이 시작됐습니다. 다시 누르십시오.", MessageBoxIcon.Information);
            return false;
        }

        // ════════════════════════════════════════════════════════════
        // ④ 학습 → 결과 파일
        // ════════════════════════════════════════════════════════════

        /// <summary>학습 — 데이터셋(④ 줄)·손목 유지 켬 행·기준 높이로 DLL SagRun_Learn (작업 스레드 — "진행 학습:" 줄이 아래 줄에, [중단] 이면 파일을 남기지 않는다).
        /// 결과 파일에는 지금 모터 매핑 키를 적는다 (motormap — 바탕 관절 모델이 다른 매핑에서 학습됐으면 DLL 이 바탕의 키를 적고 ★ 로 알린다). 끝나면 새 결과 파일을 ⑤ 에서 고른다</summary>
        private void btnArmLearn_Click(object sender, EventArgs e)
        {
            const string T = "④ 학습";
            if (!ArmSetup_ReadUi(T)) return;
            string strDir = m_strArmCfgDir;
            Ojw.C3d.CSagLearnCfg lc = m_ArmCfg.Learn;
            string strKey = ArmSetup_MapKey();
            Ojw.CSim2Real.DLog log = ArmSetup_Log();
            Ojw.CSim2Real.DIsStopped stop = ArmSetup_Stop();
            Ojw.C3d.CSagLearnResult res = null;
            ArmSetup_Run(T, false, delegate()
            {
                res = m_C3d.SagRun_Learn(lc, strDir, strKey, log, stop);
                if (res != null && res.Ok) log("보고서: " + res.ReportPath);
                return (res != null && res.Ok) ? res.Line : null;
            }, delegate()
            {
                if (res == null) return;
                // 바탕 관절 모델이 지금과 다른 모터 매핑(방향·영점)에서 학습됐으면 결과 파일은 만들어졌어도 빨강 — 한 줄 맨 앞의 ★ (DLL MotorMapWarn)
                ArmSetup_SetLabel(lblArmLearn, res.Line, (res.Ok && string.IsNullOrEmpty(res.MotorMapWarn)) ? Color.DarkGreen : Color.Firebrick);
                if (res.Ok && res.ModelPath != null)
                {
                    string strRel = Ojw.C3d.SagRun_Relative(strDir, res.ModelPath);
                    ArmSetup_SaveCfg(strDir, delegate(Ojw.C3d.CSagArmSetup c) { c.ResultFile = strRel; });   // 학습 중 밖에서 고친 sagarm.txt 를 먼저 읽고 result 칸만
                    if (ArmSetup_SamePath(strDir, m_strArmCfgDir)) m_strArmResultSel = res.ModelPath;          // 새 결과 파일을 ⑤ 에서 고른다
                    m_strArmLast = Path.GetFileNameWithoutExtension(res.ModelPath);
                }
                else m_strArmLast = res.Stopped ? "중단" : "실패";
            });
        }

        // ════════════════════════════════════════════════════════════
        // ⑤ 결과 파일 시험 — 닿기 (3D 만 / 실물)
        // ════════════════════════════════════════════════════════════

        /// <summary>결과 폴더 밖의 모델 파일 고르기 — 목록 끝에 '다른 파일' 로</summary>
        private void btnArmResultBrowse_Click(object sender, EventArgs e)
        {
            if (!ArmSetup_ListDirCheck("⑤ 결과 파일 고르기")) return;   // 목록을 읽은 urdf 폴더 기준 (출력 폴더 칸이 바뀌었으면 그 폴더로 다시 읽고 멈춤)
            string strDir = m_strArmCfgDir;
            string strPath;
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "처짐 모델(결과) 파일 고르기";
                dlg.Filter = "처짐 모델 (sagcomp_model*.txt)|sagcomp_model*.txt|텍스트 (*.txt)|*.txt|모든 파일|*.*";
                string strRes = Ojw.C3d.SagRun_ResultsDir(strDir);
                dlg.InitialDirectory = Directory.Exists(strRes) ? strRes : (Directory.Exists(strDir) ? strDir : "");
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                strPath = dlg.FileName;
            }
            Ojw.C3d.CSagComp m = null;
            try { m = Ojw.C3d.CSagComp.Load(strPath); } catch { m = null; }
            if (m == null || !m.IsUsable)
            {
                ArmMotor_MsgBox("처짐 모델 파일이 아닙니다 (읽지 못함):\r\n" + strPath, MessageBoxIcon.Warning);
                return;
            }
            m_strArmBrowsed = strPath;
            m_strArmResultSel = strPath;
            ArmSetup_FillResults();
            ArmSetup_TestLabel(true);
            if (cmbArmTwinSrc.SelectedIndex == 1) ArmSetup_TwinApply();
            ArmSetup_UpdateEnable();
        }

        private void btnArmTestDry_Click(object sender, EventArgs e) { ArmSetup_TestStart(false); }
        private void btnArmTest_Click(object sender, EventArgs e) { ArmSetup_TestStart(true); }

        /// <summary>⑤ 시험할 결과 파일 — 고른 것 (첫닿음 모델이 있어야 — 없으면 알리고 null)</summary>
        private Ojw.C3d.CSagFileInfo ArmSetup_TestFile(string strTitle)
        {
            Ojw.C3d.CSagFileInfo fi = ArmSetup_SelectedResult();
            if (fi == null || !File.Exists(fi.Path))
            { ArmMotor_MsgBox(strTitle + " — 결과 파일을 고르십시오 (④ 학습 결과 — sagcomp_results).", MessageBoxIcon.Information); return null; }
            if (fi.NFc <= 0)
            { ArmMotor_MsgBox(strTitle + " — 첫닿음 모델이 없는 파일입니다 (" + fi.Brief + ").\r\n④ 학습 결과 파일을 고르십시오.", MessageBoxIcon.Information); return null; }
            return fi;
        }

        /// <summary>시험할 결과 파일 읽기 (UI 스레드, DLL CSagComp.Load) — 읽지 못하면(다른 프로그램이 잡고 있음·그사이 지워짐) 알리고 null</summary>
        private Ojw.C3d.CSagComp ArmSetup_LoadModel(string strTitle, string strPath)
        {
            Ojw.C3d.CSagComp m = null;
            string why = "파일이 없음";
            try { m = Ojw.C3d.CSagComp.Load(strPath); }
            catch (Exception ex) { m = null; why = ex.Message; }
            if (m == null) ArmMotor_MsgBox(strTitle + " — 결과 파일을 읽지 못했습니다 (" + why + ").\r\n\r\n" + strPath, MessageBoxIcon.Warning);
            return m;
        }

        /// <summary>닿기 시험 — 결과 파일을 보정 끔(관절 모델만)·켬으로 번갈아 눌러 본다 (DLL SagRun_TestJob). 시작 전에 DLL 시험 점(SagRun_TestPoints —
        /// 피치 ≤ 90·IK·창이 예측 위인지)으로 쓸 점과 뺀 까닭을 확인창에. 3D 만이면 예상 높이로 걷는다</summary>
        private void ArmSetup_TestStart(bool bReal)
        {
            string T = bReal ? "⑤ 실물 닿기 시험" : "⑤ 3D 닿기 시험";
            if (!ArmSetup_ReadUi(T)) return;
            Ojw.C3d.CSagFileInfo fi = ArmSetup_TestFile(T);
            if (fi == null) return;
            Ojw.C3d.CSagArmSetup cfg = m_ArmCfg;
            string strDir = m_strArmCfgDir;
            string strRes = fi.Path;
            Ojw.C3d.CSagComp mOn = ArmSetup_LoadModel(T, strRes);
            if (mOn == null) return;
            string rep;
            List<Ojw.C3d.CSagTestPoint> pts = m_C3d.SagRun_TestPoints(mOn, cfg.Test, cfg.Arm, out rep);
            int nUse = 0;
            float fSum = float.NaN;
            List<string> skip = new List<string>();
            foreach (Ojw.C3d.CSagTestPoint p in pts)
            {
                if (p == null) continue;
                if (p.Skipped) { if (skip.Count < 3) skip.Add(string.Format(CultureInfo.InvariantCulture, "r{0:0} 요{1:0}: {2}", p.R, p.Yaw, p.Note)); continue; }
                nUse++;
                if (float.IsNaN(fSum)) fSum = p.QSum;
            }
            if (nUse == 0)
            { ArmMotor_MsgBox(T + " — 시험할 점이 없습니다:\r\n\r\n" + rep, MessageBoxIcon.Warning); return; }
            if (bReal)
            {
                if (!ArmDef_EyeGate(T, cfg.Arm)) return;        // 수직 눈 확인 전이면 하는 곳을 알리고 시작하지 않는다 (DLL 작업 입구도 거절)
                if (!ArmSetup_CheckStart(T, cfg.Arm)) return;
                CultureInfo ci = CultureInfo.InvariantCulture;
                Ojw.C3d.CSagTestCfg tc = cfg.Test;
                string strMap = Ojw.C3d.SagArm_MotorMapWarn(m_ArmConn, mOn);
                bool bTq = m_ArmConn.IsTorqueOn;
                string s = "⑤ 닿기 시험 — 로봇팔이 실제로 움직입니다.\r\n\r\n" +
                           "· 파일: " + fi.Name + " (" + fi.Brief + " · 시험 기록: " + fi.TestState + ")\r\n" +
                           string.Format(ci, "· 점: 반경 {0}, 요 {1}, 공구 피치 Ry {2:0.#}{3} — 쓸 점 {4}{5}\r\n",
                                         ArmSetup_Join(tc.Radii, "/", "0.#"), ArmSetup_Join(tc.Yaws, "/", "0.#"), tc.Pitch,
                                         float.IsNaN(fSum) ? "" : string.Format(ci, " (관절 합 {0:0})", fSum), nUse,
                                         (skip.Count > 0) ? " (제외 " + (pts.Count - nUse) + ": " + string.Join("; ", skip.ToArray()) + ")" : "") +
                           string.Format(ci, "· 점마다 보정 끔 → 켬 × 반복 {0} (누름 {1}번), 손목 유지 {2}\r\n",
                                         tc.Repeats, nUse * tc.Repeats * 2, tc.WristHold ? "켬" : "끔 (실사용 조건)") +
                           string.Format(ci, "· 창: 예상 닿는 높이(끔·켬 중 높은 쪽) 위 {0:0.#}mm 에서, 그 모드의 예상 닿는 높이 아래 {1:0.#}mm 까지\r\n", tc.AboveMm, tc.BelowMm) +
                           ArmSetup_PortLine(cfg.Arm) +
                           ArmSetup_PathLine(cfg.Arm) +
                           (bTq ? "· 토크는 이미 켜져 있음 — 끝나면 휴지 자세로 돌아가 토크 ON 그대로\r\n" : "· 휴지 자세를 읽어 두고 토크를 켭니다 — 끝나면 그 자세로 돌아가 토크를 끕니다\r\n") +
                           ((strMap.Length > 0) ? "· " + strMap + "\r\n" : "") +
                           "· 작업 공간을 비우십시오. [중단] 은 언제든.\r\n\r\n[예] 시작   [아니오] 취소";
                if (!ArmMotor_Confirm(s, T)) return;
                if (!ArmSetup_CheckConn(T)) return;
            }
            ArmSetup_TwinAutoShow();
            Ojw.CSim2Real.DLog log = ArmSetup_Log();
            Ojw.CSim2Real.DIsStopped stop = ArmSetup_Stop();
            Ojw.C3d.CSagTestResult res = null;
            ArmSetup_Run(T, bReal, delegate()
            {
                res = m_C3d.SagRun_TestJob(m_ArmConn, cfg.Arm, strRes, cfg.Test, strDir, !bReal, log, stop);
                return (res != null && res.Ok) ? res.Line : null;
            }, delegate()
            {
                if (res == null) return;
                ArmSetup_TestResult(strRes, res.Line, !res.Ok && !res.Stopped);
                m_strArmLast = string.IsNullOrEmpty(res.Verdict) ? "" : res.Verdict;
                ArmSetup_JobNotice(T, res.ReportPath != null, res.Dry, res.Stopped, res.RestOk, res.Why, res.RunDir);
            });
        }

        /// <summary>시험 결과 줄을 ⑤ 라벨에 (그 파일을 고르고 있는 동안 목록을 다시 읽어도 그대로)</summary>
        private void ArmSetup_TestResult(string strPath, string strLine, bool bFail)
        {
            m_strArmTestFor = strPath;
            ArmSetup_SetLabel(lblArmTest, (strLine == null) ? "" : strLine, bFail ? Color.Firebrick : Color.DarkGreen);
        }

        /// <summary>닿기 시험·실습 1·3단계 전 — 처짐 쌍둥이(실물이 있을 자세)가 꺼져 있으면 '3D' 로 켠다 (명세: 시험 때 쌍둥이 보기 자동 켬)</summary>
        private void ArmSetup_TwinAutoShow()
        {
            if (cmbSagTwin.SelectedIndex > 0 || cmbSagTwin.Items.Count < 2) return;
            cmbSagTwin.SelectedIndex = 1;                       // cmbSagTwin_SelectedIndexChanged 가 3D 표시를 켠다
            ArmMotor_Log("시험 — 처짐 쌍둥이 표시 켬 (3D)");
        }

        // ════════════════════════════════════════════════════════════
        // ⑥ 적용·되돌리기 (UI 스레드 — DLL 규칙)
        // ════════════════════════════════════════════════════════════

        private void btnArmApply_Click(object sender, EventArgs e)
        {
            const string T = "⑥ 적용";
            if (!ArmSetup_CanSwitchComp(T)) return;
            if (!ArmSetup_ListDirCheck(T)) return;              // 목록을 읽은 urdf 폴더에만 — 출력 폴더 칸이 바뀌었으면 그 폴더로 다시 읽고 멈춤
            Ojw.C3d.CSagFileInfo fi = ArmSetup_SelectedResult();
            if (fi == null || fi.IsActive || fi.SameAsActive) return;
            string strDir = m_strArmCfgDir;                     // 목록의 폴더 (출력 폴더 칸을 다시 읽지 않는다)
            string strKey = ArmSetup_MapKey();
            string d = ArmSetup_MapDiff(fi.MotorMap);
            // 시험 기록 요약(DLL)은 종류마다 마지막 기록의 판정이다 — 실물 판정(합격·불합격·판정 불가)이 안 보이면 '실물 판정 기록 없음'
            // (앞의 실물 기록이 뒤의 3D·중단 기록에 가렸을 수 있다 — 시험 기록 파일을 보게)
            string s = "이 결과 파일을 적용합니다: " + fi.Name + " (" + fi.Brief + ")\r\n" +
                       (ArmSetup_HasRealVerdict(fi.TestState) ? "시험: " + fi.TestState
                                                              : "★실물 판정 기록 없음 (마지막 기록: " + fi.TestState + ") — 앞의 실물 시험은 시험 기록 파일 " +
                                                                Path.GetFileName(Ojw.C3d.SagRun_TestRecordPath(fi.Path)) + " 에서 확인하십시오") + "\r\n" +
                       ((d.Length > 0) ? "★학습 때와 방향·영점이 다름 (" + d + ") — 다시 학습 권장\r\n" : "") +
                       "적용할 urdf 폴더: " + strDir + "\r\n" +
                       "지금 파일은 sagcomp_backup\\… 로 백업됩니다. 적용하면 1초에 걸쳐 새 보정으로 바뀝니다." + ArmSetup_SwitchNote();
            if (!ArmMotor_Confirm(s, T)) return;
            if (!ArmSetup_CanSwitchComp(T)) return;             // 확인창이 떠 있는 사이 다른 동작이 시작됐으면
            string strBak, rep;
            bool bOk = m_C3d.SagRun_Activate(fi.Path, strDir, strKey, new Ojw.CSim2Real.DLog(ArmMotor_Log), out strBak, out rep);
            ArmSetup_AfterSwitch(T, bOk, rep);
        }

        private void btnArmRollback_Click(object sender, EventArgs e)
        {
            const string T = "⑥ 되돌리기";
            if (!ArmSetup_CanSwitchComp(T)) return;
            if (!ArmSetup_ListDirCheck(T)) return;              // 백업 목록을 읽은 urdf 폴더에만 — 출력 폴더 칸이 바뀌었으면 그 폴더로 다시 읽고 멈춤
            int k = cmbArmBackup.SelectedIndex;
            if (k < 0 || k >= m_lstArmBackups.Count) return;
            Ojw.C3d.CSagFileInfo fb = m_lstArmBackups[k];
            string strDir = m_strArmCfgDir;                     // 목록의 폴더 (출력 폴더 칸을 다시 읽지 않는다)
            string s = "백업 " + fb.Time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + " (" + fb.Brief + ") 으로 되돌립니다.\r\n" +
                       "되돌릴 urdf 폴더: " + strDir + "\r\n" +
                       "지금 파일도 먼저 백업합니다. 되돌리면 1초에 걸쳐 그 보정으로 바뀝니다." + ArmSetup_SwitchNote();
            if (!ArmMotor_Confirm(s, T)) return;
            if (!ArmSetup_CanSwitchComp(T)) return;
            string rep;
            bool bOk = m_C3d.SagRun_Rollback(fb.Path, strDir, new Ojw.CSim2Real.DLog(ArmMotor_Log), out rep);
            ArmSetup_AfterSwitch(T, bOk, rep);
        }

        /// <summary>적용·되돌리기 뒤 (UI 스레드) — 보정 체크·쌍둥이(적용이 쌍둥이를 적용 중 모델로 바꿨다 — ⑦ 에서 고른 쪽으로 다시)·목록·조그 막힘 표시, 보고는 Log 탭</summary>
        private void ArmSetup_AfterSwitch(string strTitle, bool bOk, string rep)
        {
            ArmMotor_Log(strTitle + (bOk ? " — " : " 실패 — ") + ((rep == null) ? "" : rep.Replace("\r\n", " / ")));
            SagComp_UiSync();
            ArmSetup_TwinApply();
            ArmSetup_OnShown();
            ArmMotor_UpdateUi();
            ArmSetup_Busy((bOk ? "" : "★") + strTitle + (bOk ? " 완료" : " 실패") + " — 자세한 내용은 Log 탭");
            if (!bOk) ArmMotor_MsgBox(strTitle + " — 하지 못했습니다:\r\n\r\n" + rep, MessageBoxIcon.Warning);
        }

        // ════════════════════════════════════════════════════════════
        // ⑦ 쌍둥이·폴더·콤보
        // ════════════════════════════════════════════════════════════

        /// <summary>쌍둥이 모델 — '적용 중 모델' 이면 적용 중 모델로, '선택한 결과 파일' 이면 ⑤ 에서 고른 파일로 (DLL SagRun_TwinUse — 적용하지 않고 실물이 있을
        /// 자세만 본다). 작업 중이면 하지 않는다 (작업이 쌍둥이 모델을 정하고 끝에 되돌린다 — DLL 도 거절). UI 스레드</summary>
        private void ArmSetup_TwinApply()
        {
            if (m_C3d == null || m_bArmLocked || Ojw.C3d.SagRun_IsBusy) return;
            string strPath = null;
            if (cmbArmTwinSrc.SelectedIndex == 1)
            {
                Ojw.C3d.CSagFileInfo fi = ArmSetup_SelectedResult();
                if (fi != null && !fi.IsActive) strPath = fi.Path;
            }
            string strInfo;
            m_C3d.SagRun_TwinUse(strPath, out strInfo);
            m_strArmTwinInfo = strInfo;
            m_ArmTwinModel = m_C3d.SagTwin_GetModel();
            ArmSetup_TwinUiSync();
        }

        private void cmbArmTwinSrc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_bArmExpUiSync || m_bArmLocked) return;
            ArmSetup_TwinApply();
        }

        private void cmbArmResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_bArmExpUiSync) return;
            Ojw.C3d.CSagFileInfo fi = ArmSetup_SelectedResult();
            m_strArmResultSel = (fi != null) ? fi.Path : null;
            tipArmExp.SetToolTip(cmbArmResult, (fi != null) ? fi.Path + "\r\n" + fi.Display : "");
            ArmSetup_TestLabel(true);                           // 고른 파일의 요약·시험 기록
            if (cmbArmTwinSrc.SelectedIndex == 1) ArmSetup_TwinApply();
            ArmSetup_UpdateEnable();
        }

        private void chkArmHold_CheckedChanged(object sender, EventArgs e)
        {
            if (m_bArmExpUiSync) return;
            ArmSetup_MeasureLabel();                            // 데이터가 쌓일 곳 (hold 칸)
            ArmSetup_UpdateEnable();
        }

        /// <summary>폴더 열기 — 마지막 작업 폴더 (없으면 이 페이지가 읽은 urdf 폴더의 sagcomp_runs — 출력 폴더 칸을 바로 읽지 않는다: 경로로 못 쓰는 이름이면 예외)</summary>
        private void btnArmOpenDir_Click(object sender, EventArgs e)
        {
            string strDir = m_strArmLastRunDir;
            try
            {
                if (strDir == null || !Directory.Exists(strDir))
                {
                    if (m_strArmCfgDir == null) { ArmMotor_MsgBox("폴더 열기 — urdf 폴더를 아직 읽지 못했습니다 (출력 폴더 칸을 확인하십시오).", MessageBoxIcon.Information); return; }
                    strDir = Ojw.C3d.SagRun_RunsDir(m_strArmCfgDir);
                }
                Directory.CreateDirectory(strDir);
                Process.Start("explorer.exe", "\"" + strDir + "\"");
            }
            catch (Exception ex) { ArmMotor_MsgBox("폴더를 열지 못했습니다:\r\n" + strDir + "\r\n\r\n" + ex.Message, MessageBoxIcon.Information); }
        }
    }
}
