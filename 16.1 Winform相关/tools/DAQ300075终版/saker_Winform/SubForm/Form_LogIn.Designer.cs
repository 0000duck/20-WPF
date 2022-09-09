namespace saker_Winform.SubForm
{
    partial class Form_LogIn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_LogIn));
            this.button_LogIn = new System.Windows.Forms.Button();
            this.button_CancelLogIn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ID = new System.Windows.Forms.TextBox();
            this.textBox2_PassWord = new System.Windows.Forms.TextBox();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.checkBox_RecordPw = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_LogIn
            // 
            this.button_LogIn.BackColor = System.Drawing.Color.Transparent;
            this.button_LogIn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_LogIn.Location = new System.Drawing.Point(54, 186);
            this.button_LogIn.Name = "button_LogIn";
            this.button_LogIn.Size = new System.Drawing.Size(75, 23);
            this.button_LogIn.TabIndex = 3;
            this.button_LogIn.Text = "主机登录";
            this.button_LogIn.UseVisualStyleBackColor = false;
            this.button_LogIn.Click += new System.EventHandler(this.button_LogIn_Click);
            // 
            // button_CancelLogIn
            // 
            this.button_CancelLogIn.BackColor = System.Drawing.Color.Transparent;
            this.button_CancelLogIn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_CancelLogIn.Location = new System.Drawing.Point(209, 185);
            this.button_CancelLogIn.Name = "button_CancelLogIn";
            this.button_CancelLogIn.Size = new System.Drawing.Size(75, 23);
            this.button_CancelLogIn.TabIndex = 4;
            this.button_CancelLogIn.Text = "从机登录";
            this.button_CancelLogIn.UseVisualStyleBackColor = false;
            this.button_CancelLogIn.Click += new System.EventHandler(this.button_CancelLogIn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(50, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 22);
            this.label1.TabIndex = 2;
            this.label1.Text = "账号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(50, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 22);
            this.label2.TabIndex = 2;
            this.label2.Text = "密码：";
            // 
            // textBox_ID
            // 
            this.textBox_ID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox_ID.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_ID.ForeColor = System.Drawing.Color.White;
            this.textBox_ID.Location = new System.Drawing.Point(54, 53);
            this.textBox_ID.Name = "textBox_ID";
            this.textBox_ID.Size = new System.Drawing.Size(229, 23);
            this.textBox_ID.TabIndex = 1;
            // 
            // textBox2_PassWord
            // 
            this.textBox2_PassWord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox2_PassWord.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2_PassWord.ForeColor = System.Drawing.Color.White;
            this.textBox2_PassWord.Location = new System.Drawing.Point(54, 113);
            this.textBox2_PassWord.Name = "textBox2_PassWord";
            this.textBox2_PassWord.PasswordChar = '*';
            this.textBox2_PassWord.Size = new System.Drawing.Size(229, 23);
            this.textBox2_PassWord.TabIndex = 2;
            // 
            // button_Cancel
            // 
            this.button_Cancel.BackColor = System.Drawing.Color.Transparent;
            this.button_Cancel.BackgroundImage = global::saker_Winform.Properties.Resources.Close1;
            this.button_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_Cancel.ForeColor = System.Drawing.Color.White;
            this.button_Cancel.Location = new System.Drawing.Point(308, 10);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(24, 23);
            this.button_Cancel.TabIndex = 5;
            this.button_Cancel.UseVisualStyleBackColor = false;
            this.button_Cancel.Visible = false;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // checkBox_RecordPw
            // 
            this.checkBox_RecordPw.BackColor = System.Drawing.Color.White;
            this.checkBox_RecordPw.Checked = true;
            this.checkBox_RecordPw.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_RecordPw.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkBox_RecordPw.Location = new System.Drawing.Point(269, 147);
            this.checkBox_RecordPw.Name = "checkBox_RecordPw";
            this.checkBox_RecordPw.Size = new System.Drawing.Size(25, 20);
            this.checkBox_RecordPw.TabIndex = 6;
            this.checkBox_RecordPw.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(200, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "记住密码：";
            // 
            // Form_LogIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::saker_Winform.Properties.Resources.loginbody;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(337, 234);
            this.Controls.Add(this.checkBox_RecordPw);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.textBox2_PassWord);
            this.Controls.Add(this.textBox_ID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_CancelLogIn);
            this.Controls.Add(this.button_LogIn);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_LogIn";
            this.Text = "用户登录";
            this.Load += new System.EventHandler(this.Form_LogIn_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_LogIn;
        private System.Windows.Forms.Button button_CancelLogIn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ID;
        private System.Windows.Forms.TextBox textBox2_PassWord;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.CheckBox checkBox_RecordPw;
        private System.Windows.Forms.Label label3;
    }
}