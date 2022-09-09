namespace saker_Winform.UserControls
{
    partial class UCChanInfo
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
            this.label_Name = new System.Windows.Forms.Label();
            this.label_50Ohm = new System.Windows.Forms.Label();
            this.label_1M = new System.Windows.Forms.Label();
            this.iconPictureBox_State = new FontAwesome.Sharp.IconPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox_State)).BeginInit();
            this.SuspendLayout();
            // 
            // label_Name
            // 
            this.label_Name.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Name.AutoSize = true;
            this.label_Name.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Name.Location = new System.Drawing.Point(3, 31);
            this.label_Name.Name = "label_Name";
            this.label_Name.Size = new System.Drawing.Size(83, 17);
            this.label_Name.TabIndex = 9;
            this.label_Name.Text = "CH1/Tag001";
            // 
            // label_50Ohm
            // 
            this.label_50Ohm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_50Ohm.AutoSize = true;
            this.label_50Ohm.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_50Ohm.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label_50Ohm.Location = new System.Drawing.Point(51, 2);
            this.label_50Ohm.Name = "label_50Ohm";
            this.label_50Ohm.Size = new System.Drawing.Size(36, 17);
            this.label_50Ohm.TabIndex = 10;
            this.label_50Ohm.Text = " 50Ω";
            // 
            // label_1M
            // 
            this.label_1M.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_1M.AutoSize = true;
            this.label_1M.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_1M.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label_1M.Location = new System.Drawing.Point(55, 16);
            this.label_1M.Name = "label_1M";
            this.label_1M.Size = new System.Drawing.Size(31, 17);
            this.label_1M.TabIndex = 11;
            this.label_1M.Text = "1 M";
            // 
            // iconPictureBox_State
            // 
            this.iconPictureBox_State.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox_State.ForeColor = System.Drawing.SystemColors.ControlText;
            this.iconPictureBox_State.IconChar = FontAwesome.Sharp.IconChar.Circle;
            this.iconPictureBox_State.IconColor = System.Drawing.SystemColors.ControlText;
            this.iconPictureBox_State.IconSize = 28;
            this.iconPictureBox_State.Location = new System.Drawing.Point(20, 0);
            this.iconPictureBox_State.Name = "iconPictureBox_State";
            this.iconPictureBox_State.Size = new System.Drawing.Size(28, 28);
            this.iconPictureBox_State.TabIndex = 8;
            this.iconPictureBox_State.TabStop = false;
            // 
            // UCChanInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_1M);
            this.Controls.Add(this.label_50Ohm);
            this.Controls.Add(this.label_Name);
            this.Controls.Add(this.iconPictureBox_State);
            this.Name = "UCChanInfo";
            this.Size = new System.Drawing.Size(92, 52);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox_State)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FontAwesome.Sharp.IconPictureBox iconPictureBox_State;
        private System.Windows.Forms.Label label_Name;
        private System.Windows.Forms.Label label_50Ohm;
        private System.Windows.Forms.Label label_1M;
    }
}
