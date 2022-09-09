namespace saker_Winform.SubForm
{
    partial class Form_DeviceConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_DeviceConfig));
            this.flowLayoutPanel_DeviceConfig = new System.Windows.Forms.FlowLayoutPanel();
            this.panel_DevList = new System.Windows.Forms.Panel();
            this.btn_DeletDev = new System.Windows.Forms.Button();
            this.label_IncruList = new System.Windows.Forms.Label();
            this.btn_RegisterIncrument = new System.Windows.Forms.Button();
            this.panel_Right = new System.Windows.Forms.Panel();
            this.panel_OnlineList = new System.Windows.Forms.Panel();
            this.flowLayoutPanel_OnlineList = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label_OnlineIncList = new System.Windows.Forms.Label();
            this.btn_Search = new System.Windows.Forms.Button();
            this.flowLayoutPanel_RegDevList = new System.Windows.Forms.FlowLayoutPanel();
            this.userBaseNullDeviceInfo1 = new saker_Winform.UserControls.UCUserBaseNullDeviceInfo();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btn_Delet = new System.Windows.Forms.Button();
            this.label_RegisterIncList = new System.Windows.Forms.Label();
            this.panel_Buttom = new System.Windows.Forms.Panel();
            this.pictureBox_Device = new System.Windows.Forms.PictureBox();
            this.label_DeviceIP = new System.Windows.Forms.Label();
            this.label_DeviceSubName = new System.Windows.Forms.Label();
            this.label_FirmVersion = new System.Windows.Forms.Label();
            this.label_DevNumber = new System.Windows.Forms.Label();
            this.label_DevModel = new System.Windows.Forms.Label();
            this.label_DevSN = new System.Windows.Forms.Label();
            this.panel_DevList.SuspendLayout();
            this.panel_Right.SuspendLayout();
            this.panel_OnlineList.SuspendLayout();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel_RegDevList.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel_Buttom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Device)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel_DeviceConfig
            // 
            this.flowLayoutPanel_DeviceConfig.AllowDrop = true;
            this.flowLayoutPanel_DeviceConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.flowLayoutPanel_DeviceConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel_DeviceConfig.Location = new System.Drawing.Point(0, 33);
            this.flowLayoutPanel_DeviceConfig.Name = "flowLayoutPanel_DeviceConfig";
            this.flowLayoutPanel_DeviceConfig.Size = new System.Drawing.Size(598, 385);
            this.flowLayoutPanel_DeviceConfig.TabIndex = 5;
            // 
            // panel_DevList
            // 
            this.panel_DevList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.panel_DevList.Controls.Add(this.btn_DeletDev);
            this.panel_DevList.Controls.Add(this.label_IncruList);
            this.panel_DevList.Controls.Add(this.btn_RegisterIncrument);
            this.panel_DevList.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_DevList.Location = new System.Drawing.Point(0, 0);
            this.panel_DevList.Name = "panel_DevList";
            this.panel_DevList.Size = new System.Drawing.Size(598, 33);
            this.panel_DevList.TabIndex = 3;
            // 
            // btn_DeletDev
            // 
            this.btn_DeletDev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_DeletDev.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btn_DeletDev.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_DeletDev.ForeColor = System.Drawing.Color.White;
            this.btn_DeletDev.Location = new System.Drawing.Point(520, 3);
            this.btn_DeletDev.Name = "btn_DeletDev";
            this.btn_DeletDev.Size = new System.Drawing.Size(75, 27);
            this.btn_DeletDev.TabIndex = 1;
            this.btn_DeletDev.Text = "删除";
            this.btn_DeletDev.UseVisualStyleBackColor = false;
            this.btn_DeletDev.Click += new System.EventHandler(this.button_DeletDev_Click);
            // 
            // label_IncruList
            // 
            this.label_IncruList.AutoSize = true;
            this.label_IncruList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.label_IncruList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_IncruList.ForeColor = System.Drawing.Color.White;
            this.label_IncruList.Location = new System.Drawing.Point(5, 5);
            this.label_IncruList.Name = "label_IncruList";
            this.label_IncruList.Size = new System.Drawing.Size(74, 22);
            this.label_IncruList.TabIndex = 0;
            this.label_IncruList.Text = "仪器列表";
            // 
            // btn_RegisterIncrument
            // 
            this.btn_RegisterIncrument.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_RegisterIncrument.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.btn_RegisterIncrument.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btn_RegisterIncrument.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_RegisterIncrument.ForeColor = System.Drawing.Color.White;
            this.btn_RegisterIncrument.Location = new System.Drawing.Point(430, 3);
            this.btn_RegisterIncrument.Name = "btn_RegisterIncrument";
            this.btn_RegisterIncrument.Size = new System.Drawing.Size(75, 27);
            this.btn_RegisterIncrument.TabIndex = 1;
            this.btn_RegisterIncrument.Text = "注册仪器";
            this.btn_RegisterIncrument.UseVisualStyleBackColor = false;
            this.btn_RegisterIncrument.Click += new System.EventHandler(this.btn_RegisterIncrument_Click);
            // 
            // panel_Right
            // 
            this.panel_Right.AllowDrop = true;
            this.panel_Right.BackColor = System.Drawing.Color.Transparent;
            this.panel_Right.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Right.Controls.Add(this.panel_OnlineList);
            this.panel_Right.Controls.Add(this.flowLayoutPanel_RegDevList);
            this.panel_Right.Controls.Add(this.panel5);
            this.panel_Right.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_Right.Location = new System.Drawing.Point(598, 0);
            this.panel_Right.Name = "panel_Right";
            this.panel_Right.Size = new System.Drawing.Size(276, 418);
            this.panel_Right.TabIndex = 3;
            // 
            // panel_OnlineList
            // 
            this.panel_OnlineList.Controls.Add(this.flowLayoutPanel_OnlineList);
            this.panel_OnlineList.Controls.Add(this.panel2);
            this.panel_OnlineList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_OnlineList.Location = new System.Drawing.Point(0, 172);
            this.panel_OnlineList.Name = "panel_OnlineList";
            this.panel_OnlineList.Size = new System.Drawing.Size(274, 244);
            this.panel_OnlineList.TabIndex = 5;
            // 
            // flowLayoutPanel_OnlineList
            // 
            this.flowLayoutPanel_OnlineList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.flowLayoutPanel_OnlineList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel_OnlineList.Location = new System.Drawing.Point(0, 35);
            this.flowLayoutPanel_OnlineList.Name = "flowLayoutPanel_OnlineList";
            this.flowLayoutPanel_OnlineList.Size = new System.Drawing.Size(274, 209);
            this.flowLayoutPanel_OnlineList.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.panel2.Controls.Add(this.label_OnlineIncList);
            this.panel2.Controls.Add(this.btn_Search);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(274, 35);
            this.panel2.TabIndex = 3;
            // 
            // label_OnlineIncList
            // 
            this.label_OnlineIncList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_OnlineIncList.AutoSize = true;
            this.label_OnlineIncList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.label_OnlineIncList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_OnlineIncList.ForeColor = System.Drawing.Color.White;
            this.label_OnlineIncList.Location = new System.Drawing.Point(9, 7);
            this.label_OnlineIncList.Name = "label_OnlineIncList";
            this.label_OnlineIncList.Size = new System.Drawing.Size(106, 22);
            this.label_OnlineIncList.TabIndex = 0;
            this.label_OnlineIncList.Text = "在线仪器列表";
            // 
            // btn_Search
            // 
            this.btn_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.btn_Search.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btn_Search.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Search.ForeColor = System.Drawing.Color.White;
            this.btn_Search.Location = new System.Drawing.Point(198, 5);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(75, 27);
            this.btn_Search.TabIndex = 1;
            this.btn_Search.Text = "检索";
            this.btn_Search.UseVisualStyleBackColor = false;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // flowLayoutPanel_RegDevList
            // 
            this.flowLayoutPanel_RegDevList.AllowDrop = true;
            this.flowLayoutPanel_RegDevList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.flowLayoutPanel_RegDevList.Controls.Add(this.userBaseNullDeviceInfo1);
            this.flowLayoutPanel_RegDevList.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel_RegDevList.Location = new System.Drawing.Point(0, 32);
            this.flowLayoutPanel_RegDevList.Name = "flowLayoutPanel_RegDevList";
            this.flowLayoutPanel_RegDevList.Size = new System.Drawing.Size(274, 140);
            this.flowLayoutPanel_RegDevList.TabIndex = 6;
            // 
            // userBaseNullDeviceInfo1
            // 
            this.userBaseNullDeviceInfo1.AllowDrop = true;
            this.userBaseNullDeviceInfo1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userBaseNullDeviceInfo1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.userBaseNullDeviceInfo1.Location = new System.Drawing.Point(3, 3);
            this.userBaseNullDeviceInfo1.m_bLink = false;
            this.userBaseNullDeviceInfo1.m_bSelect = false;
            this.userBaseNullDeviceInfo1.m_bStatus = false;
            this.userBaseNullDeviceInfo1.m_checkselectColor = System.Drawing.Color.Empty;
            this.userBaseNullDeviceInfo1.m_isSelect = false;
            this.userBaseNullDeviceInfo1.m_selectColor = System.Drawing.Color.Empty;
            this.userBaseNullDeviceInfo1.m_strDevIP = null;
            this.userBaseNullDeviceInfo1.m_strDevModel = "DS8000-R";
            this.userBaseNullDeviceInfo1.m_strDevNumber = null;
            this.userBaseNullDeviceInfo1.m_strDevSN = null;
            this.userBaseNullDeviceInfo1.m_strDevSubName = null;
            this.userBaseNullDeviceInfo1.m_strFirmVersion = null;
            this.userBaseNullDeviceInfo1.m_strMac = null;
            this.userBaseNullDeviceInfo1.m_strRackNumber = null;
            this.userBaseNullDeviceInfo1.m_unselectColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(65)))), ((int)(((byte)(66)))));
            this.userBaseNullDeviceInfo1.Name = "userBaseNullDeviceInfo1";
            this.userBaseNullDeviceInfo1.Size = new System.Drawing.Size(86, 109);
            this.userBaseNullDeviceInfo1.TabIndex = 0;
            this.userBaseNullDeviceInfo1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.userBaseNullDeviceInfo1_MouseDown);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.panel5.Controls.Add(this.btn_Delet);
            this.panel5.Controls.Add(this.label_RegisterIncList);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(274, 32);
            this.panel5.TabIndex = 4;
            // 
            // btn_Delet
            // 
            this.btn_Delet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Delet.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btn_Delet.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Delet.ForeColor = System.Drawing.Color.White;
            this.btn_Delet.Location = new System.Drawing.Point(198, 2);
            this.btn_Delet.Name = "btn_Delet";
            this.btn_Delet.Size = new System.Drawing.Size(75, 27);
            this.btn_Delet.TabIndex = 1;
            this.btn_Delet.Text = "删除";
            this.btn_Delet.UseVisualStyleBackColor = false;
            this.btn_Delet.Click += new System.EventHandler(this.btn_Delet_Click);
            // 
            // label_RegisterIncList
            // 
            this.label_RegisterIncList.AutoSize = true;
            this.label_RegisterIncList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.label_RegisterIncList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_RegisterIncList.ForeColor = System.Drawing.Color.White;
            this.label_RegisterIncList.Location = new System.Drawing.Point(1, 4);
            this.label_RegisterIncList.Name = "label_RegisterIncList";
            this.label_RegisterIncList.Size = new System.Drawing.Size(106, 22);
            this.label_RegisterIncList.TabIndex = 0;
            this.label_RegisterIncList.Text = "注册仪器列表";
            // 
            // panel_Buttom
            // 
            this.panel_Buttom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.panel_Buttom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Buttom.Controls.Add(this.pictureBox_Device);
            this.panel_Buttom.Controls.Add(this.label_DeviceIP);
            this.panel_Buttom.Controls.Add(this.label_DeviceSubName);
            this.panel_Buttom.Controls.Add(this.label_FirmVersion);
            this.panel_Buttom.Controls.Add(this.label_DevNumber);
            this.panel_Buttom.Controls.Add(this.label_DevModel);
            this.panel_Buttom.Controls.Add(this.label_DevSN);
            this.panel_Buttom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_Buttom.Location = new System.Drawing.Point(0, 418);
            this.panel_Buttom.Name = "panel_Buttom";
            this.panel_Buttom.Size = new System.Drawing.Size(874, 117);
            this.panel_Buttom.TabIndex = 4;
            // 
            // pictureBox_Device
            // 
            this.pictureBox_Device.Image = global::saker_Winform.Properties.Resources.Device;
            this.pictureBox_Device.Location = new System.Drawing.Point(39, 22);
            this.pictureBox_Device.Name = "pictureBox_Device";
            this.pictureBox_Device.Size = new System.Drawing.Size(262, 74);
            this.pictureBox_Device.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Device.TabIndex = 0;
            this.pictureBox_Device.TabStop = false;
            // 
            // label_DeviceIP
            // 
            this.label_DeviceIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_DeviceIP.AutoSize = true;
            this.label_DeviceIP.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_DeviceIP.ForeColor = System.Drawing.Color.Black;
            this.label_DeviceIP.Location = new System.Drawing.Point(622, 78);
            this.label_DeviceIP.Name = "label_DeviceIP";
            this.label_DeviceIP.Size = new System.Drawing.Size(60, 17);
            this.label_DeviceIP.TabIndex = 1;
            this.label_DeviceIP.Text = "IP 地址：";
            // 
            // label_DeviceSubName
            // 
            this.label_DeviceSubName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_DeviceSubName.AutoSize = true;
            this.label_DeviceSubName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_DeviceSubName.ForeColor = System.Drawing.Color.Black;
            this.label_DeviceSubName.Location = new System.Drawing.Point(621, 53);
            this.label_DeviceSubName.Name = "label_DeviceSubName";
            this.label_DeviceSubName.Size = new System.Drawing.Size(68, 17);
            this.label_DeviceSubName.TabIndex = 1;
            this.label_DeviceSubName.Text = "设备别名：";
            // 
            // label_FirmVersion
            // 
            this.label_FirmVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_FirmVersion.AutoSize = true;
            this.label_FirmVersion.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_FirmVersion.ForeColor = System.Drawing.Color.Black;
            this.label_FirmVersion.Location = new System.Drawing.Point(375, 79);
            this.label_FirmVersion.Name = "label_FirmVersion";
            this.label_FirmVersion.Size = new System.Drawing.Size(68, 17);
            this.label_FirmVersion.TabIndex = 1;
            this.label_FirmVersion.Text = "固件版本：";
            // 
            // label_DevNumber
            // 
            this.label_DevNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_DevNumber.AutoSize = true;
            this.label_DevNumber.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_DevNumber.ForeColor = System.Drawing.Color.Black;
            this.label_DevNumber.Location = new System.Drawing.Point(622, 27);
            this.label_DevNumber.Name = "label_DevNumber";
            this.label_DevNumber.Size = new System.Drawing.Size(68, 17);
            this.label_DevNumber.TabIndex = 1;
            this.label_DevNumber.Text = "机器编号：";
            // 
            // label_DevModel
            // 
            this.label_DevModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_DevModel.AutoSize = true;
            this.label_DevModel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_DevModel.ForeColor = System.Drawing.Color.Black;
            this.label_DevModel.Location = new System.Drawing.Point(374, 54);
            this.label_DevModel.Name = "label_DevModel";
            this.label_DevModel.Size = new System.Drawing.Size(68, 17);
            this.label_DevModel.TabIndex = 1;
            this.label_DevModel.Text = "仪器型号：";
            // 
            // label_DevSN
            // 
            this.label_DevSN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_DevSN.AutoSize = true;
            this.label_DevSN.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_DevSN.ForeColor = System.Drawing.Color.Black;
            this.label_DevSN.Location = new System.Drawing.Point(373, 29);
            this.label_DevSN.Name = "label_DevSN";
            this.label_DevSN.Size = new System.Drawing.Size(80, 17);
            this.label_DevSN.TabIndex = 1;
            this.label_DevSN.Text = "仪器序列号：";
            // 
            // Form_DeviceConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 535);
            this.Controls.Add(this.flowLayoutPanel_DeviceConfig);
            this.Controls.Add(this.panel_DevList);
            this.Controls.Add(this.panel_Right);
            this.Controls.Add(this.panel_Buttom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_DeviceConfig";
            this.Text = "Form_DeviceConfig";
            this.Load += new System.EventHandler(this.Form_DeviceConfig_Load);
            this.SizeChanged += new System.EventHandler(this.Form_DeviceConfig_SizeChanged);
            this.panel_DevList.ResumeLayout(false);
            this.panel_DevList.PerformLayout();
            this.panel_Right.ResumeLayout(false);
            this.panel_OnlineList.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.flowLayoutPanel_RegDevList.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel_Buttom.ResumeLayout(false);
            this.panel_Buttom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Device)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label_IncruList;
        private System.Windows.Forms.Button btn_RegisterIncrument;
        private System.Windows.Forms.Panel panel_DevList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_Delet;
        private System.Windows.Forms.Label label_RegisterIncList;
        private System.Windows.Forms.Panel panel_Right;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.Label label_OnlineIncList;
        private System.Windows.Forms.Panel panel_Buttom;
        private System.Windows.Forms.Panel panel_OnlineList;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_DeviceConfig;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_RegDevList;
        private System.Windows.Forms.Button btn_DeletDev;
        private System.Windows.Forms.Label label_DeviceIP;
        private System.Windows.Forms.Label label_DeviceSubName;
        private System.Windows.Forms.Label label_FirmVersion;
        private System.Windows.Forms.Label label_DevNumber;
        private System.Windows.Forms.Label label_DevModel;
        private System.Windows.Forms.Label label_DevSN;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_OnlineList;
        private UserControls.UCUserBaseNullDeviceInfo userBaseNullDeviceInfo1;
        private System.Windows.Forms.PictureBox pictureBox_Device;
    }
}