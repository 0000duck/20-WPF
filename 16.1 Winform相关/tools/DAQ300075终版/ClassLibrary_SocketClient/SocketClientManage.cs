using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary_SocketClient
{
    public class SocketClientManage
    {       
        private static bool IsConnectionSuccessful = false;
        private static Exception socketexception;
        private static ManualResetEvent TimeoutObject = new ManualResetEvent(false);
        public static  bool Connect(string ip,int port, int timeoutMSec)
        {
            TimeoutObject.Reset();
            socketexception = null;

           
            TcpClient tcpclient = new TcpClient();
            tcpclient.BeginConnect(ip, port,
                new AsyncCallback(CallBackMethod), tcpclient);

            if (TimeoutObject.WaitOne(timeoutMSec, false))
            {
                if (IsConnectionSuccessful)
                {
                    return true;
                }
                else
                {
                    throw socketexception;
                }
            }
            else
            {
                tcpclient.Close();
                return false;
            }           
        }
        private static void CallBackMethod(IAsyncResult asyncresult)
        {
            try
            {
                IsConnectionSuccessful = false;
                TcpClient tcpclient = asyncresult.AsyncState as TcpClient;

                if (tcpclient.Client != null)
                {
                    tcpclient.EndConnect(asyncresult);
                    IsConnectionSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                IsConnectionSuccessful = false;
                socketexception = ex;
            }
            finally
            {
                TimeoutObject.Set();
            }
        }


    }
}
