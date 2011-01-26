using System.Collections;
using Discuz.Config;
using Discuz.Common;

namespace Discuz.EntLib
{
    /// <summary>
    /// MemCache管理操作类
    /// </summary>
    public sealed class MemCachedManager
    {
        #region 静态方法和属性

        /// <summary>
        /// memcachedClient对象
        /// </summary>
        private static MemcachedClient mc = null;

        /// <summary>
        /// 池对象 
        /// </summary>
        private static SockIOPool pool = null;

        /// <summary>
        /// memcached配置文件信息
        /// </summary>
        private static MemCachedConfigInfo memCachedConfigInfo = MemCachedConfigs.GetConfig();

        /// <summary>
        /// 服务器列表
        /// </summary>
        private static string [] serverList = null;

        /// <summary>
        /// 静态构造方法，初始化链接池管理对象
        /// </summary>
        static MemCachedManager()
        {
            CreateManager();
        }

        /// <summary>
        /// 创建链接池管理对象 
        /// </summary>
        private static void CreateManager()
        {
            serverList = Utils.SplitString(memCachedConfigInfo.ServerList, ",");

            pool = SockIOPool.GetInstance(memCachedConfigInfo.PoolName);
            pool.SetServers(serverList);
            pool.InitConnections = memCachedConfigInfo.IntConnections;//初始化链接数
            pool.MinConnections = memCachedConfigInfo.MinConnections;//最少链接数
            pool.MaxConnections = memCachedConfigInfo.MaxConnections;//最大连接数
            pool.SocketConnectTimeout = memCachedConfigInfo.SocketConnectTimeout;//Socket链接超时时间
            pool.SocketTimeout = memCachedConfigInfo.SocketTimeout;// Socket超时时间
            pool.MaintenanceSleep = memCachedConfigInfo.MaintenanceSleep;//维护线程休息时间
            pool.Failover = memCachedConfigInfo.FailOver; //失效转移(一种备份操作模式)
            pool.Nagle = memCachedConfigInfo.Nagle;//是否用nagle算法启动socket
            pool.HashingAlgorithm = HashingAlgorithm.NewCompatibleHash;// memCachedConfigInfo.HashingAlgorithm;
            pool.Initialize();            

            mc = new MemcachedClient();
            mc.PoolName = memCachedConfigInfo.PoolName;
            mc.EnableCompression = false;
        }

        /// <summary>
        ///  获取指定memcached服务器的链接池管理对象，并返回相应MemcachedClient实例
        /// </summary>
        /// <param name="poolName">要使用的链接池信息</param>
        /// <param name="serverArrayList">要链接的memcached服务器列表</param>
        /// <returns></returns>
        public static MemcachedClient GetMemcachedClient(string poolName, ArrayList serverArrayList)
        {
            memCachedConfigInfo = MemCachedConfigs.GetConfig();
            SockIOPool pool = SockIOPool.GetInstance(poolName);
            pool.SetServers(serverArrayList);
            pool.InitConnections = memCachedConfigInfo.IntConnections;//初始化链接数
            pool.MinConnections = memCachedConfigInfo.MinConnections;//最少链接数
            pool.MaxConnections = memCachedConfigInfo.MaxConnections;//最大连接数
            pool.SocketConnectTimeout = memCachedConfigInfo.SocketConnectTimeout;//Socket链接超时时间
            pool.SocketTimeout = memCachedConfigInfo.SocketTimeout;// Socket超时时间
            pool.MaintenanceSleep = memCachedConfigInfo.MaintenanceSleep;//维护线程休息时间
            pool.Failover = memCachedConfigInfo.FailOver; //失效转移(一种备份操作模式)
            pool.Nagle = memCachedConfigInfo.Nagle;//是否用nagle算法启动socket
            pool.HashingAlgorithm = HashingAlgorithm.NewCompatibleHash;
            pool.Initialize();

            MemcachedClient mc = new MemcachedClient();
            mc.PoolName = poolName;
            mc.EnableCompression = false;
            return mc;
        }

