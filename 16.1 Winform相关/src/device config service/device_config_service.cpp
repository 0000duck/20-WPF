/********************************************************************
                ��Դ����Ƽ��ɷ����޹�˾ ��Ȩ����(2020 - 2030)
*********************************************************************
ͷ�ļ���: device_config_service.cpp
��������: 1.���豸���÷����ϼܿͻ�����Ϣ
          2.��Tcp Socket�����¼ܿͻ�����Ϣ
          3.��������ģʽ�µ����ݽ���
          4.��ȡTcp Socket������յ�������
��   ��: sn01625
��   ��: 0.1
��������: 2020-06-30  15:06 PM

�޸ļ�¼1��// �޸���ʷ��¼�������޸����ڡ��޸��߼��޸�����
�޸����ڣ�2020-07-04
�� �� �ţ�0.2
�� �� �ˣ�sn01625
�޸����ݣ�1.�ṹ��stDeviceConfigInfo��ӽڵ㵱ǰ��ʹ�õı�ʶ,��������
            �ͻ���IP��ַһ��ʱ,ֻ���ѵ����߳�,���µ��쳣
            ��Ӱ�캯��ΪregDevConfigCilent���ʼ���ñ���
                        write2Device-ʹ��ǰ�ȳ�ʼ���ñ���,ʹ��ʱ��true
                                     ��⵽�ñ���Ϊtrueʱ,�����ñ���
                        read4Device -ͬwrite2Device��������
                        query4Device-ͬwrite2Device��������
�޸ļ�¼2��// �޸���ʷ��¼�������޸����ڡ��޸��߼��޸�����
�޸����ڣ�2020-07-07
�� �� �ţ�0.3
�� �� �ˣ�sn01625
�޸����ݣ�1.�޸�write2Device��query4Device��д����������������С Ϊ
            ÿ��IP��ַ��Ӧһ������
*********************************************************************/
#include <stdio.h>
#include <WinSock2.h>
#include <process.h>
#include "device_config_service.h"

#pragma comment(lib,"ws2_32.lib")

/********************************************************************
                             �궨������
*********************************************************************/

#define DEVICE_CONFIG_SERVICE_START_PORT                (short)10001   //  Socket�Ķ˿ڷ�����ʼֵ

#define DEVICE_CONFIG_SERVICE_PROTOCAL_UDP              (int)0         //  ʹ��UDPЭ��
#define DEVICE_CONFIG_SERVICE_PROTOCAL_TCP              (int)1         //  ʹ��TCPЭ��

#define DEVICE_CONFIG_SERVICE_SEND_BUFF_LEN             (int)1024      //  ���ͻ���������
#define DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN             (int)(1024*2)  //  ���ջ���������

#define DEVICE_CONFIG_SERVICE_WARNING_OPENED            (int)1         //  �����豸�ѽ�������
#define DEVICE_CONFIG_SERVICE_NO_ERROR                  (int)0         //  �޴���
#define DEVICE_CONFIG_SERVICE_WSA_ERROR                 (int)-1        //  WSA��ʼ��ʧ��
#define DEVICE_CONFIG_SERVICE_CONNECT_ERROR             (int)-2        //  �ͻ��˽���ʧ��
#define DEVICE_CONFIG_SERVICE_RECV_TIMEOUT_ERROR        (int)-3        //  ���ճ�ʱ
#define DEVICE_CONFIG_SERVICE_INIT_TIMEOUT_ERROR        (int)-4        //  ���̳߳�ʼ��ʧ��
#define DEVICE_CONFIG_SERVICE_ADDR_BIND_FAIL_ERROR      (int)-5        //  ��������ַ��ʧ��
#define DEVICE_CONFIG_SERVICE_CLIENT_REGISTER_ERROR     (int)-6        //  �ͻ�����Ϣ��ע��ʧ��
#define DEVICE_CONFIG_SERVICE_SOCKET_IO_ERROR           (int)-7        //  ͨ��IO����
#define DEVICE_CONFIG_SERVICE_RESOURCE_NO_FIND_ERROR    (int)-8        //  �豸δ����
#define DEVICE_CONFIG_SERVICE_PARAMETER_ERROR           (int)-9        //  �����������
#define DEVICE_CONFIG_SERVICE_DEVICE_NUM_ERROR          (int)-10       //  �豸����Ϊ0
#define DEVICE_CONFIG_SERVICE_MALLOC_ERROR              (int)-11       //  �ڴ�����ʧ��
#define DEVICE_CONFIG_SERVICE_NO_OPEN_ERROR             (int)-12       //  �豸��û�д�
#define DEVICE_CONFIG_SERVICE_PROTOCAL_ERROR            (int)-13       //  Э�����

/********************************************************************
                             ö�ٶ�������
*********************************************************************/
/*
 * �����豸���÷�����¼�
 */
enum emDevConfigEvent
{
    DEVICE_CONFIG_SERVICE_EVENT_OPEN = 0,      //  ������
    DEVICE_CONFIG_SERVICE_EVENT_WRITE,         //  �����豸д�����
    DEVICE_CONFIG_SERVICE_EVENT_READ,          //  �����豸��ȡ����
    DEVICE_CONFIG_SERVICE_EVENT_QUERY,         //  ���豸���ж�д����
    DEVICE_CONFIG_SERVICE_EVENT_CLOSE,         //  �ر�����--����Ե�̨�豸
    DEVICE_CONFIG_SERVICE_EVENT_EXIT           //  �˳��߳�
}DEVICE_CONFIG_EVENT_EM;

/********************************************************************
                            �ṹ�嶨������
*********************************************************************/

/*
 * �����¼�豸��Ϣ�Ľṹ��
 * ����:1.�豸IP��ַ
 *      2.ͨ��Э��:0--UDP,1--TCP
 *      3.ͨ��ʱ�Ķ˿ں�
 */
struct stDeviceConfigInfo
{
    int                         ip;                                             //  �豸IP��ַ
    short                       protocal;                                       //  �豸��ͨ��Э��,0--UDP,1--TCP
    short                       cport;                                          //  �豸�˿ں�
    short                       sport;                                          //  ����˿ں�
    short                       err;                                            //  ������
    bool                        valid;                                          //  ��ǰ�ͻ����Ƿ��ѽ������߳�
    char                        send_buf[DEVICE_CONFIG_SERVICE_SEND_BUFF_LEN];  //  ���ͻ�����
    int                         send_len;                                       //  ���������ݵĳ���
    char                        recv_buf[DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN];  //  ���ջ�����
    int                         recv_len;                                       //  �������ݵĳ���
    int                         timeout;                                        //  ���ճ�ʱʱ��
    enum emDevConfigEvent       opt;                                            //  ������ֵ,���嶨���emDevConfigEvent����
    HANDLE                      event;                                          //  �¼����
    HANDLE *                    finish;                                         //  �߳�ͬ��ʹ��
    bool                        use;                                            //  ��ʶ�ýڵ��Ƿ�ʹ�� 2020-07-04 ���
    struct stDeviceConfigInfo * next;                                           //  ָ����һ���ڵ�
};

/********************************************************************
                          ���ر�����������
*********************************************************************/
struct stDeviceConfigInfo    *m_pstDevConfigClientInfo    = NULL;  //  ��¼�豸���÷���Ŀͻ�����Ϣ
HANDLE *                      m_semDevConfigFinishPool    = NULL;  //  ���߳�ͬ��ʹ�õ��ź�����,���ڽ����̼߳�ͬ��
int                           m_s32DevConfigFinishPoolNum = 0;

/********************************************************************
                            �ڲ���������
*********************************************************************/

