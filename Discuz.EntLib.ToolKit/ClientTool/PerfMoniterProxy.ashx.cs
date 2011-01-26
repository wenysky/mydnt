using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Services;
using System.Diagnostics;
using System.Threading;
using System.Text;
using System.Net;
using System.IO;

namespace Discuz.EntLib.ToolKit
{
    /// <summary>
    /// 用于代理转发客户端的性能监视请求到服务端的ComputerPerfMoniter.ashx
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class PerfMoniterProxy : HttpHandlerBase
    {
        static Func<string, string> SendWebRequest = (string url) =>
        {
            try
            {
                return Utils.GetWebRequest(url);
            }
            catch
            {
                return "Process Failed!";
            };
        };

        public override void ProcessRequest(HttpContext context)
        {
            string url = context.Request.QueryString["url"] + "?" + context.Request.QueryString["math"] + "&index=" + context.Request.QueryString["index"];

            base.ProcessRequest(context);

            context.Response.Write(SendWebRequest(url));
            context.Response.End();
        }
    }
}
