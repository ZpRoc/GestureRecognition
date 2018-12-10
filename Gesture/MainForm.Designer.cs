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
            this.buttonAuto = new System.Windows.Forms.Button();
            this.buttonWrite = new System.Windows.Forms.Button();
            this.buttonGrab = new System.Windows.Forms.Button();
            this.textBoxSkeletonData = new System.Windows.Forms.TextBox();
            this.statusStripTime = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelFrameCounter = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelCurTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelAvgTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelMaxTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPageLabel = new System.Windows.Forms.TabPage();
            this.buttonStand = new System.Windows.Forms.Button();
            this.buttonTurnBack = new System.Windows.Forms.Button();
            this.buttonSitDown = new System.Windows.Forms.Button();
            this.buttonStandUp = new System.Windows.Forms.Button();
            this.buttonWalking = new System.Windows.Forms.Button();
            this.tabPageConfig = new System.Windows.Forms.TabPage();
            this.timerGrab = new System.Windows.Forms.Timer(this.components);
            this.panelControl.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageGrab.SuspendLayout();
            this.statusStripTime.SuspendLayout();
            this.tabPageLabel.SuspendLayout();
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
            this.tabPageGrab.Controls.Add(this.buttonAuto);
            this.tabPageGrab.Controls.Add(this.buttonWrite);
            this.tabPageGrab.Controls.Add(this.buttonGrab);
            this.tabPageGrab.Controls.Add(this.textBoxSkeletonData);
            this.tabPageGrab.Controls.Add(this.statusStripTime);
            this.tabPageGrab.Location = new System.Drawing.Point(4, 26);
            this.tabPageGrab.Name = "tabPageGrab";
            this.tabPageGrab.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGrab.Size = new System.Drawing.Size(360, 450);
            this.tabPageGrab.TabIndex = 0;
            this.tabPageGrab.Text = "Grab";
            this.tabPageGrab.UseVisualStyleBackColor = true;
            // 
            // buttonAuto
            // 
            this.buttonAuto.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonAuto.Location = new System.Drawing.Point(254, 82);
            this.buttonAuto.Name = "buttonAuto";
            this.buttonAuto.Size = new System.Drawing.Size(100, 32);
            this.buttonAuto.TabIndex = 9;
            this.buttonAuto.Tag = "0";
            this.buttonAuto.Text = "Auto ▶";
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
            this.buttonGrab.Text = "Grab ▶";
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
            this.textBoxSkeletonData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // statusStripTime
            // 
            this.statusStripTime.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelFrameCounter,
            this.toolStripStatusLabelCurTime,
            this.toolStripStatusLabelAvgTime,
            this.toolStripStatusLabelMaxTime});
            this.statusStripTime.Location = new System.Drawing.Point(3, 425);
            this.statusStripTime.Name = "statusStripTime";
            this.statusStripTime.Size = new System.Drawing.Size(354, 22);
            this.statusStripTime.TabIndex = 5;
            this.statusStripTime.Text = "statusStripTime";
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
            // buttonStand
            // 
            this.buttonStand.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonStand.Location = new System.Drawing.Point(237, 50);
            this.buttonStand.Name = "buttonStand";
            this.buttonStand.Size = new System.Drawing.Size(100, 32);
            this.buttonStand.TabIndex = 9;
            this.buttonStand.Text = "Stand";
            this.buttonStand.UseVisualStyleBackColor = true;
            // 
            // buttonTurnBack
            // 
            this.buttonTurnBack.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonTurnBack.Location = new System.Drawing.Point(237, 202);
            this.buttonTurnBack.Name = "buttonTurnBack";
            this.buttonTurnBack.Size = new System.Drawing.Size(100, 32);
            this.buttonTurnBack.TabIndex = 8;
            this.buttonTurnBack.Text = "Turn Back";
            this.buttonTurnBack.UseVisualStyleBackColor = true;
            // 
            // buttonSitDown
            // 
            this.buttonSitDown.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonSitDown.Location = new System.Drawing.Point(237, 164);
            this.buttonSitDown.Name = "buttonSitDown";
            this.buttonSitDown.Size = new System.Drawing.Size(100, 32);
            this.buttonSitDown.TabIndex = 7;
            this.buttonSitDown.Text = "Sit Down";
            this.buttonSitDown.UseVisualStyleBackColor = true;
            // 
            // buttonStandUp
            // 
            this.buttonStandUp.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonStandUp.Location = new System.Drawing.Point(237, 126);
            this.buttonStandUp.Name = "buttonStandUp";
            this.buttonStandUp.Size = new System.Drawing.Size(100, 32);
            this.buttonStandUp.TabIndex = 6;
            this.buttonStandUp.Text = "Stand Up";
            this.buttonStandUp.UseVisualStyleBackColor = true;
            // 
            // buttonWalking
            // 
            this.buttonWalking.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonWalking.Location = new System.Drawing.Point(237, 88);
            this.buttonWalking.Name = "buttonWalking";
            this.buttonWalking.Size = new System.Drawing.Size(100, 32);
            this.buttonWalking.TabIndex = 5;
            this.buttonWalking.Text = "Walking";
            this.buttonWalking.UseVisualStyleBackColor = true;
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 480);
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
            this.statusStripTime.ResumeLayout(false);
            this.statusStripTime.PerformLayout();
            this.tabPageLabel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Timer timerGrab;
        private System.Windows.Forms.StatusStrip statusStripTime;
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
    }
}

