/********************************************************************
��Դ����Ƽ��ɷ����޹�˾ ��Ȩ����(2020 - 2030)
*********************************************************************
ͷ�ļ���: inter_and_sample.cpp
��������: ������ֵ������Sinc��ֵ���˲�
��    ��: sn01625
��    ��: 0.1
��������: 2020-06-28  15:10 PM

�޸ļ�¼1��// �޸���ʷ��¼�������޸����ڡ��޸��߼��޸�����
�޸����ڣ�
�� �� �ţ�
�� �� �ˣ�
�޸����ݣ�
*********************************************************************/
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <process.h>
#include <Windows.h>
#include <time.h>
#include "insert_and_sample.h"

/********************************************************************
�궨������
*********************************************************************/
#define                 MEMORY_USE_OPTIMIZE_SWITCH  1   // �ڴ�ʹ���Ż�����,0��ʶ�ر��Ż�,1��ʶ���Ż�
#define                 SINC_FILTER_STEP_NUM        8   // Sinc�˲�������
#define                 SINC_FILTER_THREAD_NUM      512

/********************************************************************
ö�ٶ�������
*********************************************************************/

/********************************************************************
�ṹ�嶨������
*********************************************************************/

/*
* �����޷��ŵ��ֽ�����ķ��ֵ������Ϣ�Ľṹ��
* ����:1.���ֵ
*      2.��Сֵ
*      3.���ֵ�ڱ��ν������������λ��
*      4.��Сֵ�ڱ��ν������������λ��
*/
typedef struct stValInfo4UChar
{
    unsigned char   max;        //  ���ֵ
    unsigned char   min;        //  ��Сֵ
    short           resv;       //  �����ֽ�,���ڶ���
    unsigned int    max_pos;    //  ���ֵ������λ��
    unsigned int    min_pos;    //  ��Сֵ������λ��
}PPEAK_VALUE_INFO_FOR_CAHR;

/*
* ����˫���ȸ�������ķ��ֵ������Ϣ�Ľṹ��
* ����:1.���ֵ
*      2.��Сֵ
*      3.���ֵ�ڱ��ν������������λ��
*      4.��Сֵ�ڱ��ν������������λ��
*/
typedef struct stValInfo4Double
{
    unsigned int    max_pos;    //  ���ֵ������λ��  
    unsigned int    min_pos;    //  ��Сֵ������λ��
    double          max;        //  ���ֵ
    double          min;        //  ��Сֵ
}PPEAK_VALUE_INFO_FOR_DOUBLE;

/*
* ����Sinc�ڲ��˲�����ϵ������Ϣ�Ľṹ��
* ����:1.�ڲ�ϵ����ö��ֵ,��ö��ֵeuInterMutipule����
*      2.ϵ����洢�ĵ�ַ,�ɵ���ʱ��������
*      3.ϵ�����С
*      4.�ڲ屶��
*      5.��һ��ϵ��
*      6.ָ����һ���ڲ�ϵ����ĵ�ַ
*/
typedef struct stSincTable
{
    int					xn;         //  �ڲ�ϵ��
    int* table;      //  �ڲ�ϵ����
    int                 size;       //  �ڲ�ϵ����Ĵ�С
    int                 multipule;  //  �ڲ屶��
    double              coe;        //  ��һ��ϵ��
    struct stSincTable* next;       //  ��һ��ϵ�����λ��
}SINC_TABLE_STRU;

typedef struct stSincFilterPara
{
    unsigned char* src;
    double* dst;
    int                     start;
    int                     end;
    int                     xn;
    int                     cstart;
    int                     clen;
    int                     dsize;
    HANDLE* sync;
    unsigned int            ctime;
    unsigned int            stime;
    unsigned int            etime;
}SINC_FILTER_PARA_STRU;

/********************************************************************
���ر�����������
*********************************************************************/
SINC_TABLE_STRU* m_pstSincTable = NULL;  // ��¼Sinc�ڲ�ϵ���������
SINC_FILTER_PARA_STRU       m_astSincFilterPara[SINC_FILTER_THREAD_NUM];
HANDLE                      m_asemSincFilterThreadSync[SINC_FILTER_THREAD_NUM];
int                         m_s32CurrentSincThreadNum = 0;
int                         m_chanMemPoints = 1000008;//��¼����ͨ������ĵ���
/********************************************************************
�ڲ���������
*********************************************************************/

/*******************************************************************************
* ��    ����findMaxAndMinVal4UChar
* ��    ��������������޷��ŵ��ֽ���������ķ��ֵ��Ϣ�����
* ���������
*             ��������            ��������                ����˵��
*             pu8DataBuff         unsigned char*          �޷��ŵ��ֽ���������
*             u32DataLength       unsigned int            �޷��ŵ��ֽ���������ĳ���
* ���������
*             ��������            ��������                ����˵��
*             pstInfo      PPEAK_VALUE_INFO_FOR_CAHR*     ���ֵ��Ϣ��
* �� �� ֵ����
* ˵    ������
******************************************************************************/
void findMaxAndMinVal4UChar(unsigned char* pu8DataBuff,
    unsigned int u32DataLength,
    PPEAK_VALUE_INFO_FOR_CAHR* pstInfo)
{
    unsigned int    u32MaxPos = 0;
    unsigned int    u32MinPos = 0;
    unsigned int    i = 0;
    unsigned char   u8MaxValue = pu8DataBuff[0];
    unsigned char   u8MinValue = pu8DataBuff[0];


    // ��ʼ���������Сֵ
    for (; i < u32DataLength; i++)
    {
        if (pu8DataBuff[i] < u8MinValue)
        {
            u32MinPos = i;
            u8MinValue = pu8DataBuff[i];
        }
        else if (pu8DataBuff[i] > u8MaxValue)
        {
            u32MaxPos = i;
            u8MaxValue = pu8DataBuff[i];
        }
    }

    // ���¼�¼��Ϣ
    pstInfo->max_pos = u32MaxPos;
    pstInfo->min_pos = u32MinPos;
    pstInfo->max = u8MaxValue;
    pstInfo->min = u8MinValue;
}

/*******************************************************************************
* ��    ����findMaxAndMinVal4Double
* ��    �������������˫���ȸ���������ķ��ֵ��Ϣ�����
* ���������
*             ��������            ��������                ����˵��
*             pf64DataBuff        double*                 ˫���ȸ���������
*             u32DataLength       unsigned int            ˫���ȸ���������ĳ���
* ���������
*             ��������            ��������                ����˵��
*             pstInfo      PPEAK_VALUE_INFO_FOR_CAHR*     ���ֵ��Ϣ��
* �� �� ֵ����
* ˵    ������
******************************************************************************/
void findMaxAndMinVal4Double(double* pf64DataBuff, unsigned int u32DataLength, PPEAK_VALUE_INFO_FOR_DOUBLE* pstInfo)
{
    unsigned int   u32MaxPos = 0;
    unsigned int   u32MinPos = 0;
    unsigned int   i = 0;
    double         f64MaxValue = pf64DataBuff[0];
    double         f64MinValue = pf64DataBuff[0];


    // ��ʼ���������Сֵ
    for (; i < u32DataLength; i++)
    {
        if (pf64DataBuff[i] < f64MinValue)
        {
            u32MinPos = i;
            f64MinValue = pf64DataBuff[i];
        }
        else if (pf64DataBuff[i] > f64MaxValue)
        {
            u32MaxPos = i;
            f64MaxValue = pf64DataBuff[i];
        }
    }

    // ���¼�¼��Ϣ
    pstInfo->max_pos = u32MaxPos;
    pstInfo->min_pos = u32MinPos;
    pstInfo->max = f64MaxValue;
    pstInfo->min = f64MinValue;
}

/*******************************************************************************
* ��    ����getSincInterTable
* ��    ��������Sinc�˲���ϵ��,�����Ѽ��ص��ڲ�ϵ����
* ���������
*             ��������            ��������                ����˵��
*             emMutipule          euInterMutipule         Sinc�ڲ��˲���ϵ��
* �����������
* �� �� ֵ���������򷵻�,δ�������򷵻�NULL
* ˵    ������
******************************************************************************/
struct stSincTable* getSincInterTable(int s32MutipuleXn)
{
    struct stSincTable* pstNode = m_pstSincTable;

    while (pstNode != NULL)
    {
        if (pstNode->xn == s32MutipuleXn)
        {
            break;
        }
        else
        {
            pstNode = pstNode->next;
        }
    }

    return pstNode;
}

/*******************************************************************************
* ��    ����SyncWaitForRecvFinish(�ڲ��ӿ�)
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
DWORD SyncWaitForSincFilterSync(HANDLE* handles, int count)
{
    int waitingThreadsCount = count;
    int index = 0;
    DWORD res = 0;
    while (waitingThreadsCount >= MAXIMUM_WAIT_OBJECTS)
    {
        res = WaitForMultipleObjects(MAXIMUM_WAIT_OBJECTS, &handles[index], TRUE, INFINITE);
        waitingThreadsCount -= MAXIMUM_WAIT_OBJECTS;
        index += MAXIMUM_WAIT_OBJECTS;
    }

    if (waitingThreadsCount > 0)
    {
        res = WaitForMultipleObjects(waitingThreadsCount, &handles[index], TRUE, INFINITE);
    }

    return res;
}

/********************************************************************
�ӿں�������
*********************************************************************/



