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
    /// TcpClient IO������ 
    /// �����װ��TcpClient������streams��������.
    /// </summary>
    public class TcpClientIO : IDisposable 
    {
        // id ������.���ڸ����е�TcpClientIO ʵ��ID��ֵ
        private static int IdGenerator;   
        // ����ʱ��
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
        /// ����һ��TcpClientIO����ʵ��
        /// ���ӵ� host:port
        /// </summary>
        /// <param name="pool">�ö��󱻰󶨵���(����)��</param>
        /// <param name="host">����</param>
        /// <param name="port">�˿�</param>
        /// <param name="connectTimeout">���ڳ�ʼ�����ӵĹ���ʱ��(����)</param>
        /// <param name="noDelay">�Ƿ���ʱ</param>
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
        /// ��һ��TcpClientIO����ʵ�������ӵ� host(�����˿�)
        /// </summary>
        /// <param name="pool">�ö��󱻰󶨵���(����)��</param>
        /// <param name="host">����(���˿�)</param>
        /// <param name="connectTimeout">���ڳ�ʼ�����ӵĹ���ʱ��(����)</param>
        /// <param name="failover">ʧЧת��(���ж���δ��Ч)</param>
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
        /// ʹ���̷߳�ʽ��ȡ����ʵ����ʹ��timeout���ж������Ƿ�ʱ
        /// </summary>
        /// <param name="host">���ӵ�����</param>
        /// <param name="port">�˿�</param>
        /// <param name="timeout">���ڳ�ʼ�����ӵĹ���ʱ��(����)</param>
        /// <returns>���õ�TcpClientʵ��</returns>
        protected static TcpClient GetTcpClient(String host, int port, int timeout)
        {
            // ����һ�������̲߳�������
            ConnectThread thread = new ConnectThread(host, port);
            thread.Start();

            int timer = 0;
            int sleep = 25;

            while (timer < timeout)
            {
                // ������߳��е�TcpClientΪ�������򷵻���
                if (thread.IsConnected)
                    return thread.TcpClient;

                // ���߳��д������׳��ô���
                if (thread.IsError)
                    throw new IOException();

                try
                {
                    // ����һС��ʱ��
                    Thread.Sleep(sleep);
                }
                catch (ThreadInterruptedException) { }

                // �ۼӶ�ʱ��
                timer += sleep;
            }

            // ����ڹ涨ʱ����δ��ʼ������ʱ�����׳����ӳ�ʱ�쳣
            throw new IOException(GetLocalizedString("connect timeout").Replace("$$timeout$$", timeout.ToString(new System.Globalization.NumberFormatInfo())));
        }

        /// <summary>
        /// ��������������ַ
        /// String ��ʾhost (hostname:port)
        /// </summary>
        public string Host
        {
            get { return _host; }
        }

        /// <summary>
        /// �ر�TcpClient�����������ӵ�streams 
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
        /// ����closed ��־λ������ŵ����ӳأ������رո�����
        /// </summary>
        public void Close()
        {
            _pool.CheckIn(this);
            //���ر���
            //_tcpClient.GetStream().Close();
        }

        /// <summary>
        /// ��ȡ��ǰTcpClient �Ƿ�����.
        /// </summary>
        public bool IsConnected
        {
            get { return _tcpClient != null && _tcpClient.Connected; }
        }

        /// <summary>
        /// ��һ������
        /// </summary>
        /// <returns>���ض���������</returns>
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
        /// ������β 
        /// </summary>
        public void ClearEndOfLine()
        {
            if (_tcpClient == null || !_tcpClient.Connected)
                throw new IOException(GetLocalizedString("read closed TcpClient"));

            byte[] b = new byte[1];
            bool eol = false;
            while (_tcpClient.GetStream().Read(b, 0, 1) != -1)
            {
                // �������� \r (13) ֮����� \n (10) ʱֹͣ
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
        /// ��ȡָ�����ȵ��ֽ�
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
        /// д��byte���鵽�������
        /// </summary>
        /// <param name="bytes">byte array to write</param>
        public void Write(byte[] bytes)
        {
            Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// д��byte���鵽�������
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
        /// д��byte���鵽������У�����ȡ���صĶ�������Ϣ
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
        /// д��byte���鵽������У�����ȡ��һ���ֽ���Ϣ
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
        /// �����߳���. 
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
            /// ���췽�� 
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
            /// �����߼�.
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
            /// �����߳�        
            /// </summary>
            public void Start()
            {
                _thread.Start();
            }

            /// <summary>
            /// TcpClient �Ƿ������� 
            /// </summary>
            public bool IsConnected
            {
                get { return _TcpClient != null && _TcpClient.Connected; }
            }

            /// <summary>
            /// ���ӹ������Ƿ����쳣 
            /// </summary>
            /// <returns></returns>
            public bool IsError
            {
                get { return _error; }
            }

            /// <summary>
            /// ���ص�ǰTcpClient. 
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