/*******************************************************************************
  * ��    ����insertDeviceNode(�ڲ��ӿ�)
  * ��    �������豸�ͻ�����Ϣ�����в����µ��豸�ڵ�
  * ���������
  *           ��������              ��������           ����˵��
  *           pstNode      struct stDeviceConfigInfo*  �µ��豸�ڵ�
  * �����������
  * �� �� ֵ����
  * ˵    ������
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
  * ��    ����seekDeviceList(�ڲ��ӿ�)
  * ��    �������豸�ͻ�����Ϣ�����в����豸�ڵ�
  * ���������
  *           ��������              ��������           ����˵��
  *           s32IpAddr             int                �豸IP��ַ
  *           s32Port               int                �豸�˿ں�
  * �����������
  * �� �� ֵ����ΪNULL����Ϊû�в�ѯ��,���򷵻��豸�ڵ�
  * ˵    ������
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
  * ��    ����deleteDeviceNode(�ڲ��ӿ�)
  * ��    �������豸�ͻ�����Ϣ������ɾ��ָ�����豸�ڵ�
  * ���������
  *           ��������              ��������           ����˵��
  *           s32IpAddr             int                �豸IP��ַ
  *           s32Port               int                �豸�˿ں�
  * �����������
  * �� �� ֵ����
  * ˵    �����ڽ����豸�ڵ��ɾ��ʱ,��ر��¼��ź���
 ******************************************************************************/