/*******************************************************************************
* ��    ����waveCompAlgorithm4UChar
* ��    ������������޷��ŵ��ֽ�����������з��ֵ����
* ���������
*             ��������            ��������                ����˵��
*             pu8SrcBuff          unsigned char*          �޷��ŵ��ֽ���������
*             s32Offset           int                     ���鿪ʼѹ������ʼ����ֵ
*             s32SrcLength        int                     �޷��ŵ��ֽ��������鳤��
*             s32DstLength        int                     ׼�����������ݵ���
* ���������
*             ��������            ��������                ����˵��
*             pu8DstBuff          unsigned char *         �������
* �� �� ֵ����
* ˵    ������
******************************************************************************/
void waveCompAlgorithm4UChar(unsigned char* pu8SrcBuff,
    int s32Offset,
    int s32SrcLength,
    unsigned char* pu8DstBuff,
    int s32DstLength)
{
    unsigned int                u32MaxPos = 0;
    unsigned int                u32MinPos = 0;
    unsigned int                u32CompressNumTemp = s32DstLength * 2;//׼��ѹ�������ݳ���
    unsigned int                u32CurrCompPos = 0;
    unsigned int                u32CurrCompLength = 0;
    int	                        i = 0;
    int                         j = 0;
    int                         k = 0;
    unsigned int                u32Multiple = (unsigned int)floor((float)s32SrcLength / (float)s32DstLength); // ѹ������
    unsigned char* pu8TempBuff = (unsigned char*)0;
    unsigned char               au8Dot[4] = { 0 }; // �ж�ֻ��4����ʱ,����ѹ��
    PPEAK_VALUE_INFO_FOR_CAHR   stFindInfo = { 0 };

    // 1 ������ʱ����ѹ��������,ע��ʹ����ɺ�����ͷ�,����s32DstLength * 2�����ڻ�����ÿ�ηֶγ�ֵ�����ֵ����Сֵ
    pu8TempBuff = (unsigned char*)malloc(sizeof(unsigned char) * u32CompressNumTemp);

    // 2 ��ʼ����ѹ��
    for (; i < s32DstLength; i++)
    {
        // 2.1 ���㱾�γ��������������ݵ�λ�ü���������е����ݳ���,����Ҫ��ʣ�಻��һ�ε����ݽ��е�������
        if (i == (s32DstLength - 1))
        {
            u32CurrCompPos = u32Multiple * i;
            u32CurrCompLength = s32SrcLength - u32CurrCompPos;
        }
        else
        {
            u32CurrCompPos = u32Multiple * i;
            u32CurrCompLength = u32Multiple;
        }

        // 2.2 ���ҷֶ������е����ֵ����Сֵ
        findMaxAndMinVal4UChar(&pu8SrcBuff[u32CurrCompPos + s32Offset], u32CurrCompLength, &stFindInfo);

        // 2.3 ���ݲ��ҵ������ֵ����Сֵ��Ϣ,���������������е�λ��
        u32MaxPos = u32CurrCompPos + stFindInfo.max_pos + s32Offset;
        u32MinPos = u32CurrCompPos + stFindInfo.min_pos + s32Offset;

        // 2.4 ����λ����Ϣ,��¼����ֵ,���￼�����ݵ�ķֲ�,������ģʽ(���ֵλ�ô�����Сֵλ��),�½���ģʽ(���ֵλ��С�ڵ�����Сֵλ��)
        if (u32MaxPos > u32MinPos)// ������ģʽ
        {
            pu8TempBuff[k] = pu8SrcBuff[u32MinPos];
            pu8TempBuff[k + 1] = pu8SrcBuff[u32MaxPos];

        }
        else
        {
            pu8TempBuff[k] = pu8SrcBuff[u32MaxPos];
            pu8TempBuff[k + 1] = pu8SrcBuff[u32MinPos];

        }
        k += 2;
    }

    // 3 ��ʼ���ݷ��ֵ�������ݳ���
    // ������4��������pu8TempBuff�У�pu8TempBuff[0] < pu8TempBuff[1], pu8TempBuff[2] < pu8TempBuff[3]
    // ����4����������Ҫѡ��2����Ϊ��������������ݣ��ص��ǱȽ�pu8TempBuff[1]��pu8TempBuff[2]�Ƿ��Ǽ�ֵ
    // ���޼�ֵ pu8TempBuff[0] < pu8TempBuff[1] < pu8TempBuff[2] < pu8TempBuff[3],��ȡpu8DstBuff[j] = (pu8TempBuff[1] + pu8TempBuff[2]) / 2
    // �����Ǽ�ֵ pu8TempBuff[2] < pu8TempBuff[3] < pu8TempBuff[0] < pu8TempBuff[1],��ȡpu8DstBuff[j] = pu8TempBuff[1],pu8TempBuff[i+2] = au8Dot[2]
    // ��ֻ��һ����ֵ pu8TempBuff[0] < pu8TempBuff[2] < pu8TempBuff[3] < pu8TempBuff[1],��ȡpu8DstBuff[j] = pu8TempBuff[1]
    pu8DstBuff[0] = pu8TempBuff[0];// ��¼�ж���ʼֵ
    // ��ʼѭ���������ݲ���
    for (i = 1, j = 0; i < k; i += 2, j++)
    {
        // 3.1 ȡ����4��������ж�
        au8Dot[0] = pu8DstBuff[j];
        au8Dot[1] = pu8TempBuff[i];
        au8Dot[2] = pu8TempBuff[i + 1];
        au8Dot[3] = pu8TempBuff[i + 2];

        // 3.2 ����4��������ֵ����Сֵλ��
        findMaxAndMinVal4UChar(au8Dot, 4, &stFindInfo);

        // 3.3 ��¼���ֵ����Сֵ��λ��
        u32MaxPos = stFindInfo.max_pos;
        u32MinPos = stFindInfo.min_pos;

        // 3.4 �����ж�
        if ((u32MinPos != 1) && (u32MaxPos != 2) && (u32MinPos != 2) && (u32MaxPos != 1))
        {
            pu8DstBuff[j] = (unsigned char)((au8Dot[1] + au8Dot[2]) / 2);

        }
        else if (((u32MinPos == 2) && (u32MaxPos == 1)) || ((u32MinPos == 1) && (u32MaxPos == 2)))
        {
            pu8DstBuff[j] = au8Dot[1];

            pu8TempBuff[i + 2] = au8Dot[2];
        }
        else if ((u32MaxPos == 1) || (u32MinPos == 1))
        {
            pu8DstBuff[j] = au8Dot[1];

        }
        else if ((u32MinPos == 2) || (u32MaxPos == 2))
        {
            pu8DstBuff[j] = au8Dot[2];

        }

        pu8DstBuff[j + 1] = pu8TempBuff[i];

        //���������forѭ��
        if (i + 2 == k - 1)
        {
            break;
        }
    }

    // 4 �ͷ�ʹ�õ��ڴ�
    free(pu8TempBuff);
}
/*******************************************************************************
* ��    ����waveDataPreProcess4UChar
* ��    ������ԭʼ���ݽ���Ԥ�����Բ��ν��д��ƣ��ƶ���һ���ڴ����������
* ���������
*             ��������            ��������                ����˵��
*             pu8SrcBuff          unsigned char*          �޷��ŵ��ֽ���������
*             s32Offset           int                     ���鿪ʼѹ������ʼ����ֵ
* ���������
*             ��������            ��������                ����˵��
*             pu8SrcBuff          unsigned char *         ���º��ԭʼ��
* �� �� ֵ����
* ˵    ������
******************************************************************************/
void  origWaveDataPreProcess4UChar(unsigned char* pu8SrcBuff, int s32Offset)
{
    //����Ԥ���������Ĳ���ʹ�����һ������п���
    for (int i = s32Offset; i < m_chanMemPoints + s32Offset; i++)
    {
        if (i < m_chanMemPoints)
        {
            pu8SrcBuff[i - s32Offset] = pu8SrcBuff[i];
        }
        else
        {
            pu8SrcBuff[i - s32Offset] = pu8SrcBuff[m_chanMemPoints - 1];
        }
    }
}
/*******************************************************************************
* ��    ����waveCompAlgorithm4UCharSimple
* ��    ������������޷��ŵ��ֽ�����������з��ֵ����
* ���������
*             ��������            ��������                ����˵��
*             pu8SrcBuff          unsigned char*          �޷��ŵ��ֽ���������
*             s32Offset           int                     ���鿪ʼѹ������ʼ����ֵ
*             s32SrcLength        int                     �޷��ŵ��ֽ��������鳤��
*             s32DstLength        int                     ׼�����������ݵ���
* ���������
*             ��������            ��������                ����˵��
*             pu8DstBuff          unsigned char *         �������
*             s32OutPos           int*                    �����ʼ�����ֹ���λ��
* �� �� ֵ����
* ˵    ������
******************************************************************************/
void waveCompAlgorithm4UCharSimple(unsigned char* pu8SrcBuff,
    int s32Offset,
    int s32SrcLength,
    unsigned char* pu8DstBuff,
    int s32DstLength,
    int* s32OutPos)
{
    unsigned int                u32MaxPos = 0;
    unsigned int                u32MinPos = 0;
    unsigned int                u32CompressNumTemp = s32DstLength;
    unsigned int                u32CurrCompPos = 0;
    unsigned int                u32CurrCompLength = 0;
    int	                        i = 0;
    int                         j = 0;
    int                         k = 0;
    unsigned int                u32Multiple = (unsigned int)floor((float)s32SrcLength / (float)(s32DstLength / 2.0)); // ѹ������
    unsigned char* pu8TempBuff = (unsigned char*)0;
    unsigned int* pu32TempPos = (unsigned int*)0;
    unsigned char               au8Dot[4] = { 0 }; // �ж�ֻ��4����ʱ,����ѹ��
    PPEAK_VALUE_INFO_FOR_CAHR   stFindInfo = { 0 };

    // 1 ������ʱ����ѹ��������,ע��ʹ����ɺ�����ͷ�,����s32DstLength * 2�����ڻ�����ÿ�ηֶγ�ֵ�����ֵ����Сֵ
    pu8TempBuff = (unsigned char*)malloc(sizeof(unsigned char) * u32CompressNumTemp);
    pu32TempPos = (unsigned int*)malloc(sizeof(int) * u32CompressNumTemp);
    //����Ԥ����
    if (s32Offset + s32SrcLength >= m_chanMemPoints)
    {
        //���뿽�����ݿռ�
        unsigned char* pu8WavDataBuff = (unsigned char*)malloc(sizeof(unsigned char) * (s32Offset + s32SrcLength));
        // �ȸ���ԭʼ����
        memcpy((char*)pu8WavDataBuff, (char*)pu8SrcBuff, sizeof(unsigned char) * m_chanMemPoints);
        // ��cpyȱ�ٵĵ�
        for (int i = m_chanMemPoints; i < s32Offset + s32SrcLength; i++)
        {
            pu8WavDataBuff[i] = pu8SrcBuff[m_chanMemPoints - 1];
        }
        // 2 ��ʼ����ѹ��,�ӹ涨��������ȡ��1K����ԭ������ƽ����Ϊ500�֣�ÿһ����ȡ�������Сֵ����ѹ��
        for (; i < s32DstLength / 2; i++)
        {
            // 2.1 ���㱾�γ��������������ݵ�λ�ü���������е����ݳ���,����Ҫ��ʣ�಻��һ�ε����ݽ��е�������
            if (i == (s32DstLength - 1))
            {
                u32CurrCompPos = u32Multiple * i;
                u32CurrCompLength = s32SrcLength - u32CurrCompPos;
            }
            else
            {
                u32CurrCompPos = u32Multiple * i;
                u32CurrCompLength = u32Multiple;
            }
            // 2.2 ���ҷֶ������е����ֵ����Сֵ
            findMaxAndMinVal4UChar(&pu8WavDataBuff[u32CurrCompPos + s32Offset], u32CurrCompLength, &stFindInfo);

            // 2.3 ���ݲ��ҵ������ֵ����Сֵ��Ϣ,���������������е�λ��
            u32MaxPos = u32CurrCompPos + stFindInfo.max_pos + s32Offset;
            u32MinPos = u32CurrCompPos + stFindInfo.min_pos + s32Offset;

            // 2.4 ����λ����Ϣ,��¼����ֵ,���￼�����ݵ�ķֲ�,������ģʽ(���ֵλ�ô�����Сֵλ��),�½���ģʽ(���ֵλ��С�ڵ�����Сֵλ��)
            if (u32MaxPos > u32MinPos)// ������ģʽ
            {
                pu8TempBuff[k] = pu8WavDataBuff[u32MinPos];
                pu8TempBuff[k + 1] = pu8WavDataBuff[u32MaxPos];
                pu32TempPos[k] = u32MinPos;
                pu32TempPos[k + 1] = u32MaxPos;
                // ��һ����
                if (i == 0)
                {
                    // ��ʼ��λ��
                    s32OutPos[0] = u32MinPos;
                }
                else if (i == s32DstLength / 2 - 1)
                {
                    //��ֹ��
                    s32OutPos[1] = u32MaxPos;
                }
            }
            else
            {
                pu8TempBuff[k] = pu8WavDataBuff[u32MaxPos];
                pu8TempBuff[k + 1] = pu8WavDataBuff[u32MinPos];
                pu32TempPos[k] = u32MaxPos;
                pu32TempPos[k + 1] = u32MinPos;
                // ��һ����
                if (i == 0)
                {
                    // ��ʼ��λ��
                    s32OutPos[0] = u32MaxPos;
                }
                else if (i == s32DstLength / 2 - 1)
                {
                    //��ֹ��
                    s32OutPos[1] = u32MinPos;
                }
            }
            k += 2;
        }
        free(pu8WavDataBuff);
    }
    else
    {
        // 2 ��ʼ����ѹ��,�ӹ涨��������ȡ��1K����ԭ������ƽ����Ϊ500�֣�ÿһ����ȡ�������Сֵ����ѹ��
        for (; i < s32DstLength / 2; i++)
        {
            // 2.1 ���㱾�γ��������������ݵ�λ�ü���������е����ݳ���,����Ҫ��ʣ�಻��һ�ε����ݽ��е�������
            if (i == (s32DstLength - 1))
            {
                u32CurrCompPos = u32Multiple * i;
                u32CurrCompLength = s32SrcLength - u32CurrCompPos;
            }
            else
            {
                u32CurrCompPos = u32Multiple * i;
                u32CurrCompLength = u32Multiple;
            }
            // 2.2 ���ҷֶ������е����ֵ����Сֵ
            findMaxAndMinVal4UChar(&pu8SrcBuff[u32CurrCompPos + s32Offset], u32CurrCompLength, &stFindInfo);

            // 2.3 ���ݲ��ҵ������ֵ����Сֵ��Ϣ,���������������е�λ��
            u32MaxPos = u32CurrCompPos + stFindInfo.max_pos + s32Offset;
            u32MinPos = u32CurrCompPos + stFindInfo.min_pos + s32Offset;

            // 2.4 ����λ����Ϣ,��¼����ֵ,���￼�����ݵ�ķֲ�,������ģʽ(���ֵλ�ô�����Сֵλ��),�½���ģʽ(���ֵλ��С�ڵ�����Сֵλ��)
            if (u32MaxPos > u32MinPos)// ������ģʽ
            {
                pu8TempBuff[k] = pu8SrcBuff[u32MinPos];
                pu8TempBuff[k + 1] = pu8SrcBuff[u32MaxPos];
                pu32TempPos[k] = u32MinPos;
                pu32TempPos[k + 1] = u32MaxPos;
                // ��һ����
                if (i == 0)
                {
                    // ��ʼ��λ��
                    s32OutPos[0] = u32MinPos;
                }
                else if (i == s32DstLength / 2 - 1)
                {
                    //��ֹ��
                    s32OutPos[1] = u32MaxPos;
                }
            }
            else
            {
                pu8TempBuff[k] = pu8SrcBuff[u32MaxPos];
                pu8TempBuff[k + 1] = pu8SrcBuff[u32MinPos];
                pu32TempPos[k] = u32MaxPos;
                pu32TempPos[k + 1] = u32MinPos;
                // ��һ����
                if (i == 0)
                {
                    // ��ʼ��λ��
                    s32OutPos[0] = u32MaxPos;
                }
                else if (i == s32DstLength / 2 - 1)
                {
                    //��ֹ��
                    s32OutPos[1] = u32MinPos;
                }
            }
            k += 2;
        }
    }
    //  ����ѹ����������
    memcpy((char*)pu8DstBuff, (char*)pu8TempBuff, sizeof(unsigned char) * u32CompressNumTemp);

    // 4 �ͷ�ʹ�õ��ڴ�
    free(pu8TempBuff);
    free(pu32TempPos);
}

