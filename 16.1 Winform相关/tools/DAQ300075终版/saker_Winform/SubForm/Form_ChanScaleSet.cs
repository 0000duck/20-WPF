using ClassLibrary_MultiLanguage;
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

namespace saker_Winform.SubForm
{
    public partial class Form_ChanScaleSet : Form
    {
        #region Fields
        /// <summary>
        /// 是否点击确认按键
        /// </summary>
        private bool bOk;

        public bool m_bOk
        {
            get { return bOk; }
            set { bOk = value; }
        }
        /// <summary>
        /// checkBox 是否选中
        /// </summary>
        private bool bCheckAll;

        public bool m_bCheckAll
        {
            get { return bCheckAll; }
            set { bCheckAll = value; }
        }

        /// <summary>
        /// 是否为autozone
        /// </summary>
        private bool bAutoZone;

        public bool m_bAutoZone
        {
            get { return bAutoZone; }
            set { bAutoZone = value; }
        }
        /// <summary>
        /// scalemax
        /// </summary>
        private double scaleMax;

        public double m_scaleMax
        {
            get { return scaleMax; }
            set { scaleMax = value; }
        }
        /// <summary>
        /// scalemin
        /// </summary>
        private double scaleMin;

        public double m_scaleMin
        {
            get { return scaleMin; }
            set { scaleMin = value; }
        }
        /// <summary>
        /// offset
        /// </summary>
        private double offSet;

        public double m_offSet
        {
            get { return offSet; }
            set { offSet = value; }
        }

        #endregion
        #region Construction
        public Form_ChanScaleSet()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 自定义构造函数
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="offset"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        public Form_ChanScaleSet(string strName, bool bAuto, double offset, double max, double min)
        {
            InitializeComponent();
            this.Text = strName;
            offSet = offset;
            textBox_ChanOffset.Text = offSet.ToString();
            scaleMax = max;
            textBox_ScaleMax.Text = scaleMax.ToString();
            scaleMin = min;
            textBox_ScaleMin.Text = scaleMin.ToString();
            if (bAuto == false)
            {
                textBox_ScaleMax.Enabled = false;
                textBox_ScaleMin.Enabled = false;
                checkBox_All.Enabled = false;
            }
            else
            {
                textBox_ChanOffset.Enabled = false;
            }
        }
        #endregion


        #region Events
        /// <summary>
        /// ok按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Ok_Click(object sender, EventArgs e)
        {
            bOk = true;
            if (textBox_ChanOffset.Text == ""|| textBox_ChanOffset.Text=="0")
            {
                offSet = 0.0;
            }
            else
            {
                double x = 0;
                if (double.TryParse(textBox_ChanOffset.Text, out x))
                {
                    if (x == 0)
                    {
                        MessageBox.Show(InterpretBase.textTran("偏移值输入非常规值"));
                        return;
                    }
                    else
                    {
                        offSet = x;
                    }
                }
            }

            if (textBox_ScaleMax.Text == ""|| textBox_ScaleMax.Text=="0")
            {
                scaleMax = 0.0;
            }
            else
            {
                double x = 0;
                if (double.TryParse(textBox_ScaleMax.Text, out x))
                {
                    if (x == 0)
                    {
                        MessageBox.Show(InterpretBase.textTran("最大值输入非常规值"));
                        return;
                    }
                    else
                    {
                        scaleMax = x;
                    }
                }
            }

            if (textBox_ScaleMin.Text == ""|| textBox_ScaleMin.Text=="0")
            {
                scaleMin = 0.0;
            }
            else
            {
                double x = 0;
                if (double.TryParse(textBox_ScaleMin.Text, out x))
                {
                    if(x==0)
                    {
                        MessageBox.Show(InterpretBase.textTran("最小值输入非常规值"));
                        return;
                    }
                    else
                    {
                        scaleMin = x;
                    }
                }                            
            }
            // 判断上下限是否合理
            if (scaleMin > scaleMax)
            {
                MessageBox.Show(InterpretBase.textTran("下限不能超过上限"));
                return;
            }
            else if (scaleMax == scaleMin)
            {
                MessageBox.Show(InterpretBase.textTran("上下限不能相等"));
                return;
            }

            this.Close();
        }
        /// <summary>
        /// 取消按键响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            bOk = false;
            this.Close();
        }
        #endregion

        private void textBox_ChanOffset_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46) && (e.KeyChar != 45))
                e.Handled = true;
        }

        private void textBox_ScaleMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46) && (e.KeyChar != 45))
                e.Handled = true;
        }

        private void textBox_ScaleMin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46) && (e.KeyChar != 45))
                e.Handled = true;
        }

        private void checkBox_All_CheckedChanged(object sender, EventArgs e)
        {
            bCheckAll = checkBox_All.Checked;
        }
        #region Methods
        #endregion


    }
}
