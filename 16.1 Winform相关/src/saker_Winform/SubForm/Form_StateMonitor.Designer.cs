namespace saker_Winform.SubForm
{
    partial class Form_StateMonitor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_StateMonitor));
            this.panel_StateDev = new System.Windows.Forms.Panel();
            this.flowLayoutPanel_StateDev = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_Refresh = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_StateSys = new System.Windows.Forms.Panel();
            this.btn_ReadDetail = new System.Windows.Forms.Button();
            this.label_WritError = new System.Windows.Forms.Label();
            this.label_DataMiss = new System.Windows.Forms.Label();
            this.label_RecordNum = new System.Windows.Forms.Label();
            this.label_CalTime = new System.Windows.Forms.Label();
            this.label_State = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.panel_StateDev.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel_StateSys.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_StateDev
            // 
            this.panel_StateDev.Controls.Add(this.flowLayoutPanel_StateDev);
            this.panel_StateDev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_StateDev.Location = new System.Drawing.Point(0, 241);
            this.panel_StateDev.Name = "panel_StateDev";
            this.panel_StateDev.Size = new System.Drawing.Size(806, 354);
            this.panel_StateDev.TabIndex = 7;
            // 
            // flowLayoutPanel_StateDev
            // 
            this.flowLayoutPanel_StateDev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel_StateDev.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel_StateDev.Name = "flowLayoutPanel_StateDev";
            this.flowLayoutPanel_StateDev.Size = new System.Drawing.Size(806, 354);
            this.flowLayoutPanel_StateDev.TabIndex = 0;
            this.flowLayoutPanel_StateDev.Click += new System.EventHandler(this.flowLayoutPanel_StateDev_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.panel1.Controls.Add(this.button_Refresh);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(0, 209);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(806, 32);
            this.panel1.TabIndex = 6;
            // 
            // button_Refresh
            // 
            this.button_Refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Refresh.Location = new System.Drawing.Point(719, 2);
            this.button_Refresh.Name = "button_Refresh";
            this.button_Refresh.Size = new System.Drawing.Size(75, 28);
            this.button_Refresh.TabIndex = 2;
            this.button_Refresh.Text = "刷 新";
            this.button_Refresh.UseVisualStyleBackColor = true;
            this.button_Refresh.Click += new System.EventHandler(this.button_Refresh_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 22);
            this.label1.TabIndex = 1;
            this.label1.Text = "仪器运行状态";
            // 
            // panel_StateSys
            // 
            this.panel_StateSys.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.panel_StateSys.Controls.Add(this.btn_ReadDetail);
            this.panel_StateSys.Controls.Add(this.label_WritError);
            this.panel_StateSys.Controls.Add(this.label_DataMiss);
            this.panel_StateSys.Controls.Add(this.label_RecordNum);
            this.panel_StateSys.Controls.Add(this.label_CalTime);
            this.panel_StateSys.Controls.Add(this.label_State);
            this.panel_StateSys.Controls.Add(this.label8);
            this.panel_StateSys.Controls.Add(this.label6);
            this.panel_StateSys.Controls.Add(this.label5);
            this.panel_StateSys.Controls.Add(this.label4);
            this.panel_StateSys.Controls.Add(this.label3);
            this.panel_StateSys.Controls.Add(this.label2);
            this.panel_StateSys.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_StateSys.Location = new System.Drawing.Point(0, 32);
            this.panel_StateSys.Name = "panel_StateSys";
            this.panel_StateSys.Size = new System.Drawing.Size(806, 177);
            this.panel_StateSys.TabIndex = 5;
            // 
            // btn_ReadDetail
            // 
            this.btn_ReadDetail.Location = new System.Drawing.Point(194, 72);
            this.btn_ReadDetail.Name = "btn_ReadDetail";
            this.btn_ReadDetail.Size = new System.Drawing.Size(100, 33);
            this.btn_ReadDetail.TabIndex = 24;
            this.btn_ReadDetail.Text = "查看详情";
            this.btn_ReadDetail.UseVisualStyleBackColor = true;
            this.btn_ReadDetail.Click += new System.EventHandler(this.btn_ReadDetail_Click);
            // 
            // label_WritError
            // 
            this.label_WritError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_WritError.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_WritError.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_WritError.Location = new System.Drawing.Point(645, 128);
            this.label_WritError.Name = "label_WritError";
            this.label_WritError.Size = new System.Drawing.Size(58, 33);
            this.label_WritError.TabIndex = 23;
            this.label_WritError.Text = "0";
            this.label_WritError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label_WritError.Visible = false;
            // 
            // label_DataMiss
            // 
            this.label_DataMiss.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_DataMiss.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_DataMiss.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_DataMiss.Location = new System.Drawing.Point(645, 72);
            this.label_DataMiss.Name = "label_DataMiss";
            this.label_DataMiss.Size = new System.Drawing.Size(58, 33);
            this.label_DataMiss.TabIndex = 22;
            this.label_DataMiss.Text = "0";
            this.label_DataMiss.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_RecordNum
            // 
            this.label_RecordNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_RecordNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_RecordNum.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_RecordNum.Location = new System.Drawing.Point(645, 16);
            this.label_RecordNum.Name = "label_RecordNum";
            this.label_RecordNum.Size = new System.Drawing.Size(58, 33);
            this.label_RecordNum.TabIndex = 21;
            this.label_RecordNum.Text = "0";
            this.label_RecordNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_CalTime
            // 
            this.label_CalTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_CalTime.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.label_CalTime.Location = new System.Drawing.Point(194, 128);
            this.label_CalTime.Name = "label_CalTime";
            this.label_CalTime.Size = new System.Drawing.Size(219, 33);
            this.label_CalTime.TabIndex = 20;
            this.label_CalTime.Text = "2020.5.1 23:18:15";
            this.label_CalTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_State
            // 
            this.label_State.BackColor = System.Drawing.Color.Red;
            this.label_State.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_State.ForeColor = System.Drawing.Color.Black;
            this.label_State.Location = new System.Drawing.Point(194, 16);
            this.label_State.Name = "label_State";
            this.label_State.Size = new System.Drawing.Size(100, 33);
            this.label_State.TabIndex = 18;
            this.label_State.Text = "STOP";
            this.label_State.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(198)))), ((int)(((byte)(231)))));
            this.label8.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(465, 128);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(168, 33);
            this.label8.TabIndex = 12;
            this.label8.Text = "剩余条数：";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label8.Visible = false;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(198)))), ((int)(((byte)(231)))));
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(465, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(165, 33);
            this.label6.TabIndex = 11;
            this.label6.Text = "数据丢失：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(198)))), ((int)(((byte)(231)))));
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(464, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(165, 33);
            this.label5.TabIndex = 10;
            this.label5.Text = "已记录条目数：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(198)))), ((int)(((byte)(231)))));
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(4, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(184, 33);
            this.label4.TabIndex = 9;
            this.label4.Text = "上次校准记录：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(198)))), ((int)(((byte)(231)))));
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(4, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 33);
            this.label3.TabIndex = 8;
            this.label3.Text = "开始测试时间：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(198)))), ((int)(((byte)(231)))));
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(4, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 33);
            this.label2.TabIndex = 7;
            this.label2.Text = "工程状态：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.panel5.Controls.Add(this.label7);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(806, 32);
            this.panel5.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.SystemColors.Control;
            this.label7.Location = new System.Drawing.Point(5, 5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 22);
            this.label7.TabIndex = 1;
            this.label7.Text = "系统运行状态";
            // 
            // Form_StateMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 595);
            this.Controls.Add(this.panel_StateDev);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel_StateSys);
            this.Controls.Add(this.panel5);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_StateMonitor";
            this.Text = "Form_StateMonitor";
            this.Load += new System.EventHandler(this.Form_StateMonitor_Load);
            this.panel_StateDev.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel_StateSys.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel_StateSys;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel_StateDev;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_StateDev;
        private System.Windows.Forms.Label label_State;
        private System.Windows.Forms.Label label_WritError;
        private System.Windows.Forms.Label label_DataMiss;
        private System.Windows.Forms.Label label_RecordNum;
        private System.Windows.Forms.Button button_Refresh;
        private System.Windows.Forms.Label label_CalTime;
        private System.Windows.Forms.Button btn_ReadDetail;
    }
}