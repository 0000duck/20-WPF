/********************************************************************
                普源精电科技股份有限公司 版权所有(2020 - 2030)
*********************************************************************
头文件名: device_config_service.h
功能描述: 1.向Tcp Socket服务上架客户端信息
          2.向Tcp Socket服务下架客户端信息
          3.启动阻塞模式下的数据接收
          4.提取Tcp Socket服务接收到的数据
作   者: sn01625
版   本: 0.1
创建日期: 2020-06-30  15:06 PM

修改记录1：// 修改历史记录，包括修改日期、修改者及修改内容
修改日期：
版 本 号：
修 改 人：
修改内容：
*********************************************************************/
#ifndef _RIGOL_DEVICE_CONFIG_SERVICE_H_
#define _RIGOL_DEVICE_CONFIG_SERVICE_H_

#ifndef _RIGOL_DLL_API
#define _RIGOL_DLL_API extern "C" _declspec(dllexport) 
#else
#endif

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
_RIGOL_DLL_API int openDevice(int *ps32IpAddr,int *ps32ClientPort , int s32DeviceNum,int *ps32Result);

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
  *                                                     1 -- 设备已建立连接
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
_RIGOL_DLL_API int regDevConfigCilent( int *ps32IpAddr , 
                                       int *s32Protocol , 
                                       int *ps32ClientPort , 
                                       int *ps32ServerPort,
                                       int  s32DeviceNum , 
                                       int *ps32Result);

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
_RIGOL_DLL_API int addDevConfigClient( int s32IpAddr , 
                                       int s32Protocol , 
                                       int s32ClientPort , 
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
_RIGOL_DLL_API int checkDevConfigClient( int s32IpAddr , 
                                         int s32Protocol , 
                                         int s32ClientPort , 
                                         int s32ServerPort , 
                                         unsigned char *pu8IDNInfo );

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
_RIGOL_DLL_API void delDevConfigClient(int s32IpAddr , int s32ClientPort);

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
_RIGOL_DLL_API int editDevConfigClient( int s32SrcIpAddr, int s32SrcClientPort , 
                                        int s32DstIpAddr, int s32DstClientPort , 
                                        int s32DstServerPort, int s32DstPotocal );

/*******************************************************************************
  * 函    数：unregDevConfigCilent
  * 描    述：向设备配置服务下架客户端信息
  * 输入参数：无
  * 输出参数：无
  * 返 回 值：无
  * 说    明：1.注意线程同步信号池的信号量关闭,以及信号池的释放
  *           2.注意每个客户端申请的用于事件接收的信号量提前释放后,再释放客户端表
 ******************************************************************************/
_RIGOL_DLL_API void unregDevConfigCilent( void );

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
_RIGOL_DLL_API  int write2Device( int *ps32IpAddr , 
                                  int *ps32Port,
                                  int s32IpAddrSize , 
                                  unsigned char *pu8WriteBuff , 
                                  int *ps32WriteLen ,
                                  int *ps32Result);

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
_RIGOL_DLL_API int write2DeviceAll( int *ps32IpAddr , 
                                    int *ps32Port,
                                    int s32IpAddrSize , 
                                    unsigned char *pu8WriteBuff , 
                                    int s32WriteLen ,
                                    int *ps32Result);

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
_RIGOL_DLL_API int read4Device( int *ps32IpAddr , 
                                int *ps32Port , 
                                int s32IpAddrSize ,
                                int s32Timeout ,
                                int *ps32Result );

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
_RIGOL_DLL_API int query4Device( int *ps32IpAddr , 
                                 int *ps32Port,
                                 int s32IpAddrSize , 
                                 unsigned char *pu8WriteBuff , 
                                 int* ps32WriteLen ,
                                 int s32Timeout ,
                                 int *ps32Result);

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
_RIGOL_DLL_API int query4DeviceAll( int *ps32IpAddr , 
                                    int *ps32Port,
                                    int s32IpAddrSize , 
                                    unsigned char *pu8WriteBuff , 
                                    int s32WriteLen ,
                                    int s32Timeout ,
                                    int *ps32Result);

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
_RIGOL_DLL_API int getReadData4IP( int s32IpAddr ,
                                   int s32PortNum,
                                   unsigned char *pu8ReadBuff , 
                                   int s32ReadOffset , 
                                   int s32ReadBuffSize );
#endif