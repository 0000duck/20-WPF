
#include <stdio.h>
#include <WinSock2.h>
#include <process.h>
#include "device_recv_service.h"

#pragma comment(lib,"ws2_32.lib")

#define DATA_RECV_MAX_DEVICE_NUM                    128

#define DATA_RECV_MAX_BANDWIDTH                     (int)(80*1024*1024) // Ĭ��100MB/s�Ľ�����

#define DATA_RECV_MAX_BUFF_LEN                      (int)(8*1024*1024)

#define DATA_RECV_PROTOCAL_UDP                      (int)0         //  ʹ��UDPЭ��
#define DATA_RECV_PROTOCAL_TCP                      (int)1         //  ʹ��TCPЭ��

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
    DATA_RECV_EVENT_WAIT_FOR_RECV = 0,      //  �ȴ����ݽ���
    DATA_RECV_EVENT_RECV,                   //  �������ݽ���
    DATA_RECV_EVENT_AGAIN_RECV,             //  ��������δ�������,������ش�
    DATA_RECV_EVENT_EXIT                    //  �˳��߳�
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
* �����¼�豸��Ϣ�Ľṹ��
* ����:1.�豸IP��ַ
*      2.ͨ��Э��:0--UDP,1--TCP
*      3.ͨ��ʱ�Ķ˿ں�
*/
struct stDeviceInfo
{
    int                         ip;                                             //  �豸IP��ַ
    short                       protocal;                                       //  �豸��ͨ��Э��,0--UDP,1--TCP
    short                       cport;                                          //  �豸�˿ں�
    short                       sport;                                          //  ����˿ں�
    short                       err;                                            //  ������
    bool                        valid;                                          //  ��ǰ�ͻ����Ƿ��ѽ������߳�
    char   *                    recv_buf;                                       //  ���ջ�����
    int                         recv_size;                                      //  ���ջ������Ĵ�С
    int                         recv_len;                                       //  ���յ����ݳ���
    int                         timeout;                                        //  ���ճ�ʱʱ��
    HANDLE *                    finish;                                         //  �߳�ͬ��ʹ��
    bool                        ready;                                          //  �豸����׼�����
    bool                        exit;                                           //  ���߳��˳�
    bool                        busy;                                           //  ��ǰ�ڵ�ķ�æ״̬
    bool                        first;                                          //  �״ν������ݵı�ʶ
    bool                        run;                                            //  �Ƿ��ѷ���run����
    bool                        resend;                                         //  �������ش��¼�
    bool                        use;                                            //  ��ǰ�ڵ��Ƿ�ʹ��
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
    HANDLE                      pool[DATA_RECV_MAX_DEVICE_NUM]; //  ���߳��븸�̵߳�ͬ���ź�����
    int                         max;                            //  ֧�ֵ�����豸�ڵ���
    int                         cnt;                            //  ��ǰ��Ч���豸�ڵ���
    stDataRecvEventPara         para;                           //  �����̲߳���ʱ�Ĳ���
    struct stDeviceInfo         list[DATA_RECV_MAX_DEVICE_NUM]; //  ��ǰ��Ч���豸�ڵ���Ϣ
    bool                        block;                          //  �����̲߳���ʱ�Ĺ���ģʽ;true -- ����,false -- ������
    HANDLE                      sync;                           //  ����ʱͬ��ʹ��,ֻ�е�blockΪtrueʱ,����Ч
    bool                        limit;                          //  TCP���ݽ���ʱ,�Ƿ������������;true -- ����,false -- ������
    bool                        frist;                          //  �״�Startʱ,��ȴ��豸��λ���
    DATA_RECV_OPTION_REPORT     report;                         //  ���ݽ����߳��ϱ�
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

    // �豸����1
    (*ps32ListCnt)--;
    // ����β�ڵ���ж�
    /*if ( s32RemoveIndex <0 || s32RemoveIndex >= (*ps32ListCnt))
    {
    return;
    }*/
    // ��s32RemoveIndex��������ݽ���ǰ��
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

    if ( TRUE == bAllWait ) // �ȴ�ȫ���¼����
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
    else//ֻҪ��һ���¼����,�򷵻�
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

    // ָ����Ϣ����ͷ
    pstMsgInfo = m_pstMsgQueue;

