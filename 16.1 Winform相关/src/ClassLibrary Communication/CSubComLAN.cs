using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*****************************************************************************************************************
                                        普源精电科技有限公司版权所有(2020-)
******************************************************************************************************************
类名:      CSubComLAN
功能描述： CSubComLAN类是CCommunication类的子类，利用socket方式实现LAN通信功能，发现设备，建立通信，关闭通信，发送
           命令，接收反馈信息
作 者：    
版 本：    00.01.00.00
修改历史： 
<作者> <修改时间> <版本> <修改描述>

*****************************************************************************************************************/
namespace ClassLibrary_Communication
{
    class CSubComLAN:CCommunication
    {
        
        #region Properties
        #endregion

        #region Methods
        /// <summary>
        /// 实现具体发送
        /// </summary>
        /// <param name="">发送字符串</param>
        /// <returns>是否发送成功</returns>
        public override bool Send(string strSends)
        {
            return true;
        }
       
        /// <summary>
        /// 实现具体接收
        /// </summary>
        /// <returns>返回的字符串，如果读不回来，那么设置为null</returns>
        public override string Read()
        {
            return "";
        }
        #endregion
    }
}
