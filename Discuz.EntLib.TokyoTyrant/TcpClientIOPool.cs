namespace Discuz.EntLib.TokyoTyrant
{
    using System;
    using System.Collections;
	using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
	using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    
    using Discuz.EntLib;
    using Discuz.Config;    
   
    /// <summary>
    /// example:
    /// TcpClientIOPool pool = TcpClientIOPool.GetInstance();
    ///     pool = TcpClientIOPool.GetInstance();
    ///     pool.SetServers(new string[] { "10.0.4.66:11221" });
    ///     pool.InitConnections = 4;
    ///     pool.MinConnections = 4;
    ///     pool.MaxConnections = 4;
    ///     pool.MaxIdle = 30000;
    ///     pool.MaxBusy = 50000;
    ///     pool.MaintenanceSleep = 300000;
    ///     pool.TcpClientConnectTimeout = 30000;
    ///     pool.Initialize();
    /// </summary>
    public class TcpClientIOPool
    {
        // store instances of pools
        private static Hashtable Pools = new Hashtable();

        private MaintenanceThread _maintenanceThread;
        private bool _initialized;
        private int _maxCreate = 1;	// ������������������ӳر���ʼ��ʱ����ֵ
        private Hashtable _createShift;

        private int _poolMultiplier = 4;//���������������������
        private int _initConns = 3;
        private int _minConns = 3;
        private int _maxConns = 10;
        private long _maxIdle = 1000 * 60 * 3; // ��ЧTcpClients��������ʱ��(����)
        private long _maxBusyTime = 1000 * 60 * 5; // ��ЧTcpClients�����æµʱ��(����)
        private long _maintThreadSleep = 1000 * 5; // �߳�ά��ʱ�䣬���೤ʱ��ά��һ�³��е����ӣ���æµ��������ӽ��е�����(����)
        private int _TcpClientTimeout = 1000 * 10; // TcpClient����������ʱ�䣨Ŀǰ��δ��Ч��
        private int _TcpClientConnectTimeout = 50; // ���ڳ�ʼ�����ӵĹ���ʱ��(����)
        private bool _failover = true; // ʧЧת��
        private HashingAlgorithm _hashingAlgorithm = HashingAlgorithm.Native;

    
        // �������б�
        private ArrayList _servers;
        private ArrayList _weights;
        private ArrayList _buckets;

        // ��ʧЧ�ķ�����
        private Hashtable _hostDead;
        private Hashtable _hostDeadDuration;

        // ��Ч���ӳض��󣨼�¼���п��õ�������Ϣ��
        private Hashtable _availPool;

        // ���ڹ��������ӳ���Ϣ����¼�������ڹ�����������Ϣ��
        private Hashtable _busyPool;

        protected TcpClientIOPool() { }

        /// <summary>
        /// ��ȡָ�����ӳ����Ƶĳض��󹤳�.
        /// </summary>
        /// <param name="poolName">unique name of the pool</param>
        /// <returns>instance of TcpClientIOPool</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static TcpClientIOPool GetInstance(String poolName)
        {
            if(Pools.ContainsKey(poolName))
                return (TcpClientIOPool)Pools[poolName];

            TcpClientIOPool pool = new TcpClientIOPool();
            Pools[poolName] = pool;

            return pool;
        }

        /// <summary>
        /// ��ȡĬ�����ӳ����Ƶĳض��󹤳�.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static TcpClientIOPool GetInstance()
        {
            return GetInstance("dnt");
        }

		/// <summary>
		/// ��ȡ�������б�
		/// </summary>
		public ArrayList Servers
		{
			get { return _servers; }
		}

		/// <summary>
		/// ���÷������б�
		/// </summary>
		/// <param name="servers">�����ʽ[host:port]</param>
		public void SetServers(string[] servers)
		{
			SetServers(new ArrayList(servers));
		}

		/// <summary>
		/// ���÷������б�
		/// </summary>
		/// <param name="servers">��ʽ [host:port]</param>
		public void SetServers(ArrayList servers)
		{
			_servers = servers;
		}

		/// <summary>
		/// ��ȡ������Ȩ��
		/// </summary>
		/// <value>
		/// </value>
		public ArrayList Weights
		{
			get { return _weights; }
		}

		/// <summary>
        /// ���÷�����Ȩ��
		/// </summary>
		/// <param name="weights">
		/// </param>
		public void SetWeights(int[] weights)
		{
			SetWeights(new ArrayList(weights));
		}

		/// <summary>
        /// ���÷�����Ȩ��
		/// </summary>
		/// <param name="weights">
		/// </param>
		public void SetWeights(ArrayList weights)
		{
			_weights = weights;
		}

        /// <summary>
        /// ��ȡ�����ó�ʼ��available poolʱ��������.
        /// </summary>
        public int InitConnections
        {
            get { return _initConns; }
            set { _initConns = value; }
        }

        /// <summary>
        /// ��ȡ������ available pool����С����������
        /// </summary>
        public int MinConnections
        {
            get { return _minConns; }
            set { _minConns = value; }
        }

        /// <summary>
        /// ��ȡ������ available pool��������������
        /// </summary>
        public int MaxConnections
        {
            get { return _maxConns; }
            set { _maxConns = value; }
        }

        /// <summary>
        /// ��ȡ������ available pool�����ӵ�������ʱ�䣨���룩
        /// </summary>
        public long MaxIdle
        {
            get { return _maxIdle; }
            set { _maxIdle = value; }
        }

        /// <summary>
        /// ��ȡ������ Busy pool�����ӵ����æµʱ�䣨���룩���糬ʱ��رյ�ǰtcpclient����
        /// </summary>
        public long MaxBusy
        {
            get { return _maxBusyTime; }
            set { _maxBusyTime = value; }
        }

        /// <summary>
        /// ���ӳض�ʱά��ʱ�䣬��Ϊ0������ά���̣߳�ά�����ӳأ�
        /// </summary>
        /// <value></value>
        public long MaintenanceSleep
        {
            get { return _maintThreadSleep; }
            set { _maintThreadSleep = value; }
        }

        /// <summary>
        /// TcpClient ����������ʱ��(Ŀǰ��δ�ṩ)
        /// </summary>
        public int TcpClientTimeout
        {
            get { return _TcpClientTimeout; }
            set { _TcpClientTimeout = value; }
        }

        /// <summary>
        /// ��ʼ��TcpClient ���ӵĹ���ʱ��.
        /// </summary>
        /// <value></value>
        public int TcpClientConnectTimeout
        {
            get { return _TcpClientConnectTimeout; }
            set { _TcpClientConnectTimeout = value; }
        }

        /// <summary>
        /// ʧЧת�� 
        /// </summary>
        public bool Failover
        {
            get { return _failover; }
            set { _failover = value; }
        }

        /// <summary>
        /// ��ȡ������hashing�㷨.
        /// </summary>
        public HashingAlgorithm HashingAlgorithm
        {
            get { return _hashingAlgorithm; }
            set { _hashingAlgorithm = value; }
        }

      
        /// <summary>
        /// Initializes the pool
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Initialize() 
		{
			// ����Ƿ��ѱ���ʼ��
			if(_initialized && _buckets != null && _availPool != null && _busyPool != null) 
				return;

			// ��ʼ���ض���
			_buckets     = new ArrayList();
			_availPool   = new Hashtable(_servers.Count * _initConns);
			_busyPool    = new Hashtable(_servers.Count * _initConns);
			_hostDeadDuration = new Hashtable();
			_hostDead    = new Hashtable();
			_createShift = new Hashtable();
			_maxCreate   = (_poolMultiplier > _minConns) ? _minConns : _minConns / _poolMultiplier;		// only create up to maxCreate connections at once

            // ��Ϊ�����׳��쳣
			if(_servers == null || _servers.Count <= 0) 
				throw new ArgumentException(GetLocalizedString("initialize with no servers"));

			for(int i = 0; i < _servers.Count; i++) 
			{
				//������Ȩ�ز���Ӹ���Ϣ
				if(_weights != null && _weights.Count > i) 
				{
					for(int k = 0; k < ((int)_weights[i]); k++) 
						_buckets.Add(_servers[i]);
				}
				else 
					_buckets.Add(_servers[i]);

				for(int j = 0; j < _initConns; j++) 
				{
					TcpClientIO TcpClient = CreateTcpClient((string)_servers[i]);
					if(TcpClient == null) 
						break;

					AddTcpClientToPool(_availPool, (string)_servers[i], TcpClient);
				}
			}

			//���Ϊ�ѳ�ʼ��
            _initialized = true;

			// ����ά���߳�
			if(_maintThreadSleep > 0)
				this.StartMaintenanceThread();
		}

        /// <summary>
        /// ���Ƿ񱻳�ʼ��
        /// </summary>
        /// <returns></returns>
        public bool Initialized
        {
            get { return _initialized; }
        }

        /// <summary>
        /// ����ָ��HOST���µ�TcpClientIO ����. ������ʧ�ܣ��򷵻� null
        /// </summary>
        /// <param name="host">host:port </param>
        /// <returns>TcpClientIO����</returns>
        protected TcpClientIO CreateTcpClient(string host)
        {
            TcpClientIO TcpClient = null;

            if(_failover && _hostDead.ContainsKey(host) && _hostDeadDuration.ContainsKey(host))
            {
                DateTime store = (DateTime)_hostDead[host];
                long expire = ((long)_hostDeadDuration[host]);

                if((store.AddMilliseconds(expire)) > DateTime.Now)
                    return null;
            }

			try
			{
				TcpClient = new TcpClientIO(this, host, TcpClientConnectTimeout, _failover);

				if(!TcpClient.IsConnected)
				{
					try
					{
						TcpClient.TrueClose();
					}
					catch
					{
						TcpClient = null;
					}
				}
			}
			catch
			{
				TcpClient = null;
			}
           
            // ��δ��ȡTcpClient, ���ʶ������������
            if(TcpClient == null)
            {
                DateTime now = DateTime.Now;
                _hostDead[host] = now;
                long expire = (_hostDeadDuration.ContainsKey(host)) ? (((long)_hostDeadDuration[host]) * 2) : 100;
                _hostDeadDuration[host] = expire;

                ClearHostFromPool(_availPool, host);

                if (_buckets.BinarySearch(host) >= 0)
                    _buckets.Remove(host);
            }
            else
            {
                _hostDead.Remove(host);
                _hostDeadDuration.Remove(host);
                if(_buckets.BinarySearch(host) < 0)
                    _buckets.Add(host);
            }

            return TcpClient;
        }

        /// <summary>
        /// ��ȡserver�б�(ֻ��һ��server)�е�һ��server������
        /// </summary>
        /// <returns></returns>
        public TcpClientIO GetTcpClient()
        {
            if (!_initialized) return null;

            // ��û��servers �򷵻� null
            if (_buckets.Count == 0) return null;

            // �������server�б�,�ҵ�һ�����ܵ�����
            for (int tries = 0; tries < _buckets.Count; tries++)
            {
                TcpClientIO tcpClient = GetConnection((string)_buckets[tries]);
                if (tcpClient != null)
                    return tcpClient;
            }     
            return null;
        }
       
        /// <summary>
        /// ��ȡָ��key��hashֵ��TcpClientIO ���󣬸÷�����Ϊ���ݲ���memcached����
        /// </summary>
        /// <param name="key">��</param>
        /// <returns>TcpClientIO ����</returns>
        public TcpClientIO GetTcpClient(string key, object hashCode)
        {
            string hashCodeString = "<null>";
            if (hashCode != null)
                hashCodeString = hashCode.ToString();

            if (key == null || key.Length == 0) return null;
           
            if (!_initialized) return null;

            // ��û��servers �򷵻� null
            if (_buckets.Count == 0)  return null;

            // ��ֻ��һ��server �򷵻���
            if (_buckets.Count == 1)  return GetConnection((string)_buckets[0]);

            TcpClientIO sock = GetConnection((string)_buckets[0]);
            if (sock != null) return sock;

            // ����hashcode
            int hv;
            if (hashCode != null)
            {
                hv = (int)hashCode;
            }
            else
            {
                // NATIVE_HASH = 0�� OLD_COMPAT_HASH = 1�� NEW_COMPAT_HASH = 2
                switch (_hashingAlgorithm)
                {
                    case HashingAlgorithm.Native:
                        hv = key.GetHashCode();
                        break;
                    case HashingAlgorithm.OldCompatibleHash:
                        hv = HashingAlgorithmHelper.OriginalHashingAlgorithm(key);
                        break;
                    case HashingAlgorithm.NewCompatibleHash:
                        hv = HashingAlgorithmHelper.NewHashingAlgorithm(key);
                        break;
                    default:
                        // use the native hash as a default
                        hv = key.GetHashCode();
                        _hashingAlgorithm = HashingAlgorithm.Native;
                        break;
                }
            }
            int tries = 0;         
            // ���Բ�ͬ��servers 
            while (tries++ <= _buckets.Count)
            {
                // ʹ��hashcode��ȡbucket������ȡһ��TcpClientIO����
                int bucket = hv % _buckets.Count;
                if (bucket < 0)
                    bucket += _buckets.Count;

                TcpClientIO tcpClient = GetConnection((string)_buckets[bucket]);

                if (tcpClient != null)
                    return tcpClient;

                // �������failover, ������
                if (!_failover)  return null;

                // ��δ��ȡtcpclient�������ٴγ���ͨ���ۼ�����ȡhashֵ
                switch (_hashingAlgorithm)
                {
                    case HashingAlgorithm.Native:
                        hv += ((string)("" + tries + key)).GetHashCode();
                        break;
                    case HashingAlgorithm.OldCompatibleHash:
                        hv += HashingAlgorithmHelper.OriginalHashingAlgorithm("" + tries + key);
                        break;
                    case HashingAlgorithm.NewCompatibleHash:
                        hv += HashingAlgorithmHelper.NewHashingAlgorithm("" + tries + key);
                        break;
                    default:
                        // Ĭ��ʹ��native hash
                        hv += ((string)("" + tries + key)).GetHashCode();
                        _hashingAlgorithm = HashingAlgorithm.Native;
                        break;
                }
            }
            return null;
        }

        /// <summary>
        /// ��ȡָ�������� TcpClientIO ��Ч����.
        /// </summary>
        /// <param name="host">����</param>
        /// <returns>TcpClientIO����</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public TcpClientIO GetConnection(string host)
        {
            if(!_initialized)  return null;

            if(host == null)   return null;

            // ���_availPool���������ӣ���ֱ�ӷ���
            if(_availPool != null && !(_availPool.Count == 0))
            {
                // take first connected TcpClient
                Hashtable aTcpClients = (Hashtable)_availPool[host];
                if(aTcpClients != null && !(aTcpClients.Count == 0))
                {
                    foreach (TcpClientIO TcpClient in new IteratorIsolateCollection(aTcpClients.Keys))
                    {
                        if (TcpClient.IsConnected)
                        {
                            // ��avail pool���Ƴ���
                            aTcpClients.Remove(TcpClient);
                            // ������ӵ� busy pool ��
                            AddTcpClientToPool(_busyPool, host, TcpClient);
                            // ���ظ�����
                            return TcpClient;
                        }
                        else// ��avail pool���Ƴ���
                            aTcpClients.Remove(TcpClient);
                    }
                }
            }

            // ��û����Ч���ӣ��򰴱��������µ����ӣ�maxCreate��
            object cShift = _createShift[host];
            int shift = (cShift != null) ? (int)cShift : 0;

            int create = 1 << shift;
            if(create >= _maxCreate)
                create = _maxCreate;
            else
                shift++;

            // ���浱ǰhost��shift ֵ
            _createShift[host] = shift;

            for (int i = create; i > 0; i--)
            {
                TcpClientIO TcpClient = CreateTcpClient(host);
                if (TcpClient == null)
                    break;
                // ���һ�ε�����������ӵ� busy pool�в�������
                if (i == 1)
                {                    
                    AddTcpClientToPool(_busyPool, host, TcpClient);
                    return TcpClient;
                }
                else// ��ӵ� avail pool ��
                    AddTcpClientToPool(_availPool, host, TcpClient);
            }
            return null;
        }

        /// <summary>
        /// ����ǰ TcpClient��ӵ�ָ�����������ӳ���   
        /// </summary>
        /// <param name="pool">Ҫ��ӵ���pool</param>
        /// <param name="host">Ҫ���ӵ�host</param>
        /// <param name="TcpClient">Ҫ��ӵ�TcpClient</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected static void AddTcpClientToPool(Hashtable pool, string host, TcpClientIO TcpClient)
        {
            if (pool == null)
                return; 

            Hashtable TcpClients;
            if (host != null && host.Length != 0 && pool.ContainsKey(host))
            {
                TcpClients = (Hashtable)pool[host];
                if (TcpClients != null)
                {
                    TcpClients[TcpClient] = DateTime.Now;

                    return;
                }
            }

            TcpClients = new Hashtable();
            TcpClients[TcpClient] = DateTime.Now;
            pool[host] = TcpClients;
        }

        /// <summary>
        /// ��ָ�����������ӳ����Ƴ� TcpClient
        /// </summary>
        /// <param name="pool">Ҫ�Ƴ���TcpClientIOʵ�����е�pool</param>
        /// <param name="host">host pool</param>
        /// <param name="TcpClient">Ҫ�Ƴ���TcpClientIOʵ��</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected static void RemoveTcpClientFromPool(Hashtable pool, string host, TcpClientIO TcpClient)
        {
            if (host != null && host.Length == 0 || pool == null) return;

            if (pool.ContainsKey(host))
            {
                Hashtable TcpClients = (Hashtable)pool[host];
                if (TcpClients != null)
                    TcpClients.Remove(TcpClient);
            }
        }

        /// <summary>
        /// ��ָ�����������ӳ��йرղ��Ƴ�����TcpClient. 
        /// </summary>
        /// <param name="pool">Ҫ��յ�pool</param>
        /// <param name="host">Ҫ��յ�host</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected static void ClearHostFromPool(Hashtable pool, string host)
        {
            if (pool == null || host != null && host.Length == 0)
                return;

            if(pool.ContainsKey(host))
            {
                Hashtable TcpClients = (Hashtable)pool[host];

                if(TcpClients != null && TcpClients.Count > 0)
                {
                    foreach(TcpClientIO TcpClient in new IteratorIsolateCollection(TcpClients.Keys))
                    {
                        try
                        {
                            TcpClient.TrueClose();
                        }
                        catch {;}

                        TcpClients.Remove(TcpClient);
                    }
                }
            }
        }

        /// <summary>
        /// ��һ��TcpClientIO �������루_availPool������.
        /// </summary>
        /// <param name="TcpClientIO">TcpClientIO ����</param>
        /// <param name="addToAvail">�Ƿ���ӵ�avail pool��</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CheckIn(TcpClientIO TcpClient, bool addToAvail)
        {
            if (TcpClient == null)
                return; 

            string host = TcpClient.Host;

            RemoveTcpClientFromPool(_busyPool, host, TcpClient);

            // ��ӵ� avail pool
            if(addToAvail && TcpClient.IsConnected)
                AddTcpClientToPool(_availPool, host, TcpClient);
        }

        /// <summary>
        /// ��һ��TcpClientIO�������루_availPool������
        /// </summary>
        /// <param name="TcpClient">TcpClient to return</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CheckIn(TcpClientIO TcpClient)
        {
            CheckIn(TcpClient, true);
        }

        /// <summary>
        /// �رճ�������TcpClients ����.   
        /// </summary>
        /// <param name="pool">�ر�pool</param>
        protected static void ClosePool(Hashtable pool)
        {
            if (pool == null)
                return; 

            foreach(string host in pool.Keys)
            {
                Hashtable TcpClients = (Hashtable)pool[host];

                foreach(TcpClientIO TcpClient in new IteratorIsolateCollection(TcpClients.Keys))
                {
                    try
                    {
                        TcpClient.TrueClose();
                    }
                    catch {;}
                    TcpClients.Remove(TcpClient);
                }
            }
        }

        /// <summary>
        /// �رճ�.��ֹͣά���߳�
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Shutdown()
        {
            if (_maintenanceThread != null && _maintenanceThread.IsRunning)
                StopMaintenanceThread();

            ClosePool(_availPool);
            ClosePool(_busyPool);
            _availPool = _hostDeadDuration = _busyPool = _hostDead = null;
            _buckets = null;
            _initialized = false;
        }

        /// <summary>
        /// ����ά���߳�.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void StartMaintenanceThread()
        {
            if(_maintenanceThread != null)
            {
                if(!_maintenanceThread.IsRunning)
                   _maintenanceThread.Start();
            }
            else
            {
                _maintenanceThread = new MaintenanceThread(this);
                _maintenanceThread.Interval = _maintThreadSleep;
                _maintenanceThread.Start();
            }
        }

        /// <summary>
        /// ֹͣά���߳�.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected void StopMaintenanceThread()
        {
            if(_maintenanceThread != null && _maintenanceThread.IsRunning)
                _maintenanceThread.StopThread();
        }

        /// <summary>
        /// �������ڲ����У���������ά�� 
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected void SelfMaintain()
        {
            // ����avail_pool,�����������Ӧ��TcpClients
            foreach(string host in new IteratorIsolateCollection(_availPool.Keys))
            {
                Hashtable TcpClients = (Hashtable)_availPool[host];

                // �� pool ̫С (n < minSpare)���򴴽��µ�TcpClients
                if(TcpClients.Count < _minConns)
                {
                    int need = _minConns - TcpClients.Count;
 
                    for(int j = 0; j < need; j++)
                    {
                        TcpClientIO TcpClient = CreateTcpClient(host);

                        if(TcpClient == null) break;

                        AddTcpClientToPool(_availPool, host, TcpClient);
                    }
                }
                else if(TcpClients.Count > _maxConns)
                {
                    // ����ر�һЩTcpClients
                    int diff = TcpClients.Count - _maxConns;
                    int needToClose = (diff <= _poolMultiplier) ? diff : (diff) / _poolMultiplier;

                    foreach(TcpClientIO TcpClient in new IteratorIsolateCollection(TcpClients.Keys))
                    {
                        if(needToClose <= 0)
                            break;

                        DateTime expire = (DateTime)TcpClients[TcpClient];

                        // ������ʱ���ѵ���ر�TcpClient�������pool���Ƴ�
                        if((expire.AddMilliseconds(_maxIdle)) < DateTime.Now)
                        {
                            try
                            {
                                TcpClient.TrueClose();
                            }
                            catch {;}

                            TcpClients.Remove(TcpClient);
                            needToClose--;
                        }
                    }
                }

                //����shift value���ڴ����µ�TcpClientIO ����
                _createShift[host] = 0;
            }

            // ����busy pool���Ƴ���ӦTcpClients
            foreach(string host in _busyPool.Keys)
            {
                Hashtable TcpClients = (Hashtable)_busyPool[host];
     
                // �����������Ӳ�����ж౻���������
                foreach(TcpClientIO TcpClient in new IteratorIsolateCollection(TcpClients.Keys))
                {
                    DateTime hungTime = (DateTime)TcpClients[TcpClient];

                    // ���max busy time �ѵ���رո�TcpClient�������pool���Ƴ�
                    if((hungTime.AddMilliseconds(_maxBusyTime)) < DateTime.Now)
                    {
                        try
                        {
                            TcpClient.TrueClose();
                        }
                        catch { ; }

                        TcpClients.Remove(TcpClient);
                    }
                }
            }
        }

        /// <summary>
        /// �߳���չ�࣬���ڳض����ά��.
        /// </summary>
        private class MaintenanceThread
        {
            private MaintenanceThread() { }

            private Thread _thread;

            private TcpClientIOPool _pool;
            private long _interval = 1000 * 3; // ÿ����
            private bool _stopThread;

            public MaintenanceThread(TcpClientIOPool pool)
            {
                _thread = new Thread(new ThreadStart(Maintain));
                _pool = pool;
            }

            public long Interval
            {
                get { return _interval; }
                set { _interval = value; }
            }

            public bool IsRunning
            {
                get { return _thread.IsAlive; }
            }

            /// <summary>
            /// ����stop�������жϵ�ǰ�̲߳��ȴ�
            /// </summary>
            public void StopThread()
            {
                _stopThread = true;
                _thread.Interrupt();
            }

            /// <summary>
            /// �߳�ά���߼�.
            /// </summary>
            private void Maintain()
            {
                while(!_stopThread)
                {
                    try
                    {
                        Thread.Sleep((int)_interval);

                        // ����ѳ�ʼ��������������ά������
                        if (_pool.Initialized)
                            _pool.SelfMaintain();

                    }
                    catch (ThreadInterruptedException) { } //������TcpClientIOPool.getInstance().shutDown()����ʱ
                    catch { ; }
                }
            }

            /// <summary>
            /// �����߳�
            /// </summary>
            public void Start()
            {
                _stopThread = false;
                _thread.Start();
            }
        }

        private static ResourceManager _resourceManager = new ResourceManager("Discuz.EntLib.TokyoTyrant.StringMessages", typeof(TcpClientIOPool).Assembly);
		private static string GetLocalizedString(string key)
		{
            return key;// _resourceManager.GetString(key);
		}
    }
}