/*******************************************************************************
* ��    ����waveCompAlgorithm4Double
* ��    �����������˫���ȸ�����������з��ֵ����
* ���������
*             ��������            ��������                ����˵��
*             pf64SrcBuff         double*                 ˫���ȸ���������
*             s32Offset           int                     ���鿪ʼѹ������ʼ����ֵ
*             s32SrcLength        int                     ˫���ȸ��������鳤��
*             s32DstLength        int                     ׼�����������ݵ���
* ���������
*             ��������            ��������                ����˵��
*             pf64DstBuff         double *                �������
* �� �� ֵ����
* ˵    ������
******************************************************************************/
void waveCompAlgorithm4Double(double* pf64SrcBuff,
    int s32Offset,
    int s32SrcLength,
    double* pf64DstBuff,
    int s32DstLength)
{
    unsigned int                u32MaxPos = 0;
    unsigned int                u32MinPos = 0;
    unsigned int                u32CompressNumTemp = s32DstLength * 2;//׼��ѹ�������ݳ���
    unsigned int                u32CurrCompPos = 0;
    unsigned int                u32CurrCompLength = 0;
    int	                        i = 0;
    int                         j = 0;
    int                         k = 0;
    unsigned int                u32Multiple = (unsigned int)floor((float)s32SrcLength / (float)s32DstLength); // ѹ������
    double* pf64TempBuff = (double*)0;
    double                      af64Dot[4] = { 0 }; // �ж�ֻ��4����ʱ,����ѹ��
    PPEAK_VALUE_INFO_FOR_DOUBLE stFindInfo = { 0 };

    // 1 ������ʱ����ѹ��������,ע��ʹ����ɺ�����ͷ�,����s32DstLength * 2�����ڻ�����ÿ�ηֶγ�ֵ�����ֵ����Сֵ
    pf64TempBuff = (double*)malloc(sizeof(double) * u32CompressNumTemp);

    // 2 ��ʼ����ѹ��
    for (; i < s32DstLength; i++)
    {
        // 2.1 ���㱾�γ��������������ݵ�λ�ü���������е����ݳ���,����Ҫ��ʣ�಻��һ�ε����ݽ��е�������
        if (i == (s32DstLength - 1))
        {
            u32CurrCompPos = u32Multiple * i;
            u32CurrCompLength = s32SrcLength - u32CurrCompPos;
        }
        else
        {
            u32CurrCompPos = u32Multiple * i;
            u32CurrCompLength = u32Multiple;
        }

        // 2.2 ���ҷֶ������е����ֵ����Сֵ
        findMaxAndMinVal4Double(&pf64SrcBuff[u32CurrCompPos + s32Offset], u32CurrCompLength, &stFindInfo);

        // 2.3 ���ݲ��ҵ������ֵ����Сֵ��Ϣ,���������������е�λ��
        u32MaxPos = u32CurrCompPos + stFindInfo.max_pos + s32Offset;
        u32MinPos = u32CurrCompPos + stFindInfo.min_pos + s32Offset;

        // 2.4 ����λ����Ϣ,��¼����ֵ,���￼�����ݵ�ķֲ�,������ģʽ(���ֵλ�ô�����Сֵλ��),�½���ģʽ(���ֵλ��С�ڵ�����Сֵλ��)
        if (u32MaxPos > u32MinPos)// ������ģʽ
        {
            pf64TempBuff[k] = pf64SrcBuff[u32MinPos];
            pf64TempBuff[k + 1] = pf64SrcBuff[u32MaxPos];

        }
        else
        {
            pf64TempBuff[k] = pf64SrcBuff[u32MaxPos];
            pf64TempBuff[k + 1] = pf64SrcBuff[u32MinPos];
        }
        k += 2;
    }

    // 3 ��ʼ���ݷ��ֵ�������ݳ���
    // ������4��������pf64TempBuff�У�pf64TempBuff[0] < pf64TempBuff[1], pf64TempBuff[2] < pf64TempBuff[3]
    // ����4����������Ҫѡ��2����Ϊ��������������ݣ��ص��ǱȽ�pf64TempBuff[1]��pf64TempBuff[2]�Ƿ��Ǽ�ֵ
    // ���޼�ֵ pf64TempBuff[0] < pf64TempBuff[1] < pf64TempBuff[2] < pf64TempBuff[3],��ȡpf64DstBuff[j] = (pf64TempBuff[1] + pf64TempBuff[2]) / 2
    // �����Ǽ�ֵ pf64TempBuff[2] < pf64TempBuff[3] < pf64TempBuff[0] < pf64TempBuff[1],��ȡpf64DstBuff[j] = pf64TempBuff[1],pf64TempBuff[i+2] = af64Dot[2]
    // ��ֻ��һ����ֵ pf64TempBuff[0] < pf64TempBuff[2] < pf64TempBuff[3] < pf64TempBuff[1],��ȡpf64DstBuff[j] = pf64TempBuff[1]
    pf64DstBuff[0] = pf64TempBuff[0];// ��¼�ж���ʼֵ
    // ��ʼѭ���������ݲ���
    for (i = 1, j = 0; i < k; i += 2, j++)
    {
        // 3.1 ȡ����4��������ж�
        af64Dot[0] = pf64DstBuff[j];
        af64Dot[1] = pf64TempBuff[i];
        af64Dot[2] = pf64TempBuff[i + 1];
        af64Dot[3] = pf64TempBuff[i + 2];

        // 3.2 ����4��������ֵ����Сֵλ��
        findMaxAndMinVal4Double(af64Dot, 4, &stFindInfo);

        // 3.3 ��¼���ֵ����Сֵ��λ��
        u32MaxPos = stFindInfo.max_pos;
        u32MinPos = stFindInfo.min_pos;

        // 3.4 �����ж�
        if ((u32MinPos != 1) && (u32MaxPos != 2) && (u32MinPos != 2) && (u32MaxPos != 1))
        {
            pf64DstBuff[j] = ((af64Dot[1] + af64Dot[2]) / 2.0);
        }
        else if (((u32MinPos == 2) && (u32MaxPos == 1)) || ((u32MinPos == 1) && (u32MaxPos == 2)))
        {
            pf64DstBuff[j] = af64Dot[1];
            pf64TempBuff[i + 2] = af64Dot[2];
        }
        else if ((u32MaxPos == 1) || (u32MinPos == 1))
        {
            pf64DstBuff[j] = af64Dot[1];

        }
        else if ((u32MinPos == 2) || (u32MaxPos == 2))
        {
            pf64DstBuff[j] = af64Dot[2];

        }

        pf64DstBuff[j + 1] = pf64TempBuff[i];

        //���������forѭ��
        if (i + 2 == k - 1)
        {
            break;
        }
    }

    // 4 �ͷ�ʹ�õ��ڴ�
    free(pf64TempBuff);

}

