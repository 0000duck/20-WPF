using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary_ThreadManage;
using saker_Winform.CommonBaseModule;
using saker_Winform.DataBase;

namespace saker_Winform.Module
{
    public sealed class Module_DeviceManage
    {
        /// <summary>
        /// 仪器管理，单例模式，内存唯一
        /// </summary>
        private static readonly Lazy<Module_DeviceManage> lazy = new Lazy<Module_DeviceManage>(() => new Module_DeviceManage());
        public static Module_DeviceManage Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        // 栈
        public Stack<string> stack = new Stack<string>();
        //开始测量的时间
        public DateTime StartTime { get; set; }
        //波形数据存放表名
        public string WaveTableName { get; set; }
        //项目的识别码 GUID
        public string ProjectGUID { get; set; }
        //项目名称
        public string ProjectName { get; set; }
        //项目注释
        public string ProjectRemark { get; set; }
        //识别码
        public string GUID { get; set; }
        //所有通讯IP
        public string IP { get; set; }
        // 仪器总数
        public int TotalNumber;
        // 虚拟设备池
        public int PoolSize = 75;
        // 在线数量         
        public int OnlineNumber;
        // 仪器清单列表
        public List<Module_Device> Devices = new List<Module_Device>();

        public List<Module_Device> OnlineDevices = new List<Module_Device>();
        //显示配置
        // 触发源
        public string TriggerSource;
        // 触发模式
        public string TriggerMode;
        // 水平偏移
        public string HorizontalOffset;
        // 水平时基
        public string HorizontalTimebase;
        // 触发电平
        public string TriggerLevel;
        // 存储深度
        public string MemDepth;
        // 触发释抑
        public string HoldOff;
        //最大通道模式
        public int MaxChannelModel;
        /// <summary>
        /// 返回IP为指定值的设备,IP不存在时返回空
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Module_Device GetDeviceByIP(string ip)
        {
            var temp = Devices.Where(item => item.IP == ip);
            if (temp.Count() != 0)
            {
                return temp.FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 返回IP为指定值的在线设备,IP不存在时返回空
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Module_Device GetOnlineDeviceByIP(string ip)
        {
            var temp = OnlineDevices.Where(item => item.IP == ip);
            if (temp.Count() != 0)
            {
                return temp.FirstOrDefault();
            }
            return null;
        }
        /// <summary>
        /// 返回SN为指定值的设备，SN不存在时返回空
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public Module_Device GetDeviceBySN(string SN)
        {
            var temp = Devices.Where(item => item.SN == SN);
            if (temp.Count() != 0)
            {
                return temp.FirstOrDefault();
            }
            return null;
        }
        /// <summary>
        /// 返回IP对应hashcode的设备
        /// </summary>
        /// <param name="hashcode"></param>
        /// <returns></returns>
        public Module_Device GetDeviceByHashCode(int hashcode)
        {
            var temp = Devices.Where(item => item.HashCode == hashcode);
            if (temp.Count() != 0)
            {
                return temp.FirstOrDefault();
            }
            return null;
        }
        /// <summary>
        /// 返回虚拟编号为指定值的设备，虚拟编号不存在时返回空
        /// </summary>
        /// <param name="virtualNumberSN"></param>
        /// <returns></returns>
        public Module_Device GetDeviceByVirtualNumber(string virtualNumber)
        {
            var temp = Devices.Where(item => item.VirtualNumber == virtualNumber);
            if (temp.Count() != 0)
            {
                return temp.FirstOrDefault();
            }
            return null;
        }
        /// <summary>
        /// 根据设备名称，获取设备，如果不存在返回NULL
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Module_Device GetDeviceByName(string name)
        {
            var temp = Devices.Where(item => item.Name == name);
            if (temp.Count() != 0)
            {
                return temp.FirstOrDefault();
            }
            return null;
        }
        /// <summary>
        /// 获取通道模式，1通道，2通道，4通道模式
        /// </summary>
        /// <returns></returns>
        public int GetMaxChannelMode()
        {
            int channelMode = Convert.ToInt32(Global.CGlobalValue.euChannelMode.NONE);
            foreach (var item in Devices)
            {
                int temp = Convert.ToInt32(item.GetChannelModel());
                if (temp > channelMode)
                {
                    channelMode = temp;
                }
                if (temp == Convert.ToInt32(Global.CGlobalValue.euChannelMode.FOURCH))
                {
                    return temp;
                }
            }
            return channelMode;
        }

        /// <summary>
        /// 获取栈：GetStack
        /// </summary>
        /// <returns></returns>
        public Stack<string> GetStack()
        {
            if (0 == stack.Count)
            {
                for (int i = 0; i < PoolSize * 4; i++)
                {
                    stack.Push("Tag" + (PoolSize * 4 - i).ToString().PadLeft(3, '0'));
                }
            }
            return stack;
        }

        public Stack<string> SetStack(Stack<string> stackStick)
        {
            stack.Clear();
            stack = stackStick;
            return stack;
        }

        /// <summary>
        /// 获取通道的配置参数,每个IP对应四个通道配置项,返回一个DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable GetChanelConfig()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Collect");
            dt.Columns.Add("Record");
            dt.Columns.Add("DeviceName");
            dt.Columns.Add("ChannelID");
            dt.Columns.Add("Tag");
            dt.Columns.Add("TagDesc");
            dt.Columns.Add("MeasureType");
            dt.Columns.Add("Scale");
            dt.Columns.Add("Offset");
            dt.Columns.Add("Impedance");
            dt.Columns.Add("Coupling");
            dt.Columns.Add("ProbeRatio");
            dt.Columns.Add("SN");
            dt.Columns.Add("Open");
            dt.Columns.Add("ChannelDelayTime");
            dt.Columns.Add("Valid");
            int count = 1;
            List<Module_Device> devicesOrder = Instance.Devices.OrderBy(item => item.VirtualNumber).ToList();
            foreach (Module_Device device in devicesOrder)
            {
                if (device.Status)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Module_Channel channel = device.Channels[i];
                        DataRow row = dt.NewRow();
                        row["ID"] = count.ToString().PadLeft(3, '0');
                        row["Collect"] = channel.Collect.ToString();
                        row["Record"] = channel.Record.ToString();
                        row["DeviceName"] = device.Name;
                        row["ChannelID"] = "CH" + channel.ChannelID;
                        row["Tag"] = channel.Tag;
                        row["TagDesc"] = channel.TagDesc;
                        if (string.IsNullOrEmpty(channel.TagDesc))
                        {
                            row["TagDesc"] = "TagComment" + row["Tag"].ToString().Substring(3, 3);
                        }
                        row["MeasureType"] = channel.MeasureType;
                        row["Scale"] = channel.Scale;
                        row["Offset"] = channel.Offset;
                        row["Impedance"] = "1M";
                        if (channel.Impedance.Contains("FIFTY") || channel.Impedance.Contains("50") || channel.Impedance.Contains("FIFT"))
                        {
                            row["Impedance"] = "50";
                        }
                        row["Coupling"] = channel.Coupling;
                        row["ProbeRatio"] = channel.ProbeRatio;
                        row["SN"] = device.SN;
                        row["Open"] = channel.Open;
                        row["Valid"] = channel.Valid;
                        row["ChannelDelayTime"] = channel.ChannelDelayTime;
                        dt.Rows.Add(row);
                        count++;
                    }
                }
            }
            return dt;
        }

