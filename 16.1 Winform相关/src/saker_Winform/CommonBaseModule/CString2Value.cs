using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
/*****************************************************************************************************************
             普源精电科技有限公司版权所有(2020-)
******************************************************************************************************************
类名:      CString2Value
功能描述： CString2Value类对应的字符串带单位转换到数值
作 者：    顾泽滔
版 本：    00.01.00.00
修改历史： 
<作者> <修改时间> <版本> <修改描述>
顾泽滔       2020.5.27    初次修改
*****************************************************************************************************************/
namespace saker_Winform.CommonBaseModule
{
    class CString2Value
    {
        /// <summary>
        /// 内存深度转数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int meDepth2Value(string strValue)
        {
            int result = 0;
            strValue = strValue.ToUpper();
            if (strValue.Contains("M"))
            {
                int temp = Convert.ToInt32(strValue.Replace("M", ""));
                result = temp * 1000000;
            }
            else if (strValue.Contains("K"))
            {
                int temp = Convert.ToInt32(strValue.Replace("M", ""));
                result = temp * 1000;
            }
            return result;
        }

        #region 方法：限定只能输入数字
        /// <summary>
        /// 方法：限定只能输入数字
        /// </summary>
        /// <param name="e"></param>
        public static void LimitInput(KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9')) e.Handled = true;
            if (e.KeyChar == '\b'||e.KeyChar == '-') e.Handled = false;
        }

        public static void LimitInputIP(KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9')) e.Handled = true;
            if (e.KeyChar == '\b'|| e.KeyChar == '.') e.Handled = false;
        }

        public static void LimitInputNorCh(KeyPressEventArgs e,string text)
        {
            string pat = @"[\u4e00-\u9fff]";
            Regex rg = new Regex(pat);
            Match mch = rg.Match(text);
            if(!mch.Success)
            {
                e.Handled = true;
            }
        }
        #endregion
    }
}
