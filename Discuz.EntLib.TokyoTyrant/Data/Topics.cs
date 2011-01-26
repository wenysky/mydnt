using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Config;
using Discuz.Common.Generic;
using Discuz.Common;
using Discuz.EntLib.TokyoTyrant;
using Discuz.Data;

#if NET4
//using System.Threading.Tasks;
#endif

namespace Discuz.EntLib.TokyoTyrant.Data
{
    public class Topics : Discuz.Cache.Data.ICacheTopics
    {
        private static TcpClientIOPool pool = TcpClientIOPool.GetInstance("dnt_topics");

        private static DBCache ttCache = EntLibConfigs.GetConfig().Cachetopics;

        static Topics()
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

            //TokyoTyrantService.SetIndex(pool, "tid", IndexOption.DECIMAL);
            //TokyoTyrantService.SetIndex(pool, "displayorder", IndexOption.DECIMAL);
            //TokyoTyrantService.SetIndex(pool, "fid", IndexOption.DECIMAL);
            //TokyoTyrantService.SetIndex(pool, "lastpostid", IndexOption.DECIMAL);
            //TokyoTyrantService.SetIndex(pool, "replies", IndexOption.DECIMAL);
            //TokyoTyrantService.SetIndex(pool, "views", IndexOption.DECIMAL);
            //TokyoTyrantService.SetIndex(pool, "lastpost", IndexOption.DECIMAL);
            //TokyoTyrantService.SetIndex(pool, "postdatetime", IndexOption.LEXICAL);
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
                IDictionary<string, string> topic = LoadTopic(topicInfo);
                TokyoTyrantService.PutColumns(pool, topic["tid"], topic, true);
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
            IDictionary<string, IDictionary<string, string>> qrecords;
            switch (mode)
            {
                case 1://上一主题
                    qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("fid",fid).NumberLessThan("tid", tid).NumberGreaterThanOrEqual("displayorder", 0).LimitTo(1, 0));
                    break;
                case 2://下一主题
                    qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("fid", fid).NumberGreaterThan("tid", tid).NumberGreaterThanOrEqual("displayorder", 0).LimitTo(1, 0));
                    break;
                default://当前主题
                    qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("tid", tid));
                    break;
            }
            TopicInfo topicInfo = null;
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                topicInfo = LoadTopicInfo(column);
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
            IDictionary<string, string> topic = LoadTopic(topicInfo);
            TokyoTyrantService.PutColumns(pool, topic["tid"], topic, true);
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
            var qrecords = TokyoTyrantService.GetColumns(pool, tidList.Split(','));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                column["moderated"] = moderated.ToString();
                TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
            }
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("tid", tid));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                column["hide"] = "1";
                TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
            }
            return 1;
        }

        ///// <summary>
        ///// 获取置顶主题列表
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
            var qrecords = TokyoTyrantService.GetColumns(pool, tidList.Split(','));
            foreach (string key in qrecords.Keys)
            {
                var columns = qrecords[key];
                if (TypeConverter.StrToInt(columns["displayorder"]) > 0)
                    list.Add(LoadTopicInfo(columns));
            }

