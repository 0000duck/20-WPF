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
/*****************************************************************************************************************
                                        普源精电科技有限公司版权所有(2020-)
******************************************************************************************************************
类名:      UCChanWaveView
功能描述： UCChanWaveView类实现用户控件的相关功能
作 者：    sn02736
版 本：    00.01.00.00
修改历史： 
<作者> <修改时间> <版本> <修改描述>
 sn02736      2020.7.31    初次修改
*****************************************************************************************************************/
namespace saker_Winform.UserControls
{
    public partial class UCHistoryComp : UserControl
    {

        #region Properties
        /// <summary>
        /// 是否选中
        /// </summary>
        private bool bSelect = false;
        [Browsable(true)]
        [Description("标示当前用户控件是否被选中")]
        public bool m_bSelect
        {
            get { return bSelect; }
            set { bSelect = value; }
        }
        /// <summary>
        /// check是否选中
        /// </summary>
        private bool bCheck = false;
        [Browsable(true)]
        [Description("标示checkbox是否被选中")]
        public bool m_bCheck
        {
            get { return bCheck; }
            set { bCheck = value; }
        }
        /// <summary>
        /// 选中颜色
        /// </summary>
        private Color selectColor = Color.Yellow;
        [Browsable(true)]
        [Description("标示用户控件选中时的颜色")]
        public Color m_selectColor
        {
            get { return selectColor; }
            set { selectColor = value; }
        }
        /// <summary>
        /// 未选中颜色
        /// </summary>
        private Color unselectColor = Color.FromArgb(66, 65, 66);
        [Browsable(true)]
        [Description("标示用户控件未被选中时的颜色")]
        public Color m_unselectColor
        {
            get { return unselectColor; }
            set { unselectColor = value; }
        }
        /// <summary>
        /// 通道标记
        /// </summary>
        private string strChanID = "Tag001";
        [Browsable(true)]
        [Description("通道标记信息")]
        public string m_strChanID
        {
            get { return strChanID; }
            set { strChanID = value; }
        }
        /// <summary>
        /// 测量类型
        /// </summary>
        private string strMeasType;
        [Browsable(true)]
        [Description("测量类型")]
        public string m_strTestType
        {
            get { return strMeasType; }
            set { strMeasType = value; }
        }
        /// <summary>
        /// 测试时间
        /// </summary>
        private string strTestTim;
        [Browsable(true)]
        [Description("测试时间")]
        public string m_strTestTim
        {
            get { return strTestTim; }
            set { strTestTim = value; }
        }
        /// <summary>
        /// 设备别名
        /// </summary>
        private string strDevName;
        [Browsable(true)]
        [Description("设备别名")]
        public string m_strDevName
        {
            get { return strDevName; }
            set { strDevName = value; }
        }
        /// <summary>
        /// 实际通道
        /// </summary>
        private string strChanActual;
        [Browsable(true)]
        [Description("实际通道")]
        public string m_strChanActual
        {
            get { return strChanActual; }
            set { strChanActual = value; }
        }
        #endregion

        #region Event
        /*定义事件参数类*/
        public class chanWaveViewEventArgs : EventArgs
        {
            public readonly string KeyToRaiseEvent;
            public chanWaveViewEventArgs(string keyToRaiseEvent)
            {
                KeyToRaiseEvent = keyToRaiseEvent;
            }
        }

        public delegate void uc_Refresh();//申明用户控件的刷新委托
        //定义delegate委托申明
        public delegate void chanWaveViewEventHandler(object sender, chanWaveViewEventArgs e);
        //用event 关键字声明事件对象
        [Browsable(true)]
        [Description("自定义事件，向绑定的窗体发送对应字符串信息")]
        public event chanWaveViewEventHandler chanWaveViewEvent;
        //事件触发方法
        protected virtual void onChanWaveViewEvent(chanWaveViewEventArgs e)
        {
            if (chanWaveViewEvent != null)
                chanWaveViewEvent(this, e);
        }
        //引发事件
        private void RaiseEvent(string keyToRaiseEvent)
        {
            chanWaveViewEventArgs e = new chanWaveViewEventArgs(keyToRaiseEvent);
            onChanWaveViewEvent(e);
        }

        public delegate void labelColor_Update(bool bState);//申明背景颜色设置的委托

