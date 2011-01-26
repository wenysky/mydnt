using System;
using System.Text;
using System.Data;

using Discuz.Cache;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;
using System.IO;
using System.Text.RegularExpressions;

namespace Discuz.Forum
{
    /// <summary>
    /// 热点管理类
    /// </summary>
    public class ForumHots
    {
        private static object lockHelper = new object();

        private static string scoreFilePath = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/forumhot.config");
        /// <summary>
        /// 载入论坛热点配置文件
        /// </summary>
        /// <returns></returns>
        public static DataTable GetForumHot()
        {
            lock (lockHelper)
            {
                DNTCache cache = DNTCache.GetCacheService();
                DataTable dt = cache.RetrieveObject("/Forum/ForumHot") as DataTable;

                if (dt == null)
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(scoreFilePath);
                    dt = ds.Tables["hottab"];
                    cache.AddObject("/Forum/ForumHot", dt, new string[] { scoreFilePath });
                }
                return dt;
            }
        }

        /// <summary>
        /// 获得帖子列表
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="views">最小浏览量</param>
        /// <param name="fid">板块ID</param>
        /// <param name="timetype">期限类型,一天、一周、一月、不限制</param>
        /// <param name="ordertype">排序类型,时间倒序、浏览量倒序、最后回复倒序</param>
        /// <param name="isdigest">是否精华</param>
        /// <param name="cachetime">缓存的有效期(单位:分钟)</param>
        /// <returns></returns>
        public static DataTable GetTopicList(int count, int fid,TopicOrderType ordertype,bool digest, int cachetime,bool onlyimg,string fidlist, int tabid)
        {
            //防止恶意行为
            if (cachetime == 0)
                cachetime = 1;

            if (count > 50)
                count = 50;

            if (count < 1)
                count = 1;

            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject("/Forum/ForumHostList-" + tabid) as DataTable;

            if (dt == null)
            {
                if (fidlist == "")
                {
                    fidlist = Forums.GetVisibleForum();
                }
                if (Focuses.GetFieldName(ordertype) == "digest")
                {
                    digest = true;
                }
                dt = Discuz.Data.Topics.GetTopicList(count, 1, fid, "", Focuses.GetStartDate(TopicTimeType.All), Focuses.GetFieldName(ordertype), fidlist, digest, onlyimg);

                cache.AddObject("/Forum/ForumHostList-" + tabid, dt, cachetime);
            }
            return dt;
        }

