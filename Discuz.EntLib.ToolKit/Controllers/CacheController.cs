using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Discuz.EntLib.ToolKit.Controllers
{
    /// <summary>
    /// 缓存控制器
    /// </summary>
    public class CacheController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult SyncCache()
        {
            AppendTitle("缓存同步");
            return View();
        }
        
        /// <summary>
        /// 缓存同步方法
        /// </summary>
        /// <param name="cacheKeyArray">缓存的键值数组</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SyncCache(string cacheKeyArray)
        {
            if (!string.IsNullOrEmpty(cacheKeyArray))
            {
                foreach (string cacheKey in cacheKeyArray.Split(','))
                {
                    if(!string.IsNullOrEmpty(cacheKey))
                        Discuz.EntLib.SyncCache.SyncRemoteCache(cacheKey);
                }
                ViewData["OpInfo"] = "已完成缓存同步!";
            }
            return View();
        }  

        public ActionResult MoveCache()
        {
            AppendTitle("缓存迁移");
            return View();
        }

        ///// <summary>
        ///// 缓存同步方法
        ///// </summary>
        ///// <param name="cacheKeyArray">缓存的键值数组</param>
        ///// <returns></returns>
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult MoveCache(string opName, string SourceTb)
        //{
        //    ArrayList arrayList = new ArrayList();
        //    arrayList.Add(serverList);

        //    //SourceKeyList.DataSource = MemCachedManager.GetCachedKeyList(arrayList, "sourceCache");
        //    //SourceKeyList.DataBind();

        //    //foreach (ListItem key in SourceKeyList.Items)
        //    //{
        //    //    key.Selected = true;
        //    //}
        //    return View();
        //}  
        
        /// <summary>
        /// 缓存同步方法
        /// </summary>
        /// <param name="cacheKeyArray">缓存的键值数组</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MoveCache(string opName, string SourceTb, string TargetTb, string selectedSourceCacheKey)
        {
            ArrayList sourceList = new ArrayList();
            ArrayList targetList = new ArrayList();
            if (!string.IsNullOrEmpty(SourceTb))
            {
                sourceList.Add(SourceTb);
                ViewData["sourceCache"] = MemCachedManager.GetCachedKeyList(sourceList, "sourceCache");
            }
            if (!string.IsNullOrEmpty(TargetTb))
                targetList.Add(TargetTb);

            if (opName == "MoveCache" && !string.IsNullOrEmpty(SourceTb) && !string.IsNullOrEmpty(TargetTb) && !string.IsNullOrEmpty(selectedSourceCacheKey))
            {  
                MemcachedClient sourceMC = MemCachedManager.GetMemcachedClient("sourceCache", sourceList);
                MemcachedClient targetMC = MemCachedManager.GetMemcachedClient("tagetCache", targetList);

                foreach (object key in selectedSourceCacheKey.Split(','))
                {
                    if (!string.IsNullOrEmpty(key.ToString()) && key.ToString()!="on")
                    {
                        object o = sourceMC.Get(key.ToString());
                        targetMC.Add(key.ToString(), o);
                    }
                }
            }

            if (!string.IsNullOrEmpty(TargetTb))
                ViewData["targetCache"] = MemCachedManager.GetCachedKeyList(targetList, "targetCache");

            return View();
        }
    }
}
