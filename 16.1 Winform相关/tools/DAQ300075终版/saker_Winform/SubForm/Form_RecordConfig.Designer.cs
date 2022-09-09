namespace saker_Winform.SubForm
{
    partial class Form_RecordConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_RecordConfig));
            this.panel5 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBox_AddTime = new System.Windows.Forms.CheckBox();
            this.textBox_FileLoaction = new System.Windows.Forms.TextBox();
            this.dateTimePicker_Abs = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label_OpenFile = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkedListBox_RecordType = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox_RecordLocation = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox_RecordCondition = new System.Windows.Forms.CheckedListBox();
            this.comboBox_NameRule = new System.Windows.Forms.ComboBox();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.panel5.Controls.Add(this.label7);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(816, 35);
            this.panel5.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.SystemColors.Control;
            this.label7.Location = new System.Drawing.Point(5, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 22);
            this.label7.TabIndex = 1;
            this.label7.Text = "记录配置";
            // 
            // checkBox_AddTime
            // 
            this.checkBox_AddTime.AutoSize = true;
            this.checkBox_AddTime.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_AddTime.Location = new System.Drawing.Point(331, 367);
            this.checkBox_AddTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBox_AddTime.Name = "checkBox_AddTime";
            this.checkBox_AddTime.Size = new System.Drawing.Size(75, 21);
            this.checkBox_AddTime.TabIndex = 23;
            this.checkBox_AddTime.Text = "附加时间";
            this.checkBox_AddTime.UseVisualStyleBackColor = true;
            this.checkBox_AddTime.CheckStateChanged += new System.EventHandler(this.checkBox_AddTime_CheckStateChanged);
            // 
            // textBox_FileLoaction
            // 
            this.textBox_FileLoaction.Enabled = false;
            this.textBox_FileLoaction.Location = new System.Drawing.Point(197, 287);
            this.textBox_FileLoaction.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_FileLoaction.Multiline = true;
            this.textBox_FileLoaction.Name = "textBox_FileLoaction";
            this.textBox_FileLoaction.Size = new System.Drawing.Size(515, 33);
            this.textBox_FileLoaction.TabIndex = 22;
            // 
            // dateTimePicker_Abs
            // 
            this.dateTimePicker_Abs.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker_Abs.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker_Abs.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_Abs.Location = new System.Drawing.Point(272, 111);
            this.dateTimePicker_Abs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePicker_Abs.Name = "dateTimePicker_Abs";
            this.dateTimePicker_Abs.Size = new System.Drawing.Size(181, 23);
            this.dateTimePicker_Abs.TabIndex = 20;
            this.dateTimePicker_Abs.ValueChanged += new System.EventHandler(this.dateTimePicker_Abs_ValueChanged);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(198)))), ((int)(((byte)(231)))));
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(3, 361);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 31);
            this.label5.TabIndex = 14;
            this.label5.Text = "文件命名规则：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_OpenFile
            // 
            this.label_OpenFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(198)))), ((int)(((byte)(231)))));
            this.label_OpenFile.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_OpenFile.Location = new System.Drawing.Point(730, 285);
            this.label_OpenFile.Name = "label_OpenFile";
            this.label_OpenFile.Size = new System.Drawing.Size(64, 35);
            this.label_OpenFile.TabIndex = 15;
            this.label_OpenFile.Text = "浏览";
            this.label_OpenFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(198)))), ((int)(((byte)(231)))));
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(3, 285);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(176, 35);
            this.label4.TabIndex = 16;
            this.label4.Text = "本地文件保存位置：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(198)))), ((int)(((byte)(231)))));
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(3, 217);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(176, 33);
            this.label3.TabIndex = 17;
            this.label3.Text = "本地文件记录类型：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(198)))), ((int)(((byte)(231)))));
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(3, 155);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 28);
            this.label2.TabIndex = 18;
            this.label2.Text = "记录位置：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(198)))), ((int)(((byte)(231)))));
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(3, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 33);
            this.label1.TabIndex = 19;
            this.label1.Text = "采集条件：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkedListBox_RecordType
            // 
            this.checkedListBox_RecordType.BackColor = System.Drawing.SystemColors.Control;
            this.checkedListBox_RecordType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBox_RecordType.Enabled = false;
            this.checkedListBox_RecordType.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkedListBox_RecordType.FormattingEnabled = true;
            this.checkedListBox_RecordType.Items.AddRange(new object[] {
            "二进制",
            "Excel"});
            this.checkedListBox_RecordType.Location = new System.Drawing.Point(197, 222);
            this.checkedListBox_RecordType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkedListBox_RecordType.MultiColumn = true;
            this.checkedListBox_RecordType.Name = "checkedListBox_RecordType";
            this.checkedListBox_RecordType.Size = new System.Drawing.Size(322, 21);
            this.checkedListBox_RecordType.TabIndex = 11;
            // 
            // checkedListBox_RecordLocation
            // 
            this.checkedListBox_RecordLocation.BackColor = System.Drawing.SystemColors.Control;
            this.checkedListBox_RecordLocation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBox_RecordLocation.Enabled = false;
            this.checkedListBox_RecordLocation.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkedListBox_RecordLocation.FormattingEnabled = true;
            this.checkedListBox_RecordLocation.Items.AddRange(new object[] {
            "本地",
            "数据库"});
            this.checkedListBox_RecordLocation.Location = new System.Drawing.Point(125, 159);
            this.checkedListBox_RecordLocation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkedListBox_RecordLocation.MultiColumn = true;
            this.checkedListBox_RecordLocation.Name = "checkedListBox_RecordLocation";
            this.checkedListBox_RecordLocation.Size = new System.Drawing.Size(322, 21);
            this.checkedListBox_RecordLocation.TabIndex = 12;
            // 
            // checkedListBox_RecordCondition
            // 
            this.checkedListBox_RecordCondition.BackColor = System.Drawing.SystemColors.Control;
            this.checkedListBox_RecordCondition.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBox_RecordCondition.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkedListBox_RecordCondition.FormattingEnabled = true;
            this.checkedListBox_RecordCondition.Items.AddRange(new object[] {
            "Ext Trigger(Rise)",
            "Ext Trigger(Fall)",
            "Immediate",
            "Abs Time"});
            this.checkedListBox_RecordCondition.Location = new System.Drawing.Point(125, 50);
            this.checkedListBox_RecordCondition.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkedListBox_RecordCondition.Name = "checkedListBox_RecordCondition";
            this.checkedListBox_RecordCondition.Size = new System.Drawing.Size(141, 84);
            this.checkedListBox_RecordCondition.TabIndex = 13;
            this.checkedListBox_RecordCondition.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox_RecordCondition_ItemCheck);
            // 
            // comboBox_NameRule
            // 
            this.comboBox_NameRule.FormattingEnabled = true;
            this.comboBox_NameRule.Location = new System.Drawing.Point(155, 368);
            this.comboBox_NameRule.Name = "comboBox_NameRule";
            this.comboBox_NameRule.Size = new System.Drawing.Size(116, 20);
            this.comboBox_NameRule.TabIndex = 24;
            this.comboBox_NameRule.Text = "通道标记";
            // 
            // Form_RecordConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 422);
            this.Controls.Add(this.comboBox_NameRule);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.checkBox_AddTime);
            this.Controls.Add(this.textBox_FileLoaction);
            this.Controls.Add(this.dateTimePicker_Abs);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label_OpenFile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkedListBox_RecordType);
            this.Controls.Add(this.checkedListBox_RecordLocation);
            this.Controls.Add(this.checkedListBox_RecordCondition);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_RecordConfig";
            this.Text = "Form_RecordConfig";
            this.Load += new System.EventHandler(this.Form_RecordConfig_Load);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBox_AddTime;
        private System.Windows.Forms.TextBox textBox_FileLoaction;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Abs;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label_OpenFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox checkedListBox_RecordType;
        private System.Windows.Forms.CheckedListBox checkedListBox_RecordLocation;
        private System.Windows.Forms.CheckedListBox checkedListBox_RecordCondition;
        private System.Windows.Forms.ComboBox comboBox_NameRule;
    }
}