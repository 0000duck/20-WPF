using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ClassLibrary_SocketServer
{
    public class SocketManager
    {
        private int m_maxConnectNum;    //最大连接数  
        private int m_revBufferSize;    //最大接收字节数        
        const int opsToAlloc = 1;
        Socket listenSocket;            //监听Socket  
        SocketEventPool m_pool;
        int m_clientCount;              //连接的客户端数量  
        List<AsyncUserToken> m_clients; //客户端列表 
        public static List<SocketAsyncEventArgs> s_lst = new List<SocketAsyncEventArgs>();
        Semaphore m_maxNumberAcceptedClients;
        Semaphore m_semAcceptDataSync;
        //  public byte[] byteReadBuffer = new byte[1000 * 1000 * 4 + 8 * 4 + 10 ];
        private bool bLongLink = false;
        private Dictionary<string, bool> dic = new Dictionary<string, bool>();
        //计时器
        private Dictionary<string, int> receiveTimer = new Dictionary<string, int>();
        private Dictionary<string, int> receiveLen = new Dictionary<string, int>();
        //发送和接收的字符消息
        private byte[] Success = new byte[4];
        private byte[] Falied = new byte[] { 0xff, 0xff, 0xff, 0xff };

        #region 定义委托  

        /// <summary>  
        /// 客户端连接数量变化时触发  
        /// </summary>  
        /// <param name="num">当前增加客户的个数(用户退出时为负数,增加时为正数,一般为1)</param>  
        /// <param name="token">增加用户的信息</param>  
        public delegate void OnClientNumberChange(int num, AsyncUserToken token);

        /// <summary>  
        /// 接收到客户端的数据  
        /// </summary>  
        /// <param name="pool">客户端</param>  
        /// <param name="buff">客户端数据</param>  
        public delegate void OnReceiveData(AsyncUserToken token);

        #endregion

        #region 定义事件  
        /// <summary>  
        /// 客户端连接数量变化事件  
        /// </summary>  
        public event OnClientNumberChange ClientNumberChange;

        /// <summary>  x
        /// 接收到客户端的数据事件  
        /// </summary>  

        public event OnReceiveData ReceiveData;
        #endregion

        #region 定义属性  

        /// <summary>  
        /// 获取客户端列表  
        /// </summary>  
        public List<AsyncUserToken> ClientList { get { return m_clients; } }

        #endregion

        /// <summary>  
        /// 构造函数  
        /// </summary>  
        /// <param name="numConnections">最大连接数</param>  
        /// <param name="receiveBufferSize">缓存区大小</param>  
        public SocketManager(int numConnections, int receiveBufferSize, bool bState = false)
        {
            m_clientCount = 0;
            m_maxConnectNum = numConnections;
            m_revBufferSize = receiveBufferSize;

            // allocate buffers such that the maximum number of sockets can have one outstanding read and   
            //write posted to the socket simultaneously    
            //  m_bufferManager = new BufferManager(receiveBufferSize * numConnections * opsToAlloc, receiveBufferSize);    
            m_pool = new SocketEventPool(numConnections);
            m_maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);
            bLongLink = bState;
        }

        /// <summary>  
        /// 初始化  
        /// </summary>  
        public void Init()
        {
            // Allocates one large byte buffer which all I/O operations use a piece of.  This gaurds   
            // against memory fragmentation  
            // m_bufferManager.InitBuffer();  
            m_clients = new List<AsyncUserToken>();
            // preallocate pool of SocketAsyncEventArgs objects  
            for (int i = 0; i < m_maxConnectNum; i++)
            {
                byte[] buffer = new byte[50];
                SocketAsyncEventArgs readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                //初始化分配池子数量及其缓存区
                readWriteEventArg.UserToken = new AsyncUserToken(m_revBufferSize);
                // assign a byte buffer from the buffer pool to the SocketAsyncEventArg object                
                readWriteEventArg.SetBuffer(buffer, 0, buffer.Length);
                // add SocketAsyncEventArg to the pool              
                m_pool.Push(readWriteEventArg);                           

            }
        }
        /// <summary>  
        /// 启动服务  
        /// </summary>  
        /// <param name="localEndPoint"></param>  
        public void Start(IPEndPoint localEndPoint)
        {
            try
            {
                m_clients.Clear();
                listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listenSocket.Bind(localEndPoint);
                // start the server with a listen backlog of 100 connections  
                listenSocket.Listen(m_maxConnectNum);
                // post accepts on the listening socket  
                StartAccept(null);                
            }
            catch (Exception)
            {
               
            }
        }

        /// <summary>  
        /// 停止服务  
        /// </summary>  
        public void Stop()
        {
            for (int i = 0; i < m_clientCount; i++)
            {
                try
                {
                    m_clients[i].Socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("空间名：" + ex.Source + "；" + '\n' +
                      "方法名：" + ex.TargetSite + '\n' +
                      "故障点：" + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf("\\") + 1, ex.StackTrace.Length - ex.StackTrace.LastIndexOf("\\") - 1) + '\n' +
                      "错误提示：" + ex.Message);
                }
            }
            try
            {
                listenSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                Console.WriteLine("空间名：" + ex.Source + "；" + '\n' +
                      "方法名：" + ex.TargetSite + '\n' +
                      "故障点：" + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf("\\") + 1, ex.StackTrace.Length - ex.StackTrace.LastIndexOf("\\") - 1) + '\n' +
                      "错误提示：" + ex.Message);

            }
            listenSocket.Close();
            int c_count = m_clients.Count;
            lock (m_clients) { m_clients.Clear(); }

            if (ClientNumberChange != null)
                ClientNumberChange(-c_count, null);

        }

        public void CloseClient(AsyncUserToken token)
        {
            try
            {
                token.Socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }
        }
        // Begins an operation to accept a connection request from the client   
        //  
        // <param name="acceptEventArg">The context object to use when issuing   
        // the accept operation on the server's listening socket</param>  
        public void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
            }
            else
            {
                // socket must be cleared since the context object is being reused  
                acceptEventArg.AcceptSocket = null;
            }
            m_maxNumberAcceptedClients.WaitOne();
            if (!listenSocket.AcceptAsync(acceptEventArg))
            {
                ProcessAccept(acceptEventArg);
            }
        }

        // This method is the callback method associated with Socket.AcceptAsync   
        // operations and is invoked when an accept operation is complete  
        //  
        void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }
        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            try
            {
                IPAddress iPAddress = ((IPEndPoint)(e.AcceptSocket.RemoteEndPoint)).Address;
                Interlocked.Increment(ref m_clientCount);
                // Get the socket for the accepted client connection and put it into the   
                //ReadEventArg object user token  
                SocketAsyncEventArgs readEventArgs = m_pool.Pop();
                AsyncUserToken userToken = (AsyncUserToken)readEventArgs.UserToken;
                userToken.Socket = e.AcceptSocket;
                userToken.ConnectTime = DateTime.Now;
                userToken.Remote = e.AcceptSocket.RemoteEndPoint;
                userToken.IPAddress = iPAddress;
                readEventArgs.UserToken = userToken;
                Console.WriteLine(userToken.Socket.RemoteEndPoint.ToString() + " 建立连接:" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss  fff"));
                lock (m_clients)
                {
                    //检测如果发现客户中有相同的IP地址，则先进行关闭处理    
                    m_clients.Add(userToken);
                }
                if (ClientNumberChange != null)
                {
                    ClientNumberChange(1, userToken);
                }
                if (!e.AcceptSocket.ReceiveAsync(readEventArgs))
                {
                    ProcessReceive(readEventArgs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("空间名：" + ex.Source + "；" + '\n' +
                     "方法名：" + ex.TargetSite + '\n' +
                     "故障点：" + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf("\\") + 1, ex.StackTrace.Length - ex.StackTrace.LastIndexOf("\\") - 1) + '\n' +
                     "错误提示：" + ex.Message);
            }

            // Accept the next connection request  
            if (e.SocketError == SocketError.OperationAborted) return;
            StartAccept(e);
        }
        void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            // determine which type of operation just completed and call the associated handler  
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }

        }
        // This method is invoked when an asynchronous receive operation completes.   
        // If the remote host closed the connection, then the socket is closed.    
        // If data was received then the data is echoed back to the client.  
        //  
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            try
            {
                AsyncUserToken token = (AsyncUserToken)e.UserToken;
                string ip = token.IPAddress.ToString();
                byte[] data = new byte[e.BytesTransferred];
                Array.Copy(e.Buffer, e.Offset, data, 0, e.BytesTransferred);
                if (dic.Count == 0 || !dic.Keys.Contains(ip))
                {
                    dic.Add(ip, false);
                }
                if (dic[ip] == false)
                {
                    Console.WriteLine(ip + "开始收:" + DateTime.Now.ToString("hh:mm:ss fff"));
                }
                // check if the remote host closed the connection  
                if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
                {
                    dic[ip] = true;
                   // Console.WriteLine(e.BytesTransferred);
                    lock (token.Buffer)
                    {
                        token.Buffer.AddRange(data);
                    }
                    if (ReceiveData != null)
                    {
                        Console.WriteLine(ip + "完成收:" + DateTime.Now.ToString("hh:mm:ss fff") + "文件大小:" + token.Buffer.Count);
                        new Thread(() => ReceiveData(token)).Start();
                    }
                    else 
                    {
                        Console.WriteLine("监测服务关闭,不传递状态数据，清空buff");
                        token.Buffer.Clear();
                    }
                    if (!token.Socket.ReceiveAsync(e))
                    {
                        this.ProcessReceive(e);
                    }
                }
                else
                {                                       
                    dic[ip] = false;
                    CloseClientSocket(e);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("空间名：" + ex.Source + "；" + '\n' +
                      "方法名：" + ex.TargetSite + '\n' +
                      "故障点：" + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf("\\") + 1, ex.StackTrace.Length - ex.StackTrace.LastIndexOf("\\") - 1) + '\n' +
                      "错误提示：" + ex.Message);
            }
        }
        // This method is invoked when an asynchronous send operation completes.    
        // The method issues another receive on the socket to read any additional   
        // data sent from the client  
        //  
        // <param name="e"></param>  
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                // done echoing data back to the client  
                AsyncUserToken token = (AsyncUserToken)e.UserToken;
                // read the next block of data send from the client  
                bool willRaiseEvent = token.Socket.ReceiveAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessReceive(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }
        //关闭客户端  ,只能包含正常连接，不能够处理断电，断网异常，需要额外写心跳检测程序
        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = e.UserToken as AsyncUserToken;
            lock (m_clients) { m_clients.Remove(token); }
            //如果有事件,则调用事件,发送客户端数量变化通知  
            if (ClientNumberChange != null)
            {
                ClientNumberChange(-1, token);
            }
            // close the socket associated with the client  
            try
            {
                token.Socket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception) { }
            token.Socket.Close();
            // decrement the counter keeping track of the total number of clients connected to the server  
            Interlocked.Decrement(ref m_clientCount);
            m_maxNumberAcceptedClients.Release();
            // Free the SocketAsyncEventArg so they can be reused by another client  
            //  e.UserToken = new AsyncUserToken();
            token.Offset = 0;
            token.Write = 0;      
            e.UserToken = token;
            m_pool.Push(e);
            if (m_pool.Count == m_maxConnectNum)
            {
                Console.WriteLine("数据接收完成" + DateTime.Now.ToString());
            }
        }

        /// <summary>  
        /// 对数据进行打包,然后再发送  
        /// </summary>  
        /// <param name="token"></param>  
        /// <param name="message"></param>  
        /// <returns></returns>  
        public void SendMessage(AsyncUserToken token, byte[] message)
        {
            if (token == null || token.Socket == null || !token.Socket.Connected)
                return;
            try
            {
                //对要发送的消息,制定简单协议,头4字节指定包的大小,方便客户端接收(协议可以自己定)  


                token.Socket.Send(message);  //这句也可以发送, 可根据自己的需要来选择  
                

            

            }
            catch (Exception e)
            {
                throw (e);
            }
        }
      
  }
}
