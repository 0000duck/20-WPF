using FontAwesome.Sharp;
namespace saker_Winform.SubForm
{
    partial class Form_DeletDeviceConfigInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_DeletDeviceConfigInfo));
            this.button1_DeletOK = new System.Windows.Forms.Button();
            this.button1_DeletCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3_RefLine = new System.Windows.Forms.Label();
            this.iconButton_Warning = new FontAwesome.Sharp.IconButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // button1_DeletOK
            // 
            this.button1_DeletOK.BackColor = System.Drawing.SystemColors.Control;
            this.button1_DeletOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1_DeletOK.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1_DeletOK.Location = new System.Drawing.Point(95, 126);
            this.button1_DeletOK.Name = "button1_DeletOK";
            this.button1_DeletOK.Size = new System.Drawing.Size(75, 23);
            this.button1_DeletOK.TabIndex = 1;
            this.button1_DeletOK.Text = "确定";
            this.button1_DeletOK.UseVisualStyleBackColor = false;
            this.button1_DeletOK.Click += new System.EventHandler(this.button1_DeletOK_Click);
            // 
            // button1_DeletCancel
            // 
            this.button1_DeletCancel.BackColor = System.Drawing.SystemColors.Control;
            this.button1_DeletCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1_DeletCancel.Location = new System.Drawing.Point(218, 127);
            this.button1_DeletCancel.Name = "button1_DeletCancel";
            this.button1_DeletCancel.Size = new System.Drawing.Size(75, 23);
            this.button1_DeletCancel.TabIndex = 2;
            this.button1_DeletCancel.Text = "取消";
            this.button1_DeletCancel.UseVisualStyleBackColor = false;
            this.button1_DeletCancel.Click += new System.EventHandler(this.button1_DeletCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(105, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 22);
            this.label1.TabIndex = 10;
            this.label1.Text = "是否删除仪表（Device)?";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(6, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 22);
            this.label2.TabIndex = 10;
            this.label2.Text = "警告信息";
            // 
            // label3_RefLine
            // 
            this.label3_RefLine.BackColor = System.Drawing.Color.White;
            this.label3_RefLine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3_RefLine.Location = new System.Drawing.Point(4, 32);
            this.label3_RefLine.Name = "label3_RefLine";
            this.label3_RefLine.Size = new System.Drawing.Size(380, 2);
            this.label3_RefLine.TabIndex = 11;
            // 
            // iconButton_Warning
            // 
            this.iconButton_Warning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_Warning.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_Warning.ForeColor = System.Drawing.SystemColors.Control;
            this.iconButton_Warning.IconChar = FontAwesome.Sharp.IconChar.TimesCircle;
            this.iconButton_Warning.IconColor = System.Drawing.Color.Black;
            this.iconButton_Warning.IconSize = 45;
            this.iconButton_Warning.Location = new System.Drawing.Point(49, 55);
            this.iconButton_Warning.Name = "iconButton_Warning";
            this.iconButton_Warning.Rotation = 0D;
            this.iconButton_Warning.Size = new System.Drawing.Size(50, 45);
            this.iconButton_Warning.TabIndex = 12;
            this.iconButton_Warning.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(0, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(386, 185);
            this.panel1.TabIndex = 13;
            // 
            // Form_DeletDeviceConfigInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(386, 188);
            this.Controls.Add(this.iconButton_Warning);
            this.Controls.Add(this.label3_RefLine);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1_DeletCancel);
            this.Controls.Add(this.button1_DeletOK);
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_DeletDeviceConfigInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "From_DeviceConfigDeleInfo";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form_DeletDeviceConfigInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1_DeletOK;
        private System.Windows.Forms.Button button1_DeletCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3_RefLine;
        private FontAwesome.Sharp.IconButton iconButton_Warning;
        private System.Windows.Forms.Panel panel1;
    }
}