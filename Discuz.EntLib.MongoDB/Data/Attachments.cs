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
    public class Attachments : Discuz.Cache.Data.ICacheAttachments
    {

        private static string connectString = null;

        private static DBCache mongoCache = EntLibConfigs.GetConfig().Cacheattachments;


        static Attachments()
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
        /// 产生附件
        /// </summary>
        /// <param name="attachmentinfo">附件描述类</param>
        /// <returns>附件id</returns>
        public int CreateAttachments(AttachmentInfo attachmentinfo)
        {
            if (attachmentinfo != null)
            {
                Document attachdoc = LoadAttachment(attachmentinfo);
                MongoDbHelper.Insert(mongoDB, "attachments", attachdoc);
            }
            return attachmentinfo.Aid;
        }

        public static Document LoadAttachment(AttachmentInfo attachmentInfo)
        {
            Document doc = new Document();
            doc["_id"] = attachmentInfo.Aid;
            doc["aid"] = attachmentInfo.Aid;
            doc["uid"] = attachmentInfo.Uid;
            doc["tid"] = attachmentInfo.Tid;
            doc["pid"] = attachmentInfo.Pid;
            doc["postdatetime"] = attachmentInfo.Postdatetime;
            doc["readperm"] = attachmentInfo.Readperm;
            doc["filename"] = attachmentInfo.Filename;
            doc["description"] = attachmentInfo.Description;
            doc["filetype"] = attachmentInfo.Filetype;
            doc["filesize"] = attachmentInfo.Filesize;
            doc["attachment"] = attachmentInfo.Attachment;
            doc["downloads"] = attachmentInfo.Downloads;
            doc["extname"] = Utils.GetFileExtName(attachmentInfo.Attachment);
            doc["attachprice"] = attachmentInfo.Attachprice;
            doc["width"] = attachmentInfo.Width;
            doc["height"] = attachmentInfo.Height;
            doc["isimage"] = attachmentInfo.Isimage; 
            return doc;
        }

        /// <summary>
        /// 获得指定附件的描述信息
        /// </summary>
        /// <param name="aid">附件id</param>
        /// <returns>描述信息</returns>
        public AttachmentInfo GetAttachmentInfo(int aid)
        {
            return LoadAttachment(MongoDbHelper.FindOne(mongoDB, "attachments", new Document().Add("_id", aid)), true);
        }

        /// <summary>
        /// 将单个附件DataRow转换为AttachmentInfo类
        /// </summary>
        /// <param name="drAttach">单个附件DataRow</param>
        /// <param name="drAttach">是否返回原始路径</param>
        /// <returns>AttachmentInfo类</returns>
        private static AttachmentInfo LoadAttachment(Document doc, bool isOriginalFilename)
        {
            AttachmentInfo attach = new AttachmentInfo();
            if (doc != null)
            {
                attach.Aid = TypeConverter.ObjectToInt(doc["aid"]);
                attach.Uid = TypeConverter.ObjectToInt(doc["uid"]);
                attach.Tid = TypeConverter.ObjectToInt(doc["tid"]);
                attach.Pid = TypeConverter.ObjectToInt(doc["pid"]);
                attach.Postdatetime = doc["postdatetime"].ToString();
                attach.Readperm = TypeConverter.ObjectToInt(doc["readperm"]);

                if (isOriginalFilename)
                {
                    attach.Filename = doc["filename"].ToString();
                }
                else if (doc["filename"].ToString().Trim().ToLower().IndexOf("http") < 0)
                {
                    attach.Filename = BaseConfigs.GetForumPath + "upload/" + doc["filename"].ToString().Trim().Replace("\\", "/");
                }
                else
                {
                    attach.Filename = doc["filename"].ToString().Trim().Replace("\\", "/");
                }
                attach.Description = doc["description"].ToString().Trim();
                attach.Filetype = doc["filetype"].ToString().Trim();
                attach.Attachment = doc["attachment"].ToString().Trim();
                attach.Filesize = TypeConverter.ObjectToInt(doc["filesize"]);
                attach.Downloads = TypeConverter.ObjectToInt(doc["downloads"]);
                attach.Attachprice = TypeConverter.ObjectToInt(doc["attachprice"], 0);
                attach.Height = TypeConverter.ObjectToInt(doc["height"], 0);
                attach.Width = TypeConverter.ObjectToInt(doc["width"], 0);
                attach.Isimage = TypeConverter.ObjectToInt(doc["isimage"], 0);
            }
            return attach;
        }

        /// <summary>
        /// 获得指定帖子的附件个数
        /// </summary>
        /// <param name="pid">帖子ID</param>
        /// <returns>附件个数</returns>
        public int GetAttachmentCountByPid(int pid)
        {
            return (int)MongoDbHelper.Count(mongoDB, "attachments", new Document().Add("pid", pid));
        }

        /// <summary>
        /// 获得指定主题的附件个数
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>附件个数</returns>
        public int GetAttachmentCountByTid(int tid)
        {
            return (int)MongoDbHelper.Count(mongoDB, "attachments", new Document().Add("tid", tid));
        }

        /// <summary>
        /// 获取指定PID列表的相应附件信息列表
        /// </summary>
        /// <param name="pidList">指定PID列表</param>
        /// <returns>相应附件信息列表</returns>
        public List<ShowtopicPageAttachmentInfo> GetAttachmentListByPidList(string pidList)
        {
            System.Collections.Generic.List<Document> docList = MongoDbHelper.Find(mongoDB, "attachments", new Document().Add("pid", Op.In(TypeConverter.StringToIntArray(pidList))));
            List<ShowtopicPageAttachmentInfo> list = new List<ShowtopicPageAttachmentInfo>();
            foreach(Document doc in docList)
            {
                list.Add(LoadSingleAttachmentInfo(doc));
            }
            return list;
        }

     

        /// <summary>
        /// 更新附件下载次数
        /// </summary>
        /// <param name="aid">附件id</param>
        public void UpdateAttachmentDownloads(int aid)
        {
            Document doc = MongoDbHelper.FindOne(mongoDB, "attachments", new Document().Add("_id", aid));

            doc["downloads"] = TypeConverter.ObjectToInt(doc["downloads"]) + 1;

            MongoDbHelper.Update(mongoDB, "attachments", doc, new Document().Add("_id", aid));
        }

        /// <summary>
        /// 删除指定主题的所有附件
        /// </summary>
        /// <param name="tid">主题tid</param>
        /// <returns>删除个数</returns>
        public void DeleteAttachmentByTid(int tid)
        {
            MongoDbHelper.Delete(mongoDB, "attachments", new Document().Add("tid", tid));
        }

        /// <summary>
        /// 删除指定主题的所有附件
        /// </summary>
        /// <param name="tidlist">版块tid列表</param>
        /// <returns>删除个数</returns>
        public void DeleteAttachmentByTid(string tidlist)
        {
            MongoDbHelper.Delete(mongoDB, "attachments", new Document().Add("tid", Op.In(TypeConverter.StringToIntArray(tidlist))));
        }

        /// <summary>
        /// 删除指定附件
        /// </summary>
        /// <param name="aid">附件aid</param>
        /// <returns>删除个数</returns>
        public void DeleteAttachment(int aid)
        {
            MongoDbHelper.Delete(mongoDB, "attachments", new Document().Add("_id", aid));
        }

       
        /// <summary>
        /// 更新附件信息
        /// </summary>
        /// <param name="attachmentInfo">附件对象</param>
        /// <returns>返回被更新的数量</returns>
        public void UpdateAttachment(AttachmentInfo attachmentInfo)
        {
            MongoDbHelper.Update(mongoDB, "attachments", LoadAttachment(attachmentInfo), new Document().Add("_id", attachmentInfo.Aid));
        }


        /// <summary>
        /// 批量删除附件
        /// </summary>
        /// <param name="aidList">附件Id，以英文逗号分割</param>
        /// <returns>返回被删除的个数</returns>
        public void DeleteAttachment(string aidList)
        {
            MongoDbHelper.Delete(mongoDB, "attachments", new Document().Add("_id", Op.In(TypeConverter.StringToIntArray(aidList))));
        }

      
        /// <summary>
        /// 取得主题帖的第一个图片附件
        /// </summary>
        /// <param name="tid">主题id</param>
        public AttachmentInfo GetFirstImageAttachByTid(int tid)
        {
            //System.Collections.Generic.List<Document> docList = MongoDbHelper.Find(mongoDB, "attachments", new Document().Add("tid", tid).Add("filetype", "/^image.*/i"), "aid", IndexOrder.Ascending, 1, 0);
            System.Collections.Generic.List<Document> docList = MongoDbHelper.Find(mongoDB, "attachments", new Document().Add("tid", tid), "aid", IndexOrder.Ascending, 1, 0);
            foreach (Document doc in docList)
            {
                if(doc["filetype"].ToString().ToLower().StartsWith("image"))
                    return LoadAttachment(doc, false);
            }
            return new AttachmentInfo();
        }


        /// <summary>
        /// 根据帖子ID删除附件
        /// </summary>
        /// <param name="pid">帖子ID</param>
        public void DeleteAttachmentByPid(int pid)
        {
            MongoDbHelper.Delete(mongoDB, "attachments", new Document().Add("pid", pid));
        }      
     

        ///// <summary>
        ///// 获取指定用户未使用的附件的JSON字符串,dnt_getnousedattachmentlistbyuid
        ///// </summary>
        ///// <param name="userid">指定用户id</param>
        ///// <returns>JSON字符串</returns>
        public Discuz.Common.Generic.List<AttachmentInfo> GetNoUsedAttachmentJson(int userid, string posttime, int isimage)
        {
            StringBuilder attachmentStringBuilder = new StringBuilder();
            attachmentStringBuilder.Append("[");
            System.Collections.Generic.List<Document> docList;

            if(string.IsNullOrEmpty(posttime))
               if(isimage == 0 || isimage == 1)
                  docList = MongoDbHelper.Find(mongoDB, "attachments", new Document().Add("uid", userid).Add("tid", 0).Add("pid", 0).Add("isimage", isimage));
               else
                  docList = MongoDbHelper.Find(mongoDB, "attachments", new Document().Add("uid", userid).Add("tid", 0).Add("pid", 0));
            else	           if(isimage == 0 || isimage == 1)
                  docList = MongoDbHelper.Find(mongoDB, "attachments", new Document().Add("uid", userid).Add("tid", 0).Add("pid", 0).Add("isimage", isimage).Add("postdatetime", Op.GreaterThanOrEqual(posttime)));
               else
                  docList = MongoDbHelper.Find(mongoDB, "attachments", new Document().Add("uid", userid).Add("tid", 0).Add("pid", 0).Add("postdatetime", Op.GreaterThanOrEqual(posttime)));

            Discuz.Common.Generic.List<AttachmentInfo> attachmentList = new Discuz.Common.Generic.List<AttachmentInfo>();
            if (docList.Count > 0)
            {
                foreach (Document doc in docList)
                {
                    if (!Utils.StrIsNullOrEmpty(doc["aid"].ToString()))
                    {
                        attachmentList.Add(LoadSingleAttachmentInfo(doc) as AttachmentInfo);                        
                    }
                }
            } 
            return attachmentList;
        }

        /// <summary>
        /// 删除未被使用的论坛附件
        /// </summary>
        public void DeleteNoUsedForumAttachment()
        {
            MongoDbHelper.Delete(mongoDB, "attachments", new Document().Add("tid", 0).Add("pid", 0));
            //WHERE [tid]= 0 AND [pid]=0 AND DATEDIFF(n, postdatetime, GETDATE()) > 30
        }

        /// <summary>
        /// 删除指定用户的附件
        /// </summary>
        /// <param name="uid">用户ID</param>
        public void DeleteAttachmentByUid(int uid)
        {
            MongoDbHelper.Delete(mongoDB, "attachments", new Document().Add("uid", uid));
        }

        /// <summary>
        /// 加载单个附件实体对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static ShowtopicPageAttachmentInfo LoadSingleAttachmentInfo(Document doc)
        {
            ShowtopicPageAttachmentInfo attInfo = new ShowtopicPageAttachmentInfo();

            attInfo.Aid = TypeConverter.ObjectToInt(doc["aid"]);
            attInfo.Tid = TypeConverter.ObjectToInt(doc["tid"]);
            attInfo.Pid = TypeConverter.ObjectToInt(doc["pid"]);
            attInfo.Postdatetime = doc["postdatetime"].ToString();
            attInfo.Readperm = TypeConverter.ObjectToInt(doc["readperm"]);
            attInfo.Filename = doc["filename"].ToString();
            attInfo.Description = doc["description"].ToString();
            attInfo.Filetype = doc["filetype"].ToString();
            attInfo.Filesize = TypeConverter.ObjectToInt(doc["filesize"]);
            attInfo.Attachment = doc["attachment"].ToString();
            attInfo.Downloads = TypeConverter.ObjectToInt(doc["downloads"]);
            attInfo.Attachprice = TypeConverter.ObjectToInt(doc["attachprice"]);
            attInfo.Uid = TypeConverter.ObjectToInt(doc["uid"]);
            attInfo.Width = TypeConverter.ObjectToInt(doc["width"]);
            attInfo.Height = TypeConverter.ObjectToInt(doc["height"]);
            attInfo.Isimage = TypeConverter.ObjectToInt(doc["isimage"]);
            return attInfo;
        }      
    }
}