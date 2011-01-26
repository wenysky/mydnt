using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Config;
using Discuz.Common.Generic;
using Discuz.Common;
using Discuz.EntLib.TokyoTyrant;
using Discuz.Data;

namespace Discuz.EntLib.TokyoTyrant.Data
{
    public class OnlineUsers : Discuz.Cache.Data.ICacheOnlineUser
    {
        private static TcpClientIOPool pool = TcpClientIOPool.GetInstance("dnt_online");

        private static DBCache ttCache = EntLibConfigs.GetConfig().Cacheonlineuser;

        private static DataTable onlineUserList = null;

        static OnlineUsers()
        {
            onlineUserList = DatabaseProvider.GetInstance().GetOnlineUserListTable();
            onlineUserList.Rows.Clear();
     
            InitalTcpClientPool();
        }

        static void InitalTcpClientPool()
        {
            pool = TcpClientIOPool.GetInstance(ttCache.PoolName);
            pool.SetServers(new string[] { ttCache.Host + ":" + ttCache.Port });
            pool.InitConnections = ttCache.IntConnections;
            pool.MinConnections = ttCache.MinConnections;
            pool.MaxConnections = ttCache.MaxConnections;
            pool.MaxIdle = ttCache.MaxIdle;
            pool.MaxBusy = ttCache.MaxBusy;
            pool.MaintenanceSleep = ttCache.MaintenanceSleep;
            pool.TcpClientTimeout = ttCache.MaxBusy;
            pool.TcpClientConnectTimeout = ttCache.TcpClientConnectTimeout;
            pool.Initialize();
        }

    
        /// <summary>
        /// 获得在线用户总数量
        /// </summary>
        /// <returns>用户数量</returns>
        public int GetOnlineAllUserCount()
        {
            int count =  (int) TokyoTyrantService.GetRecordCount(pool);                 
            return count == 0 ? 1 : count;
        }

        /// <summary>
        /// 创建在线表记录(本方法在应用程序初始化时被调用)
        /// </summary>
        /// <returns></returns>
        public int CreateOnlineTable()
        {
            TokyoTyrantService.Vanish(pool); 
            TokyoTyrantService.SetIndex(pool, "olid", IndexOption.DECIMAL);
            TokyoTyrantService.SetIndex(pool, "userid", IndexOption.DECIMAL);
            TokyoTyrantService.SetIndex(pool, "password", IndexOption.LEXICAL);
            TokyoTyrantService.SetIndex(pool, "ip", IndexOption.LEXICAL);
            TokyoTyrantService.SetIndex(pool, "forumid", IndexOption.DECIMAL);
            TokyoTyrantService.SetIndex(pool, "lastupdatetime", IndexOption.DECIMAL);           
            
            return 0;
        }

        /// <summary>
        /// 获得在线注册用户总数量
        /// </summary>
        /// <returns>用户数量</returns>
        public int GetOnlineUserCount()
        {
            return TokyoTyrantService.QueryRecordsCount(pool, new Query().NumberGreaterThan("userid", 0));
        }

        /// <summary>
        /// 获得在线不可见用户量
        /// </summary>
        /// <returns>用户数量</returns>
        public int GetInvisibleOnlineUserCount()
        {
            return TokyoTyrantService.QueryRecordsCount(pool, new Query().NumberGreaterThan("userid", 0).NumberEquals("invisible", 1));
        }
     

        /// <summary>
        /// 获取在线用户列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetOnlineUserListTable()
        {
            onlineUserList.Rows.Clear();
            int pageid = Discuz.Common.DNTRequest.GetQueryInt("page", 1);
            int pagetop = (pageid > 1) ? (pageid-1)*16 : 0;
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().OrderBy("olid", QueryOrder.NUMASC).LimitTo(16, pagetop));

            foreach (string key in qrecords.Keys)
            {
                var column = TokyoTyrantService.GetColumns(pool, new string[] { key })[key];
                    onlineUserList.Rows.Add(
                        column["olid"],
                        column["userid"],
                        column["ip"],
                        column["username"],
                        column["nickname"],
                        column["password"],
                        column["groupid"],
                        column["olimg"],
                        column["adminid"],
                        column["invisible"],
                        column["action"],
                        TypeConverter.StrToInt(column["lastactivity"]),
                        TypeConverter.StrToDateTime(column["lastposttime"], Convert.ToDateTime("1900-1-1 00:00:00")),
                        TypeConverter.StrToDateTime(column["lastpostpmtime"], Convert.ToDateTime("1900-1-1 00:00:00")),
                        TypeConverter.StrToDateTime(column["lastsearchtime"], Convert.ToDateTime("1900-1-1 00:00:00")),
                        UnixDateTimeHelper.ConvertFromUnixTimestamp(TypeConverter.StrToInt(column["lastupdatetime"])),
                        column["forumid"],
                        column["forumname"],
                        column["titleid"],
                        column["title"],
                        column["verifycode"],
                        TypeConverter.StrToInt(column["newpms"], 0),
                        TypeConverter.StrToInt(column["newnotices"], 0));
            }
            return onlineUserList;
        }

