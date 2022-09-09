using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace saker_Winform.CommonBaseModule
{
    class CPingIP
    {
        /// <summary>
        /// IP检测
        /// </summary>
        /// <param name="strIP"></param>
        /// <returns></returns>
        public static bool PingIpConnect(string strIP)
        {
              bool bRet = false;
              try
             {
                 Ping pingSend = new Ping();
                 PingReply reply = pingSend.Send(strIP, 50);
                 if (reply.Status == IPStatus.Success)
                     bRet = true;
             }
             catch (Exception)
             {
                 bRet = false;
             }
             return bRet;
         }

        
      /*  public static bool CheckIPAndPort()
        {
            Telnet p = new Telnet("192.168.1.100", 23, 50);

            if (p.Connect() == false)
            {
                Debug.WriteLine("连接失败");
                return;
            }
        }*/
    }
}
