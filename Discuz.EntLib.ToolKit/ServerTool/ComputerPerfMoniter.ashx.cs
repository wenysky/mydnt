using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Services;
using System.Diagnostics;
using System.Threading;
using System.Text;

using Discuz.Common.Generic;
using Discuz.Config;

namespace Discuz.EntLib.ToolKit
{    
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ComputerPerfMoniter : HttpHandlerBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
  
            if (!LoadBalanceStatic)
            {
                context.Response.Write("noData");
                return;
            }

            StringBuilder output = new StringBuilder();
            DateTimeFormatInfo formatInfo = new DateTimeFormatInfo();
            formatInfo.DateSeparator = "/";
            output.Append("[");

            PerformanceCounter counter = PerfCounterFactory.Instance.GetCounter("Processor|% Processor Time|_Total|.");
            output.AppendFormat("[new Date(\"{0}\").getTime()+28800000, {1}]",
                                          DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss", formatInfo),
                                          counter.NextValue().ToString("0.0"));

            output.Append(",");
            counter = PerfCounterFactory.Instance.GetCounter("Memory|Available Bytes||.");
            output.AppendFormat("[new Date(\"{0}\").getTime()+28800000, {1}]",
                                          DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss", formatInfo),
                                          counter.NextValue().ToString("0.0"));

            //output.Append(",");
            //output.AppendFormat("[new Date(\"{0}\").getTime()+28800000, {1}]",
            //                             DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss", formatInfo),
            //                             //new SystemData().GetDiskData(SystemData.DiskData.ReadAndWrite).ToString("0.0"));
            //                             new SystemData().GetNetData(SystemData.NetData.ReceivedAndSent).ToString("0.0"));

            output.AppendFormat(",[{0}]]", context.Request.QueryString["index"]);//要更新的显示区域索引

            context.Response.Write(output);
        }
    }
}