    // �ж��Ƿ�����Ϣ,������Ϣ,���������Ϣ���е�β�������׳�
    if ( NULL != pstMsgInfo )
    {
        while ( NULL != pstMsgInfo->next )
        {
            pstBackMsgInfo = pstMsgInfo;
            pstMsgInfo = pstMsgInfo->next;
        }

        // ��Ϊ���е��׽ڵ�,�������Ϣ����
        if ( pstMsgInfo == m_pstMsgQueue )
        {
            m_pstMsgQueue = NULL;
        }
        else
        {
            // �����Ϣ������ɾ��β�ڵ�
            pstBackMsgInfo->next = NULL;
        }
    }

    ReleaseSemaphore(m_semMsgWRProtect,1,NULL);

    return pstMsgInfo;
}

void dataRecvFinishProcess( void *pThreadPara )
{
    struct stDeviceInfo*                pstClientInfo            = (struct stDeviceInfo*)pThreadPara; 

    // �ϱ����ݽ������,Ҫ���ϲ�������ݽ���
    m_stDataRecvCtrlInfo.report(0,pstClientInfo->ip,pstClientInfo->recv_len);
}

void dataRecvRunProcess( void *pThreadPara )
{
    struct stDeviceInfo*                pstClientInfo            = (struct stDeviceInfo*)pThreadPara;   

    // ����:RUN����
    m_stDataRecvCtrlInfo.report(3,pstClientInfo->ip,0);
}

