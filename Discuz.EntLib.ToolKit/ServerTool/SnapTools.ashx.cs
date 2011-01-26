using System;
using System.Globalization;
using System.Web;
using System.Web.Services;
using System.Diagnostics;
using System.Threading;
using System.Text;
using System.Net;
using System.IO;

using Discuz.Config;

namespace Discuz.EntLib.ToolKit.ServerTool
{
    /// <summary>
    /// 快照工具
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SnapTools : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);

            if (base.CheckAuthCode(context))
            {
                if (!DbSnapStatistics) //判断是否开始快照日志记录功能
                {
                    context.Response.Write("[no data or RecordeLog is false]");
                    return;
                }

                switch (context.Request.QueryString["opName"])
                {
                    case "getSnapLog"://获取快照日志统计信息
                        {
                            context.Response.Write(JsonCharFilter(Discuz.Data.DbHelper.GetSnapLogJson()));
                            return;
                        }
                    case "clearSnapLog"://清空快照日志统计信息
                        {
                            Discuz.Data.DbHelper.ClearSnapLogJson();
                            context.Response.Write("OK");
                            return;
                        }
                }
                base.NotValidParameter(context);
            }
        }

        /// <summary>
        /// Json特符字符过滤，参见http://www.json.org/
        /// </summary>
        /// <param name="sourceStr">要过滤的源字符串</param>
        /// <returns></returns>
        public string JsonCharFilter(string sourceStr)
        {
            sourceStr = sourceStr.Replace("\\", "\\\\");
            sourceStr = sourceStr.Replace("\b", "\\\b");
            sourceStr = sourceStr.Replace("\t", "\\\t");
            sourceStr = sourceStr.Replace("\n", "\\\n");
            sourceStr = sourceStr.Replace("\n", "\\\n");
            sourceStr = sourceStr.Replace("\f", "\\\f");
            sourceStr = sourceStr.Replace("\r", "\\\r");
            return sourceStr.Replace("\"", "\\\"");//.Replace("\'", "\"");
        }

    }
}