/*******************************************************************************
* ��    ����waveCompAlgorithm4DoubleSimple
* ��    �����������˫���ȸ�����������з��ֵ����
* ���������
*             ��������            ��������                ����˵��
*             pf64SrcBuff         double*                 ˫���ȸ���������
*             s32Offset           int                     ���鿪ʼѹ������ʼ����ֵ
*             s32SrcLength        int                     ˫���ȸ��������鳤��
*             s32DstLength        int                     ׼�����������ݵ���
* ���������
*             ��������            ��������                ����˵��
*             pf64DstBuff         double *                �������
*             s32OutPos           int*                    �����ʼ�����ֹ���λ��
* �� �� ֵ����
* ˵    ������
******************************************************************************/
void waveCompAlgorithm4DoubleSimple(double* pf64SrcBuff,
    int s32Offset,
    int s32SrcLength,
    double* pf64DstBuff,
    int s32DstLength,
    int* s32OutPos)
{
    unsigned int                u32MaxPos = 0;
    unsigned int                u32MinPos = 0;
    unsigned int                u32CompressNumTemp = s32DstLength;//׼��ѹ�������ݳ���
    unsigned int                u32CurrCompPos = 0;
    unsigned int                u32CurrCompLength = 0;
    int	                        i = 0;
    int                         j = 0;
    int                         k = 0;
    unsigned int                u32Multiple = (unsigned int)floor((float)s32SrcLength / (float)(s32DstLength / 2.0)); // ѹ������
    double* pf64TempBuff = (double*)0;
    unsigned int* pu32TempPos = (unsigned int*)0;
    double                      af64Dot[4] = { 0 }; // �ж�ֻ��4����ʱ,����ѹ��
    PPEAK_VALUE_INFO_FOR_DOUBLE stFindInfo = { 0 };

    // 1 ������ʱ����ѹ��������,ע��ʹ����ɺ�����ͷ�,����s32DstLength * 2�����ڻ�����ÿ�ηֶγ�ֵ�����ֵ����Сֵ
    pf64TempBuff = (double*)malloc(sizeof(double) * u32CompressNumTemp);
    pu32TempPos = (unsigned int*)malloc(sizeof(int) * u32CompressNumTemp);

    // 2 ��ʼ����ѹ��
    for (; i < s32DstLength / 2; i++)
    {
        // 2.1 ���㱾�γ��������������ݵ�λ�ü���������е����ݳ���,����Ҫ��ʣ�಻��һ�ε����ݽ��е�������
        if (i == (s32DstLength - 1))
        {
            u32CurrCompPos = u32Multiple * i;
            u32CurrCompLength = s32SrcLength - u32CurrCompPos;
        }
        else
        {
            u32CurrCompPos = u32Multiple * i;
            u32CurrCompLength = u32Multiple;
        }

        // 2.2 ���ҷֶ������е����ֵ����Сֵ
        findMaxAndMinVal4Double(&pf64SrcBuff[u32CurrCompPos + s32Offset], u32CurrCompLength, &stFindInfo);

        // 2.3 ���ݲ��ҵ������ֵ����Сֵ��Ϣ,���������������е�λ��
        u32MaxPos = u32CurrCompPos + stFindInfo.max_pos + s32Offset;
        u32MinPos = u32CurrCompPos + stFindInfo.min_pos + s32Offset;

        // 2.4 ����λ����Ϣ,��¼����ֵ,���￼�����ݵ�ķֲ�,������ģʽ(���ֵλ�ô�����Сֵλ��),�½���ģʽ(���ֵλ��С�ڵ�����Сֵλ��)
        if (u32MaxPos > u32MinPos)// ������ģʽ
        {
            pf64TempBuff[k] = pf64SrcBuff[u32MinPos];
            pf64TempBuff[k + 1] = pf64SrcBuff[u32MaxPos];
            pu32TempPos[k] = u32MinPos;
            pu32TempPos[k + 1] = u32MaxPos;
            // ��һ����
            if (i == 0)
            {
                // ��ʼ��λ��
                s32OutPos[0] = u32MinPos;
            }
            else if (i == s32DstLength / 2 - 1)
            {
                //��ֹ��
                s32OutPos[1] = u32MaxPos;
            }
        }
        else
        {
            pf64TempBuff[k] = pf64SrcBuff[u32MaxPos];
            pf64TempBuff[k + 1] = pf64SrcBuff[u32MinPos];
            pu32TempPos[k] = u32MaxPos;
            pu32TempPos[k + 1] = u32MinPos;
            // ��һ����
            if (i == 0)
            {
                // ��ʼ��λ��
                s32OutPos[0] = u32MaxPos;
            }
            else if (i == s32DstLength / 2 - 1)
            {
                //��ֹ��
                s32OutPos[1] = u32MinPos;
            }
        }
        k += 2;
    }

    //  ����ѹ����������
    memcpy((char*)pf64DstBuff, (char*)pf64TempBuff, sizeof(double) * u32CompressNumTemp);

    // 4 �ͷ�ʹ�õ��ڴ�
    free(pf64TempBuff);
    free(pu32TempPos);
}

