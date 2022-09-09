using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary_SocketServer;
using saker_Winform.Module;
using saker_Winform.UserControls;
using Log.Core;
using System.Runtime.CompilerServices;
using System.ComponentModel.Design;

namespace saker_Winform.CommonBaseModule
{
    public class CFileOperate
    {
        // 任务队列
        public static Queue<Module_Device> _tasks = new Queue<Module_Device>();
        // 为保证线程安全，使用一个锁来保护_task的访问
        readonly static object _locker = new object();
        // 通过 _wh 给工作线程发信号
        public static EventWaitHandle _wh = new AutoResetEvent(false);
        public static Thread _worker;
        public static byte[] Zero = new byte[1000008];
        public static byte[] TenZero = new byte[10];
        public static int DequeueSize = 0;
        public static int ErrorQueueSize = 0;
        /* 执行脚本关闭11111端口的PID进程 */
        //public static void closePort()
        //{
        //    Process proc = null;
        //    try
        //    {
        //        string targetDir = string.Format(System.IO.Directory.GetCurrentDirectory());//.bat文件所在的文件夹
        //        proc = new Process();
        //        proc.StartInfo.WorkingDirectory = targetDir;
        //        proc.StartInfo.FileName = "Close1111Port.bat";
        //        proc.StartInfo.Arguments = string.Format("20");
        //        //proc.StartInfo.CreateNoWindow = true;
        //        proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;//设置DOS窗口不显示
        //        proc.Start();
        //        proc.WaitForExit();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString());
        //    }
        //}
        public static void WriteDataToFile(AsyncUserToken token, byte[] data)              //带一个参数的委托函数  
        {
            /*csv文件操作*/
            string path = "E:\\WaveData\\";
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            string filename = token.IPAddress.ToString().Replace(".", string.Empty) + "_" + (DateTime.Now.Ticks / 1000).ToString() + ".csv";
            path = path + filename;
            FileStream csvFs = new FileStream(path, FileMode.Create);
            csvFs.Write(data, 0, data.Length);
            csvFs.Flush();
            //关闭流
            csvFs.Close();
        }      
        //读取数据到内存
        public static void WriteDataToMemory(AsyncUserToken token)
        {
                    
                Thread task = new Thread(() =>
                {
                    try
                    {                      
                        string ip = token.IPAddress.ToString();
                        Debug.WriteLine(ip + " " + token.Buffer.Count);
                        //解析数据并且赋值                       
                        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        socket.Connect(ip, 5555);
                        string command = Global.CGlobalCmd.STR_CMD_GET_WAVEPARAS+"\n" ;
                        byte[] paras = new byte[1024];
                        socket.Send(Encoding.Default.GetBytes(command));
                        int ret = socket.Receive(paras);
                        string result = Encoding.Default.GetString(paras, 0, ret);
                        socket.Close();
                       
                       
                        if (token.Buffer.Count == 4000042)
                        {
                            //提取通道精细延时
                            byte[] timeInfo = new byte[10];
                            Buffer.BlockCopy(token.Buffer.ToArray(), 1000 * 1000 * 4 + 8 * 4, timeInfo, 0, 10);                       
                            //提取波形数据
                            for (int i = 0; i < 4; i++)
                            {                                
                                Module.Module_DeviceManage.Instance.GetDeviceByIP(ip).GetChannel(i + 1).SetData(token.Buffer.ToArray(), i * (1000 * 1000 + 8), 1000 * 1000 + 8);
                                Module.Module_DeviceManage.Instance.GetDeviceByIP(ip).GetChannel(i + 1).ModifiedTime = DateTime.Now;
                                //将数据插入到写入数据库的队列中
                               // EnqueueTask(new CReceiveData(token, i * (1000 * 1000 + 8), 1000 * 1000 + 8, Module.Module_DeviceManage.Instance.GetDeviceByIP(ip).GUID, Module.Module_DeviceManage.Instance.GetDeviceByIP(ip).GetChannel(i + 1), Module.Module_DeviceManage.Instance.WaveTableName, Module.Module_DeviceManage.Instance.StartTime));
                                Debug.WriteLine(ip + " 数据写入内存 " + DateTime.Now.ToString("hh:mm:ss fff"));
                            }
                        }
                        else
                        {
                           
                         //   Module.Module_DeviceManage.Instance.GetDeviceByIP(ip).TrigTimeStamp = 0;
                            for (int i = 0; i < 4; i++)
                            {                  
                                //提取通道延时，每个通道一个值
                          //      Module.Module_DeviceManage.Instance.GetDeviceByIP(ip).GetChannel(i + 1).ChannelDelayTime = 0;
                                //完成每个通道的波形数据的解析赋值                                                    
                                Module.Module_DeviceManage.Instance.GetDeviceByIP(ip).GetChannel(i + 1).ModifiedTime = DateTime.Now;
                                //将数据插入到写入数据库的队列中
                                //EnqueueTask(new CReceiveData(token, i * (1000 * 1000 + 8), 1000 * 1000 + 8, Module.Module_DeviceManage.Instance.GetDeviceByIP(ip).GUID, Module.Module_DeviceManage.Instance.GetDeviceByIP(ip).GetChannel(i + 1), Module.Module_DeviceManage.Instance.WaveTableName, Module.Module_DeviceManage.Instance.StartTime));
                                Debug.WriteLine(ip + " 数据写入内存 " + DateTime.Now.ToString("hh:mm:ss fff"));
                            }
                        }
                        Module.Module_DeviceManage.Instance.GetDeviceByIP(ip).IsComplete = true;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("空间名：" + ex.Source + "；" + '\n' +
                          "方法名：" + ex.TargetSite + '\n' +
                          "故障点：" + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf("\\") + 1, ex.StackTrace.Length - ex.StackTrace.LastIndexOf("\\") - 1) + '\n' +
                          "错误提示：" + ex.Message);
                    }
                });
                task.Start();
                                 
        }     
        public static void WriteDataToDatabase(Module_Device task) //带一个参数的委托函数  
        {
            //更新波形数据表，将最新的波形数据插入到数据库中           
            string ip = task.IP;
            string deviceGuid = task.GUID;
            string waveTableName = Module_DeviceManage.Instance.WaveTableName;
            string channelGuid = "";          
            string startTime = Module_DeviceManage.Instance.StartTime.ToString();
            string sql = "";
            int id = 0;                   
            int TrigTimeStamp = (task.Para10Byte[8] << 8) |(task.Para10Byte[9]);
            for (int i = 1; i <= 4; i++)
            {
                if (task.GetChannel(i).Collect == true)
                {
                    int ChannelDelayTime = task.Para10Byte[(i - 1) * 2] << 8 | task.Para10Byte[(i - 1) * 2 + 1];
                    channelGuid = task.GetChannel(i).GUID;                   
                    sql = "INSERT INTO " + waveTableName + " (GUID, DeviceGUID, ChannelGUID, Data, Location,IsWholeComplete,StartTime,CreateTime,TrigTimeStamp,ChanDelayTime)VALUES(NEWID(), '{0}', '{1}', @WaveData,'{2}','{3}', '{4}',GETDATE(),'{5}','{6}')";
                    sql = string.Format(sql, deviceGuid, channelGuid, "", task.IsWholeComplete, startTime, TrigTimeStamp, ChannelDelayTime);
                    DataBase.DbHelperSql.ExecuteSqlInsertImg(sql, task.GetChannel(i).Data);                                 
                    id = Thread.CurrentThread.ManagedThreadId;
                    Debug.WriteLine("线程：" + id + " 写入来自 " + ip + " 的数据到数据库完成 " + DateTime.Now.ToString("hh:mm:ss fff"));                  
                }               
            }          
            
        }
        /// <summary>
        /// 将波形数据读取到文件中保存，并且第一行保存文件参数,设备名，通道数，第二行保存文件波形参数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="module_WaveInfo"></param>    
        public static void WriteDataToLocal(string path,Module_WaveInfo module_WaveInfo)
        {
            //string startTime = module_WaveInfo.StartTime.Replace(":","").Replace(" ","").Replace("-","").Replace("/","");
            DateTime dateTimeStart = Convert.ToDateTime(module_WaveInfo.StartTime);
            string startTime = dateTimeStart.ToString("yyyyMMddHHmmss");
            string projectName = module_WaveInfo.ProjectName;
            DateTime dateTimeRecord = Convert.ToDateTime(module_WaveInfo.RecordTime);
            string recordTime = dateTimeRecord.ToString("yyyyMMddHHmmss");
            //string recordTime = module_WaveInfo.RecordTime.Replace(":", "").Replace(" ", "").Replace("-", "").Replace("/","");
            string filePath = path + "//"+ projectName+ "//" + recordTime +"//"+startTime;
            
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fileName = module_WaveInfo.Tag;
            /*csv文件操作*/
            path = filePath + "//"+fileName+".csv";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            string deviceInfo = "设备名称:"+module_WaveInfo.DeviceName +" 通道号:"+module_WaveInfo.ChannelID;
            string para =
               " Xincrement =" + module_WaveInfo.XIncrement.ToString() +
               " Xreference =" + module_WaveInfo.XReference.ToString() +
               " Xorigin =" + module_WaveInfo.XOrigin.ToString() +
               " Yincrement =" + module_WaveInfo.YIncrement.ToString() +
               " Yreference =" + module_WaveInfo.YReference.ToString() +
               " Yorigin =" + module_WaveInfo.YOrigin.ToString() +
               " DelayTime=" + module_WaveInfo.DeviceDelayTime +
               " ChannelDelayTime=" + module_WaveInfo.ChannelDelayTime +
               " TrigTimeStamp=" + module_WaveInfo.TrigTimeStamp;          
            using (FileStream csvFs = new FileStream(path, FileMode.Create)) {
                using (StreamWriter csvSw = new StreamWriter(csvFs, System.Text.Encoding.Default))
                {
                    int i = 0;
                    csvSw.WriteLine(deviceInfo);
                    csvSw.WriteLine(para);
                    while (i < module_WaveInfo.Data.Length)
                    {                      
                        csvSw.WriteLine(module_WaveInfo.Data[i]);
                        i++;
                    }                  
                    csvSw.Flush();
                    //关闭流
                    csvSw.Close();
                    csvFs.Close();
                }              
            }           
        }
        public static void Work()
        {
            Module_Device work = null;
            while (true)
            {           
                lock (_locker)
                {
                    if (_tasks.Count > 0)
                    {
                        work = _tasks.Dequeue(); // 有任务时，出列任务
                        if (work.IsWholeComplete == false)
                        {
                            ErrorQueueSize++;
                        }
                        DequeueSize++;                      
                        if (work == null)  // 退出机制：当遇见一个null任务时，代表任务结束
                            return;
                    }
                }
                if (work != null)
                {
                    WriteDataToDatabase(work);  // 任务不为null时，处理并保存数据
                    work = null;
                }
                else
                {
                    _wh.WaitOne();   // 没有任务了，等待信号
                }  
            }
        }

