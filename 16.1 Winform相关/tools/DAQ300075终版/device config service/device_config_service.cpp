/********************************************************************
                普源精电科技股份有限公司 版权所有(2020 - 2030)
*********************************************************************
头文件名: device_config_service.cpp
功能描述: 1.向设备配置服务上架客户端信息
          2.向Tcp Socket服务下架客户端信息
          3.启动阻塞模式下的数据接收
          4.提取Tcp Socket服务接收到的数据
作   者: sn01625
版   本: 0.1
创建日期: 2020-06-30  15:06 PM

修改记录1：// 修改历史记录，包括修改日期、修改者及修改内容
修改日期：2020-07-04
版 本 号：0.2
修 改 人：sn01625
修改内容：1.结构体stDeviceConfigInfo添加节点当前被使用的标识,避免由于
            客户端IP地址一样时,只唤醒单个线程,导致的异常
            受影响函数为regDevConfigCilent需初始化该变量
                        write2Device-使用前先初始化该变量,使用时置true
                                     检测到该变量为true时,跳过该变量
                        read4Device -同write2Device函数处理
                        query4Device-同write2Device函数处理
修改记录2：// 修改历史记录，包括修改日期、修改者及修改内容
修改日期：2020-07-07
版 本 号：0.3
修 改 人：sn01625
修改内容：1.修改write2Device和query4Device的写缓冲区及缓冲区大小 为
            每个IP地址对应一条命令
*********************************************************************/
#include <stdio.h>
#include <WinSock2.h>
#include <process.h>
#include "device_config_service.h"

#pragma comment(lib,"ws2_32.lib")

/********************************************************************
                             宏定义声明
*********************************************************************/

#define DEVICE_CONFIG_SERVICE_START_PORT                (short)10001   //  Socket的端口分配起始值

#define DEVICE_CONFIG_SERVICE_PROTOCAL_UDP              (int)0         //  使用UDP协议
#define DEVICE_CONFIG_SERVICE_PROTOCAL_TCP              (int)1         //  使用TCP协议

#define DEVICE_CONFIG_SERVICE_SEND_BUFF_LEN             (int)1024      //  发送缓冲区长度
#define DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN             (int)(1024*2)  //  接收缓冲区长度

#define DEVICE_CONFIG_SERVICE_WARNING_OPENED            (int)1         //  警告设备已建立连接
#define DEVICE_CONFIG_SERVICE_NO_ERROR                  (int)0         //  无错误
#define DEVICE_CONFIG_SERVICE_WSA_ERROR                 (int)-1        //  WSA初始化失败
#define DEVICE_CONFIG_SERVICE_CONNECT_ERROR             (int)-2        //  客户端建立失败
#define DEVICE_CONFIG_SERVICE_RECV_TIMEOUT_ERROR        (int)-3        //  接收超时
#define DEVICE_CONFIG_SERVICE_INIT_TIMEOUT_ERROR        (int)-4        //  子线程初始化失败
#define DEVICE_CONFIG_SERVICE_ADDR_BIND_FAIL_ERROR      (int)-5        //  服务器地址绑定失败
#define DEVICE_CONFIG_SERVICE_CLIENT_REGISTER_ERROR     (int)-6        //  客户端信息表注册失败
#define DEVICE_CONFIG_SERVICE_SOCKET_IO_ERROR           (int)-7        //  通信IO错误
#define DEVICE_CONFIG_SERVICE_RESOURCE_NO_FIND_ERROR    (int)-8        //  设备未发现
#define DEVICE_CONFIG_SERVICE_PARAMETER_ERROR           (int)-9        //  输入参数错误
#define DEVICE_CONFIG_SERVICE_DEVICE_NUM_ERROR          (int)-10       //  设备数量为0
#define DEVICE_CONFIG_SERVICE_MALLOC_ERROR              (int)-11       //  内存申请失败
#define DEVICE_CONFIG_SERVICE_NO_OPEN_ERROR             (int)-12       //  设备还没有打开
#define DEVICE_CONFIG_SERVICE_PROTOCAL_ERROR            (int)-13       //  协议错误

/********************************************************************
                             枚举定义声明
*********************************************************************/
/*
 * 定义设备配置服务的事件
 */
enum emDevConfigEvent
{
    DEVICE_CONFIG_SERVICE_EVENT_OPEN = 0,      //  打开设置
    DEVICE_CONFIG_SERVICE_EVENT_WRITE,         //  仅向设备写入操作
    DEVICE_CONFIG_SERVICE_EVENT_READ,          //  仅向设备读取操作
    DEVICE_CONFIG_SERVICE_EVENT_QUERY,         //  向设备进行读写操作
    DEVICE_CONFIG_SERVICE_EVENT_CLOSE,         //  关闭连接--仅针对单台设备
    DEVICE_CONFIG_SERVICE_EVENT_EXIT           //  退出线程
}DEVICE_CONFIG_EVENT_EM;

/********************************************************************
                            结构体定义声明
*********************************************************************/

/*
 * 定义记录设备信息的结构体
 * 包含:1.设备IP地址
 *      2.通信协议:0--UDP,1--TCP
 *      3.通信时的端口号
 */
struct stDeviceConfigInfo
{
    int                         ip;                                             //  设备IP地址
    short                       protocal;                                       //  设备的通信协议,0--UDP,1--TCP
    short                       cport;                                          //  设备端口号
    short                       sport;                                          //  服务端口号
    short                       err;                                            //  错误码
    bool                        valid;                                          //  当前客户端是否已建立子线程
    char                        send_buf[DEVICE_CONFIG_SERVICE_SEND_BUFF_LEN];  //  发送缓冲区
    int                         send_len;                                       //  待发送数据的长度
    char                        recv_buf[DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN];  //  接收缓冲区
    int                         recv_len;                                       //  接收数据的长度
    int                         timeout;                                        //  接收超时时间
    enum emDevConfigEvent       opt;                                            //  操作码值,具体定义见emDevConfigEvent定义
    HANDLE                      event;                                          //  事件句柄
    HANDLE *                    finish;                                         //  线程同步使用
    bool                        use;                                            //  标识该节点是否被使用 2020-07-04 添加
    struct stDeviceConfigInfo * next;                                           //  指向下一个节点
};

/********************************************************************
                          本地变量定义声明
*********************************************************************/
struct stDeviceConfigInfo    *m_pstDevConfigClientInfo    = NULL;  //  记录设备配置服务的客户端信息
HANDLE *                      m_semDevConfigFinishPool    = NULL;  //  多线程同步使用的信号量池,用于进行线程间同步
int                           m_s32DevConfigFinishPoolNum = 0;

/********************************************************************
                            内部函数定义
*********************************************************************/

/*******************************************************************************
  * 函    数：insertDeviceNode(内部接口)
  * 描    述：向设备客户端信息链表中插入新的设备节点
  * 输入参数：
  *           参数名称              参数类型           参数说明
  *           pstNode      struct stDeviceConfigInfo*  新的设备节点
  * 输出参数：无
  * 返 回 值：无
  * 说    明：无
 ******************************************************************************/
void insertDeviceNode( struct stDeviceConfigInfo * pstNode )
{
     if( NULL != pstNode )
    {
        pstNode->next = m_pstDevConfigClientInfo;
        m_pstDevConfigClientInfo = pstNode;
    }
}

