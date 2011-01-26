using System;
using System.Collections.Generic;

using Discuz.Config;

namespace Discuz.EntLib.ToolKit
{
    /// <summary>
    /// 页面浏览量信息类
    /// </summary>
    [Serializable]
    public class PageViewInfo
    {
        /// <summary>
        /// 页面名称
        /// </summary>
        public string PageName;
        /// <summary>
        /// 浏览量
        /// </summary>
        public int Views;
    }

    /// <summary>
    /// WEB站点页面浏览量信息类
    /// </summary>
    [Serializable]
    public class WebSitePageViewInfo
    {
        /// <summary>
        /// 站点名称
        /// </summary>
        public string WebSiteName;
        /// <summary>
        /// 浏览总量
        /// </summary>
        public int ViewSum;
        /// <summary>
        /// 相关页面浏览量信息列表
        /// </summary>
        public List<PageViewInfo> PageViewInfoList;
    }

    /// <summary>
    /// 快照日志信息类
    /// </summary>
    [Serializable]
    public class SnapLogInfo
    {
        public int SouceID;
        public string DbconnectString;
        public string CommandText;
        public string PostDateTime;
    }

    /// <summary>
    /// 相关快照访问量汇总
    /// </summary>
    public class SnapSourceSum : DbSnapInfo
    {
        public int Sum;
    }

    /// <summary>
    /// WEB站点快照日志信息类
    /// </summary>
    [Serializable]
    public class WebSiteSnapLogInfo
    {
        public string WebSiteName;
        public List<SnapLogInfo> SnapLogInfoList;
        public List<SnapSourceSum> SnapSourceSumList;
    }   
}