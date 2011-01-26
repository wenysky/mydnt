using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

using Discuz.Config;

namespace Discuz.EntLib.ToolKit
{
    public class Utils
    {
        public static string GetWebClient(string url)
        {
            WebClient client = new WebClient();
            client.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322)");
            StreamReader reader = new StreamReader(client.OpenRead(url), Encoding.UTF8);
            string result = reader.ReadToEnd().Trim();
            reader.Close();
            return result;
        }

        public static T GetWebClient<T>(string url)
        {
            string result = GetWebClient(url);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = result.Length;
            return javaScriptSerializer.Deserialize<T>(result);
        }

        public static string CreateRequestUrl(string url, string opName)
        {
             return string.Format("{0}?opName={1}&passKey={2}", 
                                 url, 
                                 opName,
                                 Discuz.Common.Utils.UrlEncode(Discuz.Common.DES.Encode(opName, LoadBalanceConfigs.GetConfig().AuthCode)));
        }

        public static string GetWebRequest(string url)
        {
            StringBuilder builder = new StringBuilder();

            WebRequest request = WebRequest.Create(new Uri(url));
            request.Method = "GET";
            request.Timeout = 4000;
            request.ContentType = "Text/XML";
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    builder.Append(reader.ReadToEnd());
                }
            }
            return builder.ToString();
        }

    }
}