        /// <summary>
        /// chanID点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label_ChanID_Click(object sender, EventArgs e)
        {
            if (bCheck == false)
            {
                return;
            }
            /*发送事件(tag+测试时间)*/
            RaiseEvent(strChanID + ";"+ strTestTim);
        }
        /// <summary>
        /// checkBox状态更新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_Select_CheckedChanged(object sender, EventArgs e)
        {
            bCheck = checkBox_Select.Checked;
            if (bCheck == false)
            {
                bSelect = false;
                label_ChanID.BackColor = unselectColor;
                label_ChanID.ForeColor = selectColor;
                checkBox_Select.BackColor = unselectColor;
                this.Refresh();
            }
            /*发送事件*/
            RaiseEvent(strChanID +";"+ strTestTim + ";CheckBox;" + bCheck.ToString());
        }
        /// <summary>
        /// 绘制事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCHistoryComp_Paint(object sender, PaintEventArgs e)
        {
            if (bSelect)
            {
                drawFrame(e.Graphics, selectColor);
            }
            else
            {
                drawFrame(e.Graphics, unselectColor);
            }
        }
        /// <summary>
        /// load事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCHistoryComp_Load(object sender, EventArgs e)
        {
            label_MeasType.ForeColor = selectColor;
            label_DevName.ForeColor = selectColor;
            label_ChanACtual.ForeColor = selectColor;
            label_TestTim.ForeColor = selectColor;
            label_ChanID.BackColor = selectColor;
            label_ChanID.ForeColor = unselectColor;
            checkBox_Select.BackColor = selectColor;
        }
        #endregion

        #region Construction
        public UCHistoryComp()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strTag"></param>
        /// <param name="strMeasure"></param>
        /// <param name="strDev"></param>
        /// <param name="strChan"></param>
        /// <param name="strTime"></param>
        public UCHistoryComp(Color color,string strTag ,string strMeasure,string strDev,string strChan,string strTime)
        {
            InitializeComponent();
            selectColor = color;
            strChanID = strTag;
            label_ChanID.Text = strChanID;
            strMeasType = strMeasure;
            label_MeasType.Text = string.Format(label_MeasType.Text, strMeasType);
            strDevName = strDev;
            label_DevName.Text = string.Format(label_DevName.Text, strDevName);
            strChanActual = strChan;
            label_ChanACtual.Text = string.Format(label_ChanACtual.Text,strChanActual);
            strTestTim = strTime;
            label_TestTim.Text = string.Format(label_TestTim.Text, strTestTim);
            checkBox_Select.Checked = true;//checkbox为选中状态
            bCheck = true;
            bSelect = false;
            label_ChanID.BackColor = unselectColor;
            label_ChanID.ForeColor = selectColor;
            checkBox_Select.BackColor = unselectColor;
        }
        #endregion




        #region Methods
        /// <summary>
        /// 主动设置checkbox的状态
        /// </summary>
        /// <param name="bState"></param>
        public void checkBox_SelectSet(bool bState)
        {
            if (bState == true)
            {
                checkBox_Select.Checked = true;
            }
            else
            {
                checkBox_Select.Checked = false;
            }
        }
        /// <summary>
        /// 主动改变控件背景颜色
        /// </summary>
        /// <param name="bstate"></param>
        public void label_ChanID_Set(bool bstate)
        {
            if (this.InvokeRequired)
            {
                labelColor_Update s = new labelColor_Update(label_ChanID_Set);
                this.Invoke(s, bstate);
            }
            else
            {
                if (bstate == true)
                {
                    bSelect = true;
                    label_ChanID.BackColor = selectColor;
                    label_ChanID.ForeColor = Color.Black;
                    //label_Inv.BackColor = Color.Black;
                    checkBox_Select.BackColor = selectColor;
                }
                else
                {
                    bSelect = false;
                    label_ChanID.BackColor = unselectColor;
                    label_ChanID.ForeColor = selectColor;
                    //label_Inv.BackColor = selectColor;
                    checkBox_Select.BackColor = unselectColor;
                }
                update_UCHistView();
            }
        }
        /// <summary>
        /// 更新波形显示-屏幕刷新（委托）
        /// </summary>
        private void update_UCHistView()
        {
            if (this.InvokeRequired)
            {
                uc_Refresh s = new uc_Refresh(update_UCHistView);
                this.Invoke(s);
            }
            else
            {
                this.Refresh();
            }
        }
        /// <summary>
        /// 绘制边框
        /// </summary>
        /// <param name="g"></param>
        /// <param name="colorPen"></param>
        private void drawFrame(Graphics g, Color colorPen)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;  //图片柔顺模式选择
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;//高质量
            g.CompositingQuality = CompositingQuality.HighQuality;//再加一点
            Pen p = new Pen(colorPen, 3);//定义了一个灰色,宽度为2的画笔
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; //实线
            Rectangle rect = new Rectangle(this.ClientRectangle.X, this.ClientRectangle.Y,
                                             this.ClientRectangle.X + this.ClientRectangle.Width,
                                             this.ClientRectangle.Y + this.ClientRectangle.Height);

            g.DrawRectangle(p, rect);//绘制矩形框
        }
        /// <summary>
        /// 设置简略显示模式
        /// </summary>
        public void setBriefViewMode()
        {
            //label_Scale.Visible = false;
            //label_BwLimit.Visible = false;
            //label_Impdence.Visible = false;
            //label_Offset.Visible = false;
            this.Size = new Size(72, 85);
        }
        /// <summary>
        /// 设置详细显示模式
        /// </summary>
        public void setDetialViewMode()
        {
            this.Size = new Size(243, 85);
            //label_Scale.Visible = true;
            //label_BwLimit.Visible = true;
            //label_Impdence.Visible = true;
            //label_Offset.Visible = true;
        }
        #endregion
        

    }
}