/*******************************************************************************
* ��    ����loadSincInterTable
* ��    ��������Sinc�ڲ�ϵ����
* ���������
*             ��������            ��������                ����˵��
*             s32MutipuleXn       int						Sinc�ڲ��˲���ϵ��
*             ps32Table           int*                    Sinc�ڲ��˲���ϵ����
*             s32TableSize        int                     Sinc�ڲ��˲���ϵ�����С
*             s32Coe              int                     ����ת�������
*             s32Multipule        int                     �����ڲ�ʱ,���ݷŴ���
* �����������
* �� �� ֵ����
* ˵    ���������ڲ��˲���ϵ�����Ѵ���,�򲻽���ע��
******************************************************************************/
void loadSincInterTable(int s32MutipuleXn,
    int* ps32Table,
    int s32TableSize,
    int s32Coe,
    int s32Multipule)
{
    struct stSincTable* pstNode = NULL;
    int* ps32TableAddr = NULL;
    int                  i;

    // 1 �����ڲ�ϵ�����Ƿ����
    pstNode = getSincInterTable(s32MutipuleXn);
    // 2 ����������ע���ϵ����,��ֱ�ӷ���
    if (pstNode != NULL)
    {
        return;
    }

    // 3 ����ϵ������Ϣ�Ĵ洢�ڵ���ڴ�
    pstNode = (struct stSincTable*)malloc(sizeof(struct stSincTable));

    // 4 �ж��ڴ��Ƿ���Ч,����Ч��ֱ�ӷ���
    if (pstNode == NULL)
    {
        return;
    }

    // 5 ����ϵ����ʹ�õĿռ�
    ps32TableAddr = (int*)malloc(sizeof(int) * s32TableSize);

    // 6 �ж��ڴ��Ƿ���Ч,����Ч��ֱ�ӷ���
    if (ps32TableAddr == NULL)
    {
        //  ��ʱ������������ϵ������Ϣ�Ĵ洢�ڵ���ڴ�,�������ͷ�
        free(pstNode);
        return;
    }

    // 7 ����Sinc�˲���ϵ�����ڴ�
    memcpy((char*)ps32TableAddr, (char*)ps32Table, sizeof(int) * s32TableSize);

    // 8 ���½ڵ���Ϣ
    pstNode->xn = s32MutipuleXn;
    pstNode->coe = (double)s32Coe;
    pstNode->table = ps32TableAddr;
    pstNode->size = s32TableSize;
    pstNode->multipule = s32Multipule;
    pstNode->next = m_pstSincTable;

    // 9 �Ѹýڵ��������ı�ͷ
    m_pstSincTable = pstNode;

    // 10 ��ʼ���߳�ͬ��ʱ���źų�
    for (i = 0; i < SINC_FILTER_THREAD_NUM; i++)
    {
        m_asemSincFilterThreadSync[i] = CreateSemaphore(NULL, 0, 1, NULL);
    }
}

