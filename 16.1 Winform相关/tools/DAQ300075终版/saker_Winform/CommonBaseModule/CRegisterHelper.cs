using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saker_Winform.CommonBaseModule
{
    class CRegisterHelper
    {
        /// <summary>
        /// 写入注册表信息
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="data">data</param>
        /// <returns></returns>
        public static bool WriteRegisterKey(string name, string data)
        {
            int iReturn = 1;
            try
            {
                RegistryKey regWrite = Registry.CurrentUser.CreateSubKey("Saker\\MES");
                regWrite.SetValue(name, data);
                regWrite.Close();
            }
            catch (Exception ex)
            {
                iReturn = 2;
                throw ex;
            }
            return iReturn == 1;
        }

        /// <summary>
        /// 根据key值信息获取注册表信息
        /// </summary>
        /// <param name="aKey">key</param>
        /// <returns></returns>
        public static string GetRegisterKey(string aKey)
        {
            string reVal = "";
            RegistryKey softWareKey = Registry.CurrentUser.OpenSubKey("Saker\\MES");
            if (softWareKey != null)
            {
                object obj = softWareKey.GetValue(aKey);
                reVal = Convert.ToString(obj);
            }
            return reVal;
        }       
    }
}
