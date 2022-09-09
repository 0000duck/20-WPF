using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*****************************************************************************************************************
                                        普源精电科技有限公司版权所有(2020-)
******************************************************************************************************************
类名:      CGlobalValue
功能描述： CGlobalValue类中定义了saker中使用的全局数值
作 者：    sn02736
版 本：    00.01.00.00
完成日期： 
修改历史： 
<作者>               <修改时间>               <版本>                    <修改描述>
*****************************************************************************************************************/
namespace saker_Winform.Global
{
    public class CGlobalValue
    {
        #region Values
        public enum euChanID   //枚举每个设备的通道
        {
            CH1 = 1,
            CH2,
            CH3,
            CH4
        }
        public enum euChannelMode
        {
            NONE = 0,
            SINGLE = 1,
            DUALCH = 2,
            FOURCH = 4
        }

        public enum euMeasType // 枚举测量类型
        { 
            MARX = 0,
            WATERLINE = 1, // 水线
            INDUCTION = 2, // 感应腔
            MLT = 3
        }
        public const long S_TRANS_PS = 1000000000000;                          //s->ps的单位转换
        #endregion

    }
}
