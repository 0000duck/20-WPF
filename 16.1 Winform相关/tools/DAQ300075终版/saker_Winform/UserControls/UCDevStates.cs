using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*****************************************************************************************************************
                                        普源精电科技有限公司版权所有(2020-)
******************************************************************************************************************
类名:      UCDevStates
功能描述： UCDevStates类中定义了设备监控的用户控件
作 者：    顾泽滔
版 本：    00.01.00.00
完成日期： 
修改历史： 
<作者>               <修改时间>               <版本>                    <修改描述>
*****************************************************************************************************************/

namespace saker_Winform.UserControls
{
    public partial class UCDevStates : UserControl
    {
        #region Fields
        private string strVirtNum;
        public string m_strVirtNum
        {
            get {return strVirtNum; }
            set { strVirtNum = value; }
        }


        /*设备在线状态*/
        private bool bConnect = true;
        public bool m_bConnect
        {
            get { return bConnect; }
            set { bConnect = value; }
        }
        /*设备SN*/
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
        /*触发状态*/
        private string strTrigState;
        public string m_strTrigState
        {
            get { return strTrigState; }
            set { strTrigState = value; }
        }
        /*时基*/
        private string strTimBase;
        public string m_strTimBase
        {
            get { return strTimBase; }
            set { strTimBase = value; }
        }

        /*位移*/
        private string strTimOffset;
        public string m_strTimOffset
        {
            get { return strTimOffset; }
            set { strTimOffset = value; }
        }

        /*采样率*/
        private string strAcq;
        public string m_strAcq
        {
            get { return strAcq; }
            set { strAcq = value; }
        }

        /*存储深度*/
        private string strMeDepth;
        public string m_strMeDepth
        {
            get { return strMeDepth; }
            set { strMeDepth = value; }
        }
        /*ch1状态*/
        private bool bCH1Enble;
        public bool m_bCH1Enble
        {
            get { return bCH1Enble; }
            set { bCH1Enble = value; }
        }
        /*ch2状态*/
        private bool bCH2Enble;
        public bool m_bCH2Enble
        {
            get { return bCH2Enble; }
            set { bCH2Enble = value; }
        }
        /*ch3状态*/
        private bool bCH3Enble;
        public bool m_bCH3Enble
        {
            get { return bCH3Enble; }
            set { bCH3Enble = value; }
        }
        /*ch4状态*/
        private bool bCH4Enble;
        public bool m_bCH4Enble
        {
            get { return bCH4Enble; }
            set { bCH4Enble = value; }
        }
        #endregion
        #region Construction
        public UCDevStates()
        {
            InitializeComponent();
        }

        public delegate void dispProcessRefresh_TrigState(string str);//申明委托

        public delegate void dispProcessRefresh_OnlineState(bool bState);//申明委托


