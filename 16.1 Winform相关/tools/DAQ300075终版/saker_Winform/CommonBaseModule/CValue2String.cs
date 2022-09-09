using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using saker_Winform.Global;
/*****************************************************************************************************************
                                        普源精电科技有限公司版权所有(2020-)
******************************************************************************************************************
类名:      CValue2String
功能描述： CValue2String类实现各种数值转换到对应的字符串，带单位
作 者：    顾泽滔
版 本：    00.01.00.00
修改历史： 
<作者> <修改时间> <版本> <修改描述>
 顾泽滔       2020.5.9    初次修改
*****************************************************************************************************************/
namespace saker_Winform.CommonBaseModule
{
    class CValue2String
    {
        #region Methods
        /// <summary>
        /// 存储深度转字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string meDepth2String(float value)
        {
            string strMedpth = "";
            if (value / 1000000000 >= 1)
            {
                float result = value / 1000000000.0f;
                strMedpth = result.ToString() + CGlobalString.STR_MEDEPTH_GP;
            }
            else if (value / 1000000 >= 1)
            {
                float result = value / 1000000.0f;
                strMedpth = result.ToString() + CGlobalString.STR_MEDEPTH_MP;
            }
            else if (value / 1000 >= 1)
            {
                float result = value / 1000.0f;
                strMedpth = result.ToString() + CGlobalString.STR_MEDEPTH_KP;
            }
            else
            {
                strMedpth = value.ToString() + CGlobalString.STR_MEDEPTH_P;
            }
            return strMedpth;
        }
        /// <summary>
        /// 采样率转字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string acq2String(float value)
        {
            string strSamp = "";
            if (value / 1000000000 >= 1)
            {
                float result = value / 1000000000.0f;
                strSamp = result.ToString() + CGlobalString.STR_ACQ_GSA;
            }
            else if (value / 1000000 >= 1)
            {
                float result = value / 1000000.0f;
                strSamp = result.ToString() + CGlobalString.STR_ACQ_MSA;
            }
            else if (value / 1000 >= 1)
            {
                float result = value / 1000.0f;
                strSamp = result.ToString() + CGlobalString.STR_ACQ_KSA;
            }
            else
            {
                strSamp = value.ToString() + CGlobalString.STR_ACQ_SA;
            }
            return strSamp;
        }
        /// <summary>
        /// 垂直档位转换到字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string scal2String(double value)
        {
            string strScale = "";
            if (value >= 1.0)
            {
                strScale = Math.Round(value, 2).ToString() + CGlobalString.STR_VOLTAGE_V;
            }
            else if (value * 1000 >= 1.0)
            {
                if (value * 1000 >= 100)
                {
                    strScale = Math.Round(value * 1000, 0).ToString() + CGlobalString.STR_VOLTAGE_MV;
                }
                else if (value * 1000 >= 10)
                {
                    strScale = Math.Round(value * 1000, 1).ToString() + CGlobalString.STR_VOLTAGE_MV;
                }
                else
                {
                    strScale = Math.Round(value * 1000, 2).ToString() + CGlobalString.STR_VOLTAGE_MV;
                }
            }
            else
            {
                strScale = "0.00" + CGlobalString.STR_VOLTAGE_V;
            }
            return strScale;
        }
        /// <summary>
        /// 偏移转换为字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string voltage2String(double value)
        {
            string strOffset = "";
            if (Math.Abs(value) >= 1.0)
            {
                strOffset = Math.Round(value, 2).ToString() + CGlobalString.STR_VOLTAGE_V;
            }
            else if (Math.Abs(value) * 1000 >= 1.0)
            {
                if (Math.Abs(value) * 1000 >= 100)
                {
                    strOffset = Math.Round(value * 1000, 0).ToString() + CGlobalString.STR_VOLTAGE_MV;
                }
                else if (Math.Abs(value) * 1000 >= 10)
                {
                    strOffset = Math.Round(value * 1000, 1).ToString() + CGlobalString.STR_VOLTAGE_MV;
                }
                else
                {
                    strOffset = Math.Round(value * 1000, 2).ToString() + CGlobalString.STR_VOLTAGE_MV;
                }
            }
            else if (Math.Abs(value) * 1000000 >= 1.0)
            {
                if (Math.Abs(value) * 1000000 >= 100)
                {
                    strOffset = Math.Round(value * 1000, 0).ToString() + CGlobalString.STR_VOLTAGE_UV;
                }
                else if (Math.Abs(value) * 1000000 >= 10)
                {
                    strOffset = Math.Round(value * 1000, 1).ToString() + CGlobalString.STR_VOLTAGE_UV;
                }
                else
                {
                    strOffset = Math.Round(value * 1000, 2).ToString() + CGlobalString.STR_VOLTAGE_UV;
                }
            }
            else
            {
                strOffset = "0.00" + CGlobalString.STR_VOLTAGE_V;
            }
            return strOffset;
        }
        /// <summary>
        /// 时间数据转字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string time2String(double value)
        {
            string strTime = "";
            if (Math.Abs(value) >= 1.0)
            {
                strTime = Math.Round(value, 2).ToString() + CGlobalString.STR_TIME_S;
            }
            else if (Math.Abs(value) * 1000 >= 1.0)
            {
                if (Math.Abs(value) * 1000 >= 100)
                {
                    strTime = Math.Round(value * 1000, 0).ToString() + CGlobalString.STR_TIME_MS;
                }
                else if (Math.Abs(value) * 1000 >= 10)
                {
                    strTime = Math.Round(value * 1000, 1).ToString() + CGlobalString.STR_TIME_MS;
                }
                else
                {
                    strTime = Math.Round(value * 1000, 2).ToString() + CGlobalString.STR_TIME_MS;
                }
            }
            else if (Math.Abs(value) * 1000000 >= 1.0)
            {
                if (Math.Abs(value) * 1000000 >= 100)
                {
                    strTime = Math.Round(value * 1000000, 6).ToString() + CGlobalString.STR_TIME_US;
                }
                else if (Math.Abs(value) * 1000000 >= 10)
                {
                    strTime = Math.Round(value * 1000000, 6).ToString() + CGlobalString.STR_TIME_US;
                }
                else
                {
                    strTime = Math.Round(value * 1000000, 6).ToString() + CGlobalString.STR_TIME_US;
                }
            }
            else if (Math.Abs(value) * 1000000000 >= 1.0)
            {
                if (Math.Abs(value) * 1000000000 >= 100)
                {
                    strTime = Math.Round(value * 1000000000, 3).ToString() + CGlobalString.STR_TIME_NS;
                }
                else if (Math.Abs(value) * 1000000000 >= 10)
                {
                    strTime = Math.Round(value * 1000000000, 3).ToString() + CGlobalString.STR_TIME_NS;
                }
                else
                {
                    strTime = Math.Round(value * 1000000000, 3).ToString() + CGlobalString.STR_TIME_NS;
                }
            }
            else if (Math.Abs(value) * 1000000000000 >= 1.0)
            {
                if (Math.Abs(value) * 1000000000000 >= 100)
                {
                    strTime = Math.Round(value * 1000000000000, 0).ToString() + CGlobalString.STR_TIME_PS;
                }
                else if (Math.Abs(value) * 1000000000000 >= 10)
                {
                    strTime = Math.Round(value * 1000000000000, 1).ToString() + CGlobalString.STR_TIME_PS;
                }
                else
                {
                    strTime = Math.Round(value * 1000000000000, 2).ToString() + CGlobalString.STR_TIME_PS;
                }
            }
            else
            {
                strTime = "0.00s";
            }
            return strTime;
        }
        #endregion
    }
}
