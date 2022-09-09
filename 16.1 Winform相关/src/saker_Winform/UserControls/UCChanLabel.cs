using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using saker_Winform.CommonBaseModule;

namespace saker_Winform.UserControls
{
    public partial class UCChanLabel : UserControl
    {
        #region Properties
        /// <summary>
        /// 显示的标签的内容
        /// </summary>
        private string strChanId;
        [Browsable(true)]
        [Description("标签显示的内容")]
        public string m_strChanID
        {
            get 
            {
                return strChanId;
            }
            set
            {
                strChanId = value;
            }
        }
        /// <summary>
        /// 标签的颜色
        /// </summary>
        private Color labelColor = Color.Orange;
        [Browsable(true)]
        [Description("标签颜色")]
        public Color m_labelColor
        {
            get 
            {
                return labelColor;
            }
            set
            {
                labelColor = value;
            }
        }
        #endregion

        #region Construstion
        public UCChanLabel()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        /*定义事件参数类*/
        public class chanLabelEventArgs : EventArgs
        {
            public readonly string KeyToRaiseEvent;
            public chanLabelEventArgs(string keyToRaiseEvent)
            {
                KeyToRaiseEvent = keyToRaiseEvent;
            }
        }
        //定义delegate委托申明
        public delegate void chanLabelEventHandler(object sender, chanLabelEventArgs e);
        //用event 关键字声明事件对象
        [Browsable(true)]
        [Description("自定义事件，向绑定的窗体发送对应字符串信息")]
        public event chanLabelEventHandler chanLabelEvent;
        //事件触发方法
        protected virtual void onChanLabelEvent(chanLabelEventArgs e)
        {
            if (chanLabelEvent != null)
                chanLabelEvent(this, e);
        }
        //引发事件
        private void RaiseEvent(string keyToRaiseEvent)
        {
            chanLabelEventArgs e = new chanLabelEventArgs(keyToRaiseEvent);
            onChanLabelEvent(e);
        }

        /// <summary>
        /// 波形标签移动-鼠标按下
        /// 反射到用户控件本身
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label_ChanID_MouseDown(object sender, MouseEventArgs e)
        {
            CReflection.callObjectEvent(this, "OnMouseDown", e);
        }
        /// <summary>
        /// 波形标签移动-鼠标移动
        /// 反射到用户控件本身
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label_ChanID_MouseMove(object sender, MouseEventArgs e)
        {
            CReflection.callObjectEvent(this, "OnMouseMove", e);
        }
        /// <summary>
        /// 波形标签移动-鼠标抬起
        /// 反射到用户控件本身
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label_ChanID_MouseUp(object sender, MouseEventArgs e)
        {
            CReflection.callObjectEvent(this, "OnMouseUp", e);
        }
        #endregion

        #region Methods
        /// <summary>
        /// 设置标签的内容和颜色
        /// </summary>
        /// <param name="strID"></param>
        /// <param name="clrLable"></param>
        public void setLabel(string strID, Color clrLable)
        {
            label_ChanID.Text = strID;
            label_ChanID.ForeColor = clrLable;
            iconPictureBox_TagTop.IconColor = clrLable;
            //button_Tag.BackColor = clrLable;
            labelColor = clrLable;
        }
        #endregion




    }
}
