namespace saker_Winform.SubForm
{
    partial class Form_UserManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_UserManager));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.GUID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Authority = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.CreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_Add = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Modifi = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Del = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(443, 369);
            this.panel1.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GUID,
            this.UserName,
            this.Password,
            this.Authority,
            this.CreateTime});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(443, 369);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            this.dataGridView1.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridView1_UserDeletingRow);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            // 
            // GUID
            // 
            this.GUID.DataPropertyName = "GUID";
            this.GUID.HeaderText = "GUID";
            this.GUID.Name = "GUID";
            this.GUID.ReadOnly = true;
            this.GUID.Visible = false;
            // 
            // UserName
            // 
            this.UserName.DataPropertyName = "UserName";
            this.UserName.HeaderText = "UserName";
            this.UserName.Name = "UserName";
            this.UserName.ReadOnly = true;
            // 
            // Password
            // 
            this.Password.DataPropertyName = "Password";
            this.Password.HeaderText = "Password";
            this.Password.Name = "Password";
            this.Password.ReadOnly = true;
            // 
            // Authority
            // 
            this.Authority.DataPropertyName = "RoleName";
            this.Authority.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Authority.HeaderText = "Authority";
            this.Authority.Items.AddRange(new object[] {
            "管理员",
            "访客"});
            this.Authority.Name = "Authority";
            this.Authority.ReadOnly = true;
            this.Authority.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Authority.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // CreateTime
            // 
            this.CreateTime.DataPropertyName = "CreateTime";
            this.CreateTime.HeaderText = "CreateTime";
            this.CreateTime.Name = "CreateTime";
            this.CreateTime.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Add,
            this.ToolStripMenuItem_Modifi,
            this.ToolStripMenuItem_Del});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 70);
            // 
            // ToolStripMenuItem_Add
            // 
            this.ToolStripMenuItem_Add.Name = "ToolStripMenuItem_Add";
            this.ToolStripMenuItem_Add.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuItem_Add.Text = "添加用户";
            this.ToolStripMenuItem_Add.Click += new System.EventHandler(this.ToolStripMenuItem_Add_Click);
            // 
            // ToolStripMenuItem_Modifi
            // 
            this.ToolStripMenuItem_Modifi.Name = "ToolStripMenuItem_Modifi";
            this.ToolStripMenuItem_Modifi.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuItem_Modifi.Text = "修改用户";
            this.ToolStripMenuItem_Modifi.Click += new System.EventHandler(this.ToolStripMenuItem_Modifi_Click);
            // 
            // ToolStripMenuItem_Del
            // 
            this.ToolStripMenuItem_Del.Name = "ToolStripMenuItem_Del";
            this.ToolStripMenuItem_Del.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuItem_Del.Text = "删除用户";
            this.ToolStripMenuItem_Del.Click += new System.EventHandler(this.ToolStripMenuItem_Del_Click);
            // 
            // Form_UserManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 369);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_UserManager";
            this.Text = "Form_UserManager";
            this.Load += new System.EventHandler(this.Form_UserManager_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn GUID;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Password;
        private System.Windows.Forms.DataGridViewComboBoxColumn Authority;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateTime;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Add;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Modifi;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Del;
    }
}