/*******************************************************************************
  * 函    数：seekDeviceList(内部接口)
  * 描    述：向设备客户端信息链表中查找设备节点
  * 输入参数：
  *           参数名称              参数类型           参数说明
  *           s32IpAddr             int                设备IP地址
  *           s32Port               int                设备端口号
  * 输出参数：无
  * 返 回 值：若为NULL则认为没有查询到,否则返回设备节点
  * 说    明：无
 ******************************************************************************/
struct stDeviceConfigInfo * seekDeviceList( int s32IpAddr , int s32Port )
{
    struct stDeviceConfigInfo * pstNode = m_pstDevConfigClientInfo;

    while ( pstNode != NULL )
    {
        if( pstNode->ip == s32IpAddr && pstNode->cport == s32Port )
        {
            break;
        }
        pstNode = pstNode->next;
    }

    return pstNode;
}

/*******************************************************************************
  * 函    数：deleteDeviceNode(内部接口)
  * 描    述：向设备客户端信息链表中删除指定的设备节点
  * 输入参数：
  *           参数名称              参数类型           参数说明
  *           s32IpAddr             int                设备IP地址
  *           s32Port               int                设备端口号
  * 输出参数：无
  * 返 回 值：无
  * 说    明：在进行设备节点的删除时,需关闭事件信号量
 ******************************************************************************/
void deleteDeviceNode( int s32IpAddr , int s32Port )
{
    struct stDeviceConfigInfo * pstNode,*pstBackNode;

    pstNode = m_pstDevConfigClientInfo;

    // 判定链表是否为空
    if( m_pstDevConfigClientInfo == NULL )
    {
        return;
    }

    // 查找要删除的节点
    while ( (pstNode->ip != s32IpAddr)&&
            /*(pstNode->cport != s32Port)&&*/
            (pstNode->next != NULL) )
    {
        pstBackNode = pstNode;
        pstNode     = pstNode->next;
    }
    // 根据IP地址和端口号找到节点
    if( (pstNode->ip == s32IpAddr) && (pstNode->cport == s32Port))
    {
        if( pstNode == m_pstDevConfigClientInfo )// 找到的为头结点
        {
            m_pstDevConfigClientInfo = pstNode->next;
        }
        else
        {
            pstBackNode->next = pstNode->next;
        }

        CloseHandle(pstNode->event);

        free(pstNode);
    }
    else
    {
        return;
    }
}

/*******************************************************************************
  * 函    数：deleteAllDeviceNode(内部接口)
  * 描    述：删除所有的设备信息
  * 输入参数：无
  * 输出参数：无
  * 返 回 值：无
  * 说    明：在进行设备节点的删除时,需关闭事件信号量
 ******************************************************************************/
void deleteAllDeviceNode( void )
{
    struct stDeviceConfigInfo * pstList = m_pstDevConfigClientInfo;
    struct stDeviceConfigInfo * pstNode = m_pstDevConfigClientInfo;

    while (pstList != NULL)
    {
        pstList = pstList->next;

        CloseHandle(pstNode->event);
        free(pstNode);

        pstNode = pstList;
    }

    m_pstDevConfigClientInfo = NULL;
}

/*******************************************************************************
  * 函    数：deleteAllDeviceNode(内部接口)
  * 描    述：删除所有的设备信息
  * 输入参数：无
  * 输出参数：无
  * 返 回 值：无
  * 说    明：在进行设备节点的删除时,需关闭事件信号量
 ******************************************************************************/
int updateDeviceListErr( int *ps32IpAddr,int *ps32Port,int s32DeviceNum,int *ps32Result )
{
    struct stDeviceConfigInfo *pstNode = NULL;
    int                        i;
    int                        s32RetCode = 0;

    // 1. 遍历设备
    for (i = 0; i < s32DeviceNum; i++)
    {
        // 1.1 查询设备节点
        pstNode = seekDeviceList(ps32IpAddr[i],ps32Port[i]);

        // 1.2 判断设备节点是否有效
        if (pstNode != NULL)
        {
            if (pstNode->err != DEVICE_CONFIG_SERVICE_NO_ERROR)
            {
                ps32Result[i] = pstNode->err;
                s32RetCode = -1;
            }
        }
        else
        {
            ps32Result[i] = DEVICE_CONFIG_SERVICE_RESOURCE_NO_FIND_ERROR;
            s32RetCode    = -1;
        }
    }
    return s32RetCode;
}

/*******************************************************************************
  * 函    数：SyncWaitForConfigThreadFinish(内部接口)
  * 描    述：同步等待所有接收线程完成数据接收
  * 输入参数：
  *           参数名称              参数类型        参数说明
  *           handles               HANDLE*         信号量池地址
  *           count                 int             本次需要同步的信号量数量
  * 输出参数：无
  * 返 回 值：信号量的最小索引值
  * 说    明：1.由于WaitForMultipleObjects每次最大等待数量为MAXIMUM_WAIT_OBJECTS
  *             所以使用函数进行封装
  *           2.WaitForMultipleObjects的返回值可以忽略,该函数依然如此
 ******************************************************************************/
DWORD SyncWaitForConfigThreadFinish(HANDLE * handles, int count)
{
    int waitingThreadsCount = count;
    int index = 0;
    DWORD res = 0;
    while(waitingThreadsCount >= MAXIMUM_WAIT_OBJECTS)
    {
        res = WaitForMultipleObjects(MAXIMUM_WAIT_OBJECTS, &handles[index], TRUE, INFINITE);
        waitingThreadsCount -= MAXIMUM_WAIT_OBJECTS;
        index += MAXIMUM_WAIT_OBJECTS;
    }

    if(waitingThreadsCount > 0)
    {
        res = WaitForMultipleObjects(waitingThreadsCount, &handles[index], TRUE, INFINITE);
    }

    return res;
}


/*******************************************************************************
  * 函    数：devConfigProcess(内部接口)
  * 描    述：1. 根据传输协议,进行通信句柄的资源申请
  *           2. 若传输协议为TCP,则进行与客户端的连接建立
  *           3. 进行事件的接收和多线程的同步
  *           4. 处理数据的读写操作
  * 输入参数：
  *           参数名称              参数类型        参数说明
  *           pThreadPara           void*           线程的初始化参数,具体定义见
  *                                                 struct stDeviceConfigInfo
  * 输出参数：无
  * 返 回 值：无
  * 说    明：无
 ******************************************************************************/
