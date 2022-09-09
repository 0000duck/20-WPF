namespace saker_Winform
{
    partial class Form_Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.panel_Module = new System.Windows.Forms.Panel();
            this.panel_FunctionItem = new System.Windows.Forms.Panel();
            this.iconButton_ConfigRecord = new FontAwesome.Sharp.IconButton();
            this.iconButton_ConfigView = new FontAwesome.Sharp.IconButton();
            this.iconButton_ConfgChan = new FontAwesome.Sharp.IconButton();
            this.iconButton_ConfgDev = new FontAwesome.Sharp.IconButton();
            this.iconButton_DockLeft = new FontAwesome.Sharp.IconButton();
            this.iconButton_DockRight = new FontAwesome.Sharp.IconButton();
            this.panel_Project = new System.Windows.Forms.Panel();
            this.ucLabel_Prj = new saker_Winform.UserControls.UCLabel();
            this.iconButton_prjClose = new FontAwesome.Sharp.IconButton();
            this.panel_Item = new System.Windows.Forms.Panel();
            this.iconButton_Stop = new FontAwesome.Sharp.IconButton();
            this.iconButton_ForceTrig = new FontAwesome.Sharp.IconButton();
            this.iconButton_Save = new FontAwesome.Sharp.IconButton();
            this.iconButton_Start = new FontAwesome.Sharp.IconButton();
            this.iconButton_DataComparison = new FontAwesome.Sharp.IconButton();
            this.iconButton_Cal = new FontAwesome.Sharp.IconButton();
            this.iconButton_DataBase = new FontAwesome.Sharp.IconButton();
            this.iconButton_StateView = new FontAwesome.Sharp.IconButton();
            this.iconButton_WaveView = new FontAwesome.Sharp.IconButton();
            this.iconButton_SysConfig = new FontAwesome.Sharp.IconButton();
            this.panel_State = new System.Windows.Forms.Panel();
            this.label_Storage = new System.Windows.Forms.Label();
            this.label_Recive = new System.Windows.Forms.Label();
            this.label_Wait = new System.Windows.Forms.Label();
            this.menuStrip_Main = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iconMenuItem_CreateProj = new FontAwesome.Sharp.IconMenuItem();
            this.toolStripMenuItem_OpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_LocalOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.数据库ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemTranLang = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_zhCN = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_enUS = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_SaveDataBase = new System.Windows.Forms.ToolStripMenuItem();
            this.iconMenuItem_OpenSaveDb = new FontAwesome.Sharp.IconMenuItem();
            this.iconMenuItem_CloseSaveDb = new FontAwesome.Sharp.IconMenuItem();
            this.用户ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_AddUser = new FontAwesome.Sharp.IconMenuItem();
            this.ToolStripMenuItem_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.iconMenuItem1 = new FontAwesome.Sharp.IconMenuItem();
            this.iconMenuItem2 = new FontAwesome.Sharp.IconMenuItem();
            this.iconMenuItem3 = new FontAwesome.Sharp.IconMenuItem();
            this.panel_FunctionItem.SuspendLayout();
            this.panel_Project.SuspendLayout();
            this.panel_Item.SuspendLayout();
            this.panel_State.SuspendLayout();
            this.menuStrip_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_Module
            // 
            this.panel_Module.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.panel_Module.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Module.Location = new System.Drawing.Point(121, 96);
            this.panel_Module.Name = "panel_Module";
            this.panel_Module.Size = new System.Drawing.Size(871, 552);
            this.panel_Module.TabIndex = 4;
            // 
            // panel_FunctionItem
            // 
            this.panel_FunctionItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.panel_FunctionItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_FunctionItem.Controls.Add(this.iconButton_ConfigRecord);
            this.panel_FunctionItem.Controls.Add(this.iconButton_ConfigView);
            this.panel_FunctionItem.Controls.Add(this.iconButton_ConfgChan);
            this.panel_FunctionItem.Controls.Add(this.iconButton_ConfgDev);
            this.panel_FunctionItem.Controls.Add(this.iconButton_DockLeft);
            this.panel_FunctionItem.Controls.Add(this.iconButton_DockRight);
            this.panel_FunctionItem.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_FunctionItem.Location = new System.Drawing.Point(0, 96);
            this.panel_FunctionItem.Name = "panel_FunctionItem";
            this.panel_FunctionItem.Size = new System.Drawing.Size(121, 552);
            this.panel_FunctionItem.TabIndex = 3;
            // 
            // iconButton_ConfigRecord
            // 
            this.iconButton_ConfigRecord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton_ConfigRecord.FlatAppearance.BorderSize = 0;
            this.iconButton_ConfigRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_ConfigRecord.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_ConfigRecord.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.iconButton_ConfigRecord.IconChar = FontAwesome.Sharp.IconChar.Edit;
            this.iconButton_ConfigRecord.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(101)))), ((int)(((byte)(176)))));
            this.iconButton_ConfigRecord.IconSize = 27;
            this.iconButton_ConfigRecord.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iconButton_ConfigRecord.Location = new System.Drawing.Point(2, 238);
            this.iconButton_ConfigRecord.Name = "iconButton_ConfigRecord";
            this.iconButton_ConfigRecord.Rotation = 0D;
            this.iconButton_ConfigRecord.Size = new System.Drawing.Size(117, 42);
            this.iconButton_ConfigRecord.TabIndex = 16;
            this.iconButton_ConfigRecord.Text = "记录配置";
            this.iconButton_ConfigRecord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton_ConfigRecord.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.iconButton_ConfigRecord.UseVisualStyleBackColor = true;
            this.iconButton_ConfigRecord.Click += new System.EventHandler(this.iconButton_ConfigRecord_Click);
            // 
            // iconButton_ConfigView
            // 
            this.iconButton_ConfigView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton_ConfigView.FlatAppearance.BorderSize = 0;
            this.iconButton_ConfigView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_ConfigView.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_ConfigView.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.iconButton_ConfigView.IconChar = FontAwesome.Sharp.IconChar.Desktop;
            this.iconButton_ConfigView.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(101)))), ((int)(((byte)(176)))));
            this.iconButton_ConfigView.IconSize = 27;
            this.iconButton_ConfigView.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iconButton_ConfigView.Location = new System.Drawing.Point(2, 172);
            this.iconButton_ConfigView.Name = "iconButton_ConfigView";
            this.iconButton_ConfigView.Rotation = 0D;
            this.iconButton_ConfigView.Size = new System.Drawing.Size(117, 42);
            this.iconButton_ConfigView.TabIndex = 15;
            this.iconButton_ConfigView.Text = "显示配置";
            this.iconButton_ConfigView.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton_ConfigView.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.iconButton_ConfigView.UseVisualStyleBackColor = true;
            this.iconButton_ConfigView.Click += new System.EventHandler(this.iconButton_ConfigView_Click);
            // 
            // iconButton_ConfgChan
            // 
            this.iconButton_ConfgChan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton_ConfgChan.FlatAppearance.BorderSize = 0;
            this.iconButton_ConfgChan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_ConfgChan.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_ConfgChan.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.iconButton_ConfgChan.IconChar = FontAwesome.Sharp.IconChar.Braille;
            this.iconButton_ConfgChan.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(101)))), ((int)(((byte)(176)))));
            this.iconButton_ConfgChan.IconSize = 27;
            this.iconButton_ConfgChan.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iconButton_ConfgChan.Location = new System.Drawing.Point(2, 106);
            this.iconButton_ConfgChan.Name = "iconButton_ConfgChan";
            this.iconButton_ConfgChan.Rotation = 0D;
            this.iconButton_ConfgChan.Size = new System.Drawing.Size(117, 42);
            this.iconButton_ConfgChan.TabIndex = 14;
            this.iconButton_ConfgChan.Text = "通道配置";
            this.iconButton_ConfgChan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton_ConfgChan.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.iconButton_ConfgChan.UseVisualStyleBackColor = true;
            this.iconButton_ConfgChan.Click += new System.EventHandler(this.iconButton_ConfgChan_Click);
            // 
            // iconButton_ConfgDev
            // 
            this.iconButton_ConfgDev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton_ConfgDev.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(198)))), ((int)(((byte)(198)))));
            this.iconButton_ConfgDev.FlatAppearance.BorderSize = 0;
            this.iconButton_ConfgDev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_ConfgDev.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_ConfgDev.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.iconButton_ConfgDev.ForeColor = System.Drawing.Color.Black;
            this.iconButton_ConfgDev.IconChar = FontAwesome.Sharp.IconChar.Retweet;
            this.iconButton_ConfgDev.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(101)))), ((int)(((byte)(176)))));
            this.iconButton_ConfgDev.IconSize = 27;
            this.iconButton_ConfgDev.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iconButton_ConfgDev.Location = new System.Drawing.Point(2, 40);
            this.iconButton_ConfgDev.Name = "iconButton_ConfgDev";
            this.iconButton_ConfgDev.Rotation = 0D;
            this.iconButton_ConfgDev.Size = new System.Drawing.Size(117, 36);
            this.iconButton_ConfgDev.TabIndex = 13;
            this.iconButton_ConfgDev.Text = "仪器配置";
            this.iconButton_ConfgDev.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton_ConfgDev.UseVisualStyleBackColor = false;
            this.iconButton_ConfgDev.Click += new System.EventHandler(this.iconButton_ConfgDev_Click);
            // 
            // iconButton_DockLeft
            // 
            this.iconButton_DockLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton_DockLeft.FlatAppearance.BorderSize = 0;
            this.iconButton_DockLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_DockLeft.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_DockLeft.IconChar = FontAwesome.Sharp.IconChar.AngleDoubleLeft;
            this.iconButton_DockLeft.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.iconButton_DockLeft.IconSize = 20;
            this.iconButton_DockLeft.Location = new System.Drawing.Point(96, 6);
            this.iconButton_DockLeft.Name = "iconButton_DockLeft";
            this.iconButton_DockLeft.Rotation = 0D;
            this.iconButton_DockLeft.Size = new System.Drawing.Size(20, 20);
            this.iconButton_DockLeft.TabIndex = 12;
            this.iconButton_DockLeft.UseVisualStyleBackColor = true;
            this.iconButton_DockLeft.Click += new System.EventHandler(this.iconButton_DockLeft_Click);
            // 
            // iconButton_DockRight
            // 
            this.iconButton_DockRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton_DockRight.FlatAppearance.BorderSize = 0;
            this.iconButton_DockRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_DockRight.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_DockRight.IconChar = FontAwesome.Sharp.IconChar.AngleDoubleRight;
            this.iconButton_DockRight.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.iconButton_DockRight.IconSize = 20;
            this.iconButton_DockRight.Location = new System.Drawing.Point(1, 6);
            this.iconButton_DockRight.Name = "iconButton_DockRight";
            this.iconButton_DockRight.Rotation = 0D;
            this.iconButton_DockRight.Size = new System.Drawing.Size(20, 20);
            this.iconButton_DockRight.TabIndex = 11;
            this.iconButton_DockRight.UseVisualStyleBackColor = true;
            this.iconButton_DockRight.Click += new System.EventHandler(this.iconButton_DockRight_Click);
            // 
            // panel_Project
            // 
            this.panel_Project.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.panel_Project.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Project.Controls.Add(this.ucLabel_Prj);
            this.panel_Project.Controls.Add(this.iconButton_prjClose);
            this.panel_Project.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_Project.Location = new System.Drawing.Point(992, 96);
            this.panel_Project.Name = "panel_Project";
            this.panel_Project.Size = new System.Drawing.Size(40, 552);
            this.panel_Project.TabIndex = 5;
            // 
            // ucLabel_Prj
            // 
            this.ucLabel_Prj.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucLabel_Prj.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucLabel_Prj.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(101)))), ((int)(((byte)(176)))));
            this.ucLabel_Prj.Location = new System.Drawing.Point(0, 0);
            this.ucLabel_Prj.Name = "ucLabel_Prj";
            this.ucLabel_Prj.NewText = "Proj_1";
            this.ucLabel_Prj.RotateAngle = 90;
            this.ucLabel_Prj.Size = new System.Drawing.Size(38, 507);
            this.ucLabel_Prj.TabIndex = 2;
            this.ucLabel_Prj.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // iconButton_prjClose
            // 
            this.iconButton_prjClose.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.iconButton_prjClose.FlatAppearance.BorderSize = 0;
            this.iconButton_prjClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_prjClose.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_prjClose.IconChar = FontAwesome.Sharp.IconChar.WindowClose;
            this.iconButton_prjClose.IconColor = System.Drawing.Color.Black;
            this.iconButton_prjClose.IconSize = 38;
            this.iconButton_prjClose.Location = new System.Drawing.Point(0, 510);
            this.iconButton_prjClose.Name = "iconButton_prjClose";
            this.iconButton_prjClose.Rotation = 0D;
            this.iconButton_prjClose.Size = new System.Drawing.Size(38, 40);
            this.iconButton_prjClose.TabIndex = 1;
            this.iconButton_prjClose.UseVisualStyleBackColor = true;
            this.iconButton_prjClose.Click += new System.EventHandler(this.iconButton_prjClose_Click);
            // 
            // panel_Item
            // 
            this.panel_Item.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.panel_Item.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Item.Controls.Add(this.iconButton_Stop);
            this.panel_Item.Controls.Add(this.iconButton_ForceTrig);
            this.panel_Item.Controls.Add(this.iconButton_Save);
            this.panel_Item.Controls.Add(this.iconButton_Start);
            this.panel_Item.Controls.Add(this.iconButton_DataComparison);
            this.panel_Item.Controls.Add(this.iconButton_Cal);
            this.panel_Item.Controls.Add(this.iconButton_DataBase);
            this.panel_Item.Controls.Add(this.iconButton_StateView);
            this.panel_Item.Controls.Add(this.iconButton_WaveView);
            this.panel_Item.Controls.Add(this.iconButton_SysConfig);
            this.panel_Item.Controls.Add(this.panel_State);
            this.panel_Item.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Item.Location = new System.Drawing.Point(0, 25);
            this.panel_Item.Name = "panel_Item";
            this.panel_Item.Size = new System.Drawing.Size(1032, 71);
            this.panel_Item.TabIndex = 2;
            // 
            // iconButton_Stop
            // 
            this.iconButton_Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton_Stop.FlatAppearance.BorderSize = 0;
            this.iconButton_Stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_Stop.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_Stop.IconChar = FontAwesome.Sharp.IconChar.Stop;
            this.iconButton_Stop.IconColor = System.Drawing.Color.Red;
            this.iconButton_Stop.IconSize = 30;
            this.iconButton_Stop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.iconButton_Stop.Location = new System.Drawing.Point(505, 7);
            this.iconButton_Stop.Name = "iconButton_Stop";
            this.iconButton_Stop.Rotation = 0D;
            this.iconButton_Stop.Size = new System.Drawing.Size(48, 55);
            this.iconButton_Stop.TabIndex = 21;
            this.iconButton_Stop.Text = "Stop";
            this.iconButton_Stop.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.iconButton_Stop.UseVisualStyleBackColor = true;
            this.iconButton_Stop.Click += new System.EventHandler(this.iconButton_Stop_Click);
            // 
            // iconButton_ForceTrig
            // 
            this.iconButton_ForceTrig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton_ForceTrig.FlatAppearance.BorderSize = 0;
            this.iconButton_ForceTrig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_ForceTrig.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_ForceTrig.IconChar = FontAwesome.Sharp.IconChar.HandPointUp;
            this.iconButton_ForceTrig.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.iconButton_ForceTrig.IconSize = 31;
            this.iconButton_ForceTrig.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.iconButton_ForceTrig.Location = new System.Drawing.Point(556, 7);
            this.iconButton_ForceTrig.Name = "iconButton_ForceTrig";
            this.iconButton_ForceTrig.Rotation = 0D;
            this.iconButton_ForceTrig.Size = new System.Drawing.Size(50, 55);
            this.iconButton_ForceTrig.TabIndex = 20;
            this.iconButton_ForceTrig.Text = "Force";
            this.iconButton_ForceTrig.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.iconButton_ForceTrig.UseVisualStyleBackColor = true;
            this.iconButton_ForceTrig.Click += new System.EventHandler(this.iconButton_ForceTrig_Click);
            // 
            // iconButton_Save
            // 
            this.iconButton_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton_Save.CausesValidation = false;
            this.iconButton_Save.FlatAppearance.BorderSize = 0;
            this.iconButton_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_Save.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_Save.IconChar = FontAwesome.Sharp.IconChar.Save;
            this.iconButton_Save.IconColor = System.Drawing.Color.LightSeaGreen;
            this.iconButton_Save.IconSize = 31;
            this.iconButton_Save.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.iconButton_Save.Location = new System.Drawing.Point(612, 7);
            this.iconButton_Save.Name = "iconButton_Save";
            this.iconButton_Save.Rotation = 0D;
            this.iconButton_Save.Size = new System.Drawing.Size(50, 55);
            this.iconButton_Save.TabIndex = 20;
            this.iconButton_Save.Text = "Save";
            this.iconButton_Save.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.iconButton_Save.UseVisualStyleBackColor = true;
            this.iconButton_Save.Click += new System.EventHandler(this.iconButton_Save_Click);
            // 
            // iconButton_Start
            // 
            this.iconButton_Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton_Start.CausesValidation = false;
            this.iconButton_Start.FlatAppearance.BorderSize = 0;
            this.iconButton_Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_Start.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_Start.IconChar = FontAwesome.Sharp.IconChar.Play;
            this.iconButton_Start.IconColor = System.Drawing.Color.Green;
            this.iconButton_Start.IconSize = 31;
            this.iconButton_Start.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.iconButton_Start.Location = new System.Drawing.Point(450, 7);
            this.iconButton_Start.Name = "iconButton_Start";
            this.iconButton_Start.Rotation = 0D;
            this.iconButton_Start.Size = new System.Drawing.Size(50, 55);
            this.iconButton_Start.TabIndex = 20;
            this.iconButton_Start.Text = "Start";
            this.iconButton_Start.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.iconButton_Start.UseVisualStyleBackColor = true;
            this.iconButton_Start.Click += new System.EventHandler(this.iconButton_Start_Click);
            // 
            // iconButton_DataComparison
            // 
            this.iconButton_DataComparison.FlatAppearance.BorderSize = 0;
            this.iconButton_DataComparison.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_DataComparison.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_DataComparison.IconChar = FontAwesome.Sharp.IconChar.ChartBar;
            this.iconButton_DataComparison.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.iconButton_DataComparison.IconSize = 35;
            this.iconButton_DataComparison.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.iconButton_DataComparison.Location = new System.Drawing.Point(346, 7);
            this.iconButton_DataComparison.Name = "iconButton_DataComparison";
            this.iconButton_DataComparison.Rotation = 0D;
            this.iconButton_DataComparison.Size = new System.Drawing.Size(61, 55);
            this.iconButton_DataComparison.TabIndex = 19;
            this.iconButton_DataComparison.Text = "数据对比";
            this.iconButton_DataComparison.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.iconButton_DataComparison.UseVisualStyleBackColor = true;
            this.iconButton_DataComparison.Click += new System.EventHandler(this.iconButton_DataComparison_Click);
            // 
            // iconButton_Cal
            // 
            this.iconButton_Cal.FlatAppearance.BorderSize = 0;
            this.iconButton_Cal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_Cal.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_Cal.IconChar = FontAwesome.Sharp.IconChar.Wrench;
            this.iconButton_Cal.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.iconButton_Cal.IconSize = 35;
            this.iconButton_Cal.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.iconButton_Cal.Location = new System.Drawing.Point(279, 7);
            this.iconButton_Cal.Name = "iconButton_Cal";
            this.iconButton_Cal.Rotation = 0D;
            this.iconButton_Cal.Size = new System.Drawing.Size(61, 55);
            this.iconButton_Cal.TabIndex = 19;
            this.iconButton_Cal.Text = "校   准";
            this.iconButton_Cal.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.iconButton_Cal.UseVisualStyleBackColor = true;
            this.iconButton_Cal.Click += new System.EventHandler(this.iconButton_Cal_Click);
            // 
            // iconButton_DataBase
            // 
            this.iconButton_DataBase.FlatAppearance.BorderSize = 0;
            this.iconButton_DataBase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_DataBase.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_DataBase.IconChar = FontAwesome.Sharp.IconChar.Database;
            this.iconButton_DataBase.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.iconButton_DataBase.IconSize = 35;
            this.iconButton_DataBase.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.iconButton_DataBase.Location = new System.Drawing.Point(212, 7);
            this.iconButton_DataBase.Name = "iconButton_DataBase";
            this.iconButton_DataBase.Rotation = 0D;
            this.iconButton_DataBase.Size = new System.Drawing.Size(61, 55);
            this.iconButton_DataBase.TabIndex = 18;
            this.iconButton_DataBase.Text = "数据存储";
            this.iconButton_DataBase.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.iconButton_DataBase.UseVisualStyleBackColor = true;
            this.iconButton_DataBase.Click += new System.EventHandler(this.iconButton_DataBase_Click);
            // 
            // iconButton_StateView
            // 
            this.iconButton_StateView.FlatAppearance.BorderSize = 0;
            this.iconButton_StateView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_StateView.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_StateView.IconChar = FontAwesome.Sharp.IconChar.Newspaper;
            this.iconButton_StateView.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.iconButton_StateView.IconSize = 35;
            this.iconButton_StateView.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.iconButton_StateView.Location = new System.Drawing.Point(145, 7);
            this.iconButton_StateView.Name = "iconButton_StateView";
            this.iconButton_StateView.Rotation = 0D;
            this.iconButton_StateView.Size = new System.Drawing.Size(61, 55);
            this.iconButton_StateView.TabIndex = 17;
            this.iconButton_StateView.Text = "运行监测";
            this.iconButton_StateView.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.iconButton_StateView.UseVisualStyleBackColor = true;
            this.iconButton_StateView.Click += new System.EventHandler(this.iconButton_StateView_Click);
            // 
            // iconButton_WaveView
            // 
            this.iconButton_WaveView.FlatAppearance.BorderSize = 0;
            this.iconButton_WaveView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_WaveView.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_WaveView.IconChar = FontAwesome.Sharp.IconChar.ChartArea;
            this.iconButton_WaveView.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.iconButton_WaveView.IconSize = 35;
            this.iconButton_WaveView.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.iconButton_WaveView.Location = new System.Drawing.Point(78, 7);
            this.iconButton_WaveView.Name = "iconButton_WaveView";
            this.iconButton_WaveView.Rotation = 0D;
            this.iconButton_WaveView.Size = new System.Drawing.Size(61, 55);
            this.iconButton_WaveView.TabIndex = 16;
            this.iconButton_WaveView.Text = "波形监测";
            this.iconButton_WaveView.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.iconButton_WaveView.UseVisualStyleBackColor = true;
            this.iconButton_WaveView.Click += new System.EventHandler(this.iconButton_WaveView_Click);
            // 
            // iconButton_SysConfig
            // 
            this.iconButton_SysConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.iconButton_SysConfig.FlatAppearance.BorderSize = 0;
            this.iconButton_SysConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton_SysConfig.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButton_SysConfig.IconChar = FontAwesome.Sharp.IconChar.Cog;
            this.iconButton_SysConfig.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.iconButton_SysConfig.IconSize = 35;
            this.iconButton_SysConfig.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.iconButton_SysConfig.Location = new System.Drawing.Point(11, 7);
            this.iconButton_SysConfig.Name = "iconButton_SysConfig";
            this.iconButton_SysConfig.Rotation = 0D;
            this.iconButton_SysConfig.Size = new System.Drawing.Size(61, 55);
            this.iconButton_SysConfig.TabIndex = 15;
            this.iconButton_SysConfig.Text = "系统配置";
            this.iconButton_SysConfig.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.iconButton_SysConfig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.iconButton_SysConfig.UseVisualStyleBackColor = false;
            this.iconButton_SysConfig.Click += new System.EventHandler(this.iconButton_SysConfig_Click);
            // 
            // panel_State
            // 
            this.panel_State.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_State.Controls.Add(this.label_Storage);
            this.panel_State.Controls.Add(this.label_Recive);
            this.panel_State.Controls.Add(this.label_Wait);
            this.panel_State.Location = new System.Drawing.Point(687, 13);
            this.panel_State.Name = "panel_State";
            this.panel_State.Size = new System.Drawing.Size(329, 41);
            this.panel_State.TabIndex = 14;
            // 
            // label_Storage
            // 
            this.label_Storage.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label_Storage.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Storage.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label_Storage.Location = new System.Drawing.Point(225, 13);
            this.label_Storage.Name = "label_Storage";
            this.label_Storage.Size = new System.Drawing.Size(100, 15);
            this.label_Storage.TabIndex = 2;
            this.label_Storage.Text = "存储数据库";
            this.label_Storage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Recive
            // 
            this.label_Recive.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label_Recive.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Recive.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label_Recive.Location = new System.Drawing.Point(124, 13);
            this.label_Recive.Name = "label_Recive";
            this.label_Recive.Size = new System.Drawing.Size(100, 15);
            this.label_Recive.TabIndex = 1;
            this.label_Recive.Text = "接收数据";
            this.label_Recive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Wait
            // 
            this.label_Wait.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label_Wait.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Wait.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label_Wait.Location = new System.Drawing.Point(23, 13);
            this.label_Wait.Name = "label_Wait";
            this.label_Wait.Size = new System.Drawing.Size(100, 15);
            this.label_Wait.TabIndex = 0;
            this.label_Wait.Text = "等待触发";
            this.label_Wait.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip_Main
            // 
            this.menuStrip_Main.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.工具ToolStripMenuItem,
            this.操作ToolStripMenuItem,
            this.用户ToolStripMenuItem,
            this.ToolStripMenuItem_Help});
            this.menuStrip_Main.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_Main.Name = "menuStrip_Main";
            this.menuStrip_Main.Size = new System.Drawing.Size(1032, 25);
            this.menuStrip_Main.TabIndex = 0;
            this.menuStrip_Main.Text = "菜单栏";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iconMenuItem_CreateProj,
            this.toolStripMenuItem_OpenFile});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // iconMenuItem_CreateProj
            // 
            this.iconMenuItem_CreateProj.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconMenuItem_CreateProj.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconMenuItem_CreateProj.IconColor = System.Drawing.Color.Black;
            this.iconMenuItem_CreateProj.IconSize = 16;
            this.iconMenuItem_CreateProj.Name = "iconMenuItem_CreateProj";
            this.iconMenuItem_CreateProj.Rotation = 0D;
            this.iconMenuItem_CreateProj.Size = new System.Drawing.Size(100, 22);
            this.iconMenuItem_CreateProj.Text = "新建";
            this.iconMenuItem_CreateProj.Click += new System.EventHandler(this.iconMenuItem_CreateProj_Click);
            // 
            // toolStripMenuItem_OpenFile
            // 
            this.toolStripMenuItem_OpenFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_LocalOpen,
            this.数据库ToolStripMenuItem});
            this.toolStripMenuItem_OpenFile.Name = "toolStripMenuItem_OpenFile";
            this.toolStripMenuItem_OpenFile.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem_OpenFile.Text = "打开";
            // 
            // ToolStripMenuItem_LocalOpen
            // 
            this.ToolStripMenuItem_LocalOpen.Name = "ToolStripMenuItem_LocalOpen";
            this.ToolStripMenuItem_LocalOpen.Size = new System.Drawing.Size(112, 22);
            this.ToolStripMenuItem_LocalOpen.Text = "本地";
            this.ToolStripMenuItem_LocalOpen.Click += new System.EventHandler(this.ToolStripMenuItem_LocalOpen_Click);
            // 
            // 数据库ToolStripMenuItem
            // 
            this.数据库ToolStripMenuItem.Name = "数据库ToolStripMenuItem";
            this.数据库ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.数据库ToolStripMenuItem.Text = "数据库";
            this.数据库ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_LoadDB_Click);
            // 
            // 工具ToolStripMenuItem
            // 
            this.工具ToolStripMenuItem.Name = "工具ToolStripMenuItem";
            this.工具ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.工具ToolStripMenuItem.Text = "工具";
            // 
            // 操作ToolStripMenuItem
            // 
            this.操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemTranLang,
            this.ToolStripMenuItem_SaveDataBase});
            this.操作ToolStripMenuItem.Name = "操作ToolStripMenuItem";
            this.操作ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.操作ToolStripMenuItem.Text = "操作";
            // 
            // ToolStripMenuItemTranLang
            // 
            this.ToolStripMenuItemTranLang.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_zhCN,
            this.ToolStripMenuItem_enUS});
            this.ToolStripMenuItemTranLang.Name = "ToolStripMenuItemTranLang";
            this.ToolStripMenuItemTranLang.Size = new System.Drawing.Size(136, 22);
            this.ToolStripMenuItemTranLang.Text = "语言转换";
            // 
            // ToolStripMenuItem_zhCN
            // 
            this.ToolStripMenuItem_zhCN.Name = "ToolStripMenuItem_zhCN";
            this.ToolStripMenuItem_zhCN.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItem_zhCN.Text = "中文";
            this.ToolStripMenuItem_zhCN.Click += new System.EventHandler(this.ToolStripMenuItem_zhCN_Click);
            // 
            // ToolStripMenuItem_enUS
            // 
            this.ToolStripMenuItem_enUS.Name = "ToolStripMenuItem_enUS";
            this.ToolStripMenuItem_enUS.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItem_enUS.Text = "英文";
            this.ToolStripMenuItem_enUS.Click += new System.EventHandler(this.ToolStripMenuItem_enUS_Click);
            // 
            // ToolStripMenuItem_SaveDataBase
            // 
            this.ToolStripMenuItem_SaveDataBase.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iconMenuItem_OpenSaveDb,
            this.iconMenuItem_CloseSaveDb});
            this.ToolStripMenuItem_SaveDataBase.Name = "ToolStripMenuItem_SaveDataBase";
            this.ToolStripMenuItem_SaveDataBase.Size = new System.Drawing.Size(136, 22);
            this.ToolStripMenuItem_SaveDataBase.Text = "存储数据库";
            // 
            // iconMenuItem_OpenSaveDb
            // 
            this.iconMenuItem_OpenSaveDb.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconMenuItem_OpenSaveDb.IconChar = FontAwesome.Sharp.IconChar.Check;
            this.iconMenuItem_OpenSaveDb.IconColor = System.Drawing.Color.Black;
            this.iconMenuItem_OpenSaveDb.IconSize = 16;
            this.iconMenuItem_OpenSaveDb.Name = "iconMenuItem_OpenSaveDb";
            this.iconMenuItem_OpenSaveDb.Rotation = 0D;
            this.iconMenuItem_OpenSaveDb.Size = new System.Drawing.Size(100, 22);
            this.iconMenuItem_OpenSaveDb.Text = "打开";
            this.iconMenuItem_OpenSaveDb.Click += new System.EventHandler(this.iconMenuItem_OpenSaveDb_Click);
            // 
            // iconMenuItem_CloseSaveDb
            // 
            this.iconMenuItem_CloseSaveDb.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconMenuItem_CloseSaveDb.IconChar = FontAwesome.Sharp.IconChar.Times;
            this.iconMenuItem_CloseSaveDb.IconColor = System.Drawing.Color.Black;
            this.iconMenuItem_CloseSaveDb.IconSize = 16;
            this.iconMenuItem_CloseSaveDb.Name = "iconMenuItem_CloseSaveDb";
            this.iconMenuItem_CloseSaveDb.Rotation = 0D;
            this.iconMenuItem_CloseSaveDb.Size = new System.Drawing.Size(100, 22);
            this.iconMenuItem_CloseSaveDb.Text = "关闭";
            this.iconMenuItem_CloseSaveDb.Click += new System.EventHandler(this.iconMenuItem_CloseSaveDb_Click);
            // 
            // 用户ToolStripMenuItem
            // 
            this.用户ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_AddUser});
            this.用户ToolStripMenuItem.Name = "用户ToolStripMenuItem";
            this.用户ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.用户ToolStripMenuItem.Text = "用户";
            // 
            // ToolStripMenuItem_AddUser
            // 
            this.ToolStripMenuItem_AddUser.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.ToolStripMenuItem_AddUser.IconChar = FontAwesome.Sharp.IconChar.AddressBook;
            this.ToolStripMenuItem_AddUser.IconColor = System.Drawing.Color.DarkOrange;
            this.ToolStripMenuItem_AddUser.IconSize = 16;
            this.ToolStripMenuItem_AddUser.Name = "ToolStripMenuItem_AddUser";
            this.ToolStripMenuItem_AddUser.Rotation = 0D;
            this.ToolStripMenuItem_AddUser.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuItem_AddUser.Text = "用户管理";
            this.ToolStripMenuItem_AddUser.Click += new System.EventHandler(this.ToolStripMenuItem_AddUser_Click);
            // 
            // ToolStripMenuItem_Help
            // 
            this.ToolStripMenuItem_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iconMenuItem1,
            this.iconMenuItem2,
            this.iconMenuItem3});
            this.ToolStripMenuItem_Help.Name = "ToolStripMenuItem_Help";
            this.ToolStripMenuItem_Help.Size = new System.Drawing.Size(44, 21);
            this.ToolStripMenuItem_Help.Text = "帮助";
            // 
            // iconMenuItem1
            // 
            this.iconMenuItem1.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconMenuItem1.IconChar = FontAwesome.Sharp.IconChar.Parking;
            this.iconMenuItem1.IconColor = System.Drawing.Color.ForestGreen;
            this.iconMenuItem1.IconSize = 16;
            this.iconMenuItem1.Name = "iconMenuItem1";
            this.iconMenuItem1.Rotation = 0D;
            this.iconMenuItem1.Size = new System.Drawing.Size(188, 30);
            this.iconMenuItem1.Text = "工程信息";
            this.iconMenuItem1.Click += new System.EventHandler(this.iconMenuItem1_Click);
            // 
            // iconMenuItem2
            // 
            this.iconMenuItem2.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconMenuItem2.IconChar = FontAwesome.Sharp.IconChar.HSquare;
            this.iconMenuItem2.IconColor = System.Drawing.Color.Teal;
            this.iconMenuItem2.IconSize = 16;
            this.iconMenuItem2.Name = "iconMenuItem2";
            this.iconMenuItem2.Rotation = 0D;
            this.iconMenuItem2.Size = new System.Drawing.Size(188, 30);
            this.iconMenuItem2.Text = "帮助手册";
            this.iconMenuItem2.Click += new System.EventHandler(this.iconMenuItem2_Click);
            // 
            // iconMenuItem3
            // 
            this.iconMenuItem3.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconMenuItem3.IconChar = FontAwesome.Sharp.IconChar.RProject;
            this.iconMenuItem3.IconColor = System.Drawing.Color.Orange;
            this.iconMenuItem3.IconSize = 16;
            this.iconMenuItem3.Name = "iconMenuItem3";
            this.iconMenuItem3.Rotation = 0D;
            this.iconMenuItem3.Size = new System.Drawing.Size(188, 30);
            this.iconMenuItem3.Text = "关于此软件";
            this.iconMenuItem3.Click += new System.EventHandler(this.iconMenuItem3_Click);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 648);
            this.Controls.Add(this.panel_Module);
            this.Controls.Add(this.panel_FunctionItem);
            this.Controls.Add(this.panel_Project);
            this.Controls.Add(this.panel_Item);
            this.Controls.Add(this.menuStrip_Main);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip_Main;
            this.Name = "Form_Main";
            this.Text = "DAQ30075A";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Main_FormClosing);
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.SizeChanged += new System.EventHandler(this.Form_Main_SizeChanged);
            this.Resize += new System.EventHandler(this.Form_Main_Resize_1);
            this.panel_FunctionItem.ResumeLayout(false);
            this.panel_Project.ResumeLayout(false);
            this.panel_Item.ResumeLayout(false);
            this.panel_State.ResumeLayout(false);
            this.menuStrip_Main.ResumeLayout(false);
            this.menuStrip_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel_Item;
        private FontAwesome.Sharp.IconButton iconButton_SysConfig;
        private System.Windows.Forms.Panel panel_State;
        private System.Windows.Forms.Label label_Storage;
        private System.Windows.Forms.Label label_Recive;
        private System.Windows.Forms.Label label_Wait;
        private FontAwesome.Sharp.IconButton iconButton_Stop;
        private FontAwesome.Sharp.IconButton iconButton_Start;
        private FontAwesome.Sharp.IconButton iconButton_Cal;
        private FontAwesome.Sharp.IconButton iconButton_DataBase;
        private FontAwesome.Sharp.IconButton iconButton_StateView;
        private FontAwesome.Sharp.IconButton iconButton_WaveView;
        private System.Windows.Forms.Panel panel_FunctionItem;
        private FontAwesome.Sharp.IconButton iconButton_ConfigRecord;
        private FontAwesome.Sharp.IconButton iconButton_ConfigView;
        private FontAwesome.Sharp.IconButton iconButton_ConfgChan;
        private FontAwesome.Sharp.IconButton iconButton_ConfgDev;
        private FontAwesome.Sharp.IconButton iconButton_DockLeft;
        private FontAwesome.Sharp.IconButton iconButton_DockRight;
        private System.Windows.Forms.Panel panel_Module;
        private System.Windows.Forms.Panel panel_Project;
        private FontAwesome.Sharp.IconButton iconButton_prjClose;
        private UserControls.UCLabel ucLabel_Prj;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 用户ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Help;
        private System.Windows.Forms.MenuStrip menuStrip_Main;
        private FontAwesome.Sharp.IconMenuItem iconMenuItem_CreateProj;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_OpenFile;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_LocalOpen;
        private System.Windows.Forms.ToolStripMenuItem 数据库ToolStripMenuItem;
        private FontAwesome.Sharp.IconButton iconButton_ForceTrig;
        private FontAwesome.Sharp.IconButton iconButton_Save;
        private FontAwesome.Sharp.IconButton iconButton_DataComparison;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemTranLang;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_zhCN;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_enUS;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_SaveDataBase;
        private FontAwesome.Sharp.IconMenuItem iconMenuItem_OpenSaveDb;
        private FontAwesome.Sharp.IconMenuItem iconMenuItem_CloseSaveDb;
        private FontAwesome.Sharp.IconMenuItem iconMenuItem1;
        private FontAwesome.Sharp.IconMenuItem iconMenuItem2;
        private FontAwesome.Sharp.IconMenuItem iconMenuItem3;
        private FontAwesome.Sharp.IconMenuItem ToolStripMenuItem_AddUser;
    }
}