using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary_MultiLanguage;
using saker_Winform.CommonBaseModule;
using saker_Winform.Global;
using saker_Winform.Module;
using saker_Winform.UserControls;

namespace saker_Winform.SubForm
{
    public partial class Form_ChanConfig : Form
    {
        #region 属性
        /// <summary>
        /// 是否应用的属性
        /// </summary>
        private bool bConfig;
        public bool m_bConfig
        {
            get { return bConfig; }
            set { bConfig = value; }
        }

        private bool bIsFill = false;//是否填充  
        public string failConfigDevice = "";

        /// <summary>
        /// CellEdit为空  文本控件初始化
        /// </summary>
        public DataGridViewTextBoxEditingControl CellEdit = null;
        private delegate void dlgDisableBtn(bool disableBtn);//声明委托

        /* 颜色记录 */
        public List<string> colorsBack = new List<string>();

        /*波形线颜色*/
        public List<Color> lineColor = new List<Color> {
        ColorTranslator.FromHtml("#fefe00"),
        ColorTranslator.FromHtml("#01ffff"),
        ColorTranslator.FromHtml("#ff00ff"),
        ColorTranslator.FromHtml("#466fff"),
        ColorTranslator.FromHtml("#14878a"),
        ColorTranslator.FromHtml("#5dd85c"),
        ColorTranslator.FromHtml("#d86e00"),
        ColorTranslator.FromHtml("#854fc1"),
        ColorTranslator.FromHtml("#5492cf"),
        ColorTranslator.FromHtml("#71e3a7"),
        ColorTranslator.FromHtml("#ef897d"),
        ColorTranslator.FromHtml("#edd87b"),
        ColorTranslator.FromHtml("#d08dd4"),
        ColorTranslator.FromHtml("#f8b958"),
        ColorTranslator.FromHtml("#ff6452")};
        #endregion
        public Form_ChanConfig()
        {
            InitializeComponent();
            // 开启双缓冲          
            this.dataGridView_ChanSet.DoubleBufferedDataGirdView(true);
        }

        #region 委托
        private delegate bool IncreaseHandle(int value);
        private IncreaseHandle myIncrease = null;
        private delegate void refresh_PanelState(bool wait, bool recive, bool save);//声明委托RefreshPanelState 
        #endregion