        /// <summary>
        /// 获取一个帖子的缓存
        /// </summary>
        /// <param name="tid">帖子ID</param>
        /// <param name="cachetime">缓存的有效期</param>
        /// <returns></returns>
        public static DataTable GetFirstPostInfo(int tid, int cachetime)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            DataTable dt = cache.RetrieveObject("/Forum/HotForumFirst_" + tid) as DataTable;
            if (dt == null)
            {
                dt = Posts.GetPostList(tid.ToString());
                cache.AddObject("/Forum/HotForumFirst_" + tid, dt, cachetime);
            }
            return dt;
        }


        /// <summary>
        /// 获取热门板块
        /// </summary>
        /// <param name="topNumber">获取的数量</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="fid">板块ID</param>
        /// <param name="cachetime">缓存时间</param>
        /// <returns></returns>
        public static DataTable GetHotForumList(int topNumber, string orderby, int fid, int cachetime,int tabid)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();



            DataTable forumList = cache.RetrieveObject("/Aggregation/HotForumList_" + tabid) as DataTable;
            if (forumList == null)
            {
                forumList = Discuz.Data.DatabaseProvider.GetInstance().GetWebSiteAggHotForumList(topNumber <= 0 ? 10 : topNumber,orderby,fid);

                //声明新的缓存策略接口
                //Discuz.Cache.ICacheStrategy ics = new Discuz.Forum.ForumCacheStrategy();
                //ics.TimeOut = 300;
                //cache.LoadCacheStrategy(ics);
                cache.AddObject("/Aggregation/HotForumList" + tabid, forumList, cachetime);
                //cache.LoadDefaultCacheStrategy();
            }
            return forumList;
        }

        /// <summary>
        /// 获取热门用户
        /// </summary>
        /// <param name="topNumber">获取的数量</param>
        /// <param name="orderBy">排序方式</param>
        /// <param name="cachetime">缓存时间</param>
        /// <returns></returns>
        public static DataTable GetUserList(int topNumber, string orderBy, int cachetime,int tabid)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            DataTable userList = cache.RetrieveObject("/Aggregation/Users_" + tabid + "List") as DataTable;
            if (userList == null)
            {
                userList = Users.GetUserList(topNumber, 1, orderBy, "desc");
                cache.AddObject("/Aggregation/Users_" + tabid + "List", userList, cachetime);
            }
            return userList;
        }

        /// <summary>
        /// 获取热门图片
        /// </summary>
        /// <param name="topNumber">获取的数量</param>
        /// <param name="orderBy">排序方式</param>
        /// <param name="cachetime">缓存时间</param>
        /// <returns></returns>
        private static DataTable HotImages(int count,int cachetime, string orderby,int tabid,string fidlist)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            DataTable imagelist = cache.RetrieveObject("/Aggregation/HotImages_" + tabid + "List") as DataTable;
            if (imagelist == null)
            {
                imagelist = Discuz.Data.DatabaseProvider.GetInstance().GetWebSiteAggHotImages(count, orderby,fidlist);
                cache.AddObject("/Aggregation/HotImages_" + tabid + "List", imagelist, cachetime);
            }
            return imagelist;
        }

        /// <summary>
        /// 转换热门图片为数组
        /// </summary>
        /// <param name="topNumber">获取的数量</param>
        /// <param name="orderBy">排序方式</param>
        /// <param name="cachetime">缓存时间</param>
        /// <returns></returns>
        public static string HotImagesArray(int count, int cachetime, string orderby, int tabid, string fidlist)
        {
            string title = "", imgs = "", urls = "";
            //如果没有缩略图，则去生成
            if (!Directory.Exists(Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/rotatethumbnail/")))
            {
                Utils.CreateDir(Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/rotatethumbnail/"));
            }
            fidlist = (fidlist == "" ? Forums.GetVisibleForum() : fidlist);
            foreach (DataRow dr in HotImages(count, cachetime, orderby, tabid, fidlist).Rows)
            {	
				//图片文件名称
                string fullfilename = BaseConfigs.GetForumPath + "upload/" + dr["filename"].ToString().Replace('\\', '/').Trim();
                //图片缩略后的名称
				string filename = "cache/rotatethumbnail/r_"+Utils.GetFilename(fullfilename);
                //如果有缩略图，则不去生成
				if (File.Exists(Utils.GetMapPath(BaseConfigs.GetForumPath + filename)))
                {
                    imgs += filename + "|";
                    urls += "showtopic.aspx?topicid=" + TypeConverter.ObjectToInt(dr["tid"]) + "|";
                    title += dr["title"].ToString().Trim() + "|";
                }
                else
                {
	
                    if (File.Exists(Utils.GetMapPath(fullfilename)))
                    {
                        FileInfo[] files = new DirectoryInfo(Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/rotatethumbnail/")).GetFiles();
                        //如果缓存文件夹cache/rotatethumbnail 下的文件大于100个，则删除
                        if (files.Length > 100)
                        {
                            Attachments.QuickSort(files, 0, files.Length - 1);

                            for (int i = files.Length - 1; i >= 50; i--)
                            {
                                try
                                {
                                    files[i].Delete();
                                }
                                catch
                                { }
                            }
                        }
                        Thumbnail.MakeThumbnailImage(Utils.GetMapPath(fullfilename), Utils.GetMapPath(BaseConfigs.GetForumPath + filename), 360, 240);
                        imgs += filename + "|";
                        urls += "showtopic.aspx?tid=" + TypeConverter.ObjectToInt(dr["tid"]) + "|";
                        title += dr["title"].ToString().Trim() + "|";
                    }
                    else
                    {
                        continue;
                    }
                
                }
            }
            return title.TrimEnd('|') + "," + imgs.TrimEnd('|') + "," + urls.TrimEnd('|');
        }

        /// <summary>
        /// 返回删除了图片附件
        /// </summary>
        /// <param name="message">帖子内容</param>
        /// <param name="length">截取内容的长度</param>
        /// <returns></returns>
        public static string RemoveUbb(string message,int length)
        {
            message = Regex.Replace(message, @"\[attachimg\](\d+)(\[/attachimg\])*", "{图片}", RegexOptions.IgnoreCase);
            message = Regex.Replace(message, @"\[img\]\s*([^\[\<\r\n]+?)\s*\[\/img\]", "{图片}", RegexOptions.IgnoreCase);
            message = Regex.Replace(message, @"\[img=(\d{1,4})[x|\,](\d{1,4})\]\s*([^\[\<\r\n]+?)\s*\[\/img\]", "{图片}", RegexOptions.IgnoreCase);
            message = Regex.Replace(message, @"\[attach\](\d+)(\[/attach\])*", "{附件}", RegexOptions.IgnoreCase);
            message = Regex.Replace(message, @"\s*\[hide=(\d+?)\][\n\r]*([\s\S]+?)[\n\r]*\[\/hide\]\s*", "{隐藏内容}", RegexOptions.IgnoreCase);
            return Utils.GetSubString(Utils.ClearUBB(Utils.RemoveHtml(message)).Replace("{","[").Replace("}","]"), length, "......");
        }

    }
}