void devConfigProcess( void* pThreadPara )
{
    struct stDeviceConfigInfo*          pstClientInfo       = (struct stDeviceConfigInfo*)pThreadPara;
    SOCKET                              s32SocketFd         = NULL;
    sockaddr_in                         stClientAddr        = { 0 };
    sockaddr_in                         stServiceAddr       = { 0 };
    int                                 s32AddrLen          = 0;
    int                                 s32SendLen          = 0;
    int                                 s32RecvLen          = 0;
    int                                 s32RecvTimeout      = 0;
    bool                                bThreadExitFlag     = false;
    /*
     * 下面的参数定义仅用于加速Connect连接超时
     */
    int                                 s32WorkMode         = 0;
    struct timeval                      stConnTimeout       = {1,500}; // 1500毫秒
    fd_set                              stSelectFd          = {0};
    int                                 s32LastErrCode      = 0;

    // 1. 初始化错误码
    pstClientInfo->err = DEVICE_CONFIG_SERVICE_NO_ERROR;

    // 2. 初始化WSA服务
    WORD sockVersion = MAKEWORD(2,2);
    WSADATA wsaData;

    if( WSAStartup(sockVersion,&wsaData) != 0 )
    {
        pstClientInfo->err = DEVICE_CONFIG_SERVICE_WSA_ERROR;
        ReleaseSemaphore(*pstClientInfo->finish,1,NULL);
        return ;
    }

    // 3. 线程初始化完成,告知父线程
    ReleaseSemaphore(*pstClientInfo->finish,1,NULL);

    // 4. 等待事件的到来,根据事件类型进行控制 
    while (bThreadExitFlag==false)
    {
        // 4.1 等待事件
        WaitForSingleObject(pstClientInfo->event,INFINITE);
        // 4.2 根据事件类型进行处理
        switch (pstClientInfo->opt)
        {
            case DEVICE_CONFIG_SERVICE_EVENT_OPEN:  //  打开设备
                // 若当前为UDP协议,则绑定本地地址
                if ( pstClientInfo->protocal == DEVICE_CONFIG_SERVICE_PROTOCAL_UDP )
                {
                    // 申请UDP Socket句柄
                    s32SocketFd = socket(AF_INET,SOCK_DGRAM,IPPROTO_UDP);

                    // 初始化服务器地址
                    memset(&stServiceAddr,0,sizeof(sockaddr_in));
                    stServiceAddr.sin_family           = AF_INET;  //  IPv4
                    stServiceAddr.sin_addr.S_un.S_addr = INADDR_ANY;
                    stServiceAddr.sin_port             = htons(pstClientInfo->sport);
                    // 尝试绑定
                    if(bind(s32SocketFd,(sockaddr*)&stServiceAddr,sizeof(sockaddr_in)) == SOCKET_ERROR)
                    {
                        // 标记错误
                        pstClientInfo->err = DEVICE_CONFIG_SERVICE_ADDR_BIND_FAIL_ERROR;
                        // 关闭本次的Socket句柄
                        closesocket(s32SocketFd);
                        break;
                    }
                    else
                    {
                        // 置通信可以使用的标识
                        pstClientInfo->valid = true;
                    }
                }
                // 若当前为TCP协议,则与设备建立长连接
                else if (pstClientInfo->protocal == DEVICE_CONFIG_SERVICE_PROTOCAL_TCP)
                {
                    // 申请TCP Socket句柄
                    s32SocketFd = socket(AF_INET,SOCK_STREAM,IPPROTO_TCP);

                    // 初始化客户端地址
                    memset(&stClientAddr,0,sizeof(sockaddr_in));
                    stClientAddr.sin_family           = AF_INET;  //  IPv4
                    stClientAddr.sin_addr.S_un.S_addr = pstClientInfo->ip;
                    stClientAddr.sin_port             = htons(pstClientInfo->cport);

                    // 先设置地址重用
                    s32WorkMode = 1;
                    setsockopt(s32SocketFd,SOL_SOCKET,SO_REUSEADDR,(char*)&s32WorkMode,sizeof(int));

                    // 先设置当前Socket为非阻塞模式,避免connect连接失败时间过长
        
                   // ioctlsocket(s32SocketFd,FIONBIO,(u_long*)&s32WorkMode);
                    // 开始连接
                    if( connect(s32SocketFd,(sockaddr*)&stClientAddr,sizeof(stClientAddr) ) == SOCKET_ERROR )
                    {
                        FD_ZERO(&stSelectFd);
                        FD_SET(s32SocketFd,&stSelectFd);
                        if( select(0,NULL,&stSelectFd,NULL,&stConnTimeout) <= 0 )
                        {
                            // 标记错误
                            pstClientInfo->err = DEVICE_CONFIG_SERVICE_CONNECT_ERROR;
                            // 关闭本次的Socket句柄
                            closesocket(s32SocketFd);
                            break;
                        }
                        else
                        {
                            // 再次设置当前Socket为阻塞模式
                            s32WorkMode = 0;
                            ioctlsocket(s32SocketFd, FIONBIO, (u_long*)&s32WorkMode);
                            // 置通信可以使用的标识
                            pstClientInfo->valid = true;
                        }
                    }
                    else
                    {
                        // 再次设置当前Socket为阻塞模式
                        s32WorkMode = 0;
                        ioctlsocket(s32SocketFd,FIONBIO,(u_long*)&s32WorkMode);
                        // 置通信可以使用的标识
                        pstClientInfo->valid = true;
                    }
                }
                else
                {
                    pstClientInfo->err = DEVICE_CONFIG_SERVICE_PROTOCAL_ERROR;
                }
                break;

            case DEVICE_CONFIG_SERVICE_EVENT_WRITE: //  仅向设备进行写操作
                //  根据协议调用发送接口
                if (pstClientInfo->protocal == DEVICE_CONFIG_SERVICE_PROTOCAL_UDP ) // UDP协议
                {
                    s32SendLen = sendto(s32SocketFd,pstClientInfo->send_buf,pstClientInfo->send_len,0,(sockaddr*)&stClientAddr,sizeof(sockaddr));
                }
                else
                {
                    s32SendLen = send(s32SocketFd,pstClientInfo->send_buf,pstClientInfo->send_len,0);
                }
                if( s32SendLen != pstClientInfo->send_len)
                {
                    pstClientInfo->err = DEVICE_CONFIG_SERVICE_SOCKET_IO_ERROR;
                }
                break;

            case DEVICE_CONFIG_SERVICE_EVENT_READ:
                // 设置接收超时时间
                s32RecvTimeout = pstClientInfo->timeout;
                setsockopt(s32SocketFd,SOL_SOCKET,SO_RCVTIMEO,(char*)&s32RecvTimeout,sizeof(int));

                //  根据协议调用接收接口
                if (pstClientInfo->protocal == DEVICE_CONFIG_SERVICE_PROTOCAL_UDP ) // UDP协议
                {
                    s32AddrLen = sizeof(sockaddr_in);
                    s32RecvLen = recvfrom(s32SocketFd,pstClientInfo->recv_buf,pstClientInfo->recv_len,0,(sockaddr*)&stClientAddr,&s32AddrLen);
                }
                else
                {
                    s32RecvLen = recv(s32SocketFd,pstClientInfo->recv_buf,pstClientInfo->recv_len,0);
                }
                // 更新接收长度
                if( s32RecvLen == -1 )//若接收超时,则置接收长度为0
                {
                    pstClientInfo->recv_len = 0;
                    pstClientInfo->err      = DEVICE_CONFIG_SERVICE_RECV_TIMEOUT_ERROR;
                }
                else
                {
                    pstClientInfo->err      = DEVICE_CONFIG_SERVICE_NO_ERROR;
                    pstClientInfo->recv_len = s32RecvLen;
                }
                break;
                
            case DEVICE_CONFIG_SERVICE_EVENT_QUERY:
                // 设置接收超时时间
                s32RecvTimeout = pstClientInfo->timeout;
                setsockopt(s32SocketFd,SOL_SOCKET,SO_RCVTIMEO,(char*)&s32RecvTimeout,sizeof(int));
                //  根据协议调用接收接口
                if (pstClientInfo->protocal == DEVICE_CONFIG_SERVICE_PROTOCAL_UDP ) // UDP协议
                {
                    // 发送数据
                    sendto(s32SocketFd,pstClientInfo->send_buf,pstClientInfo->send_len,0,(sockaddr*)&stClientAddr,sizeof(sockaddr));
                    // 等待接收数据
                    s32AddrLen = sizeof(sockaddr_in);
                    s32RecvLen = recvfrom(s32SocketFd,pstClientInfo->recv_buf,pstClientInfo->recv_len,0,(sockaddr*)&stClientAddr,&s32AddrLen);
                }
                else
                {
                    // 发送数据
                    send(s32SocketFd,pstClientInfo->send_buf,pstClientInfo->send_len,0);
                    // 等待接收数据
                    s32RecvLen = recv(s32SocketFd,pstClientInfo->recv_buf,pstClientInfo->recv_len,0);
                }
                // 更新接收长度
                if( s32RecvLen == -1 )//若接收超时,则置接收长度为0
                {
                    pstClientInfo->recv_len = 0;
                    pstClientInfo->err      = DEVICE_CONFIG_SERVICE_RECV_TIMEOUT_ERROR;
                }
                else
                {
                    pstClientInfo->err      = DEVICE_CONFIG_SERVICE_NO_ERROR;
                    pstClientInfo->recv_len = s32RecvLen;
                }
                break;
            case DEVICE_CONFIG_SERVICE_EVENT_CLOSE:
                // 关闭Socket句柄
                closesocket(s32SocketFd);
                pstClientInfo->valid = false;
                break;
            case DEVICE_CONFIG_SERVICE_EVENT_EXIT:
                // 置线程退出标识为true
                bThreadExitFlag = true;
                break;

            default:
                break;
        }
        // 4.3. 线程操作完成,告知父线程
        if(pstClientInfo->opt != DEVICE_CONFIG_SERVICE_EVENT_EXIT)
        {
            ReleaseSemaphore(*pstClientInfo->finish,1,NULL);
        }
    }
    
    WSACleanup();
    
    // 5. 线程操作完成,告知父线程
    ReleaseSemaphore(*pstClientInfo->finish,1,NULL);
}