        #region Load面板
        /// <summary>
        /// Load面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_ChanConfig_Load(object sender, EventArgs e)
        {
            /*去除窗体的标题栏*/
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;

            /*初始值*/
            this.comboBox_TriggeSource.SelectedIndex = 0; //默认选择0
            this.comboBox_TrigStyle.SelectedIndex = 0; //默认选择0
            this.comboBox_StorgeDepth.SelectedIndex = 3; //默认选择3
            this.comboBox_StorgeDepth.Enabled = false;
            this.comboBox_TrigStyle.Enabled = false;
            this.comboBox_HoriOffsetUnit.SelectedIndex = 0;//水平偏移默认单位为V
            this.comboBox_HorTimeUnit.SelectedIndex = 2;//水平时基默认单位为us
            this.comboBox_TriggerLevelUnit.SelectedIndex = 1; //触发电平默认单位为mV
            this.comboBox_TrigHoldUnit.SelectedIndex = 3;//触发释抑默认单位为ns
            this.textBox_HorTime.Text = "1";
            this.textBox_HoriOffset.Text = "0";
            this.textBox_TriggerLevel.Text = "500";
            this.textBox_TrigHold.Text = "8";

            #region 将主机IP赋值给textbox_IP
            /*将通过IP检索回来的信息写进Device_Manager列表中*/
            Module_Device module_Device = new Module_Device();//创建设备实例

            /* 过滤非真实网卡并且取IPV4 */
            List<string> listIP = new List<string>();
            ManagementClass mcNetworkAdapterConfig = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc_NetworkAdapterConfig = mcNetworkAdapterConfig.GetInstances();
            foreach (ManagementObject mo in moc_NetworkAdapterConfig)
            {
                string mServiceName = mo["ServiceName"] as string;

                //过滤非真实的网卡
                if (!(bool)mo["IPEnabled"])
                { continue; }
                if (mServiceName.ToLower().Contains("vmnetadapter")
                 || mServiceName.ToLower().Contains("ppoe")
                 || mServiceName.ToLower().Contains("bthpan")
                 || mServiceName.ToLower().Contains("tapvpn")
                 || mServiceName.ToLower().Contains("ndisip")
                 || mServiceName.ToLower().Contains("sinforvnic"))
                { continue; }

                string[] mIPAddress = mo["IPAddress"] as string[];
                if (mIPAddress != null)
                {

                    foreach (string ip in mIPAddress)
                    {
                        if (ip != "0.0.0.0")
                        {
                            listIP.Add(ip);
                        }
                    }
                }
                mo.Dispose();
            }

            foreach (var item in listIP)
            {
                if (!item.Contains(":"))
                { this.textBox_MainIP.Text = item.ToString(); }
            }
            /* //遍历获得的IP集以得到IPV4地址
             string[] IP_Address = new string[10];
             int ipcount = 0;
             IPAddress[] ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
             foreach (IPAddress ip in ips)
             {
                 //筛选出IPV4地址
                 if (ip.AddressFamily == AddressFamily.InterNetwork)
                 {
                     IP_Address[ipcount] = ip.ToString();
                     ipcount++;
                 }
             }
             this.textBox_MainIP.Text = IP_Address[0].ToString();*/
            #endregion


            this.comboBox_StorgeDepth.DropDownStyle = ComboBoxStyle.DropDownList; //禁止下拉框手动输入
            this.comboBox_TriggeSource.DropDownStyle = ComboBoxStyle.DropDownList; //禁止下拉框手动输入
            this.comboBox_TrigStyle.DropDownStyle = ComboBoxStyle.DropDownList;//禁止下拉框手动输入
            this.textBox_HorTime.KeyPress += TextBox_HorTime_KeyPress;  //水平时基KeyPress事件，限制输入数字
            this.textBox_TriggerLevel.KeyPress += TextBox_TriggerLevel_KeyPress; //触发电平KeyPress事件，限制输入数字
            this.textBox_TrigHold.KeyPress += TextBox_TrigHold_KeyPress; //水平偏移KeyPress事件，限制输入数字
            this.textBox_HoriOffset.KeyPress += TextBox_HoriOffset_KeyPress; //触发释抑KeyPress事件，限制输入数字

            //this.AutoSizeColumn(dataGridView_ChanSet);


            m_bConfig = false; //配置按钮为false
            this.dataGridView_ChanSet.AllowUserToAddRows = false;//禁止自动添加行


            #region DataGridView只读属性设置及绑定数据源
            /*只读属性设置*/
            dataGridView_ChanSet.Columns[3].ReadOnly = true;
            dataGridView_ChanSet.Columns[4].ReadOnly = true;
            //dataGridView_ChanSet.Columns[7].ReadOnly = true;
            this.comboBox_StorgeDepth.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox_TriggeSource.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox_TrigStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox_StorgeDepth.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox_HoriOffsetUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox_HorTimeUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox_TriggerLevelUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox_TrigHoldUnit.DropDownStyle = ComboBoxStyle.DropDownList;

            /*绑定DataGridView的数据源为DataTable*/
            //LoadChannelConfig();
            #endregion           

            // 从机模式
            if (Form_LogIn.m_vistorLog)
            {
                disableBtn(false);
            }

        }
        #endregion

