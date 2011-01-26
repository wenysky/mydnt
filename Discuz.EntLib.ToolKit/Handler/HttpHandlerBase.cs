using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Discuz.Config;

namespace Discuz.EntLib.ToolKit
{
    public class HttpHandlerBase : IHttpHandler
    {
        protected static bool LoadBalanceStatic = LoadBalanceConfigs.GetConfig() != null &&
                                                  LoadBalanceConfigs.GetConfig().AppLoadBalance &&
                                                  LoadBalanceConfigs.GetConfig().RecordPageView;

        protected static bool DbSnapStatistics = DbSnapConfigs.GetConfig() != null && DbSnapConfigs.GetConfig().AppDbSnap && DbSnapConfigs.GetConfig().RecordeLog;

        public virtual void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            context.Response.Expires = 0;
            context.Response.CacheControl = "no-cache";
        }

        public virtual bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public virtual void NotValidParameter(HttpContext context)
        {
            context.Response.Write("parameter is not valid!");
        }

        public virtual bool CheckAuthCode(HttpContext context)
        {
            string passKey = context.Request.QueryString["passKey"];
            string opName = context.Request.QueryString["opName"];

            if (passKey != Discuz.Common.DES.Encode(opName, Discuz.Config.LoadBalanceConfigs.GetConfig().AuthCode))
            {
                context.Response.Write("AuthCode is not valid!");
                return false;
            }
            return true;
        }

    }
}
