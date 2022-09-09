
#include <stdio.h>
#include <WinSock2.h>
#include <process.h>
#include "device_recv_service.h"

#pragma comment(lib,"ws2_32.lib")

#define DATA_RECV_MAX_DEVICE_NUM                    128

#define DATA_RECV_MAX_BANDWIDTH                     (int)(80*1024*1024) // 默认100MB/s的接收量

#define DATA_RECV_MAX_BUFF_LEN                      (int)(8*1024*1024)

#define DATA_RECV_PROTOCAL_UDP                      (int)0         //  使用UDP协议
#define DATA_RECV_PROTOCAL_TCP                      (int)1         //  使用TCP协议

#define DATA_RECV_WARING_EXISTING                   (int)1
#define DATA_RECV_ERROR_NO_ERROR                    (int)0
#define DATA_RECV_ERROR_MEM_ALLOC                   (int)-1
#define DATA_RECV_ERROR_SOCKET_ALLOC                (int)-2
#define DATA_RECV_ERROR_ADDR_BIND_FAIL              (int)-3
#define DATA_RECV_ERROR_INVALID_PARAMETER           (int)-4
#define DATA_RECV_ERROR_TIMEOUT                     (int)-5
#define DATA_RECV_ERROR_INVALID_IO                  (int)-6
#define DATA_RECV_ERROR_SET_RECV_BUFFSIZE_FAIL      (int)-7
#define DATA_RECV_ERROR_LISTEN_FAIL                 (int)-8
#define DATA_RECV_ERROR_WSA_OPEN_FAIL               (int)-10
#define DATA_RECV_ERROR_NODE_NUM_FULL               (int)-11

typedef enum _emDataRecvEvent
{
    DATA_RECV_EVENT_WAIT_FOR_RECV = 0,      //  等待数据接收
    DATA_RECV_EVENT_RECV,                   //  开启数据接收
    DATA_RECV_EVENT_AGAIN_RECV,             //  由于数据未接收完成,则进行重传
    DATA_RECV_EVENT_EXIT                    //  退出线程
}emDataRecvEvent;

typedef enum _emDataRecvCtrlEvent
{
    DATA_RECV_CONTROL_EVENT_REGISTER = 0,
    DATA_RECV_CONTROL_EVENT_ADD_DEVICE,
    DATA_RECV_CONTROL_EVENT_DELET_DEVICE,
    DATA_RECV_CONTROL_EVENT_UNREGISTER,
    DATA_RECV_CONTROL_EVENT_START_RECV
}emDataRecvCtrlEvent;

struct stMsgInfo
{
    int                     opt;
    struct stDeviceInfo *   dev;
    HANDLE                  sync;
    struct stMsgInfo    *   next;
};


/*
* 定义记录设备信息的结构体
* 包含:1.设备IP地址
*      2.通信协议:0--UDP,1--TCP
*      3.通信时的端口号
*/
struct stDeviceInfo
{
    int                         ip;                                             //  设备IP地址
    short                       protocal;                                       //  设备的通信协议,0--UDP,1--TCP
    short                       cport;                                          //  设备端口号
    short                       sport;                                          //  服务端口号
    short                       err;                                            //  错误码
    bool                        valid;                                          //  当前客户端是否已建立子线程
    char   *                    recv_buf;                                       //  接收缓冲区
    int                         recv_size;                                      //  接收缓冲区的大小
    int                         recv_len;                                       //  接收的数据长度
    int                         timeout;                                        //  接收超时时间
    HANDLE *                    finish;                                         //  线程同步使用
    bool                        ready;                                          //  设备数据准备完成
    bool                        exit;                                           //  子线程退出
    bool                        busy;                                           //  当前节点的繁忙状态
    bool                        first;                                          //  首次接收数据的标识
    bool                        run;                                            //  是否已发送run命令
    bool                        resend;                                         //  发生了重传事件
    bool                        use;                                            //  当前节点是否被使用
    struct stDeviceInfo *       next;
};

typedef struct _stDevCommPara
{
    int        ip;
    int        protocol;
    int        sport;
    int        cport;
    int        rcv_len;
    int        err;
}stDevCommPara;

typedef struct _stDataRecvEventPara
{
    stDevCommPara       dev[DATA_RECV_MAX_DEVICE_NUM];
    int                 timeout;
    int                 size;
    int                 err;
}stDataRecvEventPara;

typedef struct _stDataRecvControlPara
{
    HANDLE                      pool[DATA_RECV_MAX_DEVICE_NUM]; //  子线程与父线程的同步信号量池
    int                         max;                            //  支持的最大设备节点数
    int                         cnt;                            //  当前有效的设备节点数
    stDataRecvEventPara         para;                           //  控制线程操作时的参数
    struct stDeviceInfo         list[DATA_RECV_MAX_DEVICE_NUM]; //  当前有效的设备节点信息
    bool                        block;                          //  控制线程操作时的工作模式;true -- 阻塞,false -- 非阻塞
    HANDLE                      sync;                           //  阻塞时同步使用,只有当block为true时,才有效
    bool                        limit;                          //  TCP数据接收时,是否进行限流接收;true -- 阻塞,false -- 非阻塞
    bool                        frist;                          //  首次Start时,需等待设备复位完成
    DATA_RECV_OPTION_REPORT     report;                         //  数据接收线程上报
}stDataRecvControlPara;

