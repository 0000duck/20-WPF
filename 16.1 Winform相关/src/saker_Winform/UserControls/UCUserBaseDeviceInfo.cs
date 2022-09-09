using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WindowsFormsApplication1
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }
        public UserControl1(string strNum,string strDev,string strIP)
        {
            InitializeComponent();
            label_Num.Text = strNum;
            label_Device.Text = strDev;
            label_IP.Text = strIP;
        }

        bool bdraw = false;

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            if (bdraw)
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;  //图片柔顺模式选择
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;//高质量
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;//再加一点
                Pen p = new Pen(new SolidBrush(ColorTranslator.FromHtml("#0072C6")), 3);//定义了一个灰色,宽度为1的画笔
                //Pen p = new Pen(Color.Green, 3);//定义了一个灰色,宽度为1的画笔
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; //实线
                Rectangle rect = new Rectangle(panel2.ClientRectangle.X, panel2.ClientRectangle.Y,
                                                 panel2.ClientRectangle.X + panel2.ClientRectangle.Width-1,
                                                 panel2.ClientRectangle.Y + panel2.ClientRectangle.Height-1);

                e.Graphics.DrawRectangle(p, rect);//绘制矩形框
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            bdraw = true;
            panel1.BackColor = System.Drawing.Color.FromArgb(0, 114, 198);
            panel2.BackColor = System.Drawing.Color.FromArgb(215, 215, 215);
            label_Num.ForeColor = System.Drawing.Color.White;
            //label_Device.ForeColor = System.Drawing.Color.White;
            //label_IP.ForeColor = System.Drawing.Color.White;
            this.Refresh();
        }

        private void label_Device_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
