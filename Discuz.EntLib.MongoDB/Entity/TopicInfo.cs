using System;
using System.Data;
using System.Linq;
using System.Text;

using Discuz.Entity;
using MongoDB.Attributes;

namespace Discuz.EntLib.MongoDB.Entity
{

    //  使用LINQ方式会导致在1500万主题分页下运行的测试周期延长（前者(document该款)从2:10秒延长到后者(linq)2：30秒）和吞吐量降低（2分钟时仅留一个脚本， 后者两分者会有2-3个脚本还在运行）
    //   Mongo db = new Mongo("Servers=10.0.4.5:27017;ConnectTimeout=30000;ConnectionLifetime=300000;MinimumPoolSize=64;MaximumPoolSize=256;Pooled=true");

    //        db.Connect();
    //        var topicColl = db.GetDatabase("dnt_mongodb").GetCollection<Discuz.EntLib.MongoDB.Entity.TopicInfo>("topics");
    //        var topicInfoList = topicColl.Linq().Where(t => t.Fid == 2 && t.Displayorder == 0).Skip(skip).OrderByDescending(t=>t.Lastpostid).Take(16).ToList();
    //        Discuz.Common.Generic.List<TopicInfo> topicList = new List<TopicInfo>();
    //        foreach (var topic  in topicInfoList)
    //        {
    //            topicList.Add(LoadTopicInfo(topic));
    //        }
    //        db.Disconnect();
    //        return topicList;

       //private TopicInfo LoadTopicInfo(Discuz.EntLib.MongoDB.Entity.TopicInfo topic)
       // {
       //     TopicInfo topicinfo = new TopicInfo();
       //     topicinfo.Tid = topic.Tid;
       //     topicinfo.Fid = topic.Fid;
       //     topicinfo.Iconid = topic.Iconid;
       //     topicinfo.Typeid = topic.Typeid;
       //     topicinfo.Readperm = topic.Readperm;
       //     topicinfo.Price = topic.Price;
       //     topicinfo.Poster = topic.Poster;
       //     topicinfo.Posterid = topic.Posterid;
       //     topicinfo.Title = topic.Title;
       //     topicinfo.Attention = topic.Attention;
       //     topicinfo.Postdatetime =  UnixDateTimeHelper.ConvertFromUnixTimestamp(TypeConverter.StrToInt(topic.Postdatetime)).ToString();
       //     topicinfo.Lastpost =  UnixDateTimeHelper.ConvertFromUnixTimestamp(TypeConverter.StrToInt(topic.Postdatetime)).ToString();
       //     topicinfo.Lastpostid = topic.Lastpostid;
       //     topicinfo.Lastposter = topic.Lastposter;
       //     topicinfo.Lastposterid = topic.Lastposterid;
       //     topicinfo.Views = topic.Views;
       //     topicinfo.Replies = topic.Replies;
       //     topicinfo.Displayorder = topic.Displayorder;
       //     topicinfo.Highlight = topic.Highlight;
       //     topicinfo.Digest = topic.Digest;
       //     topicinfo.Rate = topic.Rate;
       //     topicinfo.Hide = topic.Hide;
       //     topicinfo.Attachment = topic.Attachment;
       //     topicinfo.Moderated = topic.Moderated;
       //     topicinfo.Closed = topic.Closed;
       //     topicinfo.Magic = topic.Magic;
       //     topicinfo.Identify = topic.Identify;
       //     topicinfo.Special = (byte)topic.Special;
       //     return topicinfo;
       // }

    /// <summary>
    /// 主题信息描述类
    /// </summary>
    public class TopicInfo : Discuz.Entity.TopicInfo
    {
        public int _id { get; set; }
       

        [MongoAlias("attention")]
        public new int Attention { get; set; }
        

        ///<summary>
        ///主题tid
        ///</summary>
        [MongoAlias("tid")]
        public new int Tid { get; set; }
        
        /// <summary>
        /// 板块名称
        /// </summary>
        [MongoAlias("forumname")]
        public new string Forumname { get; set; }
       
        ///<summary>
        ///版块fid
        ///</summary>
        [MongoAlias("fid")]
        public new int Fid { get; set; }
       
