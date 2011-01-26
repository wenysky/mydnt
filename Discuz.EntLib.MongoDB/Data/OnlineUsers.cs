using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Config;
using Discuz.Common.Generic;
using Discuz.Common;
using Discuz.Data;
using MongoDB;

namespace Discuz.EntLib.MongoDB.Data
{
    public class OnlineUsers : Discuz.Cache.Data.ICacheOnlineUser
    {
        private static string connectString = null;

        private static DBCache mongoCache = EntLibConfigs.GetConfig().Cacheonlineuser;

        private static DataTable onlineUserList = null;

        private static object lockHelper = new object();
  

        static OnlineUsers()
        {
            onlineUserList = DatabaseProvider.GetInstance().GetOnlineUserListTable();
            onlineUserList.Rows.Clear();

            InitialMongoDB();
        }

        private static Mongo mongoDB
        {
            get
            {
                return new Mongo(connectString);
            }
        }

        static void InitialMongoDB()
        {
            //connectString = string.Format("Username={0};Password={1};Servers={2}:{3};ConnectTimeout={4};ConnectionLifetime={5};MinimumPoolSize={6};MaximumPoolSize={7};Pooled=true",
            //     mongoCache.Username,
            //     mongoCache.Password,
            //     mongoCache.Host,
            //     mongoCache.Port,
            //     mongoCache.TcpClientConnectTimeout,
            //     mongoCache.TcpClientTimeout,
            //     mongoCache.MinConnections,
            //     mongoCache.MaxConnections);
            connectString = string.Format("Username={0};Password={1};Servers={2}:{3};MinimumPoolSize={4};MaximumPoolSize={5};Pooled=true",
                 mongoCache.Username,
                 mongoCache.Password,
                 mongoCache.Host,
                 mongoCache.Port,
                 mongoCache.MinConnections,
                 mongoCache.MaxConnections);
        }


        /// <summary>
        /// 把字符串转换成docment
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static Document StringToDoc(string info)
        {
            Document doc = new Document();
            if (!string.IsNullOrEmpty(info))
            {
                string[] items = info.TrimEnd('|').Split('|');
                foreach (string small in items)
                {
                    string[] smallitem = small.Split(':');
                    if (smallitem[0] != null && smallitem[1] != null)
                    {
                        doc.Add(smallitem[0], int.Parse(smallitem[1]));
                    }
                }
            }
            return doc;
        }
             

        /// <summary>
        /// 获得在线用户总数量
        /// </summary>
        /// <returns>用户数量</returns>
        public int GetOnlineAllUserCount()
        {
            int count = 0;
            try
            {
                count = (int)MongoDbHelper.Count(mongoDB, "online", null);
                return count == 0 ? 1 : count;
            }
            catch
            {
                return 1;
            }
        }


        /// <summary>
        /// 创建在线表记录(本方法在应用程序初始化时被调用)
        /// </summary>
        /// <returns></returns>
        public int CreateOnlineTable()
        {
            MongoDbHelper.CleanTable(mongoDB, "online");

            mongoDB.Connect(); //Connect to localhost on the default port.
            IMongoCollection things = mongoDB.GetDatabase("dnt_mongodb").GetCollection("online");
            things.MetaData.CreateIndex("olid", StringToDoc("olid:1|"), true);
            things.MetaData.CreateIndex("userid", StringToDoc("userid:1|"), false);
            things.MetaData.CreateIndex("password", StringToDoc("password:1|"), false);
            things.MetaData.CreateIndex("userid_password", StringToDoc("userid:1|password:1|"), false);
            things.MetaData.CreateIndex("ip", StringToDoc("ip:1|"), false);
            things.MetaData.CreateIndex("userid_ip", StringToDoc("userid:1|ip:1|"), false);
            things.MetaData.CreateIndex("forumid", StringToDoc("forumid:1|"), false);
            things.MetaData.CreateIndex("lastupdatetime", StringToDoc("lastupdatetime:1|"), false);
            mongoDB.Disconnect();
            return 0;
        }

        /// <summary>
        /// 获得在线注册用户总数量
        /// </summary>
        /// <returns>用户数量</returns>
        public int GetOnlineUserCount()
        {
            return (int)MongoDbHelper.Count(mongoDB, "online", new Document().Add("userid", Op.GreaterThan(0)));
        }

