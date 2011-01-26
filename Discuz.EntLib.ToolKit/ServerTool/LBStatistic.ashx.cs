using System;
using System.Globalization;
using System.Web;
using System.Web.Services;
using System.Diagnostics;
using System.Threading;
using System.Text;
using System.Net;
using System.IO;

using Discuz.Common.Generic;
using Discuz.Config;

namespace Discuz.EntLib.ToolKit.ToolPage
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LBStatistic : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
         
            if (base.CheckAuthCode(context))
            {
                if (!LoadBalanceStatic)
                {
                    context.Response.Write("[noData]");
                    return;
                }

                switch (context.Request.QueryString["opName"])
                {
                    case "pageViewStatistic"://获取页面统计信息
                        {
                            context.Response.Write(GetPageViewStatisticJson());
                            return;
                        }
                    case "clearPageView"://清空PageView统计信息
                        {
                            Discuz.Forum.PageBase.PageViewSatisticInfo = new Dictionary<string, int>();
                            context.Response.Write("OK");
                            return;
                        }
                }
                base.NotValidParameter(context);
            }
        }

        public string GetPageViewStatisticJson()
        {
            StringBuilder message = new StringBuilder();
            message.Append("[\r\n");

            Dictionary<string, int> pageStatistic = Discuz.Forum.PageBase.PageViewSatisticInfo;
            pageStatistic.Add(Discuz.Common.Utils.GetDateTime(), -1); //统计结束时间

            foreach (var page in pageStatistic)
                message.AppendFormat("{{'PageName' : '{0}', 'Views' : {1}}},", page.Key, page.Value);

            return message.ToString().Trim(',') +"\r\n]";
        }

    }
}
