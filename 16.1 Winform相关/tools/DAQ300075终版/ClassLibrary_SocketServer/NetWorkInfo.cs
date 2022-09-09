using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary_SocketServer
{
    public class NetWorkInfo
    {
         /// <summary>
        /// 定义本地网络信息
        /// </summary>
        /// <returns>item1:ip地址,item2:子网掩码,item3:默认网关,item4:广播地址</returns>
        private string _ip;

        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        private string _mask;

        public string Mask
        {
            get { return _mask; }
            set { _mask = value; }
        }
        private string _gateway;

        public string Gateway
        {
            get { return _gateway; }
            set { _gateway = value; }
        }

        private string _broadcast;
        public string Broadcast
        {
            get { return _broadcast; }
            set { _broadcast = value; }
        }
        public NetWorkInfo(string ip, string mask, string gateway, string broakcast)
        {
            // TODO: Complete member initialization
            this.Ip = ip;
            this.Mask = mask;
            this.Gateway = gateway;
            this.Broadcast = broakcast;
        }
    }
}
