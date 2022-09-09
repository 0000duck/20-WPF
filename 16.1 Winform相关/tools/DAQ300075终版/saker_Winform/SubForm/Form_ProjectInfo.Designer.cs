namespace saker_Winform.SubForm
{
    partial class Form_ProjectInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ProjectInfo));
            this.label_Name = new System.Windows.Forms.Label();
            this.label_ProjectName = new System.Windows.Forms.Label();
            this.label_Remark = new System.Windows.Forms.Label();
            this.label_RemarkLoad = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_Name
            // 
            this.label_Name.AutoSize = true;
            this.label_Name.BackColor = System.Drawing.Color.Transparent;
            this.label_Name.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Name.ForeColor = System.Drawing.Color.Black;
            this.label_Name.Location = new System.Drawing.Point(23, 18);
            this.label_Name.Name = "label_Name";
            this.label_Name.Size = new System.Drawing.Size(74, 22);
            this.label_Name.TabIndex = 2;
            this.label_Name.Text = "工程名：";
            this.label_Name.Click += new System.EventHandler(this.label_Name_Click);
            // 
            // label_ProjectName
            // 
            this.label_ProjectName.AutoSize = true;
            this.label_ProjectName.BackColor = System.Drawing.Color.Transparent;
            this.label_ProjectName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ProjectName.ForeColor = System.Drawing.Color.Orange;
            this.label_ProjectName.Location = new System.Drawing.Point(137, 18);
            this.label_ProjectName.Name = "label_ProjectName";
            this.label_ProjectName.Size = new System.Drawing.Size(0, 22);
            this.label_ProjectName.TabIndex = 3;
            this.label_ProjectName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Remark
            // 
            this.label_Remark.AutoSize = true;
            this.label_Remark.BackColor = System.Drawing.Color.Transparent;
            this.label_Remark.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Remark.ForeColor = System.Drawing.Color.Black;
            this.label_Remark.Location = new System.Drawing.Point(23, 75);
            this.label_Remark.Name = "label_Remark";
            this.label_Remark.Size = new System.Drawing.Size(63, 22);
            this.label_Remark.TabIndex = 2;
            this.label_Remark.Text = "注 释：";
            this.label_Remark.Click += new System.EventHandler(this.label_Name_Click);
            // 
            // label_RemarkLoad
            // 
            this.label_RemarkLoad.AutoSize = true;
            this.label_RemarkLoad.BackColor = System.Drawing.Color.Transparent;
            this.label_RemarkLoad.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_RemarkLoad.ForeColor = System.Drawing.Color.Orange;
            this.label_RemarkLoad.Location = new System.Drawing.Point(137, 75);
            this.label_RemarkLoad.Name = "label_RemarkLoad";
            this.label_RemarkLoad.Size = new System.Drawing.Size(0, 22);
            this.label_RemarkLoad.TabIndex = 2;
            this.label_RemarkLoad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label_RemarkLoad.Click += new System.EventHandler(this.label_Name_Click);
            // 
            // Form_ProjectInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 140);
            this.Controls.Add(this.label_ProjectName);
            this.Controls.Add(this.label_RemarkLoad);
            this.Controls.Add(this.label_Remark);
            this.Controls.Add(this.label_Name);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_ProjectInfo";
            this.Text = "Form_ProjectInfo";
            this.Load += new System.EventHandler(this.Form_ProjectInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Name;
        private System.Windows.Forms.Label label_ProjectName;
        private System.Windows.Forms.Label label_Remark;
        private System.Windows.Forms.Label label_RemarkLoad;
    }
}