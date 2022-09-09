using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace saker_Winform.UserControls
{
    public partial class UCBaseFrm:Form
    {     
        /*******************************************************************************
          * 函    数：openDevice
          * 描    述：根据IP地址数组中的设备IP地址，进行设备连接的打开
          * 输入参数：
          *             参数名称                参数类型            参数说明
          *             ps32IpAddr              int*                设备IP地址数组
          *             ps32ClientPort          int*                设备Socket通信端口号
          *             s32DeviceNum            int                 准备打开的设备数
          * 输出参数：
          *             参数名称                参数类型            参数说明
          *             ps32Result              int*                设备打开连接的错误码
          *                                                             1 -- 设备已建立连接
          *                                                             0 -- 无错误
          *                                                            -2 -- 建立连接错误(TCP) 
          *                                                            -5 -- 地址绑定错误(UDP)
          *                                                            -8 -- 设备未发现
          * 返 回 值： 0 -- 成功
          *           -1 -- 失败
          * 说    明：只有在IP地址对应设备未建立连接时进行打开
         ******************************************************************************/
        [DllImport("cbbDeviceConfigService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int openDevice(int [] ps32IpAddr, int [] ps32ClientPort, int s32DeviceNum, int [] ps32Result);

        /*******************************************************************************
          * 函    数：regDevConfigCilent
          * 描    述：向设备配置服务上架客户端信息
          *           该函数将根据注册的设备信息进行连接的建立
          * 输入参数：
          *           参数名称              参数类型        参数说明
          *           ps32IpAddr            int*            设备IP地址数组的首地址
          *           s32Protocol           int*            设备通信使用的协议:0--UDP
          *                                                                    1--TCP
          *           ps32ClientPort        int*            设备端可连接端口的数组
          *           ps32ServerPort        int*            服务端应该使用端口的数组
          *                                                 仅UDP协议需要
          *           s32DeviceNum          int             上架设备的数量
          * 输出参数：
          *           参数名称              参数类型        参数说明
          *           ps32Result            int*            设备上架结果: 
          *                                                     1 -- 警告：设备已建立连接
          *                                                     0 -- 成功
          *                                                    -1 -- WSA初始化失败
          *                                                    -2 -- 建立连接错误(TCP) 
          *                                                    -5 -- 地址绑定错误(UDP)
          *                                                    -8 -- 设备未发现
          *                                                   -11 -- 内存错误
          * 返 回 值： 0 -- 上架成功
          *           -1 -- 上架失败,根据ps32Result的输出结果进行判定
          *           -2 -- 参数错误
          *           -3 -- 内存错误
          * 说    明：1.IP地址必须遵循网络数据的存储规则(小端存储)
          *           2.该函数仅当设备列表为空时使用,其他时间请使用addDevConfigClient进行
          *             设备的添加，以及使用editDevConfigClient进行变更
          *           3.若Result结果为-2或者-5，可通过openDevice尝试再次建立连接
          *             或者通过editDevConfigClient进行变更后自动建立连接
         ******************************************************************************/
        [DllImport("cbbDeviceConfigService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int regDevConfigCilent(int [] ps32IpAddr,
                                                    int [] s32Protocol,
                                                    int [] ps32ClientPort,
                                                    int [] ps32ServerPort,
                                                    int s32DeviceNum,
                                                    int [] ps32Result);

        /*******************************************************************************
          * 函    数：addDevConfigClient
          * 描    述：向当前的设备列表中添加新的设备节点
          * 输入参数：
          *             参数名称                参数类型            参数说明
          *             s32IpAddr               int                 设备IP地址
          *             s32Protocol             int                 设备Socket通信协议
          *                                                             0 -- UDP
          *                                                             1 -- TCP
          *             s32ClientPort           int                 设备的连接端口号
          *             s32ServerPort           int                 服务端应该使用的端口号
          *                                                         仅适用于UDP协议
          * 输出参数：无
          * 返 回 值： 0 -- 成功
          *           -1 -- 设备已存在,若设备未连接,请使用openDevice接口函数
          *           -2 -- 内存错误
          *           -3 -- 通信I/O错误,请检查设备参数
          *           -4 -- 查询超时,请确保是否为支持的设备型号列表
          * 说    明：无
         ******************************************************************************/
        [DllImport("cbbDeviceConfigService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int addDevConfigClient(int s32IpAddr,
                                                    int s32Protocol,
                                                    int s32ClientPort,
                                                    int s32ServerPort);
        /*******************************************************************************
  * 函    数：checkDevConfigClient
  * 描    述：获取当前设备信息,进行设备的注册使用
  * 输入参数：
  *             参数名称                参数类型            参数说明
  *             s32IpAddr               int                 设备IP地址
  *             s32Protocol             int                 设备Socket通信协议
  *                                                             0 -- UDP
  *                                                             1 -- TCP
  *             s32ClientPort           int                 设备的连接端口号
  *             s32ServerPort           int                 服务端应该使用的端口号
  *                                                         仅适用于UDP协议
  * 输出参数：
  *             参数名称                参数类型            参数说明
  *             pu8IDNInfo              unsigned char*      设备的*IDN?信息
  * 返 回 值：>0 -- 成功,设备*IDN?返回值的长度
  *           -1 -- 设备已存在,若设备未连接,请使用openDevice接口函数
  *           -2 -- 内存错误
  *           -3 -- 通信I/O错误,请检查设备参数
  *           -4 -- 查询超时,请确保是否为支持的设备型号列表
  * 说    明：无
 ******************************************************************************/
        [DllImport("cbbDeviceConfigService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int checkDevConfigClient(int s32IpAddr,
                                                      int s32Protocol,
                                                      int s32ClientPort,
                                                      int s32ServerPort,
                                                      byte [] pu8IDNInfo);
        /*******************************************************************************
          * 函    数：delDevConfigClient
          * 描    述：根据设备IP地址和端口号,进行设备节点的删除操作
          * 输入参数：
          *             参数名称                参数类型            参数说明
          *             s32IpAddr               int                 设备IP地址
          *             s32ClientPort           int                 设备Socket端口号
          * 输出参数：无
          * 返 回 值：无
          * 说    明：若当前设备存在连接,先关闭后，再进行子线程的退出处理
         ******************************************************************************/
        [DllImport("cbbDeviceConfigService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void delDevConfigClient(int s32IpAddr, int s32ClientPort);

        /*******************************************************************************
          * 函    数：editDevConfigClient
          * 描    述：根据设备原IP地址和端口号,进行设备节点更新到新的IP地址和端口号
          * 输入参数：
          *             参数名称                参数类型            参数说明
          *             s32SrcIpAddr            int                 设备原IP地址
          *             s32SrcClientPort        int                 设备原端口号
          *             s32DstIpAddr            int                 设备新IP地址
          *             s32DstClientPort        int                 设备新端口号
          *             s32DstServerPort        int                 服务端新的端口号
          *                                                         仅用于UDP协议
          *             s32DstPotocal           int                 通信协议：0 -- UDP
          *                                                                   1 -- TCP
          * 输出参数：无
          * 返 回 值： 0 -- 成功
          *           -1 -- 未查询到需要变更的设备节点
          *           -2 -- 与设备建立连接错误
          * 说    明：若当前设备存在连接,先关闭后，再进行设备信息的变更
         ******************************************************************************/
        [DllImport("cbbDeviceConfigService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int editDevConfigClient(int s32SrcIpAddr, int s32SrcClientPort,
                                                     int s32DstIpAddr, int s32DstClientPort,
                                                     int s32DstServerPort, int s32DstPotocal);

        /*******************************************************************************
          * 函    数：unregDevConfigCilent
          * 描    述：向设备配置服务下架客户端信息
          * 输入参数：无
          * 输出参数：无
          * 返 回 值：无
          * 说    明：1.注意线程同步信号池的信号量关闭,以及信号池的释放
          *           2.注意每个客户端申请的用于事件接收的信号量提前释放后,再释放客户端表
         ******************************************************************************/
        [DllImport("cbbDeviceConfigService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void unregDevConfigCilent( );

        /*******************************************************************************
          * 函    数：write2Device
          * 描    述：向设备写入数据
          * 输入参数：
          *           参数名称              参数类型        参数说明
          *           ps32IpAddr            int*            需要写操作的设备IP地址
          *           ps32Port              int*            需要写操作的设备端口号
          *           s32IpAddrSize         int             设备IP地址数组的元素数量
          *           pu8WriteBuff          unsignedchar*   需要写数据的地址
          *           ps32WriteLen          int*            每个IP需要写数据的长度
          * 输出参数：
          *           参数名称              参数类型        参数说明
          *           ps32Result            int*            每个客户端的写结果
          *                                                  -7 -- 通信IO错误
          *                                                  -8 -- 设备没有注册
          *                                                 -12 -- 通信未建立
          *                                                   0 -- 成功
          * 返 回 值： 0 -- 成功
          *           -1 -- 发生错误,具体错误见 ps32Result
          *           -9 -- 参数错误
          * 说    明：
         ******************************************************************************/
        [DllImport("cbbDeviceConfigService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int write2Device(  int [] ps32IpAddr,
                                                int [] ps32Port,
                                                int s32IpAddrSize,
                                                byte [] pu8WriteBuff,
                                                int  [] ps32WriteLen,
                                                int  [] ps32Result);

        /*******************************************************************************
          * 函    数：write2DeviceAll
          * 描    述：向设备写入数据，写数据所有设备一样
          * 输入参数：
          *           参数名称              参数类型        参数说明
          *           ps32IpAddr            int*            需要写操作的设备IP地址
          *           ps32Port              int*            需要写操作的设备端口号
          *           s32IpAddrSize         int             设备IP地址数组的元素数量
          *           pu8WriteBuff          unsignedchar*   需要写数据的地址
          *           s32WriteLen           int             需要写数据的长度
          * 输出参数：
          *           参数名称              参数类型        参数说明
          *           ps32Result            int*            每个客户端的写结果
          *                                                  -7 -- 通信IO错误
          *                                                  -8 -- 设备没有注册
          *                                                 -12 -- 通信未建立
          *                                                   0 -- 成功
          * 返 回 值： 0 -- 成功
          *           -1 -- 发生错误,具体错误见 ps32Result
          *           -9 -- 参数错误
          * 说    明：
         ******************************************************************************/
        [DllImport("cbbDeviceConfigService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int write2DeviceAll(int [] ps32IpAddr,
                                                 int [] ps32Port,
                                                 int s32IpAddrSize,
                                                 byte [] pu8WriteBuff,
                                                 int s32WriteLen,
                                                 int [] ps32Result);

        /*******************************************************************************
          * 函    数：read4Device
          * 描    述：从设备进行数据的读取操作
          * 输入参数：
          *           参数名称              参数类型        参数说明
          *           ps32IpAddr            int*            需要写操作的设备IP地址
          *           ps32Port              int*            需要写操作的设备端口号
          *           s32IpAddrSize         int             设备IP地址数组的元素数量
          *           s32Timeout            int             超时时间
          * 输出参数：
          *           参数名称              参数类型        参数说明
          *           ps32Result            int*            每个客户端的写结果
          *                                                  -3 -- 读操作超时
          *                                                  -7 -- 通信IO错误
          *                                                  -8 -- 设备没有注册
          *                                                 -12 -- 通信未建立
          *                                                   0 -- 成功
          * 返 回 值： 0 -- 成功
          *           -1 -- 发生错误,具体错误见 ps32Result
          *           -9 -- 参数错误
          * 说    明：该函数需配合getReadData4IP接口进行使用
         ******************************************************************************/
        [DllImport("cbbDeviceConfigService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int read4Device(int [] ps32IpAddr,
                                             int [] ps32Port,
                                             int s32IpAddrSize,
                                             int s32Timeout,
                                             int [] ps32Result);

        /*******************************************************************************
          * 函    数：query4Device
          * 描    述：从设备进行数据的读写操作，单台设备一条命令
          * 输入参数：
          *           参数名称              参数类型        参数说明
          *           ps32IpAddr            int*            需要写操作的设备IP地址
          *           ps32Port              int*            需要写操作的设备端口号
          *           s32IpAddrSize         int             设备IP地址数组的元素数量
          *           pu8WriteBuff          unsignedchar*   需要写数据的地址
          *           ps32WriteLen          int*            每个IP需要写数据的长度
          *           s32Timeout            int             超时时间
          * 输出参数：
          *           参数名称              参数类型        参数说明
          *           ps32Result            int*            每个客户端的写结果
          *                                                  -3 -- 读操作超时
          *                                                  -7 -- 通信IO错误
          *                                                  -8 -- 设备没有注册
          *                                                 -12 -- 通信未建立
          *                                                   0 -- 成功
          * 返 回 值： 0 -- 成功
          *           -1 -- 发生错误,具体错误见 ps32Result
          *           -9 -- 参数错误
          * 说    明：该函数需配合getReadData4IP接口进行使用
         ******************************************************************************/
        [DllImport("cbbDeviceConfigService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int query4Device(int [] ps32IpAddr,
                                              int [] ps32Port,
                                              int s32IpAddrSize,
                                              byte [] pu8WriteBuff,
                                              int [] ps32WriteLen,
                                              int s32Timeout,
                                              int [] ps32Result);

        /*******************************************************************************
          * 函    数：query4DeviceAll
          * 描    述：从设备进行数据的读写操作，所有设备均相同的数据
          * 输入参数：
          *           参数名称              参数类型        参数说明
          *           ps32IpAddr            int*            需要写操作的设备IP地址
          *           ps32Port              int*            需要写操作的设备端口号
          *           s32IpAddrSize         int             设备IP地址数组的元素数量
          *           pu8WriteBuff          unsignedchar*   需要写数据的地址
          *           s32WriteLen           int             需要写数据的长度
          *           s32Timeout            int             超时时间
          * 输出参数：
          *           参数名称              参数类型        参数说明
          *           ps32Result            int*            每个客户端的写结果
          *                                                  -3 -- 读操作超时
          *                                                  -7 -- 通信IO错误
          *                                                  -8 -- 设备没有注册
          *                                                 -12 -- 通信未建立
          *                                                   0 -- 成功
          * 返 回 值： 0 -- 成功
          *           -1 -- 发生错误,具体错误见 ps32Result
          *           -9 -- 参数错误
          * 说    明：该函数需配合getReadData4IP接口进行使用
         ******************************************************************************/
        [DllImport("cbbDeviceConfigService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int query4DeviceAll(int [] ps32IpAddr,
                                                 int [] ps32Port,
                                                 int s32IpAddrSize,
                                                 byte [] pu8WriteBuff,
                                                 int s32WriteLen,
                                                 int s32Timeout,
                                                 int [] ps32Result);

        /*******************************************************************************
          * 函    数：getReadData4IP
          * 描    述：根据IP地址进行对应的读回数据的提取
          * 输入参数：
          *           参数名称              参数类型        参数说明
          *           s32IpAddr             int             设备IP地址
          *           s32PortNum            int             设备通信端口
          *           s32ReadOffset         int             相对于接收缓冲区首地址的偏移值
          *           s32ReadBuffSize       int             读缓冲区大小
          * 输出参数：
          *           参数名称              参数类型        参数说明
          *           pu8ReadBuff           unsigned char*  读缓冲区
          * 返 回 值：>0 -- 读取到有效数据的长度
          *            0 -- 无可读参数
          *           -1 -- 参数错误
          *           -2 -- 未查询到设备
          * 说    明：无
         ******************************************************************************/
        [DllImport("cbbDeviceConfigService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getReadData4IP(int s32IpAddr,
                                                int s32PortNum,
                                                byte [] pu8ReadBuff,
                                                int s32ReadOffset,
                                                int s32ReadBuffSize);



        /*******************************************************************************
          * 函    数：DATA_RECV_OPTION_REPORT
          * 描    述：提供给数据接收服务的托管函数,用于数据接收服务向上层上报信息
          * 输入参数：
          *           参数名称                  参数类型        参数说明
          *           s32OptCode                int             操作码值
          *                                                         0 -- 上报接收状态
          *                                                         1 -- 通知上层进行请求数据发送命令的发送
          *                                                         2 -- 通知上层进行请求数据重传命令的发送
          *           s32IpAddr                 int             本次操作的设备IP地址
          *           s32Status                 int             本次接收的错误码值
          *                                                         >0 -- 数据的接收长度
          *                                                         -4 -- 参数错误
          *                                                         -5 -- 接收超时
          * 输出参数：无
          * 返 回 值：无
          * 说    明：无
         ******************************************************************************/
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void dataRecvReportCallback(int s32OptCode, int s32IpAddr, int s32RecvResult);

        /*******************************************************************************
          * 函    数：createDataRecvService
          * 描    述：启动数据接收服务
          * 输入参数：
          *           参数名称                  参数类型        参数说明
          *           s32MaxSupportDeviceNum    int             数据接收服务支持的最大设备数
          *           pReportIO        DATA_RECV_OPTION_REPORT  注册的托管函数
          * 输出参数：无
          * 返 回 值：无
          * 说    明：必须在添加设备之前启动数据接收服务,否则不允许设备的添加
         ******************************************************************************/
        [DllImport("cbbDevRecvService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void createDataRecvService( dataRecvReportCallback pReportIO);

        /*******************************************************************************
          * 函    数：addDevInfo2DataRecvService
          * 描    述：向数据接收服务添加设备
          * 输入参数：
          *           参数名称                  参数类型        参数说明
          *           ps32ClientIpAddr          int*            设备IP地址数组
          *           ps32ClientPort            int*            设备接收数据的端口号
          *           ps32ClientProtocol        int*            设备进行数据上传使用的协议
          *                                                         0 -- UDP
          *                                                         1 -- TCP
          *           ps32ServicePort           int*            服务端接收时分配的端口号
          *           s32ClientNum              int             本次添加的设备数量
          * 输出参数：
          *           参数名称                  参数类型        参数说明
          *           ps32RegResult             int*            设备添加的结果
          *                                                         0  -- 成功
          *                                                        -2  -- Socket句柄申请失败
          *                                                        -3  -- 设备地址绑定失败
          *                                                        -8  -- 服务监听失败
          *                                                        -10 -- WSA启动失败
          * 返 回 值：无
          * 说    明：ps32ClientPort只有在协议为UDP时,才可以使用
         ******************************************************************************/
        [DllImport("cbbDevRecvService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void addDevInfo2DataRecvService(int[] ps32ClientIpAddr,
                                                             int[] ps32ClientPort,
                                                             int[] ps32ClientProtocol,
                                                             int[] ps32ServicePort,
                                                             int s32ClientNum,
                                                             int[] ps32RegResult);
        /*******************************************************************************
          * 函    数：deleteDevNode4DataRecvService
          * 描    述：从数据接收服务中移出设备
          * 输入参数：
          *           参数名称                  参数类型        参数说明
          *           ps32ClientIpAddr          int*            设备IP地址数组
          *           s32ClientNum              int             本次移出的设备数量
          * 输出参数：无
          * 返 回 值：无
          * 说    明：无
         ******************************************************************************/
        [DllImport("cbbDevRecvService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void deleteDevNode4DataRecvService(int[] ps32ClientIpAddr, int s32ClientNum);

        /*******************************************************************************
          * 函    数：editDevNode2DataRecvService
          * 描    述：向数据接收服务添加设备
          * 输入参数：
          *           参数名称                  参数类型        参数说明
          *           ps32OldClientIpAddr       int*            待修改的设备IP地址,作为识别码
          *           ps32NewClientIpAddr       int*            设备IP地址数组
          *           ps32NewClientPort         int*            设备接收数据的端口号
          *           ps32NewClientProtocol     int*            设备进行数据上传使用的协议
          *                                                         0 -- UDP
          *                                                         1 -- TCP
          *           ps32NewServicePort        int*            服务端接收时分配的端口号
          *           s32ClientNum              int             本次待修改的设备数量
          * 输出参数：
          *           参数名称                  参数类型        参数说明
          *           ps32EditResult            int*            设备修改的结果
          *                                                         0  -- 成功
          *                                                        -2  -- Socket句柄申请失败
          *                                                        -3  -- 设备地址绑定失败
          *                                                        -8  -- 服务监听失败
          *                                                        -10 -- WSA启动失败
          * 返 回 值：无
          * 说    明：ps32ClientPort只有在协议为UDP时,才可以使用
         ******************************************************************************/
        [DllImport("cbbDevRecvService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void editDevNode2DataRecvService(int[] ps32OldClientIpAddr,
                                                              int[] ps32NewClientIpAddr,
                                                              int[] ps32NewClientPort,
                                                              int[] ps32NewClientProtocol,
                                                              int[] ps32NewServicePort,
                                                              int   s32ClientNum,
                                                              int[] ps32EditResult);

        /*******************************************************************************
          * 函    数：recvDevData
          * 描    述：进行数据的接收处理
          * 输入参数：
          *           参数名称                  参数类型        参数说明
          *           ps32ClientIpAddr          int*            设备IP地址,作为识别码
          *           ps32RecvSize              int*            针对每台设备,准备接收的数据长度
          *           s32ClientNum              int             本次待修改的设备数量
          *           s32RecvTimeout            int             超时时间,毫秒
          * 输出参数：无
          * 返 回 值：无
          * 说    明：无
         ******************************************************************************/
        [DllImport("cbbDevRecvService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void recvDevData(int[] ps32ClientIpAddr,
                                              int[] ps32RecvSize,
                                              int   s32ClientNum,
                                              int   s32RecvTimeout);

        /*******************************************************************************
          * 函    数：getTcpSocketData45(外部接口)
          * 描    述：根据客户端设备IP地址,进行数据的提取
          * 输入参数：
          *           参数名称              参数类型        参数说明
          *           s32IpAddr             int             客户端设备IP地址
          *           s32BufLen1            int             数据缓冲区1的大小
          *           s32BufLen2            int             数据缓冲区2的大小
          *           s32BufLen3            int             数据缓冲区3的大小
          *           s32BufLen4            int             数据缓冲区4的大小
          *           s32BufLen5            int             数据缓冲区5的大小
          * 输出参数：
          *           参数名称              参数类型        参数说明
          *           pu8Buf1               unsigned char*  数据缓冲区1的首地址
          *           pu8Buf2               unsigned char*  数据缓冲区2的首地址
          *           pu8Buf3               unsigned char*  数据缓冲区3的首地址
          *           pu8Buf4               unsigned char*  数据缓冲区4的首地址
          *           pu8Buf5               unsigned char*  数据缓冲区5的首地址
          * 返 回 值：无
          * 说    明：1.该函数最多允许5个缓冲区,其中缓冲区1必须存在,其他缓冲区必须保证
          *             前一个缓冲区必须保障存在
          *           2.传入的缓冲区长度必须小于等于缓冲区的有效大小
          *           3.在进行数据Copy完成后,必须进行接收缓冲区的释放,避免内存浪费
         ******************************************************************************/
        [DllImport("cbbDevRecvService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getRecvData4Ip(int s32IpAddr, byte[] pu8Buf1, int s32BufLen1,
                                                               byte[] pu8Buf2, int s32BufLen2,
                                                               byte[] pu8Buf3, int s32BufLen3,
                                                               byte[] pu8Buf4, int s32BufLen4,
                                                               byte[] pu8Buf5, int s32BufLen5);
        /*******************************************************************************
          * 函    数：restartDataRecv
          * 描    述：根据recvDevData函数配置的设备,进行数据的再次接收
          * 输入参数：无
          * 输出参数：无
          * 返 回 值：无
          * 说    明：无
         ******************************************************************************/
        [DllImport("cbbDevRecvService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void restartDataRecv();

        /*******************************************************************************
          * 函    数：stopDataRecv
          * 描    述：停止数据接收
          * 输入参数：无
          * 输出参数：无
          * 返 回 值：无
          * 说    明：无
         ******************************************************************************/
        [DllImport("cbbDevRecvService.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void stopDataRecv( );

        public dataRecvReportCallback dataRecvProcessCallback;

        ///// <summary>
        ///// 复写方法：双缓冲器打开
        ///// </summary>
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;
        //        return cp;            }
        //}
      
    }
}
