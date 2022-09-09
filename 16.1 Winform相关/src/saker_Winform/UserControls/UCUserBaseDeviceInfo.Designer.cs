namespace WindowsFormsApplication1
{
    partial class UserControl1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_Num = new System.Windows.Forms.Label();
            this.label_Device = new System.Windows.Forms.Label();
            this.label_IP = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DodgerBlue;
            this.panel1.Controls.Add(this.label_Num);
            this.panel1.Location = new System.Drawing.Point(4, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(82, 32);
            this.panel1.TabIndex = 0;
            this.panel1.Click += new System.EventHandler(this.panel1_Click);
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label_Num
            // 
            this.label_Num.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label_Num.AutoSize = true;
            this.label_Num.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Num.ForeColor = System.Drawing.Color.Black;
            this.label_Num.Location = new System.Drawing.Point(21, 7);
            this.label_Num.Name = "label_Num";
            this.label_Num.Size = new System.Drawing.Size(33, 20);
            this.label_Num.TabIndex = 0;
            this.label_Num.Text = "001";
            // 
            // label_Device
            // 
            this.label_Device.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label_Device.AutoSize = true;
            this.label_Device.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Device.Location = new System.Drawing.Point(7, 34);
            this.label_Device.Name = "label_Device";
            this.label_Device.Size = new System.Drawing.Size(75, 17);
            this.label_Device.TabIndex = 1;
            this.label_Device.Text = "Device_001";
            this.label_Device.Click += new System.EventHandler(this.label_Device_Click);
            // 
            // label_IP
            // 
            this.label_IP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label_IP.AutoSize = true;
            this.label_IP.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_IP.Location = new System.Drawing.Point(5, 91);
            this.label_IP.Name = "label_IP";
            this.label_IP.Size = new System.Drawing.Size(73, 17);
            this.label_IP.TabIndex = 2;
            this.label_IP.Text = "192.168.0.1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.label_Device);
            this.panel2.Controls.Add(this.label_IP);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(88, 113);
            this.panel2.TabIndex = 4;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::saker_Winform.Properties.Resources.ControlDevice1;
            this.pictureBox1.Location = new System.Drawing.Point(23, 51);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(39, 39);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel2);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(88, 113);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_Num;
        private System.Windows.Forms.Label label_Device;
        private System.Windows.Forms.Label label_IP;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
    }
}