        /// <summary>
        /// 缓存服务器地址列表
        /// </summary>
        public static string[] ServerList
        {
            set
            {
                if (value != null)
                    serverList = value;
            }
            get { return serverList; }
        }

        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static MemcachedClient CacheClient
        {
            get
            {
                if (mc == null)
                    CreateManager();

                return mc;
            }
        }

        /// <summary>
        /// 释放池中资源
        /// </summary>
        public static void Dispose()
        {
            if (MemCachedConfigs.GetConfig().ApplyMemCached && pool != null)
                pool.Shutdown();
        }

        /// <summary>
        /// 获取当前缓存键值所存储在的服务器
        /// </summary>
        /// <param name="key">当前缓存键</param>
        /// <returns>当前缓存键值所存储在的服务器</returns>
        public static string GetSocketHost(string key)
        {
            string hostName = "";
            SockIO sock = null;
            try
            {
                sock = SockIOPool.GetInstance(memCachedConfigInfo.PoolName).GetSock(key);
                if (sock != null)
                {
                    hostName = sock.Host;
                }
            }
            finally
            {
                if (sock != null)
                    sock.Close();
            }
            return hostName;
        }


        /// <summary>
        /// 获取有效的服务器地址
        /// </summary>
        /// <returns>有效的服务器地</returns>
        public static string[] GetConnectedSocketHost()
        {
            SockIO sock = null;
            string connectedHost = null;
            foreach (string hostName in serverList)
            {
                if (!Discuz.Common.Utils.StrIsNullOrEmpty(hostName))
                {
                    try
                    {
                        sock = SockIOPool.GetInstance(memCachedConfigInfo.PoolName).GetConnection(hostName);
                        if (sock != null)
                        {
                            connectedHost = Discuz.Common.Utils.MergeString(hostName, connectedHost);
                        }
                    }
                    finally
                    {
                        if (sock != null)
                            sock.Close();
                    }
                }
            }
            return Discuz.Common.Utils.SplitString(connectedHost, ",");
        }

        /// <summary>
        /// 获取服务器端缓存的状态信息
        /// </summary>
        /// <returns>memcached状态信息</returns>
        public static ArrayList GetStats()
        {
            ArrayList arrayList = new ArrayList();
            foreach (string server in serverList)
            {
                arrayList.Add(server);
            }
            return GetStats(arrayList, Stats.Default, null);
        }

        /// <summary>
        /// 通过特定的telnet指令获取服务器端的状态详细信息，Stats为相关命令行参数
        /// </summary>
        /// <param name="serverArrayList">memcached服务器地址</param>
        /// <param name="statsCommand">Stats相关命令行</param>
        /// <param name="param">相关参数</param>
        /// <returns>memcached状态信息</returns>
        public static ArrayList GetStats(ArrayList serverArrayList, Stats statsCommand, string param)
        {
            ArrayList statsArray = new ArrayList();
            param =  Utils.StrIsNullOrEmpty(param) ? "" : param.Trim().ToLower();

            string commandstr = "stats";
            //转换stats命令参数
            switch (statsCommand)
            {
                case Stats.Reset: { commandstr = "stats reset"; break; }
                case Stats.Malloc: { commandstr = "stats malloc"; break; }
                case Stats.Maps: { commandstr = "stats maps"; break; }
                case Stats.Sizes: { commandstr = "stats sizes"; break; }
                case Stats.Slabs: { commandstr = "stats slabs"; break; }
                case Stats.Items: { commandstr = "stats items"; break; }
                case Stats.CachedDump: 
                {
                    string[] statsparams = Utils.SplitString(param, " ");
                    if (statsparams.Length >= 2)
                        if(Utils.IsNumericArray(statsparams))
                            commandstr = "stats cachedump  " + param; 

                    break;                     
                }
                case Stats.Detail: 
                    { 
                        if(string.Equals(param, "on") || string.Equals(param, "off") || string.Equals(param, "dump"))
                            commandstr = "stats detail " + param.Trim(); 

                        break; 
                    }
                default: { commandstr = "stats"; break; }
            }
            ////加载返回值
            //Hashtable stats = MemCachedManager.CacheClient.Stats(serverArrayList, commandstr, null);
            //foreach (string key in stats.Keys)
            //{
            //    statsArray.Add(key);
            //    Hashtable values = (Hashtable)stats[key];
            //    foreach (string key2 in values.Keys)
            //    {
            //        statsArray.Add(key2 + ":" + values[key2]);
            //    }
            //}
            //return statsArray;

            //加载返回值
            Hashtable stats = MemCachedManager.CacheClient.Stats(serverArrayList, commandstr, null);
            foreach (string key in stats.Keys)
            {
                statsArray.Add("================================================================================================");
                statsArray.Add(key);
                Hashtable values = (Hashtable)stats[key];
                foreach (string key2 in values.Keys)
                {
                    if (statsCommand == Stats.CachedDump)
                        statsArray.Add(key2);
                    else
                        statsArray.Add(key2 + ":" + values[key2]);
                }
            }
            return statsArray;
        }


