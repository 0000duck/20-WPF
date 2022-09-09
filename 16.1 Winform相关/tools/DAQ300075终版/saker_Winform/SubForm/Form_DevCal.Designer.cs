namespace saker_Winform.SubForm
{
    partial class Form_DevCal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_DevCal));
            this.panel_Menu = new System.Windows.Forms.Panel();
            this.iconButton_DevCal = new FontAwesome.Sharp.IconButton();
            this.iconButton_ChanCal = new FontAwesome.Sharp.IconButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_CalStart = new System.Windows.Forms.Button();
            this.label_Title = new System.Windows.Forms.Label();
            this.progressBar_Cal = new System.Windows.Forms.ProgressBar();
            this.panel_DataGrid = new System.Windows.Forms.Panel();
            this.button_CancelAll = new System.Windows.Forms.Button();
            this.button_SelectAll = new System.Windows.Forms.Button();
            this.iconButton_LoadDev = new FontAwesome.Sharp.IconButton();
            this.dataGridView_DevChoose = new System.Windows.Forms.DataGridView();
            this.textBox_CalInfo = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel_Menu.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel_DataGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DevChoose)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_Menu
            // 
            this.panel_Menu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.panel_Menu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Menu.Controls.Add(this.iconButton_DevCal);
            this.panel_Menu.Controls.Add(this.iconButton_ChanCal);
            this.panel_Menu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_Menu.Location = new System.Drawing.Point(0, 0);
            this.panel_Menu.Name = "panel_Menu";
            this.panel_Menu.Size = new System.Drawing.Size(120, 581);
            this.panel_Menu.TabIndex = 0;
            // 
            // iconButton_DevCal
            // 
            this.iconButton_DevCal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.iconButton_DevCal.FlatAppearance.BorderSize = 0;
            this.iconButton_DevCal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_DevCal.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_DevCal.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.iconButton_DevCal.IconChar = FontAwesome.Sharp.IconChar.GlassMartini;
            this.iconButton_DevCal.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(101)))), ((int)(((byte)(176)))));
            this.iconButton_DevCal.IconSize = 16;
            this.iconButton_DevCal.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iconButton_DevCal.Location = new System.Drawing.Point(3, 116);
            this.iconButton_DevCal.Name = "iconButton_DevCal";
            this.iconButton_DevCal.Rotation = 0D;
            this.iconButton_DevCal.Size = new System.Drawing.Size(111, 37);
            this.iconButton_DevCal.TabIndex = 1;
            this.iconButton_DevCal.Text = "系统校准";
            this.iconButton_DevCal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton_DevCal.UseVisualStyleBackColor = false;
            this.iconButton_DevCal.Click += new System.EventHandler(this.iconButton_DevCal_Click);
            // 
            // iconButton_ChanCal
            // 
            this.iconButton_ChanCal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(198)))), ((int)(((byte)(198)))));
            this.iconButton_ChanCal.Enabled = false;
            this.iconButton_ChanCal.FlatAppearance.BorderSize = 0;
            this.iconButton_ChanCal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_ChanCal.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_ChanCal.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.iconButton_ChanCal.IconChar = FontAwesome.Sharp.IconChar.Outdent;
            this.iconButton_ChanCal.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(101)))), ((int)(((byte)(176)))));
            this.iconButton_ChanCal.IconSize = 16;
            this.iconButton_ChanCal.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iconButton_ChanCal.Location = new System.Drawing.Point(2, 55);
            this.iconButton_ChanCal.Name = "iconButton_ChanCal";
            this.iconButton_ChanCal.Rotation = 0D;
            this.iconButton_ChanCal.Size = new System.Drawing.Size(114, 37);
            this.iconButton_ChanCal.TabIndex = 0;
            this.iconButton_ChanCal.Text = "单机校准";
            this.iconButton_ChanCal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton_ChanCal.UseVisualStyleBackColor = false;
            this.iconButton_ChanCal.Click += new System.EventHandler(this.iconButton_ChanCal_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.panel1.Controls.Add(this.button_CalStart);
            this.panel1.Controls.Add(this.label_Title);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(120, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(921, 32);
            this.panel1.TabIndex = 1;
            // 
            // button_CalStart
            // 
            this.button_CalStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_CalStart.BackColor = System.Drawing.Color.DarkGray;
            this.button_CalStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_CalStart.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_CalStart.ForeColor = System.Drawing.SystemColors.Control;
            this.button_CalStart.Location = new System.Drawing.Point(842, 4);
            this.button_CalStart.Name = "button_CalStart";
            this.button_CalStart.Size = new System.Drawing.Size(75, 25);
            this.button_CalStart.TabIndex = 2;
            this.button_CalStart.Text = "开始校准";
            this.button_CalStart.UseVisualStyleBackColor = false;
            this.button_CalStart.Click += new System.EventHandler(this.button_CalStart_Click);
            // 
            // label_Title
            // 
            this.label_Title.AutoSize = true;
            this.label_Title.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Title.ForeColor = System.Drawing.SystemColors.Control;
            this.label_Title.Location = new System.Drawing.Point(4, 4);
            this.label_Title.Name = "label_Title";
            this.label_Title.Size = new System.Drawing.Size(106, 22);
            this.label_Title.TabIndex = 1;
            this.label_Title.Text = "通道延时校准";
            // 
            // progressBar_Cal
            // 
            this.progressBar_Cal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar_Cal.Location = new System.Drawing.Point(818, 4);
            this.progressBar_Cal.Name = "progressBar_Cal";
            this.progressBar_Cal.Size = new System.Drawing.Size(100, 23);
            this.progressBar_Cal.TabIndex = 3;
            // 
            // panel_DataGrid
            // 
            this.panel_DataGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.panel_DataGrid.Controls.Add(this.button_CancelAll);
            this.panel_DataGrid.Controls.Add(this.button_SelectAll);
            this.panel_DataGrid.Controls.Add(this.iconButton_LoadDev);
            this.panel_DataGrid.Controls.Add(this.dataGridView_DevChoose);
            this.panel_DataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_DataGrid.Location = new System.Drawing.Point(120, 32);
            this.panel_DataGrid.Name = "panel_DataGrid";
            this.panel_DataGrid.Size = new System.Drawing.Size(921, 359);
            this.panel_DataGrid.TabIndex = 2;
            // 
            // button_CancelAll
            // 
            this.button_CancelAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_CancelAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.button_CancelAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_CancelAll.FlatAppearance.BorderSize = 0;
            this.button_CancelAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_CancelAll.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_CancelAll.ForeColor = System.Drawing.Color.White;
            this.button_CancelAll.Location = new System.Drawing.Point(99, 330);
            this.button_CancelAll.Name = "button_CancelAll";
            this.button_CancelAll.Size = new System.Drawing.Size(75, 23);
            this.button_CancelAll.TabIndex = 4;
            this.button_CancelAll.Text = "取消全选";
            this.button_CancelAll.UseVisualStyleBackColor = false;
            this.button_CancelAll.Click += new System.EventHandler(this.button_CancelAll_Click);
            // 
            // button_SelectAll
            // 
            this.button_SelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_SelectAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.button_SelectAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_SelectAll.FlatAppearance.BorderSize = 0;
            this.button_SelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_SelectAll.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_SelectAll.ForeColor = System.Drawing.Color.White;
            this.button_SelectAll.Location = new System.Drawing.Point(4, 330);
            this.button_SelectAll.Name = "button_SelectAll";
            this.button_SelectAll.Size = new System.Drawing.Size(75, 23);
            this.button_SelectAll.TabIndex = 3;
            this.button_SelectAll.Text = "全 选";
            this.button_SelectAll.UseVisualStyleBackColor = false;
            this.button_SelectAll.Click += new System.EventHandler(this.button_SelectAll_Click);
            // 
            // iconButton_LoadDev
            // 
            this.iconButton_LoadDev.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.iconButton_LoadDev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconButton_LoadDev.FlatAppearance.BorderSize = 0;
            this.iconButton_LoadDev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_LoadDev.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_LoadDev.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.iconButton_LoadDev.ForeColor = System.Drawing.Color.White;
            this.iconButton_LoadDev.IconChar = FontAwesome.Sharp.IconChar.Download;
            this.iconButton_LoadDev.IconColor = System.Drawing.Color.White;
            this.iconButton_LoadDev.IconSize = 16;
            this.iconButton_LoadDev.Location = new System.Drawing.Point(3, 6);
            this.iconButton_LoadDev.Name = "iconButton_LoadDev";
            this.iconButton_LoadDev.Rotation = 0D;
            this.iconButton_LoadDev.Size = new System.Drawing.Size(107, 24);
            this.iconButton_LoadDev.TabIndex = 2;
            this.iconButton_LoadDev.Text = "加载设备信息";
            this.iconButton_LoadDev.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.iconButton_LoadDev.UseVisualStyleBackColor = false;
            this.iconButton_LoadDev.Click += new System.EventHandler(this.iconButton_LoadDev_Click);
            // 
            // dataGridView_DevChoose
            // 
            this.dataGridView_DevChoose.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_DevChoose.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_DevChoose.Location = new System.Drawing.Point(3, 36);
            this.dataGridView_DevChoose.Name = "dataGridView_DevChoose";
            this.dataGridView_DevChoose.RowTemplate.Height = 23;
            this.dataGridView_DevChoose.Size = new System.Drawing.Size(915, 289);
            this.dataGridView_DevChoose.TabIndex = 1;
            this.dataGridView_DevChoose.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView_DevChoose_CellValidating);
            this.dataGridView_DevChoose.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_DevChoose_RowPostPaint);
            // 
            // textBox_CalInfo
            // 
            this.textBox_CalInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_CalInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox_CalInfo.Location = new System.Drawing.Point(120, 423);
            this.textBox_CalInfo.Multiline = true;
            this.textBox_CalInfo.Name = "textBox_CalInfo";
            this.textBox_CalInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_CalInfo.Size = new System.Drawing.Size(921, 158);
            this.textBox_CalInfo.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.panel2.Controls.Add(this.progressBar_Cal);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(120, 391);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(921, 32);
            this.panel2.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(4, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 22);
            this.label2.TabIndex = 1;
            this.label2.Text = "校准过程信息";
            // 
            // Form_DevCal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 581);
            this.Controls.Add(this.panel_DataGrid);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.textBox_CalInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel_Menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_DevCal";
            this.Text = "Form_SingleDevCal";
            this.Load += new System.EventHandler(this.Form_DevCal_Load);
            this.panel_Menu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel_DataGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DevChoose)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_Menu;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_CalStart;
        private System.Windows.Forms.Label label_Title;
        private System.Windows.Forms.Panel panel_DataGrid;
        private System.Windows.Forms.TextBox textBox_CalInfo;
        private FontAwesome.Sharp.IconButton iconButton_ChanCal;
        private FontAwesome.Sharp.IconButton iconButton_DevCal;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView_DevChoose;
        private FontAwesome.Sharp.IconButton iconButton_LoadDev;
        private System.Windows.Forms.Button button_CancelAll;
        private System.Windows.Forms.Button button_SelectAll;
        private System.Windows.Forms.ProgressBar progressBar_Cal;

    }
}