#ifndef _RIGOL_FOR_INTER_AND_SAMP_H_
#define _RIGOL_FOR_INTER_AND_SAMP_H_

#ifndef _RIGOL_DLL_API
#define _RIGOL_DLL_API extern "C" _declspec(dllexport) 
#else
#endif

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
_RIGOL_DLL_API  void waveCompAlgorithm4UChar(unsigned char *pu8SrcBuff , 
                                             int s32Offset,
                                             int s32SrcLength ,
                                             unsigned char *pu8DstBuff ,
                                             int s32DstLength );
/*****************************************************************************/
//  C# ����
// [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
// public static extern void waveCompAlgorithm4UChar( byte[] pu8SrcBuff,
//                                                    int s32Offset,
//                                                    int s32SrcLength ,
//                                                    byte[] pu8DstBuff ,
//                                                    int s32DstLength);
/*****************************************************************************/
_RIGOL_DLL_API void  origWaveDataPreProcess4UChar(unsigned char *pu8SrcBuff , int s32Offset);
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
_RIGOL_DLL_API void waveCompAlgorithm4UCharSimple(unsigned char *pu8SrcBuff , 
                             int s32Offset,
                             int s32SrcLength ,
                             unsigned char *pu8DstBuff ,
                             int s32DstLength ,
                             int *s32OutPos);
//  C# ����
// [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
 //public static extern void waveCompAlgorithm4UCharSimple(byte[] pu8SrcBuff , 
 //                            int s32Offset,
 //                            int s32SrcLength ,
 //                            byte[] pu8DstBuff ,
 //                            int s32DstLength ,
 //                            int[] s32OutPos);
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
_RIGOL_DLL_API void waveCompAlgorithm4Double(double *pf64SrcBuff ,
                                             int s32Offset,
                                             int s32SrcLength , 
                                             double *pf64DstBuff ,
                                             int s32DstLength );
/*****************************************************************************/
//  C# ����
// [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
// public static extern void waveCompAlgorithm4Double( double[] pf64SrcBuff,
//                                                    int s32Offset,
//                                                    int s32SrcLength ,
//                                                    double[] pf64DstBuff ,
//                                                    int s32DstLength);
/*****************************************************************************/
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
_RIGOL_DLL_API void waveCompAlgorithm4DoubleSimple(double *pf64SrcBuff ,
                              int s32Offset,
                              int s32SrcLength , 
                              double *pf64DstBuff ,
                              int s32DstLength ,
                              int *s32OutPos);
/*****************************************************************************/
//  C# ����
// [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
 //public static extern void waveCompAlgorithm4DoubleSimple(double[] pf64SrcBuff ,
 //                             int s32Offset,
 //                             int s32SrcLength , 
 //                             double[] pf64DstBuff ,
 //                             int s32DstLength ,
 //                             int[] s32OutPos)
/*****************************************************************************/
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
_RIGOL_DLL_API void loadSincInterTable( int s32MutipuleXn , 
										int *ps32Table , 
										int s32TableSize , 
										int s32Coe , 
										int s32Multipule);
/*****************************************************************************/
//  C# ����
// [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
// public static extern void loadSincInterTable( int	s32MutipuleXn , 
//                                               int[]	ps32Table , 
//                                               int	s32TableSize , 
//                                               int	s32Coe , 
//                                               int	s32Multipule);
/*****************************************************************************/

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
_RIGOL_DLL_API int getSincFilterData( unsigned char *  pu8DataBuf , 
                                      int              s32StartIndex , 
                                      int              s32EndIndex , 
                                      int		        s32MutipuleXn , 
                                      int              s32CompStartIndex,
                                      int              s32CompDataLen,
                                      double *         pf64OutBuf , 
                                      int              s32OutBufLen );
/*****************************************************************************/
//  C# ����
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
_RIGOL_DLL_API void SincFilteProcess( unsigned char *  pu8DataBuf , 
                                      int              s32StartIndex , 
                                      int              s32EndIndex , 
                                      int		       s32MutipuleXn , 
                                      int              s32CompStartIndex,
                                      int              s32CompDataLen,
                                      double *         pf64OutBuf , 
                                      int              s32OutBufLen );
/*****************************************************************************/
//  C# ����
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
  * ��    ����SincFilterWaitForFinish
  * ��    �����ȴ������ڲ��̴߳������
  * �����������
  * �����������
  * �� �� ֵ����
  * ˵    ������
 ******************************************************************************/
_RIGOL_DLL_API void SincFilterWaitForFinish();
/*****************************************************************************/
//  C# ����
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