        public string GetDeviceSCPI()
        {
            string command = "";
            command += Global.CGlobalCmd.STR_CMD_SET_TRIGGERMODE + "NORMAL" + ";";
            command += Global.CGlobalCmd.STR_CMD_SET_READMODE + ";";
            command += Global.CGlobalCmd.STR_CMD_SET_READTYPE + ";";
            command += Global.CGlobalCmd.STR_CMD_SET_SERVERIP + Instance.IP + ";";
            command += Global.CGlobalCmd.STR_CMD_SET_TRIGGERSOURCE + Instance.TriggerSource + ";";
            command += Global.CGlobalCmd.STR_CMD_SET_HORIZONTAOFFSET + Instance.HorizontalOffset + ";";
            command += Global.CGlobalCmd.STR_CMD_SET_HORIZONTALTIMEBASE + Instance.HorizontalTimebase + ";";
            command += Global.CGlobalCmd.STR_CMD_SET_TRIGGERLEVEL + Instance.TriggerLevel + ";";
            //command += Global.CGlobalCmd.STR_CMD_SET_TRIGGERMODE + Instance.TriggerMode + ";";                           
            command += Global.CGlobalCmd.STR_CMD_SET_MEMDEPTH + Instance.MemDepth + ";";
            command += Global.CGlobalCmd.STR_CMD_SET_HOLDOFF + Instance.HoldOff + ";";
            ; return command;
        }
        public string ReadDeviceSCPI()
        {
            string command = "";
            command += Global.CGlobalCmd.STR_CMD_GET_READMODE + ";";
            command += Global.CGlobalCmd.STR_CMD_GET_READTYPE + ";";
            command += Global.CGlobalCmd.STR_CMD_GET_SERVERIP + ";";
            command += Global.CGlobalCmd.STR_CMD_GET_HORIZONTAOFFSET + ";";
            command += Global.CGlobalCmd.STR_CMD_GET_HORIZONTALTIMEBASE + ";";
            command += Global.CGlobalCmd.STR_CMD_GET_TRIGGERLEVEL + ";";
            command += Global.CGlobalCmd.STR_CMD_GET_TRIGGERMODE + ";";
            command += Global.CGlobalCmd.STR_CMD_GET_TRIGGERSOURCE + ";";
            command += Global.CGlobalCmd.STR_CMD_GET_MEMDEPTH + ";";
            command += Global.CGlobalCmd.STR_CMD_GET_HOLDOFF + ";";
            return command;
        }
        public List<string> GetIpList()
        {
            List<string> list = new List<string>();
            foreach (Module_Device device in Instance.Devices)
            {
                list.Add(device.IP);
            }
            return list;
        }
        public List<Module_WaveMonitor.OscilloscopeDataMemory> GetWaveData()
        {
            List<Module_WaveMonitor.OscilloscopeDataMemory> waveDataList = new List<Module_WaveMonitor.OscilloscopeDataMemory>();
            foreach (Module_Device device in Devices)
            {
                foreach (Module_Channel channel in device.Channels)
                {
                    if (channel.Valid == true)
                    {
                        Module_WaveMonitor.OscilloscopeDataMemory wavaData = new Module_WaveMonitor.OscilloscopeDataMemory();
                        wavaData.euChan = (Global.CGlobalValue.euChanID)channel.ChannelID;
                        wavaData.strChanID = channel.Tag;
                        wavaData.strDevSN = device.SN;
                        wavaData.bChanImpedence = channel.BChanImpedence;
                        wavaData.trigTimStamp = (device.Para10Byte[8] << 8) | (device.Para10Byte[9]);
                        wavaData.chanDelayTime = device.Para10Byte[(channel.ChannelID - 1) * 2] << 8 | device.Para10Byte[(channel.ChannelID - 1) * 2 + 1];
                        // wavaData.chanDelayTime = channel.ChannelDelayTime;
                        // wavaData.trigTimStamp = device.TrigTimeStamp;
                        wavaData.horzScale = Convert.ToDouble(Instance.HorizontalTimebase);
                        wavaData.horzOffset = Convert.ToDouble(Instance.HorizontalOffset);
                        wavaData.vertScale = Convert.ToDouble(channel.Scale);
                        wavaData.vertOffset = Convert.ToDouble(channel.Offset);
                        wavaData.memDepth = CString2Value.meDepth2Value(Instance.MemDepth);// Convert.ToInt32(Instance.MemDepth);
                        //wavaData.originData = channel.GetData();
                        wavaData.xIncrement = Convert.ToDouble(channel.XIncrement);
                        wavaData.xOrigin = Convert.ToDouble(channel.XOrigin);
                        wavaData.xReference = Convert.ToDouble(channel.XReference);
                        wavaData.yIncrement = Convert.ToDouble(channel.YIncrement);
                        wavaData.yOrigin = Convert.ToDouble(channel.YOrigin);
                        wavaData.yReference = Convert.ToDouble(channel.YReference);
                        wavaData.bChanInv = channel.BChanInv;
                        wavaData.bChanBWLimit = channel.BChanBWLimit;
                        wavaData.sampRate = Convert.ToDouble(device.SampRate);
                        wavaData.devDelayTime = Convert.ToInt32(device.DevDelayTime);
                        waveDataList.Add(wavaData);
                    }
                }
            }
            return waveDataList;
        }

