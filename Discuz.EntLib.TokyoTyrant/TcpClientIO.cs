using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace Discuz.EntLib.TokyoTyrant
{
    /// <summary>
    /// TcpClient IO工具类 
    /// 该类封装了TcpClient对象及其streams操作方法.
    /// </summary>
    public class TcpClientIO : IDisposable 
    {
        // id 生成器.用于给所有的TcpClientIO 实例ID赋值
        private static int IdGenerator;   
        // 创建时间
        private DateTime _created;

        private int _id;
        private TcpClientIOPool _pool;
        private String _host;
        private TcpClient _tcpClient;
 
        private TcpClientIO()
        {
            _id = Interlocked.Increment(ref IdGenerator);
            _created = DateTime.Now;
        }

        public TcpClient GetTcpClient()
        {
            return _tcpClient;
        }

        /// <summary>
        /// 构造一个TcpClientIO对象实例
        /// 链接到 host:port
        /// </summary>
        /// <param name="pool">该对象被绑定到的(链接)池</param>
        /// <param name="host">主机</param>
        /// <param name="port">端口</param>
        /// <param name="connectTimeout">用于初始化链接的过期时间(毫秒)</param>
        /// <param name="noDelay">是否延时</param>
        public TcpClientIO(TcpClientIOPool pool, String host, int port, int connectTimeout, bool noDelay) : this()
        {
            if (host == null || host.Length == 0)
                throw new ArgumentNullException(GetLocalizedString("host"), GetLocalizedString("null host"));

            _pool = pool;

            if (connectTimeout > 0)
                _tcpClient = GetTcpClient(host, port, connectTimeout);
            else
            {
                _tcpClient = new TcpClient(AddressFamily.InterNetwork);
                _tcpClient.Connect(new IPEndPoint(IPAddress.Parse(host), port));
            }
    
            _host = host + ":" + port;
        }

        /// <summary>
        /// 造一个TcpClientIO对象实例，链接到 host(包括端口)
        /// </summary>
        /// <param name="pool">该对象被绑定到的(链接)池</param>
        /// <param name="host">主机(含端口)</param>
        /// <param name="connectTimeout">用于初始化链接的过期时间(毫秒)</param>
        /// <param name="failover">失效转移(该判断暂未生效)</param>
        public TcpClientIO(TcpClientIOPool pool, String host, int connectTimeout, bool failover) : this()
        {
            if (host == null || host.Length == 0)
                throw new ArgumentNullException(GetLocalizedString("host"), GetLocalizedString("null host"));

            _pool = pool;
            String[] ip = host.Split(':');

            if (connectTimeout > 0)
                _tcpClient = GetTcpClient(ip[0], int.Parse(ip[1], new System.Globalization.NumberFormatInfo()), connectTimeout);
            else
            {
                _tcpClient = new TcpClient(AddressFamily.InterNetwork);
                _tcpClient.Connect(new IPEndPoint(IPAddress.Parse(ip[0]), int.Parse(ip[1], new System.Globalization.NumberFormatInfo())));
            }
            _tcpClient.NoDelay = true;
            _tcpClient.ReceiveBufferSize = 1024 * 100;
            _tcpClient.SendBufferSize = 1024 * 100;
            _host = host;
        }

        /// <summary>
        /// 使用线程方式获取链接实例并使用timeout来判断链接是否超时
        /// </summary>
        /// <param name="host">链接的主机</param>
        /// <param name="port">端口</param>
        /// <param name="timeout">用于初始化链接的过期时间(毫秒)</param>
        /// <returns>可用的TcpClient实例</returns>
        protected static TcpClient GetTcpClient(String host, int port, int timeout)
        {
            // 创建一个链接线程并启动它
            ConnectThread thread = new ConnectThread(host, port);
            thread.Start();

            int timer = 0;
            int sleep = 25;

            while (timer < timeout)
            {
                // 如果该线程中的TcpClient为已链接则返回它
                if (thread.IsConnected)
                    return thread.TcpClient;

                // 如线程有错误则抛出该错误
                if (thread.IsError)
                    throw new IOException();

                try
                {
                    // 休眼一小段时间
                    Thread.Sleep(sleep);
                }
                catch (ThreadInterruptedException) { }

                // 累加定时器
                timer += sleep;
            }

            // 如果在规定时间内未初始化链接时，则抛出链接超时异常
            throw new IOException(GetLocalizedString("connect timeout").Replace("$$timeout$$", timeout.ToString(new System.Globalization.NumberFormatInfo())));
        }

        /// <summary>
        /// 返回链接主机地址
        /// String 表示host (hostname:port)
        /// </summary>
        public string Host
        {
            get { return _host; }
        }

        /// <summary>
        /// 关闭TcpClient和所有已链接的streams 
        /// </summary>
        public void TrueClose()
        {
            bool err = false;
            StringBuilder errMsg = new StringBuilder();

            if (_tcpClient == null)
            {
                err = true;
                errMsg.Append(GetLocalizedString("TcpClient already closed"));
            }

            if (_tcpClient != null)
            {
                try
                {
                    _tcpClient.Close();
                }
                catch (IOException ioe)
                {
                    errMsg.Append(GetLocalizedString("error closing TcpClient").Replace("$$ToString$$", ToString()).Replace("$$Host$$", Host) + System.Environment.NewLine);
                    errMsg.Append(ioe.ToString());
                    err = true;
                }
                catch (SocketException soe)
                {
                    errMsg.Append(GetLocalizedString("error closing TcpClient").Replace("$$ToString$$", ToString()).Replace("$$Host$$", Host) + System.Environment.NewLine);
                    errMsg.Append(soe.ToString());
                    err = true;
                }
            }

            if (_tcpClient != null) _pool.CheckIn(this, false);

            _tcpClient = null;

            if (err) throw new IOException(errMsg.ToString());
        }

        /// <summary>
        /// 设置closed 标志位并将其放到链接池，但不关闭该链接
        /// </summary>
        public void Close()
        {
            _pool.CheckIn(this);
            //仅关闭流
            //_tcpClient.GetStream().Close();
        }

        /// <summary>
        /// 获取当前TcpClient 是否链接.
        /// </summary>
        public bool IsConnected
        {
            get { return _tcpClient != null && _tcpClient.Connected; }
        }

        /// <summary>
        /// 读一行数据
        /// </summary>
        /// <returns>返回读出的数据</returns>
        public string ReadLine()
        {
            if (_tcpClient == null || !_tcpClient.Connected)
                throw new IOException(GetLocalizedString("read closed TcpClient"));
  
            byte[] b = new byte[1];
            MemoryStream memoryStream = new MemoryStream();
            bool eol = false;

            while (_tcpClient.GetStream().Read(b, 0, 1) != -1)
            {
                if (b[0] == 13) eol = true;
                else if (eol)
                {
                    if (b[0] == 10)  break;
                    eol = false;
                }               
                memoryStream.Write(b, 0, 1); // cast byte into char array
            }

            if (memoryStream == null || memoryStream.Length <= 0)
                throw new IOException(GetLocalizedString("closing dead stream"));

            return UTF8Encoding.UTF8.GetString(memoryStream.GetBuffer()).TrimEnd('\0', '\r', '\n');
        }

        /// <summary>
        /// 读到流尾 
        /// </summary>
        public void ClearEndOfLine()
        {
            if (_tcpClient == null || !_tcpClient.Connected)
                throw new IOException(GetLocalizedString("read closed TcpClient"));

            byte[] b = new byte[1];
            bool eol = false;
            while (_tcpClient.GetStream().Read(b, 0, 1) != -1)
            {
                // 仅当看到 \r (13) 之后紧跟 \n (10) 时停止
                if (b[0] == 13)
                {
                    eol = true;
                    continue;
                }
                if (eol)
                {
                    if (b[0] == 10) break;
                    eol = false;
                }
            }
        }

        /// <summary>
        /// 读取指定长度的字节
        /// </summary>
        /// <param name="b">byte array</param>
        public void Read(byte[] bytes)
        {
            if (_tcpClient == null || !_tcpClient.Connected)
                throw new IOException(GetLocalizedString("read closed TcpClient"));

            if (bytes == null)  return;

            int count = 0;
            while (count < bytes.Length)
            {
                int cnt = _tcpClient.GetStream().Read(bytes, count, (bytes.Length - count));
                count += cnt;
            }
        }

        /// <summary>
        /// flushes output stream 
        /// </summary>
        public void Flush()
        {
            if (_tcpClient == null || !_tcpClient.Connected)
                throw new IOException(GetLocalizedString("write closed TcpClient"));

            _tcpClient.GetStream().Flush();
        }

        /// <summary>
        /// 写入byte数组到输出流中
        /// </summary>
        /// <param name="bytes">byte array to write</param>
        public void Write(byte[] bytes)
        {
            Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// 写入byte数组到输出流中
        /// </summary>
        /// <param name="bytes">byte array to write</param>
        /// <param name="offset">offset to begin writing from</param>
        /// <param name="count">count of bytes to write</param>
        public void Write(byte[] bytes, int offset, int count)
        {
            if (_tcpClient == null || !_tcpClient.Connected)
                throw new IOException(GetLocalizedString("write closed TcpClient"));

            if (bytes != null)
                _tcpClient.GetStream().Write(bytes, offset, count);
        }

		/// <summary>
        /// 写入byte数组到输出流中，并获取返回的读对象信息
		/// </summary>
		/// <param name="bytes">byte array to write</param>
		/// <param name="offset">offset to begin writing from</param>
		/// <param name="count">count of bytes to write</param>
        public BinaryReader WriteAndGetReader(byte[] bytes, int offset, int count)
		{
			if (_tcpClient == null || !_tcpClient.Connected)
				throw new IOException(GetLocalizedString("write closed TcpClient"));

			if (bytes != null)
                _tcpClient.GetStream().Write(bytes, offset, count);

            return new BinaryReader(_tcpClient.GetStream(), System.Text.Encoding.UTF8); // do not close this reader!            
		}

        /// <summary>
        /// 写入byte数组到输出流中，并读取第一个字节信息
        /// </summary>
        /// <param name="bytes">byte array to write</param>
        /// <param name="offset">offset to begin writing from</param>
        /// <param name="count">count of bytes to write</param>
        public byte WriteAndReadByte(byte[] bytes, int offset, int count)
        {
            return WriteAndGetReader(bytes, offset, count).ReadByte();
        }        

		/// <summary>
        /// returns the string representation of this TcpClient 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_tcpClient == null) return "";
            return _id.ToString(new System.Globalization.NumberFormatInfo());
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (this != null)
                this.Close();
        }

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!_disposed)
        //    {
        //        if (disposing)
        //            Close();

        //        TrueClose();
        //    }
        //    _disposed = true;
        //}

        #endregion       

        /// <summary>
        /// 链接线程类. 
        /// </summary>
        private class ConnectThread
        {
            //thread
            Thread _thread;
            private TcpClient _TcpClient;
            private String _host;
            private int _port;
            bool _error;

            /// <summary>
            /// 构造方法 
            /// </summary>
            /// <param name="host"></param>
            /// <param name="port"></param>
            public ConnectThread(string host, int port)
            {
                _host = host;
                _port = port;
                _thread = new Thread(new ThreadStart(Connect));
                _thread.IsBackground = true;
            }

            /// <summary>
            /// 链接逻辑.
            /// </summary>
            private void Connect()
            {
                try
                {
                    _TcpClient = new TcpClient(AddressFamily.InterNetwork);
                    _TcpClient.Connect(new IPEndPoint(IPAddress.Parse(_host), _port));
                }
                catch (IOException)
                {
                    _error = true;
                }
                catch(SocketException ex)
                {
                    _error = true;
                }
            }

            /// <summary>
            /// 启动线程        
            /// </summary>
            public void Start()
            {
                _thread.Start();
            }

            /// <summary>
            /// TcpClient 是否已链接 
            /// </summary>
            public bool IsConnected
            {
                get { return _TcpClient != null && _TcpClient.Connected; }
            }

            /// <summary>
            /// 链接过程中是否有异常 
            /// </summary>
            /// <returns></returns>
            public bool IsError
            {
                get { return _error; }
            }

            /// <summary>
            /// 返回当前TcpClient. 
            /// </summary>
            public TcpClient TcpClient
            {
                get { return _TcpClient; }
            }
        }

        private static ResourceManager _resourceManager = new ResourceManager("Discuz.EntLib.TokyoTyrant.StringMessages", typeof(TcpClientIO).Assembly);
        private static string GetLocalizedString(string key)
        {
            return key;// _resourceManager.GetString(key);
        }
    }
}
