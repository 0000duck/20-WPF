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
/*****************************************************************************************************************
                                        普源精电科技有限公司版权所有(2020-)
******************************************************************************************************************
类名:      UCChanWaveView
功能描述： UCChanWaveView类实现用户控件的相关功能
作 者：    顾泽滔
版 本：    00.01.00.00
修改历史： 
<作者> <修改时间> <版本> <修改描述>
 顾泽滔       2020.5.9    初次修改
*****************************************************************************************************************/

namespace saker_Winform.UserControls
{
    public partial class UCChanWaveView : UserControl
    {
        #region Properties
        /// <summary>
        /// 标示当前是否在autozone模式
        /// </summary>
        private bool bAutoZone = true;
        public bool m_bAutoZone
        {
            get { return bAutoZone;}
            set { bAutoZone = value ;}
        }
        /// <summary>
        /// 是否选中
        /// </summary>
        private bool bSelect = false ;
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
        private string strChanID;
        [Browsable(true)]
        [Description("通道标记信息")]
        public string m_strChanID
        {
            get { return strChanID; }
            set { strChanID = value; }
        }
        /// <summary>
        /// 通道偏移
        /// </summary>
        private string strChanOffset;
        [Browsable(true)]
        [Description("通道偏移信息")]
        public string m_strChanoffset
        {
            get { return strChanOffset; }
            set { strChanOffset = value; }
        }
        /// <summary>
        /// 垂直范围的最大值
        /// </summary>
        private double vertScalMax;
        [Browsable(true)]
        [Description("通道垂直范围的最大值")]
        public double m_vertScalMax
        {
            get {return vertScalMax ;}
            set { vertScalMax = value; }
        }
        /// <summary>
        /// 垂直范围的最小值
        /// </summary>
        private double vertScalMin;
        [Browsable(true)]
        [Description("通道垂直范围的最大值")]
        public double m_vertScalMin
        {
            get { return vertScalMin; }
            set { vertScalMin = value; }
        }

        public delegate void uc_Refresh();//申明用户控件的刷新委托
        public delegate void labelColor_Update(bool bState);//申明背景颜色设置的委托
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
        /// <summary>
        /// 点击label事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label_ChanID_Click(object sender, EventArgs e)
        {
            if (bCheck == false)
            {
                return;
            }
            /*发送事件*/
            RaiseEvent(strChanID);
        }
        /// <summary>
        /// load事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCChanWaveView_Load(object sender, EventArgs e)
        {
            label_BwLimit.ForeColor = selectColor;
            label_Impdence.ForeColor = selectColor;
            label_Offset.ForeColor = selectColor;
            label_Scale.ForeColor = selectColor;
            label_ChanID.BackColor = selectColor;
            label_ChanID.ForeColor =  unselectColor;
            label_Inv.BackColor = selectColor;
            checkBox_Select.BackColor = selectColor;
        }
        /// <summary>
        /// checkBox状态改变事件
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
                label_Inv.BackColor = selectColor;
                checkBox_Select.BackColor = unselectColor;
                this.Refresh();
            }
            /*发送事件*/
            RaiseEvent(strChanID + ":CheckBox:" + bCheck.ToString());
        }
        /// <summary>
        /// 绘制事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCChanWaveView_Paint(object sender, PaintEventArgs e)
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
        /// 标签双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label_ChanID_DoubleClick(object sender, EventArgs e)
        {
            /*发送事件*/
            RaiseEvent(strChanID + ":DoubleClick");
        }
        #endregion
        
        #region Construction

        public UCChanWaveView()
        {
            InitializeComponent();
        }
                /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="color"></param>
        /// <param name="strChanID"></param>
        /// <param name="strScale"></param>
        /// <param name="strOffset"></param>
        /// <param name="bInv"></param>
        /// <param name="bBWLimit"></param>
        /// <param name="bImpedence"></param>
        public UCChanWaveView(Color color, string strID, string strScale, string strOffset, bool bInv, bool bBWLimit, bool bImpedence)
        {
            InitializeComponent();
            selectColor = color;
            label_ChanID.Text = strID;
            strChanID = strID;
            label_Scale.Text = strScale;
            label_Offset.Text = strOffset;
            strChanOffset = strOffset;
            if (bInv == false )
            {
                label_Inv.Visible = false;
            }
            if (bBWLimit == false)
            {
                label_BwLimit.Visible = false;
            }
            if (bImpedence == true)// false->50ohm,true->1M
            {
                label_Impdence.Visible = false;
            }
            checkBox_Select.Checked = true;//checkbox为选中状态
            bCheck = true;
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
                this.Invoke(s,bstate);
            }
            else
            {
                if (bstate == true)
                {
                    bSelect = true;
                    label_ChanID.BackColor = selectColor;
                    label_ChanID.ForeColor = Color.Black;
                    label_Inv.BackColor = Color.Black;
                    checkBox_Select.BackColor = selectColor;
                }
                else
                {
                    bSelect = false;
                    label_ChanID.BackColor = unselectColor;
                    label_ChanID.ForeColor = selectColor;
                    label_Inv.BackColor = selectColor;
                    checkBox_Select.BackColor = unselectColor;
                }
                //this.Refresh();
                update_UCChanView();
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
        /// 更新偏移
        /// </summary>
        /// <param name="vertOffset"></param>
        public void updateChanOffset(double vertOffset)
        {
            string strOffset = "";
            strOffset = CValue2String.voltage2String(vertOffset);
            label_Offset.Text = strOffset;
            strChanOffset = strOffset; 
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
            this.Size = new Size(62,53);
        }
        /// <summary>
        /// 设置详细显示模式
        /// </summary>
        public void setDetialViewMode()
        {
            this.Size = new Size(143, 53);
            //label_Scale.Visible = true;
            //label_BwLimit.Visible = true;
            //label_Impdence.Visible = true;
            //label_Offset.Visible = true;
        }
        /// <summary>
        /// 更新波形显示-屏幕刷新（委托）
        /// </summary>
        private void update_UCChanView()
        {
            if (this.InvokeRequired)
            {
                uc_Refresh s = new uc_Refresh(update_UCChanView);
                this.Invoke(s);
            }
            else
            {
                this.Refresh();
            }
        }
        /// <summary>
        /// 更新sacle范围
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public void ucChanViewUpdateScale(double minValue,double maxValue)
        {
            vertScalMin = minValue;
            vertScalMax = maxValue;
        }
        #endregion


    }
}