/*******************************************************************************
* ��    ����getSincFilterData
* ��    ������ȡԭʼ��������Sinc�ڲ��˲�������ڲ�����
* ���������
*             ��������            ��������                ����˵��
*             pu8DataBuf          unsigned char *         ����Sinc�ڴ��˲���������
*                                                         ������Ϊ�޷��ŵ��ֽ�����
*             s32StartIndex       int                     Sinc�ڲ��˲�����ʼȡ����
*                                                         ʱ,�������е�λ��
*             s32EndIndex         int                     Sinc�ڲ��˲�������ȡ����
*                                                         ʱ,�������е�λ��
*             s32MutipuleXn       int			            Sinc�ڲ��˲���ʹ�õ��ڲ�
*                                                         ��
*             s32CompStartIndex   int                     �ڲ���ɺ�������������
*                                                         ģ��ʱ��������ʼ����
*             s32CompDataLen      int                     �ڲ���ɺ�������������
*                                                         ģ��ʱ�������ܳ�
*             s32OutBufLen        int                     �����Sinc�ڲ��˲������
*                                                         ��Ĵ�С
* ���������
*             ��������            ��������                ����˵��
*             pf64OutBuf          double*                 �����Sinc�ڲ��˲������
*                                                         ����
* �� �� ֵ��0 -- �ɹ� ; -1 -- û�ж�Ӧ��Sinc�ڲ��˲���ϵ����
* ˵    ������
******************************************************************************/
int getSincFilterData(unsigned char* pu8DataBuf,
    int              s32StartIndex,
    int              s32EndIndex,
    int		        s32MutipuleXn,
    int              s32CompStartIndex,
    int              s32CompDataLen,
    double* pf64OutBuf,
    int              s32OutBufLen)
{
    struct stSincTable* pstNode = NULL;    //  ����Sinc�ڲ屶��ö��ֵ,����Sinc��ֵ��Ϣ��Ĳ���
#if MEMORY_USE_OPTIMIZE_SWITCH == 0
    int* ps32SincTableData = NULL;
    int** pps32SincTablePtr = NULL;
#endif
    int                     s32InDataLen = 0;        //  
    int                     s32FrontExpEndPos = 0;
    int                     s32RearExpStartPos = 0;
    int                     s32TempFir[SINC_FILTER_STEP_NUM] = { 0 };    //  ��¼�˲������ڵ�ϵ��
    int                     s32MatrixRow = 0;
    int                     s32MatrixColum = 0;
    int                     s32IntegralVal = 0;
#if MEMORY_USE_OPTIMIZE_SWITCH == 0
    int* ps32TempAfterInter = NULL;
    int** pps32ResultAfterInter = NULL;
#endif
    int                     s32ResultAfterInterColum = 0;
    double* pf64TempWaveData = NULL;
    int                     s32WaveDataCopyStartIndex = 0;
    int                     i, j, k, row, colum;

    // 1 ��ȡSinc�ڲ��˲�����
    pstNode = getSincInterTable(s32MutipuleXn);

    // 2. �ж�Sinc����Ч��
    if (pstNode == NULL)
    {
        return -1;
    }
    /*
    * 3. ����Sinc�˲���ϵ����һάת��ά
    */
    // 3.1 ���ж�ά������к��еĸ�ֵ
    s32MatrixRow = pstNode->multipule;
    s32MatrixColum = SINC_FILTER_STEP_NUM;
#if MEMORY_USE_OPTIMIZE_SWITCH == 0 // �����ڴ����,���ﲻ�ٽ����ڴ�����,ֻ������Ϊ�ο����,���ڽ���һά����ά�ĵ��Ȳο�
    // 3.2 �������ݵ��ڴ�����,ʹ����ɺ�,�����ͷ�
    ps32SincTableData = (int*)malloc(s32MatrixRow * s32MatrixColum * sizeof(int));

    // 3.3 ���ж�ά������е�ַ�洢���ڴ�����,ʹ����ɺ�,�����ͷ�
    pps32SincTablePtr = (int**)malloc(s32MatrixRow * sizeof(int*));

    // 3.4 ���ж�ά�����ַ�ķ���
    for (i = 0; i < s32MatrixRow; i++)
    {
        pps32SincTablePtr[i] = ps32SincTableData + i * s32MatrixColum;
    }

    /*
    * 3.5 ����һά���鵽��ά�����ת��
    * ת������Ϊ
    *                             _     _
    *  _                 _       |       |
    * |                   |      | 1,4,7 |
    * | 1,2,3,4,5,6,7,8,9 | ---->| 2,5,8 |
    * |_                 _|      | 3,6,9 |
    *                            |_     _|
    */
    for (i = 0; i < s32MatrixRow; i++)
    {
        for (j = 0; j < s32MatrixColum; j++)
        {
            pps32SincTablePtr[i][j] = pstNode->table[i + j * s32MatrixRow];
        }
    }
#endif
    /*
    * 3 ���㱾����Ҫ��������ݵĳ���,�����������������
    *    ___________________________________________________________
    *   |                     |              |                      |
    *   |    ��һ����������   |   ԭʼ����   |   ���һ����������   |
    *   |_____________________|______________|______________________|
    *   ����:��һ�����ݵ����䳤��Ϊ:�˲����ķŴ���(pstNode->multipule)*�˲������ڴ�С(SINC_FILTER_STEP_NUM)/2
    *        ԭʼ���ݵĳ���Ϊ:��������ݽ�����ַ(s32EndIndex)-��������ݵ���ʼ��ַ(s32StartIndex)+1
    *        ���һ�����ݵ����䳤��Ϊ:�˲����ķŴ���(pstNode->multipule)*�˲������ڴ�С(SINC_FILTER_STEP_NUM)/2
    *   ���Ա�����������������ܳ���Ϊ:��������ݽ�����ַ(s32EndIndex)-��������ݵ���ʼ��ַ(s32StartIndex)+1 +
    *                                  �˲����ķŴ���(pstNode->multipule)*�˲������ڴ�С(SINC_FILTER_STEP_NUM)
    */
    s32FrontExpEndPos = pstNode->multipule * SINC_FILTER_STEP_NUM / 2;
    s32RearExpStartPos = s32EndIndex - s32StartIndex + 1 + pstNode->multipule * SINC_FILTER_STEP_NUM / 2;
    s32InDataLen = s32EndIndex - s32StartIndex + 1 + pstNode->multipule * SINC_FILTER_STEP_NUM;

    /*
    * 4 ������ʱ�洢�ڲ���ֽ���Ķ�ά����
    *   �˶�ά������д�СΪ��s32MatrixRow
    *               �д�СΪ��s32ResultAfterInterColum = s32InDataLen + �˲������ڴ�С - 1
    *   �˶�ά������ܳ���Ϊ: s32MatrixRow * s32ResultAfterInterColum
    */
    // 4.1 �����ά������г���
    s32ResultAfterInterColum = (s32InDataLen + SINC_FILTER_STEP_NUM - 1);
#if MEMORY_USE_OPTIMIZE_SWITCH == 0
    // 4.2 ����洢��ʱ���ݵ��ڴ档ע�⣺���ڴ���ʹ����ɺ�,�����ͷ�
    ps32TempAfterInter = (int*)malloc(s32MatrixRow * s32ResultAfterInterColum * sizeof(int));

    // 4.3 �����ά������ָ��Ŀռ䡣ע�⣺���ڴ���ʹ����ɺ�,�����ͷ�
    pps32ResultAfterInter = (int**)malloc(s32MatrixRow * sizeof(int*));

    // 4.4 ���ж�ά������е�ַ����
    for (i = 0; i < s32MatrixRow; i++)
    {
        pps32ResultAfterInter[i] = ps32TempAfterInter + i * s32ResultAfterInterColum;
    }
#else
    // 4.5 ���ݼ�������к���
    // 4.5 ����һά����Ŀռ�,�ռ��С = s32MatrixRow * s32ResultAfterInterColum * sizeof(double)
    pf64TempWaveData = (double*)malloc(s32MatrixRow * s32ResultAfterInterColum * sizeof(double));
#endif
    /*
    * 5. ��ʼ���������
    *    �˴�������õ��Ƕ�ά����ĸ�ֵ
    *    �˶�ά�������Ϊ��s32MatrixRow
    *    �˶�ά�������Ϊ��s32ResultAfterInterColum
    */
    for (row = 0; row < s32MatrixRow; row++)
    {
        for (colum = 0; colum < s32ResultAfterInterColum; colum++)
        {
            // 5.1 ����Sinc�˲������ڵ�ƽ��
            for (j = 0; j < (s32MatrixColum - 1); j++)
            {
                s32TempFir[s32MatrixColum - 1 - j] = s32TempFir[s32MatrixColum - 2 - j];
            }

            // 5.2 ��������Sinc�˲�����������,������Ч���ݳ��Ⱥ�,������0
            if (colum < s32InDataLen)
            {
                /*
                * 5.2.1 ���������˲������ݵ�ɸѡ
                * ɸѡ����Ϊ��(1).����ֵ����ֵ����*SINC_FILTER_STEP_NUM/2ʱ,���õ�һ�����������
                *             (2).����ֵ��ԭʼ���ݵ���+��ֵ����*SINC_FILTER_STEP_NUM/2ʱ���������һ�����������
                *             (3).��������ֵ˳����ȡԭʼ����
                */
                if (colum < s32FrontExpEndPos)
                {
                    s32TempFir[0] = (int)(pu8DataBuf[s32StartIndex]);
                }
                else if ((colum >= s32FrontExpEndPos) && (colum < s32RearExpStartPos))
                {
                    s32TempFir[0] = (int)(pu8DataBuf[s32StartIndex + colum - s32FrontExpEndPos]);
                }
                else
                {
                    s32TempFir[0] = (int)(pu8DataBuf[s32EndIndex]);
                }
            }
            else
            {
                s32TempFir[0] = 0;
            }
            // 5.3 ��¼ÿ�����ڵĻ���ֵ
            for (k = 0; k < s32MatrixColum; k++)
            {
#if MEMORY_USE_OPTIMIZE_SWITCH == 0
                s32IntegralVal += s32TempFir[k] * pps32SincTablePtr[row][k];
#else
                s32IntegralVal += s32TempFir[k] * pstNode->table[row + k * s32MatrixRow];
#endif
            }
#if MEMORY_USE_OPTIMIZE_SWITCH == 0
            pps32ResultAfterInter[row][colum] = s32IntegralVal;
#else
            pf64TempWaveData[row + colum * s32MatrixRow] = (double)s32IntegralVal / pstNode->coe;
#endif
            s32IntegralVal = 0;
        }
    }
#if MEMORY_USE_OPTIMIZE_SWITCH == 0
    /*
    * 6 �ѻ��ֽ���Ķ�ά����ת��Ϊһά����
    *   ת������Ϊ��
    *   _     _
    *  |       |        _                 _
    *  | 1,2,3 |       |                   |
    *  | 4,5,6 | ----> | 1,4,7,2,5,8,3,6,9 |
    *  | 7,8,9 |       |_                 _|
    *  |_     _|
    */
    for (i = 0; i < s32ResultAfterInterColum; i++)
    {
        for (j = 0; j < s32MatrixRow; j++)
        {
            pf64TempWaveData[j + i * s32MatrixRow] = (double)pps32ResultAfterInter[j][i] / pstNode->coe;
        }
    }
#endif
    /*
    * 7 ������Ч���ݵĳ�ȡ
    *   ���������˲���ʱ,�������׶κͺ�˵��������,�����������ݸ���ʱ,�����Ч����
    */
    // 7.1 �����ȡ���ݵ���ʵλ��
    s32WaveDataCopyStartIndex = pstNode->size / 2 + s32CompStartIndex;
    // 7.2 ��ʼ���ݵĳ�ȡ
    waveCompAlgorithm4Double(&pf64TempWaveData[s32WaveDataCopyStartIndex], 0, s32CompDataLen, pf64OutBuf, s32OutBufLen);
    // 7.2 ��ʼ���ݵĸ���
    //memcpy((char*)pf64OutBuf,(char*)&pf64TempWaveData[s32WaveDataCopyStartIndex],s32OutBufLen*sizeof(double));
    // 8 �����ڴ���ͷ�
#if MEMORY_USE_OPTIMIZE_SWITCH == 0
    free(ps32SincTableData);
    free(pps32SincTablePtr);
    free(ps32TempAfterInter);
    free(pps32ResultAfterInter);
#endif
    free(pf64TempWaveData);

    return 0;
}


void  SincFilterThread(void* pInitPara)
{
    SINC_FILTER_PARA_STRU* pstSincPara = (SINC_FILTER_PARA_STRU*)pInitPara;

    LARGE_INTEGER  tick;
    LARGE_INTEGER timestamp;
    QueryPerformanceFrequency(&tick);
    QueryPerformanceCounter(&timestamp);

    pstSincPara->stime = (timestamp.QuadPart % tick.QuadPart) * 1E6 / tick.QuadPart;

    getSincFilterData(pstSincPara->src,
        pstSincPara->start,
        pstSincPara->end,
        pstSincPara->xn,
        pstSincPara->cstart,
        pstSincPara->clen,
        pstSincPara->dst,
        pstSincPara->dsize);

    ReleaseSemaphore(*pstSincPara->sync, 1, NULL);
    QueryPerformanceCounter(&timestamp);
    pstSincPara->etime = (timestamp.QuadPart % tick.QuadPart) * 1E6 / tick.QuadPart;
}

