using System;
using System.Data;
using System.Linq;
using System.Text;

using Discuz.Entity;
using Discuz.Config;
using Discuz.Common.Generic;
using Discuz.Common;
using Discuz.Data;
using MongoDB;

namespace Discuz.EntLib.MongoDB.Data
{
    /// <summary>
    /// 用户数据类，注：UpdateAllUserPostCount，ResetUserDigestPosts，UpdateUserCredits这三个方法需要有客户端支持(DntTokyoTyrantTools.exe)才能使用。
    /// </summary>
    public class Users : Discuz.Cache.Data.ICacheUsers
    {
        private static DBCache mongoCache = EntLibConfigs.GetConfig().Cacheusers;

        private static string connectString = null;

        static Users()
        {
            InitialMongoDB();
        }

        public static Mongo mongoDB
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

        #region 获取用户信息
        public UserInfo LoadSingleUserInfo(Document doc)
        {
            UserInfo userinfo = new UserInfo();
            userinfo.Uid = TypeConverter.ObjectToInt(doc["uid"]);
            userinfo.Username = doc["username"].ToString();
            userinfo.Nickname = doc["nickname"].ToString();
            userinfo.Password = doc["password"].ToString();
            userinfo.Spaceid = TypeConverter.ObjectToInt(doc["spaceid"]);
            userinfo.Secques = doc["secques"].ToString();
            userinfo.Gender = TypeConverter.ObjectToInt(doc["gender"]);
            userinfo.Adminid = TypeConverter.ObjectToInt(doc["adminid"]);
            userinfo.Groupid = TypeConverter.ObjectToInt(doc["groupid"]);
            userinfo.Groupexpiry = TypeConverter.ObjectToInt(doc["groupexpiry"]);
            userinfo.Extgroupids = doc["extgroupids"] == null ? "" : doc["extgroupids"].ToString();
            userinfo.Regip = doc["regip"] == null ? "" : doc["regip"].ToString();
            userinfo.Joindate = doc["joindate"] == null ? Utils.GetDateTime() : Utils.GetStandardDateTime(doc["joindate"].ToString());
            userinfo.Lastip = doc["lastip"] == null ? "" : doc["lastip"].ToString();
            userinfo.Lastvisit = doc["lastvisit"] == null ? Utils.GetDateTime() : Utils.GetStandardDateTime(doc["lastvisit"].ToString());
            userinfo.Lastactivity = doc["lastactivity"] == null ? Utils.GetDateTime() : Utils.GetStandardDateTime(doc["lastactivity"].ToString());
            userinfo.Lastpost = doc["lastpost"] == null ? Utils.GetDateTime() : Utils.GetStandardDateTime(doc["lastpost"].ToString());
            userinfo.Lastpostid = TypeConverter.ObjectToInt(doc["lastpostid"]);
            userinfo.Lastposttitle = doc["lastposttitle"] == null ? "" : doc["lastposttitle"].ToString();
            userinfo.Posts = TypeConverter.ObjectToInt(doc["posts"]);
            userinfo.Digestposts = TypeConverter.ObjectToInt(doc["digestposts"]);
            userinfo.Oltime = TypeConverter.ObjectToInt(doc["oltime"]);
            userinfo.Pageviews = TypeConverter.ObjectToInt(doc["pageviews"]);
            userinfo.Credits = TypeConverter.ObjectToInt(doc["credits"]);
            userinfo.Extcredits1 = TypeConverter.StrToFloat(doc["extcredits1"].ToString());
            userinfo.Extcredits2 = TypeConverter.StrToFloat(doc["extcredits2"].ToString());
            userinfo.Extcredits3 = TypeConverter.StrToFloat(doc["extcredits3"].ToString());
            userinfo.Extcredits4 = TypeConverter.StrToFloat(doc["extcredits4"].ToString());
            userinfo.Extcredits5 = TypeConverter.StrToFloat(doc["extcredits5"].ToString());
            userinfo.Extcredits6 = TypeConverter.StrToFloat(doc["extcredits6"].ToString());
            userinfo.Extcredits7 = TypeConverter.StrToFloat(doc["extcredits7"].ToString());
            userinfo.Extcredits8 = TypeConverter.StrToFloat(doc["extcredits8"].ToString());
            userinfo.Medals = doc["medals"] == null ? "" : doc["medals"].ToString();
            userinfo.Email = doc["email"] == null ? "" : doc["email"].ToString();
            userinfo.Bday = doc["bday"] == null ? "" : doc["bday"].ToString();
            userinfo.Sigstatus = TypeConverter.ObjectToInt(doc["sigstatus"]);
            userinfo.Tpp = TypeConverter.ObjectToInt(doc["tpp"]);
            userinfo.Ppp = TypeConverter.ObjectToInt(doc["ppp"]);
            userinfo.Templateid = TypeConverter.ObjectToInt(doc["templateid"]);
            userinfo.Pmsound = TypeConverter.ObjectToInt(doc["pmsound"]);
            userinfo.Showemail = TypeConverter.ObjectToInt(doc["showemail"]);
            userinfo.Newsletter = (ReceivePMSettingType)TypeConverter.ObjectToInt(doc["newsletter"]);
            userinfo.Invisible = TypeConverter.ObjectToInt(doc["invisible"]);
            userinfo.Newpm = TypeConverter.ObjectToInt(doc["newpm"]);
            userinfo.Newpmcount = TypeConverter.ObjectToInt(doc["newpmcount"]);
            userinfo.Accessmasks = TypeConverter.ObjectToInt(doc["accessmasks"]);
            userinfo.Onlinestate = TypeConverter.ObjectToInt(doc["onlinestate"]);
            userinfo.Website = doc["website"] == null ? "" : doc["website"].ToString();
            userinfo.Icq = doc["icq"] == null ? "" : doc["icq"].ToString();
            userinfo.Qq = doc["qq"] == null ? "" : doc["qq"].ToString();
            userinfo.Yahoo = doc["yahoo"] == null ? "" : doc["yahoo"].ToString();
            userinfo.Msn = doc["msn"] == null ? "" : doc["msn"].ToString();
            userinfo.Skype = doc["skype"] == null ? "" : doc["skype"].ToString();
            userinfo.Location = doc["location"] == null ? "" : doc["location"].ToString();
            userinfo.Customstatus = doc["customstatus"] == null ? "" : doc["customstatus"].ToString();
            userinfo.Bio = doc["bio"] == null ? "" : doc["bio"].ToString();
            userinfo.Signature = doc["signature"] == null ? "" : doc["signature"].ToString();
            userinfo.Sightml = doc["sightml"] == null ? "" : doc["sightml"].ToString();
            userinfo.Authstr = doc["authstr"] == null ? "" : doc["authstr"].ToString(); 
            userinfo.Authtime = doc["authtime"] == null ? Utils.GetDateTime() : Utils.GetStandardDateTime(doc["authtime"].ToString());
            userinfo.Authflag = Byte.Parse(TypeConverter.ObjectToInt(doc["authflag"]).ToString());
            userinfo.Realname = doc["realname"] == null ? "" : doc["realname"].ToString();
            userinfo.Idcard = doc["idcard"] == null ? "" : doc["idcard"].ToString();
            userinfo.Mobile = doc["mobile"] == null ? "" : doc["mobile"].ToString();
            userinfo.Phone = doc["mobile"] == null ? "" : doc["mobile"].ToString();
            userinfo.Ignorepm = doc["ignorepm"] == null ? "" : doc["ignorepm"].ToString();
            userinfo.Salt = doc["ignorepm"] == null ? "" : doc["ignorepm"].ToString().Trim();
            return userinfo;
        }


