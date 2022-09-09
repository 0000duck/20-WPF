namespace saker_Winform.UserControls
{
    partial class UCHistoryComp
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
            this.checkBox_Select = new System.Windows.Forms.CheckBox();
            this.label_ChanID = new System.Windows.Forms.Label();
            this.label_MeasType = new System.Windows.Forms.Label();
            this.label_TestTim = new System.Windows.Forms.Label();
            this.label_DevName = new System.Windows.Forms.Label();
            this.label_ChanACtual = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkBox_Select
            // 
            this.checkBox_Select.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_Select.AutoSize = true;
            this.checkBox_Select.BackColor = System.Drawing.Color.Orange;
            this.checkBox_Select.Location = new System.Drawing.Point(4, 67);
            this.checkBox_Select.Name = "checkBox_Select";
            this.checkBox_Select.Size = new System.Drawing.Size(15, 14);
            this.checkBox_Select.TabIndex = 18;
            this.checkBox_Select.UseVisualStyleBackColor = false;
            this.checkBox_Select.CheckedChanged += new System.EventHandler(this.checkBox_Select_CheckedChanged);
            // 
            // label_ChanID
            // 
            this.label_ChanID.BackColor = System.Drawing.Color.Orange;
            this.label_ChanID.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_ChanID.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_ChanID.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ChanID.Location = new System.Drawing.Point(0, 0);
            this.label_ChanID.Name = "label_ChanID";
            this.label_ChanID.Size = new System.Drawing.Size(72, 85);
            this.label_ChanID.TabIndex = 16;
            this.label_ChanID.Text = "Tag001";
            this.label_ChanID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_ChanID.Click += new System.EventHandler(this.label_ChanID_Click);
            // 
            // label_MeasType
            // 
            this.label_MeasType.AutoSize = true;
            this.label_MeasType.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_MeasType.ForeColor = System.Drawing.Color.Orange;
            this.label_MeasType.Location = new System.Drawing.Point(75, 5);
            this.label_MeasType.Name = "label_MeasType";
            this.label_MeasType.Size = new System.Drawing.Size(76, 17);
            this.label_MeasType.TabIndex = 19;
            this.label_MeasType.Text = "测量类型:{0}";
            // 
            // label_TestTim
            // 
            this.label_TestTim.AutoSize = true;
            this.label_TestTim.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_TestTim.ForeColor = System.Drawing.Color.Orange;
            this.label_TestTim.Location = new System.Drawing.Point(75, 62);
            this.label_TestTim.Name = "label_TestTim";
            this.label_TestTim.Size = new System.Drawing.Size(76, 17);
            this.label_TestTim.TabIndex = 20;
            this.label_TestTim.Text = "采集时间:{0}";
            // 
            // label_DevName
            // 
            this.label_DevName.AutoSize = true;
            this.label_DevName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_DevName.ForeColor = System.Drawing.Color.Orange;
            this.label_DevName.Location = new System.Drawing.Point(75, 24);
            this.label_DevName.Name = "label_DevName";
            this.label_DevName.Size = new System.Drawing.Size(64, 17);
            this.label_DevName.TabIndex = 21;
            this.label_DevName.Text = "设备名:{0}";
            // 
            // label_ChanACtual
            // 
            this.label_ChanACtual.AutoSize = true;
            this.label_ChanACtual.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ChanACtual.ForeColor = System.Drawing.Color.Orange;
            this.label_ChanACtual.Location = new System.Drawing.Point(75, 43);
            this.label_ChanACtual.Name = "label_ChanACtual";
            this.label_ChanACtual.Size = new System.Drawing.Size(113, 17);
            this.label_ChanACtual.TabIndex = 22;
            this.label_ChanACtual.Text = "实际通道:CHAN{0}";
            // 
            // UCHistoryComp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.label_ChanACtual);
            this.Controls.Add(this.label_DevName);
            this.Controls.Add(this.label_TestTim);
            this.Controls.Add(this.label_MeasType);
            this.Controls.Add(this.checkBox_Select);
            this.Controls.Add(this.label_ChanID);
            this.Name = "UCHistoryComp";
            this.Size = new System.Drawing.Size(243, 85);
            this.Load += new System.EventHandler(this.UCHistoryComp_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UCHistoryComp_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_Select;
        private System.Windows.Forms.Label label_ChanID;
        private System.Windows.Forms.Label label_MeasType;
        private System.Windows.Forms.Label label_TestTim;
        private System.Windows.Forms.Label label_DevName;
        private System.Windows.Forms.Label label_ChanACtual;
    }
}
