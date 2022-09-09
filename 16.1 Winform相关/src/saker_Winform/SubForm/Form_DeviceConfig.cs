using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using saker_Winform.CommonBaseModule;
using saker_Winform.Global;
using saker_Winform.UserControls;
using saker_Winform.Module;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using ClassLibrary_MultiLanguage;

/*****************************************************************************************************************
                                        普源精电科技有限公司版权所有(2020-)
******************************************************************************************************************
类名:      Form_DeviceConfig
功能描述： Form_DeviceConfig实现仪器配置
作 者：    赵鸿勇
版 本：    00.01.00.00
完成日期： 
修改历史： 
<作者>               <修改时间>               <版本>                    <修改描述>
*****************************************************************************************************************/

namespace saker_Winform.SubForm
{
    public partial class Form_DeviceConfig : UCBaseFrm
    {
        public Form_DeviceConfig()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        #region 私有字段
        private CFormAutoSize fAutoSize = new CFormAutoSize();//设置窗体自动大小调节
        private delegate void dlgDisableBtn(bool disableBtn);//声明改变顶端
        #endregion

        #region 定义设备信息的数据结构
        /*定义设备信息的数据结构*/
        public struct stDevInfo
        {
            public string strDevNumber;//仪器编号
            public string strDevSN;//设备SN
            public string strDevIP;//设备IP
            public string strDevSubName;//设备别名
            public string strDevModel;//设备型号
            public string strFirmVersion;//固件版本
            public string strRackNumber;//机架编号
            public string strMac;//Mac地址
            public bool bStatus;
            public bool bLink;
        }
        /*设备信息列表*/
        List<UCUserBaseNullDeviceInfo> listDevInfo = new List<UCUserBaseNullDeviceInfo>();

        #endregion

        #region Load仪器配置面板
        private void Form_DeviceConfig_Load(object sender, EventArgs e) //Load仪器配置面板
        {
            /* 去除窗体的标题栏 */
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;

            /* 初始化流式布局控件 */
            flowLayoutPanel_DeviceConfig.AutoScroll = false;
            flowLayoutPanel_DeviceConfig.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel_DeviceConfig.WrapContents = true;
            flowLayoutPanel_DeviceConfig.HorizontalScroll.Maximum = 0;
            flowLayoutPanel_DeviceConfig.AutoScroll = true;

            flowLayoutPanel_RegDevList.AutoScroll = false;
            flowLayoutPanel_RegDevList.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel_RegDevList.WrapContents = true;
            flowLayoutPanel_RegDevList.HorizontalScroll.Maximum = 0;
            flowLayoutPanel_RegDevList.AutoScroll = true;

            flowLayoutPanel_OnlineList.AutoScroll = false;
            flowLayoutPanel_OnlineList.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel_OnlineList.WrapContents = true;
            flowLayoutPanel_OnlineList.HorizontalScroll.Maximum = 0;
            flowLayoutPanel_OnlineList.AutoScroll = true;
            // this.flowLayoutPanel_DeviceConfig.Cursor = Cursors.Default;

            /* 更改按键颜色 */
            btn_RegisterIncrument.BackColor = Color.DarkGray;
            btn_Search.BackColor = Color.DarkGray;
            btn_DeletDev.BackColor = Color.DarkGray;
            btn_Delet.BackColor = Color.DarkGray;
            btn_DeletDev.Enabled = true;
            btn_Delet.Enabled = true;

            /* 控件不可见 */
            pictureBox_Device.Visible = false;
            label_DevSN.Visible = false;
            label_DevModel.Visible = false;
            label_FirmVersion.Visible = false;
            label_DevNumber.Visible = false;
            label_DeviceSubName.Visible = false;
            label_DeviceIP.Visible = false;

            /* 控件可拖拽 */
            this.flowLayoutPanel_DeviceConfig.AllowDrop = true;
            this.flowLayoutPanel_RegDevList.AllowDrop = true;
            this.flowLayoutPanel_OnlineList.AllowDrop = true;

            //窗体自动大小调节初始化
            //   fAutoSize.formInitializeSize(this.panel_Buttom);
            #endregion

            #region listDevInfo管理自定义控件信息   
            Parallel.For(0, 75, item =>
            {
                UCUserBaseNullDeviceInfo userBaseNullDeviceInfo = new UCUserBaseNullDeviceInfo();
                listDevInfo.Add(userBaseNullDeviceInfo);
            }
           );

            /*在左侧流式布局中添加75个空的位置*/
            Debug.WriteLine(DateTime.Now.ToString("hh:mm:ss fff"));
            Parallel.For(0, 75, item =>
            {
                stDevInfo _stDevInfo = new stDevInfo();//数据结构赋值
                _stDevInfo.strDevNumber = (item + 1).ToString().PadLeft(3, '0');//设备虚拟编号
                _stDevInfo.strDevSN = "";
                _stDevInfo.strDevIP = "";
                _stDevInfo.strDevSubName = "";
                _stDevInfo.strDevModel = "";
                _stDevInfo.strFirmVersion = "";
                _stDevInfo.strRackNumber = "";
                _stDevInfo.strMac = "";
                _stDevInfo.bStatus = false;
                _stDevInfo.bLink = false;

                //listDevInfo.Add(new UCUserBaseNullDeviceInfo());
                listDevInfo[item].m_strDevNumber = _stDevInfo.strDevNumber;
                listDevInfo[item].m_strDevSN = _stDevInfo.strDevSN;//SN
                listDevInfo[item].m_strDevIP = _stDevInfo.strDevIP;//IP
                listDevInfo[item].m_strDevSubName = _stDevInfo.strDevSubName;//设备别名
                listDevInfo[item].m_strDevModel = _stDevInfo.strDevModel;//设备型号
                listDevInfo[item].m_strFirmVersion = _stDevInfo.strFirmVersion;//固件版本
                listDevInfo[item].m_strRackNumber = _stDevInfo.strRackNumber;//序列号
                listDevInfo[item].m_bStatus = _stDevInfo.bLink;//仪器在线状态
                listDevInfo[item].m_strMac = _stDevInfo.strMac;//Mac地址
                listDevInfo[item].AllowDrop = true;
                listDevInfo[item].DevInfo();//刷新自定义控件使其变为+形状
                listDevInfo[item].MouseDown += UserBaseNullDeviceInfo_MouseDown;
                listDevInfo[item].DragEnter += UserBaseNullDeviceInfo_DragEnter;
                listDevInfo[item].DragDrop += UserBaseNullDeviceInfo_DragDrop;
                Subscribe(listDevInfo[item]);
            });
            Debug.WriteLine(DateTime.Now.ToString("hh:mm:ss fff"));
            /******将listDevInfo管理的控件通过流式布局进行显示******/
            foreach (UCUserBaseNullDeviceInfo devInfo in listDevInfo)
            {
                flowLayoutPanel_DeviceConfig.Controls.Add(devInfo);  //将表里的数据add到flowlayoutPanel_DeviceConfig中
                flowLayoutPanel_DeviceConfig.AllowDrop = true;
            }

            //从机模式
            if (Form_Main.m_vistorLogin)
            {
                disableBtn(false);
            }
        }
        #endregion

        #region 方法：禁用/解除禁用部分Btn
        public void disableBtn(bool bdisableBtn)
        {
            if (this.InvokeRequired)
            {
                dlgDisableBtn s = new dlgDisableBtn(disableBtn);
                this.Invoke(s, bdisableBtn);
            }
            else
            {
                if (bdisableBtn)
                {
                    this.btn_RegisterIncrument.Enabled = true;
                    this.btn_DeletDev.Enabled = true;
                    this.btn_Search.Enabled = true;
                    this.btn_Delet.Enabled = true;
                    this.userBaseNullDeviceInfo1.Enabled = true;
                    this.Refresh();
                }
                else
                {
                    this.btn_RegisterIncrument.Enabled = false;
                    this.btn_DeletDev.Enabled = false;
                    this.btn_Search.Enabled = false;
                    this.btn_Delet.Enabled = false;
                    this.userBaseNullDeviceInfo1.Enabled = false;
                    this.Refresh();
                }
            }

            

        }
        #endregion