        public Document LoadSingleUser(UserInfo userinfo)
        {
            //该判断主要用于获取avatar等相关信息，以免该信息在更新中被覆盖（移除）
            Document doc = MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("_id", userinfo.Uid));
            //如为null则表示该方法只为创建当前用户信息
            if (doc == null)
            {
                doc = new Document();
                doc["avatar"] = "avatars\\common\\1.jpg";
                doc["avatarwidth"] = 60;
                doc["avatarheight"] = 60;
            }
            doc["_id"] = userinfo.Uid;
            doc["uid"] = userinfo.Uid;
            doc["username"] = userinfo.Username;
            doc["nickname"] = userinfo.Nickname;
            doc["password"] = userinfo.Password;
            doc["spaceid"] = userinfo.Spaceid;
            doc["secques"] = userinfo.Secques;
            doc["gender"] = userinfo.Gender;
            doc["adminid"] = userinfo.Adminid;
            doc["groupid"] = userinfo.Groupid;
            doc["groupexpiry"] = userinfo.Groupexpiry;
            doc["extgroupids"] = userinfo.Extgroupids;
            doc["regip"] = userinfo.Regip;
            doc["joindate"] = userinfo.Joindate;
            doc["lastip"] = userinfo.Lastip;
            doc["lastvisit"] = userinfo.Lastvisit;
            doc["lastactivity"] = userinfo.Lastactivity;
            doc["lastpost"] = userinfo.Lastpost;
            doc["lastpostid"] = userinfo.Lastpostid;
            doc["lastposttitle"] = userinfo.Lastposttitle;
            doc["posts"] = userinfo.Posts;
            doc["digestposts"] = userinfo.Digestposts;
            doc["oltime"] = userinfo.Oltime;
            doc["pageviews"] = userinfo.Pageviews;
            doc["credits"] = userinfo.Credits;
            doc["extcredits1"] = userinfo.Extcredits1.ToString();
            doc["extcredits2"] = userinfo.Extcredits2.ToString();
            doc["extcredits3"] = userinfo.Extcredits3.ToString();
            doc["extcredits4"] = userinfo.Extcredits4.ToString();
            doc["extcredits5"] = userinfo.Extcredits5.ToString();
            doc["extcredits6"] = userinfo.Extcredits6.ToString();
            doc["extcredits7"] = userinfo.Extcredits7.ToString();
            doc["extcredits8"] = userinfo.Extcredits8.ToString();
            doc["medals"] = userinfo.Medals;
            doc["email"] = userinfo.Email;
            doc["bday"] = userinfo.Bday;
            doc["sigstatus"] = userinfo.Sigstatus;
            doc["tpp"] = userinfo.Tpp;
            doc["ppp"] = userinfo.Ppp;
            doc["templateid"] = userinfo.Templateid;
            doc["pmsound"] = userinfo.Pmsound;
            doc["showemail"] = userinfo.Showemail;
            doc["newsletter"] = userinfo.Newsletter;
            doc["invisible"] = userinfo.Invisible;
            doc["newpm"] = userinfo.Newpm;
            doc["newpmcount"] = userinfo.Newpmcount;
            doc["accessmasks"] = userinfo.Accessmasks;
            doc["onlinestate"] = userinfo.Onlinestate;
            doc["website"] = userinfo.Website;
            doc["icq"] = userinfo.Icq;
            doc["qq"] = userinfo.Qq;
            doc["yahoo"] = userinfo.Yahoo;
            doc["msn"] = userinfo.Msn;
            doc["skype"] = userinfo.Skype;
            doc["location"] = userinfo.Location;
            doc["customstatus"] = userinfo.Customstatus;
            doc["bio"] = userinfo.Bio;
            doc["signature"] = userinfo.Signature;
            doc["sightml"] = userinfo.Sightml;
            doc["authstr"] = userinfo.Authstr;
            doc["authtime"] = userinfo.Authtime;
            doc["authflag"] = userinfo.Authflag.ToString();
            doc["realname"] = userinfo.Realname;
            doc["idcard"] = userinfo.Idcard;
            doc["mobile"] = userinfo.Mobile;
            doc["phone"] = userinfo.Phone;
            doc["ignorepm"] = string.IsNullOrEmpty(userinfo.Ignorepm) ? "" : userinfo.Ignorepm;
            doc["salt"] = userinfo.Salt;
            return doc;
        }    
        #endregion
       
#if HitCheck
        private static int getHits = 0;
        private static int getMisses = 0;
        private static int getNumber = 0;
#endif
   

