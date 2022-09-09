/********************************************************************
普源精电科技股份有限公司 版权所有(2020 - 2030)
*********************************************************************
头文件名: inter_and_sample.cpp
功能描述: 处理峰峰值抽样，Sinc插值及滤波
作    者: sn01625
版    本: 0.1
创建日期: 2020-06-28  15:10 PM

修改记录1：// 修改历史记录，包括修改日期、修改者及修改内容
修改日期：
版 本 号：
修 改 人：
修改内容：
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
宏定义声明
*********************************************************************/
#define                 MEMORY_USE_OPTIMIZE_SWITCH  1   // 内存使用优化开关,0标识关闭优化,1标识打开优化
#define                 SINC_FILTER_STEP_NUM        8   // Sinc滤波器阶数
#define                 SINC_FILTER_THREAD_NUM      512

/********************************************************************
枚举定义声明
*********************************************************************/

/********************************************************************
结构体定义声明
*********************************************************************/

/*
* 定义无符号单字节数组的峰峰值搜索信息的结构体
* 包含:1.最大值
*      2.最小值
*      3.最大值在本次进行搜索数组的位置
*      4.最小值在本次进行搜索数组的位置
*/
typedef struct stValInfo4UChar
{
    unsigned char   max;        //  最大值
    unsigned char   min;        //  最小值
    short           resv;       //  保留字节,用于对齐
    unsigned int    max_pos;    //  最大值的索引位置
    unsigned int    min_pos;    //  最小值的索引位置
}PPEAK_VALUE_INFO_FOR_CAHR;

/*
* 定义双精度浮点数组的峰峰值搜索信息的结构体
* 包含:1.最大值
*      2.最小值
*      3.最大值在本次进行搜索数组的位置
*      4.最小值在本次进行搜索数组的位置
*/
typedef struct stValInfo4Double
{
    unsigned int    max_pos;    //  最大值的索引位置  
    unsigned int    min_pos;    //  最小值的索引位置
    double          max;        //  最大值
    double          min;        //  最小值
}PPEAK_VALUE_INFO_FOR_DOUBLE;

