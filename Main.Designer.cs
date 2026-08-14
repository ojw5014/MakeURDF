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
            this.btnRedMove = new System.Windows.Forms.Button();
            this.btnBlueMove = new System.Windows.Forms.Button();
            this.btnGreenMove = new System.Windows.Forms.Button();
            this.lblRotateGroup = new System.Windows.Forms.Label();
            this.btnRedRotate = new System.Windows.Forms.Button();
            this.btnBlueRotate = new System.Windows.Forms.Button();
            this.btnGreenRotate = new System.Windows.Forms.Button();
            this.lblEtcGroup = new System.Windows.Forms.Label();
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
            this.btnUsdExport = new System.Windows.Forms.Button();
            this.btnSignCalib = new System.Windows.Forms.Button();
            this.btnSignClear = new System.Windows.Forms.Button();
            this.btnMechLimit = new System.Windows.Forms.Button();
            this.btnSim2Real = new System.Windows.Forms.Button();
            this.btnStandDroop = new System.Windows.Forms.Button();
            this.btnS2RGuide = new System.Windows.Forms.Button();
            this.btnIsaacCam = new System.Windows.Forms.Button();
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
            this.tabRight.SuspendLayout();
            this.tabCreate.SuspendLayout();
            this.pnDragArea.SuspendLayout();
            this.tabTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkAngle)).BeginInit();
            this.tabHistory.SuspendLayout();
            this.tabPlayback.SuspendLayout();
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
            this.tabRight.Location = new System.Drawing.Point(510, 5);
            this.tabRight.Margin = new System.Windows.Forms.Padding(4);
            this.tabRight.Name = "tabRight";
            this.tabRight.SelectedIndex = 0;
            this.tabRight.Size = new System.Drawing.Size(473, 600);
            this.tabRight.TabIndex = 1;
            this.tabRight.SelectedIndexChanged += new System.EventHandler(this.tabRight_SelectedIndexChanged);
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
            this.tabCreate.Controls.Add(this.btnRedMove);
            this.tabCreate.Controls.Add(this.btnBlueMove);
            this.tabCreate.Controls.Add(this.btnGreenMove);
            this.tabCreate.Controls.Add(this.lblRotateGroup);
            this.tabCreate.Controls.Add(this.btnRedRotate);
            this.tabCreate.Controls.Add(this.btnBlueRotate);
            this.tabCreate.Controls.Add(this.btnGreenRotate);
            this.tabCreate.Controls.Add(this.lblEtcGroup);
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
            this.tabCreate.Location = new System.Drawing.Point(4, 28);
            this.tabCreate.Margin = new System.Windows.Forms.Padding(4);
            this.tabCreate.Name = "tabCreate";
            this.tabCreate.Size = new System.Drawing.Size(465, 568);
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
            this.lblMoveInput.Location = new System.Drawing.Point(0, 44);
            this.lblMoveInput.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMoveInput.Name = "lblMoveInput";
            this.lblMoveInput.Size = new System.Drawing.Size(102, 46);
            this.lblMoveInput.TabIndex = 30;
            this.lblMoveInput.Text = "Move(mm)";
            this.lblMoveInput.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMoveInput.Visible = false;
            // 
            // txtMoveValue
            // 
            this.txtMoveValue.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtMoveValue.Location = new System.Drawing.Point(105, 44);
            this.txtMoveValue.Margin = new System.Windows.Forms.Padding(4);
            this.txtMoveValue.Name = "txtMoveValue";
            this.txtMoveValue.Size = new System.Drawing.Size(112, 45);
            this.txtMoveValue.TabIndex = 3;
            this.txtMoveValue.Text = "100";
            this.txtMoveValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMoveValue.Visible = false;
            // 
            // lblRotateInput
            // 
            this.lblRotateInput.Location = new System.Drawing.Point(218, 44);
            this.lblRotateInput.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRotateInput.Name = "lblRotateInput";
            this.lblRotateInput.Size = new System.Drawing.Size(102, 46);
            this.lblRotateInput.TabIndex = 31;
            this.lblRotateInput.Text = "Rot(deg)";
            this.lblRotateInput.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRotateInput.Visible = false;
            // 
            // txtRotateValue
            // 
            this.txtRotateValue.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtRotateValue.Location = new System.Drawing.Point(338, 44);
            this.txtRotateValue.Margin = new System.Windows.Forms.Padding(4);
            this.txtRotateValue.Name = "txtRotateValue";
            this.txtRotateValue.Size = new System.Drawing.Size(112, 45);
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
            this.lblMoveGroup.Location = new System.Drawing.Point(0, 95);
            this.lblMoveGroup.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMoveGroup.Name = "lblMoveGroup";
            this.lblMoveGroup.Size = new System.Drawing.Size(450, 24);
            this.lblMoveGroup.TabIndex = 5;
            this.lblMoveGroup.Text = "──── Move ────";
            this.lblMoveGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRedMove
            // 
            this.btnRedMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnRedMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedMove.ForeColor = System.Drawing.Color.White;
            this.btnRedMove.Location = new System.Drawing.Point(0, 122);
            this.btnRedMove.Margin = new System.Windows.Forms.Padding(4);
            this.btnRedMove.Name = "btnRedMove";
            this.btnRedMove.Size = new System.Drawing.Size(138, 38);
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
            this.btnBlueMove.Location = new System.Drawing.Point(156, 122);
            this.btnBlueMove.Margin = new System.Windows.Forms.Padding(4);
            this.btnBlueMove.Name = "btnBlueMove";
            this.btnBlueMove.Size = new System.Drawing.Size(138, 38);
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
            this.btnGreenMove.Location = new System.Drawing.Point(312, 122);
            this.btnGreenMove.Margin = new System.Windows.Forms.Padding(4);
            this.btnGreenMove.Name = "btnGreenMove";
            this.btnGreenMove.Size = new System.Drawing.Size(138, 38);
            this.btnGreenMove.TabIndex = 8;
            this.btnGreenMove.Text = "Y Move";
            this.btnGreenMove.UseVisualStyleBackColor = false;
            this.btnGreenMove.Click += new System.EventHandler(this.btnGreenMove_Click);
            // 
            // lblRotateGroup
            // 
            this.lblRotateGroup.Location = new System.Drawing.Point(0, 165);
            this.lblRotateGroup.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRotateGroup.Name = "lblRotateGroup";
            this.lblRotateGroup.Size = new System.Drawing.Size(450, 24);
            this.lblRotateGroup.TabIndex = 9;
            this.lblRotateGroup.Text = "──── Rotate ────";
            this.lblRotateGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRedRotate
            // 
            this.btnRedRotate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnRedRotate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRedRotate.ForeColor = System.Drawing.Color.White;
            this.btnRedRotate.Location = new System.Drawing.Point(0, 191);
            this.btnRedRotate.Margin = new System.Windows.Forms.Padding(4);
            this.btnRedRotate.Name = "btnRedRotate";
            this.btnRedRotate.Size = new System.Drawing.Size(138, 38);
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
            this.btnBlueRotate.Location = new System.Drawing.Point(156, 191);
            this.btnBlueRotate.Margin = new System.Windows.Forms.Padding(4);
            this.btnBlueRotate.Name = "btnBlueRotate";
            this.btnBlueRotate.Size = new System.Drawing.Size(138, 38);
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
            this.btnGreenRotate.Location = new System.Drawing.Point(312, 191);
            this.btnGreenRotate.Margin = new System.Windows.Forms.Padding(4);
            this.btnGreenRotate.Name = "btnGreenRotate";
            this.btnGreenRotate.Size = new System.Drawing.Size(138, 38);
            this.btnGreenRotate.TabIndex = 12;
            this.btnGreenRotate.Text = "Y Rotate";
            this.btnGreenRotate.UseVisualStyleBackColor = false;
            this.btnGreenRotate.Click += new System.EventHandler(this.btnGreenRotate_Click);
            // 
            // lblEtcGroup
            // 
            this.lblEtcGroup.Location = new System.Drawing.Point(0, 230);
            this.lblEtcGroup.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEtcGroup.Name = "lblEtcGroup";
            this.lblEtcGroup.Size = new System.Drawing.Size(450, 24);
            this.lblEtcGroup.TabIndex = 13;
            this.lblEtcGroup.Text = "──── Joint / STL ────";
            this.lblEtcGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnMotor
            // 
            this.btnMotor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(80)))));
            this.btnMotor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMotor.Location = new System.Drawing.Point(71, 257);
            this.btnMotor.Margin = new System.Windows.Forms.Padding(4);
            this.btnMotor.Name = "btnMotor";
            this.btnMotor.Size = new System.Drawing.Size(66, 38);
            this.btnMotor.TabIndex = 14;
            this.btnMotor.Text = "Joint";
            this.btnMotor.UseVisualStyleBackColor = false;
            this.btnMotor.Click += new System.EventHandler(this.btnMotor_Click);
            // 
            // cmbMotorId
            // 
            this.cmbMotorId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMotorId.Location = new System.Drawing.Point(0, 263);
            this.cmbMotorId.Margin = new System.Windows.Forms.Padding(4);
            this.cmbMotorId.Name = "cmbMotorId";
            this.cmbMotorId.Size = new System.Drawing.Size(63, 26);
            this.cmbMotorId.TabIndex = 42;
            // 
            // btnInit
            // 
            this.btnInit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnInit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInit.Location = new System.Drawing.Point(152, 257);
            this.btnInit.Margin = new System.Windows.Forms.Padding(4);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(145, 38);
            this.btnInit.TabIndex = 15;
            this.btnInit.Text = "New Chain";
            this.btnInit.UseVisualStyleBackColor = false;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // btnStl
            // 
            this.btnStl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(120)))), ((int)(((byte)(200)))));
            this.btnStl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStl.ForeColor = System.Drawing.Color.White;
            this.btnStl.Location = new System.Drawing.Point(312, 257);
            this.btnStl.Margin = new System.Windows.Forms.Padding(4);
            this.btnStl.Name = "btnStl";
            this.btnStl.Size = new System.Drawing.Size(138, 38);
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
            this.btnStlCenter.Location = new System.Drawing.Point(0, 298);
            this.btnStlCenter.Margin = new System.Windows.Forms.Padding(4);
            this.btnStlCenter.Name = "btnStlCenter";
            this.btnStlCenter.Size = new System.Drawing.Size(210, 38);
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
            this.btnStlMerge.Location = new System.Drawing.Point(240, 298);
            this.btnStlMerge.Margin = new System.Windows.Forms.Padding(4);
            this.btnStlMerge.Name = "btnStlMerge";
            this.btnStlMerge.Size = new System.Drawing.Size(210, 38);
            this.btnStlMerge.TabIndex = 34;
            this.btnStlMerge.Text = "Face Align";
            this.btnStlMerge.UseVisualStyleBackColor = false;
            this.btnStlMerge.Click += new System.EventHandler(this.btnStlMerge_Click);
            // 
            // lblStatusTitle
            // 
            this.lblStatusTitle.Location = new System.Drawing.Point(0, 343);
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
            this.lblStatus.Location = new System.Drawing.Point(0, 367);
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
            this.lblCurrentValue.Location = new System.Drawing.Point(0, 396);
            this.lblCurrentValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentValue.Name = "lblCurrentValue";
            this.lblCurrentValue.Size = new System.Drawing.Size(450, 43);
            this.lblCurrentValue.TabIndex = 18;
            this.lblCurrentValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHistoryTitle
            // 
            this.lblHistoryTitle.Location = new System.Drawing.Point(0, 444);
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
            this.lstHistory.Location = new System.Drawing.Point(0, 468);
            this.lstHistory.Margin = new System.Windows.Forms.Padding(4);
            this.lstHistory.Name = "lstHistory";
            this.lstHistory.Size = new System.Drawing.Size(450, 94);
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
            this.tabTest.Size = new System.Drawing.Size(465, 568);
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
            this.lblFkGroup.Size = new System.Drawing.Size(360, 18);
            this.lblFkGroup.TabIndex = 14;
            this.lblFkGroup.Text = "── FK (Joint Control) ──";
            this.lblFkGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMotorCtrl
            // 
            this.lblMotorCtrl.Location = new System.Drawing.Point(0, 142);
            this.lblMotorCtrl.Name = "lblMotorCtrl";
            this.lblMotorCtrl.Size = new System.Drawing.Size(34, 23);
            this.lblMotorCtrl.TabIndex = 15;
            this.lblMotorCtrl.Text = "Joint:";
            this.lblMotorCtrl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbMotorCtrl
            // 
            this.cmbMotorCtrl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMotorCtrl.Location = new System.Drawing.Point(34, 142);
            this.cmbMotorCtrl.Name = "cmbMotorCtrl";
            this.cmbMotorCtrl.Size = new System.Drawing.Size(55, 26);
            this.cmbMotorCtrl.TabIndex = 16;
            // 
            // lblAngleCtrl
            // 
            this.lblAngleCtrl.Location = new System.Drawing.Point(92, 142);
            this.lblAngleCtrl.Name = "lblAngleCtrl";
            this.lblAngleCtrl.Size = new System.Drawing.Size(34, 23);
            this.lblAngleCtrl.TabIndex = 17;
            this.lblAngleCtrl.Text = "Angle:";
            this.lblAngleCtrl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAngleCtrl
            // 
            this.txtAngleCtrl.Location = new System.Drawing.Point(126, 142);
            this.txtAngleCtrl.Name = "txtAngleCtrl";
            this.txtAngleCtrl.Size = new System.Drawing.Size(60, 28);
            this.txtAngleCtrl.TabIndex = 18;
            this.txtAngleCtrl.Text = "0";
            this.txtAngleCtrl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblAngleInfo
            // 
            this.lblAngleInfo.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblAngleInfo.Location = new System.Drawing.Point(190, 142);
            this.lblAngleInfo.Name = "lblAngleInfo";
            this.lblAngleInfo.Size = new System.Drawing.Size(160, 23);
            this.lblAngleInfo.TabIndex = 19;
            this.lblAngleInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trkAngle
            // 
            this.trkAngle.LargeChange = 100;
            this.trkAngle.Location = new System.Drawing.Point(0, 168);
            this.trkAngle.Maximum = 1800;
            this.trkAngle.Minimum = -1800;
            this.trkAngle.Name = "trkAngle";
            this.trkAngle.Size = new System.Drawing.Size(348, 69);
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
            this.tabHistory.Size = new System.Drawing.Size(465, 568);
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
            this.txtLogHistory.Size = new System.Drawing.Size(465, 568);
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
            this.tabPlayback.Size = new System.Drawing.Size(465, 568);
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
            this.btnMechLimit.Text = "2) 관절리미트";
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
            this.btnS2RGuide.Location = new System.Drawing.Point(1301, 284);
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
            this.btnIsaacCam.Size = new System.Drawing.Size(136, 41);
            this.btnIsaacCam.TabIndex = 313;
            this.btnIsaacCam.Text = "📺 Isaac 영상";
            this.btnIsaacCam.UseVisualStyleBackColor = false;
            this.btnIsaacCam.Click += new System.EventHandler(this.btnIsaacCam_Click);
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
            this.btnExportConfig.Location = new System.Drawing.Point(1296, 81);
            this.btnExportConfig.Margin = new System.Windows.Forms.Padding(4);
            this.btnExportConfig.Name = "btnExportConfig";
            this.btnExportConfig.Size = new System.Drawing.Size(211, 43);
            this.btnExportConfig.TabIndex = 46;
            this.btnExportConfig.Text = "Export Config";
            this.btnExportConfig.UseVisualStyleBackColor = false;
            this.btnExportConfig.Click += new System.EventHandler(this.btnExportConfig_Click);
            // 
            // lblWristBoundary
            // 
            this.lblWristBoundary.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblWristBoundary.Location = new System.Drawing.Point(1296, 129);
            this.lblWristBoundary.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWristBoundary.Name = "lblWristBoundary";
            this.lblWristBoundary.Size = new System.Drawing.Size(105, 29);
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
            this.cmbWristMotor.Location = new System.Drawing.Point(1409, 127);
            this.cmbWristMotor.Margin = new System.Windows.Forms.Padding(4);
            this.cmbWristMotor.Name = "cmbWristMotor";
            this.cmbWristMotor.Size = new System.Drawing.Size(98, 33);
            this.cmbWristMotor.TabIndex = 48;
            // 
            // lblWristHint
            // 
            this.lblWristHint.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblWristHint.ForeColor = System.Drawing.Color.Gray;
            this.lblWristHint.Location = new System.Drawing.Point(1297, 164);
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
            this.lblGripper.Location = new System.Drawing.Point(1296, 194);
            this.lblGripper.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGripper.Name = "lblGripper";
            this.lblGripper.Size = new System.Drawing.Size(105, 31);
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
            this.cmbGripperMotor.Location = new System.Drawing.Point(1409, 192);
            this.cmbGripperMotor.Margin = new System.Windows.Forms.Padding(4);
            this.cmbGripperMotor.Name = "cmbGripperMotor";
            this.cmbGripperMotor.Size = new System.Drawing.Size(98, 33);
            this.cmbGripperMotor.TabIndex = 51;
            // 
            // lblGripperHint
            // 
            this.lblGripperHint.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblGripperHint.ForeColor = System.Drawing.Color.Gray;
            this.lblGripperHint.Location = new System.Drawing.Point(1299, 234);
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
            this.lblDhTitle.Location = new System.Drawing.Point(991, 13);
            this.lblDhTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDhTitle.Name = "lblDhTitle";
            this.lblDhTitle.Size = new System.Drawing.Size(420, 18);
            this.lblDhTitle.TabIndex = 35;
            this.lblDhTitle.Text = "──── DH Parameters ────";
            this.lblDhTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDhParams
            // 
            this.txtDhParams.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDhParams.HideSelection = false;
            this.txtDhParams.Location = new System.Drawing.Point(991, 33);
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
            this.btnApplyDh.Location = new System.Drawing.Point(1296, 33);
            this.btnApplyDh.Margin = new System.Windows.Forms.Padding(4);
            this.btnApplyDh.Name = "btnApplyDh";
            this.btnApplyDh.Size = new System.Drawing.Size(211, 40);
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
            "5: OMX Follower (+Camera)",
            "6: Humanoid (Full)",
            "7: 2-Wheel Mobile",
            "8: 4-Wheel Mobile",
            "9: Cartesian (Desktop)",
            "[Lerobot_Ojw]Leader",
            "11: Tank + 6-Axis Arm",
            "12: XYZ Desktop Gantry",
            "13: Linear Leadscrew",
            "14: OMX Follower (Joints Only)",
            "15: Delta Robot (Parallel)"});
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
            this.pnDhEdit.Location = new System.Drawing.Point(991, 515);
            this.pnDhEdit.Name = "pnDhEdit";
            this.pnDhEdit.Size = new System.Drawing.Size(342, 206);
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
            this.txtEditAlpha.Size = new System.Drawing.Size(55, 29);
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
            this.txtEditStlFile.Size = new System.Drawing.Size(282, 29);
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
            this.lblEditStlPan.Location = new System.Drawing.Point(175, 131);
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
            this.txtEditStlPan.Location = new System.Drawing.Point(189, 131);
            this.txtEditStlPan.Name = "txtEditStlPan";
            this.txtEditStlPan.Size = new System.Drawing.Size(35, 29);
            this.txtEditStlPan.TabIndex = 24;
            this.txtEditStlPan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtEditStlPan.Visible = false;
            // 
            // lblEditStlTilt
            // 
            this.lblEditStlTilt.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblEditStlTilt.Location = new System.Drawing.Point(226, 131);
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
            this.txtEditStlTilt.Location = new System.Drawing.Point(240, 131);
            this.txtEditStlTilt.Name = "txtEditStlTilt";
            this.txtEditStlTilt.Size = new System.Drawing.Size(35, 29);
            this.txtEditStlTilt.TabIndex = 26;
            this.txtEditStlTilt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtEditStlTilt.Visible = false;
            // 
            // lblEditStlSwing
            // 
            this.lblEditStlSwing.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblEditStlSwing.Location = new System.Drawing.Point(277, 131);
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
            this.txtEditStlSwing.Location = new System.Drawing.Point(291, 131);
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
            this.btnEditApply.Location = new System.Drawing.Point(244, 165);
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
            this.Controls.Add(this.lblGripper);
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
        private System.Windows.Forms.Button btnRedMove;
        private System.Windows.Forms.Button btnBlueMove;
        private System.Windows.Forms.Button btnGreenMove;
        private System.Windows.Forms.Label lblRotateGroup;
        private System.Windows.Forms.Button btnRedRotate;
        private System.Windows.Forms.Button btnBlueRotate;
        private System.Windows.Forms.Button btnGreenRotate;
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
    }
}
