namespace saker_Winform.SubForm
{
    partial class Form_RegisterInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_RegisterInfo));
            this.label1_DeviceInfo = new System.Windows.Forms.Label();
            this.label_DeviceReg = new System.Windows.Forms.Label();
            this.label2_DeviceOtherName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_DeviceOtherName = new System.Windows.Forms.TextBox();
            this.textBox_DeviceIP = new System.Windows.Forms.TextBox();
            this.comboBox_DeviceModel = new System.Windows.Forms.ComboBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.label_SN = new System.Windows.Forms.Label();
            this.textBox_SN = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_Communication = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1_DeviceInfo
            // 
            this.label1_DeviceInfo.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1_DeviceInfo.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1_DeviceInfo.Location = new System.Drawing.Point(1, 0);
            this.label1_DeviceInfo.Name = "label1_DeviceInfo";
            this.label1_DeviceInfo.Size = new System.Drawing.Size(65, 20);
            this.label1_DeviceInfo.TabIndex = 0;
            this.label1_DeviceInfo.Text = "设备信息";
            // 
            // label_DeviceReg
            // 
            this.label_DeviceReg.BackColor = System.Drawing.Color.DarkGray;
            this.label_DeviceReg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_DeviceReg.Location = new System.Drawing.Point(0, 22);
            this.label_DeviceReg.Name = "label_DeviceReg";
            this.label_DeviceReg.Size = new System.Drawing.Size(316, 2);
            this.label_DeviceReg.TabIndex = 2;
            // 
            // label2_DeviceOtherName
            // 
            this.label2_DeviceOtherName.AutoSize = true;
            this.label2_DeviceOtherName.BackColor = System.Drawing.SystemColors.Control;
            this.label2_DeviceOtherName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2_DeviceOtherName.ForeColor = System.Drawing.Color.Black;
            this.label2_DeviceOtherName.Location = new System.Drawing.Point(33, 36);
            this.label2_DeviceOtherName.Name = "label2_DeviceOtherName";
            this.label2_DeviceOtherName.Size = new System.Drawing.Size(68, 17);
            this.label2_DeviceOtherName.TabIndex = 3;
            this.label2_DeviceOtherName.Text = "设备别名：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(33, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "型      号：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(33, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "设备 IP：";
            // 
            // textBox_DeviceOtherName
            // 
            this.textBox_DeviceOtherName.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_DeviceOtherName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_DeviceOtherName.Location = new System.Drawing.Point(111, 36);
            this.textBox_DeviceOtherName.Name = "textBox_DeviceOtherName";
            this.textBox_DeviceOtherName.Size = new System.Drawing.Size(119, 21);
            this.textBox_DeviceOtherName.TabIndex = 4;
            // 
            // textBox_DeviceIP
            // 
            this.textBox_DeviceIP.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_DeviceIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_DeviceIP.Location = new System.Drawing.Point(111, 108);
            this.textBox_DeviceIP.Name = "textBox_DeviceIP";
            this.textBox_DeviceIP.Size = new System.Drawing.Size(119, 21);
            this.textBox_DeviceIP.TabIndex = 4;
            // 
            // comboBox_DeviceModel
            // 
            this.comboBox_DeviceModel.AllowDrop = true;
            this.comboBox_DeviceModel.BackColor = System.Drawing.SystemColors.Control;
            this.comboBox_DeviceModel.FormattingEnabled = true;
            this.comboBox_DeviceModel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBox_DeviceModel.Items.AddRange(new object[] {
            "DS8000-R"});
            this.comboBox_DeviceModel.Location = new System.Drawing.Point(111, 73);
            this.comboBox_DeviceModel.Name = "comboBox_DeviceModel";
            this.comboBox_DeviceModel.Size = new System.Drawing.Size(119, 20);
            this.comboBox_DeviceModel.TabIndex = 5;
            // 
            // button_OK
            // 
            this.button_OK.BackColor = System.Drawing.SystemColors.Control;
            this.button_OK.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_OK.Location = new System.Drawing.Point(28, 221);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 6;
            this.button_OK.Text = "确定";
            this.button_OK.UseVisualStyleBackColor = false;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.BackColor = System.Drawing.SystemColors.Control;
            this.button_Cancel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Cancel.Location = new System.Drawing.Point(160, 219);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 6;
            this.button_Cancel.Text = "取消";
            this.button_Cancel.UseVisualStyleBackColor = false;
            this.button_Cancel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button1_Cancel_MouseClick);
            // 
            // label_SN
            // 
            this.label_SN.AutoSize = true;
            this.label_SN.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_SN.ForeColor = System.Drawing.Color.Black;
            this.label_SN.Location = new System.Drawing.Point(34, 146);
            this.label_SN.Name = "label_SN";
            this.label_SN.Size = new System.Drawing.Size(65, 17);
            this.label_SN.TabIndex = 3;
            this.label_SN.Text = "设备 SN：";
            // 
            // textBox_SN
            // 
            this.textBox_SN.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_SN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_SN.Location = new System.Drawing.Point(111, 146);
            this.textBox_SN.Name = "textBox_SN";
            this.textBox_SN.Size = new System.Drawing.Size(119, 21);
            this.textBox_SN.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(34, 184);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "通讯接口：";
            // 
            // comboBox_Communication
            // 
            this.comboBox_Communication.AllowDrop = true;
            this.comboBox_Communication.BackColor = System.Drawing.SystemColors.Control;
            this.comboBox_Communication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Communication.FormattingEnabled = true;
            this.comboBox_Communication.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBox_Communication.Items.AddRange(new object[] {
            "千兆"});
            this.comboBox_Communication.Location = new System.Drawing.Point(111, 184);
            this.comboBox_Communication.Name = "comboBox_Communication";
            this.comboBox_Communication.Size = new System.Drawing.Size(119, 20);
            this.comboBox_Communication.TabIndex = 18;
            // 
            // Form_RegisterInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(263, 255);
            this.ControlBox = false;
            this.Controls.Add(this.comboBox_Communication);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.comboBox_DeviceModel);
            this.Controls.Add(this.textBox_SN);
            this.Controls.Add(this.textBox_DeviceIP);
            this.Controls.Add(this.textBox_DeviceOtherName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_SN);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2_DeviceOtherName);
            this.Controls.Add(this.label_DeviceReg);
            this.Controls.Add(this.label1_DeviceInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_RegisterInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_RegisterInfo";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form_RegisterInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1_DeviceInfo;
        private System.Windows.Forms.Label label_DeviceReg;
        private System.Windows.Forms.Label label2_DeviceOtherName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_DeviceOtherName;
        private System.Windows.Forms.TextBox textBox_DeviceIP;
        private System.Windows.Forms.ComboBox comboBox_DeviceModel;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Label label_SN;
        private System.Windows.Forms.TextBox textBox_SN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_Communication;
    }
}