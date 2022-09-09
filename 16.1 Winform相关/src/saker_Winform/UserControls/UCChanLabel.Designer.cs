using FontAwesome.Sharp;
namespace saker_Winform.UserControls
{
    partial class UCChanLabel
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
            this.label_ChanID = new System.Windows.Forms.Label();
            this.iconPictureBox_TagTop = new FontAwesome.Sharp.IconPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox_TagTop)).BeginInit();
            this.SuspendLayout();
            // 
            // label_ChanID
            // 
            this.label_ChanID.AutoSize = true;
            this.label_ChanID.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_ChanID.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ChanID.ForeColor = System.Drawing.Color.Orange;
            this.label_ChanID.Location = new System.Drawing.Point(3, 5);
            this.label_ChanID.Name = "label_ChanID";
            this.label_ChanID.Size = new System.Drawing.Size(44, 15);
            this.label_ChanID.TabIndex = 0;
            this.label_ChanID.Text = "Tag300";
            this.label_ChanID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label_ChanID.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_ChanID_MouseDown);
            this.label_ChanID.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label_ChanID_MouseMove);
            this.label_ChanID.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_ChanID_MouseUp);
            // 
            // iconPictureBox_TagTop
            // 
            this.iconPictureBox_TagTop.BackColor = System.Drawing.SystemColors.Control;
            this.iconPictureBox_TagTop.ForeColor = System.Drawing.Color.Orange;
            this.iconPictureBox_TagTop.IconChar = FontAwesome.Sharp.IconChar.CaretRight;
            this.iconPictureBox_TagTop.IconColor = System.Drawing.Color.Orange;
            this.iconPictureBox_TagTop.IconSize = 21;
            this.iconPictureBox_TagTop.Location = new System.Drawing.Point(47, 3);
            this.iconPictureBox_TagTop.Name = "iconPictureBox_TagTop";
            this.iconPictureBox_TagTop.Size = new System.Drawing.Size(21, 21);
            this.iconPictureBox_TagTop.TabIndex = 1;
            this.iconPictureBox_TagTop.TabStop = false;
            // 
            // UCChanLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_ChanID);
            this.Controls.Add(this.iconPictureBox_TagTop);
            this.Name = "UCChanLabel";
            this.Size = new System.Drawing.Size(68, 24);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox_TagTop)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_ChanID;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox_TagTop;
    }
}