void deleteDeviceNode( int s32IpAddr , int s32Port )
{
    struct stDeviceConfigInfo * pstNode,*pstBackNode;

    pstNode = m_pstDevConfigClientInfo;

    // �ж������Ƿ�Ϊ��
    if( m_pstDevConfigClientInfo == NULL )
    {
        return;
    }

    // ����Ҫɾ���Ľڵ�
    while ( (pstNode->ip != s32IpAddr)&&
            /*(pstNode->cport != s32Port)&&*/
            (pstNode->next != NULL) )
    {
        pstBackNode = pstNode;
        pstNode     = pstNode->next;
    }
    // ����IP��ַ�Ͷ˿ں��ҵ��ڵ�
    if( (pstNode->ip == s32IpAddr) && (pstNode->cport == s32Port))
    {
        if( pstNode == m_pstDevConfigClientInfo )// �ҵ���Ϊͷ���
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
  * ��    ����deleteAllDeviceNode(�ڲ��ӿ�)
  * ��    ����ɾ�����е��豸��Ϣ
  * �����������
  * �����������
  * �� �� ֵ����
  * ˵    �����ڽ����豸�ڵ��ɾ��ʱ,��ر��¼��ź���
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
  * ��    ����deleteAllDeviceNode(�ڲ��ӿ�)
  * ��    ����ɾ�����е��豸��Ϣ
  * �����������
  * �����������
  * �� �� ֵ����
  * ˵    �����ڽ����豸�ڵ��ɾ��ʱ,��ر��¼��ź���
 ******************************************************************************/
int updateDeviceListErr( int *ps32IpAddr,int *ps32Port,int s32DeviceNum,int *ps32Result )
{
    struct stDeviceConfigInfo *pstNode = NULL;
    int                        i;
    int                        s32RetCode = 0;

    // 1. �����豸
    for (i = 0; i < s32DeviceNum; i++)
    {
        // 1.1 ��ѯ�豸�ڵ�
        pstNode = seekDeviceList(ps32IpAddr[i],ps32Port[i]);

        // 1.2 �ж��豸�ڵ��Ƿ���Ч
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
  * ��    ����SyncWaitForConfigThreadFinish(�ڲ��ӿ�)
  * ��    ����ͬ���ȴ����н����߳�������ݽ���
  * ���������
  *           ��������              ��������        ����˵��
  *           handles               HANDLE*         �ź����ص�ַ
  *           count                 int             ������Ҫͬ�����ź�������
  * �����������
  * �� �� ֵ���ź�������С����ֵ
  * ˵    ����1.����WaitForMultipleObjectsÿ�����ȴ�����ΪMAXIMUM_WAIT_OBJECTS
  *             ����ʹ�ú������з�װ
  *           2.WaitForMultipleObjects�ķ���ֵ���Ժ���,�ú�����Ȼ���
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
  * ��    ����devConfigProcess(�ڲ��ӿ�)
  * ��    ����1. ���ݴ���Э��,����ͨ�ž������Դ����
  *           2. ������Э��ΪTCP,�������ͻ��˵����ӽ���
  *           3. �����¼��Ľ��պͶ��̵߳�ͬ��
  *           4. �������ݵĶ�д����
  * ���������
  *           ��������              ��������        ����˵��
  *           pThreadPara           void*           �̵߳ĳ�ʼ������,���嶨���
  *                                                 struct stDeviceConfigInfo
  * �����������
  * �� �� ֵ����
  * ˵    ������
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
     * ����Ĳ�����������ڼ���Connect���ӳ�ʱ
     */
    int                                 s32WorkMode         = 0;
    struct timeval                      stConnTimeout       = {1,500}; // 1500����
    fd_set                              stSelectFd          = {0};
    int                                 s32LastErrCode      = 0;

    // 1. ��ʼ��������
    pstClientInfo->err = DEVICE_CONFIG_SERVICE_NO_ERROR;

    // 2. ��ʼ��WSA����
    WORD sockVersion = MAKEWORD(2,2);
    WSADATA wsaData;

    if( WSAStartup(sockVersion,&wsaData) != 0 )
    {
        pstClientInfo->err = DEVICE_CONFIG_SERVICE_WSA_ERROR;
        ReleaseSemaphore(*pstClientInfo->finish,1,NULL);
        return ;
    }

    // 3. �̳߳�ʼ�����,��֪���߳�
    ReleaseSemaphore(*pstClientInfo->finish,1,NULL);

    // 4. �ȴ��¼��ĵ���,�����¼����ͽ��п��� 
    while (bThreadExitFlag==false)
    {
        // 4.1 �ȴ��¼�
        WaitForSingleObject(pstClientInfo->event,INFINITE);
        // 4.2 �����¼����ͽ��д���
        switch (pstClientInfo->opt)
        {
            case DEVICE_CONFIG_SERVICE_EVENT_OPEN:  //  ���豸
                // ����ǰΪUDPЭ��,��󶨱��ص�ַ
                if ( pstClientInfo->protocal == DEVICE_CONFIG_SERVICE_PROTOCAL_UDP )
                {
                    // ����UDP Socket���
                    s32SocketFd = socket(AF_INET,SOCK_DGRAM,IPPROTO_UDP);

                    // ��ʼ����������ַ
                    memset(&stServiceAddr,0,sizeof(sockaddr_in));
                    stServiceAddr.sin_family           = AF_INET;  //  IPv4
                    stServiceAddr.sin_addr.S_un.S_addr = INADDR_ANY;
                    stServiceAddr.sin_port             = htons(pstClientInfo->sport);
                    // ���԰�
                    if(bind(s32SocketFd,(sockaddr*)&stServiceAddr,sizeof(sockaddr_in)) == SOCKET_ERROR)
                    {
                        // ��Ǵ���
                        pstClientInfo->err = DEVICE_CONFIG_SERVICE_ADDR_BIND_FAIL_ERROR;
                        // �رձ��ε�Socket���
                        closesocket(s32SocketFd);
                        break;
                    }
                    else
                    {
                        // ��ͨ�ſ���ʹ�õı�ʶ
                        pstClientInfo->valid = true;
                    }
                }
                // ����ǰΪTCPЭ��,�����豸����������
                else if (pstClientInfo->protocal == DEVICE_CONFIG_SERVICE_PROTOCAL_TCP)
                {
                    // ����TCP Socket���
                    s32SocketFd = socket(AF_INET,SOCK_STREAM,IPPROTO_TCP);

                    // ��ʼ���ͻ��˵�ַ
                    memset(&stClientAddr,0,sizeof(sockaddr_in));
                    stClientAddr.sin_family           = AF_INET;  //  IPv4
                    stClientAddr.sin_addr.S_un.S_addr = pstClientInfo->ip;
                    stClientAddr.sin_port             = htons(pstClientInfo->cport);

                    // �����õ�ַ����
                    s32WorkMode = 1;
                    setsockopt(s32SocketFd,SOL_SOCKET,SO_REUSEADDR,(char*)&s32WorkMode,sizeof(int));

                    // �����õ�ǰSocketΪ������ģʽ,����connect����ʧ��ʱ�����
        
                   // ioctlsocket(s32SocketFd,FIONBIO,(u_long*)&s32WorkMode);
                    // ��ʼ����
                    if( connect(s32SocketFd,(sockaddr*)&stClientAddr,sizeof(stClientAddr) ) == SOCKET_ERROR )
                    {
                        FD_ZERO(&stSelectFd);
                        FD_SET(s32SocketFd,&stSelectFd);
                        if( select(0,NULL,&stSelectFd,NULL,&stConnTimeout) <= 0 )
                        {
                            // ��Ǵ���
                            pstClientInfo->err = DEVICE_CONFIG_SERVICE_CONNECT_ERROR;
                            // �رձ��ε�Socket���
                            closesocket(s32SocketFd);
                            break;
                        }
                        else
                        {
                            // �ٴ����õ�ǰSocketΪ����ģʽ
                            s32WorkMode = 0;
                            ioctlsocket(s32SocketFd, FIONBIO, (u_long*)&s32WorkMode);
                            // ��ͨ�ſ���ʹ�õı�ʶ
                            pstClientInfo->valid = true;
                        }
                    }
                    else
                    {
                        // �ٴ����õ�ǰSocketΪ����ģʽ
                        s32WorkMode = 0;
                        ioctlsocket(s32SocketFd,FIONBIO,(u_long*)&s32WorkMode);
                        // ��ͨ�ſ���ʹ�õı�ʶ
                        pstClientInfo->valid = true;
                    }
                }
                else
                {
                    pstClientInfo->err = DEVICE_CONFIG_SERVICE_PROTOCAL_ERROR;
                }
                break;

            case DEVICE_CONFIG_SERVICE_EVENT_WRITE: //  �����豸����д����
                //  ����Э����÷��ͽӿ�
                if (pstClientInfo->protocal == DEVICE_CONFIG_SERVICE_PROTOCAL_UDP ) // UDPЭ��
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
                // ���ý��ճ�ʱʱ��
                s32RecvTimeout = pstClientInfo->timeout;
                setsockopt(s32SocketFd,SOL_SOCKET,SO_RCVTIMEO,(char*)&s32RecvTimeout,sizeof(int));

                //  ����Э����ý��սӿ�
                if (pstClientInfo->protocal == DEVICE_CONFIG_SERVICE_PROTOCAL_UDP ) // UDPЭ��
                {
                    s32AddrLen = sizeof(sockaddr_in);
                    s32RecvLen = recvfrom(s32SocketFd,pstClientInfo->recv_buf,pstClientInfo->recv_len,0,(sockaddr*)&stClientAddr,&s32AddrLen);
                }
                else
                {
                    s32RecvLen = recv(s32SocketFd,pstClientInfo->recv_buf,pstClientInfo->recv_len,0);
                }
                // ���½��ճ���
                if( s32RecvLen == -1 )//�����ճ�ʱ,���ý��ճ���Ϊ0
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
                // ���ý��ճ�ʱʱ��
                s32RecvTimeout = pstClientInfo->timeout;
                setsockopt(s32SocketFd,SOL_SOCKET,SO_RCVTIMEO,(char*)&s32RecvTimeout,sizeof(int));
                //  ����Э����ý��սӿ�
                if (pstClientInfo->protocal == DEVICE_CONFIG_SERVICE_PROTOCAL_UDP ) // UDPЭ��
                {
                    // ��������
                    sendto(s32SocketFd,pstClientInfo->send_buf,pstClientInfo->send_len,0,(sockaddr*)&stClientAddr,sizeof(sockaddr));
                    // �ȴ���������
                    s32AddrLen = sizeof(sockaddr_in);
                    s32RecvLen = recvfrom(s32SocketFd,pstClientInfo->recv_buf,pstClientInfo->recv_len,0,(sockaddr*)&stClientAddr,&s32AddrLen);
                }
                else
                {
                    // ��������
                    send(s32SocketFd,pstClientInfo->send_buf,pstClientInfo->send_len,0);
                    // �ȴ���������
                    s32RecvLen = recv(s32SocketFd,pstClientInfo->recv_buf,pstClientInfo->recv_len,0);
                }
                // ���½��ճ���
                if( s32RecvLen == -1 )//�����ճ�ʱ,���ý��ճ���Ϊ0
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
                // �ر�Socket���
                closesocket(s32SocketFd);
                pstClientInfo->valid = false;
                break;
            case DEVICE_CONFIG_SERVICE_EVENT_EXIT:
                // ���߳��˳���ʶΪtrue
                bThreadExitFlag = true;
                break;

            default:
                break;
        }
        // 4.3. �̲߳������,��֪���߳�
        if(pstClientInfo->opt != DEVICE_CONFIG_SERVICE_EVENT_EXIT)
        {
            ReleaseSemaphore(*pstClientInfo->finish,1,NULL);
        }
    }
    
    WSACleanup();
    
    // 5. �̲߳������,��֪���߳�
    ReleaseSemaphore(*pstClientInfo->finish,1,NULL);
}

/********************************************************************
                            �ӿں�������
*********************************************************************/

/*******************************************************************************
  * ��    ����openDevice
  * ��    ��������IP��ַ�����е��豸IP��ַ�������豸���ӵĴ�
  * ���������
  *             ��������                ��������            ����˵��
  *             ps32IpAddr              int*                �豸IP��ַ����
  *             ps32ClientPort          int*                �豸Socketͨ�Ŷ˿ں�
  *             s32DeviceNum            int                 ׼���򿪵��豸��
  * ���������
  *             ��������                ��������            ����˵��
  *             ps32Result              int*                �豸�����ӵĴ�����
  *                                                             1 -- �豸�ѽ�������
  *                                                             0 -- �޴���
  *                                                            -2 -- �������Ӵ���(TCP) 
  *                                                            -5 -- ��ַ�󶨴���(UDP)
  *                                                            -8 -- �豸δ����
  * �� �� ֵ�� 0 -- �ɹ�
  *           -1 -- ʧ��
  * ˵    ����ֻ����IP��ַ��Ӧ�豸δ��������ʱ���д�
 ******************************************************************************/
int openDevice(int *ps32IpAddr,int *ps32ClientPort , int s32DeviceNum,int *ps32Result)
{
    struct stDeviceConfigInfo *pstNode = NULL;
    int                        i;
    int                        s32SemPoolUseCnt = 0;

    // 1. ������Ҫ�򿪵��豸
    for ( i = 0; i < s32DeviceNum; i++)
    {
        // 1.1 ��ѯ�豸�ڵ�
        pstNode = seekDeviceList(ps32IpAddr[i],ps32ClientPort[i]);

        // 1.2 �ж��豸�ڵ��Ƿ���Ч
        if (pstNode != NULL)
        {
            // ֻ���豸������������״̬�Ž��д򿪲���
            if ( pstNode->valid == false )
            {
                // 1.2.1 �����豸���¼�
                pstNode->opt = DEVICE_CONFIG_SERVICE_EVENT_OPEN;
                // 1.2.2 �����ź�
                pstNode->finish = &m_semDevConfigFinishPool[s32SemPoolUseCnt++];
                // 1.2.3 �����¼������߳�
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

    // 2. �ȴ��������
    if( s32SemPoolUseCnt > 0 )
    {
        SyncWaitForConfigThreadFinish(m_semDevConfigFinishPool,s32SemPoolUseCnt);
    }

    return updateDeviceListErr(ps32IpAddr,ps32ClientPort,s32DeviceNum,ps32Result);
}

/*******************************************************************************
  * ��    ����regDevConfigCilent
  * ��    �������豸���÷����ϼܿͻ�����Ϣ
  *           �ú���������ע����豸��Ϣ�������ӵĽ���
  * ���������
  *           ��������              ��������        ����˵��
  *           ps32IpAddr            int*            �豸IP��ַ������׵�ַ
  *           s32Protocol           int*            �豸ͨ��ʹ�õ�Э��:0--UDP
  *                                                                    1--TCP
  *           ps32ClientPort        int*            �豸�˿����Ӷ˿ڵ�����
  *           ps32ServerPort        int*            �����Ӧ��ʹ�ö˿ڵ�����
  *                                                 ��UDPЭ����Ҫ
  *           s32DeviceNum          int             �ϼ��豸������
  * ���������
  *           ��������              ��������        ����˵��
  *           ps32Result            int*            �豸�ϼܽ��: 
  *                                                     1 -- �豸�ѽ�������
  *                                                     0 -- �ɹ�
  *                                                    -1 -- WSA��ʼ��ʧ��
  *                                                    -2 -- �������Ӵ���(TCP) 
  *                                                    -5 -- ��ַ�󶨴���(UDP)
  *                                                    -8 -- �豸δ����
  *                                                   -11 -- �ڴ����
  * �� �� ֵ�� 0 -- �ϼܳɹ�
  *           -1 -- �ϼ�ʧ��,����ps32Result�������������ж�
  *           -2 -- ��������
  *           -3 -- �ڴ����
  * ˵    ����1.IP��ַ������ѭ�������ݵĴ洢����(С�˴洢)
  *           2.�ú��������豸�б�Ϊ��ʱʹ��,����ʱ����ʹ��addDevConfigClient����
  *             �豸����ӣ��Լ�ʹ��editDevConfigClient���б��
  *           3.��Result���Ϊ-2����-5����ͨ��openDevice�����ٴν�������
  *             ����ͨ��editDevConfigClient���б�����Զ���������
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
   
    // 1. ���豸����Ϊ0,��ֱ�ӷ���,�ܾ�ע��
    if( s32DeviceNum == 0 )
    {
        return -2;
    }

    // 2. ������߳�ͬ���źų�
    // 2.1 �����źų��ڴ�
    m_semDevConfigFinishPool = (HANDLE*)malloc(s32DeviceNum*sizeof(HANDLE));
    // 2.2 ���ڴ�����ʧ��,��ֱ�ӷ���
    if( m_semDevConfigFinishPool == NULL)
    {
        return -3;
    }
    // 2.3 ��ʼ���źų�
    for (i = 0; i < s32DeviceNum; i++)
    {
        m_semDevConfigFinishPool[i] = CreateSemaphore(NULL,0,1,NULL); // ���������ź���
    }
    // 2.4 ��¼�źųص�����
    m_s32DevConfigFinishPoolNum = s32DeviceNum;

    // 3. �����豸���ĳ�ʼ��
    for (i = 0; i < s32DeviceNum; i++)
    {
        // 3.1 �����豸�ڵ�
        pstNode = (struct stDeviceConfigInfo*)malloc(sizeof(struct stDeviceConfigInfo));

        // 3.2 �ж��ڵ��Ƿ�����ɹ�,�����ɹ����˻ص�ѭ��
        if (pstNode == NULL )
        {
            ps32Result[i] = DEVICE_CONFIG_SERVICE_MALLOC_ERROR;
            continue;
        }
       
        // 3.3 ��ʼ���ڵ����
        pstNode->ip       = ps32IpAddr[i];
        pstNode->protocal = s32Protocol[i];
        pstNode->cport    = ps32ClientPort[i];
        pstNode->sport    = ps32ServerPort[i];
        pstNode->valid    = false;
        pstNode->event    = CreateSemaphore(NULL, 0, 1, NULL);//���������ź�������ʼ��ԴΪ�㣬��󲢷���Ϊ1
        pstNode->err      = DEVICE_CONFIG_SERVICE_NO_ERROR;
        pstNode->use      = false;   //  Added 2020-07-04
        
        // 3.4 �����ź���,���ڲ���ͬ��
        pstNode->finish   = &m_semDevConfigFinishPool[s32SemPoolUseCnt++];

        // 3.5 �������߳̽����¼��Ĵ���
        _beginthread(devConfigProcess,1024,pstNode);

        // 3.6 �����豸��Ϣ����
        insertDeviceNode(pstNode);
    }

    // 4. �ȴ������̵߳ĳ�ʼ�����
    SyncWaitForConfigThreadFinish(m_semDevConfigFinishPool,s32SemPoolUseCnt);
    
    // 5. ���豸
    s32RetCode = openDevice(ps32IpAddr,ps32ClientPort,s32DeviceNum,ps32Result);

    return s32RetCode;
}

/*******************************************************************************
  * ��    ����addDevConfigClient
  * ��    ������ǰ���豸�б�������µ��豸�ڵ�
  * ���������
  *             ��������                ��������            ����˵��
  *             s32IpAddr               int                 �豸IP��ַ
  *             s32Protocol             int                 �豸Socketͨ��Э��
  *                                                             0 -- UDP
  *                                                             1 -- TCP
  *             s32ClientPort           int                 �豸�����Ӷ˿ں�
  *             s32ServerPort           int                 �����Ӧ��ʹ�õĶ˿ں�
  *                                                         ��������UDPЭ��
  * �����������
  * �� �� ֵ�� 0 -- �ɹ�
  *           -1 -- �豸�Ѵ���,���豸δ����,��ʹ��openDevice�ӿں���
  *           -2 -- �ڴ����
  *           -3 -- ͨ��I/O����,�����豸����
  *           -4 -- ��ѯ��ʱ,��ȷ���Ƿ�Ϊ֧�ֵ��豸�ͺ��б�
  * ˵    ������
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

    //  1. ���ҵ�ǰ�豸�Ƿ��ѱ�ע��
    pstNode = seekDeviceList(s32IpAddr,s32ClientPort);
    //  2. ���ҵ���ע��,�򷵻����ʧ��
    if( pstNode != NULL )
    {
        return -1;
    }

    //  3. �����µĽڵ�
    pstNode = (struct stDeviceConfigInfo*)malloc(sizeof(struct stDeviceConfigInfo));
    //  4. �ڵ���Ч�ж�
    if( pstNode == NULL )
    {
        return -2;
    }
    // 5 �����߳�ͬ���ź���
    semSync = CreateSemaphore(NULL, 0, 1, NULL);

    // 6 ��ʼ���ڵ���Ϣ
    pstNode->ip       = s32IpAddr;
    pstNode->protocal = s32Protocol;
    pstNode->cport    = s32ClientPort;
    pstNode->sport    = s32ServerPort;
    pstNode->valid    = false;
    pstNode->event    = CreateSemaphore(NULL, 0, 1, NULL);//���������ź�������ʼ��ԴΪ�㣬��󲢷���Ϊ1
    pstNode->err      = DEVICE_CONFIG_SERVICE_NO_ERROR;

    // 7 ����������ͬ���ź�
    pstNode->finish   = &semSync;

    // 8 �������߳̽����¼��Ĵ���
    _beginthread(devConfigProcess,1024,pstNode);

    // 9 �ȴ��̳߳�ʼ�����
    WaitForSingleObject(semSync,INFINITE);

    // 10 ���豸����
    pstNode->opt      = DEVICE_CONFIG_SERVICE_EVENT_OPEN; // ���ò���Ϊ���¼�
    ReleaseSemaphore(pstNode->event,1,NULL); // �����¼������߳�
    WaitForSingleObject(semSync,INFINITE);// �ȴ��������

    // 11 �Դ���������ж�,�����ִ���,��ر����߳�,���ͷŽڵ��ڴ�,�����в������
    //    �������ڴ���,����뵽�豸�ͻ�����Ϣ�б�,�������ź����ص�����
    if ( pstNode->err == DEVICE_CONFIG_SERVICE_NO_ERROR )
    {
        // 11.1 �������źų��ڴ�ռ�
        pstSemPool = (HANDLE*)malloc(sizeof(HANDLE)*(m_s32DevConfigFinishPoolNum+1));
        // 11.2 �ж��ڴ������Ƿ����
        if( pstSemPool == NULL )
        {
            s32RetCode = -2;
        }
        else
        {
            // 11.3 ����豸�ڵ�
            insertDeviceNode(pstNode);
            // 11.4 �������е��ź�
            for (i = 0; i < m_s32DevConfigFinishPoolNum; i++) 
            {
                pstSemPool[i] = m_semDevConfigFinishPool[i];
            }
            // 11.5 �����µ��ź���
            pstSemPool[i] = CreateSemaphore(NULL, 0, 1, NULL);
            // 11.6 ��������źų�����
            m_s32DevConfigFinishPoolNum ++;
            // 11.7 �ͷžɵ��źų�
            free(m_semDevConfigFinishPool);
            // 11.8 �����µ��źų�
            m_semDevConfigFinishPool = pstSemPool;
        }
    }
    else
    {
        pstNode->opt      = DEVICE_CONFIG_SERVICE_EVENT_EXIT; // ���ò���Ϊ���¼�
        ReleaseSemaphore(pstNode->event,1,NULL); // �����¼������߳�
        WaitForSingleObject(semSync,INFINITE);// �ȴ��������
        CloseHandle(pstNode->event);// �ر��¼��ź���
        free(pstNode);// �ͷ��豸�ڵ�
    }
    // 12 �ر��߳�ͬ���ź���
    CloseHandle(semSync);
    return s32RetCode;
}
/*******************************************************************************
  * ��    ����checkDevConfigClient
  * ��    ������ȡ��ǰ�豸��Ϣ,�����豸��ע��ʹ��
  * ���������
  *             ��������                ��������            ����˵��
  *             s32IpAddr               int                 �豸IP��ַ
  *             s32Protocol             int                 �豸Socketͨ��Э��
  *                                                             0 -- UDP
  *                                                             1 -- TCP
  *             s32ClientPort           int                 �豸�����Ӷ˿ں�
  *             s32ServerPort           int                 �����Ӧ��ʹ�õĶ˿ں�
  *                                                         ��������UDPЭ��
  * ���������
  *             ��������                ��������            ����˵��
  *             pu8IDNInfo              unsigned char*      �豸��*IDN?��Ϣ
  * �� �� ֵ��>0 -- �ɹ�,�豸*IDN?����ֵ�ĳ���
  *           -1 -- �豸�Ѵ���,���豸δ����,��ʹ��openDevice�ӿں���
  *           -2 -- �ڴ����
  *           -3 -- ͨ��I/O����,�����豸����
  *           -4 -- ��ѯ��ʱ,��ȷ���Ƿ�Ϊ֧�ֵ��豸�ͺ��б�
  * ˵    ������
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

    // 1 �����߳�ͬ���ź���
    semSync = CreateSemaphore(NULL, 0, 1, NULL);

    // 2 ��ʼ���ڵ���Ϣ
    stDevClient.ip       = s32IpAddr;
    stDevClient.protocal = s32Protocol;
    stDevClient.cport    = s32ClientPort;
    stDevClient.sport    = s32ServerPort;
    stDevClient.valid    = false;
    stDevClient.event    = CreateSemaphore(NULL, 0, 1, NULL);//���������ź�������ʼ��ԴΪ�㣬��󲢷���Ϊ1
    stDevClient.err      = DEVICE_CONFIG_SERVICE_NO_ERROR;

    // 3 ����������ͬ���ź�
    stDevClient.finish   = &semSync;

    // 4 �������߳̽����¼��Ĵ���
    _beginthread(devConfigProcess,1024,&stDevClient);

    // 5 �ȴ��̳߳�ʼ�����
    WaitForSingleObject(semSync,INFINITE);

    // 6 ���豸����
    stDevClient.opt      = DEVICE_CONFIG_SERVICE_EVENT_OPEN; // ���ò���Ϊ���¼�
    ReleaseSemaphore(stDevClient.event,1,NULL); // �����¼������߳�
    WaitForSingleObject(semSync,INFINITE);// �ȴ��������

    // 7 ���豸�����ӳɹ�,����в�ѯ�¼��ķ���
    if (stDevClient.err == DEVICE_CONFIG_SERVICE_NO_ERROR )
    {
        // 7.1 ���ò���Ϊ��ѯ�¼� 
        stDevClient.opt  = DEVICE_CONFIG_SERVICE_EVENT_QUERY;
        // 7.2 ����д������
        memset(stDevClient.send_buf,0,DEVICE_CONFIG_SERVICE_SEND_BUFF_LEN);
        stDevClient.send_len = sprintf(stDevClient.send_buf,"%s\n","*IDN?;:LAN:MAC?");
        // 7.3 ���ý��ջ�����
        memset(stDevClient.recv_buf,0,DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN);
        stDevClient.recv_len = DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN;
        // 7.4 ���ó�ʱʱ��Ϊ2000ms
        stDevClient.timeout  = 2000;

        // 7.5 �����¼������߳�
        ReleaseSemaphore(stDevClient.event,1,NULL);
        // 7.6 �ȴ��������
        WaitForSingleObject(semSync,INFINITE);

        // 7.7 �Բ�ѯ������ж�,��û�д���,�������Ӧ���ݵ�Copy,�����ô�����,������������̴���
         if (stDevClient.err == DEVICE_CONFIG_SERVICE_NO_ERROR )
        {
            memcpy(pu8IDNInfo,stDevClient.recv_buf,stDevClient.recv_len);
            s32RetCode = stDevClient.recv_len;
        }
        else
        {
            s32RetCode = -4; // �豸��ѯ��ʱ
        }
    }
    else
    {
        s32RetCode = -3; // ���豸ͨ��I/O��ʧ��
    }
    // 8 ����ǰ������Ч����,����йرղ���
    if( stDevClient.valid == true )
    {
        stDevClient.opt      = DEVICE_CONFIG_SERVICE_EVENT_OPEN; // ���ò���Ϊ���¼�
        ReleaseSemaphore(stDevClient.event,1,NULL); // �����¼������߳�
        WaitForSingleObject(semSync,INFINITE);// �ȴ��������
    }
    
    // 9 �߳��˳�����
    stDevClient.opt      = DEVICE_CONFIG_SERVICE_EVENT_EXIT; // ���ò���Ϊ���¼�
    ReleaseSemaphore(stDevClient.event,1,NULL); // �����¼������߳�
    WaitForSingleObject(semSync,INFINITE);// �ȴ��������
    CloseHandle(stDevClient.event);// �ر��¼��ź���

    // 10 �ر��߳�ͬ���ź���
    CloseHandle(semSync);
    return s32RetCode;
}

/*******************************************************************************
  * ��    ����delDevConfigClient
  * ��    ���������豸IP��ַ�Ͷ˿ں�,�����豸�ڵ��ɾ������
  * ���������
  *             ��������                ��������            ����˵��
  *             s32IpAddr               int                 �豸IP��ַ
  *             s32ClientPort           int                 �豸Socket�˿ں�
  * �����������
  * �� �� ֵ����
  * ˵    ��������ǰ�豸��������,�ȹرպ��ٽ������̵߳��˳�����
 ******************************************************************************/
void delDevConfigClient(int s32IpAddr , int s32ClientPort)
{
    struct stDeviceConfigInfo *     pstNode     = NULL;
    HANDLE                    *     pstSemPool  = NULL;
    int                             i;
    HANDLE                          semSync;

    // 1 ��ѯ�ýڵ��Ƿ����
    pstNode = seekDeviceList(s32IpAddr,s32ClientPort);

    // 2 ���ýڵ���������ɾ������,���򲻽����κβ���
    if (pstNode != NULL)
    {
        // 2.1 �����߳�ͬ���ź�
        semSync = CreateSemaphore(NULL,0,1,NULL);
        // 2.2 ���ź���
        pstNode->finish = &semSync; 

        // 2.3 ����ǰ������Ч���ȹر�����
        if( pstNode->valid == true )
        {
            pstNode->opt      = DEVICE_CONFIG_SERVICE_EVENT_CLOSE; // ���ò���Ϊ���¼�
            ReleaseSemaphore(pstNode->event,1,NULL); // �����¼������߳�
            WaitForSingleObject(semSync,INFINITE);// �ȴ�������� 
        }

        // 2.4 �ٽ����˳��̲߳���
        pstNode->opt      = DEVICE_CONFIG_SERVICE_EVENT_EXIT; // ���ò���Ϊ�˳��¼�
        ReleaseSemaphore(pstNode->event,1,NULL); // �����¼������߳�
        WaitForSingleObject(semSync,INFINITE);// �ȴ��������
        // 2.3 �ͷŽڵ�
        deleteDeviceNode(s32IpAddr,s32ClientPort);
        // 2.4 �������źų��ڴ�ռ�
        pstSemPool = (HANDLE*)malloc(sizeof(HANDLE)*(m_s32DevConfigFinishPoolNum-1));
        // 2.5 �������е��ź�
        for (i = 0; i < (m_s32DevConfigFinishPoolNum-1); i++) 
        {
            pstSemPool[i] = m_semDevConfigFinishPool[i];
        }
        // 2.6 �ͷ����е��ź���
        CloseHandle(m_semDevConfigFinishPool[i]);
        // 2.7 ��������źų�����
        m_s32DevConfigFinishPoolNum --;
        // 2.8 �ͷžɵ��źų�
        free(m_semDevConfigFinishPool);
        // 2.9 �����µ��źų�
        m_semDevConfigFinishPool = pstSemPool;
    }
}

/*******************************************************************************
  * ��    ����editDevConfigClient
  * ��    ���������豸ԭIP��ַ�Ͷ˿ں�,�����豸�ڵ���µ��µ�IP��ַ�Ͷ˿ں�
  * ���������
  *             ��������                ��������            ����˵��
  *             s32SrcIpAddr            int                 �豸ԭIP��ַ
  *             s32SrcClientPort        int                 �豸ԭ�˿ں�
  *             s32DstIpAddr            int                 �豸��IP��ַ
  *             s32DstClientPort        int                 �豸�¶˿ں�
  *             s32DstServerPort        int                 ������µĶ˿ں�
  *                                                         ������UDPЭ��
  *             s32DstPotocal           int                 ͨ��Э�飺0 -- UDP
  *                                                                   1 -- TCP
  * �����������
  * �� �� ֵ�� 0 -- �ɹ�
  *           -1 -- δ��ѯ����Ҫ������豸�ڵ�
  *           -2 -- ���豸�������Ӵ���
  * ˵    ��������ǰ�豸��������,�ȹرպ��ٽ����豸��Ϣ�ı��
 ******************************************************************************/
int editDevConfigClient(int s32SrcIpAddr, int s32SrcClientPort , 
                        int s32DstIpAddr, int s32DstClientPort , 
                        int s32DstServerPort, int s32DstPotocal )
{
    struct stDeviceConfigInfo *         pstNode = NULL;
    int                                 s32RetCode = 0;

    // 1 ����ԴIP��ַ�Ͷ˿ںţ���ѯ�ڵ�
    pstNode = seekDeviceList(s32SrcIpAddr,s32SrcClientPort);

    // 2 �ж��Ƿ��ѯ���ýڵ�,��û����ֱ�ӷ���
    if (pstNode != NULL)
    {
        /*
         * ��ѯ���豸�ڵ�,�����豸����Ϣ���
         * ����1. ��⵱ǰ�Ƿ�����Ч���Ӵ���,���������ȹر�����
         * ����2. �����豸�ڵ���Ϣ
         * ����3. ���´�����
         */
        // 2.1 �жϵ�ǰ�Ƿ�����Ч����
        if (pstNode->valid == true)
        {
            pstNode->opt      = DEVICE_CONFIG_SERVICE_EVENT_CLOSE; // ���ò���Ϊ���¼�
            pstNode->finish   = &m_semDevConfigFinishPool[0]; // �����ź���
            ReleaseSemaphore(pstNode->event,1,NULL); // �����¼������߳�
            WaitForSingleObject(m_semDevConfigFinishPool[0],INFINITE);// �ȴ�������� 
        }
        
        // 2.2 �����豸��Ϣ
        pstNode->ip       = s32DstIpAddr;
        pstNode->cport    = s32SrcClientPort;
        pstNode->sport    = s32DstServerPort;
        pstNode->protocal = s32DstPotocal;

        // 2.3 ���豸����
        pstNode->opt      = DEVICE_CONFIG_SERVICE_EVENT_OPEN; // ���ò���Ϊ���¼�
        pstNode->finish   = &m_semDevConfigFinishPool[0]; // �����ź���
        ReleaseSemaphore(pstNode->event,1,NULL); // �����¼������߳�
        WaitForSingleObject(m_semDevConfigFinishPool[0],INFINITE);// �ȴ�������� 

        // 2.4 ���н�����ж�
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
  * ��    ����unregDevConfigCilent
  * ��    �������豸���÷����¼ܿͻ�����Ϣ(�ⲿ�ӿ�)
  * �����������
  * �����������
  * �� �� ֵ����
  * ˵    ����1.ע���߳�ͬ���źųص��ź����ر�,�Լ��źųص��ͷ�
  *           2.ע��ÿ���ͻ�������������¼����յ��ź�����ǰ�ͷź�,���ͷſͻ��˱�
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
    // 1. �����ͻ�����Ϣ��
    while (pstNode != NULL)
    {
        // ���¼���ʶ
        pstNode->opt    = DEVICE_CONFIG_SERVICE_EVENT_EXIT;
        pstNode->finish = &m_semDevConfigFinishPool[s32SyncSemUseCnt++];
              
        // �׳��¼�����
        ReleaseSemaphore(pstNode->event,1,NULL);

        pstNode = pstNode->next;
    }

    // 2. �ȴ��������߳��˳�
    if( 0 < s32SyncSemUseCnt )
    {
        SyncWaitForConfigThreadFinish(m_semDevConfigFinishPool,s32SyncSemUseCnt);
    }
    // 3. ���������Դ�ͷ�
    // 3.1 �����źųصĴ�С�����г��ӵ��ͷ�
    for ( i = 0; i < m_s32DevConfigFinishPoolNum; i++)
    {
        CloseHandle(m_semDevConfigFinishPool[i]);
    }
    // 3.2 �ͷ��źų�ռ�õ��ڴ�
    free(m_semDevConfigFinishPool);
    m_semDevConfigFinishPool = NULL;

    // 3.3 �ͷ������豸�ڵ�
    deleteAllDeviceNode();
}

/*******************************************************************************
  * ��    ����write2Device
  * ��    �������豸д������
  * ���������
  *           ��������              ��������        ����˵��
  *           ps32IpAddr            int*            ��Ҫд�������豸IP��ַ
  *           ps32Port              int*            ��Ҫд�������豸�˿ں�
  *           s32IpAddrSize         int             �豸IP��ַ�����Ԫ������
  *           pu8WriteBuff          unsignedchar*   ��Ҫд���ݵĵ�ַ
  *           ps32WriteLen          int*            ÿ��IP��Ҫд���ݵĳ���
  * ���������
  *           ��������              ��������        ����˵��
  *           ps32Result            int*            ÿ���ͻ��˵�д���
  *                                                  -7 -- ͨ��IO����
  *                                                  -8 -- �豸û��ע��
  *                                                 -12 -- ͨ��δ����
  *                                                   0 -- �ɹ�
  * �� �� ֵ�� 0 -- �ɹ�
  *           -1 -- ��������,�������� ps32Result
  *           -9 -- ��������
  * ˵    ����
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

    // 1. �����Ϸ������
    if( NULL == pu8WriteBuff || NULL == ps32WriteLen )
    {
        return DEVICE_CONFIG_SERVICE_PARAMETER_ERROR;
    }

    // 2. �����ڵ�
    for (i = 0; i < s32IpAddrSize; i++)
    {
        // 2.1 ����IP��ַ�Ͷ˿ںŽ��нڵ�Ļ�ȡ
        pstNode = seekDeviceList(ps32IpAddr[i],ps32Port[i]);

        // 2.2 �ж��ڵ��Ƿ���ҵ�
        if (pstNode!=NULL)
        {
            
            // 2.2.1 ���ô�����
            pstNode->err = DEVICE_CONFIG_SERVICE_NO_ERROR;
            // 2.2.2 ���ݽڵ���Ч�Խ����¼�����
            if (pstNode->valid == true)
            {
                if( ps32WriteLen[i] != 0)
                {
                    // 2.2.2.1 ����д���ݵĸ���
                    memset(pstNode->send_buf,0,DEVICE_CONFIG_SERVICE_SEND_BUFF_LEN); // ��ʼ���ռ�
                    memcpy(pstNode->send_buf,pu8WriteBuff+s32WriteBuffOffset,ps32WriteLen[i]);
                    pstNode->send_len  = ps32WriteLen[i];

                    // 2.2.2.2 �����߳�ͬ���ź�
                    pstNode->finish = &m_semDevConfigFinishPool[s32SyncSemCnt++];

                    // 2.2.2.3 �����¼�
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

    // 3. �ȴ������¼��̴߳������
    if( 0 < s32SyncSemCnt )
    {
        SyncWaitForConfigThreadFinish(m_semDevConfigFinishPool,s32SyncSemCnt);
    }

    // 4. ��ȡд���
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
  * ��    ����write2DeviceAll
  * ��    �������豸д�����ݣ�д���������豸һ��
  * ���������
  *           ��������              ��������        ����˵��
  *           ps32IpAddr            int*            ��Ҫд�������豸IP��ַ
  *           ps32Port              int*            ��Ҫд�������豸�˿ں�
  *           s32IpAddrSize         int             �豸IP��ַ�����Ԫ������
  *           pu8WriteBuff          unsignedchar*   ��Ҫд���ݵĵ�ַ
  *           s32WriteLen           int             ��Ҫд���ݵĳ���
  * ���������
  *           ��������              ��������        ����˵��
  *           ps32Result            int*            ÿ���ͻ��˵�д���
  *                                                  -7 -- ͨ��IO����
  *                                                  -8 -- �豸û��ע��
  *                                                 -12 -- ͨ��δ����
  *                                                   0 -- �ɹ�
  * �� �� ֵ�� 0 -- �ɹ�
  *           -1 -- ��������,�������� ps32Result
  *           -9 -- ��������
  * ˵    ����
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

    // 1. �����Ϸ������
    if( NULL == pu8WriteBuff || 0 == s32WriteLen )
    {
        return DEVICE_CONFIG_SERVICE_PARAMETER_ERROR;
    }

    // 2. �����ڵ�
    for (i = 0; i < s32IpAddrSize; i++)
    {
        // 2.1 ����IP��ַ�Ͷ˿ںŽ��нڵ�Ļ�ȡ
        pstNode = seekDeviceList(ps32IpAddr[i],ps32Port[i]);

        // 2.2 �ж��ڵ��Ƿ���ҵ�
        if (pstNode!=NULL)
        {
            
            // 2.2.1 ���ô�����
            pstNode->err = DEVICE_CONFIG_SERVICE_NO_ERROR;
            // 2.2.2 ���ݽڵ���Ч�Խ����¼�����
            if (pstNode->valid == true)
            {
                // 2.2.2.1 ����д���ݵĸ���
                memset(pstNode->send_buf,0,DEVICE_CONFIG_SERVICE_SEND_BUFF_LEN); // ��ʼ���ռ�
                memcpy(pstNode->send_buf,pu8WriteBuff,s32WriteLen);
                pstNode->send_len  = s32WriteLen;

                // 2.2.2.2 �����߳�ͬ���ź�
                pstNode->finish = &m_semDevConfigFinishPool[s32SyncSemCnt++];

                // 2.2.2.3 �����¼�
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

    // 3. �ȴ������¼��̴߳������
    if( 0 < s32SyncSemCnt )
    {
        SyncWaitForConfigThreadFinish(m_semDevConfigFinishPool,s32SyncSemCnt);
    }

    // 4. ��ȡд���
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
  * ��    ����read4Device
  * ��    �������豸�������ݵĶ�ȡ����
  * ���������
  *           ��������              ��������        ����˵��
  *           ps32IpAddr            int*            ��Ҫд�������豸IP��ַ
  *           ps32Port              int*            ��Ҫд�������豸�˿ں�
  *           s32IpAddrSize         int             �豸IP��ַ�����Ԫ������
  *           s32Timeout            int             ��ʱʱ��
  * ���������
  *           ��������              ��������        ����˵��
  *           ps32Result            int*            ÿ���ͻ��˵�д���
  *                                                  -3 -- ��������ʱ
  *                                                  -7 -- ͨ��IO����
  *                                                  -8 -- �豸û��ע��
  *                                                 -12 -- ͨ��δ����
  *                                                   0 -- �ɹ�
  * �� �� ֵ�� 0 -- �ɹ�
  *           -1 -- ��������,�������� ps32Result
  *           -9 -- ��������
  * ˵    �����ú��������getReadData4IP�ӿڽ���ʹ��
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


    // 1. �����ڵ�
    for (i = 0; i < s32IpAddrSize; i++)
    {
        // 1.1 ����IP��ַ�Ͷ˿ںŽ��нڵ�Ļ�ȡ
        pstNode = seekDeviceList(ps32IpAddr[i],ps32Port[i]);

        // 1.2 �ж��ڵ��Ƿ���ҵ�
        if (pstNode!=NULL)
        {
            
            // 1.2.1 ���ô�����
            pstNode->err = DEVICE_CONFIG_SERVICE_NO_ERROR;
            // 1.2.2 ���ݽڵ���Ч�Խ����¼�����
            if (pstNode->valid == true)
            {
                // 1.2.2.1 ���ý��ջ�����
                memset(pstNode->recv_buf,0,DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN);
                pstNode->recv_len  = DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN;

                // 1.2.2.2 �����߳�ͬ���ź�
                pstNode->finish = &m_semDevConfigFinishPool[s32SyncSemCnt++];

                // 1.2.2.3 �����¼�
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

    // 2. �ȴ������¼��̴߳������
    if( 0 < s32SyncSemCnt )
    {
        SyncWaitForConfigThreadFinish(m_semDevConfigFinishPool,s32SyncSemCnt);
    }

    // 3. ��ȡ�����
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
  * ��    ����query4Device
  * ��    �������豸�������ݵĶ�д��������̨�豸һ������
  * ���������
  *           ��������              ��������        ����˵��
  *           ps32IpAddr            int*            ��Ҫд�������豸IP��ַ
  *           ps32Port              int*            ��Ҫд�������豸�˿ں�
  *           s32IpAddrSize         int             �豸IP��ַ�����Ԫ������
  *           pu8WriteBuff          unsignedchar*   ��Ҫд���ݵĵ�ַ
  *           ps32WriteLen          int*            ÿ��IP��Ҫд���ݵĳ���
  *           s32Timeout            int             ��ʱʱ��
  * ���������
  *           ��������              ��������        ����˵��
  *           ps32Result            int*            ÿ���ͻ��˵�д���
  *                                                  -3 -- ��������ʱ
  *                                                  -7 -- ͨ��IO����
  *                                                  -8 -- �豸û��ע��
  *                                                 -12 -- ͨ��δ����
  *                                                   0 -- �ɹ�
  * �� �� ֵ�� 0 -- �ɹ�
  *           -1 -- ��������,�������� ps32Result
  *           -9 -- ��������
  * ˵    �����ú��������getReadData4IP�ӿڽ���ʹ��
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

    // 1. �����Ϸ������
    if( NULL == pu8WriteBuff || NULL == ps32WriteLen )
    {
        return DEVICE_CONFIG_SERVICE_PARAMETER_ERROR;
    }

    // 2. �����ڵ�
    for (i = 0; i < s32IpAddrSize; i++)
    {
        // 2.1 ����IP��ַ�Ͷ˿ںŽ��нڵ�Ļ�ȡ
        pstNode = seekDeviceList(ps32IpAddr[i],ps32Port[i]);

        // 2.2 �ж��ڵ��Ƿ���ҵ�
        if (pstNode!=NULL)
        {
            // 2.2.1 ���ô�����
            pstNode->err = DEVICE_CONFIG_SERVICE_NO_ERROR;
            // 2.2.2 ���ݽڵ���Ч�Խ����¼�����
            if (pstNode->valid == true)
            {
                if( ps32WriteLen[i] != 0)
                {
                    // 2.2.1 ����д���ݵĸ���
                    memset(pstNode->send_buf,0,DEVICE_CONFIG_SERVICE_SEND_BUFF_LEN); // ��ʼ���ռ�
                    memcpy(pstNode->send_buf,pu8WriteBuff+s32WriteBuffOffset,ps32WriteLen[i]);
                    pstNode->send_len  = ps32WriteLen[i];

                    // 2.2.2 ���ý��ջ�����
                    memset(pstNode->recv_buf,0,DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN);
                    pstNode->recv_len  = DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN;

                    // 2.2.3 �����߳�ͬ���ź�
                    pstNode->finish = &m_semDevConfigFinishPool[s32SyncSemCnt++];

                    // 2.2.4 �����¼�
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

    // 3. �ȴ������¼��̴߳������
    if( 0 < s32SyncSemCnt )
    {
        SyncWaitForConfigThreadFinish(m_semDevConfigFinishPool,s32SyncSemCnt);
    }
    // 4. ��ȡ��ѯ���
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
  * ��    ����query4DeviceAll
  * ��    �������豸�������ݵĶ�д�����������豸����ͬ������
  * ���������
  *           ��������              ��������        ����˵��
  *           ps32IpAddr            int*            ��Ҫд�������豸IP��ַ
  *           ps32Port              int*            ��Ҫд�������豸�˿ں�
  *           s32IpAddrSize         int             �豸IP��ַ�����Ԫ������
  *           pu8WriteBuff          unsignedchar*   ��Ҫд���ݵĵ�ַ
  *           s32WriteLen           int             ��Ҫд���ݵĳ���
  *           s32Timeout            int             ��ʱʱ��
  * ���������
  *           ��������              ��������        ����˵��
  *           ps32Result            int*            ÿ���ͻ��˵�д���
  *                                                  -3 -- ��������ʱ
  *                                                  -7 -- ͨ��IO����
  *                                                  -8 -- �豸û��ע��
  *                                                 -12 -- ͨ��δ����
  *                                                   0 -- �ɹ�
  * �� �� ֵ�� 0 -- �ɹ�
  *           -1 -- ��������,�������� ps32Result
  *           -9 -- ��������
  * ˵    �����ú��������getReadData4IP�ӿڽ���ʹ��
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

    // 1. �����Ϸ������
    if( NULL == pu8WriteBuff || 0 == s32WriteLen )
    {
        return DEVICE_CONFIG_SERVICE_PARAMETER_ERROR;
    }

    // 2. �����ڵ�
    for (i = 0; i < s32IpAddrSize; i++)
    {
        // 2.1 ����IP��ַ�Ͷ˿ںŽ��нڵ�Ļ�ȡ
        pstNode = seekDeviceList(ps32IpAddr[i],ps32Port[i]);

        // 2.2 �ж��ڵ��Ƿ���ҵ�
        if (pstNode!=NULL)
        {
            
            // 2.2.1 ���ô�����
            pstNode->err = DEVICE_CONFIG_SERVICE_NO_ERROR;
            // 2.2.2 ���ݽڵ���Ч�Խ����¼�����
            if (pstNode->valid == true)
            {
                // 2.2.1 ����д���ݵĸ���
                memset(pstNode->send_buf,0,DEVICE_CONFIG_SERVICE_SEND_BUFF_LEN); // ��ʼ���ռ�
                memcpy(pstNode->send_buf,pu8WriteBuff,s32WriteLen);
                pstNode->send_len  = s32WriteLen;

                // 2.2.2 ���ý��ջ�����
                memset(pstNode->recv_buf,0,DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN);
                pstNode->recv_len  = DEVICE_CONFIG_SERVICE_RECV_BUFF_LEN;

                // 2.2.3 �����߳�ͬ���ź�
                pstNode->finish = &m_semDevConfigFinishPool[s32SyncSemCnt++];

                // 2.2.4 �����¼�
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

    // 3. �ȴ������¼��̴߳������
    if( 0 < s32SyncSemCnt )
    {
        SyncWaitForConfigThreadFinish(m_semDevConfigFinishPool,s32SyncSemCnt);
    }
    // 4. ��ȡ��ѯ���
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
  * ��    ����getReadData4IP
  * ��    ��������IP��ַ���ж�Ӧ�Ķ������ݵ���ȡ
  * ���������
  *           ��������              ��������        ����˵��
  *           s32IpAddr             int             �豸IP��ַ
  *           s32PortNum            int             �豸ͨ�Ŷ˿�
  *           s32ReadOffset         int             ����ڽ��ջ������׵�ַ��ƫ��ֵ
  *           s32ReadBuffSize       int             ����������С
  * ���������
  *           ��������              ��������        ����˵��
  *           pu8ReadBuff           unsigned char*  ��������
  * �� �� ֵ��>0 -- ��ȡ����Ч���ݵĳ���
  *            0 -- �޿ɶ�����
  *           -1 -- ��������
  *           -2 -- δ��ѯ���豸
  * ˵    ������
 ******************************************************************************/
int getReadData4IP( int s32IpAddr ,
                    int s32PortNum,
                    unsigned char *pu8ReadBuff , 
                    int s32ReadOffset , 
                    int s32ReadBuffSize )
{
    int                             s32ValidDataLen = 0;
    struct stDeviceConfigInfo *     pstNode         = NULL;

    // 1. ������Ч�Լ��
    if( pu8ReadBuff == NULL || s32ReadBuffSize == 0 )
    {
        return -1;
    }

    // 2. �����ͻ����б�,����s32IpAddr��Ӧ�Ŀͻ��˽ڵ�
    pstNode = seekDeviceList(s32IpAddr,s32PortNum);

    // 3. �ж��Ƿ��ѯ����Ч�ڵ�
    if( NULL == pstNode )
    {
        // δ��ѯ����Ч�ڵ�,�򷵻�-2
        return -2;
    }

    // 4. ���ݽڵ����Ч�Ժͽ��ջ������ĳ���,����������Ч����֤
    if( pstNode->valid    == false ||
        pstNode->err      != DEVICE_CONFIG_SERVICE_NO_ERROR ||
        pstNode->recv_len == 0)
    {
        return 0;
    }

    // 5. ������Ч���ݵĸ���
    // 5.1 �ж����ݵ���Ч��
    s32ValidDataLen = (pstNode->recv_len-s32ReadOffset);
    s32ValidDataLen = s32ValidDataLen > s32ReadBuffSize ? s32ReadBuffSize : s32ValidDataLen;
    // 5.2 �������ݵĸ���
    memcpy(pu8ReadBuff,&pstNode->recv_buf[s32ReadOffset],s32ValidDataLen);

    return s32ValidDataLen;
}