/********************************************************************
                            接口函数定义
*********************************************************************/

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
int openDevice(int *ps32IpAddr,int *ps32ClientPort , int s32DeviceNum,int *ps32Result)
{
    struct stDeviceConfigInfo *pstNode = NULL;
    int                        i;
    int                        s32SemPoolUseCnt = 0;

    // 1. 遍历需要打开的设备
    for ( i = 0; i < s32DeviceNum; i++)
    {
        // 1.1 查询设备节点
        pstNode = seekDeviceList(ps32IpAddr[i],ps32ClientPort[i]);

        // 1.2 判断设备节点是否有效
        if (pstNode != NULL)
        {
            // 只有设备还处于无连接状态才进行打开操作
            if ( pstNode->valid == false )
            {
                // 1.2.1 设置设备打开事件
                pstNode->opt = DEVICE_CONFIG_SERVICE_EVENT_OPEN;
                // 1.2.2 分配信号
                pstNode->finish = &m_semDevConfigFinishPool[s32SemPoolUseCnt++];
                // 1.2.3 唤醒事件处理线程
                ReleaseSemaphore(pstNode->event,1,NULL);
            }
            else
            {
                ps32Result[i] = DEVICE_CONFIG_SERVICE_WARNING_OPENED;
            }
        }
        else
        {
            ps32Result[i] = DEVICE_CONFIG_SERVICE_RESOURCE_NO_FIND_ERROR;
        }
    }

    // 2. 等待操作完成
    if( s32SemPoolUseCnt > 0 )
    {
        SyncWaitForConfigThreadFinish(m_semDevConfigFinishPool,s32SemPoolUseCnt);
    }

    return updateDeviceListErr(ps32IpAddr,ps32ClientPort,s32DeviceNum,ps32Result);
}

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
int regDevConfigCilent( int *ps32IpAddr , 
                        int *s32Protocol , 
                        int *ps32ClientPort , 
                        int *ps32ServerPort,
                        int  s32DeviceNum , 
                        int *ps32Result)
{
    int             i;
    int             s32RetCode     = 0;
    int             s32InitSuccessCnt = 0;
    short           s16ServicePort = DEVICE_CONFIG_SERVICE_START_PORT;
    struct stDeviceConfigInfo *pstNode;
    int             s32SemPoolUseCnt = 0;
   
    // 1. 若设备数量为0,则直接返回,拒绝注册
    if( s32DeviceNum == 0 )
    {
        return -2;
    }

    // 2. 申请多线程同步信号池
    // 2.1 申请信号池内存
    m_semDevConfigFinishPool = (HANDLE*)malloc(s32DeviceNum*sizeof(HANDLE));
    // 2.2 若内存申请失败,则直接返回
    if( m_semDevConfigFinishPool == NULL)
    {
        return -3;
    }
    // 2.3 初始化信号池
    for (i = 0; i < s32DeviceNum; i++)
    {
        m_semDevConfigFinishPool[i] = CreateSemaphore(NULL,0,1,NULL); // 申请匿名信号量
    }
    // 2.4 记录信号池的数量
    m_s32DevConfigFinishPoolNum = s32DeviceNum;

    // 3. 进行设备树的初始化
    for (i = 0; i < s32DeviceNum; i++)
    {
        // 3.1 申请设备节点
        pstNode = (struct stDeviceConfigInfo*)malloc(sizeof(struct stDeviceConfigInfo));

        // 3.2 判定节点是否申请成功,若不成功则退回到循环
        if (pstNode == NULL )
        {
            ps32Result[i] = DEVICE_CONFIG_SERVICE_MALLOC_ERROR;
            continue;
        }
       
        // 3.3 初始化节点参数
        pstNode->ip       = ps32IpAddr[i];
        pstNode->protocal = s32Protocol[i];
        pstNode->cport    = ps32ClientPort[i];
        pstNode->sport    = ps32ServerPort[i];
        pstNode->valid    = false;
        pstNode->event    = CreateSemaphore(NULL, 0, 1, NULL);//创建匿名信号量，初始资源为零，最大并发数为1
        pstNode->err      = DEVICE_CONFIG_SERVICE_NO_ERROR;
        pstNode->use      = false;   //  Added 2020-07-04
        
        // 3.4 分配信号量,用于操作同步
        pstNode->finish   = &m_semDevConfigFinishPool[s32SemPoolUseCnt++];

        // 3.5 启用子线程进行事件的处理
        _beginthread(devConfigProcess,1024,pstNode);

        // 3.6 插入设备信息链表
        insertDeviceNode(pstNode);
    }

    // 4. 等待所有线程的初始化完成
    SyncWaitForConfigThreadFinish(m_semDevConfigFinishPool,s32SemPoolUseCnt);
    
    // 5. 打开设备
    s32RetCode = openDevice(ps32IpAddr,ps32ClientPort,s32DeviceNum,ps32Result);

    return s32RetCode;
}

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
int addDevConfigClient( int s32IpAddr , 
                        int s32Protocol , 
                        int s32ClientPort , 
                        int s32ServerPort)
{
    struct stDeviceConfigInfo *     pstNode     = NULL;
    int                             s32RetCode  = 0;
    HANDLE                    *     pstSemPool  = NULL;
    int                             i;
    HANDLE                          semSync;

    //  1. 查找当前设备是否已被注册
    pstNode = seekDeviceList(s32IpAddr,s32ClientPort);
    //  2. 查找到已注册,则返回添加失败
    if( pstNode != NULL )
    {
        return -1;
    }

    //  3. 申请新的节点
    pstNode = (struct stDeviceConfigInfo*)malloc(sizeof(struct stDeviceConfigInfo));
    //  4. 节点有效判定
    if( pstNode == NULL )
    {
        return -2;
    }
    // 5 创建线程同步信号量
    semSync = CreateSemaphore(NULL, 0, 1, NULL);

    // 6 初始化节点信息
    pstNode->ip       = s32IpAddr;
    pstNode->protocal = s32Protocol;
    pstNode->cport    = s32ClientPort;
    pstNode->sport    = s32ServerPort;
    pstNode->valid    = false;
    pstNode->event    = CreateSemaphore(NULL, 0, 1, NULL);//创建匿名信号量，初始资源为零，最大并发数为1
    pstNode->err      = DEVICE_CONFIG_SERVICE_NO_ERROR;

    // 7 分配操作完成同步信号
    pstNode->finish   = &semSync;

    // 8 启用子线程进行事件的处理
    _beginthread(devConfigProcess,1024,pstNode);

    // 9 等待线程初始化完成
    WaitForSingleObject(semSync,INFINITE);

    // 10 打开设备连接
    pstNode->opt      = DEVICE_CONFIG_SERVICE_EVENT_OPEN; // 设置操作为打开事件
    ReleaseSemaphore(pstNode->event,1,NULL); // 唤醒事件处理线程
    WaitForSingleObject(semSync,INFINITE);// 等待操作完成

    // 11 对错误码进行判定,若出现错误,则关闭子线程,并释放节点内存,不进行插入操作
    //    若不存在错误,则插入到设备客户端信息列表,并更新信号量池的数量
    if ( pstNode->err == DEVICE_CONFIG_SERVICE_NO_ERROR )
    {
        // 11.1 申请新信号池内存空间
        pstSemPool = (HANDLE*)malloc(sizeof(HANDLE)*(m_s32DevConfigFinishPoolNum+1));
        // 11.2 判定内存申请是否完成
        if( pstSemPool == NULL )
        {
            s32RetCode = -2;
        }
        else
        {
            // 11.3 添加设备节点
            insertDeviceNode(pstNode);
            // 11.4 复制已有的信号
            for (i = 0; i < m_s32DevConfigFinishPoolNum; i++) 
            {
                pstSemPool[i] = m_semDevConfigFinishPool[i];
            }
            // 11.5 创建新的信号量
            pstSemPool[i] = CreateSemaphore(NULL, 0, 1, NULL);
            // 11.6 更新最大信号池数量
            m_s32DevConfigFinishPoolNum ++;
            // 11.7 释放旧的信号池
            free(m_semDevConfigFinishPool);
            // 11.8 更新新的信号池
            m_semDevConfigFinishPool = pstSemPool;
        }
    }
    else
    {
        pstNode->opt      = DEVICE_CONFIG_SERVICE_EVENT_EXIT; // 设置操作为打开事件
        ReleaseSemaphore(pstNode->event,1,NULL); // 唤醒事件处理线程
        WaitForSingleObject(semSync,INFINITE);// 等待操作完成
        CloseHandle(pstNode->event);// 关闭事件信号量
        free(pstNode);// 释放设备节点
    }
    // 12 关闭线程同步信号量
    CloseHandle(semSync);
    return s32RetCode;
}
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
int checkDevConfigClient( int s32IpAddr , 
                          int s32Protocol , 
                          int s32ClientPort , 
                          int s32ServerPort , 
                          unsigned char *pu8IDNInfo )
{
    struct stDeviceConfigInfo       stDevClient = { 0 };
    int                             s32RetCode  = 0;
    HANDLE                          semSync;

    // 1 创建线程同步信号量
    semSync = CreateSemaphore(NULL, 0, 1, NULL);

    // 2 初始化节点信息
    stDevClient.ip       = s32IpAddr;
    stDevClient.protocal = s32Protocol;
    stDevClient.cport    = s32ClientPort;
    stDevClient.sport    = s32ServerPort;
    stDevClient.valid    = false;
    stDevClient.event    = CreateSemaphore(NULL, 0, 1, NULL);//创建匿名信号量，初始资源为零，最大并发数为1
    stDevClient.err      = DEVICE_CONFIG_SERVICE_NO_ERROR;

    // 3 分配操作完成同步信号
    stDevClient.finish   = &semSync;

    // 4 启用子线程进行事件的处理
    _beginthread(devConfigProcess,1024,&stDevClient);

    // 5 等待线程初始化完成
    WaitForSingleObject(semSync,INFINITE);

    // 6 打开设备连接
    stDevClient.opt      = DEVICE_CONFIG_SERVICE_EVENT_OPEN; // 设置操作为打开事件
    ReleaseSemaphore(stDevClient.event,1,NULL); // 唤醒事件处理线程
    WaitForSingleObject(semSync,INFINITE);// 等待操作完成

    // 7 若设备打开连接成功,则进行查询事件的发送
    if (stDevClient.err == DEVICE_CONFIG_SERVICE_NO_ERROR )
    {
        // 7.1 设置操作为查询事件 
        stDevClient.opt  = DEVICE_CONFIG_SERVICE_EVENT_QUERY;
        // 7.2 设置写缓冲区
        memset(stDevClient.send_buf,0,DEVICE_CONFIG_SERVICE_SEND_BUFF_LEN);
        stDevClient.send_len = sprintf(stDevClient.send_buf,"%s\n","*IDN?;:LAN:MAC?");
        // 7.3 重置接收缓冲区
        memset(stDevClient.recv_buf,0,DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN);
        stDevClient.recv_len = DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN;
        // 7.4 设置超时时间为2000ms
        stDevClient.timeout  = 2000;

        // 7.5 唤醒事件处理线程
        ReleaseSemaphore(stDevClient.event,1,NULL);
        // 7.6 等待操作完成
        WaitForSingleObject(semSync,INFINITE);

        // 7.7 对查询结果的判定,若没有错误,则进行响应数据的Copy,否则置错误码,交给后面的流程处理
         if (stDevClient.err == DEVICE_CONFIG_SERVICE_NO_ERROR )
        {
            memcpy(pu8IDNInfo,stDevClient.recv_buf,stDevClient.recv_len);
            s32RetCode = stDevClient.recv_len;
        }
        else
        {
            s32RetCode = -4; // 设备查询超时
        }
    }
    else
    {
        s32RetCode = -3; // 置设备通信I/O打开失败
    }
    // 8 若当前存在有效连接,则进行关闭操作
    if( stDevClient.valid == true )
    {
        stDevClient.opt      = DEVICE_CONFIG_SERVICE_EVENT_OPEN; // 设置操作为打开事件
        ReleaseSemaphore(stDevClient.event,1,NULL); // 唤醒事件处理线程
        WaitForSingleObject(semSync,INFINITE);// 等待操作完成
    }
    
    // 9 线程退出操作
    stDevClient.opt      = DEVICE_CONFIG_SERVICE_EVENT_EXIT; // 设置操作为打开事件
    ReleaseSemaphore(stDevClient.event,1,NULL); // 唤醒事件处理线程
    WaitForSingleObject(semSync,INFINITE);// 等待操作完成
    CloseHandle(stDevClient.event);// 关闭事件信号量

    // 10 关闭线程同步信号量
    CloseHandle(semSync);
    return s32RetCode;
}

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
void delDevConfigClient(int s32IpAddr , int s32ClientPort)
{
    struct stDeviceConfigInfo *     pstNode     = NULL;
    HANDLE                    *     pstSemPool  = NULL;
    int                             i;
    HANDLE                          semSync;

    // 1 查询该节点是否存在
    pstNode = seekDeviceList(s32IpAddr,s32ClientPort);

    // 2 若该节点存在则进行删除操作,否则不进行任何操作
    if (pstNode != NULL)
    {
        // 2.1 创建线程同步信号
        semSync = CreateSemaphore(NULL,0,1,NULL);
        // 2.2 绑定信号量
        pstNode->finish = &semSync; 

        // 2.3 若当前连接有效则先关闭连接
        if( pstNode->valid == true )
        {
            pstNode->opt      = DEVICE_CONFIG_SERVICE_EVENT_CLOSE; // 设置操作为打开事件
            ReleaseSemaphore(pstNode->event,1,NULL); // 唤醒事件处理线程
            WaitForSingleObject(semSync,INFINITE);// 等待操作完成 
        }

        // 2.4 再进行退出线程操作
        pstNode->opt      = DEVICE_CONFIG_SERVICE_EVENT_EXIT; // 设置操作为退出事件
        ReleaseSemaphore(pstNode->event,1,NULL); // 唤醒事件处理线程
        WaitForSingleObject(semSync,INFINITE);// 等待操作完成
        // 2.3 释放节点
        deleteDeviceNode(s32IpAddr,s32ClientPort);
        // 2.4 申请新信号池内存空间
        pstSemPool = (HANDLE*)malloc(sizeof(HANDLE)*(m_s32DevConfigFinishPoolNum-1));
        // 2.5 复制已有的信号
        for (i = 0; i < (m_s32DevConfigFinishPoolNum-1); i++) 
        {
            pstSemPool[i] = m_semDevConfigFinishPool[i];
        }
        // 2.6 释放已有的信号量
        CloseHandle(m_semDevConfigFinishPool[i]);
        // 2.7 更新最大信号池数量
        m_s32DevConfigFinishPoolNum --;
        // 2.8 释放旧的信号池
        free(m_semDevConfigFinishPool);
        // 2.9 更新新的信号池
        m_semDevConfigFinishPool = pstSemPool;
    }
}

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
int editDevConfigClient(int s32SrcIpAddr, int s32SrcClientPort , 
                        int s32DstIpAddr, int s32DstClientPort , 
                        int s32DstServerPort, int s32DstPotocal )
{
    struct stDeviceConfigInfo *         pstNode = NULL;
    int                                 s32RetCode = 0;

    // 1 根据源IP地址和端口号，查询节点
    pstNode = seekDeviceList(s32SrcIpAddr,s32SrcClientPort);

    // 2 判断是否查询到该节点,若没有则直接返回
    if (pstNode != NULL)
    {
        /*
         * 查询到设备节点,进行设备的信息变更
         * 步骤1. 检测当前是否有有效连接存在,若存在则先关闭连接
         * 步骤2. 更改设备节点信息
         * 步骤3. 重新打开连接
         */
        // 2.1 判断当前是否有有效连接
        if (pstNode->valid == true)
        {
            pstNode->opt      = DEVICE_CONFIG_SERVICE_EVENT_CLOSE; // 设置操作为打开事件
            pstNode->finish   = &m_semDevConfigFinishPool[0]; // 分配信号量
            ReleaseSemaphore(pstNode->event,1,NULL); // 唤醒事件处理线程
            WaitForSingleObject(m_semDevConfigFinishPool[0],INFINITE);// 等待操作完成 
        }
        
        // 2.2 更新设备信息
        pstNode->ip       = s32DstIpAddr;
        pstNode->cport    = s32SrcClientPort;
        pstNode->sport    = s32DstServerPort;
        pstNode->protocal = s32DstPotocal;

        // 2.3 打开设备连接
        pstNode->opt      = DEVICE_CONFIG_SERVICE_EVENT_OPEN; // 设置操作为打开事件
        pstNode->finish   = &m_semDevConfigFinishPool[0]; // 分配信号量
        ReleaseSemaphore(pstNode->event,1,NULL); // 唤醒事件处理线程
        WaitForSingleObject(m_semDevConfigFinishPool[0],INFINITE);// 等待操作完成 

        // 2.4 进行结果的判断
        if (pstNode->err != DEVICE_CONFIG_SERVICE_NO_ERROR )
        {
            s32RetCode = -2;
        }
    }
    else
    {
        s32RetCode = -1;
    }
    return s32RetCode;
}
/*******************************************************************************
  * 函    数：unregDevConfigCilent
  * 描    述：向设备配置服务下架客户端信息(外部接口)
  * 输入参数：无
  * 输出参数：无
  * 返 回 值：无
  * 说    明：1.注意线程同步信号池的信号量关闭,以及信号池的释放
  *           2.注意每个客户端申请的用于事件接收的信号量提前释放后,再释放客户端表
 ******************************************************************************/