        /// <summary>
        /// 获得在线不可见用户量
        /// </summary>
        /// <returns>用户数量</returns>
        public int GetInvisibleOnlineUserCount()
        {
            return (int)MongoDbHelper.Count(mongoDB, "online", new Document().Add("userid", Op.GreaterThan(0)).Add("invisible", 1));
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
            System.Collections.Generic.List<Document> docList = MongoDbHelper.Find(mongoDB, "online", null, "_id", IndexOrder.Ascending, 16, pagetop);

            foreach (Document doc in docList)
            {
                onlineUserList.Rows.Add(
                    doc["olid"],
                    doc["userid"],
                    doc["ip"],
                    doc["username"],
                    doc["nickname"],
                    doc["password"],
                    doc["groupid"],
                    doc["olimg"],
                    doc["adminid"],
                    doc["invisible"],
                    doc["action"],
                    TypeConverter.ObjectToInt(doc["lastactivity"]),
                    TypeConverter.ObjectToDateTime(doc["lastposttime"], Convert.ToDateTime("1900-1-1 00:00:00")),
                    TypeConverter.ObjectToDateTime(doc["lastpostpmtime"], Convert.ToDateTime("1900-1-1 00:00:00")),
                    TypeConverter.ObjectToDateTime(doc["lastsearchtime"], Convert.ToDateTime("1900-1-1 00:00:00")),
                    UnixDateTimeHelper.ConvertFromUnixTimestamp(TypeConverter.ObjectToInt(doc["lastupdatetime"])),
                    doc["forumid"],
                    doc["forumname"],
                    doc["titleid"],
                    doc["title"],
                    doc["verifycode"],
                    TypeConverter.ObjectToInt(doc["newpms"], 0),
                    TypeConverter.ObjectToInt(doc["newnotices"], 0));
                    //TypeConverter.ObjectToInt(doc["newfriendrequest"], 0),
                    //TypeConverter.ObjectToInt(doc["newapprequest"], 0));
            }
            return onlineUserList;
        }

        public OnlineUserInfo GetOnlineUser(int olid)
        {
            OnlineUserInfo onlineuserinfo = null;
            Document doc = MongoDbHelper.FindOne(mongoDB, "online", new Document().Add("olid", olid));
            if(doc!=null)
                onlineuserinfo = LoadSingleOnlineUser(doc);             
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
            Document doc = MongoDbHelper.FindOne(mongoDB, "online", new Document().Add("userid", userid).Add("password", password));
            if (doc != null)
                onlineuserinfo = LoadSingleOnlineUser(doc);
            return onlineuserinfo;
        }


        /// <summary>
        /// 获得指定用户的详细信息
        /// </summary>
        /// <returns>用户的详细信息</returns>
        public OnlineUserInfo GetOnlineUserByIP(int userid, string ip)
        {
            OnlineUserInfo onlineuserinfo = null;
            try
            {
                Document doc = MongoDbHelper.FindOne(mongoDB, "online", new Document().Add("userid", userid).Add("ip", ip));
                if (doc != null)
                    onlineuserinfo = LoadSingleOnlineUser(doc);                
            }
            catch {}
            return onlineuserinfo;
        }

        #region Helper

