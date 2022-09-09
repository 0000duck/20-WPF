namespace saker_Winform.UserControls
{
    partial class UCChanWaveView
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label_ChanID = new System.Windows.Forms.Label();
            this.label_Inv = new System.Windows.Forms.Label();
            this.checkBox_Select = new System.Windows.Forms.CheckBox();
            this.label_Scale = new System.Windows.Forms.Label();
            this.label_BwLimit = new System.Windows.Forms.Label();
            this.label_Impdence = new System.Windows.Forms.Label();
            this.label_Offset = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_ChanID
            // 
            this.label_ChanID.BackColor = System.Drawing.Color.Orange;
            this.label_ChanID.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_ChanID.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_ChanID.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ChanID.Location = new System.Drawing.Point(0, 0);
            this.label_ChanID.Name = "label_ChanID";
            this.label_ChanID.Size = new System.Drawing.Size(62, 53);
            this.label_ChanID.TabIndex = 1;
            this.label_ChanID.Text = "Tag001";
            this.label_ChanID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_ChanID.Click += new System.EventHandler(this.label_ChanID_Click);
            this.label_ChanID.DoubleClick += new System.EventHandler(this.label_ChanID_DoubleClick);
            // 
            // label_Inv
            // 
            this.label_Inv.BackColor = System.Drawing.Color.Black;
            this.label_Inv.Location = new System.Drawing.Point(3, 11);
            this.label_Inv.Name = "label_Inv";
            this.label_Inv.Size = new System.Drawing.Size(54, 2);
            this.label_Inv.TabIndex = 5;
            // 
            // checkBox_Select
            // 
            this.checkBox_Select.AutoSize = true;
            this.checkBox_Select.BackColor = System.Drawing.Color.Orange;
            this.checkBox_Select.Location = new System.Drawing.Point(2, 37);
            this.checkBox_Select.Name = "checkBox_Select";
            this.checkBox_Select.Size = new System.Drawing.Size(15, 14);
            this.checkBox_Select.TabIndex = 7;
            this.checkBox_Select.UseVisualStyleBackColor = false;
            this.checkBox_Select.CheckedChanged += new System.EventHandler(this.checkBox_Select_CheckedChanged);
            // 
            // label_Scale
            // 
            this.label_Scale.AutoSize = true;
            this.label_Scale.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Scale.ForeColor = System.Drawing.Color.Orange;
            this.label_Scale.Location = new System.Drawing.Point(63, 7);
            this.label_Scale.Name = "label_Scale";
            this.label_Scale.Size = new System.Drawing.Size(50, 17);
            this.label_Scale.TabIndex = 12;
            this.label_Scale.Text = "300mV";
            this.label_Scale.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_BwLimit
            // 
            this.label_BwLimit.AutoSize = true;
            this.label_BwLimit.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_BwLimit.ForeColor = System.Drawing.Color.Orange;
            this.label_BwLimit.Location = new System.Drawing.Point(120, 5);
            this.label_BwLimit.Name = "label_BwLimit";
            this.label_BwLimit.Size = new System.Drawing.Size(19, 19);
            this.label_BwLimit.TabIndex = 13;
            this.label_BwLimit.Text = "B";
            this.label_BwLimit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Impdence
            // 
            this.label_Impdence.AutoSize = true;
            this.label_Impdence.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Impdence.ForeColor = System.Drawing.Color.Orange;
            this.label_Impdence.Location = new System.Drawing.Point(62, 29);
            this.label_Impdence.Name = "label_Impdence";
            this.label_Impdence.Size = new System.Drawing.Size(21, 19);
            this.label_Impdence.TabIndex = 14;
            this.label_Impdence.Text = "Ω";
            // 
            // label_Offset
            // 
            this.label_Offset.AutoSize = true;
            this.label_Offset.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Offset.ForeColor = System.Drawing.Color.Orange;
            this.label_Offset.Location = new System.Drawing.Point(80, 30);
            this.label_Offset.Name = "label_Offset";
            this.label_Offset.Size = new System.Drawing.Size(59, 17);
            this.label_Offset.TabIndex = 15;
            this.label_Offset.Text = "+250mV";
            this.label_Offset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UCChanWaveView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label_Scale);
            this.Controls.Add(this.label_BwLimit);
            this.Controls.Add(this.label_Impdence);
            this.Controls.Add(this.label_Offset);
            this.Controls.Add(this.checkBox_Select);
            this.Controls.Add(this.label_Inv);
            this.Controls.Add(this.label_ChanID);
            this.Name = "UCChanWaveView";
            this.Size = new System.Drawing.Size(143, 53);
            this.Load += new System.EventHandler(this.UCChanWaveView_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UCChanWaveView_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_ChanID;
        private System.Windows.Forms.Label label_Inv;
        private System.Windows.Forms.CheckBox checkBox_Select;
        private System.Windows.Forms.Label label_Scale;
        private System.Windows.Forms.Label label_BwLimit;
        private System.Windows.Forms.Label label_Impdence;
        private System.Windows.Forms.Label label_Offset;
    }
}
