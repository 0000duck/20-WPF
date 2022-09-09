using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using saker_Winform.SubForm;
using saker_Winform.UserControls;
using FontAwesome;
using System.Windows;
using saker_Winform.CommonBaseModule;
using ClassLibrary_SocketServer;
using System.Net;
using saker_Winform.Module;
using System.Collections;
using System.Xml.Linq;
using System.Xml;
using saker_Winform.DataBase;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.Sockets;
using System.Diagnostics;
using saker_Winform.Global;
using System.Resources;
using System.Reflection;
using System.Globalization;
using IPret = ClassLibrary_MultiLanguage.InterpretBase;//添加引用
using ClassLibrary_MultiLanguage;
using System.Windows.Media.Animation;
using Log.Core;
using System.Windows.Forms.VisualStyles;

namespace saker_Winform
{
    public partial class Form_Main : Form
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="adminLogin"></param>
        /// <param name="vistorLogin"></param>
        public Form_Main(bool adminLogin, bool vistorLogin)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            Form_Main.m_adminLogin = adminLogin;
            Form_Main.m_vistorLogin = vistorLogin;
        }

        #region 复写方法：双缓冲器打开
        /// <summary>
        /// 复写方法：双缓冲器打开
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        #endregion

        #region 私有字段：窗体实例
        /// <summary>
        /// 私有字段
        /// </summary>
        private Form_DeviceConfig form_DeviceConfig = new Form_DeviceConfig(); //创建仪器配置窗体的实例
        private Form_ChanConfig form_ChanConfig = new Form_ChanConfig(); //创建通道配置窗体的实例     
        private Form_CreateProject form_CreateProject = new Form_CreateProject();//创建创建工程窗体实例
        private Form_ViewConfig form_ViewConfig = new Form_ViewConfig(); //创建显示配置窗体
        private Form_RecordConfig form_RecordConfig = new Form_RecordConfig();//创建记录配置窗体
        private TabControl tabControl = new TabControl(); //TableControl实例创建
        private TabControl tabControlHistoryComp = new TabControl();//创建数据对比的Tab实例
        private List<Form_WaveMonitor> form_WaveMonitorsList = new List<Form_WaveMonitor>();
        private List<Form_HistoryComp> form_HistoryCompsList = new List<Form_HistoryComp>();
        private Form_WaveMonitor form_WaveMonitor1 = new Form_WaveMonitor("1", "01显示组"); //创建波形监测窗体的实例
        private Form_WaveMonitor form_WaveMonitor2 = new Form_WaveMonitor("2", "02显示组"); //创建波形监测窗体的实例
        private Form_WaveMonitor form_WaveMonitor3 = new Form_WaveMonitor("3", "03显示组"); //创建波形监测窗体的实例
        private Form_WaveMonitor form_WaveMonitor4 = new Form_WaveMonitor("4", "04显示组"); //创建波形监测窗体的实例
        private Form_HistoryComp form_HistoryComp1 = new Form_HistoryComp("1", "01显示组"); //创建波形数据对比的实例
        private Form_HistoryComp form_HistoryComp2 = new Form_HistoryComp("2", "02显示组"); //创建波形数据对比的实例
        private Form_HistoryComp form_HistoryComp3 = new Form_HistoryComp("3", "03显示组"); //创建波形数据对比的实例
        private Form_HistoryComp form_HistoryComp4 = new Form_HistoryComp("4", "04显示组"); //创建波形数据对比的实例
        private Form_StateMonitor form_StateMonitor = new Form_StateMonitor(); //创建运行监测窗体的实例
        private Form_DataBaseSource form_DataBaseSource = new Form_DataBaseSource();// 创建数据存储窗体的实例
        private Form_DevCal form_DevCal = new Form_DevCal(); // 创建校准窗体的实例
        private Form_UserManager form_UserManager = new Form_UserManager();//创建用户管理实例
        private CFormAutoSize fAutoSize = new CFormAutoSize();//设置窗体自动大小调节
        //private Module_StateMonitor module_StateMonitor = new Module_StateMonitor();
        private Form_Progressbar myProcessBar;
        private PictureBox pictureBox = new PictureBox(); // 弹框按钮
        private PictureBox pictureBoxHistoryComp = new PictureBox();//弹窗提醒
        private SocketManager tcpServer = null;
        private Thread writeDatabseTask;
        private Task startReceiveTask;// 开始采集任务
        private Task taskTimer;
        private Task taskCmdTrg;
        private CancellationTokenSource startReceiveTaskCancel = new CancellationTokenSource();//令牌取消标志
        private CancellationTokenSource startListenTaskCancel = new CancellationTokenSource();
        private Form_AppInfo form_AppInfo = new Form_AppInfo();//关于此软件
        private Form_ProjectInfo form_ProjectInfo = new Form_ProjectInfo();//项目信息窗口
        public static bool m_adminLogin;
        public static bool m_vistorLogin;
        public static bool save;
        public static bool start;
        public static bool saveDateBase = false;
        public static string lang = "zh-CN";
        public static bool watchHistory;
        #endregion

        #region 委托
        private delegate bool IncreaseHandle(int value);
        private IncreaseHandle myIncrease = null;
        private delegate void refresh_PanelState(bool wait, bool recive, bool save);//声明委托RefreshPanelState 
        private delegate void refresh_Top(bool Eable);//声明改变顶端
        private delegate void refresh_Start(string s_startEnable);
        #endregion

        #region 方法：刷新PanelState状态
        public void ResfreshPanelState(bool wait, bool recive, bool save)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    refresh_PanelState s = new refresh_PanelState(ResfreshPanelState);
                    this.Invoke(s, wait, recive, save);
                }
                else
                {
                    if (wait)
                    {
                        this.label_Wait.BackColor = Color.Green;
                        this.label_Recive.BackColor = Color.Gray;
                        this.label_Storage.BackColor = Color.Gray;
                        this.label_Wait.ForeColor = Color.White;
                        this.label_Recive.ForeColor = Color.Black;
                        this.label_Storage.ForeColor = Color.Black;
                    }
                    else if (recive)
                    {
                        this.label_Recive.BackColor = Color.Green;
                        this.label_Wait.BackColor = Color.Gray;
                        this.label_Storage.BackColor = Color.Gray;
                        this.label_Wait.ForeColor = Color.Black;
                        this.label_Recive.ForeColor = Color.White;
                        this.label_Storage.ForeColor = Color.Black;
                    }
                    else if (save)
                    {
                        this.label_Wait.BackColor = Color.Gray;
                        this.label_Recive.BackColor = Color.Gray;
                        this.label_Storage.BackColor = Color.Green;
                        this.label_Wait.ForeColor = Color.Black;
                        this.label_Recive.ForeColor = Color.Black;
                        this.label_Storage.ForeColor = Color.White;
                    }
                    else
                    {
                        this.label_Wait.BackColor = Color.Gray;
                        this.label_Recive.BackColor = Color.Gray;
                        this.label_Storage.BackColor = Color.Gray;
                        this.label_Wait.ForeColor = Color.Black;
                        this.label_Recive.ForeColor = Color.Black;
                        this.label_Storage.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region 进度条显示进度
        /// < summary>  
        /// Open process bar window  
        /// < /summary>  
        private void ShowProcessBar()
        {
            myProcessBar = new Form_Progressbar();//创建进度条窗体的实例
            myProcessBar.ShowInTaskbar = false;//不显示TaskBar
            myProcessBar.FormBorderStyle = FormBorderStyle.None;////不显示标题栏
            myProcessBar.StartPosition = FormStartPosition.CenterParent;//在程序中间
                                                                        //显示在中间
                                                                        // Init increase event  
            myIncrease = new IncreaseHandle(myProcessBar.Increase);
            myProcessBar.ShowDialog();
            myProcessBar = null;
        }
        /// < summary>  
        /// Sub thread function  
        /// < /summary>  
        private void ThreadFun()
        {
            MethodInvoker mi = new MethodInvoker(ShowProcessBar);
            this.BeginInvoke(mi);
            Thread.Sleep(200);//Sleep a while to show window  
            bool blnIncreased = false;
            object objReturn = null;
            /*执行进度条*/
            do
            {
                Thread.Sleep(20);
                //objReturn = this.BeginInvoke(this.myIncrease,new object[] { 2 });
                objReturn = this.Invoke(this.myIncrease, new object[] { 2 });
                blnIncreased = (bool)objReturn;
            }
            while (blnIncreased);
        }
        #endregion

        #region 订阅DevCal事件
        public void SubscribeDeviceForm(Form_DevCal evenSource)
        {
            evenSource.clickDevCalFormEvent += EvenSource_clickDevCalFormEvent;
        }

        private void EvenSource_clickDevCalFormEvent(object sender, Form_DevCal.clickDevCalFormEventArgs e)
        {
            string str = e.KeyToRaiseEvent;
            if (str == "DevCalClick")
            {
                // 禁用部分窗体的按钮
                TopEnable(false);//主窗体顶部菜单栏部分不可点击
                startEnable(str);
                form_DeviceConfig.disableBtn(false);
                form_ChanConfig.disableBtn(false);
                Form_Main.m_adminLogin = false;
                Form_Main.m_vistorLogin = true;
                // this.iconButton_prjClose.Enabled = false;
            }
            else if (str == "DevCalNoClick")
            {
                // 启用部分窗体的按钮
                TopEnable(true);//主窗体顶部菜单栏部分可点击
                startEnable(str);
                form_DeviceConfig.disableBtn(true);
                form_ChanConfig.disableBtn(true);
                Form_Main.m_adminLogin = true;
                Form_Main.m_vistorLogin = false;
                // this.iconButton_prjClose.Enabled = false;
            }
        }
        #endregion

        #region 订阅ChanConfig事件
        public void SubscribeChannelForm(Form_ChanConfig evenSource)
        {
            evenSource.clickChanFormEvent += EvenSource_clickChanFormEvent;
        }

        private void EvenSource_clickChanFormEvent(object sender, Form_ChanConfig.clickChanFormEventArgs e)
        {
            //this.TopMost = true;       
        }
        #endregion

        #region 订阅ViewConfig点击事件
        /// <summary>
        /// 订阅自定义事件
        /// </summary>
        /// <param name="evenSource"></param>
        public void SubscribeViewForm(Form_ViewConfig evenSource) //绑定左侧
        {
            evenSource.clickViewFormEvent += EvenSource_clickViewFormEvent; //回调方法
        }
        /// <summary>
        /// 回调方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EvenSource_clickViewFormEvent(object sender, Form_ViewConfig.clickViewFormEventArgs e)
        {
            try
            {
                //向波形显示界面添加对应的标签
                string strTag = "";
                string strNo = "";
                double sacleMin = 0.0;
                double scaleMax = 0.0;
                Color colorWave = Color.Red;
                foreach (int ex in form_ViewConfig.dataTable_ViewConfigsShow.Keys)
                {
                    DataTable dtTemp = form_ViewConfig.dataTable_ViewConfigsShow[ex];
                    if (dtTemp.Rows.Count != 0)
                    {
                        foreach (UCChanWaveView item in form_WaveMonitorsList[ex - 1].listChanWaveView)
                        {
                            form_WaveMonitorsList[ex - 1].UnSubscribe(item);
                        }
                        form_WaveMonitorsList[ex - 1].listChanWaveView.Clear();
                        for (int j = 0; j < dtTemp.Rows.Count; j++)
                        {
                            strTag = (string)dtTemp.Rows[j]["Tag"];
                            strNo = (string)dtTemp.Rows[j]["No"];
                            sacleMin = Convert.ToDouble(dtTemp.Rows[j]["ScaleMin"]);
                            scaleMax = Convert.ToDouble(dtTemp.Rows[j]["ScaleMax"]);
                            colorWave = Color.FromArgb(int.Parse(dtTemp.Rows[j]["WaveColor"].ToString()));


                            Module_Channel chanInfo = Module_DeviceManage.Instance.GetChannelByTag(strTag);
                            UCChanWaveView uctemp = new UCChanWaveView(colorWave,
                           strTag,
                           CValue2String.scal2String(Convert.ToDouble(chanInfo.Scale)),
                           CValue2String.voltage2String(Convert.ToDouble(chanInfo.Offset)),
                           chanInfo.BChanInv,
                           chanInfo.BChanBWLimit,
                           chanInfo.BChanImpedence);
                            uctemp.ucChanViewUpdateScale(sacleMin, scaleMax);
                            uctemp.label_ChanID_Set(true);
                            form_WaveMonitorsList[ex - 1].listChanWaveView.Add(uctemp);
                        }
                        form_WaveMonitorsList[ex - 1].update_ChanLabelFlowPanel();
                        //无数据的时候工具栏禁止点击
                        form_WaveMonitorsList[ex - 1].toolStripWaveView_Enable(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("空间名：" + ex.Source + "；" + '\n' +
                     "方法名：" + ex.TargetSite + '\n' +
                     "故障点：" + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf("\\") + 1, ex.StackTrace.Length - ex.StackTrace.LastIndexOf("\\") - 1) + '\n' +
                     "错误提示：" + ex.Message);
            }
        }

        #endregion

        #region 订阅状态监测
        public void SubscribeStateMonitor(Module_StateMonitor evenSource)
        {
            evenSource.stateMonitorEvent += EvenSource_stateMonitorEvent;//回调方法       
        }

        private void EvenSource_stateMonitorEvent(object sender, Module_StateMonitor.stateMonitorEventArgs e)
        {
            string monitorSN = e.KeyToRaiseEvent;
            form_DeviceConfig.StateMonitor(monitorSN);//刷新控件显示
        }

        #endregion

        #region 订阅记录存储界面
        public void SubscribeRecord(Form_RecordConfig evenSource)
        {
            evenSource.clickRecordFormEvent += EvenSource_clickRecordFormEvent;
        }

        private void EvenSource_clickRecordFormEvent(object sender, Form_RecordConfig.clickRecordFormEventArgs e)
        {
            string str = e.KeyToRaiseEvent;
            if ("True" == str && Form_Main.m_adminLogin)
            {
                this.iconButton_ForceTrig.Enabled = true;
                form_RecordConfig.m_enableForce = false;
            }
            else
            {
                this.iconButton_ForceTrig.Enabled = false;
                form_RecordConfig.m_enableForce = false;
            }
        }
        #endregion

        #region 订阅波形显示窗体的事件
        public void SubscribeFormWave(Form_WaveMonitor evenSource)
        {
            evenSource.formWaveEvent += EvenSource_clickWaveFormEvent;
        }

        private void EvenSource_clickWaveFormEvent(object sender, Form_WaveMonitor.formWaveEventArgs e)
        {
            string strTag = "";
            string strNo = "";
            double sacleMin = 0.0;
            double scaleMax = 0.0;
            Color colorWave = Color.Red;
            //  DataTable dtTemp = form_ViewConfig.dataTable_ViewConfigsShow[Convert.ToInt32(e.KeyToRaiseEvent)];

            //DataTable dtTemp =  Module_ViewConfig.Instance.GetShowConfigByGroup(e.KeyToRaiseEvent);
            form_ViewConfig.button_ViewConfigClick(false);
            DataTable dtTemp = form_ViewConfig.dataTable_ViewConfigsShow[Convert.ToInt32(e.KeyToRaiseEvent)];
            if (dtTemp.Rows.Count != 0)
            {
                foreach (UCChanWaveView item in form_WaveMonitorsList[Convert.ToInt32(e.KeyToRaiseEvent) - 1].listChanWaveView)
                {
                    form_WaveMonitorsList[Convert.ToInt32(e.KeyToRaiseEvent) - 1].UnSubscribe(item);
                }
                form_WaveMonitorsList[Convert.ToInt32(e.KeyToRaiseEvent) - 1].listChanWaveView.Clear();
                for (int j = 0; j < dtTemp.Rows.Count; j++)
                {
                    strTag = (string)dtTemp.Rows[j]["Tag"];
                    strNo = (string)dtTemp.Rows[j]["No"];
                    sacleMin = Convert.ToDouble(dtTemp.Rows[j]["ScaleMin"]);
                    scaleMax = Convert.ToDouble(dtTemp.Rows[j]["ScaleMax"]);

                    colorWave = Color.FromArgb(Global.CGlobalColor.lineColor[j % (Global.CGlobalColor.lineColor.Count)].ToArgb());


                    Module_Channel chanInfo = Module_DeviceManage.Instance.GetChannelByTag(strTag);
                    UCChanWaveView uctemp = new UCChanWaveView(colorWave,
                   strTag,
                   CValue2String.scal2String(Convert.ToDouble(chanInfo.Scale)),
                   CValue2String.voltage2String(Convert.ToDouble(chanInfo.Offset)),
                   chanInfo.BChanInv,
                   chanInfo.BChanBWLimit,
                   chanInfo.BChanImpedence);
                    uctemp.ucChanViewUpdateScale(sacleMin, scaleMax);
                    uctemp.label_ChanID_Set(true);
                    form_WaveMonitorsList[Convert.ToInt32(e.KeyToRaiseEvent) - 1].listChanWaveView.Add(uctemp);
                }
                form_WaveMonitorsList[Convert.ToInt32(e.KeyToRaiseEvent) - 1].update_ChanLabelFlowPanel();

                form_WaveMonitorsList[Convert.ToInt32(e.KeyToRaiseEvent) - 1].toolStripWaveView_Enable(true);
                //计算设备间延时的最小值
                form_WaveMonitorsList[Convert.ToInt32(e.KeyToRaiseEvent) - 1].modWavMonitor.devDelayMin = Module_DeviceManage.Instance.GetMinDeviceDelay();
                form_WaveMonitorsList[Convert.ToInt32(e.KeyToRaiseEvent) - 1].modWavMonitor.listOscDataProcess.Clear();
                /*加载接收到的数据*/
                foreach (Module_WaveMonitor.OscilloscopeDataMemory item in Module_DeviceManage.Instance.GetWaveData())
                {
                    form_WaveMonitorsList[Convert.ToInt32(e.KeyToRaiseEvent) - 1].modWavMonitor.modWaveMonitor_Load(item);
                    ///*csv文件操作*/
                    //string path = System.IO.Directory.GetCurrentDirectory();
                    //path += "\\" + item.strChanID + ".csv";
                    //FileStream csvFs = new FileStream(path, FileMode.Create);
                    //StreamWriter csvSw = new StreamWriter(csvFs, System.Text.Encoding.Default);
                    //for (int i = 0; i < item.originData.Length; )
                    //{
                    //    int num = item.originData[i];
                    //    csvSw.WriteLine(num);
                    //    //i = i + 1000;
                    //    i = i + 1;
                    //}
                    //csvSw.Flush();
                    ////关闭流
                    //csvSw.Close();
                    //csvFs.Close();
                }
                form_WaveMonitorsList[Convert.ToInt32(e.KeyToRaiseEvent) - 1].update_PictureBoxView_Data();
                //form_WaveMonitorsList[Convert.ToInt32(e.KeyToRaiseEvent) - 1].update_PictureBoxView_Data_New();
            }

        }
        #endregion

        #region 订阅仪器配置窗体的事件
        public void SubscribeDeviceForm(Form_DeviceConfig evenSource)
        {
            evenSource.clickDeviceFormEvent += EvenSource_clickDeviceFormEvent;
        }

        private void EvenSource_clickDeviceFormEvent(object sender, Form_DeviceConfig.clickDeviceFormEventArgs e)
        {
            //form_ChanConfig.RefeshDelView();
        }
        #endregion

        #region 事件：Load面板
        /// <summary>
        /// 事件：Load面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Main_Load(object sender, EventArgs e)
        {
            /* 两个按钮可按：数据对比和数据存储 */
            this.iconButton_DataBase.IconColor = Color.LightSeaGreen;
            this.iconButton_DataComparison.IconColor = Color.LightSeaGreen;

            CCommomControl.dataRecvProcessCallback = new CCommomControl.dataRecvReportCallback(CFileOperate.DataRecvProcess);
            /*开启波形数据接收的托管线程*/
            CCommomControl.createDataRecvService(CCommomControl.dataRecvProcessCallback);

            /*菜单文件可选*/
            this.iconMenuItem_CreateProj.Enabled = true;
            this.toolStripMenuItem_OpenFile.Enabled = true;
            this.iconButton_prjClose.Visible = false;
            this.iconButton_prjClose.Enabled = false;
            this.iconMenuItem3.Visible = false;


            /* panel不可见 */
            this.panel_Project.Visible = false;
            this.panel_FunctionItem.Visible = false;
            this.panel_Module.Visible = false;

            /*List管理四个实例form_WaveMonitor*/
            form_WaveMonitorsList.Add(form_WaveMonitor1);
            form_WaveMonitorsList.Add(form_WaveMonitor2);
            form_WaveMonitorsList.Add(form_WaveMonitor3);
            form_WaveMonitorsList.Add(form_WaveMonitor4);

            /*List管理四个实例form_HistoryComp*/
            form_HistoryCompsList.Add(form_HistoryComp1);
            form_HistoryCompsList.Add(form_HistoryComp2);
            form_HistoryCompsList.Add(form_HistoryComp3);
            form_HistoryCompsList.Add(form_HistoryComp4);

            /*业务逻辑模块初始化*/
            for (int i = 0; i < form_WaveMonitorsList.Count; i++)
            {
                bool bReturn = form_WaveMonitorsList[i].modWavMonitor.modWaveMonitor_Init();
                if (bReturn != true)
                {
                    System.Windows.Forms.MessageBox.Show(InterpretBase.textTran("初始化插值系数表失败！"), InterpretBase.textTran("警告"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //订阅显示界面事件
                SubscribeFormWave(form_WaveMonitorsList[i]);
            }
            form_WaveMonitorsList[0].modWavMonitor.modWaveMonitor_Init_UsingCDLL();
            // 订阅显示配置界面的事件
            SubscribeViewForm(form_ViewConfig);
            // 订阅监测界面的信息
            SubscribeStateMonitor(form_StateMonitor.modStateMonitor);
            // 订阅记录存储界面的事件
            SubscribeRecord(form_RecordConfig);
            // 订阅仪器配置界面的事件
            SubscribeDeviceForm(form_DeviceConfig);
            // 订阅通道配置界面的事件
            SubscribeChannelForm(form_ChanConfig);
            // 订阅校准界面的事件
            SubscribeDeviceForm(form_DevCal);
            // 波形监测Closing事件
            form_WaveMonitor1.FormClosing += Form_WaveMonitor1_FormClosing;
            form_WaveMonitor2.FormClosing += Form_WaveMonitor2_FormClosing;
            form_WaveMonitor3.FormClosing += Form_WaveMonitor3_FormClosing;
            form_WaveMonitor4.FormClosing += Form_WaveMonitor4_FormClosing;
            // 数据对比窗体Closing事件
            form_HistoryComp1.FormClosing += Form_HistoryComp1_FormClosing;
            form_HistoryComp2.FormClosing += Form_HistoryComp2_FormClosing;
            form_HistoryComp3.FormClosing += Form_HistoryComp3_FormClosing;
            form_HistoryComp4.FormClosing += Form_HistoryComp4_FormClosing;

            // Force按钮Disable
            this.iconButton_ForceTrig.Enabled = false;
            this.iconButton_Start.Enabled = false;
            this.iconButton_Save.Enabled = false;
            this.iconButton_Stop.Enabled = false;

            /* 数据库开关菜单按钮颜色 */
            this.iconMenuItem_OpenSaveDb.IconColor = Color.Gray;
            this.iconMenuItem_CloseSaveDb.IconColor = Color.Red;

            if (Form_Main.m_vistorLogin)
            {
                this.menuStrip_Main.Items[3].Enabled = false;
                this.iconMenuItem_CreateProj.Enabled = false;
                this.ToolStripMenuItem_SaveDataBase.Enabled = false;
                this.iconButton_Cal.Enabled = false;
                this.iconButton_WaveView.Enabled = false;
                this.iconButton_StateView.Enabled = false;
            }
        }
        #region 事件：数据对比关闭订阅事件
        private void Form_HistoryComp4_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBoxHisClose(3);
            e.Cancel = true;
        }

        private void Form_HistoryComp3_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBoxHisClose(2);
            e.Cancel = true;
        }

        private void Form_HistoryComp2_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBoxHisClose(1);
            e.Cancel = true;
        }

        private void Form_HistoryComp1_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBoxHisClose(0);
            e.Cancel = true;
        }
        #endregion

        #region 事件：波形监测窗体关闭订阅事件
        private void Form_WaveMonitor4_FormClosing(object sender, FormClosingEventArgs e)
        {
            //CReflection.callObjectEvent(this.pictureBox, "OnClick", e);
            pictureBoxClose(3);
            e.Cancel = true;
        }

        private void Form_WaveMonitor3_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBoxClose(2);
            e.Cancel = true;
        }

        private void Form_WaveMonitor2_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBoxClose(1);
            e.Cancel = true;
        }

        private void Form_WaveMonitor1_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBoxClose(0);
            e.Cancel = true;

        }
        #endregion

        #endregion

        #region 事件：右侧折叠  事件：左侧展开
        /// <summary>
        /// 事件：右侧折叠
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_DockRight_Click(object sender, EventArgs e)
        {
            this.panel_FunctionItem.Width = 118;
            this.iconButton_DockLeft.IconChar = FontAwesome.Sharp.IconChar.AngleDoubleLeft;
            iconButton_DockLeft.Visible = false;
            iconButton_DockRight.Visible = true;
        }
        /// <summary>
        /// 事件：左侧展开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_DockLeft_Click(object sender, EventArgs e)
        {
            if (this.panel_FunctionItem.Width == 118)
            {
                this.panel_FunctionItem.Width = 37;
                this.iconButton_DockLeft.IconChar = FontAwesome.Sharp.IconChar.AngleDoubleRight;
            }
            else
            {
                this.panel_FunctionItem.Width = 118;
                this.iconButton_DockLeft.IconChar = FontAwesome.Sharp.IconChar.AngleDoubleLeft;
            }

        }
        #endregion

        #region 事件：系统配置按钮点击
        /// <summary>
        /// 事件：系统配置按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_SysConfig_Click(object sender, EventArgs e)
        {
            watchHistoryData(watchHistory);
            /*顶部按钮状态*/
            TopColor("SysConfig");

            /*侧边按钮状态*/
            CControlProp.ButtonBackColor(iconButton_ConfgDev, 198, 198, 198);//仪器配置按钮变色
            CControlProp.ButtonBackColor(iconButton_ConfgChan, 230, 230, 230);//通道配置按钮白色
            CControlProp.ButtonBackColor(iconButton_ConfigView, 230, 230, 230);//通道显示按钮白色
            CControlProp.ButtonBackColor(iconButton_ConfigRecord, 230, 230, 230);//记录配置按钮白色

            //设置按钮字体
            CControlProp.SetButtonProp(true, iconButton_ConfgDev);
            CControlProp.SetButtonProp(false, iconButton_ConfgChan);
            CControlProp.SetButtonProp(false, iconButton_ConfigView);
            CControlProp.SetButtonProp(false, iconButton_ConfigRecord);


            if (panel_Module.Controls.Contains(form_DeviceConfig) || panel_Module.Controls.Contains(form_ChanConfig))
                return;
            this.panel_Module.Controls.Clear(); //清除显示的窗体
            form_DeviceConfig.TopLevel = false;
            form_DeviceConfig.Dock = System.Windows.Forms.DockStyle.Fill;

            this.panel_Module.Controls.Add(form_DeviceConfig);

            form_DeviceConfig.Show();

        }
        #endregion

        #region 事件：波形监测按钮点击
        /// <summary>
        /// 事件：波形监测按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_WaveView_Click(object sender, EventArgs e)
        {
            watchHistoryData(watchHistory);
            TopColor("WaveView"); //顶部按钮状态
            IsClickConfig(e);
            form_ViewConfig.LoadViewConfig();
            pictureBox.Image = global::saker_Winform.Properties.Resources.Popup;//插入弹窗图片
            if (panel_Module.Controls.Contains(tabControl) && panel_Module.Controls.Contains(pictureBox))
                return;
            this.panel_Module.Controls.Clear(); //清除显示的窗体            
            this.panel_Module.Controls.Add(tabControl);  //将tabControl add进去
            this.panel_Module.Controls.Add(pictureBox);  //将pictureBox add进去
            pictureBox.Click -= PictureBox_Click;//取消事件订阅
            if (tabControl.Controls.Count == 0)
            {
                TabPage tabPage = new TabPage();//Group_01
                tabPage.Name = "Group_01";
                tabPage.Text = Module_ViewConfig.Instance.TableName[0];
                TabPage tabPage1 = new TabPage(); //Group_02
                tabPage1.Name = "Group_02";
                tabPage1.Text = Module_ViewConfig.Instance.TableName[1];
                TabPage tabPage2 = new TabPage(); //Group_03
                tabPage2.Name = "Group_03";
                tabPage2.Text = Module_ViewConfig.Instance.TableName[2];
                TabPage tabPage3 = new TabPage(); //Group_04
                tabPage3.Name = "Group_04";
                tabPage3.Text = Module_ViewConfig.Instance.TableName[3];

                ChangeListFormTop.ChangeListFormPropTop(form_WaveMonitorsList);
                tabPage.Controls.Add(form_WaveMonitorsList[0]); //将波形监测窗体add进tablecontrol中
                tabPage1.Controls.Add(form_WaveMonitorsList[1]); //将波形监测窗体add进tablecontrol中
                tabPage2.Controls.Add(form_WaveMonitorsList[2]); //将波形监测窗体add进tablecontrol中
                tabPage3.Controls.Add(form_WaveMonitorsList[3]); //将波形监测窗体add进tablecontrol中

                tabControl.TabPages.Add(tabPage);//tableControl中addTabpage
                tabControl.TabPages.Add(tabPage1);//tableControl中addTabpage
                tabControl.TabPages.Add(tabPage2);//tableControl中addTabpage
                tabControl.TabPages.Add(tabPage3); //tableControl中addTabpage 

                form_WaveMonitorsList[0].Show();
                form_WaveMonitorsList[1].Show();
                form_WaveMonitorsList[2].Show();
                form_WaveMonitorsList[3].Show();
            }
            else
            {
                tabControl.TabPages[0].Text = Module_ViewConfig.Instance.TableName[0];
                tabControl.TabPages[1].Text = Module_ViewConfig.Instance.TableName[1];
                tabControl.TabPages[2].Text = Module_ViewConfig.Instance.TableName[2];
                tabControl.TabPages[3].Text = Module_ViewConfig.Instance.TableName[3];
            }
            tabControl.SizeMode = TabSizeMode.Normal;
            //tabControl.Font = new System.Drawing.Font("微软雅黑", 9.5F, System.Drawing.FontStyle.Regular);//设置选项卡控件的标题栏字体大小
            tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            /* 调整picture_Box的位置使之适应窗体的变化 */
            pictureBox.Location = new System.Drawing.Point(panel_Module.Width - 20, 1);
            pictureBox.Anchor = AnchorStyles.Right;
            pictureBox.Anchor = AnchorStyles.Top;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Height = 19;
            pictureBox.Width = 19;
            pictureBox.BringToFront();
            tabControl.Show();
            pictureBox.Click += PictureBox_Click;
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
        }
        #endregion

        #region 事件：运行监测按钮被点击
        /// <summary>
        /// 事件：运行监测按钮被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_StateView_Click(object sender, EventArgs e)
        {
            watchHistoryData(watchHistory);
            TopColor("StateView"); //顶部按钮状态
            IsClickConfig(e);
            this.iconButton_SysConfig.BackColor = Color.FromArgb(243, 243, 243);//系统配置按钮白色
            this.iconButton_WaveView.BackColor = Color.FromArgb(243, 243, 243);//波形监测按钮白色
            this.iconButton_StateView.BackColor = Color.FromArgb(198, 198, 198);//运行监测按钮被点击
            this.iconButton_DataBase.BackColor = Color.FromArgb(243, 243, 243);//数据存储按钮白色
            this.iconButton_Cal.BackColor = Color.FromArgb(243, 243, 243);//校准按钮白色
            this.panel_FunctionItem.Width = 0;

            if (panel_Module.Controls.Contains(form_StateMonitor))
                return;
            this.panel_Module.Controls.Clear(); //清除显示的窗体    
            ChangeListFormTop.ChangeListFormPropTop(form_StateMonitor);
            this.panel_Module.Controls.Add(form_StateMonitor);  //将运行监测进去
            form_StateMonitor.Show();

        }
        #endregion

        #region 事件：数据存储被点击
        /// <summary>
        /// 事件：数据存储被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_DataBase_Click(object sender, EventArgs e)
        {
            watchHistoryData(true);
            TopColor("DataBase");//顶部按钮状态

            if (panel_Module.Controls.Contains(form_DataBaseSource))
                return;
            this.panel_Module.Controls.Clear(); //清除显示的窗体    
            ChangeListFormTop.ChangeListFormPropTop(form_DataBaseSource);
            this.panel_Module.Controls.Add(form_DataBaseSource);  //将运行监测进去
            form_DataBaseSource.HidePaintButton(false);
            form_DataBaseSource.Show();
        }
        #endregion

        #region 事件：校准被点击
        /// <summary>
        /// 事件：校准被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_Cal_Click(object sender, EventArgs e)
        {
            watchHistoryData(watchHistory);
            TopColor("Cal"); //顶部按钮状态          

            if (panel_Module.Controls.Contains(form_DevCal))
                return;
            this.panel_Module.Controls.Clear(); //清除显示的窗体    
            ChangeListFormTop.ChangeListFormPropTop(form_DevCal);
            this.panel_Module.Controls.Add(form_DevCal);  //将运行监测进去
            form_DevCal.Show();
        }
        #endregion

        #region 事件：数据对比按钮被点击
        private void iconButton_DataComparison_Click(object sender, EventArgs e)
        {
            watchHistoryData(true);
            TopColor("DateComparison"); //顶部按钮状态  
            IsClickConfig(e);
            pictureBoxHistoryComp.Image = global::saker_Winform.Properties.Resources.Popup;//插入弹窗图片
            if (panel_Module.Controls.Contains(tabControlHistoryComp) && panel_Module.Controls.Contains(pictureBoxHistoryComp))
                return;
            this.panel_Module.Controls.Clear(); //清除显示的窗体 
            this.panel_Module.Controls.Add(pictureBoxHistoryComp);  //将pictureBox add进去
            this.panel_Module.Controls.Add(tabControlHistoryComp);  //将tabControl add进去           
            pictureBoxHistoryComp.Click -= PictureBoxHistoryComp_Click;//取消事件订阅
            if (tabControlHistoryComp.Controls.Count == 0)
            {
                TabPage tabPage = new TabPage();//Group_01
                tabPage.Name = "Group_01";
                tabPage.Text = "Group_01";
                TabPage tabPage1 = new TabPage(); //Group_02
                tabPage1.Name = "Group_02";
                tabPage1.Text = "Group_02";
                TabPage tabPage2 = new TabPage(); //Group_03
                tabPage2.Name = "Group_03";
                tabPage2.Text = "Group_03";
                TabPage tabPage3 = new TabPage(); //Group_04
                tabPage3.Name = "Group_04";
                tabPage3.Text = "Group_04";

                ChangeListFormTop.ChangeListFormPropTop(form_HistoryCompsList);
                tabPage.Controls.Add(form_HistoryCompsList[0]); //将波形监测窗体add进tablecontrol中
                tabPage1.Controls.Add(form_HistoryCompsList[1]); //将波形监测窗体add进tablecontrol中
                tabPage2.Controls.Add(form_HistoryCompsList[2]); //将波形监测窗体add进tablecontrol中
                tabPage3.Controls.Add(form_HistoryCompsList[3]); //将波形监测窗体add进tablecontrol中

                tabControlHistoryComp.TabPages.Add(tabPage);//tableControl中addTabpage
                tabControlHistoryComp.TabPages.Add(tabPage1);//tableControl中addTabpage
                tabControlHistoryComp.TabPages.Add(tabPage2);//tableControl中addTabpage
                tabControlHistoryComp.TabPages.Add(tabPage3); //tableControl中addTabpage 

                form_HistoryCompsList[0].Show();
                form_HistoryCompsList[1].Show();
                form_HistoryCompsList[2].Show();
                form_HistoryCompsList[3].Show();
            }
            else
            {
                tabControlHistoryComp.TabPages[0].Text = Module_ViewConfig.Instance.TableName[0];
                tabControlHistoryComp.TabPages[1].Text = Module_ViewConfig.Instance.TableName[1];
                tabControlHistoryComp.TabPages[2].Text = Module_ViewConfig.Instance.TableName[2];
                tabControlHistoryComp.TabPages[3].Text = Module_ViewConfig.Instance.TableName[3];
            }
            tabControlHistoryComp.SizeMode = TabSizeMode.Normal;
            //tabControl.Font = new System.Drawing.Font("微软雅黑", 9.5F, System.Drawing.FontStyle.Regular);//设置选项卡控件的标题栏字体大小
            tabControlHistoryComp.Dock = System.Windows.Forms.DockStyle.Fill;
            /* 调整picture_Box的位置使之适应窗体的变化 */
            pictureBoxHistoryComp.Location = new System.Drawing.Point(panel_Module.Width - 20, 1);
            pictureBoxHistoryComp.Anchor = AnchorStyles.Right;
            pictureBoxHistoryComp.Anchor = AnchorStyles.Top;
            pictureBoxHistoryComp.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxHistoryComp.Height = 19;
            pictureBoxHistoryComp.Width = 19;
            pictureBoxHistoryComp.BringToFront();
            pictureBoxHistoryComp.Show();
            pictureBoxHistoryComp.Click += PictureBoxHistoryComp_Click;
        }
        #endregion

        #region 事件：将数据对比界面弹出
        private void PictureBoxHistoryComp_Click(object sender, EventArgs e)
        {
            if (tabControlHistoryComp.Controls.Count == 0 || this.tabControlHistoryComp.TabPages[this.tabControlHistoryComp.SelectedIndex].Controls.Count != 0)
            {
                tabControlHistoryComp.TabPages[this.tabControlHistoryComp.SelectedIndex].Controls.Remove(form_HistoryCompsList[this.tabControlHistoryComp.SelectedIndex]);//清除TabPage里面的控件
                tabControlHistoryComp.Refresh();
                form_HistoryCompsList[this.tabControlHistoryComp.SelectedIndex].FormBorderStyle = FormBorderStyle.Sizable; //指定边框样式，可调整大小的边框
                form_HistoryCompsList[this.tabControlHistoryComp.SelectedIndex].ShowInTaskbar = true; /*不去除窗体的标题栏*/
                form_HistoryCompsList[this.tabControlHistoryComp.SelectedIndex].TopLevel = true; //顶层
                form_HistoryCompsList[this.tabControlHistoryComp.SelectedIndex].ControlBox = true;
                form_HistoryCompsList[this.tabControlHistoryComp.SelectedIndex].Show();
            }
            else
            {
                form_HistoryCompsList[this.tabControlHistoryComp.SelectedIndex].TopLevel = false;
                form_HistoryCompsList[this.tabControlHistoryComp.SelectedIndex].FormBorderStyle = FormBorderStyle.None; /*去除窗体的标题栏*/
                form_HistoryCompsList[this.tabControlHistoryComp.SelectedIndex].ShowInTaskbar = false; /*去除窗体的标题栏*/
                this.tabControlHistoryComp.TabPages[this.tabControlHistoryComp.SelectedIndex].Controls.Add(form_HistoryCompsList[this.tabControlHistoryComp.SelectedIndex]);
                form_HistoryCompsList[this.tabControlHistoryComp.SelectedIndex].Show();
            }
        }
        #endregion

        #region 事件：将波形监测界面弹出
        /// <summary>
        /// 事件：将波形监测界面弹出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_Click(object sender, EventArgs e)
        {
            if (tabControl.Controls.Count == 0 || this.tabControl.TabPages[this.tabControl.SelectedIndex].Controls.Count != 0)
            {
                tabControl.TabPages[this.tabControl.SelectedIndex].Controls.Remove(form_WaveMonitorsList[this.tabControl.SelectedIndex]);//清除TabPage里面的控件
                tabControl.Refresh();
                form_WaveMonitorsList[this.tabControl.SelectedIndex].FormBorderStyle = FormBorderStyle.Sizable; //指定边框样式，可调整大小的边框
                form_WaveMonitorsList[this.tabControl.SelectedIndex].ShowInTaskbar = true; /*不去除窗体的标题栏*/
                form_WaveMonitorsList[this.tabControl.SelectedIndex].TopLevel = true; //顶层
                form_WaveMonitorsList[this.tabControl.SelectedIndex].ControlBox = true;
                form_WaveMonitorsList[this.tabControl.SelectedIndex].Show();
            }
            else
            {
                form_WaveMonitorsList[this.tabControl.SelectedIndex].TopLevel = false;
                form_WaveMonitorsList[this.tabControl.SelectedIndex].FormBorderStyle = FormBorderStyle.None; /*去除窗体的标题栏*/
                form_WaveMonitorsList[this.tabControl.SelectedIndex].ShowInTaskbar = false; /*去除窗体的标题栏*/
                this.tabControl.TabPages[this.tabControl.SelectedIndex].Controls.Add(form_WaveMonitorsList[this.tabControl.SelectedIndex]);
                form_WaveMonitorsList[this.tabControl.SelectedIndex].Show();
            }
        }

        public void pictureBoxClose(int i)
        {
            if (!(tabControl.Controls.Count == 0 || this.tabControl.TabPages[i].Controls.Count != 0))
            {
                form_WaveMonitorsList[i].TopLevel = false;
                form_WaveMonitorsList[i].FormBorderStyle = FormBorderStyle.None; /*去除窗体的标题栏*/
                form_WaveMonitorsList[i].ShowInTaskbar = false; /*去除窗体的标题栏*/
                this.tabControl.TabPages[i].Controls.Add(form_WaveMonitorsList[i]);
                form_WaveMonitorsList[i].Show();
            }
        }

        public void pictureBoxHisClose(int i)
        {
            if (!(tabControlHistoryComp.Controls.Count == 0 || this.tabControlHistoryComp.TabPages[i].Controls.Count != 0))
            {
                form_HistoryCompsList[i].TopLevel = false;
                form_HistoryCompsList[i].FormBorderStyle = FormBorderStyle.None; /*去除窗体的标题栏*/
                form_HistoryCompsList[i].ShowInTaskbar = false; /*去除窗体的标题栏*/
                this.tabControlHistoryComp.TabPages[i].Controls.Add(form_HistoryCompsList[i]);
                form_HistoryCompsList[i].Show();
            }
        }


        #endregion

        #region 事件：点击仪器配置
        /// <summary>
        /// 事件：点击仪器配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_ConfgDev_Click(object sender, EventArgs e)
        {
            IsClickConfig(e);
            LeftColor("ConfgDev");
            if (panel_Module.Controls.Contains(form_DeviceConfig))
                return;
            this.panel_Module.Controls.Clear(); //清除显示的窗体
            form_DeviceConfig.TopLevel = false;
            form_DeviceConfig.Dock = System.Windows.Forms.DockStyle.Fill;

            this.panel_Module.Controls.Add(form_DeviceConfig);
            form_DeviceConfig.Show();
        }
        #endregion

        #region 事件：通道配置被点击
        /// <summary>
        /// 事件：通道配置被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_ConfgChan_Click(object sender, EventArgs e)
        {
            LeftColor("ConfgChan");
            if (panel_Module.Controls.Contains(form_ChanConfig))
                return;
            this.panel_Module.Controls.Clear(); //清除显示的窗体
            form_ChanConfig.TopLevel = false;
            form_ChanConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Module.Controls.Add(form_ChanConfig);
            try
            {
                form_ChanConfig.LoadChannelConfig();
                form_ChanConfig.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 事件：显示配置被点击
        /// <summary>
        /// 事件：显示配置被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_ConfigView_Click(object sender, EventArgs e)
        {
            /* 通过反射，反射到MdiChildActivate事件上 *//*
            Form_ChanConfig.clickChanFormEventArgs ex = new Form_ChanConfig.clickChanFormEventArgs("");
            CReflection.callObjectEvent(form_ChanConfig, "onClickChanFormEvent", ex);*/
            //是否点击了应用
            IsClickConfig(e);
            LeftColor("ConfigView");
            if (panel_Module.Controls.Contains(form_ViewConfig))
                return;
            this.panel_Module.Controls.Clear(); //清除显示的窗体
            ChangeListFormTop.ChangeListFormPropTop(form_ViewConfig);
            this.panel_Module.Controls.Add(form_ViewConfig);
            form_ViewConfig.Show();
            form_ViewConfig.LoadViewConfig();
        }
        #endregion

        #region 事件：记录配置被点击
        /// <summary>
        /// 事件：记录配置被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_ConfigRecord_Click(object sender, EventArgs e)
        {
            IsClickConfig(e);
            LeftColor("ConfigRecord");
            if (panel_Module.Controls.Contains(form_RecordConfig))
                return;
            this.panel_Module.Controls.Clear(); // 清除显示的窗体
            ChangeListFormTop.ChangeListFormPropTop(form_RecordConfig);
            this.panel_Module.Controls.Add(form_RecordConfig);
            form_RecordConfig.Show();
        }
        #endregion

        #region 方法：是否通过反射来点击应用
        /// <summary>
        /// 方法：是否通过反射
        /// </summary>
        /// <param name="e"></param>
        public void IsClickConfig(EventArgs e)
        {
            // 主机模式
            if (Form_Main.m_adminLogin)
            {
                if (panel_Module.Controls.Contains(form_ChanConfig) && (!form_ChanConfig.m_bConfig) &&
                    (Module_DeviceManage.Instance.Devices.Count > 0))
                {
                    DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(InterpretBase.textTran("通道配置页面未被应用,是否应用?"), InterpretBase.textTran("提示信息"), MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        /* 通过反射，反射到应用按钮事件上 */
                        CReflection.callObjectEvent(form_ChanConfig, "OnClick", e);
                        form_ChanConfig.m_bConfig = false;
                    }
                }
            }

        }
        #endregion

        #region 事件：Start被点击
        private void iconButton_Start_Click(object sender, EventArgs e)
        {
            LogUtil.Init(AppDomain.CurrentDomain.BaseDirectory + "WaveRecvLog", 1024);
            start = true;
            //存储通道模式和初始化校准延时
            Module_DeviceManage.Instance.MaxChannelModel = Module_DeviceManage.Instance.GetMaxChannelMode();
            Module_DeviceManage.Instance.InitDeviceDelayTime();

            if (Module_DeviceManage.Instance.Devices.Count == 0)
            {
                System.Windows.MessageBox.Show(InterpretBase.textTran("设备数量不能为0！"));
                return;
            }

            /* 开启进度条 */
            Form_Progressbar form_Progressbar = new Form_Progressbar();
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            cancelTokenSource.Token.Register(() =>
            {
                form_Progressbar.CloseProcess();//委托方法调用进度条关闭                
                DialogResult res = System.Windows.Forms.MessageBox.Show(InterpretBase.textTran("接收服务已启动完成"), InterpretBase.textTran("服务启动情况"), MessageBoxButtons.OK);
            });

            Task.Run(() =>
            {
                form_Progressbar.ProcessMarquee(InterpretBase.textTran("接收服务启动中..."));//设置进度条显示为左右转动
                form_Progressbar.StartPosition = FormStartPosition.CenterScreen;//程序中间
                form_Progressbar.ShowDialog();
                //form_Process.Show();
            }, cancelTokenSource.Token);

            // 禁用部分窗体的按钮
            TopEnable(false);//主窗体顶部菜单栏部分不可点击
            form_DeviceConfig.disableBtn(false);
            form_ChanConfig.disableBtn(false);
            Form_Main.m_adminLogin = false;
            Form_Main.m_vistorLogin = true;
            this.iconButton_prjClose.Enabled = false;

            //初始化写入数据库队列的值
            CFileOperate.ErrorQueueSize = 0;
            CFileOperate.DequeueSize = 0;

            #region SateMonitor
            //更新运行状态
            form_StateMonitor.m_bRunState = true;
            form_StateMonitor.update_StateMonitorUI();
            //form_StateMonitor.modStateMonitor.devHeartbeatMonitor_Start();
            //form_StateMonitor.Form_StateMonitorInit();
            //form_StateMonitor.Form_StateRefreshButton(false);
            //form_StateMonitor.Form_StateUpdateTestTime(DateTime.Now.ToString());
            #endregion

            // 开始运行前清空显示界面相关数据
            foreach (Form_WaveMonitor item in form_WaveMonitorsList)
            {
                item.waveMonitorForm_Clear();
            }
            this.iconButton_Start.IconChar = FontAwesome.Sharp.IconChar.Pause;
            this.iconButton_Start.Text = "Pause";
            this.iconButton_Start.Enabled = false;
            this.iconButton_Start.Refresh();
            /* 记录配置的Dc */
            Dictionary<string, string> RecordConfigDc = form_RecordConfig.RecordConfigDc();

            //判断是否已经建立连接，如果没有，则建立          
            Parallel.For(0, Module_DeviceManage.Instance.Devices.Count, i =>
        {
            if (Module_DeviceManage.Instance.Devices[i].Status == true && Module_DeviceManage.Instance.Devices[i].GetIsOnTesting())
            {
                if (Module_DeviceManage.Instance.Devices[i].CmdSocket == null)
                {
                    Module_DeviceManage.Instance.Devices[i].CmdSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    Module_DeviceManage.Instance.Devices[i].CmdSocket.Connect(Module_DeviceManage.Instance.Devices[i].IP, 5555);
                }
            }
        });

            // 发送记录配置触发命令          
            taskCmdTrg = new Task(() =>
            {
                Parallel.For(0, Module_DeviceManage.Instance.Devices.Count, i =>
                {
                    if (Module_DeviceManage.Instance.Devices[i].Status == true && Module_DeviceManage.Instance.Devices[i].GetIsOnTesting())
                    {
                        Module_DeviceManage.Instance.Devices[i].CmdSocket.Send(Encoding.Default.GetBytes(form_RecordConfig.m_recordCondition + "\n"));
                    }
                });
            });
            taskTimer = new Task(() =>
            {
                while (true)
                {
                    TimeSpan timeSpan = (TimeSpan)(DateTime.Now - form_RecordConfig.m_AbsTime);
                    if (DateTime.Compare(DateTime.Now, form_RecordConfig.m_AbsTime) >= 0)
                    {
                        Parallel.For(0, Module_DeviceManage.Instance.Devices.Count, i =>
                        {
                            if (Module_DeviceManage.Instance.Devices[i].Status == true)
                            {
                                Module_DeviceManage.Instance.Devices[i].CmdSocket.Send(Encoding.Default.GetBytes(CGlobalCmd.STR_CMD_SET_TRIG_TFORce + "\n"));
                            }
                        });
                        break;
                    }
                }
            });

            if (form_RecordConfig.m_recordCondition == (CGlobalCmd.STR_CMD_SET_TRIG_TFORce + "Abs"))
            {
                taskTimer.Start();
            }
            else if ((form_RecordConfig.m_recordCondition == CGlobalCmd.STR_CMD_SET_TRIG_EDGE_SLOP_POS)
                        || (form_RecordConfig.m_recordCondition == CGlobalCmd.STR_CMD_SET_TRIG_EDGE_SLOP_NEG))
            {
                taskCmdTrg.Start();
            }


            #region 查询本次设备的波形参数信息,每个通道都不一样
            Parallel.For(0, Module_DeviceManage.Instance.Devices.Count, i =>
            {
                if (Module_DeviceManage.Instance.Devices[i].Status == true && Module_DeviceManage.Instance.Devices[i].GetIsOnTesting())
                {
                    try
                    {
                        string command = Global.CGlobalCmd.STR_CMD_GET_WAVEPARASALL + "\n";
                        byte[] paras = new byte[1024];
                        Module_DeviceManage.Instance.Devices[i].CmdSocket.Send(Encoding.Default.GetBytes(command));
                        int ret = Module_DeviceManage.Instance.Devices[i].CmdSocket.Receive(paras);
                        string result = Encoding.Default.GetString(paras, 0, ret);
                        Thread.Sleep(50);
                        string[] res = result.TrimEnd(Environment.NewLine.ToCharArray()).Split(';');
                        for (int k = 0; k < 4; k++)
                        {
                            string[] array = res[k].Split(',');
                            Module_DeviceManage.Instance.Devices[i].Channels[k].XIncrement = array[4].ToString();
                            Module_DeviceManage.Instance.Devices[i].Channels[k].XOrigin = array[5].ToString();
                            Module_DeviceManage.Instance.Devices[i].Channels[k].XReference = array[6].ToString();
                            Module_DeviceManage.Instance.Devices[i].Channels[k].YIncrement = array[7].ToString();
                            Module_DeviceManage.Instance.Devices[i].Channels[k].YOrigin = array[8].ToString();
                            Module_DeviceManage.Instance.Devices[i].Channels[k].YReference = array[9].ToString();
                        }
                        Module_DeviceManage.Instance.Devices[i].CmdSocket.Send(Encoding.Default.GetBytes(Global.CGlobalCmd.STR_CMD_GET_SAMPLERATE + "\n"));
                        ret = Module_DeviceManage.Instance.Devices[i].CmdSocket.Receive(paras);
                        result = Encoding.Default.GetString(paras, 0, ret);
                        Module_DeviceManage.Instance.Devices[i].SampRate = result.TrimEnd(Environment.NewLine.ToCharArray()).ToString();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
            });
            #endregion

            #region 设置本次实验中的仪器，发送数据，其他的关闭不发送
            Parallel.For(0, Module.Module_DeviceManage.Instance.Devices.Count,
                i =>
                {
                    if (Module_DeviceManage.Instance.Devices[i].Status == true && Module_DeviceManage.Instance.Devices[i].GetIsOnTesting())
                    {
                        try
                        {
                            // Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            //设置仪器的初始命令             
                            //Module_DeviceManage.Instance.Devices[i].CmdSocket.Connect(Module_DeviceManage.Instance.Devices[i].IP, 5555);
                            string command = Global.CGlobalCmd.STR_CMD_SET_SERVERIP + Module_DeviceManage.Instance.IP + "\n";
                            //   socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 2000);
                            Module_DeviceManage.Instance.Devices[i].CmdSocket.Send(Encoding.Default.GetBytes(command));
                            Thread.Sleep(50);

                            //command = ":wav:sendport " + Module_DeviceManage.Instance.Devices[i].DataDestinationPort + "\n";
                            command = Global.CGlobalCmd.STR_CMD_SET_PORT + Module_DeviceManage.Instance.Devices[i].DataDestinationPort + "\n";
                            Debug.WriteLine(command);
                            Module_DeviceManage.Instance.Devices[i].CmdSocket.Send(Encoding.Default.GetBytes(command));
                            Thread.Sleep(50);

                            command = Global.CGlobalCmd.STR_CMD_SET_CHANNELS + "0" + "\n";
                            Debug.WriteLine(command);
                            Module_DeviceManage.Instance.Devices[i].CmdSocket.Send(Encoding.Default.GetBytes(command));
                            Thread.Sleep(50);

                            command = Global.CGlobalCmd.STR_CMD_SET_TRIGGERMODE + "NORM" + "\n";
                            Debug.WriteLine(command);
                            Module_DeviceManage.Instance.Devices[i].CmdSocket.Send(Encoding.Default.GetBytes(command));
                            Thread.Sleep(50);

                            command = Global.CGlobalCmd.STR_CMD_SET_RUN + "\n";
                            Debug.WriteLine(command);
                            Module_DeviceManage.Instance.Devices[i].CmdSocket.Send(Encoding.Default.GetBytes(command));
                            Thread.Sleep(50);

                            command = Global.CGlobalCmd.STR_CMD_SET_MEMDEPTH + " 1M" + "\n";
                            //command = ":ACQuire:MDEPth 1M" + "\n";
                            Module_DeviceManage.Instance.Devices[i].CmdSocket.Send(Encoding.Default.GetBytes(command));
                            Thread.Sleep(50);

                            command = Global.CGlobalCmd.STR_CMD_SET_STOP + "\n";
                            Module_DeviceManage.Instance.Devices[i].CmdSocket.Send(Encoding.Default.GetBytes(command));
                            Thread.Sleep(200);

                            //清除内存帧
                            command = Global.CGlobalCmd.STR_CMD_SET_KEY_PRESS_CLEAR + "\n";
                            Debug.WriteLine(command);
                            Module_DeviceManage.Instance.Devices[i].CmdSocket.Send(Encoding.Default.GetBytes(command));
                            Thread.Sleep(50);

                            //判断需要发送的数据的通道                  
                            command = Global.CGlobalCmd.STR_CMD_SET_CHANNELS + Module_DeviceManage.Instance.Devices[i].GetChannelSend() + "\n";
                            Debug.WriteLine(command);
                            Module_DeviceManage.Instance.Devices[i].CmdSocket.Send(Encoding.Default.GetBytes(command));
                            Thread.Sleep(50);



                            //暂时不用
                            //command = Global.CGlobalCmd.STR_CMD_SET_TRIGGERMODE + Module_DeviceManage.Instance.TriggerMode + "\n";
                            //Debug.WriteLine(command);
                            //Module_DeviceManage.Instance.Devices[i].CmdSocket.Send(Encoding.Default.GetBytes(command));
                            //socket.Close();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                    }
                });
            #endregion           

            #region 显示配置保存到数据库      
            try
            {
                Module_DeviceManage.Instance.GUID = "";
                DbHelperSql.InsertDeviceInfo(Module_DeviceManage.Instance);
                RecordConfigDc.Add("CollectGUID", Module_DeviceManage.Instance.GetGUID());
                RecordConfigDc.Add("ProjectGUID", Module_DeviceManage.Instance.ProjectGUID);
                DbHelperSql.InsertRecordInfo(RecordConfigDc);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            // 将Tag绑定到字典中
            form_ViewConfig.button_ViewConfigClick();
            DataTable view4DT = Module_ViewConfig.Instance.Get4GroupTable();
            try
            {
                DbHelperSql.InsertViewConfig(Module_ViewConfig.Instance);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            //writeDatabseTask = new Thread(CFileOperate.Work);
            //writeDatabseTask.Start();
            #endregion

            #region 更新状态界面监测的值
            //更新状态界面监测的值
            Task.Run(() =>
            {
                while (true)
                {
                    if (startListenTaskCancel.Token.IsCancellationRequested)
                    {
                        return;
                    }
                    try
                    {
                        string recordNum = CFileOperate.DequeueSize.ToString();
                        string dataError = CFileOperate.ErrorQueueSize.ToString();
                        string remainNum = CFileOperate._tasks.Count.ToString();
                        form_StateMonitor.update_RecordInfo(recordNum, dataError, remainNum);
                        Thread.Sleep(200);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }

            }, startListenTaskCancel.Token);
            #endregion

            #region 收取数据长度判定
            List<Module_Device> devices = Module_DeviceManage.Instance.Devices.Where(item => item.IsOnTesting == true).ToList();
            int[] ps32ClientIpAddr = new int[devices.Count];
            int[] ps32RecvSize = new int[devices.Count];

            //   int s32ClientNum = Module_DeviceManage.Instance.Devices.Count;
            int s32RecvTimeout = 3000 * 2;

            for (int i = 0; i < devices.Count; i++)
            {

                ps32ClientIpAddr[i] = IPAddress.Parse(devices[i].IP).GetHashCode();
                ps32RecvSize[i] = Module_DeviceManage.Instance.GetMaxChannelMode() * 1000008 + 10;
                if (devices[i].Status == false)
                {
                    devices[i].IsComplete = true;
                }
            }
            CCommomControl.recvDevData(ps32ClientIpAddr, ps32RecvSize, ps32ClientIpAddr.Length, s32RecvTimeout);
            Thread.Sleep(200 * devices.Count);
            cancelTokenSource.Cancel();
            #endregion

            #region 收取波形数据            
            CancellationToken ct_StartReceive = startReceiveTaskCancel.Token;//令牌
            startReceiveTask = new Task(() => { updateWaveDataView(ct_StartReceive); }, ct_StartReceive);//新建循环测试任务
            startReceiveTask.Start();
            #endregion

            /* 刷新采样率label */
            if (Module_DeviceManage.Instance.Devices.Count > 0)
            {
                form_WaveMonitorsList[0].refreshLabel(Module_DeviceManage.Instance.Devices[0].SampRate);
                form_WaveMonitorsList[1].refreshLabel(Module_DeviceManage.Instance.Devices[0].SampRate);
                form_WaveMonitorsList[2].refreshLabel(Module_DeviceManage.Instance.Devices[0].SampRate);
                form_WaveMonitorsList[3].refreshLabel(Module_DeviceManage.Instance.Devices[0].SampRate);
            }

            this.iconButton_Stop.Enabled = true;
            this.iconButton_Start.Enabled = false;

        }
        #endregion

        #region 波形显示调用事件
        private void updateWaveDataView(CancellationToken _ct)
        {
            while (true)
            {
                if (_ct.IsCancellationRequested)
                {
                    return;
                }
                if (Module_DeviceManage.Instance.GetIsComing() == false)
                {
                    ResfreshPanelState(true, false, false);
                    Console.WriteLine("等待触发");
                }
                else
                {
                    ResfreshPanelState(false, true, false);
                    Console.WriteLine("接收数据");
                }
                if ((Module_DeviceManage.Instance.GetIsCompelte() == false))
                {
                    Thread.Sleep(1000);
                }
                else
                {

                    if (saveDateBase)
                    {
                        // 接收完成                    
                        Module_DeviceManage.Instance.StartTime = DateTime.Now;
                        // 写波形数据到数据库
                        writeDatabseTask = new Thread(CFileOperate.Work);
                        writeDatabseTask.Start();
                    }
                    // 存储数据库
                    ResfreshPanelState(false, false, true);
                    Console.WriteLine("存数据库");
                    for (int i = 0; i < Module_DeviceManage.Instance.Devices.Count; i++)
                    {
                        CFileOperate.EnqueueTask(Module_DeviceManage.Instance.Devices[i]);
                        Module_DeviceManage.Instance.Devices[i].IsComplete = false;
                    }

                    //并行计算
                    //Parallel.For(0, form_ViewConfig.dataTable_ViewConfigsShow.Keys.Count, ex =>
                    // {
                    //     form_WaveMonitorsList[ex].toolStripWaveView_Enable(true);
                    //     //计算设备间延时的最小值
                    //     form_WaveMonitorsList[ex].modWavMonitor.devDelayMin = Module_DeviceManage.Instance.GetMinDeviceDelay();
                    //     /*加载接收到的数据*/
                    //     foreach (Module_WaveMonitor.OscilloscopeDataMemory item in Module_DeviceManage.Instance.GetWaveData())
                    //     {
                    //         int index = form_WaveMonitorsList[ex].listChanWaveView.FindIndex(e => (e.m_strChanID == item.strChanID));
                    //         if (index != -1)
                    //         {
                    //             form_WaveMonitorsList[ex].modWavMonitor.modWaveMonitor_Load(item);
                    //         }
                    //     }
                    //     //form_WaveMonitorsList[ex].update_PictureBoxView_Data_New();
                    //     form_WaveMonitorsList[ex].update_PictureBoxView_Data();
                    //     //  restartDataRecv();                 
                    // });
                    // 串行绘制
                    int devDelayMin = Module_DeviceManage.Instance.GetMinDeviceDelay();
                    var wavaData = Module_DeviceManage.Instance.GetWaveData();
                    //for (int ex = 0; ex < form_ViewConfig.dataTable_ViewConfigsShow.Keys.Count; ex++)
                    for (int ex = 0; ex < 4; ex++)
                    {
                        form_WaveMonitorsList[ex].toolStripWaveView_Enable(true);
                        //计算设备间延时的最小值
                        form_WaveMonitorsList[ex].modWavMonitor.devDelayMin = devDelayMin;
                        /*加载接收到的数据*/
                        foreach (Module_WaveMonitor.OscilloscopeDataMemory item in wavaData)
                        {
                            int index = form_WaveMonitorsList[ex].listChanWaveView.FindIndex(e => (e.m_strChanID == item.strChanID));
                            if (index != -1)
                            {
                                form_WaveMonitorsList[ex].modWavMonitor.modWaveMonitor_Load(item);
                            }
                        }
                        form_WaveMonitorsList[ex].update_PictureBoxView_Data();
                        // form_WaveMonitorsList[ex].update_PictureBoxView_Data_Roll();
                    }
                    #region 绘图完成再次开启监听服务                   
                    CCommomControl.restartDataRecv();

                    //int[] ps32ClientIpAddr = new int[Module_DeviceManage.Instance.Devices.Count];
                    //int[] ps32RecvSize = new int[Module_DeviceManage.Instance.Devices.Count];

                    ////   int s32ClientNum = Module_DeviceManage.Instance.Devices.Count;
                    //int s32RecvTimeout = 3000;
                    //for (int i = 0; i < Module_DeviceManage.Instance.Devices.Count; i++)
                    //{
                    //    ps32ClientIpAddr[i] = IPAddress.Parse(Module_DeviceManage.Instance.Devices[i].IP).GetHashCode();
                    //    ps32RecvSize[i] = Module_DeviceManage.Instance.GetMaxChannelMode() * 1000008 + 10;
                    //    if (Module_DeviceManage.Instance.Devices[i].Status == false)
                    //    {
                    //        Module_DeviceManage.Instance.Devices[i].IsComplete = true;
                    //    }
                    //}
                    //recvDevData(ps32ClientIpAddr, ps32RecvSize, ps32ClientIpAddr.Length, s32RecvTimeout);
                    #endregion
                    //Thread.Sleep(500);
                    //#region 发送run命令
                    //Parallel.For(0, Module.Module_DeviceManage.Instance.Devices.Count,i =>
                    //{
                    //   if (Module_DeviceManage.Instance.Devices[i].Status == true)
                    //   {
                    //       try
                    //       {
                    //           Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //           //设置仪器的初始命令             
                    //           socket.Connect(Module_DeviceManage.Instance.Devices[i].IP, 5555);
                    //           string command = ":RUN" + "\n";
                    //           socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 200);
                    //           Debug.WriteLine(Module_DeviceManage.Instance.Devices[i].IP + " RUN");
                    //           socket.Send(Encoding.Default.GetBytes(command));
                    //           Thread.Sleep(50);
                    //           socket.Close();
                    //       }
                    //       catch (Exception ex1)
                    //       {
                    //           Debug.WriteLine(ex1);
                    //       }
                    //   }
                    //});
                    //#endregion

                    //foreach (int ex in form_ViewConfig.dataTable_ViewConfigsShow.Keys)

                    //{
                    //    //int ex = 1;                                             
                    //    form_WaveMonitorsList[ex - 1].toolStripWaveView_Enable(true);
                    //    //计算设备间延时的最小值
                    //    form_WaveMonitorsList[ex - 1].modWavMonitor.devDelayMin = Module_DeviceManage.Instance.GetMinDeviceDelay();
                    //    //form_WaveMonitorsList[ex - 1].modWavMonitor.listOscDataProcess.Clear();
                    //    /*加载接收到的数据*/
                    //    foreach (Module_WaveMonitor.OscilloscopeDataMemory item in Module_DeviceManage.Instance.GetWaveData())
                    //    {
                    //        form_WaveMonitorsList[ex - 1].modWavMonitor.modWaveMonitor_Load(item);
                    //    }
                    //    form_WaveMonitorsList[ex - 1].update_PictureBoxView_Data_New();
                    //}           
                }
            }
        }
        #endregion

        #region 事件：Save按钮被点击
        /// <summary>
        /// 事件：Save按钮被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_Save_Click(object sender, EventArgs e)
        {

            /* // 将Tag绑定到字典中
             form_ViewConfig.button_ViewConfigClick();*/

            #region 保存为本地XML文件                   
            //form_Process.Show();

            Form_Progressbar form_Process = new Form_Progressbar();
            save = true;
            /*#region 开启进度条
            MethodInvoker mi = new MethodInvoker(ShowProcessBar);
            this.BeginInvoke(mi);
            #endregion*/
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            cancelTokenSource.Token.Register(() =>
            {
                form_Process.CloseProcess();//委托方法调用进度条关闭
                System.Windows.Forms.MessageBox.Show(InterpretBase.textTran("文件已保存完毕，存储路径为") + Form_CreateProject.xmlPath, InterpretBase.textTran("保存情况"),
                    MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, System.Windows.Forms.MessageBoxOptions.ServiceNotification);
            });
            Task.Run(() =>
            {
                form_Process.ProcessMarquee(InterpretBase.textTran("文件保存中..."));//设置进度条显示为左右转动
                form_Process.StartPosition = FormStartPosition.CenterScreen;//程序中间
                form_Process.ShowDialog();
            }, cancelTokenSource.Token);

            if (Form_CreateProject.projectName == null)
            {
                Form_CreateProject.projectName = "Project_1";
                System.IO.Directory.CreateDirectory("C:\\" + Form_CreateProject.projectName);
                Form_CreateProject.xmlPath = "C:\\" + Form_CreateProject.projectName;
                Form_CreateProject.Note = "This is Project_1";
            }
            /*创建xml*/
            CXmlHelper xmlHelper = new CXmlHelper();
            string dataTimeXml = DateTime.Now.ToString("yyyyMM_ddHHmmss");
            string xmlFileName = dataTimeXml + Form_CreateProject.projectName + ".xml";//xml文件名（加时间戳）
            string xmlFilePath = Form_CreateProject.xmlPath + "\\" + xmlFileName;//xml文件完整路径（含文件名）
            xmlHelper.CreateXmlDocument("Project", Form_CreateProject.projectName, xmlFilePath, Form_CreateProject.Note, Module_DeviceManage.Instance.ProjectGUID.ToString());

            /*保存xml配置信息表*/
            /*DeviceConfig元素集*/
            xmlHelper.DeleteNodes(xmlFilePath, "//DeviceConfig");//先清空对应的元素集
            foreach (var item in Module_DeviceManage.Instance.Devices.OrderBy(item => item.VirtualNumber).ToList())
            {
                Hashtable hashtable = new Hashtable();//设备信息的属性
                hashtable.Add("VirtualNumber", item.VirtualNumber);
                hashtable.Add("DeviceIP", item.IP);
                hashtable.Add("DeviceStatus", item.Status);
                hashtable.Add("DeviceMAC", item.MAC);
                hashtable.Add("DeviceSoftVersion", item.SoftVersion);
                hashtable.Add("DeviceSN", item.SN);
                hashtable.Add("DeviceSubName", item.Name);
                hashtable.Add("Model", item.Model);//产品型号
                xmlHelper.InsertNode(xmlFilePath, item.SN, true, "//DeviceConfig", hashtable, null);
            }

            /*ChanConfig元素集*/
            xmlHelper.DeleteNodes(xmlFilePath, "//ChanConfig");//先清空对应的元素集
            Hashtable htCommonConfig = new Hashtable();
            htCommonConfig.Add("TrigSource", Module_DeviceManage.Instance.TriggerSource);//触发源
            htCommonConfig.Add("TrigMode", Module_DeviceManage.Instance.TriggerMode);//触发方式
            htCommonConfig.Add("TrigLevel", Module_DeviceManage.Instance.TriggerLevel);//触发电平
            htCommonConfig.Add("HorTime", Module_DeviceManage.Instance.HorizontalTimebase);//水平时基
            htCommonConfig.Add("HorOffset", Module_DeviceManage.Instance.HorizontalOffset);//水平偏移
            htCommonConfig.Add("MemDepth", Module_DeviceManage.Instance.MemDepth);//存储深度
            htCommonConfig.Add("HoldOff", Module_DeviceManage.Instance.HoldOff);
            htCommonConfig.Add("LocalIP", Module_DeviceManage.Instance.IP);//本机IP
            xmlHelper.InsertNode(xmlFilePath, "CommonConfigInfo", true, "//ChanConfig", htCommonConfig, null);

            DataTable dt = Module_DeviceManage.Instance.GetChanelConfig();  //创建DataTable的实例(数据源）
            if (dt.Rows.Count != 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Hashtable htChanDeviceDT = new Hashtable();
                    htChanDeviceDT.Add("ID", dt.Rows[i]["ID"].ToString());
                    htChanDeviceDT.Add("Collect", dt.Rows[i]["Collect"].ToString());
                    htChanDeviceDT.Add("Record", dt.Rows[i]["Record"].ToString());
                    htChanDeviceDT.Add("DeviceName", dt.Rows[i]["DeviceName"].ToString());
                    htChanDeviceDT.Add("ChannelID", dt.Rows[i]["ChannelID"].ToString());
                    htChanDeviceDT.Add("Tag", dt.Rows[i]["Tag"].ToString());
                    htChanDeviceDT.Add("TagDesc", dt.Rows[i]["TagDesc"].ToString());
                    htChanDeviceDT.Add("MeasureType", dt.Rows[i]["MeasureType"].ToString());
                    htChanDeviceDT.Add("Scale", dt.Rows[i]["Scale"].ToString());
                    htChanDeviceDT.Add("Offset", dt.Rows[i]["Offset"].ToString());
                    htChanDeviceDT.Add("Impedance", dt.Rows[i]["Impedance"].ToString());
                    htChanDeviceDT.Add("Coupling", dt.Rows[i]["Coupling"].ToString());
                    htChanDeviceDT.Add("ProbeRatio", dt.Rows[i]["ProbeRatio"].ToString());
                    htChanDeviceDT.Add("SN", dt.Rows[i]["SN"].ToString());
                    htChanDeviceDT.Add("ChannelDelayTime", dt.Rows[i]["ChannelDelayTime"].ToString());
                    htChanDeviceDT.Add("Open", dt.Rows[i]["Open"].ToString());
                    htChanDeviceDT.Add("Valid", dt.Rows[i]["Valid"].ToString());
                    xmlHelper.InsertNode(xmlFilePath, "dtRows" + (i + 1).ToString(), true, "//ChanConfig", htChanDeviceDT, null);
                }
            }
            /*ViewConfig元素集*/
            xmlHelper.DeleteNodes(xmlFilePath, "//ViewConfig");//先清空对应的元素集
            Hashtable htViewConfigTab = new Hashtable();
            htViewConfigTab.Add("GroupName", "");
            for (int i = 0; i < 4; i++)
            {
                htViewConfigTab["GroupName"] = Module_ViewConfig.Instance.TableName[i];
                xmlHelper.InsertNode(xmlFilePath, "G" + (i + 1).ToString(), true, "//ViewConfig", htViewConfigTab, null);
            }

            DataTable dataTableView1 = Module_ViewConfig.Instance.GetShowConfigByGroup("1");
            DataTable dataTableView2 = Module_ViewConfig.Instance.GetShowConfigByGroup("2");
            DataTable dataTableView3 = Module_ViewConfig.Instance.GetShowConfigByGroup("3");
            DataTable dataTableView4 = Module_ViewConfig.Instance.GetShowConfigByGroup("4");
            List<DataTable> listView = new List<DataTable>();//List管理dataTable
            listView.Add(dataTableView1);
            listView.Add(dataTableView2);
            listView.Add(dataTableView3);
            listView.Add(dataTableView4);
            if (listView.Count > 0)
            {
                for (int i = 0; i < listView.Count; i++)
                {
                    if (listView[i].Rows.Count != 0)
                    {
                        for (int j = 0; j < listView[i].Rows.Count; j++)
                        {
                            Hashtable htViewConfig = new Hashtable();
                            htViewConfig.Add("ID", listView[i].Rows[j]["ID"].ToString());
                            htViewConfig.Add("Tag", listView[i].Rows[j]["Tag"].ToString());
                            htViewConfig.Add("No", listView[i].Rows[j]["No"].ToString());
                            htViewConfig.Add("IsShow", listView[i].Rows[j]["IsShow"].ToString());
                            htViewConfig.Add("Y", listView[i].Rows[j]["Y"].ToString());
                            htViewConfig.Add("ScaleMin", listView[i].Rows[j]["ScaleMin"].ToString());
                            htViewConfig.Add("ScaleMax", listView[i].Rows[j]["ScaleMax"].ToString());
                            htViewConfig.Add("WaveColor", listView[i].Rows[j]["WaveColor"].ToString());
                            htViewConfig.Add("WaveType", listView[i].Rows[j]["WaveType"].ToString());
                            xmlHelper.InsertNode(xmlFilePath, "dtRows" + (j + 1).ToString(), true, "//G" + (i + 1).ToString(), htViewConfig, null);
                        }
                    }
                }

            }
            ///更改保存文件后缀名
            CXmlHelper.UpdateExtension(xmlFilePath, ".slr");
            //System.Windows.Forms.MessageBox.Show("文件已保存完毕，存储路径为"+ Form_CreateProject.xmlPath, "保存情况");
            cancelTokenSource.Cancel();
            #endregion
        }
        #endregion

        #region 事件：窗体缩放
        private void Form_Main_Resize_1(object sender, EventArgs e)
        {
            pictureBox.Location = new System.Drawing.Point(panel_Module.Width - 20, 1);
            pictureBoxHistoryComp.Location = new System.Drawing.Point(panel_Module.Width - 20, 1);
        }
        #endregion

        #region 方法：watch历史数据，窗体显示
        public void watchHistoryData(bool watch)
        {
            if (watch)
            {
                /* panel可见 */
                this.panel_Project.Visible = true;
                this.panel_FunctionItem.Visible = true;
                this.panel_Module.Visible = true;
                this.panel_FunctionItem.Width = 118; //窗体展开宽度为118             
                this.iconButton_DockRight.Visible = false;
                this.iconButton_DockLeft.Visible = true;

            }
            else
            {
                this.panel_Module.Visible = false;
                this.panel_FunctionItem.Visible = false;
                this.panel_FunctionItem.Width = 0; //窗体展开宽度为11 
            }

        }
        #endregion

        #region 事件：创建工程
        /// <summary>
        /// 事件：创建工程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconMenuItem_CreateProj_Click(object sender, EventArgs e)
        {

            Form_CreateProject form_CreateProject = new Form_CreateProject();
            form_CreateProject.createFile = true;
            form_CreateProject.SetProp();//设置窗体属性
            form_CreateProject.ShowDialog();

            if (form_CreateProject.CreatePrj)
            {
                Module_ViewConfig.Instance.TableName[0] = "Group_1";
                Module_ViewConfig.Instance.TableName[1] = "Group_2";
                Module_ViewConfig.Instance.TableName[2] = "Group_3";
                Module_ViewConfig.Instance.TableName[3] = "Group_4";
                watchHistory = true;
                watchHistoryData(true);
                /* 顶部状态 */
                EnableAdmin();

                form_DeviceConfig.ClearShowInfo();
                form_DeviceConfig.OpenfileXmlDeviceConfig();
                this.panel_Module.Visible = true;
                this.panel_FunctionItem.Visible = true;
                this.ucLabel_Prj.NewText = Form_CreateProject.projectName;// 刷新工程名
                Module_DeviceManage.Instance.ProjectName = Form_CreateProject.projectName;// 写进单例中
                this.ucLabel_Prj.Refresh();// 刷新工程名
                this.panel_Module.Refresh();
                form_DeviceConfig.Refresh();
                form_CreateProject.CreatePrj = false;
                this.iconMenuItem_CreateProj.Enabled = false;
                this.toolStripMenuItem_OpenFile.Enabled = false;

                /*配色*/
                // this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
                CControlProp.ButtonBackColor(iconButton_ConfgDev, 198, 198, 198);//通道配置按钮白色
                CControlProp.ButtonBackColor(iconButton_ConfgChan, 230, 230, 230);//仪器配置按钮变色
                CControlProp.ButtonBackColor(iconButton_ConfigView, 230, 230, 230);//通道显示按钮白色
                CControlProp.ButtonBackColor(iconButton_ConfigRecord, 230, 230, 230);//记录配置按钮白色

                CControlProp.ButtonBackColor(iconButton_SysConfig, 198, 198, 198);//系统配置按钮变色
                CControlProp.ButtonBackColor(iconButton_WaveView, 243, 243, 243);//波形监测按钮白色
                CControlProp.ButtonBackColor(iconButton_StateView, 243, 243, 243);//运行监测按钮白色
                CControlProp.ButtonBackColor(iconButton_DataBase, 243, 243, 243);//数据存储按钮白色
                CControlProp.ButtonBackColor(iconButton_Cal, 243, 243, 243);//数据存储按钮白色

                CControlProp.SetButtonProp(true, iconButton_ConfgDev);//设置右侧Button按钮字体大小及加粗
                this.iconButton_ForceTrig.IconChar = FontAwesome.Sharp.IconChar.HandPointUp;
                iconButton_DockLeft.Visible = false;//右侧折叠看不见

                /*加载form_DeviceConfig*/
                form_DeviceConfig.TopLevel = false;
                form_DeviceConfig.Dock = System.Windows.Forms.DockStyle.Fill;
                if (this.panel_Module.Contains(form_DeviceConfig))
                {
                    this.form_DeviceConfig.Refresh();
                    return;
                }
                this.panel_Module.Controls.Clear();
                this.panel_Module.Controls.Add(form_DeviceConfig);
                form_DeviceConfig.Show();
                //天蓝色
                SetColorDeepSkyBlue();

            }

        }
        #endregion

        #region 方法：管理员访客按钮显示
        public void EnableAdmin()
        {
            if (Form_Main.m_adminLogin)
            {
                this.iconButton_Start.Enabled = true;
                this.iconButton_Save.Enabled = true;
            }
        }
        #endregion

        #region 事件：打开工程，从本地XML取数据
        /// <summary>
        /// 从本地XML取数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_LocalOpen_Click(object sender, EventArgs e)
        {
            Form_CreateProject form_CreateProject = new Form_CreateProject();
            form_CreateProject.Text = "打开工程";
            form_CreateProject.openFile = true;
            form_CreateProject.SetProp();//设置窗体属性
            form_CreateProject.ShowDialog();
            if (form_CreateProject.CreateStatus)//Open一个XML文件
            {
                CXmlHelper.UpdateExtension(Form_CreateProject.xmlAllPath, ".xml");
                Regex regex = new Regex(".slr");
                Form_CreateProject.xmlAllPath = regex.Replace(Form_CreateProject.xmlAllPath, ".xml");
                form_DeviceConfig.ClearShowInfo();
                CXmlHelper xmlHelper = new CXmlHelper();
                /*工程信息*/
                System.Xml.XmlNode xmlNodeProject = xmlHelper.GetXmlNodeByXpath(Form_CreateProject.xmlAllPath, "//Project");
                Module_DeviceManage.Instance.ProjectGUID = xmlNodeProject.Attributes[2].Value;
                /*DeviceConfig元素集*/
                System.Xml.XmlNode xmlNode = xmlHelper.GetXmlNodeByXpath(Form_CreateProject.xmlAllPath, "//DeviceConfig");
                if (xmlNode.ChildNodes.Count != 0)
                {
                    int k = 1;
                    for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
                    {
                        Module_Device module_Device = new Module_Device();//创建设备实例
                        module_Device.IP = xmlNode.ChildNodes[i].Attributes["DeviceIP"].Value;
                        module_Device.SoftVersion = xmlNode.ChildNodes[i].Attributes["DeviceSoftVersion"].Value;
                        module_Device.Name = xmlNode.ChildNodes[i].Attributes["DeviceSubName"].Value;
                        module_Device.MAC = xmlNode.ChildNodes[i].Attributes["DeviceMAC"].Value;
                        module_Device.SN = xmlNode.ChildNodes[i].Attributes["DeviceSN"].Value;
                        module_Device.Status = bool.Parse(xmlNode.ChildNodes[i].Attributes["DeviceStatus"].Value);
                        module_Device.VirtualNumber = xmlNode.ChildNodes[i].Attributes["VirtualNumber"].Value;
                        module_Device.Model = xmlNode.ChildNodes[i].Attributes["Model"].Value;

                        Module_DeviceManage.Instance.Devices.Add(module_Device);
                        /*ChanConfig元素集*/
                        System.Xml.XmlNode xmlNode_ChanConfigCommon = xmlHelper.GetXmlNodeByXpath(Form_CreateProject.xmlAllPath, "//CommonConfigInfo");
                        if (xmlNode_ChanConfigCommon.Attributes.Count > 0)
                        {
                            Module_DeviceManage.Instance.HorizontalOffset = xmlNode_ChanConfigCommon.Attributes["HorOffset"].Value;
                            Module_DeviceManage.Instance.TriggerMode = xmlNode_ChanConfigCommon.Attributes["TrigMode"].Value;
                            Module_DeviceManage.Instance.MemDepth = xmlNode_ChanConfigCommon.Attributes["MemDepth"].Value;
                            Module_DeviceManage.Instance.TriggerLevel = xmlNode_ChanConfigCommon.Attributes["TrigLevel"].Value;
                            Module_DeviceManage.Instance.TriggerSource = xmlNode_ChanConfigCommon.Attributes["TrigSource"].Value;
                            Module_DeviceManage.Instance.IP = xmlNode_ChanConfigCommon.Attributes["LocalIP"].Value;
                            Module_DeviceManage.Instance.HorizontalTimebase = xmlNode_ChanConfigCommon.Attributes["HoldOff"].Value;
                            Module_DeviceManage.Instance.HoldOff = xmlNode_ChanConfigCommon.Attributes["HorTime"].Value;
                        }
                        System.Xml.XmlNode xmlNode_ChanConfigData = xmlHelper.GetXmlNodeByXpath(Form_CreateProject.xmlAllPath, "//ChanConfig");
                        if (xmlNode_ChanConfigData.ChildNodes.Count != 0)
                        {
                            for (int j = 1; j < 5; j++)
                            {
                                Module_DeviceManage.Instance.GetDeviceByIP(module_Device.IP).GetChannel(j).ChannelID = int.Parse(xmlNode_ChanConfigData.ChildNodes[k].Attributes["ChannelID"].Value.Substring(2, 1));
                                Module_DeviceManage.Instance.GetDeviceByIP(module_Device.IP).GetChannel(j).Collect = bool.Parse(xmlNode_ChanConfigData.ChildNodes[k].Attributes["Collect"].Value);
                                Module_DeviceManage.Instance.GetDeviceByIP(module_Device.IP).GetChannel(j).Record = bool.Parse(xmlNode_ChanConfigData.ChildNodes[k].Attributes["Record"].Value);
                                Module_DeviceManage.Instance.GetDeviceByIP(module_Device.IP).GetChannel(j).MeasureType = xmlNode_ChanConfigData.ChildNodes[k].Attributes["MeasureType"].Value;
                                Module_DeviceManage.Instance.GetDeviceByIP(module_Device.IP).GetChannel(j).Scale = xmlNode_ChanConfigData.ChildNodes[k].Attributes["Scale"].Value;
                                Module_DeviceManage.Instance.GetDeviceByIP(module_Device.IP).GetChannel(j).Offset = xmlNode_ChanConfigData.ChildNodes[k].Attributes["Offset"].Value;
                                Module_DeviceManage.Instance.GetDeviceByIP(module_Device.IP).GetChannel(j).Impedance = xmlNode_ChanConfigData.ChildNodes[k].Attributes["Impedance"].Value;
                                Module_DeviceManage.Instance.GetDeviceByIP(module_Device.IP).GetChannel(j).Coupling = xmlNode_ChanConfigData.ChildNodes[k].Attributes["Coupling"].Value;
                                Module_DeviceManage.Instance.GetDeviceByIP(module_Device.IP).GetChannel(j).ProbeRatio = xmlNode_ChanConfigData.ChildNodes[k].Attributes["ProbeRatio"].Value;
                                Module_DeviceManage.Instance.GetDeviceByIP(module_Device.IP).GetChannel(j).Tag = xmlNode_ChanConfigData.ChildNodes[k].Attributes["Tag"].Value;
                                Module_DeviceManage.Instance.GetDeviceByIP(module_Device.IP).GetChannel(j).TagDesc = xmlNode_ChanConfigData.ChildNodes[k].Attributes["TagDesc"].Value;
                                Module_DeviceManage.Instance.GetDeviceByIP(module_Device.IP).GetChannel(j).ChannelDelayTime = xmlNode_ChanConfigData.ChildNodes[k].Attributes["ChannelDelayTime"].Value;
                                Module_DeviceManage.Instance.GetDeviceByIP(module_Device.IP).GetChannel(j).Open = bool.Parse(xmlNode_ChanConfigData.ChildNodes[k].Attributes["Open"].Value);
                                Module_DeviceManage.Instance.GetDeviceByIP(module_Device.IP).GetChannel(j).Valid = bool.Parse(xmlNode_ChanConfigData.ChildNodes[k].Attributes["Valid"].Value);
                                k++;
                            }
                        }
                    }

                    /*ViewConfig元素集*/
                    Module.Module_ViewConfig.Instance.ViewConfig.Clear();//清除单例中的数据 
                    System.Xml.XmlNode xmlNode_ViewConfig = xmlHelper.GetXmlNodeByXpath(Form_CreateProject.xmlAllPath, "//ViewConfig");
                    if (xmlNode_ViewConfig.ChildNodes.Count != 0)
                    {
                        for (int i = 0; i < xmlNode_ViewConfig.ChildNodes.Count; i++)
                        {
                            Module_ViewConfig.Instance.TableName[i] = xmlNode_ViewConfig.ChildNodes[i].Attributes["GroupName"].Value;
                            List<Module_Parameter> list = new List<Module_Parameter>();
                            System.Xml.XmlNode xmlNode_ViewConfigG = xmlHelper.GetXmlNodeByXpath(Form_CreateProject.xmlAllPath, "//G" + (i + 1).ToString());
                            if (xmlNode_ViewConfigG.ChildNodes.Count != 0)
                            {

                                for (int j = 0; j < xmlNode_ViewConfigG.ChildNodes.Count; j++)
                                {
                                    Module_Parameter parameter = new Module_Parameter();
                                    parameter.ID = xmlNode_ViewConfigG.ChildNodes[j].Attributes["ID"].Value;
                                    parameter.No = xmlNode_ViewConfigG.ChildNodes[j].Attributes["No"].Value;
                                    parameter.Tag = xmlNode_ViewConfigG.ChildNodes[j].Attributes["Tag"].Value;
                                    if (string.IsNullOrEmpty(xmlNode_ViewConfigG.ChildNodes[j].Attributes["IsShow"].Value))
                                    {
                                        xmlNode_ViewConfigG.ChildNodes[j].Attributes["IsShow"].Value = "false";
                                    }
                                    parameter.IsShow = Convert.ToBoolean(xmlNode_ViewConfigG.ChildNodes[j].Attributes["IsShow"].Value);
                                    parameter.Y = xmlNode_ViewConfigG.ChildNodes[j].Attributes["Y"].Value;
                                    parameter.ScaleMin = xmlNode_ViewConfigG.ChildNodes[j].Attributes["ScaleMin"].Value;
                                    parameter.ScaleMax = xmlNode_ViewConfigG.ChildNodes[j].Attributes["ScaleMax"].Value;
                                    parameter.WaveColor = Color.FromArgb(int.Parse(xmlNode_ViewConfigG.ChildNodes[j].Attributes["WaveColor"].Value));
                                    parameter.WaveType = "趋势图";
                                    parameter.IsChoose = true;
                                    list.Add(parameter);
                                }
                            }
                            Module.Module_ViewConfig.Instance.ViewConfig.Add((i + 1).ToString(), list);
                            Module_ViewConfig.Instance.IsInit = true;
                        }

                    }
                    if (m_adminLogin)
                    {
                        int[] deviceArray = new int[Module_DeviceManage.Instance.Devices.Count];
                        int[] protocolArray = new int[Module_DeviceManage.Instance.Devices.Count];
                        int[] devRevCmdPortArray = new int[Module_DeviceManage.Instance.Devices.Count];
                        int[] resultArray = new int[Module_DeviceManage.Instance.Devices.Count];
                        int[] devSendDataPortArray = new int[Module_DeviceManage.Instance.Devices.Count];
                        int[] pcSendCmdPortArray = new int[Module_DeviceManage.Instance.Devices.Count];
                        int[] pcRevDataPortArray = new int[Module_DeviceManage.Instance.Devices.Count];
                        int portCount = CPortVerification.GetAvailablePort(Module_DeviceManage.Instance.Devices.Count);
                        if (portCount == -1)
                        {
                            System.Windows.MessageBox.Show(InterpretBase.textTran("无可用端口"));
                            return;
                        }
                        for (int i = 0; i < Module_DeviceManage.Instance.Devices.Count; i++)
                        {
                            deviceArray[i] = IPAddress.Parse(Module_DeviceManage.Instance.Devices[i].IP).GetHashCode();
                            protocolArray[i] = 1;
                            //命令端口
                            devRevCmdPortArray[i] = 5555;
                            pcSendCmdPortArray[i] = 5555;
                            devSendDataPortArray[i] = CPortVerification.PortValid[i * 4];
                            pcRevDataPortArray[i] = CPortVerification.PortValid[i * 4 + 1];
                        }
                        CCommomControl.regDevConfigCilent(deviceArray, protocolArray, devRevCmdPortArray, pcSendCmdPortArray, deviceArray.Length, resultArray);
                        CCommomControl.addDevInfo2DataRecvService(deviceArray, devSendDataPortArray, protocolArray, pcRevDataPortArray, deviceArray.Length, resultArray);
                        //string cmdSetPort = ":wav:sendport " ;
                        //List<byte> sendBuff = new List<byte>();
                        //int[] writeLen = new int[deviceArray.Length];
                        for (int i = 0; i < deviceArray.Length; i++)
                        {
                            Module_DeviceManage.Instance.Devices[i].Status = false;
                            if (resultArray[i] == 0)
                            {
                                Module_DeviceManage.Instance.Devices[i].Status = true;

                                // Module_DeviceManage.Instance.Devices[i].CmdSocket.Connect(Module_DeviceManage.Instance.Devices[i].IP, 5555);
                            }
                            Module_DeviceManage.Instance.Devices[i].HashCode = deviceArray[i];
                            Module_DeviceManage.Instance.Devices[i].CmdSourcePort = devRevCmdPortArray[i];
                            Module_DeviceManage.Instance.Devices[i].CmdDestinationPort = pcSendCmdPortArray[i];
                            Module_DeviceManage.Instance.Devices[i].DataSourcePort = devSendDataPortArray[i];
                            Module_DeviceManage.Instance.Devices[i].DataDestinationPort = pcRevDataPortArray[i];
                            //byte[] buffer = Encoding.Default.GetBytes(cmdSetPort + pcSendCmdPortArray[i]+"\n");
                            //writeLen[i] = buffer.Length;
                            //sendBuff.AddRange(buffer);
                        }
                    }
                    //Module_DeviceManage.Instance.InitDeviceDelayTime();
                    //  write2Device(deviceArray, devRevCmdPortArray,deviceArray.Length,sendBuff.ToArray(),writeLen,resultArray);               
                    /*加载form_DeviceConfig*/
                }
                watchHistory = true;
                watchHistoryData(true);
                this.ucLabel_Prj.NewText = Form_CreateProject.projectName;// 刷新工程名
                Module.Module_DeviceManage.Instance.ProjectName = Form_CreateProject.projectName;//写进单例中
                Module.Module_DeviceManage.Instance.ProjectRemark = Form_CreateProject.Note;
                this.ucLabel_Prj.Refresh();// 刷新工程名

                /* 配色 */
                CControlProp.ButtonBackColor(iconButton_SysConfig, 198, 198, 198);//系统配置按钮变色
                CControlProp.ButtonBackColor(iconButton_WaveView, 243, 243, 243);//波形监测按钮白色
                CControlProp.ButtonBackColor(iconButton_StateView, 243, 243, 243);//运行监测按钮白色
                CControlProp.ButtonBackColor(iconButton_DataBase, 243, 243, 243);//数据存储按钮白色
                CControlProp.ButtonBackColor(iconButton_Cal, 243, 243, 243);//数据存储按钮白色

                CControlProp.ButtonBackColor(iconButton_ConfgDev, 198, 198, 198);//仪器配置按钮变色
                CControlProp.ButtonBackColor(iconButton_ConfgChan, 230, 230, 230);//通道配置按钮白色
                CControlProp.ButtonBackColor(iconButton_ConfigView, 230, 230, 230);//通道显示按钮白色
                CControlProp.ButtonBackColor(iconButton_ConfigRecord, 230, 230, 230);//记录配置按钮白色

                /* 顶部状态 */
                EnableAdmin();

                /* 加载form_DeviceConfig */
                form_DeviceConfig.TopLevel = false;
                form_DeviceConfig.Dock = System.Windows.Forms.DockStyle.Fill;
                this.panel_Module.Controls.Clear();
                this.panel_Module.Controls.Add(form_DeviceConfig);
                form_DeviceConfig.Show();
                form_DeviceConfig.Visible = true;
                this.panel_Module.Visible = true;
                form_DeviceConfig.OpenfileXmlDeviceConfig();//刷新DeviceConfig的值
                this.panel_Module.Refresh();// 刷新窗体
                form_DeviceConfig.Refresh();// 刷新窗体
                /* 刷新form_ChanConfig、form_ViewConfig */
                //form_ChanConfig.RefeshDelView();
                form_ViewConfig.LoadViewConfig();

                form_CreateProject.CreateStatus = false;
                this.iconMenuItem_CreateProj.Enabled = false;
                this.toolStripMenuItem_OpenFile.Enabled = false;
                CXmlHelper.UpdateExtension(Form_CreateProject.xmlAllPath, "slr");
                SetColorDeepSkyBlue();
            }

        }
        #endregion

        #region 事件：关闭工程
        /// <summary>
        /// 事件：关闭工程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_prjClose_Click(object sender, EventArgs e)
        {
            pictureBoxClose(0);
            pictureBoxClose(1);
            pictureBoxClose(2);
            pictureBoxClose(3);

            pictureBoxHisClose(0);
            pictureBoxHisClose(1);
            pictureBoxHisClose(2);
            pictureBoxHisClose(3);

            /*开启菜单：文件*/
            if (Form_Main.m_adminLogin)
            {
                this.iconMenuItem_CreateProj.Enabled = true;
            }
            this.toolStripMenuItem_OpenFile.Enabled = true;

            this.iconButton_Start.IconChar = FontAwesome.Sharp.IconChar.Play;
            this.iconButton_Start.Text = "Start";

            this.iconButton_Start.Enabled = false;
            this.iconButton_Save.Enabled = false;
            this.iconButton_Stop.Enabled = false;

            this.ucLabel_Prj.NewText = "";
            this.ucLabel_Prj.Refresh();
            this.panel_Module.Visible = false;
            this.panel_FunctionItem.Visible = false;
            this.panel_Project.Visible = false;

            /* 配色 */
            CControlProp.ButtonBackColor(iconButton_SysConfig, 243, 243, 243);//系统配置按钮变色
            CControlProp.ButtonBackColor(iconButton_WaveView, 243, 243, 243);//波形监测按钮白色
            CControlProp.ButtonBackColor(iconButton_StateView, 243, 243, 243);//运行监测按钮白色
            CControlProp.ButtonBackColor(iconButton_DataBase, 243, 243, 243);//数据存储按钮白色
            CControlProp.ButtonBackColor(iconButton_Cal, 243, 243, 243);//数据存储按钮白色

            /* 清窗体和List */
            form_DeviceConfig.ClearShowInfo();

            form_WaveMonitor1.waveMonitorForm_Clear();
            form_WaveMonitor2.waveMonitorForm_Clear();
            form_WaveMonitor3.waveMonitorForm_Clear();
            form_WaveMonitor4.waveMonitorForm_Clear();
            Module.Module_ViewConfig.Instance.ViewConfig.Clear();//清除显示配置单例中的数据
            form_ViewConfig.ClearViewConfigShow();
            if (!this.panel_Module.Controls.Contains(form_DeviceConfig))
            {
                this.panel_Module.Controls.Clear();
            }

            startReceiveTaskCancel.Cancel();
            startReceiveTaskCancel = new CancellationTokenSource();

            //单例初始化
            Module_DeviceManage.Instance.GUID = "";
            CCommomControl.stopDataRecv();
            pictureBoxClose(0);
            pictureBoxClose(1);
            pictureBoxClose(2);
            pictureBoxClose(3);

            //清空队列任务
            if (writeDatabseTask == null)
            {
                CFileOperate._tasks.Clear();
            }
            CCommomControl.unregDevConfigCilent();//向设备配置服务下架客户端信息 

            if (false == save)
            {
                DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(InterpretBase.textTran("您尚未保存工程，是否保存工程？"), InterpretBase.textTran("保存情况"), MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    CReflection.callObjectEvent(this.iconButton_Save, "OnClick", e);
                }
            }
            Module_DeviceManage.Instance.Clear();
        }
        #endregion

        #region 事件:从数据库加载数据
        /// 从数据库加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_LoadDB_Click(object sender, EventArgs e)
        {
            Form_ChooseProject form_ChooseProject = new Form_ChooseProject();
            form_ChooseProject.StartPosition = FormStartPosition.CenterParent;
            form_ChooseProject.ShowDialog();
            if (Form_ChooseProject.dataBaseLoad)
            {
                Module_ViewConfig.Instance.TableName[0] = "Group_1";
                Module_ViewConfig.Instance.TableName[1] = "Group_2";
                Module_ViewConfig.Instance.TableName[2] = "Group_3";
                Module_ViewConfig.Instance.TableName[3] = "Group_4";

                watchHistory = true;
                watchHistoryData(true);
                this.ucLabel_Prj.NewText = "";
                this.ucLabel_Prj.NewText = Module_DeviceManage.Instance.ProjectName;// 刷新工程名
                this.ucLabel_Prj.Refresh();// 刷新工程名

                /* 配色 */
                CControlProp.ButtonBackColor(iconButton_SysConfig, 198, 198, 198);//系统配置按钮变色
                CControlProp.ButtonBackColor(iconButton_WaveView, 243, 243, 243);//波形监测按钮白色
                CControlProp.ButtonBackColor(iconButton_StateView, 243, 243, 243);//运行监测按钮白色
                CControlProp.ButtonBackColor(iconButton_DataBase, 243, 243, 243);//数据存储按钮白色
                CControlProp.ButtonBackColor(iconButton_Cal, 243, 243, 243);//数据存储按钮白色

                CControlProp.ButtonBackColor(iconButton_ConfgDev, 198, 198, 198);//仪器配置按钮变色
                CControlProp.ButtonBackColor(iconButton_ConfgChan, 230, 230, 230);//通道配置按钮白色
                CControlProp.ButtonBackColor(iconButton_ConfigView, 230, 230, 230);//通道显示按钮白色
                CControlProp.ButtonBackColor(iconButton_ConfigRecord, 230, 230, 230);//记录配置按钮白色

                if (m_adminLogin)
                {
                    int[] deviceArray = new int[Module_DeviceManage.Instance.Devices.Count];
                    int[] protocolArray = new int[Module_DeviceManage.Instance.Devices.Count];
                    int[] devRevCmdPortArray = new int[Module_DeviceManage.Instance.Devices.Count];
                    int[] resultArray = new int[Module_DeviceManage.Instance.Devices.Count];
                    int[] devSendDataPortArray = new int[Module_DeviceManage.Instance.Devices.Count];
                    int[] pcSendCmdPortArray = new int[Module_DeviceManage.Instance.Devices.Count];
                    int[] pcRevDataPortArray = new int[Module_DeviceManage.Instance.Devices.Count];
                    int portCount = CPortVerification.GetAvailablePort(Module_DeviceManage.Instance.Devices.Count);
                    if (portCount == -1)//0台设备
                    {
                        /*加载form_DeviceConfig*/
                        form_DeviceConfig.TopLevel = false;
                        form_DeviceConfig.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.panel_Module.Controls.Clear();
                        this.panel_Module.Controls.Add(form_DeviceConfig);
                        form_DeviceConfig.Show();
                        form_DeviceConfig.OpenfileXmlDeviceConfig();//刷新DeviceConfig的值
                        this.panel_Module.Refresh();// 刷新窗体
                        form_DeviceConfig.Refresh();// 刷新窗体
                        /* 顶部状态 */
                        EnableAdmin();

                        form_CreateProject.CreateStatus = false;
                        this.iconMenuItem_CreateProj.Enabled = false;
                        this.toolStripMenuItem_OpenFile.Enabled = false;

                        //form_ChanConfig.dataBaseChanDT(Module_DeviceManage.Instance.GetChanelConfig());
                        Form_ChooseProject.dataBaseLoad = false;
                        //form_ChanConfig.RefeshDelView();
                        form_ViewConfig.LoadViewConfig();
                        //Module_DeviceManage.Instance.InitDeviceDelayTime();
                        SetColorDeepSkyBlue();//天蓝色
                        return;
                    }
                    else if (portCount == -2)
                    {
                        System.Windows.MessageBox.Show(InterpretBase.textTran("无可用端口"));
                        return;
                    }
                    for (int i = 0; i < Module_DeviceManage.Instance.Devices.Count; i++)
                    {
                        deviceArray[i] = IPAddress.Parse(Module_DeviceManage.Instance.Devices[i].IP).GetHashCode();
                        protocolArray[i] = 1;
                        //命令端口
                        devRevCmdPortArray[i] = 5555;
                        pcSendCmdPortArray[i] = 5555;
                        devSendDataPortArray[i] = CPortVerification.PortValid[i * 4];
                        pcRevDataPortArray[i] = CPortVerification.PortValid[i * 4 + 1];

                    }
                    CCommomControl.regDevConfigCilent(deviceArray, protocolArray, devRevCmdPortArray, pcSendCmdPortArray, deviceArray.Length, resultArray);
                    CCommomControl.addDevInfo2DataRecvService(deviceArray, devSendDataPortArray, protocolArray, pcRevDataPortArray, deviceArray.Length, resultArray);
                    //string cmdSetPort = ":wav:sendport " ;
                    //List<byte> sendBuff = new List<byte>();
                    //int[] writeLen = new int[deviceArray.Length];
                    for (int i = 0; i < deviceArray.Length; i++)
                    {
                        Module_DeviceManage.Instance.Devices[i].Status = false;
                        if (resultArray[i] == 0)
                        {
                            Module_DeviceManage.Instance.Devices[i].Status = true;

                            // Module_DeviceManage.Instance.Devices[i].CmdSocket.Connect(Module_DeviceManage.Instance.Devices[i].IP, 5555);
                        }
                        Module_DeviceManage.Instance.Devices[i].HashCode = deviceArray[i];
                        Module_DeviceManage.Instance.Devices[i].CmdSourcePort = devRevCmdPortArray[i];
                        Module_DeviceManage.Instance.Devices[i].CmdDestinationPort = pcSendCmdPortArray[i];
                        Module_DeviceManage.Instance.Devices[i].DataSourcePort = devSendDataPortArray[i];
                        Module_DeviceManage.Instance.Devices[i].DataDestinationPort = pcRevDataPortArray[i];
                        //byte[] buffer = Encoding.Default.GetBytes(cmdSetPort + pcSendCmdPortArray[i]+"\n");
                        //writeLen[i] = buffer.Length;
                        //sendBuff.AddRange(buffer);
                    }
                    //Module_DeviceManage.Instance.InitDeviceDelayTime();
                    //  write2Device(deviceArray, devRevCmdPortArray,deviceArray.Length,sendBuff.ToArray(),writeLen,resultArray);
                }

                /*加载form_DeviceConfig*/
                form_DeviceConfig.TopLevel = false;
                form_DeviceConfig.Dock = System.Windows.Forms.DockStyle.Fill;
                this.panel_Module.Controls.Clear();
                this.panel_Module.Controls.Add(form_DeviceConfig);
                form_DeviceConfig.Show();
                form_DeviceConfig.OpenfileXmlDeviceConfig();//刷新DeviceConfig的值
                this.panel_Module.Refresh();// 刷新窗体
                form_DeviceConfig.Refresh();// 刷新窗体
                /* 顶部状态 */
                EnableAdmin();

                form_CreateProject.CreateStatus = false;
                this.iconMenuItem_CreateProj.Enabled = false;
                this.toolStripMenuItem_OpenFile.Enabled = false;

                //form_ChanConfig.dataBaseChanDT(Module_DeviceManage.Instance.GetChanelConfig());
                Form_ChooseProject.dataBaseLoad = false;
                //form_ChanConfig.RefeshDelView();
                form_ViewConfig.LoadViewConfig();
                //Module_DeviceManage.Instance.InitDeviceDelayTime();
                SetColorDeepSkyBlue();//天蓝色
            }


        }
        private void LoadDeviceManage()
        {
            form_DeviceConfig.OpenfileXmlDeviceConfig();//刷新DeviceConfig的值                                               
            form_CreateProject.CreateStatus = false;
        }

        #endregion

        #region 控件自适应大小
        //控件自适应大小
        private void Form_Main_SizeChanged(object sender, EventArgs e)
        {
            //fAutoSize.formAutoSize(this);
        }
        #endregion

        #region 方法：顶部配色
        public void TopColor(string iconButton)
        {
            switch (iconButton)
            {
                case "SysConfig":
                    CControlProp.ButtonBackColor(iconButton_SysConfig, 198, 198, 198);//系统配置按钮被点击
                    CControlProp.ButtonBackColor(iconButton_WaveView, 243, 243, 243);//波形监测按钮白色
                    CControlProp.ButtonBackColor(iconButton_StateView, 243, 243, 243);//运行监测按钮白色
                    CControlProp.ButtonBackColor(iconButton_DataBase, 243, 243, 243);//数据存储按钮白色
                    CControlProp.ButtonBackColor(iconButton_Cal, 243, 243, 243);//校准按钮白色
                    CControlProp.ButtonBackColor(iconButton_DataComparison, 243, 243, 243);//数据对比按钮白色
                    this.panel_FunctionItem.Width = 118;
                    break;
                case "WaveView":
                    CControlProp.ButtonBackColor(iconButton_SysConfig, 243, 243, 243);//系统配置按钮白色
                    CControlProp.ButtonBackColor(iconButton_WaveView, 198, 198, 198);//波形监测按钮被点击
                    CControlProp.ButtonBackColor(iconButton_StateView, 243, 243, 243);//运行监测按钮白色
                    CControlProp.ButtonBackColor(iconButton_DataBase, 243, 243, 243);//数据存储按钮白色
                    CControlProp.ButtonBackColor(iconButton_Cal, 243, 243, 243);//校准按钮白色
                    CControlProp.ButtonBackColor(iconButton_DataComparison, 243, 243, 243);//数据对比按钮白色
                    this.panel_FunctionItem.Width = 0;
                    break;
                case "StateView":
                    CControlProp.ButtonBackColor(iconButton_SysConfig, 243, 243, 243);//系统配置按钮白色
                    CControlProp.ButtonBackColor(iconButton_WaveView, 243, 243, 243);//波形监测按钮白色
                    CControlProp.ButtonBackColor(iconButton_StateView, 198, 198, 198);//运行监测按钮被点击
                    CControlProp.ButtonBackColor(iconButton_DataBase, 243, 243, 243);//数据存储按钮白色
                    CControlProp.ButtonBackColor(iconButton_Cal, 243, 243, 243);//校准按钮白色
                    CControlProp.ButtonBackColor(iconButton_DataComparison, 243, 243, 243);//数据对比按钮白色
                    this.panel_FunctionItem.Width = 0;
                    break;
                case "DataBase":
                    CControlProp.ButtonBackColor(iconButton_SysConfig, 243, 243, 243);//系统配置按钮白色
                    CControlProp.ButtonBackColor(iconButton_WaveView, 243, 243, 243);//波形监测按钮白色
                    CControlProp.ButtonBackColor(iconButton_StateView, 243, 243, 243);//运行监测按钮白色
                    CControlProp.ButtonBackColor(iconButton_DataBase, 198, 198, 198);//数据存储按钮被点击
                    CControlProp.ButtonBackColor(iconButton_Cal, 243, 243, 243);//校准按钮白色
                    CControlProp.ButtonBackColor(iconButton_DataComparison, 243, 243, 243);//数据对比按钮白色
                    this.panel_FunctionItem.Width = 0;
                    break;
                case "Cal":
                    CControlProp.ButtonBackColor(iconButton_SysConfig, 243, 243, 243);//系统配置按钮白色
                    CControlProp.ButtonBackColor(iconButton_WaveView, 243, 243, 243);//波形监测按钮白色
                    CControlProp.ButtonBackColor(iconButton_StateView, 243, 243, 243);//运行监测按钮白色
                    CControlProp.ButtonBackColor(iconButton_DataBase, 243, 243, 243);//数据存储按钮白色
                    CControlProp.ButtonBackColor(iconButton_Cal, 198, 198, 198);//校准按钮被点击
                    CControlProp.ButtonBackColor(iconButton_DataComparison, 243, 243, 243);//数据对比按钮白色
                    this.panel_FunctionItem.Width = 0;
                    break;
                case "DateComparison":
                    CControlProp.ButtonBackColor(iconButton_SysConfig, 243, 243, 243);//系统配置按钮白色
                    CControlProp.ButtonBackColor(iconButton_WaveView, 243, 243, 243);//波形监测按钮白色
                    CControlProp.ButtonBackColor(iconButton_StateView, 243, 243, 243);//运行监测按钮白色
                    CControlProp.ButtonBackColor(iconButton_DataBase, 243, 243, 243);//数据存储按钮白色
                    CControlProp.ButtonBackColor(iconButton_Cal, 243, 243, 243);//校准按钮白色
                    CControlProp.ButtonBackColor(iconButton_DataComparison, 198, 198, 198);//数据对比按钮白色
                    this.panel_FunctionItem.Width = 0;
                    break;
            }

        }
        #endregion

        #region 方法：左侧栏配色
        public void LeftColor(string iconButtonLeft)
        {
            switch (iconButtonLeft)
            {
                case "ConfgDev":
                    CControlProp.ButtonBackColor(iconButton_ConfgDev, 198, 198, 198);//仪器配置按钮变色
                    CControlProp.ButtonBackColor(iconButton_ConfgChan, 230, 230, 230);//通道配置按钮白色
                    CControlProp.ButtonBackColor(iconButton_ConfigView, 230, 230, 230);//通道显示按钮白色
                    CControlProp.ButtonBackColor(iconButton_ConfigRecord, 230, 230, 230);//记录配置按钮白色
                    CControlProp.SetButtonProp(true, iconButton_ConfgDev);
                    CControlProp.SetButtonProp(false, iconButton_ConfgChan);
                    CControlProp.SetButtonProp(false, iconButton_ConfigView);
                    CControlProp.SetButtonProp(false, iconButton_ConfigRecord);
                    break;
                case "ConfgChan":
                    CControlProp.ButtonBackColor(iconButton_ConfgDev, 230, 230, 230);//通道配置按钮白色
                    CControlProp.ButtonBackColor(iconButton_ConfgChan, 198, 198, 198);//仪器配置按钮变色
                    CControlProp.ButtonBackColor(iconButton_ConfigView, 230, 230, 230);//通道显示按钮白色
                    CControlProp.ButtonBackColor(iconButton_ConfigRecord, 230, 230, 230);//记录配置按钮白色       
                    CControlProp.SetButtonProp(false, iconButton_ConfgDev);
                    CControlProp.SetButtonProp(true, iconButton_ConfgChan);
                    CControlProp.SetButtonProp(false, iconButton_ConfigView);
                    CControlProp.SetButtonProp(false, iconButton_ConfigRecord);
                    break;
                case "ConfigView":
                    /*侧边按钮状态*/
                    CControlProp.ButtonBackColor(iconButton_ConfgDev, 230, 230, 230);//仪器配置按钮白色
                    CControlProp.ButtonBackColor(iconButton_ConfgChan, 230, 230, 230);//通道配置按钮白色
                    CControlProp.ButtonBackColor(iconButton_ConfigView, 198, 198, 198);//通道显示按钮变色
                    CControlProp.ButtonBackColor(iconButton_ConfigRecord, 230, 230, 230);//记录配置按钮白色
                    CControlProp.SetButtonProp(false, iconButton_ConfgDev);
                    CControlProp.SetButtonProp(false, iconButton_ConfgChan);
                    CControlProp.SetButtonProp(true, iconButton_ConfigView);
                    CControlProp.SetButtonProp(false, iconButton_ConfigRecord);
                    break;
                case "ConfigRecord":
                    CControlProp.ButtonBackColor(iconButton_ConfgDev, 230, 230, 230);// 仪器配置按钮白色
                    CControlProp.ButtonBackColor(iconButton_ConfgChan, 230, 230, 230);// 通道配置按钮白色
                    CControlProp.ButtonBackColor(iconButton_ConfigView, 230, 230, 230);// 通道显示按钮白色
                    CControlProp.ButtonBackColor(iconButton_ConfigRecord, 198, 198, 198);// 记录配置按钮变色
                    CControlProp.SetButtonProp(false, iconButton_ConfgDev);
                    CControlProp.SetButtonProp(false, iconButton_ConfgChan);
                    CControlProp.SetButtonProp(false, iconButton_ConfigView);
                    CControlProp.SetButtonProp(true, iconButton_ConfigRecord);
                    break;
            }
        }
        #endregion

        #region 事件：Stop按钮被点击
        /// <summary>
        /// 事件：Stop按钮被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_Stop_Click(object sender, EventArgs e)
        {
            CCommomControl.stopDataRecv();
            // 停止运行监测服务
            //form_StateMonitor.modStateMonitor.devHeartbeatMonitor_Stop();
            // 启用部分窗体的按钮
            TopEnable(true);//主窗体顶部菜单栏部分可点击
            form_DeviceConfig.disableBtn(true);
            form_ChanConfig.disableBtn(true);
            Form_Main.m_adminLogin = true;
            Form_Main.m_vistorLogin = false;
            this.iconButton_prjClose.Enabled = false;
            startReceiveTaskCancel.Cancel();
            startReceiveTaskCancel = new CancellationTokenSource();
            startListenTaskCancel.Cancel();
            startListenTaskCancel = new CancellationTokenSource();
            //this.iconButton_Start.Enabled = true;
            //this.iconButton_Start.IconChar = FontAwesome.Sharp.IconChar.Play;
            //this.iconButton_Start.Text = "Start";

            #region SateMonitor
            //更新运行状态
            form_StateMonitor.m_bRunState = false;
            form_StateMonitor.update_StateMonitorUI();
            //form_StateMonitor.Form_StateRefreshButton(true);
            #endregion

            //单例初始化
            Module_DeviceManage.Instance.GUID = "";

            //清空队列任务
            if (writeDatabseTask == null)
            {
                CFileOperate._tasks.Clear();
            }
            while (CFileOperate._tasks.Count != 0)
            {
                Console.WriteLine("数据还未写完，请稍后");
                Thread.Sleep(500);
            }
            Thread.Sleep(2000);
            this.iconButton_Stop.Enabled = false;
            this.iconButton_Start.Enabled = true;
            this.iconButton_Start.IconChar = FontAwesome.Sharp.IconChar.Play;
            this.iconButton_Start.Text = "Start";

            System.Windows.Forms.MessageBox.Show(InterpretBase.textTran("服务已关闭，数据已写入，内存已清理"));
            ResfreshPanelState(false, false, false);
        }

        #endregion

        #region 事件：默认工程打开
        /// <summary>
        /// 事件：默认工程打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_InitPrj_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 数据库管理用户
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_AddUser_Click(object sender, EventArgs e)
        {
            this.form_UserManager.StartPosition = FormStartPosition.CenterParent;
            this.form_UserManager.ShowDialog();
        }
        #endregion

        #region 关闭窗体事件
        /// <summary>
        /// 事件：关闭窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (false == save && Module_DeviceManage.Instance.Devices.Count > 0 && Form_Main.m_adminLogin)
            {
                MessageBoxResult dialogResult = System.Windows.MessageBox.Show(InterpretBase.textTran("您尚未保存工程，是否保存工程？"), InterpretBase.textTran("保存情况"), MessageBoxButton.YesNoCancel
                    , MessageBoxImage.Warning);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    /* 记录配置的Dc */
                    Dictionary<string, string> RecordConfigDc = form_RecordConfig.RecordConfigDc();
                    if (false == start)
                    {
                        //保存数据到数据库                       
                        Debug.WriteLine("保存项目信息到数据库");
                        Module_DeviceManage.Instance.GUID = "";
                        Module_DeviceManage.Instance.StartTime = DateTime.Now;
                        DbHelperSql.InsertDeviceInfo(Module_DeviceManage.Instance);
                        RecordConfigDc.Add("CollectGUID", Module_DeviceManage.Instance.GetGUID());
                        RecordConfigDc.Add("ProjectGUID", Module_DeviceManage.Instance.ProjectGUID);
                        DbHelperSql.InsertRecordInfo(RecordConfigDc);
                    }
                    CReflection.callObjectEvent(this.iconButton_Save, "OnClick", e);
                }
                else if (dialogResult == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
            startReceiveTaskCancel.Cancel();
            CCommomControl.stopDataRecv();
            pictureBoxClose(0);
            pictureBoxClose(1);
            pictureBoxClose(2);
            pictureBoxClose(3);

            pictureBoxHisClose(0);
            pictureBoxHisClose(1);
            pictureBoxHisClose(2);
            pictureBoxHisClose(3);

            //清空和仪器建立的socket链接
            Parallel.For(0, Module_DeviceManage.Instance.Devices.Count, i =>
            {
                if (Module_DeviceManage.Instance.Devices[i].CmdSocket != null)
                {
                    try
                    {
                        Module_DeviceManage.Instance.Devices[i].CmdSocket.Close();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
            });

            //清空队列任务
            if (writeDatabseTask == null)
            {
                CFileOperate._tasks.Clear();
            }
            Module_DeviceManage.Instance.GUID = "";
            CCommomControl.unregDevConfigCilent();//向设备配置服务下架客户端信息 
            System.Environment.Exit(0);//关闭所有进程                    
        }
        #endregion

        #region 事件：强制触发被点击
        /// <summary>
        /// 事件：强制触发被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconButton_ForceTrig_Click(object sender, EventArgs e)
        {
            //判断是否已经建立连接，如果没有，则建立
            Parallel.For(0, Module_DeviceManage.Instance.Devices.Count, i =>
            {
                if (Module_DeviceManage.Instance.Devices[i].Status == true)
                {
                    if (Module_DeviceManage.Instance.Devices[i].CmdSocket == null)
                    {
                        Module_DeviceManage.Instance.Devices[i].CmdSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        Module_DeviceManage.Instance.Devices[i].CmdSocket.Connect(Module_DeviceManage.Instance.Devices[i].IP, 5555);
                    }
                }
            });
            // 发送记录配置触发命令
            Parallel.For(0, Module_DeviceManage.Instance.Devices.Count, i =>
            {
                if (Module_DeviceManage.Instance.Devices[i].Status == true)
                {
                    if ((form_RecordConfig.m_recordCondition == CGlobalCmd.STR_CMD_SET_TRIG_TFORce))
                    {
                        Module_DeviceManage.Instance.Devices[i].CmdSocket.Send(Encoding.Default.GetBytes(form_RecordConfig.m_recordCondition + "\n"));
                    }
                }
            });
        }
        #endregion

        #region 事件语言转换
        private void ToolStripMenuItem_zhCN_Click(object sender, EventArgs e)
        {
            lang = "zh-CN";
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
            IPret.LoadLanguage(lang);
            IPret.InitLanguage(this);
        }


        private void ToolStripMenuItem_enUS_Click(object sender, EventArgs e)
        {
            lang = "en-US";
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
            IPret.LoadLanguage(lang);
            IPret.InitLanguage(this);
        }
        #endregion

        #region 未使用事件
        /// <summary>
        /// tabControl切页操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (tabControl.SelectedIndex == 0)
            {

            }
            else if (tabControl.SelectedIndex == 1)
            {
                tabControl.SelectedTab.Controls.Add(form_WaveMonitorsList[1]);
                form_WaveMonitorsList[1].Show();
            }
            else if (tabControl.SelectedIndex == 2)
            {
                form_WaveMonitorsList[2].Show();
            }
            else if (tabControl.SelectedIndex == 3)
            {
                form_WaveMonitorsList[3].Show();
            }*/
        }
        #endregion

        #region 事件：开关数据库
        private void iconMenuItem_OpenSaveDb_Click(object sender, EventArgs e)
        {
            saveDateBase = true;
            this.iconMenuItem_OpenSaveDb.IconColor = Color.Green;
            this.iconMenuItem_CloseSaveDb.IconColor = Color.Gray;
        }
        private void iconMenuItem_CloseSaveDb_Click(object sender, EventArgs e)
        {
            saveDateBase = false;
            this.iconMenuItem_OpenSaveDb.IconColor = Color.Gray;
            this.iconMenuItem_CloseSaveDb.IconColor = Color.Red;
        }
        #endregion

        #region 方法：按钮全变天蓝
        public void SetColorDeepSkyBlue()
        {
            if (Form_Main.m_vistorLogin)
            {
                this.iconButton_SysConfig.IconColor = Color.LightSeaGreen;
            }
            else
            {
                this.iconButton_SysConfig.IconColor = Color.LightSeaGreen;
                this.iconButton_StateView.IconColor = Color.LightSeaGreen;
                this.iconButton_WaveView.IconColor = Color.LightSeaGreen;
                this.iconButton_Cal.IconColor = Color.LightSeaGreen;
            }

        }
        #endregion

        #region 事件：关于此软件
        private void iconMenuItem3_Click(object sender, EventArgs e)
        {
            form_AppInfo.ShowDialog();
        }
        #endregion

        #region 事件：项目信息窗体
        private void iconMenuItem1_Click(object sender, EventArgs e)
        {
            form_ProjectInfo.ShowDialog();
        }
        #endregion

        #region 方法：顶部菜单栏是否可以点击使用
        public void TopEnable(bool Enable)
        {
            if (this.InvokeRequired)
            {
                refresh_Top s = new refresh_Top(TopEnable);
                this.Invoke(s, Enable);
            }
            else
            {
                this.ToolStripMenuItem_SaveDataBase.Enabled = Enable;
                this.ToolStripMenuItemTranLang.Enabled = Enable;
                this.ToolStripMenuItem_AddUser.Enabled = Enable;
            }

        }

        public void startEnable(string Enable)
        {
            if (this.InvokeRequired)
            {
                refresh_Start s = new refresh_Start(startEnable);
                this.Invoke(s, Enable);
            }
            else
            {
                if (Enable == "DevCalClick")
                {
                    this.iconButton_Start.Enabled = false;
                }
                else if (Enable == "DevCalNoClick")
                {
                    this.iconButton_Start.Enabled = true;
                }
            }
        }
        #endregion

        //帮助手册打开
        private void iconMenuItem2_Click(object sender, EventArgs e)
        {
            string str = AppDomain.CurrentDomain.BaseDirectory + "HelpDocument\\DAQ30075A_HelpDocument_CN.chm";
            if (System.IO.File.Exists(str))
            {
                try
                {
                    System.Diagnostics.Process.Start("IEXPLORE.exe", str);
                }
                catch (Exception)
                {

                }
                RunCmd(""+str+"", out str);
                //System.Diagnostics.Process.Start(str);
                //Help.ShowHelp(null, @str);
                //System.Windows.Forms.Help.ShowHelp(this, @str);
                //System.IO.File.Open(str,FileMode.Open);
            }

        }

        public void RunCmd(string cmd, out string output)
        {
            string CmdPath = @"C:\Windows\System32\cmd.exe";
            cmd = cmd.Trim().TrimEnd('&') + "&exit";//说明：不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状态
            using (Process p = new Process())
            {
                p.StartInfo.FileName = CmdPath;
                p.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;          //不显示程序窗口
                p.Start();//启动程序
                //向cmd窗口写入命令
                p.StandardInput.WriteLine(cmd);
                p.StandardInput.AutoFlush = true;
                //获取cmd窗口的输出信息
                output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();//等待程序执行完退出进程
                p.Close();
            }
        }
    }
}
