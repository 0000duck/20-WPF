using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using saker_Winform.SubForm;
using saker_Winform.CommonBaseModule;
using saker_Winform.UserControls;
using System.Threading;
using System.Net.Sockets;
using saker_Winform.Global;
using System.Net;
using ClassLibrary_SocketServer;
using System.Diagnostics;

namespace saker_Winform.Module
{
    public class Module_StateMonitor
    {

        #region Fields
        /// <summary>
        /// 定义设备相关的信息
        /// </summary>
        public class devSysInfo
        {
            public string strDevName;
            public string strDevSN;
            public string strIP;
            public string strVirtNum;//机架编号
            public string[] arrChanTag = new string[4];
            public bool bOnline = false;
            public int offLineNum = 0;
        }
        public class devParaInfo
        {
            public string strTimBase;
            public string strTimOffset;
            public string strAcq;
            public string strMemDepth;
            public string strTrigState;
            public chanInfo[] arrChanParaInfo = new chanInfo[4];
        }
        public struct chanInfo
        {
            public bool bOpen;//通道打开状态
            public bool bImpedance;//通道阻抗状态
        }
        /// <summary>
        /// 信息链表
        /// </summary>
        public List<devSysInfo> devSysInfoList = new List<devSysInfo>();
        //用户控件列表
        public List<UCDevStates> ucDevStatesList = new List<UCDevStates>();
        //定义任务的最大并发数
        private const int taskMax = 75;

        //定义接受服务端用于仪器心跳监听
        private SocketManager tcpServerHeartBeat = new SocketManager(taskMax, 20,true);
        IPEndPoint checkEndPoint =  new IPEndPoint(IPAddress.Any, 22222);
        //定时器
        System.Timers.Timer timListen =  new System.Timers.Timer(1000);
        private int tickCountInStep = 0;
        private const int tickCount = 3;


        #endregion
        #region Construction
        /// <summary>
        /// 构造函数
        /// </summary>
        public Module_StateMonitor()
        { }
        #endregion

        #region Events

        #region 自定义事件
        /*定义事件参数类*/
        public class stateMonitorEventArgs : EventArgs
        {
            public readonly string KeyToRaiseEvent;
            public stateMonitorEventArgs(string keyToRaiseEvent)
            {
                KeyToRaiseEvent = keyToRaiseEvent;
            }
        }
        //定义delegate委托申明
        public delegate void stateMonitorEventHandler(object sender, stateMonitorEventArgs e);
        //用event 关键字声明事件对象
        public event stateMonitorEventHandler stateMonitorEvent;
        //事件触发方法
        protected virtual void onStateMonitorEvent(stateMonitorEventArgs e)
        {
            if (stateMonitorEvent != null)
                stateMonitorEvent(this, e);
        }
        //引发事件
        private void RaiseEvent(string keyToRaiseEvent)
        {
            stateMonitorEventArgs e = new stateMonitorEventArgs(keyToRaiseEvent);
            onStateMonitorEvent(e);
        }
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// 启动心跳监听的服务
        /// </summary>
        public void devHeartbeatMonitor_Init()
        {
            
            //tcpServerHeartBeat = new SocketManager(taskMax, 20,true);
            tcpServerHeartBeat.Init();
            //tcpServerHeartBeat.ReceiveData += checkOnline;
            //tcpServerHeartBeat.Start(checkEndPoint);
            //启动轮训的定时器
            //timListen = new System.Timers.Timer(1000);
            
            timListen.Enabled = true;
            timListen.AutoReset = true;
            //timListen.Elapsed += new System.Timers.ElapsedEventHandler(CheckListen);
            //timListen.Elapsed += CheckListen;
            //timListen.Start();
        }

        public void devHeartbeatMonitor_Stop()
        {
            //tcpServerHeartBeat.Stop();
            tcpServerHeartBeat.ReceiveData -= checkOnline;
            timListen.Stop();
            timListen.Elapsed -= CheckListen;
            
        }

