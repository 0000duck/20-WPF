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
    public partial class Form_RegisterInfo : Form
    {
        public Form_RegisterInfo()
        {
            InitializeComponent();
            this.CenterToScreen();
            this.ShowInTaskbar = false;
            this.Text = "注册仪器";
            this.comboBox_DeviceModel.SelectedIndex = 0;
            this.textBox_DeviceOtherName.Text = "Device_";
        }
        public bool RegisterInfoStatus = false;

        private string strDevSubName;

        public string m_strDevSubName
        {
            get { return strDevSubName; }
            set { strDevSubName = value; }
        }

        private string strSN;

        public string m_strSN
        {
            get { return strSN; }
            set { strSN = value; }
        }

        private string strIP;

        public string m_strIP
        {
            get { return strIP; }
            set { strIP = value; }
        }

        private string strModel;

        public string m_strModel
        {
            get { return strModel; }
            set { strModel = value; }
        }

        private int communication;

        public int m_communication
        {
            get { return communication; }
            set { communication = value; }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {

            //必须有内容才允许注册，不允许空值的出现
            if(textBox_DeviceIP.Text != ""&&textBox_DeviceOtherName.Text!="")
            {
                RegisterInfoStatus = true;
                m_strDevSubName = this.textBox_DeviceOtherName.Text;
                m_strIP = this.textBox_DeviceIP.Text;
                m_strModel = this.comboBox_DeviceModel.Text;
                m_strSN = this.textBox_SN.Text;
                if(this.comboBox_Communication.Text == "千兆"|| this.comboBox_Communication.Text == InterpretBase.textTran("千兆"))
                {
                    m_communication = 1;
                }
                else { m_communication = 0; }
                this.Close();
            }
            else
            {
                MessageBox.Show(InterpretBase.textTran("您的信息未填写完全，无法注册"));  //空值提醒
            }
            RegisterInfoStatus = true;

        }

        private void button1_Cancel_MouseClick(object sender, MouseEventArgs e)
        {
            RegisterInfoStatus = false;
            this.Close();
        }

        private void Form_RegisterInfo_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Form_Main.lang);
            IPret.LoadLanguage(Form_Main.lang);
            IPret.InitLanguage(this);
            this.textBox_DeviceOtherName.Focus();//设置控件焦点
            this.ActiveControl = this.textBox_DeviceOtherName;
            this.comboBox_Communication.SelectedIndex = 0;
        }

        public void disAbleTextBox()
        {          
            this.textBox_DeviceIP.Text = m_strIP;
            this.textBox_SN.Text = m_strSN;
            this.textBox_DeviceIP.Enabled = false;
            this.textBox_SN.Enabled = false;
            this.comboBox_DeviceModel.Enabled = false;
        }
    }
}