/*
* 定义Sinc内插滤波器的系数表信息的结构体
* 包含:1.内插系数的枚举值,见枚举值euInterMutipule定义
*      2.系数表存储的地址,由导入时进行申请
*      3.系数表大小
*      4.内插倍数
*      5.归一化系数
*      6.指向下一个内插系数表的地址
*/
typedef struct stSincTable
{
    int					xn;         //  内插系数
    int* table;      //  内插系数表
    int                 size;       //  内插系数表的大小
    int                 multipule;  //  内插倍数
    double              coe;        //  归一化系数
    struct stSincTable* next;       //  下一张系数表的位置
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
本地变量定义声明
*********************************************************************/
SINC_TABLE_STRU* m_pstSincTable = NULL;  // 记录Sinc内插系数表的链表
SINC_FILTER_PARA_STRU       m_astSincFilterPara[SINC_FILTER_THREAD_NUM];
HANDLE                      m_asemSincFilterThreadSync[SINC_FILTER_THREAD_NUM];
int                         m_s32CurrentSincThreadNum = 0;
int                         m_chanMemPoints = 1000008;//记录单个通道保存的点数
/********************************************************************
内部函数定义
*********************************************************************/

/*******************************************************************************
* 函    数：findMaxAndMinVal4UChar
* 描    述：搜索输入的无符号单字节整型数组的峰峰值信息并输出
* 输入参数：
*             参数名称            参数类型                参数说明
*             pu8DataBuff         unsigned char*          无符号单字节整型数组
*             u32DataLength       unsigned int            无符号单字节整型数组的长度
* 输出参数：
*             参数名称            参数类型                参数说明
*             pstInfo      PPEAK_VALUE_INFO_FOR_CAHR*     峰峰值信息表
* 返 回 值：无
* 说    明：无
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


    // 开始查找最大最小值
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

    // 更新记录信息
    pstInfo->max_pos = u32MaxPos;
    pstInfo->min_pos = u32MinPos;
    pstInfo->max = u8MaxValue;
    pstInfo->min = u8MinValue;
}

/*******************************************************************************
* 函    数：findMaxAndMinVal4Double
* 描    述：搜索输入的双精度浮点型数组的峰峰值信息并输出
* 输入参数：
*             参数名称            参数类型                参数说明
*             pf64DataBuff        double*                 双精度浮点型数组
*             u32DataLength       unsigned int            双精度浮点型数组的长度
* 输出参数：
*             参数名称            参数类型                参数说明
*             pstInfo      PPEAK_VALUE_INFO_FOR_CAHR*     峰峰值信息表
* 返 回 值：无
* 说    明：无
******************************************************************************/
void findMaxAndMinVal4Double(double* pf64DataBuff, unsigned int u32DataLength, PPEAK_VALUE_INFO_FOR_DOUBLE* pstInfo)
{
    unsigned int   u32MaxPos = 0;
    unsigned int   u32MinPos = 0;
    unsigned int   i = 0;
    double         f64MaxValue = pf64DataBuff[0];
    double         f64MinValue = pf64DataBuff[0];


    // 开始查找最大最小值
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

    // 更新记录信息
    pstInfo->max_pos = u32MaxPos;
    pstInfo->min_pos = u32MinPos;
    pstInfo->max = f64MaxValue;
    pstInfo->min = f64MinValue;
}

/*******************************************************************************
* 函    数：getSincInterTable
* 描    述：根据Sinc滤波器系数,搜索已加载的内插系数表
* 输入参数：
*             参数名称            参数类型                参数说明
*             emMutipule          euInterMutipule         Sinc内插滤波器系数
* 输出参数：无
* 返 回 值：搜索到则返回,未搜索到则返回NULL
* 说    明：无
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
* 函    数：SyncWaitForRecvFinish(内部接口)
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
接口函数定义
*********************************************************************/



/*******************************************************************************
* 函    数：waveCompAlgorithm4UChar
* 描    述：对输入的无符号单字节整型数组进行峰峰值抽样
* 输入参数：
*             参数名称            参数类型                参数说明
*             pu8SrcBuff          unsigned char*          无符号单字节整型数组
*             s32Offset           int                     数组开始压缩的起始索引值
*             s32SrcLength        int                     无符号单字节整型数组长度
*             s32DstLength        int                     准备抽样的数据点数
* 输出参数：
*             参数名称            参数类型                参数说明
*             pu8DstBuff          unsigned char *         抽样结果
* 返 回 值：无
* 说    明：无
******************************************************************************/
void waveCompAlgorithm4UChar(unsigned char* pu8SrcBuff,
    int s32Offset,
    int s32SrcLength,
    unsigned char* pu8DstBuff,
    int s32DstLength)
{
    unsigned int                u32MaxPos = 0;
    unsigned int                u32MinPos = 0;
    unsigned int                u32CompressNumTemp = s32DstLength * 2;//准备压缩的数据长度
    unsigned int                u32CurrCompPos = 0;
    unsigned int                u32CurrCompLength = 0;
    int	                        i = 0;
    int                         j = 0;
    int                         k = 0;
    unsigned int                u32Multiple = (unsigned int)floor((float)s32SrcLength / (float)s32DstLength); // 压缩倍率
    unsigned char* pu8TempBuff = (unsigned char*)0;
    unsigned char               au8Dot[4] = { 0 }; // 判定只有4个点时,进行压缩
    PPEAK_VALUE_INFO_FOR_CAHR   stFindInfo = { 0 };

    // 1 申请临时数据压缩缓冲区,注意使用完成后进行释放,这里s32DstLength * 2是由于缓存了每次分段抽值的最大值及最小值
    pu8TempBuff = (unsigned char*)malloc(sizeof(unsigned char) * u32CompressNumTemp);

    // 2 开始数据压缩
    for (; i < s32DstLength; i++)
    {
        // 2.1 计算本次抽样的在整包数据的位置及送入抽样中的数据长度,这里要对剩余不足一段的数据进行单独处理
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

        // 2.2 查找分段数据中的最大值和最小值
        findMaxAndMinVal4UChar(&pu8SrcBuff[u32CurrCompPos + s32Offset], u32CurrCompLength, &stFindInfo);

        // 2.3 根据查找到的最大值及最小值信息,计算在整包数据中的位置
        u32MaxPos = u32CurrCompPos + stFindInfo.max_pos + s32Offset;
        u32MinPos = u32CurrCompPos + stFindInfo.min_pos + s32Offset;

        // 2.4 根据位置信息,记录数据值,这里考虑数据点的分布,上升沿模式(最大值位置大于最小值位置),下降沿模式(最大值位置小于等于最小值位置)
        if (u32MaxPos > u32MinPos)// 上升沿模式
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

    // 3 开始根据峰峰值进行数据抽样
    // 假如有4个数据在pu8TempBuff中，pu8TempBuff[0] < pu8TempBuff[1], pu8TempBuff[2] < pu8TempBuff[3]
    // 在这4个数据中需要选择2点作为最终输出波形数据，重点是比较pu8TempBuff[1]和pu8TempBuff[2]是否是极值
    // 若无极值 pu8TempBuff[0] < pu8TempBuff[1] < pu8TempBuff[2] < pu8TempBuff[3],则取pu8DstBuff[j] = (pu8TempBuff[1] + pu8TempBuff[2]) / 2
    // 若都是极值 pu8TempBuff[2] < pu8TempBuff[3] < pu8TempBuff[0] < pu8TempBuff[1],则取pu8DstBuff[j] = pu8TempBuff[1],pu8TempBuff[i+2] = au8Dot[2]
    // 若只有一个极值 pu8TempBuff[0] < pu8TempBuff[2] < pu8TempBuff[3] < pu8TempBuff[1],则取pu8DstBuff[j] = pu8TempBuff[1]
    pu8DstBuff[0] = pu8TempBuff[0];// 记录判定初始值
    // 开始循环进行数据查找
    for (i = 1, j = 0; i < k; i += 2, j++)
    {
        // 3.1 取连续4个点进行判定
        au8Dot[0] = pu8DstBuff[j];
        au8Dot[1] = pu8TempBuff[i];
        au8Dot[2] = pu8TempBuff[i + 1];
        au8Dot[3] = pu8TempBuff[i + 2];

        // 3.2 搜素4个点的最大值及最小值位置
        findMaxAndMinVal4UChar(au8Dot, 4, &stFindInfo);

        // 3.3 记录最大值及最小值的位置
        u32MaxPos = stFindInfo.max_pos;
        u32MinPos = stFindInfo.min_pos;

        // 3.4 进行判定
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

        //溢出则跳出for循环
        if (i + 2 == k - 1)
        {
            break;
        }
    }

    // 4 释放使用的内存
    free(pu8TempBuff);
}
/*******************************************************************************
* 函    数：waveDataPreProcess4UChar
* 描    述：对原始数据进行预处理，对波形进行粗移，移动到一个内存采样点的误差
* 输入参数：
*             参数名称            参数类型                参数说明
*             pu8SrcBuff          unsigned char*          无符号单字节整型数组
*             s32Offset           int                     数组开始压缩的起始索引值
* 输出参数：
*             参数名称            参数类型                参数说明
*             pu8SrcBuff          unsigned char *         更新后的原始点
* 返 回 值：无
* 说    明：无
******************************************************************************/
void  origWaveDataPreProcess4UChar(unsigned char* pu8SrcBuff, int s32Offset)
{
    //数据预处理，超出的部分使用最后一个点进行拷贝
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
* 函    数：waveCompAlgorithm4UCharSimple
* 描    述：对输入的无符号单字节整型数组进行峰峰值抽样
* 输入参数：
*             参数名称            参数类型                参数说明
*             pu8SrcBuff          unsigned char*          无符号单字节整型数组
*             s32Offset           int                     数组开始压缩的起始索引值
*             s32SrcLength        int                     无符号单字节整型数组长度
*             s32DstLength        int                     准备抽样的数据点数
* 输出参数：
*             参数名称            参数类型                参数说明
*             pu8DstBuff          unsigned char *         抽样结果
*             s32OutPos           int*                    输出起始点和终止点的位置
* 返 回 值：无
* 说    明：无
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
    unsigned int                u32Multiple = (unsigned int)floor((float)s32SrcLength / (float)(s32DstLength / 2.0)); // 压缩倍率
    unsigned char* pu8TempBuff = (unsigned char*)0;
    unsigned int* pu32TempPos = (unsigned int*)0;
    unsigned char               au8Dot[4] = { 0 }; // 判定只有4个点时,进行压缩
    PPEAK_VALUE_INFO_FOR_CAHR   stFindInfo = { 0 };

    // 1 申请临时数据压缩缓冲区,注意使用完成后进行释放,这里s32DstLength * 2是由于缓存了每次分段抽值的最大值及最小值
    pu8TempBuff = (unsigned char*)malloc(sizeof(unsigned char) * u32CompressNumTemp);
    pu32TempPos = (unsigned int*)malloc(sizeof(int) * u32CompressNumTemp);
    //数据预处理
    if (s32Offset + s32SrcLength >= m_chanMemPoints)
    {
        //申请拷贝数据空间
        unsigned char* pu8WavDataBuff = (unsigned char*)malloc(sizeof(unsigned char) * (s32Offset + s32SrcLength));
        // 先复制原始数据
        memcpy((char*)pu8WavDataBuff, (char*)pu8SrcBuff, sizeof(unsigned char) * m_chanMemPoints);
        // 再cpy缺少的点
        for (int i = m_chanMemPoints; i < s32Offset + s32SrcLength; i++)
        {
            pu8WavDataBuff[i] = pu8SrcBuff[m_chanMemPoints - 1];
        }
        // 2 开始数据压缩,从规定的数据中取出1K，将原有数据平均分为500分，每一份中取出最大最小值构成压缩
        for (; i < s32DstLength / 2; i++)
        {
            // 2.1 计算本次抽样的在整包数据的位置及送入抽样中的数据长度,这里要对剩余不足一段的数据进行单独处理
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
            // 2.2 查找分段数据中的最大值和最小值
            findMaxAndMinVal4UChar(&pu8WavDataBuff[u32CurrCompPos + s32Offset], u32CurrCompLength, &stFindInfo);

            // 2.3 根据查找到的最大值及最小值信息,计算在整包数据中的位置
            u32MaxPos = u32CurrCompPos + stFindInfo.max_pos + s32Offset;
            u32MinPos = u32CurrCompPos + stFindInfo.min_pos + s32Offset;

            // 2.4 根据位置信息,记录数据值,这里考虑数据点的分布,上升沿模式(最大值位置大于最小值位置),下降沿模式(最大值位置小于等于最小值位置)
            if (u32MaxPos > u32MinPos)// 上升沿模式
            {
                pu8TempBuff[k] = pu8WavDataBuff[u32MinPos];
                pu8TempBuff[k + 1] = pu8WavDataBuff[u32MaxPos];
                pu32TempPos[k] = u32MinPos;
                pu32TempPos[k + 1] = u32MaxPos;
                // 第一个点
                if (i == 0)
                {
                    // 起始点位置
                    s32OutPos[0] = u32MinPos;
                }
                else if (i == s32DstLength / 2 - 1)
                {
                    //终止点
                    s32OutPos[1] = u32MaxPos;
                }
            }
            else
            {
                pu8TempBuff[k] = pu8WavDataBuff[u32MaxPos];
                pu8TempBuff[k + 1] = pu8WavDataBuff[u32MinPos];
                pu32TempPos[k] = u32MaxPos;
                pu32TempPos[k + 1] = u32MinPos;
                // 第一个点
                if (i == 0)
                {
                    // 起始点位置
                    s32OutPos[0] = u32MaxPos;
                }
                else if (i == s32DstLength / 2 - 1)
                {
                    //终止点
                    s32OutPos[1] = u32MinPos;
                }
            }
            k += 2;
        }
        free(pu8WavDataBuff);
    }
    else
    {
        // 2 开始数据压缩,从规定的数据中取出1K，将原有数据平均分为500分，每一份中取出最大最小值构成压缩
        for (; i < s32DstLength / 2; i++)
        {
            // 2.1 计算本次抽样的在整包数据的位置及送入抽样中的数据长度,这里要对剩余不足一段的数据进行单独处理
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
            // 2.2 查找分段数据中的最大值和最小值
            findMaxAndMinVal4UChar(&pu8SrcBuff[u32CurrCompPos + s32Offset], u32CurrCompLength, &stFindInfo);

            // 2.3 根据查找到的最大值及最小值信息,计算在整包数据中的位置
            u32MaxPos = u32CurrCompPos + stFindInfo.max_pos + s32Offset;
            u32MinPos = u32CurrCompPos + stFindInfo.min_pos + s32Offset;

            // 2.4 根据位置信息,记录数据值,这里考虑数据点的分布,上升沿模式(最大值位置大于最小值位置),下降沿模式(最大值位置小于等于最小值位置)
            if (u32MaxPos > u32MinPos)// 上升沿模式
            {
                pu8TempBuff[k] = pu8SrcBuff[u32MinPos];
                pu8TempBuff[k + 1] = pu8SrcBuff[u32MaxPos];
                pu32TempPos[k] = u32MinPos;
                pu32TempPos[k + 1] = u32MaxPos;
                // 第一个点
                if (i == 0)
                {
                    // 起始点位置
                    s32OutPos[0] = u32MinPos;
                }
                else if (i == s32DstLength / 2 - 1)
                {
                    //终止点
                    s32OutPos[1] = u32MaxPos;
                }
            }
            else
            {
                pu8TempBuff[k] = pu8SrcBuff[u32MaxPos];
                pu8TempBuff[k + 1] = pu8SrcBuff[u32MinPos];
                pu32TempPos[k] = u32MaxPos;
                pu32TempPos[k + 1] = u32MinPos;
                // 第一个点
                if (i == 0)
                {
                    // 起始点位置
                    s32OutPos[0] = u32MaxPos;
                }
                else if (i == s32DstLength / 2 - 1)
                {
                    //终止点
                    s32OutPos[1] = u32MinPos;
                }
            }
            k += 2;
        }
    }
    //  复制压缩结果到输出
    memcpy((char*)pu8DstBuff, (char*)pu8TempBuff, sizeof(unsigned char) * u32CompressNumTemp);

    // 4 释放使用的内存
    free(pu8TempBuff);
    free(pu32TempPos);
}

/*******************************************************************************
* 函    数：waveCompAlgorithm4Double
* 描    述：对输入的双精度浮点型数组进行峰峰值抽样
* 输入参数：
*             参数名称            参数类型                参数说明
*             pf64SrcBuff         double*                 双精度浮点型数组
*             s32Offset           int                     数组开始压缩的起始索引值
*             s32SrcLength        int                     双精度浮点型数组长度
*             s32DstLength        int                     准备抽样的数据点数
* 输出参数：
*             参数名称            参数类型                参数说明
*             pf64DstBuff         double *                抽样结果
* 返 回 值：无
* 说    明：无
******************************************************************************/
void waveCompAlgorithm4Double(double* pf64SrcBuff,
    int s32Offset,
    int s32SrcLength,
    double* pf64DstBuff,
    int s32DstLength)
{
    unsigned int                u32MaxPos = 0;
    unsigned int                u32MinPos = 0;
    unsigned int                u32CompressNumTemp = s32DstLength * 2;//准备压缩的数据长度
    unsigned int                u32CurrCompPos = 0;
    unsigned int                u32CurrCompLength = 0;
    int	                        i = 0;
    int                         j = 0;
    int                         k = 0;
    unsigned int                u32Multiple = (unsigned int)floor((float)s32SrcLength / (float)s32DstLength); // 压缩倍率
    double* pf64TempBuff = (double*)0;
    double                      af64Dot[4] = { 0 }; // 判定只有4个点时,进行压缩
    PPEAK_VALUE_INFO_FOR_DOUBLE stFindInfo = { 0 };

    // 1 申请临时数据压缩缓冲区,注意使用完成后进行释放,这里s32DstLength * 2是由于缓存了每次分段抽值的最大值及最小值
    pf64TempBuff = (double*)malloc(sizeof(double) * u32CompressNumTemp);

    // 2 开始数据压缩
    for (; i < s32DstLength; i++)
    {
        // 2.1 计算本次抽样的在整包数据的位置及送入抽样中的数据长度,这里要对剩余不足一段的数据进行单独处理
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

        // 2.2 查找分段数据中的最大值和最小值
        findMaxAndMinVal4Double(&pf64SrcBuff[u32CurrCompPos + s32Offset], u32CurrCompLength, &stFindInfo);

        // 2.3 根据查找到的最大值及最小值信息,计算在整包数据中的位置
        u32MaxPos = u32CurrCompPos + stFindInfo.max_pos + s32Offset;
        u32MinPos = u32CurrCompPos + stFindInfo.min_pos + s32Offset;

        // 2.4 根据位置信息,记录数据值,这里考虑数据点的分布,上升沿模式(最大值位置大于最小值位置),下降沿模式(最大值位置小于等于最小值位置)
        if (u32MaxPos > u32MinPos)// 上升沿模式
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

    // 3 开始根据峰峰值进行数据抽样
    // 假如有4个数据在pf64TempBuff中，pf64TempBuff[0] < pf64TempBuff[1], pf64TempBuff[2] < pf64TempBuff[3]
    // 在这4个数据中需要选择2点作为最终输出波形数据，重点是比较pf64TempBuff[1]和pf64TempBuff[2]是否是极值
    // 若无极值 pf64TempBuff[0] < pf64TempBuff[1] < pf64TempBuff[2] < pf64TempBuff[3],则取pf64DstBuff[j] = (pf64TempBuff[1] + pf64TempBuff[2]) / 2
    // 若都是极值 pf64TempBuff[2] < pf64TempBuff[3] < pf64TempBuff[0] < pf64TempBuff[1],则取pf64DstBuff[j] = pf64TempBuff[1],pf64TempBuff[i+2] = af64Dot[2]
    // 若只有一个极值 pf64TempBuff[0] < pf64TempBuff[2] < pf64TempBuff[3] < pf64TempBuff[1],则取pf64DstBuff[j] = pf64TempBuff[1]
    pf64DstBuff[0] = pf64TempBuff[0];// 记录判定初始值
    // 开始循环进行数据查找
    for (i = 1, j = 0; i < k; i += 2, j++)
    {
        // 3.1 取连续4个点进行判定
        af64Dot[0] = pf64DstBuff[j];
        af64Dot[1] = pf64TempBuff[i];
        af64Dot[2] = pf64TempBuff[i + 1];
        af64Dot[3] = pf64TempBuff[i + 2];

        // 3.2 搜素4个点的最大值及最小值位置
        findMaxAndMinVal4Double(af64Dot, 4, &stFindInfo);

        // 3.3 记录最大值及最小值的位置
        u32MaxPos = stFindInfo.max_pos;
        u32MinPos = stFindInfo.min_pos;

        // 3.4 进行判定
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

        //溢出则跳出for循环
        if (i + 2 == k - 1)
        {
            break;
        }
    }

    // 4 释放使用的内存
    free(pf64TempBuff);

}

/*******************************************************************************
* 函    数：waveCompAlgorithm4DoubleSimple
* 描    述：对输入的双精度浮点型数组进行峰峰值抽样
* 输入参数：
*             参数名称            参数类型                参数说明
*             pf64SrcBuff         double*                 双精度浮点型数组
*             s32Offset           int                     数组开始压缩的起始索引值
*             s32SrcLength        int                     双精度浮点型数组长度
*             s32DstLength        int                     准备抽样的数据点数
* 输出参数：
*             参数名称            参数类型                参数说明
*             pf64DstBuff         double *                抽样结果
*             s32OutPos           int*                    输出起始点和终止点的位置
* 返 回 值：无
* 说    明：无
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
    unsigned int                u32CompressNumTemp = s32DstLength;//准备压缩的数据长度
    unsigned int                u32CurrCompPos = 0;
    unsigned int                u32CurrCompLength = 0;
    int	                        i = 0;
    int                         j = 0;
    int                         k = 0;
    unsigned int                u32Multiple = (unsigned int)floor((float)s32SrcLength / (float)(s32DstLength / 2.0)); // 压缩倍率
    double* pf64TempBuff = (double*)0;
    unsigned int* pu32TempPos = (unsigned int*)0;
    double                      af64Dot[4] = { 0 }; // 判定只有4个点时,进行压缩
    PPEAK_VALUE_INFO_FOR_DOUBLE stFindInfo = { 0 };

    // 1 申请临时数据压缩缓冲区,注意使用完成后进行释放,这里s32DstLength * 2是由于缓存了每次分段抽值的最大值及最小值
    pf64TempBuff = (double*)malloc(sizeof(double) * u32CompressNumTemp);
    pu32TempPos = (unsigned int*)malloc(sizeof(int) * u32CompressNumTemp);

    // 2 开始数据压缩
    for (; i < s32DstLength / 2; i++)
    {
        // 2.1 计算本次抽样的在整包数据的位置及送入抽样中的数据长度,这里要对剩余不足一段的数据进行单独处理
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

        // 2.2 查找分段数据中的最大值和最小值
        findMaxAndMinVal4Double(&pf64SrcBuff[u32CurrCompPos + s32Offset], u32CurrCompLength, &stFindInfo);

        // 2.3 根据查找到的最大值及最小值信息,计算在整包数据中的位置
        u32MaxPos = u32CurrCompPos + stFindInfo.max_pos + s32Offset;
        u32MinPos = u32CurrCompPos + stFindInfo.min_pos + s32Offset;

        // 2.4 根据位置信息,记录数据值,这里考虑数据点的分布,上升沿模式(最大值位置大于最小值位置),下降沿模式(最大值位置小于等于最小值位置)
        if (u32MaxPos > u32MinPos)// 上升沿模式
        {
            pf64TempBuff[k] = pf64SrcBuff[u32MinPos];
            pf64TempBuff[k + 1] = pf64SrcBuff[u32MaxPos];
            pu32TempPos[k] = u32MinPos;
            pu32TempPos[k + 1] = u32MaxPos;
            // 第一个点
            if (i == 0)
            {
                // 起始点位置
                s32OutPos[0] = u32MinPos;
            }
            else if (i == s32DstLength / 2 - 1)
            {
                //终止点
                s32OutPos[1] = u32MaxPos;
            }
        }
        else
        {
            pf64TempBuff[k] = pf64SrcBuff[u32MaxPos];
            pf64TempBuff[k + 1] = pf64SrcBuff[u32MinPos];
            pu32TempPos[k] = u32MaxPos;
            pu32TempPos[k + 1] = u32MinPos;
            // 第一个点
            if (i == 0)
            {
                // 起始点位置
                s32OutPos[0] = u32MaxPos;
            }
            else if (i == s32DstLength / 2 - 1)
            {
                //终止点
                s32OutPos[1] = u32MinPos;
            }
        }
        k += 2;
    }

    //  复制压缩结果到输出
    memcpy((char*)pf64DstBuff, (char*)pf64TempBuff, sizeof(double) * u32CompressNumTemp);

    // 4 释放使用的内存
    free(pf64TempBuff);
    free(pu32TempPos);
}

/*******************************************************************************
* 函    数：loadSincInterTable
* 描    述：加载Sinc内插系数表
* 输入参数：
*             参数名称            参数类型                参数说明
*             s32MutipuleXn       int						Sinc内插滤波器系数
*             ps32Table           int*                    Sinc内插滤波器系数表
*             s32TableSize        int                     Sinc内插滤波器系数表大小
*             s32Coe              int                     定点转浮点比率
*             s32Multipule        int                     进行内插时,数据放大倍数
* 输出参数：无
* 返 回 值：无
* 说    明：当该内插滤波器系数表已存在,则不进行注册
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

    // 1 搜索内插系数表是否存在
    pstNode = getSincInterTable(s32MutipuleXn);
    // 2 若搜索到已注册的系数表,则直接返回
    if (pstNode != NULL)
    {
        return;
    }

    // 3 申请系数表信息的存储节点的内存
    pstNode = (struct stSincTable*)malloc(sizeof(struct stSincTable));

    // 4 判定内存是否有效,若无效则直接返回
    if (pstNode == NULL)
    {
        return;
    }

    // 5 申请系数表使用的空间
    ps32TableAddr = (int*)malloc(sizeof(int) * s32TableSize);

    // 6 判定内存是否有效,若无效则直接返回
    if (ps32TableAddr == NULL)
    {
        //  此时由于已申请了系数表信息的存储节点的内存,必须先释放
        free(pstNode);
        return;
    }

    // 7 复制Sinc滤波器系数表到内存
    memcpy((char*)ps32TableAddr, (char*)ps32Table, sizeof(int) * s32TableSize);

    // 8 更新节点信息
    pstNode->xn = s32MutipuleXn;
    pstNode->coe = (double)s32Coe;
    pstNode->table = ps32TableAddr;
    pstNode->size = s32TableSize;
    pstNode->multipule = s32Multipule;
    pstNode->next = m_pstSincTable;

    // 9 把该节点加入链表的表头
    m_pstSincTable = pstNode;

    // 10 初始化线程同步时的信号池
    for (i = 0; i < SINC_FILTER_THREAD_NUM; i++)
    {
        m_asemSincFilterThreadSync[i] = CreateSemaphore(NULL, 0, 1, NULL);
    }
}

/*******************************************************************************
* 函    数：getSincFilterData
* 描    述：获取原始数据送入Sinc内插滤波器后的内插数据
* 输入参数：
*             参数名称            参数类型                参数说明
*             pu8DataBuf          unsigned char *         送入Sinc内存滤波器的数组
*                                                         该数组为无符号单字节整型
*             s32StartIndex       int                     Sinc内插滤波器开始取数据
*                                                         时,在数组中的位置
*             s32EndIndex         int                     Sinc内插滤波器结束取数据
*                                                         时,在数组中的位置
*             s32MutipuleXn       int			            Sinc内插滤波器使用的内插
*                                                         表
*             s32CompStartIndex   int                     内插完成后的数据送入抽样
*                                                         模块时的数据起始索引
*             s32CompDataLen      int                     内插完成后的数据送入抽样
*                                                         模块时的数据总长
*             s32OutBufLen        int                     待存放Sinc内插滤波结果数
*                                                         组的大小
* 输出参数：
*             参数名称            参数类型                参数说明
*             pf64OutBuf          double*                 待存放Sinc内插滤波结果的
*                                                         数组
* 返 回 值：0 -- 成功 ; -1 -- 没有对应的Sinc内插滤波器系数表
* 说    明：无
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
    struct stSincTable* pstNode = NULL;    //  根据Sinc内插倍数枚举值,进行Sinc插值信息表的查找
#if MEMORY_USE_OPTIMIZE_SWITCH == 0
    int* ps32SincTableData = NULL;
    int** pps32SincTablePtr = NULL;
#endif
    int                     s32InDataLen = 0;        //  
    int                     s32FrontExpEndPos = 0;
    int                     s32RearExpStartPos = 0;
    int                     s32TempFir[SINC_FILTER_STEP_NUM] = { 0 };    //  记录滤波器窗口的系数
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

    // 1 获取Sinc内插滤波器表
    pstNode = getSincInterTable(s32MutipuleXn);

    // 2. 判定Sinc表有效性
    if (pstNode == NULL)
    {
        return -1;
    }
    /*
    * 3. 进行Sinc滤波器系数表一维转二维
    */
    // 3.1 进行二维数组的行和列的赋值
    s32MatrixRow = pstNode->multipule;
    s32MatrixColum = SINC_FILTER_STEP_NUM;
#if MEMORY_USE_OPTIMIZE_SWITCH == 0 // 降低内存损耗,这里不再进行内存申请,只保留作为参考设计,便于进行一维到二维的调度参考
    // 3.2 进行数据的内存申请,使用完成后,必须释放
    ps32SincTableData = (int*)malloc(s32MatrixRow * s32MatrixColum * sizeof(int));

    // 3.3 进行二维数组的行地址存储的内存申请,使用完成后,必须释放
    pps32SincTablePtr = (int**)malloc(s32MatrixRow * sizeof(int*));

    // 3.4 进行二维数组地址的分配
    for (i = 0; i < s32MatrixRow; i++)
    {
        pps32SincTablePtr[i] = ps32SincTableData + i * s32MatrixColum;
    }

    /*
    * 3.5 进行一维数组到二维数组的转换
    * 转换规则为
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
    * 3 计算本次需要运算的数据的长度,包含以下三部分组成
    *    ___________________________________________________________
    *   |                     |              |                      |
    *   |    第一个数据扩充   |   原始数据   |   最后一个数据扩充   |
    *   |_____________________|______________|______________________|
    *   其中:第一个数据的扩充长度为:滤波器的放大倍数(pstNode->multipule)*滤波器窗口大小(SINC_FILTER_STEP_NUM)/2
    *        原始数据的长度为:传入的数据结束地址(s32EndIndex)-传入的数据的起始地址(s32StartIndex)+1
    *        最后一个数据的扩充长度为:滤波器的放大倍数(pstNode->multipule)*滤波器窗口大小(SINC_FILTER_STEP_NUM)/2
    *   所以本次送入运算的数据总长度为:传入的数据结束地址(s32EndIndex)-传入的数据的起始地址(s32StartIndex)+1 +
    *                                  滤波器的放大倍数(pstNode->multipule)*滤波器窗口大小(SINC_FILTER_STEP_NUM)
    */
    s32FrontExpEndPos = pstNode->multipule * SINC_FILTER_STEP_NUM / 2;
    s32RearExpStartPos = s32EndIndex - s32StartIndex + 1 + pstNode->multipule * SINC_FILTER_STEP_NUM / 2;
    s32InDataLen = s32EndIndex - s32StartIndex + 1 + pstNode->multipule * SINC_FILTER_STEP_NUM;

    /*
    * 4 申请临时存储内插积分结果的二维数组
    *   此二维数组的行大小为：s32MatrixRow
    *               列大小为：s32ResultAfterInterColum = s32InDataLen + 滤波器窗口大小 - 1
    *   此二维数组的总长度为: s32MatrixRow * s32ResultAfterInterColum
    */
    // 4.1 计算二维数组的列长度
    s32ResultAfterInterColum = (s32InDataLen + SINC_FILTER_STEP_NUM - 1);
#if MEMORY_USE_OPTIMIZE_SWITCH == 0
    // 4.2 申请存储临时数据的内存。注意：此内存在使用完成后,必须释放
    ps32TempAfterInter = (int*)malloc(s32MatrixRow * s32ResultAfterInterColum * sizeof(int));

    // 4.3 申请二维数组行指针的空间。注意：此内存在使用完成后,必须释放
    pps32ResultAfterInter = (int**)malloc(s32MatrixRow * sizeof(int*));

    // 4.4 进行二维数组的行地址分配
    for (i = 0; i < s32MatrixRow; i++)
    {
        pps32ResultAfterInter[i] = ps32TempAfterInter + i * s32ResultAfterInterColum;
    }
#else
    // 4.5 根据计算出的行和列
    // 4.5 申请一维数组的空间,空间大小 = s32MatrixRow * s32ResultAfterInterColum * sizeof(double)
    pf64TempWaveData = (double*)malloc(s32MatrixRow * s32ResultAfterInterColum * sizeof(double));
#endif
    /*
    * 5. 开始卷积的运算
    *    此处运算采用的是二维数组的赋值
    *    此二维数组的行为：s32MatrixRow
    *    此二维数组的列为：s32ResultAfterInterColum
    */
    for (row = 0; row < s32MatrixRow; row++)
    {
        for (colum = 0; colum < s32ResultAfterInterColum; colum++)
        {
            // 5.1 进行Sinc滤波器窗口的平移
            for (j = 0; j < (s32MatrixColum - 1); j++)
            {
                s32TempFir[s32MatrixColum - 1 - j] = s32TempFir[s32MatrixColum - 2 - j];
            }

            // 5.2 更新送入Sinc滤波器窗口数据,超出有效数据长度后,需送入0
            if (colum < s32InDataLen)
            {
                /*
                * 5.2.1 进行送入滤波器数据的筛选
                * 筛选规则为：(1).索引值＜插值倍数*SINC_FILTER_STEP_NUM/2时,采用第一个点进行扩充
                *             (2).索引值≥原始数据点数+插值倍数*SINC_FILTER_STEP_NUM/2时，采用最后一个点进行扩充
                *             (3).其他索引值顺序提取原始数据
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
            // 5.3 记录每个窗口的积分值
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
    * 6 把积分结果的二维数组转换为一维数组
    *   转换规则为：
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
    * 7 进行有效数据的抽取
    *   由于送入滤波器时,采用了首段和后端的数据填充,所以这里数据复制时,规避无效数据
    */
    // 7.1 计算抽取数据的其实位置
    s32WaveDataCopyStartIndex = pstNode->size / 2 + s32CompStartIndex;
    // 7.2 开始数据的抽取
    waveCompAlgorithm4Double(&pf64TempWaveData[s32WaveDataCopyStartIndex], 0, s32CompDataLen, pf64OutBuf, s32OutBufLen);
    // 7.2 开始数据的复制
    //memcpy((char*)pf64OutBuf,(char*)&pf64TempWaveData[s32WaveDataCopyStartIndex],s32OutBufLen*sizeof(double));
    // 8 进行内存的释放
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
* 函    数：SincFilteProcess
* 描    述：对原始数据进行Sinc内插滤波处理
* 输入参数：
*             参数名称            参数类型                参数说明
*             pu8DataBuf          unsigned char *         送入Sinc内存滤波器的数组
*                                                         该数组为无符号单字节整型
*             s32StartIndex       int                     Sinc内插滤波器开始取数据
*                                                         时,在数组中的位置
*             s32EndIndex         int                     Sinc内插滤波器结束取数据
*                                                         时,在数组中的位置
*             s32MutipuleXn       int			            Sinc内插滤波器使用的内插
*                                                         表
*             s32CompStartIndex   int                     内插完成后的数据送入抽样
*                                                         模块时的数据起始索引
*             s32CompDataLen      int                     内插完成后的数据送入抽样
*                                                         模块时的数据总长
*             s32OutBufLen        int                     待存放Sinc内插滤波结果数
*                                                         组的大小
* 输出参数：
*             参数名称            参数类型                参数说明
*             pf64OutBuf          double*                 待存放Sinc内插滤波结果的
*                                                         数组
* 返 回 值：无
* 说    明：无
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
* 函    数：SincFilterWaitForFinish
* 描    述：等待所有内插线程处理完成
* 输入参数：无
* 输出参数：无
* 返 回 值：无
* 说    明：无
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
    struct stSincTable* pstNode = NULL;    //  根据Sinc内插倍数枚举值,进行Sinc插值信息表的查找
    int* ps32SincTableData = NULL;
    int** pps32SincTablePtr = NULL;
    int                     s32InDataLen = 0;        //  
    int                     s32FrontExpEndPos = 0;
    int                     s32RearExpStartPos = 0;
    int                     s32TempFir[SINC_FILTER_STEP_NUM] = { 0 };    //  记录滤波器窗口的系数
    int                     s32MatrixRow = 0;
    int                     s32MatrixColum = 0;
    int                     s32IntegralVal = 0;
    int* ps32TempAfterInter = NULL;
    int** pps32ResultAfterInter = NULL;
    int                     s32ResultAfterInterColum = 0;
    double* pf64TempWaveData = NULL;
    int                     s32WaveDataCopyStartIndex = 0;
    int                     i, j, k, row, colum;

    // 1 获取Sinc内插滤波器表
    pstNode = getSincInterTable(s32MutipuleXn);

    // 2. 判定Sinc表有效性
    if (pstNode == (struct stSincTable*)0)
    {
        return -1;
    }
    /*
    * 3. 进行Sinc滤波器系数表一维转二维
    */
    // 3.1 进行二维数组的行和列的赋值
    s32MatrixRow = pstNode->multipule;
    s32MatrixColum = SINC_FILTER_STEP_NUM;

    // 3.2 进行数据的内存申请,使用完成后,必须释放
    ps32SincTableData = (int*)malloc(s32MatrixRow * s32MatrixColum * sizeof(int));

    // 3.3 进行二维数组的行地址存储的内存申请,使用完成后,必须释放
    pps32SincTablePtr = (int**)malloc(s32MatrixRow * sizeof(int*));

    // 3.4 进行二维数组地址的分配
    for (i = 0; i < s32MatrixRow; i++)
    {
        pps32SincTablePtr[i] = ps32SincTableData + i * s32MatrixColum;
    }

    /*
    * 3.5 进行一维数组到二维数组的转换
    * 转换规则为
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
    * 3 计算本次需要运算的数据的长度,包含以下三部分组成
    *    ___________________________________________________________
    *   |                     |              |                      |
    *   |    第一个数据扩充   |   原始数据   |   最后一个数据扩充   |
    *   |_____________________|______________|______________________|
    *   其中:第一个数据的扩充长度为:滤波器的放大倍数(pstNode->multipule)*滤波器窗口大小(SINC_FILTER_STEP_NUM)/2
    *        原始数据的长度为:传入的数据结束地址(s32EndIndex)-传入的数据的起始地址(s32StartIndex)+1
    *        最后一个数据的扩充长度为:滤波器的放大倍数(pstNode->multipule)*滤波器窗口大小(SINC_FILTER_STEP_NUM)/2
    *   所以本次送入运算的数据总长度为:传入的数据结束地址(s32EndIndex)-传入的数据的起始地址(s32StartIndex)+1 +
    *                                  滤波器的放大倍数(pstNode->multipule)*滤波器窗口大小(SINC_FILTER_STEP_NUM)
    */
    s32FrontExpEndPos = pstNode->multipule * SINC_FILTER_STEP_NUM / 2;
    s32RearExpStartPos = s32EndIndex - s32StartIndex + 1 + pstNode->multipule * SINC_FILTER_STEP_NUM / 2;
    s32InDataLen = s32EndIndex - s32StartIndex + 1 + pstNode->multipule * SINC_FILTER_STEP_NUM;

    /*
    * 4 申请临时存储内插积分结果的二维数组
    *   此二维数组的行大小为：s32MatrixRow
    *               列大小为：s32ResultAfterInterColum = s32InDataLen + 滤波器窗口大小 - 1
    *   此二维数组的总长度为: s32MatrixRow * s32ResultAfterInterColum
    */
    // 4.1 计算二维数组的列长度
    s32ResultAfterInterColum = (s32InDataLen + SINC_FILTER_STEP_NUM - 1);

    // 4.2 申请存储临时数据的内存。注意：此内存在使用完成后,必须释放
    ps32TempAfterInter = (int*)malloc(s32MatrixRow * s32ResultAfterInterColum * sizeof(int));

    // 4.3 申请二维数组行指针的空间。注意：此内存在使用完成后,必须释放
    pps32ResultAfterInter = (int**)malloc(s32MatrixRow * sizeof(int*));

    // 4.4 进行二维数组的行地址分配
    for (i = 0; i < s32MatrixRow; i++)
    {
        pps32ResultAfterInter[i] = ps32TempAfterInter + i * s32ResultAfterInterColum;
    }

    /*
    * 5. 开始卷积的运算
    *    此处运算采用的是二维数组的赋值
    *    此二维数组的行为：s32MatrixRow
    *    此二维数组的列为：s32ResultAfterInterColum
    */
    //如果大于当前点
    if (s32EndIndex > m_chanMemPoints)
    {
        //申请拷贝数据空间
        unsigned char* pu8WavDataBuff = (unsigned char*)malloc(sizeof(unsigned char) * s32EndIndex);
        // 先复制原始数据
        memcpy((char*)pu8WavDataBuff, (char*)pu8DataBuf, sizeof(unsigned char) * m_chanMemPoints);
        // 再cpy缺少的点
        for (int i = m_chanMemPoints; i < s32EndIndex; i++)
        {
            pu8WavDataBuff[i] = pu8DataBuf[m_chanMemPoints - 1];
        }
        for (row = 0; row < s32MatrixRow; row++)
        {
            for (colum = 0; colum < s32ResultAfterInterColum; colum++)
            {
                // 5.1 进行Sinc滤波器窗口的平移
                for (j = 0; j < (s32MatrixColum - 1); j++)
                {
                    s32TempFir[s32MatrixColum - 1 - j] = s32TempFir[s32MatrixColum - 2 - j];
                }

                // 5.2 更新送入Sinc滤波器窗口数据,超出有效数据长度后,需送入0
                if (colum < s32InDataLen)
                {
                    /*
                    * 5.2.1 进行送入滤波器数据的筛选
                    * 筛选规则为：(1).索引值＜插值倍数*SINC_FILTER_STEP_NUM/2时,采用第一个点进行扩充
                    *             (2).索引值≥原始数据点数+插值倍数*SINC_FILTER_STEP_NUM/2时，采用最后一个点进行扩充
                    *             (3).其他索引值顺序提取原始数据
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
                // 5.3 记录每个窗口的积分值
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
                // 5.1 进行Sinc滤波器窗口的平移
                for (j = 0; j < (s32MatrixColum - 1); j++)
                {
                    s32TempFir[s32MatrixColum - 1 - j] = s32TempFir[s32MatrixColum - 2 - j];
                }

                // 5.2 更新送入Sinc滤波器窗口数据,超出有效数据长度后,需送入0
                if (colum < s32InDataLen)
                {
                    /*
                    * 5.2.1 进行送入滤波器数据的筛选
                    * 筛选规则为：(1).索引值＜插值倍数*SINC_FILTER_STEP_NUM/2时,采用第一个点进行扩充
                    *             (2).索引值≥原始数据点数+插值倍数*SINC_FILTER_STEP_NUM/2时，采用最后一个点进行扩充
                    *             (3).其他索引值顺序提取原始数据
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
                // 5.3 记录每个窗口的积分值
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
    //        // 5.1 进行Sinc滤波器窗口的平移
    //        for ( j = 0; j < (s32MatrixColum - 1); j++)
    //        {
    //            s32TempFir[s32MatrixColum-1-j] = s32TempFir[s32MatrixColum-2-j];
    //        }

    //        // 5.2 更新送入Sinc滤波器窗口数据,超出有效数据长度后,需送入0
    //        if( colum < s32InDataLen )
    //        {
    //            /*
    //            * 5.2.1 进行送入滤波器数据的筛选
    //            * 筛选规则为：(1).索引值＜插值倍数*SINC_FILTER_STEP_NUM/2时,采用第一个点进行扩充
    //            *             (2).索引值≥原始数据点数+插值倍数*SINC_FILTER_STEP_NUM/2时，采用最后一个点进行扩充
    //            *             (3).其他索引值顺序提取原始数据
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
    //        // 5.3 记录每个窗口的积分值
    //        for( k = 0 ; k < s32MatrixColum ; k ++ )
    //        {
    //            s32IntegralVal += s32TempFir[k] * pps32SincTablePtr[row][k];
    //        }
    //        pps32ResultAfterInter[row][colum] = s32IntegralVal;
    //        s32IntegralVal = 0;
    //    }
    //}

    /*
    * 6 把积分结果的二维数组转换为一维数组
    *   转换规则为：
    *   _     _
    *  |       |        _                 _
    *  | 1,2,3 |       |                   |
    *  | 4,5,6 | ----> | 1,4,7,2,5,8,3,6,9 |
    *  | 7,8,9 |       |_                 _|
    *  |_     _|
    */
    // 6.1 申请一维数组的空间,空间大小 = s32MatrixRow * s32ResultAfterInterColum * sizeof(double)
    pf64TempWaveData = (double*)malloc(s32MatrixRow * s32ResultAfterInterColum * sizeof(double));
    // 6.2 开始进行二维数据到一维的转换
    for (i = 0; i < s32ResultAfterInterColum; i++)
    {
        for (j = 0; j < s32MatrixRow; j++)
        {
            pf64TempWaveData[j + i * s32MatrixRow] = (double)pps32ResultAfterInter[j][i] / pstNode->coe;
        }
    }
    /*
    * 7 进行有效数据的输出
    *   由于送入滤波器时,采用了首段和后端的数据填充,所以这里数据复制时,规避无效数据
    */
    // 7.1 计算复制数据的位置
    s32WaveDataCopyStartIndex = pstNode->size / 2 + s32OutBufOffset;
    // 7.2 开始数据的复制
    for (i = s32WaveDataCopyStartIndex; i < s32WaveDataCopyStartIndex + s32OutBufLen; i++)
    {
        pf64OutBuf[i - s32WaveDataCopyStartIndex] = pf64TempWaveData[i];
    }

    // 8 进行内存的释放
    free(ps32SincTableData);
    free(pps32SincTablePtr);
    free(ps32TempAfterInter);
    free(pps32ResultAfterInter);
    free(pf64TempWaveData);

    return 0;
}