        ///<summary>
        ///主题图标id
        ///</summary>
        [MongoAlias("iconid")]
        public new int Iconid { get; set; }
       
        ///<summary>
        ///主题分类id
        ///</summary>
        [MongoAlias("typeid")]
        public new int Typeid { get; set; }
       
        ///<summary>
        ///阅读权限
        ///</summary>
        [MongoAlias("readperm")]
        public new int Readperm { get; set; }
        
        ///<summary>
        ///主题出售价格积分
        ///</summary>
        [MongoAlias("price")]
        public new int Price { get; set; }
        
        ///<summary>
        ///作者
        ///</summary>
        [MongoAlias("poster")]
        public new string Poster { get; set; }
       
        ///<summary>
        ///作者uid
        ///</summary>
        [MongoAlias("posterid")]
        public new int Posterid { get; set; }
       
        ///<summary>
        ///标题
        ///</summary>
        [MongoAlias("title")]
        public new string Title { get; set; }
       
        ///<summary>
        ///发布时间
        ///</summary>
        [MongoAlias("postdatetime")]
        public new string Postdatetime { get; set; }
       
        ///<summary>
        ///最后回复时间
        ///</summary>
        [MongoAlias("lastpost")]
        public new string Lastpost { get; set; }
       
        ///<summary>
        ///最后回复帖子ID
        ///</summary>
        [MongoAlias("lastpostid")]
        public new int Lastpostid { get; set; }
        
        ///<summary>
        ///最后回复用户名
        ///</summary>
        [MongoAlias("lastposter")]
        public new string Lastposter { get; set; }        

        ///<summary>
        ///最后回复用户名ID
        ///</summary>
        [MongoAlias("lastposterid")]
        public new int Lastposterid { get; set; }
       
        ///<summary>
        ///查看数
        ///</summary>
        [MongoAlias("views")]
        public new int Views { get; set; }
       
        ///<summary>
        ///回复数
        ///</summary>
        [MongoAlias("replies")]
        public new int Replies { get; set; }
        
        ///<summary>
        ///>0为置顶,小于0不显示,==0正常
        ///</summary>
        [MongoAlias("displayorder")]
        public new int Displayorder { get; set; }
        
        ///<summary>
        ///主题高亮识别号
        ///</summary>
        [MongoAlias("highlight")]
        public new string Highlight { get; set; }
        
        ///<summary>
        ///精华级别,1~3
        ///</summary>
        [MongoAlias("digest")]
        public new int Digest { get; set; }
        
        ///<summary>
        ///评分分数
        ///</summary>
        [MongoAlias("rate")]
        public new int Rate { get; set; }
      
        ///<summary>
        ///是否为回复可见帖
        ///</summary>
        [MongoAlias("hide")]
        public new int Hide { get; set; }
       
        ///<summary>
        ///是否是投票帖
        ///</summary>
        //public int Poll
        //{
        //    get { return m_poll;}
        //    set { m_poll = value;}
        //}
        ///<summary>
        ///是否含有附件
        ///</summary>
        [MongoAlias("attachment")]
        public new int Attachment { get; set; }        
        ///<summary>
        ///是否被执行管理操作
        ///</summary>
        [MongoAlias("moderated")]
        public new int Moderated { get; set; }       

        ///<summary>
        ///是否关闭,如果数值>1,值代表转向目标主题的tid
        ///</summary>
        [MongoAlias("closed")]
        public new int Closed { get; set; }       

        ///<summary>
        ///魔法id,按照附加位/htmltitle(1位)/magic(3位)/tag(1位)/以后扩展（未知位数） 的方式来存储
        ///</summary>
        [MongoAlias("magic")]
        public new int Magic{ get; set; }        

        /// <summary>
        /// 鉴定Id
        /// </summary>
        [MongoAlias("identify")]
        public new int Identify { get; set; }
       
        /// <summary>
        /// 0=普通主题, 1=投票帖, 2=正在进行的悬赏帖, 3=结束的悬赏帖, 4=辩论帖
        /// </summary>
        [MongoAlias("special")]
        public new byte Special { get; set; }

        #region 附加属性

        public new string Folder { get; set; }


        public new string Topictypename { get; set; }
       
        #endregion
     
    }
}