        #endregion
        #region Events
        #endregion
        #region Methods

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.Style &= ~0x02000000; // Turn off WS_CLIPCHILDREN 
                return parms;
            }
        }

        /// <summary>
        /// 更新控健UI-不带参数
        /// </summary>
        public void updateDevStateUI()
        {
            if (strTrigState == "STOP")
            {
                label_TrigState.ForeColor = System.Drawing.Color.Crimson;
            }
            else
            {
                label_TrigState.ForeColor = System.Drawing.Color.ForestGreen;
            }
            label_DevSubName.Text = strDevSubName + "/" + strDevSN;
            label_TrigState.Text = strTrigState;
            label_TimBase.Text = "时基:" + strTimBase;
            label_TimOffset.Text = "位移:" + strTimOffset;
            label_Acq.Text = "Acq:" + strAcq;
            label_MDepth.Text = "Mdepth:" + strMeDepth;
            ucChanInfo_CH1.UpdateChanInfo();
            ucChanInfo_CH2.UpdateChanInfo();
            ucChanInfo_CH3.UpdateChanInfo();
            ucChanInfo_CH4.UpdateChanInfo();
        }
        /// <summary>
        /// 设置通道1信息
        /// </summary>
        /// <param name="strTag"></param>
        /// <param name="bState"></param>
        /// <param name="bImpedance"></param>
        public void setCH1Info(string strTag,bool bState,bool bImpedance)
        {
            ucChanInfo_CH1.m_euChanIndex = Global.CGlobalValue.euChanID.CH1;
            ucChanInfo_CH1.m_strChanTag = strTag;
            ucChanInfo_CH1.m_bChanImpedance = bImpedance;
            ucChanInfo_CH1.m_bChanState = bState;
        }
        /// <summary>
        /// 设置通道2信息
        /// </summary>
        /// <param name="strTag"></param>
        /// <param name="bState"></param>
        /// <param name="bImpedance"></param>
        public void setCH2Info(string strTag, bool bState, bool bImpedance)
        {
            ucChanInfo_CH2.m_euChanIndex = Global.CGlobalValue.euChanID.CH2;
            ucChanInfo_CH2.m_strChanTag = strTag;
            ucChanInfo_CH2.m_bChanImpedance = bImpedance;
            ucChanInfo_CH2.m_bChanState = bState;
        }
        /// <summary>
        /// 设置通道3信息
        /// </summary>
        /// <param name="strTag"></param>
        /// <param name="bState"></param>
        /// <param name="bImpedance"></param>
        public void setCH3Info(string strTag, bool bState, bool bImpedance)
        {
            ucChanInfo_CH3.m_euChanIndex = Global.CGlobalValue.euChanID.CH3;
            ucChanInfo_CH3.m_strChanTag = strTag;
            ucChanInfo_CH3.m_bChanImpedance = bImpedance;
            ucChanInfo_CH3.m_bChanState = bState;
        }
        /// <summary>
        /// 设置通道4信息
        /// </summary>
        /// <param name="strTag"></param>
        /// <param name="bState"></param>
        /// <param name="bImpedance"></param>
        public void setCH4Info(string strTag, bool bState, bool bImpedance)
        {
            ucChanInfo_CH4.m_euChanIndex = Global.CGlobalValue.euChanID.CH4;
            ucChanInfo_CH4.m_strChanTag = strTag;
            ucChanInfo_CH4.m_bChanImpedance = bImpedance;
            ucChanInfo_CH4.m_bChanState = bState;
        }
        /// <summary>
        /// 更新触发状态
        /// </summary>
        /// <param name="strstate"></param>
        public void updateTrigState(string strstate)
        {
            if (label_TrigState.InvokeRequired)
            {
                dispProcessRefresh_TrigState s = new dispProcessRefresh_TrigState(updateTrigState);
                label_TrigState.Invoke(s, strstate);
            }
            else
            {
                if (strstate.Contains("STOP"))
                {
                    label_TrigState.ForeColor = System.Drawing.Color.Crimson;
                    label_TrigState.Text = "STOP";
                    strTrigState = "STOP";
                }
                else if(strstate.Contains("RUN"))
                {
                    label_TrigState.ForeColor = System.Drawing.Color.ForestGreen;
                    label_TrigState.Text = "RUN";
                    strTrigState = "RUN";
                }
                else if(strstate.Contains("WAIT"))
                {
                    label_TrigState.ForeColor = System.Drawing.Color.ForestGreen;
                    label_TrigState.Text = "WAIT";
                    strTrigState = "WAIT";
                }
                else
                {
                    label_TrigState.ForeColor = System.Drawing.Color.ForestGreen;
                    label_TrigState.Text = strstate;
                    strTrigState = strstate;
                }
               
            }
        }
        /// <summary>
        /// 更新在线状态
        /// </summary>
        /// <param name="state"></param>
        public void updateOnlineState(bool state)
        {
            if (this.InvokeRequired)
            {
                dispProcessRefresh_OnlineState s = new dispProcessRefresh_OnlineState(updateOnlineState);
                this.Invoke(s, state);
            }
            else
            {
                if (state)
                {
                    iconPictureBox_State.IconChar = FontAwesome.Sharp.IconChar.Link;
                    iconPictureBox_State.IconColor = System.Drawing.SystemColors.MenuHighlight;
                    label_DevSubName.ForeColor = System.Drawing.SystemColors.MenuHighlight;
                }
                else
                {
                    iconPictureBox_State.IconChar = FontAwesome.Sharp.IconChar.Unlink;
                    iconPictureBox_State.IconColor = System.Drawing.SystemColors.ButtonShadow;
                    label_DevSubName.ForeColor = System.Drawing.SystemColors.ButtonShadow;
                }
                bConnect = state;

            }
        }
        #endregion
        
    }
}