void unregDevConfigCilent( void )
{
    int                         s32SyncSemUseCnt = 0;
    int                         i;
    struct stDeviceConfigInfo * pstNode          = m_pstDevConfigClientInfo;
    if (pstNode == NULL)
    {
        return;
    }
    // 1. 遍历客户端信息表
    while (pstNode != NULL)
    {
        // 置事件标识
        pstNode->opt    = DEVICE_CONFIG_SERVICE_EVENT_EXIT;
        pstNode->finish = &m_semDevConfigFinishPool[s32SyncSemUseCnt++];
              
        // 抛出事件处理
        ReleaseSemaphore(pstNode->event,1,NULL);

        pstNode = pstNode->next;
    }

    // 2. 等待所有子线程退出
    if( 0 < s32SyncSemUseCnt )
    {
        SyncWaitForConfigThreadFinish(m_semDevConfigFinishPool,s32SyncSemUseCnt);
    }
    // 3. 进行相关资源释放
    // 3.1 根据信号池的大小，进行池子的释放
    for ( i = 0; i < m_s32DevConfigFinishPoolNum; i++)
    {
        CloseHandle(m_semDevConfigFinishPool[i]);
    }
    // 3.2 释放信号池占用的内存
    free(m_semDevConfigFinishPool);
    m_semDevConfigFinishPool = NULL;

    // 3.3 释放所有设备节点
    deleteAllDeviceNode();
}

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
int write2Device( int *ps32IpAddr , 
                  int *ps32Port,
                  int s32IpAddrSize , 
                  unsigned char *pu8WriteBuff , 
                  int *ps32WriteLen ,
                  int *ps32Result)
{
    int             s32RetCode    = 0;
    int             s32SyncSemCnt = 0;
    int             i;
    int             s32WriteBuffOffset = 0;
    struct stDeviceConfigInfo *     pstNode;

    // 1. 参数合法化检测
    if( NULL == pu8WriteBuff || NULL == ps32WriteLen )
    {
        return DEVICE_CONFIG_SERVICE_PARAMETER_ERROR;
    }

    // 2. 遍历节点
    for (i = 0; i < s32IpAddrSize; i++)
    {
        // 2.1 根据IP地址和端口号进行节点的获取
        pstNode = seekDeviceList(ps32IpAddr[i],ps32Port[i]);

        // 2.2 判定节点是否查找到
        if (pstNode!=NULL)
        {
            
            // 2.2.1 重置错误码
            pstNode->err = DEVICE_CONFIG_SERVICE_NO_ERROR;
            // 2.2.2 根据节点有效性进行事件发送
            if (pstNode->valid == true)
            {
                if( ps32WriteLen[i] != 0)
                {
                    // 2.2.2.1 进行写数据的复制
                    memset(pstNode->send_buf,0,DEVICE_CONFIG_SERVICE_SEND_BUFF_LEN); // 初始化空间
                    memcpy(pstNode->send_buf,pu8WriteBuff+s32WriteBuffOffset,ps32WriteLen[i]);
                    pstNode->send_len  = ps32WriteLen[i];

                    // 2.2.2.2 分配线程同步信号
                    pstNode->finish = &m_semDevConfigFinishPool[s32SyncSemCnt++];

                    // 2.2.2.3 发出事件
                    pstNode->opt    = DEVICE_CONFIG_SERVICE_EVENT_WRITE;
                    ReleaseSemaphore(pstNode->event,1,NULL);
                }
            }
            else
            {
                ps32Result[i] = DEVICE_CONFIG_SERVICE_NO_OPEN_ERROR;
                s32RetCode    = -1;
            }
        }
        else
        {
            ps32Result[i] = DEVICE_CONFIG_SERVICE_RESOURCE_NO_FIND_ERROR;
            s32RetCode    = -1;
        }
        
        s32WriteBuffOffset += ps32WriteLen[i];
    }

    // 3. 等待所有事件线程处理完成
    if( 0 < s32SyncSemCnt )
    {
        SyncWaitForConfigThreadFinish(m_semDevConfigFinishPool,s32SyncSemCnt);
    }

    // 4. 提取写结果
    if( s32RetCode == 0 )
    {
        s32RetCode = updateDeviceListErr(ps32IpAddr,ps32Port,s32IpAddrSize,ps32Result);
    }
    else
    {
        updateDeviceListErr(ps32IpAddr,ps32Port,s32IpAddrSize,ps32Result);
    }

    return s32RetCode;
}

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
int write2DeviceAll( int *ps32IpAddr , 
                     int *ps32Port,
                     int s32IpAddrSize , 
                     unsigned char *pu8WriteBuff , 
                     int s32WriteLen ,
                     int *ps32Result)
{
    int             s32RetCode    = 0;
    int             s32SyncSemCnt = 0;
    int             i;
    struct stDeviceConfigInfo *     pstNode;

    // 1. 参数合法化检测
    if( NULL == pu8WriteBuff || 0 == s32WriteLen )
    {
        return DEVICE_CONFIG_SERVICE_PARAMETER_ERROR;
    }

    // 2. 遍历节点
    for (i = 0; i < s32IpAddrSize; i++)
    {
        // 2.1 根据IP地址和端口号进行节点的获取
        pstNode = seekDeviceList(ps32IpAddr[i],ps32Port[i]);

        // 2.2 判定节点是否查找到
        if (pstNode!=NULL)
        {
            
            // 2.2.1 重置错误码
            pstNode->err = DEVICE_CONFIG_SERVICE_NO_ERROR;
            // 2.2.2 根据节点有效性进行事件发送
            if (pstNode->valid == true)
            {
                // 2.2.2.1 进行写数据的复制
                memset(pstNode->send_buf,0,DEVICE_CONFIG_SERVICE_SEND_BUFF_LEN); // 初始化空间
                memcpy(pstNode->send_buf,pu8WriteBuff,s32WriteLen);
                pstNode->send_len  = s32WriteLen;

                // 2.2.2.2 分配线程同步信号
                pstNode->finish = &m_semDevConfigFinishPool[s32SyncSemCnt++];

                // 2.2.2.3 发出事件
                pstNode->opt    = DEVICE_CONFIG_SERVICE_EVENT_WRITE;
                ReleaseSemaphore(pstNode->event,1,NULL);
            }
            else
            {
                ps32Result[i] = DEVICE_CONFIG_SERVICE_NO_OPEN_ERROR;
                s32RetCode    = -1;
            }
        }
        else
        {
            ps32Result[i] = DEVICE_CONFIG_SERVICE_RESOURCE_NO_FIND_ERROR;
            s32RetCode    = -1;
        }
    }

    // 3. 等待所有事件线程处理完成
    if( 0 < s32SyncSemCnt )
    {
        SyncWaitForConfigThreadFinish(m_semDevConfigFinishPool,s32SyncSemCnt);
    }

    // 4. 提取写结果
    if( s32RetCode == 0 )
    {
        s32RetCode = updateDeviceListErr(ps32IpAddr,ps32Port,s32IpAddrSize,ps32Result);
    }
    else
    {
        updateDeviceListErr(ps32IpAddr,ps32Port,s32IpAddrSize,ps32Result);
    }

    return s32RetCode;
}
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
int read4Device( int *ps32IpAddr , 
                 int *ps32Port , 
                 int s32IpAddrSize ,
                 int s32Timeout ,
                 int *ps32Result )
{
    int                             s32RetCode    = 0;
    int                             s32SyncSemCnt = 0;
    struct stDeviceConfigInfo *     pstNode       = NULL;
    int                             i;


    // 1. 遍历节点
    for (i = 0; i < s32IpAddrSize; i++)
    {
        // 1.1 根据IP地址和端口号进行节点的获取
        pstNode = seekDeviceList(ps32IpAddr[i],ps32Port[i]);

        // 1.2 判定节点是否查找到
        if (pstNode!=NULL)
        {
            
            // 1.2.1 重置错误码
            pstNode->err = DEVICE_CONFIG_SERVICE_NO_ERROR;
            // 1.2.2 根据节点有效性进行事件发送
            if (pstNode->valid == true)
            {
                // 1.2.2.1 重置接收缓冲区
                memset(pstNode->recv_buf,0,DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN);
                pstNode->recv_len  = DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN;

                // 1.2.2.2 分配线程同步信号
                pstNode->finish = &m_semDevConfigFinishPool[s32SyncSemCnt++];

                // 1.2.2.3 发出事件
                pstNode->timeout = s32Timeout;
                pstNode->opt     = DEVICE_CONFIG_SERVICE_EVENT_READ;
                ReleaseSemaphore(pstNode->event,1,NULL);
            }
            else
            {
                ps32Result[i] = DEVICE_CONFIG_SERVICE_NO_OPEN_ERROR;
                s32RetCode    = -1;
            }
        }
        else
        {
            ps32Result[i] = DEVICE_CONFIG_SERVICE_RESOURCE_NO_FIND_ERROR;
            s32RetCode    = -1;
        }
    }

    // 2. 等待所有事件线程处理完成
    if( 0 < s32SyncSemCnt )
    {
        SyncWaitForConfigThreadFinish(m_semDevConfigFinishPool,s32SyncSemCnt);
    }

    // 3. 提取读结果
    if( s32RetCode == 0 )
    {
        s32RetCode = updateDeviceListErr(ps32IpAddr,ps32Port,s32IpAddrSize,ps32Result);
    }
    else
    {
        updateDeviceListErr(ps32IpAddr,ps32Port,s32IpAddrSize,ps32Result);
    }

    return s32RetCode;
}

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
int query4Device( int *ps32IpAddr , 
                  int *ps32Port,
                  int s32IpAddrSize , 
                  unsigned char *pu8WriteBuff , 
                  int* ps32WriteLen ,
                  int s32Timeout ,
                  int *ps32Result)
{
    int             s32RetCode    = 0;
    int             s32SyncSemCnt = 0;
    int             i;
    int             s32WriteBuffOffset = 0;
    struct stDeviceConfigInfo *     pstNode;

    // 1. 参数合法化检测
    if( NULL == pu8WriteBuff || NULL == ps32WriteLen )
    {
        return DEVICE_CONFIG_SERVICE_PARAMETER_ERROR;
    }

    // 2. 遍历节点
    for (i = 0; i < s32IpAddrSize; i++)
    {
        // 2.1 根据IP地址和端口号进行节点的获取
        pstNode = seekDeviceList(ps32IpAddr[i],ps32Port[i]);

        // 2.2 判定节点是否查找到
        if (pstNode!=NULL)
        {
            // 2.2.1 重置错误码
            pstNode->err = DEVICE_CONFIG_SERVICE_NO_ERROR;
            // 2.2.2 根据节点有效性进行事件发送
            if (pstNode->valid == true)
            {
                if( ps32WriteLen[i] != 0)
                {
                    // 2.2.1 进行写数据的复制
                    memset(pstNode->send_buf,0,DEVICE_CONFIG_SERVICE_SEND_BUFF_LEN); // 初始化空间
                    memcpy(pstNode->send_buf,pu8WriteBuff+s32WriteBuffOffset,ps32WriteLen[i]);
                    pstNode->send_len  = ps32WriteLen[i];

                    // 2.2.2 重置接收缓冲区
                    memset(pstNode->recv_buf,0,DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN);
                    pstNode->recv_len  = DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN;

                    // 2.2.3 分配线程同步信号
                    pstNode->finish = &m_semDevConfigFinishPool[s32SyncSemCnt++];

                    // 2.2.4 发出事件
                    pstNode->timeout = s32Timeout; 
                    pstNode->opt     = DEVICE_CONFIG_SERVICE_EVENT_QUERY;
                    ReleaseSemaphore(pstNode->event,1,NULL);
                }
            }
            else
            {
                ps32Result[i] = DEVICE_CONFIG_SERVICE_NO_OPEN_ERROR;
                s32RetCode    = -1;
            }
        }
        else
        {
            ps32Result[i] = DEVICE_CONFIG_SERVICE_RESOURCE_NO_FIND_ERROR;
            s32RetCode    = -1;
        }
        s32WriteBuffOffset += ps32WriteLen[i];
    }

    // 3. 等待所有事件线程处理完成
    if( 0 < s32SyncSemCnt )
    {
        SyncWaitForConfigThreadFinish(m_semDevConfigFinishPool,s32SyncSemCnt);
    }
    // 4. 提取查询结果
    if( s32RetCode == 0 )
    {
        s32RetCode = updateDeviceListErr(ps32IpAddr,ps32Port,s32IpAddrSize,ps32Result);
    }
    else
    {
        updateDeviceListErr(ps32IpAddr,ps32Port,s32IpAddrSize,ps32Result);
    }

    return s32RetCode;
}
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
int query4DeviceAll( int *ps32IpAddr , 
                     int *ps32Port,
                     int s32IpAddrSize , 
                     unsigned char *pu8WriteBuff , 
                     int s32WriteLen ,
                     int s32Timeout ,
                     int *ps32Result)
{
    int                             s32RetCode    = 0;
    int                             s32SyncSemCnt = 0;
    int                             i;
    struct stDeviceConfigInfo *     pstNode       = NULL;

    // 1. 参数合法化检测
    if( NULL == pu8WriteBuff || 0 == s32WriteLen )
    {
        return DEVICE_CONFIG_SERVICE_PARAMETER_ERROR;
    }

    // 2. 遍历节点
    for (i = 0; i < s32IpAddrSize; i++)
    {
        // 2.1 根据IP地址和端口号进行节点的获取
        pstNode = seekDeviceList(ps32IpAddr[i],ps32Port[i]);

        // 2.2 判定节点是否查找到
        if (pstNode!=NULL)
        {
            
            // 2.2.1 重置错误码
            pstNode->err = DEVICE_CONFIG_SERVICE_NO_ERROR;
            // 2.2.2 根据节点有效性进行事件发送
            if (pstNode->valid == true)
            {
                // 2.2.1 进行写数据的复制
                memset(pstNode->send_buf,0,DEVICE_CONFIG_SERVICE_SEND_BUFF_LEN); // 初始化空间
                memcpy(pstNode->send_buf,pu8WriteBuff,s32WriteLen);
                pstNode->send_len  = s32WriteLen;

                // 2.2.2 重置接收缓冲区
                memset(pstNode->recv_buf,0,DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN);
                pstNode->recv_len  = DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN;

                // 2.2.3 分配线程同步信号
                pstNode->finish = &m_semDevConfigFinishPool[s32SyncSemCnt++];

                // 2.2.4 发出事件
                pstNode->timeout = s32Timeout; 
                pstNode->opt     = DEVICE_CONFIG_SERVICE_EVENT_QUERY;
                ReleaseSemaphore(pstNode->event,1,NULL);
            }
            else
            {
                ps32Result[i] = DEVICE_CONFIG_SERVICE_NO_OPEN_ERROR;
                s32RetCode    = -1;
            }
        }
        else
        {
            ps32Result[i] = DEVICE_CONFIG_SERVICE_RESOURCE_NO_FIND_ERROR;
            s32RetCode    = -1;
        }
    }

    // 3. 等待所有事件线程处理完成
    if( 0 < s32SyncSemCnt )
    {
        SyncWaitForConfigThreadFinish(m_semDevConfigFinishPool,s32SyncSemCnt);
    }
    // 4. 提取查询结果
    if( s32RetCode == 0 )
    {
        s32RetCode = updateDeviceListErr(ps32IpAddr,ps32Port,s32IpAddrSize,ps32Result);
    }
    else
    {
        updateDeviceListErr(ps32IpAddr,ps32Port,s32IpAddrSize,ps32Result);
    }

    return s32RetCode;
}
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
int getReadData4IP( int s32IpAddr ,
                    int s32PortNum,
                    unsigned char *pu8ReadBuff , 
                    int s32ReadOffset , 
                    int s32ReadBuffSize )
{
    int                             s32ValidDataLen = 0;
    struct stDeviceConfigInfo *     pstNode         = NULL;

    // 1. 参数有效性检测
    if( pu8ReadBuff == NULL || s32ReadBuffSize == 0 )
    {
        return -1;
    }

    // 2. 遍历客户端列表,查找s32IpAddr对应的客户端节点
    pstNode = seekDeviceList(s32IpAddr,s32PortNum);

    // 3. 判定是否查询到有效节点
    if( NULL == pstNode )
    {
        // 未查询到有效节点,则返回-2
        return -2;
    }

    // 4. 根据节点的有效性和接收缓冲区的长度,进行数据有效性验证
    if( pstNode->valid    == false ||
        pstNode->err      != DEVICE_CONFIG_SERVICE_NO_ERROR ||
        pstNode->recv_len == 0)
    {
        return 0;
    }

    // 5. 进行有效数据的复制
    // 5.1 判定数据的有效性
    s32ValidDataLen = (pstNode->recv_len-s32ReadOffset);
    s32ValidDataLen = s32ValidDataLen > s32ReadBuffSize ? s32ReadBuffSize : s32ValidDataLen;
    // 5.2 进行数据的复制
    memcpy(pu8ReadBuff,&pstNode->recv_buf[s32ReadOffset],s32ValidDataLen);

    return s32ValidDataLen;
}