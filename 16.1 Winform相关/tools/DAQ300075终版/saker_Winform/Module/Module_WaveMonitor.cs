using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using saker_Winform.Global;
using ClassLibrary_WfmDataProcess;
using saker_Winform.SubForm;
using System.Drawing;
using System.Data;
using saker_Winform.CommonBaseModule;
using System.Runtime.InteropServices;

/*****************************************************************************************************************
                                        普源精电科技有限公司版权所有(2020-)
******************************************************************************************************************
类名:      Module_WaveMonitor
功能描述： Module_WaveMonitor类中定义了波形监控界面中所需数据的处理
作 者：    sn02736
版 本：    00.01.00.00
完成日期： 
修改历史： 
<作者>               <修改时间>               <版本>                    <修改描述>
*****************************************************************************************************************/
namespace saker_Winform.Module
{
    public class Module_WaveMonitor
    {
        [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void origWaveDataPreProcess4UChar(byte[] pu8SrcBuff, int s32Offset);

        [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void waveCompAlgorithm4UChar(byte[] pu8SrcBuff,
                                                           int s32Offset,
                                                           int s32SrcLength,
                                                           byte[] pu8DstBuff,
                                                           int s32DstLength);

        [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void waveCompAlgorithm4UCharSimple(byte[] pu8SrcBuff,
                                                                int s32Offset,
                                                                int s32SrcLength,
                                                                byte[] pu8DstBuff,
                                                                int s32DstLength,
                                                                int[] s32OutPos);

        [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void waveCompAlgorithm4Double(double[] pf64SrcBuff,
                                                           int  s32Offset,
                                                           int  s32SrcLength,
                                                           double[] pf64DstBuff,
                                                           int  s32DstLength);

        [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void waveCompAlgorithm4DoubleSimple(double[] pf64SrcBuff,
                                                                 int s32Offset,
                                                                 int s32SrcLength,
                                                                 double[] pf64DstBuff,
                                                                 int s32DstLength,
                                                                 int[] s32OutPos);

        [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void loadSincInterTable(int s32MutipuleXn,
                                                      int[] ps32Table,
                                                      int s32TableSize,
                                                      int s32Coe,
                                                      int s32Multipule);

        [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getSincFilterData(byte[] pu8DataBuf,
                                                    int s32StartIndex,
                                                    int s32EndIndex,
                                                    int s32MutipuleXn,
                                                    int s32CompStartIndex,
                                                    int s32CompDataLen,
                                                    double[] pf64OutBuf,
                                                    int s32OutBufLen);

        [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SincFilteProcess(byte[] pu8DataBuf,
                                                    int s32StartIndex,
                                                    int s32EndIndex,
                                                    int s32MutipuleXn,
                                                    int s32CompStartIndex,
                                                    int s32CompDataLen,
                                                    double[] pf64OutBuf,
                                                    int s32OutBufLen);

        [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getSincFilterDataAddOffset( byte[] pu8DataBuf , 
                       int              s32StartIndex , 
                       int              s32EndIndex , 
                       int		        s32MutipuleXn , 
                       double[]         pf64OutBuf , 
					   int			    s32OutBufOffset,
                       int              s32OutBufLen );

        [DllImport("cbbInsertAndSample.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SincFilterWaitForFinish();
        #region Fields
        /// <summary>
        /// 示波器内存获取的数据
        /// </summary>
        public class OscilloscopeDataMemory
        {
            public string strChanID;//通道标示

            public string strDevSN;//设备序列号

            public CGlobalValue.euChanID euChan;//对应设备的实际通道

            public int memDepth;//存储深度

            public double xIncrement;//x方向上相邻两点的时间差

            public double xOrigin;//x方向上数据的起始时间

            public double xReference;//x方向上数据的参考时间基准

            public double yIncrement;//y方向上波形的步进值

            public double yOrigin;//y方向上相对于“垂直参考位置”的垂直偏移

            public double yReference;//y方向的处置参考位置

            public double horzScale;//水平时基

            public double horzOffset;//水平偏移

            public double vertScale;//垂直档位

            public double vertOffset;//垂直偏移

            public bool bChanInv;//通道是否反向

            public bool bChanBWLimit;//通道是否带宽限制

            public bool bChanImpedence;//通道是否高阻

            public double sampRate;//采样率

            public int trigTimStamp;//触发时间戳

            public int chanDelayTime;//通道相对于触发位置的精细延时

            public int devDelayTime;//多机之间的延时

            //public byte[] originData;//内存的原始波形
        }
        /// <summary>
        /// 处理后的内存数据
        /// </summary>
        public class OscilloscopeDataProcess : OscilloscopeDataMemory
        {

            public int totalTimStamp;//整个的时间戳

            public int roughAdj;//粗略调整的时间戳

            public int finAgj;//精细调整的时间戳

            public int shiftPointsRough;//粗调时位移的点数

            public double vertViewOffset;//显示时的垂直偏移

            public int curInterMutilpe;//当前内插倍数

            public double wfmFirstHalfPeakWidth;//波形的第一个半峰宽

            public double wfmFirstPeakValue;//波形的第一个峰的峰峰值

            public double wfmFirstTopValue;//波形第一个波的波峰值

            public double wfmMinValue;//波形最小值

            public Color lineColor = Color.Red;//当前通道波形的绘制颜色

            //public byte[] dataAfterRoughProcess;//处理后的数据-与内存深度保持一致,将波形数据先对齐到一个采样点的间隔

            public double[] dataDispRough;//粗调后初始显示用的数据 ,压缩后

            public double[] dataDispFin;//细调后初始显示用的数据 ,压缩后

            //public int enlargeStartPos;//放大后的起始位置

            //public int enlargeStartPosDefault;

            //public int enlargeStopPos;//放大后的终止位置

            //public int enlargeStopPosDefault;

        }
        /*定义下位机每帧多上报的点数*/
        private const int pointsAdditional = 8;
        //定义屏幕显示的总点数
        private const int pointDisplay = 2000;
        /*初始化sinc插值的类*/
        private CSincInter cSinc_Inter = new CSincInter();
        /*显示与处理数据链表*/
        public List<OscilloscopeDataProcess> listOscDataProcess = new List<OscilloscopeDataProcess>();
        //设备延时
        public int devDelayMin = 0;
        /*默认的内插倍数,最大内插倍数252*/
        public  int[] interMutipleDefault = { 1, 2, 4, 5, 8, 10, 16, 20, 25, 32, 40, 50, 80, 100, 160, 200, 252 };
        public CSincInter.euInterMutipule[] euInterMutipleDefault =  {CSincInter.euInterMutipule.X1,
                                                                               CSincInter.euInterMutipule.X2,
                                                                               CSincInter.euInterMutipule.X4,
                                                                            CSincInter.euInterMutipule.X5,
                                                                           CSincInter.euInterMutipule.X8,
                                                                           CSincInter.euInterMutipule.X10,
                                                                           CSincInter.euInterMutipule.X16,
                                                                           CSincInter.euInterMutipule.X20,
                                                                           CSincInter.euInterMutipule.X25,
                                                                           CSincInter.euInterMutipule.X32,
                                                                           CSincInter.euInterMutipule.X40,
                                                                           CSincInter.euInterMutipule.X50,
                                                                           CSincInter.euInterMutipule.X80,
                                                                           CSincInter.euInterMutipule.X100,
                                                                           CSincInter.euInterMutipule.X160,
                                                                           CSincInter.euInterMutipule.X200,
                                                                           CSincInter.euInterMutipule.X252};
        // 归一化系数
        private const int s32FirCoe = 32767;
        #endregion

        #region Construction
        /// <summary>
        /// 构造函数
        /// </summary>
        public Module_WaveMonitor()
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// 初始化，加载插值系数表,原始数据进行粗调整
        /// </summary>
        /// <returns>true初始化成功  false初始化失败</returns>
        public bool modWaveMonitor_Init()
        {
            //加载所有系数表
            bool bResult = cSinc_Inter.CSincInter_LoadAll();
            if (bResult != true)
            {
                return bResult;
            }
            listOscDataProcess.Clear();
            return bResult;
        }
        /// <summary>
        /// 初始化c库使用的方法
        /// </summary>
        /// <returns></returns>
        public void modWaveMonitor_Init_UsingCDLL()
        {
            // 初始化C库计算所需的信息
            for (int i = 0; i < cSinc_Inter.cSincDataList.Count; i++)
            {
                loadSincInterTable((int)cSinc_Inter.cSincDataList[i].euMultiple,
                    cSinc_Inter.cSincDataList[i].interCoefficient,
                    cSinc_Inter.cSincDataList[i].interCoefficient.Length,
                    s32FirCoe,
                    cSinc_Inter.cSincDataList[i].intMultiple);
            }
        }
        /// <summary>
        /// 从外部加载原始数据到当前的数据结构
        /// </summary>

        public void modWaveMonitor_Load(OscilloscopeDataMemory oscData)
        {
            int index = listOscDataProcess.FindIndex(item => (item.strChanID == oscData.strChanID));
            int[] posTemp = new int[2];//用于保存抽样后的起始和终止点
            if (index != -1)
            {
                // 该元素已存在
                listOscDataProcess[index].yReference = oscData.yReference;
                listOscDataProcess[index].yOrigin = oscData.yOrigin;
                listOscDataProcess[index].yIncrement = oscData.yIncrement;
                listOscDataProcess[index].memDepth = oscData.memDepth;
                listOscDataProcess[index].strChanID = oscData.strChanID;
                listOscDataProcess[index].trigTimStamp = oscData.trigTimStamp;
                listOscDataProcess[index].chanDelayTime = oscData.chanDelayTime;
                listOscDataProcess[index].devDelayTime = oscData.devDelayTime;
                listOscDataProcess[index].vertScale = oscData.vertScale;
                listOscDataProcess[index].vertOffset = oscData.vertOffset;
                listOscDataProcess[index].vertViewOffset = oscData.vertOffset;
                listOscDataProcess[index].bChanBWLimit = oscData.bChanBWLimit;
                listOscDataProcess[index].bChanImpedence = oscData.bChanImpedence;
                listOscDataProcess[index].bChanInv = oscData.bChanInv;
                listOscDataProcess[index].xIncrement = oscData.xIncrement;
                listOscDataProcess[index].xOrigin = oscData.xOrigin;
                listOscDataProcess[index].euChan = oscData.euChan;
                listOscDataProcess[index].strDevSN = oscData.strDevSN;
                listOscDataProcess[index].sampRate = oscData.sampRate;
                // 根据当前通道判断取余的大小
                int chanMode = Module_DeviceManage.Instance.GetMaxChannelMode();
                int stampTime = 100;
                stampTime = chanMode * 100;
                if (chanMode == 0)
                {
                    stampTime = 400;
                }
                //计算通道的整体偏移时间
                listOscDataProcess[index].totalTimStamp = listOscDataProcess[index].trigTimStamp % stampTime + listOscDataProcess[index].chanDelayTime + listOscDataProcess[index].devDelayTime - devDelayMin;
                //listOscDataProcess[index].totalTimStamp =  listOscDataProcess[index].chanDelayTime + listOscDataProcess[index].devDelayTime + 800;
                //计算粗移的位置点数
                listOscDataProcess[index].shiftPointsRough = listOscDataProcess[index].totalTimStamp / (int)(listOscDataProcess[index].xIncrement * CGlobalValue.S_TRANS_PS);
                if (listOscDataProcess[index].shiftPointsRough >= pointsAdditional)
                {
                    //最多粗移8个点
                    //listOscDataProcess[index].shiftPointsRough = pointsAdditional;
                }
                //计算粗略调整的时间戳
                listOscDataProcess[index].roughAdj = listOscDataProcess[index].shiftPointsRough * (int)(listOscDataProcess[index].xIncrement * CGlobalValue.S_TRANS_PS);
                //计算需要细调的时间戳
                listOscDataProcess[index].finAgj = listOscDataProcess[index].totalTimStamp - listOscDataProcess[index].roughAdj;
                //从原始数据中抽取10k点
                byte[] waveDataTemp = new byte[pointDisplay];
                // 先对波形进行预处理，完成粗移的点数
                //origWaveDataPreProcess4UChar(Module_DeviceManage.Instance.GetDeviceBySN(oscData.strDevSN).GetChannel((int)oscData.euChan).GetData(),listOscDataProcess[index].shiftPointsRough);
                waveCompAlgorithm4UCharSimple(Module_DeviceManage.Instance.GetDeviceBySN(oscData.strDevSN).GetChannel((int)oscData.euChan).GetData(),
                    listOscDataProcess[index].shiftPointsRough,
                    listOscDataProcess[index].memDepth,
                    waveDataTemp,
                    pointDisplay,
                    posTemp);
                //waveCompAlgorithm4UCharSimple(Module_DeviceManage.Instance.GetDeviceBySN(oscData.strDevSN).GetChannel((int)oscData.euChan).GetData(),
                //    0,
                //    listOscDataProcess[index].memDepth,
                //    waveDataTemp,
                //    pointDisplay,
                //    posTemp);
                // 更新抽样后的初始位置和终止位置
                //listOscDataProcess[index].enlargeStartPos = posTemp[0];
                //listOscDataProcess[index].enlargeStopPos = posTemp[1];
                //listOscDataProcess[index].enlargeStartPosDefault = posTemp[0];
                //listOscDataProcess[index].enlargeStopPosDefault = posTemp[1];
                for (int i = 0; i < pointDisplay; i++)
                {
                    //换算到实际的电压值
                    listOscDataProcess[index].dataDispRough[i] = (double)(waveDataTemp[i] - (listOscDataProcess[index].yOrigin + listOscDataProcess[index].yReference)) * listOscDataProcess[index].yIncrement;
                    listOscDataProcess[index].dataDispFin[i] = (double)(waveDataTemp[i] - (listOscDataProcess[index].yOrigin + listOscDataProcess[index].yReference)) * listOscDataProcess[index].yIncrement;
                }

            }
            else
            {
                //新增元素
                OscilloscopeDataProcess oscDataProcess = new OscilloscopeDataProcess();
                oscDataProcess.yReference = oscData.yReference;
                oscDataProcess.yOrigin = oscData.yOrigin;
                oscDataProcess.yIncrement = oscData.yIncrement;
                oscDataProcess.memDepth = oscData.memDepth;
                oscDataProcess.strChanID = oscData.strChanID;
                oscDataProcess.trigTimStamp = oscData.trigTimStamp;
                oscDataProcess.chanDelayTime = oscData.chanDelayTime;
                oscDataProcess.devDelayTime = oscData.devDelayTime;
                oscDataProcess.vertScale = oscData.vertScale;
                oscDataProcess.vertOffset = oscData.vertOffset;
                oscDataProcess.vertViewOffset = oscData.vertOffset;
                oscDataProcess.bChanBWLimit = oscData.bChanBWLimit;
                oscDataProcess.bChanImpedence = oscData.bChanImpedence;
                oscDataProcess.bChanInv = oscData.bChanInv;
                oscDataProcess.xIncrement = oscData.xIncrement;
                oscDataProcess.xOrigin = oscData.xOrigin;
                oscDataProcess.euChan = oscData.euChan;
                oscDataProcess.strDevSN = oscData.strDevSN;
                oscDataProcess.sampRate = oscData.sampRate;

                //默认显示的点数为1k
                oscDataProcess.dataDispRough = new double[pointDisplay];
                oscDataProcess.dataDispFin = new double[pointDisplay];
                int chanMode = Module_DeviceManage.Instance.GetMaxChannelMode();
                int stampTime = 100;
                stampTime = chanMode * 100;
                if (chanMode == 0)
                {
                    stampTime = 400;
                }
                //计算通道的整体偏移时间
                oscDataProcess.totalTimStamp = oscDataProcess.trigTimStamp % stampTime + oscDataProcess.chanDelayTime + oscDataProcess.devDelayTime - devDelayMin;
                //oscDataProcess.totalTimStamp = oscDataProcess.chanDelayTime + oscDataProcess.devDelayTime + 800;
                //计算粗移的位置点数
                oscDataProcess.shiftPointsRough = oscDataProcess.totalTimStamp / (int)(oscDataProcess.xIncrement * CGlobalValue.S_TRANS_PS);
                if (oscDataProcess.shiftPointsRough >= pointsAdditional)
                {
                    //最多粗移8个点
                    //oscDataProcess.shiftPointsRough = pointsAdditional;
                }
                //计算粗略调整的时间戳
                oscDataProcess.roughAdj = oscDataProcess.shiftPointsRough * (int)(oscDataProcess.xIncrement * CGlobalValue.S_TRANS_PS);
                //计算需要细调的时间戳
                oscDataProcess.finAgj = oscDataProcess.totalTimStamp - oscDataProcess.roughAdj;
                //从原始数据中抽取10k点
                byte[] waveDataTemp = new byte[pointDisplay];
                // 先对波形进行预处理，完成粗移的点数
                //origWaveDataPreProcess4UChar(Module_DeviceManage.Instance.GetDeviceBySN(oscData.strDevSN).GetChannel((int)oscData.euChan).GetData(), oscDataProcess.shiftPointsRough);
                waveCompAlgorithm4UCharSimple(Module_DeviceManage.Instance.GetDeviceBySN(oscData.strDevSN).GetChannel((int)oscData.euChan).GetData(),
                    oscDataProcess.shiftPointsRough,
                    oscDataProcess.memDepth,
                    waveDataTemp,
                    pointDisplay,
                    posTemp);
                //waveCompAlgorithm4UCharSimple(Module_DeviceManage.Instance.GetDeviceBySN(oscData.strDevSN).GetChannel((int)oscData.euChan).GetData(),
                //    0,
                //    oscDataProcess.memDepth,
                //    waveDataTemp,
                //    pointDisplay,
                //    posTemp);
                // 更新抽样后的初始位置和终止位置
                //oscDataProcess.enlargeStartPos = posTemp[0];
                //oscDataProcess.enlargeStopPos = posTemp[1];
                //oscDataProcess.enlargeStartPosDefault = posTemp[0];
                //oscDataProcess.enlargeStopPosDefault = posTemp[1];
                for (int i = 0; i < pointDisplay; i++)
                {
                    //换算到实际的电压值
                    oscDataProcess.dataDispRough[i] = (double)(waveDataTemp[i] - (oscDataProcess.yOrigin + oscDataProcess.yReference)) * oscDataProcess.yIncrement;
                    oscDataProcess.dataDispFin[i] = (double)(waveDataTemp[i] - (oscDataProcess.yOrigin + oscDataProcess.yReference)) * oscDataProcess.yIncrement;
                }

                /*添加到链表*/
                listOscDataProcess.Add(oscDataProcess);
            }
        }

        /// <summary>
        /// 根据起始位置和终止位置输出用于显示的波形点
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="stopPos"></param>
        /// <param name="interMutiple">内插倍数</param>
        public bool calEnlargeWaveData(int startPos, int stopPos, int interMutiple)
        {
            if (startPos > stopPos)
            {
                return false;
            }
            //放大的最小限制10个点
            if (stopPos <= startPos + 4)
            {
                return false;
            }


            int pointNum = stopPos - startPos + 1;
            int offsetPos = 0;
            //先根据内插倍数，找到实际内插使用的枚举值
            for (int i = 0; i < interMutipleDefault.Length; i++)
            {
                if (interMutiple == interMutipleDefault[i])
                {
                    offsetPos = i;
                    break;
                }
            }
            //并行计算
            //Parallel.For(0, listOscDataProcess.Count,
            //    item =>
            //    {
            //        //保存当前内插倍数
            //        listOscDataProcess[item].curInterMutilpe = interMutiple;
            //        //计算经过插值后两点之间最佳间隔时间
            //        double timIntervalFin = (double)(listOscDataProcess[item].xIncrement * CGlobalValue.S_TRANS_PS) / interMutiple;
            //        //计算插值后精细移动的点数 = 剩余的精细时间/插值后的时间间隔
            //        int shiftPointsFin = (int)(listOscDataProcess[item].finAgj / timIntervalFin);
            //        //根据内插倍数进行插值计算,插值点数为原始点补数，补的数量为插值倍数*8
            //        double[] outData = new double[(pointNum + interMutiple * 8) * interMutiple];
            //        int[] inData = new int[pointNum + interMutiple * 8];
            //        byte[] dataAfterRoughProcess = new byte[listOscDataProcess[item].memDepth];
            //        Buffer.BlockCopy(Module_DeviceManage.Instance.GetDeviceBySN(listOscDataProcess[item].strDevSN).GetChannel((int)listOscDataProcess[item].euChan).GetData(),
            //            listOscDataProcess[item].shiftPointsRough,
            //            dataAfterRoughProcess,
            //            0, listOscDataProcess[item].memDepth);
            //        //补充首尾数据
            //        for (int i = 0; i < inData.Length; i++)
            //        {
            //            if (i < interMutiple * 4)
            //            {
            //                inData[i] = dataAfterRoughProcess[startPos];
            //            }
            //            else if ((i >= interMutiple * 4) && (i < interMutiple * 4 + pointNum))
            //            {
            //                inData[i] = dataAfterRoughProcess[startPos + i - interMutiple * 4];
            //            }
            //            else
            //            {
            //                inData[i] = dataAfterRoughProcess[stopPos];
            //            }

            //        }
            //        outData = cSinc_Inter.sincConvolut_Cal(euInterMutipleDefault[offsetPos], ref inData);
            //        //保存插值并移动后的点= 点数*插值倍数 - （2*内插倍数-1）
            //        //为了平移，多截一个点
            //        double[] outDataAfterShift = new double[pointNum * interMutiple - 2 * interMutiple + 1];
            //        //计算出的插值结果需要再移动相应的精细移动点数
            //        //需要舍弃插零的点
            //        for (int i = 0; i < outDataAfterShift.Length; i++)
            //        {
            //            outDataAfterShift[i] = outData[i + shiftPointsFin + interMutiple * 4 * interMutiple];
            //        }
            //        //峰峰值抽样出需要显示的10K点
            //        //从原始数据中抽取10k点
            //        if (outDataAfterShift.Length > pointDisplay)
            //        {
            //            CP2PCompress<double> cP2P_Compress = new CP2PCompress<double>();
            //            cP2P_Compress.waveCompressAlgorithm(ref outDataAfterShift,
            //                (UInt32)outDataAfterShift.Length,
            //                ref listOscDataProcess[item].dataDispFin,
            //                (UInt32)pointDisplay);

            //            //换算到电压值-未加上实际的偏移
            //            for (int i = 0; i < listOscDataProcess[item].dataDispFin.Length; i++)
            //            {
            //                listOscDataProcess[item].dataDispFin[i] = (listOscDataProcess[item].dataDispFin[i] - (listOscDataProcess[item].yOrigin + listOscDataProcess[item].yReference)) * listOscDataProcess[item].yIncrement;
            //            }
            //        }
            //    });


            for (int item = 0; item < listOscDataProcess.Count; item++)
            {
                if (listOscDataProcess[item].strChanID == "Tag018")
                {
                }
                //保存当前内插倍数
                listOscDataProcess[item].curInterMutilpe = interMutiple;
                //计算经过插值后两点之间最佳间隔时间
                double timIntervalFin = (double)(listOscDataProcess[item].xIncrement * CGlobalValue.S_TRANS_PS) / interMutiple;
                //计算插值后精细移动的点数 = 剩余的精细时间/插值后的时间间隔
                int shiftPointsFin = (int)(listOscDataProcess[item].finAgj / timIntervalFin);
                //根据内插倍数进行插值计算,插值点数为原始点补数，补的数量为插值倍数*8
                double[] outData = new double[(pointNum + interMutiple * 8) * interMutiple];
                int[] inData = new int[pointNum + interMutiple * 8];
                byte[] dataAfterRoughProcess = new byte[listOscDataProcess[item].memDepth];
                Buffer.BlockCopy(Module_DeviceManage.Instance.GetDeviceBySN(listOscDataProcess[item].strDevSN).GetChannel((int)listOscDataProcess[item].euChan).GetData(),
                    listOscDataProcess[item].shiftPointsRough,
                    dataAfterRoughProcess,
                    0, listOscDataProcess[item].memDepth);
                //补充首尾数据
                for (int i = 0; i < inData.Length; i++)
                {
                    if (i < interMutiple * 4)
                    {
                        inData[i] = dataAfterRoughProcess[startPos];
                    }
                    else if ((i >= interMutiple * 4) && (i < interMutiple * 4 + pointNum))
                    {
                        inData[i] = dataAfterRoughProcess[startPos + i - interMutiple * 4];
                    }
                    else
                    {
                        inData[i] = dataAfterRoughProcess[stopPos];
                    }

                }
                outData = cSinc_Inter.sincConvolut_Cal(euInterMutipleDefault[offsetPos], ref inData);
                //保存插值并移动后的点= 点数*插值倍数 - （2*内插倍数-1）
                //为了平移，多截一个点
                //double[] outDataAfterShift = new double[pointNum * interMutiple - 2 * interMutiple + 1];
                double[] outDataAfterShift = new double[pointNum * interMutiple];
                //计算出的插值结果需要再移动相应的精细移动点数
                //需要舍弃插零的点
                for (int i = 0; i < outDataAfterShift.Length; i++)
                {
                    outDataAfterShift[i] = outData[i + shiftPointsFin + interMutiple * 4 * interMutiple];
                }
                //峰峰值抽样出需要显示的10K点
                //从原始数据中抽取10k点
                if (outDataAfterShift.Length > pointDisplay)
                {
                    CP2PCompress<double> cP2P_Compress = new CP2PCompress<double>();
                    cP2P_Compress.waveCompressAlgorithm(ref outDataAfterShift,
                        (UInt32)outDataAfterShift.Length,
                        ref listOscDataProcess[item].dataDispFin,
                        (UInt32)pointDisplay);

                    //换算到电压值-未加上实际的偏移
                    for (int i = 0; i < listOscDataProcess[item].dataDispFin.Length; i++)
                    {
                        listOscDataProcess[item].dataDispFin[i] = (listOscDataProcess[item].dataDispFin[i] - (listOscDataProcess[item].yOrigin + listOscDataProcess[item].yReference)) * listOscDataProcess[item].yIncrement;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 波形放大操作（调用C库实现）
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="stopPos"></param>
        /// <param name="interMutiple"></param>
        /// <returns></returns>
        public bool calEnlargeWaveData_UsingCDLL(int startPos, int stopPos, int interMutiple)
        {
            if (startPos > stopPos)
            {
                return false;
            }
            //放大的最小限制10个点
            if (stopPos <= startPos + 9)
            {
                return false;
            }
            int pointNum = stopPos - startPos + 1;
            // 大于屏幕显示的2倍时，压缩显示
            if (pointNum >= pointDisplay*2)
            {
                //并行计算
                Parallel.For(0, listOscDataProcess.Count,
                    item =>
                    {
                        //从原始数据中抽取1k点
                        byte[] waveDataTemp = new byte[pointDisplay];
                        // temp暂存位置，暂无实际用途
                        int[] temp = new int[2];
                        waveCompAlgorithm4UCharSimple(Module_DeviceManage.Instance.GetDeviceBySN(listOscDataProcess[item].strDevSN).GetChannel((int)listOscDataProcess[item].euChan).GetData(),
                            startPos + listOscDataProcess[item].shiftPointsRough,
                            pointNum,
                            waveDataTemp,
                            pointDisplay,
                            temp);
                        //waveCompAlgorithm4UCharSimple(Module_DeviceManage.Instance.GetDeviceBySN(listOscDataProcess[item].strDevSN).GetChannel((int)listOscDataProcess[item].euChan).GetData(),
                        //    startPos,
                        //    pointNum,
                        //    waveDataTemp,
                        //    pointDisplay,
                        //    temp);
                        for (int i = 0; i < pointDisplay; i++)
                        {
                            //换算到实际的电压值
                            //listOscDataProcess[item].dataDispRough[i] = (double)(waveDataTemp[i] - (listOscDataProcess[item].yOrigin + listOscDataProcess[item].yReference)) * listOscDataProcess[item].yIncrement;
                            listOscDataProcess[item].dataDispFin[i] = (double)(waveDataTemp[i] - (listOscDataProcess[item].yOrigin + listOscDataProcess[item].yReference)) * listOscDataProcess[item].yIncrement;
                        }
                    });
            }
            else
            {
                // 如果小于1K点，先插值，从左侧开始显示1K点
                int offsetPos = 0;
                //先根据内插倍数，找到实际内插使用的枚举值
                for (int i = 0; i < interMutipleDefault.Length; i++)
                {
                    if (interMutiple == interMutipleDefault[i])
                    {
                        offsetPos = i;
                        break;
                    }
                }

                //并行计算
                Parallel.For(0, listOscDataProcess.Count,
                    item =>
                    {
                        //保存当前内插倍数
                        listOscDataProcess[item].curInterMutilpe = interMutiple;
                        //计算经过插值后两点之间最佳间隔时间
                        double timIntervalFin = (double)(listOscDataProcess[item].xIncrement * CGlobalValue.S_TRANS_PS) / interMutiple;
                        //计算插值后精细移动的点数 = 剩余的精细时间/插值后的时间间隔
                        int shiftPointsFin = (int)(listOscDataProcess[item].finAgj / timIntervalFin);
                        // c库计算插值
                        //int s32CompDataLen = pointNum * interMutiple - 2 * interMutiple + 1;
                        int s32CompDataLen = pointNum * interMutiple;
                        double[] outDataAfterShift = new double[s32CompDataLen];
                        getSincFilterDataAddOffset(Module_DeviceManage.Instance.GetDeviceBySN(listOscDataProcess[item].strDevSN).GetChannel((int)listOscDataProcess[item].euChan).GetData(),
                        startPos + listOscDataProcess[item].shiftPointsRough,
                        stopPos + listOscDataProcess[item].shiftPointsRough,
                        (int)euInterMutipleDefault[offsetPos],
                        outDataAfterShift,
                        shiftPointsFin + interMutiple * 4 * interMutiple,
                        outDataAfterShift.Length);
                        //getSincFilterDataAddOffset(Module_DeviceManage.Instance.GetDeviceBySN(listOscDataProcess[item].strDevSN).GetChannel((int)listOscDataProcess[item].euChan).GetData(),
                        //startPos ,
                        //stopPos ,
                        //(int)euInterMutipleDefault[offsetPos],
                        //outDataAfterShift,
                        //shiftPointsFin + interMutiple * 4 * interMutiple,
                        //outDataAfterShift.Length);
                        //从左侧起截取1k的点
                        for (int i = 0; i < pointDisplay; i++)
                        {
                            listOscDataProcess[item].dataDispFin[i] = outDataAfterShift[i];
                        }
                        //换算到电压值-未加上实际的偏移
                        for (int i = 0; i < listOscDataProcess[item].dataDispFin.Length; i++)
                        {
                            listOscDataProcess[item].dataDispFin[i] = (listOscDataProcess[item].dataDispFin[i] - (listOscDataProcess[item].yOrigin + listOscDataProcess[item].yReference)) * listOscDataProcess[item].yIncrement;
                        }
                    });
            }
            return true;
        }
        /// <summary>
        /// 根据当前点数选择最佳的内插倍数
        /// </summary>
        /// <param name="memDepth"></param>
        /// <param name="pointNum"></param>
        /// <returns>内插倍数</returns>
        public int findFinMutiple(int memDepth, int pointNum)
        {
            int result = 1;
            double  temp = memDepth / (pointNum*1.0);
            //在默认的内插倍数中选择最佳的内插倍数
            for (int i = 0; i < interMutipleDefault.Length; i++)
            {
                if (temp >= interMutipleDefault[interMutipleDefault.Length - 1])
                {
                    result = interMutipleDefault[interMutipleDefault.Length - 1];
                    break;
                }
                if (temp == interMutipleDefault[i])
                {
                    result = interMutipleDefault[i];
                    break;
                }
                if ((temp > interMutipleDefault[i]) && (temp < interMutipleDefault[i + 1]))
                {
                    result = interMutipleDefault[i+1];
                    break;
                }
            }
            return result;
        }
        #region 测量功能
        /// <summary>
        /// 找最大值
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public int findMaxValue_MemeoryData(string strID)
        {
            int pos = -1;
            int index = listOscDataProcess.FindIndex(item => item.strChanID == strID);
            if (index != -1)
            {
                byte[] dataAfterRoughProcess = new byte[listOscDataProcess[index].memDepth];
                Buffer.BlockCopy(Module_DeviceManage.Instance.GetDeviceBySN(listOscDataProcess[index].strDevSN).GetChannel((int)listOscDataProcess[index].euChan).GetData(),
                    listOscDataProcess[index].shiftPointsRough,
                    dataAfterRoughProcess,
                    0, listOscDataProcess[index].memDepth);

                byte temp = dataAfterRoughProcess[0];
                for (int i = 1; i < dataAfterRoughProcess.Length; i++)
                {
                    if (temp < dataAfterRoughProcess[i])
                    {
                        temp = dataAfterRoughProcess[i];
                        pos = i;
                    }
                }
            }
            else
            {
                pos = -1;
            }
            return pos;
        }
        /// <summary>
        /// 找打最小值
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public int findMinValue_MemeoryData(string strID)
        {
            int pos = -1;
            int index = listOscDataProcess.FindIndex(item => item.strChanID == strID);
            if (index != -1)
            {
                byte[] dataAfterRoughProcess = new byte[listOscDataProcess[index].memDepth];
                Buffer.BlockCopy(Module_DeviceManage.Instance.GetDeviceBySN(listOscDataProcess[index].strDevSN).GetChannel((int)listOscDataProcess[index].euChan).GetData(),
                    listOscDataProcess[index].shiftPointsRough,
                    dataAfterRoughProcess,
                    0, listOscDataProcess[index].memDepth);

                byte temp = dataAfterRoughProcess[0];
                for (int i = 1; i < dataAfterRoughProcess.Length; i++)
                {
                    if (temp > dataAfterRoughProcess[i])
                    {
                        temp = dataAfterRoughProcess[i];
                        pos = i;
                    }
                }
            }
            else
            {
                pos = -1;
            }
            return pos;
        }
        /// <summary>
        /// 查找第一个上升沿，并返回上升沿的峰值位置
        /// </summary>
        /// <param name="strID"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public int findFirstRisingEdge_MemoryData(string strID, int threshold)
        {
            int pos = -1;
            int index = listOscDataProcess.FindIndex(item => item.strChanID == strID);
            if (index != -1)
            {
                byte[] dataAfterRoughProcess = new byte[listOscDataProcess[index].memDepth];
                Buffer.BlockCopy(Module_DeviceManage.Instance.GetDeviceBySN(listOscDataProcess[index].strDevSN).GetChannel((int)listOscDataProcess[index].euChan).GetData(),
                    listOscDataProcess[index].shiftPointsRough,
                    dataAfterRoughProcess,
                    0, listOscDataProcess[index].memDepth);

                byte temp = dataAfterRoughProcess[0];
                int incNum = 0;
                int decContinue = 0;//连续减小的个数
                int pos_Temp = 0;
                for (int i = 1; i < dataAfterRoughProcess.Length; i++)
                {
                    pos = i;
                    if (dataAfterRoughProcess[i] >= temp)
                    {
                        decContinue = 0;
                        if (dataAfterRoughProcess[i] == temp)
                        {
                            temp = dataAfterRoughProcess[i];
                        }
                        else
                        {
                            incNum++;
                            pos_Temp = i;
                            temp = dataAfterRoughProcess[i];
                        }
                    }
                    else
                    {
                        /*下降超过10认为找到top点*/
                        if (temp - dataAfterRoughProcess[i] <= 10)
                        {
                            decContinue++;
                            if (decContinue == threshold)
                            {
                                /*连续3次下降则退出*/
                                if (incNum > threshold)
                                {
                                    decContinue = 0;
                                    pos = pos_Temp;
                                    break;
                                }
                                else//统计的上升个数不满足要求
                                {
                                    decContinue = 0;//清零
                                    pos_Temp = 0;//置位posTemp
                                    temp = dataAfterRoughProcess[i];
                                    incNum = 0;
                                }
                            }
                            else//连续下降不满3次
                            {
                                temp = dataAfterRoughProcess[i];
                            }
                        }
                        else
                        {
                            /*连续增加才判断为上升沿*/
                            if (incNum > threshold)
                            {
                                pos = pos_Temp;
                                break;
                            }
                            else
                            {
                                temp = dataAfterRoughProcess[i];
                                incNum = 0;
                            }
                        }
                    }
                }
            }
            else
            {
                pos = -1;
            }
            return pos;
        }
        /// <summary>
        /// 查找第一个峰的底部---》向左寻找
        /// </summary>
        /// <param name="strID"></param>
        /// <param name="top_Pos">第一个峰的峰顶位置</param>
        /// <returns></returns>
        public int findFirstRisingEdgeBottom_MemoryData(string strID, int top_Pos)
        {
            int pos = -1;
            int index = listOscDataProcess.FindIndex(item => item.strChanID == strID);
            if (index != -1)
            {
                byte[] dataAfterRoughProcess = new byte[listOscDataProcess[index].memDepth];
                Buffer.BlockCopy(Module_DeviceManage.Instance.GetDeviceBySN(listOscDataProcess[index].strDevSN).GetChannel((int)listOscDataProcess[index].euChan).GetData(),
                    listOscDataProcess[index].shiftPointsRough,
                    dataAfterRoughProcess,
                    0, listOscDataProcess[index].memDepth);

                byte temp = dataAfterRoughProcess[top_Pos];
                for (int i = top_Pos - 1; i >= 0; i--)
                {
                    if (dataAfterRoughProcess[i] <= temp)
                    {
                        temp = dataAfterRoughProcess[i];
                        pos = i;
                    }
                    else
                    {
                        pos = i + 1;
                        break;
                    }
                }
            }
            else
            {
                pos = -1;
            }
            return pos;
        }
        /// <summary>
        /// 查找对应通道ID的第一个峰的最大值
        /// </summary>
        /// <param name="strID">通道ID</param>
        /// <param name="searchScale"> 搜索的点的范围</param>
        /// <returns></returns>
        public int findFirstTop_MemeoryData(string strID, int searchScale, int threshold)
        {
            int pos = -1;
            int index = listOscDataProcess.FindIndex(item => item.strChanID == strID);
            if (index != -1)
            {
                byte[] dataAfterRoughProcess = new byte[listOscDataProcess[index].memDepth];
                Buffer.BlockCopy(Module_DeviceManage.Instance.GetDeviceBySN(listOscDataProcess[index].strDevSN).GetChannel((int)listOscDataProcess[index].euChan).GetData(),
                    listOscDataProcess[index].shiftPointsRough,
                    dataAfterRoughProcess,
                    0, listOscDataProcess[index].memDepth);
                byte temp = 0;
                int tempDevMax = 0;
                for (int i = 0; i < dataAfterRoughProcess.Length - searchScale; i++)
                {
                    temp = dataAfterRoughProcess[i];
                    tempDevMax = 0;
                    pos = i;
                    bool bResult = false;
                    for (int j = 1; j <= searchScale; j++)
                    {
                        if (temp > dataAfterRoughProcess[i + j])
                        {
                            bResult = true;
                            int tempDev = temp - dataAfterRoughProcess[i + j];
                            if (tempDev > tempDevMax)
                            {
                                tempDevMax = tempDev;
                            }
                        }
                        else
                        {
                            bResult = false;
                            break;
                        }
                    }
                    if (bResult && tempDevMax >= threshold)
                    {
                        break;
                    }
                }
            }
            else
            {
                pos = -1;
            }
            return pos;
        }
        /// <summary>
        /// 查找对应通道ID的第一个负峰
        /// </summary>
        /// <param name="strID"></param>
        /// <param name="searchScale"></param>
        /// <returns></returns>
        public int findFirstBottom_MemeoryData(string strID, int searchScale, int threshold)
        {
            int pos = -1;
            int index = listOscDataProcess.FindIndex(item => item.strChanID == strID);
            if (index != -1)
            {
                byte[] dataAfterRoughProcess = new byte[listOscDataProcess[index].memDepth];
                Buffer.BlockCopy(Module_DeviceManage.Instance.GetDeviceBySN(listOscDataProcess[index].strDevSN).GetChannel((int)listOscDataProcess[index].euChan).GetData(),
                    listOscDataProcess[index].shiftPointsRough,
                    dataAfterRoughProcess,
                    0, listOscDataProcess[index].memDepth);
                byte temp = 0;
                int tempDevMax = 0;
                for (int i = 0; i < dataAfterRoughProcess.Length - searchScale; i++)
                {
                    temp = dataAfterRoughProcess[i];
                    tempDevMax = 0;
                    pos = i;
                    bool bResult = false;
                    for (int j = 1; j <= searchScale; j++)
                    {
                        if (temp < dataAfterRoughProcess[i + j])
                        {
                            bResult = true;
                            int tempDev = dataAfterRoughProcess[i + j] - temp;
                            if (tempDev > tempDevMax)
                            {
                                tempDevMax = tempDev;
                            }
                        }
                        else
                        {
                            bResult = false;
                            break;
                        }
                    }
                    if (bResult)
                    {
                    }
                    if (bResult && tempDevMax >= threshold)
                    {
                        break;
                    }
                }
            }
            else
            {
                pos = -1;
            }
            return pos;
        }

        /// <summary>
        /// 查找第一个半峰高的位置
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        const int searchScale = 1000;
        public int findFirstMiddle_MemoryData(string strID)
        {
            int pos = -1;
            int index = listOscDataProcess.FindIndex(item => item.strChanID == strID);
            if (index != -1)
            {
                byte[] dataAfterRoughProcess = new byte[listOscDataProcess[index].memDepth];
                Buffer.BlockCopy(Module_DeviceManage.Instance.GetDeviceBySN(listOscDataProcess[index].strDevSN).GetChannel((int)listOscDataProcess[index].euChan).GetData(),
                    listOscDataProcess[index].shiftPointsRough,
                    dataAfterRoughProcess,
                    0, listOscDataProcess[index].memDepth);
                int top_Pos = findFirstRisingEdge_MemoryData(listOscDataProcess[index].strChanID, 5);
                int max_Pos = findMaxValue_MemeoryData(strID);
                if (top_Pos == -1)
                {
                    /*缩小范围再查一次*/
                    top_Pos = findFirstRisingEdge_MemoryData(listOscDataProcess[index].strChanID, 3);
                    if (top_Pos == -1)
                    {
                        top_Pos = findMaxValue_MemeoryData(strID);
                        if (top_Pos == -1)//说明没有对应的数据
                        {
                            pos = -1;
                            return pos;
                        }
                    }
                    else
                    {
                        /*防止误判*/
                        if ((max_Pos > top_Pos) && (max_Pos - top_Pos <= 100))
                        {
                            top_Pos = max_Pos;
                        }
                    }
                }
                else
                {
                    /*防止误判*/
                    if ((max_Pos > top_Pos) && (max_Pos - top_Pos <= 100))
                    {
                        top_Pos = max_Pos;
                    }
                }
                //int bottom_Pos = findFirstRisingEdgeBottom_MemoryData(m_listOscDataMemory[index].strChanID, top_Pos);
                int bottom_Pos = findMinValue_MemeoryData(listOscDataProcess[index].strChanID);
                if (bottom_Pos == -1)//说明没有对应的数据
                {
                    pos = -1;
                    return pos;
                }
                byte maxValue = dataAfterRoughProcess[top_Pos];
                byte minValue = dataAfterRoughProcess[bottom_Pos];
                byte midValue = (byte)((maxValue - minValue) / 2 + minValue);//计算中间值
                /*在bottom_Pos到top_Pos范围内寻找midvalue对应的位置*/
                for (int i = bottom_Pos; i < top_Pos; i++)
                {
                    if (dataAfterRoughProcess[i] >= midValue)
                    {
                        pos = i;
                        break;
                    }
                }
            }
            else
            {
                pos = -1;
            }

            return pos;
        }
        /// <summary>
        /// 测量半峰宽度
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public double getHafPeakWidthResult(string strID)
        {
            double halfPeakWidth = 0.0;
            int pos_left = 0;
            int pos_Right = 0;
            int index = listOscDataProcess.FindIndex(item => item.strChanID == strID);
            if (index != -1)
            {
                byte[] dataAfterRoughProcess = new byte[listOscDataProcess[index].memDepth];
                Buffer.BlockCopy(Module_DeviceManage.Instance.GetDeviceBySN(listOscDataProcess[index].strDevSN).GetChannel((int)listOscDataProcess[index].euChan).GetData(),
                    listOscDataProcess[index].shiftPointsRough,
                    dataAfterRoughProcess,
                    0, listOscDataProcess[index].memDepth);

                int top_Pos = findFirstRisingEdge_MemoryData(listOscDataProcess[index].strChanID, 5);
                int max_Pos = findMaxValue_MemeoryData(strID);
                if (top_Pos == -1)
                {
                    top_Pos = findFirstRisingEdge_MemoryData(listOscDataProcess[index].strChanID, 3);
                    if (top_Pos == -1)
                    {
                        top_Pos = findMaxValue_MemeoryData(strID);
                        if (top_Pos == -1)//说明没有对应的数据
                        {
                            return 0.0;
                        }
                    }
                    else
                    {
                        /*防止误判*/
                        if ((max_Pos > top_Pos) && (max_Pos - top_Pos <= 100))
                        {
                            top_Pos = max_Pos;
                        }
                    }
                }
                else
                {
                    /*防止误判*/
                    if ((max_Pos > top_Pos) && (max_Pos - top_Pos <= 100))
                    {
                        top_Pos = max_Pos;
                    }
                }
                //int bottom_Pos = findFirstRisingEdgeBottom_MemoryData(m_listOscDataMemory[index].strChanID, top_Pos);
                int bottom_Pos = findMinValue_MemeoryData(listOscDataProcess[index].strChanID);
                if (bottom_Pos == -1)//说明没有对应的数据
                {
                    return 0.0;
                }
                /*如果底部位置在顶部位置后*/
                //if (bottom_Pos > top_Pos)
                //{
                //    bottom_Pos = 0;
                //}
                byte maxValue = dataAfterRoughProcess[top_Pos];
                byte minValue = dataAfterRoughProcess[bottom_Pos];
                byte midValue = (byte)((maxValue - minValue) / 2 + minValue);//计算中间值
                /*在0到top_Pos范围内寻找midvalue对应的位置----->左侧值*/
                for (int i = 0; i < top_Pos; i++)
                {
                    if (dataAfterRoughProcess[i] >= midValue)
                    {
                        pos_left = i;
                        break;
                    }
                }
                /*在top_Pos到后续所有点范围内寻找midvalue对应的位置----->右侧值*/
                for (int i = top_Pos; i < dataAfterRoughProcess.Length; i++)
                {
                    if (dataAfterRoughProcess[i] <= midValue)
                    {
                        pos_Right = i;
                        break;
                    }
                }
                if ((pos_left == 0) || (pos_Right == 0))
                {
                    /*未找到*/
                    halfPeakWidth = 0.0;
                }
                else
                {
                    //string[] arr = listOscDataProcess[index].strPreAmple.Split(',');
                    double xincrement = listOscDataProcess[index].xIncrement;
                    halfPeakWidth = xincrement * (pos_Right - pos_left);
                }
                listOscDataProcess[index].wfmFirstHalfPeakWidth = halfPeakWidth;
            }
            else
            {
                halfPeakWidth = 0.0;
            }

            return halfPeakWidth;
        }
        /// <summary>
        /// 计算第一峰的峰值
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public double getFirstPeakValue(string strID)
        {
            double result = 0.0;
            int index = listOscDataProcess.FindIndex(item => item.strChanID == strID);
            if (index != -1)
            {
                byte[] dataAfterRoughProcess = new byte[listOscDataProcess[index].memDepth];
                Buffer.BlockCopy(Module_DeviceManage.Instance.GetDeviceBySN(listOscDataProcess[index].strDevSN).GetChannel((int)listOscDataProcess[index].euChan).GetData(),
                    listOscDataProcess[index].shiftPointsRough,
                    dataAfterRoughProcess,
                    0, listOscDataProcess[index].memDepth);

                //计算第一个峰的顶点
                int topPos = findFirstRisingEdge_MemoryData(strID, 5);
                //计算底部
                int bottomPos = findMinValue_MemeoryData(strID);
                double resultTop = (dataAfterRoughProcess[topPos] - (listOscDataProcess[index].yOrigin + listOscDataProcess[index].yReference)) * listOscDataProcess[index].yIncrement;
                double resultBottom = (dataAfterRoughProcess[bottomPos] - (listOscDataProcess[index].yOrigin + listOscDataProcess[index].yReference)) * listOscDataProcess[index].yIncrement;
                result = resultTop - resultBottom;
                listOscDataProcess[index].wfmFirstTopValue = resultTop;
                listOscDataProcess[index].wfmMinValue = resultBottom;
                listOscDataProcess[index].wfmFirstPeakValue = result;
            }
            return result;
        }
        #endregion
        #endregion

    }
}
