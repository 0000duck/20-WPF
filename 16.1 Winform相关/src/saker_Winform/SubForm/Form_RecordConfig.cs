using saker_Winform.Global;
using saker_Winform.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary_MultiLanguage;

namespace saker_Winform.SubForm
{
    public partial class Form_RecordConfig : Form
    {
        public Form_RecordConfig()
        {
            InitializeComponent();
        }

        #region 字段
        // Record Condition
        private string recordCondition;

        public string m_recordCondition
        {
            get { return recordCondition; }
            set { recordCondition = value; }
        }

        //绝对时间
        private DateTime AbsTime;

        public DateTime m_AbsTime
        {
            get { return AbsTime; }
            set { AbsTime = value; }
        }


        //能否点击触发按钮
        private bool enableForce =false;

        public bool m_enableForce
        {
            get { return enableForce; }
            set { enableForce = value; }
        }

        //是否附加时间
        private bool addTime;

        public bool m_addTime
        {
            get { return addTime; }
            set { addTime = value; }
        }

        #endregion

        #region 自定义事件
        /*定义事件参数类*/
        public class clickRecordFormEventArgs : EventArgs
        {
            public readonly string KeyToRaiseEvent;

            public clickRecordFormEventArgs(string keyToRaiseEvent)
            {
                KeyToRaiseEvent = keyToRaiseEvent;
            }
        }
        /*定义委托声明*/
        public delegate void clickRecordFormEventHandler(object sender, clickRecordFormEventArgs e);

        //用event关键字声明事件对象
        public event clickRecordFormEventHandler clickRecordFormEvent;

        //事件触发方法
        protected virtual void onClickRecordFormEvent(clickRecordFormEventArgs e)
        {
            if (clickRecordFormEvent != null)
            {
                clickRecordFormEvent(this, e);
            }
        }

        //引发事件
        private void RaiseEvent(string keyToRaiseEvent)
        {
            clickRecordFormEventArgs e = new clickRecordFormEventArgs(keyToRaiseEvent);

            onClickRecordFormEvent(e);
        }

        #endregion

        private void checkedListBox_RecordCondition_ItemCheck(object sender, ItemCheckEventArgs e)
        {           
            if (this.checkedListBox_RecordCondition.CheckedItems.Count > 0)
            {
                for (int i = 0; i < checkedListBox_RecordCondition.CheckedIndices.Count; i++)
                {
                    if (checkedListBox_RecordCondition.CheckedIndices[i] != e.Index)
                    {
                        checkedListBox_RecordCondition.SetItemChecked(checkedListBox_RecordCondition.CheckedIndices[i], false);                   
                    }
                }              
            }
            if (e.Index == 2)
            {
                m_enableForce = true;
            }
            RaiseEvent(m_enableForce.ToString());
            switch(checkedListBox_RecordCondition.SelectedIndex)
            {
                case -1:
                    m_recordCondition = CGlobalCmd.STR_CMD_SET_TRIG_EDGE_SLOP_POS;
                    break;
                case 0:
                    m_recordCondition = CGlobalCmd.STR_CMD_SET_TRIG_EDGE_SLOP_POS;                  
                    break;
                case 1:
                    m_recordCondition = CGlobalCmd.STR_CMD_SET_TRIG_EDGE_SLOP_NEG;                  
                    break;
                case 2:
                    m_recordCondition = CGlobalCmd.STR_CMD_SET_TRIG_TFORce;
                    break;
                case 3:
                    m_recordCondition = CGlobalCmd.STR_CMD_SET_TRIG_TFORce+"Abs";
                    m_AbsTime = this.dateTimePicker_Abs.Value;
                    break;
            }
           
        }

        /// <summary>
        /// 事件：Load面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_RecordConfig_Load(object sender, EventArgs e)
        {
            this.checkedListBox_RecordCondition.SetItemChecked(0, true);
            this.checkedListBox_RecordLocation.SetItemChecked(1, true);
            this.checkedListBox_RecordLocation.Enabled = false;
            this.checkedListBox_RecordType.SetItemChecked(1, true);
            this.checkedListBox_RecordType.Enabled = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("FileNameRule");
            dt.Columns.Add("FileNameRuleDesc");
            DataRow dr = dt.NewRow();
            dr[0] = "1";
            dr[1] = InterpretBase.textTran("通道标记");
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = "2";
            dr[1] = InterpretBase.textTran("设备名+通道数");
            dt.Rows.Add(dr);
            this.comboBox_NameRule.DataSource = dt;
            this.comboBox_NameRule.DisplayMember = "FileNameRuleDesc";
            this.comboBox_NameRule.ValueMember = "FileNameRule";         
        }     

        /// <summary>
        /// 事件：附加时间按钮被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_AddTime_CheckStateChanged(object sender, EventArgs e)
        {
            m_addTime = this.checkBox_AddTime.Checked;
        }

        /// <summary>
        /// 返回一个Dc
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> RecordConfigDc()
        {
            Dictionary<string, string> recordConfigDc = new Dictionary<string, string>();
            try
            {
                recordConfigDc.Add("CollectCondition", "");
                if (this.checkedListBox_RecordCondition.SelectedItems.Count != 0)
                {
                    recordConfigDc["CollectCondition"] = this.checkedListBox_RecordCondition.CheckedItems[0].ToString();
                }
                //    recordConfigDc.Add("AbsTime", this.dateTimePicker_Abs.Value.ToString());
                recordConfigDc.Add("RecordLocation", "");
                if (this.checkedListBox_RecordLocation.CheckedItems.Count != 0)
                {
                    recordConfigDc["RecordLocation"] = this.checkedListBox_RecordLocation.CheckedItems[0].ToString();
                }
                recordConfigDc.Add("FileType", "");
                if (this.checkedListBox_RecordLocation.CheckedItems.Count != 0)
                {
                    recordConfigDc["FileType"] = this.checkedListBox_RecordType.CheckedItems[0].ToString();
                }
                recordConfigDc.Add("FileLocation", this.textBox_FileLoaction.Text);
                recordConfigDc.Add("FileNameRule", "");
                if (this.comboBox_NameRule.SelectedIndex != -1)
                {
                    recordConfigDc["FileNameRule"] = this.comboBox_NameRule.SelectedValue.ToString();
                }              
                recordConfigDc.Add("FileNameRuleDesc", this.comboBox_NameRule.Text);
                recordConfigDc.Add("AbsTime", "");
                if (this.checkBox_AddTime.Checked)
                {
                    recordConfigDc["AbsTime"] = this.dateTimePicker_Abs.Value.ToString();
                }
                recordConfigDc.Add("IsAddDateTime", this.checkBox_AddTime.Checked.ToString());
                return recordConfigDc;
            }
            catch (Exception ex)
            {
                throw (ex);
            }                        
        }

        private void dateTimePicker_Abs_ValueChanged(object sender, EventArgs e)
        {
            m_AbsTime = this.dateTimePicker_Abs.Value;
        }
    }
}