        #region 方法：对设备进行二次赋值
        /// <summary>
        /// 方法：对设备进行二次赋值
        /// </summary>
        public void OpenfileXmlDeviceConfig()
        {
            /*如果设备信息不为空则进行赋值，针对第二次开程序*/
            if (Module.Module_DeviceManage.Instance.Devices.Count != 0)
            {
                foreach (var item in Module.Module_DeviceManage.Instance.Devices)
                {
                    listDevInfo.ElementAt(Convert.ToInt32(item.VirtualNumber) - 1).m_strDevSN = item.SN.ToString();//SN
                    listDevInfo.ElementAt(Convert.ToInt32(item.VirtualNumber) - 1).m_strDevSubName = item.Name.ToString();//设备别名
                    listDevInfo.ElementAt(Convert.ToInt32(item.VirtualNumber) - 1).m_strDevIP = item.IP.ToString();//IP
                    listDevInfo.ElementAt(Convert.ToInt32(item.VirtualNumber) - 1).m_strDevModel = item.Model.ToString();//设备型号
                    listDevInfo.ElementAt(Convert.ToInt32(item.VirtualNumber) - 1).m_strFirmVersion = item.SoftVersion.ToString();//固件版本
                    listDevInfo.ElementAt(Convert.ToInt32(item.VirtualNumber) - 1).m_bStatus = true;
                    listDevInfo.ElementAt(Convert.ToInt32(item.VirtualNumber) - 1).m_bLink = item.Status;//仪器在线状态
                    listDevInfo.ElementAt(Convert.ToInt32(item.VirtualNumber) - 1).DevInfo();
                }
            }
        }
        #endregion

        #region  方法：状态监测
        /// <summary>
        /// 方法：状态监测
        /// </summary>
        public void StateMonitor(string SN)
        {
            /*如果设备信息不为空则进行赋值，针对第二次开程序*/
            if (Module.Module_DeviceManage.Instance.Devices.Count != 0)
            {
                foreach (UCUserBaseNullDeviceInfo item in flowLayoutPanel_DeviceConfig.Controls)
                {
                    if (item.m_strDevSN == SN)
                    {
                        item.m_bLink = Module_DeviceManage.Instance.GetDeviceBySN(SN).Status;//更新仪器在线状态
                        item.m_strDevIP = Module_DeviceManage.Instance.GetDeviceBySN(SN).IP;//更新仪器IP
                        item.m_bStatus = true;
                        item.DevInfo();//刷新自定义控件显示
                    }
                }
            }
        }
        #endregion

        #region 事件：拖放操作
        /// <summary>
        /// 注册栏仪器被MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void userBaseNullDeviceInfo1_MouseDown(object sender, MouseEventArgs e)
        {

            UCUserBaseNullDeviceInfo usReg = (UCUserBaseNullDeviceInfo)sender;
            usReg.DoDragDrop(usReg, DragDropEffects.Move);
        }
        /// <summary>
        /// 设备栏列表被MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserBaseNullDeviceInfo_MouseDown(object sender, MouseEventArgs e) //鼠标点击
        {
            UCUserBaseNullDeviceInfo userBaseNullDeviceInfo = (UCUserBaseNullDeviceInfo)sender;
            if (userBaseNullDeviceInfo.m_strDevIP != "")
            {
                userBaseNullDeviceInfo.DoDragDrop(userBaseNullDeviceInfo, DragDropEffects.Move);
            }
            else
            {
                userBaseNullDeviceInfo.DoDragDrop(userBaseNullDeviceInfo, DragDropEffects.None);
            }
        }

        /// <summary>
        /// 拖放进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserBaseNullDeviceInfo_DragEnter(object sender, DragEventArgs e)//拖放进入时发生
        {
            e.Effect = DragDropEffects.Move;//拖放效果为移动
        }