        private OnlineUserInfo LoadSingleOnlineUser(Document doc)
        {
            OnlineUserInfo onlineuserinfo = new OnlineUserInfo();
            onlineuserinfo.Olid = TypeConverter.ObjectToInt(doc["olid"]);
            onlineuserinfo.Userid = TypeConverter.ObjectToInt(doc["userid"]);
            onlineuserinfo.Ip = doc["ip"].ToString();
            onlineuserinfo.Username = doc["username"].ToString().Trim();
            onlineuserinfo.Nickname = doc["nickname"].ToString().Trim();
            onlineuserinfo.Password = doc["password"].ToString();
            onlineuserinfo.Groupid = (short)TypeConverter.ObjectToInt(doc["groupid"]);
            onlineuserinfo.Olimg = doc["olimg"].ToString();
            onlineuserinfo.Adminid = (short)TypeConverter.ObjectToInt(doc["adminid"]);
            onlineuserinfo.Invisible = (short)TypeConverter.ObjectToInt(doc["invisible"]);
            onlineuserinfo.Action = (short)TypeConverter.ObjectToInt(doc["action"]);
            onlineuserinfo.Actionname = "";
            onlineuserinfo.Lastactivity = (short)TypeConverter.ObjectToInt(doc["lastactivity"]);
            onlineuserinfo.Lastposttime = doc["lastposttime"].ToString();
            onlineuserinfo.Lastpostpmtime = doc["lastpostpmtime"].ToString();
            onlineuserinfo.Lastsearchtime = doc["lastsearchtime"].ToString();
            onlineuserinfo.Lastupdatetime = UnixDateTimeHelper.ConvertFromUnixTimestamp(TypeConverter.ObjectToInt(doc["lastupdatetime"])).ToString();
            onlineuserinfo.Forumid = TypeConverter.ObjectToInt(doc["forumid"]);
            onlineuserinfo.Forumname = !string.IsNullOrEmpty(doc["forumname"].ToString()) ?  doc["forumname"].ToString(): "";
            onlineuserinfo.Titleid = TypeConverter.ObjectToInt(doc["titleid"]);
            onlineuserinfo.Title = !string.IsNullOrEmpty(doc["title"].ToString()) ? doc["title"].ToString() : "";
            onlineuserinfo.Verifycode = doc["verifycode"].ToString();
            onlineuserinfo.Newpms = (short)TypeConverter.ObjectToInt(doc["newpms"]);
            onlineuserinfo.Newnotices = (short)TypeConverter.ObjectToInt(doc["newnotices"]);
            //onlineuserinfo.Newfriendrequest = (short)TypeConverter.ObjectToInt(doc["newfriendrequest"]);
            //onlineuserinfo.Newapprequest = (short)TypeConverter.ObjectToInt(doc["newapprequest"]);
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
            MongoDbHelper.Update(mongoDB, "online",
                  new Document() { { "$set", new Document() { { "verifycode", newverifycode } } } },
                   new Document().Add("olid", olid).Add("verifycode", verifycode));        
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
            lock (lockHelper)
            {
                //System.Collections.Generic.List<Document> docList = MongoDbHelper.Find(mongoDB, "online", null, "_id", IndexOrder.Descending, 1, 0);
                //foreach (Document doc in docList)
                //{
                //    onlineuserinfo.Olid = TypeConverter.ObjectToInt(doc["olid"]) + 1;
                //}
                //注释上面代码，使用下面单创建表的方式来保存最大的在线用户ID（maxolid）
                Document doc = MongoDbHelper.FindOne(mongoDB, "onlinemaxolid", new Document().Add("_id", 1));
                if (doc == null)
                {
                    MongoDbHelper.Insert(mongoDB, "onlinemaxolid", new Document { { "_id", 1 }, {"maxolid", 1} });
                    onlineuserinfo.Olid = 1;
                }
                else
                {
                    onlineuserinfo.Olid = TypeConverter.ObjectToInt(doc["maxolid"], 0) + 1;
                    MongoDbHelper.Update(mongoDB, "onlinemaxolid", 
                        new Document() { { "$set", new Document() { { "maxolid", onlineuserinfo.Olid } } } },
                        new Document().Add("_id", 1));
                }

                // 如果id值太大则重建在线表
                if (onlineuserinfo.Olid > 2147483000)
                {
                    CreateOnlineTable();
                    MongoDbHelper.Update(mongoDB, "onlinemaxolid",
                       new Document() { { "$set", new Document() { { "maxolid", 1 } } } },
                       new Document().Add("_id", 1));
                }

                if (onlineuserinfo.Olid <= 0)
                    onlineuserinfo.Olid = 1;

                Document userDoc = new Document();
                userDoc["_id"] = onlineuserinfo.Olid;
                userDoc["olid"] = onlineuserinfo.Olid;
                userDoc["userid"] = onlineuserinfo.Userid;
                userDoc["ip"] = onlineuserinfo.Ip;
                userDoc["username"] = onlineuserinfo.Username;
                userDoc["nickname"] = onlineuserinfo.Nickname;
                userDoc["password"] = onlineuserinfo.Password;
                userDoc["groupid"] = int.Parse(onlineuserinfo.Groupid.ToString());
                userDoc["olimg"] = onlineuserinfo.Olimg;
                userDoc["adminid"] = int.Parse(onlineuserinfo.Adminid.ToString());
                userDoc["invisible"] = int.Parse(onlineuserinfo.Invisible.ToString());
                userDoc["action"] = int.Parse(onlineuserinfo.Action.ToString());
                userDoc["lastactivity"] = int.Parse(onlineuserinfo.Lastactivity.ToString());
                userDoc["lastposttime"] = onlineuserinfo.Lastposttime;
                userDoc["lastpostpmtime"] = onlineuserinfo.Lastpostpmtime;
                userDoc["lastsearchtime"] = onlineuserinfo.Lastsearchtime;
                userDoc["lastupdatetime"] = UnixDateTimeHelper.ConvertToUnixTimestamp(DateTime.Now);
                userDoc["forumid"] = onlineuserinfo.Forumid;
                userDoc["forumname"] = string.IsNullOrEmpty(onlineuserinfo.Forumname) ? "" : onlineuserinfo.Forumname;
                userDoc["titleid"] = onlineuserinfo.Titleid;
                userDoc["title"] = string.IsNullOrEmpty(onlineuserinfo.Title) ? "" : onlineuserinfo.Title;
                userDoc["verifycode"] = onlineuserinfo.Verifycode;
                userDoc["newpms"] = int.Parse(onlineuserinfo.Newpms.ToString());
                userDoc["newnotices"] = int.Parse(onlineuserinfo.Newnotices.ToString());
                //userDoc["newfriendrequest"] = int.Parse(onlineuserinfo.Newfriendrequest.ToString());
                //userDoc["newapprequest"] = int.Parse(onlineuserinfo.Newapprequest.ToString());
                MongoDbHelper.Insert(mongoDB, "online", userDoc);
                return onlineuserinfo.Olid;
            }
        }

