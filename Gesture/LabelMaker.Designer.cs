namespace Gesture
{
    partial class LabelMaker
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.numericUpDownIndex = new System.Windows.Forms.NumericUpDown();
            this.buttonCnt = new System.Windows.Forms.Button();
            this.buttonLabel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDownIndex
            // 
            this.numericUpDownIndex.Location = new System.Drawing.Point(160, 4);
            this.numericUpDownIndex.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDownIndex.Name = "numericUpDownIndex";
            this.numericUpDownIndex.Size = new System.Drawing.Size(48, 25);
            this.numericUpDownIndex.TabIndex = 41;
            this.numericUpDownIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownIndex.ValueChanged += new System.EventHandler(this.numericUpDownIndex_ValueChanged);
            // 
            // buttonCnt
            // 
            this.buttonCnt.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonCnt.Location = new System.Drawing.Point(106, 0);
            this.buttonCnt.Name = "buttonCnt";
            this.buttonCnt.Size = new System.Drawing.Size(48, 32);
            this.buttonCnt.TabIndex = 40;
            this.buttonCnt.Text = "0";
            this.buttonCnt.UseVisualStyleBackColor = true;
            this.buttonCnt.Click += new System.EventHandler(this.buttonCnt_Click);
            // 
            // buttonLabel
            // 
            this.buttonLabel.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.buttonLabel.Location = new System.Drawing.Point(0, 0);
            this.buttonLabel.Name = "buttonLabel";
            this.buttonLabel.Size = new System.Drawing.Size(100, 32);
            this.buttonLabel.TabIndex = 39;
            this.buttonLabel.Text = "Labalname";
            this.buttonLabel.UseVisualStyleBackColor = true;
            this.buttonLabel.Click += new System.EventHandler(this.buttonLabel_Click);
            // 
            // LabelMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numericUpDownIndex);
            this.Controls.Add(this.buttonCnt);
            this.Controls.Add(this.buttonLabel);
            this.Font = new System.Drawing.Font("YaHei Consolas Hybrid", 10F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LabelMaker";
            this.Size = new System.Drawing.Size(208, 32);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIndex)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDownIndex;
        private System.Windows.Forms.Button buttonCnt;
        private System.Windows.Forms.Button buttonLabel;
    }
}
