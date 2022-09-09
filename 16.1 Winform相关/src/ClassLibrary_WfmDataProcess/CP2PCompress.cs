using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*****************************************************************************************************************
                                        普源精电科技有限公司版权所有(2020-)
******************************************************************************************************************
类名:      CP2PCompress
功能描述： CP2PCompress类实现峰峰值压缩算法
作 者：    顾泽滔
版 本：    00.01.00.00
修改历史： 
<作者> <修改时间> <版本> <修改描述>
*****************************************************************************************************************/
namespace ClassLibrary_WfmDataProcess
{
    public class CP2PCompress<T> where T : IComparable
    {
        #region Fields
        public class CExtremumValue
        {
            public UInt32 u32MinPos;
            public T minValue;
            public UInt32 u32MaxPos;
            public T maxValue;
        }
        #endregion

        #region Construction
        public CP2PCompress()
        { 
        }
        #endregion

        #region Methods
        /// <summary>
        /// 波形压缩算法-峰峰值压缩
        /// </summary>
        /// <param name="source"></param>
        /// <param name="u32SourceNum"></param>
        /// <param name="des"></param>
        /// <param name="u32DesNum"></param>
        public void waveCompressAlgorithm(ref T[] source, UInt32 u32SourceNum, ref T[] des, UInt32 u32DesNum)
        {
            UInt32 u32MaxPos, u32MinPos;
            T[] dot = new T[4];

            // 极值结构体
            CExtremumValue strExtremum = new CExtremumValue();

            // 临时数据压缩数据
            T[] tempDot = new T[u32DesNum * 2];

            // 先将原波形数据压缩成(u32DesNum * 2 - 2)个点
            //    u32 u32CompressNumTemp = u32DesNum * 2 - 2;
            UInt32 u32CompressNumTemp = u32DesNum * 2;

            // 计算压缩倍率
            float f32Multiple = (float)u32SourceNum / (float)u32CompressNumTemp * 2;

            // 计算循环次数
            UInt32 u32Cycle = u32CompressNumTemp / 2;

            UInt32 k = 0;
            //    for (u32 i = 0; i <= u32Cycle; i ++)
            for (UInt32 i = 0; i < u32Cycle; i++)
            {
                T[] temp = new T[(UInt32)f32Multiple];
                Array.Copy(source, (UInt32)(f32Multiple * i), temp, 0, (UInt32)f32Multiple);
                // 查找指定长度的最大值、最小值
                if (i == (u32Cycle - 1))
                {
                    //this.findExtremum(temp, (u32SourceNum - (UInt32)(f32Multiple * i)), ref strExtremum);
                    this.findExtremum(temp, (UInt32)temp.Length, ref strExtremum);
                }
                else
                {
                    this.findExtremum(temp, (UInt32)f32Multiple, ref strExtremum);
                }

                // 获得最大值、最小值的位置
                u32MaxPos = (UInt32)(f32Multiple * i) + strExtremum.u32MaxPos;
                u32MinPos = (UInt32)(f32Multiple * i) + strExtremum.u32MinPos;

                if (u32MinPos < u32MaxPos)
                {
                    tempDot[k] = source[u32MinPos];
                    tempDot[k + 1] = source[u32MaxPos];
                }
                else
                {
                    tempDot[k] = source[u32MaxPos];
                    tempDot[k + 1] = source[u32MinPos];
                }
                k += 2;
            }

            // 假如有4个数据在pf64TempDot中，pf64TempDot[0] < pf64TempDot[1], pf64TempDot[2] < pf64TempDot[3]
            // 在这4个数据中需要选择2点作为最终输出波形数据，重点是比较pf64TempDot[1]和pf64TempDot[2]是否是极值
            // 若无极值 pf64TempDot[0] < pf64TempDot[1] < pf64TempDot[2] < pf64TempDot[3],则取pf32Des[j] = (pf64TempDot[1] + pf64TempDot[2]) / 2
            // 若都是极值 pf64TempDot[2] < pf64TempDot[3] < pf64TempDot[0] < pf64TempDot[1],则取pf32Des[j] = pf64TempDot[1],pf64TempDot[i+2] = f64Dot[2]
            // 若只有一个极值 pf64TempDot[0] < pf64TempDot[2] < pf64TempDot[3] < pf64TempDot[1],则取pf32Des[j] = pf64TempDot[1]
            des[0] = tempDot[0];
            //    for (u32 i = 1, j = 0; i < k-1; i +=2, j++)
            for (UInt32 i = 1, j = 0; i < k; i += 2, j++)
            {

                dot[0] = des[j];
                dot[1] = tempDot[i];
                dot[2] = tempDot[i + 1];
                dot[3] = tempDot[i + 2];

                // 查找最大值、最小值
                this.findExtremum(dot, 4, ref strExtremum);

                u32MaxPos = strExtremum.u32MaxPos;
                u32MinPos = strExtremum.u32MinPos;

                if ((u32MinPos != 1) && (u32MaxPos != 2) && (u32MinPos != 2) && (u32MaxPos != 1))
                {
                    dynamic v1 = dot[1];
                    dynamic v2 = dot[2];
                    des[j] = (T)((v1+v2) / 2);
                }
                else if (((u32MinPos == 2) && (u32MaxPos == 1)) || ((u32MinPos == 1) && (u32MaxPos == 2)))
                {
                    des[j] = dot[1];
                    tempDot[i + 2] = dot[2];
                }
                else if ((u32MaxPos == 1) || (u32MinPos == 1))
                {
                    des[j] = dot[1];
                }
                else if ((u32MinPos == 2) || (u32MaxPos == 2))
                {
                    des[j] = dot[2];
                }

                des[j + 1] = tempDot[i];
                //溢出则跳出for循环
                if (i + 2 == k - 1)
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 寻找极值点
        /// </summary>
        /// <param name="value"></param>
        /// <param name="u32Length"></param>
        /// <param name="stExtremumValue"></param>
        private void findExtremum(T[] value, UInt32 u32Length, ref CExtremumValue stExtremumValue)
        {
            UInt32 u32MinPos = 0, u32MaxPos = 0;
            T minvalue , maxvalue ;

            //if (value.Length != 0)
            {
                minvalue = value[0];
                maxvalue = value[0];
            }

            // 查找极小值、极大值
            for (UInt32 i = 0; i < u32Length; i++)
            {
                if ( value[i].CompareTo(minvalue) < 0)
                {
                    u32MinPos = i;
                    minvalue = value[i];
                }
                else if (value[i].CompareTo(maxvalue) > 0)
                {
                    u32MaxPos = i;
                    maxvalue = value[i];
                }
            }

            stExtremumValue.u32MinPos = u32MinPos;
            stExtremumValue.minValue = minvalue;
            stExtremumValue.u32MaxPos = u32MaxPos;
            stExtremumValue.maxValue = maxvalue;
        }
        #endregion
    }
}