        /// <summary>
        /// 获取指定服务器的缓存项信息列表
        /// </summary>
        /// <param name="serverArrayList">指定服务器列表</param>
        /// <param name="poolName">使用的链接池名称</param>
        /// <returns>缓存键信</returns>
        public static ArrayList GetCachedKeyList(ArrayList serverArrayList, string poolName)
        {
            //加载返回值
            Hashtable stats = GetMemcachedClient(poolName, serverArrayList).Stats(serverArrayList, "stats items", poolName);
            ArrayList keyList = new ArrayList();
            foreach (string key in stats.Keys)
            {
                //查出已存储数据的items
                foreach (string key2 in ((Hashtable)stats[key]).Keys)
                {
                    Hashtable currentstats = MemCachedManager.CacheClient.Stats(serverArrayList, "stats cachedump " + key2.Split(':')[1] + " 0", poolName);
                    //找出相应的缓存KEY
                    foreach (string currentkey in currentstats.Keys)
                    {
                        foreach (string cachekey in ((Hashtable)currentstats[currentkey]).Keys)
                        {
                            if (!keyList.Contains(cachekey))
                                keyList.Add(cachekey);
                        }
                    }
                }
            }
            return keyList;
        }


        /// <summary>
        /// Stats命令行参数
        /// </summary>
        public enum Stats
        {
            /// <summary>
            /// stats : 显示服务器信息, 统计数据等
            /// </summary>
            Default = 0,
            /// <summary>
            /// stats reset : 清空统计数据
            /// </summary>
            Reset = 1,
            /// <summary>
            /// stats malloc : 显示内存分配数据
            /// </summary>
            Malloc = 2,
            /// <summary>
            /// stats maps : 显示"/proc/self/maps"数据
            /// </summary>
            Maps =3,
            /// <summary>
            /// stats sizes
            /// </summary>
            Sizes = 4,
            /// <summary>
            /// stats slabs : 显示各个slab的信息,包括chunk的大小,数目,使用情况等
            /// </summary>
            Slabs = 5,
            /// <summary>
            /// stats items : 显示各个slab中item的数目和最老item的年龄(最后一次访问距离现在的秒数)
            /// </summary>
            Items = 6, 
            /// <summary>
            /// stats cachedump slab_id limit_num : 显示某个slab中的前 limit_num 个 key 列表
            /// </summary>
            CachedDump =7,
            /// <summary>
            /// stats detail [on|off|dump] : 设置或者显示详细操作记录   on:打开详细操作记录  off:关闭详细操作记录 dump: 显示详细操作记录(每一个键值get,set,hit,del的次数)
            /// </summary>
            Detail = 8 
        }
        #endregion
    }
}