/*******************************************************************************
* ��    ����SincFilteProcess
* ��    ������ԭʼ���ݽ���Sinc�ڲ��˲�����
* ���������
*             ��������            ��������                ����˵��
*             pu8DataBuf          unsigned char *         ����Sinc�ڴ��˲���������
*                                                         ������Ϊ�޷��ŵ��ֽ�����
*             s32StartIndex       int                     Sinc�ڲ��˲�����ʼȡ����
*                                                         ʱ,�������е�λ��
*             s32EndIndex         int                     Sinc�ڲ��˲�������ȡ����
*                                                         ʱ,�������е�λ��
*             s32MutipuleXn       int			            Sinc�ڲ��˲���ʹ�õ��ڲ�
*                                                         ��
*             s32CompStartIndex   int                     �ڲ���ɺ�������������
*                                                         ģ��ʱ��������ʼ����
*             s32CompDataLen      int                     �ڲ���ɺ�������������
*                                                         ģ��ʱ�������ܳ�
*             s32OutBufLen        int                     �����Sinc�ڲ��˲������
*                                                         ��Ĵ�С
* ���������
*             ��������            ��������                ����˵��
*             pf64OutBuf          double*                 �����Sinc�ڲ��˲������
*                                                         ����
* �� �� ֵ����
* ˵    ������
******************************************************************************/
void SincFilteProcess(unsigned char* pu8DataBuf,
    int              s32StartIndex,
    int              s32EndIndex,
    int		       s32MutipuleXn,
    int              s32CompStartIndex,
    int              s32CompDataLen,
    double* pf64OutBuf,
    int              s32OutBufLen)
{
    LARGE_INTEGER  tick;
    LARGE_INTEGER timestamp;
    QueryPerformanceFrequency(&tick);
    QueryPerformanceCounter(&timestamp);

    m_astSincFilterPara[m_s32CurrentSincThreadNum].ctime = (timestamp.QuadPart % tick.QuadPart) * 1E6 / tick.QuadPart;

    m_astSincFilterPara[m_s32CurrentSincThreadNum].src = pu8DataBuf;
    m_astSincFilterPara[m_s32CurrentSincThreadNum].start = s32StartIndex;
    m_astSincFilterPara[m_s32CurrentSincThreadNum].end = s32EndIndex;
    m_astSincFilterPara[m_s32CurrentSincThreadNum].xn = s32MutipuleXn;
    m_astSincFilterPara[m_s32CurrentSincThreadNum].cstart = s32CompStartIndex;
    m_astSincFilterPara[m_s32CurrentSincThreadNum].clen = s32CompDataLen;
    m_astSincFilterPara[m_s32CurrentSincThreadNum].dst = pf64OutBuf;
    m_astSincFilterPara[m_s32CurrentSincThreadNum].dsize = s32OutBufLen;
    m_astSincFilterPara[m_s32CurrentSincThreadNum].sync = &m_asemSincFilterThreadSync[m_s32CurrentSincThreadNum];
    _beginthread(SincFilterThread, 1024, &m_astSincFilterPara[m_s32CurrentSincThreadNum]);
    m_s32CurrentSincThreadNum++;
}

/*******************************************************************************
* ��    ����SincFilterWaitForFinish
* ��    �����ȴ������ڲ��̴߳������
* �����������
* �����������
* �� �� ֵ����
* ˵    ������
******************************************************************************/
void SincFilterWaitForFinish()
{
    SyncWaitForSincFilterSync(m_asemSincFilterThreadSync, m_s32CurrentSincThreadNum);
    m_s32CurrentSincThreadNum = 0;
}

