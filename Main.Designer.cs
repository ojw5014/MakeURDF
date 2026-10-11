namespace MakeUrdf
{
    partial class Main
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnDisp = new System.Windows.Forms.Panel();
            this.tmrDisp = new System.Windows.Forms.Timer(this.components);
            this.tabRight = new System.Windows.Forms.TabControl();
            this.tabCreate = new System.Windows.Forms.TabPage();
            this.rdModeMouse = new System.Windows.Forms.RadioButton();
            this.rdModeButton = new System.Windows.Forms.RadioButton();
            this.rdModeHybrid = new System.Windows.Forms.RadioButton();
            this.lblMoveInput = new System.Windows.Forms.Label();
            this.txtMoveValue = new System.Windows.Forms.TextBox();
            this.lblRotateInput = new System.Windows.Forms.Label();
            this.txtRotateValue = new System.Windows.Forms.TextBox();
            this.pnDragArea = new System.Windows.Forms.Panel();
            this.lblDragArea = new System.Windows.Forms.Label();
            this.lblMoveGroup = new System.Windows.Forms.Label();
            this.txtStateXMove = new System.Windows.Forms.TextBox();
            this.txtStateZMove = new System.Windows.Forms.TextBox();
            this.txtStateYMove = new System.Windows.Forms.TextBox();
            this.txtStateXRotate = new System.Windows.Forms.TextBox();
            this.txtStateZRotate = new System.Windows.Forms.TextBox();
            this.txtStateYRotate = new System.Windows.Forms.TextBox();
            this.lblStateJoint = new System.Windows.Forms.Label();
            this.txtStateJoint = new System.Windows.Forms.TextBox();
            this.chkStateInit = new System.Windows.Forms.CheckBox();
            this.lblStateDir = new System.Windows.Forms.Label();
            this.cmbStateDir = new System.Windows.Forms.ComboBox();
            this.lblStateNote = new System.Windows.Forms.Label();
            this.btnRedMove = new System.Windows.Forms.Button();
            this.btnBlueMove = new System.Windows.Forms.Button();
            this.btnGreenMove = new System.Windows.Forms.Button();
            this.lblRotateGroup = new System.Windows.Forms.Label();
            this.btnRedRotate = new System.Windows.Forms.Button();
            this.btnBlueRotate = new System.Windows.Forms.Button();
            this.btnGreenRotate = new System.Windows.Forms.Button();
            this.btnRedMoveM10 = new System.Windows.Forms.Button();
            this.btnRedMoveP10 = new System.Windows.Forms.Button();
            this.btnRedMoveMTxt = new System.Windows.Forms.Button();
            this.btnRedMovePTxt = new System.Windows.Forms.Button();
            this.btnBlueMoveM10 = new System.Windows.Forms.Button();
            this.btnBlueMoveP10 = new System.Windows.Forms.Button();
            this.btnBlueMoveMTxt = new System.Windows.Forms.Button();
            this.btnBlueMovePTxt = new System.Windows.Forms.Button();
            this.btnGreenMoveM10 = new System.Windows.Forms.Button();
            this.btnGreenMoveP10 = new System.Windows.Forms.Button();
            this.btnGreenMoveMTxt = new System.Windows.Forms.Button();
            this.btnGreenMovePTxt = new System.Windows.Forms.Button();
            this.btnRedRotateM90 = new System.Windows.Forms.Button();
            this.btnRedRotateP90 = new System.Windows.Forms.Button();
            this.btnRedRotateMTxt = new System.Windows.Forms.Button();
            this.btnRedRotatePTxt = new System.Windows.Forms.Button();
            this.btnBlueRotateM90 = new System.Windows.Forms.Button();
            this.btnBlueRotateP90 = new System.Windows.Forms.Button();
            this.btnBlueRotateMTxt = new System.Windows.Forms.Button();
            this.btnBlueRotatePTxt = new System.Windows.Forms.Button();
            this.btnGreenRotateM90 = new System.Windows.Forms.Button();
            this.btnGreenRotateP90 = new System.Windows.Forms.Button();
            this.btnGreenRotateMTxt = new System.Windows.Forms.Button();
            this.btnGreenRotatePTxt = new System.Windows.Forms.Button();
            this.btnMotor = new System.Windows.Forms.Button();
            this.cmbMotorId = new System.Windows.Forms.ComboBox();
            this.btnInit = new System.Windows.Forms.Button();
            this.btnStl = new System.Windows.Forms.Button();
            this.btnStlCenter = new System.Windows.Forms.Button();
            this.btnStlMerge = new System.Windows.Forms.Button();
            this.lblStatusTitle = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCurrentValue = new System.Windows.Forms.Label();
            this.lblHistoryTitle = new System.Windows.Forms.Label();
            this.lstHistory = new System.Windows.Forms.ListBox();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblEtcGroup = new System.Windows.Forms.Label();
            this.tabTest = new System.Windows.Forms.TabPage();
            this.lblIkGroup = new System.Windows.Forms.Label();
            this.lblFuncNum = new System.Windows.Forms.Label();
            this.cmbFuncNum = new System.Windows.Forms.ComboBox();
            this.lblIkMode = new System.Windows.Forms.Label();
            this.cmbCalcInvMode = new System.Windows.Forms.ComboBox();
            this.lblIkX = new System.Windows.Forms.Label();
            this.txtIkX = new System.Windows.Forms.TextBox();
            this.lblIkY = new System.Windows.Forms.Label();
            this.txtIkY = new System.Windows.Forms.TextBox();
            this.lblIkZ = new System.Windows.Forms.Label();
            this.txtIkZ = new System.Windows.Forms.TextBox();
            this.btnGoIk = new System.Windows.Forms.Button();
            this.btnGetFk = new System.Windows.Forms.Button();
            this.btnResetAngle = new System.Windows.Forms.Button();
            this.lblFkGroup = new System.Windows.Forms.Label();
            this.lblMotorCtrl = new System.Windows.Forms.Label();
            this.cmbMotorCtrl = new System.Windows.Forms.ComboBox();
            this.lblAngleCtrl = new System.Windows.Forms.Label();
            this.txtAngleCtrl = new System.Windows.Forms.TextBox();
            this.lblAngleInfo = new System.Windows.Forms.Label();
            this.trkAngle = new System.Windows.Forms.TrackBar();
            this.lblMouseModeTitle = new System.Windows.Forms.Label();
            this.rdMouseCam = new System.Windows.Forms.RadioButton();
            this.rdMouseJoint = new System.Windows.Forms.RadioButton();
            this.tabHistory = new System.Windows.Forms.TabPage();
            this.txtLogHistory = new System.Windows.Forms.TextBox();
            this.tabPlayback = new System.Windows.Forms.TabPage();
            this.lblPbDataset = new System.Windows.Forms.Label();
            this.btnBrowseDataset = new System.Windows.Forms.Button();
            this.cmbEpisode = new System.Windows.Forms.ComboBox();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblPlaySpeed = new System.Windows.Forms.Label();
            this.trkSpeed = new System.Windows.Forms.TrackBar();
            this.trkFrame = new System.Windows.Forms.TrackBar();
            this.lblFrameInfo = new System.Windows.Forms.Label();
            this.lblPbWrist = new System.Windows.Forms.Label();
            this.lblPbExternal = new System.Windows.Forms.Label();
            this.picWrist = new System.Windows.Forms.PictureBox();
            this.picExternal = new System.Windows.Forms.PictureBox();
            this.lblPbQposTitle = new System.Windows.Forms.Label();
            this.picGraphQpos = new System.Windows.Forms.PictureBox();
            this.lblPbTgtTitle = new System.Windows.Forms.Label();
            this.picGraphTgt = new System.Windows.Forms.PictureBox();
            this.tabArmSetup = new System.Windows.Forms.TabPage();
            this.grpArmConn = new System.Windows.Forms.GroupBox();
            this.lblArmPort = new System.Windows.Forms.Label();
            this.txtArmPort = new System.Windows.Forms.TextBox();
            this.lblArmBaud = new System.Windows.Forms.Label();
            this.txtArmBaud = new System.Windows.Forms.TextBox();
            this.btnArmConnect = new System.Windows.Forms.Button();
            this.btnArmTorque = new System.Windows.Forms.Button();
            this.lblArmState = new System.Windows.Forms.Label();
            this.tabArmInner = new System.Windows.Forms.TabControl();
            this.tabArmPrac = new System.Windows.Forms.TabPage();
            this.lblPracGuide = new System.Windows.Forms.Label();
            this.chkPracPrecise = new System.Windows.Forms.CheckBox();
            this.chkPracDry = new System.Windows.Forms.CheckBox();
            this.btnPrac1 = new System.Windows.Forms.Button();
            this.btnPrac2 = new System.Windows.Forms.Button();
            this.btnPrac3 = new System.Windows.Forms.Button();
            this.btnPrac4 = new System.Windows.Forms.Button();
            this.lblPracStep = new System.Windows.Forms.Label();
            this.picPracChart = new System.Windows.Forms.PictureBox();
            this.lblPracResult = new System.Windows.Forms.Label();
            this.btnPracStop = new System.Windows.Forms.Button();
            this.lblPracBusy = new System.Windows.Forms.Label();
            this.lblPracDef = new System.Windows.Forms.Label();
            this.chkPracDefShow = new System.Windows.Forms.CheckBox();
            this.btnPracDef = new System.Windows.Forms.Button();
            this.lnkPracEye = new System.Windows.Forms.LinkLabel();
            this.tabArmMotor = new System.Windows.Forms.TabPage();
            this.dgvArmMotor = new System.Windows.Forms.DataGridView();
            this.colArmAxis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArmDir = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colArmCenter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArmLo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArmHi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArmMirror = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colArmModel = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colArmNowDeg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArmChecked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colArmNowTick = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArmRealModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArmNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArmGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArmBusId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArmMechMove = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArmMechAngle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArmInit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArmInit2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArmProto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnArmRead = new System.Windows.Forms.Button();
            this.btnArmJogMinus = new System.Windows.Forms.Button();
            this.txtArmJogStep = new System.Windows.Forms.TextBox();
            this.lblArmJogUnit = new System.Windows.Forms.Label();
            this.btnArmJogPlus = new System.Windows.Forms.Button();
            this.btnArmSetZero = new System.Windows.Forms.Button();
            this.btnArmSave = new System.Windows.Forms.Button();
            this.btnArmLoad = new System.Windows.Forms.Button();
            this.btnArmImportAuto = new System.Windows.Forms.Button();
            this.chkArmFollow = new System.Windows.Forms.CheckBox();
            this.chkArmAdvanced = new System.Windows.Forms.CheckBox();
            this.lblArmFile = new System.Windows.Forms.Label();
            this.lblArmHint = new System.Windows.Forms.Label();
            this.tabArmDef = new System.Windows.Forms.TabPage();
            this.lblDefGuide = new System.Windows.Forms.Label();
            this.lblDefPreset = new System.Windows.Forms.Label();
            this.cmbDefPreset = new System.Windows.Forms.ComboBox();
            this.btnDefPresetSave = new System.Windows.Forms.Button();
            this.lblDefTcp = new System.Windows.Forms.Label();
            this.cmbDefTcp = new System.Windows.Forms.ComboBox();
            this.lblDefTcpInfo = new System.Windows.Forms.Label();
            this.lblDefWrist = new System.Windows.Forms.Label();
            this.cmbDefWrist = new System.Windows.Forms.ComboBox();
            this.lblDefWristInfo = new System.Windows.Forms.Label();
            this.lblDefDown = new System.Windows.Forms.Label();
            this.lblDefDownSum = new System.Windows.Forms.Label();
            this.txtDefDownSum = new System.Windows.Forms.TextBox();
            this.lblDefDownInfo = new System.Windows.Forms.Label();
            this.lblDefShowNote = new System.Windows.Forms.Label();
            this.btnDefSave = new System.Windows.Forms.Button();
            this.lblDefStatus = new System.Windows.Forms.Label();
            this.chkDefAdv = new System.Windows.Forms.CheckBox();
            this.lblDefEye = new System.Windows.Forms.Label();
            this.btnDefEye = new System.Windows.Forms.Button();
            this.grpDefAdv = new System.Windows.Forms.GroupBox();
            this.lblDefName = new System.Windows.Forms.Label();
            this.txtDefName = new System.Windows.Forms.TextBox();
            this.lblDefBase = new System.Windows.Forms.Label();
            this.txtDefBase = new System.Windows.Forms.TextBox();
            this.lblDefPitch = new System.Windows.Forms.Label();
            this.txtDefPitch = new System.Windows.Forms.TextBox();
            this.lblDefHold = new System.Windows.Forms.Label();
            this.txtDefHold = new System.Windows.Forms.TextBox();
            this.lblDefTcpLen = new System.Windows.Forms.Label();
            this.txtDefTcpLen = new System.Windows.Forms.TextBox();
            this.lblDefFixed = new System.Windows.Forms.Label();
            this.tabArmExp = new System.Windows.Forms.TabPage();
            this.grpArmPlan = new System.Windows.Forms.GroupBox();
            this.btnArmPlanMake = new System.Windows.Forms.Button();
            this.btnArmPlanLoad = new System.Windows.Forms.Button();
            this.btnArmPlanDry = new System.Windows.Forms.Button();
            this.lblArmPlan = new System.Windows.Forms.Label();
            this.grpArmMeasure = new System.Windows.Forms.GroupBox();
            this.chkArmHold = new System.Windows.Forms.CheckBox();
            this.btnArmMeasure = new System.Windows.Forms.Button();
            this.lblArmMeasure = new System.Windows.Forms.Label();
            this.grpArmLearn = new System.Windows.Forms.GroupBox();
            this.lblArmData = new System.Windows.Forms.Label();
            this.lblArmDataInfo = new System.Windows.Forms.Label();
            this.chkArmUseHold = new System.Windows.Forms.CheckBox();
            this.lblArmRef = new System.Windows.Forms.Label();
            this.txtArmRef = new System.Windows.Forms.TextBox();
            this.btnArmLearn = new System.Windows.Forms.Button();
            this.lblArmLearn = new System.Windows.Forms.Label();
            this.grpArmTest = new System.Windows.Forms.GroupBox();
            this.lblArmResult = new System.Windows.Forms.Label();
            this.cmbArmResult = new System.Windows.Forms.ComboBox();
            this.btnArmResultBrowse = new System.Windows.Forms.Button();
            this.lblArmTouchRow = new System.Windows.Forms.Label();
            this.lblArmTestR = new System.Windows.Forms.Label();
            this.txtArmTestR = new System.Windows.Forms.TextBox();
            this.lblArmTestYaw = new System.Windows.Forms.Label();
            this.txtArmTestYaw = new System.Windows.Forms.TextBox();
            this.lblArmTestPitch = new System.Windows.Forms.Label();
            this.txtArmTestPitch = new System.Windows.Forms.TextBox();
            this.lblArmTestRep = new System.Windows.Forms.Label();
            this.txtArmTestRep = new System.Windows.Forms.TextBox();
            this.chkArmTestHold = new System.Windows.Forms.CheckBox();
            this.btnArmTestDry = new System.Windows.Forms.Button();
            this.btnArmTest = new System.Windows.Forms.Button();
            this.lblArmTest = new System.Windows.Forms.Label();
            this.grpArmApply = new System.Windows.Forms.GroupBox();
            this.btnArmApply = new System.Windows.Forms.Button();
            this.lblArmActive = new System.Windows.Forms.Label();
            this.lblArmBackup = new System.Windows.Forms.Label();
            this.cmbArmBackup = new System.Windows.Forms.ComboBox();
            this.btnArmRollback = new System.Windows.Forms.Button();
            this.grpArmTwin = new System.Windows.Forms.GroupBox();
            this.lblArmTwinSrc = new System.Windows.Forms.Label();
            this.cmbArmTwinSrc = new System.Windows.Forms.ComboBox();
            this.lblArmTwin = new System.Windows.Forms.Label();
            this.btnArmStop = new System.Windows.Forms.Button();
            this.btnArmOpenDir = new System.Windows.Forms.Button();
            this.lblArmBusy = new System.Windows.Forms.Label();
            this.tmrArmFollow = new System.Windows.Forms.Timer(this.components);
            this.tipArmExp = new System.Windows.Forms.ToolTip(this.components);
            this.btnUsdExport = new System.Windows.Forms.Button();
            this.btnSignCalib = new System.Windows.Forms.Button();
            this.btnSignClear = new System.Windows.Forms.Button();
            this.btnMechLimit = new System.Windows.Forms.Button();
            this.btnSim2Real = new System.Windows.Forms.Button();
            this.btnStandDroop = new System.Windows.Forms.Button();
            this.btnS2RGuide = new System.Windows.Forms.Button();
            this.btnIsaacCam = new System.Windows.Forms.Button();
            this.btnSagComp = new System.Windows.Forms.Button();
            this.btnLoadVmax = new System.Windows.Forms.Button();
            this.btnServoIdent = new System.Windows.Forms.Button();
            this.btnTipBound = new System.Windows.Forms.Button();
            this.btnPowerProf = new System.Windows.Forms.Button();
            this.btnViewMjcf = new System.Windows.Forms.Button();
            this.btnViewUsd = new System.Windows.Forms.Button();
            this.btnIsaacUsd = new System.Windows.Forms.Button();
            this.btnView1 = new System.Windows.Forms.Button();
            this.btnView2 = new System.Windows.Forms.Button();
            this.btnView3 = new System.Windows.Forms.Button();
            this.btnView4 = new System.Windows.Forms.Button();
            this.btnView5 = new System.Windows.Forms.Button();
            this.btnView6 = new System.Windows.Forms.Button();
            this.btnView7 = new System.Windows.Forms.Button();
            this.btnView8 = new System.Windows.Forms.Button();
            this.btnViewDefault = new System.Windows.Forms.Button();
            this.lblViewTX = new System.Windows.Forms.Label();
            this.txtViewTX = new System.Windows.Forms.TextBox();
            this.lblViewTY = new System.Windows.Forms.Label();
            this.txtViewTY = new System.Windows.Forms.TextBox();
            this.lblViewTZ = new System.Windows.Forms.Label();
            this.txtViewTZ = new System.Windows.Forms.TextBox();
            this.lblViewRX = new System.Windows.Forms.Label();
            this.txtViewRX = new System.Windows.Forms.TextBox();
            this.lblViewRY = new System.Windows.Forms.Label();
            this.txtViewRY = new System.Windows.Forms.TextBox();
            this.lblViewRZ = new System.Windows.Forms.Label();
            this.txtViewRZ = new System.Windows.Forms.TextBox();
            this.tmrViewSync = new System.Windows.Forms.Timer(this.components);
            this.btnExportUrdf = new System.Windows.Forms.Button();
            this.btnPyBullet = new System.Windows.Forms.Button();
            this.btnMuJoCo = new System.Windows.Forms.Button();
            this.btnIsaac = new System.Windows.Forms.Button();
            this.txtIsaacPath = new System.Windows.Forms.TextBox();
            this.lblIsaacPath = new System.Windows.Forms.Label();
            this.btnFormulaView = new System.Windows.Forms.Button();
            this.btnSendToSim = new System.Windows.Forms.Button();
            this.chkAutoSync = new System.Windows.Forms.CheckBox();
            this.tmrAutoSync = new System.Windows.Forms.Timer(this.components);
            this.btnCaptureAll = new System.Windows.Forms.Button();
            this.lblSpawnHeight = new System.Windows.Forms.Label();
            this.txtSpawnHeight = new System.Windows.Forms.TextBox();
            this.chkFixedBase = new System.Windows.Forms.CheckBox();
            this.chkExportScene = new System.Windows.Forms.CheckBox();
            this.chkShowAxis = new System.Windows.Forms.CheckBox();
            this.lblUrdfFolder = new System.Windows.Forms.Label();
            this.txtUrdfFolder = new System.Windows.Forms.TextBox();
            this.lblStlFolder = new System.Windows.Forms.Label();
            this.txtStlFolder = new System.Windows.Forms.TextBox();
            this.btnExportConfig = new System.Windows.Forms.Button();
            this.lblWristBoundary = new System.Windows.Forms.Label();
            this.cmbWristMotor = new System.Windows.Forms.ComboBox();
            this.lblWristHint = new System.Windows.Forms.Label();
            this.lblGripper = new System.Windows.Forms.Label();
            this.cmbGripperMotor = new System.Windows.Forms.ComboBox();
            this.lblGripperHint = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lblDhTitle = new System.Windows.Forms.Label();
            this.txtDhParams = new System.Windows.Forms.RichTextBox();
            this.btnApplyDh = new System.Windows.Forms.Button();
            this.lblViewMode = new System.Windows.Forms.Label();
            this.cmbViewMode = new System.Windows.Forms.ComboBox();
            this.lblSagTwin = new System.Windows.Forms.Label();
            this.cmbSagTwin = new System.Windows.Forms.ComboBox();
            this.chkSagCompOn = new System.Windows.Forms.CheckBox();
            this.lblSkeletonSize = new System.Windows.Forms.Label();
            this.txtSkeletonSize = new System.Windows.Forms.TextBox();
            this.lblAxisSize = new System.Windows.Forms.Label();
            this.txtAxisSize = new System.Windows.Forms.TextBox();
            this.cmbExample = new System.Windows.Forms.ComboBox();
            this.pnDhEdit = new System.Windows.Forms.Panel();
            this.lblDhEditTitle = new System.Windows.Forms.Label();
            this.lblEditA = new System.Windows.Forms.Label();
            this.txtEditA = new System.Windows.Forms.TextBox();
            this.lblEditD = new System.Windows.Forms.Label();
            this.txtEditD = new System.Windows.Forms.TextBox();
            this.lblEditTheta = new System.Windows.Forms.Label();
            this.txtEditTheta = new System.Windows.Forms.TextBox();
            this.lblEditAlpha = new System.Windows.Forms.Label();
            this.txtEditAlpha = new System.Windows.Forms.TextBox();
            this.lblEditAxis = new System.Windows.Forms.Label();
            this.txtEditAxis = new System.Windows.Forms.TextBox();
            this.lblEditDir = new System.Windows.Forms.Label();
            this.txtEditDir = new System.Windows.Forms.TextBox();
            this.lblEditInit = new System.Windows.Forms.Label();
            this.txtEditInit = new System.Windows.Forms.TextBox();
            this.lblEditStlFile = new System.Windows.Forms.Label();
            this.txtEditStlFile = new System.Windows.Forms.TextBox();
            this.lblEditStlX = new System.Windows.Forms.Label();
            this.txtEditStlX = new System.Windows.Forms.TextBox();
            this.lblEditStlY = new System.Windows.Forms.Label();
            this.txtEditStlY = new System.Windows.Forms.TextBox();
            this.lblEditStlZ = new System.Windows.Forms.Label();
            this.txtEditStlZ = new System.Windows.Forms.TextBox();
            this.lblEditStlPan = new System.Windows.Forms.Label();
            this.txtEditStlPan = new System.Windows.Forms.TextBox();
            this.lblEditStlTilt = new System.Windows.Forms.Label();
            this.txtEditStlTilt = new System.Windows.Forms.TextBox();
            this.lblEditStlSwing = new System.Windows.Forms.Label();
            this.txtEditStlSwing = new System.Windows.Forms.TextBox();
            this.btnEditApply = new System.Windows.Forms.Button();
            this.chkShowArrow = new System.Windows.Forms.CheckBox();
            this.txtArrowSize = new System.Windows.Forms.TextBox();
            this.chkShowBall = new System.Windows.Forms.CheckBox();
            this.txtBallSize = new System.Windows.Forms.TextBox();
            this.chkShowSphere = new System.Windows.Forms.CheckBox();
            this.txtSphereSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabRight.SuspendLayout();
            this.tabCreate.SuspendLayout();
            this.pnDragArea.SuspendLayout();
            this.tabTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkAngle)).BeginInit();
            this.tabHistory.SuspendLayout();
            this.tabPlayback.SuspendLayout();
            this.tabArmSetup.SuspendLayout();
            this.grpArmConn.SuspendLayout();
            this.tabArmInner.SuspendLayout();
            this.tabArmPrac.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPracChart)).BeginInit();
            this.tabArmMotor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArmMotor)).BeginInit();
            this.tabArmDef.SuspendLayout();
            this.grpDefAdv.SuspendLayout();
            this.tabArmExp.SuspendLayout();
            this.grpArmPlan.SuspendLayout();
            this.grpArmMeasure.SuspendLayout();
            this.grpArmLearn.SuspendLayout();
            this.grpArmTest.SuspendLayout();
            this.grpArmApply.SuspendLayout();
            this.grpArmTwin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWrist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picExternal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGraphQpos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGraphTgt)).BeginInit();
            this.pnDhEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnDisp
            // 
            this.pnDisp.Location = new System.Drawing.Point(5, 100);
            this.pnDisp.Margin = new System.Windows.Forms.Padding(4);
            this.pnDisp.Name = "pnDisp";
            this.pnDisp.Size = new System.Drawing.Size(500, 505);
            this.pnDisp.TabIndex = 0;
            // 
            // tmrDisp
            // 
            this.tmrDisp.Enabled = true;
            this.tmrDisp.Interval = 33;
            this.tmrDisp.Tick += new System.EventHandler(this.tmrDisp_Tick);
            // 
            // tabRight
            // 
            this.tabRight.Controls.Add(this.tabCreate);
            this.tabRight.Controls.Add(this.tabTest);
            this.tabRight.Controls.Add(this.tabHistory);
            this.tabRight.Controls.Add(this.tabPlayback);
            this.tabRight.Controls.Add(this.tabArmSetup);
            this.tabRight.Location = new System.Drawing.Point(510, 5);
            this.tabRight.Margin = new System.Windows.Forms.Padding(4);
            this.tabRight.Name = "tabRight";
            this.tabRight.SelectedIndex = 0;
            this.tabRight.Size = new System.Drawing.Size(565, 600);
            this.tabRight.TabIndex = 1;
            this.tabRight.SelectedIndexChanged += new System.EventHandler(this.tabRight_SelectedIndexChanged);
            this.tabRight.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabRight_Selecting);
            // 
            // tabCreate
            // 
            this.tabCreate.Controls.Add(this.rdModeMouse);
            this.tabCreate.Controls.Add(this.rdModeButton);
            this.tabCreate.Controls.Add(this.rdModeHybrid);
            this.tabCreate.Controls.Add(this.lblMoveInput);
            this.tabCreate.Controls.Add(this.txtMoveValue);
            this.tabCreate.Controls.Add(this.lblRotateInput);
            this.tabCreate.Controls.Add(this.txtRotateValue);
            this.tabCreate.Controls.Add(this.pnDragArea);
            this.tabCreate.Controls.Add(this.lblMoveGroup);
            this.tabCreate.Controls.Add(this.txtStateXMove);
            this.tabCreate.Controls.Add(this.txtStateZMove);
            this.tabCreate.Controls.Add(this.txtStateYMove);
            this.tabCreate.Controls.Add(this.txtStateXRotate);
            this.tabCreate.Controls.Add(this.txtStateZRotate);
            this.tabCreate.Controls.Add(this.txtStateYRotate);
            this.tabCreate.Controls.Add(this.lblStateJoint);
            this.tabCreate.Controls.Add(this.txtStateJoint);
            this.tabCreate.Controls.Add(this.chkStateInit);
            this.tabCreate.Controls.Add(this.lblStateDir);
            this.tabCreate.Controls.Add(this.cmbStateDir);
            this.tabCreate.Controls.Add(this.lblStateNote);
            this.tabCreate.Controls.Add(this.btnRedMove);
            this.tabCreate.Controls.Add(this.btnBlueMove);
            this.tabCreate.Controls.Add(this.btnGreenMove);
            this.tabCreate.Controls.Add(this.lblRotateGroup);
            this.tabCreate.Controls.Add(this.btnRedRotate);
            this.tabCreate.Controls.Add(this.btnBlueRotate);
            this.tabCreate.Controls.Add(this.btnGreenRotate);
            this.tabCreate.Controls.Add(this.btnRedMoveM10);
            this.tabCreate.Controls.Add(this.btnRedMoveP10);
            this.tabCreate.Controls.Add(this.btnRedMoveMTxt);
            this.tabCreate.Controls.Add(this.btnRedMovePTxt);
            this.tabCreate.Controls.Add(this.btnBlueMoveM10);
            this.tabCreate.Controls.Add(this.btnBlueMoveP10);
            this.tabCreate.Controls.Add(this.btnBlueMoveMTxt);
            this.tabCreate.Controls.Add(this.btnBlueMovePTxt);
            this.tabCreate.Controls.Add(this.btnGreenMoveM10);
            this.tabCreate.Controls.Add(this.btnGreenMoveP10);
            this.tabCreate.Controls.Add(this.btnGreenMoveMTxt);
            this.tabCreate.Controls.Add(this.btnGreenMovePTxt);
            this.tabCreate.Controls.Add(this.btnRedRotateM90);
            this.tabCreate.Controls.Add(this.btnRedRotateP90);
            this.tabCreate.Controls.Add(this.btnRedRotateMTxt);
            this.tabCreate.Controls.Add(this.btnRedRotatePTxt);
            this.tabCreate.Controls.Add(this.btnBlueRotateM90);
            this.tabCreate.Controls.Add(this.btnBlueRotateP90);
            this.tabCreate.Controls.Add(this.btnBlueRotateMTxt);
            this.tabCreate.Controls.Add(this.btnBlueRotatePTxt);
            this.tabCreate.Controls.Add(this.btnGreenRotateM90);
            this.tabCreate.Controls.Add(this.btnGreenRotateP90);
            this.tabCreate.Controls.Add(this.btnGreenRotateMTxt);
            this.tabCreate.Controls.Add(this.btnGreenRotatePTxt);
            this.tabCreate.Controls.Add(this.btnMotor);
            this.tabCreate.Controls.Add(this.cmbMotorId);
            this.tabCreate.Controls.Add(this.btnInit);
            this.tabCreate.Controls.Add(this.btnStl);
            this.tabCreate.Controls.Add(this.btnStlCenter);
            this.tabCreate.Controls.Add(this.btnStlMerge);
            this.tabCreate.Controls.Add(this.lblStatusTitle);
            this.tabCreate.Controls.Add(this.lblStatus);
            this.tabCreate.Controls.Add(this.lblCurrentValue);
            this.tabCreate.Controls.Add(this.lblHistoryTitle);
            this.tabCreate.Controls.Add(this.lstHistory);
            this.tabCreate.Controls.Add(this.btnUndo);
            this.tabCreate.Controls.Add(this.btnDelete);
            this.tabCreate.Controls.Add(this.btnClear);
            this.tabCreate.Controls.Add(this.lblEtcGroup);
            this.tabCreate.Controls.Add(this.label1);
            this.tabCreate.Location = new System.Drawing.Point(4, 28);
            this.tabCreate.Margin = new System.Windows.Forms.Padding(4);
            this.tabCreate.Name = "tabCreate";
            this.tabCreate.Size = new System.Drawing.Size(557, 568);
            this.tabCreate.TabIndex = 0;
            this.tabCreate.Text = "Build";
            // 
            // rdModeMouse
            // 
            this.rdModeMouse.AutoSize = true;
            this.rdModeMouse.Location = new System.Drawing.Point(5, 8);
            this.rdModeMouse.Margin = new System.Windows.Forms.Padding(4);
            this.rdModeMouse.Name = "rdModeMouse";
            this.rdModeMouse.Size = new System.Drawing.Size(89, 22);
            this.rdModeMouse.TabIndex = 0;
            this.rdModeMouse.Text = "Mouse";
            this.rdModeMouse.UseVisualStyleBackColor = true;
            this.rdModeMouse.CheckedChanged += new System.EventHandler(this.rdModeMouse_CheckedChanged);
            // 
            // rdModeButton
            // 
            this.rdModeButton.AutoSize = true;
            this.rdModeButton.Checked = true;
            this.rdModeButton.Location = new System.Drawing.Point(105, 8);
            this.rdModeButton.Margin = new System.Windows.Forms.Padding(4);
            this.rdModeButton.Name = "rdModeButton";
            this.rdModeButton.Size = new System.Drawing.Size(71, 22);
            this.rdModeButton.TabIndex = 1;
            this.rdModeButton.TabStop = true;
            this.rdModeButton.Text = "Input";
            this.rdModeButton.UseVisualStyleBackColor = true;
            this.rdModeButton.CheckedChanged += new System.EventHandler(this.rdModeButton_CheckedChanged);
            // 
            // rdModeHybrid
            // 
            this.rdModeHybrid.AutoSize = true;
            this.rdModeHybrid.Location = new System.Drawing.Point(221, 8);
            this.rdModeHybrid.Margin = new System.Windows.Forms.Padding(4);
            this.rdModeHybrid.Name = "rdModeHybrid";
            this.rdModeHybrid.Size = new System.Drawing.Size(70, 22);
            this.rdModeHybrid.TabIndex = 2;
            this.rdModeHybrid.Text = "Drag";
            this.rdModeHybrid.UseVisualStyleBackColor = true;
            this.rdModeHybrid.CheckedChanged += new System.EventHandler(this.rdModeHybrid_CheckedChanged);
            // 
            // lblMoveInput
            // 
            this.lblMoveInput.Location = new System.Drawing.Point(2, 34);
            this.lblMoveInput.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMoveInput.Name = "lblMoveInput";
            this.lblMoveInput.Size = new System.Drawing.Size(102, 36);
            this.lblMoveInput.TabIndex = 30;
            this.lblMoveInput.Text = "Move(mm)";
            this.lblMoveInput.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMoveInput.Visible = false;
            // 
            // txtMoveValue
            // 
            this.txtMoveValue.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtMoveValue.Location = new System.Drawing.Point(107, 34);
            this.txtMoveValue.Margin = new System.Windows.Forms.Padding(4);
            this.txtMoveValue.Name = "txtMoveValue";
            this.txtMoveValue.Size = new System.Drawing.Size(112, 39);
            this.txtMoveValue.TabIndex = 3;
            this.txtMoveValue.Text = "100";
            this.txtMoveValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMoveValue.Visible = false;
            // 
            // lblRotateInput
            // 
            this.lblRotateInput.Location = new System.Drawing.Point(220, 34);
            this.lblRotateInput.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRotateInput.Name = "lblRotateInput";
            this.lblRotateInput.Size = new System.Drawing.Size(102, 36);
            this.lblRotateInput.TabIndex = 31;
            this.lblRotateInput.Text = "Rot(deg)";
            this.lblRotateInput.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRotateInput.Visible = false;
            // 
            // txtRotateValue
            // 
            this.txtRotateValue.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtRotateValue.Location = new System.Drawing.Point(340, 34);
            this.txtRotateValue.Margin = new System.Windows.Forms.Padding(4);
            this.txtRotateValue.Name = "txtRotateValue";
            this.txtRotateValue.Size = new System.Drawing.Size(112, 39);
            this.txtRotateValue.TabIndex = 4;
            this.txtRotateValue.Text = "90";
            this.txtRotateValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRotateValue.Visible = false;
            // 
            // pnDragArea
            // 
            this.pnDragArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.pnDragArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnDragArea.Controls.Add(this.lblDragArea);
            this.pnDragArea.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.pnDragArea.Location = new System.Drawing.Point(0, 34);
            this.pnDragArea.Margin = new System.Windows.Forms.Padding(4);
            this.pnDragArea.Name = "pnDragArea";
            this.pnDragArea.Size = new System.Drawing.Size(434, 50);
            this.pnDragArea.TabIndex = 4;
            this.pnDragArea.Visible = false;
            this.pnDragArea.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnDragArea_MouseDown);
            this.pnDragArea.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnDragArea_MouseMove);
            this.pnDragArea.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnDragArea_MouseUp);
            // 
            // lblDragArea
            // 
            this.lblDragArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDragArea.ForeColor = System.Drawing.Color.Gray;
            this.lblDragArea.Location = new System.Drawing.Point(0, 0);
            this.lblDragArea.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDragArea.Name = "lblDragArea";
            this.lblDragArea.Size = new System.Drawing.Size(432, 48);
            this.lblDragArea.TabIndex = 0;
            this.lblDragArea.Text = "Select axis, then drag here";
            this.lblDragArea.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDragArea.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnDragArea_MouseDown);
            this.lblDragArea.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnDragArea_MouseMove);
            this.lblDragArea.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnDragArea_MouseUp);
            // 
            // lblMoveGroup
            // 
            this.lblMoveGroup.Location = new System.Drawing.Point(0, 85);
            this.lblMoveGroup.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMoveGroup.Name = "lblMoveGroup";
            this.lblMoveGroup.Size = new System.Drawing.Size(450, 24);
            this.lblMoveGroup.TabIndex = 5;
            this.lblMoveGroup.Text = "──── Move(click하고 3d 화면에서 scrolling) ────";
            this.lblMoveGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtStateXMove
            // 
            this.txtStateXMove.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtStateXMove.Location = new System.Drawing.Point(0, 142);
            this.txtStateXMove.Margin = new System.Windows.Forms.Padding(4);
            this.txtStateXMove.Name = "txtStateXMove";
            this.txtStateXMove.Size = new System.Drawing.Size(88, 28);
            this.txtStateXMove.TabIndex = 301;
            this.txtStateXMove.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtStateXMove.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StateField_KeyDown);
            this.txtStateXMove.Leave += new System.EventHandler(this.StateField_Leave);
            // 
            // txtStateZMove
            // 
            this.txtStateZMove.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtStateZMove.Location = new System.Drawing.Point(184, 142);
            this.txtStateZMove.Margin = new System.Windows.Forms.Padding(4);
            this.txtStateZMove.Name = "txtStateZMove";
            this.txtStateZMove.Size = new System.Drawing.Size(88, 28);
            this.txtStateZMove.TabIndex = 302;
            this.txtStateZMove.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtStateZMove.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StateField_KeyDown);
            this.txtStateZMove.Leave += new System.EventHandler(this.StateField_Leave);
            // 
            // txtStateYMove
            // 
            this.txtStateYMove.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtStateYMove.Location = new System.Drawing.Point(368, 142);
            this.txtStateYMove.Margin = new System.Windows.Forms.Padding(4);
            this.txtStateYMove.Name = "txtStateYMove";
            this.txtStateYMove.Size = new System.Drawing.Size(88, 28);
            this.txtStateYMove.TabIndex = 303;
            this.txtStateYMove.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtStateYMove.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StateField_KeyDown);
            this.txtStateYMove.Leave += new System.EventHandler(this.StateField_Leave);
            // 
            // txtStateXRotate
            // 
            this.txtStateXRotate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtStateXRotate.Location = new System.Drawing.Point(0, 231);
            this.txtStateXRotate.Margin = new System.Windows.Forms.Padding(4);
            this.txtStateXRotate.Name = "txtStateXRotate";
            this.txtStateXRotate.Size = new System.Drawing.Size(88, 28);
            this.txtStateXRotate.TabIndex = 304;
            this.txtStateXRotate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtStateXRotate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StateField_KeyDown);
            this.txtStateXRotate.Leave += new System.EventHandler(this.StateField_Leave);
            // 
            // txtStateZRotate
            // 
            this.txtStateZRotate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtStateZRotate.Location = new System.Drawing.Point(184, 231);
            this.txtStateZRotate.Margin = new System.Windows.Forms.Padding(4);
            this.txtStateZRotate.Name = "txtStateZRotate";
            this.txtStateZRotate.Size = new System.Drawing.Size(88, 28);
            this.txtStateZRotate.TabIndex = 305;
            this.txtStateZRotate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtStateZRotate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StateField_KeyDown);
            this.txtStateZRotate.Leave += new System.EventHandler(this.StateField_Leave);
            // 
            // txtStateYRotate
            // 
            this.txtStateYRotate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtStateYRotate.Location = new System.Drawing.Point(368, 231);
            this.txtStateYRotate.Margin = new System.Windows.Forms.Padding(4);
            this.txtStateYRotate.Name = "txtStateYRotate";
            this.txtStateYRotate.Size = new System.Drawing.Size(88, 28);
            this.txtStateYRotate.TabIndex = 306;
            this.txtStateYRotate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtStateYRotate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StateField_KeyDown);
            this.txtStateYRotate.Leave += new System.EventHandler(this.StateField_Leave);
            // 
            // lblStateJoint
            // 
            this.lblStateJoint.Location = new System.Drawing.Point(0, 266);
            this.lblStateJoint.Margin = new System.Windows.Forms.Padding(4);
            this.lblStateJoint.Name = "lblStateJoint";
            this.lblStateJoint.Size = new System.Drawing.Size(38, 20);
            this.lblStateJoint.TabIndex = 307;
            this.lblStateJoint.Text = "joint";
            this.lblStateJoint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblStateJoint.ForeColor = System.Drawing.Color.DimGray;
            // 
            // txtStateJoint
            // 
            this.txtStateJoint.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtStateJoint.Location = new System.Drawing.Point(40, 262);
            this.txtStateJoint.Margin = new System.Windows.Forms.Padding(4);
            this.txtStateJoint.Name = "txtStateJoint";
            this.txtStateJoint.Size = new System.Drawing.Size(56, 28);
            this.txtStateJoint.TabIndex = 308;
            this.txtStateJoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtStateJoint.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StateField_KeyDown);
            this.txtStateJoint.Leave += new System.EventHandler(this.StateField_Leave);
            // 
            // chkStateInit
            // 
            this.chkStateInit.Location = new System.Drawing.Point(104, 264);
            this.chkStateInit.Margin = new System.Windows.Forms.Padding(4);
            this.chkStateInit.Name = "chkStateInit";
            this.chkStateInit.Size = new System.Drawing.Size(150, 24);
            this.chkStateInit.TabIndex = 309;
            this.chkStateInit.Text = "New Chain(Init)";
            this.chkStateInit.UseVisualStyleBackColor = true;
            this.chkStateInit.CheckedChanged += new System.EventHandler(this.StateMeta_Changed);
            // 
            // lblStateDir
            // 
            this.lblStateDir.Location = new System.Drawing.Point(258, 266);
            this.lblStateDir.Margin = new System.Windows.Forms.Padding(4);
            this.lblStateDir.Name = "lblStateDir";
            this.lblStateDir.Size = new System.Drawing.Size(26, 20);
            this.lblStateDir.TabIndex = 310;
            this.lblStateDir.Text = "dir";
            this.lblStateDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblStateDir.ForeColor = System.Drawing.Color.DimGray;
            // 
            // cmbStateDir
            // 
            this.cmbStateDir.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbStateDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStateDir.Location = new System.Drawing.Point(286, 262);
            this.cmbStateDir.Margin = new System.Windows.Forms.Padding(4);
            this.cmbStateDir.Name = "cmbStateDir";
            this.cmbStateDir.Size = new System.Drawing.Size(152, 28);
            this.cmbStateDir.TabIndex = 311;
            this.cmbStateDir.SelectedIndexChanged += new System.EventHandler(this.StateMeta_Changed);
            // 
            // lblStateNote
            // 
            this.lblStateNote.Location = new System.Drawing.Point(444, 266);
            this.lblStateNote.Margin = new System.Windows.Forms.Padding(4);
            this.lblStateNote.Name = "lblStateNote";
            this.lblStateNote.Size = new System.Drawing.Size(102, 20);
            this.lblStateNote.TabIndex = 312;
            this.lblStateNote.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblStateNote.ForeColor = System.Drawing.Color.DimGray;
            // 
            // btnRedMove
            // 
            this.btnRedMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnRedMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedMove.ForeColor = System.Drawing.Color.White;
            this.btnRedMove.Location = new System.Drawing.Point(0, 112);
            this.btnRedMove.Margin = new System.Windows.Forms.Padding(4);
            this.btnRedMove.Name = "btnRedMove";
            this.btnRedMove.Size = new System.Drawing.Size(88, 28);
            this.btnRedMove.TabIndex = 6;
            this.btnRedMove.Text = "X Move";
            this.btnRedMove.UseVisualStyleBackColor = false;
            this.btnRedMove.Click += new System.EventHandler(this.btnRedMove_Click);
            // 
            // btnBlueMove
            // 
            this.btnBlueMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(255)))));
            this.btnBlueMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlueMove.ForeColor = System.Drawing.Color.White;
            this.btnBlueMove.Location = new System.Drawing.Point(184, 112);
            this.btnBlueMove.Margin = new System.Windows.Forms.Padding(4);
            this.btnBlueMove.Name = "btnBlueMove";
            this.btnBlueMove.Size = new System.Drawing.Size(88, 28);
            this.btnBlueMove.TabIndex = 7;
            this.btnBlueMove.Text = "Z Move";
            this.btnBlueMove.UseVisualStyleBackColor = false;
            this.btnBlueMove.Click += new System.EventHandler(this.btnBlueMove_Click);
            // 
            // btnGreenMove
            // 
            this.btnGreenMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(200)))), ((int)(((byte)(100)))));
            this.btnGreenMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGreenMove.ForeColor = System.Drawing.Color.White;
            this.btnGreenMove.Location = new System.Drawing.Point(368, 112);
            this.btnGreenMove.Margin = new System.Windows.Forms.Padding(4);
            this.btnGreenMove.Name = "btnGreenMove";
            this.btnGreenMove.Size = new System.Drawing.Size(88, 28);
            this.btnGreenMove.TabIndex = 8;
            this.btnGreenMove.Text = "Y Move";
            this.btnGreenMove.UseVisualStyleBackColor = false;
            this.btnGreenMove.Click += new System.EventHandler(this.btnGreenMove_Click);
            // 
            // lblRotateGroup
            // 
            this.lblRotateGroup.Location = new System.Drawing.Point(0, 175);
            this.lblRotateGroup.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRotateGroup.Name = "lblRotateGroup";
            this.lblRotateGroup.Size = new System.Drawing.Size(452, 24);
            this.lblRotateGroup.TabIndex = 9;
            this.lblRotateGroup.Text = "──── Rotate(click하고 3d 화면에서 scrolling) ────";
            this.lblRotateGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRedRotate
            // 
            this.btnRedRotate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnRedRotate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedRotate.ForeColor = System.Drawing.Color.White;
            this.btnRedRotate.Location = new System.Drawing.Point(0, 201);
            this.btnRedRotate.Margin = new System.Windows.Forms.Padding(4);
            this.btnRedRotate.Name = "btnRedRotate";
            this.btnRedRotate.Size = new System.Drawing.Size(88, 28);
            this.btnRedRotate.TabIndex = 10;
            this.btnRedRotate.Text = "X Rotate";
            this.btnRedRotate.UseVisualStyleBackColor = false;
            this.btnRedRotate.Click += new System.EventHandler(this.btnRedRotate_Click);
            // 
            // btnBlueRotate
            // 
            this.btnBlueRotate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(255)))));
            this.btnBlueRotate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlueRotate.ForeColor = System.Drawing.Color.White;
            this.btnBlueRotate.Location = new System.Drawing.Point(184, 201);
            this.btnBlueRotate.Margin = new System.Windows.Forms.Padding(4);
            this.btnBlueRotate.Name = "btnBlueRotate";
            this.btnBlueRotate.Size = new System.Drawing.Size(88, 28);
            this.btnBlueRotate.TabIndex = 11;
            this.btnBlueRotate.Text = "Z Rotate";
            this.btnBlueRotate.UseVisualStyleBackColor = false;
            this.btnBlueRotate.Click += new System.EventHandler(this.btnBlueRotate_Click);
            // 
            // btnGreenRotate
            // 
            this.btnGreenRotate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(200)))), ((int)(((byte)(100)))));
            this.btnGreenRotate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGreenRotate.ForeColor = System.Drawing.Color.White;
            this.btnGreenRotate.Location = new System.Drawing.Point(368, 201);
            this.btnGreenRotate.Margin = new System.Windows.Forms.Padding(4);
            this.btnGreenRotate.Name = "btnGreenRotate";
            this.btnGreenRotate.Size = new System.Drawing.Size(88, 28);
            this.btnGreenRotate.TabIndex = 12;
            this.btnGreenRotate.Text = "Y Rotate";
            this.btnGreenRotate.UseVisualStyleBackColor = false;
            this.btnGreenRotate.Click += new System.EventHandler(this.btnGreenRotate_Click);
            // 
            // btnRedMoveM10
            // 
            this.btnRedMoveM10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(82)))), ((int)(((byte)(82)))));
            this.btnRedMoveM10.FlatAppearance.BorderSize = 0;
            this.btnRedMoveM10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedMoveM10.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnRedMoveM10.ForeColor = System.Drawing.Color.White;
            this.btnRedMoveM10.Location = new System.Drawing.Point(88, 112);
            this.btnRedMoveM10.Margin = new System.Windows.Forms.Padding(0);
            this.btnRedMoveM10.Name = "btnRedMoveM10";
            this.btnRedMoveM10.Size = new System.Drawing.Size(44, 28);
            this.btnRedMoveM10.TabIndex = 201;
            this.btnRedMoveM10.Tag = "RedMove,-1,10";
            this.btnRedMoveM10.Text = "-10";
            this.btnRedMoveM10.UseVisualStyleBackColor = false;
            this.btnRedMoveM10.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnRedMoveP10
            // 
            this.btnRedMoveP10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(82)))), ((int)(((byte)(82)))));
            this.btnRedMoveP10.FlatAppearance.BorderSize = 0;
            this.btnRedMoveP10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedMoveP10.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnRedMoveP10.ForeColor = System.Drawing.Color.White;
            this.btnRedMoveP10.Location = new System.Drawing.Point(134, 112);
            this.btnRedMoveP10.Margin = new System.Windows.Forms.Padding(0);
            this.btnRedMoveP10.Name = "btnRedMoveP10";
            this.btnRedMoveP10.Size = new System.Drawing.Size(44, 28);
            this.btnRedMoveP10.TabIndex = 202;
            this.btnRedMoveP10.Tag = "RedMove,1,10";
            this.btnRedMoveP10.Text = "+10";
            this.btnRedMoveP10.UseVisualStyleBackColor = false;
            this.btnRedMoveP10.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnRedMoveMTxt
            // 
            this.btnRedMoveMTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(133)))), ((int)(((byte)(133)))));
            this.btnRedMoveMTxt.FlatAppearance.BorderSize = 0;
            this.btnRedMoveMTxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedMoveMTxt.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnRedMoveMTxt.ForeColor = System.Drawing.Color.White;
            this.btnRedMoveMTxt.Location = new System.Drawing.Point(88, 142);
            this.btnRedMoveMTxt.Margin = new System.Windows.Forms.Padding(0);
            this.btnRedMoveMTxt.Name = "btnRedMoveMTxt";
            this.btnRedMoveMTxt.Size = new System.Drawing.Size(44, 28);
            this.btnRedMoveMTxt.TabIndex = 203;
            this.btnRedMoveMTxt.Tag = "RedMove,-1,0";
            this.btnRedMoveMTxt.Text = "-";
            this.btnRedMoveMTxt.UseVisualStyleBackColor = false;
            this.btnRedMoveMTxt.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnRedMovePTxt
            // 
            this.btnRedMovePTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(133)))), ((int)(((byte)(133)))));
            this.btnRedMovePTxt.FlatAppearance.BorderSize = 0;
            this.btnRedMovePTxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedMovePTxt.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnRedMovePTxt.ForeColor = System.Drawing.Color.White;
            this.btnRedMovePTxt.Location = new System.Drawing.Point(134, 142);
            this.btnRedMovePTxt.Margin = new System.Windows.Forms.Padding(0);
            this.btnRedMovePTxt.Name = "btnRedMovePTxt";
            this.btnRedMovePTxt.Size = new System.Drawing.Size(44, 28);
            this.btnRedMovePTxt.TabIndex = 204;
            this.btnRedMovePTxt.Tag = "RedMove,1,0";
            this.btnRedMovePTxt.Text = "+";
            this.btnRedMovePTxt.UseVisualStyleBackColor = false;
            this.btnRedMovePTxt.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnBlueMoveM10
            // 
            this.btnBlueMoveM10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(82)))), ((int)(((byte)(231)))));
            this.btnBlueMoveM10.FlatAppearance.BorderSize = 0;
            this.btnBlueMoveM10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlueMoveM10.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnBlueMoveM10.ForeColor = System.Drawing.Color.White;
            this.btnBlueMoveM10.Location = new System.Drawing.Point(272, 112);
            this.btnBlueMoveM10.Margin = new System.Windows.Forms.Padding(0);
            this.btnBlueMoveM10.Name = "btnBlueMoveM10";
            this.btnBlueMoveM10.Size = new System.Drawing.Size(44, 28);
            this.btnBlueMoveM10.TabIndex = 205;
            this.btnBlueMoveM10.Tag = "BlueMove,-1,10";
            this.btnBlueMoveM10.Text = "-10";
            this.btnBlueMoveM10.UseVisualStyleBackColor = false;
            this.btnBlueMoveM10.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnBlueMoveP10
            // 
            this.btnBlueMoveP10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(82)))), ((int)(((byte)(231)))));
            this.btnBlueMoveP10.FlatAppearance.BorderSize = 0;
            this.btnBlueMoveP10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlueMoveP10.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnBlueMoveP10.ForeColor = System.Drawing.Color.White;
            this.btnBlueMoveP10.Location = new System.Drawing.Point(318, 112);
            this.btnBlueMoveP10.Margin = new System.Windows.Forms.Padding(0);
            this.btnBlueMoveP10.Name = "btnBlueMoveP10";
            this.btnBlueMoveP10.Size = new System.Drawing.Size(44, 28);
            this.btnBlueMoveP10.TabIndex = 206;
            this.btnBlueMoveP10.Tag = "BlueMove,1,10";
            this.btnBlueMoveP10.Text = "+10";
            this.btnBlueMoveP10.UseVisualStyleBackColor = false;
            this.btnBlueMoveP10.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnBlueMoveMTxt
            // 
            this.btnBlueMoveMTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(133)))), ((int)(((byte)(255)))));
            this.btnBlueMoveMTxt.FlatAppearance.BorderSize = 0;
            this.btnBlueMoveMTxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlueMoveMTxt.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnBlueMoveMTxt.ForeColor = System.Drawing.Color.White;
            this.btnBlueMoveMTxt.Location = new System.Drawing.Point(272, 142);
            this.btnBlueMoveMTxt.Margin = new System.Windows.Forms.Padding(0);
            this.btnBlueMoveMTxt.Name = "btnBlueMoveMTxt";
            this.btnBlueMoveMTxt.Size = new System.Drawing.Size(44, 28);
            this.btnBlueMoveMTxt.TabIndex = 207;
            this.btnBlueMoveMTxt.Tag = "BlueMove,-1,0";
            this.btnBlueMoveMTxt.Text = "-";
            this.btnBlueMoveMTxt.UseVisualStyleBackColor = false;
            this.btnBlueMoveMTxt.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnBlueMovePTxt
            // 
            this.btnBlueMovePTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(133)))), ((int)(((byte)(255)))));
            this.btnBlueMovePTxt.FlatAppearance.BorderSize = 0;
            this.btnBlueMovePTxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlueMovePTxt.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnBlueMovePTxt.ForeColor = System.Drawing.Color.White;
            this.btnBlueMovePTxt.Location = new System.Drawing.Point(318, 142);
            this.btnBlueMovePTxt.Margin = new System.Windows.Forms.Padding(0);
            this.btnBlueMovePTxt.Name = "btnBlueMovePTxt";
            this.btnBlueMovePTxt.Size = new System.Drawing.Size(44, 28);
            this.btnBlueMovePTxt.TabIndex = 208;
            this.btnBlueMovePTxt.Tag = "BlueMove,1,0";
            this.btnBlueMovePTxt.Text = "+";
            this.btnBlueMovePTxt.UseVisualStyleBackColor = false;
            this.btnBlueMovePTxt.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnGreenMoveM10
            // 
            this.btnGreenMoveM10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(176)))), ((int)(((byte)(82)))));
            this.btnGreenMoveM10.FlatAppearance.BorderSize = 0;
            this.btnGreenMoveM10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGreenMoveM10.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnGreenMoveM10.ForeColor = System.Drawing.Color.White;
            this.btnGreenMoveM10.Location = new System.Drawing.Point(456, 112);
            this.btnGreenMoveM10.Margin = new System.Windows.Forms.Padding(0);
            this.btnGreenMoveM10.Name = "btnGreenMoveM10";
            this.btnGreenMoveM10.Size = new System.Drawing.Size(44, 28);
            this.btnGreenMoveM10.TabIndex = 209;
            this.btnGreenMoveM10.Tag = "GreenMove,-1,10";
            this.btnGreenMoveM10.Text = "-10";
            this.btnGreenMoveM10.UseVisualStyleBackColor = false;
            this.btnGreenMoveM10.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnGreenMoveP10
            // 
            this.btnGreenMoveP10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(176)))), ((int)(((byte)(82)))));
            this.btnGreenMoveP10.FlatAppearance.BorderSize = 0;
            this.btnGreenMoveP10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGreenMoveP10.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnGreenMoveP10.ForeColor = System.Drawing.Color.White;
            this.btnGreenMoveP10.Location = new System.Drawing.Point(502, 112);
            this.btnGreenMoveP10.Margin = new System.Windows.Forms.Padding(0);
            this.btnGreenMoveP10.Name = "btnGreenMoveP10";
            this.btnGreenMoveP10.Size = new System.Drawing.Size(44, 28);
            this.btnGreenMoveP10.TabIndex = 210;
            this.btnGreenMoveP10.Tag = "GreenMove,1,10";
            this.btnGreenMoveP10.Text = "+10";
            this.btnGreenMoveP10.UseVisualStyleBackColor = false;
            this.btnGreenMoveP10.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnGreenMoveMTxt
            // 
            this.btnGreenMoveMTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(216)))), ((int)(((byte)(133)))));
            this.btnGreenMoveMTxt.FlatAppearance.BorderSize = 0;
            this.btnGreenMoveMTxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGreenMoveMTxt.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnGreenMoveMTxt.ForeColor = System.Drawing.Color.White;
            this.btnGreenMoveMTxt.Location = new System.Drawing.Point(456, 142);
            this.btnGreenMoveMTxt.Margin = new System.Windows.Forms.Padding(0);
            this.btnGreenMoveMTxt.Name = "btnGreenMoveMTxt";
            this.btnGreenMoveMTxt.Size = new System.Drawing.Size(44, 28);
            this.btnGreenMoveMTxt.TabIndex = 211;
            this.btnGreenMoveMTxt.Tag = "GreenMove,-1,0";
            this.btnGreenMoveMTxt.Text = "-";
            this.btnGreenMoveMTxt.UseVisualStyleBackColor = false;
            this.btnGreenMoveMTxt.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnGreenMovePTxt
            // 
            this.btnGreenMovePTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(216)))), ((int)(((byte)(133)))));
            this.btnGreenMovePTxt.FlatAppearance.BorderSize = 0;
            this.btnGreenMovePTxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGreenMovePTxt.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnGreenMovePTxt.ForeColor = System.Drawing.Color.White;
            this.btnGreenMovePTxt.Location = new System.Drawing.Point(502, 142);
            this.btnGreenMovePTxt.Margin = new System.Windows.Forms.Padding(0);
            this.btnGreenMovePTxt.Name = "btnGreenMovePTxt";
            this.btnGreenMovePTxt.Size = new System.Drawing.Size(44, 28);
            this.btnGreenMovePTxt.TabIndex = 212;
            this.btnGreenMovePTxt.Tag = "GreenMove,1,0";
            this.btnGreenMovePTxt.Text = "+";
            this.btnGreenMovePTxt.UseVisualStyleBackColor = false;
            this.btnGreenMovePTxt.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnRedRotateM90
            // 
            this.btnRedRotateM90.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(82)))), ((int)(((byte)(82)))));
            this.btnRedRotateM90.FlatAppearance.BorderSize = 0;
            this.btnRedRotateM90.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedRotateM90.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnRedRotateM90.ForeColor = System.Drawing.Color.White;
            this.btnRedRotateM90.Location = new System.Drawing.Point(88, 201);
            this.btnRedRotateM90.Margin = new System.Windows.Forms.Padding(0);
            this.btnRedRotateM90.Name = "btnRedRotateM90";
            this.btnRedRotateM90.Size = new System.Drawing.Size(44, 28);
            this.btnRedRotateM90.TabIndex = 213;
            this.btnRedRotateM90.Tag = "RedRotate,-1,90";
            this.btnRedRotateM90.Text = "-90";
            this.btnRedRotateM90.UseVisualStyleBackColor = false;
            this.btnRedRotateM90.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnRedRotateP90
            // 
            this.btnRedRotateP90.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(82)))), ((int)(((byte)(82)))));
            this.btnRedRotateP90.FlatAppearance.BorderSize = 0;
            this.btnRedRotateP90.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedRotateP90.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnRedRotateP90.ForeColor = System.Drawing.Color.White;
            this.btnRedRotateP90.Location = new System.Drawing.Point(134, 201);
            this.btnRedRotateP90.Margin = new System.Windows.Forms.Padding(0);
            this.btnRedRotateP90.Name = "btnRedRotateP90";
            this.btnRedRotateP90.Size = new System.Drawing.Size(44, 28);
            this.btnRedRotateP90.TabIndex = 214;
            this.btnRedRotateP90.Tag = "RedRotate,1,90";
            this.btnRedRotateP90.Text = "+90";
            this.btnRedRotateP90.UseVisualStyleBackColor = false;
            this.btnRedRotateP90.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnRedRotateMTxt
            // 
            this.btnRedRotateMTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(133)))), ((int)(((byte)(133)))));
            this.btnRedRotateMTxt.FlatAppearance.BorderSize = 0;
            this.btnRedRotateMTxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedRotateMTxt.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnRedRotateMTxt.ForeColor = System.Drawing.Color.White;
            this.btnRedRotateMTxt.Location = new System.Drawing.Point(88, 231);
            this.btnRedRotateMTxt.Margin = new System.Windows.Forms.Padding(0);
            this.btnRedRotateMTxt.Name = "btnRedRotateMTxt";
            this.btnRedRotateMTxt.Size = new System.Drawing.Size(44, 28);
            this.btnRedRotateMTxt.TabIndex = 215;
            this.btnRedRotateMTxt.Tag = "RedRotate,-1,0";
            this.btnRedRotateMTxt.Text = "-";
            this.btnRedRotateMTxt.UseVisualStyleBackColor = false;
            this.btnRedRotateMTxt.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnRedRotatePTxt
            // 
            this.btnRedRotatePTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(133)))), ((int)(((byte)(133)))));
            this.btnRedRotatePTxt.FlatAppearance.BorderSize = 0;
            this.btnRedRotatePTxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedRotatePTxt.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnRedRotatePTxt.ForeColor = System.Drawing.Color.White;
            this.btnRedRotatePTxt.Location = new System.Drawing.Point(134, 231);
            this.btnRedRotatePTxt.Margin = new System.Windows.Forms.Padding(0);
            this.btnRedRotatePTxt.Name = "btnRedRotatePTxt";
            this.btnRedRotatePTxt.Size = new System.Drawing.Size(44, 28);
            this.btnRedRotatePTxt.TabIndex = 216;
            this.btnRedRotatePTxt.Tag = "RedRotate,1,0";
            this.btnRedRotatePTxt.Text = "+";
            this.btnRedRotatePTxt.UseVisualStyleBackColor = false;
            this.btnRedRotatePTxt.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnBlueRotateM90
            // 
            this.btnBlueRotateM90.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(82)))), ((int)(((byte)(231)))));
            this.btnBlueRotateM90.FlatAppearance.BorderSize = 0;
            this.btnBlueRotateM90.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlueRotateM90.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnBlueRotateM90.ForeColor = System.Drawing.Color.White;
            this.btnBlueRotateM90.Location = new System.Drawing.Point(272, 201);
            this.btnBlueRotateM90.Margin = new System.Windows.Forms.Padding(0);
            this.btnBlueRotateM90.Name = "btnBlueRotateM90";
            this.btnBlueRotateM90.Size = new System.Drawing.Size(44, 28);
            this.btnBlueRotateM90.TabIndex = 217;
            this.btnBlueRotateM90.Tag = "BlueRotate,-1,90";
            this.btnBlueRotateM90.Text = "-90";
            this.btnBlueRotateM90.UseVisualStyleBackColor = false;
            this.btnBlueRotateM90.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnBlueRotateP90
            // 
            this.btnBlueRotateP90.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(82)))), ((int)(((byte)(231)))));
            this.btnBlueRotateP90.FlatAppearance.BorderSize = 0;
            this.btnBlueRotateP90.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlueRotateP90.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnBlueRotateP90.ForeColor = System.Drawing.Color.White;
            this.btnBlueRotateP90.Location = new System.Drawing.Point(318, 201);
            this.btnBlueRotateP90.Margin = new System.Windows.Forms.Padding(0);
            this.btnBlueRotateP90.Name = "btnBlueRotateP90";
            this.btnBlueRotateP90.Size = new System.Drawing.Size(44, 28);
            this.btnBlueRotateP90.TabIndex = 218;
            this.btnBlueRotateP90.Tag = "BlueRotate,1,90";
            this.btnBlueRotateP90.Text = "+90";
            this.btnBlueRotateP90.UseVisualStyleBackColor = false;
            this.btnBlueRotateP90.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnBlueRotateMTxt
            // 
            this.btnBlueRotateMTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(133)))), ((int)(((byte)(255)))));
            this.btnBlueRotateMTxt.FlatAppearance.BorderSize = 0;
            this.btnBlueRotateMTxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlueRotateMTxt.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnBlueRotateMTxt.ForeColor = System.Drawing.Color.White;
            this.btnBlueRotateMTxt.Location = new System.Drawing.Point(272, 231);
            this.btnBlueRotateMTxt.Margin = new System.Windows.Forms.Padding(0);
            this.btnBlueRotateMTxt.Name = "btnBlueRotateMTxt";
            this.btnBlueRotateMTxt.Size = new System.Drawing.Size(44, 28);
            this.btnBlueRotateMTxt.TabIndex = 219;
            this.btnBlueRotateMTxt.Tag = "BlueRotate,-1,0";
            this.btnBlueRotateMTxt.Text = "-";
            this.btnBlueRotateMTxt.UseVisualStyleBackColor = false;
            this.btnBlueRotateMTxt.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnBlueRotatePTxt
            // 
            this.btnBlueRotatePTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(133)))), ((int)(((byte)(255)))));
            this.btnBlueRotatePTxt.FlatAppearance.BorderSize = 0;
            this.btnBlueRotatePTxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlueRotatePTxt.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnBlueRotatePTxt.ForeColor = System.Drawing.Color.White;
            this.btnBlueRotatePTxt.Location = new System.Drawing.Point(318, 231);
            this.btnBlueRotatePTxt.Margin = new System.Windows.Forms.Padding(0);
            this.btnBlueRotatePTxt.Name = "btnBlueRotatePTxt";
            this.btnBlueRotatePTxt.Size = new System.Drawing.Size(44, 28);
            this.btnBlueRotatePTxt.TabIndex = 220;
            this.btnBlueRotatePTxt.Tag = "BlueRotate,1,0";
            this.btnBlueRotatePTxt.Text = "+";
            this.btnBlueRotatePTxt.UseVisualStyleBackColor = false;
            this.btnBlueRotatePTxt.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnGreenRotateM90
            // 
            this.btnGreenRotateM90.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(176)))), ((int)(((byte)(82)))));
            this.btnGreenRotateM90.FlatAppearance.BorderSize = 0;
            this.btnGreenRotateM90.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGreenRotateM90.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnGreenRotateM90.ForeColor = System.Drawing.Color.White;
            this.btnGreenRotateM90.Location = new System.Drawing.Point(456, 201);
            this.btnGreenRotateM90.Margin = new System.Windows.Forms.Padding(0);
            this.btnGreenRotateM90.Name = "btnGreenRotateM90";
            this.btnGreenRotateM90.Size = new System.Drawing.Size(44, 28);
            this.btnGreenRotateM90.TabIndex = 221;
            this.btnGreenRotateM90.Tag = "GreenRotate,-1,90";
            this.btnGreenRotateM90.Text = "-90";
            this.btnGreenRotateM90.UseVisualStyleBackColor = false;
            this.btnGreenRotateM90.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnGreenRotateP90
            // 
            this.btnGreenRotateP90.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(176)))), ((int)(((byte)(82)))));
            this.btnGreenRotateP90.FlatAppearance.BorderSize = 0;
            this.btnGreenRotateP90.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGreenRotateP90.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnGreenRotateP90.ForeColor = System.Drawing.Color.White;
            this.btnGreenRotateP90.Location = new System.Drawing.Point(502, 201);
            this.btnGreenRotateP90.Margin = new System.Windows.Forms.Padding(0);
            this.btnGreenRotateP90.Name = "btnGreenRotateP90";
            this.btnGreenRotateP90.Size = new System.Drawing.Size(44, 28);
            this.btnGreenRotateP90.TabIndex = 222;
            this.btnGreenRotateP90.Tag = "GreenRotate,1,90";
            this.btnGreenRotateP90.Text = "+90";
            this.btnGreenRotateP90.UseVisualStyleBackColor = false;
            this.btnGreenRotateP90.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnGreenRotateMTxt
            // 
            this.btnGreenRotateMTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(216)))), ((int)(((byte)(133)))));
            this.btnGreenRotateMTxt.FlatAppearance.BorderSize = 0;
            this.btnGreenRotateMTxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGreenRotateMTxt.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnGreenRotateMTxt.ForeColor = System.Drawing.Color.White;
            this.btnGreenRotateMTxt.Location = new System.Drawing.Point(456, 231);
            this.btnGreenRotateMTxt.Margin = new System.Windows.Forms.Padding(0);
            this.btnGreenRotateMTxt.Name = "btnGreenRotateMTxt";
            this.btnGreenRotateMTxt.Size = new System.Drawing.Size(44, 28);
            this.btnGreenRotateMTxt.TabIndex = 223;
            this.btnGreenRotateMTxt.Tag = "GreenRotate,-1,0";
            this.btnGreenRotateMTxt.Text = "-";
            this.btnGreenRotateMTxt.UseVisualStyleBackColor = false;
            this.btnGreenRotateMTxt.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnGreenRotatePTxt
            // 
            this.btnGreenRotatePTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(133)))), ((int)(((byte)(216)))), ((int)(((byte)(133)))));
            this.btnGreenRotatePTxt.FlatAppearance.BorderSize = 0;
            this.btnGreenRotatePTxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGreenRotatePTxt.Font = new System.Drawing.Font("맑은 고딕", 7F);
            this.btnGreenRotatePTxt.ForeColor = System.Drawing.Color.White;
            this.btnGreenRotatePTxt.Location = new System.Drawing.Point(502, 231);
            this.btnGreenRotatePTxt.Margin = new System.Windows.Forms.Padding(0);
            this.btnGreenRotatePTxt.Name = "btnGreenRotatePTxt";
            this.btnGreenRotatePTxt.Size = new System.Drawing.Size(44, 28);
            this.btnGreenRotatePTxt.TabIndex = 224;
            this.btnGreenRotatePTxt.Tag = "GreenRotate,1,0";
            this.btnGreenRotatePTxt.Text = "+";
            this.btnGreenRotatePTxt.UseVisualStyleBackColor = false;
            this.btnGreenRotatePTxt.Click += new System.EventHandler(this.btnAxisDelta_Click);
            // 
            // btnMotor
            // 
            this.btnMotor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(80)))));
            this.btnMotor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMotor.Location = new System.Drawing.Point(71, 321);
            this.btnMotor.Margin = new System.Windows.Forms.Padding(4);
            this.btnMotor.Name = "btnMotor";
            this.btnMotor.Size = new System.Drawing.Size(66, 30);
            this.btnMotor.TabIndex = 14;
            this.btnMotor.Text = "Joint";
            this.btnMotor.UseVisualStyleBackColor = false;
            this.btnMotor.Click += new System.EventHandler(this.btnMotor_Click);
            // 
            // cmbMotorId
            // 
            this.cmbMotorId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMotorId.Location = new System.Drawing.Point(0, 323);
            this.cmbMotorId.Margin = new System.Windows.Forms.Padding(4);
            this.cmbMotorId.Name = "cmbMotorId";
            this.cmbMotorId.Size = new System.Drawing.Size(63, 26);
            this.cmbMotorId.TabIndex = 42;
            // 
            // btnInit
            // 
            this.btnInit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnInit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInit.Location = new System.Drawing.Point(141, 321);
            this.btnInit.Margin = new System.Windows.Forms.Padding(4);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(145, 30);
            this.btnInit.TabIndex = 15;
            this.btnInit.Text = "New Chain(Init)";
            this.btnInit.UseVisualStyleBackColor = false;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // btnStl
            // 
            this.btnStl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(120)))), ((int)(((byte)(200)))));
            this.btnStl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStl.ForeColor = System.Drawing.Color.White;
            this.btnStl.Location = new System.Drawing.Point(305, 320);
            this.btnStl.Margin = new System.Windows.Forms.Padding(4);
            this.btnStl.Name = "btnStl";
            this.btnStl.Size = new System.Drawing.Size(240, 30);
            this.btnStl.TabIndex = 32;
            this.btnStl.Text = "Add STL";
            this.btnStl.UseVisualStyleBackColor = false;
            this.btnStl.Click += new System.EventHandler(this.btnStl_Click);
            // 
            // btnStlCenter
            // 
            this.btnStlCenter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(100)))), ((int)(((byte)(180)))));
            this.btnStlCenter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStlCenter.ForeColor = System.Drawing.Color.White;
            this.btnStlCenter.Location = new System.Drawing.Point(305, 349);
            this.btnStlCenter.Margin = new System.Windows.Forms.Padding(4);
            this.btnStlCenter.Name = "btnStlCenter";
            this.btnStlCenter.Size = new System.Drawing.Size(119, 30);
            this.btnStlCenter.TabIndex = 33;
            this.btnStlCenter.Text = "STL Center";
            this.btnStlCenter.UseVisualStyleBackColor = false;
            this.btnStlCenter.Click += new System.EventHandler(this.btnStlCenter_Click);
            // 
            // btnStlMerge
            // 
            this.btnStlMerge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.btnStlMerge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStlMerge.ForeColor = System.Drawing.Color.White;
            this.btnStlMerge.Location = new System.Drawing.Point(425, 348);
            this.btnStlMerge.Margin = new System.Windows.Forms.Padding(4);
            this.btnStlMerge.Name = "btnStlMerge";
            this.btnStlMerge.Size = new System.Drawing.Size(119, 30);
            this.btnStlMerge.TabIndex = 34;
            this.btnStlMerge.Text = "Face Align";
            this.btnStlMerge.UseVisualStyleBackColor = false;
            this.btnStlMerge.Click += new System.EventHandler(this.btnStlMerge_Click);
            // 
            // lblStatusTitle
            // 
            this.lblStatusTitle.Location = new System.Drawing.Point(0, 379);
            this.lblStatusTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatusTitle.Name = "lblStatusTitle";
            this.lblStatusTitle.Size = new System.Drawing.Size(450, 22);
            this.lblStatusTitle.TabIndex = 16;
            this.lblStatusTitle.Text = "──── Status ────";
            this.lblStatusTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatus
            // 
            this.lblStatus.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblStatus.Location = new System.Drawing.Point(0, 397);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(450, 26);
            this.lblStatus.TabIndex = 17;
            this.lblStatus.Text = "Click 3D axis to start";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurrentValue
            // 
            this.lblCurrentValue.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCurrentValue.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblCurrentValue.Location = new System.Drawing.Point(0, 424);
            this.lblCurrentValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentValue.Name = "lblCurrentValue";
            this.lblCurrentValue.Size = new System.Drawing.Size(450, 43);
            this.lblCurrentValue.TabIndex = 18;
            this.lblCurrentValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHistoryTitle
            // 
            this.lblHistoryTitle.Location = new System.Drawing.Point(0, 466);
            this.lblHistoryTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHistoryTitle.Name = "lblHistoryTitle";
            this.lblHistoryTitle.Size = new System.Drawing.Size(450, 22);
            this.lblHistoryTitle.TabIndex = 19;
            this.lblHistoryTitle.Text = "──── History ────";
            this.lblHistoryTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstHistory
            // 
            this.lstHistory.FormattingEnabled = true;
            this.lstHistory.ItemHeight = 18;
            this.lstHistory.Location = new System.Drawing.Point(0, 488);
            this.lstHistory.Margin = new System.Windows.Forms.Padding(4);
            this.lstHistory.Name = "lstHistory";
            this.lstHistory.Size = new System.Drawing.Size(450, 76);
            this.lstHistory.TabIndex = 20;
            this.lstHistory.SelectedIndexChanged += new System.EventHandler(this.lstHistory_SelectedIndexChanged);
            // 
            // btnUndo
            // 
            this.btnUndo.Location = new System.Drawing.Point(0, 571);
            this.btnUndo.Margin = new System.Windows.Forms.Padding(4);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(138, 34);
            this.btnUndo.TabIndex = 21;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(142, 571);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(138, 34);
            this.btnDelete.TabIndex = 22;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(285, 571);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(138, 34);
            this.btnClear.TabIndex = 23;
            this.btnClear.Text = "Clear All";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblEtcGroup
            // 
            this.lblEtcGroup.Location = new System.Drawing.Point(0, 301);
            this.lblEtcGroup.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEtcGroup.Name = "lblEtcGroup";
            this.lblEtcGroup.Size = new System.Drawing.Size(291, 24);
            this.lblEtcGroup.TabIndex = 13;
            this.lblEtcGroup.Text = "──── Joint / DH ────";
            this.lblEtcGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabTest
            // 
            this.tabTest.Controls.Add(this.lblIkGroup);
            this.tabTest.Controls.Add(this.lblFuncNum);
            this.tabTest.Controls.Add(this.cmbFuncNum);
            this.tabTest.Controls.Add(this.lblIkMode);
            this.tabTest.Controls.Add(this.cmbCalcInvMode);
            this.tabTest.Controls.Add(this.lblIkX);
            this.tabTest.Controls.Add(this.txtIkX);
            this.tabTest.Controls.Add(this.lblIkY);
            this.tabTest.Controls.Add(this.txtIkY);
            this.tabTest.Controls.Add(this.lblIkZ);
            this.tabTest.Controls.Add(this.txtIkZ);
            this.tabTest.Controls.Add(this.btnGoIk);
            this.tabTest.Controls.Add(this.btnGetFk);
            this.tabTest.Controls.Add(this.btnResetAngle);
            this.tabTest.Controls.Add(this.lblFkGroup);
            this.tabTest.Controls.Add(this.lblMotorCtrl);
            this.tabTest.Controls.Add(this.cmbMotorCtrl);
            this.tabTest.Controls.Add(this.lblAngleCtrl);
            this.tabTest.Controls.Add(this.txtAngleCtrl);
            this.tabTest.Controls.Add(this.lblAngleInfo);
            this.tabTest.Controls.Add(this.trkAngle);
            this.tabTest.Controls.Add(this.lblMouseModeTitle);
            this.tabTest.Controls.Add(this.rdMouseCam);
            this.tabTest.Controls.Add(this.rdMouseJoint);
            this.tabTest.Location = new System.Drawing.Point(4, 28);
            this.tabTest.Margin = new System.Windows.Forms.Padding(4);
            this.tabTest.Name = "tabTest";
            this.tabTest.Size = new System.Drawing.Size(557, 568);
            this.tabTest.TabIndex = 1;
            this.tabTest.Text = "Control";
            // 
            // lblIkGroup
            // 
            this.lblIkGroup.Location = new System.Drawing.Point(0, 4);
            this.lblIkGroup.Name = "lblIkGroup";
            this.lblIkGroup.Size = new System.Drawing.Size(360, 18);
            this.lblIkGroup.TabIndex = 0;
            this.lblIkGroup.Text = "── IK (Position → Angle) ──";
            this.lblIkGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFuncNum
            // 
            this.lblFuncNum.Location = new System.Drawing.Point(0, 26);
            this.lblFuncNum.Name = "lblFuncNum";
            this.lblFuncNum.Size = new System.Drawing.Size(34, 23);
            this.lblFuncNum.TabIndex = 1;
            this.lblFuncNum.Text = "Func:";
            this.lblFuncNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbFuncNum
            // 
            this.cmbFuncNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFuncNum.Location = new System.Drawing.Point(34, 26);
            this.cmbFuncNum.Name = "cmbFuncNum";
            this.cmbFuncNum.Size = new System.Drawing.Size(80, 26);
            this.cmbFuncNum.TabIndex = 2;
            // 
            // lblIkMode
            // 
            this.lblIkMode.Location = new System.Drawing.Point(118, 26);
            this.lblIkMode.Name = "lblIkMode";
            this.lblIkMode.Size = new System.Drawing.Size(24, 23);
            this.lblIkMode.TabIndex = 3;
            this.lblIkMode.Text = "IK:";
            this.lblIkMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbCalcInvMode
            // 
            this.cmbCalcInvMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCalcInvMode.Items.AddRange(new object[] {
            "Ojw CCD-IK",
            "FABRIK (Paper)",
            "Jacobian DLS",
            "Jacobian + Tool",
            "Formula Editor",
            "TRAC-IK (ROS)",
            "Paper CCD (Wang-Chen damped)",
            "WangChen CCD",
            "Kenwright CCD",
            "Paper Jacobian DLS (3xN)",
            "Paper Jacobian Euler (6xN)",
            "Jacobian + Tool Global"});
            this.cmbCalcInvMode.Location = new System.Drawing.Point(142, 26);
            this.cmbCalcInvMode.Name = "cmbCalcInvMode";
            this.cmbCalcInvMode.Size = new System.Drawing.Size(188, 26);
            this.cmbCalcInvMode.TabIndex = 4;
            // 
            // lblIkX
            // 
            this.lblIkX.Location = new System.Drawing.Point(0, 54);
            this.lblIkX.Name = "lblIkX";
            this.lblIkX.Size = new System.Drawing.Size(18, 23);
            this.lblIkX.TabIndex = 5;
            this.lblIkX.Text = "X:";
            this.lblIkX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtIkX
            // 
            this.txtIkX.Location = new System.Drawing.Point(18, 54);
            this.txtIkX.Name = "txtIkX";
            this.txtIkX.Size = new System.Drawing.Size(60, 28);
            this.txtIkX.TabIndex = 6;
            this.txtIkX.Text = "0";
            this.txtIkX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblIkY
            // 
            this.lblIkY.Location = new System.Drawing.Point(84, 54);
            this.lblIkY.Name = "lblIkY";
            this.lblIkY.Size = new System.Drawing.Size(18, 23);
            this.lblIkY.TabIndex = 7;
            this.lblIkY.Text = "Y:";
            this.lblIkY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtIkY
            // 
            this.txtIkY.Location = new System.Drawing.Point(102, 54);
            this.txtIkY.Name = "txtIkY";
            this.txtIkY.Size = new System.Drawing.Size(60, 28);
            this.txtIkY.TabIndex = 8;
            this.txtIkY.Text = "0";
            this.txtIkY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblIkZ
            // 
            this.lblIkZ.Location = new System.Drawing.Point(168, 54);
            this.lblIkZ.Name = "lblIkZ";
            this.lblIkZ.Size = new System.Drawing.Size(18, 23);
            this.lblIkZ.TabIndex = 9;
            this.lblIkZ.Text = "Z:";
            this.lblIkZ.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtIkZ
            // 
            this.txtIkZ.Location = new System.Drawing.Point(186, 54);
            this.txtIkZ.Name = "txtIkZ";
            this.txtIkZ.Size = new System.Drawing.Size(60, 28);
            this.txtIkZ.TabIndex = 10;
            this.txtIkZ.Text = "0";
            this.txtIkZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnGoIk
            // 
            this.btnGoIk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnGoIk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGoIk.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnGoIk.ForeColor = System.Drawing.Color.White;
            this.btnGoIk.Location = new System.Drawing.Point(0, 82);
            this.btnGoIk.Name = "btnGoIk";
            this.btnGoIk.Size = new System.Drawing.Size(110, 30);
            this.btnGoIk.TabIndex = 11;
            this.btnGoIk.Text = "Go (IK)";
            this.btnGoIk.UseVisualStyleBackColor = false;
            this.btnGoIk.Click += new System.EventHandler(this.btnGoIk_Click);
            // 
            // btnGetFk
            // 
            this.btnGetFk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(160)))), ((int)(((byte)(70)))));
            this.btnGetFk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetFk.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnGetFk.ForeColor = System.Drawing.Color.White;
            this.btnGetFk.Location = new System.Drawing.Point(114, 82);
            this.btnGetFk.Name = "btnGetFk";
            this.btnGetFk.Size = new System.Drawing.Size(110, 30);
            this.btnGetFk.TabIndex = 12;
            this.btnGetFk.Text = "Get (FK)";
            this.btnGetFk.UseVisualStyleBackColor = false;
            this.btnGetFk.Click += new System.EventHandler(this.btnGetFk_Click);
            // 
            // btnResetAngle
            // 
            this.btnResetAngle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnResetAngle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetAngle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnResetAngle.ForeColor = System.Drawing.Color.White;
            this.btnResetAngle.Location = new System.Drawing.Point(228, 82);
            this.btnResetAngle.Name = "btnResetAngle";
            this.btnResetAngle.Size = new System.Drawing.Size(110, 30);
            this.btnResetAngle.TabIndex = 13;
            this.btnResetAngle.Text = "Reset";
            this.btnResetAngle.UseVisualStyleBackColor = false;
            this.btnResetAngle.Click += new System.EventHandler(this.btnResetAngle_Click);
            // 
            // lblFkGroup
            // 
            this.lblFkGroup.Location = new System.Drawing.Point(0, 120);
            this.lblFkGroup.Name = "lblFkGroup";
            this.lblFkGroup.Size = new System.Drawing.Size(504, 18);
            this.lblFkGroup.TabIndex = 14;
            this.lblFkGroup.Text = "── FK (Joint Control) ──";
            this.lblFkGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMotorCtrl
            // 
            this.lblMotorCtrl.Location = new System.Drawing.Point(0, 142);
            this.lblMotorCtrl.Name = "lblMotorCtrl";
            this.lblMotorCtrl.Size = new System.Drawing.Size(50, 23);
            this.lblMotorCtrl.TabIndex = 15;
            this.lblMotorCtrl.Text = "Joint:";
            this.lblMotorCtrl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbMotorCtrl
            // 
            this.cmbMotorCtrl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMotorCtrl.Location = new System.Drawing.Point(54, 142);
            this.cmbMotorCtrl.Name = "cmbMotorCtrl";
            this.cmbMotorCtrl.Size = new System.Drawing.Size(100, 26);
            this.cmbMotorCtrl.TabIndex = 16;
            // 
            // lblAngleCtrl
            // 
            this.lblAngleCtrl.Location = new System.Drawing.Point(162, 142);
            this.lblAngleCtrl.Name = "lblAngleCtrl";
            this.lblAngleCtrl.Size = new System.Drawing.Size(55, 23);
            this.lblAngleCtrl.TabIndex = 17;
            this.lblAngleCtrl.Text = "Angle:";
            this.lblAngleCtrl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAngleCtrl
            // 
            this.txtAngleCtrl.Location = new System.Drawing.Point(221, 142);
            this.txtAngleCtrl.Name = "txtAngleCtrl";
            this.txtAngleCtrl.Size = new System.Drawing.Size(75, 28);
            this.txtAngleCtrl.TabIndex = 18;
            this.txtAngleCtrl.Text = "0";
            this.txtAngleCtrl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblAngleInfo
            // 
            this.lblAngleInfo.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblAngleInfo.Location = new System.Drawing.Point(304, 142);
            this.lblAngleInfo.Name = "lblAngleInfo";
            this.lblAngleInfo.Size = new System.Drawing.Size(200, 23);
            this.lblAngleInfo.TabIndex = 19;
            this.lblAngleInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trkAngle
            // 
            this.trkAngle.LargeChange = 100;
            this.trkAngle.Location = new System.Drawing.Point(0, 178);
            this.trkAngle.Maximum = 1800;
            this.trkAngle.Minimum = -1800;
            this.trkAngle.Name = "trkAngle";
            this.trkAngle.Size = new System.Drawing.Size(504, 69);
            this.trkAngle.SmallChange = 10;
            this.trkAngle.TabIndex = 20;
            this.trkAngle.TickFrequency = 300;
            this.trkAngle.Scroll += new System.EventHandler(this.trkAngle_Scroll);
            // 
            // lblMouseModeTitle
            // 
            this.lblMouseModeTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMouseModeTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblMouseModeTitle.Location = new System.Drawing.Point(0, 250);
            this.lblMouseModeTitle.Name = "lblMouseModeTitle";
            this.lblMouseModeTitle.Size = new System.Drawing.Size(330, 20);
            this.lblMouseModeTitle.TabIndex = 21;
            this.lblMouseModeTitle.Text = "── Mouse Control (3D View) ──";
            this.lblMouseModeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rdMouseCam
            // 
            this.rdMouseCam.AutoSize = true;
            this.rdMouseCam.Checked = true;
            this.rdMouseCam.Location = new System.Drawing.Point(20, 275);
            this.rdMouseCam.Name = "rdMouseCam";
            this.rdMouseCam.Size = new System.Drawing.Size(210, 22);
            this.rdMouseCam.TabIndex = 22;
            this.rdMouseCam.TabStop = true;
            this.rdMouseCam.Text = "Camera (L-click: Pan)";
            this.rdMouseCam.UseVisualStyleBackColor = true;
            this.rdMouseCam.CheckedChanged += new System.EventHandler(this.rdMouseCam_CheckedChanged);
            // 
            // rdMouseJoint
            // 
            this.rdMouseJoint.AutoSize = true;
            this.rdMouseJoint.Location = new System.Drawing.Point(238, 275);
            this.rdMouseJoint.Name = "rdMouseJoint";
            this.rdMouseJoint.Size = new System.Drawing.Size(205, 22);
            this.rdMouseJoint.TabIndex = 23;
            this.rdMouseJoint.Text = "Joint (L-click: Rotate)";
            this.rdMouseJoint.UseVisualStyleBackColor = true;
            this.rdMouseJoint.CheckedChanged += new System.EventHandler(this.rdMouseJoint_CheckedChanged);
            // 
            // tabHistory
            // 
            this.tabHistory.Controls.Add(this.txtLogHistory);
            this.tabHistory.Location = new System.Drawing.Point(4, 28);
            this.tabHistory.Margin = new System.Windows.Forms.Padding(4);
            this.tabHistory.Name = "tabHistory";
            this.tabHistory.Size = new System.Drawing.Size(557, 568);
            this.tabHistory.TabIndex = 2;
            this.tabHistory.Text = "Log";
            // 
            // txtLogHistory
            // 
            this.txtLogHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtLogHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogHistory.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLogHistory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtLogHistory.Location = new System.Drawing.Point(0, 0);
            this.txtLogHistory.Margin = new System.Windows.Forms.Padding(4);
            this.txtLogHistory.Multiline = true;
            this.txtLogHistory.Name = "txtLogHistory";
            this.txtLogHistory.ReadOnly = true;
            this.txtLogHistory.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLogHistory.Size = new System.Drawing.Size(557, 568);
            this.txtLogHistory.TabIndex = 0;
            this.txtLogHistory.WordWrap = false;
            // 
            // tabPlayback
            // 
            this.tabPlayback.BackColor = System.Drawing.SystemColors.Control;
            this.tabPlayback.Controls.Add(this.lblPbDataset);
            this.tabPlayback.Controls.Add(this.btnBrowseDataset);
            this.tabPlayback.Controls.Add(this.cmbEpisode);
            this.tabPlayback.Controls.Add(this.btnPlay);
            this.tabPlayback.Controls.Add(this.btnPause);
            this.tabPlayback.Controls.Add(this.btnStop);
            this.tabPlayback.Controls.Add(this.lblPlaySpeed);
            this.tabPlayback.Controls.Add(this.trkSpeed);
            this.tabPlayback.Controls.Add(this.trkFrame);
            this.tabPlayback.Controls.Add(this.lblFrameInfo);
            this.tabPlayback.Controls.Add(this.lblPbWrist);
            this.tabPlayback.Controls.Add(this.lblPbExternal);
            this.tabPlayback.Controls.Add(this.picWrist);
            this.tabPlayback.Controls.Add(this.picExternal);
            this.tabPlayback.Controls.Add(this.lblPbQposTitle);
            this.tabPlayback.Controls.Add(this.picGraphQpos);
            this.tabPlayback.Controls.Add(this.lblPbTgtTitle);
            this.tabPlayback.Controls.Add(this.picGraphTgt);
            this.tabPlayback.Location = new System.Drawing.Point(4, 28);
            this.tabPlayback.Margin = new System.Windows.Forms.Padding(4);
            this.tabPlayback.Name = "tabPlayback";
            this.tabPlayback.Size = new System.Drawing.Size(557, 568);
            this.tabPlayback.TabIndex = 3;
            this.tabPlayback.Text = "Playback";
            // 
            // lblPbDataset
            // 
            this.lblPbDataset.Location = new System.Drawing.Point(5, 11);
            this.lblPbDataset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPbDataset.Name = "lblPbDataset";
            this.lblPbDataset.Size = new System.Drawing.Size(69, 22);
            this.lblPbDataset.TabIndex = 0;
            this.lblPbDataset.Text = "Dataset:";
            // 
            // btnBrowseDataset
            // 
            this.btnBrowseDataset.Location = new System.Drawing.Point(75, 7);
            this.btnBrowseDataset.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseDataset.Name = "btnBrowseDataset";
            this.btnBrowseDataset.Size = new System.Drawing.Size(365, 29);
            this.btnBrowseDataset.TabIndex = 1;
            this.btnBrowseDataset.Text = "Browse";
            this.btnBrowseDataset.UseVisualStyleBackColor = true;
            this.btnBrowseDataset.Click += new System.EventHandler(this.btnBrowseDataset_Click);
            // 
            // cmbEpisode
            // 
            this.cmbEpisode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEpisode.FormattingEnabled = true;
            this.cmbEpisode.Location = new System.Drawing.Point(5, 41);
            this.cmbEpisode.Margin = new System.Windows.Forms.Padding(4);
            this.cmbEpisode.Name = "cmbEpisode";
            this.cmbEpisode.Size = new System.Drawing.Size(434, 26);
            this.cmbEpisode.TabIndex = 2;
            this.cmbEpisode.SelectedIndexChanged += new System.EventHandler(this.cmbEpisode_SelectedIndexChanged);
            // 
            // btnPlay
            // 
            this.btnPlay.BackColor = System.Drawing.Color.LimeGreen;
            this.btnPlay.Location = new System.Drawing.Point(5, 72);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(4);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(62, 31);
            this.btnPlay.TabIndex = 3;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(72, 72);
            this.btnPause.Margin = new System.Windows.Forms.Padding(4);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(62, 31);
            this.btnPause.TabIndex = 4;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Salmon;
            this.btnStop.Location = new System.Drawing.Point(140, 72);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(62, 31);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblPlaySpeed
            // 
            this.lblPlaySpeed.Location = new System.Drawing.Point(208, 77);
            this.lblPlaySpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPlaySpeed.Name = "lblPlaySpeed";
            this.lblPlaySpeed.Size = new System.Drawing.Size(50, 22);
            this.lblPlaySpeed.TabIndex = 6;
            this.lblPlaySpeed.Text = "1.0x";
            // 
            // trkSpeed
            // 
            this.trkSpeed.Location = new System.Drawing.Point(258, 72);
            this.trkSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.trkSpeed.Maximum = 40;
            this.trkSpeed.Minimum = 1;
            this.trkSpeed.Name = "trkSpeed";
            this.trkSpeed.Size = new System.Drawing.Size(182, 69);
            this.trkSpeed.TabIndex = 7;
            this.trkSpeed.TickFrequency = 5;
            this.trkSpeed.Value = 10;
            this.trkSpeed.Scroll += new System.EventHandler(this.trkSpeed_Scroll);
            // 
            // trkFrame
            // 
            this.trkFrame.Location = new System.Drawing.Point(5, 108);
            this.trkFrame.Margin = new System.Windows.Forms.Padding(4);
            this.trkFrame.Maximum = 1;
            this.trkFrame.Name = "trkFrame";
            this.trkFrame.Size = new System.Drawing.Size(435, 69);
            this.trkFrame.TabIndex = 8;
            this.trkFrame.TickFrequency = 50;
            this.trkFrame.Scroll += new System.EventHandler(this.trkFrame_Scroll);
            // 
            // lblFrameInfo
            // 
            this.lblFrameInfo.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblFrameInfo.Location = new System.Drawing.Point(5, 144);
            this.lblFrameInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFrameInfo.Name = "lblFrameInfo";
            this.lblFrameInfo.Size = new System.Drawing.Size(435, 22);
            this.lblFrameInfo.TabIndex = 9;
            this.lblFrameInfo.Text = "Frame: -/-";
            // 
            // lblPbWrist
            // 
            this.lblPbWrist.ForeColor = System.Drawing.Color.Gray;
            this.lblPbWrist.Location = new System.Drawing.Point(5, 170);
            this.lblPbWrist.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPbWrist.Name = "lblPbWrist";
            this.lblPbWrist.Size = new System.Drawing.Size(212, 18);
            this.lblPbWrist.TabIndex = 10;
            this.lblPbWrist.Text = "Wrist";
            this.lblPbWrist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPbExternal
            // 
            this.lblPbExternal.ForeColor = System.Drawing.Color.Gray;
            this.lblPbExternal.Location = new System.Drawing.Point(222, 170);
            this.lblPbExternal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPbExternal.Name = "lblPbExternal";
            this.lblPbExternal.Size = new System.Drawing.Size(212, 18);
            this.lblPbExternal.TabIndex = 11;
            this.lblPbExternal.Text = "External";
            this.lblPbExternal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picWrist
            // 
            this.picWrist.BackColor = System.Drawing.Color.Black;
            this.picWrist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picWrist.Location = new System.Drawing.Point(5, 190);
            this.picWrist.Margin = new System.Windows.Forms.Padding(4);
            this.picWrist.Name = "picWrist";
            this.picWrist.Size = new System.Drawing.Size(212, 120);
            this.picWrist.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picWrist.TabIndex = 12;
            this.picWrist.TabStop = false;
            // 
            // picExternal
            // 
            this.picExternal.BackColor = System.Drawing.Color.Black;
            this.picExternal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picExternal.Location = new System.Drawing.Point(222, 190);
            this.picExternal.Margin = new System.Windows.Forms.Padding(4);
            this.picExternal.Name = "picExternal";
            this.picExternal.Size = new System.Drawing.Size(212, 120);
            this.picExternal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picExternal.TabIndex = 13;
            this.picExternal.TabStop = false;
            // 
            // lblPbQposTitle
            // 
            this.lblPbQposTitle.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblPbQposTitle.Location = new System.Drawing.Point(5, 314);
            this.lblPbQposTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPbQposTitle.Name = "lblPbQposTitle";
            this.lblPbQposTitle.Size = new System.Drawing.Size(435, 18);
            this.lblPbQposTitle.TabIndex = 14;
            this.lblPbQposTitle.Text = "qpos (Follower)";
            // 
            // picGraphQpos
            // 
            this.picGraphQpos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.picGraphQpos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGraphQpos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picGraphQpos.Location = new System.Drawing.Point(5, 334);
            this.picGraphQpos.Margin = new System.Windows.Forms.Padding(4);
            this.picGraphQpos.Name = "picGraphQpos";
            this.picGraphQpos.Size = new System.Drawing.Size(434, 144);
            this.picGraphQpos.TabIndex = 15;
            this.picGraphQpos.TabStop = false;
            this.picGraphQpos.DoubleClick += new System.EventHandler(this.picGraphQpos_DoubleClick);
            // 
            // lblPbTgtTitle
            // 
            this.lblPbTgtTitle.ForeColor = System.Drawing.Color.DarkRed;
            this.lblPbTgtTitle.Location = new System.Drawing.Point(5, 482);
            this.lblPbTgtTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPbTgtTitle.Name = "lblPbTgtTitle";
            this.lblPbTgtTitle.Size = new System.Drawing.Size(435, 18);
            this.lblPbTgtTitle.TabIndex = 16;
            this.lblPbTgtTitle.Text = "tgt_pos (Leader)";
            // 
            // picGraphTgt
            // 
            this.picGraphTgt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.picGraphTgt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGraphTgt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picGraphTgt.Location = new System.Drawing.Point(5, 502);
            this.picGraphTgt.Margin = new System.Windows.Forms.Padding(4);
            this.picGraphTgt.Name = "picGraphTgt";
            this.picGraphTgt.Size = new System.Drawing.Size(434, 132);
            this.picGraphTgt.TabIndex = 17;
            this.picGraphTgt.TabStop = false;
            this.picGraphTgt.DoubleClick += new System.EventHandler(this.picGraphTgt_DoubleClick);
            // 
            // tabArmSetup
            // 
            this.tabArmSetup.Controls.Add(this.grpArmConn);
            this.tabArmSetup.Controls.Add(this.tabArmInner);
            this.tabArmSetup.BackColor = System.Drawing.SystemColors.Control;
            this.tabArmSetup.Location = new System.Drawing.Point(4, 28);
            this.tabArmSetup.Margin = new System.Windows.Forms.Padding(4);
            this.tabArmSetup.Name = "tabArmSetup";
            this.tabArmSetup.Size = new System.Drawing.Size(557, 568);
            this.tabArmSetup.TabIndex = 4;
            this.tabArmSetup.Text = "로봇팔 세팅";
            // 
            // grpArmConn
            // 
            this.grpArmConn.Controls.Add(this.lblArmPort);
            this.grpArmConn.Controls.Add(this.txtArmPort);
            this.grpArmConn.Controls.Add(this.lblArmBaud);
            this.grpArmConn.Controls.Add(this.txtArmBaud);
            this.grpArmConn.Controls.Add(this.btnArmConnect);
            this.grpArmConn.Controls.Add(this.btnArmTorque);
            this.grpArmConn.Controls.Add(this.lblArmState);
            this.grpArmConn.Location = new System.Drawing.Point(4, 2);
            this.grpArmConn.Name = "grpArmConn";
            this.grpArmConn.Size = new System.Drawing.Size(549, 58);
            this.grpArmConn.TabIndex = 0;
            this.grpArmConn.TabStop = false;
            this.grpArmConn.Text = "연결";
            // 
            // lblArmPort
            // 
            this.lblArmPort.Location = new System.Drawing.Point(6, 24);
            this.lblArmPort.Name = "lblArmPort";
            this.lblArmPort.Size = new System.Drawing.Size(40, 23);
            this.lblArmPort.TabIndex = 0;
            this.lblArmPort.Text = "포트";
            this.lblArmPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtArmPort
            // 
            this.txtArmPort.Location = new System.Drawing.Point(48, 21);
            this.txtArmPort.Name = "txtArmPort";
            this.txtArmPort.Size = new System.Drawing.Size(64, 28);
            this.txtArmPort.TabIndex = 1;
            this.txtArmPort.Text = "COM4";
            // 
            // lblArmBaud
            // 
            this.lblArmBaud.Location = new System.Drawing.Point(116, 24);
            this.lblArmBaud.Name = "lblArmBaud";
            this.lblArmBaud.Size = new System.Drawing.Size(40, 23);
            this.lblArmBaud.TabIndex = 2;
            this.lblArmBaud.Text = "속도";
            this.lblArmBaud.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtArmBaud
            // 
            this.txtArmBaud.Location = new System.Drawing.Point(158, 21);
            this.txtArmBaud.Name = "txtArmBaud";
            this.txtArmBaud.Size = new System.Drawing.Size(76, 28);
            this.txtArmBaud.TabIndex = 3;
            this.txtArmBaud.Text = "1000000";
            // 
            // btnArmConnect
            // 
            this.btnArmConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnArmConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmConnect.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmConnect.ForeColor = System.Drawing.Color.White;
            this.btnArmConnect.Location = new System.Drawing.Point(240, 15);
            this.btnArmConnect.Name = "btnArmConnect";
            this.btnArmConnect.Size = new System.Drawing.Size(84, 36);
            this.btnArmConnect.TabIndex = 4;
            this.btnArmConnect.Text = "연결";
            this.btnArmConnect.UseVisualStyleBackColor = false;
            this.btnArmConnect.Click += new System.EventHandler(this.btnArmConnect_Click);
            // 
            // btnArmTorque
            // 
            this.btnArmTorque.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnArmTorque.Enabled = false;
            this.btnArmTorque.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmTorque.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmTorque.ForeColor = System.Drawing.Color.White;
            this.btnArmTorque.Location = new System.Drawing.Point(328, 15);
            this.btnArmTorque.Name = "btnArmTorque";
            this.btnArmTorque.Size = new System.Drawing.Size(84, 36);
            this.btnArmTorque.TabIndex = 5;
            this.btnArmTorque.Text = "토크 켜기";
            this.btnArmTorque.UseVisualStyleBackColor = false;
            this.btnArmTorque.Click += new System.EventHandler(this.btnArmTorque_Click);
            // 
            // lblArmState
            // 
            this.lblArmState.ForeColor = System.Drawing.Color.Gray;
            this.lblArmState.Location = new System.Drawing.Point(416, 12);
            this.lblArmState.Name = "lblArmState";
            this.lblArmState.Size = new System.Drawing.Size(128, 42);
            this.lblArmState.TabIndex = 6;
            this.lblArmState.Text = "연결 안 됨";
            this.lblArmState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabArmInner
            // 
            this.tabArmInner.Controls.Add(this.tabArmPrac);
            this.tabArmInner.Controls.Add(this.tabArmMotor);
            this.tabArmInner.Controls.Add(this.tabArmDef);
            this.tabArmInner.Controls.Add(this.tabArmExp);
            this.tabArmInner.Location = new System.Drawing.Point(4, 62);
            this.tabArmInner.Name = "tabArmInner";
            this.tabArmInner.SelectedIndex = 0;
            this.tabArmInner.Size = new System.Drawing.Size(549, 502);
            this.tabArmInner.TabIndex = 1;
            this.tabArmInner.SelectedIndexChanged += new System.EventHandler(this.tabArmInner_SelectedIndexChanged);
            // 
            // tabArmPrac
            // 
            this.tabArmPrac.Controls.Add(this.lblPracGuide);
            this.tabArmPrac.Controls.Add(this.chkPracPrecise);
            this.tabArmPrac.Controls.Add(this.chkPracDry);
            this.tabArmPrac.Controls.Add(this.btnPrac1);
            this.tabArmPrac.Controls.Add(this.btnPrac2);
            this.tabArmPrac.Controls.Add(this.btnPrac3);
            this.tabArmPrac.Controls.Add(this.btnPrac4);
            this.tabArmPrac.Controls.Add(this.lblPracStep);
            this.tabArmPrac.Controls.Add(this.picPracChart);
            this.tabArmPrac.Controls.Add(this.lblPracResult);
            this.tabArmPrac.Controls.Add(this.btnPracStop);
            this.tabArmPrac.Controls.Add(this.lblPracBusy);
            this.tabArmPrac.Controls.Add(this.lblPracDef);
            this.tabArmPrac.Controls.Add(this.chkPracDefShow);
            this.tabArmPrac.Controls.Add(this.btnPracDef);
            this.tabArmPrac.Controls.Add(this.lnkPracEye);
            this.tabArmPrac.AutoScroll = true;
            this.tabArmPrac.BackColor = System.Drawing.SystemColors.Control;
            this.tabArmPrac.Location = new System.Drawing.Point(4, 28);
            this.tabArmPrac.Name = "tabArmPrac";
            this.tabArmPrac.Size = new System.Drawing.Size(541, 470);
            this.tabArmPrac.TabIndex = 0;
            this.tabArmPrac.Text = "처짐 보정 실습";
            // 
            // lblPracGuide
            // 
            this.lblPracGuide.Font = new System.Drawing.Font("맑은 고딕", 8.5F);
            this.lblPracGuide.Location = new System.Drawing.Point(8, 4);
            this.lblPracGuide.Name = "lblPracGuide";
            this.lblPracGuide.Size = new System.Drawing.Size(516, 72);
            this.lblPracGuide.TabIndex = 0;
            this.lblPracGuide.Text = "로봇팔은 무게 때문에 처져서 3D 가 생각하는 끝 높이와 실제가 다릅니다.\r\n자 없이 바닥을 살짝 눌러 잽니다: 1 오차 재기 → 2 보정 데이터 만들기\r\n→ 3 효과 보기 (같은 점을 다시) → 4 적용 (그 뒤 모든 동작이 자동 보정)\r\n실물: 위에서 먼저 연결하고 팔 둘레를 비우십시오. 연결이 없으면 [3D만] 으로.";
            // 
            // chkPracPrecise
            // 
            this.chkPracPrecise.AutoSize = true;
            this.chkPracPrecise.Location = new System.Drawing.Point(10, 104);
            this.chkPracPrecise.Name = "chkPracPrecise";
            this.chkPracPrecise.Size = new System.Drawing.Size(232, 22);
            this.chkPracPrecise.TabIndex = 1;
            this.chkPracPrecise.Text = "정밀 (점을 더 많이 — 오래 걸림)";
            this.tipArmExp.SetToolTip(this.chkPracPrecise, "끄면 빠르게 (수업용 — 세 단계 합쳐 약 10분), 켜면 점을 더 많이 (더 정확, 오래 걸림)");
            this.chkPracPrecise.UseVisualStyleBackColor = true;
            // 
            // chkPracDry
            // 
            this.chkPracDry.AutoSize = true;
            this.chkPracDry.Location = new System.Drawing.Point(300, 104);
            this.chkPracDry.Name = "chkPracDry";
            this.chkPracDry.Size = new System.Drawing.Size(170, 22);
            this.chkPracDry.TabIndex = 2;
            this.chkPracDry.Text = "3D만 (실물 안 움직임)";
            this.tipArmExp.SetToolTip(this.chkPracDry, "실물 없이 가상 로봇으로 같은 순서를 3D 로 보여 준다 (실제 처짐과 다름, 적용은 안 함)");
            this.chkPracDry.UseVisualStyleBackColor = true;
            this.chkPracDry.CheckedChanged += new System.EventHandler(this.chkPracDry_CheckedChanged);
            // 
            // btnPrac1
            // 
            this.btnPrac1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnPrac1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrac1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnPrac1.ForeColor = System.Drawing.Color.White;
            this.btnPrac1.Location = new System.Drawing.Point(8, 130);
            this.btnPrac1.Name = "btnPrac1";
            this.btnPrac1.Size = new System.Drawing.Size(125, 44);
            this.btnPrac1.TabIndex = 3;
            this.btnPrac1.Text = "1. 오차 재기";
            this.tipArmExp.SetToolTip(this.btnPrac1, "보정을 모두 끈 채 바닥을 살짝 눌러 점마다 3D 와 실물의 높이 차를 잰다 (처음이면 바닥부터 찾는다)");
            this.btnPrac1.UseVisualStyleBackColor = false;
            this.btnPrac1.Click += new System.EventHandler(this.btnPrac1_Click);
            // 
            // btnPrac2
            // 
            this.btnPrac2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnPrac2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrac2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnPrac2.ForeColor = System.Drawing.Color.White;
            this.btnPrac2.Location = new System.Drawing.Point(137, 130);
            this.btnPrac2.Name = "btnPrac2";
            this.btnPrac2.Size = new System.Drawing.Size(125, 44);
            this.btnPrac2.TabIndex = 4;
            this.btnPrac2.Text = "2. 보정 데이터\r\n만들기";
            this.tipArmExp.SetToolTip(this.btnPrac2, "공중에서 관절 처짐을 재고, 바닥을 더 눌러 데이터를 모아 보정 모델을 학습한다 (1 다음)");
            this.btnPrac2.UseVisualStyleBackColor = false;
            this.btnPrac2.Click += new System.EventHandler(this.btnPrac2_Click);
            // 
            // btnPrac3
            // 
            this.btnPrac3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnPrac3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrac3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnPrac3.ForeColor = System.Drawing.Color.White;
            this.btnPrac3.Location = new System.Drawing.Point(266, 130);
            this.btnPrac3.Name = "btnPrac3";
            this.btnPrac3.Size = new System.Drawing.Size(125, 44);
            this.btnPrac3.TabIndex = 5;
            this.btnPrac3.Text = "3. 효과 보기";
            this.tipArmExp.SetToolTip(this.btnPrac3, "보정을 켜고 1과 같은 점을 다시 눌러 오차가 얼마나 줄었는지 본다 (2 다음)");
            this.btnPrac3.UseVisualStyleBackColor = false;
            this.btnPrac3.Click += new System.EventHandler(this.btnPrac3_Click);
            // 
            // btnPrac4
            // 
            this.btnPrac4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(160)))), ((int)(((byte)(70)))));
            this.btnPrac4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrac4.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnPrac4.ForeColor = System.Drawing.Color.White;
            this.btnPrac4.Location = new System.Drawing.Point(395, 130);
            this.btnPrac4.Name = "btnPrac4";
            this.btnPrac4.Size = new System.Drawing.Size(129, 44);
            this.btnPrac4.TabIndex = 6;
            this.btnPrac4.Text = "4. 적용\r\n(자동 보정)";
            this.tipArmExp.SetToolTip(this.btnPrac4, "이 보정 모델을 적용한다 — 그 뒤 모든 동작이 자동으로 보정된다 (3 다음, 지금 파일은 백업)");
            this.btnPrac4.UseVisualStyleBackColor = false;
            this.btnPrac4.Click += new System.EventHandler(this.btnPrac4_Click);
            // 
            // lblPracStep
            // 
            this.lblPracStep.Location = new System.Drawing.Point(8, 178);
            this.lblPracStep.Name = "lblPracStep";
            this.lblPracStep.Size = new System.Drawing.Size(516, 36);
            this.lblPracStep.TabIndex = 7;
            this.lblPracStep.Text = "";
            this.lblPracStep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picPracChart
            // 
            this.picPracChart.BackColor = System.Drawing.Color.White;
            this.picPracChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPracChart.Location = new System.Drawing.Point(8, 216);
            this.picPracChart.Name = "picPracChart";
            this.picPracChart.Size = new System.Drawing.Size(516, 176);
            this.picPracChart.TabIndex = 8;
            this.picPracChart.TabStop = false;
            this.picPracChart.Paint += new System.Windows.Forms.PaintEventHandler(this.picPracChart_Paint);
            // 
            // lblPracResult
            // 
            this.lblPracResult.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.lblPracResult.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblPracResult.Location = new System.Drawing.Point(8, 396);
            this.lblPracResult.Name = "lblPracResult";
            this.lblPracResult.Size = new System.Drawing.Size(516, 28);
            this.lblPracResult.TabIndex = 9;
            this.lblPracResult.Text = "";
            this.lblPracResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPracStop
            // 
            this.btnPracStop.BackColor = System.Drawing.Color.Salmon;
            this.btnPracStop.Enabled = false;
            this.btnPracStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPracStop.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnPracStop.ForeColor = System.Drawing.Color.Black;
            this.btnPracStop.Location = new System.Drawing.Point(8, 428);
            this.btnPracStop.Name = "btnPracStop";
            this.btnPracStop.Size = new System.Drawing.Size(100, 32);
            this.btnPracStop.TabIndex = 10;
            this.btnPracStop.Text = "중단";
            this.tipArmExp.SetToolTip(this.btnPracStop, "지금 점을 마치고 원위치로 돌아가 토크를 시작 상태로 돌린다");
            this.btnPracStop.UseVisualStyleBackColor = false;
            this.btnPracStop.Click += new System.EventHandler(this.btnPracStop_Click);
            // 
            // lblPracBusy
            // 
            this.lblPracBusy.Font = new System.Drawing.Font("맑은 고딕", 8.5F);
            this.lblPracBusy.Location = new System.Drawing.Point(112, 428);
            this.lblPracBusy.Name = "lblPracBusy";
            this.lblPracBusy.Size = new System.Drawing.Size(412, 36);
            this.lblPracBusy.TabIndex = 11;
            this.lblPracBusy.Text = "대기";
            this.lblPracBusy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPracDef
            // 
            this.lblPracDef.AutoEllipsis = true;
            this.lblPracDef.Font = new System.Drawing.Font("맑은 고딕", 8.5F);
            this.lblPracDef.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblPracDef.Location = new System.Drawing.Point(8, 80);
            this.lblPracDef.Name = "lblPracDef";
            this.lblPracDef.Size = new System.Drawing.Size(262, 22);
            this.lblPracDef.TabIndex = 12;
            this.lblPracDef.Text = "로봇 정의 확인 전";
            this.lblPracDef.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tipArmExp.SetToolTip(this.lblPracDef, "측정·실습이 쓰는 로봇 정의 (공구 끝 · 꺾는 손목 · 바닥으로 수직) — 3D 와 맞는지, 수직 눈 확인을 했는지");
            // 
            // chkPracDefShow
            // 
            this.chkPracDefShow.Location = new System.Drawing.Point(348, 81);
            this.chkPracDefShow.Name = "chkPracDefShow";
            this.chkPracDefShow.Size = new System.Drawing.Size(88, 22);
            this.chkPracDefShow.TabIndex = 13;
            this.chkPracDefShow.Text = "3D 표시";
            this.tipArmExp.SetToolTip(this.chkPracDefShow, "3D 에 공구 끝(빨강)·공구축(초록 = 수직, 파랑 = 기울어짐)·꺾는 손목(주황)을 표시");
            this.chkPracDefShow.UseVisualStyleBackColor = true;
            this.chkPracDefShow.CheckedChanged += new System.EventHandler(this.chkPracDefShow_CheckedChanged);
            // 
            // btnPracDef
            // 
            this.btnPracDef.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnPracDef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPracDef.Font = new System.Drawing.Font("맑은 고딕", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnPracDef.ForeColor = System.Drawing.Color.White;
            this.btnPracDef.Location = new System.Drawing.Point(440, 78);
            this.btnPracDef.Name = "btnPracDef";
            this.btnPracDef.Size = new System.Drawing.Size(84, 24);
            this.btnPracDef.TabIndex = 14;
            this.btnPracDef.Text = "정의 바꾸기";
            this.tipArmExp.SetToolTip(this.btnPracDef, "'로봇 정의' 페이지로 — 어디가 공구 끝인지, 어느 관절로 공구를 꺾는지, 무엇이 '바닥으로 수직' 인지 보고 고친다");
            this.btnPracDef.UseVisualStyleBackColor = false;
            this.btnPracDef.Click += new System.EventHandler(this.btnPracDef_Click);
            // 
            // lnkPracEye
            // 
            this.lnkPracEye.Font = new System.Drawing.Font("맑은 고딕", 8.5F);
            this.lnkPracEye.Location = new System.Drawing.Point(272, 81);
            this.lnkPracEye.Name = "lnkPracEye";
            this.lnkPracEye.Size = new System.Drawing.Size(74, 20);
            this.lnkPracEye.TabIndex = 15;
            this.lnkPracEye.TabStop = true;
            this.lnkPracEye.Text = "눈 확인하기";
            this.lnkPracEye.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tipArmExp.SetToolTip(this.lnkPracEye, "수직 눈 확인 ('로봇 정의' 페이지의 [수직 눈 확인] 과 같다) — 팔을 바닥 위 옮기는 높이에서 수직 자세로 펼쳐 실물 펜이 바닥에 수직인지 묻고 기록한다\r\n바닥은 누르지 않고 실습 진행은 그대로. [3D만] 이 켜져 있으면 3D 로만 미리 본다");
            this.lnkPracEye.Visible = false;
            this.lnkPracEye.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPracEye_LinkClicked);
            // 
            // tabArmMotor
            // 
            this.tabArmMotor.Controls.Add(this.dgvArmMotor);
            this.tabArmMotor.Controls.Add(this.btnArmRead);
            this.tabArmMotor.Controls.Add(this.btnArmJogMinus);
            this.tabArmMotor.Controls.Add(this.txtArmJogStep);
            this.tabArmMotor.Controls.Add(this.lblArmJogUnit);
            this.tabArmMotor.Controls.Add(this.btnArmJogPlus);
            this.tabArmMotor.Controls.Add(this.btnArmSetZero);
            this.tabArmMotor.Controls.Add(this.btnArmSave);
            this.tabArmMotor.Controls.Add(this.btnArmLoad);
            this.tabArmMotor.Controls.Add(this.btnArmImportAuto);
            this.tabArmMotor.Controls.Add(this.chkArmFollow);
            this.tabArmMotor.Controls.Add(this.chkArmAdvanced);
            this.tabArmMotor.Controls.Add(this.lblArmFile);
            this.tabArmMotor.Controls.Add(this.lblArmHint);
            this.tabArmMotor.BackColor = System.Drawing.SystemColors.Control;
            this.tabArmMotor.Location = new System.Drawing.Point(4, 28);
            this.tabArmMotor.Name = "tabArmMotor";
            this.tabArmMotor.Size = new System.Drawing.Size(541, 470);
            this.tabArmMotor.TabIndex = 1;
            this.tabArmMotor.Text = "모터 세팅";
            // 
            // dgvArmMotor
            // 
            this.dgvArmMotor.AllowUserToAddRows = false;
            this.dgvArmMotor.AllowUserToDeleteRows = false;
            this.dgvArmMotor.AllowUserToResizeRows = false;
            this.dgvArmMotor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvArmMotor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colArmAxis,
            this.colArmName,
            this.colArmDir,
            this.colArmCenter,
            this.colArmLo,
            this.colArmHi,
            this.colArmMirror,
            this.colArmModel,
            this.colArmNowDeg,
            this.colArmChecked,
            this.colArmNowTick,
            this.colArmRealModel,
            this.colArmNote,
            this.colArmGroup,
            this.colArmBusId,
            this.colArmMechMove,
            this.colArmMechAngle,
            this.colArmInit,
            this.colArmInit2,
            this.colArmProto});
            this.dgvArmMotor.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvArmMotor.Location = new System.Drawing.Point(3, 3);
            this.dgvArmMotor.MultiSelect = false;
            this.dgvArmMotor.Name = "dgvArmMotor";
            this.dgvArmMotor.RowHeadersVisible = false;
            this.dgvArmMotor.RowTemplate.Height = 23;
            this.dgvArmMotor.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.dgvArmMotor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvArmMotor.Size = new System.Drawing.Size(535, 256);
            this.dgvArmMotor.TabIndex = 0;
            this.dgvArmMotor.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvArmMotor_CellBeginEdit);
            this.dgvArmMotor.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvArmMotor_CellValidating);
            this.dgvArmMotor.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArmMotor_CellValueChanged);
            this.dgvArmMotor.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvArmMotor_CurrentCellDirtyStateChanged);
            this.dgvArmMotor.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvArmMotor_DataError);
            this.dgvArmMotor.SelectionChanged += new System.EventHandler(this.dgvArmMotor_SelectionChanged);
            // 
            // colArmAxis
            // 
            this.colArmAxis.HeaderText = "ID";
            this.colArmAxis.Name = "colArmAxis";
            this.colArmAxis.ReadOnly = true;
            this.colArmAxis.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmAxis.ToolTipText = "축 번호 = 실물 ID (디자이너 목록 인덱스)";
            this.colArmAxis.Width = 32;
            // 
            // colArmName
            // 
            this.colArmName.HeaderText = "이름";
            this.colArmName.Name = "colArmName";
            this.colArmName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmName.ToolTipText = "Nick Name";
            this.colArmName.Width = 60;
            // 
            // colArmDir
            // 
            this.colArmDir.HeaderText = "방향";
            this.colArmDir.Items.AddRange(new object[] {
            "정",
            "역"});
            this.colArmDir.Name = "colArmDir";
            this.colArmDir.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmDir.ToolTipText = "Motor Dir (정 = Forward / 역 = Backward)";
            this.colArmDir.Width = 48;
            // 
            // colArmCenter
            // 
            this.colArmCenter.HeaderText = "영점 틱";
            this.colArmCenter.Name = "colArmCenter";
            this.colArmCenter.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmCenter.ToolTipText = "CenterPos — 3D 0° 일 때의 서보 틱";
            this.colArmCenter.Width = 54;
            // 
            // colArmLo
            // 
            this.colArmLo.HeaderText = "하한°";
            this.colArmLo.Name = "colArmLo";
            this.colArmLo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmLo.ToolTipText = "Limit(-) — 빈칸 = 없음";
            this.colArmLo.Width = 48;
            // 
            // colArmHi
            // 
            this.colArmHi.HeaderText = "상한°";
            this.colArmHi.Name = "colArmHi";
            this.colArmHi.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmHi.ToolTipText = "Limit(+) — 빈칸 = 없음";
            this.colArmHi.Width = 48;
            // 
            // colArmMirror
            // 
            this.colArmMirror.HeaderText = "미러";
            this.colArmMirror.Name = "colArmMirror";
            this.colArmMirror.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmMirror.ToolTipText = "Mirror Axis";
            this.colArmMirror.Width = 64;
            // 
            // colArmModel
            // 
            this.colArmModel.HeaderText = "기종";
            this.colArmModel.Name = "colArmModel";
            this.colArmModel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmModel.ToolTipText = "Motor Name";
            this.colArmModel.Width = 92;
            // 
            // colArmNowDeg
            // 
            this.colArmNowDeg.HeaderText = "현재각°";
            this.colArmNowDeg.Name = "colArmNowDeg";
            this.colArmNowDeg.ReadOnly = true;
            this.colArmNowDeg.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmNowDeg.ToolTipText = "읽기·따라가기로 읽은 3D 각";
            this.colArmNowDeg.Width = 54;
            // 
            // colArmChecked
            // 
            this.colArmChecked.HeaderText = "확인";
            this.colArmChecked.Name = "colArmChecked";
            this.colArmChecked.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmChecked.ToolTipText = "방향 확인함";
            this.colArmChecked.Width = 36;
            // 
            // colArmNowTick
            // 
            this.colArmNowTick.HeaderText = "현재 틱";
            this.colArmNowTick.Name = "colArmNowTick";
            this.colArmNowTick.ReadOnly = true;
            this.colArmNowTick.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmNowTick.ToolTipText = "읽기·따라가기로 읽은 서보 틱";
            this.colArmNowTick.Width = 56;
            // 
            // colArmRealModel
            // 
            this.colArmRealModel.HeaderText = "실물 기종";
            this.colArmRealModel.Name = "colArmRealModel";
            this.colArmRealModel.ReadOnly = true;
            this.colArmRealModel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmRealModel.ToolTipText = "실물에서 읽은 기종 (주소 0)";
            this.colArmRealModel.Width = 96;
            // 
            // colArmNote
            // 
            this.colArmNote.HeaderText = "상태";
            this.colArmNote.Name = "colArmNote";
            this.colArmNote.ReadOnly = true;
            this.colArmNote.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmNote.ToolTipText = "읽기 경고 + 출처";
            this.colArmNote.Width = 200;
            // 
            // colArmGroup
            // 
            this.colArmGroup.HeaderText = "그룹";
            this.colArmGroup.Name = "colArmGroup";
            this.colArmGroup.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmGroup.ToolTipText = "Group Name (번호)";
            this.colArmGroup.Visible = false;
            this.colArmGroup.Width = 40;
            // 
            // colArmBusId
            // 
            this.colArmBusId.HeaderText = "실물 ID";
            this.colArmBusId.Name = "colArmBusId";
            this.colArmBusId.ReadOnly = true;
            this.colArmBusId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmBusId.ToolTipText = "ID(Real) — 지금은 축 번호와 같음";
            this.colArmBusId.Visible = false;
            this.colArmBusId.Width = 50;
            // 
            // colArmMechMove
            // 
            this.colArmMechMove.HeaderText = "틱/회전";
            this.colArmMechMove.Name = "colArmMechMove";
            this.colArmMechMove.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmMechMove.ToolTipText = "Mech Move";
            this.colArmMechMove.Visible = false;
            this.colArmMechMove.Width = 56;
            // 
            // colArmMechAngle
            // 
            this.colArmMechAngle.HeaderText = "각/회전";
            this.colArmMechAngle.Name = "colArmMechAngle";
            this.colArmMechAngle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmMechAngle.ToolTipText = "Mech Angle";
            this.colArmMechAngle.Visible = false;
            this.colArmMechAngle.Width = 56;
            // 
            // colArmInit
            // 
            this.colArmInit.HeaderText = "초기각°";
            this.colArmInit.Name = "colArmInit";
            this.colArmInit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmInit.ToolTipText = "Init Angle";
            this.colArmInit.Visible = false;
            this.colArmInit.Width = 52;
            // 
            // colArmInit2
            // 
            this.colArmInit2.HeaderText = "초기각2°";
            this.colArmInit2.Name = "colArmInit2";
            this.colArmInit2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmInit2.ToolTipText = "Init Angle2";
            this.colArmInit2.Visible = false;
            this.colArmInit2.Width = 52;
            // 
            // colArmProto
            // 
            this.colArmProto.HeaderText = "프로토콜";
            this.colArmProto.Name = "colArmProto";
            this.colArmProto.ReadOnly = true;
            this.colArmProto.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colArmProto.ToolTipText = "Protocol Version";
            this.colArmProto.Visible = false;
            this.colArmProto.Width = 50;
            // 
            // btnArmRead
            // 
            this.btnArmRead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnArmRead.Enabled = false;
            this.btnArmRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmRead.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmRead.ForeColor = System.Drawing.Color.White;
            this.btnArmRead.Location = new System.Drawing.Point(3, 264);
            this.btnArmRead.Name = "btnArmRead";
            this.btnArmRead.Size = new System.Drawing.Size(78, 34);
            this.btnArmRead.TabIndex = 1;
            this.btnArmRead.Text = "읽기";
            this.btnArmRead.UseVisualStyleBackColor = false;
            this.btnArmRead.Click += new System.EventHandler(this.btnArmRead_Click);
            // 
            // btnArmJogMinus
            // 
            this.btnArmJogMinus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnArmJogMinus.Enabled = false;
            this.btnArmJogMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmJogMinus.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmJogMinus.ForeColor = System.Drawing.Color.White;
            this.btnArmJogMinus.Location = new System.Drawing.Point(85, 264);
            this.btnArmJogMinus.Name = "btnArmJogMinus";
            this.btnArmJogMinus.Size = new System.Drawing.Size(70, 34);
            this.btnArmJogMinus.TabIndex = 2;
            this.btnArmJogMinus.Tag = "-1";
            this.btnArmJogMinus.Text = "조그 -";
            this.btnArmJogMinus.UseVisualStyleBackColor = false;
            this.btnArmJogMinus.Click += new System.EventHandler(this.btnArmJog_Click);
            // 
            // txtArmJogStep
            // 
            this.txtArmJogStep.Location = new System.Drawing.Point(159, 267);
            this.txtArmJogStep.Name = "txtArmJogStep";
            this.txtArmJogStep.Size = new System.Drawing.Size(40, 28);
            this.txtArmJogStep.TabIndex = 3;
            this.txtArmJogStep.Text = "3";
            this.txtArmJogStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblArmJogUnit
            // 
            this.lblArmJogUnit.Location = new System.Drawing.Point(201, 270);
            this.lblArmJogUnit.Name = "lblArmJogUnit";
            this.lblArmJogUnit.Size = new System.Drawing.Size(16, 23);
            this.lblArmJogUnit.TabIndex = 4;
            this.lblArmJogUnit.Text = "°";
            this.lblArmJogUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnArmJogPlus
            // 
            this.btnArmJogPlus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnArmJogPlus.Enabled = false;
            this.btnArmJogPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmJogPlus.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmJogPlus.ForeColor = System.Drawing.Color.White;
            this.btnArmJogPlus.Location = new System.Drawing.Point(219, 264);
            this.btnArmJogPlus.Name = "btnArmJogPlus";
            this.btnArmJogPlus.Size = new System.Drawing.Size(70, 34);
            this.btnArmJogPlus.TabIndex = 5;
            this.btnArmJogPlus.Tag = "1";
            this.btnArmJogPlus.Text = "조그 +";
            this.btnArmJogPlus.UseVisualStyleBackColor = false;
            this.btnArmJogPlus.Click += new System.EventHandler(this.btnArmJog_Click);
            // 
            // btnArmSetZero
            // 
            this.btnArmSetZero.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(160)))), ((int)(((byte)(70)))));
            this.btnArmSetZero.Enabled = false;
            this.btnArmSetZero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmSetZero.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmSetZero.ForeColor = System.Drawing.Color.White;
            this.btnArmSetZero.Location = new System.Drawing.Point(293, 264);
            this.btnArmSetZero.Name = "btnArmSetZero";
            this.btnArmSetZero.Size = new System.Drawing.Size(160, 34);
            this.btnArmSetZero.TabIndex = 6;
            this.btnArmSetZero.Text = "현재 위치를 0 으로";
            this.btnArmSetZero.UseVisualStyleBackColor = false;
            this.btnArmSetZero.Click += new System.EventHandler(this.btnArmSetZero_Click);
            // 
            // btnArmSave
            // 
            this.btnArmSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(160)))), ((int)(((byte)(70)))));
            this.btnArmSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmSave.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmSave.ForeColor = System.Drawing.Color.White;
            this.btnArmSave.Location = new System.Drawing.Point(3, 302);
            this.btnArmSave.Name = "btnArmSave";
            this.btnArmSave.Size = new System.Drawing.Size(78, 34);
            this.btnArmSave.TabIndex = 7;
            this.btnArmSave.Text = "저장";
            this.btnArmSave.UseVisualStyleBackColor = false;
            this.btnArmSave.Click += new System.EventHandler(this.btnArmSave_Click);
            // 
            // btnArmLoad
            // 
            this.btnArmLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(160)))), ((int)(((byte)(70)))));
            this.btnArmLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmLoad.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmLoad.ForeColor = System.Drawing.Color.White;
            this.btnArmLoad.Location = new System.Drawing.Point(85, 302);
            this.btnArmLoad.Name = "btnArmLoad";
            this.btnArmLoad.Size = new System.Drawing.Size(90, 34);
            this.btnArmLoad.TabIndex = 8;
            this.btnArmLoad.Text = "불러오기";
            this.btnArmLoad.UseVisualStyleBackColor = false;
            this.btnArmLoad.Click += new System.EventHandler(this.btnArmLoad_Click);
            // 
            // btnArmImportAuto
            // 
            this.btnArmImportAuto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(160)))), ((int)(((byte)(70)))));
            this.btnArmImportAuto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmImportAuto.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmImportAuto.ForeColor = System.Drawing.Color.White;
            this.btnArmImportAuto.Location = new System.Drawing.Point(179, 302);
            this.btnArmImportAuto.Name = "btnArmImportAuto";
            this.btnArmImportAuto.Size = new System.Drawing.Size(200, 34);
            this.btnArmImportAuto.TabIndex = 9;
            this.btnArmImportAuto.Text = "자동 방향 결과 가져오기";
            this.btnArmImportAuto.UseVisualStyleBackColor = false;
            this.btnArmImportAuto.Click += new System.EventHandler(this.btnArmImportAuto_Click);
            // 
            // chkArmFollow
            // 
            this.chkArmFollow.AutoSize = true;
            this.chkArmFollow.Enabled = false;
            this.chkArmFollow.Location = new System.Drawing.Point(3, 342);
            this.chkArmFollow.Name = "chkArmFollow";
            this.chkArmFollow.Size = new System.Drawing.Size(230, 22);
            this.chkArmFollow.TabIndex = 10;
            this.chkArmFollow.Text = "3D 가 실물 따라가기 (토크 OFF)";
            this.chkArmFollow.UseVisualStyleBackColor = true;
            this.chkArmFollow.CheckedChanged += new System.EventHandler(this.chkArmFollow_CheckedChanged);
            // 
            // chkArmAdvanced
            // 
            this.chkArmAdvanced.AutoSize = true;
            this.chkArmAdvanced.Location = new System.Drawing.Point(300, 342);
            this.chkArmAdvanced.Name = "chkArmAdvanced";
            this.chkArmAdvanced.Size = new System.Drawing.Size(120, 22);
            this.chkArmAdvanced.TabIndex = 11;
            this.chkArmAdvanced.Text = "고급 열 보기";
            this.chkArmAdvanced.UseVisualStyleBackColor = true;
            this.chkArmAdvanced.CheckedChanged += new System.EventHandler(this.chkArmAdvanced_CheckedChanged);
            // 
            // lblArmFile
            // 
            this.lblArmFile.Location = new System.Drawing.Point(3, 368);
            this.lblArmFile.Name = "lblArmFile";
            this.lblArmFile.Size = new System.Drawing.Size(535, 46);
            this.lblArmFile.TabIndex = 12;
            this.lblArmFile.Text = "파일: (없음)";
            this.lblArmFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblArmHint
            // 
            this.lblArmHint.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblArmHint.ForeColor = System.Drawing.Color.DimGray;
            this.lblArmHint.Location = new System.Drawing.Point(3, 416);
            this.lblArmHint.Name = "lblArmHint";
            this.lblArmHint.Size = new System.Drawing.Size(535, 52);
            this.lblArmHint.TabIndex = 13;
            this.lblArmHint.Text = "방향 확인: 토크 OFF 에서 '3D 가 실물 따라가기' 를 켜고 관절을 손으로 돌려 보십시오. 3D 가 반대로 돌면 그 줄의 '방향' 을 바꿉니다. 토크 ON 이면 조그 ± 로 몇 도씩 움직여 비교합니다. 영점: 관절을 3D 의 0° 모양에 맞춰 잡고 '현재 위치를 0 으로'. Control 탭·마우스로 바꾼 3D 자세는 실물로 가지 않습니다.";
            this.lblArmHint.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // tabArmDef
            // 
            this.tabArmDef.Controls.Add(this.lblDefGuide);
            this.tabArmDef.Controls.Add(this.lblDefPreset);
            this.tabArmDef.Controls.Add(this.cmbDefPreset);
            this.tabArmDef.Controls.Add(this.btnDefPresetSave);
            this.tabArmDef.Controls.Add(this.lblDefTcp);
            this.tabArmDef.Controls.Add(this.cmbDefTcp);
            this.tabArmDef.Controls.Add(this.lblDefTcpInfo);
            this.tabArmDef.Controls.Add(this.lblDefWrist);
            this.tabArmDef.Controls.Add(this.cmbDefWrist);
            this.tabArmDef.Controls.Add(this.lblDefWristInfo);
            this.tabArmDef.Controls.Add(this.lblDefDown);
            this.tabArmDef.Controls.Add(this.lblDefDownSum);
            this.tabArmDef.Controls.Add(this.txtDefDownSum);
            this.tabArmDef.Controls.Add(this.lblDefDownInfo);
            this.tabArmDef.Controls.Add(this.lblDefShowNote);
            this.tabArmDef.Controls.Add(this.btnDefSave);
            this.tabArmDef.Controls.Add(this.lblDefStatus);
            this.tabArmDef.Controls.Add(this.chkDefAdv);
            this.tabArmDef.Controls.Add(this.grpDefAdv);
            this.tabArmDef.Controls.Add(this.lblDefEye);
            this.tabArmDef.Controls.Add(this.btnDefEye);
            this.tabArmDef.AutoScroll = true;
            this.tabArmDef.BackColor = System.Drawing.SystemColors.Control;
            this.tabArmDef.Location = new System.Drawing.Point(4, 28);
            this.tabArmDef.Name = "tabArmDef";
            this.tabArmDef.Size = new System.Drawing.Size(541, 470);
            this.tabArmDef.TabIndex = 2;
            this.tabArmDef.Text = "로봇 정의";
            // 
            // lblDefGuide
            // 
            this.lblDefGuide.Font = new System.Drawing.Font("맑은 고딕", 8.5F);
            this.lblDefGuide.Location = new System.Drawing.Point(8, 4);
            this.lblDefGuide.Name = "lblDefGuide";
            this.lblDefGuide.Size = new System.Drawing.Size(516, 34);
            this.lblDefGuide.TabIndex = 0;
            this.lblDefGuide.Text = "측정·실습이 쓰는 로봇 정의 — 공구 끝, 공구를 꺾는 손목, '바닥으로 수직' 의 뜻.\r\n보통은 그대로 두십시오. 3D 와 맞을 때만 저장됩니다 (sagarm.txt).";
            this.lblDefGuide.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDefPreset
            // 
            this.lblDefPreset.Location = new System.Drawing.Point(8, 47);
            this.lblDefPreset.Name = "lblDefPreset";
            this.lblDefPreset.Size = new System.Drawing.Size(104, 20);
            this.lblDefPreset.TabIndex = 1;
            this.lblDefPreset.Text = "본보기";
            this.lblDefPreset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbDefPreset
            // 
            this.cmbDefPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefPreset.FormattingEnabled = true;
            this.cmbDefPreset.Location = new System.Drawing.Point(120, 43);
            this.cmbDefPreset.Name = "cmbDefPreset";
            this.cmbDefPreset.Size = new System.Drawing.Size(264, 26);
            this.cmbDefPreset.TabIndex = 2;
            this.tipArmExp.SetToolTip(this.cmbDefPreset, "고르면 칸만 채웁니다 (저장 안 함) — [확인·저장] 을 눌러야 sagarm.txt 에 들어갑니다");
            this.cmbDefPreset.SelectedIndexChanged += new System.EventHandler(this.cmbDefPreset_SelectedIndexChanged);
            // 
            // btnDefPresetSave
            // 
            this.btnDefPresetSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnDefPresetSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDefPresetSave.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnDefPresetSave.ForeColor = System.Drawing.Color.White;
            this.btnDefPresetSave.Location = new System.Drawing.Point(390, 42);
            this.btnDefPresetSave.Name = "btnDefPresetSave";
            this.btnDefPresetSave.Size = new System.Drawing.Size(134, 28);
            this.btnDefPresetSave.TabIndex = 3;
            this.btnDefPresetSave.Text = "본보기로 저장…";
            this.tipArmExp.SetToolTip(this.btnDefPresetSave, "지금 칸의 정의를 본보기로 저장 — <urdf>\\sagarm_defs\\<이름>.txt (sagarm.txt 는 바꾸지 않음)");
            this.btnDefPresetSave.UseVisualStyleBackColor = false;
            this.btnDefPresetSave.Click += new System.EventHandler(this.btnDefPresetSave_Click);
            // 
            // lblDefTcp
            // 
            this.lblDefTcp.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblDefTcp.Location = new System.Drawing.Point(8, 81);
            this.lblDefTcp.Name = "lblDefTcp";
            this.lblDefTcp.Size = new System.Drawing.Size(108, 20);
            this.lblDefTcp.TabIndex = 4;
            this.lblDefTcp.Text = "① 공구 끝";
            this.lblDefTcp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tipArmExp.SetToolTip(this.lblDefTcp, "공구 끝(TCP) = 고른 IK 수식 체인의 끝 — 첫 '!' 바로 앞 DH 줄 ('!' 가 없으면 체인 마지막 DH 줄)");
            // 
            // cmbDefTcp
            // 
            this.cmbDefTcp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefTcp.FormattingEnabled = true;
            this.cmbDefTcp.Location = new System.Drawing.Point(120, 77);
            this.cmbDefTcp.Name = "cmbDefTcp";
            this.cmbDefTcp.Size = new System.Drawing.Size(160, 26);
            this.cmbDefTcp.TabIndex = 5;
            this.tipArmExp.SetToolTip(this.cmbDefTcp, "공구 끝(TCP) = 이 수식 체인의 끝 — 첫 '!' 바로 앞 DH 줄 ('!' 가 없으면 체인 마지막 DH 줄)");
            this.cmbDefTcp.SelectedIndexChanged += new System.EventHandler(this.cmbDefTcp_SelectedIndexChanged);
            // 
            // lblDefTcpInfo
            // 
            this.lblDefTcpInfo.Location = new System.Drawing.Point(286, 81);
            this.lblDefTcpInfo.Name = "lblDefTcpInfo";
            this.lblDefTcpInfo.Size = new System.Drawing.Size(238, 20);
            this.lblDefTcpInfo.TabIndex = 6;
            this.lblDefTcpInfo.Text = "";
            this.lblDefTcpInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDefWrist
            // 
            this.lblDefWrist.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblDefWrist.Location = new System.Drawing.Point(8, 115);
            this.lblDefWrist.Name = "lblDefWrist";
            this.lblDefWrist.Size = new System.Drawing.Size(108, 20);
            this.lblDefWrist.TabIndex = 7;
            this.lblDefWrist.Text = "② 꺾는 손목";
            this.lblDefWrist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tipArmExp.SetToolTip(this.lblDefWrist, "공구를 바닥으로 수직으로 꺾는 관절 — 공구 끝 앞 마지막 관절");
            // 
            // cmbDefWrist
            // 
            this.cmbDefWrist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefWrist.FormattingEnabled = true;
            this.cmbDefWrist.Location = new System.Drawing.Point(120, 111);
            this.cmbDefWrist.Name = "cmbDefWrist";
            this.cmbDefWrist.Size = new System.Drawing.Size(90, 26);
            this.cmbDefWrist.TabIndex = 8;
            this.tipArmExp.SetToolTip(this.cmbDefWrist, "공구를 바닥으로 수직으로 꺾는 관절 — 공구 끝 앞 마지막 관절");
            this.cmbDefWrist.SelectedIndexChanged += new System.EventHandler(this.cmbDefWrist_SelectedIndexChanged);
            // 
            // lblDefWristInfo
            // 
            this.lblDefWristInfo.Location = new System.Drawing.Point(216, 115);
            this.lblDefWristInfo.Name = "lblDefWristInfo";
            this.lblDefWristInfo.Size = new System.Drawing.Size(308, 20);
            this.lblDefWristInfo.TabIndex = 9;
            this.lblDefWristInfo.Text = "";
            this.lblDefWristInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDefDown
            // 
            this.lblDefDown.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblDefDown.Location = new System.Drawing.Point(8, 149);
            this.lblDefDown.Name = "lblDefDown";
            this.lblDefDown.Size = new System.Drawing.Size(108, 20);
            this.lblDefDown.TabIndex = 10;
            this.lblDefDown.Text = "③ 바닥으로 수직";
            this.lblDefDown.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tipArmExp.SetToolTip(this.lblDefDown, "공구축(손목 → 공구 끝)이 아래(−Z)를 향할 때의 관절 합. 3D 와 0.5° 안이어야 저장됩니다. 실물이 이 값에서 수직이 아니면 고칠 곳은 이 칸이 아니라 모터 영점입니다");
            // 
            // lblDefDownSum
            // 
            this.lblDefDownSum.Location = new System.Drawing.Point(120, 149);
            this.lblDefDownSum.Name = "lblDefDownSum";
            this.lblDefDownSum.Size = new System.Drawing.Size(140, 20);
            this.lblDefDownSum.TabIndex = 11;
            this.lblDefDownSum.Text = "관절 합 12+13+14 =";
            this.lblDefDownSum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tipArmExp.SetToolTip(this.lblDefDownSum, "공구축(손목 → 공구 끝)이 아래(−Z)를 향할 때의 관절 합. 3D 와 0.5° 안이어야 저장됩니다. 실물이 이 값에서 수직이 아니면 고칠 곳은 이 칸이 아니라 모터 영점입니다");
            // 
            // txtDefDownSum
            // 
            this.txtDefDownSum.Location = new System.Drawing.Point(262, 145);
            this.txtDefDownSum.Name = "txtDefDownSum";
            this.txtDefDownSum.Size = new System.Drawing.Size(56, 28);
            this.txtDefDownSum.TabIndex = 12;
            this.txtDefDownSum.Text = "90";
            this.txtDefDownSum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipArmExp.SetToolTip(this.txtDefDownSum, "공구축(손목 → 공구 끝)이 아래(−Z)를 향할 때의 관절 합. 3D 와 0.5° 안이어야 저장됩니다. 실물이 이 값에서 수직이 아니면 고칠 곳은 이 칸이 아니라 모터 영점입니다");
            this.txtDefDownSum.TextChanged += new System.EventHandler(this.txtDefDownSum_TextChanged);
            // 
            // lblDefDownInfo
            // 
            this.lblDefDownInfo.Location = new System.Drawing.Point(322, 149);
            this.lblDefDownInfo.Name = "lblDefDownInfo";
            this.lblDefDownInfo.Size = new System.Drawing.Size(202, 20);
            this.lblDefDownInfo.TabIndex = 13;
            this.lblDefDownInfo.Text = "°";
            this.lblDefDownInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDefShowNote
            // 
            this.lblDefShowNote.Font = new System.Drawing.Font("맑은 고딕", 8.5F);
            this.lblDefShowNote.ForeColor = System.Drawing.Color.DimGray;
            this.lblDefShowNote.Location = new System.Drawing.Point(8, 184);
            this.lblDefShowNote.Name = "lblDefShowNote";
            this.lblDefShowNote.Size = new System.Drawing.Size(376, 20);
            this.lblDefShowNote.TabIndex = 14;
            this.lblDefShowNote.Text = "이 페이지를 보는 동안 3D 에 공구 끝·공구축·손목 표시";
            this.lblDefShowNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDefSave
            // 
            this.btnDefSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnDefSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDefSave.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnDefSave.ForeColor = System.Drawing.Color.White;
            this.btnDefSave.Location = new System.Drawing.Point(390, 178);
            this.btnDefSave.Name = "btnDefSave";
            this.btnDefSave.Size = new System.Drawing.Size(134, 30);
            this.btnDefSave.TabIndex = 15;
            this.btnDefSave.Text = "확인·저장";
            this.tipArmExp.SetToolTip(this.btnDefSave, "3D 와 맞으면 sagarm.txt 에 저장 — 측정·실습이 이 정의를 씁니다 (실습 기록과 다르면 다음은 1단계부터)");
            this.btnDefSave.UseVisualStyleBackColor = false;
            this.btnDefSave.Click += new System.EventHandler(this.btnDefSave_Click);
            // 
            // lblDefStatus
            // 
            this.lblDefStatus.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblDefStatus.Location = new System.Drawing.Point(8, 214);
            this.lblDefStatus.Name = "lblDefStatus";
            this.lblDefStatus.Size = new System.Drawing.Size(516, 40);
            this.lblDefStatus.TabIndex = 16;
            this.lblDefStatus.Text = "";
            this.lblDefStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkDefAdv
            // 
            this.chkDefAdv.Location = new System.Drawing.Point(8, 258);
            this.chkDefAdv.Name = "chkDefAdv";
            this.chkDefAdv.Size = new System.Drawing.Size(120, 22);
            this.chkDefAdv.TabIndex = 17;
            this.chkDefAdv.Text = "고급 보기";
            this.tipArmExp.SetToolTip(this.chkDefAdv, "관절 번호·이름·공구 끝 거리와 고정 규약을 봅니다");
            this.chkDefAdv.UseVisualStyleBackColor = true;
            this.chkDefAdv.CheckedChanged += new System.EventHandler(this.chkDefAdv_CheckedChanged);
            // 
            // lblDefEye
            // 
            this.lblDefEye.AutoEllipsis = true;
            this.lblDefEye.Font = new System.Drawing.Font("맑은 고딕", 8.5F);
            this.lblDefEye.Location = new System.Drawing.Point(132, 258);
            this.lblDefEye.Name = "lblDefEye";
            this.lblDefEye.Size = new System.Drawing.Size(254, 22);
            this.lblDefEye.TabIndex = 19;
            this.lblDefEye.Text = "";
            this.lblDefEye.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnDefEye
            // 
            this.btnDefEye.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnDefEye.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDefEye.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnDefEye.ForeColor = System.Drawing.Color.White;
            this.btnDefEye.Location = new System.Drawing.Point(390, 256);
            this.btnDefEye.Name = "btnDefEye";
            this.btnDefEye.Size = new System.Drawing.Size(134, 24);
            this.btnDefEye.TabIndex = 20;
            this.btnDefEye.Text = "수직 눈 확인";
            this.tipArmExp.SetToolTip(this.btnDefEye, "저장된 정의로 팔을 바닥 위 옮기는 높이(가운데 점 위)에서 '바닥으로 수직' 자세로 펼쳐 실물 펜이 바닥에 수직인지 묻는다 — [예] 면 기록\r\n바닥은 누르지 않고 실습 진행은 그대로. 실물 ③ 측정·⑤ 닿기 시험·실습 2·3단계 전에 이 로봇·정의·모터 영점으로 한 번\r\n연결이 없으면 3D 로만 미리 본다 (기록하지 않음)");
            this.btnDefEye.UseVisualStyleBackColor = false;
            this.btnDefEye.Click += new System.EventHandler(this.btnDefEye_Click);
            // 
            // grpDefAdv
            // 
            this.grpDefAdv.Controls.Add(this.lblDefName);
            this.grpDefAdv.Controls.Add(this.txtDefName);
            this.grpDefAdv.Controls.Add(this.lblDefBase);
            this.grpDefAdv.Controls.Add(this.txtDefBase);
            this.grpDefAdv.Controls.Add(this.lblDefPitch);
            this.grpDefAdv.Controls.Add(this.txtDefPitch);
            this.grpDefAdv.Controls.Add(this.lblDefHold);
            this.grpDefAdv.Controls.Add(this.txtDefHold);
            this.grpDefAdv.Controls.Add(this.lblDefTcpLen);
            this.grpDefAdv.Controls.Add(this.txtDefTcpLen);
            this.grpDefAdv.Controls.Add(this.lblDefFixed);
            this.grpDefAdv.Location = new System.Drawing.Point(8, 282);
            this.grpDefAdv.Name = "grpDefAdv";
            this.grpDefAdv.Size = new System.Drawing.Size(516, 180);
            this.grpDefAdv.TabIndex = 18;
            this.grpDefAdv.TabStop = false;
            this.grpDefAdv.Text = "고급 — 관절 번호 · 이름 · 공구 끝 거리";
            this.grpDefAdv.Visible = false;
            // 
            // lblDefName
            // 
            this.lblDefName.Location = new System.Drawing.Point(10, 23);
            this.lblDefName.Name = "lblDefName";
            this.lblDefName.Size = new System.Drawing.Size(96, 20);
            this.lblDefName.TabIndex = 0;
            this.lblDefName.Text = "이름";
            this.lblDefName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDefName
            // 
            this.txtDefName.Location = new System.Drawing.Point(110, 20);
            this.txtDefName.Name = "txtDefName";
            this.txtDefName.Size = new System.Drawing.Size(140, 28);
            this.txtDefName.TabIndex = 1;
            this.txtDefName.Text = "OMX follower";
            this.tipArmExp.SetToolTip(this.txtDefName, "정의 이름 (1~40자, // = | # 은 쓸 수 없음)");
            this.txtDefName.TextChanged += new System.EventHandler(this.txtDefName_TextChanged);
            // 
            // lblDefBase
            // 
            this.lblDefBase.Location = new System.Drawing.Point(262, 23);
            this.lblDefBase.Name = "lblDefBase";
            this.lblDefBase.Size = new System.Drawing.Size(110, 20);
            this.lblDefBase.TabIndex = 2;
            this.lblDefBase.Text = "베이스(요) 관절";
            this.lblDefBase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDefBase
            // 
            this.txtDefBase.Location = new System.Drawing.Point(376, 20);
            this.txtDefBase.Name = "txtDefBase";
            this.txtDefBase.Size = new System.Drawing.Size(130, 28);
            this.txtDefBase.TabIndex = 3;
            this.txtDefBase.Text = "11";
            this.tipArmExp.SetToolTip(this.txtDefBase, "베이스(요) 관절 번호 — 공구 끝 체인의 첫 관절");
            this.txtDefBase.TextChanged += new System.EventHandler(this.txtDefBase_TextChanged);
            // 
            // lblDefPitch
            // 
            this.lblDefPitch.Location = new System.Drawing.Point(10, 53);
            this.lblDefPitch.Name = "lblDefPitch";
            this.lblDefPitch.Size = new System.Drawing.Size(96, 20);
            this.lblDefPitch.TabIndex = 4;
            this.lblDefPitch.Text = "피치 관절 2개";
            this.lblDefPitch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDefPitch
            // 
            this.txtDefPitch.Location = new System.Drawing.Point(110, 50);
            this.txtDefPitch.Name = "txtDefPitch";
            this.txtDefPitch.Size = new System.Drawing.Size(140, 28);
            this.txtDefPitch.TabIndex = 5;
            this.txtDefPitch.Text = "12,13";
            this.tipArmExp.SetToolTip(this.txtDefPitch, "피치 관절 두 개 (베이스 쪽부터, 예: 12,13) — 관절 합 = 이 둘 + 손목");
            this.txtDefPitch.TextChanged += new System.EventHandler(this.txtDefPitch_TextChanged);
            // 
            // lblDefHold
            // 
            this.lblDefHold.Location = new System.Drawing.Point(262, 53);
            this.lblDefHold.Name = "lblDefHold";
            this.lblDefHold.Size = new System.Drawing.Size(110, 20);
            this.lblDefHold.TabIndex = 6;
            this.lblDefHold.Text = "토크만 관절";
            this.lblDefHold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDefHold
            // 
            this.txtDefHold.Location = new System.Drawing.Point(376, 50);
            this.txtDefHold.Name = "txtDefHold";
            this.txtDefHold.Size = new System.Drawing.Size(130, 28);
            this.txtDefHold.TabIndex = 7;
            this.txtDefHold.Text = "15,16";
            this.tipArmExp.SetToolTip(this.txtDefHold, "토크만 켜서 고정하는 관절 (그리퍼 — 공구 끝 체인 밖, 비우면 없음)");
            this.txtDefHold.TextChanged += new System.EventHandler(this.txtDefHold_TextChanged);
            // 
            // lblDefTcpLen
            // 
            this.lblDefTcpLen.Location = new System.Drawing.Point(10, 83);
            this.lblDefTcpLen.Name = "lblDefTcpLen";
            this.lblDefTcpLen.Size = new System.Drawing.Size(98, 20);
            this.lblDefTcpLen.TabIndex = 8;
            this.lblDefTcpLen.Text = "공구 끝 거리 mm";
            this.lblDefTcpLen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDefTcpLen
            // 
            this.txtDefTcpLen.Location = new System.Drawing.Point(110, 80);
            this.txtDefTcpLen.Name = "txtDefTcpLen";
            this.txtDefTcpLen.Size = new System.Drawing.Size(140, 28);
            this.txtDefTcpLen.TabIndex = 9;
            this.txtDefTcpLen.Text = "103.2";
            this.tipArmExp.SetToolTip(this.txtDefTcpLen, "손목 원점 → 공구 끝 기대 거리 (mm, 10~500) — 3D 와 0.5mm 안이어야. 비우면 확인 안 함");
            this.txtDefTcpLen.TextChanged += new System.EventHandler(this.txtDefTcpLen_TextChanged);
            // 
            // lblDefFixed
            // 
            this.lblDefFixed.Font = new System.Drawing.Font("맑은 고딕", 8.5F);
            this.lblDefFixed.ForeColor = System.Drawing.Color.Gray;
            this.lblDefFixed.Location = new System.Drawing.Point(10, 112);
            this.lblDefFixed.Name = "lblDefFixed";
            this.lblDefFixed.Size = new System.Drawing.Size(496, 62);
            this.lblDefFixed.TabIndex = 10;
            this.lblDefFixed.Text = "고정 규약: 아래 = 3D 의 −Z · 바닥은 3D 의 XY 평면과 평행 (높이는 바닥 찾기가 잼) · 로봇 베이스는 수평 · 펜은 손목 → 공구 끝 직선 위 · 확인 허용 0.5 mm / 0.5° · 실습 목표 자세 Ry 90 (= 이 정의의 수직)";
            this.lblDefFixed.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // tabArmExp
            // 
            this.tabArmExp.Controls.Add(this.grpArmPlan);
            this.tabArmExp.Controls.Add(this.grpArmMeasure);
            this.tabArmExp.Controls.Add(this.grpArmLearn);
            this.tabArmExp.Controls.Add(this.grpArmTest);
            this.tabArmExp.Controls.Add(this.grpArmApply);
            this.tabArmExp.Controls.Add(this.grpArmTwin);
            this.tabArmExp.Controls.Add(this.btnArmStop);
            this.tabArmExp.Controls.Add(this.btnArmOpenDir);
            this.tabArmExp.Controls.Add(this.lblArmBusy);
            this.tabArmExp.AutoScroll = true;
            this.tabArmExp.BackColor = System.Drawing.SystemColors.Control;
            this.tabArmExp.Location = new System.Drawing.Point(4, 28);
            this.tabArmExp.Name = "tabArmExp";
            this.tabArmExp.Size = new System.Drawing.Size(541, 470);
            this.tabArmExp.TabIndex = 3;
            this.tabArmExp.Text = "고급";
            // 
            // grpArmPlan
            // 
            this.grpArmPlan.Controls.Add(this.btnArmPlanMake);
            this.grpArmPlan.Controls.Add(this.btnArmPlanLoad);
            this.grpArmPlan.Controls.Add(this.btnArmPlanDry);
            this.grpArmPlan.Controls.Add(this.lblArmPlan);
            this.grpArmPlan.Location = new System.Drawing.Point(3, 3);
            this.grpArmPlan.Name = "grpArmPlan";
            this.grpArmPlan.Size = new System.Drawing.Size(515, 68);
            this.grpArmPlan.TabIndex = 0;
            this.grpArmPlan.TabStop = false;
            this.grpArmPlan.Text = "② 측정 계획";
            // 
            // btnArmPlanMake
            // 
            this.btnArmPlanMake.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(160)))), ((int)(((byte)(70)))));
            this.btnArmPlanMake.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmPlanMake.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmPlanMake.ForeColor = System.Drawing.Color.White;
            this.btnArmPlanMake.Location = new System.Drawing.Point(8, 18);
            this.btnArmPlanMake.Name = "btnArmPlanMake";
            this.btnArmPlanMake.Size = new System.Drawing.Size(100, 30);
            this.btnArmPlanMake.TabIndex = 0;
            this.btnArmPlanMake.Text = "계획 만들기";
            this.btnArmPlanMake.UseVisualStyleBackColor = false;
            this.btnArmPlanMake.Click += new System.EventHandler(this.btnArmPlanMake_Click);
            // 
            // btnArmPlanLoad
            // 
            this.btnArmPlanLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(160)))), ((int)(((byte)(70)))));
            this.btnArmPlanLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmPlanLoad.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmPlanLoad.ForeColor = System.Drawing.Color.White;
            this.btnArmPlanLoad.Location = new System.Drawing.Point(112, 18);
            this.btnArmPlanLoad.Name = "btnArmPlanLoad";
            this.btnArmPlanLoad.Size = new System.Drawing.Size(80, 30);
            this.btnArmPlanLoad.TabIndex = 1;
            this.btnArmPlanLoad.Text = "불러오기";
            this.btnArmPlanLoad.UseVisualStyleBackColor = false;
            this.btnArmPlanLoad.Click += new System.EventHandler(this.btnArmPlanLoad_Click);
            // 
            // btnArmPlanDry
            // 
            this.btnArmPlanDry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnArmPlanDry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmPlanDry.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmPlanDry.ForeColor = System.Drawing.Color.White;
            this.btnArmPlanDry.Location = new System.Drawing.Point(196, 18);
            this.btnArmPlanDry.Name = "btnArmPlanDry";
            this.btnArmPlanDry.Size = new System.Drawing.Size(100, 30);
            this.btnArmPlanDry.TabIndex = 2;
            this.btnArmPlanDry.Text = "3D 미리보기";
            this.tipArmExp.SetToolTip(this.btnArmPlanDry, "3D 만 — 실물은 움직이지 않는다");
            this.btnArmPlanDry.UseVisualStyleBackColor = false;
            this.btnArmPlanDry.Click += new System.EventHandler(this.btnArmPlanDry_Click);
            // 
            // lblArmPlan
            // 
            this.lblArmPlan.Location = new System.Drawing.Point(300, 14);
            this.lblArmPlan.Name = "lblArmPlan";
            this.lblArmPlan.Size = new System.Drawing.Size(208, 50);
            this.lblArmPlan.TabIndex = 3;
            this.lblArmPlan.Text = "계획 없음";
            this.lblArmPlan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpArmMeasure
            // 
            this.grpArmMeasure.Controls.Add(this.chkArmHold);
            this.grpArmMeasure.Controls.Add(this.btnArmMeasure);
            this.grpArmMeasure.Controls.Add(this.lblArmMeasure);
            this.grpArmMeasure.Location = new System.Drawing.Point(3, 75);
            this.grpArmMeasure.Name = "grpArmMeasure";
            this.grpArmMeasure.Size = new System.Drawing.Size(515, 68);
            this.grpArmMeasure.TabIndex = 1;
            this.grpArmMeasure.TabStop = false;
            this.grpArmMeasure.Text = "③ 바닥 측정 (실물)";
            // 
            // chkArmHold
            // 
            this.chkArmHold.AutoSize = true;
            this.chkArmHold.Checked = true;
            this.chkArmHold.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkArmHold.Location = new System.Drawing.Point(10, 24);
            this.chkArmHold.Name = "chkArmHold";
            this.chkArmHold.Size = new System.Drawing.Size(118, 22);
            this.chkArmHold.TabIndex = 0;
            this.chkArmHold.Text = "손목 수직 유지";
            this.tipArmExp.SetToolTip(this.chkArmHold, "측정 때 손목 수직 유지 — 데이터셋 hold 칸으로 남는다");
            this.chkArmHold.UseVisualStyleBackColor = true;
            this.chkArmHold.CheckedChanged += new System.EventHandler(this.chkArmHold_CheckedChanged);
            // 
            // btnArmMeasure
            // 
            this.btnArmMeasure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnArmMeasure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmMeasure.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmMeasure.ForeColor = System.Drawing.Color.White;
            this.btnArmMeasure.Location = new System.Drawing.Point(132, 18);
            this.btnArmMeasure.Name = "btnArmMeasure";
            this.btnArmMeasure.Size = new System.Drawing.Size(112, 30);
            this.btnArmMeasure.TabIndex = 1;
            this.btnArmMeasure.Text = "측정 시작";
            this.tipArmExp.SetToolTip(this.btnArmMeasure, "실물이 움직인다 — 확인창 뒤에 시작 ([중단] 은 언제든)");
            this.btnArmMeasure.UseVisualStyleBackColor = false;
            this.btnArmMeasure.Click += new System.EventHandler(this.btnArmMeasure_Click);
            // 
            // lblArmMeasure
            // 
            this.lblArmMeasure.Location = new System.Drawing.Point(248, 14);
            this.lblArmMeasure.Name = "lblArmMeasure";
            this.lblArmMeasure.Size = new System.Drawing.Size(260, 50);
            this.lblArmMeasure.TabIndex = 2;
            this.lblArmMeasure.Text = "";
            this.lblArmMeasure.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpArmLearn
            // 
            this.grpArmLearn.Controls.Add(this.lblArmData);
            this.grpArmLearn.Controls.Add(this.lblArmDataInfo);
            this.grpArmLearn.Controls.Add(this.chkArmUseHold);
            this.grpArmLearn.Controls.Add(this.lblArmRef);
            this.grpArmLearn.Controls.Add(this.txtArmRef);
            this.grpArmLearn.Controls.Add(this.btnArmLearn);
            this.grpArmLearn.Controls.Add(this.lblArmLearn);
            this.grpArmLearn.Location = new System.Drawing.Point(3, 147);
            this.grpArmLearn.Name = "grpArmLearn";
            this.grpArmLearn.Size = new System.Drawing.Size(515, 116);
            this.grpArmLearn.TabIndex = 2;
            this.grpArmLearn.TabStop = false;
            this.grpArmLearn.Text = "④ 학습 → 결과 파일";
            // 
            // lblArmData
            // 
            this.lblArmData.Location = new System.Drawing.Point(8, 20);
            this.lblArmData.Name = "lblArmData";
            this.lblArmData.Size = new System.Drawing.Size(50, 23);
            this.lblArmData.TabIndex = 0;
            this.lblArmData.Text = "데이터";
            this.lblArmData.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblArmDataInfo
            // 
            this.lblArmDataInfo.Location = new System.Drawing.Point(60, 18);
            this.lblArmDataInfo.Name = "lblArmDataInfo";
            this.lblArmDataInfo.Size = new System.Drawing.Size(448, 28);
            this.lblArmDataInfo.TabIndex = 1;
            this.lblArmDataInfo.Text = "floor_dataset_fc.csv";
            this.lblArmDataInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkArmUseHold
            // 
            this.chkArmUseHold.AutoSize = true;
            this.chkArmUseHold.Checked = true;
            this.chkArmUseHold.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkArmUseHold.Location = new System.Drawing.Point(10, 50);
            this.chkArmUseHold.Name = "chkArmUseHold";
            this.chkArmUseHold.Size = new System.Drawing.Size(122, 22);
            this.chkArmUseHold.TabIndex = 2;
            this.chkArmUseHold.Text = "손목 유지 켬 행도";
            this.tipArmExp.SetToolTip(this.chkArmUseHold, "손목 유지 켬(hold=1)으로 잰 행도 학습에 넣는다");
            this.chkArmUseHold.UseVisualStyleBackColor = true;
            // 
            // lblArmRef
            // 
            this.lblArmRef.Location = new System.Drawing.Point(138, 50);
            this.lblArmRef.Name = "lblArmRef";
            this.lblArmRef.Size = new System.Drawing.Size(134, 23);
            this.lblArmRef.TabIndex = 3;
            this.lblArmRef.Text = "기준 mm (빈칸=평균)";
            this.lblArmRef.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtArmRef
            // 
            this.txtArmRef.Location = new System.Drawing.Point(274, 48);
            this.txtArmRef.Name = "txtArmRef";
            this.txtArmRef.Size = new System.Drawing.Size(56, 28);
            this.txtArmRef.TabIndex = 4;
            this.txtArmRef.Text = "";
            this.txtArmRef.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipArmExp.SetToolTip(this.txtArmRef, "기준 높이 Ref (mm) — 비우면 학습 평균, 숫자면 그 높이로 고정 (fcref)");
            // 
            // btnArmLearn
            // 
            this.btnArmLearn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(160)))), ((int)(((byte)(70)))));
            this.btnArmLearn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmLearn.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmLearn.ForeColor = System.Drawing.Color.White;
            this.btnArmLearn.Location = new System.Drawing.Point(404, 46);
            this.btnArmLearn.Name = "btnArmLearn";
            this.btnArmLearn.Size = new System.Drawing.Size(104, 30);
            this.btnArmLearn.TabIndex = 5;
            this.btnArmLearn.Text = "학습";
            this.btnArmLearn.UseVisualStyleBackColor = false;
            this.btnArmLearn.Click += new System.EventHandler(this.btnArmLearn_Click);
            // 
            // lblArmLearn
            // 
            this.lblArmLearn.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblArmLearn.Location = new System.Drawing.Point(8, 80);
            this.lblArmLearn.Name = "lblArmLearn";
            this.lblArmLearn.Size = new System.Drawing.Size(500, 32);
            this.lblArmLearn.TabIndex = 6;
            this.lblArmLearn.Text = "";
            this.lblArmLearn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpArmTest
            // 
            this.grpArmTest.Controls.Add(this.lblArmResult);
            this.grpArmTest.Controls.Add(this.cmbArmResult);
            this.grpArmTest.Controls.Add(this.btnArmResultBrowse);
            this.grpArmTest.Controls.Add(this.lblArmTouchRow);
            this.grpArmTest.Controls.Add(this.lblArmTestR);
            this.grpArmTest.Controls.Add(this.txtArmTestR);
            this.grpArmTest.Controls.Add(this.lblArmTestYaw);
            this.grpArmTest.Controls.Add(this.txtArmTestYaw);
            this.grpArmTest.Controls.Add(this.lblArmTestPitch);
            this.grpArmTest.Controls.Add(this.txtArmTestPitch);
            this.grpArmTest.Controls.Add(this.lblArmTestRep);
            this.grpArmTest.Controls.Add(this.txtArmTestRep);
            this.grpArmTest.Controls.Add(this.chkArmTestHold);
            this.grpArmTest.Controls.Add(this.btnArmTestDry);
            this.grpArmTest.Controls.Add(this.btnArmTest);
            this.grpArmTest.Controls.Add(this.lblArmTest);
            this.grpArmTest.Location = new System.Drawing.Point(3, 267);
            this.grpArmTest.Name = "grpArmTest";
            this.grpArmTest.Size = new System.Drawing.Size(515, 184);
            this.grpArmTest.TabIndex = 3;
            this.grpArmTest.TabStop = false;
            this.grpArmTest.Text = "⑤ 결과 파일 닿기 시험 (보정 끔 ↔ 켬)";
            // 
            // lblArmResult
            // 
            this.lblArmResult.Location = new System.Drawing.Point(8, 20);
            this.lblArmResult.Name = "lblArmResult";
            this.lblArmResult.Size = new System.Drawing.Size(40, 23);
            this.lblArmResult.TabIndex = 0;
            this.lblArmResult.Text = "파일";
            this.lblArmResult.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbArmResult
            // 
            this.cmbArmResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbArmResult.FormattingEnabled = true;
            this.cmbArmResult.Location = new System.Drawing.Point(50, 20);
            this.cmbArmResult.Name = "cmbArmResult";
            this.cmbArmResult.Size = new System.Drawing.Size(418, 26);
            this.cmbArmResult.TabIndex = 1;
            this.cmbArmResult.SelectedIndexChanged += new System.EventHandler(this.cmbArmResult_SelectedIndexChanged);
            // 
            // btnArmResultBrowse
            // 
            this.btnArmResultBrowse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnArmResultBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmResultBrowse.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmResultBrowse.ForeColor = System.Drawing.Color.White;
            this.btnArmResultBrowse.Location = new System.Drawing.Point(472, 19);
            this.btnArmResultBrowse.Name = "btnArmResultBrowse";
            this.btnArmResultBrowse.Size = new System.Drawing.Size(36, 28);
            this.btnArmResultBrowse.TabIndex = 2;
            this.btnArmResultBrowse.Text = "…";
            this.btnArmResultBrowse.UseVisualStyleBackColor = false;
            this.btnArmResultBrowse.Click += new System.EventHandler(this.btnArmResultBrowse_Click);
            // 
            // lblArmTouchRow
            // 
            this.lblArmTouchRow.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblArmTouchRow.Location = new System.Drawing.Point(8, 58);
            this.lblArmTouchRow.Name = "lblArmTouchRow";
            this.lblArmTouchRow.Size = new System.Drawing.Size(66, 23);
            this.lblArmTouchRow.TabIndex = 7;
            this.lblArmTouchRow.Text = "닿기 시험";
            this.lblArmTouchRow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblArmTestR
            // 
            this.lblArmTestR.Location = new System.Drawing.Point(76, 58);
            this.lblArmTestR.Name = "lblArmTestR";
            this.lblArmTestR.Size = new System.Drawing.Size(34, 23);
            this.lblArmTestR.TabIndex = 8;
            this.lblArmTestR.Text = "반경";
            this.lblArmTestR.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtArmTestR
            // 
            this.txtArmTestR.Location = new System.Drawing.Point(112, 56);
            this.txtArmTestR.Name = "txtArmTestR";
            this.txtArmTestR.Size = new System.Drawing.Size(120, 28);
            this.txtArmTestR.TabIndex = 9;
            this.txtArmTestR.Text = "255,235,175,145,125";
            this.tipArmExp.SetToolTip(this.txtArmTestR, "닿기 시험 반경 (mm, 쉼표로 여러 개) — 학습 반경 사이 값 (배운 점을 다시 재지 않는다)");
            // 
            // lblArmTestYaw
            // 
            this.lblArmTestYaw.Location = new System.Drawing.Point(234, 58);
            this.lblArmTestYaw.Name = "lblArmTestYaw";
            this.lblArmTestYaw.Size = new System.Drawing.Size(20, 23);
            this.lblArmTestYaw.TabIndex = 10;
            this.lblArmTestYaw.Text = "요";
            this.lblArmTestYaw.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtArmTestYaw
            // 
            this.txtArmTestYaw.Location = new System.Drawing.Point(256, 56);
            this.txtArmTestYaw.Name = "txtArmTestYaw";
            this.txtArmTestYaw.Size = new System.Drawing.Size(56, 28);
            this.txtArmTestYaw.TabIndex = 11;
            this.txtArmTestYaw.Text = "0";
            this.txtArmTestYaw.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipArmExp.SetToolTip(this.txtArmTestYaw, "닿기 시험 요 (도, 쉼표로 여러 개) — 점 = 요 × 반경");
            // 
            // lblArmTestPitch
            // 
            this.lblArmTestPitch.Location = new System.Drawing.Point(314, 58);
            this.lblArmTestPitch.Name = "lblArmTestPitch";
            this.lblArmTestPitch.Size = new System.Drawing.Size(50, 23);
            this.lblArmTestPitch.TabIndex = 12;
            this.lblArmTestPitch.Text = "공구 Ry";
            this.lblArmTestPitch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tipArmExp.SetToolTip(this.lblArmTestPitch, "공구 피치 Ry (도) — 90 = 수직 아래, 90 이하. 손목 해는 관절 합(피치 관절 2개 + 손목) = 로봇 정의의 '바닥으로 수직' 관절 합 + (90 − Ry) 쪽 (OMX: 12+13+14 = 180 − Ry). 데이터셋·판정의 '피치 110' = Ry 70.");
            // 
            // txtArmTestPitch
            // 
            this.txtArmTestPitch.Location = new System.Drawing.Point(366, 56);
            this.txtArmTestPitch.Name = "txtArmTestPitch";
            this.txtArmTestPitch.Size = new System.Drawing.Size(36, 28);
            this.txtArmTestPitch.TabIndex = 13;
            this.txtArmTestPitch.Text = "90";
            this.txtArmTestPitch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipArmExp.SetToolTip(this.txtArmTestPitch, "공구 피치 Ry (도) — 90 = 수직 아래, 90 이하. 손목 해는 관절 합(피치 관절 2개 + 손목) = 로봇 정의의 '바닥으로 수직' 관절 합 + (90 − Ry) 쪽 (OMX: 12+13+14 = 180 − Ry). 데이터셋·판정의 '피치 110' = Ry 70.");
            // 
            // lblArmTestRep
            // 
            this.lblArmTestRep.Location = new System.Drawing.Point(404, 58);
            this.lblArmTestRep.Name = "lblArmTestRep";
            this.lblArmTestRep.Size = new System.Drawing.Size(32, 23);
            this.lblArmTestRep.TabIndex = 14;
            this.lblArmTestRep.Text = "반복";
            this.lblArmTestRep.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtArmTestRep
            // 
            this.txtArmTestRep.Location = new System.Drawing.Point(438, 56);
            this.txtArmTestRep.Name = "txtArmTestRep";
            this.txtArmTestRep.Size = new System.Drawing.Size(28, 28);
            this.txtArmTestRep.TabIndex = 15;
            this.txtArmTestRep.Text = "2";
            this.txtArmTestRep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tipArmExp.SetToolTip(this.txtArmTestRep, "점마다 보정 끔 → 켬 을 몇 번 (2 = 끔·켬·끔·켬)");
            // 
            // chkArmTestHold
            // 
            this.chkArmTestHold.AutoSize = true;
            this.chkArmTestHold.Location = new System.Drawing.Point(78, 94);
            this.chkArmTestHold.Name = "chkArmTestHold";
            this.chkArmTestHold.Size = new System.Drawing.Size(84, 22);
            this.chkArmTestHold.TabIndex = 16;
            this.chkArmTestHold.Text = "손목 유지";
            this.tipArmExp.SetToolTip(this.chkArmTestHold, "닿기 시험 때 손목 수직 유지 (끔 = 실사용 조건)");
            this.chkArmTestHold.UseVisualStyleBackColor = true;
            // 
            // btnArmTestDry
            // 
            this.btnArmTestDry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnArmTestDry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmTestDry.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmTestDry.ForeColor = System.Drawing.Color.White;
            this.btnArmTestDry.Location = new System.Drawing.Point(166, 90);
            this.btnArmTestDry.Name = "btnArmTestDry";
            this.btnArmTestDry.Size = new System.Drawing.Size(96, 30);
            this.btnArmTestDry.TabIndex = 17;
            this.btnArmTestDry.Text = "3D 닿기";
            this.tipArmExp.SetToolTip(this.btnArmTestDry, "3D 만 — 실물은 움직이지 않는다");
            this.btnArmTestDry.UseVisualStyleBackColor = false;
            this.btnArmTestDry.Click += new System.EventHandler(this.btnArmTestDry_Click);
            // 
            // btnArmTest
            // 
            this.btnArmTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnArmTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmTest.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmTest.ForeColor = System.Drawing.Color.White;
            this.btnArmTest.Location = new System.Drawing.Point(266, 90);
            this.btnArmTest.Name = "btnArmTest";
            this.btnArmTest.Size = new System.Drawing.Size(104, 30);
            this.btnArmTest.TabIndex = 18;
            this.btnArmTest.Text = "실물 닿기";
            this.tipArmExp.SetToolTip(this.btnArmTest, "실물이 움직인다 — 확인창 뒤에 시작 ([중단] 은 언제든)");
            this.btnArmTest.UseVisualStyleBackColor = false;
            this.btnArmTest.Click += new System.EventHandler(this.btnArmTest_Click);
            // 
            // lblArmTest
            // 
            this.lblArmTest.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblArmTest.Location = new System.Drawing.Point(8, 128);
            this.lblArmTest.Name = "lblArmTest";
            this.lblArmTest.Size = new System.Drawing.Size(500, 52);
            this.lblArmTest.TabIndex = 23;
            this.lblArmTest.Text = "";
            this.lblArmTest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpArmApply
            // 
            this.grpArmApply.Controls.Add(this.btnArmApply);
            this.grpArmApply.Controls.Add(this.lblArmActive);
            this.grpArmApply.Controls.Add(this.lblArmBackup);
            this.grpArmApply.Controls.Add(this.cmbArmBackup);
            this.grpArmApply.Controls.Add(this.btnArmRollback);
            this.grpArmApply.Location = new System.Drawing.Point(3, 455);
            this.grpArmApply.Name = "grpArmApply";
            this.grpArmApply.Size = new System.Drawing.Size(515, 102);
            this.grpArmApply.TabIndex = 4;
            this.grpArmApply.TabStop = false;
            this.grpArmApply.Text = "⑥ 적용 (활성 모델 바꾸기)";
            // 
            // btnArmApply
            // 
            this.btnArmApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(160)))), ((int)(((byte)(70)))));
            this.btnArmApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmApply.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmApply.ForeColor = System.Drawing.Color.White;
            this.btnArmApply.Location = new System.Drawing.Point(8, 18);
            this.btnArmApply.Name = "btnArmApply";
            this.btnArmApply.Size = new System.Drawing.Size(120, 30);
            this.btnArmApply.TabIndex = 0;
            this.btnArmApply.Text = "선택 파일 적용";
            this.btnArmApply.UseVisualStyleBackColor = false;
            this.btnArmApply.Click += new System.EventHandler(this.btnArmApply_Click);
            // 
            // lblArmActive
            // 
            this.lblArmActive.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblArmActive.Location = new System.Drawing.Point(132, 14);
            this.lblArmActive.Name = "lblArmActive";
            this.lblArmActive.Size = new System.Drawing.Size(376, 50);
            this.lblArmActive.TabIndex = 1;
            this.lblArmActive.Text = "";
            this.lblArmActive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblArmBackup
            // 
            this.lblArmBackup.Location = new System.Drawing.Point(8, 70);
            this.lblArmBackup.Name = "lblArmBackup";
            this.lblArmBackup.Size = new System.Drawing.Size(40, 23);
            this.lblArmBackup.TabIndex = 2;
            this.lblArmBackup.Text = "백업";
            this.lblArmBackup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbArmBackup
            // 
            this.cmbArmBackup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbArmBackup.FormattingEnabled = true;
            this.cmbArmBackup.Location = new System.Drawing.Point(50, 70);
            this.cmbArmBackup.Name = "cmbArmBackup";
            this.cmbArmBackup.Size = new System.Drawing.Size(336, 26);
            this.cmbArmBackup.TabIndex = 3;
            // 
            // btnArmRollback
            // 
            this.btnArmRollback.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(120)))), ((int)(((byte)(60)))));
            this.btnArmRollback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmRollback.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmRollback.ForeColor = System.Drawing.Color.White;
            this.btnArmRollback.Location = new System.Drawing.Point(394, 67);
            this.btnArmRollback.Name = "btnArmRollback";
            this.btnArmRollback.Size = new System.Drawing.Size(114, 30);
            this.btnArmRollback.TabIndex = 4;
            this.btnArmRollback.Text = "되돌리기";
            this.btnArmRollback.UseVisualStyleBackColor = false;
            this.btnArmRollback.Click += new System.EventHandler(this.btnArmRollback_Click);
            // 
            // grpArmTwin
            // 
            this.grpArmTwin.Controls.Add(this.lblSagTwin);
            this.grpArmTwin.Controls.Add(this.cmbSagTwin);
            this.grpArmTwin.Controls.Add(this.chkSagCompOn);
            this.grpArmTwin.Controls.Add(this.lblArmTwinSrc);
            this.grpArmTwin.Controls.Add(this.cmbArmTwinSrc);
            this.grpArmTwin.Controls.Add(this.lblArmTwin);
            this.grpArmTwin.Location = new System.Drawing.Point(3, 561);
            this.grpArmTwin.Name = "grpArmTwin";
            this.grpArmTwin.Size = new System.Drawing.Size(515, 128);
            this.grpArmTwin.TabIndex = 5;
            this.grpArmTwin.TabStop = false;
            this.grpArmTwin.Text = "⑦ 쌍둥이 (실물이 있을 자세)";
            // 
            // lblArmTwinSrc
            // 
            this.lblArmTwinSrc.Location = new System.Drawing.Point(8, 60);
            this.lblArmTwinSrc.Name = "lblArmTwinSrc";
            this.lblArmTwinSrc.Size = new System.Drawing.Size(90, 20);
            this.lblArmTwinSrc.TabIndex = 3;
            this.lblArmTwinSrc.Text = "쌍둥이 모델:";
            this.lblArmTwinSrc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbArmTwinSrc
            // 
            this.cmbArmTwinSrc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbArmTwinSrc.FormattingEnabled = true;
            this.cmbArmTwinSrc.Items.AddRange(new object[] {
            "적용 중 모델",
            "선택한 결과 파일"});
            this.cmbArmTwinSrc.Location = new System.Drawing.Point(100, 56);
            this.cmbArmTwinSrc.Name = "cmbArmTwinSrc";
            this.cmbArmTwinSrc.Size = new System.Drawing.Size(150, 26);
            this.cmbArmTwinSrc.TabIndex = 4;
            this.cmbArmTwinSrc.SelectedIndexChanged += new System.EventHandler(this.cmbArmTwinSrc_SelectedIndexChanged);
            // 
            // lblArmTwin
            // 
            this.lblArmTwin.Location = new System.Drawing.Point(8, 84);
            this.lblArmTwin.Name = "lblArmTwin";
            this.lblArmTwin.Size = new System.Drawing.Size(500, 40);
            this.lblArmTwin.TabIndex = 5;
            this.lblArmTwin.Text = "";
            this.lblArmTwin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnArmStop
            // 
            this.btnArmStop.BackColor = System.Drawing.Color.Salmon;
            this.btnArmStop.Enabled = false;
            this.btnArmStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmStop.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmStop.ForeColor = System.Drawing.Color.Black;
            this.btnArmStop.Location = new System.Drawing.Point(3, 693);
            this.btnArmStop.Name = "btnArmStop";
            this.btnArmStop.Size = new System.Drawing.Size(110, 30);
            this.btnArmStop.TabIndex = 6;
            this.btnArmStop.Text = "중단";
            this.btnArmStop.UseVisualStyleBackColor = false;
            this.btnArmStop.Click += new System.EventHandler(this.btnArmStop_Click);
            // 
            // btnArmOpenDir
            // 
            this.btnArmOpenDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnArmOpenDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArmOpenDir.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnArmOpenDir.ForeColor = System.Drawing.Color.White;
            this.btnArmOpenDir.Location = new System.Drawing.Point(117, 693);
            this.btnArmOpenDir.Name = "btnArmOpenDir";
            this.btnArmOpenDir.Size = new System.Drawing.Size(100, 30);
            this.btnArmOpenDir.TabIndex = 7;
            this.btnArmOpenDir.Text = "폴더 열기";
            this.btnArmOpenDir.UseVisualStyleBackColor = false;
            this.btnArmOpenDir.Click += new System.EventHandler(this.btnArmOpenDir_Click);
            // 
            // lblArmBusy
            // 
            this.lblArmBusy.Location = new System.Drawing.Point(221, 693);
            this.lblArmBusy.Name = "lblArmBusy";
            this.lblArmBusy.Size = new System.Drawing.Size(297, 40);
            this.lblArmBusy.TabIndex = 8;
            this.lblArmBusy.Text = "대기";
            this.lblArmBusy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tmrArmFollow
            // 
            this.tmrArmFollow.Interval = 100;
            this.tmrArmFollow.Tick += new System.EventHandler(this.tmrArmFollow_Tick);
            // 
            // tipArmExp
            // 
            this.tipArmExp.AutoPopDelay = 20000;
            this.tipArmExp.InitialDelay = 400;
            this.tipArmExp.ReshowDelay = 100;
            // 
            // btnUsdExport
            // 
            this.btnUsdExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnUsdExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsdExport.Font = new System.Drawing.Font("맑은 고딕", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnUsdExport.ForeColor = System.Drawing.Color.LightSkyBlue;
            this.btnUsdExport.Location = new System.Drawing.Point(1397, 737);
            this.btnUsdExport.Name = "btnUsdExport";
            this.btnUsdExport.Size = new System.Drawing.Size(144, 54);
            this.btnUsdExport.TabIndex = 301;
            this.btnUsdExport.Text = "USD Export";
            this.btnUsdExport.UseVisualStyleBackColor = false;
            this.btnUsdExport.Click += new System.EventHandler(this.btnExportUsd_Click);
            // 
            // btnSignCalib
            // 
            this.btnSignCalib.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnSignCalib.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSignCalib.Font = new System.Drawing.Font("맑은 고딕", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSignCalib.ForeColor = System.Drawing.Color.Orange;
            this.btnSignCalib.Location = new System.Drawing.Point(1397, 284);
            this.btnSignCalib.Name = "btnSignCalib";
            this.btnSignCalib.Size = new System.Drawing.Size(144, 38);
            this.btnSignCalib.TabIndex = 303;
            this.btnSignCalib.Text = "1) 관절방향";
            this.btnSignCalib.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSignCalib.UseVisualStyleBackColor = false;
            this.btnSignCalib.Click += new System.EventHandler(this.btnSignCalib_Click);
            // 
            // btnSignClear
            // 
            this.btnSignClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnSignClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSignClear.Font = new System.Drawing.Font("맑은 고딕", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnSignClear.ForeColor = System.Drawing.Color.Tomato;
            this.btnSignClear.Location = new System.Drawing.Point(1397, 689);
            this.btnSignClear.Name = "btnSignClear";
            this.btnSignClear.Size = new System.Drawing.Size(144, 38);
            this.btnSignClear.TabIndex = 304;
            this.btnSignClear.Text = "지움(다시하자)";
            this.btnSignClear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSignClear.UseVisualStyleBackColor = false;
            this.btnSignClear.Click += new System.EventHandler(this.btnSignClear_Click);
            // 
            // btnMechLimit
            // 
            this.btnMechLimit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnMechLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMechLimit.Font = new System.Drawing.Font("맑은 고딕", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnMechLimit.ForeColor = System.Drawing.Color.Gold;
            this.btnMechLimit.Location = new System.Drawing.Point(1397, 328);
            this.btnMechLimit.Name = "btnMechLimit";
            this.btnMechLimit.Size = new System.Drawing.Size(144, 38);
            this.btnMechLimit.TabIndex = 305;
            this.btnMechLimit.Text = "2) 관절리미트(손)";
            this.btnMechLimit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMechLimit.UseVisualStyleBackColor = false;
            this.btnMechLimit.Click += new System.EventHandler(this.btnMechLimit_Click);
            // 
            // btnSim2Real
            // 
            this.btnSim2Real.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnSim2Real.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSim2Real.Font = new System.Drawing.Font("맑은 고딕", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSim2Real.ForeColor = System.Drawing.Color.LightGreen;
            this.btnSim2Real.Location = new System.Drawing.Point(1397, 372);
            this.btnSim2Real.Name = "btnSim2Real";
            this.btnSim2Real.Size = new System.Drawing.Size(144, 38);
            this.btnSim2Real.TabIndex = 306;
            this.btnSim2Real.Text = "3) sim2real";
            this.btnSim2Real.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSim2Real.UseVisualStyleBackColor = false;
            this.btnSim2Real.Click += new System.EventHandler(this.btnSim2Real_Click);
            // 
            // btnStandDroop
            // 
            this.btnStandDroop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnStandDroop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStandDroop.Font = new System.Drawing.Font("맑은 고딕", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnStandDroop.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnStandDroop.Location = new System.Drawing.Point(1397, 416);
            this.btnStandDroop.Name = "btnStandDroop";
            this.btnStandDroop.Size = new System.Drawing.Size(144, 38);
            this.btnStandDroop.TabIndex = 307;
            this.btnStandDroop.Text = "4) 기립처짐";
            this.btnStandDroop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStandDroop.UseVisualStyleBackColor = false;
            this.btnStandDroop.Click += new System.EventHandler(this.btnStandDroop_Click);
            // 
            // btnS2RGuide
            // 
            this.btnS2RGuide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnS2RGuide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnS2RGuide.Font = new System.Drawing.Font("맑은 고딕", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnS2RGuide.ForeColor = System.Drawing.Color.White;
            this.btnS2RGuide.Location = new System.Drawing.Point(1455, 244);
            this.btnS2RGuide.Name = "btnS2RGuide";
            this.btnS2RGuide.Size = new System.Drawing.Size(86, 38);
            this.btnS2RGuide.TabIndex = 311;
            this.btnS2RGuide.Text = "? 설명서";
            this.btnS2RGuide.UseVisualStyleBackColor = false;
            this.btnS2RGuide.Click += new System.EventHandler(this.btnS2RGuide_Click);
            // 
            // btnIsaacCam
            // 
            this.btnIsaacCam.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnIsaacCam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIsaacCam.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.btnIsaacCam.ForeColor = System.Drawing.Color.Aqua;
            this.btnIsaacCam.Location = new System.Drawing.Point(656, 654);
            this.btnIsaacCam.Name = "btnIsaacCam";
            this.btnIsaacCam.Size = new System.Drawing.Size(149, 41);
            this.btnIsaacCam.TabIndex = 313;
            this.btnIsaacCam.Text = "📺 Isaac 영상";
            this.btnIsaacCam.UseVisualStyleBackColor = false;
            this.btnIsaacCam.Click += new System.EventHandler(this.btnIsaacCam_Click);
            // 
            // btnSagComp
            // 
            this.btnSagComp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnSagComp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSagComp.Font = new System.Drawing.Font("맑은 고딕", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnSagComp.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnSagComp.Location = new System.Drawing.Point(1397, 632);
            this.btnSagComp.Name = "btnSagComp";
            this.btnSagComp.Size = new System.Drawing.Size(144, 38);
            this.btnSagComp.TabIndex = 313;
            this.btnSagComp.Text = "9) 처짐보상";
            this.btnSagComp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSagComp.UseVisualStyleBackColor = false;
            this.btnSagComp.Click += new System.EventHandler(this.btnSagComp_Click);
            // 
            // btnLoadVmax
            // 
            this.btnLoadVmax.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnLoadVmax.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadVmax.Font = new System.Drawing.Font("맑은 고딕", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnLoadVmax.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btnLoadVmax.Location = new System.Drawing.Point(1397, 589);
            this.btnLoadVmax.Name = "btnLoadVmax";
            this.btnLoadVmax.Size = new System.Drawing.Size(144, 38);
            this.btnLoadVmax.TabIndex = 312;
            this.btnLoadVmax.Text = "8) 부하속도";
            this.btnLoadVmax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoadVmax.UseVisualStyleBackColor = false;
            this.btnLoadVmax.Click += new System.EventHandler(this.btnLoadVmax_Click);
            // 
            // btnServoIdent
            // 
            this.btnServoIdent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnServoIdent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnServoIdent.Font = new System.Drawing.Font("맑은 고딕", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnServoIdent.ForeColor = System.Drawing.Color.Violet;
            this.btnServoIdent.Location = new System.Drawing.Point(1397, 461);
            this.btnServoIdent.Name = "btnServoIdent";
            this.btnServoIdent.Size = new System.Drawing.Size(144, 38);
            this.btnServoIdent.TabIndex = 308;
            this.btnServoIdent.Text = "5) 서보동정";
            this.btnServoIdent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnServoIdent.UseVisualStyleBackColor = false;
            this.btnServoIdent.Click += new System.EventHandler(this.btnServoIdent_Click);
            // 
            // btnTipBound
            // 
            this.btnTipBound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnTipBound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTipBound.Font = new System.Drawing.Font("맑은 고딕", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnTipBound.ForeColor = System.Drawing.Color.Khaki;
            this.btnTipBound.Location = new System.Drawing.Point(1397, 505);
            this.btnTipBound.Name = "btnTipBound";
            this.btnTipBound.Size = new System.Drawing.Size(144, 38);
            this.btnTipBound.TabIndex = 309;
            this.btnTipBound.Text = "6) 전도경계";
            this.btnTipBound.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTipBound.UseVisualStyleBackColor = false;
            this.btnTipBound.Click += new System.EventHandler(this.btnTipBound_Click);
            // 
            // btnPowerProf
            // 
            this.btnPowerProf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnPowerProf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPowerProf.Font = new System.Drawing.Font("맑은 고딕", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnPowerProf.ForeColor = System.Drawing.Color.SandyBrown;
            this.btnPowerProf.Location = new System.Drawing.Point(1397, 546);
            this.btnPowerProf.Name = "btnPowerProf";
            this.btnPowerProf.Size = new System.Drawing.Size(144, 38);
            this.btnPowerProf.TabIndex = 310;
            this.btnPowerProf.Text = "7) 전원측정";
            this.btnPowerProf.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPowerProf.UseVisualStyleBackColor = false;
            this.btnPowerProf.Click += new System.EventHandler(this.btnPowerProf_Click);
            // 
            // btnViewMjcf
            // 
            this.btnViewMjcf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnViewMjcf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewMjcf.Font = new System.Drawing.Font("맑은 고딕", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnViewMjcf.ForeColor = System.Drawing.Color.LightGoldenrodYellow;
            this.btnViewMjcf.Location = new System.Drawing.Point(327, 699);
            this.btnViewMjcf.Name = "btnViewMjcf";
            this.btnViewMjcf.Size = new System.Drawing.Size(125, 41);
            this.btnViewMjcf.TabIndex = 302;
            this.btnViewMjcf.Text = "📄 View MJCF";
            this.btnViewMjcf.UseVisualStyleBackColor = false;
            this.btnViewMjcf.Click += new System.EventHandler(this.btnViewMjcf_Click);
            // 
            // btnViewUsd
            // 
            this.btnViewUsd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnViewUsd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewUsd.Font = new System.Drawing.Font("맑은 고딕", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnViewUsd.ForeColor = System.Drawing.Color.LightSkyBlue;
            this.btnViewUsd.Location = new System.Drawing.Point(134, 612);
            this.btnViewUsd.Name = "btnViewUsd";
            this.btnViewUsd.Size = new System.Drawing.Size(144, 41);
            this.btnViewUsd.TabIndex = 303;
            this.btnViewUsd.Text = "📄 View USD";
            this.btnViewUsd.UseVisualStyleBackColor = false;
            this.btnViewUsd.Click += new System.EventHandler(this.btnViewUsd_Click);
            // 
            // btnIsaacUsd
            // 
            this.btnIsaacUsd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnIsaacUsd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIsaacUsd.Font = new System.Drawing.Font("맑은 고딕", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnIsaacUsd.ForeColor = System.Drawing.Color.LightGreen;
            this.btnIsaacUsd.Location = new System.Drawing.Point(327, 655);
            this.btnIsaacUsd.Name = "btnIsaacUsd";
            this.btnIsaacUsd.Size = new System.Drawing.Size(125, 41);
            this.btnIsaacUsd.TabIndex = 304;
            this.btnIsaacUsd.Text = "🚀 Isaac (USD)";
            this.btnIsaacUsd.UseVisualStyleBackColor = false;
            this.btnIsaacUsd.Click += new System.EventHandler(this.btnIsaacUsd_Click);
            // 
            // btnView1
            // 
            this.btnView1.Location = new System.Drawing.Point(6, 3);
            this.btnView1.Name = "btnView1";
            this.btnView1.Size = new System.Drawing.Size(40, 26);
            this.btnView1.TabIndex = 200;
            this.btnView1.Text = "V1";
            this.btnView1.UseVisualStyleBackColor = true;
            this.btnView1.Click += new System.EventHandler(this.btnView1_Click);
            // 
            // btnView2
            // 
            this.btnView2.Location = new System.Drawing.Point(46, 3);
            this.btnView2.Name = "btnView2";
            this.btnView2.Size = new System.Drawing.Size(40, 26);
            this.btnView2.TabIndex = 201;
            this.btnView2.Text = "V2";
            this.btnView2.UseVisualStyleBackColor = true;
            this.btnView2.Click += new System.EventHandler(this.btnView2_Click);
            // 
            // btnView3
            // 
            this.btnView3.Location = new System.Drawing.Point(86, 3);
            this.btnView3.Name = "btnView3";
            this.btnView3.Size = new System.Drawing.Size(40, 26);
            this.btnView3.TabIndex = 202;
            this.btnView3.Text = "V3";
            this.btnView3.UseVisualStyleBackColor = true;
            this.btnView3.Click += new System.EventHandler(this.btnView3_Click);
            // 
            // btnView4
            // 
            this.btnView4.Location = new System.Drawing.Point(126, 3);
            this.btnView4.Name = "btnView4";
            this.btnView4.Size = new System.Drawing.Size(40, 26);
            this.btnView4.TabIndex = 203;
            this.btnView4.Text = "V4";
            this.btnView4.UseVisualStyleBackColor = true;
            this.btnView4.Click += new System.EventHandler(this.btnView4_Click);
            // 
            // btnView5
            // 
            this.btnView5.Location = new System.Drawing.Point(6, 35);
            this.btnView5.Name = "btnView5";
            this.btnView5.Size = new System.Drawing.Size(40, 26);
            this.btnView5.TabIndex = 204;
            this.btnView5.Text = "V5";
            this.btnView5.UseVisualStyleBackColor = true;
            this.btnView5.Click += new System.EventHandler(this.btnView5_Click);
            // 
            // btnView6
            // 
            this.btnView6.Location = new System.Drawing.Point(46, 35);
            this.btnView6.Name = "btnView6";
            this.btnView6.Size = new System.Drawing.Size(40, 26);
            this.btnView6.TabIndex = 205;
            this.btnView6.Text = "V6";
            this.btnView6.UseVisualStyleBackColor = true;
            this.btnView6.Click += new System.EventHandler(this.btnView6_Click);
            // 
            // btnView7
            // 
            this.btnView7.Location = new System.Drawing.Point(86, 35);
            this.btnView7.Name = "btnView7";
            this.btnView7.Size = new System.Drawing.Size(40, 26);
            this.btnView7.TabIndex = 206;
            this.btnView7.Text = "V7";
            this.btnView7.UseVisualStyleBackColor = true;
            this.btnView7.Click += new System.EventHandler(this.btnView7_Click);
            // 
            // btnView8
            // 
            this.btnView8.Location = new System.Drawing.Point(126, 35);
            this.btnView8.Name = "btnView8";
            this.btnView8.Size = new System.Drawing.Size(40, 26);
            this.btnView8.TabIndex = 207;
            this.btnView8.Text = "V8";
            this.btnView8.UseVisualStyleBackColor = true;
            this.btnView8.Click += new System.EventHandler(this.btnView8_Click);
            // 
            // btnViewDefault
            // 
            this.btnViewDefault.Location = new System.Drawing.Point(172, 3);
            this.btnViewDefault.Name = "btnViewDefault";
            this.btnViewDefault.Size = new System.Drawing.Size(70, 58);
            this.btnViewDefault.TabIndex = 208;
            this.btnViewDefault.Text = "Default";
            this.btnViewDefault.UseVisualStyleBackColor = true;
            this.btnViewDefault.Click += new System.EventHandler(this.btnViewDefault_Click);
            // 
            // lblViewTX
            // 
            this.lblViewTX.Location = new System.Drawing.Point(6, 69);
            this.lblViewTX.Name = "lblViewTX";
            this.lblViewTX.Size = new System.Drawing.Size(22, 18);
            this.lblViewTX.TabIndex = 210;
            this.lblViewTX.Text = "TX";
            // 
            // txtViewTX
            // 
            this.txtViewTX.Location = new System.Drawing.Point(28, 65);
            this.txtViewTX.Name = "txtViewTX";
            this.txtViewTX.Size = new System.Drawing.Size(54, 28);
            this.txtViewTX.TabIndex = 211;
            this.txtViewTX.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtViewAny_KeyDown);
            this.txtViewTX.Leave += new System.EventHandler(this.txtViewTrans_Leave);
            // 
            // lblViewTY
            // 
            this.lblViewTY.Location = new System.Drawing.Point(88, 69);
            this.lblViewTY.Name = "lblViewTY";
            this.lblViewTY.Size = new System.Drawing.Size(22, 18);
            this.lblViewTY.TabIndex = 212;
            this.lblViewTY.Text = "TY";
            // 
            // txtViewTY
            // 
            this.txtViewTY.Location = new System.Drawing.Point(110, 65);
            this.txtViewTY.Name = "txtViewTY";
            this.txtViewTY.Size = new System.Drawing.Size(54, 28);
            this.txtViewTY.TabIndex = 213;
            this.txtViewTY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtViewAny_KeyDown);
            this.txtViewTY.Leave += new System.EventHandler(this.txtViewTrans_Leave);
            // 
            // lblViewTZ
            // 
            this.lblViewTZ.Location = new System.Drawing.Point(170, 69);
            this.lblViewTZ.Name = "lblViewTZ";
            this.lblViewTZ.Size = new System.Drawing.Size(22, 18);
            this.lblViewTZ.TabIndex = 214;
            this.lblViewTZ.Text = "TZ";
            // 
            // txtViewTZ
            // 
            this.txtViewTZ.Location = new System.Drawing.Point(192, 65);
            this.txtViewTZ.Name = "txtViewTZ";
            this.txtViewTZ.Size = new System.Drawing.Size(54, 28);
            this.txtViewTZ.TabIndex = 215;
            this.txtViewTZ.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtViewAny_KeyDown);
            this.txtViewTZ.Leave += new System.EventHandler(this.txtViewTrans_Leave);
            // 
            // lblViewRX
            // 
            this.lblViewRX.Location = new System.Drawing.Point(264, 69);
            this.lblViewRX.Name = "lblViewRX";
            this.lblViewRX.Size = new System.Drawing.Size(22, 18);
            this.lblViewRX.TabIndex = 216;
            this.lblViewRX.Text = "RX";
            // 
            // txtViewRX
            // 
            this.txtViewRX.Location = new System.Drawing.Point(286, 65);
            this.txtViewRX.Name = "txtViewRX";
            this.txtViewRX.Size = new System.Drawing.Size(54, 28);
            this.txtViewRX.TabIndex = 217;
            this.txtViewRX.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtViewAny_KeyDown);
            this.txtViewRX.Leave += new System.EventHandler(this.txtViewRot_Leave);
            // 
            // lblViewRY
            // 
            this.lblViewRY.Location = new System.Drawing.Point(346, 69);
            this.lblViewRY.Name = "lblViewRY";
            this.lblViewRY.Size = new System.Drawing.Size(22, 18);
            this.lblViewRY.TabIndex = 218;
            this.lblViewRY.Text = "RY";
            // 
            // txtViewRY
            // 
            this.txtViewRY.Location = new System.Drawing.Point(368, 65);
            this.txtViewRY.Name = "txtViewRY";
            this.txtViewRY.Size = new System.Drawing.Size(54, 28);
            this.txtViewRY.TabIndex = 219;
            this.txtViewRY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtViewAny_KeyDown);
            this.txtViewRY.Leave += new System.EventHandler(this.txtViewRot_Leave);
            // 
            // lblViewRZ
            // 
            this.lblViewRZ.Location = new System.Drawing.Point(428, 69);
            this.lblViewRZ.Name = "lblViewRZ";
            this.lblViewRZ.Size = new System.Drawing.Size(22, 18);
            this.lblViewRZ.TabIndex = 220;
            this.lblViewRZ.Text = "RZ";
            // 
            // txtViewRZ
            // 
            this.txtViewRZ.Location = new System.Drawing.Point(450, 65);
            this.txtViewRZ.Name = "txtViewRZ";
            this.txtViewRZ.Size = new System.Drawing.Size(54, 28);
            this.txtViewRZ.TabIndex = 221;
            this.txtViewRZ.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtViewAny_KeyDown);
            this.txtViewRZ.Leave += new System.EventHandler(this.txtViewRot_Leave);
            // 
            // tmrViewSync
            // 
            this.tmrViewSync.Enabled = true;
            this.tmrViewSync.Interval = 150;
            this.tmrViewSync.Tick += new System.EventHandler(this.tmrViewSync_Tick);
            // 
            // btnExportUrdf
            // 
            this.btnExportUrdf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnExportUrdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportUrdf.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExportUrdf.ForeColor = System.Drawing.Color.White;
            this.btnExportUrdf.Location = new System.Drawing.Point(5, 612);
            this.btnExportUrdf.Margin = new System.Windows.Forms.Padding(4);
            this.btnExportUrdf.Name = "btnExportUrdf";
            this.btnExportUrdf.Size = new System.Drawing.Size(127, 41);
            this.btnExportUrdf.TabIndex = 2;
            this.btnExportUrdf.Text = "View URDF";
            this.btnExportUrdf.UseVisualStyleBackColor = false;
            this.btnExportUrdf.Click += new System.EventHandler(this.btnExportUrdf_Click);
            // 
            // btnPyBullet
            // 
            this.btnPyBullet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnPyBullet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPyBullet.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPyBullet.ForeColor = System.Drawing.Color.White;
            this.btnPyBullet.Location = new System.Drawing.Point(7, 655);
            this.btnPyBullet.Margin = new System.Windows.Forms.Padding(4);
            this.btnPyBullet.Name = "btnPyBullet";
            this.btnPyBullet.Size = new System.Drawing.Size(100, 41);
            this.btnPyBullet.TabIndex = 3;
            this.btnPyBullet.Text = "PyBullet";
            this.btnPyBullet.UseVisualStyleBackColor = false;
            this.btnPyBullet.Click += new System.EventHandler(this.btnPyBullet_Click);
            // 
            // btnMuJoCo
            // 
            this.btnMuJoCo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnMuJoCo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMuJoCo.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMuJoCo.ForeColor = System.Drawing.Color.White;
            this.btnMuJoCo.Location = new System.Drawing.Point(110, 655);
            this.btnMuJoCo.Margin = new System.Windows.Forms.Padding(4);
            this.btnMuJoCo.Name = "btnMuJoCo";
            this.btnMuJoCo.Size = new System.Drawing.Size(100, 41);
            this.btnMuJoCo.TabIndex = 4;
            this.btnMuJoCo.Text = "MuJoCo";
            this.btnMuJoCo.UseVisualStyleBackColor = false;
            this.btnMuJoCo.Click += new System.EventHandler(this.btnMuJoCo_Click);
            // 
            // btnIsaac
            // 
            this.btnIsaac.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnIsaac.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIsaac.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnIsaac.ForeColor = System.Drawing.Color.White;
            this.btnIsaac.Location = new System.Drawing.Point(214, 655);
            this.btnIsaac.Margin = new System.Windows.Forms.Padding(4);
            this.btnIsaac.Name = "btnIsaac";
            this.btnIsaac.Size = new System.Drawing.Size(112, 41);
            this.btnIsaac.TabIndex = 5;
            this.btnIsaac.Text = "Isaac Sim";
            this.btnIsaac.UseVisualStyleBackColor = false;
            this.btnIsaac.Click += new System.EventHandler(this.btnIsaac_Click);
            // 
            // txtIsaacPath
            // 
            this.txtIsaacPath.Location = new System.Drawing.Point(350, 763);
            this.txtIsaacPath.Name = "txtIsaacPath";
            this.txtIsaacPath.Size = new System.Drawing.Size(324, 28);
            this.txtIsaacPath.TabIndex = 52;
            this.txtIsaacPath.Text = "C:\\isaacsim\\python.bat";
            // 
            // lblIsaacPath
            // 
            this.lblIsaacPath.AutoSize = true;
            this.lblIsaacPath.ForeColor = System.Drawing.Color.Black;
            this.lblIsaacPath.Location = new System.Drawing.Point(194, 766);
            this.lblIsaacPath.Name = "lblIsaacPath";
            this.lblIsaacPath.Size = new System.Drawing.Size(150, 18);
            this.lblIsaacPath.TabIndex = 51;
            this.lblIsaacPath.Text = "Isaac python.bat:";
            // 
            // btnFormulaView
            // 
            this.btnFormulaView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(120)))), ((int)(((byte)(40)))));
            this.btnFormulaView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFormulaView.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFormulaView.ForeColor = System.Drawing.Color.White;
            this.btnFormulaView.Location = new System.Drawing.Point(281, 612);
            this.btnFormulaView.Margin = new System.Windows.Forms.Padding(4);
            this.btnFormulaView.Name = "btnFormulaView";
            this.btnFormulaView.Size = new System.Drawing.Size(85, 41);
            this.btnFormulaView.TabIndex = 40;
            this.btnFormulaView.Text = "Formula";
            this.btnFormulaView.UseVisualStyleBackColor = false;
            this.btnFormulaView.Click += new System.EventHandler(this.btnFormulaView_Click);
            // 
            // btnSendToSim
            // 
            this.btnSendToSim.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(160)))));
            this.btnSendToSim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendToSim.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSendToSim.ForeColor = System.Drawing.Color.White;
            this.btnSendToSim.Location = new System.Drawing.Point(368, 612);
            this.btnSendToSim.Margin = new System.Windows.Forms.Padding(4);
            this.btnSendToSim.Name = "btnSendToSim";
            this.btnSendToSim.Size = new System.Drawing.Size(118, 41);
            this.btnSendToSim.TabIndex = 41;
            this.btnSendToSim.Text = "3D → Sim";
            this.btnSendToSim.UseVisualStyleBackColor = false;
            this.btnSendToSim.Click += new System.EventHandler(this.btnSendToSim_Click);
            // 
            // chkAutoSync
            // 
            this.chkAutoSync.AutoSize = true;
            this.chkAutoSync.ForeColor = System.Drawing.Color.Black;
            this.chkAutoSync.Location = new System.Drawing.Point(491, 621);
            this.chkAutoSync.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutoSync.Name = "chkAutoSync";
            this.chkAutoSync.Size = new System.Drawing.Size(182, 22);
            this.chkAutoSync.TabIndex = 54;
            this.chkAutoSync.Text = "Auto-sync 100ms";
            this.chkAutoSync.UseVisualStyleBackColor = true;
            this.chkAutoSync.CheckedChanged += new System.EventHandler(this.chkAutoSync_CheckedChanged);
            // 
            // tmrAutoSync
            // 
            this.tmrAutoSync.Tick += new System.EventHandler(this.tmrAutoSync_Tick);
            // 
            // btnCaptureAll
            // 
            this.btnCaptureAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(120)))), ((int)(((byte)(170)))));
            this.btnCaptureAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCaptureAll.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCaptureAll.ForeColor = System.Drawing.Color.White;
            this.btnCaptureAll.Location = new System.Drawing.Point(849, 752);
            this.btnCaptureAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnCaptureAll.Name = "btnCaptureAll";
            this.btnCaptureAll.Size = new System.Drawing.Size(130, 41);
            this.btnCaptureAll.TabIndex = 53;
            this.btnCaptureAll.Text = "Capture All";
            this.btnCaptureAll.UseVisualStyleBackColor = false;
            this.btnCaptureAll.Click += new System.EventHandler(this.btnCaptureAll_Click);
            // 
            // lblSpawnHeight
            // 
            this.lblSpawnHeight.AutoSize = true;
            this.lblSpawnHeight.ForeColor = System.Drawing.Color.Black;
            this.lblSpawnHeight.Location = new System.Drawing.Point(784, 713);
            this.lblSpawnHeight.Name = "lblSpawnHeight";
            this.lblSpawnHeight.Size = new System.Drawing.Size(119, 18);
            this.lblSpawnHeight.TabIndex = 54;
            this.lblSpawnHeight.Text = "Spawn Z (m):";
            // 
            // txtSpawnHeight
            // 
            this.txtSpawnHeight.Location = new System.Drawing.Point(909, 707);
            this.txtSpawnHeight.Name = "txtSpawnHeight";
            this.txtSpawnHeight.Size = new System.Drawing.Size(70, 28);
            this.txtSpawnHeight.TabIndex = 55;
            this.txtSpawnHeight.Text = "0.4";
            // 
            // chkFixedBase
            // 
            this.chkFixedBase.AutoSize = true;
            this.chkFixedBase.Checked = true;
            this.chkFixedBase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFixedBase.ForeColor = System.Drawing.Color.Black;
            this.chkFixedBase.Location = new System.Drawing.Point(9, 711);
            this.chkFixedBase.Margin = new System.Windows.Forms.Padding(4);
            this.chkFixedBase.Name = "chkFixedBase";
            this.chkFixedBase.Size = new System.Drawing.Size(123, 22);
            this.chkFixedBase.TabIndex = 50;
            this.chkFixedBase.Text = "Fixed Base";
            this.chkFixedBase.UseVisualStyleBackColor = true;
            // 
            // chkExportScene
            // 
            this.chkExportScene.AutoSize = true;
            this.chkExportScene.Checked = true;
            this.chkExportScene.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExportScene.ForeColor = System.Drawing.Color.Black;
            this.chkExportScene.Location = new System.Drawing.Point(9, 761);
            this.chkExportScene.Margin = new System.Windows.Forms.Padding(4);
            this.chkExportScene.Name = "chkExportScene";
            this.chkExportScene.Size = new System.Drawing.Size(154, 22);
            this.chkExportScene.TabIndex = 53;
            this.chkExportScene.Text = "Export $scene";
            this.chkExportScene.UseVisualStyleBackColor = true;
            // 
            // chkShowAxis
            // 
            this.chkShowAxis.AutoSize = true;
            this.chkShowAxis.Checked = true;
            this.chkShowAxis.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowAxis.ForeColor = System.Drawing.Color.Black;
            this.chkShowAxis.Location = new System.Drawing.Point(9, 736);
            this.chkShowAxis.Margin = new System.Windows.Forms.Padding(4);
            this.chkShowAxis.Name = "chkShowAxis";
            this.chkShowAxis.Size = new System.Drawing.Size(121, 22);
            this.chkShowAxis.TabIndex = 51;
            this.chkShowAxis.Text = "Show Axis";
            this.chkShowAxis.UseVisualStyleBackColor = true;
            this.chkShowAxis.CheckedChanged += new System.EventHandler(this.chkShowAxis_CheckedChanged);
            // 
            // lblUrdfFolder
            // 
            this.lblUrdfFolder.ForeColor = System.Drawing.Color.Black;
            this.lblUrdfFolder.Location = new System.Drawing.Point(194, 737);
            this.lblUrdfFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUrdfFolder.Name = "lblUrdfFolder";
            this.lblUrdfFolder.Size = new System.Drawing.Size(119, 20);
            this.lblUrdfFolder.TabIndex = 60;
            this.lblUrdfFolder.Text = "Output:";
            // 
            // txtUrdfFolder
            // 
            this.txtUrdfFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.txtUrdfFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUrdfFolder.ForeColor = System.Drawing.Color.White;
            this.txtUrdfFolder.Location = new System.Drawing.Point(267, 733);
            this.txtUrdfFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtUrdfFolder.Name = "txtUrdfFolder";
            this.txtUrdfFolder.Size = new System.Drawing.Size(59, 28);
            this.txtUrdfFolder.TabIndex = 53;
            this.txtUrdfFolder.Text = "urdf";
            // 
            // lblStlFolder
            // 
            this.lblStlFolder.ForeColor = System.Drawing.Color.Black;
            this.lblStlFolder.Location = new System.Drawing.Point(194, 706);
            this.lblStlFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStlFolder.Name = "lblStlFolder";
            this.lblStlFolder.Size = new System.Drawing.Size(122, 20);
            this.lblStlFolder.TabIndex = 61;
            this.lblStlFolder.Text = "STL Dir:";
            // 
            // txtStlFolder
            // 
            this.txtStlFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.txtStlFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStlFolder.ForeColor = System.Drawing.Color.White;
            this.txtStlFolder.Location = new System.Drawing.Point(267, 703);
            this.txtStlFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtStlFolder.Name = "txtStlFolder";
            this.txtStlFolder.Size = new System.Drawing.Size(59, 28);
            this.txtStlFolder.TabIndex = 55;
            this.txtStlFolder.Text = "stl";
            // 
            // btnExportConfig
            // 
            this.btnExportConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(100)))), ((int)(((byte)(50)))));
            this.btnExportConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportConfig.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExportConfig.ForeColor = System.Drawing.Color.White;
            this.btnExportConfig.Location = new System.Drawing.Point(1397, 80);
            this.btnExportConfig.Margin = new System.Windows.Forms.Padding(4);
            this.btnExportConfig.Name = "btnExportConfig";
            this.btnExportConfig.Size = new System.Drawing.Size(144, 43);
            this.btnExportConfig.TabIndex = 46;
            this.btnExportConfig.Text = "Export Config";
            this.btnExportConfig.UseVisualStyleBackColor = false;
            this.btnExportConfig.Click += new System.EventHandler(this.btnExportConfig_Click);
            // 
            // lblWristBoundary
            // 
            this.lblWristBoundary.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblWristBoundary.Location = new System.Drawing.Point(1387, 128);
            this.lblWristBoundary.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWristBoundary.Name = "lblWristBoundary";
            this.lblWristBoundary.Size = new System.Drawing.Size(66, 29);
            this.lblWristBoundary.TabIndex = 47;
            this.lblWristBoundary.Text = "Wrist:";
            this.lblWristBoundary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbWristMotor
            // 
            this.cmbWristMotor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWristMotor.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.cmbWristMotor.Items.AddRange(new object[] {
            "None (Full Arm)"});
            this.cmbWristMotor.Location = new System.Drawing.Point(1451, 126);
            this.cmbWristMotor.Margin = new System.Windows.Forms.Padding(4);
            this.cmbWristMotor.Name = "cmbWristMotor";
            this.cmbWristMotor.Size = new System.Drawing.Size(90, 33);
            this.cmbWristMotor.TabIndex = 48;
            // 
            // lblWristHint
            // 
            this.lblWristHint.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblWristHint.ForeColor = System.Drawing.Color.Gray;
            this.lblWristHint.Location = new System.Drawing.Point(1386, 155);
            this.lblWristHint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWristHint.Name = "lblWristHint";
            this.lblWristHint.Size = new System.Drawing.Size(115, 18);
            this.lblWristHint.TabIndex = 49;
            this.lblWristHint.Text = "← wrist boundary";
            this.lblWristHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGripper
            // 
            this.lblGripper.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblGripper.Location = new System.Drawing.Point(1383, 193);
            this.lblGripper.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGripper.Name = "lblGripper";
            this.lblGripper.Size = new System.Drawing.Size(83, 31);
            this.lblGripper.TabIndex = 50;
            this.lblGripper.Text = "Gripper:";
            this.lblGripper.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbGripperMotor
            // 
            this.cmbGripperMotor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGripperMotor.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.cmbGripperMotor.Items.AddRange(new object[] {
            "None"});
            this.cmbGripperMotor.Location = new System.Drawing.Point(1463, 191);
            this.cmbGripperMotor.Margin = new System.Windows.Forms.Padding(4);
            this.cmbGripperMotor.Name = "cmbGripperMotor";
            this.cmbGripperMotor.Size = new System.Drawing.Size(78, 33);
            this.cmbGripperMotor.TabIndex = 51;
            // 
            // lblGripperHint
            // 
            this.lblGripperHint.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblGripperHint.ForeColor = System.Drawing.Color.Gray;
            this.lblGripperHint.Location = new System.Drawing.Point(1387, 228);
            this.lblGripperHint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGripperHint.Name = "lblGripperHint";
            this.lblGripperHint.Size = new System.Drawing.Size(115, 18);
            this.lblGripperHint.TabIndex = 52;
            this.lblGripperHint.Text = "← gripper joint";
            this.lblGripperHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(0, 0);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(4);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(0, 28);
            this.txtMessage.TabIndex = 5;
            this.txtMessage.Visible = false;
            // 
            // lblDhTitle
            // 
            this.lblDhTitle.Location = new System.Drawing.Point(1083, 13);
            this.lblDhTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDhTitle.Name = "lblDhTitle";
            this.lblDhTitle.Size = new System.Drawing.Size(300, 18);
            this.lblDhTitle.TabIndex = 35;
            this.lblDhTitle.Text = "─ DH Parameters[a,d,Θ,α] ─";
            this.lblDhTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDhParams
            // 
            this.txtDhParams.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDhParams.HideSelection = false;
            this.txtDhParams.Location = new System.Drawing.Point(1083, 32);
            this.txtDhParams.Margin = new System.Windows.Forms.Padding(4);
            this.txtDhParams.Name = "txtDhParams";
            this.txtDhParams.Size = new System.Drawing.Size(300, 478);
            this.txtDhParams.TabIndex = 36;
            this.txtDhParams.Text = "";
            this.txtDhParams.WordWrap = false;
            // 
            // btnApplyDh
            // 
            this.btnApplyDh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(160)))), ((int)(((byte)(70)))));
            this.btnApplyDh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyDh.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnApplyDh.ForeColor = System.Drawing.Color.White;
            this.btnApplyDh.Location = new System.Drawing.Point(1397, 32);
            this.btnApplyDh.Margin = new System.Windows.Forms.Padding(4);
            this.btnApplyDh.Name = "btnApplyDh";
            this.btnApplyDh.Size = new System.Drawing.Size(144, 40);
            this.btnApplyDh.TabIndex = 37;
            this.btnApplyDh.Text = "Apply DH";
            this.btnApplyDh.UseVisualStyleBackColor = false;
            this.btnApplyDh.Click += new System.EventHandler(this.btnApplyDh_Click);
            // 
            // lblViewMode
            // 
            this.lblViewMode.Location = new System.Drawing.Point(796, 661);
            this.lblViewMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblViewMode.Name = "lblViewMode";
            this.lblViewMode.Size = new System.Drawing.Size(74, 20);
            this.lblViewMode.TabIndex = 38;
            this.lblViewMode.Text = "View:";
            this.lblViewMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbViewMode
            // 
            this.cmbViewMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbViewMode.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbViewMode.Items.AddRange(new object[] {
            "All",
            "Skeleton",
            "STL"});
            this.cmbViewMode.Location = new System.Drawing.Point(872, 653);
            this.cmbViewMode.Margin = new System.Windows.Forms.Padding(4);
            this.cmbViewMode.Name = "cmbViewMode";
            this.cmbViewMode.Size = new System.Drawing.Size(107, 33);
            this.cmbViewMode.TabIndex = 39;
            this.cmbViewMode.SelectedIndexChanged += new System.EventHandler(this.cmbViewMode_SelectedIndexChanged);
            // 
            // lblSagTwin
            // 
            this.lblSagTwin.Location = new System.Drawing.Point(8, 24);
            this.lblSagTwin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSagTwin.Name = "lblSagTwin";
            this.lblSagTwin.Size = new System.Drawing.Size(90, 20);
            this.lblSagTwin.TabIndex = 0;
            this.lblSagTwin.Text = "처짐 쌍둥이:";
            this.lblSagTwin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbSagTwin
            // 
            this.cmbSagTwin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSagTwin.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbSagTwin.Items.AddRange(new object[] {
            "끔",
            "3D",
            "3D+시뮬 겹쳐",
            "3D+시뮬 실물"});
            this.cmbSagTwin.Location = new System.Drawing.Point(100, 18);
            this.cmbSagTwin.Margin = new System.Windows.Forms.Padding(4);
            this.cmbSagTwin.Name = "cmbSagTwin";
            this.cmbSagTwin.Size = new System.Drawing.Size(150, 33);
            this.cmbSagTwin.TabIndex = 1;
            this.cmbSagTwin.SelectedIndexChanged += new System.EventHandler(this.cmbSagTwin_SelectedIndexChanged);
            // 
            // chkSagCompOn
            // 
            this.chkSagCompOn.AutoSize = true;
            this.chkSagCompOn.ForeColor = System.Drawing.Color.Black;
            this.chkSagCompOn.Location = new System.Drawing.Point(262, 22);
            this.chkSagCompOn.Margin = new System.Windows.Forms.Padding(4);
            this.chkSagCompOn.Name = "chkSagCompOn";
            this.chkSagCompOn.Size = new System.Drawing.Size(170, 22);
            this.chkSagCompOn.TabIndex = 2;
            this.chkSagCompOn.Text = "처짐 보정 켬";
            this.chkSagCompOn.UseVisualStyleBackColor = true;
            this.chkSagCompOn.CheckedChanged += new System.EventHandler(this.chkSagCompOn_CheckedChanged);
            // 
            // lblSkeletonSize
            // 
            this.lblSkeletonSize.Location = new System.Drawing.Point(635, 701);
            this.lblSkeletonSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSkeletonSize.Name = "lblSkeletonSize";
            this.lblSkeletonSize.Size = new System.Drawing.Size(91, 20);
            this.lblSkeletonSize.TabIndex = 40;
            this.lblSkeletonSize.Text = "Bar Size:";
            this.lblSkeletonSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSkeletonSize
            // 
            this.txtSkeletonSize.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSkeletonSize.Location = new System.Drawing.Point(724, 696);
            this.txtSkeletonSize.Margin = new System.Windows.Forms.Padding(4);
            this.txtSkeletonSize.Name = "txtSkeletonSize";
            this.txtSkeletonSize.Size = new System.Drawing.Size(38, 31);
            this.txtSkeletonSize.TabIndex = 41;
            this.txtSkeletonSize.Text = "10";
            this.txtSkeletonSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSkeletonSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSkeletonSize_KeyDown);
            // 
            // lblAxisSize
            // 
            this.lblAxisSize.Location = new System.Drawing.Point(628, 729);
            this.lblAxisSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAxisSize.Name = "lblAxisSize";
            this.lblAxisSize.Size = new System.Drawing.Size(98, 20);
            this.lblAxisSize.TabIndex = 43;
            this.lblAxisSize.Text = "Axis Size:";
            this.lblAxisSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAxisSize
            // 
            this.txtAxisSize.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtAxisSize.Location = new System.Drawing.Point(724, 724);
            this.txtAxisSize.Margin = new System.Windows.Forms.Padding(4);
            this.txtAxisSize.Name = "txtAxisSize";
            this.txtAxisSize.Size = new System.Drawing.Size(38, 31);
            this.txtAxisSize.TabIndex = 44;
            this.txtAxisSize.Text = "20";
            this.txtAxisSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAxisSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAxisSize_KeyDown);
            // 
            // cmbExample
            // 
            this.cmbExample.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExample.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbExample.Items.AddRange(new object[] {
            "Select Example...",
            "1: Simple Robot",
            "2: Humanoid (Skeleton)",
            "3: 6-Axis + STL",
            "4: OMX Leader",
            "5: OMX Follower (Joints Only)",
            "6: OMX Follower (+Camera)",
            "7: Humanoid (Full)",
            "8: 2-Wheel Mobile",
            "9: 4-Wheel Mobile",
            "10: Cartesian (Desktop)",
            "11: [Lerobot_Ojw]Leader",
            "12: Tank + 6-Axis Arm",
            "13: XYZ Desktop Gantry",
            "14: Linear Leadscrew",
            "15: Delta Robot (Parallel)",
            "16: Delta Biped (Walking)"});
            this.cmbExample.Location = new System.Drawing.Point(769, 616);
            this.cmbExample.Margin = new System.Windows.Forms.Padding(4);
            this.cmbExample.Name = "cmbExample";
            this.cmbExample.Size = new System.Drawing.Size(210, 33);
            this.cmbExample.TabIndex = 45;
            this.cmbExample.SelectedIndexChanged += new System.EventHandler(this.cmbExample_SelectedIndexChanged);
            // 
            // pnDhEdit
            // 
            this.pnDhEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.pnDhEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnDhEdit.Controls.Add(this.lblDhEditTitle);
            this.pnDhEdit.Controls.Add(this.lblEditA);
            this.pnDhEdit.Controls.Add(this.txtEditA);
            this.pnDhEdit.Controls.Add(this.lblEditD);
            this.pnDhEdit.Controls.Add(this.txtEditD);
            this.pnDhEdit.Controls.Add(this.lblEditTheta);
            this.pnDhEdit.Controls.Add(this.txtEditTheta);
            this.pnDhEdit.Controls.Add(this.lblEditAlpha);
            this.pnDhEdit.Controls.Add(this.txtEditAlpha);
            this.pnDhEdit.Controls.Add(this.lblEditAxis);
            this.pnDhEdit.Controls.Add(this.txtEditAxis);
            this.pnDhEdit.Controls.Add(this.lblEditDir);
            this.pnDhEdit.Controls.Add(this.txtEditDir);
            this.pnDhEdit.Controls.Add(this.lblEditInit);
            this.pnDhEdit.Controls.Add(this.txtEditInit);
            this.pnDhEdit.Controls.Add(this.lblEditStlFile);
            this.pnDhEdit.Controls.Add(this.txtEditStlFile);
            this.pnDhEdit.Controls.Add(this.lblEditStlX);
            this.pnDhEdit.Controls.Add(this.txtEditStlX);
            this.pnDhEdit.Controls.Add(this.lblEditStlY);
            this.pnDhEdit.Controls.Add(this.txtEditStlY);
            this.pnDhEdit.Controls.Add(this.lblEditStlZ);
            this.pnDhEdit.Controls.Add(this.txtEditStlZ);
            this.pnDhEdit.Controls.Add(this.lblEditStlPan);
            this.pnDhEdit.Controls.Add(this.txtEditStlPan);
            this.pnDhEdit.Controls.Add(this.lblEditStlTilt);
            this.pnDhEdit.Controls.Add(this.txtEditStlTilt);
            this.pnDhEdit.Controls.Add(this.lblEditStlSwing);
            this.pnDhEdit.Controls.Add(this.txtEditStlSwing);
            this.pnDhEdit.Controls.Add(this.btnEditApply);
            this.pnDhEdit.Location = new System.Drawing.Point(1083, 515);
            this.pnDhEdit.Name = "pnDhEdit";
            this.pnDhEdit.Size = new System.Drawing.Size(300, 206);
            this.pnDhEdit.TabIndex = 60;
            this.pnDhEdit.Visible = false;
            // 
            // lblDhEditTitle
            // 
            this.lblDhEditTitle.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.lblDhEditTitle.Location = new System.Drawing.Point(2, 2);
            this.lblDhEditTitle.Name = "lblDhEditTitle";
            this.lblDhEditTitle.Size = new System.Drawing.Size(340, 16);
            this.lblDhEditTitle.TabIndex = 0;
            this.lblDhEditTitle.Text = "Edit";
            // 
            // lblEditA
            // 
            this.lblEditA.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblEditA.Location = new System.Drawing.Point(1, 30);
            this.lblEditA.Name = "lblEditA";
            this.lblEditA.Size = new System.Drawing.Size(16, 20);
            this.lblEditA.TabIndex = 1;
            this.lblEditA.Text = "A:";
            this.lblEditA.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEditA
            // 
            this.txtEditA.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.txtEditA.Location = new System.Drawing.Point(17, 30);
            this.txtEditA.Name = "txtEditA";
            this.txtEditA.Size = new System.Drawing.Size(55, 29);
            this.txtEditA.TabIndex = 2;
            this.txtEditA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblEditD
            // 
            this.lblEditD.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblEditD.Location = new System.Drawing.Point(74, 30);
            this.lblEditD.Name = "lblEditD";
            this.lblEditD.Size = new System.Drawing.Size(16, 20);
            this.lblEditD.TabIndex = 3;
            this.lblEditD.Text = "D:";
            this.lblEditD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEditD
            // 
            this.txtEditD.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.txtEditD.Location = new System.Drawing.Point(90, 30);
            this.txtEditD.Name = "txtEditD";
            this.txtEditD.Size = new System.Drawing.Size(55, 29);
            this.txtEditD.TabIndex = 4;
            this.txtEditD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblEditTheta
            // 
            this.lblEditTheta.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblEditTheta.Location = new System.Drawing.Point(147, 30);
            this.lblEditTheta.Name = "lblEditTheta";
            this.lblEditTheta.Size = new System.Drawing.Size(20, 20);
            this.lblEditTheta.TabIndex = 5;
            this.lblEditTheta.Text = "Th:";
            this.lblEditTheta.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEditTheta
            // 
            this.txtEditTheta.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.txtEditTheta.Location = new System.Drawing.Point(167, 30);
            this.txtEditTheta.Name = "txtEditTheta";
            this.txtEditTheta.Size = new System.Drawing.Size(55, 29);
            this.txtEditTheta.TabIndex = 6;
            this.txtEditTheta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblEditAlpha
            // 
            this.lblEditAlpha.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblEditAlpha.Location = new System.Drawing.Point(224, 30);
            this.lblEditAlpha.Name = "lblEditAlpha";
            this.lblEditAlpha.Size = new System.Drawing.Size(20, 20);
            this.lblEditAlpha.TabIndex = 7;
            this.lblEditAlpha.Text = "Al:";
            this.lblEditAlpha.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEditAlpha
            // 
            this.txtEditAlpha.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.txtEditAlpha.Location = new System.Drawing.Point(244, 30);
            this.txtEditAlpha.Name = "txtEditAlpha";
            this.txtEditAlpha.Size = new System.Drawing.Size(49, 29);
            this.txtEditAlpha.TabIndex = 8;
            this.txtEditAlpha.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblEditAxis
            // 
            this.lblEditAxis.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblEditAxis.Location = new System.Drawing.Point(1, 91);
            this.lblEditAxis.Name = "lblEditAxis";
            this.lblEditAxis.Size = new System.Drawing.Size(20, 20);
            this.lblEditAxis.TabIndex = 9;
            this.lblEditAxis.Text = "Ax:";
            this.lblEditAxis.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEditAxis
            // 
            this.txtEditAxis.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.txtEditAxis.Location = new System.Drawing.Point(21, 91);
            this.txtEditAxis.Name = "txtEditAxis";
            this.txtEditAxis.Size = new System.Drawing.Size(40, 29);
            this.txtEditAxis.TabIndex = 10;
            this.txtEditAxis.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblEditDir
            // 
            this.lblEditDir.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblEditDir.Location = new System.Drawing.Point(63, 91);
            this.lblEditDir.Name = "lblEditDir";
            this.lblEditDir.Size = new System.Drawing.Size(24, 20);
            this.lblEditDir.TabIndex = 11;
            this.lblEditDir.Text = "Dir:";
            this.lblEditDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEditDir
            // 
            this.txtEditDir.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.txtEditDir.Location = new System.Drawing.Point(87, 91);
            this.txtEditDir.Name = "txtEditDir";
            this.txtEditDir.Size = new System.Drawing.Size(30, 29);
            this.txtEditDir.TabIndex = 12;
            this.txtEditDir.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblEditInit
            // 
            this.lblEditInit.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblEditInit.Location = new System.Drawing.Point(119, 91);
            this.lblEditInit.Name = "lblEditInit";
            this.lblEditInit.Size = new System.Drawing.Size(28, 20);
            this.lblEditInit.TabIndex = 13;
            this.lblEditInit.Text = "Init:";
            this.lblEditInit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEditInit
            // 
            this.txtEditInit.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.txtEditInit.Location = new System.Drawing.Point(147, 91);
            this.txtEditInit.Name = "txtEditInit";
            this.txtEditInit.Size = new System.Drawing.Size(30, 29);
            this.txtEditInit.TabIndex = 14;
            this.txtEditInit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblEditStlFile
            // 
            this.lblEditStlFile.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblEditStlFile.Location = new System.Drawing.Point(1, 30);
            this.lblEditStlFile.Name = "lblEditStlFile";
            this.lblEditStlFile.Size = new System.Drawing.Size(28, 20);
            this.lblEditStlFile.TabIndex = 15;
            this.lblEditStlFile.Text = "File:";
            this.lblEditStlFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblEditStlFile.Visible = false;
            // 
            // txtEditStlFile
            // 
            this.txtEditStlFile.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.txtEditStlFile.Location = new System.Drawing.Point(17, 60);
            this.txtEditStlFile.Name = "txtEditStlFile";
            this.txtEditStlFile.Size = new System.Drawing.Size(276, 29);
            this.txtEditStlFile.TabIndex = 16;
            this.txtEditStlFile.Visible = false;
            // 
            // lblEditStlX
            // 
            this.lblEditStlX.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblEditStlX.Location = new System.Drawing.Point(7, 131);
            this.lblEditStlX.Name = "lblEditStlX";
            this.lblEditStlX.Size = new System.Drawing.Size(14, 20);
            this.lblEditStlX.TabIndex = 17;
            this.lblEditStlX.Text = "X:";
            this.lblEditStlX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblEditStlX.Visible = false;
            // 
            // txtEditStlX
            // 
            this.txtEditStlX.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.txtEditStlX.Location = new System.Drawing.Point(21, 131);
            this.txtEditStlX.Name = "txtEditStlX";
            this.txtEditStlX.Size = new System.Drawing.Size(40, 29);
            this.txtEditStlX.TabIndex = 18;
            this.txtEditStlX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtEditStlX.Visible = false;
            // 
            // lblEditStlY
            // 
            this.lblEditStlY.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblEditStlY.Location = new System.Drawing.Point(63, 131);
            this.lblEditStlY.Name = "lblEditStlY";
            this.lblEditStlY.Size = new System.Drawing.Size(14, 20);
            this.lblEditStlY.TabIndex = 19;
            this.lblEditStlY.Text = "Y:";
            this.lblEditStlY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblEditStlY.Visible = false;
            // 
            // txtEditStlY
            // 
            this.txtEditStlY.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.txtEditStlY.Location = new System.Drawing.Point(77, 131);
            this.txtEditStlY.Name = "txtEditStlY";
            this.txtEditStlY.Size = new System.Drawing.Size(40, 29);
            this.txtEditStlY.TabIndex = 20;
            this.txtEditStlY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtEditStlY.Visible = false;
            // 
            // lblEditStlZ
            // 
            this.lblEditStlZ.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblEditStlZ.Location = new System.Drawing.Point(119, 131);
            this.lblEditStlZ.Name = "lblEditStlZ";
            this.lblEditStlZ.Size = new System.Drawing.Size(14, 20);
            this.lblEditStlZ.TabIndex = 21;
            this.lblEditStlZ.Text = "Z:";
            this.lblEditStlZ.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblEditStlZ.Visible = false;
            // 
            // txtEditStlZ
            // 
            this.txtEditStlZ.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.txtEditStlZ.Location = new System.Drawing.Point(133, 131);
            this.txtEditStlZ.Name = "txtEditStlZ";
            this.txtEditStlZ.Size = new System.Drawing.Size(40, 29);
            this.txtEditStlZ.TabIndex = 22;
            this.txtEditStlZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtEditStlZ.Visible = false;
            // 
            // lblEditStlPan
            // 
            this.lblEditStlPan.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblEditStlPan.Location = new System.Drawing.Point(12, 170);
            this.lblEditStlPan.Name = "lblEditStlPan";
            this.lblEditStlPan.Size = new System.Drawing.Size(14, 20);
            this.lblEditStlPan.TabIndex = 23;
            this.lblEditStlPan.Text = "P:";
            this.lblEditStlPan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblEditStlPan.Visible = false;
            // 
            // txtEditStlPan
            // 
            this.txtEditStlPan.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.txtEditStlPan.Location = new System.Drawing.Point(26, 170);
            this.txtEditStlPan.Name = "txtEditStlPan";
            this.txtEditStlPan.Size = new System.Drawing.Size(35, 29);
            this.txtEditStlPan.TabIndex = 24;
            this.txtEditStlPan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtEditStlPan.Visible = false;
            // 
            // lblEditStlTilt
            // 
            this.lblEditStlTilt.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblEditStlTilt.Location = new System.Drawing.Point(63, 170);
            this.lblEditStlTilt.Name = "lblEditStlTilt";
            this.lblEditStlTilt.Size = new System.Drawing.Size(14, 20);
            this.lblEditStlTilt.TabIndex = 25;
            this.lblEditStlTilt.Text = "T:";
            this.lblEditStlTilt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblEditStlTilt.Visible = false;
            // 
            // txtEditStlTilt
            // 
            this.txtEditStlTilt.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.txtEditStlTilt.Location = new System.Drawing.Point(77, 170);
            this.txtEditStlTilt.Name = "txtEditStlTilt";
            this.txtEditStlTilt.Size = new System.Drawing.Size(35, 29);
            this.txtEditStlTilt.TabIndex = 26;
            this.txtEditStlTilt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtEditStlTilt.Visible = false;
            // 
            // lblEditStlSwing
            // 
            this.lblEditStlSwing.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblEditStlSwing.Location = new System.Drawing.Point(114, 170);
            this.lblEditStlSwing.Name = "lblEditStlSwing";
            this.lblEditStlSwing.Size = new System.Drawing.Size(14, 20);
            this.lblEditStlSwing.TabIndex = 27;
            this.lblEditStlSwing.Text = "S:";
            this.lblEditStlSwing.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblEditStlSwing.Visible = false;
            // 
            // txtEditStlSwing
            // 
            this.txtEditStlSwing.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.txtEditStlSwing.Location = new System.Drawing.Point(128, 170);
            this.txtEditStlSwing.Name = "txtEditStlSwing";
            this.txtEditStlSwing.Size = new System.Drawing.Size(35, 29);
            this.txtEditStlSwing.TabIndex = 28;
            this.txtEditStlSwing.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtEditStlSwing.Visible = false;
            // 
            // btnEditApply
            // 
            this.btnEditApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnEditApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditApply.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.btnEditApply.ForeColor = System.Drawing.Color.White;
            this.btnEditApply.Location = new System.Drawing.Point(212, 154);
            this.btnEditApply.Name = "btnEditApply";
            this.btnEditApply.Size = new System.Drawing.Size(83, 36);
            this.btnEditApply.TabIndex = 29;
            this.btnEditApply.Text = "Apply";
            this.btnEditApply.UseVisualStyleBackColor = false;
            this.btnEditApply.Click += new System.EventHandler(this.btnEditApply_Click);
            // 
            // chkShowArrow
            // 
            this.chkShowArrow.AutoSize = true;
            this.chkShowArrow.Checked = true;
            this.chkShowArrow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowArrow.Location = new System.Drawing.Point(247, 5);
            this.chkShowArrow.Name = "chkShowArrow";
            this.chkShowArrow.Size = new System.Drawing.Size(83, 22);
            this.chkShowArrow.TabIndex = 62;
            this.chkShowArrow.Text = "Arrow";
            this.chkShowArrow.UseVisualStyleBackColor = true;
            this.chkShowArrow.CheckedChanged += new System.EventHandler(this.chkShowArrow_CheckedChanged);
            // 
            // txtArrowSize
            // 
            this.txtArrowSize.Location = new System.Drawing.Point(333, 3);
            this.txtArrowSize.Name = "txtArrowSize";
            this.txtArrowSize.Size = new System.Drawing.Size(50, 28);
            this.txtArrowSize.TabIndex = 63;
            this.txtArrowSize.Text = "25";
            this.txtArrowSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtArrowSize_KeyDown);
            // 
            // chkShowBall
            // 
            this.chkShowBall.AutoSize = true;
            this.chkShowBall.Checked = true;
            this.chkShowBall.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowBall.Location = new System.Drawing.Point(391, 5);
            this.chkShowBall.Name = "chkShowBall";
            this.chkShowBall.Size = new System.Drawing.Size(61, 22);
            this.chkShowBall.TabIndex = 64;
            this.chkShowBall.Text = "Ball";
            this.chkShowBall.UseVisualStyleBackColor = true;
            this.chkShowBall.CheckedChanged += new System.EventHandler(this.chkShowBall_CheckedChanged);
            // 
            // txtBallSize
            // 
            this.txtBallSize.Location = new System.Drawing.Point(458, 5);
            this.txtBallSize.Name = "txtBallSize";
            this.txtBallSize.Size = new System.Drawing.Size(50, 28);
            this.txtBallSize.TabIndex = 65;
            this.txtBallSize.Text = "15";
            this.txtBallSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBallSize_KeyDown);
            // 
            // chkShowSphere
            // 
            this.chkShowSphere.AutoSize = true;
            this.chkShowSphere.Checked = true;
            this.chkShowSphere.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowSphere.Location = new System.Drawing.Point(247, 33);
            this.chkShowSphere.Name = "chkShowSphere";
            this.chkShowSphere.Size = new System.Drawing.Size(91, 22);
            this.chkShowSphere.TabIndex = 66;
            this.chkShowSphere.Text = "Sphere";
            this.chkShowSphere.UseVisualStyleBackColor = true;
            this.chkShowSphere.CheckedChanged += new System.EventHandler(this.chkShowSphere_CheckedChanged);
            // 
            // txtSphereSize
            // 
            this.txtSphereSize.Location = new System.Drawing.Point(333, 31);
            this.txtSphereSize.Name = "txtSphereSize";
            this.txtSphereSize.Size = new System.Drawing.Size(50, 28);
            this.txtSphereSize.TabIndex = 67;
            this.txtSphereSize.Text = "3";
            this.txtSphereSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSphereSize_KeyDown);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(678, 622);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 20);
            this.label2.TabIndex = 40;
            this.label2.Text = "Examples:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(305, 301);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 24);
            this.label1.TabIndex = 225;
            this.label1.Text = "──── STL ────";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1545, 797);
            this.Controls.Add(this.btnUsdExport);
            this.Controls.Add(this.btnSignCalib);
            this.Controls.Add(this.btnSignClear);
            this.Controls.Add(this.btnMechLimit);
            this.Controls.Add(this.btnSim2Real);
            this.Controls.Add(this.btnStandDroop);
            this.Controls.Add(this.btnS2RGuide);
            this.Controls.Add(this.btnIsaacCam);
            this.Controls.Add(this.btnSagComp);
            this.Controls.Add(this.btnLoadVmax);
            this.Controls.Add(this.btnServoIdent);
            this.Controls.Add(this.btnTipBound);
            this.Controls.Add(this.btnPowerProf);
            this.Controls.Add(this.btnViewMjcf);
            this.Controls.Add(this.btnViewUsd);
            this.Controls.Add(this.btnIsaacUsd);
            this.Controls.Add(this.txtSphereSize);
            this.Controls.Add(this.chkShowArrow);
            this.Controls.Add(this.txtArrowSize);
            this.Controls.Add(this.chkShowBall);
            this.Controls.Add(this.txtBallSize);
            this.Controls.Add(this.chkShowSphere);
            this.Controls.Add(this.pnDisp);
            this.Controls.Add(this.btnView1);
            this.Controls.Add(this.btnView2);
            this.Controls.Add(this.btnView3);
            this.Controls.Add(this.btnView4);
            this.Controls.Add(this.btnView5);
            this.Controls.Add(this.btnView6);
            this.Controls.Add(this.btnView7);
            this.Controls.Add(this.btnView8);
            this.Controls.Add(this.btnViewDefault);
            this.Controls.Add(this.lblViewTX);
            this.Controls.Add(this.txtViewTX);
            this.Controls.Add(this.lblViewTY);
            this.Controls.Add(this.txtViewTY);
            this.Controls.Add(this.lblViewTZ);
            this.Controls.Add(this.txtViewTZ);
            this.Controls.Add(this.lblViewRX);
            this.Controls.Add(this.txtViewRX);
            this.Controls.Add(this.lblViewRY);
            this.Controls.Add(this.txtViewRY);
            this.Controls.Add(this.lblViewRZ);
            this.Controls.Add(this.txtViewRZ);
            this.Controls.Add(this.tabRight);
            this.Controls.Add(this.lblDhTitle);
            this.Controls.Add(this.txtDhParams);
            this.Controls.Add(this.lblWristBoundary);
            this.Controls.Add(this.cmbWristMotor);
            this.Controls.Add(this.lblWristHint);
            this.Controls.Add(this.cmbGripperMotor);
            this.Controls.Add(this.lblGripperHint);
            this.Controls.Add(this.btnApplyDh);
            this.Controls.Add(this.btnExportConfig);
            this.Controls.Add(this.btnExportUrdf);
            this.Controls.Add(this.btnPyBullet);
            this.Controls.Add(this.btnMuJoCo);
            this.Controls.Add(this.btnIsaac);
            this.Controls.Add(this.lblIsaacPath);
            this.Controls.Add(this.txtIsaacPath);
            this.Controls.Add(this.btnFormulaView);
            this.Controls.Add(this.btnSendToSim);
            this.Controls.Add(this.chkAutoSync);
            this.Controls.Add(this.btnCaptureAll);
            this.Controls.Add(this.lblSpawnHeight);
            this.Controls.Add(this.txtSpawnHeight);
            this.Controls.Add(this.chkFixedBase);
            this.Controls.Add(this.chkExportScene);
            this.Controls.Add(this.chkShowAxis);
            this.Controls.Add(this.txtUrdfFolder);
            this.Controls.Add(this.txtStlFolder);
            this.Controls.Add(this.cmbExample);
            this.Controls.Add(this.lblViewMode);
            this.Controls.Add(this.cmbViewMode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblSkeletonSize);
            this.Controls.Add(this.txtSkeletonSize);
            this.Controls.Add(this.lblAxisSize);
            this.Controls.Add(this.txtAxisSize);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.pnDhEdit);
            this.Controls.Add(this.lblUrdfFolder);
            this.Controls.Add(this.lblStlFolder);
            this.Controls.Add(this.lblGripper);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.Text = "MakeUrdf - Robot Designer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.tabRight.ResumeLayout(false);
            this.tabCreate.ResumeLayout(false);
            this.tabCreate.PerformLayout();
            this.pnDragArea.ResumeLayout(false);
            this.tabTest.ResumeLayout(false);
            this.tabTest.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkAngle)).EndInit();
            this.tabHistory.ResumeLayout(false);
            this.tabHistory.PerformLayout();
            this.tabPlayback.ResumeLayout(false);
            this.tabPlayback.PerformLayout();
            this.tabArmSetup.ResumeLayout(false);
            this.tabArmSetup.PerformLayout();
            this.grpArmConn.ResumeLayout(false);
            this.grpArmConn.PerformLayout();
            this.tabArmInner.ResumeLayout(false);
            this.tabArmInner.PerformLayout();
            this.tabArmPrac.ResumeLayout(false);
            this.tabArmPrac.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPracChart)).EndInit();
            this.tabArmMotor.ResumeLayout(false);
            this.tabArmMotor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArmMotor)).EndInit();
            this.tabArmDef.ResumeLayout(false);
            this.tabArmDef.PerformLayout();
            this.grpDefAdv.ResumeLayout(false);
            this.grpDefAdv.PerformLayout();
            this.tabArmExp.ResumeLayout(false);
            this.tabArmExp.PerformLayout();
            this.grpArmPlan.ResumeLayout(false);
            this.grpArmPlan.PerformLayout();
            this.grpArmMeasure.ResumeLayout(false);
            this.grpArmMeasure.PerformLayout();
            this.grpArmLearn.ResumeLayout(false);
            this.grpArmLearn.PerformLayout();
            this.grpArmTest.ResumeLayout(false);
            this.grpArmTest.PerformLayout();
            this.grpArmApply.ResumeLayout(false);
            this.grpArmApply.PerformLayout();
            this.grpArmTwin.ResumeLayout(false);
            this.grpArmTwin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWrist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picExternal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGraphQpos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGraphTgt)).EndInit();
            this.pnDhEdit.ResumeLayout(false);
            this.pnDhEdit.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnDisp;
        // === View control ===
        private System.Windows.Forms.Button btnUsdExport;
        private System.Windows.Forms.Button btnSignCalib;
        private System.Windows.Forms.Button btnSignClear;
        private System.Windows.Forms.Button btnMechLimit;
        private System.Windows.Forms.Button btnSim2Real;
        private System.Windows.Forms.Button btnStandDroop;
        private System.Windows.Forms.Button btnS2RGuide;
        private System.Windows.Forms.Button btnIsaacCam;
        private System.Windows.Forms.Button btnSagComp;
        private System.Windows.Forms.Button btnLoadVmax;
        private System.Windows.Forms.Button btnServoIdent;
        private System.Windows.Forms.Button btnTipBound;
        private System.Windows.Forms.Button btnPowerProf;
        private System.Windows.Forms.Button btnViewMjcf;
        private System.Windows.Forms.Button btnViewUsd;
        private System.Windows.Forms.Button btnIsaacUsd;
        private System.Windows.Forms.Button btnView1;
        private System.Windows.Forms.Button btnView2;
        private System.Windows.Forms.Button btnView3;
        private System.Windows.Forms.Button btnView4;
        private System.Windows.Forms.Button btnView5;
        private System.Windows.Forms.Button btnView6;
        private System.Windows.Forms.Button btnView7;
        private System.Windows.Forms.Button btnView8;
        private System.Windows.Forms.Button btnViewDefault;
        private System.Windows.Forms.Label lblViewTX;
        private System.Windows.Forms.TextBox txtViewTX;
        private System.Windows.Forms.Label lblViewTY;
        private System.Windows.Forms.TextBox txtViewTY;
        private System.Windows.Forms.Label lblViewTZ;
        private System.Windows.Forms.TextBox txtViewTZ;
        private System.Windows.Forms.Label lblViewRX;
        private System.Windows.Forms.TextBox txtViewRX;
        private System.Windows.Forms.Label lblViewRY;
        private System.Windows.Forms.TextBox txtViewRY;
        private System.Windows.Forms.Label lblViewRZ;
        private System.Windows.Forms.TextBox txtViewRZ;
        private System.Windows.Forms.Timer tmrViewSync;
        private System.Windows.Forms.Timer tmrDisp;
        private System.Windows.Forms.TabControl tabRight;
        private System.Windows.Forms.TabPage tabCreate;
        private System.Windows.Forms.TabPage tabTest;
        private System.Windows.Forms.RadioButton rdModeMouse;
        private System.Windows.Forms.RadioButton rdModeButton;
        private System.Windows.Forms.RadioButton rdModeHybrid;
        private System.Windows.Forms.Label lblMoveInput;
        private System.Windows.Forms.TextBox txtMoveValue;
        private System.Windows.Forms.Label lblRotateInput;
        private System.Windows.Forms.TextBox txtRotateValue;
        private System.Windows.Forms.Panel pnDragArea;
        private System.Windows.Forms.Label lblDragArea;
        private System.Windows.Forms.Label lblMoveGroup;
        private System.Windows.Forms.TextBox txtStateXMove;
        private System.Windows.Forms.TextBox txtStateZMove;
        private System.Windows.Forms.TextBox txtStateYMove;
        private System.Windows.Forms.TextBox txtStateXRotate;
        private System.Windows.Forms.TextBox txtStateZRotate;
        private System.Windows.Forms.TextBox txtStateYRotate;
        private System.Windows.Forms.Label lblStateJoint;
        private System.Windows.Forms.TextBox txtStateJoint;
        private System.Windows.Forms.CheckBox chkStateInit;
        private System.Windows.Forms.Label lblStateDir;
        private System.Windows.Forms.ComboBox cmbStateDir;
        private System.Windows.Forms.Label lblStateNote;
        private System.Windows.Forms.Button btnRedMove;
        private System.Windows.Forms.Button btnBlueMove;
        private System.Windows.Forms.Button btnGreenMove;
        private System.Windows.Forms.Label lblRotateGroup;
        private System.Windows.Forms.Button btnRedRotate;
        private System.Windows.Forms.Button btnBlueRotate;
        private System.Windows.Forms.Button btnGreenRotate;
        private System.Windows.Forms.Button btnRedMoveM10;
        private System.Windows.Forms.Button btnRedMoveP10;
        private System.Windows.Forms.Button btnRedMoveMTxt;
        private System.Windows.Forms.Button btnRedMovePTxt;
        private System.Windows.Forms.Button btnBlueMoveM10;
        private System.Windows.Forms.Button btnBlueMoveP10;
        private System.Windows.Forms.Button btnBlueMoveMTxt;
        private System.Windows.Forms.Button btnBlueMovePTxt;
        private System.Windows.Forms.Button btnGreenMoveM10;
        private System.Windows.Forms.Button btnGreenMoveP10;
        private System.Windows.Forms.Button btnGreenMoveMTxt;
        private System.Windows.Forms.Button btnGreenMovePTxt;
        private System.Windows.Forms.Button btnRedRotateM90;
        private System.Windows.Forms.Button btnRedRotateP90;
        private System.Windows.Forms.Button btnRedRotateMTxt;
        private System.Windows.Forms.Button btnRedRotatePTxt;
        private System.Windows.Forms.Button btnBlueRotateM90;
        private System.Windows.Forms.Button btnBlueRotateP90;
        private System.Windows.Forms.Button btnBlueRotateMTxt;
        private System.Windows.Forms.Button btnBlueRotatePTxt;
        private System.Windows.Forms.Button btnGreenRotateM90;
        private System.Windows.Forms.Button btnGreenRotateP90;
        private System.Windows.Forms.Button btnGreenRotateMTxt;
        private System.Windows.Forms.Button btnGreenRotatePTxt;
        private System.Windows.Forms.Label lblEtcGroup;
        private System.Windows.Forms.Button btnMotor;
        private System.Windows.Forms.Button btnInit;
        private System.Windows.Forms.Label lblStatusTitle;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCurrentValue;
        private System.Windows.Forms.Label lblHistoryTitle;
        private System.Windows.Forms.ListBox lstHistory;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TabPage tabHistory;
        private System.Windows.Forms.TextBox txtLogHistory;
        private System.Windows.Forms.Button btnExportConfig;
        private System.Windows.Forms.Label lblWristBoundary;
        private System.Windows.Forms.ComboBox cmbWristMotor;
        private System.Windows.Forms.Label lblWristHint;
        private System.Windows.Forms.Label lblGripper;
        private System.Windows.Forms.ComboBox cmbGripperMotor;
        private System.Windows.Forms.Label lblGripperHint;
        private System.Windows.Forms.Button btnExportUrdf;
        private System.Windows.Forms.Button btnPyBullet;
        private System.Windows.Forms.Button btnMuJoCo;
        private System.Windows.Forms.Button btnIsaac;
        private System.Windows.Forms.Label lblIsaacPath;
        private System.Windows.Forms.TextBox txtIsaacPath;
        private System.Windows.Forms.Button btnFormulaView;
        private System.Windows.Forms.Button btnSendToSim;
        private System.Windows.Forms.CheckBox chkAutoSync;
        private System.Windows.Forms.Timer tmrAutoSync;
        private System.Windows.Forms.Button btnCaptureAll;
        private System.Windows.Forms.Label lblSpawnHeight;
        private System.Windows.Forms.TextBox txtSpawnHeight;
        private System.Windows.Forms.CheckBox chkFixedBase;
        private System.Windows.Forms.CheckBox chkExportScene;
        private System.Windows.Forms.CheckBox chkShowAxis;
        private System.Windows.Forms.Label lblUrdfFolder;
        private System.Windows.Forms.TextBox txtUrdfFolder;
        private System.Windows.Forms.Label lblStlFolder;
        private System.Windows.Forms.TextBox txtStlFolder;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnStl;
        private System.Windows.Forms.Button btnStlCenter;
        private System.Windows.Forms.Button btnStlMerge;
        private System.Windows.Forms.Label lblDhTitle;
        private System.Windows.Forms.RichTextBox txtDhParams;
        private System.Windows.Forms.Button btnApplyDh;
        private System.Windows.Forms.Label lblViewMode;
        private System.Windows.Forms.ComboBox cmbViewMode;
        private System.Windows.Forms.Label lblSagTwin;
        private System.Windows.Forms.ComboBox cmbSagTwin;
        private System.Windows.Forms.CheckBox chkSagCompOn;
        private System.Windows.Forms.Label lblSkeletonSize;
        private System.Windows.Forms.TextBox txtSkeletonSize;
        private System.Windows.Forms.Label lblAxisSize;
        private System.Windows.Forms.TextBox txtAxisSize;
        private System.Windows.Forms.ComboBox cmbMotorId;
        private System.Windows.Forms.ComboBox cmbExample;
        private System.Windows.Forms.TabPage tabPlayback;
        private System.Windows.Forms.Label lblPbDataset;
        private System.Windows.Forms.Button btnBrowseDataset;
        private System.Windows.Forms.ComboBox cmbEpisode;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblPlaySpeed;
        private System.Windows.Forms.TrackBar trkSpeed;
        private System.Windows.Forms.TrackBar trkFrame;
        private System.Windows.Forms.Label lblFrameInfo;
        private System.Windows.Forms.Label lblPbWrist;
        private System.Windows.Forms.Label lblPbExternal;
        private System.Windows.Forms.PictureBox picWrist;
        private System.Windows.Forms.PictureBox picExternal;
        private System.Windows.Forms.Label lblPbQposTitle;
        private System.Windows.Forms.PictureBox picGraphQpos;
        private System.Windows.Forms.Label lblPbTgtTitle;
        private System.Windows.Forms.PictureBox picGraphTgt;
        // ── tabArmSetup 컨트롤 (로봇팔 세팅 — 연결 줄·모터 세팅·측정·학습·시험, 코드는 Main.ArmMotor.cs / Main.ArmSetup.cs) ──
        private System.Windows.Forms.TabPage tabArmSetup;
        private System.Windows.Forms.GroupBox grpArmConn;
        private System.Windows.Forms.Label lblArmPort;
        private System.Windows.Forms.TextBox txtArmPort;
        private System.Windows.Forms.Label lblArmBaud;
        private System.Windows.Forms.TextBox txtArmBaud;
        private System.Windows.Forms.Button btnArmConnect;
        private System.Windows.Forms.Button btnArmTorque;
        private System.Windows.Forms.Label lblArmState;
        private System.Windows.Forms.TabControl tabArmInner;
        private System.Windows.Forms.TabPage tabArmPrac;
        private System.Windows.Forms.Label lblPracGuide;
        private System.Windows.Forms.CheckBox chkPracPrecise;
        private System.Windows.Forms.CheckBox chkPracDry;
        private System.Windows.Forms.Button btnPrac1;
        private System.Windows.Forms.Button btnPrac2;
        private System.Windows.Forms.Button btnPrac3;
        private System.Windows.Forms.Button btnPrac4;
        private System.Windows.Forms.Label lblPracStep;
        private System.Windows.Forms.PictureBox picPracChart;
        private System.Windows.Forms.Label lblPracResult;
        private System.Windows.Forms.Button btnPracStop;
        private System.Windows.Forms.Label lblPracBusy;
        private System.Windows.Forms.Label lblPracDef;
        private System.Windows.Forms.CheckBox chkPracDefShow;
        private System.Windows.Forms.Button btnPracDef;
        private System.Windows.Forms.LinkLabel lnkPracEye;
        private System.Windows.Forms.TabPage tabArmMotor;
        private System.Windows.Forms.DataGridView dgvArmMotor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArmAxis;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArmName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colArmDir;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArmCenter;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArmLo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArmHi;
        private System.Windows.Forms.DataGridViewComboBoxColumn colArmMirror;
        private System.Windows.Forms.DataGridViewComboBoxColumn colArmModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArmNowDeg;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colArmChecked;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArmNowTick;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArmRealModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArmNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArmGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArmBusId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArmMechMove;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArmMechAngle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArmInit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArmInit2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArmProto;
        private System.Windows.Forms.Button btnArmRead;
        private System.Windows.Forms.Button btnArmJogMinus;
        private System.Windows.Forms.TextBox txtArmJogStep;
        private System.Windows.Forms.Label lblArmJogUnit;
        private System.Windows.Forms.Button btnArmJogPlus;
        private System.Windows.Forms.Button btnArmSetZero;
        private System.Windows.Forms.Button btnArmSave;
        private System.Windows.Forms.Button btnArmLoad;
        private System.Windows.Forms.Button btnArmImportAuto;
        private System.Windows.Forms.CheckBox chkArmFollow;
        private System.Windows.Forms.CheckBox chkArmAdvanced;
        private System.Windows.Forms.Label lblArmFile;
        private System.Windows.Forms.Label lblArmHint;
        private System.Windows.Forms.TabPage tabArmDef;
        private System.Windows.Forms.Label lblDefGuide;
        private System.Windows.Forms.Label lblDefPreset;
        private System.Windows.Forms.ComboBox cmbDefPreset;
        private System.Windows.Forms.Button btnDefPresetSave;
        private System.Windows.Forms.Label lblDefTcp;
        private System.Windows.Forms.ComboBox cmbDefTcp;
        private System.Windows.Forms.Label lblDefTcpInfo;
        private System.Windows.Forms.Label lblDefWrist;
        private System.Windows.Forms.ComboBox cmbDefWrist;
        private System.Windows.Forms.Label lblDefWristInfo;
        private System.Windows.Forms.Label lblDefDown;
        private System.Windows.Forms.Label lblDefDownSum;
        private System.Windows.Forms.TextBox txtDefDownSum;
        private System.Windows.Forms.Label lblDefDownInfo;
        private System.Windows.Forms.Label lblDefShowNote;
        private System.Windows.Forms.Button btnDefSave;
        private System.Windows.Forms.Label lblDefStatus;
        private System.Windows.Forms.CheckBox chkDefAdv;
        private System.Windows.Forms.Label lblDefEye;
        private System.Windows.Forms.Button btnDefEye;
        private System.Windows.Forms.GroupBox grpDefAdv;
        private System.Windows.Forms.Label lblDefName;
        private System.Windows.Forms.TextBox txtDefName;
        private System.Windows.Forms.Label lblDefBase;
        private System.Windows.Forms.TextBox txtDefBase;
        private System.Windows.Forms.Label lblDefPitch;
        private System.Windows.Forms.TextBox txtDefPitch;
        private System.Windows.Forms.Label lblDefHold;
        private System.Windows.Forms.TextBox txtDefHold;
        private System.Windows.Forms.Label lblDefTcpLen;
        private System.Windows.Forms.TextBox txtDefTcpLen;
        private System.Windows.Forms.Label lblDefFixed;
        private System.Windows.Forms.TabPage tabArmExp;
        private System.Windows.Forms.GroupBox grpArmPlan;
        private System.Windows.Forms.Button btnArmPlanMake;
        private System.Windows.Forms.Button btnArmPlanLoad;
        private System.Windows.Forms.Button btnArmPlanDry;
        private System.Windows.Forms.Label lblArmPlan;
        private System.Windows.Forms.GroupBox grpArmMeasure;
        private System.Windows.Forms.CheckBox chkArmHold;
        private System.Windows.Forms.Button btnArmMeasure;
        private System.Windows.Forms.Label lblArmMeasure;
        private System.Windows.Forms.GroupBox grpArmLearn;
        private System.Windows.Forms.Label lblArmData;
        private System.Windows.Forms.Label lblArmDataInfo;
        private System.Windows.Forms.CheckBox chkArmUseHold;
        private System.Windows.Forms.Label lblArmRef;
        private System.Windows.Forms.TextBox txtArmRef;
        private System.Windows.Forms.Button btnArmLearn;
        private System.Windows.Forms.Label lblArmLearn;
        private System.Windows.Forms.GroupBox grpArmTest;
        private System.Windows.Forms.Label lblArmResult;
        private System.Windows.Forms.ComboBox cmbArmResult;
        private System.Windows.Forms.Button btnArmResultBrowse;
        private System.Windows.Forms.Label lblArmTouchRow;
        private System.Windows.Forms.Label lblArmTestR;
        private System.Windows.Forms.TextBox txtArmTestR;
        private System.Windows.Forms.Label lblArmTestYaw;
        private System.Windows.Forms.TextBox txtArmTestYaw;
        private System.Windows.Forms.Label lblArmTestPitch;
        private System.Windows.Forms.TextBox txtArmTestPitch;
        private System.Windows.Forms.Label lblArmTestRep;
        private System.Windows.Forms.TextBox txtArmTestRep;
        private System.Windows.Forms.CheckBox chkArmTestHold;
        private System.Windows.Forms.Button btnArmTestDry;
        private System.Windows.Forms.Button btnArmTest;
        private System.Windows.Forms.Label lblArmTest;
        private System.Windows.Forms.GroupBox grpArmApply;
        private System.Windows.Forms.Button btnArmApply;
        private System.Windows.Forms.Label lblArmActive;
        private System.Windows.Forms.Label lblArmBackup;
        private System.Windows.Forms.ComboBox cmbArmBackup;
        private System.Windows.Forms.Button btnArmRollback;
        private System.Windows.Forms.GroupBox grpArmTwin;
        private System.Windows.Forms.Label lblArmTwinSrc;
        private System.Windows.Forms.ComboBox cmbArmTwinSrc;
        private System.Windows.Forms.Label lblArmTwin;
        private System.Windows.Forms.Button btnArmStop;
        private System.Windows.Forms.Button btnArmOpenDir;
        private System.Windows.Forms.Label lblArmBusy;
        private System.Windows.Forms.Timer tmrArmFollow;
        private System.Windows.Forms.ToolTip tipArmExp;
        // ── tabTest 컨트롤 ──
        private System.Windows.Forms.Label lblIkGroup;
        private System.Windows.Forms.Label lblFuncNum;
        private System.Windows.Forms.ComboBox cmbFuncNum;
        private System.Windows.Forms.Label lblIkMode;
        private System.Windows.Forms.ComboBox cmbCalcInvMode;
        private System.Windows.Forms.Label lblIkX;
        private System.Windows.Forms.TextBox txtIkX;
        private System.Windows.Forms.Label lblIkY;
        private System.Windows.Forms.TextBox txtIkY;
        private System.Windows.Forms.Label lblIkZ;
        private System.Windows.Forms.TextBox txtIkZ;
        private System.Windows.Forms.Button btnGoIk;
        private System.Windows.Forms.Button btnGetFk;
        private System.Windows.Forms.Button btnResetAngle;
        private System.Windows.Forms.Label lblFkGroup;
        private System.Windows.Forms.Label lblMotorCtrl;
        private System.Windows.Forms.ComboBox cmbMotorCtrl;
        private System.Windows.Forms.Label lblAngleCtrl;
        private System.Windows.Forms.TextBox txtAngleCtrl;
        private System.Windows.Forms.Label lblAngleInfo;
        private System.Windows.Forms.TrackBar trkAngle;
        private System.Windows.Forms.Label lblMouseModeTitle;
        private System.Windows.Forms.RadioButton rdMouseCam;
        private System.Windows.Forms.RadioButton rdMouseJoint;
        // ── pnDhEdit 컨트롤 ──
        private System.Windows.Forms.Panel pnDhEdit;
        private System.Windows.Forms.Label lblDhEditTitle;
        private System.Windows.Forms.Label lblEditA;
        private System.Windows.Forms.TextBox txtEditA;
        private System.Windows.Forms.Label lblEditD;
        private System.Windows.Forms.TextBox txtEditD;
        private System.Windows.Forms.Label lblEditTheta;
        private System.Windows.Forms.TextBox txtEditTheta;
        private System.Windows.Forms.Label lblEditAlpha;
        private System.Windows.Forms.TextBox txtEditAlpha;
        private System.Windows.Forms.Label lblEditAxis;
        private System.Windows.Forms.TextBox txtEditAxis;
        private System.Windows.Forms.Label lblEditDir;
        private System.Windows.Forms.TextBox txtEditDir;
        private System.Windows.Forms.Label lblEditInit;
        private System.Windows.Forms.TextBox txtEditInit;
        private System.Windows.Forms.Label lblEditStlFile;
        private System.Windows.Forms.TextBox txtEditStlFile;
        private System.Windows.Forms.Label lblEditStlX;
        private System.Windows.Forms.TextBox txtEditStlX;
        private System.Windows.Forms.Label lblEditStlY;
        private System.Windows.Forms.TextBox txtEditStlY;
        private System.Windows.Forms.Label lblEditStlZ;
        private System.Windows.Forms.TextBox txtEditStlZ;
        private System.Windows.Forms.Label lblEditStlPan;
        private System.Windows.Forms.TextBox txtEditStlPan;
        private System.Windows.Forms.Label lblEditStlTilt;
        private System.Windows.Forms.TextBox txtEditStlTilt;
        private System.Windows.Forms.Label lblEditStlSwing;
        private System.Windows.Forms.TextBox txtEditStlSwing;
        private System.Windows.Forms.Button btnEditApply;
        private System.Windows.Forms.CheckBox chkShowArrow;
        private System.Windows.Forms.TextBox txtArrowSize;
        private System.Windows.Forms.CheckBox chkShowBall;
        private System.Windows.Forms.TextBox txtBallSize;
        private System.Windows.Forms.CheckBox chkShowSphere;
        private System.Windows.Forms.TextBox txtSphereSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
