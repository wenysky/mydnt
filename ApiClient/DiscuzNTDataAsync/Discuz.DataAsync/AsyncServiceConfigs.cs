using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Async.Entity;
using Discuz.Common;
using System.Web;

namespace Discuz.Async.Control
{
    public class AsyncServiceConfigs
    {
        private static object lockHelper = new object();

        private static AsyncServiceConfigInfo configInfo;


        static AsyncServiceConfigs()
        {
            if (configInfo == null)
                configInfo = Deserialize(HttpContext.Current.Server.MapPath("config/service.config"));
        }

        public static AsyncServiceConfigInfo GetConfigInfo()
        {
            return configInfo;
        }


        /// <summary>
        /// 序列化配置信息为XML
        /// </summary>
        /// <param name="configinfo">配置信息</param>
        /// <param name="configFilePath">配置文件完整路径</param>
        public static AsyncServiceConfigInfo Serialiaze(AsyncServiceConfigInfo asyncServiceConfigInfo, string configFilePath)
        {
            lock (lockHelper)
            {
                SerializationHelper.Save(asyncServiceConfigInfo, configFilePath);
            }
            configInfo = null;
            return asyncServiceConfigInfo;
        }

        public static AsyncServiceConfigInfo Deserialize(string configFilePath)
        {
            return (AsyncServiceConfigInfo)SerializationHelper.Load(typeof(AsyncServiceConfigInfo), configFilePath);
        }
    }
}
