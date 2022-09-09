using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*****************************************************************************************************************
                                        普源精电科技有限公司版权所有(2020-)
******************************************************************************************************************
类名:      CGlobalString
功能描述： CGlobalString类中定义了saker中使用的字符串
作 者：    顾泽滔
版 本：    00.01.00.00
完成日期： 
修改历史： 
<作者>               <修改时间>               <版本>                    <修改描述>
*****************************************************************************************************************/
namespace saker_Winform.Global
{
    public class CGlobalString
    {
        #region Strings

        public const string STR_VOLTAGE_V = "V";
        public const string STR_VOLTAGE_MV = "mV";
        public const string STR_VOLTAGE_UV = "uV";

        public const string STR_TIME_S = "s";
        public const string STR_TIME_MS = "ms";
        public const string STR_TIME_US = "us";
        public const string STR_TIME_NS = "ns";
        public const string STR_TIME_PS = "ps";

        public const string STR_MEDEPTH_P = "pts";
        public const string STR_MEDEPTH_KP = "Kpts";
        public const string STR_MEDEPTH_MP = "Mpts";
        public const string STR_MEDEPTH_GP = "Gpts";

        public const string STR_ACQ_SA = "Sa/s";
        public const string STR_ACQ_KSA = "KSa/s";
        public const string STR_ACQ_MSA = "MSa/s";
        public const string STR_ACQ_GSA = "GSa/s";

       public const string label_DevSN = "仪器序列号：";
       public const string label_DevModel = "仪器型号：";
       public const string label_FirmVersion = "固件版本：";
       public const string label_DevNumber = "机器编号：";
       public const string label_DeviceSubName = "设备别名：";
       public const string label_DeviceIP = "IP 地址：";

        #endregion


    }

    public class TestInfo
    {
        

        public string[] strDevSN = new string[] { "101", "102", "103", "104", "105", "106", "107", "108", "109"};
        public string[] strDevIP = new string[] { "192.1.1.1", "192.1.1.2", "192.1.1.3", "192.1.1.4", "192.1.1.5", "192.1.1.6", "192.1.1.7", "192.1.1.8", "192.1.1.10" };
        public string[] strDevSubName = new string[] { "Device_001", "Device_002", "Device_003", "Device_004", "Device_005", "Device_006", "Device_007", "Device_008", "Device_009" };
        public string[] strDevModel = new string[] { "MSO7000", "MSO8000", "MSO7000", "MSO7000", "MSO7000", "MSO7000", "MSO7000", "MSO7000", "MSO7000" };
        public string[] strFirmVersion = new string[] { "1.1", "1.2", "1.3", "1.4", "1.5", "1.6", "1.7", "1.8", "1.9" };
        public string[] strRackNumber = new string[] { "101", "102", "103", "104", "105", "106", "107", "108", "109" };
        public bool[] bStatus = new bool[] { false, true, true, true, true, false, true, true, true, true };    
    }
}
