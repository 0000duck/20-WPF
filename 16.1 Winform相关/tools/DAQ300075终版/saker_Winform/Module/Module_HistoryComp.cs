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
类名:      Module_HistoryComp
功能描述： Module_HistoryComp类中定义了历史数据对比界面中所需数据的处理
           继承Module_WaveMonitor类
作 者：    sn02736
版 本：    00.01.00.00
完成日期： 
修改历史： 
<作者>               <修改时间>               <版本>                    <修改描述>
*****************************************************************************************************************/
namespace saker_Winform.Module
{
    public class Module_HistoryComp : Module_WaveMonitor
    {
        #region Fields
        /// <summary>
        /// 示波器内存获取的数据
        /// </summary>
        public class HistoryDataOriginal
        {
            public string strTag;//通道标示

            public string strDevName;// 设备别名

            public string strTestTim;// 测试时间

            public string strDevSN;//设备序列号

            public int chanID;//对应设备的实际通道

            public string strMeasType;//对应通道的测量类型

            public int chanMode;// 通道模式

            public int memDepth;//存储深度

            public double xIncrement;//x方向上相邻两点的时间差

            public double xOrigin;//x方向上数据的起始时间

            public double xReference;//x方向上数据的参考时间基准

            public double yIncrement;//y方向上波形的步进值

            public double yOrigin;//y方向上相对于“垂直参考位置”的垂直偏移

            public double yReference;//y方向的处置参考位置

            public double sampRate;//采样率

            public int trigTimStamp;//触发时间戳

            public int chanDelayTime;//通道相对于触发位置的精细延时

            public int devDelayTime;//多机之间的延时

            public byte[] waveData ; // 原始波形点1000008

        }
        /// <summary>
        /// 处理后的内存历史数据
        /// </summary>
        public class HistoryDataProcess : HistoryDataOriginal
        {

            public int totalTimStamp;//整个的时间戳

            public int roughAdj = 0;//粗略调整的时间戳

            public int finAgj = 0;//精细调整的时间戳

            public int shiftPointsRough;//粗调时位移的点数

            public double vertViewOffset = 0;//显示时的垂直偏移

            public int curInterMutilpe = 1;//当前内插倍数

            public double wfmHalfPeakWidth;//波形的半峰宽

            public double wfmDispP2PValue;//当前显示波形的峰峰值

            public double wfmDataMinValue;//当前显示波形的最小值

            public double wfmDataMaxValue;//当前显示波形的最大值

            public double wfmDataMidValue;//当前显示波形的半峰高

            public byte origWfmDataMidValue;//原始波形的半峰高

            public int midValuePosition;// 半峰高对应的位置（第一个沿）--对齐使用

            public int firstHalfPeakRisePos;// 第一个上升沿的半峰位置

            public int firstHalfPeakFallPos; //下降沿的半峰位置

            public int aglinLeftPoints = 0;// 向左移动的点数

            public double normalMutiple = 1.0;//归一化的放大倍数

            public Color lineColor = Color.Red;//当前通道波形的绘制颜色

            public double[] dataDispRough;//粗调后初始显示用的数据 ,压缩后

            public double[] dataDispFin;//细调后初始显示用的数据 ,压缩后

            public double[] dataDispAfterNormal;//归一化后的数据

            public double[] dataDispFinAfterAglin;//半峰高对齐后的数据

        }
        /*定义下位机每帧多上报的点数*/
        private const int pointsAdditional = 8;
        //定义屏幕显示的总点数
        private const int pointDisplay = 2000;
        /*显示与处理数据链表*/
        public List<HistoryDataProcess> listHistDataProcess = new List<HistoryDataProcess>();
        //设备延时
        public int devDelayMin = 0;
        #endregion

