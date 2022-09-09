namespace saker_Winform.SubForm
{
    partial class Form_ChooseProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ChooseProject));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_Project_Name = new System.Windows.Forms.ComboBox();
            this.comboBox_TestTime = new System.Windows.Forms.ComboBox();
            this.iconButton_Search = new FontAwesome.Sharp.IconButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "项目名称：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "测试时间：";
            // 
            // comboBox_Project_Name
            // 
            this.comboBox_Project_Name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Project_Name.FormattingEnabled = true;
            this.comboBox_Project_Name.IntegralHeight = false;
            this.comboBox_Project_Name.Location = new System.Drawing.Point(131, 25);
            this.comboBox_Project_Name.MaxDropDownItems = 5;
            this.comboBox_Project_Name.Name = "comboBox_Project_Name";
            this.comboBox_Project_Name.Size = new System.Drawing.Size(152, 20);
            this.comboBox_Project_Name.TabIndex = 2;
            this.comboBox_Project_Name.SelectedIndexChanged += new System.EventHandler(this.comboBox_Project_Name_SelectedIndexChanged);
            // 
            // comboBox_TestTime
            // 
            this.comboBox_TestTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_TestTime.FormattingEnabled = true;
            this.comboBox_TestTime.IntegralHeight = false;
            this.comboBox_TestTime.Location = new System.Drawing.Point(131, 65);
            this.comboBox_TestTime.MaxDropDownItems = 5;
            this.comboBox_TestTime.Name = "comboBox_TestTime";
            this.comboBox_TestTime.Size = new System.Drawing.Size(152, 20);
            this.comboBox_TestTime.TabIndex = 3;
            // 
            // iconButton_Search
            // 
            this.iconButton_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton_Search.CausesValidation = false;
            this.iconButton_Search.FlatAppearance.BorderSize = 0;
            this.iconButton_Search.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_Search.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.iconButton_Search.IconChar = FontAwesome.Sharp.IconChar.Search;
            this.iconButton_Search.IconColor = System.Drawing.Color.Green;
            this.iconButton_Search.IconSize = 21;
            this.iconButton_Search.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.iconButton_Search.Location = new System.Drawing.Point(307, 61);
            this.iconButton_Search.Name = "iconButton_Search";
            this.iconButton_Search.Rotation = 0D;
            this.iconButton_Search.Size = new System.Drawing.Size(77, 27);
            this.iconButton_Search.TabIndex = 37;
            this.iconButton_Search.Text = "查询";
            this.iconButton_Search.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.iconButton_Search.UseVisualStyleBackColor = true;
            this.iconButton_Search.Click += new System.EventHandler(this.iconButton_Search_Click);
            // 
            // Form_ChooseProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 110);
            this.Controls.Add(this.iconButton_Search);
            this.Controls.Add(this.comboBox_TestTime);
            this.Controls.Add(this.comboBox_Project_Name);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_ChooseProject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据库导入数据";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_ChooseProject_FormClosed);
            this.Load += new System.EventHandler(this.Form_ChooseProject_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_Project_Name;
        private System.Windows.Forms.ComboBox comboBox_TestTime;
        private FontAwesome.Sharp.IconButton iconButton_Search;
    }
}