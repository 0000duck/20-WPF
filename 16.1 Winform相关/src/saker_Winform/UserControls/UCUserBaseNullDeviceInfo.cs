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
using saker_Winform.Properties;
using saker_Winform.CommonBaseModule;
using FontAwesome.Sharp;
using System.Runtime.CompilerServices;

namespace saker_Winform.UserControls
{

    public partial class UCUserBaseNullDeviceInfo : UserControl
    {
        #region 属性
        /* Mac地址 */
        private string strMac;

        public string m_strMac
        {
            get { return strMac; }
            set { strMac = value; }
        }

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

        /*设备是否注册*/
        private bool bStatus;

        public bool m_bStatus
        {
            get { return bStatus; }
            set { bStatus = value; }
        }

        private bool bLink;

        public bool m_bLink
        {
            get { return bLink; }
            set { bLink = value; }
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


        System.Timers.Timer timer = new System.Timers.Timer(500) { 
        AutoReset=false,Enabled = true
        };//AutoReset 设置是执行一次（false）还是一直执行(true)；Enable是否执行System.Timers.Timer.Elapsed事件；

        MouseEventArgs mouseEventArgs;

        public UCUserBaseNullDeviceInfo()
        {
            InitializeComponent();
            this.iconPictureBox_Plus.Cursor = Cursors.Hand;
            this.label_Device.Cursor = Cursors.Default;
            this.label_IP.Cursor = Cursors.Default;
            this.label_Num.Cursor = Cursors.Default;
            this.panel_In.Cursor = Cursors.Default;
            this.Cursor = Cursors.Hand;
        }
        #endregion

        #region 自定义事件

        public delegate void refresh_OnlineState();//申明委托
        public delegate void refresh_CReflection(Object obj, string EventName, EventArgs e = null);

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

        /// <summary>
        /// 构造函数
        /// <param name="color "></param>
        /// <param name="strDevNumber"></param>
        /// <param name="strDevSN"></param>
        /// <param name="strDevIP"></param>
        /// <param name="strDevSubName"></param>
        /// <param name="strDevModel"></param>
        /// <param name="strFirmVersion"></param>
        /// <param name="strRackNumber"></param>
        /// </summary>
        public UCUserBaseNullDeviceInfo(Color color, string strDevNumber, string strDevSN, string strDevIP, string strDevSubName, string strDevModel, string strFirmVersion, string strRackNumber, string strDevSerialNum)
        {
            InitializeComponent();
            label_Num.Text = strDevNumber;
            label_Device.Text = strDevSubName;
            label_IP.Text = strDevIP;
            iconPictureBox_Plus.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            //this.Cursor = Cursors.Default;
        }
        private void UserBaseNullDeviceInfo_Load(object sender, EventArgs e)
        {
            m_strDevModel = this.label_Modle.Text;
        }
        public UCUserBaseNullDeviceInfo(string strNum, string strDev, string strIP)
        {
            InitializeComponent();
            label_Num.Text = strNum;
            label_Device.Text = strDev;
            label_IP.Text = strIP;
        }

        #region 方法：刷新控件值
        /// <summary>
        /// 刷新自定义控件的显示，刷新四个显示值m_strDevNumber，m_strDevSubName，m_strDevIP，IconChar
        /// </summary>
        public void DevInfo()
        {
            /* 是否需要调用委托 */
            if (this.InvokeRequired)
            {
                refresh_OnlineState s = new refresh_OnlineState(DevInfo);
                this.Invoke(s);
            }
            else
            {
                if (m_bStatus)
                {
                    iconPictureBox_Plus.IconChar = FontAwesome.Sharp.IconChar.None;
                    iconPictureBox_Plus.BackgroundImage = global::saker_Winform.Properties.Resources.ControlDevice; //更换图片
                    iconPictureBox_Plus.IconColor = Color.DodgerBlue;
                    label_Num.Text = m_strDevNumber;
                    label_Device.Text = m_strDevSubName;
                    label_IP.Text = m_strDevIP;
                    label_Modle.Visible = false;
                    if (m_bLink)
                    {
                        //this.iconPictureBox_Status.BackColor = Color.FromArgb(166, 209, 245);
                        this.iconPictureBox_Status.Visible = true;
                        this.iconPictureBox_Status.IconColor = Color.Green;
                        this.iconPictureBox_Status.IconChar = IconChar.Link;
                    }
                    else
                    {
                        this.iconPictureBox_Status.Visible = true;
                        this.iconPictureBox_Status.IconColor = System.Drawing.SystemColors.ButtonShadow;
                        this.iconPictureBox_Status.IconChar = IconChar.Unlink;
                    }
                }
                else
                {
                    iconPictureBox_Plus.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
                    iconPictureBox_Plus.BackgroundImage = null; //更换图片
                    iconPictureBox_Plus.IconColor = Color.FromArgb(166, 209, 245);
                    label_Num.Text = m_strDevNumber;
                    label_Device.Text = m_strDevSubName;
                    label_IP.Text = m_strDevIP;
                    label_Modle.Text = m_strDevModel;
                    label_Modle.Visible = false;
                    this.iconPictureBox_Status.Visible = false;
                }
            }

        }

        /// <summary>
        /// 主动改变控件背景颜色
        /// </summary>
        public void panel_Set(bool pstate)
        {
            if (pstate == true)
            {
                bSelect = true;
                m_isSelect = true;
                bdraw = true;
                panel_In.BackColor = m_selectColor;
                label_Num.BackColor = m_selectColor;
                iconPictureBox_Plus.IconColor = m_selectColor;
                //this.iconPictureBox_Status.BackColor = m_selectColor;
                //this.iconPictureBox_Status.ForeColor = m_selectColor;
                label_Num.ForeColor = System.Drawing.Color.White;

                this.Refresh();
            }
            else
            {
                bSelect = false;
                m_isSelect = false;
                bdraw = false;
                panel_In.BackColor = m_unselectColor;
                label_Num.BackColor = m_unselectColor;
                iconPictureBox_Plus.IconColor = m_unselectColor;
                //this.iconPictureBox_Status.BackColor = m_unselectColor;
                //this.iconPictureBox_Status.ForeColor = Color.FromArgb(166, 209, 245);
                label_Num.ForeColor = System.Drawing.Color.Black;
                this.Refresh();
            }
        }

        /// <summary>
        /// 清空显示控件信息
        /// </summary>
        public void clearControl()
        {
            //panel_In.BackColor = m_unselectColor;//Color.DodgerBlue
            label_Num.ForeColor = System.Drawing.Color.Black;
            label_IP.Text = "";
            label_Device.Text = "";
            iconPictureBox_Plus.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            iconPictureBox_Plus.IconColor = Color.FromArgb(166, 209, 245);
            iconPictureBox_Plus.BackgroundImage = null;
            this.iconPictureBox_Status.Visible = false;
            clearPropfull();
        }
        /// <summary>
        /// 清空属性值
        /// </summary>
        public void clearPropfull()
        {
            m_strDevIP = "";
            m_strDevSubName = "";
            m_strDevModel = "";
            m_strDevSN = "";
            m_strFirmVersion = "";
            m_strRackNumber = "";
        }
        #endregion

        #region 事件：点击选中发送自定义事件
        public bool bdraw = false;
        /// <summary>
        /// panel_In点击事件，当panel1_In被点击发送自定义事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel_In_Click(object sender, EventArgs e)
        {
            RaiseEvent("panelInClick");
        }

        /// <summary>
        /// label_Number点击事件，发送自定义事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label_Num_Click(object sender, EventArgs e)
        {
            RaiseEvent("panelInClick");
        }

        /// <summary>
        /// iconPicture_Pluse_Click被点击，发送自定义事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconPictureBox_Plus_Click(object sender, EventArgs e)
        {
            if(Form_Main.m_adminLogin)
            {
                RaiseEvent("PictureBoxDouleClick");
            }          
        }
        #endregion
          
        #region 外侧Panel被点击时绘制边框
        private void UserBaseNullDeviceInfo_Paint(object sender, PaintEventArgs e)
        {
            if (bdraw)
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;  //图片柔顺模式选择
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;//高质量
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;//再加一点
                Pen p = new Pen(new SolidBrush(ColorTranslator.FromHtml("#0072C6")), 3);//定义了一个灰色,宽度为1的画笔
                //Pen p = new Pen(Color.Green, 3);//定义了一个灰色,宽度为1的画笔
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; //实线
                Rectangle rect = new Rectangle(87, 120,
                                                 87 + 86,
                                                 120 + 119);
                e.Graphics.DrawRectangle(p, rect);//绘制矩形框
            }
        }
        #endregion

