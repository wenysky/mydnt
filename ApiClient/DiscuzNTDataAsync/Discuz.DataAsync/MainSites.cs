using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Async.Entity;
using Discuz.Common;
using System.Web;

namespace Discuz.Async.Control
{
    public class MainSites
    {
        private static object lockHelper = new object();

        private static MainSiteInfo mainSiteInfo;

        static MainSites()
        {
            if (mainSiteInfo == null)
                mainSiteInfo = Deserialize(HttpContext.Current.Server.MapPath("config/mainsite.config"));
        }

        public static MainSiteInfo GetMainSiteInfo()
        {
            return mainSiteInfo;
        }

        /// <summary>
        /// 序列化配置信息为XML
        /// </summary>
        /// <param name="configinfo">配置信息</param>
        /// <param name="configFilePath">配置文件完整路径</param>
        public static MainSiteInfo Serialiaze(MainSiteInfo siteInfo, string configFilePath)
        {
            lock (lockHelper)
            {
                SerializationHelper.Save(siteInfo, configFilePath);
            }
            mainSiteInfo = null;
            return siteInfo;
        }

        private static MainSiteInfo Deserialize(string configFilePath)
        {
            return (MainSiteInfo)SerializationHelper.Load(typeof(MainSiteInfo), configFilePath);
        }
    }
}