        public OnlineUserInfo GetOnlineUser(int olid)
        {
            OnlineUserInfo onlineuserinfo = null;

            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("olid", olid));
            foreach (string key in qrecords.Keys)
            {
                var column = TokyoTyrantService.GetColumns(pool, new string[] { key })[key];
                onlineuserinfo = LoadSingleOnlineUser(column);
                break;
            }
            return onlineuserinfo;
        }

       

        /// <summary>
        /// 获得指定用户的详细信息
        /// </summary>
        /// <param name="userid">在线用户ID</param>
        /// <param name="password">用户密码</param>
        /// <returns>用户的详细信息</returns>
        public OnlineUserInfo GetOnlineUser(int userid, string password)
        {
            OnlineUserInfo onlineuserinfo = null;
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("userid", userid).StringEquals("password", password).LimitTo(1, 0));
            foreach (string key in qrecords.Keys)
            {
                onlineuserinfo = LoadSingleOnlineUser(TokyoTyrantService.GetColumns(pool, new string[] { key })[key]);
                break;
            }
            return onlineuserinfo;
        }


        /// <summary>
        /// 获得指定用户的详细信息
        /// </summary>
        /// <returns>用户的详细信息</returns>
        public OnlineUserInfo GetOnlineUserByIP(int userid, string ip)
        {
            OnlineUserInfo oluser = null;
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("userid", userid).StringEquals("ip", ip).LimitTo(1, 0));
            foreach (string key in qrecords.Keys)
            {
                oluser = LoadSingleOnlineUser(qrecords[key]);
                break;
            }
            return oluser;
        }

        #region Helper

        private OnlineUserInfo LoadSingleOnlineUser(IDictionary<string, string> column)
        {
            OnlineUserInfo onlineuserinfo = new OnlineUserInfo();
            onlineuserinfo.Olid = TypeConverter.ObjectToInt(column["olid"]);
            onlineuserinfo.Userid = TypeConverter.ObjectToInt(column["userid"]);
            onlineuserinfo.Ip = column["ip"];
            onlineuserinfo.Username = column["username"].Trim();
            onlineuserinfo.Nickname = column["nickname"].Trim();
            onlineuserinfo.Password = column["password"];
            onlineuserinfo.Groupid = Int16.Parse(column["groupid"]);
            onlineuserinfo.Olimg = column["olimg"].ToString();
            onlineuserinfo.Adminid = Int16.Parse(column["adminid"]);
            onlineuserinfo.Invisible = Int16.Parse(column["invisible"]);
            onlineuserinfo.Action = Int16.Parse(column["action"]);
            onlineuserinfo.Actionname = "";
            onlineuserinfo.Lastactivity = Int16.Parse(column["lastactivity"]);
            onlineuserinfo.Lastposttime = column["lastposttime"];
            onlineuserinfo.Lastpostpmtime = column["lastpostpmtime"];
            onlineuserinfo.Lastsearchtime = column["lastsearchtime"];
            onlineuserinfo.Lastupdatetime = UnixDateTimeHelper.ConvertFromUnixTimestamp(TypeConverter.StrToInt(column["lastupdatetime"])).ToString();
            onlineuserinfo.Forumid = TypeConverter.ObjectToInt(column["forumid"]);
            onlineuserinfo.Forumname = !string.IsNullOrEmpty(column["forumname"]) ?  column["forumname"]: "";
            onlineuserinfo.Titleid = TypeConverter.ObjectToInt(column["titleid"]);
            onlineuserinfo.Title = !string.IsNullOrEmpty(column["title"]) ? column["title"] : "";
            onlineuserinfo.Verifycode = column["verifycode"];
            onlineuserinfo.Newpms = (short)TypeConverter.StrToInt(column["newpms"], 0);
            onlineuserinfo.Newnotices = (short)TypeConverter.StrToInt(column["newnotices"], 0);

            return onlineuserinfo;
        }    
        #endregion

        /// <summary>
        /// 检查在线用户验证码是否有效
        /// </summary>
        /// <param name="olid">在组用户ID</param>
        /// <param name="verifycode">验证码</param>
        /// <param name="newverifycode">新验证码</param>
        /// <returns>在组用户ID</returns>
        public bool CheckUserVerifyCode(int olid, string verifycode, string newverifycode)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("olid", olid).StringEquals("verifycode", verifycode).LimitTo(1, 0));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                columns["verifycode"] = newverifycode;
                TokyoTyrantService.PutColumns(pool, columns["olid"], columns, true);
                break;
            }
            return true;
        }

        /// <summary>
        /// 执行在线用户向表及缓存中添加的操作。
        /// </summary>
        /// <param name="onlineuserinfo">在组用户信息内容</param>
        /// <returns>添加成功则返回刚刚添加的olid,失败则返回0</returns>
        public int CreateOnlineUserInfo(OnlineUserInfo onlineuserinfo, int timeout)
        {
            // 如果timeout为负数则代表不需要精确更新用户是否在线的状态
            if (timeout > 0)
            {
                if (onlineuserinfo.Userid > 0)
                    DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [onlinestate]=1 WHERE [uid]={1}", BaseConfigs.GetTablePrefix, onlineuserinfo.Userid));
            }
            else timeout = timeout * -1;

            if (timeout > 9999) timeout = 9999;

            //按照系统设置频率(默认5分钟)清除过期用户
            if (_lastRemoveTimeout == 0 || (System.Environment.TickCount - _lastRemoveTimeout) > 60000 * GeneralConfigs.GetConfig().Deletingexpireduserfrequency)
            {
                DeleteExpiredOnlineUsers(timeout);
                _lastRemoveTimeout = System.Environment.TickCount;
            }

            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().OrderBy("olid", QueryOrder.NUMDESC).LimitTo(1, 0));
            foreach (var k in qrecords.Keys)
            {
                var column = qrecords[k];
                onlineuserinfo.Olid = TypeConverter.StrToInt(column["olid"]) + 1;
            }

            // 如果id值太大则重建在线表
            if (onlineuserinfo.Olid > 2147483000)
                CreateOnlineTable();

            if (onlineuserinfo.Olid <= 0)
                onlineuserinfo.Olid = 1;

            IDictionary<string, string> columns = new System.Collections.Generic.Dictionary<string, string>();
            columns.Add("olid", onlineuserinfo.Olid.ToString());
            columns.Add("userid", onlineuserinfo.Userid.ToString());
            columns.Add("ip", onlineuserinfo.Ip);
            columns.Add("username", onlineuserinfo.Username);
            columns.Add("nickname", onlineuserinfo.Nickname);
            columns.Add("password", onlineuserinfo.Password);
            columns.Add("groupid", onlineuserinfo.Groupid.ToString());
            columns.Add("olimg", onlineuserinfo.Olimg);
            columns.Add("adminid", onlineuserinfo.Adminid.ToString());
            columns.Add("invisible", onlineuserinfo.Invisible.ToString());
            columns.Add("action", onlineuserinfo.Action.ToString());
            columns.Add("lastactivity", onlineuserinfo.Lastactivity.ToString());
            columns.Add("lastposttime", onlineuserinfo.Lastposttime);
            columns.Add("lastpostpmtime", onlineuserinfo.Lastpostpmtime);
            columns.Add("lastsearchtime", onlineuserinfo.Lastsearchtime);
            columns.Add("lastupdatetime", UnixDateTimeHelper.ConvertToUnixTimestamp(DateTime.Now).ToString());
            columns.Add("forumid", onlineuserinfo.Forumid.ToString());
            columns.Add("forumname", string.IsNullOrEmpty(onlineuserinfo.Forumname) ? "" : onlineuserinfo.Forumname);
            columns.Add("titleid", onlineuserinfo.Titleid.ToString());
            columns.Add("title", string.IsNullOrEmpty(onlineuserinfo.Title) ? "" : onlineuserinfo.Title);
            columns.Add("verifycode", onlineuserinfo.Verifycode);
            columns.Add("newpms", onlineuserinfo.Newpms.ToString());
            columns.Add("newnotices", onlineuserinfo.Newnotices.ToString());
            TokyoTyrantService.PutColumns(pool, onlineuserinfo.Olid.ToString(), columns, true);

            return onlineuserinfo.Olid;
        }

        private void DeleteExpiredOnlineUsers(int timeOut)
        {
            System.Text.StringBuilder timeoutStrBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder memberStrBuilder = new System.Text.StringBuilder();
            int dateTime = UnixDateTimeHelper.ConvertToUnixTimestamp(DateTime.Now.AddMinutes(timeOut * -1));

            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberLessThan("lastupdatetime", dateTime));
            foreach (var k in qrecords.Keys)
            {
                var column = qrecords[k];
                timeoutStrBuilder.Append(",");
                timeoutStrBuilder.Append(column["olid"]);
                if (column["userid"] != "-1")
                {
                    memberStrBuilder.Append(",");
                    memberStrBuilder.Append(column["userid"]);
                }
            }

            if (timeoutStrBuilder.Length > 0)
            {
                timeoutStrBuilder.Remove(0, 1);
                TokyoTyrantService.QueryDelete(pool, new Query().NumberLessThan("lastupdatetime", dateTime));
            }
            if (memberStrBuilder.Length > 0)
            {
                memberStrBuilder.Remove(0, 1);
                DbHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                         string.Format("{0}updateuseronlinestates", BaseConfigs.GetTablePrefix),
                                         DbHelper.MakeInParam("@uidlist", (DbType)SqlDbType.VarChar, 5000, memberStrBuilder.Length <= 5000 ? memberStrBuilder.ToString() : memberStrBuilder.ToString().Substring(0, 5000).TrimEnd(',')));
            }
        }

        private static int _lastRemoveTimeout;


        /// <summary>
        /// 更新用户的当前动作及相关信息
        /// </summary>
        /// <param name="olid">在线列表id</param>
        /// <param name="action">动作</param>
        /// <param name="inid">所在位置代码</param>
        public void UpdateAction(int olid, int action, int inid)
        {    
            UpdateAction(olid, action, inid, "", inid, "");         
        }

        /// <summary>
        /// 更新用户动作
        /// </summary>
        /// <param name="olid">在线用户id</param>
        /// <param name="action">用户操作</param>
        /// <param name="fid">版块id</param>
        /// <param name="forumname">版块名称</param>
        /// <param name="tid">主题id</param>
        /// <param name="topictitle">主题标题</param>
        public void UpdateAction(int olid, int action, int fid, string forumname, int tid, string topictitle)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("olid", olid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = TokyoTyrantService.GetColumns(pool, new string[] { key })[key];

                columns["action"] = action.ToString();
                columns["lastactivity"] = action.ToString();
                if (action == 5 || action == 6)
                    columns["lastposttime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                columns["lastupdatetime"] = UnixDateTimeHelper.ConvertToUnixTimestamp(DateTime.Now).ToString();
                columns["forumid"] = fid.ToString();
                columns["forumname"] = forumname;
                columns["titleid"] = tid.ToString();
                columns["title"] = topictitle;

                TokyoTyrantService.PutColumns(pool, columns["olid"], columns, true);
            }
        }

     
        /// <summary>
        /// 更新用户最后活动时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public void UpdateLastTime(int olid)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("olid", olid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                columns["lastupdatetime"] = UnixDateTimeHelper.ConvertToUnixTimestamp(DateTime.Now).ToString();
                TokyoTyrantService.PutColumns(pool, columns["olid"], columns, true);
            }
        }

        /// <summary>
        /// 更新用户最后发帖时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public void UpdatePostTime(int olid)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("olid", olid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                columns["lastposttime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                TokyoTyrantService.PutColumns(pool, columns["olid"], columns, true);
            }
        }

        /// <summary>
        /// 更新用户最后发短消息时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public void UpdatePostPMTime(int olid)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("olid", olid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                columns["lastpostpmtime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                TokyoTyrantService.PutColumns(pool, columns["olid"], columns, true);
            }
        }

        /// <summary>
        /// 更新在线表中指定用户是否隐身
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="invisible">是否隐身</param>
        public void UpdateInvisible(int olid, int invisible)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("olid", olid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                columns["invisible"] = invisible.ToString();
                TokyoTyrantService.PutColumns(pool, columns["olid"], columns, true);
            }
        }

        /// <summary>
        /// 更新在线表中指定用户的用户密码
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="password">用户密码</param>
        public void UpdatePassword(int olid, string password)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("olid", olid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                columns["password"] = password;
                TokyoTyrantService.PutColumns(pool, columns["olid"], columns, true);
            }
        }


        /// <summary>
        /// 更新用户IP地址
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="ip">ip地址</param>
        public void UpdateIP(int olid, string ip)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("olid", olid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                columns["ip"] = ip;
                TokyoTyrantService.PutColumns(pool, columns["olid"], columns, true);
            }
        }

        /// <summary>
        /// 更新用户的用户组
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="groupid">组名</param>
        public void UpdateGroupid(int userid, int groupid)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("userid", userid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                columns["groupid"] = groupid.ToString();
                TokyoTyrantService.PutColumns(pool, columns["olid"], columns, true);
            }
        }

        #region 删除符合条件的用户信息

        /// <summary>
        /// 删除符合条件的一个或多个用户信息
        /// </summary>
        /// <returns>删除行数</returns>
        public int DeleteRowsByIP(string ip)
        {
            TokyoTyrantService.QueryDelete(pool, new Query().StringEquals("ip", ip));
            return 0;
        }

        /// <summary>
        /// 删除在线表中指定在线id的行
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <returns></returns>
        public int DeleteRows(int olid)
        {
            TokyoTyrantService.QueryDelete(pool, new Query().NumberEquals("olid", olid));
            return 0;
        }


        #endregion

        /// <summary>
        /// 返回在线用户列表
        /// </summary>
        /// <param name="forumid">版块id</param>
        /// <returns></returns>
        public Discuz.Common.Generic.List<OnlineUserInfo> GetForumOnlineUserCollection(int forumid)
        {
            Discuz.Common.Generic.List<OnlineUserInfo> coll = new Discuz.Common.Generic.List<OnlineUserInfo>();

            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("forumid", forumid));
            foreach (string key in qrecords.Keys)
            {
                OnlineUserInfo onlineUserInfo = LoadSingleOnlineUser(qrecords[key]);
                coll.Add(onlineUserInfo);
            }
            //返回当前版块的在线用户表
            return coll;
        }

        /// <summary>
        /// 返回在线用户列表
        /// </summary>
        public Discuz.Common.Generic.List<OnlineUserInfo> GetOnlineUserCollection()
        {
            Discuz.Common.Generic.List<OnlineUserInfo> onlineUserInfoColl = new Discuz.Common.Generic.List<OnlineUserInfo>();
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberGreaterThan("olid", 0));
            foreach (string key in qrecords.Keys)
            {
                OnlineUserInfo onlineUserInfo = LoadSingleOnlineUser(qrecords[key]);
                if (onlineUserInfo.Userid > 0 || (onlineUserInfo.Userid == -1 && GeneralConfigs.GetConfig().Whosonlinecontract == 0))
                {
                    onlineUserInfoColl.Add(onlineUserInfo);
                }
            }
            //返回当前版块的在线用户表          
            return onlineUserInfoColl;
        }

        /// <summary>
        /// 根据Uid获得Olid
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int GetOlidByUid(int uid)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("userid", uid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                return TypeConverter.StrToInt(columns["olid"]);
            }
            return -1;
        }

        /// <summary>
        /// 更新用户新短消息数
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="count">增加量</param>
        /// <returns></returns>
        public int UpdateNewPms(int olid, int count)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("olid", olid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                columns["newpms"] = count.ToString();
                TokyoTyrantService.PutColumns(pool, olid.ToString(), columns, true);
                return 1;
            }
            return -1;
        }

        /// <summary>
        /// 更新用户新通知数
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="pluscount">增加量</param>
        /// <returns></returns>
        public int UpdateNewNotices(int olid, int pluscount)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("olid", olid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                columns["newnotices"] = (TypeConverter.StrToInt(columns["newnotices"]) + pluscount).ToString();
                TokyoTyrantService.PutColumns(pool, olid.ToString(), columns, true);
            }
            return -1;
        }

        /// <summary>
        /// 更新在线表中好友关系请求计数
        /// </summary>
        /// <param name="olId">在线id</param>
        /// <param name="count">增加量</param>
        /// <returns></returns>
        //public int UpdateNewFriendsRequest(int olId, int count)
        //{
        //    var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("olid", olId));
        //    foreach (string key in qrecords.Keys)
        //    {
        //        IDictionary<string, string> columns = qrecords[key];
        //        int newfriendrequest = TypeConverter.StrToInt(columns["newfriendrequest"]) + count;           
        //        columns["newfriendrequest"] = (newfriendrequest < 0 ? 0 : newfriendrequest).ToString();
        //        TokyoTyrantService.PutColumns(pool, olId.ToString(), columns, true);
        //        return 1;
        //    }
        //    return -1;
        //}

        /// <summary>
        /// 更新在线表中应用请求计数
        /// </summary>
        /// <param name="olId">在线id</param>
        /// <param name="count">更新数</param>
        /// <returns></returns>
        //public int UpdateNewApplicationRequest(int olId, int count)
        //{
        //    var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("olid", olId));
        //    foreach (string key in qrecords.Keys)
        //    {
        //        IDictionary<string, string> columns = qrecords[key];
        //        columns["newapprequest"] = columns["newapprequest"];
        //        TokyoTyrantService.PutColumns(pool, olId.ToString(), columns, true);
        //        return 1;
        //    }
        //    return -1;
        //}
    }
}
