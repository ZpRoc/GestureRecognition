namespace Gesture
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelControl = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageGrab = new System.Windows.Forms.TabPage();
            this.labelCameraInfo = new System.Windows.Forms.Label();
            this.numericUpDownHeight = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownWidth = new System.Windows.Forms.NumericUpDown();
            this.labelHeight = new System.Windows.Forms.Label();
            this.labelWidth = new System.Windows.Forms.Label();
            this.numericUpDownFPS = new System.Windows.Forms.NumericUpDown();
            this.labelFPS = new System.Windows.Forms.Label();
            this.buttonAuto = new System.Windows.Forms.Button();
            this.buttonWrite = new System.Windows.Forms.Button();
            this.buttonGrab = new System.Windows.Forms.Button();
            this.textBoxSkeletonData = new System.Windows.Forms.TextBox();
            this.statusStripGrab = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelFrameCounter = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelCurTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelAvgTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelMaxTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPageLabel = new System.Windows.Forms.TabPage();
            this.buttonOthersCnt = new System.Windows.Forms.Button();
            this.buttonSitCnt = new System.Windows.Forms.Button();
            this.buttonOthers = new System.Windows.Forms.Button();
            this.buttonSit = new System.Windows.Forms.Button();
            this.numericUpDownCoverData = new System.Windows.Forms.NumericUpDown();
            this.buttonAutoSelect = new System.Windows.Forms.Button();
            this.numericUpDownDispDelay = new System.Windows.Forms.NumericUpDown();
            this.buttonDispVedio = new System.Windows.Forms.Button();
            this.buttonTurnBackCnt = new System.Windows.Forms.Button();
            this.buttonSitDownCnt = new System.Windows.Forms.Button();
            this.buttonStandUpCnt = new System.Windows.Forms.Button();
            this.buttonWalkingCnt = new System.Windows.Forms.Button();
            this.buttonStandCnt = new System.Windows.Forms.Button();
            this.labelLoadData = new System.Windows.Forms.Label();
            this.labelSelectLable = new System.Windows.Forms.Label();
            this.numericUpDownCombineData = new System.Windows.Forms.NumericUpDown();
            this.labelCombineData = new System.Windows.Forms.Label();
            this.listBoxData = new System.Windows.Forms.ListBox();
            this.statusStripLabel = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelSelectData = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSeparator = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelAllData = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonLoadData = new System.Windows.Forms.Button();
            this.buttonStand = new System.Windows.Forms.Button();
            this.buttonTurnBack = new System.Windows.Forms.Button();
            this.buttonSitDown = new System.Windows.Forms.Button();
            this.buttonStandUp = new System.Windows.Forms.Button();
            this.buttonWalking = new System.Windows.Forms.Button();
            this.tabPageConfig = new System.Windows.Forms.TabPage();
            this.timerGrab = new System.Windows.Forms.Timer(this.components);
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.panelControl.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageGrab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFPS)).BeginInit();
            this.statusStripGrab.SuspendLayout();
            this.tabPageLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCoverData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDispDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCombineData)).BeginInit();
            this.statusStripLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            this.panelControl.Controls.Add(this.tabControl);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl.Location = new System.Drawing.Point(640, 0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(368, 480);
            this.panelControl.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageGrab);
            this.tabControl.Controls.Add(this.tabPageLabel);
            this.tabControl.Controls.Add(this.tabPageConfig);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(368, 480);
            this.tabControl.TabIndex = 8;
            // 
            // tabPageGrab
            // 
            this.tabPageGrab.Controls.Add(this.labelCameraInfo);
            this.tabPageGrab.Controls.Add(this.numericUpDownHeight);
            this.tabPageGrab.Controls.Add(this.numericUpDownWidth);
            this.tabPageGrab.Controls.Add(this.labelHeight);
            this.tabPageGrab.Controls.Add(this.labelWidth);
            this.tabPageGrab.Controls.Add(this.numericUpDownFPS);
            this.tabPageGrab.Controls.Add(this.labelFPS);
            this.tabPageGrab.Controls.Add(this.buttonAuto);
            this.tabPageGrab.Controls.Add(this.buttonWrite);
            this.tabPageGrab.Controls.Add(this.buttonGrab);
            this.tabPageGrab.Controls.Add(this.textBoxSkeletonData);
            this.tabPageGrab.Controls.Add(this.statusStripGrab);
            this.tabPageGrab.Location = new System.Drawing.Point(4, 26);
            this.tabPageGrab.Name = "tabPageGrab";
            this.tabPageGrab.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGrab.Size = new System.Drawing.Size(360, 450);
            this.tabPageGrab.TabIndex = 0;
            this.tabPageGrab.Text = "Grab";
            this.tabPageGrab.UseVisualStyleBackColor = true;
            // 
            // labelCameraInfo
            // 
            this.labelCameraInfo.AutoSize = true;
            this.labelCameraInfo.Location = new System.Drawing.Point(256, 128);
            this.labelCameraInfo.Name = "labelCameraInfo";
            this.labelCameraInfo.Size = new System.Drawing.Size(97, 19);
            this.labelCameraInfo.TabIndex = 16;
            this.labelCameraInfo.Text = "Camera Info";
            this.labelCameraInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDownHeight
            // 
            this.numericUpDownHeight.Enabled = false;
            this.numericUpDownHeight.Location = new System.Drawing.Point(305, 212);
            this.numericUpDownHeight.Maximum = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.numericUpDownHeight.Minimum = new decimal(new int[] {
            270,
            0,
            0,
            0});
            this.numericUpDownHeight.Name = "numericUpDownHeight";
            this.numericUpDownHeight.Size = new System.Drawing.Size(48, 25);
            this.numericUpDownHeight.TabIndex = 15;
            this.numericUpDownHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownHeight.Value = new decimal(new int[] {
            480,
            0,
            0,
            0});
            // 
            // numericUpDownWidth
            // 
            this.numericUpDownWidth.Enabled = false;
            this.numericUpDownWidth.Location = new System.Drawing.Point(306, 181);
            this.numericUpDownWidth.Maximum = new decimal(new int[] {
            1280,
            0,
            0,
            0});
            this.numericUpDownWidth.Minimum = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.numericUpDownWidth.Name = "numericUpDownWidth";
            this.numericUpDownWidth.Size = new System.Drawing.Size(48, 25);
            this.numericUpDownWidth.TabIndex = 14;
            this.numericUpDownWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownWidth.Value = new decimal(new int[] {
            640,
            0,
            0,
            0});
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(256, 215);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(25, 19);
            this.labelHeight.TabIndex = 13;
            this.labelHeight.Text = "H:";
            this.labelHeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Location = new System.Drawing.Point(256, 184);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(25, 19);
            this.labelWidth.TabIndex = 12;
            this.labelWidth.Text = "W:";
            this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDownFPS
            // 
            this.numericUpDownFPS.Location = new System.Drawing.Point(306, 150);
            this.numericUpDownFPS.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownFPS.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownFPS.Name = "numericUpDownFPS";
            this.numericUpDownFPS.Size = new System.Drawing.Size(48, 25);
            this.numericUpDownFPS.TabIndex = 11;
            this.numericUpDownFPS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownFPS.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownFPS.ValueChanged += new System.EventHandler(this.numericUpDownFPS_ValueChanged);
            // 
            // labelFPS
            // 
            this.labelFPS.AutoSize = true;
            this.labelFPS.Location = new System.Drawing.Point(256, 153);
            this.labelFPS.Name = "labelFPS";
            this.labelFPS.Size = new System.Drawing.Size(41, 19);
            this.labelFPS.TabIndex = 10;
            this.labelFPS.Text = "FPS:";
            this.labelFPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonAuto
            // 
            this.buttonAuto.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonAuto.Location = new System.Drawing.Point(254, 82);
            this.buttonAuto.Name = "buttonAuto";
            this.buttonAuto.Size = new System.Drawing.Size(100, 32);
            this.buttonAuto.TabIndex = 9;
            this.buttonAuto.Tag = "0";
            this.buttonAuto.Text = " Auto ▶";
            this.buttonAuto.UseVisualStyleBackColor = true;
            this.buttonAuto.Click += new System.EventHandler(this.buttonAuto_Click);
            // 
            // buttonWrite
            // 
            this.buttonWrite.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonWrite.Location = new System.Drawing.Point(254, 44);
            this.buttonWrite.Name = "buttonWrite";
            this.buttonWrite.Size = new System.Drawing.Size(100, 32);
            this.buttonWrite.TabIndex = 8;
            this.buttonWrite.Tag = "0";
            this.buttonWrite.Text = "Write ▶";
            this.buttonWrite.UseVisualStyleBackColor = true;
            this.buttonWrite.Click += new System.EventHandler(this.buttonWrite_Click);
            // 
            // buttonGrab
            // 
            this.buttonGrab.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonGrab.Location = new System.Drawing.Point(254, 6);
            this.buttonGrab.Name = "buttonGrab";
            this.buttonGrab.Size = new System.Drawing.Size(100, 32);
            this.buttonGrab.TabIndex = 6;
            this.buttonGrab.Tag = "0";
            this.buttonGrab.Text = " Grab ▶";
            this.buttonGrab.UseVisualStyleBackColor = true;
            this.buttonGrab.Click += new System.EventHandler(this.buttonGrab_Click);
            // 
            // textBoxSkeletonData
            // 
            this.textBoxSkeletonData.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 8F);
            this.textBoxSkeletonData.Location = new System.Drawing.Point(6, 6);
            this.textBoxSkeletonData.Multiline = true;
            this.textBoxSkeletonData.Name = "textBoxSkeletonData";
            this.textBoxSkeletonData.Size = new System.Drawing.Size(242, 414);
            this.textBoxSkeletonData.TabIndex = 7;
            this.textBoxSkeletonData.Text = "          X           Y           Z   ";
            this.textBoxSkeletonData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // statusStripGrab
            // 
            this.statusStripGrab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelFrameCounter,
            this.toolStripStatusLabelCurTime,
            this.toolStripStatusLabelAvgTime,
            this.toolStripStatusLabelMaxTime});
            this.statusStripGrab.Location = new System.Drawing.Point(3, 425);
            this.statusStripGrab.Name = "statusStripGrab";
            this.statusStripGrab.Size = new System.Drawing.Size(354, 22);
            this.statusStripGrab.TabIndex = 5;
            this.statusStripGrab.Text = "statusStripTime";
            // 
            // toolStripStatusLabelFrameCounter
            // 
            this.toolStripStatusLabelFrameCounter.Name = "toolStripStatusLabelFrameCounter";
            this.toolStripStatusLabelFrameCounter.Size = new System.Drawing.Size(43, 17);
            this.toolStripStatusLabelFrameCounter.Text = "00000";
            // 
            // toolStripStatusLabelCurTime
            // 
            this.toolStripStatusLabelCurTime.Name = "toolStripStatusLabelCurTime";
            this.toolStripStatusLabelCurTime.Size = new System.Drawing.Size(87, 17);
            this.toolStripStatusLabelCurTime.Text = "Cur: 00.00 ms";
            // 
            // toolStripStatusLabelAvgTime
            // 
            this.toolStripStatusLabelAvgTime.Name = "toolStripStatusLabelAvgTime";
            this.toolStripStatusLabelAvgTime.Size = new System.Drawing.Size(89, 17);
            this.toolStripStatusLabelAvgTime.Text = "Avg: 00.00 ms";
            // 
            // toolStripStatusLabelMaxTime
            // 
            this.toolStripStatusLabelMaxTime.Name = "toolStripStatusLabelMaxTime";
            this.toolStripStatusLabelMaxTime.Size = new System.Drawing.Size(92, 17);
            this.toolStripStatusLabelMaxTime.Tag = "";
            this.toolStripStatusLabelMaxTime.Text = "Max: 00.00 ms";
            // 
            // tabPageLabel
            // 
            this.tabPageLabel.Controls.Add(this.buttonOthersCnt);
            this.tabPageLabel.Controls.Add(this.buttonSitCnt);
            this.tabPageLabel.Controls.Add(this.buttonOthers);
            this.tabPageLabel.Controls.Add(this.buttonSit);
            this.tabPageLabel.Controls.Add(this.numericUpDownCoverData);
            this.tabPageLabel.Controls.Add(this.buttonAutoSelect);
            this.tabPageLabel.Controls.Add(this.numericUpDownDispDelay);
            this.tabPageLabel.Controls.Add(this.buttonDispVedio);
            this.tabPageLabel.Controls.Add(this.buttonTurnBackCnt);
            this.tabPageLabel.Controls.Add(this.buttonSitDownCnt);
            this.tabPageLabel.Controls.Add(this.buttonStandUpCnt);
            this.tabPageLabel.Controls.Add(this.buttonWalkingCnt);
            this.tabPageLabel.Controls.Add(this.buttonStandCnt);
            this.tabPageLabel.Controls.Add(this.labelLoadData);
            this.tabPageLabel.Controls.Add(this.labelSelectLable);
            this.tabPageLabel.Controls.Add(this.numericUpDownCombineData);
            this.tabPageLabel.Controls.Add(this.labelCombineData);
            this.tabPageLabel.Controls.Add(this.listBoxData);
            this.tabPageLabel.Controls.Add(this.statusStripLabel);
            this.tabPageLabel.Controls.Add(this.buttonLoadData);
            this.tabPageLabel.Controls.Add(this.buttonStand);
            this.tabPageLabel.Controls.Add(this.buttonTurnBack);
            this.tabPageLabel.Controls.Add(this.buttonSitDown);
            this.tabPageLabel.Controls.Add(this.buttonStandUp);
            this.tabPageLabel.Controls.Add(this.buttonWalking);
            this.tabPageLabel.Location = new System.Drawing.Point(4, 26);
            this.tabPageLabel.Name = "tabPageLabel";
            this.tabPageLabel.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLabel.Size = new System.Drawing.Size(360, 450);
            this.tabPageLabel.TabIndex = 1;
            this.tabPageLabel.Text = "Label";
            this.tabPageLabel.UseVisualStyleBackColor = true;
            // 
            // buttonOthersCnt
            // 
            this.buttonOthersCnt.Enabled = false;
            this.buttonOthersCnt.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonOthersCnt.Location = new System.Drawing.Point(247, 386);
            this.buttonOthersCnt.Name = "buttonOthersCnt";
            this.buttonOthersCnt.Size = new System.Drawing.Size(48, 32);
            this.buttonOthersCnt.TabIndex = 37;
            this.buttonOthersCnt.Text = "0";
            this.buttonOthersCnt.UseVisualStyleBackColor = true;
            this.buttonOthersCnt.Click += new System.EventHandler(this.buttonOthersCnt_Click);
            // 
            // buttonSitCnt
            // 
            this.buttonSitCnt.Enabled = false;
            this.buttonSitCnt.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonSitCnt.Location = new System.Drawing.Point(247, 196);
            this.buttonSitCnt.Name = "buttonSitCnt";
            this.buttonSitCnt.Size = new System.Drawing.Size(48, 32);
            this.buttonSitCnt.TabIndex = 36;
            this.buttonSitCnt.Text = "0";
            this.buttonSitCnt.UseVisualStyleBackColor = true;
            this.buttonSitCnt.Click += new System.EventHandler(this.buttonSitCnt_Click);
            // 
            // buttonOthers
            // 
            this.buttonOthers.Enabled = false;
            this.buttonOthers.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonOthers.Location = new System.Drawing.Point(128, 386);
            this.buttonOthers.Name = "buttonOthers";
            this.buttonOthers.Size = new System.Drawing.Size(100, 32);
            this.buttonOthers.TabIndex = 35;
            this.buttonOthers.Text = "Others";
            this.buttonOthers.UseVisualStyleBackColor = true;
            this.buttonOthers.Click += new System.EventHandler(this.buttonOthers_Click);
            // 
            // buttonSit
            // 
            this.buttonSit.Enabled = false;
            this.buttonSit.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonSit.Location = new System.Drawing.Point(128, 196);
            this.buttonSit.Name = "buttonSit";
            this.buttonSit.Size = new System.Drawing.Size(100, 32);
            this.buttonSit.TabIndex = 34;
            this.buttonSit.Text = "Sit";
            this.buttonSit.UseVisualStyleBackColor = true;
            this.buttonSit.Click += new System.EventHandler(this.buttonSit_Click);
            // 
            // numericUpDownCoverData
            // 
            this.numericUpDownCoverData.Location = new System.Drawing.Point(301, 48);
            this.numericUpDownCoverData.Name = "numericUpDownCoverData";
            this.numericUpDownCoverData.Size = new System.Drawing.Size(48, 25);
            this.numericUpDownCoverData.TabIndex = 33;
            this.numericUpDownCoverData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownCoverData.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // buttonAutoSelect
            // 
            this.buttonAutoSelect.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonAutoSelect.Location = new System.Drawing.Point(128, 120);
            this.buttonAutoSelect.Name = "buttonAutoSelect";
            this.buttonAutoSelect.Size = new System.Drawing.Size(64, 32);
            this.buttonAutoSelect.TabIndex = 31;
            this.buttonAutoSelect.Text = "Auto";
            this.buttonAutoSelect.UseVisualStyleBackColor = true;
            this.buttonAutoSelect.Click += new System.EventHandler(this.buttonAutoSelect_Click);
            // 
            // numericUpDownDispDelay
            // 
            this.numericUpDownDispDelay.Location = new System.Drawing.Point(236, 124);
            this.numericUpDownDispDelay.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numericUpDownDispDelay.Name = "numericUpDownDispDelay";
            this.numericUpDownDispDelay.Size = new System.Drawing.Size(48, 25);
            this.numericUpDownDispDelay.TabIndex = 30;
            this.numericUpDownDispDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownDispDelay.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // buttonDispVedio
            // 
            this.buttonDispVedio.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonDispVedio.Location = new System.Drawing.Point(198, 120);
            this.buttonDispVedio.Name = "buttonDispVedio";
            this.buttonDispVedio.Size = new System.Drawing.Size(32, 32);
            this.buttonDispVedio.TabIndex = 29;
            this.buttonDispVedio.Text = "▶";
            this.buttonDispVedio.UseVisualStyleBackColor = true;
            this.buttonDispVedio.Click += new System.EventHandler(this.buttonDispVedio_Click);
            // 
            // buttonTurnBackCnt
            // 
            this.buttonTurnBackCnt.Enabled = false;
            this.buttonTurnBackCnt.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonTurnBackCnt.Location = new System.Drawing.Point(247, 348);
            this.buttonTurnBackCnt.Name = "buttonTurnBackCnt";
            this.buttonTurnBackCnt.Size = new System.Drawing.Size(48, 32);
            this.buttonTurnBackCnt.TabIndex = 28;
            this.buttonTurnBackCnt.Text = "0";
            this.buttonTurnBackCnt.UseVisualStyleBackColor = true;
            this.buttonTurnBackCnt.Click += new System.EventHandler(this.buttonTurnbackCnt_Click);
            // 
            // buttonSitDownCnt
            // 
            this.buttonSitDownCnt.Enabled = false;
            this.buttonSitDownCnt.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonSitDownCnt.Location = new System.Drawing.Point(247, 310);
            this.buttonSitDownCnt.Name = "buttonSitDownCnt";
            this.buttonSitDownCnt.Size = new System.Drawing.Size(48, 32);
            this.buttonSitDownCnt.TabIndex = 27;
            this.buttonSitDownCnt.Text = "0";
            this.buttonSitDownCnt.UseVisualStyleBackColor = true;
            this.buttonSitDownCnt.Click += new System.EventHandler(this.buttonSitDownCnt_Click);
            // 
            // buttonStandUpCnt
            // 
            this.buttonStandUpCnt.Enabled = false;
            this.buttonStandUpCnt.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonStandUpCnt.Location = new System.Drawing.Point(247, 272);
            this.buttonStandUpCnt.Name = "buttonStandUpCnt";
            this.buttonStandUpCnt.Size = new System.Drawing.Size(48, 32);
            this.buttonStandUpCnt.TabIndex = 26;
            this.buttonStandUpCnt.Text = "0";
            this.buttonStandUpCnt.UseVisualStyleBackColor = true;
            this.buttonStandUpCnt.Click += new System.EventHandler(this.buttonStandUpCnt_Click);
            // 
            // buttonWalkingCnt
            // 
            this.buttonWalkingCnt.Enabled = false;
            this.buttonWalkingCnt.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonWalkingCnt.Location = new System.Drawing.Point(247, 234);
            this.buttonWalkingCnt.Name = "buttonWalkingCnt";
            this.buttonWalkingCnt.Size = new System.Drawing.Size(48, 32);
            this.buttonWalkingCnt.TabIndex = 25;
            this.buttonWalkingCnt.Text = "0";
            this.buttonWalkingCnt.UseVisualStyleBackColor = true;
            this.buttonWalkingCnt.Click += new System.EventHandler(this.buttonWalkingCnt_Click);
            // 
            // buttonStandCnt
            // 
            this.buttonStandCnt.Enabled = false;
            this.buttonStandCnt.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonStandCnt.Location = new System.Drawing.Point(247, 158);
            this.buttonStandCnt.Name = "buttonStandCnt";
            this.buttonStandCnt.Size = new System.Drawing.Size(48, 32);
            this.buttonStandCnt.TabIndex = 24;
            this.buttonStandCnt.Text = "0";
            this.buttonStandCnt.UseVisualStyleBackColor = true;
            this.buttonStandCnt.Click += new System.EventHandler(this.buttonStandCnt_Click);
            // 
            // labelLoadData
            // 
            this.labelLoadData.AutoSize = true;
            this.labelLoadData.Location = new System.Drawing.Point(112, 13);
            this.labelLoadData.Name = "labelLoadData";
            this.labelLoadData.Size = new System.Drawing.Size(97, 19);
            this.labelLoadData.TabIndex = 17;
            this.labelLoadData.Text = "Load data: ";
            this.labelLoadData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSelectLable
            // 
            this.labelSelectLable.AutoSize = true;
            this.labelSelectLable.Location = new System.Drawing.Point(112, 89);
            this.labelSelectLable.Name = "labelSelectLable";
            this.labelSelectLable.Size = new System.Drawing.Size(121, 19);
            this.labelSelectLable.TabIndex = 16;
            this.labelSelectLable.Text = "Select label: ";
            this.labelSelectLable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDownCombineData
            // 
            this.numericUpDownCombineData.Location = new System.Drawing.Point(247, 48);
            this.numericUpDownCombineData.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownCombineData.Name = "numericUpDownCombineData";
            this.numericUpDownCombineData.Size = new System.Drawing.Size(48, 25);
            this.numericUpDownCombineData.TabIndex = 15;
            this.numericUpDownCombineData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownCombineData.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // labelCombineData
            // 
            this.labelCombineData.AutoSize = true;
            this.labelCombineData.Location = new System.Drawing.Point(112, 51);
            this.labelCombineData.Name = "labelCombineData";
            this.labelCombineData.Size = new System.Drawing.Size(129, 19);
            this.labelCombineData.TabIndex = 14;
            this.labelCombineData.Text = "Combine datas: ";
            this.labelCombineData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listBoxData
            // 
            this.listBoxData.FormattingEnabled = true;
            this.listBoxData.ItemHeight = 17;
            this.listBoxData.Location = new System.Drawing.Point(6, 6);
            this.listBoxData.Name = "listBoxData";
            this.listBoxData.ScrollAlwaysVisible = true;
            this.listBoxData.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxData.Size = new System.Drawing.Size(100, 412);
            this.listBoxData.TabIndex = 13;
            this.listBoxData.SelectedIndexChanged += new System.EventHandler(this.listBoxData_SelectedIndexChanged);
            // 
            // statusStripLabel
            // 
            this.statusStripLabel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelSelectData,
            this.toolStripStatusLabelSeparator,
            this.toolStripStatusLabelAllData});
            this.statusStripLabel.Location = new System.Drawing.Point(3, 425);
            this.statusStripLabel.Name = "statusStripLabel";
            this.statusStripLabel.Size = new System.Drawing.Size(354, 22);
            this.statusStripLabel.TabIndex = 12;
            this.statusStripLabel.Text = "statusStrip1";
            // 
            // toolStripStatusLabelSelectData
            // 
            this.toolStripStatusLabelSelectData.Name = "toolStripStatusLabelSelectData";
            this.toolStripStatusLabelSelectData.Size = new System.Drawing.Size(22, 17);
            this.toolStripStatusLabelSelectData.Text = "00";
            // 
            // toolStripStatusLabelSeparator
            // 
            this.toolStripStatusLabelSeparator.Name = "toolStripStatusLabelSeparator";
            this.toolStripStatusLabelSeparator.Size = new System.Drawing.Size(13, 17);
            this.toolStripStatusLabelSeparator.Text = "/";
            // 
            // toolStripStatusLabelAllData
            // 
            this.toolStripStatusLabelAllData.Name = "toolStripStatusLabelAllData";
            this.toolStripStatusLabelAllData.Size = new System.Drawing.Size(43, 17);
            this.toolStripStatusLabelAllData.Text = "00000";
            // 
            // buttonLoadData
            // 
            this.buttonLoadData.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonLoadData.Location = new System.Drawing.Point(247, 6);
            this.buttonLoadData.Name = "buttonLoadData";
            this.buttonLoadData.Size = new System.Drawing.Size(100, 32);
            this.buttonLoadData.TabIndex = 11;
            this.buttonLoadData.Text = "Load Data";
            this.buttonLoadData.UseVisualStyleBackColor = true;
            this.buttonLoadData.Click += new System.EventHandler(this.buttonLoadData_Click);
            // 
            // buttonStand
            // 
            this.buttonStand.Enabled = false;
            this.buttonStand.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonStand.Location = new System.Drawing.Point(128, 158);
            this.buttonStand.Name = "buttonStand";
            this.buttonStand.Size = new System.Drawing.Size(100, 32);
            this.buttonStand.TabIndex = 9;
            this.buttonStand.Text = "Stand";
            this.buttonStand.UseVisualStyleBackColor = true;
            this.buttonStand.Click += new System.EventHandler(this.buttonStand_Click);
            // 
            // buttonTurnBack
            // 
            this.buttonTurnBack.Enabled = false;
            this.buttonTurnBack.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonTurnBack.Location = new System.Drawing.Point(128, 348);
            this.buttonTurnBack.Name = "buttonTurnBack";
            this.buttonTurnBack.Size = new System.Drawing.Size(100, 32);
            this.buttonTurnBack.TabIndex = 8;
            this.buttonTurnBack.Text = "Turn Back";
            this.buttonTurnBack.UseVisualStyleBackColor = true;
            this.buttonTurnBack.Click += new System.EventHandler(this.buttonTurnBack_Click);
            // 
            // buttonSitDown
            // 
            this.buttonSitDown.Enabled = false;
            this.buttonSitDown.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonSitDown.Location = new System.Drawing.Point(128, 310);
            this.buttonSitDown.Name = "buttonSitDown";
            this.buttonSitDown.Size = new System.Drawing.Size(100, 32);
            this.buttonSitDown.TabIndex = 7;
            this.buttonSitDown.Text = "Sit Down";
            this.buttonSitDown.UseVisualStyleBackColor = true;
            this.buttonSitDown.Click += new System.EventHandler(this.buttonSitDown_Click);
            // 
            // buttonStandUp
            // 
            this.buttonStandUp.Enabled = false;
            this.buttonStandUp.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonStandUp.Location = new System.Drawing.Point(128, 272);
            this.buttonStandUp.Name = "buttonStandUp";
            this.buttonStandUp.Size = new System.Drawing.Size(100, 32);
            this.buttonStandUp.TabIndex = 6;
            this.buttonStandUp.Text = "Stand Up";
            this.buttonStandUp.UseVisualStyleBackColor = true;
            this.buttonStandUp.Click += new System.EventHandler(this.buttonStandUp_Click);
            // 
            // buttonWalking
            // 
            this.buttonWalking.Enabled = false;
            this.buttonWalking.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonWalking.Location = new System.Drawing.Point(128, 234);
            this.buttonWalking.Name = "buttonWalking";
            this.buttonWalking.Size = new System.Drawing.Size(100, 32);
            this.buttonWalking.TabIndex = 5;
            this.buttonWalking.Text = "Walking";
            this.buttonWalking.UseVisualStyleBackColor = true;
            this.buttonWalking.Click += new System.EventHandler(this.buttonWalking_Click);
            // 
            // tabPageConfig
            // 
            this.tabPageConfig.Location = new System.Drawing.Point(4, 26);
            this.tabPageConfig.Name = "tabPageConfig";
            this.tabPageConfig.Size = new System.Drawing.Size(360, 450);
            this.tabPageConfig.TabIndex = 2;
            this.tabPageConfig.Text = "Config";
            this.tabPageConfig.UseVisualStyleBackColor = true;
            // 
            // timerGrab
            // 
            this.timerGrab.Interval = 32;
            this.timerGrab.Tick += new System.EventHandler(this.timerGrab_Tick);
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(640, 480);
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 480);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.panelControl);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.panelControl.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageGrab.ResumeLayout(false);
            this.tabPageGrab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFPS)).EndInit();
            this.statusStripGrab.ResumeLayout(false);
            this.statusStripGrab.PerformLayout();
            this.tabPageLabel.ResumeLayout(false);
            this.tabPageLabel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCoverData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDispDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCombineData)).EndInit();
            this.statusStripLabel.ResumeLayout(false);
            this.statusStripLabel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Timer timerGrab;
        private System.Windows.Forms.StatusStrip statusStripGrab;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelAvgTime;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMaxTime;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCurTime;
        private System.Windows.Forms.Button buttonGrab;
        private System.Windows.Forms.TextBox textBoxSkeletonData;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageLabel;
        private System.Windows.Forms.Button buttonStand;
        private System.Windows.Forms.Button buttonTurnBack;
        private System.Windows.Forms.Button buttonSitDown;
        private System.Windows.Forms.Button buttonStandUp;
        private System.Windows.Forms.Button buttonWalking;
        private System.Windows.Forms.TabPage tabPageGrab;
        private System.Windows.Forms.TabPage tabPageConfig;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelFrameCounter;
        private System.Windows.Forms.Button buttonWrite;
        private System.Windows.Forms.Button buttonAuto;
        private System.Windows.Forms.Button buttonLoadData;
        private System.Windows.Forms.StatusStrip statusStripLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelAllData;
        private System.Windows.Forms.ListBox listBoxData;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSelectData;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSeparator;
        private System.Windows.Forms.Label labelCombineData;
        private System.Windows.Forms.NumericUpDown numericUpDownCombineData;
        private System.Windows.Forms.Label labelLoadData;
        private System.Windows.Forms.Label labelSelectLable;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button buttonStandCnt;
        private System.Windows.Forms.Button buttonTurnBackCnt;
        private System.Windows.Forms.Button buttonSitDownCnt;
        private System.Windows.Forms.Button buttonStandUpCnt;
        private System.Windows.Forms.Button buttonWalkingCnt;
        private System.Windows.Forms.Label labelCameraInfo;
        private System.Windows.Forms.NumericUpDown numericUpDownHeight;
        private System.Windows.Forms.NumericUpDown numericUpDownWidth;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.NumericUpDown numericUpDownFPS;
        private System.Windows.Forms.Label labelFPS;
        private System.Windows.Forms.Button buttonDispVedio;
        private System.Windows.Forms.NumericUpDown numericUpDownDispDelay;
        private System.Windows.Forms.Button buttonAutoSelect;
        private System.Windows.Forms.NumericUpDown numericUpDownCoverData;
        private System.Windows.Forms.Button buttonOthersCnt;
        private System.Windows.Forms.Button buttonSitCnt;
        private System.Windows.Forms.Button buttonOthers;
        private System.Windows.Forms.Button buttonSit;
    }
}

