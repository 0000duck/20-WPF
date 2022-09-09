#ifndef _RIGOL_FOR_INTER_AND_SAMP_H_
#define _RIGOL_FOR_INTER_AND_SAMP_H_

#ifndef _RIGOL_DLL_API
#define _RIGOL_DLL_API extern "C" _declspec(dllexport) 
#else
#endif

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
_RIGOL_DLL_API  void waveCompAlgorithm4UChar(unsigned char *pu8SrcBuff , 
                                             int s32Offset,
                                             int s32SrcLength ,
                                             unsigned char *pu8DstBuff ,
                                             int s32DstLength );
/*****************************************************************************/
//  C# 声明
// [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
// public static extern void waveCompAlgorithm4UChar( byte[] pu8SrcBuff,
//                                                    int s32Offset,
//                                                    int s32SrcLength ,
//                                                    byte[] pu8DstBuff ,
//                                                    int s32DstLength);
/*****************************************************************************/
_RIGOL_DLL_API void  origWaveDataPreProcess4UChar(unsigned char *pu8SrcBuff , int s32Offset);
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
_RIGOL_DLL_API void waveCompAlgorithm4UCharSimple(unsigned char *pu8SrcBuff , 
                             int s32Offset,
                             int s32SrcLength ,
                             unsigned char *pu8DstBuff ,
                             int s32DstLength ,
                             int *s32OutPos);
//  C# 声明
// [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
 //public static extern void waveCompAlgorithm4UCharSimple(byte[] pu8SrcBuff , 
 //                            int s32Offset,
 //                            int s32SrcLength ,
 //                            byte[] pu8DstBuff ,
 //                            int s32DstLength ,
 //                            int[] s32OutPos);
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
_RIGOL_DLL_API void waveCompAlgorithm4Double(double *pf64SrcBuff ,
                                             int s32Offset,
                                             int s32SrcLength , 
                                             double *pf64DstBuff ,
                                             int s32DstLength );
/*****************************************************************************/
//  C# 声明
// [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
// public static extern void waveCompAlgorithm4Double( double[] pf64SrcBuff,
//                                                    int s32Offset,
//                                                    int s32SrcLength ,
//                                                    double[] pf64DstBuff ,
//                                                    int s32DstLength);
/*****************************************************************************/
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
_RIGOL_DLL_API void waveCompAlgorithm4DoubleSimple(double *pf64SrcBuff ,
                              int s32Offset,
                              int s32SrcLength , 
                              double *pf64DstBuff ,
                              int s32DstLength ,
                              int *s32OutPos);
/*****************************************************************************/
//  C# 声明
// [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
 //public static extern void waveCompAlgorithm4DoubleSimple(double[] pf64SrcBuff ,
 //                             int s32Offset,
 //                             int s32SrcLength , 
 //                             double[] pf64DstBuff ,
 //                             int s32DstLength ,
 //                             int[] s32OutPos)
/*****************************************************************************/
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
_RIGOL_DLL_API void loadSincInterTable( int s32MutipuleXn , 
										int *ps32Table , 
										int s32TableSize , 
										int s32Coe , 
										int s32Multipule);
/*****************************************************************************/
//  C# 声明
// [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
// public static extern void loadSincInterTable( int	s32MutipuleXn , 
//                                               int[]	ps32Table , 
//                                               int	s32TableSize , 
//                                               int	s32Coe , 
//                                               int	s32Multipule);
/*****************************************************************************/

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
_RIGOL_DLL_API int getSincFilterData( unsigned char *  pu8DataBuf , 
                                      int              s32StartIndex , 
                                      int              s32EndIndex , 
                                      int		        s32MutipuleXn , 
                                      int              s32CompStartIndex,
                                      int              s32CompDataLen,
                                      double *         pf64OutBuf , 
                                      int              s32OutBufLen );
/*****************************************************************************/
//  C# 声明
// [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
// public static extern int getSincFilterData( byte[]           pu8DataBuf , 
//                                             int              s32StartIndex , 
//                                             int              s32EndIndex , 
//                                             int				s32MutipuleXn ,
//                                             int              s32CompStartIndex,
//                                             int              s32CompDataLen,
//                                             double[]         pf64OutBuf , 
//                                             int              s32OutBufLen );
/*****************************************************************************/
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
_RIGOL_DLL_API void SincFilteProcess( unsigned char *  pu8DataBuf , 
                                      int              s32StartIndex , 
                                      int              s32EndIndex , 
                                      int		       s32MutipuleXn , 
                                      int              s32CompStartIndex,
                                      int              s32CompDataLen,
                                      double *         pf64OutBuf , 
                                      int              s32OutBufLen );
/*****************************************************************************/
//  C# 声明
// [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
// public static extern void SincFilteProcess( byte[]           pu8DataBuf , 
//                                             int              s32StartIndex , 
//                                             int              s32EndIndex , 
//                                             int				s32MutipuleXn ,
//                                             int              s32CompStartIndex,
//                                             int              s32CompDataLen,
//                                             double[]         pf64OutBuf , 
//                                             int              s32OutBufLen );
/*****************************************************************************/

/*******************************************************************************
  * 函    数：SincFilterWaitForFinish
  * 描    述：等待所有内插线程处理完成
  * 输入参数：无
  * 输出参数：无
  * 返 回 值：无
  * 说    明：无
 ******************************************************************************/
_RIGOL_DLL_API void SincFilterWaitForFinish();
/*****************************************************************************/
//  C# 声明
// [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
// public static extern void SincFilterWaitForFinish( );
/*****************************************************************************/

_RIGOL_DLL_API int getSincFilterDataAddOffset( unsigned char *  pu8DataBuf , 
                       int              s32StartIndex , 
                       int              s32EndIndex , 
                       int		        s32MutipuleXn , 
                       double *         pf64OutBuf , 
					   int			    s32OutBufOffset,
                       int              s32OutBufLen );
#endif