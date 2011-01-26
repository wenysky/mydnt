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
    public class Topics : Discuz.Cache.Data.ICacheTopics
    {
        private static string connectString = null;

        private static DBCache mongoCache = EntLibConfigs.GetConfig().Cachetopics;

       
        static Topics()
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
        /// 创建新主题
        /// </summary>
        /// <param name="topicInfo">主题信息</param>
        /// <returns>返回主题ID</returns>
        public int CreateTopic(TopicInfo topicInfo)
        {
            if (topicInfo != null)
            {
                Document topicdoc = LoadTopic(topicInfo);
                MongoDbHelper.Insert(mongoDB, "topics", topicdoc);
                return topicInfo.Tid;
            }
            return -1;
        }

        /// <summary>
        /// 获得主题信息
        /// </summary>
        /// <param name="tid">要获得的主题ID</param>
        /// <param name="fid">版块ID</param>
        /// <param name="mode">模式选择, 0=当前主题, 1=上一主题, 2=下一主题</param>
        public TopicInfo GetTopicInfo(int tid, int fid, byte mode)
        {
            System.Collections.Generic.List<Document> docList;
            switch (mode)
            {
                case 1://上一主题
                    docList = MongoDbHelper.Find(mongoDB, "topics", 
                                       new Document().Add("fid", fid).
                                           Add("_id", Op.LessThan(tid)).
                                           Add("displayorder", Op.GreaterThanOrEqual(0)), 1, 0);
                    break;
                case 2://下一主题
                    docList = MongoDbHelper.Find(mongoDB, "topics", 
                                       new Document().Add("fid", fid).
                                           Add("_id", Op.GreaterThan(tid)).
                                           Add("displayorder", Op.GreaterThanOrEqual(0)), 1, 0);
                    break;
                default://当前主题
                    docList = MongoDbHelper.Find(mongoDB, "topics", new Document().Add("tid", tid), 1, 0);
                    break;
            }
            TopicInfo topicInfo = null;
            foreach (Document doc in docList)
            {
                topicInfo = LoadTopicInfo(doc);
                break;
            }
          
            return topicInfo;
        }


        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="topicInfo">主题信息</param>
        /// <returns>成功返回1，否则返回0</returns>
        public int UpdateTopic(TopicInfo topicInfo)
        {
            Document topicdoc = LoadTopic(topicInfo);
            MongoDbHelper.Update(mongoDB, "topics", topicdoc);
            return 1;
        }



        /// <summary>
        /// 列新主题的回复数
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postTableId">当前帖子分表Id</param>
        public void UpdateTopicReplyCount(int tid)
        {
            ResetTopicByTid(tid);
        }

        /// <summary>
        /// 更新主题为已被管理
        /// </summary>
        /// <param name="tidList">主题id列表</param>
        /// <param name="moderated">管理操作id</param>
        /// <returns>成功返回1，否则返回0</returns>
        public int UpdateTopicModerated(string tidList, int moderated)
        {
            MongoDbHelper.Update(mongoDB, "topics", 
                new Document() { { "$set", new Document() { { "moderated", moderated } } } }, 
                new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(tidList)))); 
            return 1;
        }

        //TODO:没用的方法，早晚给你拿下
        /// <summary>
        /// 将主题设置为隐藏主题
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns></returns>
        public int UpdateTopicHide(int tid)
        {
            MongoDbHelper.Update(mongoDB, "topics",
                     new Document() { { "$set", new Document() { { "hide", 1 } } } },
                     new Document().Add("_id", tid));            
            return 1;
        }

        ///// <summary>
        ///// 获取置顶主题列表(该方法还有优化空间，即直接使用mongodb排序)
        ///// </summary>
        ///// <param name="fid">版块id</param>
        ///// <param name="pageIndex">页号</param>
        ///// <param name="pageSize">每页条数</param>
        ///// <param name="tids">置顶主题Id列表</param>
        ///// <returns></returns>
        public Discuz.Common.Generic.List<TopicInfo> GetTopTopicList(int fid, int pageSize, int pageIndex, string tidList)
        {
            int skip = (pageIndex - 1) * pageSize;

            Discuz.Common.Generic.List<TopicInfo> list = new Discuz.Common.Generic.List<TopicInfo>();

            System.Collections.Generic.List<Document> docList = MongoDbHelper.Find(mongoDB, "topics",
                                     new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(tidList))).
                                                    Add("displayorder", Op.GreaterThan(0)));
            foreach (Document doc in docList)
            {
                list.Add(LoadTopicInfo(doc));
            }

            //#if NET4
            //            int workThreadNumber = System.Environment.ProcessorCount;
            //            var topList = (from Topic in list.AsParallel().WithDegreeOfParallelism(workThreadNumber)
            //                           orderby Topic.Displayorder descending, Topic.Lastpostid descending
            //                           select new { Topic }).Skip(skip).Take(pageSize).ToList();       
            //#else
            var topList = (from Topic in list
                           orderby Topic.Displayorder descending, Topic.Lastpostid descending
                           select new { Topic }).Skip(skip).Take(pageSize).ToList();
            //#endif

            Discuz.Common.Generic.List<TopicInfo> topTopicList = new Discuz.Common.Generic.List<TopicInfo>();
            foreach (var top in topList)
            {
                topTopicList.Add(top.Topic);
            }
            return topTopicList;
        }

        /// <summary>
        /// 获得一般主题信息列表
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <param name="pageSize">每页显示主题数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="startNumber">置顶帖数量</param>
        /// <param name="condition">条件</param>
        /// <returns>主题信息列表</returns>   
        public Discuz.Common.Generic.List<TopicInfo> GetTopicList(int fid, int pageSize, int pageIndex, int startNumber)
        {
            int skip = 0;
            if (pageIndex <= 1)
                pageSize = pageSize - startNumber;
            else
                skip = (pageIndex - 1) * pageSize - startNumber;

            Discuz.Common.Generic.List<TopicInfo> topicInfoList = new Common.Generic.List<TopicInfo>();
            System.Collections.Generic.List<Document> docList = MongoDbHelper.Find(mongoDB, "topics",
                           new Document().Add("fid", fid).Add("displayorder", 0), "lastpostid", IndexOrder.Descending, pageSize, skip);
            foreach (Document doc in docList)
            {
                topicInfoList.Add(LoadTopicInfo(doc));
            }
            return topicInfoList;
        }
        

        /// <summary>
        /// 通过待验证的主题
        /// </summary>
        /// <param name="postTableId">当前帖子分表Id</param>
        /// <param name="tid">主题Id</param>
        public void PassAuditNewTopic(int tid)
        {
            if (tid > 0)
            {
                MongoDbHelper.Update(mongoDB, "topics",
                    new Document() { { "$set", new Document() { { "displayorder", 0 } } } },
                    new Document().Add("_id", tid));
            }
        }


        /// <summary>
        /// 通过待验证的主题
        /// </summary>
        /// <param name="postTableId">当前帖子分表Id</param>
        /// <param name="tid">主题Id</param>
        public void PassAuditNewTopic(string tidList)
        {
            foreach (string tid in tidList.Split(','))
            {
                PassAuditNewTopic(TypeConverter.StrToInt(tid, 0));
            }
        }

        /// <summary>
        /// 通过待验证的主题
        /// </summary>
        /// <param name="ignoreTidList">忽略的主题列表</param>
        /// <param name="validateTidList">需要验证的主题列表</param>
        /// <param name="deleteTidList">删除的主题列表</param>
        /// <param name="fidList">版块列表</param>
        public void PassAuditNewTopic(string ignoreTidList, string validateTidList, string deleteTidList, string fidList)
        {
            if (!Utils.StrIsNullOrEmpty(validateTidList))
            {  
                MongoDbHelper.Update(mongoDB, "topics",
                    new Document() { { "$set", new Document() { { "displayorder", 0 } } } },
                    new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(validateTidList))));
            }

            if (!Utils.StrIsNullOrEmpty(deleteTidList))
                MongoDbHelper.Delete(mongoDB, "topics",
                    new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(deleteTidList))));

            if (!Utils.StrIsNullOrEmpty(ignoreTidList))
            {
                MongoDbHelper.Update(mongoDB, "topics",
                   new Document() { { "$set", new Document() { { "displayorder", -3 } } } },
                   new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(ignoreTidList))));
            }
        }

        /// <summary>
        /// 批量更新关注列表
        /// </summary>
        /// <param name="tidList">主题列表</param>
        /// <param name="attention">关注/取消关注(1/0)</param>
        public void UpdateTopicAttentionByTidList(string tidList, int attention)
        {
            MongoDbHelper.Update(mongoDB, "topics",
                  new Document() { { "$set", new Document() { { "attention", attention } } } },
                  new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(tidList))));
        }

        /// <summary>
        /// 批量更新关注列表
        /// </summary>
        /// <param name="fidList">版块列表</param>
        /// <param name="attention">关注/取消关注(1/0)</param>
        /// <param name="datetime">时间</param>
        public void UpdateTopicAttentionByFidList(string fidList, int attention, string datetime)
        {
            if (!Utils.IsNumericList(fidList))
                return;
            int postdatetime = UnixDateTimeHelper.ConvertToUnixTimestamp(TypeConverter.StrToDateTime(datetime));
            if (fidList != "0")
            {
                MongoDbHelper.Update(mongoDB, "topics",
                               new Document() { { "$set", new Document() { { "attention", attention } } } },
                               new Document().Add("fid", Op.In(TypeConverter.StringToIntArray(fidList))).
                                              Add("postdatetime", Op.LessThan(postdatetime)));
            }
            else
            {
                MongoDbHelper.Update(mongoDB, "topics",
                    new Document() { { "$set", new Document() { { "attention", attention } } } },
                     new Document().Add("postdatetime", Op.LessThan(postdatetime)));
            }
        }

        /// <summary>
        /// 更新主题附件类型
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <param name="attType">附件类型,1普通附件,2为图片附件</param>
        /// <returns></returns>
        public int UpdateTopicAttachmentType(int tid, int attType)
        {
            MongoDbHelper.Update(mongoDB, "topics",
                   new Document() { { "$set", new Document() { { "attachment", attType } } } },
                    new Document().Add("_id", tid));        
            return 1;
        }

        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="topicList">要更新的主题id列表</param>
        /// <param name="fid">版块id</param>
        /// <param name="topicType">要绑定的主题类型</param>
        /// <returns></returns>
        public int UpdateTopic(string topicList, int fid, int topicType)
        {
            MongoDbHelper.Update(mongoDB, "topics",
                 new Document() { { "$set", new Document() { { "fid", fid }, { "typeid", topicType } } } },
                   new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(topicList)))) ;
            return topicList.Split(',').Length;
        }

        /// <summary>
        /// 删除关闭的主题
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <param name="topicList">要更新的主题id列表</param>
        /// <returns></returns>
        public int UpdateTopic(int fid, string topicList)
        {
            MongoDbHelper.Delete(mongoDB, "topics",
                 new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(topicList)).Add("fid", fid)));

            return topicList.Split(',').Length;
        }

        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public int DeleteTopic(int tid)
        {
            MongoDbHelper.Delete(mongoDB, "topics",
                new Document().Add("_id", tid));
            return 1;
        }


        /// <summary>
        /// 更新主题最后发帖人Id
        /// </summary>
        /// <param name="tid"></param>
        public void UpdateTopicLastPosterId(int tid)
        {
            ResetTopicByTid(tid);
        }


        public void DeleteTopicByPosterid(int uid)
        {
            MongoDbHelper.Delete(mongoDB, "topics",
               new Document().Add("posterid", uid));
        }


        /// <summary>
        /// 清除主题分类
        /// </summary>
        /// <param name="typeid">主题分类Id</param>
        public void ClearTopicType(int typeid)
        {
            MongoDbHelper.Update(mongoDB, "topics",
                 new Document() { { "$set", new Document() { { "typeid", 0 } } } },
                 new Document().Add("typeid", typeid));           
        }


        /// <summary>
        /// 设置主题分类
        /// </summary>
        /// <param name="topicList">主题ID列表</param>
        /// <param name="value">分类ID</param>
        /// <returns></returns>
        public bool SetTypeid(string topicList, int value)
        {
            MongoDbHelper.Update(mongoDB, "topics",
                new Document() { { "$set", new Document() { { "typeid", value } } } },
                new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(topicList))));        
            return true;
        }

        /// <summary>
        /// 设置主题属性
        /// </summary>
        /// <param name="topicList">主题ID列表</param>
        /// <param name="value">主题属性</param>
        /// <returns></returns>
        public bool SetDisplayorder(string topicList, int value)
        {
            MongoDbHelper.Update(mongoDB, "topics",
                new Document() { { "$set", new Document() { { "displayorder", value } } } },
                new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(topicList))));
            return true;
        }

        /// <summary>
        /// 更新主题最后回复人
        /// </summary>
        /// <param name="uid">最后回复人的Uid</param>
        /// <param name="newUserName">最后回复人的新用户名</param>
        public void UpdateTopicLastPoster(int uid, string newUserName)
        {
            MongoDbHelper.Update(mongoDB, "topics",
               new Document() { { "$set", new Document() { { "lastposter", newUserName } } } },
               new Document().Add("lastposterid", uid));
        }

        /// <summary>
        /// 更新主题作者
        /// </summary>
        /// <param name="posterid">作者Id</param>
        /// <param name="poster">作者的新名称</param>
        public void UpdateTopicPoster(int posterid, string poster)
        {
            MongoDbHelper.Update(mongoDB, "topics",
                 new Document() { { "$set", new Document() { { "poster", poster } } } },
                 new Document().Add("posterid", posterid));
        }


        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postcount">帖子数</param>
        /// <param name="lastpostid">最后发帖人</param>
        /// <param name="lastpost">最后发帖时间</param>
        /// <param name="lastposterid">最后发帖人ID</param>
        /// <param name="poster">最后发帖人</param>
        public void UpdateTopic(int tid, int postcount, int lastpostid, string lastpost, int lastposterid, string poster)
        {
            MongoDbHelper.Update(mongoDB, "topics",
                new Document() { { "$set", new Document() { { "lastpost", lastpost }, { "lastposterid", lastposterid }, { "lastposter", poster }, { "replies", postcount }, { "lastpostid", lastpostid } } } },
                new Document().Add("_id", tid));
        }

        /// <summary>
        /// 得到论坛中主题总数;
        /// </summary>
        /// <returns>主题总数</returns>
        public int GetTopicCount()
        {
            return (int)MongoDbHelper.Count(mongoDB, "topics", null);
        }

        /// <summary>
        /// 重置缓存数据（从数据库中获取主题信息并更新到MongoDB中）
        /// </summary>
        /// <param name="tid"></param>
        public void ResetTopicByTid(int tid)
        {
            if (tid > 0)
            {
                TopicInfo topicInfo  = LoadSingleTopicInfo(Discuz.Data.DatabaseProvider.GetInstance().GetTopicInfo(tid, 0, 0));
                MongoDbHelper.Delete(mongoDB, "topics", new Document().Add("_id", tid));
                MongoDbHelper.Insert(mongoDB, "topics", LoadTopic(topicInfo));
            }
        }

        /// <summary>
        /// 清除主题里面已经移走的主题
        /// </summary>
        public void ReSetClearMove()
        {
            MongoDbHelper.Delete(mongoDB, "topics", new Document().Add("closed", new Document().Add("$gt", 1)));           
        }

        /// <summary>
        /// 删除版块
        /// </summary>
        public void DeleteForumTopic(int fid)
        {
            MongoDbHelper.Delete(mongoDB, "topics", new Document().Add("fid", fid));    
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="delPosts">是否删除帖子</param>
        public void DeleteUserTopic(int uid, bool delPosts)
        {
            if (delPosts)
                MongoDbHelper.Delete(mongoDB, "topics", new Document().Add("posterid", uid));
            else
                MongoDbHelper.Update(mongoDB, "topics",
                    new Document() { { "$set", new Document() { { "poster", "该用户已被删除" } } } },
                    new Document().Add("posterid", uid));

            if (!delPosts)
                MongoDbHelper.Update(mongoDB, "topics",
                       new Document() { { "$set", new Document() { { "lastposter", "该用户已被删除" } } } },
                       new Document().Add("lastpostid", uid));
        }


        /// <summary>
        /// 通过未审核的帖子
        /// </summary>
        /// <param name="currentPostTableId">当前表ID</param>
        /// <param name="pidlist">帖子ID列表</param>
        public void PassPostTopic(int currentPostTableId, string pidlist)
        {
            if (Utils.IsNumericList(pidlist))
            {
                DataTable dt = Discuz.Data.DatabaseProvider.GetInstance().GetPostList(currentPostTableId.ToString(), pidlist);
                foreach (DataRow dr in dt.Rows)
                {
                    ResetTopicByTid(TypeConverter.ObjectToInt(dr["tid"]));
                }
            }
        }

        /// <summary>
        /// 根据分表名更新主题的最后回复等信息,该方法已不再适合mongodb的业务逻辑，它需要在后台去更新主题表的相应信息
        /// </summary>
        /// <param name="postTableID">当前表ID</param>
        public void ResetLastRepliesInfoOfTopics(int postTableID)
        {
            //找出当前分表中所有帖子的主题TID，并更新它们
            IDataReader dataReader = Discuz.Data.DbHelper.ExecuteReader(CommandType.Text, "SELECT " + DbFields.TOPICS + " FROM [" + BaseConfigs.GetTablePrefix + "topics] Where tid in (Select Distinct(tid) from [" + BaseConfigs.GetTablePrefix + "posts" + postTableID + "])");
            while (dataReader.Read())
            {
                MongoDbHelper.Update(mongoDB, "topics", LoadTopic(LoadSingleTopicInfo(dataReader)));
            }
            dataReader.Close();
        }

        #region 注:下面方法来自TopicAdmins
        /// <summary>
        /// 设置主题指定字段的属性值(字符型)
        /// </summary>
        /// <param name="topicList">要设置的主题列表</param>
        /// <param name="field">要设置的字段</param>
        /// <param name="intValue">属性值</param>
        /// <returns>更新主题个数</returns>
        public int SetTopicStatus(string topicList, string field, string intValue)
        {
            if(field =="highlight")
              MongoDbHelper.Update(mongoDB, "topics",
                    new Document() { { "$set", new Document() { { field, intValue } } } },
                  new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(topicList))));
            else
              MongoDbHelper.Update(mongoDB, "topics",
                   new Document() { { "$set", new Document() { { field, TypeConverter.StrToInt(intValue, 0) } } } },
                 new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(topicList))));

            return topicList.Split(',').Length;
        }

        /// <summary>
        /// 将主题设置关闭/打开
        /// </summary>
        /// <param name="topicList">要设置的主题列表</param>
        /// <param name="intValue">关闭/打开标志( 0 为打开,1 为关闭)</param>
        /// <returns>更新主题个数</returns>
        public int SetTopicClose(string topicList, short intValue)
        {
            MongoDbHelper.Update(mongoDB, "topics",
                new Document() { { "$set", new Document() { { "closed", (int)intValue } } } },
                new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(topicList))).
                               Add("closed", Op.In(new int[] { 0, 1 })));
            return topicList.Split(',').Length;
        }

        /// <summary>
        /// 删除指定主题
        /// </summary>
        /// <param name="topiclist">要删除的主题ID列表</param>
        /// <returns></returns>
        public int DeleteTopicByTidList(string topicList)
        {
            MongoDbHelper.Delete(mongoDB, "topics",
                          new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(topicList))));
            MongoDbHelper.Delete(mongoDB, "topics",
                          new Document().Add("closed", Op.In(TypeConverter.StringToIntArray(topicList))));        
            return topicList.Split(',').Length;
        }

        /// <summary>
        /// 复制主题链接,因为该操作无效获取新增主题ID信息，所以只能变通，使用获取新增主题之前的最大TID来批量更新MongoDB信息
        /// </summary>
        /// <param name="oldfid"></param>
        /// <param name="topicList"></param>
        /// <returns></returns>
        public int CopyTopicLink(int maxtid)
        {
            IDataReader dataReader = Discuz.Data.DbHelper.ExecuteReader(CommandType.Text, "Select * From [" + BaseConfigs.GetTablePrefix + "topics] WHERE [tid]>" + maxtid);
            while (dataReader.Read())
            {
                TopicInfo topicInfo = LoadSingleTopicInfo(dataReader);
                MongoDbHelper.Update(mongoDB, "topics", LoadTopic(topicInfo));
            }
            dataReader.Close();
            return 1;
        }

        /// <summary>
        /// 修复主题
        /// </summary>
        /// <param name="topiclist"></param>
        /// <param name="posttable"></param>
        /// <returns></returns>
        public int RepairTopics(string topicList)
        {
            foreach (string tid in topicList.Split(','))
            {
                TopicInfo topicInfo = LoadSingleTopicInfo(Discuz.Data.DatabaseProvider.GetInstance().GetTopicInfo(TypeConverter.StrToInt(tid), 0, 0));
                if (topicInfo != null)
                {
                    var column = LoadTopic(topicInfo);
                    MongoDbHelper.Update(mongoDB, "topics", LoadTopic(topicInfo));
                }
            }
            return topicList.Split(',').Length;
        }


        /// <summary>
        /// 重设主题类型
        /// </summary>
        /// <param name="topictypeid">主题类型</param>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <returns></returns>
        public int ResetTopicTypes(int topictypeid, string topiclist)
        {
            SetTypeid(topiclist, topictypeid);
            return 1;
        }


        /// <summary>
        /// 设置主题鉴定信息
        /// </summary>
        /// <param name="topicList"></param>
        /// <param name="identify"></param>
        public void IdentifyTopic(string topicList, int identify)
        {
            MongoDbHelper.Update(mongoDB, "topics",
               new Document() { { "$set", new Document() { { "identify", identify } } } },
               new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(topicList))));
        }

        /// <summary>
        /// 设置主题的下沉和提升
        /// </summary>
        /// <param name="tidList"></param>
        /// <param name="lastpostid"></param>
        public void SetTopicsBump(string tidList, int lastpostid)
        {
            System.Collections.Generic.List<Document> docList = MongoDbHelper.Find(mongoDB, "topics", new Document().Add("_id", new Document().Add("$in", TypeConverter.StringToIntArray(tidList))));
            foreach (Document doc in docList)
            {
                if (lastpostid != 0)
                    doc["lastpostid"] = lastpostid;
                else
                    doc["lastpostid"] = 0 - TypeConverter.ObjectToInt(doc["lastpostid"]);
                MongoDbHelper.Update(mongoDB, "topics", doc);
            }
        }

        /// <summary>
        /// 删除关闭主题
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="topicList"></param>
        public void DeleteClosedTopics(int fid, string topicList)
        {
            MongoDbHelper.Delete(mongoDB, "topics", 
                    new Document().Add("fid", fid).
                                   Add("closed", Op.In(TypeConverter.StringToIntArray(topicList))).
                                   Add("_id", Op.In(TypeConverter.StringToIntArray(topicList))));

        }
        #endregion


        #region 获取主题信息
        private TopicInfo LoadTopicInfo(Document doc)
        {
            TopicInfo topicinfo = new TopicInfo();
            topicinfo.Tid = TypeConverter.ObjectToInt(doc["tid"]);
            topicinfo.Fid = TypeConverter.ObjectToInt(doc["fid"]);
            topicinfo.Iconid = TypeConverter.ObjectToInt(doc["iconid"]);
            topicinfo.Typeid = TypeConverter.ObjectToInt(doc["typeid"]);
            topicinfo.Readperm = TypeConverter.ObjectToInt(doc["readperm"]);
            topicinfo.Price = TypeConverter.ObjectToInt(doc["price"]);
            topicinfo.Poster = doc["poster"].ToString();
            topicinfo.Posterid = TypeConverter.ObjectToInt(doc["posterid"]);
            topicinfo.Title = doc["title"].ToString();
            topicinfo.Attention = TypeConverter.ObjectToInt(doc["attention"]);
            topicinfo.Postdatetime = UnixDateTimeHelper.ConvertFromUnixTimestamp(TypeConverter.ObjectToInt(doc["postdatetime"])).ToString();
            topicinfo.Lastpost = UnixDateTimeHelper.ConvertFromUnixTimestamp(TypeConverter.ObjectToInt(doc["lastpost"])).ToString();
            topicinfo.Lastpostid = TypeConverter.ObjectToInt(doc["lastpostid"]);
            topicinfo.Lastposter = doc["lastposter"].ToString();
            topicinfo.Lastposterid = TypeConverter.ObjectToInt(doc["lastposterid"]);
            topicinfo.Views = TypeConverter.ObjectToInt(doc["views"]);
            topicinfo.Replies = TypeConverter.ObjectToInt(doc["replies"]);
            topicinfo.Displayorder = TypeConverter.ObjectToInt(doc["displayorder"]);
            topicinfo.Highlight = doc["highlight"].ToString();
            topicinfo.Digest = TypeConverter.ObjectToInt(doc["digest"]);
            topicinfo.Rate = TypeConverter.ObjectToInt(doc["rate"]);
            topicinfo.Hide = TypeConverter.ObjectToInt(doc["hide"]);
            topicinfo.Attachment = TypeConverter.ObjectToInt(doc["attachment"]);
            topicinfo.Moderated = TypeConverter.ObjectToInt(doc["moderated"]);
            topicinfo.Closed = TypeConverter.ObjectToInt(doc["closed"]);
            topicinfo.Magic = TypeConverter.ObjectToInt(doc["magic"]);
            topicinfo.Identify = TypeConverter.ObjectToInt(doc["identify"]);
            topicinfo.Special = (byte)TypeConverter.ObjectToInt(doc["special"]);
            return topicinfo;
        }

        private Document LoadTopic(TopicInfo topicinfo)
        {
            Document topicdoc = new Document();
            topicdoc["_id"] = topicinfo.Tid;
            topicdoc["tid"] = topicinfo.Tid;
            topicdoc["fid"] = topicinfo.Fid;
            topicdoc["iconid"] = topicinfo.Iconid;
            topicdoc["typeid"] = topicinfo.Typeid;
            topicdoc["readperm"] = topicinfo.Readperm;
            topicdoc["price"] = topicinfo.Price;
            topicdoc["poster"] = topicinfo.Poster;
            topicdoc["posterid"] = topicinfo.Posterid;
            topicdoc["title"] = topicinfo.Title;
            topicdoc["attention"] = topicinfo.Attention;
            topicdoc["postdatetime"] = UnixDateTimeHelper.ConvertToUnixTimestamp(TypeConverter.StrToDateTime(topicinfo.Postdatetime));
            topicdoc["lastpost"] =  UnixDateTimeHelper.ConvertToUnixTimestamp(TypeConverter.StrToDateTime(topicinfo.Lastpost));       
            topicdoc["lastpostid"] = topicinfo.Lastpostid;
            topicdoc["lastposter"] = topicinfo.Lastposter;
            topicdoc["lastposterid"] = topicinfo.Lastposterid;
            topicdoc["views"] = topicinfo.Views;
            topicdoc["replies"] = topicinfo.Replies;
            topicdoc["displayorder"] = topicinfo.Displayorder;
            topicdoc["highlight"] = topicinfo.Highlight;
            topicdoc["digest"] = topicinfo.Digest;
            topicdoc["rate"] = topicinfo.Rate;
            topicdoc["hide"] = topicinfo.Hide;
            topicdoc["attachment"] = topicinfo.Attachment;
            topicdoc["moderated"] = topicinfo.Moderated;
            topicdoc["closed"] = topicinfo.Closed;
            topicdoc["magic"] = topicinfo.Magic;
            topicdoc["identify"] = topicinfo.Identify;
            topicdoc["special"] = (int)topicinfo.Special;
            return topicdoc;
        }

    
        /// <summary>
        /// 加载单个主题对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static TopicInfo LoadSingleTopicInfo(IDataReader reader)
        {          
            reader.Read();
            TopicInfo topicInfo = new TopicInfo();
            topicInfo.Tid = TypeConverter.ObjectToInt(reader["tid"]);
            topicInfo.Fid = TypeConverter.ObjectToInt(reader["fid"]);
            topicInfo.Iconid = TypeConverter.ObjectToInt(reader["iconid"]);
            topicInfo.Title = reader["title"].ToString();
            topicInfo.Typeid = TypeConverter.ObjectToInt(reader["typeid"]);
            topicInfo.Readperm = TypeConverter.ObjectToInt(reader["readperm"]);
            topicInfo.Price = TypeConverter.ObjectToInt(reader["price"]);
            topicInfo.Poster = reader["poster"].ToString();
            topicInfo.Posterid = TypeConverter.ObjectToInt(reader["posterid"]);
            topicInfo.Postdatetime = reader["postdatetime"].ToString();
            topicInfo.Lastpost = reader["lastpost"].ToString();
            topicInfo.Lastposter = reader["lastposter"].ToString();
            topicInfo.Lastposterid = TypeConverter.ObjectToInt(reader["LastposterID"]);
            topicInfo.Lastpostid = TypeConverter.ObjectToInt(reader["LastpostID"]);
            topicInfo.Views = TypeConverter.ObjectToInt(reader["views"]);
            topicInfo.Replies = TypeConverter.ObjectToInt(reader["replies"]);
            topicInfo.Displayorder = TypeConverter.ObjectToInt(reader["displayorder"]);
            topicInfo.Highlight = reader["highlight"].ToString();
            topicInfo.Digest = TypeConverter.ObjectToInt(reader["digest"]);
            topicInfo.Rate = TypeConverter.ObjectToInt(reader["rate"]);
            topicInfo.Hide = TypeConverter.ObjectToInt(reader["hide"]);
            topicInfo.Attachment = TypeConverter.ObjectToInt(reader["attachment"]);
            topicInfo.Moderated = TypeConverter.ObjectToInt(reader["moderated"]);
            topicInfo.Closed = TypeConverter.ObjectToInt(reader["closed"]);
            topicInfo.Magic = TypeConverter.ObjectToInt(reader["magic"]);
            topicInfo.Identify = TypeConverter.ObjectToInt(reader["identify"]);
            topicInfo.Special = byte.Parse(reader["special"].ToString());
            topicInfo.Attention = TypeConverter.ObjectToInt(reader["attention"]);
            reader.Close();
            return topicInfo;
        }       
        #endregion

    }
}