stDataRecvControlPara           m_stDataRecvCtrlInfo = { 0 };
HANDLE                          m_semDataRecvCtrl;
bool                            m_bSubThreadStopWait = false;
struct stMsgInfo *              m_pstMsgQueue = NULL;
HANDLE                          m_semMsgWRProtect = NULL;
HANDLE                          m_semMsgProcess = NULL;

bool findDevNode4Ip( int s32IpAddr , int *ps32Index )
{
    struct stDeviceInfo *       pstDevList  = m_stDataRecvCtrlInfo.list;
    bool                        bFindResult = false;
    int i;

    for ( i = 0 ; i < DATA_RECV_MAX_DEVICE_NUM;  i++ )
    {
        if ( pstDevList[i].ip == s32IpAddr )
        {
            if( NULL != ps32Index )
            {
                *ps32Index = i;
            }
            bFindResult = true;
            break;
        }
    }

    return bFindResult;
}

int findIdleNode4List()
{
    for (int i = 0 ; i < DATA_RECV_MAX_DEVICE_NUM ;  i++ )
    {
        if ( m_stDataRecvCtrlInfo.list[i].use == false )
        {
            return i;
        }
    }
    return -1;
}

void removeDevNode( struct stDeviceInfo *pstDevList , int *ps32ListCnt , int s32RemoveIndex)
{
    int i;

    // 设备数减1
    (*ps32ListCnt)--;
    // 进行尾节点的判定
    /*if ( s32RemoveIndex <0 || s32RemoveIndex >= (*ps32ListCnt))
    {
    return;
    }*/
    // 对s32RemoveIndex后面的数据进行前移
    /*for (i = s32RemoveIndex; i < (*ps32ListCnt); i++)
    {
    pstDevList[i] = pstDevList[i+1];
    }*/
    memset((void*)&pstDevList[s32RemoveIndex],0,sizeof(struct stDeviceInfo));
}

DWORD SyncWaitForRecvThreadFinish(HANDLE * handles, int count , BOOL bAllWait,DWORD s32Timeout)
{
    int waitingThreadsCount = count;
    int index = 0;
    DWORD res = 0;

    if ( TRUE == bAllWait ) // 等待全部事件完成
    {
        while(waitingThreadsCount >= MAXIMUM_WAIT_OBJECTS)
        {
            res = WaitForMultipleObjects(MAXIMUM_WAIT_OBJECTS, &handles[index], TRUE, s32Timeout);
            waitingThreadsCount -= MAXIMUM_WAIT_OBJECTS;
            index += MAXIMUM_WAIT_OBJECTS;
        }

        if(waitingThreadsCount > 0)
        {
            res = WaitForMultipleObjects(waitingThreadsCount, &handles[index], TRUE, s32Timeout);
        }
    }
    else//只要有一个事件完成,则返回
    {
        while(waitingThreadsCount >= MAXIMUM_WAIT_OBJECTS)
        {
            res = WaitForMultipleObjects(MAXIMUM_WAIT_OBJECTS, &handles[index], TRUE, s32Timeout);
            waitingThreadsCount -= MAXIMUM_WAIT_OBJECTS;
            index += MAXIMUM_WAIT_OBJECTS;
            if ( res != WAIT_TIMEOUT )
            {
                return res;
            }
        }

        if(waitingThreadsCount > 0)
        {
            res = WaitForMultipleObjects(waitingThreadsCount, &handles[index], TRUE, s32Timeout);
        }
    }

    return res;
}

void initMsgQueue4DataRecv( void )
{
    m_pstMsgQueue = NULL;
    m_semMsgWRProtect = CreateSemaphore(NULL,1,1,NULL);
    m_semMsgProcess   = CreateSemaphore(NULL,0,DATA_RECV_MAX_DEVICE_NUM*2,NULL);
}

void postMsg2DataRecv( struct stMsgInfo * pstMsgInfo )
{
    WaitForSingleObject(m_semMsgWRProtect,INFINITE);

    pstMsgInfo->next = m_pstMsgQueue;
    m_pstMsgQueue    = pstMsgInfo;

    ReleaseSemaphore(m_semMsgProcess,1,NULL);

    ReleaseSemaphore(m_semMsgWRProtect,1,NULL);
}

struct stMsgInfo *getMsg4DataRecv( void )
{
    struct stMsgInfo * pstMsgInfo = NULL;
    struct stMsgInfo * pstBackMsgInfo = NULL;
    DWORD              s32RetCode = 0;

    s32RetCode = WaitForSingleObject( m_semMsgProcess , 100 );

    WaitForSingleObject(m_semMsgWRProtect,INFINITE);

    // 指向消息队列头
    pstMsgInfo = m_pstMsgQueue;

