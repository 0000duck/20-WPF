using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using saker_Winform.Global;
/*****************************************************************************************************************
                                        普源精电科技有限公司版权所有(2020-)
******************************************************************************************************************
类名:      UCChanInfo
功能描述： UCChanInfo类中定义了通道信息的用户控件
作 者：    顾泽滔
版 本：    00.01.00.00
完成日期： 
修改历史： 
<作者>               <修改时间>               <版本>                    <修改描述>
*****************************************************************************************************************/

namespace saker_Winform.UserControls
{
    public partial class UCChanInfo : UserControl
    {
        #region Fields
        /*通道标记*/
        private string strChanTag;
        public string m_strChanTag
        {
            get { return strChanTag; }
            set { strChanTag = value; }
        }
        /*实际通道*/
        private CGlobalValue.euChanID euChanIndex;
        public CGlobalValue.euChanID m_euChanIndex
        {
            get { return euChanIndex; }
            set { euChanIndex = value; }
        }
        /*通道状态true->打开,false->关闭*/
        private bool bChanState;
        public bool m_bChanState
        {
            get { return bChanState; }
            set { bChanState = value; }
        }
        /*阻抗状态 false->50ohm,true->1M */
        private bool bChanImpedance;
        public bool m_bChanImpedance
        {
            get { return bChanImpedance; }
            set { bChanImpedance = value; }
        }
        #endregion
        #region Construction
        public UCChanInfo()
        {
            InitializeComponent();
        }
        
        #endregion
        #region Events
        #endregion
        #region Methods
        /// <summary>
        /// 更新通道信息-带参数
        /// </summary>
        /// <param name="bState">通道打开状态</param>
        /// <param name="bImpedance"></param>
        /// <param name="strSubName"></param>
        /// <param name="euChan"></param>
        public void UpdateChanInfo(bool bState, bool bImpedance, string strSubName, CGlobalValue.euChanID euChan)
        {
            switch (euChan)
            {
                case CGlobalValue.euChanID.CH1:
                    label_Name.Text = "CH1/" + strSubName;
                    break;
                case CGlobalValue.euChanID.CH2:
                    label_Name.Text = "CH2/" + strSubName;
                    break;
                case CGlobalValue.euChanID.CH3:
                    label_Name.Text = "CH3/" + strSubName;
                    break;
                case CGlobalValue.euChanID.CH4:
                    label_Name.Text = "CH4/" + strSubName;
                    break;
                default:
                    break;
            }
            euChanIndex = euChan;
            strChanTag = strSubName;
            if (bImpedance == false)
            {
                this.label_50Ohm.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
                this.label_50Ohm.ForeColor = System.Drawing.SystemColors.HotTrack;
                this.label_1M.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular);
                this.label_1M.ForeColor = System.Drawing.SystemColors.GrayText;
            }
            else
            {
                this.label_50Ohm.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular);
                this.label_50Ohm.ForeColor = System.Drawing.SystemColors.GrayText;
                this.label_1M.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
                this.label_1M.ForeColor = System.Drawing.SystemColors.HotTrack;
            }
            if (bState == true)
            {
                iconPictureBox_State.IconColor = System.Drawing.Color.FromArgb(50, 170, 102);
            }
            else
            {
                iconPictureBox_State.IconColor = System.Drawing.Color.FromArgb(75, 80, 93);
            }
            bChanImpedance = bImpedance;
            bChanState = bState;
        }
        /// <summary>
        /// 更新通道信息-不带参数
        /// </summary>
        /// <param name="bState"></param>
        /// <param name="bImpedance"></param>
        /// <param name="strSubName"></param>
        /// <param name="euChan"></param>
        public void UpdateChanInfo()
        {
            switch (euChanIndex)
            {
                case CGlobalValue.euChanID.CH1:
                    label_Name.Text = "CH1/" + strChanTag;
                    break;
                case CGlobalValue.euChanID.CH2:
                    label_Name.Text = "CH2/" + strChanTag;
                    break;
                case CGlobalValue.euChanID.CH3:
                    label_Name.Text = "CH3/" + strChanTag;
                    break;
                case CGlobalValue.euChanID.CH4:
                    label_Name.Text = "CH4/" + strChanTag;
                    break;
                default:
                    break;
            }
            if (bChanImpedance == false)
            {
                this.label_50Ohm.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
                this.label_50Ohm.ForeColor = System.Drawing.SystemColors.HotTrack;
                this.label_1M.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular);
                this.label_1M.ForeColor = System.Drawing.SystemColors.GrayText;
            }
            else
            {
                this.label_50Ohm.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular);
                this.label_50Ohm.ForeColor = System.Drawing.SystemColors.GrayText;
                this.label_1M.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
                this.label_1M.ForeColor = System.Drawing.SystemColors.HotTrack;
            }
            if (bChanState == true)
            {
                iconPictureBox_State.IconColor = System.Drawing.Color.FromArgb(50, 170, 102);
            }
            else
            {
                iconPictureBox_State.IconColor = System.Drawing.Color.FromArgb(75, 80, 93);
            }
        }
        #endregion

    }
}
