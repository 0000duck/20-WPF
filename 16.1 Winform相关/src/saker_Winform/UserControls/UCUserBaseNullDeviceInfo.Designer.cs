namespace saker_Winform.UserControls
{
    partial class UCUserBaseNullDeviceInfo
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
            this.components = new System.ComponentModel.Container();
            this.label_IP = new System.Windows.Forms.Label();
            this.label_Device = new System.Windows.Forms.Label();
            this.panel_In = new System.Windows.Forms.Panel();
            this.label_Modle = new System.Windows.Forms.Label();
            this.label_Num = new System.Windows.Forms.Label();
            this.iconPictureBox_Status = new FontAwesome.Sharp.IconPictureBox();
            this.iconPictureBox_Plus = new FontAwesome.Sharp.IconPictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_Register = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Modif = new System.Windows.Forms.ToolStripMenuItem();
            this.iconMenuItem_delDevice = new FontAwesome.Sharp.IconMenuItem();
            this.panel_In.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox_Status)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox_Plus)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_IP
            // 
            this.label_IP.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_IP.Location = new System.Drawing.Point(2, 94);
            this.label_IP.Name = "label_IP";
            this.label_IP.Size = new System.Drawing.Size(73, 22);
            this.label_IP.TabIndex = 2;
            // 
            // label_Device
            // 
            this.label_Device.AutoSize = true;
            this.label_Device.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Device.Location = new System.Drawing.Point(9, 35);
            this.label_Device.Name = "label_Device";
            this.label_Device.Size = new System.Drawing.Size(0, 16);
            this.label_Device.TabIndex = 1;
            // 
            // panel_In
            // 
            this.panel_In.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(209)))), ((int)(((byte)(245)))));
            this.panel_In.Controls.Add(this.label_Modle);
            this.panel_In.Controls.Add(this.label_Num);
            this.panel_In.Location = new System.Drawing.Point(0, -1);
            this.panel_In.Name = "panel_In";
            this.panel_In.Size = new System.Drawing.Size(86, 32);
            this.panel_In.TabIndex = 0;
            this.panel_In.Click += new System.EventHandler(this.panel_In_Click);
            // 
            // label_Modle
            // 
            this.label_Modle.AutoSize = true;
            this.label_Modle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Modle.Location = new System.Drawing.Point(4, 8);
            this.label_Modle.Name = "label_Modle";
            this.label_Modle.Size = new System.Drawing.Size(65, 17);
            this.label_Modle.TabIndex = 1;
            this.label_Modle.Text = "DS8000-R";
            // 
            // label_Num
            // 
            this.label_Num.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label_Num.AutoSize = true;
            this.label_Num.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(209)))), ((int)(((byte)(245)))));
            this.label_Num.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Num.ForeColor = System.Drawing.Color.Black;
            this.label_Num.Location = new System.Drawing.Point(25, 6);
            this.label_Num.Name = "label_Num";
            this.label_Num.Size = new System.Drawing.Size(33, 20);
            this.label_Num.TabIndex = 0;
            this.label_Num.Text = "001";
            this.label_Num.Click += new System.EventHandler(this.label_Num_Click);
            // 
            // iconPictureBox_Status
            // 
            this.iconPictureBox_Status.BackColor = System.Drawing.SystemColors.Control;
            this.iconPictureBox_Status.ForeColor = System.Drawing.Color.Lime;
            this.iconPictureBox_Status.IconChar = FontAwesome.Sharp.IconChar.Link;
            this.iconPictureBox_Status.IconColor = System.Drawing.Color.Lime;
            this.iconPictureBox_Status.IconSize = 18;
            this.iconPictureBox_Status.Location = new System.Drawing.Point(66, 52);
            this.iconPictureBox_Status.Name = "iconPictureBox_Status";
            this.iconPictureBox_Status.Size = new System.Drawing.Size(18, 18);
            this.iconPictureBox_Status.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.iconPictureBox_Status.TabIndex = 6;
            this.iconPictureBox_Status.TabStop = false;
            this.iconPictureBox_Status.Visible = false;
            // 
            // iconPictureBox_Plus
            // 
            this.iconPictureBox_Plus.BackColor = System.Drawing.SystemColors.Control;
            this.iconPictureBox_Plus.BackgroundImage = global::saker_Winform.Properties.Resources.ControlDevice;
            this.iconPictureBox_Plus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.iconPictureBox_Plus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconPictureBox_Plus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(209)))), ((int)(((byte)(245)))));
            this.iconPictureBox_Plus.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconPictureBox_Plus.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(209)))), ((int)(((byte)(245)))));
            this.iconPictureBox_Plus.IconSize = 39;
            this.iconPictureBox_Plus.Location = new System.Drawing.Point(21, 52);
            this.iconPictureBox_Plus.Name = "iconPictureBox_Plus";
            this.iconPictureBox_Plus.Size = new System.Drawing.Size(39, 39);
            this.iconPictureBox_Plus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.iconPictureBox_Plus.TabIndex = 4;
            this.iconPictureBox_Plus.TabStop = false;
            this.iconPictureBox_Plus.Click += new System.EventHandler(this.iconPictureBox_Plus_Click);
            this.iconPictureBox_Plus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.iconPictureBox_Plus_MouseDown);
            this.iconPictureBox_Plus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.iconPictureBox_Plus_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Register,
            this.ToolStripMenuItem_Modif,
            this.iconMenuItem_delDevice});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 92);
            // 
            // ToolStripMenuItem_Register
            // 
            this.ToolStripMenuItem_Register.Image = global::saker_Winform.Properties.Resources.Popup;
            this.ToolStripMenuItem_Register.Name = "ToolStripMenuItem_Register";
            this.ToolStripMenuItem_Register.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem_Register.Text = "注册仪器";
            this.ToolStripMenuItem_Register.Click += new System.EventHandler(this.ToolStripMenuItem_Register_Click);
            // 
            // ToolStripMenuItem_Modif
            // 
            this.ToolStripMenuItem_Modif.Image = global::saker_Winform.Properties.Resources.全选1;
            this.ToolStripMenuItem_Modif.Name = "ToolStripMenuItem_Modif";
            this.ToolStripMenuItem_Modif.Size = new System.Drawing.Size(148, 22);
            this.ToolStripMenuItem_Modif.Text = "修改仪器信息";
            this.ToolStripMenuItem_Modif.Click += new System.EventHandler(this.ToolStripMenuItem_Modif_Click);
            // 
            // iconMenuItem_delDevice
            // 
            this.iconMenuItem_delDevice.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconMenuItem_delDevice.IconChar = FontAwesome.Sharp.IconChar.Times;
            this.iconMenuItem_delDevice.IconColor = System.Drawing.SystemColors.Highlight;
            this.iconMenuItem_delDevice.IconSize = 16;
            this.iconMenuItem_delDevice.Name = "iconMenuItem_delDevice";
            this.iconMenuItem_delDevice.Rotation = 0D;
            this.iconMenuItem_delDevice.Size = new System.Drawing.Size(180, 22);
            this.iconMenuItem_delDevice.Text = "删除仪器";
            this.iconMenuItem_delDevice.Click += new System.EventHandler(this.iconMenuItem_delDevice_Click);
            // 
            // UCUserBaseNullDeviceInfo
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.iconPictureBox_Status);
            this.Controls.Add(this.label_IP);
            this.Controls.Add(this.label_Device);
            this.Controls.Add(this.panel_In);
            this.Controls.Add(this.iconPictureBox_Plus);
            this.Name = "UCUserBaseNullDeviceInfo";
            this.Size = new System.Drawing.Size(86, 109);
            this.Load += new System.EventHandler(this.UserBaseNullDeviceInfo_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UserBaseNullDeviceInfo_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UCUserBaseNullDeviceInfo_MouseDown);
            this.panel_In.ResumeLayout(false);
            this.panel_In.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox_Status)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox_Plus)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel_In;
        private System.Windows.Forms.Label label_Num;
        private System.Windows.Forms.Label label_Device;
        private System.Windows.Forms.Label label_IP;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox_Plus;
        private System.Windows.Forms.Label label_Modle;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox_Status;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Register;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Modif;
        private FontAwesome.Sharp.IconMenuItem iconMenuItem_delDevice;
    }
}
