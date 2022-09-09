/********************************************************************
                ��Դ����Ƽ��ɷ����޹�˾ ��Ȩ����(2020 - 2030)
*********************************************************************
ͷ�ļ���: device_config_service.h
��������: 1.��Tcp Socket�����ϼܿͻ�����Ϣ
          2.��Tcp Socket�����¼ܿͻ�����Ϣ
          3.��������ģʽ�µ����ݽ���
          4.��ȡTcp Socket������յ�������
��   ��: sn01625
��   ��: 0.1
��������: 2020-06-30  15:06 PM

�޸ļ�¼1��// �޸���ʷ��¼�������޸����ڡ��޸��߼��޸�����
�޸����ڣ�
�� �� �ţ�
�� �� �ˣ�
�޸����ݣ�
*********************************************************************/
#ifndef _RIGOL_DEVICE_RECV_SERVICE_H_
#define _RIGOL_DEVICE_RECV_SERVICE_H_

#ifndef _RIGOL_DLL_API
#define _RIGOL_DLL_API extern "C" _declspec(dllexport) 
#else
#endif

/*******************************************************************************
  * ��    ����DATA_RECV_OPTION_REPORT
  * ��    �����ṩ�����ݽ��շ�����йܺ���,�������ݽ��շ������ϲ��ϱ���Ϣ
  * ���������
  *           ��������                  ��������        ����˵��
  *           s32OptCode                int             ������ֵ
  *                                                         0 -- �ϱ�����״̬
  *                                                         1 -- ֪ͨ�ϲ�����������ݷ�������ķ���
  *                                                         2 -- ֪ͨ�ϲ�������������ش�����ķ���
  *           s32IpAddr                 int             ���β������豸IP��ַ
  *           s32Status                 int             ���ν��յĴ�����ֵ
  *                                                         >0 -- ���ݵĽ��ճ���
  *                                                         -4 -- ��������
  *                                                         -5 -- ���ճ�ʱ
  * �����������
  * �� �� ֵ����
  * ˵    ������
 ******************************************************************************/
//typedef void (*DATA_RECV_OPTION_REPORT)(int s32OptCode , int *ps32IpAddr , int *ps32Status );
typedef void (*DATA_RECV_OPTION_REPORT)(int s32OptCode , int s32IpAddr , int s32Status );
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
_RIGOL_DLL_API void createDataRecvService( DATA_RECV_OPTION_REPORT pReportIO );

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
  * �� �� ֵ����
  * ˵    ����ps32ClientPortֻ����Э��ΪUDPʱ,�ſ���ʹ��
 ******************************************************************************/
_RIGOL_DLL_API void addDevInfo2DataRecvService( int *ps32ClientIpAddr , 
                                                int *ps32ClientPort,
                                                int *ps32ClientProtocol,
                                                int *ps32ServicePort,
                                                int  s32ClientNum,
                                                int *ps32RegResult);

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
_RIGOL_DLL_API void deleteDevNode4DataRecvService( int *ps32ClientIpAddr , int  s32ClientNum );

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
_RIGOL_DLL_API void editDevNode2DataRecvService( int *ps32OldClientIpAddr ,
                                                 int *ps32NewClientIpAddr , 
                                                 int *ps32NewClientPort,
                                                 int *ps32NewClientProtocol,
                                                 int *ps32NewServicePort,
                                                 int  s32ClientNum,
                                                 int *ps32EditResult);

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
_RIGOL_DLL_API void recvDevData( int *ps32ClientIpAddr , 
                                 int *ps32RecvSize,
                                 int  s32ClientNum , 
                                 int  s32RecvTimeout);

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
_RIGOL_DLL_API int getRecvData4Ip( int s32IpAddr , unsigned char *pu8Buf1 , int s32BufLen1,
                                                   unsigned char *pu8Buf2 , int s32BufLen2,
                                                   unsigned char *pu8Buf3 , int s32BufLen3,
                                                   unsigned char *pu8Buf4 , int s32BufLen4,
                                                   unsigned char *pu8Buf5 , int s32BufLen5);

/*******************************************************************************
  * ��    ����restartDataRecv
  * ��    ������λ���ݽ���,���½������ݽ���
  * �����������
  * �����������
  * �� �� ֵ����
  * ˵    ������
 ******************************************************************************/
_RIGOL_DLL_API void restartDataRecv( void );

/*******************************************************************************
  * ��    ����stopDataRecv
  * ��    ����ֹͣ���ݽ���
  * �����������
  * �����������
  * �� �� ֵ����
  * ˵    ������
 ******************************************************************************/
_RIGOL_DLL_API void stopDataRecv( void );
#endif