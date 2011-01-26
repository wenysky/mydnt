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
    /// �ȵ������
    /// </summary>
    public class ForumHots
    {
        private static object lockHelper = new object();

        private static string scoreFilePath = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/forumhot.config");
        /// <summary>
        /// ������̳�ȵ������ļ�
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
        /// ��������б�
        /// </summary>
        /// <param name="count">����</param>
        /// <param name="views">��С�����</param>
        /// <param name="fid">���ID</param>
        /// <param name="timetype">��������,һ�졢һ�ܡ�һ�¡�������</param>
        /// <param name="ordertype">��������,ʱ�䵹��������������ظ�����</param>
        /// <param name="isdigest">�Ƿ񾫻�</param>
        /// <param name="cachetime">�������Ч��(��λ:����)</param>
        /// <returns></returns>
        public static DataTable GetTopicList(int count, int fid,TopicOrderType ordertype,bool digest, int cachetime,bool onlyimg,string fidlist, int tabid)
        {
            //��ֹ������Ϊ
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
        /// ��ȡһ�����ӵĻ���
        /// </summary>
        /// <param name="tid">����ID</param>
        /// <param name="cachetime">�������Ч��</param>
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
        /// ��ȡ���Ű��
        /// </summary>
        /// <param name="topNumber">��ȡ������</param>
        /// <param name="orderby">����ʽ</param>
        /// <param name="fid">���ID</param>
        /// <param name="cachetime">����ʱ��</param>
        /// <returns></returns>
        public static DataTable GetHotForumList(int topNumber, string orderby, int fid, int cachetime,int tabid)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();



            DataTable forumList = cache.RetrieveObject("/Aggregation/HotForumList_" + tabid) as DataTable;
            if (forumList == null)
            {
                forumList = Discuz.Data.DatabaseProvider.GetInstance().GetWebSiteAggHotForumList(topNumber <= 0 ? 10 : topNumber,orderby,fid);

                //�����µĻ�����Խӿ�
                //Discuz.Cache.ICacheStrategy ics = new Discuz.Forum.ForumCacheStrategy();
                //ics.TimeOut = 300;
                //cache.LoadCacheStrategy(ics);
                cache.AddObject("/Aggregation/HotForumList" + tabid, forumList, cachetime);
                //cache.LoadDefaultCacheStrategy();
            }
            return forumList;
        }

        /// <summary>
        /// ��ȡ�����û�
        /// </summary>
        /// <param name="topNumber">��ȡ������</param>
        /// <param name="orderBy">����ʽ</param>
        /// <param name="cachetime">����ʱ��</param>
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
        /// ��ȡ����ͼƬ
        /// </summary>
        /// <param name="topNumber">��ȡ������</param>
        /// <param name="orderBy">����ʽ</param>
        /// <param name="cachetime">����ʱ��</param>
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
        /// ת������ͼƬΪ����
        /// </summary>
        /// <param name="topNumber">��ȡ������</param>
        /// <param name="orderBy">����ʽ</param>
        /// <param name="cachetime">����ʱ��</param>
        /// <returns></returns>
        public static string HotImagesArray(int count, int cachetime, string orderby, int tabid, string fidlist)
        {
            string title = "", imgs = "", urls = "";
            //���û������ͼ����ȥ����
            if (!Directory.Exists(Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/rotatethumbnail/")))
            {
                Utils.CreateDir(Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/rotatethumbnail/"));
            }
            fidlist = (fidlist == "" ? Forums.GetVisibleForum() : fidlist);
            foreach (DataRow dr in HotImages(count, cachetime, orderby, tabid, fidlist).Rows)
            {	
				//ͼƬ�ļ�����
                string fullfilename = BaseConfigs.GetForumPath + "upload/" + dr["filename"].ToString().Replace('\\', '/').Trim();
                //ͼƬ���Ժ������
				string filename = "cache/rotatethumbnail/r_"+Utils.GetFilename(fullfilename);
                //���������ͼ����ȥ����
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
                        //��������ļ���cache/rotatethumbnail �µ��ļ�����100������ɾ��
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
        /// ����ɾ����ͼƬ����
        /// </summary>
        /// <param name="message">��������</param>
        /// <param name="length">��ȡ���ݵĳ���</param>
        /// <returns></returns>
        public static string RemoveUbb(string message,int length)
        {
            message = Regex.Replace(message, @"\[attachimg\](\d+)(\[/attachimg\])*", "{ͼƬ}", RegexOptions.IgnoreCase);
            message = Regex.Replace(message, @"\[img\]\s*([^\[\<\r\n]+?)\s*\[\/img\]", "{ͼƬ}", RegexOptions.IgnoreCase);
            message = Regex.Replace(message, @"\[img=(\d{1,4})[x|\,](\d{1,4})\]\s*([^\[\<\r\n]+?)\s*\[\/img\]", "{ͼƬ}", RegexOptions.IgnoreCase);
            message = Regex.Replace(message, @"\[attach\](\d+)(\[/attach\])*", "{����}", RegexOptions.IgnoreCase);
            message = Regex.Replace(message, @"\s*\[hide=(\d+?)\][\n\r]*([\s\S]+?)[\n\r]*\[\/hide\]\s*", "{��������}", RegexOptions.IgnoreCase);
            return Utils.GetSubString(Utils.ClearUBB(Utils.RemoveHtml(message)).Replace("{","[").Replace("}","]"), length, "......");
        }

    }
}