        /// <summary>
        ///  根据通道配置表，开始设置仪器
        /// </summary>
        public void SetDevices()
        {
            Debug.WriteLine(DateTime.Now.ToString() + " 开始配置");
            using (var countdown = new MutipleThreadResetEvent(Instance.Devices.Count))
            {
                for (int k = 0; k < Instance.Devices.Count; k++)
                {
                    var model = new MutipleThreadSendCmd();
                    model.SetDevice(Instance.Devices[k]);
                    model.MTREvent = countdown;
                    //开启N个线程，传递MutipleThreadResetEvent对象给子线程
                    ThreadPool.QueueUserWorkItem(SetSingleDevice, model);
                }
                //等待所有线程执行完毕
                countdown.WaitAll();
            }
            Debug.WriteLine(DateTime.Now.ToString() + " ");
        }
        /// <summary>
        /// 设置单台仪器命令
        /// </summary>
        /// <param name="obj"></param>
        private void SetSingleDevice(Object obj)
        {
            MutipleThreadSendCmd model = obj as MutipleThreadSendCmd;
            if (model.GetDevice().IsOnTesting == false)
            {
                model.MTREvent.SetOne();
                return;
            }
            try
            {
                Dictionary<string, string> origin = new Dictionary<string, string>();
                string send = GetDeviceSCPI();
                List<string> channelset = model.GetDevice().GetChannelSCPI();
                //发送命令，关闭通道
                //Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 2000);
                //sock.Connect(model.GetDevice().IP, 5555);
                Debug.WriteLine("初始化仪器，关闭所有通道");
                string channelInit = ":CHANnel1:DISPlay OFF;:CHANnel2:DISPlay OFF;:CHANnel3:DISPlay OFF;:CHANnel4:DISPlay OFF;" + "\n";
                model.GetDevice().CmdSocket.Send(Encoding.Default.GetBytes(channelInit));
                Thread.Sleep(50);
                if (channelset.Count == 0)
                {
                    model.MTREvent.SetOne();
                    return;
                }
                Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + " " + model.GetDevice().IP);
                foreach (string cmd in channelset)
                {
                    send += cmd;
                }
                send += Global.CGlobalCmd.STR_CMD_SET_TRIGGERMODE + Instance.TriggerMode + ";";
                send = send.Remove(send.Length - 1, 1);
                List<string> cmdSend = send.Split(';').ToList();
                send += "\n";
                Debug.WriteLine(DateTime.Now.ToString() + " 开始配置仪器：" + model.GetDevice().IP);
                model.GetDevice().CmdSocket.Send(Encoding.Default.GetBytes(send));
                Debug.WriteLine(send);
                Thread.Sleep(50);
                Debug.WriteLine(DateTime.Now.ToString() + " 配置仪器完成：" + model.GetDevice().IP);
                //开始回读仪器的参数
                string cmdGet = "";
                for (int i = 1; i < cmdSend.Count; i++)
                {
                    string cmd = cmdSend[i];
                    string[] strTemp = cmd.Split(' ');
                    origin.Add(strTemp[0].ToString(), strTemp[1].ToString());
                    cmdGet = cmdGet + strTemp[0].ToString() + "?" + ";";
                }
                cmdGet = cmdGet.Remove(cmdGet.Length - 1, 1);
                cmdGet += "\n";
                Debug.WriteLine(DateTime.Now.ToString() + " 发送回读命令：" + model.GetDevice().IP);
                Debug.WriteLine(cmdGet);
                model.GetDevice().CmdSocket.Send(Encoding.Default.GetBytes(cmdGet));
                byte[] buffer = new byte[1024];
                int ret = model.GetDevice().CmdSocket.Receive(buffer);
                string result = Encoding.Default.GetString(buffer, 0, ret);
                result = result.Remove(result.Length - 1, 1);
                Debug.WriteLine(result);
                List<string> read = result.Split(';').ToList();
                Debug.WriteLine(DateTime.Now.ToString() + " 回读结果解析：" + model.GetDevice().IP);
                //按顺序取出结果，进行对比
                model.GetDevice().ConfigSuccess = true;
                for (int i = 0; i < origin.Count; i++)
                {
                    //直接字符串比较的查询结果
                    var elem = origin.ElementAt(i);
                    string para = elem.Key.ToString().ToUpper();
                    string value = elem.Value.ToString();
                    /* 通道参数回读校验 */
                    if (para.Contains("PROB"))
                    {
                        model.GetDevice().GetChannel(int.Parse(para.Substring(8, 1))).ProbeRatio = "*" + int.Parse(read[i]).ToString();
                        Debug.WriteLine("*" + int.Parse(read[i]).ToString());
                    }
                    else if (para.Contains("SCALE") && !para.Contains("TIMEBASE:SCALE"))
                    {
                        model.GetDevice().GetChannel(int.Parse(para.Substring(8, 1))).Scale = double.Parse(read[i]).ToString();
                        Debug.WriteLine(model.GetDevice().GetChannel(int.Parse(para.Substring(8, 1))).Scale);
                    }
                    else if (para.Contains("IMP"))
                    {
                        model.GetDevice().GetChannel(int.Parse(para.Substring(8, 1))).Impedance = read[i].ToString();
                        Debug.WriteLine(model.GetDevice().GetChannel(int.Parse(para.Substring(8, 1))).Impedance);
                    }
                    else if (para.Contains("OFFSET") && !para.Contains("TIMEBASE:OFFSET"))
                    {
                        model.GetDevice().GetChannel(int.Parse(para.Substring(8, 1))).Offset = double.Parse(read[i]).ToString();
                        Debug.WriteLine(model.GetDevice().GetChannel(int.Parse(para.Substring(8, 1))).Offset);
                    }
                    else if (para.Contains("COUP"))
                    {
                        model.GetDevice().GetChannel(int.Parse(para.Substring(8, 1))).Coupling = read[i].ToString();
                        Debug.WriteLine(model.GetDevice().GetChannel(int.Parse(para.Substring(8, 1))).Coupling);
                    }
                    else if (para.Contains("DISP"))
                    {
                        if (read[i].ToString() == "1")
                        {
                            model.GetDevice().GetChannel(int.Parse(para.Substring(8, 1))).Collect = true;
                            model.GetDevice().GetChannel(int.Parse(para.Substring(8, 1))).Record = true;
                            Debug.WriteLine(model.GetDevice().GetChannel(int.Parse(para.Substring(8, 1))).Collect);
                        }
                        else
                        {
                            model.GetDevice().GetChannel(int.Parse(para.Substring(8, 1))).Collect = false;
                            model.GetDevice().GetChannel(int.Parse(para.Substring(8, 1))).Record = false;
                        }

                    }
                    /* 设备回读校验 */
                    if (para.Contains("MODE") || para.Contains("FROM") || para.Contains("SERVER") || para.Contains("SOURCE") || para.Contains("SWEEP") || para.Contains("WAVE"))
                    {
                        //记录比较结果
                        if (!value.Contains(read[i].ToString()))
                        {
                            //写入到某个List中或者日志中去
                            model.GetDevice().ConfigSuccess = false;
                            Debug.WriteLine(model.GetDevice().IP + "仪器未设置成功");
                        }
                    }
                    //直接转成double形式比较的查询结果
                    if (para.Contains("TIMEBASE:SCALE") || para.Contains("LEVEL") || para.Contains("TIMEBASE:OFFSET") || para.Contains(":TRIGGER:HOLDOFF"))
                    {
                        if (!Convert.ToDouble(value).Equals(Convert.ToDouble(read[i])))
                        {
                            //写入到某个List中去
                            model.GetDevice().ConfigSuccess = false;
                            Debug.WriteLine(model.GetDevice().IP + "仪器未设置成功");
                        }
                    }
                }
                Debug.WriteLine(DateTime.Now.ToString() + " 解析完成：" + model.GetDevice().IP);
                //获取采样率         
                model.GetDevice().CmdSocket.Send(Encoding.Default.GetBytes(Global.CGlobalCmd.STR_CMD_GET_SAMPLERATE + "\n"));
                byte[] bufferSample = new byte[1024];
                int retSample = model.GetDevice().CmdSocket.Receive(bufferSample);
                Instance.GetDeviceByIP(model.GetDevice().IP).SampRate = Encoding.Default.GetString(bufferSample, 0, retSample).TrimEnd(Environment.NewLine.ToCharArray());
                model.MTREvent.SetOne();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(model.GetDevice().IP + " 配置异常：" + ex.Message);
                model.MTREvent.SetOne();
            }

        }
        /// <summary>
        /// 获取设备的有效tag
        /// </summary>
        /// <returns></returns>
        public List<string> GetDevicesValidTag()
        {
            List<string> validTag = new List<string>();
            foreach (var item in Devices)
            {
                for (int i = 1; i <= 4; i++)
                {
                    var channel = item.GetChannel(i);
                    if (channel.Collect)
                    {
                        validTag.Add(channel.Tag);
                    }
                }
            }
            return validTag;
        }
        /// <summary>
        /// 通道配置表赋值给响应设备信息
        /// </summary>
        public void AssignDeviceAndChannel(Dictionary<string, string> dc, DataTable dt)
        {
            try
            {
                Instance.TriggerMode = dc["TriggerMode"];
                Instance.TriggerSource = dc["TriggerSource"];
                Instance.TriggerLevel = dc["TriggerLevel"];
                Instance.MemDepth = dc["MemDepth"];
                Instance.HoldOff = dc["HoldOff"];
                Instance.HorizontalTimebase = dc["HorizontalTimebase"];
                Instance.HorizontalOffset = dc["HorizontalOffset"];
                Instance.IP = dc["IP"];

                foreach (DataRow dr in dt.Rows)
                {
                    var sn = dr["SN"].ToString();
                    if (string.IsNullOrEmpty(Instance.GetDeviceBySN(sn).ServerIP))
                    {
                        Instance.GetDeviceBySN(sn).ServerIP = dc["IP"];

                    }

                    var channelId = Convert.ToInt32(dr["ChannelID"].ToString().Substring(2, 1));
                    Instance.GetDeviceBySN(sn).SetChannelConfig(dr);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// 判断数据是否收取完毕
        /// </summary>
        /// <returns></returns>
        public bool GetIsCompelte()
        {
            bool flag = false;
            List<Module_Device> IsOnTestingDevices = Devices.Where(item => item.IsOnTesting == true).ToList();
            if (IsOnTestingDevices.Count == 0)
            {
                return flag;
            }
            var temp = IsOnTestingDevices.Where(item => item.IsComplete == false);
            if (temp.Count() == 0)
            {
                return true;
            }
            return flag;
        }
        /// <summary>
        /// 判断数据是否收来到
        /// </summary>
        /// <returns></returns>
        public bool GetIsComing()
        {
            List<Module_Device> IsOnTestingDevices = Devices.Where(item => item.IsOnTesting == true).ToList();
            var temp = IsOnTestingDevices.Where(item => item.IsComplete == true);
            if (temp.Count() == 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据tag返回对应的通道
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Module_Channel GetChannelByTag(string tag)
        {
            for (int i = 0; i < Devices.Count; i++)
            {
                for (int j = 0; j < Devices[i].Channels.Length; j++)
                {
                    var channel = Devices[i].Channels[j];
                    if (channel.Collect == true && channel.Tag.Equals(tag))
                    {
                        return channel;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 获取设备中的最小延时
        /// </summary>
        /// <returns></returns>
        public int GetMinDeviceDelay()
        {
            int flag = 0;
            int minDelay = 0;
            for (int i = 0; i < Devices.Count; i++)
            {
                //如果这台设备有通道模式，则采集数据
                if (Devices[i].GetChannelModel() != 0 && flag == 0)
                {
                    if (!string.IsNullOrEmpty(Devices[i].DevDelayTime))
                    {
                        minDelay = Convert.ToInt32(Devices[i].DevDelayTime);
                        flag = 1;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Devices[i].DevDelayTime))
                    {
                        if (minDelay > Convert.ToInt32(Devices[i].DevDelayTime))
                        {
                            minDelay = Convert.ToInt32(Devices[i].DevDelayTime);
                        }
                    }
                }
            }
            return minDelay;
        }
        //获取设备list，输出为dt，直接插入到数据库
        public DataTable GetDeviceDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("GUID");
            dt.Columns.Add("CollectGUID");
            dt.Columns.Add("Name");
            dt.Columns.Add("IP");
            dt.Columns.Add("SerialNumber");
            dt.Columns.Add("MAC");
            dt.Columns.Add("SoftVersion");
            dt.Columns.Add("Model");
            dt.Columns.Add("VirtualNumber");
            dt.Columns.Add("Channel");

            //    dt.Columns.Add("TrigTimeStamp");
            dt.Columns.Add("Status");
            dt.Columns.Add("SampleRate");
            dt.Columns.Add("CreateTime");
            dt.Columns.Add("DelayTime");
            dt.Columns.Add("ServerIP");
            dt.Columns.Add("ChannelModel");
            foreach (Module_Device item in Devices)
            {
                DataRow dr = dt.NewRow();
                item.GUID = Guid.NewGuid().ToString();
                dr["GUID"] = item.GUID;
                dr["CollectGUID"] = GUID;
                dr["Name"] = item.Name;
                dr["IP"] = item.IP;
                dr["SerialNumber"] = item.SN;
                dr["MAC"] = item.MAC;
                dr["SoftVersion"] = item.SoftVersion;
                dr["Model"] = item.Model;
                dr["VirtualNumber"] = item.VirtualNumber;
                dr["Channel"] = item.GetChannelModel();
                //   dr["TrigTimeStamp"] = item.TrigTimeStamp;
                dr["Status"] = item.Status.ToString();
                dr["SampleRate"] = item.SampRate;
                dr["CreateTime"] = DateTime.Now.ToString();
                dr["ServerIP"] = item.ServerIP;
                dr["DelayTime"] = item.DevDelayTime;
                dr["ChannelModel"] = MaxChannelModel.ToString();
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public DataTable GetAllChannelConfigDataTable()
        {
            DataTable dt = new DataTable();
            foreach (Module_Device device in Devices)
            {
                DataTable temp = device.GetChannelDataTable();
                dt.Merge(temp);
            }
            return dt;
        }
        public DataTable GetAllChannelWaveDataTable()
        {
            DataTable dt = new DataTable();
            foreach (Module_Device device in Devices)
            {
                DataTable temp = device.GetChannelWaveData();
                dt.Merge(temp);
            }
            return dt;
        }
        public string GetGUID()
        {
            if (string.IsNullOrEmpty(GUID))
            {
                GUID = Guid.NewGuid().ToString();
            }
            return GUID;
        }
        public Module_Device GetDeviceByGUID(string guid)
        {
            var temp = Devices.Where(item => item.GUID == guid);
            if (temp.Count() != 0)
            {
                return temp.FirstOrDefault();
            }
            return null;
        }
        public void Clear()
        {

            for (int i = 0; i < Instance.Devices.Count; i++)
            {
                Instance.Devices[i].Channels[0] = null;
                Instance.Devices[i].Channels[1] = null;
                Instance.Devices[i].Channels[2] = null;
                Instance.Devices[i].Channels[3] = null;
                Instance.Devices[i] = null;
            }
            Instance.GUID = "";
        }
        public void InitDeviceDelayTime()
        {
            string sql = "SELECT SerialNumber,DeviceDelayTime FROM [View_Data_Calibration]";
            DataTable dt = DbHelperSql.QueryDt(sql);
            foreach (Module_Device device in Devices)
            {
                var temp = dt.Select("SerialNumber='" + device.SN + "'");
                if (temp == null || temp.Count() == 0)
                {
                    device.DevDelayTime = "0";
                }
                else
                {
                    device.DevDelayTime = temp.FirstOrDefault()["DeviceDelayTime"].ToString();
                }
            }
        }
        /// <summary>
        /// 初始化仪器为不采集状态，参与测试设置为false
        /// </summary>
        public void InitDeviceOnTesting()
        {
            for (int i = 0; i < Instance.Devices.Count; i++)
            {
                Instance.Devices[i].IsOnTesting = false;
            }
        }
    }
}
