namespace saker_Winform.UserControls
{
    partial class UCDevStates
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCDevStates));
            this.panel_Horz = new System.Windows.Forms.Panel();
            this.label_MDepth = new System.Windows.Forms.Label();
            this.label_Acq = new System.Windows.Forms.Label();
            this.label_TimOffset = new System.Windows.Forms.Label();
            this.label_TimBase = new System.Windows.Forms.Label();
            this.label_TrigState = new System.Windows.Forms.Label();
            this.label_DevSubName = new System.Windows.Forms.Label();
            this.iconPictureBox_State = new FontAwesome.Sharp.IconPictureBox();
            this.ucChanInfo_CH4 = new saker_Winform.UserControls.UCChanInfo();
            this.ucChanInfo_CH3 = new saker_Winform.UserControls.UCChanInfo();
            this.ucChanInfo_CH2 = new saker_Winform.UserControls.UCChanInfo();
            this.ucChanInfo_CH1 = new saker_Winform.UserControls.UCChanInfo();
            this.panel_Horz.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox_State)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_Horz
            // 
            this.panel_Horz.BackColor = System.Drawing.Color.Black;
            this.panel_Horz.Controls.Add(this.iconPictureBox_State);
            this.panel_Horz.Controls.Add(this.label_MDepth);
            this.panel_Horz.Controls.Add(this.label_Acq);
            this.panel_Horz.Controls.Add(this.label_TimOffset);
            this.panel_Horz.Controls.Add(this.label_TimBase);
            this.panel_Horz.Controls.Add(this.label_TrigState);
            this.panel_Horz.Controls.Add(this.label_DevSubName);
            this.panel_Horz.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Horz.Location = new System.Drawing.Point(0, 0);
            this.panel_Horz.Name = "panel_Horz";
            this.panel_Horz.Size = new System.Drawing.Size(392, 57);
            this.panel_Horz.TabIndex = 0;
            // 
            // label_MDepth
            // 
            this.label_MDepth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_MDepth.AutoSize = true;
            this.label_MDepth.BackColor = System.Drawing.Color.Black;
            this.label_MDepth.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_MDepth.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label_MDepth.Location = new System.Drawing.Point(286, 36);
            this.label_MDepth.Name = "label_MDepth";
            this.label_MDepth.Size = new System.Drawing.Size(58, 17);
            this.label_MDepth.TabIndex = 11;
            this.label_MDepth.Text = "MDepth:";
            // 
            // label_Acq
            // 
            this.label_Acq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label_Acq.AutoSize = true;
            this.label_Acq.BackColor = System.Drawing.Color.Black;
            this.label_Acq.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Acq.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label_Acq.Location = new System.Drawing.Point(193, 36);
            this.label_Acq.Name = "label_Acq";
            this.label_Acq.Size = new System.Drawing.Size(33, 17);
            this.label_Acq.TabIndex = 10;
            this.label_Acq.Text = "Acq:";
            // 
            // label_TimOffset
            // 
            this.label_TimOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label_TimOffset.AutoSize = true;
            this.label_TimOffset.BackColor = System.Drawing.Color.Black;
            this.label_TimOffset.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_TimOffset.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label_TimOffset.Location = new System.Drawing.Point(98, 36);
            this.label_TimOffset.Name = "label_TimOffset";
            this.label_TimOffset.Size = new System.Drawing.Size(35, 17);
            this.label_TimOffset.TabIndex = 9;
            this.label_TimOffset.Text = "位移:";
            // 
            // label_TimBase
            // 
            this.label_TimBase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_TimBase.AutoSize = true;
            this.label_TimBase.BackColor = System.Drawing.Color.Black;
            this.label_TimBase.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_TimBase.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label_TimBase.Location = new System.Drawing.Point(3, 36);
            this.label_TimBase.Name = "label_TimBase";
            this.label_TimBase.Size = new System.Drawing.Size(35, 17);
            this.label_TimBase.TabIndex = 8;
            this.label_TimBase.Text = "时基:";
            // 
            // label_TrigState
            // 
            this.label_TrigState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_TrigState.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_TrigState.ForeColor = System.Drawing.Color.ForestGreen;
            this.label_TrigState.Image = ((System.Drawing.Image)(resources.GetObject("label_TrigState.Image")));
            this.label_TrigState.Location = new System.Drawing.Point(338, 0);
            this.label_TrigState.Name = "label_TrigState";
            this.label_TrigState.Size = new System.Drawing.Size(51, 25);
            this.label_TrigState.TabIndex = 7;
            this.label_TrigState.Text = "AUTO";
            this.label_TrigState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_DevSubName
            // 
            this.label_DevSubName.AutoSize = true;
            this.label_DevSubName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_DevSubName.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label_DevSubName.Location = new System.Drawing.Point(40, 3);
            this.label_DevSubName.Name = "label_DevSubName";
            this.label_DevSubName.Size = new System.Drawing.Size(224, 16);
            this.label_DevSubName.TabIndex = 1;
            this.label_DevSubName.Text = "Device_001/DS8R000000009";
            // 
            // iconPictureBox_State
            // 
            this.iconPictureBox_State.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.iconPictureBox_State.IconChar = FontAwesome.Sharp.IconChar.Link;
            this.iconPictureBox_State.IconColor = System.Drawing.SystemColors.MenuHighlight;
            this.iconPictureBox_State.IconSize = 34;
            this.iconPictureBox_State.Location = new System.Drawing.Point(1, 1);
            this.iconPictureBox_State.Name = "iconPictureBox_State";
            this.iconPictureBox_State.Size = new System.Drawing.Size(34, 34);
            this.iconPictureBox_State.TabIndex = 12;
            this.iconPictureBox_State.TabStop = false;
            // 
            // ucChanInfo_CH4
            // 
            this.ucChanInfo_CH4.Location = new System.Drawing.Point(296, 60);
            this.ucChanInfo_CH4.m_bChanImpedance = false;
            this.ucChanInfo_CH4.m_bChanState = false;
            this.ucChanInfo_CH4.m_euChanIndex = saker_Winform.Global.CGlobalValue.euChanID.CH4;
            this.ucChanInfo_CH4.m_strChanTag = null;
            this.ucChanInfo_CH4.Name = "ucChanInfo_CH4";
            this.ucChanInfo_CH4.Size = new System.Drawing.Size(92, 52);
            this.ucChanInfo_CH4.TabIndex = 4;
            // 
            // ucChanInfo_CH3
            // 
            this.ucChanInfo_CH3.Location = new System.Drawing.Point(198, 60);
            this.ucChanInfo_CH3.m_bChanImpedance = false;
            this.ucChanInfo_CH3.m_bChanState = false;
            this.ucChanInfo_CH3.m_euChanIndex = saker_Winform.Global.CGlobalValue.euChanID.CH3;
            this.ucChanInfo_CH3.m_strChanTag = null;
            this.ucChanInfo_CH3.Name = "ucChanInfo_CH3";
            this.ucChanInfo_CH3.Size = new System.Drawing.Size(92, 52);
            this.ucChanInfo_CH3.TabIndex = 3;
            // 
            // ucChanInfo_CH2
            // 
            this.ucChanInfo_CH2.Location = new System.Drawing.Point(100, 60);
            this.ucChanInfo_CH2.m_bChanImpedance = false;
            this.ucChanInfo_CH2.m_bChanState = false;
            this.ucChanInfo_CH2.m_euChanIndex = saker_Winform.Global.CGlobalValue.euChanID.CH2;
            this.ucChanInfo_CH2.m_strChanTag = null;
            this.ucChanInfo_CH2.Name = "ucChanInfo_CH2";
            this.ucChanInfo_CH2.Size = new System.Drawing.Size(92, 52);
            this.ucChanInfo_CH2.TabIndex = 2;
            // 
            // ucChanInfo_CH1
            // 
            this.ucChanInfo_CH1.Location = new System.Drawing.Point(2, 60);
            this.ucChanInfo_CH1.m_bChanImpedance = false;
            this.ucChanInfo_CH1.m_bChanState = false;
            this.ucChanInfo_CH1.m_euChanIndex = saker_Winform.Global.CGlobalValue.euChanID.CH1;
            this.ucChanInfo_CH1.m_strChanTag = null;
            this.ucChanInfo_CH1.Name = "ucChanInfo_CH1";
            this.ucChanInfo_CH1.Size = new System.Drawing.Size(92, 52);
            this.ucChanInfo_CH1.TabIndex = 1;
            // 
            // UCDevStates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.ucChanInfo_CH4);
            this.Controls.Add(this.ucChanInfo_CH3);
            this.Controls.Add(this.ucChanInfo_CH2);
            this.Controls.Add(this.ucChanInfo_CH1);
            this.Controls.Add(this.panel_Horz);
            this.Name = "UCDevStates";
            this.Size = new System.Drawing.Size(392, 115);
            this.panel_Horz.ResumeLayout(false);
            this.panel_Horz.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox_State)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_Horz;
        private System.Windows.Forms.Label label_MDepth;
        private System.Windows.Forms.Label label_Acq;
        private System.Windows.Forms.Label label_TimOffset;
        private System.Windows.Forms.Label label_TimBase;
        private System.Windows.Forms.Label label_TrigState;
        private System.Windows.Forms.Label label_DevSubName;
        private UCChanInfo ucChanInfo_CH1;
        private UCChanInfo ucChanInfo_CH2;
        private UCChanInfo ucChanInfo_CH3;
        private UCChanInfo ucChanInfo_CH4;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox_State;
    }
}