        /// <summary>
        /// 拖放完成时，分为两种情况：在流布局中或注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserBaseNullDeviceInfo_DragDrop(object sender, DragEventArgs e)   //拖放完成时发生
        {
            UCUserBaseNullDeviceInfo uCBtn = (UCUserBaseNullDeviceInfo)e.Data.GetData(typeof(UCUserBaseNullDeviceInfo));//源控件
            UCSmallDev uCSmallDev = (UCSmallDev)e.Data.GetData(typeof(UCSmallDev));//源控件
            UCUserBaseNullDeviceInfo uCBtnDragDrop = (UCUserBaseNullDeviceInfo)sender; //目标控件
            if (uCBtn != null)
            {
                if (uCBtnDragDrop.m_strDevIP == "")  //目标控件IP为空
                {
                    if (uCBtn.m_strDevIP != null) //源控件IP不为null，在流列表中
                    {
                        //通过IP从Device_Manager中获取相应的设备信息
                        uCBtnDragDrop.m_strDevIP = uCBtn.m_strDevIP;  //将源控件的Ip给拖放完成的IP
                        uCBtnDragDrop.m_bLink = Module.Module_DeviceManage.Instance.GetDeviceByIP(uCBtn.m_strDevIP).Status;//在线状态
                        uCBtnDragDrop.m_bStatus = true;
                        uCBtnDragDrop.m_strDevSubName = Module.Module_DeviceManage.Instance.GetDeviceByIP(uCBtn.m_strDevIP).Name;//设备别名
                        uCBtnDragDrop.m_strDevModel = Module.Module_DeviceManage.Instance.GetDeviceByIP(uCBtn.m_strDevIP).Model;//型号
                        uCBtnDragDrop.m_strFirmVersion = Module.Module_DeviceManage.Instance.GetDeviceByIP(uCBtn.m_strDevIP).SoftVersion;//固件版本
                        uCBtnDragDrop.m_strRackNumber = Module.Module_DeviceManage.Instance.GetDeviceByIP(uCBtn.m_strDevIP).SN; //序列号（SN）
                        uCBtnDragDrop.m_strDevSN = Module.Module_DeviceManage.Instance.GetDeviceByIP(uCBtn.m_strDevIP).SN;//序列号SN
                        uCBtnDragDrop.m_strMac = Module.Module_DeviceManage.Instance.GetDeviceByIP(uCBtn.m_strDevIP).MAC;//Mac
                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtn.m_strDevIP).VirtualNumber = uCBtnDragDrop.m_strDevNumber;//将目标控件的虚拟编号更新列表信息  
                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtn.m_strDevIP).GetChannel(1).Tag = "Tag" + ((int.Parse(uCBtnDragDrop.m_strDevNumber) - 1) * 4 + 1).ToString().PadLeft(3, '0');
                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtn.m_strDevIP).GetChannel(2).Tag = "Tag" + ((int.Parse(uCBtnDragDrop.m_strDevNumber) - 1) * 4 + 2).ToString().PadLeft(3, '0');
                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtn.m_strDevIP).GetChannel(3).Tag = "Tag" + ((int.Parse(uCBtnDragDrop.m_strDevNumber) - 1) * 4 + 3).ToString().PadLeft(3, '0');
                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtn.m_strDevIP).GetChannel(4).Tag = "Tag" + ((int.Parse(uCBtnDragDrop.m_strDevNumber) - 1) * 4 + 4).ToString().PadLeft(3, '0');
                        /* 从Module_ViewConfig单例中更新Tag,查询原始的Tag来检索，而后直接更新单例中的Tag值，可优化查询算法 */
                        for (int j = 1; j <= 4; j++)
                        {
                            for (int i = 0; i < Module_ViewConfig.Instance.ViewConfig[j.ToString()].Count; i++)
                            {
                                if(("Tag" + ((int.Parse(uCBtn.m_strDevNumber) - 1) * 4 + 1).ToString().PadLeft(3, '0'))== Module_ViewConfig.Instance.ViewConfig[j.ToString()][i].Tag)
                                {
                                    Module_ViewConfig.Instance.ViewConfig[j.ToString()][i].Tag = "Tag" + ((int.Parse(uCBtnDragDrop.m_strDevNumber) - 1) * 4 + 1).ToString().PadLeft(3, '0');
                                }
                                else if(("Tag" + ((int.Parse(uCBtn.m_strDevNumber) - 1) * 4 + 2).ToString().PadLeft(3, '0')) == Module_ViewConfig.Instance.ViewConfig[j.ToString()][i].Tag)
                                {
                                    Module_ViewConfig.Instance.ViewConfig[j.ToString()][i].Tag = "Tag" + ((int.Parse(uCBtnDragDrop.m_strDevNumber) - 1) * 4 + 2).ToString().PadLeft(3, '0');
                                }
                                else if(("Tag" + ((int.Parse(uCBtn.m_strDevNumber) - 1) * 4 + 3).ToString().PadLeft(3, '0')) == Module_ViewConfig.Instance.ViewConfig[j.ToString()][i].Tag)
                                {
                                    Module_ViewConfig.Instance.ViewConfig[j.ToString()][i].Tag = "Tag" + ((int.Parse(uCBtnDragDrop.m_strDevNumber) - 1) * 4 + 3).ToString().PadLeft(3, '0');
                                }
                                else if(("Tag" + ((int.Parse(uCBtn.m_strDevNumber) - 1) * 4 + 4).ToString().PadLeft(3, '0')) == Module_ViewConfig.Instance.ViewConfig[j.ToString()][i].Tag)
                                {
                                    Module_ViewConfig.Instance.ViewConfig[j.ToString()][i].Tag = "Tag" + ((int.Parse(uCBtnDragDrop.m_strDevNumber) - 1) * 4 + 4).ToString().PadLeft(3, '0');
                                }                            
                            }
                        }              
                        uCBtnDragDrop.DevInfo();//刷新控件显示
                        uCBtn.clearControl();//清空源控件的显示信息
                        uCBtn.clearPropfull();//清空源控件的属性值   
                        /*改变拖放两个控件的背景颜色*/
                        Panel_SetUnselect();
                        bool pstate = true;
                        uCBtnDragDrop.m_selectColor = Color.DodgerBlue;
                        uCBtnDragDrop.panel_Set(pstate);                     
                        gvValue(uCBtnDragDrop);
                    }
                    else
                    {
                        Form_ModifDeviceConfig form_ModifDeviceConfig = new Form_ModifDeviceConfig();//弹出添加仪器的设备信息
                        Form_ModifDeviceConfig.DeviceName = "Device_" + uCBtnDragDrop.m_strDevNumber;
                        form_ModifDeviceConfig.RefreshDeviceName();
                        form_ModifDeviceConfig.ShowDialog();
                        if (form_ModifDeviceConfig.RegisterInfoStatus) //确认注册
                        {
                            CPortVerification.GetAvailablePort(1);
                            int s32RetCode = addDevConfigClient(IPAddress.Parse(form_ModifDeviceConfig.m_strIP).GetHashCode(), 1, 5555, 5555);
                            if (0 == s32RetCode)
                            {
                                int[] IpAddr = new int[1];
                                int[] Port = new int[1];
                                int[] Result = new int[1];
                                int[] Protocol = new int[1];
                                int[] ServicePort = new int[1];
                                int[] SendPort = new int[1];
                                string strSend = Global.CGlobalCmd.STR_CMD_INQUIRE + ";" + Global.CGlobalCmd.STR_CMD_GET_MAC + "\n";
                                byte[] WriteBuff = Encoding.Default.GetBytes(strSend);
                                IpAddr[0] = IPAddress.Parse(form_ModifDeviceConfig.m_strIP).GetHashCode();
                                Port[0] = 5555;
                                Protocol[0] = form_ModifDeviceConfig.m_Communication;
                                ServicePort[0] = CPortVerification.PortValid[0];
                                SendPort[0] = CPortVerification.PortValid[2];
                                CCommomControl.addDevInfo2DataRecvService(IpAddr, SendPort, Protocol, ServicePort, 1, Result);
                                query4DeviceAll(IpAddr, Port, 1, WriteBuff, WriteBuff.Length, 2000, Result);
                                if (Result[0] == 0)
                                {
                                    byte[] recvBuff = new byte[1024];
                                    int s32ValidLen = getReadData4IP(IpAddr[0], Port[0], recvBuff, 0, 1024);
                                    string receive = Encoding.Default.GetString(recvBuff, 0, s32ValidLen);
                                    receive = receive.TrimEnd(Environment.NewLine.ToCharArray());
                                    string[] recv = receive.Split(';');
                                    /*将通过IP检索回来的信息写进Device_Manager列表中*/
                                    Module_Device module_Device = new Module_Device();//创建设备实例
                                    /* 端口赋值 */
                                    module_Device.CmdSourcePort = 5555;
                                    module_Device.CmdDestinationPort = 5555;
                                    module_Device.DataSourcePort = SendPort[0];
                                    module_Device.DataDestinationPort = ServicePort[0];
                                    module_Device.Protocol = form_ModifDeviceConfig.m_Communication;

                                    module_Device.IP = form_ModifDeviceConfig.m_strIP;//IP
                                    module_Device.Name = form_ModifDeviceConfig.m_strDevSubName; //设备别名
                                    module_Device.VirtualNumber = uCBtnDragDrop.m_strDevNumber; //虚拟编号
                                    module_Device.MAC = recv[1];//MAC
                                    module_Device.Model = recv[0].Split(',')[1];//型号
                                    module_Device.SN = recv[0].Split(',')[2];//SN
                                    module_Device.SoftVersion = recv[0].Split(',')[3];//固件版本
                                    module_Device.Status = true;
                                    module_Device.HashCode = IPAddress.Parse(form_ModifDeviceConfig.m_strIP).GetHashCode();
                                    /*绑定固定Tag*/
                                    module_Device.GetChannel(1).Tag = "Tag" + ((int.Parse(uCBtnDragDrop.m_strDevNumber) - 1) * 4 + 1).ToString().PadLeft(3, '0');
                                    module_Device.GetChannel(2).Tag = "Tag" + ((int.Parse(uCBtnDragDrop.m_strDevNumber) - 1) * 4 + 2).ToString().PadLeft(3, '0');
                                    module_Device.GetChannel(3).Tag = "Tag" + ((int.Parse(uCBtnDragDrop.m_strDevNumber) - 1) * 4 + 3).ToString().PadLeft(3, '0');
                                    module_Device.GetChannel(4).Tag = "Tag" + ((int.Parse(uCBtnDragDrop.m_strDevNumber) - 1) * 4 + 4).ToString().PadLeft(3, '0');

                                    Module_DeviceManage.Instance.Devices.Add(module_Device);
                                    uCBtnDragDrop.m_strDevIP = form_ModifDeviceConfig.m_strIP;  //将添加仪器面板上有效的IP相关的设备信息添加到DeviceManager中

                                    /*从DeviceManager取需要显示的值*/
                                    uCBtnDragDrop.m_strDevIP = form_ModifDeviceConfig.m_strIP; //IP
                                    uCBtnDragDrop.m_strDevSubName = form_ModifDeviceConfig.m_strDevSubName;//设备别名             
                                    uCBtnDragDrop.m_bLink = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).Status;//状态                                           
                                    uCBtnDragDrop.m_strDevModel = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).Model;//型号
                                    uCBtnDragDrop.m_strFirmVersion = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).SoftVersion;//固件版本
                                    uCBtnDragDrop.m_strRackNumber = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).SN; //序列号（SN）
                                    uCBtnDragDrop.m_strDevSN = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).SN;//序列号SN
                                    uCBtnDragDrop.m_strMac = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).MAC;//Mac
                                    uCBtnDragDrop.m_bStatus = true;
                                    uCBtnDragDrop.DevInfo();//刷新控件显示
                                    showControl();
                                    gvValue(uCBtnDragDrop);
                                    /*绑定初始的通道显示信息*/
                                    for (int i = 1; i < 5; i++)
                                    {
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).ChannelID = i;//通道
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).Collect = true;//采集
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).Record = true;//记录
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).MeasureType = "MARX";//测量类型
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).Scale = "1";//范围
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).Offset = "0";//偏移
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).Impedance = "1M";//阻抗
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).Coupling = "DC";//耦合
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).ProbeRatio = "*1";//探头比
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).ChannelDelayTime = "0";//通道延时
                                    }
                                }
                                else //有效的IP是否已经在流式布局中，此分支：在布局中
                                {
                                    MessageBox.Show(InterpretBase.textTran("添加失败，当前仪器配置列表已存在该设备"), InterpretBase.textTran("添加情况"));
                                }
                            }
                            else if (-1 == s32RetCode)
                            {
                                MessageBox.Show(InterpretBase.textTran("添加失败，当前仪器配置列表已存在该设备"), InterpretBase.textTran("添加情况"));
                            }
                            else if (-2 == s32RetCode)
                            {
                                MessageBox.Show(InterpretBase.textTran("内存错误"), InterpretBase.textTran("添加情况"));
                            }
                            else if (-3 == s32RetCode)
                            {
                                MessageBox.Show(InterpretBase.textTran("通信I/O错误,请检查设备参数"), InterpretBase.textTran("添加情况"));
                            }
                            else if (-4 == s32RetCode)
                            {
                                MessageBox.Show(InterpretBase.textTran("查询超时,请确保是否为支持的设备型号列表"), InterpretBase.textTran("添加情况"));
                            }
                        }
                    }
                }
            }
            else
            {
                if (uCBtnDragDrop.m_strDevIP == "")//源控件IP为空，空位置
                {
                    bool IsIP = IsIPExist(uCSmallDev.m_strDevIP);
                    if (!IsIP)//IP不在此布局中
                    {
                        Module_Device module_Device = new Module_Device();//创建设备实例
                        Form_RegisterInfo form_RegisterInfo = new Form_RegisterInfo();
                        form_RegisterInfo.m_strIP = uCSmallDev.m_strDevIP;
                        form_RegisterInfo.m_strSN = uCSmallDev.m_strDevSN;
                        form_RegisterInfo.disAbleTextBox();
                        form_RegisterInfo.ShowDialog();
                        if (form_RegisterInfo.RegisterInfoStatus)
                        {
                            CPortVerification.GetAvailablePort(1);
                            int s32RetCode = addDevConfigClient(IPAddress.Parse(uCSmallDev.m_strDevIP).GetHashCode(), 1, 5555, 5555);
                            if (0 == s32RetCode)
                            {
                                int[] IpAddr = new int[1];
                                int[] Port = new int[1];
                                int[] Result = new int[1];
                                int[] Protocol = new int[1];
                                int[] ServicePort = new int[1];
                                int[] SendPort = new int[1];
                                string strSend = Global.CGlobalCmd.STR_CMD_INQUIRE + ";" + Global.CGlobalCmd.STR_CMD_GET_MAC + "\n";
                                byte[] WriteBuff = Encoding.Default.GetBytes(strSend);
                                IpAddr[0] = IPAddress.Parse(uCSmallDev.m_strDevIP).GetHashCode();
                                Port[0] = 5555;
                                Protocol[0] = form_RegisterInfo.m_communication;
                                ServicePort[0] = CPortVerification.PortValid[3];
                                SendPort[0] = CPortVerification.PortValid[2];
                                CCommomControl.addDevInfo2DataRecvService(IpAddr, SendPort, Protocol, ServicePort, 1, Result);
                                query4DeviceAll(IpAddr, Port, 1, WriteBuff, WriteBuff.Length, 2000, Result);
                                if (Result[0] == 0)
                                {
                                    byte[] recvBuff = new byte[1024];
                                    int s32ValidLen = getReadData4IP(IpAddr[0], Port[0], recvBuff, 0, 1024);
                                    string receive = Encoding.Default.GetString(recvBuff, 0, s32ValidLen);
                                    receive = receive.TrimEnd(Environment.NewLine.ToCharArray());
                                    string[] recv = receive.Split(';');
                                    uCBtnDragDrop.m_strDevIP = uCSmallDev.m_strDevIP;  //将源控件的Ip给拖放完成的IP
                                    uCBtnDragDrop.m_bLink = true;//在线状态
                                    uCBtnDragDrop.m_bStatus = true;
                                    uCBtnDragDrop.m_strDevSubName = form_RegisterInfo.m_strDevSubName;//设备别名
                                    uCBtnDragDrop.m_strDevModel = form_RegisterInfo.m_strModel;//型号
                                    uCBtnDragDrop.m_strFirmVersion = uCSmallDev.m_strFirmVersion;//固件版本 
                                    uCBtnDragDrop.m_strMac = uCSmallDev.m_strMac;//Mac
                                    uCBtnDragDrop.m_strDevSN = uCSmallDev.m_strDevSN;//序列号SN                        
                                    uCBtnDragDrop.DevInfo();//刷新控件显示
                                    form_RegisterInfo.RegisterInfoStatus = false;
                                    /*改变拖放两个控件的背景颜色*/
                                    Panel_SetUnselect();
                                    bool pstate = true;
                                    uCBtnDragDrop.m_selectColor = Color.DodgerBlue;
                                    uCBtnDragDrop.panel_Set(pstate);
                                    showControl();
                                    gvValue(uCBtnDragDrop);
                                    /* 端口赋值 */
                                    module_Device.CmdSourcePort = 5555;
                                    module_Device.CmdDestinationPort = 5555;
                                    module_Device.DataSourcePort = SendPort[0];
                                    module_Device.DataDestinationPort = ServicePort[0];
                                    module_Device.Protocol = form_RegisterInfo.m_communication;

                                    //Module_DeviceManage.Instance.IP = uCSmallDev.m_strDevIP;
                                    module_Device.IP = uCSmallDev.m_strDevIP;//IP
                                    module_Device.Name = uCBtnDragDrop.m_strDevSubName; //设备别名
                                    module_Device.VirtualNumber = uCBtnDragDrop.m_strDevNumber; //虚拟编号                            
                                    module_Device.Model = uCBtnDragDrop.m_strDevModel;//型号
                                    module_Device.SN = uCBtnDragDrop.m_strDevSN;//SN
                                    module_Device.MAC = uCBtnDragDrop.m_strMac;//Mac
                                    module_Device.SoftVersion = uCBtnDragDrop.m_strFirmVersion;//固件版本
                                    module_Device.Status = true;
                                    module_Device.HashCode = IPAddress.Parse(uCSmallDev.m_strDevIP).GetHashCode();
                                    module_Device.GetChannel(1).Tag = "Tag" + ((int.Parse(uCBtnDragDrop.m_strDevNumber) - 1) * 4 + 1).ToString().PadLeft(3, '0');
                                    module_Device.GetChannel(2).Tag = "Tag" + ((int.Parse(uCBtnDragDrop.m_strDevNumber) - 1) * 4 + 2).ToString().PadLeft(3, '0');
                                    module_Device.GetChannel(3).Tag = "Tag" + ((int.Parse(uCBtnDragDrop.m_strDevNumber) - 1) * 4 + 3).ToString().PadLeft(3, '0');
                                    module_Device.GetChannel(4).Tag = "Tag" + ((int.Parse(uCBtnDragDrop.m_strDevNumber) - 1) * 4 + 4).ToString().PadLeft(3, '0');
                                    Module_DeviceManage.Instance.Devices.Add(module_Device);

                                    /*绑定初始的通道显示信息*/
                                    for (int i = 1; i < 5; i++)
                                    {
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).ChannelID = i;//通道
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).Collect = true;//采集
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).Record = true;//记录
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).MeasureType = "MARX";//测量类型
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).Scale = "1";//范围
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).Offset = "0";//偏移
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).Impedance = "1M";//阻抗
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).Coupling = "DC";//耦合
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).ProbeRatio = "*1";//探头比
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCBtnDragDrop.m_strDevIP).GetChannel(i).ChannelDelayTime = "0";//通道延时
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(InterpretBase.textTran("添加失败，当前仪器配置列表已存在该设备"), InterpretBase.textTran("添加情况"));
                    }
                }
                else
                {
                    MessageBox.Show(InterpretBase.textTran("添加失败，当前位置已存在设备"), InterpretBase.textTran("添加情况"));
                }
            }
        }
        #endregion

        #region 事件：订阅自定义事件
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="evenSoure"></param>
        public void Subscribe(UCUserBaseNullDeviceInfo evenSource) //绑定左侧
        {
            evenSource.clickUerEvent += EvenSource_clickUerEvent; //回调方法
            evenSource.clickUerEvent += EvenSource_clickUerEvent1;
        }



        public void RightSubscribe(UCUserBaseNullDeviceInfo evenSource) //绑定右侧
        {
            evenSource.clickUerEvent += EvenSource_clickUerEventRightLay; //回调方法
        }
        public void OnlineSubscribe(UCSmallDev eventSource)
        {
            eventSource.clickUerEvent += EventSource_clickUCSDEvent;
        }
        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <param name="evenSoure"></param>
        public void UnSubscribe(UCUserBaseNullDeviceInfo evenSource) //取消左侧绑定
        {
            evenSource.clickUerEvent -= EvenSource_clickUerEvent;
        }
        public void UnRightSubscribe(UCUserBaseNullDeviceInfo evenSource)//取消右侧绑定
        {
            evenSource.clickUerEvent -= EvenSource_clickUerEventRightLay;
        }
        #endregion

        #region 回调方法：自定义控件被点击
        /// <summary>
        /// 自定义控件panel_in在flowLayoutPanel_DeviceConfig被被点击 响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EvenSource_clickUerEvent(object sender, UCUserBaseNullDeviceInfo.clickUerBaseNullEventArgs e)
        {
            string str = e.KeyToRaiseEvent; //获取当前点击的自定义控件的label_Number 
            UCUserBaseNullDeviceInfo uCUserBaseNullDeviceInfo = (UCUserBaseNullDeviceInfo)sender;
            if (str == "PictureBoxDouleClick")
            {
                if (uCUserBaseNullDeviceInfo.m_strDevIP == "" && uCUserBaseNullDeviceInfo.m_strDevNumber != null)
                {
                    Form_ModifDeviceConfig.DeviceName = "Device_" + uCUserBaseNullDeviceInfo.m_strDevNumber;
                    CReflection.callObjectEvent(this.btn_RegisterIncrument, "OnClick", e);
                }
                else if ((uCUserBaseNullDeviceInfo.m_strDevIP != "") && (!string.IsNullOrEmpty(uCUserBaseNullDeviceInfo.m_strDevIP)))
                {
                    Panel_SetUnselect();
                    showControl();
                    uCUserBaseNullDeviceInfo.m_selectColor = Color.DodgerBlue;
                    bool pstate = true;
                    uCUserBaseNullDeviceInfo.panel_Set(pstate);
                    gvValue(uCUserBaseNullDeviceInfo);
                }
            }
            else if (str == "panelInClick")
            {
                Panel_SetUnselect();
                showControl();
                uCUserBaseNullDeviceInfo.m_selectColor = Color.DodgerBlue;
                bool pstate = true;
                uCUserBaseNullDeviceInfo.panel_Set(pstate);
                gvValue(uCUserBaseNullDeviceInfo);
            }
        }

        public void Panel_SetUnselect()
        {
            foreach (UCUserBaseNullDeviceInfo item in flowLayoutPanel_DeviceConfig.Controls)
            {
                if (item.m_selectColor == Color.DodgerBlue)
                {
                    item.m_unselectColor = Color.FromArgb(166, 209, 245);
                    bool pstate = false;
                    item.panel_Set(pstate);
                }
            }
        }


        /// <summary>
        /// Double
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EvenSource_clickUerEvent1(object sender, UCUserBaseNullDeviceInfo.clickUerBaseNullEventArgs e)
        {
            string str = e.KeyToRaiseEvent;
            UCUserBaseNullDeviceInfo uCUserBaseNullDeviceInfo = (UCUserBaseNullDeviceInfo)sender;
            if (str == "ToolStripRegister")
            {
                Form_ModifDeviceConfig.DeviceName = "Device_" + uCUserBaseNullDeviceInfo.m_strDevNumber;
                // 反射到注册按钮事件上
                CReflection.callObjectEvent(btn_RegisterIncrument, "OnClick", e);
            }
            else if (str == "ToolStripModif")
            {
                Form_ModifDeviceConfig form_ModifDeviceConfig = new Form_ModifDeviceConfig();
                form_ModifDeviceConfig.ModifInfo(uCUserBaseNullDeviceInfo.m_strDevSN, uCUserBaseNullDeviceInfo.m_strDevIP,
                    uCUserBaseNullDeviceInfo.m_strDevSubName);
                if (!string.IsNullOrEmpty(uCUserBaseNullDeviceInfo.m_strDevSN))//不为空则show出修改窗体
                {
                    form_ModifDeviceConfig.ShowDialog();
                    if (form_ModifDeviceConfig.RegisterInfoStatus) //确认修改
                    {
                        if (uCUserBaseNullDeviceInfo.m_strDevIP == form_ModifDeviceConfig.m_strIP)
                        {
                            Module_DeviceManage.Instance.GetDeviceBySN(uCUserBaseNullDeviceInfo.m_strDevSN).Name = form_ModifDeviceConfig.m_strDevSubName; //设备别名
                            uCUserBaseNullDeviceInfo.m_strDevSubName = form_ModifDeviceConfig.m_strDevSubName;//设备别名    
                            uCUserBaseNullDeviceInfo.DevInfo();//刷新控件显示
                            showControl();
                            gvValue(uCUserBaseNullDeviceInfo);
                        }
                        else
                        {
                            CPortVerification.GetAvailablePort(1);
                            int[] IpAddr = new int[1];
                            int[] Port = new int[1];
                            int[] Result = new int[1];
                            int[] Protocol = new int[1];
                            int[] ServicePort = new int[1];
                            int[] SendPort = new int[1];
                            string strSend = Global.CGlobalCmd.STR_CMD_INQUIRE + ";" + Global.CGlobalCmd.STR_CMD_GET_MAC + "\n";
                            byte[] WriteBuff = Encoding.Default.GetBytes(strSend);
                            IpAddr[0] = IPAddress.Parse(form_ModifDeviceConfig.m_strIP).GetHashCode();
                            Port[0] = 5555;
                            Protocol[0] = form_ModifDeviceConfig.m_Communication;
                            ServicePort[0] = CPortVerification.PortValid[3];
                            SendPort[0] = CPortVerification.PortValid[2];
                            int s32RetCode = editDevConfigClient(IPAddress.Parse(uCUserBaseNullDeviceInfo.m_strDevIP).GetHashCode(), Port[0],
                            IpAddr[0], Port[0], ServicePort[0], Protocol[0]);
                            if (0 == s32RetCode)
                            {
                                //addDevInfo2DataRecvService(IpAddr, Port, Protocol, ServicePort, 1, Result);
                                query4DeviceAll(IpAddr, Port, 1, WriteBuff, WriteBuff.Length, 2000, Result);
                                if (Result[0] == 0)
                                {
                                    // 更新信息
                                    byte[] recvBuff = new byte[1024];
                                    int s32ValidLen = getReadData4IP(IpAddr[0], Port[0], recvBuff, 0, 1024);
                                    string receive = Encoding.Default.GetString(recvBuff, 0, s32ValidLen);
                                    receive = receive.TrimEnd(Environment.NewLine.ToCharArray());
                                    string[] recv = receive.Split(';');
                                    /*将通过IP检索回来的信息写进Device_Manager列表中*/
                                    Module_DeviceManage.Instance.GetDeviceBySN(uCUserBaseNullDeviceInfo.m_strDevSN).IP = form_ModifDeviceConfig.m_strIP;
                                    Module_DeviceManage.Instance.GetDeviceBySN(uCUserBaseNullDeviceInfo.m_strDevSN).Name = form_ModifDeviceConfig.m_strDevSubName; //设备别名
                                    Module_DeviceManage.Instance.GetDeviceBySN(uCUserBaseNullDeviceInfo.m_strDevSN).VirtualNumber = uCUserBaseNullDeviceInfo.m_strDevNumber; //虚拟编号
                                    Module_DeviceManage.Instance.GetDeviceBySN(uCUserBaseNullDeviceInfo.m_strDevSN).MAC = recv[1];//MAC
                                    Module_DeviceManage.Instance.GetDeviceBySN(uCUserBaseNullDeviceInfo.m_strDevSN).Model = recv[0].Split(',')[1];
                                    Module_DeviceManage.Instance.GetDeviceBySN(uCUserBaseNullDeviceInfo.m_strDevSN).SoftVersion = recv[0].Split(',')[3];
                                    Module_DeviceManage.Instance.GetDeviceBySN(uCUserBaseNullDeviceInfo.m_strDevSN).Status = true;
                                    Module_DeviceManage.Instance.GetDeviceBySN(uCUserBaseNullDeviceInfo.m_strDevSN).CmdSourcePort = 5555;
                                    Module_DeviceManage.Instance.GetDeviceBySN(uCUserBaseNullDeviceInfo.m_strDevSN).CmdDestinationPort = 5555;
                                    Module_DeviceManage.Instance.GetDeviceBySN(uCUserBaseNullDeviceInfo.m_strDevSN).DataSourcePort = CPortVerification.PortValid[2];
                                    Module_DeviceManage.Instance.GetDeviceBySN(uCUserBaseNullDeviceInfo.m_strDevSN).DataDestinationPort = CPortVerification.PortValid[3];
                                    Module_DeviceManage.Instance.GetDeviceBySN(uCUserBaseNullDeviceInfo.m_strDevSN).Protocol = form_ModifDeviceConfig.m_Communication;
                                    /*从DeviceManager取需要显示的值*/
                                    uCUserBaseNullDeviceInfo.m_strDevIP = form_ModifDeviceConfig.m_strIP; //IP
                                    uCUserBaseNullDeviceInfo.m_strDevSubName = form_ModifDeviceConfig.m_strDevSubName;//设备别名             
                                    uCUserBaseNullDeviceInfo.m_bStatus = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).Status;//状态                                           
                                    uCUserBaseNullDeviceInfo.m_strDevModel = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).Model;//型号
                                    uCUserBaseNullDeviceInfo.m_strFirmVersion = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).SoftVersion;//固件版本
                                    uCUserBaseNullDeviceInfo.m_strRackNumber = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).SN; //序列号（SN）
                                    uCUserBaseNullDeviceInfo.m_strDevSN = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).SN;//序列号SN*/
                                    uCUserBaseNullDeviceInfo.m_bLink = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).Status;
                                    uCUserBaseNullDeviceInfo.DevInfo();//刷新控件显示
                                    showControl();
                                    gvValue(uCUserBaseNullDeviceInfo);
                                    /*绑定初始的通道显示信息*/
                                    for (int i = 1; i < 5; i++)
                                    {
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCUserBaseNullDeviceInfo.m_strDevIP).GetChannel(i).ChannelID = i;//通道
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCUserBaseNullDeviceInfo.m_strDevIP).GetChannel(i).Collect = true;//采集
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCUserBaseNullDeviceInfo.m_strDevIP).GetChannel(i).Record = true;//记录
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCUserBaseNullDeviceInfo.m_strDevIP).GetChannel(i).MeasureType = "MARX";//测量类型
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCUserBaseNullDeviceInfo.m_strDevIP).GetChannel(i).Scale = "1";//范围
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCUserBaseNullDeviceInfo.m_strDevIP).GetChannel(i).Offset = "0";//偏移
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCUserBaseNullDeviceInfo.m_strDevIP).GetChannel(i).Impedance = "1M";//阻抗
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCUserBaseNullDeviceInfo.m_strDevIP).GetChannel(i).Coupling = "DC";//耦合
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCUserBaseNullDeviceInfo.m_strDevIP).GetChannel(i).ProbeRatio = "*1";//探头比
                                        Module_DeviceManage.Instance.GetDeviceByIP(uCUserBaseNullDeviceInfo.m_strDevIP).GetChannel(i).ChannelDelayTime = "0";//通道延时
                                    }
                                }

                            }
                            else if (-1 == s32RetCode)
                            {
                                MessageBox.Show(InterpretBase.textTran("添加失败，当前仪器配置列表已存在该设备"), InterpretBase.textTran("添加情况"));
                            }
                            else if (-2 == s32RetCode)
                            {
                                MessageBox.Show(InterpretBase.textTran("内存错误"), InterpretBase.textTran("添加情况"));
                            }
                            else if (-3 == s32RetCode)
                            {
                                MessageBox.Show(InterpretBase.textTran("通信I/O错误,请检查设备参数"), InterpretBase.textTran("添加情况"));
                            }
                            else if (-4 == s32RetCode)
                            {
                                MessageBox.Show(InterpretBase.textTran("查询超时,请确保是否为支持的设备型号列表"), InterpretBase.textTran("添加情况"));
                            }
                        }

                    }
                }
            }
            else if(str == "ToolStripDelDevice")
            {
                Panel_SetUnselect();
                showControl();
                uCUserBaseNullDeviceInfo.m_selectColor = Color.DodgerBlue;
                bool pstate = true;
                uCUserBaseNullDeviceInfo.panel_Set(pstate);
                gvValue(uCUserBaseNullDeviceInfo);
                foreach (UCUserBaseNullDeviceInfo item in this.flowLayoutPanel_DeviceConfig.Controls)
                {
                    item.m_isSelect = false;
                }
                uCUserBaseNullDeviceInfo.m_isSelect = true;
                // 反射到注册按钮事件上
                CReflection.callObjectEvent(this.btn_DeletDev, "OnClick", e);
            }
        }

        /// <summary>
        /// 自定义事件：注册流布局控件被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EvenSource_clickUerEventRightLay(object sender, UCUserBaseNullDeviceInfo.clickUerBaseNullEventArgs e)
        {
            string str = e.KeyToRaiseEvent; //获取当前点击的自定义控件的label_Number:IP 
            showControl();
            foreach (UCUserBaseNullDeviceInfo item in flowLayoutPanel_RegDevList.Controls)
            {
                if (item.m_strDevIP == str.Substring(1, (str.Length - 1))) //遍历右侧流控件中的与产生自定义事件的自定义用户控件IP，相同则进入自定义控件的方法中执行颜色变更操作
                {
                    item.m_selectColor = Color.DodgerBlue; //设置选中颜色
                    bool pstate = true;
                    item.panel_Set(pstate);//主动更改控件背景颜色
                    gvValue(item);//从自定义控件中获取相关信息  两个方法重载

                }
                else  //不同则还原未未选中状态的颜色
                {
                    item.m_unselectColor = Color.FromArgb(166, 209, 245);
                    bool pstate = false;
                    item.panel_Set(pstate);
                }
            }
        }
        /// <summary>
        /// 自定义事件：Online流布局控件被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventSource_clickUCSDEvent(object sender, UCSmallDev.clickUerBaseNullEventArgs e)
        {
            string str = e.KeyToRaiseEvent;  //获取点击的IP
            showControl();
            foreach (UCSmallDev item in flowLayoutPanel_OnlineList.Controls)
            {
                if (item.m_strDevIP == str) //遍历在线流控件中的与产生自定义事件的自定义用户控件IP，相同则进入自定义控件的方法中执行颜色变更操作
                {
                    bool bSelectSmall = true;
                    item.SetPanel(bSelectSmall);//主动改变控件背景颜色
                    gvValue(item);//从自定义控件中获取相关信息  两个方法重载

                }
                else  //不同则还原未选中状态的颜色
                {
                    bool bSelectSmall = false;
                    item.SetPanel(bSelectSmall);
                }
            }
        }
        #endregion

        #region 自定义事件
        /*定义事件参数类*/
        public class clickDeviceFormEventArgs : EventArgs
        {
            public readonly string KeyToRaiseEvent;

            public clickDeviceFormEventArgs(string keyToRaiseEvent)
            {
                KeyToRaiseEvent = keyToRaiseEvent;
            }
        }
        /*定义委托声明*/
        public delegate void clickDeviceFormEventHandler(object sender, clickDeviceFormEventArgs e);

        //用event关键字声明事件对象
        public event clickDeviceFormEventHandler clickDeviceFormEvent;

        //事件触发方法
        protected virtual void onClickDeviceFormEvent(clickDeviceFormEventArgs e)
        {
            if (clickDeviceFormEvent != null)
            {
                clickDeviceFormEvent(this, e);
            }
        }

        //引发事件
        private void RaiseEvent(string keyToRaiseEvent)
        {
            clickDeviceFormEventArgs e = new clickDeviceFormEventArgs(keyToRaiseEvent);

            onClickDeviceFormEvent(e);
        }

        #endregion

        #region 事件：删除仪器列表选中设备
        /// <summary>
        /// 事件：删除仪器列表选中设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_DeletDev_Click(object sender, EventArgs e)
        {
            foreach (UCUserBaseNullDeviceInfo item in this.flowLayoutPanel_DeviceConfig.Controls)
            {
                if (item.m_isSelect)
                {
                    if (item.m_strDevIP != "")
                    {
                        Form_DeletDeviceConfigInfo form_DeletDeviceConfigInfo = new Form_DeletDeviceConfigInfo();
                        form_DeletDeviceConfigInfo.ShowDialog();
                        if (Form_DeletDeviceConfigInfo.DeletStatus)
                        {                       
                            var device = Module_DeviceManage.Instance.GetDeviceByIP(item.m_strDevIP);
                            int[] deviceArray = new int[1];
                            deviceArray[0] = IPAddress.Parse(item.m_strDevIP).GetHashCode();
                            deleteDevNode4DataRecvService(deviceArray, 1);
                            if (Module_DeviceManage.Instance.GetDeviceByIP(item.m_strDevIP).CmdSocket != null)
                            {
                                Module_DeviceManage.Instance.GetDeviceByIP(item.m_strDevIP).CmdSocket.Close();
                            }
                            Module_ViewConfig.Instance.Remove((Module_DeviceManage.Instance.GetDeviceByIP(item.m_strDevIP)));//从显示配置单例移除相关设备
                            Module_DeviceManage.Instance.Devices.Remove(Module_DeviceManage.Instance.GetDeviceByIP(item.m_strDevIP));//从List中移除相关设备                          
                            
                            delDevConfigClient(IPAddress.Parse(item.m_strDevIP).GetHashCode(), 5555);
                            item.clearControl();//清楚自定义控件的显示信息
                            item.clearPropfull();//清楚自定义控件的属性
                            gvValue(item);//更新Button仪器显示
                                          //UnSubscribe(item);取消订阅                           
                        }
                    }
                }
            }
            // 通知主窗体删除了仪器，更新ViewConfig窗体
            RaiseEvent("DeviceDel");

        }
        #endregion

        #region 事件：删除注册列表中选中设备
        /// <summary>
        /// 事件：删除注册列表中选中设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delet_Click(object sender, EventArgs e)
        {
            Form_DeletDeviceConfigInfo form_DeletDeviceConfigInfo = new Form_DeletDeviceConfigInfo();
            form_DeletDeviceConfigInfo.ShowDialog();
            if (Form_DeletDeviceConfigInfo.DeletStatus)
            {
                foreach (UCUserBaseNullDeviceInfo item in this.flowLayoutPanel_RegDevList.Controls)
                {
                    if (item.m_isSelect)
                    {
                        flowLayoutPanel_RegDevList.Controls.Remove(item);
                        UnRightSubscribe(item);//取消订阅
                    }
                }
            }
        }
        #endregion

        #region 事件：注册仪器
        /// <summary>
        /// 事件：注册仪器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RegisterIncrument_Click(object sender, EventArgs e)
        {
            Form_ModifDeviceConfig form_ModifDeviceConfig = new Form_ModifDeviceConfig();
            form_ModifDeviceConfig.RefreshDeviceName();
            form_ModifDeviceConfig.ShowDialog();
            if (form_ModifDeviceConfig.RegisterInfoStatus) //确认注册
            {
                CPortVerification.GetAvailablePort(1);
                int s32RetCode = addDevConfigClient(IPAddress.Parse(form_ModifDeviceConfig.m_strIP).GetHashCode(), 1, 5555, 5555);
                if (0 == s32RetCode)
                {
                    int[] IpAddr = new int[1];
                    int[] Port = new int[1];
                    int[] Result = new int[1];
                    int[] Protocol = new int[1];
                    int[] ServicePort = new int[1];
                    int[] SendPort = new int[1];
                    
                    string strSend = Global.CGlobalCmd.STR_CMD_INQUIRE + ";" + Global.CGlobalCmd.STR_CMD_GET_MAC + "\n";
                    byte[] WriteBuff = Encoding.Default.GetBytes(strSend);
                    IpAddr[0] = IPAddress.Parse(form_ModifDeviceConfig.m_strIP).GetHashCode();
                    Port[0] = 5555;
                    Protocol[0] = form_ModifDeviceConfig.m_Communication;                   
                    ServicePort[0] = CPortVerification.PortValid[3];
                    SendPort[0] = CPortVerification.PortValid[2];
                    CCommomControl.addDevInfo2DataRecvService(IpAddr, SendPort, Protocol, ServicePort, 1, Result);
                    query4DeviceAll(IpAddr, Port, 1, WriteBuff, WriteBuff.Length, 2000, Result);
                    if (Result[0] == 0)
                    {
                        byte[] recvBuff = new byte[1024];
                        int s32ValidLen = getReadData4IP(IpAddr[0], Port[0], recvBuff, 0, 1024);
                        string receive = Encoding.Default.GetString(recvBuff, 0, s32ValidLen);
                        receive = receive.TrimEnd(Environment.NewLine.ToCharArray());
                        string[] recv = receive.Split(';');

                        /*将通过IP检索回来的信息写进Device_Manager列表中*/
                        Module_Device module_Device = new Module_Device();//创建设备实例  
                        /* 端口赋值 */
                        module_Device.CmdSourcePort = 5555;
                        module_Device.CmdDestinationPort = 5555;
                        module_Device.DataSourcePort = SendPort[0];
                        module_Device.DataDestinationPort = ServicePort[0];
                        module_Device.Protocol = form_ModifDeviceConfig.m_Communication;
                        module_Device.IP = form_ModifDeviceConfig.m_strIP;
                        module_Device.Name = form_ModifDeviceConfig.m_strDevSubName; //设备别名
                                                                                     //module_Device.VirtualNumber = uCUserBaseNullDeviceInfo.m_strDevNumber; //虚拟编号
                        module_Device.MAC = recv[1];//MAC
                        module_Device.Model = recv[0].Split(',')[1];
                        module_Device.SN = recv[0].Split(',')[2];
                        module_Device.SoftVersion = recv[0].Split(',')[3];
                        module_Device.Status = true;
                        module_Device.HashCode = IPAddress.Parse(form_ModifDeviceConfig.m_strIP).GetHashCode();

                        /* 检索流列表中IP为空的位置 */
                        foreach (UCUserBaseNullDeviceInfo item in this.flowLayoutPanel_DeviceConfig.Controls)
                        {
                            if (string.IsNullOrEmpty(item.m_strDevIP))
                            {
                                module_Device.VirtualNumber = item.m_strDevNumber;
                                module_Device.GetChannel(1).Tag = "Tag" + ((int.Parse(item.m_strDevNumber) - 1) * 4 + 1).ToString().PadLeft(3, '0');
                                module_Device.GetChannel(2).Tag = "Tag" + ((int.Parse(item.m_strDevNumber) - 1) * 4 + 2).ToString().PadLeft(3, '0');
                                module_Device.GetChannel(3).Tag = "Tag" + ((int.Parse(item.m_strDevNumber) - 1) * 4 + 3).ToString().PadLeft(3, '0');
                                module_Device.GetChannel(4).Tag = "Tag" + ((int.Parse(item.m_strDevNumber) - 1) * 4 + 4).ToString().PadLeft(3, '0');
                                Module_DeviceManage.Instance.Devices.Add(module_Device);

                                /*从DeviceManager取需要显示的值*/
                                item.m_strDevIP = form_ModifDeviceConfig.m_strIP; //IP
                                item.m_strDevSubName = form_ModifDeviceConfig.m_strDevSubName;//设备别名             
                                item.m_bStatus = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).Status;//状态                                           
                                item.m_strDevModel = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).Model;//型号
                                item.m_strFirmVersion = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).SoftVersion;//固件版本
                                item.m_strRackNumber = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).SN; //序列号（SN）
                                item.m_strDevSN = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).SN;//序列号SN*/
                                item.m_strMac = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).MAC;//Mac地址
                                item.m_bLink = Module_DeviceManage.Instance.GetDeviceByIP(form_ModifDeviceConfig.m_strIP).Status;
                                item.DevInfo();//刷新控件显示
                                showControl();
                                gvValue(item);
                                /*绑定初始的通道显示信息*/
                                for (int i = 1; i < 5; i++)
                                {
                                    Module_DeviceManage.Instance.GetDeviceByIP(item.m_strDevIP).GetChannel(i).ChannelID = i;//通道
                                    Module_DeviceManage.Instance.GetDeviceByIP(item.m_strDevIP).GetChannel(i).Collect = true;//采集
                                    Module_DeviceManage.Instance.GetDeviceByIP(item.m_strDevIP).GetChannel(i).Record = true;//记录
                                    Module_DeviceManage.Instance.GetDeviceByIP(item.m_strDevIP).GetChannel(i).MeasureType = "MARX";//测量类型
                                    Module_DeviceManage.Instance.GetDeviceByIP(item.m_strDevIP).GetChannel(i).Scale = "1";//范围
                                    Module_DeviceManage.Instance.GetDeviceByIP(item.m_strDevIP).GetChannel(i).Offset = "0";//偏移
                                    Module_DeviceManage.Instance.GetDeviceByIP(item.m_strDevIP).GetChannel(i).Impedance = "1M";//阻抗
                                    Module_DeviceManage.Instance.GetDeviceByIP(item.m_strDevIP).GetChannel(i).Coupling = "DC";//耦合
                                    Module_DeviceManage.Instance.GetDeviceByIP(item.m_strDevIP).GetChannel(i).ProbeRatio = "*1";//探头比
                                    Module_DeviceManage.Instance.GetDeviceByIP(item.m_strDevIP).GetChannel(i).ChannelDelayTime = "0";//通道延时
                                }
                                return;
                            }
                        }
                    }
                }
                else if (-1 == s32RetCode)
                {
                    MessageBox.Show(InterpretBase.textTran("添加失败，当前仪器配置列表已存在该设备"), InterpretBase.textTran("添加情况"));
                }
                else if (-2 == s32RetCode)
                {
                    MessageBox.Show(InterpretBase.textTran("内存错误"), InterpretBase.textTran("添加情况"));
                }
                else if (-3 == s32RetCode)
                {
                    MessageBox.Show(InterpretBase.textTran("通信I/O错误,请检查设备参数"), InterpretBase.textTran("添加情况"));
                }
                else if (-4 == s32RetCode)
                {
                    MessageBox.Show(InterpretBase.textTran("查询超时,请确保是否为支持的设备型号列表"), InterpretBase.textTran("添加情况"));
                }
            }
        }

        #endregion

        #region 事件：检索在线仪器
        /// <summary>
        /// 事件：检索在线仪器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Search_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;//等待                 
            flowLayoutPanel_OnlineList.Controls.Clear();
            Dictionary<string, string> onLineDevice = CFileOperate.Broadcast();
            Debug.WriteLine("广播到仪器:+" + onLineDevice.Count + "台");
            foreach (var item in onLineDevice)
            {
                Debug.WriteLine(item.Key + " " + item.Value);
            }
            if (onLineDevice.Count > 0)
            {
                for (int i = 0; i < onLineDevice.Count; i++)
                {
                    var item = onLineDevice.ElementAt(i);            
                    //&& !item.Value.Contains("172.18.8.27")
                    if (item.Key.Contains("DS8R") )
                    {
                        /*发送IP,获取信息*/
                        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);
                        string strSend = Global.CGlobalCmd.STR_CMD_INQUIRE + ";" + Global.CGlobalCmd.STR_CMD_GET_MAC + "\n";
                        Dictionary<string, string> deviceInfo = new Dictionary<string, string>();
                        deviceInfo.Add("errorInfo", "");
                        //序列号，MAC地址，软件版本，产品型号   
                        try
                        {
                            if (!socket.Connected)
                            {
                                socket.Connect(item.Value.Split(':')[0], 5555);
                            }
                            socket.SendTimeout = 1000;
                            int n = socket.Send(Encoding.Default.GetBytes(strSend));
                            byte[] byteReadBuf = new byte[256];
                            int ret = socket.Receive(byteReadBuf);
                            string receive = Encoding.Default.GetString(byteReadBuf, 0, ret);
                            strSend = strSend.TrimEnd(Environment.NewLine.ToCharArray());
                            string[] send = strSend.Split(';');
                            receive = receive.TrimEnd(Environment.NewLine.ToCharArray());
                            string[] recv = receive.Split(';');
                            if (recv.Length != send.Length)
                            {
                                deviceInfo["errorInfo"] = "发送命令条数和返回结果条数不匹配，存在无效命令";
                            }
                            for (int j = 0; j < send.Length; j++)
                            {
                                deviceInfo.Add(send[j], recv[j]);
                            }
                            socket.Close();
                        }
                        catch (SocketException ex)
                        {
                            deviceInfo["errorInfo"] = ex.Message;
                            continue;
                        }
                        if (!string.IsNullOrEmpty(deviceInfo["errorInfo"].ToString()))  //判断IP是否有效
                        {
                            //MessageBox.Show(deviceInfo["errorInfo"].ToString() + ":" + item.Value, InterpretBase.textTran("添加情况"))
                            Debug.WriteLine(deviceInfo["errorInfo"].ToString()+":"+item.Value);
                        }
                        else  //有效进入到此分支
                        {
                            string[] IP_Address = new string[10];
                            int ipcount = 0;
                            IPAddress[] ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                            //遍历获得的IP集以得到IPV4地址
                            foreach (IPAddress ip in ips)
                            {
                                //筛选出IPV4地址
                                if (ip.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    IP_Address[ipcount] = ip.ToString();
                                    ipcount++;
                                }
                            }
                            //刷新显示信息，并添加到在线仪器列表中
                            UCSmallDev uCSmallDevice = new UCSmallDev();
                            uCSmallDevice.AllowDrop = true;
                            uCSmallDevice.m_strDevSN = deviceInfo[CGlobalCmd.STR_CMD_INQUIRE].Split(',')[2].ToString();// SN
                            uCSmallDevice.m_strDevIP = item.Value.Split(':')[0];// IP
                            uCSmallDevice.m_strDevModel = deviceInfo[CGlobalCmd.STR_CMD_INQUIRE].Split(',')[1].ToString();// 型号
                            uCSmallDevice.m_strFirmVersion = deviceInfo[CGlobalCmd.STR_CMD_INQUIRE].Split(',')[3].ToString();// 版本号
                            uCSmallDevice.m_bStatus = true;
                            uCSmallDevice.SmallDevInfo();
                            OnlineSubscribe(uCSmallDevice);
                            flowLayoutPanel_OnlineList.Controls.Add(uCSmallDevice);
                            this.flowLayoutPanel_OnlineList.Refresh();
                            uCSmallDevice.MouseDown += UCSmallDevice_MouseDown;
                        }
                    }
                }
            }
            this.Cursor = Cursors.Default;//正常状态
            MessageBox.Show(InterpretBase.textTran("检索完成"));
        }
        /// <summary>
        /// 事件：在线仪器被MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCSmallDevice_MouseDown(object sender, MouseEventArgs e)
        {
            UCSmallDev usReg = (UCSmallDev)sender;
            usReg.DoDragDrop(usReg, DragDropEffects.Move);
        }
        #endregion

        #region 方法：显示底部控件
        /// <summary>
        /// 显示控件方法
        /// </summary>
        public void showControl()
        {
            this.pictureBox_Device.Visible = true;
            label_DevSN.Visible = true;
            label_DevModel.Visible = true;
            label_FirmVersion.Visible = true;
            label_DevNumber.Visible = true;
            label_DeviceSubName.Visible = true;
            label_DeviceIP.Visible = true;
        }
        #endregion

        #region 方法：当点击时显示设备信息_赋值,从自定义控件属性中获取主界面显示信息
        /// <summary>
        /// 从自定义控件中获取相关信息  两个方法重载
        /// </summary>
        /// <param name="userControlInfo"></param>
        public void gvValue(UCUserBaseNullDeviceInfo userControlInfo)
        {

            label_DevSN.Text = InterpretBase.textTran(CGlobalString.label_DevSN) + userControlInfo.m_strDevSN;
            label_DevModel.Text = InterpretBase.textTran(CGlobalString.label_DevModel) + userControlInfo.m_strDevModel;//仪器型号
            label_FirmVersion.Text = InterpretBase.textTran(CGlobalString.label_FirmVersion) + userControlInfo.m_strFirmVersion;
            label_DevNumber.Text = InterpretBase.textTran(CGlobalString.label_DevNumber) + userControlInfo.m_strDevNumber;
            label_DeviceSubName.Text = InterpretBase.textTran(CGlobalString.label_DeviceSubName) + userControlInfo.m_strDevSubName; //别名
            label_DeviceIP.Text = InterpretBase.textTran(CGlobalString.label_DeviceIP) + userControlInfo.m_strDevIP;
        }

        public void gvValue(UCSmallDev uCSmallDev)
        {

            label_DevSN.Text = InterpretBase.textTran(CGlobalString.label_DevSN) + uCSmallDev.m_strDevSN;
            label_DevModel.Text = InterpretBase.textTran(CGlobalString.label_DevModel) + uCSmallDev.m_strDevModel;
            label_FirmVersion.Text = InterpretBase.textTran(CGlobalString.label_FirmVersion) + uCSmallDev.m_strFirmVersion;
            label_DevNumber.Text = InterpretBase.textTran(CGlobalString.label_DevNumber) + uCSmallDev.m_strDevNumber;
            label_DeviceSubName.Text = InterpretBase.textTran(CGlobalString.label_DeviceSubName) + uCSmallDev.m_strDevSubName;
            label_DeviceIP.Text = InterpretBase.textTran(CGlobalString.label_DeviceIP) + uCSmallDev.m_strDevIP;
        }


        #endregion

        #region 方法：判断仪器列表流布局中是否有相同的IP
        /// <summary>
        /// 方法：判断仪器列表流布局中是否有相同的IP
        /// </summary>
        /// <param name="form_RegisterInfo"></param>
        /// <returns></returns>
        public bool IsIPExist(Form_RegisterInfo form_RegisterInfo)  //注册仪器流式布局IP是否存在的判定
        {
            bool bIsExist = false;

            foreach (UCUserBaseNullDeviceInfo item in flowLayoutPanel_RegDevList.Controls)
            {
                if (form_RegisterInfo.m_strIP == item.m_strDevIP) //如果设备IP相同则不允许添加
                {
                    bIsExist = true;
                    break;
                }
                else { bIsExist = false; }
            }
            return bIsExist;
        }

        public bool IsIPExist(Form_ModifDeviceConfig form_ModifDeviceConfig)
        {
            bool bIsExist = false;

            foreach (UCUserBaseNullDeviceInfo item in flowLayoutPanel_DeviceConfig.Controls)
            {
                if (form_ModifDeviceConfig.m_strIP == item.m_strDevIP) //如果设备IP不相同则允许添加
                {
                    bIsExist = true;
                    break;
                }
                else { bIsExist = false; }
            }
            return bIsExist;
        }

        public bool IsIPExist(string ip)
        {
            bool bIsExist = false;

            foreach (UCUserBaseNullDeviceInfo item in flowLayoutPanel_DeviceConfig.Controls)
            {
                if (ip == item.m_strDevIP) //如果设备IP不相同则允许添加
                {
                    bIsExist = true;
                    break;
                }
                else { bIsExist = false; }
            }
            return bIsExist;
        }

        public bool IsIPExist(int i)  //[华波]从信息库中取IP和所有在线仪器流式布局比对
        {
            bool bIsExist = false;
            foreach (UCSmallDev item in flowLayoutPanel_OnlineList.Controls)
            {
                if (Module_DeviceManage.Instance.Devices.ElementAt(i).Status)
                {
                    if (Module_DeviceManage.Instance.Devices.ElementAt(i).IP == item.m_strDevIP) //如果设备IP不相同则允许添加
                    {
                        bIsExist = true;
                        break;
                    }
                    else { bIsExist = false; }
                }
                else
                {
                    bIsExist = true;
                    break;
                }
            }
            return bIsExist;
        }

        #endregion

        #region 方法：设置光标样式为图片
        /// <summary>
        /// 设置光标样式为图片
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="point"></param>
        public void SetCursor(Bitmap cursor, Point hotPoint)
        {
            int hotX = hotPoint.X;
            int hotY = hotPoint.Y;
            Bitmap myNewCursor = new Bitmap(cursor.Width * 2 - hotX, cursor.Height * 2 - hotY);
            Graphics g = Graphics.FromImage(myNewCursor);
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g.DrawImage(cursor, cursor.Width - hotX, cursor.Height - hotY, cursor.Width,
            cursor.Height);
            this.Cursor = new Cursor(myNewCursor.GetHicon());

            g.Dispose();
            myNewCursor.Dispose();
        }
        #endregion

        #region 方法：删除所有信息，空白工程
        public void ClearShowInfo()
        {
            foreach (UCUserBaseNullDeviceInfo item in this.flowLayoutPanel_DeviceConfig.Controls)
            {
                Module_DeviceManage.Instance.Devices.Clear();//从List中移除相关设备
                item.clearControl();//清楚自定义控件的显示信息
                item.clearPropfull();//清楚自定义控件的属性
                gvValue(item);//更新Button仪器显示
                              //UnSubscribe(item);取消订阅
                              //清除掉显示配置的表            
            }
            // 显示配置单例清除
            Module.Module_ViewConfig.Instance.ViewConfig.Clear();//清除单例中的数据
        }
        #endregion

        #region 事件：改变窗体尺寸时底部panel尺寸变更
        /// <summary>
        /// 方法：改变尺寸时候底部panel尺寸变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_DeviceConfig_SizeChanged(object sender, EventArgs e)
        {
            this.panel_Buttom.Height = this.Height / 5;
            fAutoSize.formAutoSize(this.panel_Buttom);
        }
        #endregion
    }
}