        private void DeleteExpiredOnlineUsers(int timeOut)
        {
            System.Text.StringBuilder timeoutStrBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder memberStrBuilder = new System.Text.StringBuilder();
            int dateTime = UnixDateTimeHelper.ConvertToUnixTimestamp(DateTime.Now.AddMinutes(timeOut * -1));

            System.Collections.Generic.List<Document> docList = MongoDbHelper.Find(mongoDB, "online", new Document().Add("lastupdatetime", Op.LessThan(dateTime)));
            foreach (Document doc in docList)
            {
                timeoutStrBuilder.Append(",");
                timeoutStrBuilder.Append(doc["olid"]);
                if (doc["userid"].ToString() != "-1")
                {
                    memberStrBuilder.Append(",");
                    memberStrBuilder.Append(doc["userid"]);
                }
            }

            if (timeoutStrBuilder.Length > 0)
            {
                timeoutStrBuilder.Remove(0, 1);
                MongoDbHelper.Delete(mongoDB, "online", new Document().Add("lastupdatetime", Op.LessThan(dateTime)));
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
            System.Collections.Generic.List<Document> docList = MongoDbHelper.Find(mongoDB, "online", new Document().Add("olid", olid));
            foreach (Document doc in docList)
            {
                doc["action"] = action;
                doc["lastactivity"] = action;
                if (action == 5 || action == 6)
                    doc["lastposttime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                doc["lastupdatetime"] = UnixDateTimeHelper.ConvertToUnixTimestamp(DateTime.Now);
                doc["forumid"] = fid;
                doc["forumname"] = forumname;
                doc["titleid"] = tid;
                doc["title"] = topictitle;

                MongoDbHelper.Update(mongoDB, "online", doc);
            }
        }

     
        /// <summary>
        /// 更新用户最后活动时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public void UpdateLastTime(int olid)
        {
            MongoDbHelper.Update(mongoDB, "online",
                  new Document() { { "$set", new Document() { { "lastupdatetime", UnixDateTimeHelper.ConvertToUnixTimestamp(DateTime.Now) } } } },
                   new Document().Add("olid", olid));
        }

        /// <summary>
        /// 更新用户最后发帖时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public void UpdatePostTime(int olid)
        {
            MongoDbHelper.Update(mongoDB, "online",
                 new Document() { { "$set", new Document() { { "lastposttime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") } } } },
                  new Document().Add("olid", olid));
        }

        /// <summary>
        /// 更新用户最后发短消息时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public void UpdatePostPMTime(int olid)
        {
            MongoDbHelper.Update(mongoDB, "online",
               new Document() { { "$set", new Document() { { "lastpostpmtime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") } } } },
                new Document().Add("olid", olid));
        }

