using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web;

using Discuz.Cache;
using Discuz.Common;

namespace Discuz.EntLib
{
    /// <summary>
    /// 同步本地缓存
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SyncLocalCache : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        { 
            context.Response.ContentType = "text/plain";
            string cacheKey = context.Request.QueryString["cacheKey"];
            string passKey = context.Request.QueryString["passKey"];

            if (Utils.StrIsNullOrEmpty(cacheKey))
            {
                context.Response.Write("CacheKey is not null!");
                return;
            }
            if (!cacheKey.StartsWith("/Forum"))
            {
                context.Response.Write("CacheKey is not valid!");
                return;
            }
            if (passKey != Discuz.Common.DES.Encode(cacheKey, Discuz.Config.LoadBalanceConfigs.GetConfig().AuthCode))
            {
                context.Response.Write("AuthCode is not valid!");
                return;
            }
            ////hack:如果是该缓存键，则直接修改web.config重启IIS
            //if (cacheKey == "/Forum/PostTables_PostTableID")
            //    Utils.RestartIISProcess();
            //else
            {
                //更新本地缓存（注：此处不可使用MemCachedStrategy的RemoveObject方法，因为该方法中有SyncRemoteCache的调用，会造成循环调用）
                Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
                cache.LoadCacheStrategy(new DefaultCacheStrategy());
                cache.RemoveObject(cacheKey);
                cache.LoadDefaultCacheStrategy();
            }
            context.Response.Write("OK");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
