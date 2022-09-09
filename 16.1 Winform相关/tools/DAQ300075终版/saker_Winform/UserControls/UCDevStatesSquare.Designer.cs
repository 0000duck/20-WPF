namespace saker_Winform.UserControls
{
    partial class UCDevStatesSquare
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCDevStatesSquare));
            this.label_DevSubName = new System.Windows.Forms.Label();
            this.ucChanInfo1 = new saker_Winform.UserControls.UCChanInfo();
            this.ucChanInfo2 = new saker_Winform.UserControls.UCChanInfo();
            this.ucChanInfo3 = new saker_Winform.UserControls.UCChanInfo();
            this.ucChanInfo4 = new saker_Winform.UserControls.UCChanInfo();
            this.panel_Horz = new System.Windows.Forms.Panel();
            this.label_TimOffset = new System.Windows.Forms.Label();
            this.label_TimBase = new System.Windows.Forms.Label();
            this.label_MDepth = new System.Windows.Forms.Label();
            this.label_Acq = new System.Windows.Forms.Label();
            this.label_TrigState = new System.Windows.Forms.Label();
            this.label_IP = new System.Windows.Forms.Label();
            this.panel_Horz.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_DevSubName
            // 
            this.label_DevSubName.AutoSize = true;
            this.label_DevSubName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_DevSubName.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label_DevSubName.Location = new System.Drawing.Point(3, 0);
            this.label_DevSubName.Name = "label_DevSubName";
            this.label_DevSubName.Size = new System.Drawing.Size(98, 16);
            this.label_DevSubName.TabIndex = 2;
            this.label_DevSubName.Text = "Device_001";
            // 
            // ucChanInfo1
            // 
            this.ucChanInfo1.Location = new System.Drawing.Point(6, 141);
            this.ucChanInfo1.m_bChanImpedance = false;
            this.ucChanInfo1.m_bChanState = false;
            this.ucChanInfo1.m_strChanTag = null;
            this.ucChanInfo1.Name = "ucChanInfo1";
            this.ucChanInfo1.Size = new System.Drawing.Size(92, 52);
            this.ucChanInfo1.TabIndex = 3;
            // 
            // ucChanInfo2
            // 
            this.ucChanInfo2.Location = new System.Drawing.Point(159, 143);
            this.ucChanInfo2.m_bChanImpedance = false;
            this.ucChanInfo2.m_bChanState = false;
            this.ucChanInfo2.m_strChanTag = null;
            this.ucChanInfo2.Name = "ucChanInfo2";
            this.ucChanInfo2.Size = new System.Drawing.Size(92, 52);
            this.ucChanInfo2.TabIndex = 4;
            // 
            // ucChanInfo3
            // 
            this.ucChanInfo3.Location = new System.Drawing.Point(6, 199);
            this.ucChanInfo3.m_bChanImpedance = false;
            this.ucChanInfo3.m_bChanState = false;
            this.ucChanInfo3.m_strChanTag = null;
            this.ucChanInfo3.Name = "ucChanInfo3";
            this.ucChanInfo3.Size = new System.Drawing.Size(92, 52);
            this.ucChanInfo3.TabIndex = 5;
            // 
            // ucChanInfo4
            // 
            this.ucChanInfo4.Location = new System.Drawing.Point(159, 199);
            this.ucChanInfo4.m_bChanImpedance = false;
            this.ucChanInfo4.m_bChanState = false;
            this.ucChanInfo4.m_strChanTag = null;
            this.ucChanInfo4.Name = "ucChanInfo4";
            this.ucChanInfo4.Size = new System.Drawing.Size(92, 52);
            this.ucChanInfo4.TabIndex = 6;
            // 
            // panel_Horz
            // 
            this.panel_Horz.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel_Horz.Controls.Add(this.label_IP);
            this.panel_Horz.Controls.Add(this.label_TrigState);
            this.panel_Horz.Controls.Add(this.label_MDepth);
            this.panel_Horz.Controls.Add(this.label_Acq);
            this.panel_Horz.Controls.Add(this.label_TimOffset);
            this.panel_Horz.Controls.Add(this.label_TimBase);
            this.panel_Horz.Controls.Add(this.label_DevSubName);
            this.panel_Horz.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Horz.Location = new System.Drawing.Point(0, 0);
            this.panel_Horz.Name = "panel_Horz";
            this.panel_Horz.Size = new System.Drawing.Size(254, 135);
            this.panel_Horz.TabIndex = 7;
            // 
            // label_TimOffset
            // 
            this.label_TimOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label_TimOffset.AutoSize = true;
            this.label_TimOffset.BackColor = System.Drawing.Color.Black;
            this.label_TimOffset.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_TimOffset.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label_TimOffset.Location = new System.Drawing.Point(124, 72);
            this.label_TimOffset.Name = "label_TimOffset";
            this.label_TimOffset.Size = new System.Drawing.Size(35, 17);
            this.label_TimOffset.TabIndex = 11;
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
            this.label_TimBase.Location = new System.Drawing.Point(3, 72);
            this.label_TimBase.Name = "label_TimBase";
            this.label_TimBase.Size = new System.Drawing.Size(35, 17);
            this.label_TimBase.TabIndex = 10;
            this.label_TimBase.Text = "时基:";
            // 
            // label_MDepth
            // 
            this.label_MDepth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_MDepth.AutoSize = true;
            this.label_MDepth.BackColor = System.Drawing.Color.Black;
            this.label_MDepth.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_MDepth.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label_MDepth.Location = new System.Drawing.Point(124, 109);
            this.label_MDepth.Name = "label_MDepth";
            this.label_MDepth.Size = new System.Drawing.Size(58, 17);
            this.label_MDepth.TabIndex = 13;
            this.label_MDepth.Text = "MDepth:";
            // 
            // label_Acq
            // 
            this.label_Acq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label_Acq.AutoSize = true;
            this.label_Acq.BackColor = System.Drawing.Color.Black;
            this.label_Acq.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Acq.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label_Acq.Location = new System.Drawing.Point(5, 109);
            this.label_Acq.Name = "label_Acq";
            this.label_Acq.Size = new System.Drawing.Size(33, 17);
            this.label_Acq.TabIndex = 12;
            this.label_Acq.Text = "Acq:";
            // 
            // label_TrigState
            // 
            this.label_TrigState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_TrigState.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_TrigState.ForeColor = System.Drawing.Color.ForestGreen;
            this.label_TrigState.Image = ((System.Drawing.Image)(resources.GetObject("label_TrigState.Image")));
            this.label_TrigState.Location = new System.Drawing.Point(200, 3);
            this.label_TrigState.Name = "label_TrigState";
            this.label_TrigState.Size = new System.Drawing.Size(51, 25);
            this.label_TrigState.TabIndex = 14;
            this.label_TrigState.Text = "AUTO";
            this.label_TrigState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_IP
            // 
            this.label_IP.AutoSize = true;
            this.label_IP.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_IP.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label_IP.Location = new System.Drawing.Point(5, 36);
            this.label_IP.Name = "label_IP";
            this.label_IP.Size = new System.Drawing.Size(116, 16);
            this.label_IP.TabIndex = 15;
            this.label_IP.Text = "172.18.8.252";
            // 
            // UCDevStatesSquare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel_Horz);
            this.Controls.Add(this.ucChanInfo4);
            this.Controls.Add(this.ucChanInfo3);
            this.Controls.Add(this.ucChanInfo2);
            this.Controls.Add(this.ucChanInfo1);
            this.Name = "UCDevStatesSquare";
            this.Size = new System.Drawing.Size(254, 254);
            this.panel_Horz.ResumeLayout(false);
            this.panel_Horz.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_DevSubName;
        private UCChanInfo ucChanInfo1;
        private UCChanInfo ucChanInfo2;
        private UCChanInfo ucChanInfo3;
        private UCChanInfo ucChanInfo4;
        private System.Windows.Forms.Panel panel_Horz;
        private System.Windows.Forms.Label label_TimOffset;
        private System.Windows.Forms.Label label_TimBase;
        private System.Windows.Forms.Label label_MDepth;
        private System.Windows.Forms.Label label_Acq;
        private System.Windows.Forms.Label label_IP;
        private System.Windows.Forms.Label label_TrigState;
    }
}
