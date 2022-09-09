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
using IPret = ClassLibrary_MultiLanguage.InterpretBase;//添加引用

namespace saker_Winform.SubForm
{
    public partial class Form_SettingWaveMonitorViewScale : Form
    {
        /// <summary>
        /// 小图标范围
        /// </summary>
        private int littleNum;
        public int m_littleNum
        {
            get { return littleNum;}
            set { littleNum = value; }
        }
        /// <summary>
        /// 正常图标范围
        /// </summary>
        private int normalNum;
        public int m_normalNum
        {
            get { return normalNum; }
            set { normalNum = value; }
        }
        /// <summary>
        /// 大图标范围
        /// </summary>
        private int largeNum;
        public int m_largeNum
        {
            get { return largeNum; }
            set { largeNum = value; }
        }


        public bool bOkClick = false;


        public Form_SettingWaveMonitorViewScale()
        {
            InitializeComponent();
            this.Text = "显示范围设置";
        }

        public void RefreshTextBox(int autZonLittleScale,int autZonNormalScale,int autZonLargeScale)
        {
            this.textBox_Little.Text = autZonLittleScale.ToString();
            this.textBox_Normal.Text = autZonNormalScale.ToString();
            this.textBox_Large.Text = autZonLargeScale.ToString();
        }
        /// <summary>
        /// 确认按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Apply_Click(object sender, EventArgs e)
        {
            if ((textBox_Little.Text == "") || (textBox_Normal.Text == "") || (textBox_Large.Text == ""))
            {
                MessageBox.Show(InterpretBase.textTran("请输入有效的范围！"));
                return;
            }
            try
            {
                littleNum = Convert.ToInt32(textBox_Little.Text);   
                normalNum = Convert.ToInt32(textBox_Normal.Text);
                largeNum = Convert.ToInt32(textBox_Large.Text);
                if (littleNum <= 0||normalNum <=0||largeNum <= 0)
                {
                    MessageBox.Show(InterpretBase.textTran("最小显示个数需大于0"));
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(InterpretBase.textTran("请输入有效的范围！"));
                return;
            }
            bOkClick = true;
            this.Close();
        }
        /// <summary>
        /// 取消按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 键值过滤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Large_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8))
                e.Handled = true;
        }
        /// <summary>
        /// 键值过滤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Normal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8))
                e.Handled = true;
        }
        /// <summary>
        /// 键值过滤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Little_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8))
                e.Handled = true;
        }

        private void Form_SettingWaveMonitorViewScale_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Form_Main.lang);
            IPret.LoadLanguage(Form_Main.lang);
            IPret.InitLanguage(this);
        }
    }
}