        /// <summary>
        /// 更新在线表中指定用户是否隐身
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="invisible">是否隐身</param>
        public void UpdateInvisible(int olid, int invisible)
        {
            MongoDbHelper.Update(mongoDB, "online",
              new Document() { { "$set", new Document() { { "invisible", invisible } } } },
               new Document().Add("olid", olid));
        }

        /// <summary>
        /// 更新在线表中指定用户的用户密码
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="password">用户密码</param>
        public void UpdatePassword(int olid, string password)
        {
            MongoDbHelper.Update(mongoDB, "online",
               new Document() { { "$set", new Document() { { "password", password } } } },
               new Document().Add("olid", olid));
        }


        /// <summary>
        /// 更新用户IP地址
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="ip">ip地址</param>
        public void UpdateIP(int olid, string ip)
        {
            MongoDbHelper.Update(mongoDB, "online",
               new Document() { { "$set", new Document() { { "ip", ip } } } },
               new Document().Add("olid", olid));
        }

        /// <summary>
        /// 更新用户的用户组
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="groupid">组名</param>
        public void UpdateGroupid(int userid, int groupid)
        {
            MongoDbHelper.Update(mongoDB, "online",
              new Document() { { "$set", new Document() { { "groupid", groupid } } } },
              new Document().Add("userid", userid));
        }

        #region 删除符合条件的用户信息

        /// <summary>
        /// 删除符合条件的一个或多个用户信息
        /// </summary>
        /// <returns>删除行数</returns>
        public int DeleteRowsByIP(string ip)
        {
            MongoDbHelper.Delete(mongoDB, "online", new Document().Add("ip", ip));
            return 0;
        }

        /// <summary>
        /// 删除在线表中指定在线id的行
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <returns></returns>
        public int DeleteRows(int olid)
        {
            MongoDbHelper.Delete(mongoDB, "online", new Document().Add("olid", olid));
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

            System.Collections.Generic.List<Document> docList = MongoDbHelper.Find(mongoDB, "online", new Document().Add("forumid", forumid));
            foreach (Document doc in docList)
            {
                OnlineUserInfo onlineUserInfo = LoadSingleOnlineUser(doc);
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
            System.Collections.Generic.List<Document> docList = MongoDbHelper.Find(mongoDB, "online", new Document().Add("_id", new Document().Add("$gt", 0)));
            foreach (Document doc in docList)
            {
                OnlineUserInfo onlineUserInfo = LoadSingleOnlineUser(doc);
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
            Document doc = MongoDbHelper.FindOne(mongoDB, "online", new Document().Add("userid", uid));
            if(doc != null)
                return TypeConverter.ObjectToInt(doc["olid"]);
          
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
            MongoDbHelper.Update(mongoDB, "online",
              new Document() { { "$set", new Document() { { "newpms", count } } } },
              new Document().Add("olid", olid));

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
             Document doc = MongoDbHelper.FindOne(mongoDB, "online", new Document().Add("olid", olid));
             if (doc != null)
             {
                 int newnotices = TypeConverter.ObjectToInt(doc["newnotices"]) + pluscount;
                 MongoDbHelper.Update(mongoDB, "online",
                     new Document() { { "$set", new Document() { { "newnotices", newnotices } } } },
                      new Document().Add("olid", olid));
                 return 1;
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
        //    Document doc = MongoDbHelper.FindOne(mongoDB, "online", new Document().Add("olid", olId));
        //    if (doc != null)
        //    {
        //        int newfriendrequest = TypeConverter.ObjectToInt(doc["newfriendrequest"]) + count;
        //        MongoDbHelper.Update(mongoDB, "online",
        //            new Document() { { "$set", new Document() { { "newfriendrequest", newfriendrequest < 0 ? 0 : newfriendrequest } } } },
        //            new Document().Add("olid", olId));
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
        //    Document doc = MongoDbHelper.FindOne(mongoDB, "online", new Document().Add("olid", olId));
        //    if (doc != null)
        //    {
        //        MongoDbHelper.Update(mongoDB, "online",
        //             new Document() { { "$set", new Document() { { "newapprequest", count } } } },
        //             new Document().Add("olid", olId));
        //        return 1;
        //    }
        //    return -1;
        //}
    }
}