        public void devHeartbeatMonitor_Start()
        {
            tcpServerHeartBeat.ReceiveData += checkOnline;
            tcpServerHeartBeat.Start(checkEndPoint);
            timListen.Elapsed += CheckListen;
            timListen.Start();
        }
        /// <summary>
        /// 定时器超时回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckListen(object sender, System.Timers.ElapsedEventArgs e)
        {
            tickCountInStep++;
            foreach (devSysInfo item in devSysInfoList)
            {
                //在线就自加
                if (item.bOnline == true)
                {
                    item.offLineNum++;
                }
            }
            //到达超时时间
            if (tickCountInStep >= tickCount)
            {
                //var disconnectedUsers = devSysInfoList.Where(p => p.bOnline == false ).ToList();
                foreach (devSysInfo item in devSysInfoList)
                {
                    //item.offLineNum++;
                    if (item.offLineNum >= 6)
                    {
                        //机器掉线
                        item.offLineNum = 0;
                        item.bOnline = false;
                        //查找到用户控件中的掉线机器
                        int index = ucDevStatesList.FindIndex(ex => ex.m_strDevSN == item.strDevSN);
                        if (index != -1)
                        {
                            if (Module_DeviceManage.Instance.GetDeviceBySN(item.strDevSN) != null)
                            {
                                //更新掉线状态
                                if (Module_DeviceManage.Instance.GetDeviceBySN(item.strDevSN).Status == true)
                                {
                                    ucDevStatesList[index].updateOnlineState(false);
                                    Module_DeviceManage.Instance.GetDeviceBySN(item.strDevSN).Status = false;
                                    //发送更新事件
                                    RaiseEvent(item.strDevSN);
                                }
                            }
                        }
                    }
                }
                tickCountInStep = 0;
            }
        }
        /// <summary>
        /// 监测仪器的在线状态-回调
        /// </summary>
        /// <param name="token"></param>
        /// <param name="data"></param>
        private void checkOnline(AsyncUserToken token)
        {
            Thread task = new Thread(() =>
                {
                    byte[] data = token.Buffer.ToArray();                  
                    token.Buffer.Clear();
                    string strReturn = Encoding.ASCII.GetString(data, 0, data.Length);
                    Console.WriteLine(strReturn);
                    if (strReturn == "")
                    {
                        return;
                    }
                //返回数据格式为SN+触发状态
                string[] arr = strReturn.Split(',');

                //strReturn = strReturn.Substring(0, 13);
                int index = devSysInfoList.FindIndex(item => (item.strDevSN == arr[0]));
                if (index != -1)
                {

                    devSysInfoList[index].bOnline = true;
                    devSysInfoList[index].offLineNum = 0;
                    //ip发生更改
                    if (devSysInfoList[index].strIP != token.IPAddress.ToString())
                    {
                        //ip改变后需要通知其他窗体
                        if (Module_DeviceManage.Instance.GetDeviceBySN(arr[0]) != null)
                        {
                            Module_DeviceManage.Instance.GetDeviceBySN(arr[0]).IP = token.IPAddress.ToString();//将管理界面仪器的IP进行更改
                        }
                        //发送更新事件
                        RaiseEvent(arr[0]);
                        devSysInfoList[index].strIP = token.IPAddress.ToString();
                    }
                    //找到的用户控件中对应的单元
                    int indexUC = ucDevStatesList.FindIndex(item => (item.m_strDevSN == arr[0]));
                    if (indexUC != -1)
                    {
                        ucDevStatesList[indexUC].m_strDevIP = token.IPAddress.ToString();
                        //更新在线状态
                        if (Module_DeviceManage.Instance.GetDeviceBySN(arr[0]).Status == false)
                        //if (ucDevStatesList[indexUC].m_bConnect == false)
                        {
                            Module_DeviceManage.Instance.GetDeviceBySN(arr[0]).Status = true;
                            ucDevStatesList[indexUC].m_bConnect = true;
                            ucDevStatesList[indexUC].updateOnlineState(true);
                            //发送更新事件
                            RaiseEvent(arr[0]);
                        }

                        //更新触发状态     
                        ucDevStatesList[indexUC].updateTrigState(arr[1]);

                            //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            //try
                            //{
                            //    socket.Connect(token.IPAddress.ToString(), 5555);
                            //    //水平方向参数命令
                            //    string command = CGlobalCmd.STR_CMD_GET_TRIGGERSTATUS + "\n";
                            //    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 200);
                            //    socket.Send(Encoding.Default.GetBytes(command));
                            //    byte[] byteReadBuf = new byte[128];
                            //    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 200);
                            //    var retCount = socket.Receive(byteReadBuf);
                            //    string result = Encoding.Default.GetString(byteReadBuf, 0, retCount);
                            //    result = result.Remove(result.Length - 1);//移除\n字符
                            //    ucDevStatesList[indexUC].updateTrigState(result);
                            //}
                            //catch (Exception e)
                            //{
                            //    socket.Close();
                            //}
                            //finally
                            //{
                            //    socket.Close();
                            //}
                        }
                    }
                });
            task.Start();
        }
        /// <summary>
        /// 加载设备系统信息
        /// </summary>
        public void devSysInfoInit()
        {
            devSysInfoList.Clear();
            foreach (Module_Device item in Module_DeviceManage.Instance.Devices)
            {
                //初始化设备信息
                devSysInfo devInfoTemp = new devSysInfo();
                devInfoTemp.strDevName = item.Name;
                devInfoTemp.strDevSN = item.SN;
                devInfoTemp.strIP = item.IP;
                devInfoTemp.strVirtNum = item.VirtualNumber;
                devInfoTemp.arrChanTag[0] = item.GetChannel(1).Tag;
                devInfoTemp.arrChanTag[1] = item.GetChannel(2).Tag;
                devInfoTemp.arrChanTag[2] = item.GetChannel(3).Tag;
                devInfoTemp.arrChanTag[3] = item.GetChannel(4).Tag;
                devSysInfoList.Add(devInfoTemp);
            }
        }
        /// <summary>
        /// list排序
        /// </summary>
        public void ucDevStatesList_SortByVirtNum()
        {
            ucDevStatesList = ucDevStatesList.OrderBy(item => item.m_strVirtNum).ToList() ;
        }
        /// <summary>
        /// 用户控件初始化
        /// </summary>
        /// <param name="bBlock">是否阻塞标志</param>
        public void ucDevStatesInit(bool bBlock)
        {
            ucDevStatesList.Clear();//清楚用户控件列表
            CTaskScheduler.LimitedConcurrencyLevelTaskScheduler lcts = new CTaskScheduler.LimitedConcurrencyLevelTaskScheduler(taskMax);
            List<Task> tasks = new List<Task>();
            // Create a TaskFactory and pass it our custom scheduler. 
            TaskFactory factory = new TaskFactory(lcts);
            CancellationTokenSource cts = new CancellationTokenSource();
            // Use our factory to run a set of tasks. 
            Object lockObj = new Object();
            for (int i = 0; i < devSysInfoList.Count; i++)
            {
                int iteration = i;
                Task t = factory.StartNew(() =>
                    {
                        bool bReturn = true;
                        for (int j = 0; j < 4; j++)
                        {
                            bReturn = CPingIP.PingIpConnect(devSysInfoList[iteration].strIP);
                            if (bReturn == true)
                            {
                                break;
                            }
                            Thread.Sleep(50);
                        }
                        if (bReturn)
                        {
                            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            try
                            {
                                socket.Connect(devSysInfoList[iteration].strIP, 5555);
                                //水平方向参数命令
                                string command = CGlobalCmd.STR_CMD_GET_TRIGGERSTATUS + ";"
                                    + CGlobalCmd.STR_CMD_GET_HORIZONTALTIMEBASE + ";"
                                    + CGlobalCmd.STR_CMD_GET_HORIZONTAOFFSET + ";"
                                    + CGlobalCmd.STR_CMD_GET_SAMPLERATE + ";"
                                    + CGlobalCmd.STR_CMD_GET_MEMDEPTH + "\n";
                                //socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 10000);
                                socket.Send(Encoding.Default.GetBytes(command));
                                Thread.Sleep(50);
                                byte[] byteReadBuf = new byte[128];
                                //socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 10000);
                                var retCount = socket.Receive(byteReadBuf);
                                string result = Encoding.Default.GetString(byteReadBuf, 0, retCount);
                                result = result.Remove(result.Length - 1);//移除\n字符
                                string[] arr = result.Split(';');
                                devParaInfo paraInfoTemp = new devParaInfo();
                                UCDevStates ucDevTemp = new UCDevStates();

                                ucDevTemp.m_bConnect = true;
                                ucDevTemp.m_strDevSN = devSysInfoList[iteration].strDevSN;
                                Module_DeviceManage.Instance.GetDeviceBySN(devSysInfoList[iteration].strDevSN).Status = true;
                                Module_DeviceManage.Instance.GetDeviceBySN(devSysInfoList[iteration].strDevSN).IsOnTesting = true;
                                Module_DeviceManage.Instance.GetDeviceBySN(devSysInfoList[iteration].strDevSN).CmdSocket.Close();
                                Module_DeviceManage.Instance.GetDeviceBySN(devSysInfoList[iteration].strDevSN).CmdSocket = null;
                                ucDevTemp.m_strDevIP = devSysInfoList[iteration].strIP;
                                ucDevTemp.m_strDevSubName = devSysInfoList[iteration].strDevName;
                                ucDevTemp.m_strVirtNum = devSysInfoList[iteration].strVirtNum;

                                paraInfoTemp.strTrigState = arr[0];
                                ucDevTemp.m_strTrigState = arr[0];
                                paraInfoTemp.strTimBase = CValue2String.time2String(Convert.ToDouble(arr[1]));
                                ucDevTemp.m_strTimBase = paraInfoTemp.strTimBase;
                                paraInfoTemp.strTimOffset = CValue2String.time2String(Convert.ToDouble(arr[2]));
                                ucDevTemp.m_strTimOffset = paraInfoTemp.strTimOffset;
                                paraInfoTemp.strAcq = CValue2String.acq2String((float)Convert.ToDouble(arr[3]));
                                ucDevTemp.m_strAcq = paraInfoTemp.strAcq;
                                paraInfoTemp.strMemDepth = CValue2String.meDepth2String((float)Convert.ToDouble(arr[4]));
                                ucDevTemp.m_strMeDepth = paraInfoTemp.strMemDepth;


                                for (int j = 1; j <= 4; j++)
                                {
                                    //通道参数命令
                                    command = CGlobalCmd.STR_CMD_GET_CHAN_STATE + ";"
                                        + CGlobalCmd.STR_CMD_GET_IMPENDENCE + "\n";
                                    command = command.Replace("<n>", j.ToString());
                                    socket.Send(Encoding.Default.GetBytes(command));
                                    Thread.Sleep(50);
                                    retCount = socket.Receive(byteReadBuf);
                                    result = Encoding.Default.GetString(byteReadBuf, 0, retCount);
                                    result = result.Remove(result.Length - 1);//移除\n字符
                                    arr = result.Split(';');
                                    if (arr[0] == "1")
                                    {
                                        paraInfoTemp.arrChanParaInfo[j - 1].bOpen = true;
                                    }
                                    else
                                    {
                                        paraInfoTemp.arrChanParaInfo[j - 1].bOpen = false;
                                    }
                                    if (arr[1].Contains("FIFT"))
                                    {
                                        paraInfoTemp.arrChanParaInfo[j - 1].bImpedance = false;
                                    }
                                    else
                                    {
                                        paraInfoTemp.arrChanParaInfo[j - 1].bImpedance = true;
                                    }
                                    switch (j)
                                    {
                                        case 1:
                                            ucDevTemp.setCH1Info(devSysInfoList[iteration].arrChanTag[0],
                                                paraInfoTemp.arrChanParaInfo[j - 1].bOpen,
                                                paraInfoTemp.arrChanParaInfo[j - 1].bImpedance);
                                            break;
                                        case 2:
                                            ucDevTemp.setCH2Info(devSysInfoList[iteration].arrChanTag[1],
                                                paraInfoTemp.arrChanParaInfo[j - 1].bOpen,
                                                paraInfoTemp.arrChanParaInfo[j - 1].bImpedance);
                                            break;
                                        case 3:
                                            ucDevTemp.setCH3Info(devSysInfoList[iteration].arrChanTag[2],
                                                paraInfoTemp.arrChanParaInfo[j - 1].bOpen,
                                                paraInfoTemp.arrChanParaInfo[j - 1].bImpedance);
                                            break;
                                        case 4:
                                            ucDevTemp.setCH4Info(devSysInfoList[iteration].arrChanTag[3],
                                                paraInfoTemp.arrChanParaInfo[j - 1].bOpen,
                                                paraInfoTemp.arrChanParaInfo[j - 1].bImpedance);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                lock (lockObj)
                                {
                                    //链表增加元素
                                    ucDevStatesList.Add(ucDevTemp);
                                }
                            }
                            catch (Exception e)
                            {
                                Module_DeviceManage.Instance.GetDeviceBySN(devSysInfoList[iteration].strDevSN).Status = false;
                                Module_DeviceManage.Instance.GetDeviceBySN(devSysInfoList[iteration].strDevSN).IsOnTesting = false;
                                socket.Close();
                                Debug.WriteLine("空间名：" + e.Source + "；" + '\n' +
                                      "方法名：" + e.TargetSite + '\n' +
                                      "故障点：" + e.StackTrace.Substring(e.StackTrace.LastIndexOf("\\") + 1, e.StackTrace.Length - e.StackTrace.LastIndexOf("\\") - 1) + '\n' +
                                      "错误提示：" + e.Message);
                            }
                            finally
                            {

                                socket.Close();
                            }
                        }
                        else
                        {
                            UCDevStates ucDevTemp = new UCDevStates();
                            ucDevTemp.m_bConnect = false;
                            ucDevTemp.m_strDevSN = devSysInfoList[iteration].strDevSN;
                            Module_DeviceManage.Instance.GetDeviceBySN(devSysInfoList[iteration].strDevSN).Status = false;
                            ucDevTemp.m_strDevIP = devSysInfoList[iteration].strIP;
                            ucDevTemp.m_strDevSubName = devSysInfoList[iteration].strDevName;
                            ucDevTemp.m_strVirtNum = devSysInfoList[iteration].strVirtNum;
                            ucDevTemp.updateOnlineState(false);
                            lock (lockObj)
                            {
                                //链表增加元素
                                ucDevStatesList.Add(ucDevTemp);
                            }
                        }
                        

                    }, cts.Token);
                tasks.Add(t);
            }
            if (bBlock)
            {
                Task.WaitAll(tasks.ToArray());
            }
            cts.Dispose();
            foreach (Module_Device device in Module_DeviceManage.Instance.Devices)
            {
                RaiseEvent(device.SN);
            }
            
        }
        #endregion

    }
}
