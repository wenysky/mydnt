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
    public class Posts : Discuz.Cache.Data.ICachePosts
    {
        private static string connectString = null;

        private static DBCache mongoCache = EntLibConfigs.GetConfig().Cacheposts;


        static Posts()
        {
            InitialMongoDB();
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

        private static Mongo mongoDB
        {
            get
            {
                return new Mongo(connectString);
            }
        }
      

        /// <summary>
        /// 创建帖子
        /// </summary>
        /// <param name="postInfo">帖子信息</param>
        /// <param name="postTableId">分表ID</param>
        /// <returns>帖子ID</returns>
        public void CreatePost(PostInfo postInfo, string postTableId)
        {
            Document doc = new Document();
            doc["_id"] = postInfo.Pid;
            doc["pid"] = postInfo.Pid;
            doc["attachment"] = postInfo.Attachment;
            doc["fid"] = postInfo.Fid;
            doc["tid"] = postInfo.Tid;
            doc["title"] = postInfo.Title;
            doc["layer"] = postInfo.Layer;
            doc["message"] = postInfo.Message.Trim();
            doc["lastedit"] = postInfo.Lastedit.Trim();
            doc["postdatetime"] = postInfo.Postdatetime.Trim();
            doc["poster"] = postInfo.Poster.Trim();
            doc["posterid"] = postInfo.Posterid;
            doc["invisible"] = postInfo.Invisible;
            doc["usesig"] = postInfo.Usesig;
            doc["htmlon"] = postInfo.Htmlon;
            doc["smileyoff"] = postInfo.Smileyoff;
            doc["parseurloff"] = postInfo.Parseurloff;
            doc["bbcodeoff"] = postInfo.Bbcodeoff;
            doc["rate"] = postInfo.Rate;
            doc["ratetimes"] = postInfo.Ratetimes;
            doc["ubbmessage"] = postInfo.Message.Trim();
            //作者ID为-1即表明作者为游客, 为了区分会直接公开显示游客发帖时的IP, 这里将IP最后一位修改为*
            doc["ip"] = postInfo.Ip.Trim();
       
            MongoDbHelper.Insert(mongoDB, "posts" + postTableId, doc);
        }

        /// <summary>
        /// 更新指定帖子信息
        /// </summary>
        /// <param name="postsInfo">帖子信息</param>
        /// <param name="postTableId">帖子所在分表Id</param>
        /// <returns>更新数量</returns>
        public void UpdatePost(PostInfo postsInfo, string postTableId)
        {
            MongoDbHelper.Update(mongoDB, "posts" + postTableId,
                    new Document() { { "$set", new Document() { { "title", postsInfo.Title }, { "message", postsInfo.Message }, { "lastedit", postsInfo.Lastedit }, 
                                                                { "invisible", postsInfo.Invisible },  
                                                                { "usesig", postsInfo.Usesig },
                                                                { "htmlon", postsInfo.Htmlon },
                                                                { "smileyoff", postsInfo.Smileyoff },
                                                                { "bbcodeoff", postsInfo.Bbcodeoff },
                                                                { "parseurloff", postsInfo.Parseurloff } 
                                                              }
                                   } },
                   new Document().Add("_id", postsInfo.Pid));
        }

        /// <summary>
        /// 删除指定ID的帖子
        /// </summary>
        /// <param name="postTableId">帖子所在分表Id</param>
        /// <param name="pid">帖子ID</param>
        /// <returns>删除数量</returns>
        public void DeletePost(string postTableId, int pid)
        {
             MongoDbHelper.Delete(mongoDB, "posts" + postTableId, new Document().Add("_id", pid));
        }

      
        /// <summary>
        /// 更新帖子的评分值
        /// </summary>
        /// <param name="postTableId">当前分表ID</param>
        /// <param name="postidlist">帖子ID列表</param>
        /// <returns>更新的帖子数量</returns>
        public void UpdatePostRateTimes(string postTableId, string postidlist)
        {
            //找出当前分表中相应帖子，并更新它们在mongodb中的内容
            IDataReader dataReader = Discuz.Data.DbHelper.ExecuteReader(CommandType.Text, "SELECT [pid], [ratetimes] FROM [" + BaseConfigs.GetTablePrefix + "posts" + postTableId + "] WHERE [pid] IN (" + postidlist + ")");
            while (dataReader.Read())
            {
                MongoDbHelper.Update(mongoDB, "posts" + postTableId,
                     new Document() { { "$set", new Document() { { "ratetimes", TypeConverter.ObjectToInt(dataReader["ratetimes"]) } } } },
                     new Document().Add("pid", TypeConverter.ObjectToInt(dataReader["pid"])));
            }
            dataReader.Close();
        }

        /// <summary>
        /// 获取指定条件的帖子DataSet
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <param name="postTableId">当前分表ID</param>
        /// <returns>指定条件的帖子DataSet</returns>
        public List<ShowtopicPagePostInfo> GetPostList(PostpramsInfo postpramsInfo, string postTableId)
        {
            return LoadPostList(postpramsInfo, postTableId);
        }


        /// <summary>
        /// 更新帖子附件类型
        /// </summary>
        /// <param name="postTableId">当前分表ID</param>
        /// <param name="pid"></param>
        /// <param name="attType"></param>
        public void UpdatePostAttachmentType(string postTableId, int pid, float attType)
        {
            MongoDbHelper.Update(mongoDB, "posts" + postTableId,
              new Document() { { "$set", new Document() { { "attachment", attType } } } },
              new Document().Add("pid", pid));
        }

        /// <summary>
        /// 更新帖子评分
        /// </summary>
        /// <param name="pid">帖子id</param>
        /// <param name="rate">评分</param>
        /// <param name="postTableId">当前分表ID</param>
        /// <returns></returns>
        public void UpdatePostRate(int pid, float rate, string postTableId)
        {
            Document doc = MongoDbHelper.FindOne(mongoDB, "posts" + postTableId, new Document().Add("pid", pid));
            doc["rate"] = TypeConverter.ObjectToFloat(doc["rate"]) + rate;
            MongoDbHelper.Update(mongoDB, "posts" + postTableId, doc, new Document().Add("pid", pid));
        }

        /// <summary>
        /// 取消帖子评分
        /// </summary>
        /// <param name="pidIdList">帖子ID列表</param>
        /// <param name="postTableId">当前分表ID</param>
        public void CancelPostRate(string pidIdList, string postTableId)
        {
            MongoDbHelper.Update(mongoDB, "posts" + postTableId,
               new Document() { { "$set", new Document() { { "rate", 0 }, { "ratetimes", 0 } } } },
               new Document().Add("pid", Op.In(TypeConverter.StringToIntArray(pidIdList))));
        }

     
  
        /// <summary>
        /// 装载帖子列表
        /// </summary>
        /// <param name="postpramsInfo">参数对象</param>
        /// <param name="postTableId">当前分表ID</param>
        /// <returns></returns>
        public List<ShowtopicPagePostInfo> LoadPostList(PostpramsInfo postpramsInfo, string postTableId)
        {
            List<ShowtopicPagePostInfo> postList = new List<ShowtopicPagePostInfo>();

            //序号(楼层)的初值
            int id = (postpramsInfo.Pageindex - 1) * postpramsInfo.Pagesize;
            System.Collections.Generic.List<Document> docList = MongoDbHelper.Find(mongoDB, "posts" + postTableId, new Document().Add("tid", postpramsInfo.Tid).Add("invisible", Op.LessThanOrEqual(0)), "_id", IndexOrder.Ascending, postpramsInfo.Pagesize, (postpramsInfo.Pageindex - 1) * postpramsInfo.Pagesize);
            //System.Collections.Generic.List<Document> docList = MongoDbHelper.Find(mongoDB, "posts" + postTableId, new Document().Add("tid", postpramsInfo.Tid).Add("invisible", Op.LessThanOrEqual(0)), postpramsInfo.Pagesize, (postpramsInfo.Pageindex - 1) * postpramsInfo.Pagesize);
            List<int> uidList = new List<int>();
            foreach (Document doc in docList)
            { 
                if (TypeConverter.ObjectToInt(doc["posterid"]) == 0)
                    continue;

                uidList.Add(TypeConverter.ObjectToInt(doc["posterid"]));
            }

            System.Collections.Generic.List<Document> userList = MongoDbHelper.Find(Users.mongoDB, "users", new Document().Add("_id", Op.In(uidList.ToArray())));
            Predicate<Document> matchUser = null;
            foreach (Document doc in docList)
            {
                //当帖子中的posterid字段为0时, 表示该数据出现异常
                if (doc == null || TypeConverter.ObjectToInt(doc["posterid"]) == 0)
                    continue;

                matchUser = new Predicate<Document>
                       (
                           delegate(Document userDoc)
                           {
                               return userDoc["_id"].ToString() == doc["posterid"].ToString(); //移除本地站点链接，因为当前站点缓存已被移除。
                           }
                       );
                ShowtopicPagePostInfo postInfo = LoadSingleShowtopicPagePostInfo(doc, userList.Find(matchUser));
                //扩展属性
                id++;
                postInfo.Id = id;
                postList.Add(postInfo);
            }
            return postList;
        }
        

        /// <summary>
        /// 屏蔽帖子内容
        /// </summary>
        /// <param name="tableId">分表Id</param>
        /// <param name="postList">帖子Id列表</param>
        /// <param name="invisible">屏蔽还是解除屏蔽</param>
        public void SetPostsBanned(string postTableId, string postList, int invisible)
        {
            MongoDbHelper.Update(mongoDB, "posts" + postTableId,
                 new Document() { { "$set", new Document() { { "invisible", invisible } } } },
                 new Document().Add("pid", Op.In(TypeConverter.StringToIntArray(postList))));
        }

           

        /// <summary>
        /// 通过待验证的帖子
        /// </summary>
        /// <param name="postTableId">帖子分表Id</param>
        /// <param name="validatePidList">需要验证的帖子Id列表</param>
        /// <param name="deletePidList">需要删除的帖子Id列表</param>
        /// <param name="ignorePidList">需要忽略的帖子ID列表</param>
        /// <param name="fidList">版块Id列表</param>
        public void PassPost(int postTableId, string validatePidList, string deletePidList, string ignorePidList, string fidList)
        {
            if (!Utils.StrIsNullOrEmpty(validatePidList))
                MongoDbHelper.Update(mongoDB, "posts" + postTableId,
                  new Document() { { "$set", new Document() { { "invisible", 0 } } } },
                  new Document().Add("pid", Op.In(TypeConverter.StringToIntArray(validatePidList)).Add("fid", Op.In(TypeConverter.StringToIntArray(fidList)))));

            if (!Utils.StrIsNullOrEmpty(deletePidList))
                MongoDbHelper.Delete(mongoDB, "posts" + postTableId,
                  new Document().Add("pid", Op.In(TypeConverter.StringToIntArray(deletePidList)).Add("fid", Op.In(TypeConverter.StringToIntArray(fidList)))));


            if (!Utils.StrIsNullOrEmpty(ignorePidList))
                MongoDbHelper.Update(mongoDB, "posts" + postTableId,
                  new Document() { { "$set", new Document() { { "invisible", -3 } } } },
                  new Document().Add("pid", Op.In(TypeConverter.StringToIntArray(ignorePidList)).Add("fid", Op.In(TypeConverter.StringToIntArray(fidList)))));
        }

        /// <summary>
        /// 删除指定用户的帖子
        /// </summary>
        /// <param name="postTableId">帖子分表Id</param>
        /// <param name="uid">用户ID</param>
        public void DeletePostByPosterid(int postTableId, int uid)
        {
            MongoDbHelper.Delete(mongoDB, "posts" + postTableId, new Document().Add("posterid", uid));
        }

        /// <summary>
        /// 更新帖子作者名称
        /// </summary>
        /// <param name="uid">要更新的帖子作者的Uid</param>
        /// <param name="newUserName">作者的新用户名</param>
        public void UpdatePostPoster(int uid, string newUserName)
        {
            DataTable dt = Discuz.Data.PostTables.GetAllPostTableName();
            foreach (DataRow dr in dt.Rows)
            {
                MongoDbHelper.Update(mongoDB, "posts" + dr["id"],
                      new Document() { { "$set", new Document() { { "poster", newUserName } } } },
                      new Document().Add("posterid", uid));
            }           
        }

     
        /// <summary>
        /// 删除指定主题的帖子
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postTableName">帖子分表</param>
        /// <returns></returns>
        public void DeleteTopicByTid(int tid, string postTableName)
        {
            MongoDbHelper.Delete(mongoDB, postTableName, new Document().Add("tid", tid));
        }


       
        /// <summary>
        /// 通过未审核的帖子
        /// </summary>
        /// <param name="postTableId">当前表ID</param>
        /// <param name="pidlist">帖子ID列表</param>
        public void PassPost(int postTableId, string pidlist)
        {
            MongoDbHelper.Update(mongoDB, "posts" + postTableId, 
                   new Document() { { "$set", new Document() { { "invisible", 0 } } } },
                   new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(pidlist))));
        }

        #region Private Method

        /// <summary>
        /// 装载ShowtopicPagePostInfo对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static ShowtopicPagePostInfo LoadSingleShowtopicPagePostInfo(Document doc , Document userDoc)
        {
            ShowtopicPagePostInfo showtopicPagePostInfo = new ShowtopicPagePostInfo();
            showtopicPagePostInfo.Pid = TypeConverter.ObjectToInt(doc["pid"]);
            showtopicPagePostInfo.Attachment = TypeConverter.ObjectToInt(doc["attachment"]);
            showtopicPagePostInfo.Fid = TypeConverter.ObjectToInt(doc["fid"]);
            //showtopicPagePostInfo.["tid"] = TypeConverter.ObjectToInt(doc["tid"]);
            showtopicPagePostInfo.Title = doc["title"].ToString().Trim();
            showtopicPagePostInfo.Layer = TypeConverter.ObjectToInt(doc["layer"]);
            showtopicPagePostInfo.Message = doc["message"].ToString().Trim();
            showtopicPagePostInfo.Lastedit = doc["lastedit"].ToString().Trim();
            showtopicPagePostInfo.Postdatetime = doc["postdatetime"].ToString().Trim();
            showtopicPagePostInfo.Poster = doc["poster"].ToString().Trim();
            showtopicPagePostInfo.Posterid = TypeConverter.ObjectToInt(doc["posterid"]);
            showtopicPagePostInfo.Invisible = TypeConverter.ObjectToInt(doc["invisible"]);
            showtopicPagePostInfo.Usesig = TypeConverter.ObjectToInt(doc["usesig"]);
            showtopicPagePostInfo.Htmlon = TypeConverter.ObjectToInt(doc["htmlon"]);
            showtopicPagePostInfo.Smileyoff = TypeConverter.ObjectToInt(doc["smileyoff"]);
            showtopicPagePostInfo.Parseurloff = TypeConverter.ObjectToInt(doc["parseurloff"]);
            showtopicPagePostInfo.Bbcodeoff = TypeConverter.ObjectToInt(doc["bbcodeoff"]);
            showtopicPagePostInfo.Rate = TypeConverter.ObjectToInt(doc["rate"]);
            showtopicPagePostInfo.Ratetimes = TypeConverter.ObjectToInt(doc["ratetimes"]);
            showtopicPagePostInfo.Ubbmessage = doc["message"].ToString().Trim();
            //作者ID为-1即表明作者为游客, 为了区分会直接公开显示游客发帖时的IP, 这里将IP最后一位修改为*
            showtopicPagePostInfo.Ip = doc["ip"].ToString().Trim();

            if (showtopicPagePostInfo.Posterid > 0 && userDoc != null)
            {
                showtopicPagePostInfo.Oltime = userDoc["oltime"] == null ? "" : userDoc["oltime"].ToString().Trim();
                showtopicPagePostInfo.Lastvisit = userDoc["lastvisit"] == null ? "" : userDoc["lastvisit"].ToString().Trim();
                showtopicPagePostInfo.Nickname = userDoc["nickname"].ToString().Trim();
                showtopicPagePostInfo.Username = userDoc["username"].ToString().Trim();
                showtopicPagePostInfo.Groupid = TypeConverter.ObjectToInt(userDoc["groupid"]);
                showtopicPagePostInfo.Spaceid = TypeConverter.ObjectToInt(userDoc["spaceid"]);
                showtopicPagePostInfo.Gender = TypeConverter.ObjectToInt(userDoc["gender"], 2);
                showtopicPagePostInfo.Bday = userDoc["bday"] == null ? "" : userDoc["bday"].ToString().Trim();
                showtopicPagePostInfo.Showemail = TypeConverter.ObjectToInt(userDoc["showemail"]);
                showtopicPagePostInfo.Digestposts = TypeConverter.ObjectToInt(userDoc["digestposts"]);
                showtopicPagePostInfo.Credits = TypeConverter.ObjectToInt(userDoc["credits"]);
                showtopicPagePostInfo.Extcredits1 = TypeConverter.ObjectToFloat(userDoc["extcredits1"]);
                showtopicPagePostInfo.Extcredits2 = TypeConverter.ObjectToFloat(userDoc["extcredits2"]);
                showtopicPagePostInfo.Extcredits3 = TypeConverter.ObjectToFloat(userDoc["extcredits3"]);
                showtopicPagePostInfo.Extcredits4 = TypeConverter.ObjectToFloat(userDoc["extcredits4"]);
                showtopicPagePostInfo.Extcredits5 = TypeConverter.ObjectToFloat(userDoc["extcredits5"]);
                showtopicPagePostInfo.Extcredits6 = TypeConverter.ObjectToFloat(userDoc["extcredits6"]);
                showtopicPagePostInfo.Extcredits7 = TypeConverter.ObjectToFloat(userDoc["extcredits7"]);
                showtopicPagePostInfo.Extcredits8 = TypeConverter.ObjectToFloat(userDoc["extcredits8"]);
                showtopicPagePostInfo.Posts = TypeConverter.ObjectToInt(userDoc["posts"]);
                showtopicPagePostInfo.Joindate = userDoc["joindate"].ToString().Trim();
                showtopicPagePostInfo.Lastactivity = userDoc["lastactivity"].ToString().Trim();
                showtopicPagePostInfo.Userinvisible = TypeConverter.ObjectToInt(userDoc["invisible"].ToString());
                showtopicPagePostInfo.Avatar = userDoc["avatar"].ToString();
                showtopicPagePostInfo.Avatarwidth = TypeConverter.ObjectToInt(userDoc["avatarwidth"]);
                showtopicPagePostInfo.Avatarheight = TypeConverter.ObjectToInt(userDoc["avatarheight"]);
                showtopicPagePostInfo.Medals = userDoc["medals"].ToString();
                showtopicPagePostInfo.Signature = userDoc["sightml"].ToString();//此处用于转换字段用于前台签名显示
                showtopicPagePostInfo.Location = userDoc["location"].ToString();
                showtopicPagePostInfo.Customstatus = userDoc["customstatus"].ToString();
                showtopicPagePostInfo.Website = userDoc["website"].ToString();
                showtopicPagePostInfo.Icq = userDoc["icq"].ToString();
                showtopicPagePostInfo.Qq = userDoc["qq"].ToString();
                showtopicPagePostInfo.Msn = userDoc["msn"].ToString();
                showtopicPagePostInfo.Yahoo = userDoc["yahoo"].ToString();
                showtopicPagePostInfo.Skype = userDoc["skype"].ToString();

                //部分属性需要根据不同情况来赋值

                //根据用户自己的设置决定是否显示邮箱地址
                if (showtopicPagePostInfo.Showemail == 0)
                    showtopicPagePostInfo.Email = "";
                else
                    showtopicPagePostInfo.Email = userDoc["email"].ToString().Trim();

                // 最后活动时间50分钟内的为在线, 否则不在线
                if (Utils.StrDateDiffMinutes(showtopicPagePostInfo.Lastactivity, 50) < 0)
                    showtopicPagePostInfo.Onlinestate = 1;
                else
                    showtopicPagePostInfo.Onlinestate = 0;             
            }
            else
            {
                showtopicPagePostInfo.Oltime = "";
                showtopicPagePostInfo.Lastvisit = "";
                showtopicPagePostInfo.Nickname = "游客";
                showtopicPagePostInfo.Username = "游客";
                showtopicPagePostInfo.Groupid = 7;              
                showtopicPagePostInfo.Bday = "";
                showtopicPagePostInfo.Showemail = 0;
                showtopicPagePostInfo.Digestposts = 0;
                showtopicPagePostInfo.Credits = 0;
                showtopicPagePostInfo.Extcredits1 = 0;
                showtopicPagePostInfo.Extcredits2 = 0;
                showtopicPagePostInfo.Extcredits3 = 0;
                showtopicPagePostInfo.Extcredits4 = 0;
                showtopicPagePostInfo.Extcredits5 = 0;
                showtopicPagePostInfo.Extcredits6 = 0;
                showtopicPagePostInfo.Extcredits7 = 0;
                showtopicPagePostInfo.Extcredits8 = 0;
                showtopicPagePostInfo.Posts = 0;
                showtopicPagePostInfo.Joindate = "2006-9-1 1:1:1";
                showtopicPagePostInfo.Lastactivity = "2006-9-1 1:1:1"; ;
                showtopicPagePostInfo.Userinvisible = 0;
                showtopicPagePostInfo.Avatar = "";
                showtopicPagePostInfo.Avatarwidth = 0;
                showtopicPagePostInfo.Avatarheight = 0;
                showtopicPagePostInfo.Medals = "";
                showtopicPagePostInfo.Signature = "";
                showtopicPagePostInfo.Location = "";
                showtopicPagePostInfo.Customstatus = "";
                showtopicPagePostInfo.Website = "";
                showtopicPagePostInfo.Icq = "";
                showtopicPagePostInfo.Qq = "";
                showtopicPagePostInfo.Msn = "";
                showtopicPagePostInfo.Yahoo = "";
                showtopicPagePostInfo.Skype = "";
                //部分属性需要根据不同情况来赋值
                showtopicPagePostInfo.Email = "";
                showtopicPagePostInfo.Onlinestate = 1;
                showtopicPagePostInfo.Medals = "";

                if (showtopicPagePostInfo.Ip.IndexOf('.') > -1)
                {
                    showtopicPagePostInfo.Ip = showtopicPagePostInfo.Ip.Substring(0, showtopicPagePostInfo.Ip.LastIndexOf(".") + 1) + "*";
                }
            }
            return showtopicPagePostInfo;
        }

        #endregion
    }
}
