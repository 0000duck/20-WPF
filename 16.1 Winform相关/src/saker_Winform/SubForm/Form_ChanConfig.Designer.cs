namespace saker_Winform.SubForm
{
    partial class Form_ChanConfig
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ChanConfig));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AllChooseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CancelChooseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView_ChanSet = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Collect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Record = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DeviceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChannelID = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Tag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TagDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeasureType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Scale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Offset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Impedance = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Coupling = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ProbeRatio = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.SN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Open = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChannelDelayTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Valid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBox_TrigHoldUnit = new System.Windows.Forms.ComboBox();
            this.comboBox_TriggerLevelUnit = new System.Windows.Forms.ComboBox();
            this.comboBox_HoriOffsetUnit = new System.Windows.Forms.ComboBox();
            this.comboBox_HorTimeUnit = new System.Windows.Forms.ComboBox();
            this.textBox_MainIP = new System.Windows.Forms.TextBox();
            this.textBox_HorTime = new System.Windows.Forms.TextBox();
            this.textBox_HoriOffset = new System.Windows.Forms.TextBox();
            this.textBox_TrigHold = new System.Windows.Forms.TextBox();
            this.textBox_TriggerLevel = new System.Windows.Forms.TextBox();
            this.comboBox_StorgeDepth = new System.Windows.Forms.ComboBox();
            this.comboBox_TrigStyle = new System.Windows.Forms.ComboBox();
            this.comboBox_TriggeSource = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_MainIp = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_Config = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ChanSet)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AllChooseToolStripMenuItem,
            this.CancelChooseToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 48);
            // 
            // AllChooseToolStripMenuItem
            // 
            this.AllChooseToolStripMenuItem.Image = global::saker_Winform.Properties.Resources.全选1;
            this.AllChooseToolStripMenuItem.Name = "AllChooseToolStripMenuItem";
            this.AllChooseToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.AllChooseToolStripMenuItem.Text = "全选";
            this.AllChooseToolStripMenuItem.Click += new System.EventHandler(this.AllChooseToolStripMenuItem_Click);
            // 
            // CancelChooseToolStripMenuItem
            // 
            this.CancelChooseToolStripMenuItem.Image = global::saker_Winform.Properties.Resources.取消全选;
            this.CancelChooseToolStripMenuItem.Name = "CancelChooseToolStripMenuItem";
            this.CancelChooseToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.CancelChooseToolStripMenuItem.Text = "取消全选";
            this.CancelChooseToolStripMenuItem.Click += new System.EventHandler(this.CancelChooseToolStripMenuItem_Click);
            // 
            // dataGridView_ChanSet
            // 
            this.dataGridView_ChanSet.AllowUserToAddRows = false;
            this.dataGridView_ChanSet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_ChanSet.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_ChanSet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_ChanSet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_ChanSet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Collect,
            this.Record,
            this.DeviceName,
            this.ChannelID,
            this.Tag,
            this.TagDesc,
            this.MeasureType,
            this.Scale,
            this.Offset,
            this.Impedance,
            this.Coupling,
            this.ProbeRatio,
            this.SN,
            this.Open,
            this.ChannelDelayTime,
            this.Valid});
            this.dataGridView_ChanSet.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_ChanSet.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView_ChanSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_ChanSet.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridView_ChanSet.Location = new System.Drawing.Point(0, 159);
            this.dataGridView_ChanSet.Name = "dataGridView_ChanSet";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_ChanSet.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView_ChanSet.RowHeadersWidth = 25;
            this.dataGridView_ChanSet.RowTemplate.Height = 23;
            this.dataGridView_ChanSet.Size = new System.Drawing.Size(1027, 437);
            this.dataGridView_ChanSet.TabIndex = 3;
            this.dataGridView_ChanSet.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_ChanSet_CellContentClick);
            this.dataGridView_ChanSet.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_ChanSet_CellDoubleClick);
            this.dataGridView_ChanSet.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView_ChanSet_CellValidating);
            this.dataGridView_ChanSet.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView_ChanSet_MouseDown);
            this.dataGridView_ChanSet.MouseLeave += new System.EventHandler(this.dataGridView_ChanSet_MouseLeave);
            this.dataGridView_ChanSet.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridView_ChanSet_MouseMove);
            this.dataGridView_ChanSet.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGridView_ChanSet_MouseUp);
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Collect
            // 
            this.Collect.DataPropertyName = "Collect";
            this.Collect.HeaderText = "采集";
            this.Collect.Name = "Collect";
            // 
            // Record
            // 
            this.Record.DataPropertyName = "Record";
            this.Record.HeaderText = "记录";
            this.Record.Name = "Record";
            // 
            // DeviceName
            // 
            this.DeviceName.DataPropertyName = "DeviceName";
            this.DeviceName.HeaderText = "设备";
            this.DeviceName.Name = "DeviceName";
            this.DeviceName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DeviceName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ChannelID
            // 
            this.ChannelID.DataPropertyName = "ChannelID";
            this.ChannelID.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ChannelID.HeaderText = "通道";
            this.ChannelID.Items.AddRange(new object[] {
            "CH1",
            "CH2",
            "CH3",
            "CH4"});
            this.ChannelID.Name = "ChannelID";
            this.ChannelID.ReadOnly = true;
            this.ChannelID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Tag
            // 
            this.Tag.DataPropertyName = "Tag";
            this.Tag.HeaderText = "通道标记";
            this.Tag.MaxInputLength = 7;
            this.Tag.Name = "Tag";
            this.Tag.ReadOnly = true;
            this.Tag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TagDesc
            // 
            this.TagDesc.DataPropertyName = "TagDesc";
            this.TagDesc.HeaderText = "标记注释";
            this.TagDesc.MaxInputLength = 13;
            this.TagDesc.Name = "TagDesc";
            this.TagDesc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MeasureType
            // 
            this.MeasureType.DataPropertyName = "MeasureType";
            this.MeasureType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.MeasureType.HeaderText = "测量类型";
            this.MeasureType.Items.AddRange(new object[] {
            "MARX",
            "水线",
            "感应腔",
            "MLT"});
            this.MeasureType.Name = "MeasureType";
            // 
            // Scale
            // 
            this.Scale.DataPropertyName = "Scale";
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = "\"0\"";
            this.Scale.DefaultCellStyle = dataGridViewCellStyle2;
            this.Scale.HeaderText = "Scale(V)";
            this.Scale.Name = "Scale";
            this.Scale.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Offset
            // 
            this.Offset.DataPropertyName = "Offset";
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = "\"0\"";
            this.Offset.DefaultCellStyle = dataGridViewCellStyle3;
            this.Offset.HeaderText = "Offset（V）";
            this.Offset.Name = "Offset";
            this.Offset.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Impedance
            // 
            this.Impedance.DataPropertyName = "Impedance";
            this.Impedance.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Impedance.HeaderText = "阻抗(Ω）";
            this.Impedance.Items.AddRange(new object[] {
            "50",
            "1M"});
            this.Impedance.Name = "Impedance";
            // 
            // Coupling
            // 
            this.Coupling.DataPropertyName = "Coupling";
            this.Coupling.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Coupling.HeaderText = "耦合";
            this.Coupling.Items.AddRange(new object[] {
            "DC",
            "AC",
            "GND"});
            this.Coupling.Name = "Coupling";
            this.Coupling.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ProbeRatio
            // 
            this.ProbeRatio.DataPropertyName = "ProbeRatio";
            this.ProbeRatio.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ProbeRatio.HeaderText = "探头比";
            this.ProbeRatio.Items.AddRange(new object[] {
            "*1",
            "*10"});
            this.ProbeRatio.Name = "ProbeRatio";
            this.ProbeRatio.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // SN
            // 
            this.SN.DataPropertyName = "SN";
            this.SN.HeaderText = "SN";
            this.SN.Name = "SN";
            this.SN.ReadOnly = true;
            this.SN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SN.Visible = false;
            // 
            // Open
            // 
            this.Open.DataPropertyName = "Open";
            this.Open.HeaderText = "Open";
            this.Open.Name = "Open";
            this.Open.ReadOnly = true;
            this.Open.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Open.Visible = false;
            // 
            // ChannelDelayTime
            // 
            this.ChannelDelayTime.DataPropertyName = "ChannelDelayTime";
            this.ChannelDelayTime.HeaderText = "通道延迟";
            this.ChannelDelayTime.Name = "ChannelDelayTime";
            this.ChannelDelayTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Valid
            // 
            this.Valid.DataPropertyName = "Valid";
            this.Valid.HeaderText = "Valid";
            this.Valid.Name = "Valid";
            this.Valid.ReadOnly = true;
            this.Valid.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Valid.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel2.Controls.Add(this.comboBox_TrigHoldUnit);
            this.panel2.Controls.Add(this.comboBox_TriggerLevelUnit);
            this.panel2.Controls.Add(this.comboBox_HoriOffsetUnit);
            this.panel2.Controls.Add(this.comboBox_HorTimeUnit);
            this.panel2.Controls.Add(this.textBox_MainIP);
            this.panel2.Controls.Add(this.textBox_HorTime);
            this.panel2.Controls.Add(this.textBox_HoriOffset);
            this.panel2.Controls.Add(this.textBox_TrigHold);
            this.panel2.Controls.Add(this.textBox_TriggerLevel);
            this.panel2.Controls.Add(this.comboBox_StorgeDepth);
            this.panel2.Controls.Add(this.comboBox_TrigStyle);
            this.panel2.Controls.Add(this.comboBox_TriggeSource);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label_MainIp);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 32);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1027, 127);
            this.panel2.TabIndex = 1;
            // 
            // comboBox_TrigHoldUnit
            // 
            this.comboBox_TrigHoldUnit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox_TrigHoldUnit.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_TrigHoldUnit.FormattingEnabled = true;
            this.comboBox_TrigHoldUnit.Items.AddRange(new object[] {
            "s",
            "ms",
            "us",
            "ns"});
            this.comboBox_TrigHoldUnit.Location = new System.Drawing.Point(814, 32);
            this.comboBox_TrigHoldUnit.Name = "comboBox_TrigHoldUnit";
            this.comboBox_TrigHoldUnit.Size = new System.Drawing.Size(44, 24);
            this.comboBox_TrigHoldUnit.TabIndex = 5;
            // 
            // comboBox_TriggerLevelUnit
            // 
            this.comboBox_TriggerLevelUnit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox_TriggerLevelUnit.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_TriggerLevelUnit.FormattingEnabled = true;
            this.comboBox_TriggerLevelUnit.Items.AddRange(new object[] {
            "V",
            "mV",
            "uV"});
            this.comboBox_TriggerLevelUnit.Location = new System.Drawing.Point(602, 33);
            this.comboBox_TriggerLevelUnit.Name = "comboBox_TriggerLevelUnit";
            this.comboBox_TriggerLevelUnit.Size = new System.Drawing.Size(42, 24);
            this.comboBox_TriggerLevelUnit.TabIndex = 5;
            // 
            // comboBox_HoriOffsetUnit
            // 
            this.comboBox_HoriOffsetUnit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox_HoriOffsetUnit.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_HoriOffsetUnit.FormattingEnabled = true;
            this.comboBox_HoriOffsetUnit.Items.AddRange(new object[] {
            "s",
            "ms",
            "us",
            "ns",
            "ps"});
            this.comboBox_HoriOffsetUnit.Location = new System.Drawing.Point(383, 77);
            this.comboBox_HoriOffsetUnit.Name = "comboBox_HoriOffsetUnit";
            this.comboBox_HoriOffsetUnit.Size = new System.Drawing.Size(44, 24);
            this.comboBox_HoriOffsetUnit.TabIndex = 5;
            // 
            // comboBox_HorTimeUnit
            // 
            this.comboBox_HorTimeUnit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox_HorTimeUnit.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_HorTimeUnit.FormattingEnabled = true;
            this.comboBox_HorTimeUnit.Items.AddRange(new object[] {
            "s",
            "ms",
            "us",
            "ns",
            "ps"});
            this.comboBox_HorTimeUnit.Location = new System.Drawing.Point(158, 78);
            this.comboBox_HorTimeUnit.Name = "comboBox_HorTimeUnit";
            this.comboBox_HorTimeUnit.Size = new System.Drawing.Size(45, 24);
            this.comboBox_HorTimeUnit.TabIndex = 5;
            // 
            // textBox_MainIP
            // 
            this.textBox_MainIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_MainIP.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_MainIP.Location = new System.Drawing.Point(750, 82);
            this.textBox_MainIP.Name = "textBox_MainIP";
            this.textBox_MainIP.Size = new System.Drawing.Size(108, 21);
            this.textBox_MainIP.TabIndex = 3;
            // 
            // textBox_HorTime
            // 
            this.textBox_HorTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_HorTime.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_HorTime.Location = new System.Drawing.Point(88, 79);
            this.textBox_HorTime.Name = "textBox_HorTime";
            this.textBox_HorTime.Size = new System.Drawing.Size(63, 21);
            this.textBox_HorTime.TabIndex = 3;
            // 
            // textBox_HoriOffset
            // 
            this.textBox_HoriOffset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_HoriOffset.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_HoriOffset.Location = new System.Drawing.Point(308, 78);
            this.textBox_HoriOffset.Name = "textBox_HoriOffset";
            this.textBox_HoriOffset.Size = new System.Drawing.Size(68, 21);
            this.textBox_HoriOffset.TabIndex = 3;
            // 
            // textBox_TrigHold
            // 
            this.textBox_TrigHold.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_TrigHold.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_TrigHold.Location = new System.Drawing.Point(750, 34);
            this.textBox_TrigHold.Multiline = true;
            this.textBox_TrigHold.Name = "textBox_TrigHold";
            this.textBox_TrigHold.Size = new System.Drawing.Size(57, 21);
            this.textBox_TrigHold.TabIndex = 3;
            // 
            // textBox_TriggerLevel
            // 
            this.textBox_TriggerLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_TriggerLevel.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_TriggerLevel.Location = new System.Drawing.Point(527, 32);
            this.textBox_TriggerLevel.Multiline = true;
            this.textBox_TriggerLevel.Name = "textBox_TriggerLevel";
            this.textBox_TriggerLevel.Size = new System.Drawing.Size(69, 24);
            this.textBox_TriggerLevel.TabIndex = 3;
            // 
            // comboBox_StorgeDepth
            // 
            this.comboBox_StorgeDepth.AutoCompleteCustomSource.AddRange(new string[] {
            "External Trig",
            "CH1",
            "CH2",
            "CH3",
            "CH4"});
            this.comboBox_StorgeDepth.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox_StorgeDepth.Font = new System.Drawing.Font("微软雅黑", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_StorgeDepth.FormattingEnabled = true;
            this.comboBox_StorgeDepth.Items.AddRange(new object[] {
            "1K",
            "10K",
            "100K",
            "1M"});
            this.comboBox_StorgeDepth.Location = new System.Drawing.Point(529, 79);
            this.comboBox_StorgeDepth.Name = "comboBox_StorgeDepth";
            this.comboBox_StorgeDepth.Size = new System.Drawing.Size(69, 22);
            this.comboBox_StorgeDepth.TabIndex = 2;
            // 
            // comboBox_TrigStyle
            // 
            this.comboBox_TrigStyle.AutoCompleteCustomSource.AddRange(new string[] {
            "External Trig",
            "CH1",
            "CH2",
            "CH3",
            "CH4"});
            this.comboBox_TrigStyle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox_TrigStyle.Font = new System.Drawing.Font("微软雅黑", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_TrigStyle.FormattingEnabled = true;
            this.comboBox_TrigStyle.Items.AddRange(new object[] {
            "SINGLE"});
            this.comboBox_TrigStyle.Location = new System.Drawing.Point(308, 31);
            this.comboBox_TrigStyle.Name = "comboBox_TrigStyle";
            this.comboBox_TrigStyle.Size = new System.Drawing.Size(68, 22);
            this.comboBox_TrigStyle.TabIndex = 2;
            // 
            // comboBox_TriggeSource
            // 
            this.comboBox_TriggeSource.AutoCompleteCustomSource.AddRange(new string[] {
            "External Trig",
            "CH1",
            "CH2",
            "CH3",
            "CH4"});
            this.comboBox_TriggeSource.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox_TriggeSource.Font = new System.Drawing.Font("微软雅黑", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_TriggeSource.FormattingEnabled = true;
            this.comboBox_TriggeSource.Items.AddRange(new object[] {
            "EXT",
            "CHAN1",
            "CHAN2",
            "CHAN3",
            "CHAN4"});
            this.comboBox_TriggeSource.Location = new System.Drawing.Point(88, 30);
            this.comboBox_TriggeSource.Name = "comboBox_TriggeSource";
            this.comboBox_TriggeSource.Size = new System.Drawing.Size(63, 22);
            this.comboBox_TriggeSource.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(444, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 19);
            this.label6.TabIndex = 0;
            this.label6.Text = "存储深度：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(221, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 19);
            this.label5.TabIndex = 0;
            this.label5.Text = "水平偏移：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(5, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 19);
            this.label4.TabIndex = 0;
            this.label4.Text = "水平时基：";
            // 
            // label_MainIp
            // 
            this.label_MainIp.AutoSize = true;
            this.label_MainIp.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_MainIp.Location = new System.Drawing.Point(662, 82);
            this.label_MainIp.Name = "label_MainIp";
            this.label_MainIp.Size = new System.Drawing.Size(65, 19);
            this.label_MainIp.TabIndex = 0;
            this.label_MainIp.Text = "主机IP：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(662, 34);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 19);
            this.label8.TabIndex = 0;
            this.label8.Text = "触发释抑：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(444, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 19);
            this.label3.TabIndex = 0;
            this.label3.Text = "触发电平：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(221, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 19);
            this.label2.TabIndex = 0;
            this.label2.Text = "触发方式：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(8, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "触发源：";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198)))));
            this.panel1.Controls.Add(this.button_Config);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1027, 32);
            this.panel1.TabIndex = 0;
            // 
            // button_Config
            // 
            this.button_Config.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Config.BackColor = System.Drawing.Color.DarkGray;
            this.button_Config.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Config.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Config.ForeColor = System.Drawing.SystemColors.Control;
            this.button_Config.Location = new System.Drawing.Point(940, 4);
            this.button_Config.Name = "button_Config";
            this.button_Config.Size = new System.Drawing.Size(75, 23);
            this.button_Config.TabIndex = 2;
            this.button_Config.Text = "应用";
            this.button_Config.UseVisualStyleBackColor = false;
            this.button_Config.Click += new System.EventHandler(this.button_Config_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.SystemColors.Control;
            this.label7.Location = new System.Drawing.Point(12, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 22);
            this.label7.TabIndex = 1;
            this.label7.Text = "通道配置";
            // 
            // Form_ChanConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 596);
            this.Controls.Add(this.dataGridView_ChanSet);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_ChanConfig";
            this.Text = "Form_ChanConfig";
            this.Load += new System.EventHandler(this.Form_ChanConfig_Load);
            this.Click += new System.EventHandler(this.Form_ChanConfig_Click);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ChanSet)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView_ChanSet;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_Config;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_TriggerLevel;
        private System.Windows.Forms.ComboBox comboBox_StorgeDepth;
        private System.Windows.Forms.ComboBox comboBox_TriggeSource;
        private System.Windows.Forms.TextBox textBox_HoriOffset;
        private System.Windows.Forms.TextBox textBox_HorTime;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem CancelChooseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AllChooseToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBox_TrigStyle;
        private System.Windows.Forms.TextBox textBox_TrigHold;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox_HoriOffsetUnit;
        private System.Windows.Forms.ComboBox comboBox_HorTimeUnit;
        private System.Windows.Forms.ComboBox comboBox_TriggerLevelUnit;
        private System.Windows.Forms.ComboBox comboBox_TrigHoldUnit;
        private System.Windows.Forms.TextBox textBox_MainIP;
        private System.Windows.Forms.Label label_MainIp;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Collect;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Record;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceName;
        private System.Windows.Forms.DataGridViewComboBoxColumn ChannelID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tag;
        private System.Windows.Forms.DataGridViewTextBoxColumn TagDesc;
        private System.Windows.Forms.DataGridViewComboBoxColumn MeasureType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Scale;
        private System.Windows.Forms.DataGridViewTextBoxColumn Offset;
        private System.Windows.Forms.DataGridViewComboBoxColumn Impedance;
        private System.Windows.Forms.DataGridViewComboBoxColumn Coupling;
        private System.Windows.Forms.DataGridViewComboBoxColumn ProbeRatio;
        private System.Windows.Forms.DataGridViewTextBoxColumn SN;
        private System.Windows.Forms.DataGridViewTextBoxColumn Open;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChannelDelayTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Valid;
    }
}