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
            this.btnExportUrdf = new System.Windows.Forms.Button();
            this.btnPyBullet = new System.Windows.Forms.Button();
            this.btnMuJoCo = new System.Windows.Forms.Button();
            this.btnFormulaView = new System.Windows.Forms.Button();
            this.chkFixedBase = new System.Windows.Forms.CheckBox();
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
            this.label1 = new System.Windows.Forms.Label();
            this.tabRight.SuspendLayout();
            this.tabCreate.SuspendLayout();
            this.pnDragArea.SuspendLayout();
            this.tabHistory.SuspendLayout();
            this.tabPlayback.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWrist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picExternal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGraphQpos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGraphTgt)).BeginInit();
            this.SuspendLayout();
            // 
            // pnDisp
            // 
            this.pnDisp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.pnDisp.Location = new System.Drawing.Point(15, 14);
            this.pnDisp.Margin = new System.Windows.Forms.Padding(4);
            this.pnDisp.Name = "pnDisp";
            this.pnDisp.Size = new System.Drawing.Size(975, 672);
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
            this.tabRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabRight.Controls.Add(this.tabCreate);
            this.tabRight.Controls.Add(this.tabTest);
            this.tabRight.Controls.Add(this.tabHistory);
            this.tabRight.Controls.Add(this.tabPlayback);
            this.tabRight.Location = new System.Drawing.Point(1000, 14);
            this.tabRight.Margin = new System.Windows.Forms.Padding(4);
            this.tabRight.Name = "tabRight";
            this.tabRight.SelectedIndex = 0;
            this.tabRight.Size = new System.Drawing.Size(462, 672);
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
            this.tabCreate.Size = new System.Drawing.Size(454, 640);
            this.tabCreate.TabIndex = 0;
            this.tabCreate.Text = "생성";
            // 
            // rdModeMouse
            // 
            this.rdModeMouse.AutoSize = true;
            this.rdModeMouse.Checked = true;
            this.rdModeMouse.Location = new System.Drawing.Point(5, 8);
            this.rdModeMouse.Margin = new System.Windows.Forms.Padding(4);
            this.rdModeMouse.Name = "rdModeMouse";
            this.rdModeMouse.Size = new System.Drawing.Size(87, 22);
            this.rdModeMouse.TabIndex = 0;
            this.rdModeMouse.TabStop = true;
            this.rdModeMouse.Text = "마우스";
            this.rdModeMouse.UseVisualStyleBackColor = true;
            this.rdModeMouse.CheckedChanged += new System.EventHandler(this.rdModeMouse_CheckedChanged);
            // 
            // rdModeButton
            // 
            this.rdModeButton.AutoSize = true;
            this.rdModeButton.Location = new System.Drawing.Point(130, 8);
            this.rdModeButton.Margin = new System.Windows.Forms.Padding(4);
            this.rdModeButton.Name = "rdModeButton";
            this.rdModeButton.Size = new System.Drawing.Size(105, 22);
            this.rdModeButton.TabIndex = 1;
            this.rdModeButton.Text = "버튼입력";
            this.rdModeButton.UseVisualStyleBackColor = true;
            this.rdModeButton.CheckedChanged += new System.EventHandler(this.rdModeButton_CheckedChanged);
            // 
            // rdModeHybrid
            // 
            this.rdModeHybrid.AutoSize = true;
            this.rdModeHybrid.Location = new System.Drawing.Point(280, 8);
            this.rdModeHybrid.Margin = new System.Windows.Forms.Padding(4);
            this.rdModeHybrid.Name = "rdModeHybrid";
            this.rdModeHybrid.Size = new System.Drawing.Size(123, 22);
            this.rdModeHybrid.TabIndex = 2;
            this.rdModeHybrid.Text = "하이브리드";
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
            this.lblMoveInput.Text = "이동(mm)";
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
            this.lblRotateInput.Text = "회전(도)";
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
            this.lblDragArea.Text = "버튼 선택 후 여기서 드래그";
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
            this.lblMoveGroup.Text = "──── 이동 ────";
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
            this.btnRedMove.Text = "빨간(X) 이동";
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
            this.btnBlueMove.Text = "파란(Z) 이동";
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
            this.btnGreenMove.Text = "초록(Y) 이동";
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
            this.lblRotateGroup.Text = "──── 회전 ────";
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
            this.btnRedRotate.Text = "빨간(X) 회전";
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
            this.btnBlueRotate.Text = "파란(Z) 회전";
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
            this.btnGreenRotate.Text = "초록(Y) 회전";
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
            this.lblEtcGroup.Text = "──── 기타 ────";
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
            this.btnMotor.Text = "모터";
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
            this.btnInit.Text = "여기서 초기화";
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
            this.btnStl.Text = "STL 추가";
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
            this.btnStlCenter.Text = "STL 중심이동";
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
            this.btnStlMerge.Text = "면 축 정렬";
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
            this.lblStatusTitle.Text = "──── 상태 ────";
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
            this.lblStatus.Text = "3D를 드래그하세요 (자동감지)";
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
            this.lblHistoryTitle.Text = "──── 히스토리 ────";
            this.lblHistoryTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstHistory
            // 
            this.lstHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstHistory.FormattingEnabled = true;
            this.lstHistory.ItemHeight = 18;
            this.lstHistory.Location = new System.Drawing.Point(0, 468);
            this.lstHistory.Margin = new System.Windows.Forms.Padding(4);
            this.lstHistory.Name = "lstHistory";
            this.lstHistory.Size = new System.Drawing.Size(449, 94);
            this.lstHistory.TabIndex = 20;
            this.lstHistory.SelectedIndexChanged += new System.EventHandler(this.lstHistory_SelectedIndexChanged);
            // 
            // btnUndo
            // 
            this.btnUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUndo.Location = new System.Drawing.Point(0, 571);
            this.btnUndo.Margin = new System.Windows.Forms.Padding(4);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(138, 34);
            this.btnUndo.TabIndex = 21;
            this.btnUndo.Text = "되돌리기";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(142, 571);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(138, 34);
            this.btnDelete.TabIndex = 22;
            this.btnDelete.Text = "선택 삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Location = new System.Drawing.Point(285, 571);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(138, 34);
            this.btnClear.TabIndex = 23;
            this.btnClear.Text = "전체 삭제";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tabTest
            // 
            this.tabTest.Location = new System.Drawing.Point(4, 28);
            this.tabTest.Margin = new System.Windows.Forms.Padding(4);
            this.tabTest.Name = "tabTest";
            this.tabTest.Size = new System.Drawing.Size(454, 640);
            this.tabTest.TabIndex = 1;
            this.tabTest.Text = "테스트";
            // 
            // tabHistory
            // 
            this.tabHistory.Controls.Add(this.txtLogHistory);
            this.tabHistory.Location = new System.Drawing.Point(4, 28);
            this.tabHistory.Margin = new System.Windows.Forms.Padding(4);
            this.tabHistory.Name = "tabHistory";
            this.tabHistory.Size = new System.Drawing.Size(454, 640);
            this.tabHistory.TabIndex = 2;
            this.tabHistory.Text = "로그";
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
            this.txtLogHistory.Size = new System.Drawing.Size(454, 640);
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
            this.tabPlayback.Size = new System.Drawing.Size(454, 640);
            this.tabPlayback.TabIndex = 3;
            this.tabPlayback.Text = "재생";
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
            this.btnBrowseDataset.Text = "폴더 선택";
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
            // btnExportUrdf
            // 
            this.btnExportUrdf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportUrdf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnExportUrdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportUrdf.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExportUrdf.ForeColor = System.Drawing.Color.White;
            this.btnExportUrdf.Location = new System.Drawing.Point(15, 696);
            this.btnExportUrdf.Margin = new System.Windows.Forms.Padding(4);
            this.btnExportUrdf.Name = "btnExportUrdf";
            this.btnExportUrdf.Size = new System.Drawing.Size(162, 42);
            this.btnExportUrdf.TabIndex = 2;
            this.btnExportUrdf.Text = "URDF 보기";
            this.btnExportUrdf.UseVisualStyleBackColor = false;
            this.btnExportUrdf.Click += new System.EventHandler(this.btnExportUrdf_Click);
            // 
            // btnPyBullet
            // 
            this.btnPyBullet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPyBullet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnPyBullet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPyBullet.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPyBullet.ForeColor = System.Drawing.Color.White;
            this.btnPyBullet.Location = new System.Drawing.Point(185, 696);
            this.btnPyBullet.Margin = new System.Windows.Forms.Padding(4);
            this.btnPyBullet.Name = "btnPyBullet";
            this.btnPyBullet.Size = new System.Drawing.Size(162, 42);
            this.btnPyBullet.TabIndex = 3;
            this.btnPyBullet.Text = "PyBullet 실행";
            this.btnPyBullet.UseVisualStyleBackColor = false;
            this.btnPyBullet.Click += new System.EventHandler(this.btnPyBullet_Click);
            // 
            // btnMuJoCo
            // 
            this.btnMuJoCo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMuJoCo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnMuJoCo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMuJoCo.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMuJoCo.ForeColor = System.Drawing.Color.White;
            this.btnMuJoCo.Location = new System.Drawing.Point(355, 696);
            this.btnMuJoCo.Margin = new System.Windows.Forms.Padding(4);
            this.btnMuJoCo.Name = "btnMuJoCo";
            this.btnMuJoCo.Size = new System.Drawing.Size(162, 42);
            this.btnMuJoCo.TabIndex = 4;
            this.btnMuJoCo.Text = "MuJoCo 실행";
            this.btnMuJoCo.UseVisualStyleBackColor = false;
            this.btnMuJoCo.Click += new System.EventHandler(this.btnMuJoCo_Click);
            // 
            // btnFormulaView
            // 
            this.btnFormulaView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFormulaView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(120)))), ((int)(((byte)(40)))));
            this.btnFormulaView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFormulaView.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFormulaView.ForeColor = System.Drawing.Color.White;
            this.btnFormulaView.Location = new System.Drawing.Point(525, 696);
            this.btnFormulaView.Margin = new System.Windows.Forms.Padding(4);
            this.btnFormulaView.Name = "btnFormulaView";
            this.btnFormulaView.Size = new System.Drawing.Size(112, 42);
            this.btnFormulaView.TabIndex = 40;
            this.btnFormulaView.Text = "수식보기";
            this.btnFormulaView.UseVisualStyleBackColor = false;
            this.btnFormulaView.Click += new System.EventHandler(this.btnFormulaView_Click);
            // 
            // chkFixedBase
            // 
            this.chkFixedBase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkFixedBase.AutoSize = true;
            this.chkFixedBase.Checked = true;
            this.chkFixedBase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFixedBase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.chkFixedBase.Location = new System.Drawing.Point(15, 747);
            this.chkFixedBase.Margin = new System.Windows.Forms.Padding(4);
            this.chkFixedBase.Name = "chkFixedBase";
            this.chkFixedBase.Size = new System.Drawing.Size(106, 22);
            this.chkFixedBase.TabIndex = 50;
            this.chkFixedBase.Text = "좌표고정";
            this.chkFixedBase.UseVisualStyleBackColor = true;
            // 
            // chkShowAxis
            // 
            this.chkShowAxis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowAxis.AutoSize = true;
            this.chkShowAxis.Checked = true;
            this.chkShowAxis.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowAxis.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.chkShowAxis.Location = new System.Drawing.Point(122, 747);
            this.chkShowAxis.Margin = new System.Windows.Forms.Padding(4);
            this.chkShowAxis.Name = "chkShowAxis";
            this.chkShowAxis.Size = new System.Drawing.Size(88, 22);
            this.chkShowAxis.TabIndex = 51;
            this.chkShowAxis.Text = "좌표축";
            this.chkShowAxis.UseVisualStyleBackColor = true;
            this.chkShowAxis.CheckedChanged += new System.EventHandler(this.chkShowAxis_CheckedChanged);
            // 
            // lblUrdfFolder
            // 
            this.lblUrdfFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblUrdfFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.lblUrdfFolder.Location = new System.Drawing.Point(236, 746);
            this.lblUrdfFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUrdfFolder.Name = "lblUrdfFolder";
            this.lblUrdfFolder.Size = new System.Drawing.Size(88, 23);
            this.lblUrdfFolder.TabIndex = 60;
            this.lblUrdfFolder.Text = "출력폴더:";
            // 
            // txtUrdfFolder
            // 
            this.txtUrdfFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtUrdfFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.txtUrdfFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUrdfFolder.ForeColor = System.Drawing.Color.White;
            this.txtUrdfFolder.Location = new System.Drawing.Point(321, 742);
            this.txtUrdfFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtUrdfFolder.Name = "txtUrdfFolder";
            this.txtUrdfFolder.Size = new System.Drawing.Size(100, 28);
            this.txtUrdfFolder.TabIndex = 53;
            this.txtUrdfFolder.Text = "urdf";
            // 
            // lblStlFolder
            // 
            this.lblStlFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStlFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.lblStlFolder.Location = new System.Drawing.Point(463, 746);
            this.lblStlFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStlFolder.Name = "lblStlFolder";
            this.lblStlFolder.Size = new System.Drawing.Size(86, 23);
            this.lblStlFolder.TabIndex = 61;
            this.lblStlFolder.Text = "STL폴더:";
            // 
            // txtStlFolder
            // 
            this.txtStlFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtStlFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.txtStlFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStlFolder.ForeColor = System.Drawing.Color.White;
            this.txtStlFolder.Location = new System.Drawing.Point(549, 742);
            this.txtStlFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtStlFolder.Name = "txtStlFolder";
            this.txtStlFolder.Size = new System.Drawing.Size(100, 28);
            this.txtStlFolder.TabIndex = 55;
            this.txtStlFolder.Text = "stl";
            // 
            // btnExportConfig
            // 
            this.btnExportConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(100)))), ((int)(((byte)(50)))));
            this.btnExportConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportConfig.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExportConfig.ForeColor = System.Drawing.Color.White;
            this.btnExportConfig.Location = new System.Drawing.Point(1750, 712);
            this.btnExportConfig.Margin = new System.Windows.Forms.Padding(4);
            this.btnExportConfig.Name = "btnExportConfig";
            this.btnExportConfig.Size = new System.Drawing.Size(162, 42);
            this.btnExportConfig.TabIndex = 46;
            this.btnExportConfig.Text = "Config 저장";
            this.btnExportConfig.UseVisualStyleBackColor = false;
            this.btnExportConfig.Click += new System.EventHandler(this.btnExportConfig_Click);
            // 
            // lblWristBoundary
            // 
            this.lblWristBoundary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWristBoundary.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblWristBoundary.Location = new System.Drawing.Point(1475, 640);
            this.lblWristBoundary.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWristBoundary.Name = "lblWristBoundary";
            this.lblWristBoundary.Size = new System.Drawing.Size(100, 29);
            this.lblWristBoundary.TabIndex = 47;
            this.lblWristBoundary.Text = "손목 경계:";
            this.lblWristBoundary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbWristMotor
            // 
            this.cmbWristMotor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbWristMotor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWristMotor.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.cmbWristMotor.Items.AddRange(new object[] {
            "없음 (전체 팔)"});
            this.cmbWristMotor.Location = new System.Drawing.Point(1580, 638);
            this.cmbWristMotor.Margin = new System.Windows.Forms.Padding(4);
            this.cmbWristMotor.Name = "cmbWristMotor";
            this.cmbWristMotor.Size = new System.Drawing.Size(162, 33);
            this.cmbWristMotor.TabIndex = 48;
            // 
            // lblWristHint
            // 
            this.lblWristHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWristHint.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblWristHint.ForeColor = System.Drawing.Color.Gray;
            this.lblWristHint.Location = new System.Drawing.Point(1748, 640);
            this.lblWristHint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWristHint.Name = "lblWristHint";
            this.lblWristHint.Size = new System.Drawing.Size(165, 29);
            this.lblWristHint.TabIndex = 49;
            this.lblWristHint.Text = "← 이 모터 뒤부터 손목";
            this.lblWristHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGripper
            // 
            this.lblGripper.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGripper.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblGripper.Location = new System.Drawing.Point(1475, 676);
            this.lblGripper.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGripper.Name = "lblGripper";
            this.lblGripper.Size = new System.Drawing.Size(100, 29);
            this.lblGripper.TabIndex = 50;
            this.lblGripper.Text = "그리퍼:";
            this.lblGripper.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbGripperMotor
            // 
            this.cmbGripperMotor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbGripperMotor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGripperMotor.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.cmbGripperMotor.Items.AddRange(new object[] {
            "없음"});
            this.cmbGripperMotor.Location = new System.Drawing.Point(1580, 674);
            this.cmbGripperMotor.Margin = new System.Windows.Forms.Padding(4);
            this.cmbGripperMotor.Name = "cmbGripperMotor";
            this.cmbGripperMotor.Size = new System.Drawing.Size(162, 33);
            this.cmbGripperMotor.TabIndex = 51;
            // 
            // lblGripperHint
            // 
            this.lblGripperHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGripperHint.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.lblGripperHint.ForeColor = System.Drawing.Color.Gray;
            this.lblGripperHint.Location = new System.Drawing.Point(1748, 676);
            this.lblGripperHint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGripperHint.Name = "lblGripperHint";
            this.lblGripperHint.Size = new System.Drawing.Size(165, 29);
            this.lblGripperHint.TabIndex = 52;
            this.lblGripperHint.Text = "← 그리퍼 모터 ID";
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
            this.lblDhTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDhTitle.Location = new System.Drawing.Point(1475, 14);
            this.lblDhTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDhTitle.Name = "lblDhTitle";
            this.lblDhTitle.Size = new System.Drawing.Size(438, 24);
            this.lblDhTitle.TabIndex = 35;
            this.lblDhTitle.Text = "──── DH 파라미터 ────";
            this.lblDhTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDhParams
            // 
            this.txtDhParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDhParams.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDhParams.HideSelection = false;
            this.txtDhParams.Location = new System.Drawing.Point(1475, 41);
            this.txtDhParams.Margin = new System.Windows.Forms.Padding(4);
            this.txtDhParams.Name = "txtDhParams";
            this.txtDhParams.Size = new System.Drawing.Size(436, 551);
            this.txtDhParams.TabIndex = 36;
            this.txtDhParams.Text = "";
            this.txtDhParams.WordWrap = false;
            // 
            // btnApplyDh
            // 
            this.btnApplyDh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApplyDh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(160)))), ((int)(((byte)(70)))));
            this.btnApplyDh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyDh.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnApplyDh.ForeColor = System.Drawing.Color.White;
            this.btnApplyDh.Location = new System.Drawing.Point(1475, 712);
            this.btnApplyDh.Margin = new System.Windows.Forms.Padding(4);
            this.btnApplyDh.Name = "btnApplyDh";
            this.btnApplyDh.Size = new System.Drawing.Size(269, 42);
            this.btnApplyDh.TabIndex = 37;
            this.btnApplyDh.Text = "DH 직접 적용";
            this.btnApplyDh.UseVisualStyleBackColor = false;
            this.btnApplyDh.Click += new System.EventHandler(this.btnApplyDh_Click);
            // 
            // lblViewMode
            // 
            this.lblViewMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblViewMode.Location = new System.Drawing.Point(657, 746);
            this.lblViewMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblViewMode.Name = "lblViewMode";
            this.lblViewMode.Size = new System.Drawing.Size(97, 23);
            this.lblViewMode.TabIndex = 38;
            this.lblViewMode.Text = "보기종류:";
            this.lblViewMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbViewMode
            // 
            this.cmbViewMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbViewMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbViewMode.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbViewMode.Items.AddRange(new object[] {
            "전체",
            "스켈레톤",
            "STL"});
            this.cmbViewMode.Location = new System.Drawing.Point(750, 742);
            this.cmbViewMode.Margin = new System.Windows.Forms.Padding(4);
            this.cmbViewMode.Name = "cmbViewMode";
            this.cmbViewMode.Size = new System.Drawing.Size(124, 33);
            this.cmbViewMode.TabIndex = 39;
            this.cmbViewMode.SelectedIndexChanged += new System.EventHandler(this.cmbViewMode_SelectedIndexChanged);
            // 
            // lblSkeletonSize
            // 
            this.lblSkeletonSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSkeletonSize.Location = new System.Drawing.Point(885, 746);
            this.lblSkeletonSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSkeletonSize.Name = "lblSkeletonSize";
            this.lblSkeletonSize.Size = new System.Drawing.Size(84, 23);
            this.lblSkeletonSize.TabIndex = 40;
            this.lblSkeletonSize.Text = "바(Size):";
            this.lblSkeletonSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSkeletonSize
            // 
            this.txtSkeletonSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSkeletonSize.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSkeletonSize.Location = new System.Drawing.Point(961, 742);
            this.txtSkeletonSize.Margin = new System.Windows.Forms.Padding(4);
            this.txtSkeletonSize.Name = "txtSkeletonSize";
            this.txtSkeletonSize.Size = new System.Drawing.Size(52, 31);
            this.txtSkeletonSize.TabIndex = 41;
            this.txtSkeletonSize.Text = "10";
            this.txtSkeletonSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSkeletonSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSkeletonSize_KeyDown);
            // 
            // lblAxisSize
            // 
            this.lblAxisSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAxisSize.Location = new System.Drawing.Point(1021, 746);
            this.lblAxisSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAxisSize.Name = "lblAxisSize";
            this.lblAxisSize.Size = new System.Drawing.Size(85, 23);
            this.lblAxisSize.TabIndex = 43;
            this.lblAxisSize.Text = "축(Size):";
            this.lblAxisSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAxisSize
            // 
            this.txtAxisSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtAxisSize.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtAxisSize.Location = new System.Drawing.Point(1109, 742);
            this.txtAxisSize.Margin = new System.Windows.Forms.Padding(4);
            this.txtAxisSize.Name = "txtAxisSize";
            this.txtAxisSize.Size = new System.Drawing.Size(52, 31);
            this.txtAxisSize.TabIndex = 44;
            this.txtAxisSize.Text = "20";
            this.txtAxisSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAxisSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAxisSize_KeyDown);
            // 
            // cmbExample
            // 
            this.cmbExample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbExample.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExample.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbExample.Items.AddRange(new object[] {
            "예제 선택...",
            "예제1: 간단로봇",
            "예제2: 휴머노이드(스켈레톤)",
            "예제3: 6축+STL",
            "예제4: OMX리더",
            "예제5: OMX팔로워",
            "예제6: 휴머노이드",
            "예제7: 2륜 모바일",
            "예제8: 4륜 모바일"});
            this.cmbExample.Location = new System.Drawing.Point(700, 698);
            this.cmbExample.Margin = new System.Windows.Forms.Padding(4);
            this.cmbExample.Name = "cmbExample";
            this.cmbExample.Size = new System.Drawing.Size(212, 33);
            this.cmbExample.TabIndex = 45;
            this.cmbExample.SelectedIndexChanged += new System.EventHandler(this.cmbExample_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(648, 703);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 28);
            this.label1.TabIndex = 38;
            this.label1.Text = "예제:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1938, 792);
            this.Controls.Add(this.pnDisp);
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
            this.Controls.Add(this.btnFormulaView);
            this.Controls.Add(this.chkFixedBase);
            this.Controls.Add(this.chkShowAxis);
            this.Controls.Add(this.txtUrdfFolder);
            this.Controls.Add(this.txtStlFolder);
            this.Controls.Add(this.cmbExample);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblViewMode);
            this.Controls.Add(this.cmbViewMode);
            this.Controls.Add(this.lblSkeletonSize);
            this.Controls.Add(this.txtSkeletonSize);
            this.Controls.Add(this.lblAxisSize);
            this.Controls.Add(this.txtAxisSize);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lblUrdfFolder);
            this.Controls.Add(this.lblStlFolder);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.Text = "MakeUrdf - 쉬운 로봇 설계기";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.tabRight.ResumeLayout(false);
            this.tabCreate.ResumeLayout(false);
            this.tabCreate.PerformLayout();
            this.pnDragArea.ResumeLayout(false);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnDisp;
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
        private System.Windows.Forms.Button btnFormulaView;
        private System.Windows.Forms.CheckBox chkFixedBase;
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
        private System.Windows.Forms.Label label1;
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
    }
}
