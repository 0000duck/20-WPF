using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using saker_Winform.CommonBaseModule;

namespace saker_Winform.UserControls
{
    public partial class UCSmallDev : UserControl
    {
        #region 属性
        /*设备Number*/
        private string strDevNumber;

        public string m_strDevNumber
        {
            get { return strDevNumber; }
            set { strDevNumber = value; }
        }


        /*设备SN*/    /*仪器序列号*/
        private string strDevSN;

        public string m_strDevSN
        {
            get { return strDevSN; }
            set { strDevSN = value; }
        }

        /*设备IP*/
        private string strDevIP;

        public string m_strDevIP
        {
            get { return strDevIP; }
            set { strDevIP = value; }
        }

        /*设备别名*/
        private string strDevSubName;

        public string m_strDevSubName
        {
            get { return strDevSubName; }
            set { strDevSubName = value; }
        }

        /*设备型号*/
        private string strDevModel;

        public string m_strDevModel
        {
            get { return strDevModel; }
            set { strDevModel = value; }
        }

        /*固件版本*/
        private string strFirmVersion;

        public string m_strFirmVersion
        {
            get { return strFirmVersion; }
            set { strFirmVersion = value; }
        }

        /*机架编号*/
        private string strRackNumber;

        public string m_strRackNumber
        {
            get { return strRackNumber; }
            set { strRackNumber = value; }
        }

        /* Mac */
        private string strMac;

        public string m_strMac
        {
            get { return strMac; }
            set { strMac = value; }
        }


        /*设备是否连接*/
        private bool bStatus;

        public bool m_bStatus
        {
            get { return bStatus; }
            set { bStatus = value; }
        }


        /*设备选中状态*/
        private bool bSelect = false;

        public bool m_bSelect
        {
            get { return bSelect; }
            set { bSelect = value; }
        }

        /*check是否选中*/
        private bool isSelect = false;

        public bool m_isSelect
        {
            get { return isSelect; }
            set { isSelect = value; }
        }

        /*选中颜色*/
        private Color selectColor;
        public Color m_selectColor
        {
            get { return selectColor; }
            set { selectColor = value; }
        }

        /*未选中颜色*/
        private Color unselectColor = Color.FromArgb(66, 65, 66);

        public Color m_unselectColor
        {
            get { return unselectColor; }
            set { unselectColor = value; }
        }

        /*判断选中颜色*/
        private Color checkselectColor;

        public Color m_checkselectColor
        {
            get { return checkselectColor; }
            set { checkselectColor = value; }
        }


        #endregion

        /*      #region 自定义事件
              *//*定义事件参数类*//*
              public class clickUCSmallEventArgs : EventArgs
              {
                  public readonly string KeyToRaiseEvent;

                  public clickUCSmallEventArgs(string keyToRaiseEvent)
                  {
                      KeyToRaiseEvent = keyToRaiseEvent;
                  }
              }
              *//*定义委托声明*//*
              public delegate void clickUserBaseNullEventHandler(object sender, clickUCSmallEventArgs e);

              //用event关键字声明事件对象
              public event clickUserBaseNullEventHandler clickUCSmallDevEvent;

              //事件触发方法
              protected virtual void onclickUCSmallEvent(clickUCSmallEventArgs e)
              {
                  if (clickUCSmallDevEvent != null)
                  {
                      clickUCSmallDevEvent(this, e);
                  }
              }

              //引发事件
              private void RaiseEvent(string keyToRaiseEvent)
              {
                  clickUCSmallEventArgs e = new clickUCSmallEventArgs(keyToRaiseEvent);

                  onclickUCSmallEvent(e);
              }*/


        #region 自定义事件
        /*定义事件参数类*/
        public class clickUerBaseNullEventArgs : EventArgs
        {
            public readonly string KeyToRaiseEvent;

            public clickUerBaseNullEventArgs(string keyToRaiseEvent)
            {
                KeyToRaiseEvent = keyToRaiseEvent;
            }
        }
        /*定义委托声明*/
        public delegate void clickUserBaseNullEventHandler(object sender, clickUerBaseNullEventArgs e);

        //用event关键字声明事件对象
        public event clickUserBaseNullEventHandler clickUerEvent;

        //事件触发方法
        protected virtual void onClickUserBaseNullEvent(clickUerBaseNullEventArgs e)
        {
            if (clickUerEvent != null)
            {
                clickUerEvent(this, e);
            }
        }

        //引发事件
        private void RaiseEvent(string keyToRaiseEvent)
        {
            clickUerBaseNullEventArgs e = new clickUerBaseNullEventArgs(keyToRaiseEvent);

            onClickUserBaseNullEvent(e);
        }

        #endregion



        public UCSmallDev()
        {
            InitializeComponent();
            this.pictureBox_OnlineDev.Cursor = Cursors.Hand;
            this.panel_out.Cursor = Cursors.Hand;
            
        }

        public void SmallDevInfo()
        {
            label_OnlineDev.Text = m_strDevIP;
        }

        public void SetPanel(bool bSelectSmall)
        {
            bdraw = bSelectSmall;
            this.Refresh();
        }

        public bool bdraw = false;

        #region 外侧Panel被点击时绘制边框
        private void panel_out_Paint(object sender, PaintEventArgs e)
        {
            if (bdraw)
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;  //图片柔顺模式选择
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;//高质量
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;//再加一点
                Pen p = new Pen(new SolidBrush(ColorTranslator.FromHtml("#0072C6")), 3);//定义了一个灰色,宽度为1的画笔
                //Pen p = new Pen(Color.Green, 3);//定义了一个灰色,宽度为1的画笔
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; //实线
                Rectangle rect = new Rectangle(panel_out.ClientRectangle.X, panel_out.ClientRectangle.Y,
                                                 panel_out.ClientRectangle.X + panel_out.ClientRectangle.Width - 1,
                                                 panel_out.ClientRectangle.Y + panel_out.ClientRectangle.Height - 1);

                e.Graphics.DrawRectangle(p, rect);//绘制矩形框
            }
        }
        #endregion

        #region 发送事件

        private void panel_out_Click(object sender, EventArgs e)
        {
            RaiseEvent(m_strDevIP);
        }

        private void pictureBox_OnlineDev_Click(object sender, EventArgs e)
        {
            RaiseEvent(m_strDevIP);
        }

        private void label_OnlineDev_Click(object sender, EventArgs e)
        {
            RaiseEvent(m_strDevIP);
        }
        #endregion

        /// <summary>
        /// MouseDown反射到到Panel和图标的MouseDown上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCSmallDev_MouseDown(object sender, MouseEventArgs e)
        {

        }

        /// <summary>
        ///  MouseDown反射到到Panel和图标的MouseDown上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel_out_MouseDown(object sender, MouseEventArgs e)
        {
            CReflection.callObjectEvent(this, "OnMouseDown", e);
        }

        /// <summary>
        ///  MouseDown反射到到Panel和图标的MouseDown上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_OnlineDev_MouseDown(object sender, MouseEventArgs e)
        {
            CReflection.callObjectEvent(this, "OnMouseDown", e);
        }
    }
}