        #region 事件：MouseDown,MouseUp定时器避免和Click事件冲突
        private void iconPictureBox_Plus_MouseDown(object sender, MouseEventArgs e)
        {
            mouseEventArgs = e;
            if (e.Button == MouseButtons.Left)
            {
                //timer.Elapsed += Timer_Elapsed;
                timer.Start();
                //timer.Elapsed -= Timer_Elapsed;
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (!string.IsNullOrEmpty(m_strDevIP) && m_strDevIP != "")
                {
                    ToolVisable(true);
                }
                else if(m_strDevIP == null&& m_strDevSubName == null)
                {
                    this.ToolStripMenuItem_Register.Visible = false;
                    this.ToolStripMenuItem_Modif.Visible = false;
                    this.iconMenuItem_delDevice.Visible = false;
                }
                else
                {
                    ToolVisable(false);
                }
                this.contextMenuStrip1.Show(MousePosition);
            }
        }

        private void iconPictureBox_Plus_MouseUp(object sender, MouseEventArgs e)
        {
            //timer.Elapsed -= Timer_Elapsed;
            timer.Stop();        
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            /* 是否需要调用委托 */
            if (this.InvokeRequired)
            {
                refresh_CReflection s = new refresh_CReflection(CReflection.callObjectEvent);
                this.Invoke(s, this, "OnMouseDown", mouseEventArgs);
            }
        }
        #endregion

        #region 事件：鼠标右键菜单
        /// <summary>
        /// 事件：鼠标右键注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Register_Click(object sender, EventArgs e)
        {
            RaiseEvent("ToolStripRegister");     
        }

        /// <summary>
        /// 事件：鼠标右键修改信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Modif_Click(object sender, EventArgs e)
        {
            RaiseEvent("ToolStripModif");
        }

        private void iconMenuItem_delDevice_Click(object sender, EventArgs e)
        {
            RaiseEvent("ToolStripDelDevice");
        }

        /// <summary>
        /// 修改按键菜单不显示
        /// </summary>
        /// <param name="modif"></param>
        public void ToolVisable(bool modif)
        {
            if (Form_Main.m_adminLogin)//从机无权限
            {
                this.ToolStripMenuItem_Register.Visible = !modif;
                this.ToolStripMenuItem_Modif.Visible = modif;
                this.iconMenuItem_delDevice.Visible = modif;
            }
            else
            {
                this.ToolStripMenuItem_Register.Visible = false;
                this.ToolStripMenuItem_Modif.Visible = false;
                this.iconMenuItem_delDevice.Visible = false;
            }
        }
        #endregion

        #region 事件：未使用事件
        private void UCUserBaseNullDeviceInfo_MouseDown(object sender, MouseEventArgs e)
        {

        }
        #endregion

       
    }
}