//#if NET4
            //int workThreadNumber = System.Environment.ProcessorCount;
            //var topList = (from Topic in list.AsParallel().WithDegreeOfParallelism(workThreadNumber)
            //               orderby Topic.Displayorder descending, Topic.Lastpostid descending
            //               select new { Topic }).Skip(skip).Take(pageSize).ToList();       
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

        #region 下面注释方法因为比上面的方法效率低50%，所以被放弃了
        //public Discuz.Common.Generic.List<TopicInfo> GetTopTopicList(int fid, int pageSize, int pageIndex, string tidList)
        //{
        //    int skip = (pageIndex - 1) * pageSize;

        //    Discuz.Common.Generic.List<TopicInfo> list = new Discuz.Common.Generic.List<TopicInfo>();
        //    var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberGreaterThan("displayorder", 0));

        //    foreach (string key in qrecords.Keys)
        //    {
        //        list.Add(LoadTopicInfo(qrecords[key]));
        //    }

        //    IEnumerable<TopicInfo> topTopicList = (from topic in list
        //                        where tidList.Split(',').Contains(topic.Tid.ToString())
        //                        orderby topic.Displayorder descending, topic.Lastpostid descending
        //                               select new { topic }).Skip(skip).Take(pageSize).Cast<TopicInfo>();
        //    list.Clear();
        //    foreach (TopicInfo topic in topTopicList)
        //    {
        //        list.Add(topic);
        //    }            
        //    return list;
        //}
        #endregion

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
            string commandText = "";
            if (pageIndex <= 1)
                commandText = string.Format("SELECT TOP {0} [tid] FROM [{1}topics] WHERE [fid]={2} AND [displayorder]=0  ORDER BY [lastpostid] DESC",
                                                pageSize, BaseConfigs.GetTablePrefix, fid);
            else
                commandText = string.Format("SELECT TOP {0} [tid] FROM [{1}topics] WHERE [lastpostid] < (SELECT min([lastpostid])  FROM (SELECT TOP {2} [lastpostid] FROM [{1}topics] WHERE [fid]={3} AND [displayorder]=0 ORDER BY [lastpostid] DESC) AS tblTmp ) AND [fid]={3} AND [displayorder]=0 ORDER BY [lastpostid] DESC",
                                                pageSize, BaseConfigs.GetTablePrefix, (pageIndex - 1) * pageSize - startNumber, fid);

            Discuz.Common.Generic.List<TopicInfo> topicInfoList = new Common.Generic.List<TopicInfo>();
            IDataReader dataReader = DbHelper.ExecuteReader(CommandType.Text, commandText);
            Discuz.Common.Generic.List<string> tidList = new Discuz.Common.Generic.List<string>();
            while (dataReader.Read())
                tidList.Add(dataReader["tid"].ToString());
            dataReader.Close();

            var qrecords = TokyoTyrantService.GetColumns(pool, tidList.ToArray());
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                topicInfoList.Add(LoadTopicInfo(column));
            }
            return topicInfoList;
        }
        #region 下面注释方法因为比上面的方法效率低100%，所以被放弃了
        //public Discuz.Common.Generic.List<TopicInfo> GetTopicList(int fid, int pageSize, int pageIndex, int startNumber)
        //{
        //    int skip = 0;
        //    if(pageIndex <=1)
        //        pageSize = pageSize - startNumber;
        //    else
        //        skip = (pageIndex - 1) * pageSize - startNumber;

        //    Discuz.Common.Generic.List<TopicInfo> topicInfoList = new Common.Generic.List<TopicInfo>();
        //    var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("fid", fid).NumberEquals("displayorder", 0).OrderBy("lastpostid",QueryOrder.NUMDESC).LimitTo(pageSize, skip));
        //    foreach (string key in qrecords.Keys)
        //    {
        //        var column = qrecords[key];
        //        topicInfoList.Add(LoadTopicInfo(column));
        //    }
        //    return topicInfoList;
        //}
        #endregion

        /// <summary>
        /// 通过待验证的主题
        /// </summary>
        /// <param name="postTableId">当前帖子分表Id</param>
        /// <param name="tid">主题Id</param>
        public void PassAuditNewTopic(int tid)
        {
            if (tid > 0)
            {
                var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("tid", tid));
                foreach (string key in qrecords.Keys)
                {
                    var column = qrecords[key];
                    column["displayorder"] = "0";
                    TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
                    break;
                }
            }
        }


        /// <summary>
        /// 通过待验证的主题
        /// </summary>
        /// <param name="postTableId">当前帖子分表Id</param>
        /// <param name="tid">主题Id</param>
        public void PassAuditNewTopic(string tidList)
        {
            foreach(string tid in tidList.Split(','))
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
                var qrecords = TokyoTyrantService.GetColumns(pool, validateTidList.Split(','));
                foreach (string key in qrecords.Keys)
                {
                    var column = qrecords[key];
                    column["displayorder"] = "0";
                    TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
                    break;
                }
            }     

            if (!Utils.StrIsNullOrEmpty(deleteTidList))
                TokyoTyrantService.DeleteMultiple(pool, deleteTidList.Split(','));

            if (!Utils.StrIsNullOrEmpty(ignoreTidList))
            {
                var qrecords = TokyoTyrantService.GetColumns(pool, ignoreTidList.Split(','));
                foreach (string key in qrecords.Keys)
                {
                    var column = qrecords[key];
                    column["displayorder"] = "-3";
                    TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
                    break;
                }
            }
        }
       
        /// <summary>
        /// 批量更新关注列表
        /// </summary>
        /// <param name="tidList">主题列表</param>
        /// <param name="attention">关注/取消关注(1/0)</param>
        public void UpdateTopicAttentionByTidList(string tidList, int attention)
        {
            var qrecords = TokyoTyrantService.GetColumns(pool, tidList.Split(','));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                column["attention"] = attention.ToString();
                TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
                break;
            }
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
            IDictionary<string, IDictionary<string, string>> qrecords = null;
            if (fidList != "0")
            {
                foreach (string fid in fidList.Split(','))
                {
                    qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("fid", TypeConverter.StrToInt(fid)).NumberLessThan("postdatetime", postdatetime));
                    foreach (string key in qrecords.Keys)
                    {
                        var column = qrecords[key];
                        column["attention"] = attention.ToString();
                        TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
                        break;
                    }
                }
            }
            else
            {
                qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberLessThan("postdatetime", postdatetime));
                foreach (string key in qrecords.Keys)
                {
                    var column = qrecords[key];
                    column["attention"] = attention.ToString();
                    TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
                    break;
                }
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("tid", tid));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                column["attachment"] = attType.ToString();
                TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
                break;
            }
            return 1;
        }

        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="topicList">要更新的主题id列表</param>
        /// <param name="fid">版块id</param>
        /// <returns></returns>
        public int UpdateTopic(string topicList, int fid)
        {
            var qrecords = TokyoTyrantService.GetColumns(pool, topicList.Split(','));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                column["fid"] = fid.ToString();
                column["typeid"] = "0";
                TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
            }
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
            var qrecords = TokyoTyrantService.GetColumns(pool, topicList.Split(','));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                if(column["fid"] == fid.ToString())
                   TokyoTyrantService.Delete(pool, column["tid"]);
            }
            return topicList.Split(',').Length;
        }

        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public int DeleteTopic(int tid)
        {
            TokyoTyrantService.Delete(pool, tid.ToString());
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("posterid", uid));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                TokyoTyrantService.Delete(pool, column["tid"]);
            }
        }


        /// <summary>
        /// 清除主题分类
        /// </summary>
        /// <param name="typeid">主题分类Id</param>
        public void ClearTopicType(int typeid)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("typeid", typeid));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                column["typeid"] = "0";
                TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
            }
        }

        
        /// <summary>
        /// 设置主题分类
        /// </summary>
        /// <param name="topiclist">主题ID列表</param>
        /// <param name="value">分类ID</param>
        /// <returns></returns>
        public bool SetTypeid(string topiclist, int value)
        {
            var qrecords = TokyoTyrantService.GetColumns(pool, topiclist.Split(','));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                column["typeid"] = value.ToString();
                TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
            }
            return true;
        }

        /// <summary>
        /// 设置主题属性
        /// </summary>
        /// <param name="topiclist">主题ID列表</param>
        /// <param name="value">主题属性</param>
        /// <returns></returns>
        public bool SetDisplayorder(string topiclist, int value)
        {
            var qrecords = TokyoTyrantService.GetColumns(pool, topiclist.Split(','));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                column["displayorder"] = value.ToString();
                TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
            }
            return true;
        }

        /// <summary>
        /// 更新主题最后回复人
        /// </summary>
        /// <param name="uid">最后回复人的Uid</param>
        /// <param name="newUserName">最后回复人的新用户名</param>
        public void UpdateTopicLastPoster(int uid, string newUserName)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("lastposterid", uid));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                column["lastposter"] = newUserName;
                TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
            }
        }

        /// <summary>
        /// 更新主题作者
        /// </summary>
        /// <param name="posterid">作者Id</param>
        /// <param name="poster">作者的新名称</param>
        public void UpdateTopicPoster(int posterid, string poster)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("posterid", posterid));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                column["poster"] = poster;
                TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
            }
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
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("tid", tid));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                column["lastpost"] = lastpost;
                column["lastposterid"] = lastposterid.ToString();
                column["lastposter"] = poster;
                column["replies"] = postcount.ToString();
                column["lastpostid"] = lastpostid.ToString();
                TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
                break;
            }
        }

        /// <summary>
        /// 得到论坛中主题总数;
        /// </summary>
        /// <returns>主题总数</returns>
        public int GetTopicCount()
        {
            return (int)TokyoTyrantService.GetRecordCount(pool);
        }

        /// <summary>
        /// 重置缓存数据（从数据库中获取主题信息并更新到TTCACHE中）
        /// </summary>
        /// <param name="tid"></param>
        public void ResetTopicByTid(int tid)
        {
            if (tid > 0)
            {
                TopicInfo topicInfo = LoadSingleTopicInfo(Discuz.Data.DatabaseProvider.GetInstance().GetTopicInfo(tid, 0, 0));
                if (topicInfo != null)
                    TokyoTyrantService.PutColumns(pool, topicInfo.Tid.ToString(), LoadTopic(topicInfo), true);
                else
                    TokyoTyrantService.Delete(pool, topicInfo.Tid.ToString());
            }
        }

        /// <summary>
        /// 清除主题里面已经移走的主题
        /// </summary>
        public void ReSetClearMove()
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberGreaterThan("closed", 1));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                TokyoTyrantService.Delete(pool, column["tid"]);
            }
        }

        /// <summary>
        /// 删除版块
        /// </summary>
        public void DeleteForumTopic(int fid)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("fid", fid));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                TokyoTyrantService.Delete(pool, column["tid"]);
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="delPosts">是否删除帖子</param>
        public void DeleteUserTopic(int uid, bool delPosts)
        {
            var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("posterid", uid));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                if (delPosts)
                    TokyoTyrantService.Delete(pool, column["tid"]);
                else
                {
                    column["poster"] = "该用户已被删除";
                    TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
                }
            }
            if (!delPosts)
            {
                qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("lastpostid", uid));
                foreach (string key in qrecords.Keys)
                {
                    var column = qrecords[key];

                    column["lastposter"] = "该用户已被删除";
                    TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
                }
            }
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
        /// 根据分表名更新主题的最后回复等信息
        /// </summary>
        /// <param name="postTableID">当前表ID</param>
        public void ResetLastRepliesInfoOfTopics(int postTableID)
        {
            //找出当前分表中所有帖子的主题TID，并更新它们
            IDataReader dataReader = Discuz.Data.DbHelper.ExecuteReader(CommandType.Text, "SELECT " + DbFields.TOPICS + " FROM [" + BaseConfigs.GetTablePrefix + "topics] Where tid in (Select Distinct(tid) from [" + BaseConfigs.GetTablePrefix + "posts" + postTableID + "])");
            while (dataReader.Read())
            {
                var column = LoadTopic(LoadSingleTopicInfo(dataReader));
                TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
            }
            dataReader.Close();
        }

        #region 注:下面方法来自TopicAdmins
        /// <summary>
        /// 设置主题指定字段的属性值(字符型)
        /// </summary>
        /// <param name="topiclist">要设置的主题列表</param>
        /// <param name="field">要设置的字段</param>
        /// <param name="intValue">属性值</param>
        /// <returns>更新主题个数</returns>
        public int SetTopicStatus(string topiclist, string field, string intValue)
        {
            var qrecords = TokyoTyrantService.GetColumns(pool, topiclist.Split(','));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                column[field] = intValue;
                TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
            }
            return topiclist.Split(',').Length;
        }

        /// <summary>
        /// 将主题设置关闭/打开
        /// </summary>
        /// <param name="topicList">要设置的主题列表</param>
        /// <param name="intValue">关闭/打开标志( 0 为打开,1 为关闭)</param>
        /// <returns>更新主题个数</returns>
        public int SetTopicClose(string topicList, short intValue)
        {
            var qrecords = TokyoTyrantService.GetColumns(pool, topicList.Split(','));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                if(column["closed"] =="0" || column["closed"] =="1") 
                {
                    column["closed"] = intValue.ToString();
                    TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
                }
            }
            return topicList.Split(',').Length;
        }

         /// <summary>
        /// 删除指定主题
        /// </summary>
        /// <param name="topiclist">要删除的主题ID列表</param>
       /// <returns></returns>
        public int DeleteTopicByTidList(string topicList)
        {
            TokyoTyrantService.DeleteMultiple(pool, topicList.Split(','));
            foreach(string tid in topicList.Split(','))
            {
                var qrecords = TokyoTyrantService.QueryRecords(pool,new Query().NumberEquals("closed", TypeConverter.StrToInt(tid)));
                foreach (string key in qrecords.Keys)
                {
                    var column = qrecords[key];
                    TokyoTyrantService.Delete(pool, column["tid"]);
                }
            }
            return topicList.Split(',').Length;
        }

        /// <summary>
        /// 复制主题链接,因为该操作无效获取新增主题ID信息，所以只能变通，使用获取新增主题之前的最大TID来批量添加TTCACHE信息
        /// </summary>
        /// <param name="oldfid"></param>
        /// <param name="topicList"></param>
        /// <returns></returns>
        public int CopyTopicLink(int maxtid)
        {
            IDataReader dataReader = Discuz.Data.DbHelper.ExecuteReader(CommandType.Text, "Select * From ["+ BaseConfigs.GetTablePrefix+"topics] WHERE [tid]>"+maxtid);
            while (dataReader.Read())
            {
                TopicInfo topicInfo = LoadSingleTopicInfo(dataReader);
                TokyoTyrantService.PutColumns(pool, topicInfo.Tid.ToString(), LoadTopic(topicInfo), true);
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
            foreach(string tid in topicList.Split(','))
            {
                TopicInfo topicInfo = LoadSingleTopicInfo(Discuz.Data.DatabaseProvider.GetInstance().GetTopicInfo(TypeConverter.StrToInt(tid), 0, 0));
                if(topicInfo != null)
                {
                    var column = LoadTopic(topicInfo);
                    TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
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
        /// <param name="topiclist"></param>
        /// <param name="identify"></param>
        public void IdentifyTopic(string topiclist, int identify)
        {
            var qrecords = TokyoTyrantService.GetColumns(pool, topiclist.Split(','));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                column["identify"] = identify.ToString();
                TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
            }
        }

        /// <summary>
        /// 设置主题的下沉和提升
        /// </summary>
        /// <param name="tidList"></param>
        /// <param name="lastpostid"></param>
        public void SetTopicsBump(string tidList, int lastpostid)
        {
            var qrecords = TokyoTyrantService.GetColumns(pool, tidList.Split(','));
            foreach (string key in qrecords.Keys)
            {
                var column = qrecords[key];
                if (lastpostid != 0)
                    column["lastpostid"] = lastpostid.ToString();
                else
                    column["lastpostid"] = (0-TypeConverter.StrToInt(column["lastpostid"])).ToString();
                TokyoTyrantService.PutColumns(pool, column["tid"], column, true);
            }         
        }

        /// <summary>
        /// 删除关闭主题
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="tidList"></param>
        public void DeleteClosedTopics(int fid, string tidList)
        {
            foreach (string tid in tidList.Split(','))
            {
                var qrecords = TokyoTyrantService.QueryRecords(pool, new Query().NumberEquals("fid", fid).NumberEquals("closed", TypeConverter.StrToInt(tid)));
                foreach (string key in qrecords.Keys)
                {
                    var column = qrecords[key];
                    TokyoTyrantService.Delete(pool, column["tid"]);
                }
            }
        }
        #endregion


        #region 获取主题信息
        private TopicInfo LoadTopicInfo(IDictionary<string, string> column)
        {
            TopicInfo topicinfo = new TopicInfo();
            topicinfo.Tid = TypeConverter.StrToInt(column["tid"]);
            topicinfo.Fid = TypeConverter.StrToInt(column["fid"]);
            topicinfo.Iconid = TypeConverter.StrToInt(column["iconid"]);
            topicinfo.Typeid = TypeConverter.StrToInt(column["typeid"]);
            topicinfo.Readperm = TypeConverter.StrToInt(column["readperm"]);
            topicinfo.Price = TypeConverter.StrToInt(column["price"]);
            topicinfo.Poster = column["poster"];
            topicinfo.Posterid = TypeConverter.StrToInt(column["posterid"]);
            topicinfo.Title = column["title"];
            topicinfo.Attention = TypeConverter.StrToInt(column["attention"]);
            topicinfo.Postdatetime = UnixDateTimeHelper.ConvertFromUnixTimestamp(TypeConverter.StrToInt(column["postdatetime"])).ToString();
            topicinfo.Lastpost = UnixDateTimeHelper.ConvertFromUnixTimestamp(TypeConverter.StrToInt(column["lastpost"])).ToString();
            topicinfo.Lastpostid = TypeConverter.StrToInt(column["lastpostid"]);
            topicinfo.Lastposter = column["lastposter"];
            topicinfo.Lastposterid = TypeConverter.StrToInt(column["lastposterid"]);
            topicinfo.Views = TypeConverter.StrToInt(column["views"]);
            topicinfo.Replies = TypeConverter.StrToInt(column["replies"]);
            topicinfo.Displayorder = TypeConverter.StrToInt(column["displayorder"]);
            topicinfo.Highlight = column["highlight"];
            topicinfo.Digest = TypeConverter.StrToInt(column["digest"]);
            topicinfo.Rate = TypeConverter.StrToInt(column["rate"]);
            topicinfo.Hide = TypeConverter.StrToInt(column["hide"]);
            topicinfo.Attachment = TypeConverter.StrToInt(column["attachment"]);
            topicinfo.Moderated = TypeConverter.StrToInt(column["moderated"]);
            topicinfo.Closed = TypeConverter.StrToInt(column["closed"]);
            topicinfo.Magic = TypeConverter.StrToInt(column["magic"]);
            topicinfo.Identify = TypeConverter.StrToInt(column["identify"]);
            topicinfo.Special = (byte)TypeConverter.StrToInt(column["special"]);
            return topicinfo;
        }

        private IDictionary<string, string> LoadTopic(TopicInfo topicinfo)
        {
            IDictionary<string, string> topicdic = new System.Collections.Generic.Dictionary<string, string>();
            topicdic.Add("tid", topicinfo.Tid.ToString());
            topicdic.Add("fid", topicinfo.Fid.ToString());
            topicdic.Add("iconid", topicinfo.Iconid.ToString());
            topicdic.Add("typeid", topicinfo.Typeid.ToString());
            topicdic.Add("readperm", topicinfo.Readperm.ToString());
            topicdic.Add("price", topicinfo.Price.ToString());
            topicdic.Add("poster", topicinfo.Poster);
            topicdic.Add("posterid", topicinfo.Posterid.ToString());
            topicdic.Add("title", topicinfo.Title);
            topicdic.Add("attention", topicinfo.Attention.ToString());
            topicdic.Add("postdatetime", UnixDateTimeHelper.ConvertToUnixTimestamp(TypeConverter.StrToDateTime(topicinfo.Postdatetime)).ToString());
            topicdic.Add("lastpost", UnixDateTimeHelper.ConvertToUnixTimestamp(TypeConverter.StrToDateTime(topicinfo.Lastpost)).ToString());
            topicdic.Add("lastpostid", topicinfo.Lastpostid.ToString());
            topicdic.Add("lastposter", topicinfo.Lastposter);
            topicdic.Add("lastposterid", topicinfo.Lastposterid.ToString());
            topicdic.Add("views", topicinfo.Views.ToString());
            topicdic.Add("replies", topicinfo.Replies.ToString());
            topicdic.Add("displayorder", topicinfo.Displayorder.ToString());
            topicdic.Add("highlight", topicinfo.Highlight);
            topicdic.Add("digest", topicinfo.Digest.ToString());
            topicdic.Add("rate", topicinfo.Rate.ToString());
            topicdic.Add("hide", topicinfo.Hide.ToString());
            topicdic.Add("attachment", topicinfo.Attachment.ToString());
            topicdic.Add("moderated", topicinfo.Moderated.ToString());
            topicdic.Add("closed", topicinfo.Closed.ToString());
            topicdic.Add("magic", topicinfo.Magic.ToString());
            topicdic.Add("identify", topicinfo.Identify.ToString());
            topicdic.Add("special", topicinfo.Special.ToString());
            return topicdic;
        }

        /// <summary>
        /// 加载单个主题对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static TopicInfo LoadSingleTopicInfo(IDataReader reader)
        {
            //TODO:字段查询不同，改查询
            //StringBuilder tablefield = new StringBuilder();
            //tablefield.Append(",");
            //foreach (DataRow dr in reader.GetSchemaTable().Rows)
            //{
            //    tablefield.Append(dr["ColumnName"].ToString().ToLower() + ",");
            //}
            reader.Read();
            TopicInfo topicInfo = new TopicInfo();
            topicInfo.Tid = TypeConverter.ObjectToInt(reader["tid"]);
            topicInfo.Fid =  TypeConverter.ObjectToInt(reader["fid"]);
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


        /// <summary>
        /// 装帖子信息
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static PostInfo LoadSinglePostInfo(IDataReader reader)
        {
            PostInfo postInfo = new PostInfo();
            postInfo.Pid = TypeConverter.ObjectToInt(reader["pid"]);
            postInfo.Fid = TypeConverter.ObjectToInt(reader["fid"]);
            postInfo.Tid = TypeConverter.ObjectToInt(reader["tid"]);
            postInfo.Parentid = TypeConverter.ObjectToInt(reader["parentid"]);
            postInfo.Layer = TypeConverter.ObjectToInt(reader["layer"]);
            postInfo.Poster = reader["poster"].ToString();
            postInfo.Posterid = TypeConverter.ObjectToInt(reader["posterid"]);
            postInfo.Title = reader["title"].ToString();
            postInfo.Postdatetime = reader["postdatetime"].ToString();
            postInfo.Message = reader["message"].ToString();
            postInfo.Ip = reader["ip"].ToString();
            postInfo.Lastedit = reader["lastedit"].ToString();
            postInfo.Invisible = TypeConverter.ObjectToInt(reader["invisible"]);
            postInfo.Usesig = TypeConverter.ObjectToInt(reader["usesig"]);
            postInfo.Htmlon = TypeConverter.ObjectToInt(reader["htmlon"]);
            postInfo.Smileyoff = TypeConverter.ObjectToInt(reader["smileyoff"]);
            postInfo.Bbcodeoff = TypeConverter.ObjectToInt(reader["bbcodeoff"]);
            postInfo.Parseurloff = TypeConverter.ObjectToInt(reader["parseurloff"]);
            postInfo.Attachment = TypeConverter.ObjectToInt(reader["attachment"]);
            postInfo.Rate = TypeConverter.ObjectToInt(reader["rate"]);
            postInfo.Ratetimes = TypeConverter.ObjectToInt(reader["ratetimes"]);
            return postInfo;
        }

        #endregion

    }
}