        #region 方法：禁用窗体的应用按钮
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
                    this.button_Config.Enabled = true;
                }
                else
                {
                    this.button_Config.Enabled = false;
                }
            }          
        }
        #endregion

        #region 方法：绑定数据源
        /// <summary>
        /// 绑定数据源为DataTable
        /// </summary>
        public void LoadChannelConfig()
        {
            try
            {
                //dataGridView_ChanSet.DataSource = Module_DeviceManage.Instance.GetChanelConfig();
                //绑定数据源为DataTable
                colorsBack.Clear();
                if (dataGridView_ChanSet.Rows.Count > 0)
                {
                    for (int i = 0; i < dataGridView_ChanSet.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataGridView_ChanSet.Columns.Count; j++)
                        {
                            if(this.dataGridView_ChanSet.Rows[i].Cells[j].Style.BackColor == Color.Red)
                            {
                                colorsBack.Add(i.ToString() + "," + j.ToString());
                            }
                            
                        }
                    }
                }

                dataGridView_ChanSet.DataSource = Module_DeviceManage.Instance.GetChanelConfig();

                foreach (var item in colorsBack)
                {
                    var colorIndex = item.Split(',');
                    int i = int.Parse(colorIndex[0]);
                    int j = int.Parse(colorIndex[1]);
                    this.dataGridView_ChanSet.Rows[i].Cells[j].Style.BackColor = Color.Red;                  
                }
                this.dataGridView_ChanSet.Refresh();
                //DataTable dt = (dataGridView_ChanSet.DataSource as DataTable);             
                //if (dt == null)
                //{
                //    dataGridView_ChanSet.DataSource = Module_DeviceManage.Instance.GetChanelConfig();
                //}              
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        #endregion

        #region 事件：限制只能输入数字
        /// <summary>
        /// 水平偏移KeyPress事件，限制输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_HoriOffset_KeyPress(object sender, KeyPressEventArgs e)
        {
            LimitInput(e, true);
        }

        /// <summary>
        /// 触发释抑KeyPress事件，限制输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TrigHold_KeyPress(object sender, KeyPressEventArgs e)
        {
            LimitInput(e, false);
        }

        /// <summary>
        /// 触发电平KeyPress事件，限制输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TriggerLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            LimitInput(e, true);
        }

        /// <summary>
        /// 水平时基KeyPress事件，限制输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_HorTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            LimitInput(e, false);
        }
        #endregion

        #region 方法：限定只能输入数字
        /// <summary>
        /// 方法：限定只能输入数字
        /// </summary>
        /// <param name="e"></param>
        public void LimitInput(KeyPressEventArgs e, bool negative)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9')) e.Handled = true;
            if (negative)
            {
                if (e.KeyChar == '\b' || e.KeyChar == '-' || e.KeyChar == '.') e.Handled = false;
            }
            else
            {
                if (e.KeyChar == '\b' || e.KeyChar == '.') e.Handled = false;
            }

        }
        #endregion

        #region 自定义事件
        /*定义事件参数类*/
        public class clickChanFormEventArgs : EventArgs
        {
            public readonly string KeyToRaiseEvent;

            public clickChanFormEventArgs(string keyToRaiseEvent)
            {
                KeyToRaiseEvent = keyToRaiseEvent;
            }
        }
        /*定义委托声明*/
        public delegate void clickChanFormEventHandler(object sender, clickChanFormEventArgs e);

        //用event关键字声明事件对象
        public event clickChanFormEventHandler clickChanFormEvent;

        //事件触发方法 反射进入
        protected virtual void onClickChanFormEvent(clickChanFormEventArgs e)
        {

            if (clickChanFormEvent != null)
            {
                clickChanFormEvent(this, e);
            }
        }

        public void RefeshDelView()
        {
            DataTable dt = Module_DeviceManage.Instance.GetChanelConfig();  //创建DataTable的实例  
            //this.dataGridView_ChanSet.DataSource = dt;//删除仪器则重新绑定数据源为单例中
            DataRow[] dataRows_DeviceChannel = dt.Select("Collect=True");
            if (dataRows_DeviceChannel.Count() > 0)//DataRows转换为DataTable
            {
                DataTable dataTableValidChannel = dataRows_DeviceChannel.CopyToDataTable();
                DataTable dataTable_ViewConfig1 = dataTableValidChannel.DefaultView.ToTable(false, new string[] { "Tag" });
                DataTable dataTable_ViewConfig2 = dataTableValidChannel.DefaultView.ToTable(false, new string[] { "Tag" });
                DataTable dataTable_ViewConfig3 = dataTableValidChannel.DefaultView.ToTable(false, new string[] { "Tag" });
                DataTable dataTable_ViewConfig4 = dataTableValidChannel.DefaultView.ToTable(false, new string[] { "Tag" });

                /* 将有效Tag列生成整张DT */
                dataTable_ViewConfigProp(dataTable_ViewConfig1);
                dataTable_ViewConfigProp(dataTable_ViewConfig2);
                dataTable_ViewConfigProp(dataTable_ViewConfig3);
                dataTable_ViewConfigProp(dataTable_ViewConfig4);

                /*将数据写进单历中*/
                Module.Module_ViewConfig.Instance.ViewConfig.Clear();//清除单例中的数据
                Module.Module_ViewConfig.Instance.SetShowConfig("1", dataTable_ViewConfig1);
                Module.Module_ViewConfig.Instance.SetShowConfig("2", dataTable_ViewConfig2);
                Module.Module_ViewConfig.Instance.SetShowConfig("3", dataTable_ViewConfig3);
                Module.Module_ViewConfig.Instance.SetShowConfig("4", dataTable_ViewConfig4);
            }
            else
            {
                /*将数据写进单历中*/
                Module.Module_ViewConfig.Instance.ViewConfig.Clear();//清除单例中的数据
            }

        }


        /// <summary>
        /// 把dataBase的数据给到Instance的单例里面
        /// </summary>
        /// <param name="dataTable"></param>
        public void dataBaseChanDT(DataTable dataTable)
        {
            if (dataTable != null)
            {
                DataRow[] dataRows_DeviceChannel = dataTable.Select("Collect=True");
                if (dataRows_DeviceChannel.Count() > 0)//DataRows转换为DataTable
                {
                    DataTable dataTableValidChannel = dataRows_DeviceChannel.CopyToDataTable();
                    DataTable dataTable_ViewConfig1 = dataTableValidChannel.DefaultView.ToTable(false, new string[] { "Tag" });
                    DataTable dataTable_ViewConfig2 = dataTableValidChannel.DefaultView.ToTable(false, new string[] { "Tag" });
                    DataTable dataTable_ViewConfig3 = dataTableValidChannel.DefaultView.ToTable(false, new string[] { "Tag" });
                    DataTable dataTable_ViewConfig4 = dataTableValidChannel.DefaultView.ToTable(false, new string[] { "Tag" });

                    /* 将有效Tag列生成整张DT */
                    dataTable_ViewConfigProp(dataTable_ViewConfig1);
                    dataTable_ViewConfigProp(dataTable_ViewConfig2);
                    dataTable_ViewConfigProp(dataTable_ViewConfig3);
                    dataTable_ViewConfigProp(dataTable_ViewConfig4);

                    /*将数据写进单历中*/
                    Module.Module_ViewConfig.Instance.ViewConfig.Clear();//清除单例中的数据
                    Module.Module_ViewConfig.Instance.SetShowConfig("1", dataTable_ViewConfig1);
                    Module.Module_ViewConfig.Instance.SetShowConfig("2", dataTable_ViewConfig2);
                    Module.Module_ViewConfig.Instance.SetShowConfig("3", dataTable_ViewConfig3);
                    Module.Module_ViewConfig.Instance.SetShowConfig("4", dataTable_ViewConfig4);

                }
            }
        }

        #region 私有方法：给数据源添加列(将有效的Tag从通道配置的DataTable中获取
        /// <summary>
        /// 私有方法：给数据源添加列(将有效的Tag从通道配置的DataTable中获取
        /// </summary>
        /// <param name="dataTable"></param>
        private void dataTable_ViewConfigProp(DataTable dataTable)//初始Tag有效一列或未点击应用拿到生成的全表
        {
            if (dataTable.Columns.Count == 1)//初始一列Tag
            {
                dataTable.Columns.Add("ID");
                dataTable.Columns.Add("IsShow");
                dataTable.Columns.Add("No");
                dataTable.Columns.Add("Y");
                dataTable.Columns.Add("ScaleMin");
                dataTable.Columns.Add("ScaleMax");
                dataTable.Columns.Add("WaveColor");
                dataTable.Columns.Add("WaveType");
                dataTable.Columns["Tag"].SetOrdinal(2);

                if (dataTable.Rows.Count != 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        dataTable.Rows[i]["ID"] = i.ToString();
                        dataTable.Rows[i]["IsShow"] = true;
                        dataTable.Rows[i]["No"] = "W" + (i + 1).ToString();
                        dataTable.Rows[i]["Y"] = "Y" + (i + 1).ToString();
                        dataTable.Rows[i]["ScaleMin"] = "-10";
                        dataTable.Rows[i]["ScaleMax"] = "10";
                        dataTable.Rows[i]["WaveColor"] = Global.CGlobalColor.lineColor[i % (Global.CGlobalColor.lineColor.Count)].ToArgb();
                        dataTable.Rows[i]["WaveType"] = "趋势图";
                    }
                }
            }
        }

        #endregion

        //引发事件
        private void RaiseEvent(string keyToRaiseEvent)
        {
            clickChanFormEventArgs e = new clickChanFormEventArgs(keyToRaiseEvent);

            onClickChanFormEvent(e);
        }

        #endregion

        #region 事件：应用按钮点击
        /// <summary>
        /// 事件：配置按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Config_Click(object sender, EventArgs e)
        {
            Form_Progressbar form_Progressbar = new Form_Progressbar();

            /* 开启进度条 */
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            cancelTokenSource.Token.Register(() =>
            {
                form_Progressbar.CloseProcess();//委托方法调用进度条关闭                
                DialogResult res = MessageBox.Show(InterpretBase.textTran("通道配置已完成"), InterpretBase.textTran("配置情况"), MessageBoxButtons.OK);
                if (res == DialogResult.OK)
                {
                    RaiseEvent("ProcessClose");
                    return;
                }
            });
            Debug.WriteLine(DateTime.Now.ToString());
            m_bConfig = true;

            //this.dataGridView_ChanSet.CommitEdit(DataGridViewDataErrorContexts.Commit);
            this.dataGridView_ChanSet.EndEdit();//结束编辑
         
            bool IsEmpty = IsEmptyCheckout();
            bool IsValid = IsValidCheck();
            if(IsValid)
            {
                MessageBox.Show(InterpretBase.textTran("存在不合法参数，请检查！"), InterpretBase.textTran("警告"));
                return;
            }
            if (!IsEmpty)//不为空值
            {
                Task.Run(() =>
                {
                    form_Progressbar.ProcessMarquee(InterpretBase.textTran("通道配置中..."));//设置进度条显示为左右转动
                    form_Progressbar.StartPosition = FormStartPosition.CenterScreen;//程序中间
                    form_Progressbar.ShowDialog();
                    //form_Process.Show();
                }, cancelTokenSource.Token);

                //仪器设备信息             
                Dictionary<string, string> device_CommonSCPI = new Dictionary<string, string>();
                device_CommonSCPI.Add("TriggerSource", comboBox_TriggeSource.Text);//触发源
                device_CommonSCPI.Add("TriggerMode", comboBox_TrigStyle.Text);//触发方式
                device_CommonSCPI.Add("MemDepth", comboBox_StorgeDepth.Text);//存储深度
                device_CommonSCPI.Add("HoldOff", CUnitTransform.UnitTransF(comboBox_TrigHoldUnit, textBox_TrigHold)); //触发释抑
                device_CommonSCPI.Add("HorizontalTimebase", CUnitTransform.UnitTransF(comboBox_HorTimeUnit, textBox_HorTime));//水平时基
                device_CommonSCPI.Add("HorizontalOffset", CUnitTransform.UnitTransF(comboBox_HoriOffsetUnit, textBox_HoriOffset)); //水平偏移
                device_CommonSCPI.Add("TriggerLevel", CUnitTransform.UnitTransF(comboBox_TriggerLevelUnit, textBox_TriggerLevel));//触发电平  
                device_CommonSCPI.Add("IP", textBox_MainIP.Text);//本机IP

                dataGridView_ChanSet.Refresh();
                /* 更新单例 */
                DataTable dataTable_DeviceChannel = dataGridView_ChanSet.DataSource as DataTable;//将DataGridView转换为DataTable
                //if (dataTable_DeviceChannel != null)
                //{

                //    DataRow[] dataRows_DeviceChannel = dataTable_DeviceChannel.AsEnumerable().Where(item => item.ItemArray[1].ToString() == "True").ToArray();
                //    if (dataRows_DeviceChannel.Count() > 0)//DataRows转换为DataTable
                //    {
                //        DataTable dataTableValidChannel = dataRows_DeviceChannel.CopyToDataTable();
                //        DataTable dataTable_ViewConfig1 = dataTableValidChannel.DefaultView.ToTable(false, new string[] { "Tag" });
                //        DataTable dataTable_ViewConfig2 = dataTableValidChannel.DefaultView.ToTable(false, new string[] { "Tag" });
                //        DataTable dataTable_ViewConfig3 = dataTableValidChannel.DefaultView.ToTable(false, new string[] { "Tag" });
                //        DataTable dataTable_ViewConfig4 = dataTableValidChannel.DefaultView.ToTable(false, new string[] { "Tag" });

                //        /* 将有效Tag列生成整张DT */
                //        dataTable_ViewConfigProp(dataTable_ViewConfig1);
                //        dataTable_ViewConfigProp(dataTable_ViewConfig2);
                //        dataTable_ViewConfigProp(dataTable_ViewConfig3);
                //        dataTable_ViewConfigProp(dataTable_ViewConfig4);

                //        /*将数据写进单历中*/
                //        Module.Module_ViewConfig.Instance.ViewConfig.Clear();//清除单例中的数据
                //        Module.Module_ViewConfig.Instance.SetShowConfiUpdate("1", dataTable_ViewConfig1);
                //        Module.Module_ViewConfig.Instance.SetShowConfiUpdate("2", dataTable_ViewConfig2);
                //        Module.Module_ViewConfig.Instance.SetShowConfiUpdate("3", dataTable_ViewConfig3);
                //        Module.Module_ViewConfig.Instance.SetShowConfiUpdate("4", dataTable_ViewConfig4);

                //    }
                //    else
                //    {
                //        /*将数据写进单历中*/
                //        Module.Module_ViewConfig.Instance.ViewConfig.Clear();//清除单例中的数据
                //    }
                //}
                //初始化显示界面           
                if (Module_ViewConfig.Instance.IsInit == false)
                {
                    Module.Module_ViewConfig.Instance.ViewConfig.Clear();
                    Module_ViewConfig.Instance.Init(dataTable_DeviceChannel);
                }            
                /* 赋值给单例 */
                Module_DeviceManage.Instance.AssignDeviceAndChannel(device_CommonSCPI, dataTable_DeviceChannel);
                //建立长连接和仪器的命令长连接
                Parallel.For(0, Module_DeviceManage.Instance.Devices.Count, i =>
                {
                    if(Module_DeviceManage.Instance.Devices[i].Status == true)
                    {
                        if (Module_DeviceManage.Instance.Devices[i].CmdSocket == null)
                        {
                            Module_DeviceManage.Instance.Devices[i].CmdSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            if (!Module_DeviceManage.Instance.Devices[i].CmdSocket.Connected)
                            {
                                try
                                {
                                    Module_DeviceManage.Instance.Devices[i].CmdSocket.Connect(Module_DeviceManage.Instance.Devices[i].IP, 5555);
                                }
                                catch (Exception ex)
                                {
                                    Module_DeviceManage.Instance.Devices[i].ConfigSuccess = false;
                                    Module_DeviceManage.Instance.Devices[i].Status = false;
                                }

                            }
                        }
                    }
                    else
                    {
                        Module_DeviceManage.Instance.Devices[i].CmdSocket = null;
                        Module_DeviceManage.Instance.Devices[i].ConfigSuccess = false;
                    }
                    
                });
                Module_DeviceManage.Instance.SetDevices();
                Module_DeviceManage.Instance.MaxChannelModel = Module_DeviceManage.Instance.GetMaxChannelMode();
                cancelTokenSource.Cancel();
                Debug.WriteLine(DateTime.Now.ToString());
                #region 事件：回读参数比较
                /* 回读比对 */
                //参数回读校验
                if (Module_DeviceManage.Instance.GetChanelConfig() != null)
                {
                    failConfigDevice = "";
                    RefreshDataGridView();
                    CDataGridViewShow.dataTableCompare.Clear();
                    DataTable dataTableRead = Module_DeviceManage.Instance.GetChanelConfig();
                    CDataGridViewShow.CompareDataTable(dataTable_DeviceChannel, dataTableRead);
                    for (int i = 0; i < CDataGridViewShow.dataTableCompare.Count; i++)
                    {
                        int row = int.Parse(CDataGridViewShow.dataTableCompare[i].Split(',')[0]);
                        int column = int.Parse(CDataGridViewShow.dataTableCompare[i].Split(',')[1]);
                        this.dataGridView_ChanSet.Rows[row].Cells[column].Style.BackColor = Color.Red;
                    }
                    int count = 0;
                    foreach (var item in Module_DeviceManage.Instance.Devices)
                    {
                        if (false == item.ConfigSuccess)
                        {
                            if ((0 == count % 3) && (0 != count))
                            {
                                failConfigDevice += "\n";
                            }
                            failConfigDevice += item.Name + "(" + item.IP + ")" + "  ";
                            count++;
                        }
                    }
                    if ("" != failConfigDevice)
                    {
                        MessageBox.Show(InterpretBase.textTran("仪器的通用参数未设置成功：") + "\n" + failConfigDevice);
                    }
                }
                #endregion

            }
        }
        #endregion      

        #region 方法：使DataGridView单元格颜色恢复为白色
        public void RefreshDataGridView()
        {
            Debug.WriteLine("开始修改颜色" + DateTime.Now.Millisecond.ToString());
            if (dataGridView_ChanSet.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView_ChanSet.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView_ChanSet.Columns.Count; j++)
                    {
                        this.dataGridView_ChanSet.Rows[i].Cells[j].Style.BackColor = Color.White;
                    }
                }
            }
            Debug.WriteLine(DateTime.Now.Millisecond.ToString());
        }
        #endregion

        #region 方法：判断是否为空值，为空弹出对话框
        /// <summary>
        /// 判断是否为空值，为空弹出对话框
        /// </summary>
        /// <returns></returns>
        public bool IsEmptyCheckout()
        {
            bool IsEmpty = false;
            if (string.IsNullOrEmpty(textBox_MainIP.Text) || string.IsNullOrEmpty(textBox_TriggerLevel.Text) || string.IsNullOrEmpty(textBox_TrigHold.Text)
                || string.IsNullOrEmpty(textBox_HorTime.Text) || string.IsNullOrEmpty(textBox_HoriOffset.Text) ||
                (this.dataGridView_ChanSet.Rows.Count <= 0))
            {
                MessageBox.Show(InterpretBase.textTran("存在空值或无设备信息，请输入完成或注册仪器再点击应用"));
                return IsEmpty = true;
            }
            return IsEmpty;
        }

        /// <summary>
        /// 验证输入是否合法，尝试转换为Double
        /// </summary>
        /// <returns></returns>
        public bool IsValidCheck()
        {
            bool IsValid = false;
            double numberValid;
            if(!(double.TryParse(textBox_TriggerLevel.Text,out numberValid)&& double.TryParse(textBox_TrigHold.Text, out numberValid)
                && double.TryParse(textBox_HorTime.Text, out numberValid)&& double.TryParse(textBox_HoriOffset.Text, out numberValid)))
            {
                IsValid = true;
                return IsValid;
            }
            return IsValid;
        }
        #endregion

        #region 事件：DataGridView鼠标右键菜单全选/取消全选
        /// <summary>
        /// 事件：鼠标右键菜单全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllChooseToolStripMenuItem_Click(object sender, EventArgs e)
        {
           this.dataGridView_ChanSet.EndEdit();//结束编辑 
            CDataGridViewShow.AllChooseOrNot(dataGridView_ChanSet, true, 1);
            CDataGridViewShow.AllChooseOrNot(dataGridView_ChanSet, true, 2);
            //this.dataGridView_ChanSet.DataSource = CDataGridViewShow.DataTableAllChooseOrNot(dataGridView_ChanSet, true, 1, 2);
            //dataGridView_ChanSet.Refresh();
        }

        /// <summary>
        /// 鼠标右键菜单取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelChooseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dataGridView_ChanSet.EndEdit();//结束编辑
            CDataGridViewShow.AllChooseOrNot(dataGridView_ChanSet, false, 1);
            CDataGridViewShow.AllChooseOrNot(dataGridView_ChanSet, false, 2);
            //this.dataGridView_ChanSet.DataSource = CDataGridViewShow.DataTableAllChooseOrNot(dataGridView_ChanSet, false, 1, 2);
            //dataGridView_ChanSet.Refresh();
        }
        #endregion

        #region  事件：将当前单元格中类容被点击，实现同勾同选。
        /// <summary>
        /// 事件：将当前单元格中类容被点击，实现同勾同选。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_ChanSet_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                switch (e.ColumnIndex)
                {
                    case 1://第1列采集判断是否勾选，若勾选则将第2列记录勾选（取消同理）
                        if (Convert.ToBoolean(dataGridView_ChanSet.Rows[e.RowIndex].Cells[1].EditedFormattedValue))
                        {
                            dataGridView_ChanSet.Rows[e.RowIndex].Cells[2].Value = "True";
                            dataGridView_ChanSet.Rows[e.RowIndex].Cells[1].Value = "True";
                            dataGridView_ChanSet.Refresh();
                        }
                        else
                        {
                            dataGridView_ChanSet.Rows[e.RowIndex].Cells[1].Value = "False";
                            dataGridView_ChanSet.Rows[e.RowIndex].Cells[2].Value = "False";
                            dataGridView_ChanSet.Refresh();
                        }
                        break;
                    case 2://第2列采集判断是否勾选，若勾选则将第1列记录勾选（取消同理）
                        if (Convert.ToBoolean(dataGridView_ChanSet.Rows[e.RowIndex].Cells[2].EditedFormattedValue))
                        {
                            dataGridView_ChanSet.Rows[e.RowIndex].Cells[1].Value = "True";
                            dataGridView_ChanSet.Rows[e.RowIndex].Cells[2].Value = "True";
                            dataGridView_ChanSet.Refresh();
                        }
                        else
                        {
                            dataGridView_ChanSet.Rows[e.RowIndex].Cells[1].Value = "False";
                            dataGridView_ChanSet.Rows[e.RowIndex].Cells[2].Value = "False";
                            dataGridView_ChanSet.Refresh();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region 反射
        /// <summary>
        /// 通过panel被点击反射到应用按钮被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_ChanConfig_Click(object sender, EventArgs e)
        {
            CReflection.callObjectEvent(this.button_Config, "OnClick", e);//反射     
        }
        #endregion

        #region 方法：实现拉选复制
        /* 实现拉选复制 */
        private void dataGridView_ChanSet_MouseMove(object sender, MouseEventArgs e)
        {
            if (dataGridView_ChanSet.SelectedCells.Count == 1)
            {
                int rowIndex = dataGridView_ChanSet.SelectedCells[0].RowIndex;
                int columnIndex = dataGridView_ChanSet.SelectedCells[0].ColumnIndex;
                if (columnIndex > 5)
                {
                    //返回单元格相对于datagridview的Rectangle
                    Rectangle r = dataGridView_ChanSet.GetCellDisplayRectangle(columnIndex, rowIndex, false);
                    int x = r.X + r.Width;//单元格右下角相对于datagridview的坐标x
                    int y = r.Y + r.Height;//单元格右下角相对于datagridview的坐标y
                    if (Math.Abs(e.X - x) < r.Width / 10 && Math.Abs(e.Y - y) < r.Height / 5)
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Cross;
                        bIsFill = true;
                    }
                    else
                    {
                        bIsFill = false;
                    }
                }
            }
        }

        private void dataGridView_ChanSet_MouseUp(object sender, MouseEventArgs e)
        {
            if (bIsFill)
            {
                bIsFill = false;
                if (dataGridView_ChanSet.SelectedCells.Count > 1)
                {
                    int count = dataGridView_ChanSet.SelectedCells.Count;
                    for (int i = 0; i < (count - 1); i++)
                    {
                        if (dataGridView_ChanSet.SelectedCells[i].ColumnIndex != dataGridView_ChanSet.SelectedCells[i + 1].ColumnIndex)
                        {
                            return;
                        }
                    }
                    for (int i = 0; i < count - 1; i++)
                    {
                        dataGridView_ChanSet.SelectedCells[i].Value = dataGridView_ChanSet.SelectedCells[count - 1].Value;
                    }
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void dataGridView_ChanSet_MouseDown(object sender, MouseEventArgs e)
        {
            if (bIsFill)
            {
                this.Cursor = System.Windows.Forms.Cursors.Cross;
            }
        }

        private void dataGridView_ChanSet_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        #endregion

        #region 事件：双击改变值
        private void dataGridView_ChanSet_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView_ChanSet.Rows.Count > 0)
            {
                var clickColumn = e.ColumnIndex;
                var clickRow = e.RowIndex;
                if (clickRow < 0)
                {
                    switch (clickColumn)
                    {
                        case 8:
                            doubleClickDataGridView(this.dataGridView_ChanSet, 1, 8);
                            break;
                        case 9:
                            doubleClickDataGridView(this.dataGridView_ChanSet, 1, 9);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 方法：双击实现
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="j"></param>
        /// <param name="column"></param>
        public void doubleClickDataGridView(DataGridView dataGridView, int j, int column)
        {
            dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            if (column == 8)
            {
                int newInteger;
                if (int.TryParse(dataGridView[column, 0].Value.ToString(), out newInteger) && newInteger >= 0)
                {
                    for (int i = 0; i < dataGridView.Rows.Count; i++)
                    {
                        dataGridView[column, i].Value = dataGridView[column, 0].Value;
                    }
                }
            }
            else if (column == 9)
            {
                double newDouble;
                if (double.TryParse(dataGridView[column, 0].Value.ToString(), out newDouble))
                {
                    for (int i = 0; i < dataGridView.Rows.Count; i++)
                    {
                        dataGridView[column, i].Value = dataGridView[column, 0].Value;
                    }
                }
            }
            dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            dataGridView.Refresh();
        }
        #endregion

        #region 事件：限制DataGridView只能输入数字和-
        /// <summary>
        /// 限制单元格8,9列只能输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*        private void dataGridView_ChanSet_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
                {
                    if (CellEdit != null)
                    {
                        CellEdit.KeyPress -= CellEdit_KeyPress1; ; //绑定事件
                    }        
                    if (this.dataGridView_ChanSet.CurrentCellAddress.X == 8)//获取当前处于活动状态的单元格索引
                    {            
                        CellEdit = (DataGridViewTextBoxEditingControl)e.Control;
                        //CellEdit.SelectAll();
                        CellEdit.KeyPress += CellEdit_KeyPress1; ; //绑定事件
                    }
                }*/

        /*        private void CellEdit_KeyPress1(object sender, KeyPressEventArgs e)
                {
                    CString2Value.LimitInput(e);
                    if (e.KeyChar >= '0' && e.KeyChar <= '9')
                    {
                        var num = e.KeyChar;
                        this.dataGridView_ChanSet.CurrentCell.Value = e.KeyChar;
                        this.dataGridView_ChanSet.Refresh();
                        e.Handled = true;
                    }   
                    if(e.KeyChar == '-')
                    {
                        var num = e.KeyChar;
                        this.dataGridView_ChanSet.CurrentCell.Value = e.KeyChar;
                        this.dataGridView_ChanSet.Refresh();
                        e.Handled = true;
                    }
                    if (e.KeyChar == '\b') e.Handled = false;     
                }*/
        #endregion

        #region 事件：单元格校验
        private void dataGridView_ChanSet_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 8)
            {
                if (dataGridView_ChanSet.Rows[e.RowIndex].IsNewRow) { return; }
                double newInteger;
                if (!double.TryParse(e.FormattedValue.ToString(), out newInteger) || newInteger < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show(InterpretBase.textTran("此输入不合法，请重新输入"));
                }
            }
            else if (e.ColumnIndex == 9|| e.ColumnIndex == 15)
            {
                if (dataGridView_ChanSet.Rows[e.RowIndex].IsNewRow) { return; }
                double newDouble;
                if (!double.TryParse(e.FormattedValue.ToString(), out newDouble))
                {
                    e.Cancel = true;
                    MessageBox.Show(InterpretBase.textTran("此输入不合法，请重新输入"));
                }
            }

        }
        #endregion
    }
}
