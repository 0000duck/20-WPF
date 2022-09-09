namespace saker_Winform.SubForm
{
    partial class Form_AppInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_AppInfo));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_Title = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label_Company = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label_Version = new System.Windows.Forms.Label();
            this.label_Name = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.label_Title);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(379, 27);
            this.panel1.TabIndex = 1;
            // 
            // label_Title
            // 
            this.label_Title.AutoSize = true;
            this.label_Title.BackColor = System.Drawing.Color.Transparent;
            this.label_Title.ForeColor = System.Drawing.Color.Orange;
            this.label_Title.Location = new System.Drawing.Point(6, 10);
            this.label_Title.Name = "label_Title";
            this.label_Title.Size = new System.Drawing.Size(107, 12);
            this.label_Title.TabIndex = 0;
            this.label_Title.Text = "About Application";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DimGray;
            this.panel2.Controls.Add(this.label_Company);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 116);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(379, 34);
            this.panel2.TabIndex = 2;
            // 
            // label_Company
            // 
            this.label_Company.AutoSize = true;
            this.label_Company.BackColor = System.Drawing.Color.Transparent;
            this.label_Company.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Company.ForeColor = System.Drawing.Color.Orange;
            this.label_Company.Location = new System.Drawing.Point(31, 10);
            this.label_Company.Name = "label_Company";
            this.label_Company.Size = new System.Drawing.Size(311, 17);
            this.label_Company.TabIndex = 4;
            this.label_Company.Text = "2020,RIGOL Technologies,Inc  All Rights Reserved";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 27);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(113, 89);
            this.panel3.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.DimGray;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::saker_Winform.Properties.Resources.R;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(113, 89);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label_Version
            // 
            this.label_Version.AutoSize = true;
            this.label_Version.BackColor = System.Drawing.Color.Transparent;
            this.label_Version.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Version.ForeColor = System.Drawing.Color.Orange;
            this.label_Version.Location = new System.Drawing.Point(129, 80);
            this.label_Version.Name = "label_Version";
            this.label_Version.Size = new System.Drawing.Size(150, 19);
            this.label_Version.TabIndex = 1;
            this.label_Version.Text = "Version:00.00.00.01";
            // 
            // label_Name
            // 
            this.label_Name.AutoSize = true;
            this.label_Name.BackColor = System.Drawing.Color.Transparent;
            this.label_Name.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Name.ForeColor = System.Drawing.Color.Orange;
            this.label_Name.Location = new System.Drawing.Point(129, 50);
            this.label_Name.Name = "label_Name";
            this.label_Name.Size = new System.Drawing.Size(123, 19);
            this.label_Name.TabIndex = 1;
            this.label_Name.Text = "Name:UltraDAQ";
            this.label_Name.Click += new System.EventHandler(this.label_Name_Click);
            // 
            // Form_AppInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(379, 150);
            this.Controls.Add(this.label_Name);
            this.Controls.Add(this.label_Version);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_AppInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form_AppInfo";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label_Title;
        private System.Windows.Forms.Label label_Company;
        private System.Windows.Forms.Label label_Version;
        private System.Windows.Forms.Label label_Name;
    }
}