        /// <summary>
        /// 返回指定用户的完整信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>用户信息</returns>
        public UserInfo GetUserInfo(int uid)
        {            
#if HitCheck  
            getNumber++;
#endif
            UserInfo userinfo = null;
            Document doc = MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("_id", uid));
            if(doc !=null)
                userinfo = LoadSingleUserInfo(doc);
#if HitCheck   
            if(userinfo == null) getMisses++;
            else getHits++;
            System.Web.HttpContext.Current.Response.Write("查询总数:" + getNumber + " 命中次数:" + getHits + " 失败次数:" + getMisses + " 命中率:" + ((getHits * 100) / getNumber) + "%");
#endif
            return userinfo;
        }

      
        /// <summary>
        /// 购买主题
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="tid">主题ID</param>
        /// <param name="posterid">发帖人ID</param>
        /// <param name="price">售价</param>
        /// <param name="netamount">净收入</param>
        /// <param name="creditsTrans">要更新的扩展积分ID</param>
        public void BuyTopic(int uid, int tid, int posterid, int price, float netamount, int creditsTrans)
        {
            Document doc = MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("_id", uid));
            if (doc != null)
            {
                doc["extcredits" + creditsTrans] = (TypeConverter.ObjectToFloat(doc["extcredits" + creditsTrans]) - price).ToString();
                MongoDbHelper.Update(mongoDB, "users", doc);
            }
            doc = MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("_id", posterid));
            if (doc != null)
            {
                doc["extcredits" + creditsTrans] = (TypeConverter.ObjectToFloat(doc["extcredits" + creditsTrans]) + netamount).ToString();
                MongoDbHelper.Update(mongoDB, "users", doc);             
            }
        }

        /// <summary>
        /// 更新用户的用户组信息
        /// </summary>
        /// <param name="uidList">用户ID列表</param>
        /// <param name="groupId">用户组ID</param>
        public void UpdateUserGroup(string uidList, int groupId)
        {
            MongoDbHelper.Update(mongoDB, "users",
               new Document() { { "$set", new Document() { { "groupid", groupId } } } },
               new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(uidList))));
        }


        /// <summary>
        /// 根据IP查找用户
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>用户信息</returns>
        public UserInfo GetUserInfoByIP(string ip)
        {
            Document doc = MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("regip", ip));
            if (doc != null)
            {
                return LoadSingleUserInfo(doc);
            }
            return null;
        }

        /// <summary>
        /// 根据用户名返回用户id
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>用户id</returns>
        public UserInfo GetUserInfoByName(string username)
        {
            Document doc = MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("username", username));
            if (doc != null)
            {
                return LoadSingleUserInfo(doc);
            }
            return null;
        }

        /// <summary>
        /// 检测Email和安全项
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="email">email</param>
        /// <param name="userSecques">用户安全问题答案的存储数据</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public int CheckEmailAndSecques(string username, string email, string userSecques)
        {
            Document doc = MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("username", username).Add("email", email).Add("secques", userSecques));
            if (doc != null)
            {
                return TypeConverter.ObjectToInt(doc["uid"], -1);
            }
            return 0;
        }

        /// <summary>
        /// 检测密码和安全项
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <param name="userSecques">用户安全问题答案的存储数据</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public int CheckPasswordAndSecques(string username, string password, bool originalpassword, string userSecques)
        {
            Document doc = MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("username", username).Add("password", originalpassword ? Utils.MD5(password) : password).Add("secques", userSecques));
            if (doc != null)
            {
                return TypeConverter.ObjectToInt(doc["uid"], -1);
            }
            return 0;
        }


        /// <summary>
        /// 判断用户密码是否正确
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否为未MD5密码</param>
        /// <returns>如果正确则返回uid</returns>
        public UserInfo CheckPassword(string username, string password, bool originalpassword)
        {
            Document doc = MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("username", username).Add("password", originalpassword ? Utils.MD5(password) : password));
            if (doc != null)
            {
                return LoadSingleUserInfo(doc);
            }
            return null;
        }

        /// <summary>
        /// 检测密码
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <returns>如果用户密码正确则返回uid, 否则返回-1</returns>
        public UserInfo CheckPassword(int uid, string password, bool originalpassword)
        {
            Document doc = MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("_id", uid).Add("password", originalpassword ? Utils.MD5(password) : password));
            if (doc != null)
            {
                return LoadSingleUserInfo(doc);
            }
            return null;
        }

        /// <summary>
        /// 根据指定的email查找用户并返回用户uid
        /// </summary>
        /// <param name="email">email地址</param>
        /// <returns>用户uid</returns>
        public int FindUserEmail(string email)
        {
            Document doc = MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("email", email));
            if (doc != null)
            {
                return TypeConverter.ObjectToInt(doc["uid"], -1);
            }
            return -1;
        }

        /// <summary>
        /// 得到论坛中用户总数,该函数暂不可用
        /// </summary>
        /// <returns>用户总数</returns>
        //public int GetUserCount()
        //{
        //    return (int)TokyoTyrantService.GetRecordCount(pool);
        //}

        /// <summary>
        /// 得到论坛中用户总数,该函数暂不可用
        /// </summary>
        /// <returns>用户总数</returns>
        //public int GetUserCountByAdmin()
        //{
        //    return TokyoTyrantService.QueryRecordsCount(pool, new TokyoQuery().NumberGreaterThan("adminid", 0));
        //}

        /// <summary>
        /// 创建新用户.
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>返回用户ID, 如果已存在该用户名则返回-1</returns>
        public int CreateUser(UserInfo userinfo)
        {
            Document doc = LoadSingleUser(userinfo);
            doc["avatar"] = "avatars\\common\\0.gif";
            doc["avatarwidth"] = 60;
            doc["avatarheight"] = 60;
                
            MongoDbHelper.Insert(mongoDB, "users", doc);
            return userinfo.Uid;
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="userinfo">用户信息</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateUser(UserInfo userinfo)
        {
            Document doc = LoadSingleUser(userinfo);
            MongoDbHelper.Update(mongoDB, "users", doc);
            return DatabaseProvider.GetInstance().UpdateUser(userinfo);
        }

        /// <summary>
        /// 更新权限验证字符串
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="authstr">验证串</param>
        /// <param name="authflag">验证标志</param>
        public void UpdateAuthStr(int uid, string authstr, int authflag)
        {
            MongoDbHelper.Update(mongoDB, "users",
              new Document() { { "$set", new Document() { { "authstr", authstr }, { "authtime", Utils.GetDate() }, { "authflag", authflag } } } },
              new Document().Add("_id", uid));
        }


        /// <summary>
        /// 更新指定用户的个人资料
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>如果用户不存在则为false, 否则为true</returns>
        public void UpdateUserProfile(UserInfo userinfo)
        {
            Document doc = LoadSingleUser(userinfo);
            MongoDbHelper.Update(mongoDB, "users", doc);      
        }

        /// <summary>
        /// 更新用户论坛设置
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>如果用户不存在则返回false, 否则返回true</returns>
        public void UpdateUserForumSetting(UserInfo userinfo)
        {
            Document doc = LoadSingleUser(userinfo);
            MongoDbHelper.Update(mongoDB, "users", doc);         
        }

        /// <summary>
        /// 修改用户自定义积分字段的值
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="extid">扩展字段序号(1-8)</param>
        /// <param name="pos">增加的数值(可以是负数)</param>
        /// <returns>执行是否成功</returns>
        public void UpdateUserExtCredits(int uid, int extid, float pos)
        {
            Document doc = MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("_id", uid));
            if (doc != null)
            {
                doc["extcredits" + extid] = (TypeConverter.ObjectToFloat(doc["extcredits" + extid]) + pos).ToString();
                MongoDbHelper.Update(mongoDB, "users", doc);
            }
        }

        /// <summary>
        /// 获得指定用户的指定积分扩展字段的值
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="extid">扩展字段序号(1-8)</param>
        /// <returns>值</returns>
        public float GetUserExtCredits(int uid, int extid)
        {
            Document doc = MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("_id", uid));
            if (doc != null)
            {
                return TypeConverter.ObjectToFloat(doc["extcredits" + extid]);
            }
            return 0;
        }

        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="avatar">头像</param>
        /// <param name="avatarwidth">头像宽度</param>
        /// <param name="avatarheight">头像高度</param>
        /// <param name="templateid">模板Id</param>
        /// <returns>如果用户不存在则返回false, 否则返回true</returns>
        public void UpdateUserPreference(int uid, string avatar, int avatarwidth, int avatarheight, int templateid)
        {
            MongoDbHelper.Update(mongoDB, "users",
                 new Document() { { "$set", new Document() { { "avatar", avatar }, { "avatarwidth", avatarwidth }, { "avatarheight", avatarheight }, { "templateid", templateid } } } },
                 new Document().Add("_id", uid));
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <returns>成功返回true否则false</returns>
        public void UpdateUserPassword(int uid, string password, bool originalpassword)
        {
            MongoDbHelper.Update(mongoDB, "users",
                new Document() { { "$set", new Document() { { "password", originalpassword ? Utils.MD5(password) : password } } } },
                new Document().Add("_id", uid));
        }

        /// <summary>
        /// 更新用户安全问题
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="userSecques">用户安全问题答案的存储数据</param>
        /// <returns>成功返回true否则false</returns>
        public void UpdateUserSecques(int uid, string userSecques)
        {
            MongoDbHelper.Update(mongoDB, "users",
               new Document() { { "$set", new Document() { { "secques", userSecques } } } },
               new Document().Add("_id", uid));
        }


        /// <summary>
        /// 更新用户最后登录时间
        /// </summary>
        /// <param name="uid">用户id</param>
        public void UpdateUserLastvisit(int uid, string ip)
        {
            MongoDbHelper.Update(mongoDB, "users",
              new Document() { { "$set", new Document() { { "lastvisit", DateTime.Now.ToString() }, { "ip", ip } } } },
              new Document().Add("_id", uid));
        }

        /// <summary>
        /// 更新用户当前的在线状态
        /// </summary>
        /// <param name="uidlist">用户uid列表</param>
        /// <param name="state">当前在线状态(0:离线,1:在线)</param>
        public void UpdateUserOnlineState(string uidlist, int state, string activitytime)
        {
            foreach (string uid in uidlist.Split(','))
            {
                UpdateUserOnlineState(TypeConverter.StrToInt(uid), state, activitytime);
            }
        }

        /// <summary>
        /// 更新用户当前的在线状态
        /// </summary>
        /// <param name="uid">用户uid列表</param>
        /// <param name="state">当前在线状态(0:离线,1:在线)</param>
        public void UpdateUserOnlineState(int uid, int state, string activitytime)
        {
            Document doc =  MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("_id", uid));
            if(doc !=null)
            {
                switch (state)
                {
                    case 0:		//正常退出
                        {
                            doc["onlinestate"] = "0";
                            doc["lastactivity"] = activitytime;
                            break;
                        }
                    case 1:		//正常登录
                        {
                            doc["onlinestate"] = "1";
                            doc["lastvisit"] = activitytime;
                            break;
                        }
                    case 2:		//超时退出
                        {
                            doc["onlinestate"] = "0";
                            doc["lastactivity"] = activitytime;
                            break;
                        }
                    case 3:		//隐身登录
                        {
                            doc["onlinestate"] = "0";
                            doc["lastvisit"] = activitytime;
                            break;
                        }
                }
                MongoDbHelper.Update(mongoDB, "users", doc);
            }
        }

        /// <summary>
        /// 更新用户当前的在线时间和最后活动时间
        /// </summary>
        /// <param name="uid">用户uid</param>
        public void UpdateUserOnlineTime(int uid, string activitytime)
        {
            MongoDbHelper.Update(mongoDB, "users",
            new Document() { { "$set", new Document() { { "lastactivity", activitytime } } } },
            new Document().Add("_id", uid));
        }

        /// <summary>
        /// 设置用户信息表中未读短消息的数量
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="pmnum">短消息数量</param>
        /// <returns>更新记录个数</returns>
        public int SetUserNewPMCount(int uid, int pmnum)
        {
            Document doc = MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("_id", uid));
            if (doc != null)
            {
                MongoDbHelper.Update(mongoDB, "users",
                   new Document() { { "$set", new Document() { { "newpmcount", pmnum } } } },
                   new Document().Add("_id", uid));
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 将用户的未读短信息数量减小一个指定的值
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="subval">短消息将要减小的值,负数为加</param>
        /// <returns>更新记录个数</returns>
        public int DecreaseNewPMCount(int uid, int subval)
        {
            Document doc = MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("_id", uid));
            if (doc != null)
            {
                int newpmcount = TypeConverter.ObjectToInt(doc["newpmcount"]);
                MongoDbHelper.Update(mongoDB, "users",
                   new Document() { { "$set", new Document() { { "newpmcount", newpmcount >= 0 ? newpmcount - subval : 0 } } } },
                   new Document().Add("_id", uid));
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 更新用户精华数
        /// </summary>
        /// <param name="useridlist">uid列表</param>
        /// <returns></returns>
        public int UpdateUserDigest(string useridlist)
        {
            foreach(string uid in useridlist.Split(','))
            {
                IDataReader dataReader = DatabaseProvider.GetInstance().GetUserInfoToReader(TypeConverter.StrToInt(uid));
                UserInfo userinfo = null;
                if (dataReader.Read())
                {
                    userinfo = Discuz.Data.Users.LoadSingleUserInfo(dataReader);
                    dataReader.Close();
                }
               if (userinfo != null)
               {
                   Document columns = LoadSingleUser(userinfo);
                   MongoDbHelper.Update(mongoDB, "users", columns);
               }
            }
            return useridlist.Split(',').Length;
        }

        /// <summary>
        /// 更新用户SpaceID
        /// </summary>
        /// <param name="spaceid">要更新的SpaceId</param>
        /// <param name="userid">要更新的UserId</param>
        /// <returns>是否更新成功</returns>
        public void UpdateUserSpaceId(int spaceid, int userid)
        {
            MongoDbHelper.Update(mongoDB, "users",
               new Document() { { "$set", new Document() { { "spaceid", spaceid } } } },
               new Document().Add("_id", userid));
        }

        /// <summary>
        /// 更新用户短消息设置
        /// </summary>
        /// <param name="user">用户信息</param>
        public void UpdateUserPMSetting(UserInfo user)
        {
            MongoDbHelper.Update(mongoDB, "users",
              new Document() { { "$set", new Document() { { "pmsound", user.Pmsound }, { "newsletter", user.Newsletter }, { "ignorepm", user.Ignorepm } } } },
              new Document().Add("_id", user.Uid));
        }

        /// <summary>
        /// 更新被禁止的用户
        /// </summary>
        /// <param name="groupid">用户组id</param>
        /// <param name="groupexpiry">过期时间</param>
        /// <param name="uid">用户id</param>
        public void UpdateBanUser(int groupid, string groupexpiry, int uid)
        {
            MongoDbHelper.Update(mongoDB, "users",
              new Document() { { "$set", new Document() { { "groupid", groupid }, { "groupexpiry", groupexpiry } } } },
              new Document().Add("_id", uid));
        }

        /// <summary>
        /// 得到指定用户的指定积分扩展字段的积分值
        /// </summary>
        /// <param name="uid">指定用户id</param>
        /// <param name="extnumber">指定扩展字段</param>
        /// <returns>扩展字展积分值</returns>
        public int GetUserExtCreditsByUserid(int uid, int extnumber)
        {
            Document doc = MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("_id", uid));
            if (doc != null)
            {
                return TypeConverter.ObjectToInt(doc["extcredits" + extnumber]);
            }
            return -1;
        }

        /// <summary>
        /// 更新用户勋章信息
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="medals">勋章信息</param>
        public void UpdateMedals(int uid, string medals)
        {
            MongoDbHelper.Update(mongoDB, "users",
              new Document() { { "$set", new Document() { { "medals", medals } } } },
              new Document().Add("_id", uid));
        }

        /// <summary>
        /// 更改用户组用户的管理权限
        /// </summary>
        /// <param name="adminId">管理组Id</param>
        /// <param name="groupId">用户组Id</param>
        public void UpdateUserAdminIdByGroupId(int adminId, int groupId)
        {
            MongoDbHelper.Update(mongoDB, "users",
              new Document() { { "$set", new Document() { { "adminid", adminId } } } },
              new Document().Add("groupid", groupId));
        }

        /// <summary>
        /// 更新用户到禁言组
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        public void UpdateUserToStopTalkGroup(string uidList)
        {
            MongoDbHelper.Update(mongoDB, "users",
              new Document() { { "$set", new Document() { { "groupid", 4 }, { "adminid", 0 } } } },
              new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(uidList))));
        }

        /// <summary>
        /// 清除用户所发帖数以及精华数
        /// </summary>
        /// <param name="uid">用户Id</param>
        public void ClearPosts(int uid)
        {
            MongoDbHelper.Update(mongoDB, "users",
              new Document() { { "$set", new Document() { { "digestposts", 0 }, { "posts", 0 } } } },
              new Document().Add("_id", uid));
        }

        /// <summary>
        /// 更新Email验证信息
        /// </summary>
        /// <param name="authstr">验证字符串</param>
        /// <param name="authtime">验证时间</param>
        /// <param name="uid">用户Id</param>
        public void UpdateEmailValidateInfo(string authstr, DateTime authTime, int uid)
        {
            MongoDbHelper.Update(mongoDB, "users",
             new Document() { { "$set", new Document() { { "authstr", authstr }, { "authtime", authTime.ToString() }, { "authflag", 1 } } } },
             new Document().Add("_id", uid));
        }

        /// <summary>
        /// 更新用户积分,目前只能清空全部数据，然后再从数据库中获取相应用户信息
        /// </summary>
        /// <param name="credits">积分</param>
        public void UpdateUserCredits(string credits)
        {
            //注:此处未实现相应方法，需要有客户端工具来重新统计用过发帖数
            MongoDbHelper.CleanTable(mongoDB, "users");
        }

        /// <summary>
        /// 将Uid列表中的用户更新到目标组中
        /// </summary>
        /// <param name="groupid">目标组</param>
        /// <param name="uidList">用户列表</param>
        public void UpdateUserGroupByUidList(int groupid, string uidList)
        {
            MongoDbHelper.Update(mongoDB, "users",
              new Document() { { "$set", new Document() { { "groupid", groupid } } } },
              new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(uidList))));
        }

        /// <summary>
        /// 按用户Id列表删除用户
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        public void DeleteUsers(string uidList)
        {
            //TODO:是否应该调用DeleteUser方法？
            MongoDbHelper.Delete(mongoDB, "users",
                   new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(uidList))));
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="delPosts">是否删除帖子</param>
        /// <param name="delPms">是否删除短信</param>
        /// <returns></returns>
        public bool DeleteUser(int uid, bool delPosts, bool delPms)
        {
            MongoDbHelper.Delete(mongoDB, "users",
                  new Document().Add("_id", 1));
            return true;
        }

        /// <summary>
        /// 清空用户Id列表中的验证码
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        public void ClearUsersAuthstr(string uidList)
        {
            MongoDbHelper.Update(mongoDB, "users",
              new Document() { { "$set", new Document() { { "authstr", "" } } } },
              new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(uidList))));
        }

        /// <summary>
        /// 更新用户签名，来自，简介三个字段
        /// </summary>
        /// <param name="__userinfo"></param>
        /// <returns></returns>
        public void UpdateUserInfo(string location, string bio, string signature, int uid)
        {
            MongoDbHelper.Update(mongoDB, "users",
                new Document() { { "$set", new Document() { { "signature", signature }, { "location", location }, { "bio", bio } } } },
                new Document().Add("_id", uid));
        }

        /// <summary>
        /// 设置用户为版主
        /// </summary>
        /// <param name="userName">用户名</param>
        public void SetUserToModerator(string userName)
        {
            MongoDbHelper.Update(mongoDB, "users",
               new Document() { { "$set", new Document() { { "adminid", 3 }, { "groupid", 3 } } } },
               new Document().Add("username", userName));
        }

        /// <summary>
        /// 合并用户
        /// </summary>
        /// <param name="postTableName">分表名称</param>
        /// <param name="targetUserInfo">目标用户</param>
        /// <param name="srcUserInfo">要合并用户</param>
        public void CombinationUser(string postTableName, UserInfo targetUserInfo, UserInfo srcUserInfo)
        {
            MongoDbHelper.Update(mongoDB, "users",
              new Document() { { "$set", new Document() { { "posts", srcUserInfo.Posts + targetUserInfo.Posts } } } },
              new Document().Add("_id", targetUserInfo.Uid));
        }
       

        /// <summary>
        /// 更新用户帖数
        /// </summary>
        /// <param name="postcount">帖子数</param>
        /// <param name="userid">用户ID</param>
        public void UpdateUserPostCount(int postcount, int userid)
        {
            MongoDbHelper.Update(mongoDB, "users",
               new Document() { { "$set", new Document() { { "posts", postcount } } } },
               new Document().Add("_id", userid));
        }

        /// <summary>
        /// 更新所有用户的帖子数
        /// </summary>
        /// <param name="postTableId"></param>
        public void UpdateAllUserPostCount(int postTableId)
        {
            //注:此处未实现存储过程（[dnt_resetuserspostcount]）中的相应方法，需要有客户端工具来重新统计用过发帖数
            MongoDbHelper.CleanTable(mongoDB, "users");
        }

        /// <summary>
        /// 重建用户精华帖数,访方法运行完成后应清除所有有户的缓存信息
        /// </summary>
        public void ResetUserDigestPosts()
        {
            //注:此处未实现存储过程（[dnt_resetuserspostcount]）中的相应方法，需要有客户端工具来重新统计用过发帖数
            MongoDbHelper.CleanTable(mongoDB, "users");
        }

        /// <summary>
        /// 更新普通用户用户组
        /// </summary>
        /// <param name="groupid">用户组id</param>
        /// <param name="userid">用户ID</param>
        public void UpdateUserOtherInfo(int groupid, int userid)
        {
            MongoDbHelper.Update(mongoDB, "users",
               new Document() { { "$set", new Document() { { "groupid", groupid }, { "adminid", 0 } } } },
               new Document().Add("_id", userid));
        }

        /// <summary>
        /// 更新在线表用户信息
        /// </summary>
        /// <param name="groupid">用户组id</param>
        /// <param name="userid">用户ID</param>
        public void UpdateUserOnlineInfo(int groupid, int userid)
        {
            MongoDbHelper.Update(mongoDB, "users",
              new Document() { { "$set", new Document() { { "groupid", groupid } } } },
              new Document().Add("_id", userid));
        }

        /// <summary>
        /// 根据pidlist中的发帖人信息移除用户信息
        /// </summary>
        /// <param name="currentPostTableId">当前分表ID</param>
        /// <param name="pidlist">pidlist串</param>
        public void RemoveUser(int currentPostTableId, string pidlist)
        {
            IDataReader dataReader = DbHelper.ExecuteReader(System.Data.CommandType.Text, string.Format("SELECT [posterid] FROM [{0}posts{1}] WHERE pid IN ({2})", BaseConfigs.GetTablePrefix, currentPostTableId, pidlist));
            while(dataReader.Read())
                MongoDbHelper.Update(mongoDB, "users", new Document().Add("_id", TypeConverter.ObjectToInt(dataReader["posterid"])));
            dataReader.Close();
        }

        /// <summary>
        /// 根据tidlist中的发帖人信息更新用户的发帖数
        /// </summary>
        /// <param name="currentPostTableId">当前分表ID</param>
        /// <param name="tidlist">tidlist串</param>
        public void UpdateUserPost(string currentPostTableId, string tidlist)
        {
            IDataReader dataReader = DbHelper.ExecuteReader(System.Data.CommandType.Text, string.Format("SELECT [posterid] FROM [{0}posts{1}] WHERE [tid] IN ({2})", BaseConfigs.GetTablePrefix, currentPostTableId, tidlist));
            while (dataReader.Read())
            {
                Document doc = MongoDbHelper.FindOne(mongoDB, "users", new Document().Add("_id", TypeConverter.ObjectToInt(dataReader["posterid"])));
                if (doc != null)
                {
                    int newpmcount = TypeConverter.ObjectToInt(doc["newpmcount"]);
                    doc["posts"] = TypeConverter.ObjectToInt(doc["posts"]) - 1;
                    MongoDbHelper.Update(mongoDB, "users", doc);
                }
            }
            dataReader.Close();
        }
    }
}