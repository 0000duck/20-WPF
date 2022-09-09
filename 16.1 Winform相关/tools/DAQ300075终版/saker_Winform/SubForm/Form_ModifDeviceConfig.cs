
using ClassLibrary_SocketClient;
using ClassLibrary_MultiLanguage;
using saker_Winform.CommonBaseModule;
using saker_Winform.Global;
using saker_Winform.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IPret = ClassLibrary_MultiLanguage.InterpretBase;//添加引用
using saker_Winform.Module;

namespace saker_Winform.SubForm
{
    public partial class Form_ModifDeviceConfig : Form
    {
        public Form_ModifDeviceConfig()
        {
            InitializeComponent();
            this.CenterToScreen();
            this.Text = "添加仪器";
            this.textBox_DeviceOtherName.Text = "Device_";
            this.comboBox_DeviceModel.SelectedIndex = 0;
            this.comboBox_Communication.SelectedIndex = 0;
            //this.comboBox_Communication.Enabled = false;
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

        private bool bModif;

        public bool m_bModif
        {
            get { return bModif; }
            set { bModif = value; }
        }
        private string fixedSN;

        public string m_fixedSN
        {
            get { return fixedSN; }
            set { fixedSN = value; }
        }

        private string OriIP;

        public string m_OriIP
        {
            get { return OriIP; }
            set { OriIP = value; }
        }

        private int Communication;

        public int m_Communication
        {
            get { return Communication; }
            set { Communication = value; }
        }

        public static string DeviceName;

        private void button1_Cancel_MouseClick(object sender, EventArgs e)
        {
            RegisterInfoStatus = false;
            this.Close();
        }

        private void button_OK_Click_1(object sender, EventArgs e)
        {
            //必须有内容才允许注册，不允许空值的出现
            if (textBox_DeviceIP.Text != "" && textBox_DeviceOtherName.Text != "")
            {
                RegisterInfoStatus = true;
                m_strDevSubName = this.textBox_DeviceOtherName.Text;
                m_strIP = this.textBox_DeviceIP.Text;
                m_strModel = this.comboBox_DeviceModel.Text;
                if (this.comboBox_Communication.Text == "千兆" || this.comboBox_Communication.Text == InterpretBase.textTran("千兆")) //TCP 
                {
                    m_Communication = 1;
                }
                else
                {
                    m_Communication = 0;
                }
                this.Close();
            }
            else
            {
                MessageBox.Show(InterpretBase.textTran("您的信息未填写完全，无法添加"));  //空值提醒
            }
        }

        /// <summary>
        /// 修改仪器信息
        /// </summary>
        public void ModifInfo(string deviceSN, string deviceIP, string subName)
        {
            bModif = true;
            this.button_RegistorView.Text = "修改";
            m_fixedSN = deviceSN;
            m_OriIP = deviceIP;
            this.textBox_SN.Text = deviceSN;
            this.textBox_DeviceIP.Text = deviceIP;
            this.textBox_DeviceOtherName.Text = subName;
            this.textBox_SN.Enabled = false;
        }

        private void Form_ModifDeviceConfig_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Form_Main.lang);
            IPret.LoadLanguage(Form_Main.lang);
            IPret.InitLanguage(this);
            this.textBox_DeviceIP.Focus();//设置控件焦点
            this.ActiveControl = this.textBox_DeviceIP;
            this.button_OK.Enabled = false;
            this.label_Mark.Visible = true;
        }

        public void RefreshDeviceName()
        {
            this.textBox_DeviceOtherName.Text = DeviceName;
            this.textBox_DeviceOtherName.Refresh();
        }

        public string[] SocketConnect()
        {
            byte[] idninfo = new byte[1024];
            IPAddress iPAddress;
            if (IPAddress.TryParse(textBox_DeviceIP.Text, out iPAddress))
            {
                if (CPingIP.PingIpConnect(textBox_DeviceIP.Text))
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    // int s32RetCode = CCommomControl.checkDevConfigClient(IPAddress.Parse(textBox_DeviceIP.Text).GetHashCode(), 1, 5555, 5555, idninfo);
                    //bool iPValid = SocketClientManage.Connect(textBox_DeviceIP.Text, 5555, 300);//300ms延时，看IP是否有效
                    //string receive = Encoding.Default.GetString(idninfo, 0, s32RetCode);
                    byte[] send = Encoding.Default.GetBytes(CGlobalCmd.STR_CMD_INQUIRE + "\n");
                    try
                    {
                        socket.Connect(textBox_DeviceIP.Text, 5555);
                        socket.Send(send);
                        int length = socket.Receive(idninfo);
                        string receive = Encoding.Default.GetString(idninfo, 0, length);
                        receive = receive.TrimEnd(Environment.NewLine.ToCharArray());
                        socket.Close();
                        string[] recv = receive.Split(';');
                        if (recv[0].Split(',')[1] == "DS8204-R" || recv[0].Split(',')[1] == "DS8104-R")
                        {
                            return recv;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, InterpretBase.textTran("提示信息"));
                        return null;
                       
                    }
                                                    
                }
                else
                {                   
                    MessageBox.Show(InterpretBase.textTran("您的IP无效，请确认"));  //空值提醒
                    return null;
                }

            }
            else
            {              
                MessageBox.Show(InterpretBase.textTran("您的IP无效，请确认"));  //空值提醒
                return null;
            }
        }

        /// <summary>
        /// 注册按钮被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_RegistView(object sender, EventArgs e)
        {                 
            if (textBox_DeviceIP.Text != "" && textBox_DeviceOtherName.Text != "")
            {
                if(!bModif)//注册
                {
                    string[] recv = SocketConnect();
                    if(recv == null)
                    {
                        return;
                    }
                    this.textBox_SN.Text = recv[0].Split(',')[2];
                    this.button_OK.Enabled = true;
                    this.label_Mark.Visible = false;
                }
                else//修改
                {
                    /* 仅仅修改名称 */
                    if (Module_DeviceManage.Instance.GetDeviceBySN(textBox_SN.Text).IP == textBox_DeviceIP.Text) 
                    {
                        Module_DeviceManage.Instance.GetDeviceBySN(textBox_SN.Text).Name = textBox_DeviceOtherName.Text;
                        MessageBox.Show(InterpretBase.textTran("修改成功"));
                        this.button_OK.Enabled = true;
                        this.label_Mark.Visible = false;
                    }
                    /* 修改IP */
                    else
                    {
                        string[] recv = SocketConnect();
                        if (recv == null)
                        {
                            return;
                        }
                        if (recv[0].Split(',')[2] != m_fixedSN)
                        {
                            MessageBox.Show(InterpretBase.textTran("您输入的IP对应的设备SN和原位置的不匹配，请重新输入"));
                            this.button_OK.Enabled = false;
                            this.label_Mark.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show(InterpretBase.textTran("修改成功"));
                            this.button_OK.Enabled = true;
                            this.label_Mark.Visible = false;
                        }
                    }               
                }               
            }
            else
            {
                MessageBox.Show(InterpretBase.textTran("信息未填写完全"));  //空值提醒
            }
        }

        private void textBox_DeviceIP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                CReflection.callObjectEvent(button_RegistorView, "OnClick", e);
            }
        }

        private void textBox_DeviceIP_KeyPress(object sender, KeyPressEventArgs e)
        {
            CString2Value.LimitInputIP(e);
        }
    }
}
