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
    /// <summary>
    /// 用户数据类，注：UpdateAllUserPostCount，ResetUserDigestPosts，UpdateUserCredits这三个方法需要有客户端支持(DntTokyoTyrantTools.exe)才能使用。
    /// </summary>
    public class Users : Discuz.Cache.Data.ICacheUsers
    {
        private static TcpClientIOPool pool = TcpClientIOPool.GetInstance("dnt_users");

        private static DBCache ttCache = EntLibConfigs.GetConfig().Cacheusers;

        static Users()
        {
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

            //TokyoTyrantService.SetIndex(pool, "uid", IndexOption.DECIMAL);
            //TokyoTyrantService.SetIndex(pool, "groupid", IndexOption.DECIMAL);
            //TokyoTyrantService.SetIndex(pool, "adminid", IndexOption.DECIMAL);
            //TokyoTyrantService.SetIndex(pool, "username", IndexOption.LEXICAL);
            //TokyoTyrantService.SetIndex(pool, "secques", IndexOption.LEXICAL);             
            //TokyoTyrantService.SetIndex(pool, "password", IndexOption.LEXICAL);
            //TokyoTyrantService.SetIndex(pool, "email", IndexOption.LEXICAL);
            //TokyoTyrantService.SetIndex(pool, "regip", IndexOption.LEXICAL);      
        }      

        #region 获取用户信息
        public UserInfo LoadSingleUserInfo(IDictionary<string, string> column)
        {
            UserInfo userinfo = new UserInfo();
            userinfo.Uid = TypeConverter.ObjectToInt(column["uid"]);
            userinfo.Username = column["username"];
            userinfo.Nickname = column["nickname"];
            userinfo.Password = column["password"];
            userinfo.Spaceid = TypeConverter.ObjectToInt(column["spaceid"]);
            userinfo.Secques = column["secques"];
            userinfo.Gender = TypeConverter.ObjectToInt(column["gender"]);
            userinfo.Adminid = TypeConverter.ObjectToInt(column["adminid"]);
            userinfo.Groupid = TypeConverter.ObjectToInt(column["groupid"]);
            userinfo.Groupexpiry = TypeConverter.ObjectToInt(column["groupexpiry"]);
            userinfo.Extgroupids = column["extgroupids"].ToString();
            userinfo.Regip = column["regip"];
            userinfo.Joindate = Utils.GetStandardDateTime(column["joindate"].ToString());
            userinfo.Lastip = column["lastip"];
            userinfo.Lastvisit = Utils.GetStandardDateTime(column["lastvisit"].ToString());
            userinfo.Lastactivity = Utils.GetStandardDateTime(column["lastactivity"].ToString());
            userinfo.Lastpost = Utils.GetStandardDateTime(column["lastpost"].ToString());
            userinfo.Lastpostid = TypeConverter.ObjectToInt(column["lastpostid"]);
            userinfo.Lastposttitle = column["lastposttitle"].ToString();
            userinfo.Posts = TypeConverter.ObjectToInt(column["posts"]);
            userinfo.Digestposts = TypeConverter.ObjectToInt(column["digestposts"]);
            userinfo.Oltime = TypeConverter.ObjectToInt(column["oltime"]);
            userinfo.Pageviews = TypeConverter.ObjectToInt(column["pageviews"]);
            userinfo.Credits = TypeConverter.ObjectToInt(column["credits"]);
            userinfo.Extcredits1 = TypeConverter.StrToFloat(column["extcredits1"].ToString());
            userinfo.Extcredits2 = TypeConverter.StrToFloat(column["extcredits2"].ToString());
            userinfo.Extcredits3 = TypeConverter.StrToFloat(column["extcredits3"].ToString());
            userinfo.Extcredits4 = TypeConverter.StrToFloat(column["extcredits4"].ToString());
            userinfo.Extcredits5 = TypeConverter.StrToFloat(column["extcredits5"].ToString());
            userinfo.Extcredits6 = TypeConverter.StrToFloat(column["extcredits6"].ToString());
            userinfo.Extcredits7 = TypeConverter.StrToFloat(column["extcredits7"].ToString());
            userinfo.Extcredits8 = TypeConverter.StrToFloat(column["extcredits8"].ToString());
            userinfo.Medals = column["medals"].ToString();
            userinfo.Email = column["email"].ToString();
            userinfo.Bday = column["bday"].ToString();
            userinfo.Sigstatus = TypeConverter.ObjectToInt(column["sigstatus"]);
            userinfo.Tpp = TypeConverter.ObjectToInt(column["tpp"]);
            userinfo.Ppp = TypeConverter.ObjectToInt(column["ppp"]);
            userinfo.Templateid = TypeConverter.ObjectToInt(column["templateid"]);
            userinfo.Pmsound = TypeConverter.ObjectToInt(column["pmsound"]);
            userinfo.Showemail = TypeConverter.ObjectToInt(column["showemail"]);
            userinfo.Newsletter = (ReceivePMSettingType)TypeConverter.ObjectToInt(column["newsletter"]);
            userinfo.Invisible = TypeConverter.ObjectToInt(column["invisible"]);
            userinfo.Newpm = TypeConverter.ObjectToInt(column["newpm"]);
            userinfo.Newpmcount = TypeConverter.ObjectToInt(column["newpmcount"]);
            userinfo.Accessmasks = TypeConverter.ObjectToInt(column["accessmasks"]);
            userinfo.Onlinestate = TypeConverter.ObjectToInt(column["onlinestate"]);
            userinfo.Website = column["website"];
            userinfo.Icq = column["icq"];
            userinfo.Qq = column["qq"];
            userinfo.Yahoo = column["yahoo"];
            userinfo.Msn = column["msn"];
            userinfo.Skype = column["skype"];
            userinfo.Location = column["location"];
            userinfo.Customstatus = column["customstatus"];
            userinfo.Bio = column["bio"];
            userinfo.Signature = column["signature"];
            userinfo.Sightml = column["sightml"];
            userinfo.Authstr = column["authstr"];
            userinfo.Authtime = column["authtime"];
            userinfo.Authflag = Byte.Parse(column["authflag"]);
            userinfo.Realname = column["realname"];
            userinfo.Idcard = column["idcard"];
            userinfo.Mobile = column["mobile"];
            userinfo.Phone = column["phone"];
            userinfo.Ignorepm = column["ignorepm"];
            userinfo.Salt = column["salt"].Trim();
            return userinfo;
        }


        public IDictionary<string, string> LoadSingleUser(UserInfo userinfo)
        {
            IDictionary<string, string> userdic = new System.Collections.Generic.Dictionary<string, string>();
            userdic.Add("uid", userinfo.Uid.ToString());
            userdic.Add("username", userinfo.Username);
            userdic.Add("nickname", userinfo.Nickname);
            userdic.Add("password", userinfo.Password);
            userdic.Add("spaceid", userinfo.Spaceid.ToString());
            userdic.Add("secques", userinfo.Secques);
            userdic.Add("gender", userinfo.Gender.ToString());
            userdic.Add("adminid", userinfo.Adminid.ToString());
            userdic.Add("groupid", userinfo.Groupid.ToString());
            userdic.Add("groupexpiry", userinfo.Groupexpiry.ToString());
            userdic.Add("extgroupids", userinfo.Extgroupids);
            userdic.Add("regip", userinfo.Regip);
            userdic.Add("joindate", userinfo.Joindate);
            userdic.Add("lastip", userinfo.Lastip);
            userdic.Add("lastvisit", userinfo.Lastvisit);
            userdic.Add("lastactivity", userinfo.Lastactivity);
            userdic.Add("lastpost", userinfo.Lastpost);
            userdic.Add("lastpostid", userinfo.Lastpostid.ToString());
            userdic.Add("lastposttitle", userinfo.Lastposttitle);
            userdic.Add("posts", userinfo.Posts.ToString());
            userdic.Add("digestposts", userinfo.Digestposts.ToString());
            userdic.Add("oltime", userinfo.Oltime.ToString());
            userdic.Add("pageviews", userinfo.Pageviews.ToString());
            userdic.Add("credits", userinfo.Credits.ToString());
            userdic.Add("extcredits1", userinfo.Extcredits1.ToString());
            userdic.Add("extcredits2", userinfo.Extcredits2.ToString());
            userdic.Add("extcredits3", userinfo.Extcredits3.ToString());
            userdic.Add("extcredits4", userinfo.Extcredits4.ToString());
            userdic.Add("extcredits5", userinfo.Extcredits5.ToString());
            userdic.Add("extcredits6", userinfo.Extcredits6.ToString());
            userdic.Add("extcredits7", userinfo.Extcredits7.ToString());
            userdic.Add("extcredits8", userinfo.Extcredits8.ToString());
            userdic.Add("medals", userinfo.Medals);
            userdic.Add("email", userinfo.Email);
            userdic.Add("bday", userinfo.Bday);
            userdic.Add("sigstatus", userinfo.Sigstatus.ToString());
            userdic.Add("tpp", userinfo.Tpp.ToString());
            userdic.Add("ppp", userinfo.Ppp.ToString());
            userdic.Add("templateid", userinfo.Templateid.ToString());
            userdic.Add("pmsound", userinfo.Pmsound.ToString());
            userdic.Add("showemail", userinfo.Showemail.ToString());
            userdic.Add("newsletter", userinfo.Newsletter.ToString());
            userdic.Add("invisible", userinfo.Invisible.ToString());
            userdic.Add("newpm", userinfo.Newpm.ToString());
            userdic.Add("newpmcount", userinfo.Newpmcount.ToString());
            userdic.Add("accessmasks", userinfo.Accessmasks.ToString());
            userdic.Add("onlinestate", userinfo.Onlinestate.ToString());
            userdic.Add("website", userinfo.Website);
            userdic.Add("icq", userinfo.Icq);
            userdic.Add("qq", userinfo.Qq);
            userdic.Add("yahoo", userinfo.Yahoo);
            userdic.Add("msn", userinfo.Msn);
            userdic.Add("skype", userinfo.Skype);
            userdic.Add("location", userinfo.Location);
            userdic.Add("customstatus", userinfo.Customstatus);
            userdic.Add("bio", userinfo.Bio);
            userdic.Add("signature", userinfo.Signature);
            userdic.Add("sightml", userinfo.Sightml);
            userdic.Add("authstr", userinfo.Authstr);
            userdic.Add("authtime", userinfo.Authtime);
            userdic.Add("authflag", userinfo.Authflag.ToString());
            userdic.Add("realname", userinfo.Realname);
            userdic.Add("idcard", userinfo.Idcard);
            userdic.Add("mobile", userinfo.Mobile);
            userdic.Add("phone", userinfo.Phone);
            userdic.Add("ignorepm", string.IsNullOrEmpty(userinfo.Ignorepm) ? "" : userinfo.Ignorepm);
            userdic.Add("salt", userinfo.Salt);
            return userdic;
        }

        //public ShortUserInfo LoadSingleShortUserInfo(IDictionary<string, string> column)
        //{
        //    ShortUserInfo userInfo = new ShortUserInfo();
        //    userInfo.Uid = TypeConverter.ObjectToInt(column["uid"]);
        //    userInfo.Username = column["username"];
        //    userInfo.Nickname = column["nickname"];
        //    userInfo.Password = column["password"];
        //    userInfo.Spaceid = TypeConverter.ObjectToInt(column["spaceid"]);
        //    userInfo.Secques = column["secques"];
        //    userInfo.Gender = TypeConverter.ObjectToInt(column["gender"]);
        //    userInfo.Adminid = TypeConverter.ObjectToInt(column["adminid"]);
        //    userInfo.Groupid = TypeConverter.ObjectToInt(column["groupid"]);
        //    userInfo.Groupexpiry = TypeConverter.ObjectToInt(column["groupexpiry"]);
        //    userInfo.Extgroupids = column["extgroupids"];
        //    userInfo.Regip = column["regip"];
        //    userInfo.Joindate = column["joindate"];
        //    userInfo.Lastip = column["lastip"];
        //    userInfo.Lastvisit = column["lastvisit"];
        //    userInfo.Lastactivity = column["lastactivity"];
        //    userInfo.Lastpost = column["lastpost"];
        //    userInfo.Lastpostid = TypeConverter.ObjectToInt(column["lastpostid"]);
        //    userInfo.Lastposttitle = column["lastposttitle"].ToString();
        //    userInfo.Posts = TypeConverter.ObjectToInt(column["posts"]);
        //    userInfo.Digestposts = TypeConverter.StrToInt(column["digestposts"]);
        //    userInfo.Oltime = TypeConverter.ObjectToInt(column["oltime"]);
        //    userInfo.Pageviews = TypeConverter.StrToInt(column["pageviews"]);
        //    userInfo.Credits = TypeConverter.ObjectToInt(column["credits"]);
        //    userInfo.Extcredits1 = TypeConverter.StrToFloat(column["extcredits1"]);
        //    userInfo.Extcredits2 = TypeConverter.StrToFloat(column["extcredits2"]);
        //    userInfo.Extcredits3 = TypeConverter.StrToFloat(column["extcredits3"]);
        //    userInfo.Extcredits4 = TypeConverter.StrToFloat(column["extcredits4"]);
        //    userInfo.Extcredits5 = TypeConverter.StrToFloat(column["extcredits5"]);
        //    userInfo.Extcredits6 = TypeConverter.StrToFloat(column["extcredits6"]);
        //    userInfo.Extcredits7 = TypeConverter.StrToFloat(column["extcredits7"]);
        //    userInfo.Extcredits8 = TypeConverter.StrToFloat(column["extcredits8"]);
        //    userInfo.Email = column["email"];
        //    userInfo.Bday = column["bday"];
        //    userInfo.Sigstatus = TypeConverter.ObjectToInt(column["sigstatus"]);
        //    userInfo.Tpp = TypeConverter.ObjectToInt(column["tpp"]);
        //    userInfo.Ppp = TypeConverter.ObjectToInt(column["ppp"]);
        //    userInfo.Templateid = TypeConverter.ObjectToInt(column["templateid"]);
        //    userInfo.Pmsound = TypeConverter.ObjectToInt(column["pmsound"]);
        //    userInfo.Showemail = TypeConverter.ObjectToInt(column["showemail"]);
        //    userInfo.Newsletter = (ReceivePMSettingType)TypeConverter.ObjectToInt(column["newsletter"]);
        //    userInfo.Invisible = TypeConverter.ObjectToInt(column["invisible"]);
        //    userInfo.Newpm = TypeConverter.ObjectToInt(column["newpm"]);
        //    userInfo.Newpmcount = TypeConverter.ObjectToInt(column["newpmcount"]);
        //    userInfo.Accessmasks = TypeConverter.ObjectToInt(column["accessmasks"]);
        //    userInfo.Onlinestate = TypeConverter.ObjectToInt(column["onlinestate"]);
        //    userInfo.Salt = column["salt"];//二次MD5所用的字段
        //    return userInfo;
        //}
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                userinfo = LoadSingleUserInfo(column);
            }
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                columns["extcredits" + creditsTrans] = (TypeConverter.StrToInt(columns["extcredits" + creditsTrans]) - price).ToString();
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
                break;
            }

            qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", posterid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                columns["extcredits" + creditsTrans] = (TypeConverter.StrToInt(columns["extcredits" + creditsTrans]) + netamount).ToString();
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
                return;
            }
        }

        /// <summary>
        /// 更新用户的用户组信息
        /// </summary>
        /// <param name="uidList">用户ID列表</param>
        /// <param name="groupId">用户组ID</param>
        public void UpdateUserGroup(string uidList, int groupId)
        {
            foreach (string uid in uidList.Split(','))
            {
                var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", TypeConverter.StrToInt(uid)));
                foreach (string key in qrecords.Keys)
                {
                    IDictionary<string, string> columns = qrecords[key];
                    columns["groupid"] = groupId.ToString();
                    TokyoTyrantService.PutColumns(pool, key, columns, true);
                    break;
                }
            }
        }


        /// <summary>
        /// 根据IP查找用户
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>用户信息</returns>
        public UserInfo GetUserInfoByIP(string ip)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().StringEquals("regip", ip).LimitTo(1, 0));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                return LoadSingleUserInfo(columns);
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().StringEquals("username", username).LimitTo(1, 0));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                return LoadSingleUserInfo(columns);
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().StringEquals("username", username).StringEquals("email", email).StringEquals("secques", userSecques).LimitTo(1, 0));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                return TypeConverter.ObjectToInt(columns["uid"], -1);
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().StringEquals("username", username).StringEquals("password", originalpassword ? Utils.MD5(password) : password).StringEquals("secques", userSecques).LimitTo(1, 0));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                return TypeConverter.ObjectToInt(columns["uid"], -1);
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().StringEquals("username", username).StringEquals("password", originalpassword ? Utils.MD5(password) : password).LimitTo(1, 0));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                return LoadSingleUserInfo(columns);
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid).StringEquals("password", originalpassword ? Utils.MD5(password) : password).LimitTo(1, 0));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                return LoadSingleUserInfo(columns);
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().StringEquals("email", email).LimitTo(1, 0));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                return TypeConverter.ObjectToInt(columns["uid"], -1);
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
            IDictionary<string, string> columns = LoadSingleUser(userinfo);
            TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
            return userinfo.Uid;
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="userinfo">用户信息</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateUser(UserInfo userinfo)
        {
            IDictionary<string, string> columns = LoadSingleUser(userinfo);
            TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                columns["authstr"] = authstr;
                columns["authtime"] = Utils.GetDate();
                columns["authflag"] = authflag.ToString();
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
                return;
            }            
        }


        /// <summary>
        /// 更新指定用户的个人资料
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>如果用户不存在则为false, 否则为true</returns>
        public void UpdateUserProfile(UserInfo userinfo)
        {
            IDictionary<string, string> columns = LoadSingleUser(userinfo);           
            TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);            
        }

        /// <summary>
        /// 更新用户论坛设置
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>如果用户不存在则返回false, 否则返回true</returns>
        public void UpdateUserForumSetting(UserInfo userinfo)
        {
            IDictionary<string, string> columns = LoadSingleUser(userinfo);
            TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);            
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
             var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
             foreach (string key in qrecords.Keys)
             {
                 IDictionary<string, string> columns = qrecords[key];
                 columns["extcredits" + extid] = (TypeConverter.StrToFloat(columns["extcredits" + extid]) + pos).ToString();                 
                 TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);      
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                return TypeConverter.StrToFloat(columns["extcredits" + extid]);
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                columns["avatar"] = avatar;
                columns["avatarwidth"] = avatarwidth.ToString();
                columns["avatarheight"] = avatarheight.ToString();
                columns["templateid"] = templateid.ToString();
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);    
            }
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                columns["password"] = originalpassword ? Utils.MD5(password) : password;
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
            }
        }

        /// <summary>
        /// 更新用户安全问题
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="userSecques">用户安全问题答案的存储数据</param>
        /// <returns>成功返回true否则false</returns>
        public void UpdateUserSecques(int uid, string userSecques)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                columns["secques"] = userSecques;
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
            }
        }


        /// <summary>
        /// 更新用户最后登录时间
        /// </summary>
        /// <param name="uid">用户id</param>
        public void UpdateUserLastvisit(int uid, string ip)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                columns["lastvisit"] = DateTime.Now.ToString();
                columns["ip"] = ip;
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
            }
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
            foreach (string key in qrecords.Keys)
            {
                IDictionary<string, string> columns = qrecords[key];
                switch (state)
                {
                    case 0:		//正常退出
                        {
                            columns["onlinestate"] = "0";
                            columns["lastactivity"] = activitytime;
                            break;
                        }
                    case 1:		//正常登录
                        {
                            columns["onlinestate"] = "1";
                            columns["lastvisit"] = activitytime;
                            break;
                        }
                    case 2:		//超时退出
                        {
                            columns["onlinestate"] = "0";
                            columns["lastactivity"] = activitytime;
                            break;
                        }
                    case 3:		//隐身登录
                        {
                            columns["onlinestate"] = "0";
                            columns["lastvisit"] = activitytime;
                            break;
                        }
                }
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
            }
        }

        /// <summary>
        /// 更新用户当前的在线时间和最后活动时间
        /// </summary>
        /// <param name="uid">用户uid</param>
        public void UpdateUserOnlineTime(int uid, string activitytime)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                columns["lastactivity"] = activitytime;
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
                return;
            }
        }

        /// <summary>
        /// 设置用户信息表中未读短消息的数量
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="pmnum">短消息数量</param>
        /// <returns>更新记录个数</returns>
        public int SetUserNewPMCount(int uid, int pmnum)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                columns["newpmcount"] = pmnum.ToString();
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                int newpmcount = TypeConverter.StrToInt(columns["newpmcount"]);
                columns["newpmcount"] = (newpmcount >=0? newpmcount - subval : 0).ToString();
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
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
            foreach (string uid in useridlist.Split(','))
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
                    var columns = LoadSingleUser(userinfo);
                    TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", userid));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                columns["spaceid"] = spaceid.ToString();
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
                break;
            }
        }

        /// <summary>
        /// 更新用户短消息设置
        /// </summary>
        /// <param name="user">用户信息</param>
        public void UpdateUserPMSetting(UserInfo user)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", user.Uid));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                columns["pmsound"] = user.Pmsound.ToString();
                columns["newsletter"] = user.Newsletter.ToString();
                columns["ignorepm"] = user.Ignorepm;
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
                break;
            }
        }

        /// <summary>
        /// 更新被禁止的用户
        /// </summary>
        /// <param name="groupid">用户组id</param>
        /// <param name="groupexpiry">过期时间</param>
        /// <param name="uid">用户id</param>
        public void UpdateBanUser(int groupid, string groupexpiry, int uid)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                columns["groupid"] = groupid.ToString();
                columns["groupexpiry"] = groupexpiry;
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
                break;
            }
        }

        /// <summary>
        /// 得到指定用户的指定积分扩展字段的积分值
        /// </summary>
        /// <param name="uid">指定用户id</param>
        /// <param name="extnumber">指定扩展字段</param>
        /// <returns>扩展字展积分值</returns>
        public int GetUserExtCreditsByUserid(int uid, int extnumber)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                return TypeConverter.StrToInt(columns["extcredits" + extnumber]);
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                columns["medals"] = medals;
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
                break;
            }
        }

        /// <summary>
        /// 更改用户组用户的管理权限
        /// </summary>
        /// <param name="adminId">管理组Id</param>
        /// <param name="groupId">用户组Id</param>
        public void UpdateUserAdminIdByGroupId(int adminId, int groupId)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("groupid", groupId));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                columns["adminid"] = adminId.ToString();
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
                break;
            }
        }

        /// <summary>
        /// 更新用户到禁言组
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        public void UpdateUserToStopTalkGroup(string uidList)
        {
            var qrecords = TokyoTyrantService.GetColumns(pool, uidList.Split(','));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                columns["groupid"] = "4";
                columns["adminid"] = "0";
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
                break;
            }
        }

        /// <summary>
        /// 清除用户所发帖数以及精华数
        /// </summary>
        /// <param name="uid">用户Id</param>
        public void ClearPosts(int uid)
        {
             var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
             foreach (string key in qrecords.Keys)
             {
                 var columns = qrecords[key];
                 columns["digestposts"] = "0";
                 columns["posts"] = "0";
                 TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
                 break;
             }
        }

        /// <summary>
        /// 更新Email验证信息
        /// </summary>
        /// <param name="authstr">验证字符串</param>
        /// <param name="authtime">验证时间</param>
        /// <param name="uid">用户Id</param>
        public void UpdateEmailValidateInfo(string authstr, DateTime authTime, int uid)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                columns["authstr"] = authstr;
                columns["authtime"] = authTime.ToString();
                columns["authflag"] = "1";
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
                break;
            }
        }

        /// <summary>
        /// 更新用户积分,目前只能清空全部数据，然后再从数据库中获取相应用户信息
        /// </summary>
        /// <param name="credits">积分</param>
        public void UpdateUserCredits(string credits)
        {
            //注:此处未实现相应方法，需要有客户端工具来重新统计用过发帖数
            TokyoTyrantService.Vanish(pool);
        }

        /// <summary>
        /// 将Uid列表中的用户更新到目标组中
        /// </summary>
        /// <param name="groupid">目标组</param>
        /// <param name="uidList">用户列表</param>
        public void UpdateUserGroupByUidList(int groupid, string uidList)
        {
            var qrecords = TokyoTyrantService.GetColumns(pool, uidList.Split(','));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                columns["groupid"] = groupid.ToString();
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
            }
        }

        /// <summary>
        /// 按用户Id列表删除用户
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        public void DeleteUsers(string uidList)
        {
            //TODO:是否应该调用DeleteUser方法？
            TokyoTyrantService.DeleteMultiple(pool, uidList.Split(','));
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
            TokyoTyrantService.QueryDelete(pool, new Query().NumberEquals("uid", uid));
            return true;
        }

        /// <summary>
        /// 清空用户Id列表中的验证码
        /// </summary>
        /// <param name="uidList">用户Id列表</param>
        public void ClearUsersAuthstr(string uidList)
        {
            var qrecords = TokyoTyrantService.GetColumns(pool, uidList.Split(','));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                columns["authstr"] = "";
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
            }
        }

        /// <summary>
        /// 更新用户签名，来自，简介三个字段
        /// </summary>
        /// <param name="__userinfo"></param>
        /// <returns></returns>
        public void UpdateUserInfo(string location, string bio, string signature, int uid)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", uid));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                columns["signature"] = signature;
                columns["location"] = location;
                columns["bio"] = bio;
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
            }
        }

        /// <summary>
        /// 设置用户为版主
        /// </summary>
        /// <param name="userName">用户名</param>
        public void SetUserToModerator(string userName)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().StringEquals("username", userName).LimitTo(1, 0));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                columns["adminid"] = "3";
                columns["groupid"] = "3";
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
                break;
            }
        }

        /// <summary>
        /// 合并用户
        /// </summary>
        /// <param name="postTableName">分表名称</param>
        /// <param name="targetUserInfo">目标用户</param>
        /// <param name="srcUserInfo">要合并用户</param>
        public void CombinationUser(string postTableName, UserInfo targetUserInfo, UserInfo srcUserInfo)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", targetUserInfo.Uid));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                columns["posts"] = (srcUserInfo.Posts + targetUserInfo.Posts).ToString();
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
                break;
            }
        }
       

        /// <summary>
        /// 更新用户帖数
        /// </summary>
        /// <param name="postcount">帖子数</param>
        /// <param name="userid">用户ID</param>
        public void UpdateUserPostCount(int postcount, int userid)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", userid));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                columns["posts"] = postcount.ToString();
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
                break;
            }
        }

        /// <summary>
        /// 更新所有用户的帖子数
        /// </summary>
        /// <param name="postTableId"></param>
        public void UpdateAllUserPostCount(int postTableId)
        {
            //var qrecords = TokyoTyrantService.QueryRecords(pool, new TokyoQuery().NumberGreaterThan("posts", 0));
            //foreach (string key in qrecords.Keys)
            //{
            //    var columns = qrecords[key];
            //    columns["posts"] = "0";
            //    TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
            //}
            //注:此处未实现存储过程（[dnt_resetuserspostcount]）中的相应方法，需要有客户端工具来重新统计用过发帖数
            TokyoTyrantService.Vanish(pool);
        }

        /// <summary>
        /// 重建用户精华帖数,访方法运行完成后应清除所有有户的缓存信息
        /// </summary>
        public void ResetUserDigestPosts()
        {
            //注:此处未实现存储过程（[dnt_resetuserspostcount]）中的相应方法，需要有客户端工具来重新统计用过发帖数
            TokyoTyrantService.Vanish(pool);
        }

        /// <summary>
        /// 更新普通用户用户组
        /// </summary>
        /// <param name="groupid">用户组id</param>
        /// <param name="userid">用户ID</param>
        public void UpdateUserOtherInfo(int groupid, int userid)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("userid", userid));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                columns["groupid"] = groupid.ToString();
                columns["adminid"] = "0";
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
            }
        }

        /// <summary>
        /// 更新在线表用户信息
        /// </summary>
        /// <param name="groupid">用户组id</param>
        /// <param name="userid">用户ID</param>
        public void UpdateUserOnlineInfo(int groupid, int userid)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("userid", userid));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                columns["groupid"] = groupid.ToString();
                TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
            }
        }

        /// <summary>
        /// 根据pidlist中的发帖人信息移除用户信息
        /// </summary>
        /// <param name="currentPostTableId">当前分表ID</param>
        /// <param name="pidlist">pidlist串</param>
        public void RemoveUser(int currentPostTableId, string pidlist)
        {
            IDataReader dataReader = DbHelper.ExecuteReader(string.Format("SELECT [posterid] FROM [{0}posts{1}] WHERE pid IN ({2})", BaseConfigs.GetTablePrefix, currentPostTableId, pidlist));
            while(dataReader.Read())
                TokyoTyrantService.QueryDelete(pool, new Query().NumberEquals("uid", TypeConverter.ObjectToInt(dataReader["posterid"])));
            dataReader.Close();
        }

        /// <summary>
        /// 根据tidlist中的发帖人信息更新用户的发帖数
        /// </summary>
        /// <param name="currentPostTableId">当前分表ID</param>
        /// <param name="tidlist">tidlist串</param>
        public void UpdateUserPost(string currentPostTableId, string tidlist)
        {
            IDataReader dataReader = DbHelper.ExecuteReader(string.Format("SELECT [posterid] FROM [{0}posts{1}] WHERE [tid] IN ({2})", BaseConfigs.GetTablePrefix, currentPostTableId, tidlist));
            while (dataReader.Read())
            {
                var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("uid", TypeConverter.ObjectToInt(dataReader["posterid"])));
                foreach (string key in qrecords.Keys)
                {
                    var columns = qrecords[key];
                    columns["posts"] = (TypeConverter.StrToInt(columns["posts"]) - 1).ToString();
                    TokyoTyrantService.PutColumns(pool, columns["uid"], columns, true);
                }
            }
            dataReader.Close();
        }
    }
}