namespace saker_Winform.SubForm
{
    partial class Form_SettingWaveMonitorViewScale
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_SettingWaveMonitorViewScale));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button_Apply = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.textBox_Little = new System.Windows.Forms.TextBox();
            this.textBox_Normal = new System.Windows.Forms.TextBox();
            this.textBox_Large = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "小图标显示模式:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(13, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "正常图标显示模式:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(13, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "大图标显示模式:";
            // 
            // button_Apply
            // 
            this.button_Apply.Location = new System.Drawing.Point(205, 227);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(75, 23);
            this.button_Apply.TabIndex = 3;
            this.button_Apply.Text = "确定";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(299, 227);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 4;
            this.button_Cancel.Text = "取消";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // textBox_Little
            // 
            this.textBox_Little.Location = new System.Drawing.Point(131, 12);
            this.textBox_Little.Name = "textBox_Little";
            this.textBox_Little.Size = new System.Drawing.Size(149, 21);
            this.textBox_Little.TabIndex = 5;
            this.textBox_Little.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_Little_KeyPress);
            // 
            // textBox_Normal
            // 
            this.textBox_Normal.Location = new System.Drawing.Point(131, 74);
            this.textBox_Normal.Name = "textBox_Normal";
            this.textBox_Normal.Size = new System.Drawing.Size(149, 21);
            this.textBox_Normal.TabIndex = 6;
            this.textBox_Normal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_Normal_KeyPress);
            // 
            // textBox_Large
            // 
            this.textBox_Large.Location = new System.Drawing.Point(131, 139);
            this.textBox_Large.Name = "textBox_Large";
            this.textBox_Large.Size = new System.Drawing.Size(149, 21);
            this.textBox_Large.TabIndex = 7;
            this.textBox_Large.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_Large_KeyPress);
            // 
            // Form_SettingWaveMonitorViewScale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 262);
            this.Controls.Add(this.textBox_Large);
            this.Controls.Add(this.textBox_Normal);
            this.Controls.Add(this.textBox_Little);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Apply);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_SettingWaveMonitorViewScale";
            this.Text = "Form_SettingWaveMonitorViewScale";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form_SettingWaveMonitorViewScale_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_Apply;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.TextBox textBox_Little;
        private System.Windows.Forms.TextBox textBox_Normal;
        private System.Windows.Forms.TextBox textBox_Large;
    }
}