int getSincFilterDataAddOffset(unsigned char* pu8DataBuf,
    int              s32StartIndex,
    int              s32EndIndex,
    int		        s32MutipuleXn,
    double* pf64OutBuf,
    int			    s32OutBufOffset,
    int              s32OutBufLen)
{
    struct stSincTable* pstNode = NULL;    //  ����Sinc�ڲ屶��ö��ֵ,����Sinc��ֵ��Ϣ��Ĳ���
    int* ps32SincTableData = NULL;
    int** pps32SincTablePtr = NULL;
    int                     s32InDataLen = 0;        //  
    int                     s32FrontExpEndPos = 0;
    int                     s32RearExpStartPos = 0;
    int                     s32TempFir[SINC_FILTER_STEP_NUM] = { 0 };    //  ��¼�˲������ڵ�ϵ��
    int                     s32MatrixRow = 0;
    int                     s32MatrixColum = 0;
    int                     s32IntegralVal = 0;
    int* ps32TempAfterInter = NULL;
    int** pps32ResultAfterInter = NULL;
    int                     s32ResultAfterInterColum = 0;
    double* pf64TempWaveData = NULL;
    int                     s32WaveDataCopyStartIndex = 0;
    int                     i, j, k, row, colum;

    // 1 ��ȡSinc�ڲ��˲�����
    pstNode = getSincInterTable(s32MutipuleXn);

    // 2. �ж�Sinc����Ч��
    if (pstNode == (struct stSincTable*)0)
    {
        return -1;
    }
    /*
    * 3. ����Sinc�˲���ϵ����һάת��ά
    */
    // 3.1 ���ж�ά������к��еĸ�ֵ
    s32MatrixRow = pstNode->multipule;
    s32MatrixColum = SINC_FILTER_STEP_NUM;

    // 3.2 �������ݵ��ڴ�����,ʹ����ɺ�,�����ͷ�
    ps32SincTableData = (int*)malloc(s32MatrixRow * s32MatrixColum * sizeof(int));

    // 3.3 ���ж�ά������е�ַ�洢���ڴ�����,ʹ����ɺ�,�����ͷ�
    pps32SincTablePtr = (int**)malloc(s32MatrixRow * sizeof(int*));

    // 3.4 ���ж�ά�����ַ�ķ���
    for (i = 0; i < s32MatrixRow; i++)
    {
        pps32SincTablePtr[i] = ps32SincTableData + i * s32MatrixColum;
    }

    /*
    * 3.5 ����һά���鵽��ά�����ת��
    * ת������Ϊ
    *                             _     _
    *  _                 _       |       |
    * |                   |      | 1,4,7 |
    * | 1,2,3,4,5,6,7,8,9 | ---->| 2,5,8 |
    * |_                 _|      | 3,6,9 |
    *                            |_     _|
    */
    for (i = 0; i < s32MatrixRow; i++)
    {
        for (j = 0; j < s32MatrixColum; j++)
        {
            pps32SincTablePtr[i][j] = pstNode->table[i + j * s32MatrixRow];
        }
    }

    /*
    * 3 ���㱾����Ҫ��������ݵĳ���,�����������������
    *    ___________________________________________________________
    *   |                     |              |                      |
    *   |    ��һ����������   |   ԭʼ����   |   ���һ����������   |
    *   |_____________________|______________|______________________|
    *   ����:��һ�����ݵ����䳤��Ϊ:�˲����ķŴ���(pstNode->multipule)*�˲������ڴ�С(SINC_FILTER_STEP_NUM)/2
    *        ԭʼ���ݵĳ���Ϊ:��������ݽ�����ַ(s32EndIndex)-��������ݵ���ʼ��ַ(s32StartIndex)+1
    *        ���һ�����ݵ����䳤��Ϊ:�˲����ķŴ���(pstNode->multipule)*�˲������ڴ�С(SINC_FILTER_STEP_NUM)/2
    *   ���Ա�����������������ܳ���Ϊ:��������ݽ�����ַ(s32EndIndex)-��������ݵ���ʼ��ַ(s32StartIndex)+1 +
    *                                  �˲����ķŴ���(pstNode->multipule)*�˲������ڴ�С(SINC_FILTER_STEP_NUM)
    */
    s32FrontExpEndPos = pstNode->multipule * SINC_FILTER_STEP_NUM / 2;
    s32RearExpStartPos = s32EndIndex - s32StartIndex + 1 + pstNode->multipule * SINC_FILTER_STEP_NUM / 2;
    s32InDataLen = s32EndIndex - s32StartIndex + 1 + pstNode->multipule * SINC_FILTER_STEP_NUM;

    /*
    * 4 ������ʱ�洢�ڲ���ֽ���Ķ�ά����
    *   �˶�ά������д�СΪ��s32MatrixRow
    *               �д�СΪ��s32ResultAfterInterColum = s32InDataLen + �˲������ڴ�С - 1
    *   �˶�ά������ܳ���Ϊ: s32MatrixRow * s32ResultAfterInterColum
    */
    // 4.1 �����ά������г���
    s32ResultAfterInterColum = (s32InDataLen + SINC_FILTER_STEP_NUM - 1);

    // 4.2 ����洢��ʱ���ݵ��ڴ档ע�⣺���ڴ���ʹ����ɺ�,�����ͷ�
    ps32TempAfterInter = (int*)malloc(s32MatrixRow * s32ResultAfterInterColum * sizeof(int));

    // 4.3 �����ά������ָ��Ŀռ䡣ע�⣺���ڴ���ʹ����ɺ�,�����ͷ�
    pps32ResultAfterInter = (int**)malloc(s32MatrixRow * sizeof(int*));

    // 4.4 ���ж�ά������е�ַ����
    for (i = 0; i < s32MatrixRow; i++)
    {
        pps32ResultAfterInter[i] = ps32TempAfterInter + i * s32ResultAfterInterColum;
    }

    /*
    * 5. ��ʼ���������
    *    �˴�������õ��Ƕ�ά����ĸ�ֵ
    *    �˶�ά�������Ϊ��s32MatrixRow
    *    �˶�ά�������Ϊ��s32ResultAfterInterColum
    */
    //������ڵ�ǰ��
    if (s32EndIndex > m_chanMemPoints)
    {
        //���뿽�����ݿռ�
        unsigned char* pu8WavDataBuff = (unsigned char*)malloc(sizeof(unsigned char) * s32EndIndex);
        // �ȸ���ԭʼ����
        memcpy((char*)pu8WavDataBuff, (char*)pu8DataBuf, sizeof(unsigned char) * m_chanMemPoints);
        // ��cpyȱ�ٵĵ�
        for (int i = m_chanMemPoints; i < s32EndIndex; i++)
        {
            pu8WavDataBuff[i] = pu8DataBuf[m_chanMemPoints - 1];
        }
        for (row = 0; row < s32MatrixRow; row++)
        {
            for (colum = 0; colum < s32ResultAfterInterColum; colum++)
            {
                // 5.1 ����Sinc�˲������ڵ�ƽ��
                for (j = 0; j < (s32MatrixColum - 1); j++)
                {
                    s32TempFir[s32MatrixColum - 1 - j] = s32TempFir[s32MatrixColum - 2 - j];
                }

                // 5.2 ��������Sinc�˲�����������,������Ч���ݳ��Ⱥ�,������0
                if (colum < s32InDataLen)
                {
                    /*
                    * 5.2.1 ���������˲������ݵ�ɸѡ
                    * ɸѡ����Ϊ��(1).����ֵ����ֵ����*SINC_FILTER_STEP_NUM/2ʱ,���õ�һ�����������
                    *             (2).����ֵ��ԭʼ���ݵ���+��ֵ����*SINC_FILTER_STEP_NUM/2ʱ���������һ�����������
                    *             (3).��������ֵ˳����ȡԭʼ����
                    */
                    if (colum < s32FrontExpEndPos)
                    {
                        s32TempFir[0] = (int)(pu8WavDataBuff[s32StartIndex]);
                    }
                    else if ((colum >= s32FrontExpEndPos) && (colum < s32RearExpStartPos))
                    {
                        s32TempFir[0] = (int)(pu8WavDataBuff[s32StartIndex + colum - s32FrontExpEndPos]);
                    }
                    else
                    {
                        s32TempFir[0] = (int)(pu8WavDataBuff[s32EndIndex]);
                    }
                }
                else
                {
                    s32TempFir[0] = 0;
                }
                // 5.3 ��¼ÿ�����ڵĻ���ֵ
                for (k = 0; k < s32MatrixColum; k++)
                {
                    s32IntegralVal += s32TempFir[k] * pps32SincTablePtr[row][k];
                }
                pps32ResultAfterInter[row][colum] = s32IntegralVal;
                s32IntegralVal = 0;
            }
        }
        free(pu8WavDataBuff);
    }
    else
    {
        for (row = 0; row < s32MatrixRow; row++)
        {
            for (colum = 0; colum < s32ResultAfterInterColum; colum++)
            {
                // 5.1 ����Sinc�˲������ڵ�ƽ��
                for (j = 0; j < (s32MatrixColum - 1); j++)
                {
                    s32TempFir[s32MatrixColum - 1 - j] = s32TempFir[s32MatrixColum - 2 - j];
                }

                // 5.2 ��������Sinc�˲�����������,������Ч���ݳ��Ⱥ�,������0
                if (colum < s32InDataLen)
                {
                    /*
                    * 5.2.1 ���������˲������ݵ�ɸѡ
                    * ɸѡ����Ϊ��(1).����ֵ����ֵ����*SINC_FILTER_STEP_NUM/2ʱ,���õ�һ�����������
                    *             (2).����ֵ��ԭʼ���ݵ���+��ֵ����*SINC_FILTER_STEP_NUM/2ʱ���������һ�����������
                    *             (3).��������ֵ˳����ȡԭʼ����
                    */
                    if (colum < s32FrontExpEndPos)
                    {
                        s32TempFir[0] = (int)(pu8DataBuf[s32StartIndex]);
                    }
                    else if ((colum >= s32FrontExpEndPos) && (colum < s32RearExpStartPos))
                    {
                        s32TempFir[0] = (int)(pu8DataBuf[s32StartIndex + colum - s32FrontExpEndPos]);
                    }
                    else
                    {
                        s32TempFir[0] = (int)(pu8DataBuf[s32EndIndex]);
                    }
                }
                else
                {
                    s32TempFir[0] = 0;
                }
                // 5.3 ��¼ÿ�����ڵĻ���ֵ
                for (k = 0; k < s32MatrixColum; k++)
                {
                    s32IntegralVal += s32TempFir[k] * pps32SincTablePtr[row][k];
                }
                pps32ResultAfterInter[row][colum] = s32IntegralVal;
                s32IntegralVal = 0;
            }
        }
    }

    //for( row = 0 ; row < s32MatrixRow ; row ++ )
    //{
    //    for( colum = 0 ; colum < s32ResultAfterInterColum ; colum ++ )
    //    {
    //        // 5.1 ����Sinc�˲������ڵ�ƽ��
    //        for ( j = 0; j < (s32MatrixColum - 1); j++)
    //        {
    //            s32TempFir[s32MatrixColum-1-j] = s32TempFir[s32MatrixColum-2-j];
    //        }

    //        // 5.2 ��������Sinc�˲�����������,������Ч���ݳ��Ⱥ�,������0
    //        if( colum < s32InDataLen )
    //        {
    //            /*
    //            * 5.2.1 ���������˲������ݵ�ɸѡ
    //            * ɸѡ����Ϊ��(1).����ֵ����ֵ����*SINC_FILTER_STEP_NUM/2ʱ,���õ�һ�����������
    //            *             (2).����ֵ��ԭʼ���ݵ���+��ֵ����*SINC_FILTER_STEP_NUM/2ʱ���������һ�����������
    //            *             (3).��������ֵ˳����ȡԭʼ����
    //            */
    //            if( colum < s32FrontExpEndPos )
    //            {
    //                s32TempFir[0] = (int)(pu8DataBuf[s32StartIndex]);
    //            }
    //            else if( ( colum >= s32FrontExpEndPos ) && ( colum < s32RearExpStartPos ))
    //            {
    //                s32TempFir[0] = (int)(pu8DataBuf[s32StartIndex+colum-s32FrontExpEndPos]);
    //            }
    //            else
    //            {
    //                s32TempFir[0] = (int)(pu8DataBuf[s32EndIndex]);
    //            }
    //        }
    //        else
    //        {
    //            s32TempFir[0] = 0;
    //        }
    //        // 5.3 ��¼ÿ�����ڵĻ���ֵ
    //        for( k = 0 ; k < s32MatrixColum ; k ++ )
    //        {
    //            s32IntegralVal += s32TempFir[k] * pps32SincTablePtr[row][k];
    //        }
    //        pps32ResultAfterInter[row][colum] = s32IntegralVal;
    //        s32IntegralVal = 0;
    //    }
    //}

    /*
    * 6 �ѻ��ֽ���Ķ�ά����ת��Ϊһά����
    *   ת������Ϊ��
    *   _     _
    *  |       |        _                 _
    *  | 1,2,3 |       |                   |
    *  | 4,5,6 | ----> | 1,4,7,2,5,8,3,6,9 |
    *  | 7,8,9 |       |_                 _|
    *  |_     _|
    */
    // 6.1 ����һά����Ŀռ�,�ռ��С = s32MatrixRow * s32ResultAfterInterColum * sizeof(double)
    pf64TempWaveData = (double*)malloc(s32MatrixRow * s32ResultAfterInterColum * sizeof(double));
    // 6.2 ��ʼ���ж�ά���ݵ�һά��ת��
    for (i = 0; i < s32ResultAfterInterColum; i++)
    {
        for (j = 0; j < s32MatrixRow; j++)
        {
            pf64TempWaveData[j + i * s32MatrixRow] = (double)pps32ResultAfterInter[j][i] / pstNode->coe;
        }
    }
    /*
    * 7 ������Ч���ݵ����
    *   ���������˲���ʱ,�������׶κͺ�˵��������,�����������ݸ���ʱ,�����Ч����
    */
    // 7.1 ���㸴�����ݵ�λ��
    s32WaveDataCopyStartIndex = pstNode->size / 2 + s32OutBufOffset;
    // 7.2 ��ʼ���ݵĸ���
    for (i = s32WaveDataCopyStartIndex; i < s32WaveDataCopyStartIndex + s32OutBufLen; i++)
    {
        pf64OutBuf[i - s32WaveDataCopyStartIndex] = pf64TempWaveData[i];
    }

    // 8 �����ڴ���ͷ�
    free(ps32SincTableData);
    free(pps32SincTablePtr);
    free(ps32TempAfterInter);
    free(pps32ResultAfterInter);
    free(pf64TempWaveData);

    return 0;
}