void dataRecvStopProcess( void *pThreadPara )
{
    struct stDeviceInfo*                pstClientInfo            = (struct stDeviceInfo*)pThreadPara;   
    // ����:WAV:CHANSEND 0����
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

    // �ȴ�������Ϣ
    while (true)
    {
        pstMsgInfo = getMsg4DataRecv();

        if ( NULL == pstMsgInfo ) // ����Ϣ
        {
            // ��ǰ�����豸��������ݽ���
            if ( s32RealRecvFinishCnt > 0 )
            {
                // ������ǰ������Ч�豸,�Ƿ���ڻ����豸û�з�������(������ս�������)
                for ( i = 0;  i < m_stDataRecvCtrlInfo.para.size ;  i++)
                {
                    if (true == findDevNode4Ip(m_stDataRecvCtrlInfo.para.dev[i].ip,&s32DevNodeIndex))
                    {
                        if (( false == m_stDataRecvCtrlInfo.list[s32DevNodeIndex].ready )||(true == m_stDataRecvCtrlInfo.list[s32DevNodeIndex].ready)&&( 0 == m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_len))
                        {
                            // ��ʱ����
                            s32RecvTimeoutCnt ++;
                            break;
                        }
                    }

                }
                // ��2���������豸δ����,���������豸����������ݽ���,��ʼ�ϱ�
                if ( ( s32RecvTimeoutCnt == (m_stDataRecvCtrlInfo.para.timeout/100) ) || ( s32RealRecvFinishCnt == s32IdleDevCnt ) )
                {
                    for ( i = 0 ;  i < m_stDataRecvCtrlInfo.para.size ;  i++)
                    {
                        // ��ѯ�ڵ��Ƿ����
                        // ��ѯ�豸�ڵ�
                        if ( true == findDevNode4Ip(m_stDataRecvCtrlInfo.para.dev[i].ip,&s32DevNodeIndex) )
                        {
                            if (m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_len == 0)
                            {
                                m_stDataRecvCtrlInfo.report(0,m_stDataRecvCtrlInfo.para.dev[i].ip,DATA_RECV_ERROR_TIMEOUT);
                            }
                            else
                            {
                                 // �������ݽ�����ɵ��ϱ�
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
                    //    // ��ѯ�ڵ��Ƿ����
                    //    // ��ѯ�豸�ڵ�
                    //    if ( true == findDevNode4Ip(m_stDataRecvCtrlInfo.para.dev[i].ip,&s32DevNodeIndex) )
                    //    {
                    //        if (m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_len == 0)
                    //        {
                    //            continue;
                    //        }
                    //        // �������ݽ�����ɵ��ϱ�
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
        else // ������Ч��Ϣ
        {
            // ������Ϣ������ֵ,���д���
            switch ( pstMsgInfo->opt)
            {
            case 0 : // ��¼��ǰ�豸��������ݽ���
                s32NetBandWidth -= pstMsgInfo->dev->recv_size; // ���µ�ǰ�������ռ����

                // ���µ�ǰ�������ݽ��յ��豸���� �� 1
                s32CurrRecvDataDevCnt --;

                // ����ǰ�ڵ�Ľ��ճ��Ȳ�����0,����Ϊ�������
                if ( 0 != pstMsgInfo->dev->recv_len)
                {
                    s32RealRecvFinishCnt ++;
                    s32RecvTimeoutCnt = 0;
                }

                break;

            case 1 : // ���ݵ�ǰ����ģʽ,���豸�����������ݷ���ָ��,���¼�����TCPЭ�����
                // ����ǰ���������ռ�������ҵ�ǰ���������ڽ������ݽ���,��Ѹ���Ϣ�ͻ���Ϣ����
                if ( ( s32NetBandWidth > DATA_RECV_MAX_BANDWIDTH ) && s32CurrRecvDataDevCnt > 0 )
                {
                    postMsg2DataRecv(pstMsgInfo);
                }
                else
                {
                    /*
                    * ���Խ������ݵĽ���
                    * 1. �������豸�����߳�
                    * 2. �������ݷ����������
                    * 3. �����豸�Ľ��ռĴ����
                    */
                    ReleaseSemaphore(pstMsgInfo->sync,1,NULL); // �����豸�Ľ���

                    m_stDataRecvCtrlInfo.report(1,pstMsgInfo->dev->ip,0);// �������ݿ�ʼ���͵�����豸

                    // ���µ�ǰ�������ռ��
                    s32NetBandWidth += pstMsgInfo->dev->recv_size;
                    // ���µ�ǰ�������ݽ��յ��豸���� �� 1
                    s32CurrRecvDataDevCnt ++;
                }
                break;

            case 2 : // ���豸���������ش�ָ��

                /*
                * ���Խ��������ش�
                * 1. �õ�ǰ�豸���״ν��ձ�ʶΪfalse,�����ݽ����̲߳��ٽ������ݷ��������ִ��
                * 2. ���������ش�����豸
                * 3. �����豸�Ľ��ռĴ����
                */
                m_stDataRecvCtrlInfo.report(2,pstMsgInfo->dev->ip,0);// ���������ش�������豸

                break;

            case 3 : // ���ݵ�ǰ���е��豸�ڵ��Ƿ����,��������ָ��ķ���

                // ����ǰ��ֹͣ���ݽ���,���ٽ���RUN
                if ( true == bRecvStataIsStop )
                {
                    // �ͷű���RUN�¼�
                    free(pstMsgInfo);
                    bRecvStataIsStop = false;
                    break;
                }

                // ���״ο�������,�����5�θ��¼�
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

                // ����ǰ�����豸�ڽ������ݽ���,��ȴ�100ms��,����Ϣ�Żض���
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
                    // ��ʼ�����豸��:RUN�����
                    for ( i = 0 ;  i < m_stDataRecvCtrlInfo.para.size ;  i++)
                    {
                        // ��ѯ�ڵ��Ƿ����
                        // ��ѯ�豸�ڵ�
                        if ( true == findDevNode4Ip(m_stDataRecvCtrlInfo.para.dev[i].ip,&s32DevNodeIndex) )
                        {
                            // �����豸�ڵ�
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
                            // ����:RUN����
                            //_beginthread(dataRecvRunProcess,1024,&m_stDataRecvCtrlInfo.list[s32DevNodeIndex]);
                            dataRecvRunProcess(&m_stDataRecvCtrlInfo.list[s32DevNodeIndex]);
                            s32IdleDevCnt ++;
                        }
                    }
                    // �ͷ���Ϣ
                    free(pstMsgInfo);
                }
                break;
            case 4:
                // ����ǰ�����豸�ڽ������ݽ���,��ȴ�100ms��,����Ϣ�Żض���
                if ( 0 != s32CurrRecvDataDevCnt )
                {
                    Sleep(100);
                    postMsg2DataRecv(pstMsgInfo);
                }
                else
                {
                    bRecvStataIsStop = true;

                    // ������Ч�ڵ�,����:WAV:CHANSEND 0����
                    for ( i = 0;  i < 128;  i++)
                    {
                        if (m_stDataRecvCtrlInfo.list[i].finish != NULL)
                        {
                            // ����:WAV:CHANSEND 0����
                            _beginthread(dataRecvStopProcess, 1024, &m_stDataRecvCtrlInfo.list[i]);
                        }
                        
                    }
                    // �ͷ���Ϣ
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

    // 1. ��ʼ��WSA����
    if(WSAStartup(sockVersion,&wsaData)!=0)
    {
        // WSA��ʧ��
        pstClientInfo->err = DATA_RECV_ERROR_WSA_OPEN_FAIL;
        // ���̳߳�ʼ�����
        ReleaseSemaphore(*pstClientInfo->finish,1,NULL);
        return;
    }
    // 2 ����TCP Socket���
    s32SocketFd = socket(AF_INET,SOCK_STREAM,IPPROTO_TCP);
    if (s32SocketFd == INVALID_SOCKET)
    {
        // Socket�������ʧ��
        pstClientInfo->err = DATA_RECV_ERROR_SOCKET_ALLOC;
        // �ر�WSA����
        WSACleanup();
        // ���̳߳�ʼ�����
        ReleaseSemaphore(*pstClientInfo->finish,1,NULL);
        return;
    }

    // 3. �󶨷����ַ
    // 3.1 ��ʼ�������ַ��Ϣ
    memset(&stServiceAddr,0,sizeof(sockaddr_in));
    stServiceAddr.sin_family           = AF_INET;  //  IPv4
    stServiceAddr.sin_addr.S_un.S_addr = INADDR_ANY;
    stServiceAddr.sin_port             = htons(pstClientInfo->sport);
    // 3.2 ��
    if(bind(s32SocketFd,(sockaddr*)&stServiceAddr,s32AddrLen) == SOCKET_ERROR)
    {
        // ��ַ��ʧ��
        pstClientInfo->err = DATA_RECV_ERROR_ADDR_BIND_FAIL;
        // �ر�Socket���
        closesocket(s32SocketFd);
        // �ر�WSA����
        WSACleanup();
        // ���̳߳�ʼ�����
        ReleaseSemaphore(*pstClientInfo->finish,1,NULL);
        return;
    }

    // 4. ��ʼ����
    if(listen(s32SocketFd,5) == SOCKET_ERROR)
    {
        pstClientInfo->err = DATA_RECV_ERROR_LISTEN_FAIL;
        // �ر�Socket���
        closesocket(s32SocketFd);
        // �ر�WSA����
        WSACleanup();
        // ���̳߳�ʼ�����
        ReleaseSemaphore(*pstClientInfo->finish,1,NULL);
        return;
    }
    // �����̳߳�ʼ���ɹ�
    pstClientInfo->valid = true;

    // ���̳߳�ʼ�����
    ReleaseSemaphore(*pstClientInfo->finish,1,NULL);

    // ��ʼ����Ϣ
    stMsg.sync = CreateSemaphore(NULL,0,1,NULL);
    stMsg.dev  = pstClientInfo;

    //// �����̳߳�ʼ���ɹ�
    //pstClientInfo->valid = true;

    // ����Select��
    FD_ZERO(&back_set);
    FD_SET(s32SocketFd,&back_set);

    // ��ʼ�ȴ����ӵĽ���
    while ( false == pstClientInfo->exit )
    {
        read_set = back_set;

        // ��������
        if ( select(0,&read_set,NULL,NULL,&stSelectTimeout) < 0 )
        {
            continue;
        }

        // �ж��Ƿ������ӽ���
        if ( FD_ISSET(s32SocketFd,&read_set) )
        {
            // ��������
            s32CommSockFd = accept(s32SocketFd,(sockaddr*)&stClientAddr,&s32AddrLen);
            // ������׼����ʶΪ׼�����
            if ( true == pstClientInfo->run )
            {
                pstClientInfo->ready = true;
            }
            // ���ý��ջ�������С
            setsockopt(s32CommSockFd,SOL_SOCKET,SO_RCVBUF,(char*)&s32MaxRecvLen,sizeof(int));
            // ����ǰΪ�״ν�������
            if (true == pstClientInfo->first )
            {
                // �׳��������ݴ������Ϣ
                stMsg.opt = 1;
                postMsg2DataRecv(&stMsg);
                // �ȴ��������
                WaitForSingleObject(stMsg.sync,INFINITE);
                // ���״ν������ݱ�ʶΪfalse
                pstClientInfo->first = false;
            }
            // ��ʼ������ʹ�õı���
            s32RemainLen    = pstClientInfo->recv_size;
            u32RecvTotalLen = 0;
            ps8RecvBuff     = pstClientInfo->recv_buf;

            // �������ݽ���
            do
            {
                // ���ݵ�ǰ�Ƿ��ѷ���:RUN����������ݵĽ���,��û�з���,��ֱ�ӽ���,��������Ч���ݵ����
                if ( false == pstClientInfo->run )
                {

                    s32RecvLen = recv(s32CommSockFd,as8InvalidRecvBuff,1024,0);
                    if ( s32RecvLen == 0 ) // �ر�����
                    {
                        closesocket(s32CommSockFd);
                        // �׳����ݽ�����ɵ���Ϣ
                        stMsg.opt = 0;
                        postMsg2DataRecv(&stMsg);
                        break;
                    }
                }
                else
                {
                    s32RecvLen = recv(s32CommSockFd,ps8RecvBuff,s32RemainLen,0);
                    if ( s32RecvLen == 0 ) // �ر�����
                    {
                        closesocket(s32CommSockFd);
                        // ����ǰ���յ����ݵ��ܳ��ȵ���Ԥ������,������������
                        if ( u32RecvTotalLen == pstClientInfo->recv_size )
                        {
                            // ���½��յ����ݵĳ���
                            pstClientInfo->recv_len = u32RecvTotalLen;
                            // �׳����ݽ�����ɵ���Ϣ
                            stMsg.opt = 0;
                            postMsg2DataRecv(&stMsg);
                        }
                        else
                        {
                            if ( false == pstClientInfo->resend )
                            {
                                // �׳����������ش�����Ϣ
                                stMsg.opt = 2;
                                postMsg2DataRecv(&stMsg);
                                pstClientInfo->resend = true;
                            }
                            else
                            {
                                // ���½��յ����ݵĳ���
                                pstClientInfo->recv_len = u32RecvTotalLen;
                                // �׳����ݽ�����ɵ���Ϣ
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
    // �߳��˳�,֪ͨ�ϲ�
    ReleaseSemaphore(*pstClientInfo->finish,1,NULL);
}

/*******************************************************************************
* ��    ����createDataRecvService
* ��    �����������ݽ��շ���
* ���������
*           ��������                  ��������        ����˵��
*           pReportIO        DATA_RECV_OPTION_REPORT  ע����йܺ���
* �����������
* �� �� ֵ����
* ˵    ��������������豸֮ǰ�������ݽ��շ���,���������豸�����
******************************************************************************/
void createDataRecvService( DATA_RECV_OPTION_REPORT pReportIO )
{
    int i;

    // 1 ��ʼ�����ݽ��շ�����豸��Ϣ��
    memset(&m_stDataRecvCtrlInfo,0,sizeof(stDataRecvControlPara));

    // 2 ��ʼ���źų�
    for (i = 0; i < DATA_RECV_MAX_DEVICE_NUM ; i++)
    {
        m_stDataRecvCtrlInfo.pool[i] = CreateSemaphore(NULL,0,1,NULL); // ���������ź���
    }

    // 3 ��¼״̬�ϱ��ص�����
    m_stDataRecvCtrlInfo.report = pReportIO;

    // 4 �������տ����̻߳����ź���
    //m_semDataRecvCtrl = CreateSemaphore(NULL,0,1,NULL); // ���������ź���

    // 5 �������ݽ��յĿ����߳�
    //_beginthread(dataRecvControlProcess,1024,&m_stDataRecvCtrlInfo);

    // 6 ��ʼ����Ϣ����
    initMsgQueue4DataRecv();

    // 7 ������Ϣ�����߳�
    _beginthread(dataRecvMsgProcess,1024,NULL);
}
/*******************************************************************************
* ��    ����addDevInfo2DataRecvService
* ��    ���������ݽ��շ�������豸
* ���������
*           ��������                  ��������        ����˵��
*           ps32ClientIpAddr          int*            �豸IP��ַ����
*           ps32ClientPort            int*            �豸�������ݵĶ˿ں�
*           ps32ClientProtocol        int*            �豸���������ϴ�ʹ�õ�Э��
*                                                         0 -- UDP
*                                                         1 -- TCP
*           ps32ServicePort           int*            ����˽���ʱ����Ķ˿ں�
*           s32ClientNum              int             ������ӵ��豸����
* ���������
*           ��������                  ��������        ����˵��
*           ps32RegResult             int*            �豸��ӵĽ��
*                                                         0  -- �ɹ�
*                                                        -2  -- Socket�������ʧ��
*                                                        -3  -- �豸��ַ��ʧ��
*                                                        -8  -- �������ʧ��
*                                                        -10 -- WSA����ʧ��
*                                                        -11 -- ��������豸֧����
* �� �� ֵ����
* ˵    ����ps32ClientPortֻ����Э��ΪUDPʱ,�ſ���ʹ��
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

    // 1 �����豸IP��ַ
    for (i = 0; i < s32ClientNum; i++)
    {
        // 1.1 ����IP��ַ����Ƿ������豸�б���,��û������β�����
        if( false == findDevNode4Ip( ps32ClientIpAddr[i],NULL) && DATA_RECV_ERROR_NO_ERROR == ps32RegResult[i] )
        {
            s32IdleNodeIndex = findIdleNode4List();
            if ( -1 == s32IdleNodeIndex )
            {
                ps32RegResult[i] = DATA_RECV_ERROR_NODE_NUM_FULL;
                continue;
            }
            // ��ʼ������
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].ip        = ps32ClientIpAddr[i];                //  �豸IP��ַ
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].protocal  = ps32ClientProtocol[i];              //  �豸��ͨ��Э��,0--UDP,1--TCP
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].cport     = ps32ClientPort[i];                  //  �豸�˿ں�
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].sport     = ps32ServicePort[i];                 //  ����˿ں�
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].err       = DATA_RECV_ERROR_NO_ERROR;           //  ������
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].valid     = false;                              //  ��ǰ�ͻ����Ƿ��ѽ������߳�
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].recv_buf  = NULL;                               //  ���ջ�����
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].recv_size = 0;                                  //  ���ջ������Ĵ�С
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].recv_len  = 0;                                  //  ���յ����ݳ���
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].timeout   = 2000;                               //  ���ճ�ʱʱ��
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].finish    = NULL;                               //  �߳�ͬ��ʹ��
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].ready     = false;                              //  �豸����׼�����
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].busy      = false;                              //  ��ǰ�ڵ�ķ�æ״̬
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].exit      = false;                              //  ���߳��˳�
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].first     = true;                               //  �״ν������ݵı�ʶ
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].run       = false;                              //  �Ƿ��ѷ���run����
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].resend    = false;                              //  �������ش��¼�
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].use       = true;                               //  ���豸�ڵ�Ϊ��ʹ��(true)

            // ����ͬ���ź���
           // m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].finish    = &m_stDataRecvCtrlInfo.pool[s32SyncSemUseCnt++];
            m_stDataRecvCtrlInfo.list[s32IdleNodeIndex].finish = &m_stDataRecvCtrlInfo.pool[s32SyncSemUseCnt++];
         
            // ����Э������̷߳���
            if ( DATA_RECV_PROTOCAL_UDP == ps32ClientProtocol[i] ) // UDPЭ��
            {

            }
            else // TCPЭ��
            {
                _beginthread(tcpRecvServer,2048,&m_stDataRecvCtrlInfo.list[s32IdleNodeIndex]);
            }
            // �����豸������1
            m_stDataRecvCtrlInfo.cnt ++;
        }
    }
    // 2 �ȴ��������̳߳�ʼ�����
    if (s32SyncSemUseCnt > 0)
    {
        SyncWaitForRecvThreadFinish(m_stDataRecvCtrlInfo.pool,s32SyncSemUseCnt,TRUE,INFINITE);
    }

    // 3 ������ӽ��
    for (i = 0; i < s32ClientNum; i++)
    {
        if( DATA_RECV_ERROR_NO_ERROR == ps32RegResult[i] )
        {
            // 3.1 �����豸�ڵ�
            findDevNode4Ip( ps32ClientIpAddr[i] , &s32FindIndex );

            // 3.2 ��ȡ���
            ps32RegResult[i] = m_stDataRecvCtrlInfo.list[s32FindIndex].err;

            // 3.3 �ж��ڵ�����߳��Ƿ����ɹ�,�����ɹ�,������Ƴ�
            if ( false == m_stDataRecvCtrlInfo.list[s32FindIndex].valid )
            {
                // �Ƴ��豸����
                removeDevNode( m_stDataRecvCtrlInfo.list , &m_stDataRecvCtrlInfo.cnt , s32FindIndex );
            }
        }
    }
}
/*******************************************************************************
* ��    ����deleteDevNode4DataRecvService
* ��    ���������ݽ��շ������Ƴ��豸
* ���������
*           ��������                  ��������        ����˵��
*           ps32ClientIpAddr          int*            �豸IP��ַ����
*           s32ClientNum              int             �����Ƴ����豸����
* �����������
* �� �� ֵ����
* ˵    ������
******************************************************************************/
void deleteDevNode4DataRecvService( int *ps32ClientIpAddr , int  s32ClientNum )
{
    int         s32SyncSemUseCnt = 0;
    int         i;
    int         s32FindIndex = 0;

    // 1 ����IP��ַ�����豸����
    for ( i = 0;  i < s32ClientNum ;  i++)
    {
        if (true == findDevNode4Ip(ps32ClientIpAddr[i],&s32FindIndex))
        {
            m_stDataRecvCtrlInfo.list[s32FindIndex].finish = &m_stDataRecvCtrlInfo.pool[s32SyncSemUseCnt++];
            m_stDataRecvCtrlInfo.list[s32FindIndex].exit   = true;
        }
    }
    // 2 �ȴ��������߳��˳�
    if (s32SyncSemUseCnt > 0)
    {
        SyncWaitForRecvThreadFinish(m_stDataRecvCtrlInfo.pool,s32SyncSemUseCnt,TRUE,INFINITE);
    }

    // 3 �Ƴ��豸����
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
* ��    ����editDevNode2DataRecvService
* ��    ���������ݽ��շ�������豸
* ���������
*           ��������                  ��������        ����˵��
*           ps32OldClientIpAddr       int*            ���޸ĵ��豸IP��ַ,��Ϊʶ����
*           ps32NewClientIpAddr       int*            �豸IP��ַ����
*           ps32NewClientPort         int*            �豸�������ݵĶ˿ں�
*           ps32NewClientProtocol     int*            �豸���������ϴ�ʹ�õ�Э��
*                                                         0 -- UDP
*                                                         1 -- TCP
*           ps32NewServicePort        int*            ����˽���ʱ����Ķ˿ں�
*           s32ClientNum              int             ���δ��޸ĵ��豸����
* ���������
*           ��������                  ��������        ����˵��
*           ps32EditResult            int*            �豸�޸ĵĽ��
*                                                         0  -- �ɹ�
*                                                        -2  -- Socket�������ʧ��
*                                                        -3  -- �豸��ַ��ʧ��
*                                                        -8  -- �������ʧ��
*                                                        -10 -- WSA����ʧ��
* �� �� ֵ����
* ˵    ����ps32ClientPortֻ����Э��ΪUDPʱ,�ſ���ʹ��
******************************************************************************/
void editDevNode2DataRecvService( int *ps32OldClientIpAddr ,
                                 int *ps32NewClientIpAddr , 
                                 int *ps32NewClientPort,
                                 int *ps32NewClientProtocol,
                                 int *ps32NewServicePort,
                                 int  s32ClientNum,
                                 int *ps32EditResult)
{
    // ��ɾ���ڵ�
    deleteDevNode4DataRecvService(ps32OldClientIpAddr,s32ClientNum);

    // ��ע���µ��豸�ڵ�
    addDevInfo2DataRecvService(ps32NewClientIpAddr,
        ps32NewClientPort,
        ps32NewClientProtocol,
        ps32NewServicePort,
        s32ClientNum,
        ps32EditResult);
}

/*******************************************************************************
* ��    ����recvDevData
* ��    �����������ݵĽ��մ���
* ���������
*           ��������                  ��������        ����˵��
*           ps32ClientIpAddr          int*            �豸IP��ַ,��Ϊʶ����
*           ps32RecvSize              int*            ���ÿ̨�豸,׼�����յ����ݳ���
*           s32ClientNum              int             ���δ��޸ĵ��豸����
*           s32RecvTimeout            int             ��ʱʱ��,����
* �����������
* �� �� ֵ����
* ˵    ������
******************************************************************************/
void recvDevData( int *ps32ClientIpAddr , 
                 int *ps32RecvSize,
                 int  s32ClientNum , 
                 int  s32RecvTimeout)
{
    int i;
    struct stMsgInfo *  pstMsgNode;

    // 1 ���½������ݿ����̵߳��豸��������
    for ( i = 0;  i < s32ClientNum;  i++)
    {
        m_stDataRecvCtrlInfo.para.dev[i].ip      = ps32ClientIpAddr[i];
        m_stDataRecvCtrlInfo.para.dev[i].rcv_len = ps32RecvSize[i];
        m_stDataRecvCtrlInfo.para.dev[i].err     = DATA_RECV_ERROR_NO_ERROR;
    }

    // 2 ���½������ݿ����̵߳Ĺ�������
    m_stDataRecvCtrlInfo.para.size    = s32ClientNum;
    m_stDataRecvCtrlInfo.para.timeout = s32RecvTimeout;

    m_stDataRecvCtrlInfo.frist = true;

    //Sleep(500);

    // 3 ����:RUN��Ϣ������Ϣ����
    pstMsgNode = (struct stMsgInfo*)malloc(sizeof(struct stMsgInfo));
  
    pstMsgNode->opt = 3;

    postMsg2DataRecv(pstMsgNode);
}

/*******************************************************************************
* ��    ����getTcpSocketData45(�ⲿ�ӿ�)
* ��    �������ݿͻ����豸IP��ַ,�������ݵ���ȡ
* ���������
*           ��������              ��������        ����˵��
*           s32IpAddr             int             �ͻ����豸IP��ַ
*           s32BufLen1            int             ���ݻ�����1�Ĵ�С
*           s32BufLen2            int             ���ݻ�����2�Ĵ�С
*           s32BufLen3            int             ���ݻ�����3�Ĵ�С
*           s32BufLen4            int             ���ݻ�����4�Ĵ�С
*           s32BufLen5            int             ���ݻ�����5�Ĵ�С
* ���������
*           ��������              ��������        ����˵��
*           pu8Buf1               unsigned char*  ���ݻ�����1���׵�ַ
*           pu8Buf2               unsigned char*  ���ݻ�����2���׵�ַ
*           pu8Buf3               unsigned char*  ���ݻ�����3���׵�ַ
*           pu8Buf4               unsigned char*  ���ݻ�����4���׵�ַ
*           pu8Buf5               unsigned char*  ���ݻ�����5���׵�ַ
* �� �� ֵ����
* ˵    ����1.�ú����������5��������,���л�����1�������,�������������뱣֤
*             ǰһ�����������뱣�ϴ���
*           2.����Ļ��������ȱ���С�ڵ��ڻ���������Ч��С
*           3.�ڽ�������Copy��ɺ�,������н��ջ��������ͷ�,�����ڴ��˷�
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
        // ��������
        return DATA_RECV_ERROR_NO_ERROR;
    }

    if ( (s32BufLen1+s32BufLen2+s32BufLen3+s32BufLen4+s32BufLen5) > m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_len)
    {
        // ��������
        return DATA_RECV_ERROR_NO_ERROR;
    }

    if ( DATA_RECV_ERROR_NO_ERROR != m_stDataRecvCtrlInfo.list[s32DevNodeIndex].err )
    {
        return m_stDataRecvCtrlInfo.list[s32DevNodeIndex].err;
    }

    // ���л�����1����Copy
    if( s32BufLen1 == 0 )
    {
        return DATA_RECV_ERROR_NO_ERROR;
    }
    else
    {
        memcpy(pu8Buf1,m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_buf,s32BufLen1);
    }

    // ���л�����2������Copy
    if( s32BufLen2 != 0 )
    {
        memcpy(pu8Buf2,&m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_buf[s32BufLen1],s32BufLen2);
    }

    // ���л�����3������Copy
    if( s32BufLen3 != 0 )
    {
        memcpy(pu8Buf3,&m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_buf[s32BufLen1+s32BufLen2],s32BufLen3);
    }

    // ���л�����4������Copy
    if( s32BufLen4 != 0 )
    {
        memcpy(pu8Buf4,&m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_buf[s32BufLen1+s32BufLen2+s32BufLen3],s32BufLen4);
    }

    // ���л�����5������Copy
    if( s32BufLen5 != 0 )
    {
        memcpy(pu8Buf5,&m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_buf[s32BufLen1+s32BufLen2+s32BufLen3+s32BufLen4],s32BufLen5);
    }

    // �ͷ��ڴ�ռ��
    free(m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_buf);
    m_stDataRecvCtrlInfo.list[s32DevNodeIndex].recv_buf = NULL;
    return 0;
}

/*******************************************************************************
* ��    ����restartDataRecv
* ��    ������λ���ݽ���,���½������ݽ���
* �����������
* �����������
* �� �� ֵ����
* ˵    ������
******************************************************************************/
void restartDataRecv( void )
{
    struct stMsgInfo *  pstMsgNode;

    // ����:RUN��Ϣ������Ϣ����
    pstMsgNode      = (struct stMsgInfo*)malloc(sizeof(struct stMsgInfo));

    pstMsgNode->opt = 3;

    postMsg2DataRecv(pstMsgNode);

}

/*******************************************************************************
* ��    ����stopDataRecv
* ��    ����ֹͣ���ݽ���
* �����������
* �����������
* �� �� ֵ����
* ˵    ������
******************************************************************************/
void stopDataRecv( void )
{
    struct stMsgInfo *  pstMsgNode;

    // ����":WAV:CHANSEND 0"��Ϣ������Ϣ����
    pstMsgNode      = (struct stMsgInfo*)malloc(sizeof(struct stMsgInfo));

    pstMsgNode->opt = 4;

    postMsg2DataRecv(pstMsgNode);
}