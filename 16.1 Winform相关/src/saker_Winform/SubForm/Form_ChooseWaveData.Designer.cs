namespace saker_Winform.SubForm
{
    partial class Form_ChooseWaveData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ChooseWaveData));
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_projectname = new System.Windows.Forms.TextBox();
            this.button_import = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox_startTime = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "项目名称：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "采集时间：";
            // 
            // txt_projectname
            // 
            this.txt_projectname.Location = new System.Drawing.Point(86, 19);
            this.txt_projectname.Name = "txt_projectname";
            this.txt_projectname.Size = new System.Drawing.Size(165, 21);
            this.txt_projectname.TabIndex = 3;
            // 
            // button_import
            // 
            this.button_import.Location = new System.Drawing.Point(37, 91);
            this.button_import.Name = "button_import";
            this.button_import.Size = new System.Drawing.Size(75, 23);
            this.button_import.TabIndex = 6;
            this.button_import.Text = "导入";
            this.button_import.UseVisualStyleBackColor = true;
            this.button_import.Click += new System.EventHandler(this.button_import_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(159, 91);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBox_startTime
            // 
            this.comboBox_startTime.FormattingEnabled = true;
            this.comboBox_startTime.Location = new System.Drawing.Point(86, 55);
            this.comboBox_startTime.Name = "comboBox_startTime";
            this.comboBox_startTime.Size = new System.Drawing.Size(165, 20);
            this.comboBox_startTime.TabIndex = 8;
            // 
            // Form_ChooseWaveData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 126);
            this.Controls.Add(this.comboBox_startTime);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button_import);
            this.Controls.Add(this.txt_projectname);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_ChooseWaveData";
            this.Text = "Form_ChooseWaveData";
            this.Load += new System.EventHandler(this.Form_ChooseWaveData_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_projectname;
        private System.Windows.Forms.Button button_import;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBox_startTime;
    }
}