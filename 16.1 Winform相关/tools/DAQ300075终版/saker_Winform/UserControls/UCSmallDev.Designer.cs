namespace saker_Winform.UserControls
{
    partial class UCSmallDev
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
            this.panel_out = new System.Windows.Forms.Panel();
            this.label_OnlineDev = new System.Windows.Forms.Label();
            this.pictureBox_OnlineDev = new System.Windows.Forms.PictureBox();
            this.panel_out.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_OnlineDev)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_out
            // 
            this.panel_out.Controls.Add(this.label_OnlineDev);
            this.panel_out.Controls.Add(this.pictureBox_OnlineDev);
            this.panel_out.Location = new System.Drawing.Point(1, 1);
            this.panel_out.Name = "panel_out";
            this.panel_out.Size = new System.Drawing.Size(75, 76);
            this.panel_out.TabIndex = 0;
            this.panel_out.Click += new System.EventHandler(this.panel_out_Click);
            this.panel_out.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_out_Paint);
            this.panel_out.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_out_MouseDown);
            // 
            // label_OnlineDev
            // 
            this.label_OnlineDev.AutoSize = true;
            this.label_OnlineDev.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_OnlineDev.Location = new System.Drawing.Point(2, 58);
            this.label_OnlineDev.Name = "label_OnlineDev";
            this.label_OnlineDev.Size = new System.Drawing.Size(52, 15);
            this.label_OnlineDev.TabIndex = 1;
            this.label_OnlineDev.Text = "192.1.1.1";
            this.label_OnlineDev.Click += new System.EventHandler(this.label_OnlineDev_Click);
            // 
            // pictureBox_OnlineDev
            // 
            this.pictureBox_OnlineDev.Image = global::saker_Winform.Properties.Resources.ControlDevice;
            this.pictureBox_OnlineDev.Location = new System.Drawing.Point(16, 14);
            this.pictureBox_OnlineDev.Name = "pictureBox_OnlineDev";
            this.pictureBox_OnlineDev.Size = new System.Drawing.Size(43, 41);
            this.pictureBox_OnlineDev.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_OnlineDev.TabIndex = 0;
            this.pictureBox_OnlineDev.TabStop = false;
            this.pictureBox_OnlineDev.Click += new System.EventHandler(this.pictureBox_OnlineDev_Click);
            this.pictureBox_OnlineDev.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_OnlineDev_MouseDown);
            // 
            // UCSmallDev
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_out);
            this.Name = "UCSmallDev";
            this.Size = new System.Drawing.Size(78, 79);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UCSmallDev_MouseDown);
            this.panel_out.ResumeLayout(false);
            this.panel_out.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_OnlineDev)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_out;
        private System.Windows.Forms.PictureBox pictureBox_OnlineDev;
        private System.Windows.Forms.Label label_OnlineDev;
    }
}