    // 判定是否有消息,若有消息,则遍历到消息队列的尾部进行抛出
    if ( NULL != pstMsgInfo )
    {
        while ( NULL != pstMsgInfo->next )
        {
            pstBackMsgInfo = pstMsgInfo;
            pstMsgInfo = pstMsgInfo->next;
        }

        // 若为队列的首节点,则清空消息队列
        if ( pstMsgInfo == m_pstMsgQueue )
        {
            m_pstMsgQueue = NULL;
        }
        else
        {
            // 则从消息队列中删除尾节点
            pstBackMsgInfo->next = NULL;
        }
    }

    ReleaseSemaphore(m_semMsgWRProtect,1,NULL);

    return pstMsgInfo;
}

void dataRecvFinishProcess( void *pThreadPara )
{
    struct stDeviceInfo*                pstClientInfo            = (struct stDeviceInfo*)pThreadPara; 

    // 上报数据接收完成,要求上层进行数据接收
    m_stDataRecvCtrlInfo.report(0,pstClientInfo->ip,pstClientInfo->recv_len);
}

void dataRecvRunProcess( void *pThreadPara )
{
    struct stDeviceInfo*                pstClientInfo            = (struct stDeviceInfo*)pThreadPara;   

    // 发送:RUN命令
    m_stDataRecvCtrlInfo.report(3,pstClientInfo->ip,0);
}

void dataRecvStopProcess( void *pThreadPara )
{
    struct stDeviceInfo*                pstClientInfo            = (struct stDeviceInfo*)pThreadPara;   
    // 发送:WAV:CHANSEND 0命令
    m_stDataRecvCtrlInfo.report(4,pstClientInfo->ip,0);
}

