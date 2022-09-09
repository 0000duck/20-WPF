namespace saker_Winform.SubForm
{
    partial class Form_ChanScaleSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ChanScaleSet));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_ChanOffset = new System.Windows.Forms.TextBox();
            this.textBox_ScaleMax = new System.Windows.Forms.TextBox();
            this.textBox_ScaleMin = new System.Windows.Forms.TextBox();
            this.button_Ok = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.checkBox_All = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(3, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "chan_OffSet(v):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(3, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "scale_Max(v):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(3, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "scale_Min(v):";
            // 
            // textBox_ChanOffset
            // 
            this.textBox_ChanOffset.Location = new System.Drawing.Point(115, 15);
            this.textBox_ChanOffset.Name = "textBox_ChanOffset";
            this.textBox_ChanOffset.Size = new System.Drawing.Size(164, 21);
            this.textBox_ChanOffset.TabIndex = 3;
            this.textBox_ChanOffset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_ChanOffset_KeyPress);
            // 
            // textBox_ScaleMax
            // 
            this.textBox_ScaleMax.Location = new System.Drawing.Point(115, 91);
            this.textBox_ScaleMax.Name = "textBox_ScaleMax";
            this.textBox_ScaleMax.Size = new System.Drawing.Size(164, 21);
            this.textBox_ScaleMax.TabIndex = 4;
            this.textBox_ScaleMax.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_ScaleMax_KeyPress);
            // 
            // textBox_ScaleMin
            // 
            this.textBox_ScaleMin.Location = new System.Drawing.Point(115, 164);
            this.textBox_ScaleMin.Name = "textBox_ScaleMin";
            this.textBox_ScaleMin.Size = new System.Drawing.Size(164, 21);
            this.textBox_ScaleMin.TabIndex = 5;
            this.textBox_ScaleMin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_ScaleMin_KeyPress);
            // 
            // button_Ok
            // 
            this.button_Ok.Location = new System.Drawing.Point(112, 260);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 23);
            this.button_Ok.TabIndex = 6;
            this.button_Ok.Text = "OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            this.button_Ok.Click += new System.EventHandler(this.button_Ok_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(201, 260);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 7;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // checkBox_All
            // 
            this.checkBox_All.AutoSize = true;
            this.checkBox_All.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_All.Location = new System.Drawing.Point(13, 219);
            this.checkBox_All.Name = "checkBox_All";
            this.checkBox_All.Size = new System.Drawing.Size(123, 21);
            this.checkBox_All.TabIndex = 8;
            this.checkBox_All.Text = "是否应用所有通道";
            this.checkBox_All.UseVisualStyleBackColor = true;
            this.checkBox_All.CheckedChanged += new System.EventHandler(this.checkBox_All_CheckedChanged);
            // 
            // Form_ChanScaleSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 296);
            this.Controls.Add(this.checkBox_All);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.textBox_ScaleMin);
            this.Controls.Add(this.textBox_ScaleMax);
            this.Controls.Add(this.textBox_ChanOffset);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_ChanScaleSet";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_ChanOffset;
        private System.Windows.Forms.TextBox textBox_ScaleMax;
        private System.Windows.Forms.TextBox textBox_ScaleMin;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.CheckBox checkBox_All;
    }
}