        /// <summary>插入任务</summary>
        public static void EnqueueTask(Module_Device task)
        {
            lock (_locker)
            {
                _tasks.Enqueue(task);  // 向队列中插入任务                      
            }             
            _wh.Set();  // 给工作线程发信号
        }

        /// <summary>结束释放</summary>
        public static void ThreadDispose()
        {
            EnqueueTask(null);      // 插入一个Null任务，通知工作线程退出
            _worker.Join();         // 等待工作线程完成
            _wh.Close();            // 释放资源
        }

        #region 广播
        /// <summary>
        /// 广播当前广播域内的仪器，返回IP和序列号
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> Broadcast()
        {

            Dictionary<string, string> devices = new Dictionary<string, string>();/*用于存放广播到仪器的IP地址和序列号*/
            byte[] as8FindCmd = Encoding.ASCII.GetBytes("*?"); /*设备发现命令*/
            byte[] as8DeviceRspData = new byte[1024];
            string ipAddress = "";
            string subnetMask = "";        
            List<string> listIP = new List<string>();
            List<string> listIPSubnet = new List<string>();
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection nics = mc.GetInstances();
            foreach (ManagementObject nic in nics)
            {
                string mServiceName = nic["ServiceName"] as string;

                /* 过滤非真实网卡并且取IPV4 */
                if (!(bool)nic["IPEnabled"])
                { continue; }
                if (mServiceName.ToLower().Contains("vmnetadapter")
                 || mServiceName.ToLower().Contains("ppoe")
                 || mServiceName.ToLower().Contains("bthpan")
                 || mServiceName.ToLower().Contains("tapvpn")
                 || mServiceName.ToLower().Contains("ndisip")
                 || mServiceName.ToLower().Contains("sinforvnic"))
                { continue; }

                string[] mIPAddress = nic["IPAddress"] as string[];
                string[] mIPSubnet = nic["IPSubnet"] as string[];
                // 添加IP
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
                // 添加子网掩码
                if(mIPSubnet != null)
                {
                    foreach (string item in mIPSubnet)
                    {
                        if(item != "0.0.0.0")
                        {
                            listIPSubnet.Add(item);
                        }
                    }
                }
            }
            // 查询IP
            foreach (var item in listIP)
            {
                if (!item.Contains(":"))
                { ipAddress = item.ToString(); }      
            }
            // 查询子网掩码
            foreach (var item in listIPSubnet)
            {
                if (item.Contains("."))
                { subnetMask = item.ToString(); }
            }

            mc.Dispose();
            string broadcastAddress = GetBroadcast(ipAddress, subnetMask);
            IPEndPoint FindServiceLocalIp = new IPEndPoint(System.Net.IPAddress.Any, 1625); /*设备发现服务本地端口定义*/
            IPAddress Broadcast = System.Net.IPAddress.Parse(broadcastAddress);
            IPEndPoint FindServiceRemoteIP = new IPEndPoint(Broadcast, 6000);/*设备接收端口为6000*/
            EndPoint RemoteAddr = (EndPoint)FindServiceRemoteIP;
            int recvlen = 0;
            // 申请 Udp Socket 句柄, 用于进行设备的发现
            Socket FindSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            // 设置接收超时时间为3秒
            FindSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);
            // 设置地址复用使能
            FindSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            FindSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
            // Udp Socket 绑定 本地地址
            FindSock.Bind(FindServiceLocalIp);
            FindSock.SendTo(as8FindCmd, as8FindCmd.Length, SocketFlags.None, (EndPoint)FindServiceRemoteIP);
            while (true)
            {
                try
                {
                    recvlen = FindSock.ReceiveFrom(as8DeviceRspData, ref RemoteAddr);
                    string sn = Encoding.ASCII.GetString(as8DeviceRspData, 0, recvlen).Split(',')[1].ToString();
                    string ip = RemoteAddr.ToString();
                    devices.Add(sn, ip);
                }
                catch (Exception ex)
                {
                    devices.Add("end", ex.Message);
                    break;
                }
            }            
            FindSock.Close();
            return devices;
        }
        /// <summary>
        /// 根据IP地址和子网掩码计算广播地址
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="subnetMask"></param>
        /// <returns>广播的地址</returns>
        public static string GetBroadcast(string ipAddress, string subnetMask)
        {

            byte[] ip = System.Net.IPAddress.Parse(ipAddress).GetAddressBytes();
            byte[] sub = System.Net.IPAddress.Parse(subnetMask).GetAddressBytes();

            // 广播地址=子网按位求反 再 或IP地址
            for (int i = 0; i < ip.Length; i++)
            {
                ip[i] = (byte)((~sub[i]) | ip[i]);
            }
            return new IPAddress(ip).ToString();
        }
        #endregion
        public static void DataRecvProcess(int s32OptCode, int s32IpAddr, int s32RecvResult)
        {
            if (s32OptCode == 0)
            {
                Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).IsComplete = true;
                int len = Module_DeviceManage.Instance.GetMaxChannelMode() * 1000008 + 10;
                Debug.WriteLine(DateTime.Now.ToString("hh:mm:ss fff")+" "+ Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).IP + " 接收结果:" + s32RecvResult.ToString());
                //数据接收完成，根据result判断是否收齐,如果收齐，发送请求波形参数数据命令
                int[] a = new int[4];
                int[] b = new int[4];
                int start = 0;
                int end = 3;               
                for (int i = 1; i <= 4; i++)
                {
                    if (Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).GetChannel(i).Open == true)
                    {
                        a[start] = i;
                        b[start] = 1;
                        start++;
                    }
                    else
                    {
                        a[end] = i;
                        end--;
                    }                 
                }
                if (s32RecvResult == len)
                {
                    Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).IsWholeComplete = true;
                    CCommomControl.getRecvData4Ip(s32IpAddr, Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).GetChannel(a[0]).Data, 1000008 * b[0],
                    Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).GetChannel(a[1]).Data, 1000008 * b[1],
                    Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).GetChannel(a[2]).Data, 1000008 * b[2],
                    Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).GetChannel(a[3]).Data, 1000008 * b[3],
                    Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).Para10Byte, 10
                    );

                }
                else
                {
                    LogUtil.LogError(Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).IP + " 数据未收齐:" + s32RecvResult);
                    Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).IsWholeComplete = false;
                    Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).GetChannel(1).Data = Zero;
                    Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).GetChannel(2).Data = Zero;
                    Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).GetChannel(3).Data = Zero;
                    Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).GetChannel(4).Data = Zero;
                    Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).Para10Byte = TenZero;              
                }
              //  EnqueueTask(Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr));               
            }
            else if (s32OptCode == 1)
            {
                try
                {
                    //Socket socket = CSocket.GetCmdSocket();
                    //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //设置仪器的初始命令             
                    // Module.Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).CmdSocket.Connect(Module.Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).IP, 5555);
                    // string command = "Wav:AutoSend 1" + "\n";     
                    string command = Global.CGlobalCmd.STR_CMD_SET_AUTOSEND + "1" + "\n";

                    Debug.WriteLine(DateTime.Now.ToString("hh:mm:ss fff")+" " +Module.Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).IP + " Wav:AutoSend 1");
                    Module.Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).CmdSocket.Send(Encoding.Default.GetBytes(command));
                    Thread.Sleep(50);
                    //socket.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            else if (s32OptCode == 2)
            {
                //向指定的设备发送数据重传请求
                try
                {
                    // Socket socket = CSocket.GetCmdSocket();
                    // Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //设置仪器的初始命令             
                    // Module.Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).CmdSocket.Connect(Module.Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).IP, 5555);
                    // string command = "Wav:Repeat 1" + "\n";                 
                    string command = Global.CGlobalCmd.STR_CMD_SET_REPEAT + "1" + "\n";
                    Debug.WriteLine(Module.Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).IP + " Wav:Repeat 1");
                    Module.Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).CmdSocket.Send(Encoding.Default.GetBytes(command));
                    Thread.Sleep(50);
                  //  socket.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

            }
            else if (s32OptCode == 3)
            {
                //向指定的设备发送运行命令
                try
                {
                    //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //Socket socket = CSocket.GetCmdSocket();
                    // Module.Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).CmdSocket.Connect(Module.Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).IP, 5555);

                    //设置仪器的初始命令             

                    //string command = ":RUN" + "\n";
                    string command;
                    if (Module_DeviceManage.Instance.TriggerMode == "SINGLE")
                    {
                        command = Global.CGlobalCmd.STR_CMD_SET_SINGLE + "\n";
                    }
                    else
                    {
                        command = Global.CGlobalCmd.STR_CMD_SET_RUN + "\n";
                    }
                    Debug.WriteLine(DateTime.Now.ToString("hh:mm:ss fff") +" "+ Module.Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).IP + " RUN");
                    Module.Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).CmdSocket.Send(Encoding.Default.GetBytes(command));
                    Thread.Sleep(50);
                   // socket.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            else
            {
                //向指定的设备发送运行命令
                try
                {
                    // string command = ":WAVeform:CHANsend 0;:WAV:AUTOsend 0" + "\n";
                    if (Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).CmdSocket != null)
                    {
                        string command = Global.CGlobalCmd.STR_CMD_SET_CHANNELS + "0" + ";" + Global.CGlobalCmd.STR_CMD_SET_AUTOSEND + "0" + "\n";
                        Debug.WriteLine(DateTime.Now.ToString("hh:mm:ss fff") + " " + Module.Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).IP + " :WAVeform:CHANsend 0;:WAV:AUTOsend 0");
                        Module.Module_DeviceManage.Instance.GetDeviceByHashCode(s32IpAddr).CmdSocket.Send(Encoding.Default.GetBytes(command));
                        Thread.Sleep(50);

                    }                
                 
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
        public static int RemoveNode(int[] array,int n,int index)
        {
            for (int i = index; i < n-1; i++)
            {
                array[i] = array[i + 1];
            }
            return n - 1;
        }
    }
}