void dataRecvMsgProcess( void *pThreadPara )
{
    struct stMsgInfo*       pstMsgInfo      = NULL;
    int                     s32IdleDevCnt   = 0;
    int                     s32NetBandWidth = 0;
    int                     s32CurrRecvDataDevCnt = 0;
    int                     s32RealRecvFinishCnt = 0;
    int                     s32DevNodeIndex = 0;
    int                     s32RecvTimeoutCnt = 0;
    int                     s32WaitForSystRstCnt = 0;
    int                     i;
    bool                    bRecvStataIsStop = false;

    // 等待接收消息
    while (true)
    {
        pstMsgInfo = getMsg4DataRecv();

        if ( NULL == pstMsgInfo ) // 空消息
        {
            // 当前已有设备完成了数据接收
            if ( s32RealRecvFinishCnt > 0 )
            {
                // 遍历当前所有有效设备,是否存在还有设备没有发生触发(及与接收建立连接)
                for ( i = 0;  i < m_stDataRecvCtrlInfo.para.size ;  i++)
                {
                    if (true == findDevNode4Ip(m_stDataRecvCtrlInfo.para.dev[i].ip,&s32DevNodeIndex))
                    {
                        if (( false == m_stDataRecvCtrlInfo.list[s32DevNodeIndex].ready )||(true == m_stDataRecvCtrlInfo.list[s32DevNodeIndex].ready)&&( 0 == m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_len))
                        {
                            // 超时计数
                            s32RecvTimeoutCnt ++;
                            break;
                        }
                    }

                }
                // 若2秒内仍有设备未触发,或者所有设备均已完成数据接收,则开始上报
                if ( ( s32RecvTimeoutCnt == (m_stDataRecvCtrlInfo.para.timeout/100) ) || ( s32RealRecvFinishCnt == s32IdleDevCnt ) )
                {
                    for ( i = 0 ;  i < m_stDataRecvCtrlInfo.para.size ;  i++)
                    {
                        // 查询节点是否存在
                        // 查询设备节点
                        if ( true == findDevNode4Ip(m_stDataRecvCtrlInfo.para.dev[i].ip,&s32DevNodeIndex) )
                        {
                            if (m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_len == 0)
                            {
                                m_stDataRecvCtrlInfo.report(0,m_stDataRecvCtrlInfo.para.dev[i].ip,DATA_RECV_ERROR_TIMEOUT);
                            }
                            else
                            {
                                 // 进行数据接收完成的上报
                                //m_stDataRecvCtrlInfo.report(0,m_stDataRecvCtrlInfo.para.dev[i].ip,m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_len);
                                 _beginthread(dataRecvFinishProcess,1024,&m_stDataRecvCtrlInfo.list[s32DevNodeIndex]);
                            }
                        }
                        else
                        {
                            m_stDataRecvCtrlInfo.report(0,m_stDataRecvCtrlInfo.para.dev[i].ip,DATA_RECV_ERROR_INVALID_PARAMETER);
                        }
                    }
                    //for ( i = 0 ;  i < m_stDataRecvCtrlInfo.para.size ;  i++)
                    //{
                    //    // 查询节点是否存在
                    //    // 查询设备节点
                    //    if ( true == findDevNode4Ip(m_stDataRecvCtrlInfo.para.dev[i].ip,&s32DevNodeIndex) )
                    //    {
                    //        if (m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_len == 0)
                    //        {
                    //            continue;
                    //        }
                    //        // 进行数据接收完成的上报
                    //        _beginthread(dataRecvFinishProcess,1024,&m_stDataRecvCtrlInfo.list[s32DevNodeIndex]);
                    //    }
                    //    else
                    //    {
                    //        m_stDataRecvCtrlInfo.report(0,m_stDataRecvCtrlInfo.para.dev[i].ip,DATA_RECV_ERROR_INVALID_PARAMETER);
                    //    }
                    //}
                    s32RealRecvFinishCnt = 0;
                }
            }
            bRecvStataIsStop = false;
        }
        else // 存在有效消息
        {
            // 根据消息操作码值,进行处理
            switch ( pstMsgInfo->opt)
            {
            case 0 : // 记录当前设备完成了数据接收
                s32NetBandWidth -= pstMsgInfo->dev->recv_size; // 更新当前网络带宽占用量

                // 更新当前处于数据接收的设备数量 减 1
                s32CurrRecvDataDevCnt --;

                // 若当前节点的接收长度不等于0,则认为接收完成
                if ( 0 != pstMsgInfo->dev->recv_len)
                {
                    s32RealRecvFinishCnt ++;
                    s32RecvTimeoutCnt = 0;
                }

                break;

            case 1 : // 根据当前限流模式,向设备发送请求数据发送指令,该事件仅有TCP协议进入
                // 若当前网络带宽已占用上限且当前已有数据在进行数据接收,则把该消息送回消息队列
                if ( ( s32NetBandWidth > DATA_RECV_MAX_BANDWIDTH ) && s32CurrRecvDataDevCnt > 0 )
                {
                    postMsg2DataRecv(pstMsgInfo);
                }
                else
                {
                    /*
                    * 可以进行数据的接收
                    * 1. 启动该设备接收线程
                    * 2. 发送数据发送命令到设置
                    * 3. 更新设备的接收寄存变量
                    */
                    ReleaseSemaphore(pstMsgInfo->sync,1,NULL); // 启动设备的接收

                    m_stDataRecvCtrlInfo.report(1,pstMsgInfo->dev->ip,0);// 发送数据开始发送的命令到设备

                    // 更新当前网络带宽占用
                    s32NetBandWidth += pstMsgInfo->dev->recv_size;
                    // 更新当前处于数据接收的设备数量 加 1
                    s32CurrRecvDataDevCnt ++;
                }
                break;

            case 2 : // 向设备发送数据重传指令

                /*
                * 可以进行数据重传
                * 1. 置当前设备的首次接收标识为false,让数据接收线程不再进行数据发送命令的执行
                * 2. 发送数据重传命令到设备
                * 3. 更新设备的接收寄存变量
                */
                m_stDataRecvCtrlInfo.report(2,pstMsgInfo->dev->ip,0);// 发送数据重传的命令到设备

                break;

            case 3 : // 根据当前所有的设备节点是否空闲,进行运行指令的发送

                // 若当前已停止数据接收,则不再进行RUN
                if ( true == bRecvStataIsStop )
                {
                    // 释放本次RUN事件
                    free(pstMsgInfo);
                    bRecvStataIsStop = false;
                    break;
                }

                // 若首次开启服务,则忽略5次改事件
                if (true == m_stDataRecvCtrlInfo.frist )
                {
                    if ( 5 == s32WaitForSystRstCnt )
                    {
                        s32WaitForSystRstCnt = 0;
                        m_stDataRecvCtrlInfo.frist = false;
                    }
                    else
                    {
                        s32WaitForSystRstCnt ++;
                        Sleep(100);
                        postMsg2DataRecv(pstMsgInfo);
                        break;
                    }
                }

                // 若当前仍有设备在进行数据接收,则等待100ms后,把消息放回队列
                if ( 0 != s32CurrRecvDataDevCnt )
                {
                    Sleep(100);
                    postMsg2DataRecv(pstMsgInfo);
                }
                else
                {
                    s32IdleDevCnt = 0;
                    s32RealRecvFinishCnt = 0;
                    s32RecvTimeoutCnt = 0;
                    // 开始进行设备的:RUN命令发送
                    for ( i = 0 ;  i < m_stDataRecvCtrlInfo.para.size ;  i++)
                    {
                        // 查询节点是否存在
                        // 查询设备节点
                        if ( true == findDevNode4Ip(m_stDataRecvCtrlInfo.para.dev[i].ip,&s32DevNodeIndex) )
                        {
                            // 重置设备节点
                            m_stDataRecvCtrlInfo.list[s32DevNodeIndex].busy      = false;
                            m_stDataRecvCtrlInfo.list[s32DevNodeIndex].first     = true;
                            m_stDataRecvCtrlInfo.list[s32DevNodeIndex].ready     = false;
                            m_stDataRecvCtrlInfo.list[s32DevNodeIndex].run       = true;
                            m_stDataRecvCtrlInfo.list[s32DevNodeIndex].resend    = false;
                            m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_len  = 0;
                            m_stDataRecvCtrlInfo.list[s32DevNodeIndex].timeout   = m_stDataRecvCtrlInfo.para.timeout;
                            m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_size = m_stDataRecvCtrlInfo.para.dev[i].rcv_len;
                            if (NULL!=m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_buf)
                            {
                                free(m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_buf);
                            }
                            m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_buf = (char*)malloc(m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_size*sizeof(char));
                            // 发送:RUN命令
                            //_beginthread(dataRecvRunProcess,1024,&m_stDataRecvCtrlInfo.list[s32DevNodeIndex]);
                            dataRecvRunProcess(&m_stDataRecvCtrlInfo.list[s32DevNodeIndex]);
                            s32IdleDevCnt ++;
                        }
                    }
                    // 释放信息
                    free(pstMsgInfo);
                }
                break;
            case 4:
                // 若当前仍有设备在进行数据接收,则等待100ms后,把消息放回队列
                if ( 0 != s32CurrRecvDataDevCnt )
                {
                    Sleep(100);
                    postMsg2DataRecv(pstMsgInfo);
                }
                else
                {
                    bRecvStataIsStop = true;

                    // 所有有效节点,发送:WAV:CHANSEND 0命令
                    for ( i = 0;  i < 128;  i++)
                    {
                        if (m_stDataRecvCtrlInfo.list[i].finish != NULL)
                        {
                            // 发送:WAV:CHANSEND 0命令
                            _beginthread(dataRecvStopProcess, 1024, &m_stDataRecvCtrlInfo.list[i]);
                        }
                        
                    }
                    // 释放信息
                    free(pstMsgInfo);
                }
            default:
                break;
            }
        }
    }
}

