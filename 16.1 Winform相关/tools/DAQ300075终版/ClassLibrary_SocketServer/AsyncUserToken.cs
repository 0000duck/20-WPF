﻿using System;  
using System.Collections;  
using System.Collections.Generic;  
using System.Linq;  
using System.Net;  
using System.Net.Sockets;  
using System.Text; 

namespace ClassLibrary_SocketServer
{
    public class AsyncUserToken
    {
        /// <summary>  
        /// 客户端IP地址  
        /// </summary>  
        public IPAddress IPAddress { get; set; }  
  
        /// <summary>  
        /// 远程地址  
        /// </summary>  
        public EndPoint Remote { get; set; }  
  
        /// <summary>  
        /// 通信SOKET  
        /// </summary>  
        public Socket Socket { get; set; }  
  
        /// <summary>  
        /// 连接时间  
        /// </summary>  
        public DateTime ConnectTime { get; set; }  
  
        public int DataLength { get; set; }

        /// <summary>  
        /// 数据缓存区  
        /// </summary>  
        public List<byte> Buffer;

        
        public int RealSize { get; set; }
        public int Offset { get; set; }
        public int Write { get; set; }
        public AsyncUserToken(int dataLength)  
        {
            this.Offset = 0;
            this.Write = 0;
            this.DataLength = dataLength;           
            this.Buffer = new List<byte>();
        }  
    }     
}
