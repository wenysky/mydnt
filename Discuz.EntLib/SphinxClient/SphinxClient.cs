using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Discuz.EntLib.SphinxClient
{
    /// <summary>
    /// SphinxClient API compatable with Sphinx 0.9.8-dev (r985)
    /// 2008 January - Ported to .NET 2.0 by Christopher Gooley (http://iterat.ive.ly) from the Java API version.
    /// publinfo: http://iterat.ive.ly/index.php/2008/01/05/sphinx-search-c-net-client-api/ic 
    /// </summary>    
    class SphinxClient
    {
        #region 静态变量 该代码已被移到了SphinxEnum.cs文件中
        /* 区配方式 */
        //public static int SPH_MATCH_ALL = 0;
        //public static int SPH_MATCH_ANY = 1;
        //public static int SPH_MATCH_PHRASE = 2;
        //public static int SPH_MATCH_BOOLEAN = 3;
        //public static int SPH_MATCH_EXTENDED = 4;

        /* 排序方式 */
        //public static int SPH_SORT_RELEVANCE = 0;
        //public static int SPH_SORT_ATTR_DESC = 1;
        //public static int SPH_SORT_ATTR_ASC = 2;
        //public static int SPH_SORT_TIME_SEGMENTS = 3;
        //public static int SPH_SORT_EXTENDED = 4;

        /* 分组方法 */
        //public static int SPH_GROUPBY_DAY = 0;
        //public static int SPH_GROUPBY_WEEK = 1;
        //public static int SPH_GROUPBY_MONTH = 2;
        //public static int SPH_GROUPBY_YEAR = 3;
        //public static int SPH_GROUPBY_ATTR = 4;
        //public static int SPH_GROUPBY_ATTRPAIR = 5;

        /* 守护进程（searchd）返回信息 */
        //public static int SEARCHD_OK = 0;
        //public static int SEARCHD_ERROR = 1;
        //public static int SEARCHD_RETRY = 2;
        //public static int SEARCHD_WARNING = 3;

        ///* 属性类型 */
        //public static int SPH_ATTR_INTEGER = 1;
        //public static int SPH_ATTR_TIMESTAMP = 2;
        //public static int SPH_ATTR_ORDINAL = 3;
        //public static int SPH_ATTR_BOOL = 4;
        //public static int SPH_ATTR_FLOAT = 5;
        //public static int SPH_ATTR_MULTI = 0x40000000;

        /* 守护进程命令 */
        //private static int SEARCHD_COMMAND_SEARCH = 0;
        //private static int SEARCHD_COMMAND_EXCERPT = 1;
        //private static int SEARCHD_COMMAND_UPDATE = 2;

        /* 守护进程命令版本信息 */
        //private static int VER_MAJOR_PROTO = 0x1;
        //private static int VER_COMMAND_SEARCH = 0x10f;
        //private static int VER_COMMAND_EXCERPT = 0x100;
        //private static int VER_COMMAND_UPDATE = 0x100;

        /* 过滤类型 */
        //private static int SPH_FILTER_VALUES = 0;
        //private static int SPH_FILTER_RANGE = 1;
        //private static int SPH_FILTER_FLOATRANGE = 2; 
        #endregion

        #region 变量声明

        private string _host;
        private int _port;
        private int _offset;
        private int _limit;
        private MatchMode _mode;
        private int[] _weights;
        private SortMode _sort;
        private string _sortby;
        private int _minId;
        private int _maxId;
        private int _filterCount;
        private string _groupBy;
        private GroupBy _groupFunc;
        private string _groupSort;
        private string _groupDistinct;
        private int _maxMatches;
        private int _cutoff;
        private int _retrycount;
        private int _retrydelay;
        private string _latitudeAttr;
        private string _longitudeAttr;
        private float _latitude;
        private float _longitude;
        private string _error;
        private string _warning;
        // 已创建的请求查询数
        private List<byte[]> _requestQueries = new List<byte[]>();
        private Dictionary<string, int> _indexWeights;
        //客户端连接超时数，单位:毫秒 SPH_CLIENT_TIMEOUT_MILLISEC
        public int _sphclienttimeout = 30000;
        // 使用内存流对象代替字节数组，因为它更容易被参数华
        MemoryStream _filterStreamData = new MemoryStream();

        #endregion

        #region 构造方法

        /// <summary>
        /// 创建SphinxClient 实例. 默认主机端口为：localhost:3312. 可使用SetServer()方法进行设置.
        /// </summary>
        public SphinxClient() : this("localhost", 3312)
        {}

        /// <summary>
       /// 创建SphinxClient 实例,可用SetServer()方法进行设置.
       /// </summary>
       /// <param name="host"></param>
       /// <param name="port"></param>
        public SphinxClient(string host, int port)
        {
            _host = host;
            _port = port;
            _offset = 0;
            _limit = 20;
            _mode = MatchMode.SPH_MATCH_ALL;
            _sort = SortMode.SPH_SORT_RELEVANCE;
            _sortby = "";
            _minId = 0;
            _maxId = 0;
            _filterCount = 0;
            //_rawFilters		= new ByteArrayOutputStream();
            //_filters		= new DataOutputStream(_rawFilters);
            _groupBy = "";
            _groupFunc = GroupBy.SPH_GROUPBY_DAY;
            _groupSort = "@group desc";
            _groupDistinct = "";

            _maxMatches = 1000;
            _cutoff = 0;
            _retrycount = 0;
            _retrydelay = 0;
            _latitudeAttr = null;
            _longitudeAttr = null;
            _latitude = 0;
            _longitude = 0;
            _error = "";
            _warning = "";
            //_reqs			= new ArrayList();
            _weights = null;
            //_indexWeights	= new LinkedHashMap();
        }
        #endregion

        #region 主要方法
        /// <summary>
        /// 连接到searchd server 并查询所有索引
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public SphinxResult Query(string query)
        {
            return Query(query, "*");
        }

        /// <summary>
        /// 连接到searchd server 并查询相应索引
        /// </summary>
        /// <param name="query">查询串</param>
        /// <param name="index">索引名称</param>
        /// <returns></returns>
        public SphinxResult Query(string query, string index)
        {
            MyAssert(_requestQueries == null || _requestQueries.Count == 0, "AddQuery() and Query() can not be combined; use RunQueries() instead");

            AddQuery(query, index);
            SphinxResult[] results = RunQueries();
            if (results == null || results.Length < 1)
                return null; // 可能出现网络错误; 应该补充错误信息

            SphinxResult res = results[0];
            _warning = res.warning;
            _error = res.error;
            if (res == null || res.getStatus() == (int)ResultType.SEARCHD_ERROR)
                return null;
            else
                return res;
        }

        public int AddQuery(string query, string index)
        {
            byte[] outputdata = new byte[2048];

            /* build request */
            try
            {
                MemoryStream ms = new MemoryStream();
                BinaryWriter sw = new BinaryWriter(ms);

                SphinxUtils.WriteToStream(sw, _offset);
                SphinxUtils.WriteToStream(sw, _limit);
                SphinxUtils.WriteToStream(sw, (int)_mode);
                SphinxUtils.WriteToStream(sw, (int)_sort);
                SphinxUtils.WriteToStream(sw, _sortby);
                SphinxUtils.WriteToStream(sw, query);

                _weights = new int[] { 100, 1 };
                int weightLen = _weights != null ? _weights.Length : 0;

                SphinxUtils.WriteToStream(sw, weightLen);
                if (_weights != null)
                {
                    for (int i = 0; i < _weights.Length; i++)
                        SphinxUtils.WriteToStream(sw, _weights[i]);
                }

                SphinxUtils.WriteToStream(sw, index);
                SphinxUtils.WriteToStream(sw, 0);
                SphinxUtils.WriteToStream(sw, _minId);
                SphinxUtils.WriteToStream(sw, _maxId);

                /* filters */
                SphinxUtils.WriteToStream(sw, _filterCount);
                if (_filterCount > 0 && _filterStreamData.Length > 0)
                {
                    byte[] filterdata = new byte[_filterStreamData.Length];
                    _filterStreamData.Seek(0, SeekOrigin.Begin);
                    _filterStreamData.Read(filterdata, 0, (int)_filterStreamData.Length);
                    SphinxUtils.WriteToStream(sw, filterdata);
                }

                /* group-by, max matches, sort-by-group flag */
                SphinxUtils.WriteToStream(sw, (int)_groupFunc);
                SphinxUtils.WriteToStream(sw, _groupBy);
                SphinxUtils.WriteToStream(sw, _maxMatches);
                SphinxUtils.WriteToStream(sw, _groupSort);
                SphinxUtils.WriteToStream(sw, _cutoff);
                SphinxUtils.WriteToStream(sw, _retrycount);
                SphinxUtils.WriteToStream(sw, _retrydelay);
                SphinxUtils.WriteToStream(sw, _groupDistinct);

                /* anchor point */
                if (_latitudeAttr == null || _latitudeAttr.Length == 0 || _longitudeAttr == null || _longitudeAttr.Length == 0)
                {
                    SphinxUtils.WriteToStream(sw, 0);
                }
                else
                {
                    SphinxUtils.WriteToStream(sw, 1);
                    SphinxUtils.WriteToStream(sw, _latitudeAttr);
                    SphinxUtils.WriteToStream(sw, _longitudeAttr);
                    SphinxUtils.WriteToStream(sw, _latitude);
                    SphinxUtils.WriteToStream(sw, _longitude);
                }

                /* per-index weights */
                //sw.Write(_indexWeights.size());
                SphinxUtils.WriteToStream(sw, 0);
                //for (Iterator e = _indexWeights.keySet().iterator(); e.hasNext();) {
                //    string indexName = (string) e.next();
                //    Integer weight = (Integer) _indexWeights.get(indexName);
                //    sw.Write(Encoding.UTF8.GetBytes(indexName));
                //    sw.Write(weight.intValue());
                //}
                sw.Flush();
                ms.Seek(0, SeekOrigin.Begin);

                byte[] data = new byte[ms.Length];
                ms.Read(data, 0, (int)ms.Length);

                int qIndex = _requestQueries.Count;
                _requestQueries.Add(data);

                return qIndex;

            }
            catch (Exception ex)
            {
                MyAssert(false, "error on AddQuery: " + ex.Message);
            }
            finally
            {
                try
                {
                    //_filters.close();
                    //_rawFilters.close();
                }
                catch (IOException e)
                {
                    MyAssert(false, "error on AddQuery: " + e.Message);
                }
            }
            return -1;
        }

        /// <summary>
        /// 运行之前添加的查询请求
        /// </summary>
        /// <returns></returns>
        public SphinxResult[] RunQueries()
        {
            if (_requestQueries == null || _requestQueries.Count < 1)
            {
                _error = "no queries defined, issue AddQuery() first";
                return null;
            }

            TcpClient sock = Connect();
            if (sock == null) return null;

            StreamReader sr = new StreamReader(sock.GetStream());
            BinaryWriter sw = new BinaryWriter(sock.GetStream());
            /* send query, get response */
            int nreqs = _requestQueries.Count;
            try
            {
                SphinxUtils.WriteToStream(sw, 1);
                SphinxUtils.WriteToStream(sw, (short)CommandType.SEARCHD_COMMAND_SEARCH);
                //sw.Write(BitConverter.GetBytes((short)SEARCHD_COMMAND_SEARCH));
                SphinxUtils.WriteToStream(sw, (short)VerCommand.VER_COMMAND_SEARCH);
                //return null;

                int rqLen = 4;
                for (int i = 0; i < nreqs; i++)
                {
                    byte[] subRq = (byte[])_requestQueries[i];
                    rqLen += subRq.Length;
                }
                SphinxUtils.WriteToStream(sw, rqLen);
                SphinxUtils.WriteToStream(sw, nreqs);
                for (int i = 0; i < nreqs; i++)
                {
                    byte[] subRq = (byte[])_requestQueries[i];
                    SphinxUtils.WriteToStream(sw, subRq);
                }
            }
            catch (Exception e)
            {
                MyAssert(false, "Query: Unable to create read/write streams: " + e.Message);
                return null;
            }

            /* reset requests */
            _requestQueries = new List<byte[]>();
            /* get response */
            byte[] response = null;
            response = GetResponse(sock, (int)VerCommand.VER_COMMAND_SEARCH);
            if (response == null) return null;

            /* parse response */
            SphinxResult[] results = new SphinxResult[nreqs];
            BinaryReader bw = new BinaryReader(new MemoryStream(response));

            /* read schema */
            int ires;
            try
            {
                for (ires = 0; ires < nreqs; ires++)
                {
                    SphinxResult res = new SphinxResult();
                    results[ires] = res;

                    int status = SphinxUtils.ReadInt32(bw);
                    res.setStatus(status);
                    if (status != (int)ResultType.SEARCHD_OK)
                    {
                        string message = SphinxUtils.ReadUtf8(bw);
                        if (status == (int)ResultType.SEARCHD_WARNING)
                            res.warning = message;
                        else
                        {
                            res.error = message;
                            continue;
                        }
                    }

                    /* read fields */
                    int nfields = SphinxUtils.ReadInt32(bw);
                    res.fields = new string[nfields];
                    //int pos = 0;
                    for (int i = 0; i < nfields; i++)
                        res.fields[i] = SphinxUtils.ReadUtf8(bw);

                    /* read arrts */
                    int nattrs = SphinxUtils.ReadInt32(bw);
                    res.attrTypes = new int[nattrs];
                    res.attrNames = new string[nattrs];
                    for (int i = 0; i < nattrs; i++)
                    {
                        string AttrName = SphinxUtils.ReadUtf8(bw);
                        int AttrType = SphinxUtils.ReadInt32(bw);
                        res.attrNames[i] = AttrName;
                        res.attrTypes[i] = AttrType;
                    }

                    /* read match count */
                    int count = SphinxUtils.ReadInt32(bw);
                    int id64 = SphinxUtils.ReadInt32(bw);
                    res.matches = new SphinxMatch[count];
                    for (int matchesNo = 0; matchesNo < count; matchesNo++)
                    {
                        SphinxMatch docInfo;
                        docInfo = new SphinxMatch((id64 == 0) ? SphinxUtils.ReadUInt32(bw) : SphinxUtils.ReadInt64(bw),
                                SphinxUtils.ReadInt32(bw));

                        /* read matches */
                        for (int attrNumber = 0; attrNumber < res.attrTypes.Length; attrNumber++)
                        {
                            string attrName = res.attrNames[attrNumber];
                            int type = res.attrTypes[attrNumber];
                            /* handle floats */
                            if (type == (int)AttrType.SPH_ATTR_FLOAT)
                            {
                                //docInfo.attrValues.add ( attrNumber, bw.ReadDouble  ) );
                                throw new NotImplementedException("we don't read floats yet");
                                continue;
                            }

                            /* handle everything else as unsigned ints */
                            long val = bw.ReadUInt32();
                            if ((type & (int)AttrType.SPH_ATTR_MULTI) != 0)
                            {
                                long[] vals = new long[(int)val];
                                for (int k = 0; k < val; k++)
                                    vals[k] = SphinxUtils.ReadUInt32(bw);

                                docInfo.attrValues.Add(vals);
                            }
                            else
                                docInfo.attrValues.Add(val);
                        }
                        res.matches[matchesNo] = docInfo;
                    }

                    res.total = SphinxUtils.ReadInt32(bw);
                    res.totalFound = SphinxUtils.ReadInt32(bw);
                    res.time = SphinxUtils.ReadInt32(bw) / 1000; /* FIXME! format should be %.3f */

                    res.words = new SphinxWordInfo[SphinxUtils.ReadInt32(bw)];
                    for (int i = 0; i < res.words.Length; i++)
                        res.words[i] = new SphinxWordInfo(SphinxUtils.ReadUtf8(bw), SphinxUtils.ReadUInt32(bw), SphinxUtils.ReadUInt32(bw));
                }
                bw.Close();
                return results;

            }
            catch (IOException e)
            {
                MyAssert(false, "unable to parse response: " + e.Message);
            }
            finally
            {
                try
                {
                    bw.Close();
                }
                catch (IOException e)
                {
                    MyAssert(false, "unable to close DataInputStream: " + e.Message);
                }
            }
            return null;
        }

        #endregion

        #region Getters and Setters
        /// <summary>
        /// 返回最新的错误信息（如为空表示没有错误）
        /// </summary>
        /// <returns></returns>
        public string GetLastError()
        {
            return _error;
        }

        /// <summary>
        /// 返回最新的警告信息（如为空表示没有警告）
        /// </summary>
        /// <returns></returns>
        public string GetLastWarning()
        {
            return _warning;
        }

        /// <summary>
        /// 设置搜索服务的地址和端口
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void SetServer(string host, int port)
        {
            MyAssert(host != null && host.Length > 0, "host name must not be empty");
            MyAssert(port > 0 && port < 65536, "port must be in 1..65535 range");
            _host = host;
            _port = port;
        }

        /** Set matches offset and limit to return to client, max matches to retrieve on server, and cutoff. */
        public void SetLimits(int offset, int limit, int max, int cutoff)
        {
            MyAssert(offset >= 0, "offset must be greater than or equal to 0");
            MyAssert(limit > 0, "limit must be greater than 0");
            MyAssert(max > 0, "max must be greater than 0");
            MyAssert(cutoff >= 0, "max must be greater than or equal to 0");

            _offset = offset;
            _limit = limit;
            _maxMatches = max;
            _cutoff = cutoff;
        }

        /** Set matches offset and limit to return to client, and max matches to retrieve on server. */
        public void SetLimits(int offset, int limit, int max)
        {
            SetLimits(offset, limit, max, _cutoff);
        }

        /** Set matches offset and limit to return to client. */
        public void SetLimits(int offset, int limit)
        {
            SetLimits(offset, limit, _maxMatches, _cutoff);
        }

        /** Set matches offset and limit to return to client. */
        public void SetClientTimeOut(int timeout)
        {
            _sphclienttimeout = timeout;
        }

        /** Set matching mode. */
        public void SetMatchMode(MatchMode mode)
        {
            MyAssert(
                mode == MatchMode.SPH_MATCH_ALL ||
                mode == MatchMode.SPH_MATCH_ANY ||
                mode == MatchMode.SPH_MATCH_PHRASE ||
                mode == MatchMode.SPH_MATCH_BOOLEAN ||
                mode == MatchMode.SPH_MATCH_EXTENDED, "unknown mode value; use one of the available SPH_MATCH_xxx constants");
            _mode = mode;
        }

        /** Set sorting mode. */
        public void SetSortMode(SortMode mode, string sortby)
        {
            MyAssert(
                mode == SortMode.SPH_SORT_RELEVANCE ||
                mode == SortMode.SPH_SORT_ATTR_DESC ||
                mode == SortMode.SPH_SORT_ATTR_ASC ||
                mode == SortMode.SPH_SORT_TIME_SEGMENTS ||
                mode == SortMode.SPH_SORT_EXTENDED, "unknown mode value; use one of the available SPH_SORT_xxx constants");
            MyAssert(mode == SortMode.SPH_SORT_RELEVANCE || (sortby != null && sortby.Length > 0), "sortby string must not be empty in selected mode");

            _sort = mode;
            _sortby = (sortby == null) ? "" : sortby;
        }

        /** Set per-field weights (all values must be positive). */
        public void SetWeights(int[] weights)
        {
            MyAssert(weights != null, "weights must not be null");
            for (int i = 0; i < weights.Length; i++)
            {
                int weight = weights[i];
                MyAssert(weight > 0, "all weights must be greater than 0");
            }
            _weights = weights;
        }

        /**
         * Set per-index weights
         *
         * @param indexWeights hash which maps string index names to Integer weights
         */
        public void SetIndexWeights(Dictionary<string, int> indexWeights)
        {
            /* FIXME! implement checks here */
            _indexWeights = indexWeights;
        }

        /// <summary>
        /// 设置 document IDs 的区配范围.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void SetIDRange(int min, int max)
        {
            MyAssert(min <= max, "min must be less or equal to max");
            _minId = min;
            _maxId = max;
        }

        /// <summary>
        ///  设置值过滤条件.                
        /// </summary>
        /// <param name="attribute">过滤属性名称</param>
        /// <param name="values">要被匹配的值(数组)</param>
        /// <param name="exclude">是否排除匹配的结果</param>
        public void SetFilter(string attribute, int[] values, bool exclude)
        {
            MyAssert(values != null && values.Length > 0, "values array must not be null or empty");
            MyAssert(attribute != null && attribute.Length > 0, "attribute name must not be null or empty");

            try
            {
                BinaryWriter bw = new BinaryWriter(_filterStreamData);

                SphinxUtils.WriteToStream(bw, attribute);
                SphinxUtils.WriteToStream(bw, (int)FilterType.SPH_FILTER_VALUES);
                SphinxUtils.WriteToStream(bw, values.Length);

                for (int i = 0; i < values.Length; i++)
                    SphinxUtils.WriteToStream(bw, values[i]);

                SphinxUtils.WriteToStream(bw, exclude ? 1 : 0);

            }
            catch (Exception e)
            {
                MyAssert(false, "IOException: " + e.Message);
            }
            _filterCount++;
        }

        /// <summary>
        ///  设置值过滤条件.                
        /// </summary>
        /// <param name="attribute">过滤属性名称</param>
        /// <param name="values">要被匹配的值</param>
        /// <param name="exclude">是否排除匹配的结果</param>
        public void SetFilter(string attribute, int value, bool exclude)
        {
            int[] values = new int[] { value };
            SetFilter(attribute, values, exclude);
        }

        /// <summary>
        ///  设置值过滤范围条件.                
        /// </summary>
        /// <param name="attribute">过滤属性名称</param>
        /// <param name="min">要被匹配的最小值</param>
        /// <param name="max">要被匹配的最大值</param>
        /// <param name="exclude">是否排除匹配的结果</param>
        public void SetFilterRange(string attribute, int min, int max, bool exclude)
        {
            MyAssert(min <= max, "min must be less or equal to max");
            try
            {
                BinaryWriter bw = new BinaryWriter(_filterStreamData);
                SphinxUtils.WriteToStream(bw, attribute);
                SphinxUtils.WriteToStream(bw, (int)FilterType.SPH_FILTER_RANGE);
                SphinxUtils.WriteToStream(bw, min);
                SphinxUtils.WriteToStream(bw, max);
                SphinxUtils.WriteToStream(bw, exclude ? 1 : 0);

            }
            catch (Exception e)
            {
                MyAssert(false, "IOException: " + e.Message);
            }
            _filterCount++;
        }

        /// <summary>
        ///  设置值（float类型）过滤范围条件.                
        /// </summary>
        /// <param name="attribute">过滤属性名称</param>
        /// <param name="min">要被匹配的最小值</param>
        /// <param name="max">要被匹配的最大值</param>
        /// <param name="exclude">是否排除匹配的结果</param>
        public void SetFilterFloatRange(string attribute, float min, float max, bool exclude)
        {
            throw new NotImplementedException("Float support not available yet");
#if false
		    MyAssert ( min<=max, "min must be less or equal to max" );
		    try
		    {
			    writeNetUTF8 ( _filters, attribute );
			    _filters.writeInt ( SPH_FILTER_RANGE );
			    _filters.writeFloat ( min );
			    _filters.writeFloat ( max );
			    _filters.writeInt ( exclude ? 1 : 0 );
		    } catch ( Exception e )
		    {
			    MyAssert ( false, "IOException: " + e.getMessage() );
		    }
		    _filterCount++;
#endif
        }

        /// <summary>
        /// 重设所有过滤选项信息
        /// </summary>
        public void ResetFilters()
        {
            /* should we close them first? */
            _filterStreamData = new MemoryStream();
            _filterCount = 0;

            /* reset GEO anchor */
            _latitudeAttr = null;
            _longitudeAttr = null;
            _latitude = 0;
            _longitude = 0;
        }

        /**
	     * Setup geographical anchor point.
	     *
	     * Required to use @geodist in filters and sorting.
	     * Distance will be computed to this point.
	     *
	     * @param latitudeAttr		the name of latitude attribute
	     * @param longitudeAttr		the name of longitude attribute
	     * @param latitude			anchor point latitude, in radians
	     * @param longitude			anchor point longitude, in radians
	     *
	     * @throws SphinxException on invalid parameters
	     */
        public void SetGeoAnchor(string latitudeAttr, string longitudeAttr, float latitude, float longitude)
        {
            MyAssert(latitudeAttr != null && latitudeAttr.Length > 0, "longitudeAttr string must not be null or empty");
            MyAssert(longitudeAttr != null && longitudeAttr.Length > 0, "longitudeAttr string must not be null or empty");

            _latitudeAttr = latitudeAttr;
            _longitudeAttr = longitudeAttr;
            _latitude = latitude;
            _longitude = longitude;
        }

        /** Set grouping attribute and function. */
        public void SetGroupBy(string attribute, GroupBy func, string groupsort)
        {
            MyAssert(
                func == GroupBy.SPH_GROUPBY_DAY ||
                func == GroupBy.SPH_GROUPBY_WEEK ||
                func == GroupBy.SPH_GROUPBY_MONTH ||
                func == GroupBy.SPH_GROUPBY_YEAR ||
                func == GroupBy.SPH_GROUPBY_ATTR ||
                func == GroupBy.SPH_GROUPBY_ATTRPAIR, "unknown func value; use one of the available SPH_GROUPBY_xxx constants");

            _groupBy = attribute;
            _groupFunc = func;
            _groupSort = groupsort;
        }

        /** Set grouping attribute and function with default ("@group desc") groupsort (syntax sugar). */
        public void SetGroupBy(string attribute, GroupBy func)
        {
            SetGroupBy(attribute, func, "@group desc");
        }

        /** Set count-distinct attribute for group-by queries. */
        public void SetGroupDistinct(string attribute)
        {
            _groupDistinct = attribute;
        }

        /** Set distributed retries count and delay. */
        public void SetRetries(int count, int delay)
        {
            MyAssert(count >= 0, "count must not be negative");
            MyAssert(delay >= 0, "delay must not be negative");
            _retrycount = count;
            _retrydelay = delay;
        }

        /** Set distributed retries count with default (zero) delay (syntax sugar). */
        public void SetRetries(int count)
        {
            SetRetries(count, 0);
        }


        #endregion

        #region 私有方法

        /** Get and check response packet from searchd (internal method). */
        private byte[] GetResponse(TcpClient sock, int client_ver)
        {
            /* connect */
            BinaryReader br = null;
            NetworkStream SockInput = null;
            try
            {
                SockInput = sock.GetStream();
                br = new BinaryReader(SockInput);
            }
            catch (IOException e)
            {
                MyAssert(false, "getInputStream() failed: " + e.Message);
                return null;
            }

            /* read response */
            byte[] response = null;
            short status = 0, ver = 0;
            int len = 0;
            try
            {
                /* read status fields */
                status = SphinxUtils.ReadInt16(br);
                ver = SphinxUtils.ReadInt16(br);
                len = SphinxUtils.ReadInt32(br);

                /* read response if non-empty */
                MyAssert(len > 0, "zero-sized searchd response body");
                if (len > 0)
                {
                    response = br.ReadBytes(len);
                }
                else
                {
                    /* FIXME! no response, return null? */
                }

                /* check status */
                if (status == (int)ResultType.SEARCHD_WARNING)
                {
                    //DataInputStream in = new DataInputStream ( new ByteArrayInputStream ( response ) );

                    //int iWarnLen = in.ReadInt32 ();
                    //_warning = new string ( response, 4, iWarnLen );

                    //System.arraycopy ( response, 4+iWarnLen, response, 0, response.Length-4-iWarnLen );
                    _error = "searchd warning";
                    return null;

                }
                else if (status == (int)ResultType.SEARCHD_ERROR)
                {
                    _error = "searchd error: " + Encoding.UTF8.GetString(response, 4, response.Length - 4);
                    return null;

                }
                else if (status == (int)ResultType.SEARCHD_RETRY)
                {
                    _error = "temporary searchd error: " + Encoding.UTF8.GetString(response, 4, response.Length - 4);
                    return null;

                }
                else if (status != (int)ResultType.SEARCHD_OK)
                {
                    _error = "searched returned unknown status, code=" + status;
                    return null;
                }

            }
            catch (IOException e)
            {
                if (len != 0)
                {
                    _error = "failed to read searchd response (status=" + status + ", ver=" + ver + ", len=" + len + ", trace=" + e.StackTrace + ")";
                }
                else
                {
                    _error = "received zero-sized searchd response (searchd crashed?): " + e.Message;
                }
                return null;

            }
            finally
            {
                try
                {
                    if (br != null) br.Close();
                    if (sock != null && !sock.Connected) sock.Close();
                }
                catch (IOException e)
                {
                    /* silently ignore close failures; nothing could be done anyway */
                }
            }

            return response;
        }

        /** Connect to searchd and exchange versions (internal method). */
        private TcpClient Connect()
        {
            TcpClient sock;
            try
            {
                //sock = new Socket(_host, _port);
                //sock = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IPv4);
                sock = new TcpClient(_host, _port);
                sock.ReceiveTimeout = _sphclienttimeout;
            }
            catch (Exception e)
            {
                _error = "connection to " + _host + ":" + _port + " failed: " + e.Message;
                return null;
            }

            NetworkStream ns = null;

            try
            {
                ns = sock.GetStream();

                BinaryReader sr = new BinaryReader(ns);
                BinaryWriter sw = new BinaryWriter(ns);

                //char[] buf = new char[1024];

                //if (size != 1)
                //    throw new Exception("Connect: Could not read version number from searchd server");

                //int version = Convert.ToInt16(buf[0]);
                int version = sr.ReadInt32();

                if (version < 1)
                {
                    sock.Close();
                    _error = "expected searchd protocol version 1+, got version " + version;
                    return null;
                }

                //sw.Write(VER_MAJOR_PROTO);
            }
            catch (IOException e)
            {
                _error = "Connect: Read from socket failed: " + e.Message;
                try
                {
                    sock.Close();
                }
                catch (IOException e1)
                {
                    _error = _error + " Cannot close socket: " + e1.Message;
                }
                return null;
            }
            return sock;
        }

        #endregion

        #region Other Helpers
        private void MyAssert(bool condition, string err)
        {
            if (!condition)
            {
                _error = err;
                throw new Exception(err);
            }
        }
        #endregion
    }
}