void tcpRecvServer( void *pThreadPara )
{
    struct stDeviceInfo*                pstClientInfo            = (struct stDeviceInfo*)pThreadPara;   
    SOCKET                              s32SocketFd              = INVALID_SOCKET;
    SOCKET                              s32CommSockFd            = INVALID_SOCKET;
    sockaddr_in                         stClientAddr             = { 0 };
    sockaddr_in                         stServiceAddr            = { 0 };
    int                                 s32AddrLen               = sizeof(sockaddr_in);
    char                   *            ps8RecvBuff              = NULL;
    char                                as8InvalidRecvBuff[1024] = { 0 };
    int                                 s32RecvLen               = 0;
    unsigned int                        u32RecvTotalLen          = 0;
    int                                 s32RemainLen             = 0;
    int                                 s32MaxRecvLen            = DATA_RECV_MAX_BUFF_LEN;
    WORD                                sockVersion              = MAKEWORD(2,2);
    WSADATA                             wsaData;
    fd_set                              read_set,back_set;
    timeval                             stSelectTimeout          = {0,100000};
    struct stMsgInfo                    stMsg;

    // 1. 初始化WSA服务
    if(WSAStartup(sockVersion,&wsaData)!=0)
    {
        // WSA打开失败
        pstClientInfo->err = DATA_RECV_ERROR_WSA_OPEN_FAIL;
        // 子线程初始化完成
        ReleaseSemaphore(*pstClientInfo->finish,1,NULL);
        return;
    }
    // 2 申请TCP Socket句柄
    s32SocketFd = socket(AF_INET,SOCK_STREAM,IPPROTO_TCP);
    if (s32SocketFd == INVALID_SOCKET)
    {
        // Socket句柄申请失败
        pstClientInfo->err = DATA_RECV_ERROR_SOCKET_ALLOC;
        // 关闭WSA服务
        WSACleanup();
        // 子线程初始化完成
        ReleaseSemaphore(*pstClientInfo->finish,1,NULL);
        return;
    }

    // 3. 绑定服务地址
    // 3.1 初始化服务地址信息
    memset(&stServiceAddr,0,sizeof(sockaddr_in));
    stServiceAddr.sin_family           = AF_INET;  //  IPv4
    stServiceAddr.sin_addr.S_un.S_addr = INADDR_ANY;
    stServiceAddr.sin_port             = htons(pstClientInfo->sport);
    // 3.2 绑定
    if(bind(s32SocketFd,(sockaddr*)&stServiceAddr,s32AddrLen) == SOCKET_ERROR)
    {
        // 地址绑定失败
        pstClientInfo->err = DATA_RECV_ERROR_ADDR_BIND_FAIL;
        // 关闭Socket句柄
        closesocket(s32SocketFd);
        // 关闭WSA服务
        WSACleanup();
        // 子线程初始化完成
        ReleaseSemaphore(*pstClientInfo->finish,1,NULL);
        return;
    }

    // 4. 开始监听
    if(listen(s32SocketFd,5) == SOCKET_ERROR)
    {
        pstClientInfo->err = DATA_RECV_ERROR_LISTEN_FAIL;
        // 关闭Socket句柄
        closesocket(s32SocketFd);
        // 关闭WSA服务
        WSACleanup();
        // 子线程初始化完成
        ReleaseSemaphore(*pstClientInfo->finish,1,NULL);
        return;
    }
    // 置子线程初始化成功
    pstClientInfo->valid = true;

    // 子线程初始化完成
    ReleaseSemaphore(*pstClientInfo->finish,1,NULL);

    // 初始化消息
    stMsg.sync = CreateSemaphore(NULL,0,1,NULL);
    stMsg.dev  = pstClientInfo;

    //// 置子线程初始化成功
    //pstClientInfo->valid = true;

    // 设置Select字
    FD_ZERO(&back_set);
    FD_SET(s32SocketFd,&back_set);

    // 开始等待连接的建立
    while ( false == pstClientInfo->exit )
    {
        read_set = back_set;

        // 开启监听
        if ( select(0,&read_set,NULL,NULL,&stSelectTimeout) < 0 )
        {
            continue;
        }

        // 判定是否有连接接入
        if ( FD_ISSET(s32SocketFd,&read_set) )
        {
            // 建立连接
            s32CommSockFd = accept(s32SocketFd,(sockaddr*)&stClientAddr,&s32AddrLen);
            // 置数据准备标识为准备完成
            if ( true == pstClientInfo->run )
            {
                pstClientInfo->ready = true;
            }
            // 设置接收缓冲区大小
            setsockopt(s32CommSockFd,SOL_SOCKET,SO_RCVBUF,(char*)&s32MaxRecvLen,sizeof(int));
            // 若当前为首次接收数据
            if (true == pstClientInfo->first )
            {
                // 抛出请求数据传输的消息
                stMsg.opt = 1;
                postMsg2DataRecv(&stMsg);
                // 等待允许接收
                WaitForSingleObject(stMsg.sync,INFINITE);
                // 置首次接收数据标识为false
                pstClientInfo->first = false;
            }
            // 初始化接收使用的变量
            s32RemainLen    = pstClientInfo->recv_size;
            u32RecvTotalLen = 0;
            ps8RecvBuff     = pstClientInfo->recv_buf;

            // 进行数据接收
            do
            {
                // 根据当前是否已发送:RUN命令进行数据的接收,若没有发送,则直接接收,不进行有效数据的填充
                if ( false == pstClientInfo->run )
                {

                    s32RecvLen = recv(s32CommSockFd,as8InvalidRecvBuff,1024,0);
                    if ( s32RecvLen == 0 ) // 关闭连接
                    {
                        closesocket(s32CommSockFd);
                        // 抛出数据接收完成的消息
                        stMsg.opt = 0;
                        postMsg2DataRecv(&stMsg);
                        break;
                    }
                }
                else
                {
                    s32RecvLen = recv(s32CommSockFd,ps8RecvBuff,s32RemainLen,0);
                    if ( s32RecvLen == 0 ) // 关闭连接
                    {
                        closesocket(s32CommSockFd);
                        // 若当前接收到数据的总长度等于预读长度,则任务接收完成
                        if ( u32RecvTotalLen == pstClientInfo->recv_size )
                        {
                            // 更新接收到数据的长度
                            pstClientInfo->recv_len = u32RecvTotalLen;
                            // 抛出数据接收完成的消息
                            stMsg.opt = 0;
                            postMsg2DataRecv(&stMsg);
                        }
                        else
                        {
                            if ( false == pstClientInfo->resend )
                            {
                                // 抛出请求数据重传的消息
                                stMsg.opt = 2;
                                postMsg2DataRecv(&stMsg);
                                pstClientInfo->resend = true;
                            }
                            else
                            {
                                // 更新接收到数据的长度
                                pstClientInfo->recv_len = u32RecvTotalLen;
                                // 抛出数据接收完成的消息
                                stMsg.opt = 0;
                                postMsg2DataRecv(&stMsg);
                            }
                        }

                        break;
                    }
                    else
                    {
                        s32RemainLen    -= s32RecvLen;
                        u32RecvTotalLen += s32RecvLen;
                        ps8RecvBuff     += s32RecvLen;
                    }
                }
            } while (true);
        }
    }
    pstClientInfo->valid = false;
    // 线程退出,通知上层
    ReleaseSemaphore(*pstClientInfo->finish,1,NULL);
}

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
void createDataRecvService( DATA_RECV_OPTION_REPORT pReportIO )
{
    int i;

    // 1 初始化数据接收服务的设备信息表
    memset(&m_stDataRecvCtrlInfo,0,sizeof(stDataRecvControlPara));

    // 2 初始化信号池
    for (i = 0; i < DATA_RECV_MAX_DEVICE_NUM ; i++)
    {
        m_stDataRecvCtrlInfo.pool[i] = CreateSemaphore(NULL,0,1,NULL); // 申请匿名信号量
    }

    // 3 记录状态上报回调函数
    m_stDataRecvCtrlInfo.report = pReportIO;

    // 4 创建接收控制线程唤醒信号量
    //m_semDataRecvCtrl = CreateSemaphore(NULL,0,1,NULL); // 申请匿名信号量

    // 5 启动数据接收的控制线程
    //_beginthread(dataRecvControlProcess,1024,&m_stDataRecvCtrlInfo);

    // 6 初始化消息队列
    initMsgQueue4DataRecv();

    // 7 创建消息处理线程
    _beginthread(dataRecvMsgProcess,1024,NULL);
}
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
*                                                        -11 -- 超出最大设备支持数
* 返 回 值：无
* 说    明：ps32ClientPort只有在协议为UDP时,才可以使用
******************************************************************************/
void addDevInfo2DataRecvService( int *ps32ClientIpAddr , 
                                int *ps32ClientPort,
                                int *ps32ClientProtocol,
                                int *ps32ServicePort,
                                int  s32ClientNum,
                                int *ps32RegResult)
{
    int         s32SyncSemUseCnt = 0;
    int         i;
    int         s32FindIndex = 0;
    int         s32IdleNodeIndex = -1;

    // 1 遍历设备IP地址
    for (i = 0; i < s32ClientNum; i++)
    {
        // 1.1 根据IP地址检测是否已在设备列表中,若没有则在尾部添加
        if( false == findDevNode4Ip( ps32ClientIpAddr[i],NULL) && DATA_RECV_ERROR_NO_ERROR == ps32RegResult[i] )
        {
            s32IdleNodeIndex = findIdleNode4List();
            if ( -1 == s32IdleNodeIndex )
            {
                ps32RegResult[i] = DATA_RECV_ERROR_NODE_NUM_FULL;
                continue;
            }
            // 初始化参数
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].ip        = ps32ClientIpAddr[i];                //  设备IP地址
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].protocal  = ps32ClientProtocol[i];              //  设备的通信协议,0--UDP,1--TCP
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].cport     = ps32ClientPort[i];                  //  设备端口号
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].sport     = ps32ServicePort[i];                 //  服务端口号
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].err       = DATA_RECV_ERROR_NO_ERROR;           //  错误码
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].valid     = false;                              //  当前客户端是否已建立子线程
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].recv_buf  = NULL;                               //  接收缓冲区
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].recv_size = 0;                                  //  接收缓冲区的大小
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].recv_len  = 0;                                  //  接收的数据长度
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].timeout   = 2000;                               //  接收超时时间
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].finish    = NULL;                               //  线程同步使用
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].ready     = false;                              //  设备数据准备完成
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].busy      = false;                              //  当前节点的繁忙状态
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].exit      = false;                              //  子线程退出
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].first     = true;                               //  首次接收数据的标识
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].run       = false;                              //  是否已发送run命令
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].resend    = false;                              //  发生了重传事件
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].use       = true;                               //  置设备节点为被使用(true)

            // 分配同步信号量
           // m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].finish    = &m_stDataRecvCtrlInfo.pool[s32SyncSemUseCnt++];
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].finish = &m_stDataRecvCtrlInfo.pool[s32SyncSemUseCnt++];
         
            // 根据协议进行线程分配
            if ( DATA_RECV_PROTOCAL_UDP == ps32ClientProtocol[i] ) // UDP协议
            {

            }
            else // TCP协议
            {
                _beginthread(tcpRecvServer,2048,&m_stDataRecvCtrlInfo.list[s32IdleNodeIndex]);
            }
            // 已有设备数量加1
            m_stDataRecvCtrlInfo.cnt ++;
        }
    }
    // 2 等待所有子线程初始化完成
    if (s32SyncSemUseCnt > 0)
    {
        SyncWaitForRecvThreadFinish(m_stDataRecvCtrlInfo.pool,s32SyncSemUseCnt,TRUE,INFINITE);
    }

    // 3 更新添加结果
    for (i = 0; i < s32ClientNum; i++)
    {
        if( DATA_RECV_ERROR_NO_ERROR == ps32RegResult[i] )
        {
            // 3.1 查找设备节点
            findDevNode4Ip( ps32ClientIpAddr[i] , &s32FindIndex );

            // 3.2 提取结果
            ps32RegResult[i] = m_stDataRecvCtrlInfo.list[s32FindIndex].err;

            // 3.3 判定节点的子线程是否建立成功,若不成功,则进行移出
            if ( false == m_stDataRecvCtrlInfo.list[s32FindIndex].valid )
            {
                // 移出设备队列
                removeDevNode( m_stDataRecvCtrlInfo.list , &m_stDataRecvCtrlInfo.cnt , s32FindIndex );
            }
        }
    }
}
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
void deleteDevNode4DataRecvService( int *ps32ClientIpAddr , int  s32ClientNum )
{
    int         s32SyncSemUseCnt = 0;
    int         i;
    int         s32FindIndex = 0;

    // 1 根据IP地址查找设备队列
    for ( i = 0;  i < s32ClientNum ;  i++)
    {
        if (true == findDevNode4Ip(ps32ClientIpAddr[i],&s32FindIndex))
        {
            m_stDataRecvCtrlInfo.list[s32FindIndex].finish = &m_stDataRecvCtrlInfo.pool[s32SyncSemUseCnt++];
            m_stDataRecvCtrlInfo.list[s32FindIndex].exit   = true;
        }
    }
    // 2 等待所有子线程退出
    if (s32SyncSemUseCnt > 0)
    {
        SyncWaitForRecvThreadFinish(m_stDataRecvCtrlInfo.pool,s32SyncSemUseCnt,TRUE,INFINITE);
    }

    // 3 移出设备队列
    for ( i = 0;  i < s32ClientNum ;  i++)
    {
        if(true == findDevNode4Ip(ps32ClientIpAddr[i],&s32FindIndex) )
        {
            //CloseHandle(m_stDataRecvCtrlInfo.list[s32FindIndex].event);

            removeDevNode(m_stDataRecvCtrlInfo.list,&m_stDataRecvCtrlInfo.cnt,s32FindIndex);
        }
    }
}

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
void editDevNode2DataRecvService( int *ps32OldClientIpAddr ,
                                 int *ps32NewClientIpAddr , 
                                 int *ps32NewClientPort,
                                 int *ps32NewClientProtocol,
                                 int *ps32NewServicePort,
                                 int  s32ClientNum,
                                 int *ps32EditResult)
{
    // 先删除节点
    deleteDevNode4DataRecvService(ps32OldClientIpAddr,s32ClientNum);

    // 再注册新的设备节点
    addDevInfo2DataRecvService(ps32NewClientIpAddr,
        ps32NewClientPort,
        ps32NewClientProtocol,
        ps32NewServicePort,
        s32ClientNum,
        ps32EditResult);
}

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
void recvDevData( int *ps32ClientIpAddr , 
                 int *ps32RecvSize,
                 int  s32ClientNum , 
                 int  s32RecvTimeout)
{
    int i;
    struct stMsgInfo *  pstMsgNode;

    // 1 更新接收数据控制线程的设备参数序列
    for ( i = 0;  i < s32ClientNum;  i++)
    {
        m_stDataRecvCtrlInfo.para.dev[i].ip      = ps32ClientIpAddr[i];
        m_stDataRecvCtrlInfo.para.dev[i].rcv_len = ps32RecvSize[i];
        m_stDataRecvCtrlInfo.para.dev[i].err     = DATA_RECV_ERROR_NO_ERROR;
    }

    // 2 更新接收数据控制线程的公共参数
    m_stDataRecvCtrlInfo.para.size    = s32ClientNum;
    m_stDataRecvCtrlInfo.para.timeout = s32RecvTimeout;

    m_stDataRecvCtrlInfo.frist = true;

    //Sleep(500);

    // 3 发送:RUN消息进入消息队列
    pstMsgNode = (struct stMsgInfo*)malloc(sizeof(struct stMsgInfo));
  
    pstMsgNode->opt = 3;

    postMsg2DataRecv(pstMsgNode);
}

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
int getRecvData4Ip( int s32IpAddr , unsigned char *pu8Buf1 , int s32BufLen1,
                   unsigned char *pu8Buf2 , int s32BufLen2,
                   unsigned char *pu8Buf3 , int s32BufLen3,
                   unsigned char *pu8Buf4 , int s32BufLen4,
                   unsigned char *pu8Buf5 , int s32BufLen5)
{
    int  s32DevNodeIndex = 0;

    if (false == findDevNode4Ip(s32IpAddr,&s32DevNodeIndex))
    {
        // 参数错误
        return DATA_RECV_ERROR_NO_ERROR;
    }

    if ( (s32BufLen1+s32BufLen2+s32BufLen3+s32BufLen4+s32BufLen5) > m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_len)
    {
        // 参数错误
        return DATA_RECV_ERROR_NO_ERROR;
    }

    if ( DATA_RECV_ERROR_NO_ERROR != m_stDataRecvCtrlInfo.list[s32DevNodeIndex].err )
    {
        return m_stDataRecvCtrlInfo.list[s32DevNodeIndex].err;
    }

    // 进行缓冲区1数据Copy
    if( s32BufLen1 == 0 )
    {
        return DATA_RECV_ERROR_NO_ERROR;
    }
    else
    {
        memcpy(pu8Buf1,m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_buf,s32BufLen1);
    }

    // 进行缓冲区2的数据Copy
    if( s32BufLen2 != 0 )
    {
        memcpy(pu8Buf2,&m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_buf[s32BufLen1],s32BufLen2);
    }

    // 进行缓冲区3的数据Copy
    if( s32BufLen3 != 0 )
    {
        memcpy(pu8Buf3,&m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_buf[s32BufLen1+s32BufLen2],s32BufLen3);
    }

    // 进行缓冲区4的数据Copy
    if( s32BufLen4 != 0 )
    {
        memcpy(pu8Buf4,&m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_buf[s32BufLen1+s32BufLen2+s32BufLen3],s32BufLen4);
    }

    // 进行缓冲区5的数据Copy
    if( s32BufLen5 != 0 )
    {
        memcpy(pu8Buf5,&m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_buf[s32BufLen1+s32BufLen2+s32BufLen3+s32BufLen4],s32BufLen5);
    }

    // 释放内存占用
    free(m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_buf);
    m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_buf = NULL;
    return 0;
}

/*******************************************************************************
* 函    数：restartDataRecv
* 描    述：复位数据接收,重新进行数据接收
* 输入参数：无
* 输出参数：无
* 返 回 值：无
* 说    明：无
******************************************************************************/
void restartDataRecv( void )
{
    struct stMsgInfo *  pstMsgNode;

    // 发送:RUN消息进入消息队列
    pstMsgNode      = (struct stMsgInfo*)malloc(sizeof(struct stMsgInfo));

    pstMsgNode->opt = 3;

    postMsg2DataRecv(pstMsgNode);

}

/*******************************************************************************
* 函    数：stopDataRecv
* 描    述：停止数据接收
* 输入参数：无
* 输出参数：无
* 返 回 值：无
* 说    明：无
******************************************************************************/
void stopDataRecv( void )
{
    struct stMsgInfo *  pstMsgNode;

    // 发送":WAV:CHANSEND 0"消息进入消息队列
    pstMsgNode      = (struct stMsgInfo*)malloc(sizeof(struct stMsgInfo));

    pstMsgNode->opt = 4;

    postMsg2DataRecv(pstMsgNode);
}