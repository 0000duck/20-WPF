namespace saker_Winform.SubForm
{
    partial class Form_CalibrationInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_CalibrationInfo));
            this.dataGridView_Calibration = new System.Windows.Forms.DataGridView();
            this.设备名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.序列号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.设备延时校准时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.设备延时 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.通道校准时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LineLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Calibration)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_Calibration
            // 
            this.dataGridView_Calibration.AllowUserToAddRows = false;
            this.dataGridView_Calibration.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_Calibration.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.设备名称,
            this.序列号,
            this.设备延时校准时间,
            this.设备延时,
            this.通道校准时间,
            this.LineLength});
            this.dataGridView_Calibration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Calibration.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_Calibration.Name = "dataGridView_Calibration";
            this.dataGridView_Calibration.ReadOnly = true;
            this.dataGridView_Calibration.Size = new System.Drawing.Size(800, 450);
            this.dataGridView_Calibration.TabIndex = 0;
            // 
            // 设备名称
            // 
            this.设备名称.DataPropertyName = "Name";
            this.设备名称.HeaderText = "设备名称";
            this.设备名称.Name = "设备名称";
            this.设备名称.ReadOnly = true;
            // 
            // 序列号
            // 
            this.序列号.DataPropertyName = "SerialNumber";
            this.序列号.HeaderText = "序列号";
            this.序列号.Name = "序列号";
            this.序列号.ReadOnly = true;
            // 
            // 设备延时校准时间
            // 
            this.设备延时校准时间.DataPropertyName = "DevDelayCalTime";
            this.设备延时校准时间.HeaderText = "设备延时校准时间";
            this.设备延时校准时间.Name = "设备延时校准时间";
            this.设备延时校准时间.ReadOnly = true;
            // 
            // 设备延时
            // 
            this.设备延时.DataPropertyName = "DeviceDelayTime";
            this.设备延时.HeaderText = "设备延时";
            this.设备延时.Name = "设备延时";
            this.设备延时.ReadOnly = true;
            // 
            // 通道校准时间
            // 
            this.通道校准时间.DataPropertyName = "ChanDelayCalTime";
            this.通道校准时间.HeaderText = "通道校准时间";
            this.通道校准时间.Name = "通道校准时间";
            this.通道校准时间.ReadOnly = true;
            // 
            // LineLength
            // 
            this.LineLength.DataPropertyName = "LineLength";
            this.LineLength.HeaderText = "线长";
            this.LineLength.Name = "LineLength";
            this.LineLength.ReadOnly = true;
            // 
            // Form_CalibrationInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView_Calibration);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_CalibrationInfo";
            this.Text = "Form_CalibrationInfo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_CalibrationInfo_FormClosed);
            this.Load += new System.EventHandler(this.Form_CalibrationInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Calibration)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_Calibration;
        private System.Windows.Forms.DataGridViewTextBoxColumn 设备名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 序列号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 设备延时校准时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 设备延时;
        private System.Windows.Forms.DataGridViewTextBoxColumn 通道校准时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn LineLength;
    }
}