        #region Methods
        /// <summary>
        /// 获取当前选中的设备延时中的最小值
        /// </summary>
        void getDevDelayMin()
        {
            devDelayMin = 0;
        }
        /// <summary>
        /// 加载历史数据
        /// </summary>
        /// <param name="oscData"></param>
        public void modHistoryComp_Load(Module_WaveInfo oscData)
        {
            // start为每一次触发的采集时间
            int index = listHistDataProcess.FindIndex(item => (item.strTag == oscData.Tag) && ((item.strTestTim == oscData.StartTime)));
            int[] posTemp = new int[2];//用于保存抽样后的起始和终止点
            if (index != -1)
            {
                // 该元素已存在
                listHistDataProcess[index].strTag = oscData.Tag;
                listHistDataProcess[index].strDevName = oscData.DeviceName;
                listHistDataProcess[index].strTestTim = oscData.StartTime;
                listHistDataProcess[index].strDevSN = oscData.SN;
                listHistDataProcess[index].chanID = oscData.ChannelID;
                listHistDataProcess[index].strMeasType = oscData.MeasureType;
                listHistDataProcess[index].chanMode = oscData.ChannelMode;
                listHistDataProcess[index].memDepth = CString2Value.meDepth2Value(oscData.MemDepth);
                listHistDataProcess[index].xIncrement = Convert.ToDouble(oscData.XIncrement);
                listHistDataProcess[index].xOrigin = Convert.ToDouble(oscData.XOrigin);
                listHistDataProcess[index].xReference = Convert.ToDouble(oscData.XReference);
                listHistDataProcess[index].yReference = Convert.ToDouble(oscData.YReference);
                listHistDataProcess[index].yOrigin = Convert.ToDouble(oscData.YOrigin);
                listHistDataProcess[index].yIncrement = Convert.ToDouble(oscData.YIncrement);
                listHistDataProcess[index].sampRate = Convert.ToDouble(oscData.SampRate);
                listHistDataProcess[index].trigTimStamp = Convert.ToInt32(oscData.TrigTimeStamp);
                listHistDataProcess[index].chanDelayTime = Convert.ToInt32(oscData.ChannelDelayTime);
                listHistDataProcess[index].devDelayTime = Convert.ToInt32(oscData.DeviceDelayTime);
                
                
                // 根据当前通道判断取余的大小
                int stampTime = 100;
                stampTime = oscData.ChannelMode * 100;
                if (oscData.ChannelMode == 0)
                {
                    stampTime = 400;
                }
                //计算通道的整体偏移时间
                listHistDataProcess[index].totalTimStamp =  listHistDataProcess[index].trigTimStamp % stampTime + listHistDataProcess[index].chanDelayTime + listHistDataProcess[index].devDelayTime - devDelayMin;
                //listHistDataProcess[index].totalTimStamp = listHistDataProcess[index].chanDelayTime + listHistDataProcess[index].devDelayTime - devDelayMin;
                //计算粗移的位置点数
                listHistDataProcess[index].shiftPointsRough = listHistDataProcess[index].totalTimStamp / (int)(listHistDataProcess[index].xIncrement * CGlobalValue.S_TRANS_PS);
                if (listHistDataProcess[index].shiftPointsRough >= pointsAdditional)
                {
                    //最多粗移8个点
                    //listHistDataProcess[index].shiftPointsRough = pointsAdditional;
                }
                //计算粗略调整的时间戳
                listHistDataProcess[index].roughAdj = listHistDataProcess[index].shiftPointsRough * (int)(listHistDataProcess[index].xIncrement * CGlobalValue.S_TRANS_PS);
                //计算需要细调的时间戳
                listHistDataProcess[index].finAgj = listHistDataProcess[index].totalTimStamp - listHistDataProcess[index].roughAdj;
                //从原始数据中抽取10k点
                byte[] waveDataTemp = new byte[pointDisplay];
                // 先对波形进行预处理，完成粗移的点数
                //origWaveDataPreProcess4UChar(oscData.Data, listHistDataProcess[index].shiftPointsRough);
                waveCompAlgorithm4UCharSimple(oscData.Data,
                    listHistDataProcess[index].shiftPointsRough,
                    listHistDataProcess[index].memDepth,
                    waveDataTemp,
                    pointDisplay,
                    posTemp);
                //waveCompAlgorithm4UCharSimple(oscData.Data,
                //    0,
                //    listHistDataProcess[index].memDepth,
                //    waveDataTemp,
                //    pointDisplay,
                //    posTemp);
                for (int i = 0; i < pointDisplay; i++)
                {
                    //换算到实际的电压值
                    listHistDataProcess[index].dataDispRough[i] = (double)(waveDataTemp[i] - (listHistDataProcess[index].yOrigin + listHistDataProcess[index].yReference)) * listHistDataProcess[index].yIncrement;
                    listHistDataProcess[index].dataDispFin[i] = (double)(waveDataTemp[i] - (listHistDataProcess[index].yOrigin + listHistDataProcess[index].yReference)) * listHistDataProcess[index].yIncrement;
                }
                // 复制原始数据
                Buffer.BlockCopy(oscData.Data, 0, listHistDataProcess[index].waveData, 0, oscData.Data.Length);
            }
            else
            {
                //新增元素
                HistoryDataProcess histDataProcess = new HistoryDataProcess();

                histDataProcess.strTag = oscData.Tag;
                histDataProcess.strDevName = oscData.DeviceName;
                histDataProcess.strTestTim = oscData.StartTime;
                histDataProcess.strDevSN = oscData.SN;
                histDataProcess.chanID = oscData.ChannelID;
                histDataProcess.strMeasType = oscData.MeasureType;
                histDataProcess.chanMode = oscData.ChannelMode;
                histDataProcess.memDepth = CString2Value.meDepth2Value(oscData.MemDepth);
                histDataProcess.xIncrement = Convert.ToDouble(oscData.XIncrement);
                histDataProcess.xOrigin = Convert.ToDouble(oscData.XOrigin);
                histDataProcess.xReference = Convert.ToDouble(oscData.XReference);
                histDataProcess.yReference = Convert.ToDouble(oscData.YReference);
                histDataProcess.yOrigin = Convert.ToDouble(oscData.YOrigin);
                histDataProcess.yIncrement = Convert.ToDouble(oscData.YIncrement);
                histDataProcess.sampRate = Convert.ToDouble(oscData.SampRate);
                histDataProcess.trigTimStamp = Convert.ToInt32(oscData.TrigTimeStamp);
                histDataProcess.chanDelayTime = Convert.ToInt32(oscData.ChannelDelayTime);
                histDataProcess.devDelayTime = Convert.ToInt32(oscData.DeviceDelayTime);

                //默认显示的点数为1k
                histDataProcess.dataDispRough = new double[pointDisplay];
                histDataProcess.dataDispFin = new double[pointDisplay];
                histDataProcess.dataDispAfterNormal = new double[pointDisplay];
                histDataProcess.dataDispFinAfterAglin = new double[pointDisplay];
                histDataProcess.waveData = new byte[histDataProcess.memDepth + pointsAdditional];
                // 根据当前通道判断取余的大小
                int stampTime = 100;
                stampTime = oscData.ChannelMode * 100;
                if (oscData.ChannelMode == 0)
                {
                    stampTime = 400;
                }
                //计算通道的整体偏移时间
                histDataProcess.totalTimStamp =  histDataProcess.trigTimStamp % stampTime + histDataProcess.chanDelayTime + histDataProcess.devDelayTime - devDelayMin;
                //histDataProcess.totalTimStamp = histDataProcess.chanDelayTime + histDataProcess.devDelayTime - devDelayMin;
                //计算粗移的位置点数
                histDataProcess.shiftPointsRough = histDataProcess.totalTimStamp / (int)(histDataProcess.xIncrement * CGlobalValue.S_TRANS_PS);
                if (histDataProcess.shiftPointsRough >= pointsAdditional)
                {
                    //最多粗移8个点
                    //histDataProcess.shiftPointsRough = pointsAdditional;
                }
                //计算粗略调整的时间戳
                histDataProcess.roughAdj = histDataProcess.shiftPointsRough * (int)(histDataProcess.xIncrement * CGlobalValue.S_TRANS_PS);
                //计算需要细调的时间戳
                histDataProcess.finAgj = histDataProcess.totalTimStamp - histDataProcess.roughAdj;
                //从原始数据中抽取10k点
                byte[] waveDataTemp = new byte[pointDisplay];
                // 先对波形进行预处理，完成粗移的点数
                //origWaveDataPreProcess4UChar(oscData.Data, histDataProcess.shiftPointsRough);
                waveCompAlgorithm4UCharSimple(oscData.Data,
                    histDataProcess.shiftPointsRough,
                    histDataProcess.memDepth,
                    waveDataTemp,
                    pointDisplay,
                    posTemp);
                //waveCompAlgorithm4UCharSimple(oscData.Data,
                //    0,
                //    histDataProcess.memDepth,
                //    waveDataTemp,
                //    pointDisplay,
                //    posTemp);
                // 更新抽样后的初始位置和终止位置
                for (int i = 0; i < pointDisplay; i++)
                {
                    //换算到实际的电压值
                    histDataProcess.dataDispRough[i] = (double)(waveDataTemp[i] - (histDataProcess.yOrigin + histDataProcess.yReference)) * histDataProcess.yIncrement;
                    histDataProcess.dataDispFin[i] = (double)(waveDataTemp[i] - (histDataProcess.yOrigin + histDataProcess.yReference)) * histDataProcess.yIncrement;
                }
                // 复制原始数据
                Buffer.BlockCopy(oscData.Data, 0, histDataProcess.waveData, 0, oscData.Data.Length);
                /*添加到链表*/
                listHistDataProcess.Add(histDataProcess);
            }
        }
        /// <summary>
        /// 波形放大对齐操作（调用C库实现）
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="stopPos"></param>
        /// <param name="interMutiple"></param>
        /// <returns></returns>
        public new bool calEnlargeWaveData_UsingCDLL(int startPos, int stopPos, int interMutiple)
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
            // 大于屏幕显示的2倍时，压缩显示
            if (pointNum >= pointDisplay * 2)
            {
                //并行计算
                Parallel.For(0, listHistDataProcess.Count,
                    item =>
                    {
                        listHistDataProcess[item].curInterMutilpe = interMutiple;
                        //从原始数据中抽取1k点
                        byte[] waveDataTemp = new byte[pointDisplay];
                        // temp暂存位置，暂无实际用途
                        int[] temp = new int[2];
                        waveCompAlgorithm4UCharSimple(listHistDataProcess[item].waveData,
                            startPos + listHistDataProcess[item].shiftPointsRough,
                            pointNum,
                            waveDataTemp,
                            pointDisplay,
                            temp);
                        //waveCompAlgorithm4UCharSimple(listHistDataProcess[item].waveData,
                        //    startPos ,
                        //    pointNum,
                        //    waveDataTemp,
                        //    pointDisplay,
                        //    temp);
                        for (int i = 0; i < pointDisplay; i++)
                        {
                            //换算到实际的电压值
                            listHistDataProcess[item].dataDispFin[i] = (double)(waveDataTemp[i] - (listHistDataProcess[item].yOrigin + listHistDataProcess[item].yReference)) * listHistDataProcess[item].yIncrement;
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
                Parallel.For(0, listHistDataProcess.Count,
                    item =>
                    {
                        //保存当前内插倍数
                        listHistDataProcess[item].curInterMutilpe = interMutiple;
                        //计算经过插值后两点之间最佳间隔时间
                        double timIntervalFin = (double)(listHistDataProcess[item].xIncrement * CGlobalValue.S_TRANS_PS) / interMutiple;
                        //计算插值后精细移动的点数 = 剩余的精细时间/插值后的时间间隔
                        int shiftPointsFin = (int)(listHistDataProcess[item].finAgj / timIntervalFin);
                        // c库计算插值
                        //int s32CompDataLen = pointNum * interMutiple - 2 * interMutiple + 1;
                        int s32CompDataLen = pointNum * interMutiple;
                        double[] outDataAfterShift = new double[s32CompDataLen];
                        //进行插值，插值后需要移除前面补充的点
                        getSincFilterDataAddOffset(listHistDataProcess[item].waveData,
                        startPos + listHistDataProcess[item].shiftPointsRough,
                        stopPos + listHistDataProcess[item].shiftPointsRough,
                        (int)euInterMutipleDefault[offsetPos],
                        outDataAfterShift,
                        shiftPointsFin + interMutiple * 4 * interMutiple,
                        outDataAfterShift.Length);
                        //getSincFilterDataAddOffset(listHistDataProcess[item].waveData,
                        //startPos,
                        //stopPos,
                        //(int)euInterMutipleDefault[offsetPos],
                        //outDataAfterShift,
                        //shiftPointsFin + interMutiple * 4 * interMutiple,
                        //outDataAfterShift.Length);
                        //从左侧起截取1k的点
                        for (int i = 0; i < pointDisplay; i++)
                        {
                            listHistDataProcess[item].dataDispFin[i] = outDataAfterShift[i];
                        }
                        // 不截取，直接抽取
                        // temp暂存位置，暂无实际用途
                        //int[] temp = new int[2];
                        //if (outDataAfterShift.Length > pointDisplay)
                        //{
                        //    double[] waveDataTemp = new double[pointDisplay];
                        //    waveCompAlgorithm4DoubleSimple(outDataAfterShift,
                        //    0,
                        //    outDataAfterShift.Length,
                        //    waveDataTemp,
                        //    pointDisplay,
                        //    temp);
                        //    for (int i = 0; i < pointDisplay; i++)
                        //    {
                        //        listHistDataProcess[item].dataDispFin[i] = waveDataTemp[i];
                        //    }
                        //}
                        //else
                        //{
                        //    for (int i = 0; i < pointDisplay; i++)
                        //    {
                        //        listHistDataProcess[item].dataDispFin[i] = outDataAfterShift[i];
                        //    }
                        //}
                        //换算到电压值-未加上实际的偏移
                        for (int i = 0; i < listHistDataProcess[item].dataDispFin.Length; i++)
                        {
                            listHistDataProcess[item].dataDispFin[i] = (listHistDataProcess[item].dataDispFin[i] - (listHistDataProcess[item].yOrigin + listHistDataProcess[item].yReference)) * listHistDataProcess[item].yIncrement;
                        }
                    });
            }
            return true;
        }
        /// <summary>
        /// 归一化波形数据操作
        /// </summary>
        public void normalWaveData()
        {
            // 先计算每个显示波形的峰峰值
            foreach (HistoryDataProcess item in listHistDataProcess)
            {
                // 根据当前显示波形的数据，找到对应的最大值和最小值
                findMinAndMaxValue(ref item.dataDispFin,ref item.wfmDataMinValue,ref item.wfmDataMaxValue);
                item.wfmDispP2PValue = item.wfmDataMaxValue - item.wfmDataMinValue;
            }
            // 查找出最大的峰峰值
            double p2pValueMax = listHistDataProcess.Max(t => t.wfmDispP2PValue);

            // 计算每个波形的归一化参数
            for (int i = 0; i < listHistDataProcess.Count; i++)
            {
                listHistDataProcess[i].normalMutiple = p2pValueMax / listHistDataProcess[i].wfmDispP2PValue;
            }
            // 计算归一化的波形
            // 1.先统一乘上归一化的倍率
            foreach (HistoryDataProcess item in listHistDataProcess)
            {
                item.wfmDataMinValue *= item.normalMutiple;
                item.wfmDataMaxValue *= item.normalMutiple;
                for(int i = 0;i<item.dataDispFin.Length;i++)
                {
                    item.dataDispAfterNormal[i] = item.dataDispFin[i] * item.normalMutiple;
                }
            }
            // 2.再计算偏移
            // 获取最大值,其他波形都向最大值靠拢
            double maxValue = listHistDataProcess.Max(t => t.wfmDataMaxValue);
            foreach (HistoryDataProcess item in listHistDataProcess)
            {
                for (int i = 0; i < item.dataDispAfterNormal.Length; i++)
                {
                    item.dataDispAfterNormal[i] = item.dataDispAfterNormal[i] + maxValue - item.wfmDataMaxValue;
                    item.dataDispFin[i] = item.dataDispAfterNormal[i];
                }
            }
        }
        /// <summary>
        /// 半峰高对齐操作
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="stopPos"></param>
        /// <param name="interMutiple"></param>
        public bool  aglinWaveData(int startPos,int stopPos)
        {
            // 先计算当前所有波形的半峰高,并查找半峰高的位置
            foreach (HistoryDataProcess item in listHistDataProcess)
            {
                if (item.curInterMutilpe == 1)
                {
                    // 无内插的情况，根据原始数据进行对齐
                    int posTemp = findmidValuePositionRise(ref item.waveData, 5);
                    if (posTemp == -1)
                    {
                        item.midValuePosition = findmidValuePositionRise(ref item.waveData, 3);
                        if (item.midValuePosition == -1)
                        {
                            byte maxTemp = item.waveData.Max();
                            byte minTemp = item.waveData.Min();
                            item.origWfmDataMidValue = (byte)((maxTemp + minTemp) / 2);
                            for (int i = 0; i < item.waveData.Length -1; i++)
                            {
                                if ((item.origWfmDataMidValue >= item.waveData[i]) && (item.origWfmDataMidValue <= item.waveData[i + 1]))
                                {
                                    item.midValuePosition = i;
                                    break;
                                }
                            }
                            if (item.midValuePosition == -1)
                            {
                                return false ;
                            }
                        }
                        else
                        {
                            item.origWfmDataMidValue = item.waveData[item.midValuePosition];
                        }
                    }
                    else
                    {
                        item.midValuePosition = posTemp;
                        item.origWfmDataMidValue = item.waveData[item.midValuePosition];
                    }
                }
                else
                {
                    // 有内插的情况，根据当前显示的数据进行对齐
                    int posTemp = findmidValuePositionRise(ref item.dataDispFin, 3);
                    if (posTemp == -1)
                    {
                        item.midValuePosition = findmidValuePositionRise(ref item.dataDispFin, 2);
                        if (item.midValuePosition == -1)
                        {
                            double maxTemp = item.dataDispFin.Max();
                            double minTemp = item.dataDispFin.Min();
                            item.wfmDataMidValue = (maxTemp + minTemp) / 2.0;

                            for (int i = 0; i < item.dataDispFin.Length - 1; i++)
                            {
                                if ((item.wfmDataMidValue >= item.dataDispFin[i]) && (item.wfmDataMidValue <= item.dataDispFin[i + 1]))
                                {
                                    item.midValuePosition = i;
                                    break;
                                }
                            }
                            if (item.midValuePosition == -1)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            item.origWfmDataMidValue = item.waveData[item.midValuePosition];
                        }
                    }
                    else
                    {
                        item.midValuePosition = posTemp;
                        item.wfmDataMidValue = item.dataDispFin[item.midValuePosition];
                    }
                }
            }
            // 找到半峰高的最小值，向左对齐
            int minPosition = listHistDataProcess.Min( t => t.midValuePosition);
            if (minPosition == 1)
            {
                return false;
            }
            foreach (HistoryDataProcess item in listHistDataProcess)
            {
                item.aglinLeftPoints = item.midValuePosition - minPosition;
                int stopPosTemp = stopPos;
                int startPosTemp = startPos;
                if (item.curInterMutilpe == 1)
                {
                    // 当前不插值,首先计算移位后是否超出内存
                    int endPosTemp = startPosTemp + item.aglinLeftPoints;
                    if (endPosTemp < item.memDepth + 8)
                    {
                        stopPosTemp += item.aglinLeftPoints;
                        startPosTemp += item.aglinLeftPoints;
                    }
                    else
                    {
                        return false;
                    }
                    //if (endPosTemp < item.memDepth + 8)
                    //{
                    //    stopPosTemp += item.aglinLeftPoints;
                    //    startPosTemp += item.aglinLeftPoints;
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                    // 计算当前每个波形需要向左移动的点数
                    int pointNum = stopPosTemp - startPosTemp + 1;
                    byte[] waveDataTemp = new byte[pointDisplay];
                    // temp暂存位置，暂无实际用途
                    int[] temp = new int[2];
                    waveCompAlgorithm4UCharSimple(item.waveData,
                        startPosTemp,
                        pointNum,
                        waveDataTemp,
                        pointDisplay,
                        temp);
                    for (int i = 0; i < pointDisplay; i++)
                    {
                        //换算到实际的电压值
                        item.dataDispFinAfterAglin[i] = (double)(waveDataTemp[i] - (item.yOrigin + item.yReference)) * item.yIncrement;
                    }
                }
                else
                { 
                    //当前有插值,更具当前显示
                    //先计算插值后两点之间最佳间隔时间
                    double timIntervalFin = (double)(item.xIncrement * CGlobalValue.S_TRANS_PS) / item.curInterMutilpe;
                    //计算插值后精细移动的点数 = 剩余的精细时间/插值后的时间间隔
                    int shiftPointsFin = (int)(item.finAgj / timIntervalFin);
                    // 计算当前每个波形需要向左移动的点数
                    int pointNum = stopPos - startPos + 1;
                    // c库计算插值
                    //int s32CompDataLen = pointNum * interMutiple - 2 * interMutiple + 1;
                    int s32CompDataLen = pointNum * item.curInterMutilpe;
                    int offsetPos = 0;
                    //先根据内插倍数，找到实际内插使用的枚举值
                    for (int i = 0; i < interMutipleDefault.Length; i++)
                    {
                        if (item.curInterMutilpe == interMutipleDefault[i])
                        {
                            offsetPos = i;
                            break;
                        }
                    }
                    // 如果放大后的数据满足位移条件
                    if (s32CompDataLen - pointDisplay >= item.aglinLeftPoints)
                    {
                        double[] outDataAfterShift = new double[s32CompDataLen];
                        //插值加上位移+半峰高移动的位置
                        getSincFilterDataAddOffset(item.waveData,
                        startPos + item.shiftPointsRough,
                        stopPos + item.shiftPointsRough,
                        (int)euInterMutipleDefault[offsetPos],
                        outDataAfterShift,
                        shiftPointsFin + item.curInterMutilpe * 4 * item.curInterMutilpe + item.aglinLeftPoints,
                        outDataAfterShift.Length);
                        //getSincFilterDataAddOffset(item.waveData,
                        //startPos,
                        //stopPos,
                        //(int)euInterMutipleDefault[offsetPos],
                        //outDataAfterShift,
                        //shiftPointsFin + item.curInterMutilpe * 4 * item.curInterMutilpe + item.aglinLeftPoints,
                        //outDataAfterShift.Length);
                        //从左侧起截取1k的点
                        for (int i = 0; i < pointDisplay; i++)
                        {
                            item.dataDispFinAfterAglin[i] = outDataAfterShift[i];
                        }
                        //换算到电压值-未加上实际的偏移
                        for (int i = 0; i < item.dataDispFinAfterAglin.Length; i++)
                        {
                            item.dataDispFinAfterAglin[i] = (item.dataDispFinAfterAglin[i] - (item.yOrigin + item.yReference)) * item.yIncrement;
                        }
                    }
                    else
                    {
                        // 不满足位移条件，stop位置向后移动
                        int numLack = item.aglinLeftPoints - (s32CompDataLen - pointDisplay);
                        // 计算原始点需要移动的点数
                        int origNumLack = (int)Math.Ceiling(numLack / (item.curInterMutilpe*1.0));

                        int stopTemp = stopPos;

                        stopTemp += origNumLack;
                        //if (stopTemp + origNumLack < item.memDepth + 8)
                        //{
                        //    stopTemp += origNumLack;
                        //}
                        //else
                        //{
                        //    return false ;
                        //}
                        // 更新点数
                        pointNum = stopTemp - startPos + 1;
                        // 更新插值后的点数
                        s32CompDataLen = pointNum * item.curInterMutilpe;
                        // 申请空间
                        double[] outDataAfterShift = new double[s32CompDataLen];
                        //插值加上位移+半峰高移动的位置
                        getSincFilterDataAddOffset(item.waveData,
                        startPos + item.shiftPointsRough,
                        stopTemp + item.shiftPointsRough,
                        (int)euInterMutipleDefault[offsetPos],
                        outDataAfterShift,
                        shiftPointsFin + item.curInterMutilpe * 4 * item.curInterMutilpe + item.aglinLeftPoints,
                        outDataAfterShift.Length);
                        //getSincFilterDataAddOffset(item.waveData,
                        //startPos ,
                        //stopTemp ,
                        //(int)euInterMutipleDefault[offsetPos],
                        //outDataAfterShift,
                        //shiftPointsFin + item.curInterMutilpe * 4 * item.curInterMutilpe + item.aglinLeftPoints,
                        //outDataAfterShift.Length);
                        //从左侧起截取1k的点
                        for (int i = 0; i < pointDisplay; i++)
                        {
                            item.dataDispFinAfterAglin[i] = outDataAfterShift[i];
                        }
                        //换算到电压值-未加上实际的偏移
                        for (int i = 0; i < item.dataDispFinAfterAglin.Length; i++)
                        {
                            item.dataDispFinAfterAglin[i] = (item.dataDispFinAfterAglin[i] - (item.yOrigin + item.yReference)) * item.yIncrement;
                        }
                    }
                }
                // 复制数据到dataDisFin
                for (int i = 0; i < item.dataDispFin.Length; i++)
                {
                    item.dataDispFin[i] = item.dataDispFinAfterAglin[i];
                }
            }
            return true;
        }
        /// <summary>
        /// 查找目标数据源的最大值和最小值
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        private void findMinAndMaxValue(ref double[] dataSource,ref double minValue, ref double maxValue)
        {
            maxValue = dataSource[0];
            minValue = dataSource[0];
            for (int i = 0; i < dataSource.Length; i++)
            {
                if (dataSource[i] > maxValue)
                {
                    maxValue = dataSource[i];
                }
                if (dataSource[i] < minValue)
                {
                    minValue = dataSource[i];
                }
            }
        }
        /// <summary>
        /// 查找半峰高的位置-上升沿
        /// </summary>
        /// <param name="waveData">数据源</param>
        /// <param name="threShold">连续增长的次数</param>
        /// <returns></returns>
        public int findmidValuePositionRise(ref byte[] waveData,int threShold)
        {
            int pos = 0;
            int posLower = 0;
            int posHigher = 0;
            // 默认最大值和最小的一半位置为中值点
            byte maxTemp = waveData.Max();
            byte minTemp = waveData.Min();

            byte minTempLower = (byte)(minTemp - (Math.Abs(minTemp) * 0.05));
            byte minTempHigher = (byte)(minTemp + (Math.Abs(minTemp) * 0.05));
            int minPositon = 0;
            byte wfmDataMidValue = (byte)((maxTemp + minTemp) / 2);
            // 规定噪声因子
            byte noiseFactor = (byte)(wfmDataMidValue * 0.05);
            int incNum = 0;
            // 先找底部位置
            for (int i = 0; i < waveData.Length; i++)
            {
                if ((waveData[i] > minTempLower) && (waveData[i] < minTempHigher))
                {
                    minPositon = i;
                    break;
                }
            }
            for (int i = minPositon; i < waveData.Length; i++)
            {
                // 判断连续上升
                if (waveData[i] >= wfmDataMidValue - noiseFactor)
                {
                    incNum++;
                    if (incNum > threShold)
                    {
                        posLower = i;
                        break;
                    }
                }
                else
                {
                    incNum = 0;
                }
            }
            // 清零
            incNum = 0;
            for (int i = posLower; i < waveData.Length; i++)
            {
                // 判断连续上升
                if (waveData[i] >= wfmDataMidValue + noiseFactor)
                {
                    incNum++;
                    if (incNum > threShold)
                    {
                        posHigher = i;
                        break;
                    }
                }
                else
                {
                    incNum = 0;
                }
            }
            // 判断是否有效
            if (posHigher < posLower)
            {
                pos = -1;
            }
            else
            {
                pos = (posLower + posHigher) / 2;
            }
            return pos;
        }
        /// <summary>
        /// 查找半峰高的位置-上升沿
        /// </summary>
        /// <param name="waveData"></param>
        /// <param name="threShold"></param>
        /// <param name="startPos"></param>
        /// <returns></returns>
        public int findmidValuePositionRise(ref byte[] waveData, int threShold, int startPos)
        {
            int pos = 0;
            int posLower = 0;
            int posHigher = 0;
            // 默认最大值和最小的一半位置为中值点
            byte maxTemp = waveData.Max();
            byte minTemp = waveData.Min();

            byte minTempLower = (byte)(minTemp - (Math.Abs(minTemp) * 0.05));
            byte minTempHigher = (byte)(minTemp + (Math.Abs(minTemp) * 0.05));
            int minPositon = 0;
            byte wfmDataMidValue = (byte)((maxTemp + minTemp) / 2);
            // 规定噪声因子
            byte noiseFactor = (byte)(wfmDataMidValue * 0.05);
            int incNum = 0;
            // 先找底部位置
            for (int i = startPos; i < waveData.Length; i++)
            {
                if ((waveData[i] > minTempLower) && (waveData[i] < minTempHigher))
                {
                    minPositon = i;
                    break;
                }
            }
            for (int i = minPositon; i < waveData.Length; i++)
            {
                // 判断连续上升
                if (waveData[i] >= wfmDataMidValue - noiseFactor)
                {
                    incNum++;
                    if (incNum > threShold)
                    {
                        posLower = i;
                        break;
                    }
                }
                else
                {
                    incNum = 0;
                }
            }
            // 清零
            incNum = 0;
            for (int i = posLower; i < waveData.Length; i++)
            {
                // 判断连续上升
                if (waveData[i] >= wfmDataMidValue + noiseFactor)
                {
                    incNum++;
                    if (incNum > threShold)
                    {
                        posHigher = i;
                        break;
                    }
                }
                else
                {
                    incNum = 0;
                }
            }
            // 判断是否有效
            if (posHigher < posLower)
            {
                pos = -1;
            }
            else
            {
                pos = (posLower + posHigher) / 2;
            }
            return pos;
        }
        /// <summary>
        /// 查找半峰高的位置-下降沿
        /// </summary>
        /// <param name="waveData"></param>
        /// <param name="threShold"></param>
        /// <param name="startPos">搜索的起始位置</param>
        /// <returns></returns>
        public int findmidValuePositionFall(ref byte[] waveData, int threShold , int startPos)
        {
            int pos = 0;
            int posLower = 0;
            int posHigher = 0;
            // 默认最大值和最小的一半位置为中值点
            byte maxTemp = waveData.Max();
            byte minTemp = waveData.Min();

            byte maxTempLower = (byte)(maxTemp - (Math.Abs(maxTemp) * 0.05));
            byte maxTempHigher = (byte)(maxTemp + (Math.Abs(maxTemp) * 0.05));
            int maxPositon = 0;
            byte wfmDataMidValue = (byte)((maxTemp + minTemp) / 2);
            // 规定噪声因子
            byte noiseFactor = (byte)(wfmDataMidValue * 0.05);
            int incNum = 0;
            // 先找顶部位置,从规定的位置开始寻找
            for (int i = startPos; i < waveData.Length; i++)
            {
                if ((waveData[i] > maxTempLower) && (waveData[i] < maxTempHigher))
                {
                    maxPositon = i;
                    break;
                }
            }
            for (int i = maxPositon; i < waveData.Length; i++)
            {
                // 判断连续下降
                if (waveData[i] <= wfmDataMidValue + noiseFactor)
                {
                    incNum++;
                    if (incNum > threShold)
                    {
                        posLower = i;
                        break;
                    }
                }
                else
                {
                    incNum = 0;
                }
            }
            // 清零
            incNum = 0;
            for (int i = posLower; i < waveData.Length; i++)
            {
                // 判断连续下降
                if (waveData[i] <= wfmDataMidValue - noiseFactor)
                {
                    incNum++;
                    if (incNum > threShold)
                    {
                        posHigher = i;
                        break;
                    }
                }
                else
                {
                    incNum = 0;
                }
            }
            // 判断是否有效
            if (posHigher < posLower)
            {
                pos = -1;
            }
            else
            {
                pos = (posLower + posHigher) / 2;
            }
            return pos;
        }
        /// <summary>
        /// 查找半峰高的位置
        /// </summary>
        /// <param name="waveData">数据源</param>
        /// <param name="threShold">连续增长的次数</param>
        /// <returns></returns>
        public int findmidValuePositionRise(ref double[] waveData, int threShold)
        {
            int pos = 0;
            int posLower = 0;
            int posHigher = 0;
            // 默认最大值和最小的一半位置为中值点
            double maxTemp = waveData.Max();
            double minTemp = waveData.Min();

            double minTempLower = minTemp - (Math.Abs(minTemp) * 0.05);
            double minTempHigher = minTemp + (Math.Abs(minTemp) * 0.05);
            int minPositon = 0;

            double wfmDataMidValue = (maxTemp + minTemp) / 2.0;
            // 规定噪声因子
            double noiseFactor = wfmDataMidValue * 0.05;
            int incNum = 0;
            // 先找底部位置
            for (int i = 0; i < waveData.Length; i++)
            {
                if ((waveData[i] > minTempLower) && (waveData[i] < minTempHigher))
                {
                    minPositon = i;
                    break;
                }
            }
            for (int i = minPositon; i < waveData.Length; i++)
            {
                // 判断连续上升
                if (waveData[i] >= wfmDataMidValue - noiseFactor)
                {
                    incNum++;
                    if (incNum > threShold)
                    {
                        posLower = i;
                        break;
                    }
                }
                else
                {
                    incNum = 0;
                }
            }
            // 清零
            incNum = 0;
            for (int i = posLower; i < waveData.Length; i++)
            {
                // 判断连续上升
                if (waveData[i] >= wfmDataMidValue + noiseFactor)
                {
                    incNum++;
                    if (incNum > threShold)
                    {
                        posHigher = i;
                        break;
                    }
                }
                else
                {
                    incNum = 0;
                }
            }
            // 判断是否有效
            if (posHigher < posLower)
            {
                pos = -1;
            }
            else
            {
                pos = (posLower + posHigher) / 2;
            }
            return pos;
        }
        /// <summary>
        /// 计算波形的半峰宽，从原始数据进行计算
        /// </summary>
        public bool getHalfPeakWidth()
        {
            // 先找到一个峰的上升沿的半峰高位置
            foreach (HistoryDataProcess item in listHistDataProcess)
            {
                int posTemp = findmidValuePositionRise(ref item.waveData, 5);
                if (posTemp == -1)
                {
                    item.firstHalfPeakRisePos = findmidValuePositionRise(ref item.waveData, 3);
                    if (item.firstHalfPeakRisePos == -1)
                    {
                        byte maxTemp = item.waveData.Max();
                        byte minTemp = item.waveData.Min();
                        item.origWfmDataMidValue = (byte)((maxTemp + minTemp) / 2);
                        for (int i = 0; i < item.waveData.Length - 1; i++)
                        {
                            if ((item.origWfmDataMidValue >= item.waveData[i]) && (item.origWfmDataMidValue <= item.waveData[i + 1]))
                            {
                                item.firstHalfPeakRisePos = i;
                                break;
                            }
                        }
                        if (item.firstHalfPeakRisePos == -1)
                        {
                            return false ;
                        }
                    }
                    else 
                    {
                        item.origWfmDataMidValue = item.waveData[item.firstHalfPeakRisePos];
                    }
                }
                else
                {
                    item.firstHalfPeakRisePos = posTemp;
                    item.origWfmDataMidValue = item.waveData[item.firstHalfPeakRisePos];
                }
            }
            // 找到第一个峰的下降沿的半峰高位置
            foreach (HistoryDataProcess item in listHistDataProcess)
            {
                int posTemp = findmidValuePositionFall(ref item.waveData, 5, item.firstHalfPeakRisePos);
                if (posTemp == -1)
                {
                    item.firstHalfPeakFallPos = findmidValuePositionFall(ref item.waveData, 3, item.firstHalfPeakRisePos);
                    if (item.firstHalfPeakFallPos == -1)
                    {
                        byte maxTemp = item.waveData.Max();
                        byte minTemp = item.waveData.Min();
                        item.origWfmDataMidValue = (byte)((maxTemp + minTemp) / 2);
                        for (int i = item.firstHalfPeakRisePos; i < item.waveData.Length - 1; i++)
                        {
                            if ((item.origWfmDataMidValue <= item.waveData[i]) && (item.origWfmDataMidValue >= item.waveData[i + 1]))
                            {
                                item.firstHalfPeakFallPos = i;
                                break;
                            }
                        }
                    }
                    if (item.firstHalfPeakFallPos == -1)
                    {
                        return false;
                    }
                }
                else
                {
                    item.firstHalfPeakFallPos = posTemp;
                }
            }
            // 计算半峰高
            foreach (HistoryDataProcess item in listHistDataProcess)
            {
                item.wfmHalfPeakWidth = (item.firstHalfPeakFallPos - item.firstHalfPeakRisePos) * item.xIncrement;
            }
            return true;
        }
        /// <summary>
        /// 计算负脉宽
        /// </summary>
        /// <returns></returns>
        public bool getHalfPeakWidth_Neg()
        {
            // 先找到一个下降沿的半峰高位置
            foreach (HistoryDataProcess item in listHistDataProcess)
            {
                int posTemp = findmidValuePositionFall(ref item.waveData, 5,0);
                if (posTemp == -1)
                {
                    item.firstHalfPeakFallPos = findmidValuePositionFall(ref item.waveData, 3,0);
                    if (item.firstHalfPeakFallPos == -1)
                    {
                        byte maxTemp = item.waveData.Max();
                        byte minTemp = item.waveData.Min();
                        item.origWfmDataMidValue = (byte)((maxTemp + minTemp) / 2);
                        for (int i = 0; i < item.waveData.Length - 1; i++)
                        {
                            if ((item.origWfmDataMidValue <= item.waveData[i]) && (item.origWfmDataMidValue >= item.waveData[i + 1]))
                            {
                                item.firstHalfPeakFallPos = i;
                                break;
                            }
                        }
                        if (item.firstHalfPeakFallPos == -1)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        item.origWfmDataMidValue = item.waveData[item.firstHalfPeakFallPos];
                    }
                }
                else
                {
                    item.firstHalfPeakFallPos = posTemp;
                    item.origWfmDataMidValue = item.waveData[item.firstHalfPeakFallPos];
                }
            }
            // 找到第一个峰的上升沿的半峰高位置
            foreach (HistoryDataProcess item in listHistDataProcess)
            {
                int posTemp = findmidValuePositionRise(ref item.waveData, 5, item.firstHalfPeakFallPos);
                if (posTemp == -1)
                {
                    item.firstHalfPeakRisePos = findmidValuePositionRise(ref item.waveData, 3, item.firstHalfPeakFallPos);
                    if (item.firstHalfPeakRisePos == -1)
                    {
                        byte maxTemp = item.waveData.Max();
                        byte minTemp = item.waveData.Min();
                        item.origWfmDataMidValue = (byte)((maxTemp + minTemp) / 2);
                        for (int i = item.firstHalfPeakFallPos; i < item.waveData.Length - 1; i++)
                        {
                            if ((item.origWfmDataMidValue >= item.waveData[i]) && (item.origWfmDataMidValue <= item.waveData[i + 1]))
                            {
                                item.firstHalfPeakRisePos = i;
                                break;
                            }
                        }
                    }
                    if (item.firstHalfPeakRisePos == -1)
                    {
                        return false;
                    }
                }
                else
                {
                    item.firstHalfPeakRisePos = posTemp;
                }
            }
            // 计算半峰高
            foreach (HistoryDataProcess item in listHistDataProcess)
            {
                item.wfmHalfPeakWidth = (item.firstHalfPeakRisePos - item.firstHalfPeakFallPos) * item.xIncrement;
            }
            return true;
        }
        #endregion

    }
}
