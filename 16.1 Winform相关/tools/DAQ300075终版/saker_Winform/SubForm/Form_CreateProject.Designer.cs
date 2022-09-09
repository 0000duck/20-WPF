namespace saker_Winform.SubForm
{
    partial class Form_CreateProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_CreateProject));
            this.button_CreateOK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_ProjectName = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.textBox_FileLoaction = new System.Windows.Forms.TextBox();
            this.button_Find = new System.Windows.Forms.Button();
            this.textBox_Note = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // button_CreateOK
            // 
            this.button_CreateOK.BackColor = System.Drawing.SystemColors.Control;
            this.button_CreateOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_CreateOK.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button_CreateOK.Location = new System.Drawing.Point(99, 202);
            this.button_CreateOK.Name = "button_CreateOK";
            this.button_CreateOK.Size = new System.Drawing.Size(75, 23);
            this.button_CreateOK.TabIndex = 4;
            this.button_CreateOK.Text = "确定";
            this.button_CreateOK.UseVisualStyleBackColor = false;
            this.button_CreateOK.Click += new System.EventHandler(this.button_CreateOK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.BackColor = System.Drawing.SystemColors.Control;
            this.button_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Cancel.Location = new System.Drawing.Point(282, 202);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 5;
            this.button_Cancel.Text = "取消";
            this.button_Cancel.UseVisualStyleBackColor = false;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(7, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 22);
            this.label1.TabIndex = 10;
            this.label1.Text = "工程名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(7, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 22);
            this.label2.TabIndex = 10;
            this.label2.Text = "位 置：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(7, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 22);
            this.label3.TabIndex = 10;
            this.label3.Text = "注 释：";
            // 
            // textBox_ProjectName
            // 
            this.textBox_ProjectName.Location = new System.Drawing.Point(99, 26);
            this.textBox_ProjectName.Multiline = true;
            this.textBox_ProjectName.Name = "textBox_ProjectName";
            this.textBox_ProjectName.Size = new System.Drawing.Size(261, 21);
            this.textBox_ProjectName.TabIndex = 1;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "|*.slr";
            this.openFileDialog.Title = "Saker";
            // 
            // textBox_FileLoaction
            // 
            this.textBox_FileLoaction.Location = new System.Drawing.Point(99, 67);
            this.textBox_FileLoaction.Multiline = true;
            this.textBox_FileLoaction.Name = "textBox_FileLoaction";
            this.textBox_FileLoaction.Size = new System.Drawing.Size(261, 39);
            this.textBox_FileLoaction.TabIndex = 11;
            // 
            // button_Find
            // 
            this.button_Find.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_Find.Location = new System.Drawing.Point(379, 66);
            this.button_Find.Name = "button_Find";
            this.button_Find.Size = new System.Drawing.Size(75, 23);
            this.button_Find.TabIndex = 2;
            this.button_Find.Text = "浏览";
            this.button_Find.UseVisualStyleBackColor = true;
            this.button_Find.Click += new System.EventHandler(this.button_Find_Click);
            // 
            // textBox_Note
            // 
            this.textBox_Note.Location = new System.Drawing.Point(99, 120);
            this.textBox_Note.Multiline = true;
            this.textBox_Note.Name = "textBox_Note";
            this.textBox_Note.Size = new System.Drawing.Size(261, 58);
            this.textBox_Note.TabIndex = 3;
            // 
            // Form_CreateProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 236);
            this.Controls.Add(this.button_Find);
            this.Controls.Add(this.textBox_Note);
            this.Controls.Add(this.textBox_FileLoaction);
            this.Controls.Add(this.textBox_ProjectName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_CreateOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_CreateProject";
            this.Text = "新建工程";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form_CreateProject_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_CreateOK;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_ProjectName;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox textBox_FileLoaction;
        private System.Windows.Forms.Button button_Find;
        private System.Windows.Forms.TextBox textBox_Note;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}