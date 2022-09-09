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
#ifndef _RIGOL_DEVICE_RECV_SERVICE_H_
#define _RIGOL_DEVICE_RECV_SERVICE_H_

#ifndef _RIGOL_DLL_API
#define _RIGOL_DLL_API extern "C" _declspec(dllexport) 
#else
#endif

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
//typedef void (*DATA_RECV_OPTION_REPORT)(int s32OptCode , int *ps32IpAddr , int *ps32Status );
typedef void (*DATA_RECV_OPTION_REPORT)(int s32OptCode , int s32IpAddr , int s32Status );
/*******************************************************************************
  * 函    数：createDataRecvService
  * 描    述：启动数据接收服务
  * 输入参数：
  *           参数名称                  参数类型        参数说明
  *           pReportIO        DATA_RECV_OPTION_REPORT  注册的托管函数
  * 输出参数：无
  * 返 回 值：无
  * 说    明：必须在添加设备之前启动数据接收服务,否则不允许设备的添加
 ******************************************************************************/
_RIGOL_DLL_API void createDataRecvService( DATA_RECV_OPTION_REPORT pReportIO );

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
_RIGOL_DLL_API void addDevInfo2DataRecvService( int *ps32ClientIpAddr , 
                                                int *ps32ClientPort,
                                                int *ps32ClientProtocol,
                                                int *ps32ServicePort,
                                                int  s32ClientNum,
                                                int *ps32RegResult);

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
_RIGOL_DLL_API void deleteDevNode4DataRecvService( int *ps32ClientIpAddr , int  s32ClientNum );

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
_RIGOL_DLL_API void editDevNode2DataRecvService( int *ps32OldClientIpAddr ,
                                                 int *ps32NewClientIpAddr , 
                                                 int *ps32NewClientPort,
                                                 int *ps32NewClientProtocol,
                                                 int *ps32NewServicePort,
                                                 int  s32ClientNum,
                                                 int *ps32EditResult);

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
_RIGOL_DLL_API void recvDevData( int *ps32ClientIpAddr , 
                                 int *ps32RecvSize,
                                 int  s32ClientNum , 
                                 int  s32RecvTimeout);

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
_RIGOL_DLL_API int getRecvData4Ip( int s32IpAddr , unsigned char *pu8Buf1 , int s32BufLen1,
                                                   unsigned char *pu8Buf2 , int s32BufLen2,
                                                   unsigned char *pu8Buf3 , int s32BufLen3,
                                                   unsigned char *pu8Buf4 , int s32BufLen4,
                                                   unsigned char *pu8Buf5 , int s32BufLen5);

/*******************************************************************************
  * 函    数：restartDataRecv
  * 描    述：复位数据接收,重新进行数据接收
  * 输入参数：无
  * 输出参数：无
  * 返 回 值：无
  * 说    明：无
 ******************************************************************************/
_RIGOL_DLL_API void restartDataRecv( void );

/*******************************************************************************
  * 函    数：stopDataRecv
  * 描    述：停止数据接收
  * 输入参数：无
  * 输出参数：无
  * 返 回 值：无
  * 说    明：无
 ******************************************************************************/
_RIGOL_DLL_API void stopDataRecv( void );
#endif