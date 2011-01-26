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
        private int _maxCreate = 1;	// 创建的最大数，当链接池被初始化时被赋值
        private Hashtable _createShift;

        private int _poolMultiplier = 4;//按倍数构造池中链接数量
        private int _initConns = 3;
        private int _minConns = 3;
        private int _maxConns = 10;
        private long _maxIdle = 1000 * 60 * 3; // 有效TcpClients的最大空闲时间(毫秒)
        private long _maxBusyTime = 1000 * 60 * 5; // 有效TcpClients的最大忙碌时间(毫秒)
        private long _maintThreadSleep = 1000 * 5; // 线程维护时间，即多长时间维护一下池中的链接（对忙碌或空闲链接进行调整）(毫秒)
        private int _TcpClientTimeout = 1000 * 10; // TcpClient读操作过期时间（目前暂未生效）
        private int _TcpClientConnectTimeout = 50; // 用于初始化链接的过期时间(毫秒)
        private bool _failover = true; // 失效转移
        private HashingAlgorithm _hashingAlgorithm = HashingAlgorithm.Native;

    
        // 服务器列表
        private ArrayList _servers;
        private ArrayList _weights;
        private ArrayList _buckets;

        // 已失效的服务器
        private Hashtable _hostDead;
        private Hashtable _hostDeadDuration;

        // 有效链接池对象（记录所有可用的链接信息）
        private Hashtable _availPool;

        // 正在工作的链接池信息（记录所有正在工作的链接信息）
        private Hashtable _busyPool;

        protected TcpClientIOPool() { }

        /// <summary>
        /// 获取指定链接池名称的池对象工厂.
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
        /// 获取默认链接池名称的池对象工厂.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static TcpClientIOPool GetInstance()
        {
            return GetInstance("dnt");
        }

		/// <summary>
		/// 获取服务器列表
		/// </summary>
		public ArrayList Servers
		{
			get { return _servers; }
		}

		/// <summary>
		/// 设置服务器列表
		/// </summary>
		/// <param name="servers">数组格式[host:port]</param>
		public void SetServers(string[] servers)
		{
			SetServers(new ArrayList(servers));
		}

		/// <summary>
		/// 设置服务器列表
		/// </summary>
		/// <param name="servers">格式 [host:port]</param>
		public void SetServers(ArrayList servers)
		{
			_servers = servers;
		}

		/// <summary>
		/// 获取服务器权重
		/// </summary>
		/// <value>
		/// </value>
		public ArrayList Weights
		{
			get { return _weights; }
		}

		/// <summary>
        /// 设置服务器权重
		/// </summary>
		/// <param name="weights">
		/// </param>
		public void SetWeights(int[] weights)
		{
			SetWeights(new ArrayList(weights));
		}

		/// <summary>
        /// 设置服务器权重
		/// </summary>
		/// <param name="weights">
		/// </param>
		public void SetWeights(ArrayList weights)
		{
			_weights = weights;
		}

        /// <summary>
        /// 获取或设置初始化available pool时的链接数.
        /// </summary>
        public int InitConnections
        {
            get { return _initConns; }
            set { _initConns = value; }
        }

        /// <summary>
        /// 获取或设置 available pool的最小空闲链接数
        /// </summary>
        public int MinConnections
        {
            get { return _minConns; }
            set { _minConns = value; }
        }

        /// <summary>
        /// 获取或设置 available pool的最大空闲链接数
        /// </summary>
        public int MaxConnections
        {
            get { return _maxConns; }
            set { _maxConns = value; }
        }

        /// <summary>
        /// 获取或设置 available pool中链接的最大空闲时间（毫秒）
        /// </summary>
        public long MaxIdle
        {
            get { return _maxIdle; }
            set { _maxIdle = value; }
        }

        /// <summary>
        /// 获取或设置 Busy pool中链接的最大忙碌时间（毫秒），如超时则关闭当前tcpclient链接
        /// </summary>
        public long MaxBusy
        {
            get { return _maxBusyTime; }
            set { _maxBusyTime = value; }
        }

        /// <summary>
        /// 链接池定时维护时间，如为0则不启动维护线程（维护链接池）
        /// </summary>
        /// <value></value>
        public long MaintenanceSleep
        {
            get { return _maintThreadSleep; }
            set { _maintThreadSleep = value; }
        }

        /// <summary>
        /// TcpClient 读操作过期时间(目前暂未提供)
        /// </summary>
        public int TcpClientTimeout
        {
            get { return _TcpClientTimeout; }
            set { _TcpClientTimeout = value; }
        }

        /// <summary>
        /// 初始化TcpClient 链接的过期时间.
        /// </summary>
        /// <value></value>
        public int TcpClientConnectTimeout
        {
            get { return _TcpClientConnectTimeout; }
            set { _TcpClientConnectTimeout = value; }
        }

        /// <summary>
        /// 失效转移 
        /// </summary>
        public bool Failover
        {
            get { return _failover; }
            set { _failover = value; }
        }

        /// <summary>
        /// 获取或设置hashing算法.
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
			// 检查是否已被初始化
			if(_initialized && _buckets != null && _availPool != null && _busyPool != null) 
				return;

			// 初始化池对象
			_buckets     = new ArrayList();
			_availPool   = new Hashtable(_servers.Count * _initConns);
			_busyPool    = new Hashtable(_servers.Count * _initConns);
			_hostDeadDuration = new Hashtable();
			_hostDead    = new Hashtable();
			_createShift = new Hashtable();
			_maxCreate   = (_poolMultiplier > _minConns) ? _minConns : _minConns / _poolMultiplier;		// only create up to maxCreate connections at once

            // 如为空则抛出异常
			if(_servers == null || _servers.Count <= 0) 
				throw new ArgumentException(GetLocalizedString("initialize with no servers"));

			for(int i = 0; i < _servers.Count; i++) 
			{
				//如设置权重测添加该信息
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

			//标记为已初始化
            _initialized = true;

			// 启动维护线程
			if(_maintThreadSleep > 0)
				this.StartMaintenanceThread();
		}

        /// <summary>
        /// 池是否被初始化
        /// </summary>
        /// <returns></returns>
        public bool Initialized
        {
            get { return _initialized; }
        }

        /// <summary>
        /// 创建指定HOST上新的TcpClientIO 对象. 如链接失败，则返回 null
        /// </summary>
        /// <param name="host">host:port </param>
        /// <returns>TcpClientIO对象</returns>
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
           
            // 如未获取TcpClient, 则标识该主机不可用
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
        /// 获取server列表(只有一个server)中第一个server的链接
        /// </summary>
        /// <returns></returns>
        public TcpClientIO GetTcpClient()
        {
            if (!_initialized) return null;

            // 如没有servers 则返回 null
            if (_buckets.Count == 0) return null;

            // 否则遍历server列表,找到一个可能的链接
            for (int tries = 0; tries < _buckets.Count; tries++)
            {
                TcpClientIO tcpClient = GetConnection((string)_buckets[tries]);
                if (tcpClient != null)
                    return tcpClient;
            }     
            return null;
        }
       
        /// <summary>
        /// 获取指定key和hash值的TcpClientIO 对象，该方法仅为兼容部分memcached功能
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>TcpClientIO 对象</returns>
        public TcpClientIO GetTcpClient(string key, object hashCode)
        {
            string hashCodeString = "<null>";
            if (hashCode != null)
                hashCodeString = hashCode.ToString();

            if (key == null || key.Length == 0) return null;
           
            if (!_initialized) return null;

            // 如没有servers 则返回 null
            if (_buckets.Count == 0)  return null;

            // 如只有一个server 则返回它
            if (_buckets.Count == 1)  return GetConnection((string)_buckets[0]);

            TcpClientIO sock = GetConnection((string)_buckets[0]);
            if (sock != null) return sock;

            // 生成hashcode
            int hv;
            if (hashCode != null)
            {
                hv = (int)hashCode;
            }
            else
            {
                // NATIVE_HASH = 0， OLD_COMPAT_HASH = 1， NEW_COMPAT_HASH = 2
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
            // 尝试不同的servers 
            while (tries++ <= _buckets.Count)
            {
                // 使用hashcode获取bucket，并获取一个TcpClientIO对象
                int bucket = hv % _buckets.Count;
                if (bucket < 0)
                    bucket += _buckets.Count;

                TcpClientIO tcpClient = GetConnection((string)_buckets[bucket]);

                if (tcpClient != null)
                    return tcpClient;

                // 如果不想failover, 则跳出
                if (!_failover)  return null;

                // 如未获取tcpclient对象，则再次尝试通过累加器获取hash值
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
                        // 默认使用native hash
                        hv += ((string)("" + tries + key)).GetHashCode();
                        _hashingAlgorithm = HashingAlgorithm.Native;
                        break;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取指定主机的 TcpClientIO 有效链接.
        /// </summary>
        /// <param name="host">主机</param>
        /// <returns>TcpClientIO对象</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public TcpClientIO GetConnection(string host)
        {
            if(!_initialized)  return null;

            if(host == null)   return null;

            // 如果_availPool池中有链接，则直接返回
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
                            // 从avail pool中移除它
                            aTcpClients.Remove(TcpClient);
                            // 将它添加到 busy pool 中
                            AddTcpClientToPool(_busyPool, host, TcpClient);
                            // 返回该链接
                            return TcpClient;
                        }
                        else// 从avail pool中移除它
                            aTcpClients.Remove(TcpClient);
                    }
                }
            }

            // 如没有有效链接，则按倍数创建新的链接（maxCreate）
            object cShift = _createShift[host];
            int shift = (cShift != null) ? (int)cShift : 0;

            int create = 1 << shift;
            if(create >= _maxCreate)
                create = _maxCreate;
            else
                shift++;

            // 保存当前host的shift 值
            _createShift[host] = shift;

            for (int i = create; i > 0; i--)
            {
                TcpClientIO TcpClient = CreateTcpClient(host);
                if (TcpClient == null)
                    break;
                // 最后一次迭代，将其添加到 busy pool中并返回它
                if (i == 1)
                {                    
                    AddTcpClientToPool(_busyPool, host, TcpClient);
                    return TcpClient;
                }
                else// 添加到 avail pool 中
                    AddTcpClientToPool(_availPool, host, TcpClient);
            }
            return null;
        }

        /// <summary>
        /// 将当前 TcpClient添加到指定的主机链接池中   
        /// </summary>
        /// <param name="pool">要添加到的pool</param>
        /// <param name="host">要链接的host</param>
        /// <param name="TcpClient">要添加的TcpClient</param>
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
        /// 从指定的主机链接池中移除 TcpClient
        /// </summary>
        /// <param name="pool">要移除的TcpClientIO实例所有的pool</param>
        /// <param name="host">host pool</param>
        /// <param name="TcpClient">要移除的TcpClientIO实例</param>
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
        /// 从指定的主机链接池中关闭并移除所有TcpClient. 
        /// </summary>
        /// <param name="pool">要清空的pool</param>
        /// <param name="host">要清空的host</param>
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
        /// 将一个TcpClientIO 对象移入（_availPool）池中.
        /// </summary>
        /// <param name="TcpClientIO">TcpClientIO 对象</param>
        /// <param name="addToAvail">是否添加到avail pool中</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CheckIn(TcpClientIO TcpClient, bool addToAvail)
        {
            if (TcpClient == null)
                return; 

            string host = TcpClient.Host;

            RemoveTcpClientFromPool(_busyPool, host, TcpClient);

            // 添加到 avail pool
            if(addToAvail && TcpClient.IsConnected)
                AddTcpClientToPool(_availPool, host, TcpClient);
        }

        /// <summary>
        /// 对一个TcpClientIO对象移入（_availPool）池中
        /// </summary>
        /// <param name="TcpClient">TcpClient to return</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CheckIn(TcpClientIO TcpClient)
        {
            CheckIn(TcpClient, true);
        }

        /// <summary>
        /// 关闭池中所有TcpClients 对象.   
        /// </summary>
        /// <param name="pool">关闭pool</param>
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
        /// 关闭池.并停止维护线程
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
        /// 启动维护线程.
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
        /// 停止维护线程.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected void StopMaintenanceThread()
        {
            if(_maintenanceThread != null && _maintenanceThread.IsRunning)
                _maintenanceThread.StopThread();
        }

        /// <summary>
        /// 在所有内部池中，运行自身维护 
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected void SelfMaintain()
        {
            // 遍历avail_pool,构造或销毁相应的TcpClients
            foreach(string host in new IteratorIsolateCollection(_availPool.Keys))
            {
                Hashtable TcpClients = (Hashtable)_availPool[host];

                // 如 pool 太小 (n < minSpare)，则创建新的TcpClients
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
                    // 否则关闭一些TcpClients
                    int diff = TcpClients.Count - _maxConns;
                    int needToClose = (diff <= _poolMultiplier) ? diff : (diff) / _poolMultiplier;

                    foreach(TcpClientIO TcpClient in new IteratorIsolateCollection(TcpClients.Keys))
                    {
                        if(needToClose <= 0)
                            break;

                        DateTime expire = (DateTime)TcpClients[TcpClient];

                        // 过空闲时间已到则关闭TcpClient并将其从pool中移除
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

                //重置shift value用于创建新的TcpClientIO 对象
                _createShift[host] = 0;
            }

            // 遍历busy pool并移除相应TcpClients
            foreach(string host in _busyPool.Keys)
            {
                Hashtable TcpClients = (Hashtable)_busyPool[host];
     
                // 遍历所有链接并检查有多被挂起的链接
                foreach(TcpClientIO TcpClient in new IteratorIsolateCollection(TcpClients.Keys))
                {
                    DateTime hungTime = (DateTime)TcpClients[TcpClient];

                    // 如果max busy time 已到则关闭该TcpClient并将其从pool中移除
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
        /// 线程扩展类，用于池对象的维护.
        /// </summary>
        private class MaintenanceThread
        {
            private MaintenanceThread() { }

            private Thread _thread;

            private TcpClientIOPool _pool;
            private long _interval = 1000 * 3; // 每三秒
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
            /// 设置stop变量，中断当前线程并等待
            /// </summary>
            public void StopThread()
            {
                _stopThread = true;
                _thread.Interrupt();
            }

            /// <summary>
            /// 线程维护逻辑.
            /// </summary>
            private void Maintain()
            {
                while(!_stopThread)
                {
                    try
                    {
                        Thread.Sleep((int)_interval);

                        // 如池已初始化，则运行自身维护方法
                        if (_pool.Initialized)
                            _pool.SelfMaintain();

                    }
                    catch (ThreadInterruptedException) { } //当调用TcpClientIOPool.getInstance().shutDown()方法时
                    catch { ; }
                }
            }

            /// <summary>
            /// 启动线程
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