using System;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Script.Serialization;

using Discuz.Config;
using Discuz.Common.Generic;

namespace Discuz.EntLib.ToolKit.Controllers
{
    public class DbSnapController : ControllerBase
    {
        //
        // GET: /DbSnap/
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Snap()
        {
            AppendTitle("快照工具");

            List<string> statisticUrl = new List<string>();
            statisticUrl.AddRange(LoadBalanceConfigs.GetConfig().SiteUrl.
                Replace("tools/", "tools/SnapTools.ashx").Split(','));
            if (statisticUrl.Count > 0)
                ViewData["StatisticUrl"] = statisticUrl;

            ViewData["ViewName"] = "Snap";
            return View();
        }


        /// <summary>
        /// 快照Action
        /// </summary>
        /// <param name="opName">操作名称</param>
        /// <param name="moniterUrl">相关操作要监视的站点URL</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Snap(string opName, string moniterUrl)
        {
            switch(opName)
            {
                case "GetSnapState" : //获取快照链接状态
                    ViewData["DbSnapState"] = DbSnapHelper.TestDbSnapState(); break;

                case "GetSnapLogList"://获取快照日志列表
                    {
                        if (!string.IsNullOrEmpty(moniterUrl))
                            ViewData["SnapLogList"] = GetWebSiteSnapLogList(moniterUrl);
                      
                        break;
                    }

                case "ClearSnapLogList"://清空快照日志列表
                    {
                        foreach (string url in moniterUrl.Split(','))
                        {
                            if (!string.IsNullOrEmpty(url))
                                ClearPageViews(url);
                        }
                        break;
                    }         
            }

            ViewData["MoniterUrl"] = moniterUrl;
            return Snap();
        }

        /// <summary>
        /// 获取指定URL站点的快照日志信息列表
        /// </summary>
        /// <param name="moniterUrl">指定监视的站点url</param>
        /// <returns></returns>
        private List<WebSiteSnapLogInfo> GetWebSiteSnapLogList(string moniterUrl)
        {
            List<WebSiteSnapLogInfo> webSiteSnapLogInfoList = new List<WebSiteSnapLogInfo>();

            if (!string.IsNullOrEmpty(moniterUrl))
            {
                foreach (string url in moniterUrl.Split(','))
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        List<SnapLogInfo> dbSnapLogInfoList = GetDbSnapLogList(url);
                        List<SnapSourceSum> snapSourceSum = new List<SnapSourceSum>();
                        foreach (DbSnapInfo dbSnapInfo in DbSnapConfigs.GetEnableSnapList())
                        {
                            //计算相应快照被访问的次数
                            snapSourceSum.Add(
                                new SnapSourceSum()
                                {
                                    SouceID = dbSnapInfo.SouceID,
                                    DbconnectString = dbSnapInfo.DbconnectString,
                                    Enable = dbSnapInfo.Enable,
                                    //Sum = (from s in dbSnapLogInfoList
                                    //       where s.SouceID == dbSnapInfo.SouceID
                                    //       select new { s.SouceID }).Count()
                                    Sum = dbSnapLogInfoList.Where(s => s.SouceID == dbSnapInfo.SouceID).Count()
                                });

                        }
                        //绑定显示信息
                        webSiteSnapLogInfoList.Add(
                            new WebSiteSnapLogInfo()
                            {
                                WebSiteName = url,
                                SnapLogInfoList = dbSnapLogInfoList,
                                SnapSourceSumList = snapSourceSum
                            });
                    }
                }                
            }
            return webSiteSnapLogInfoList;
        }

        /// <summary>
        /// 获取指定url地址上的快照日志列表信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<SnapLogInfo> GetDbSnapLogList(string url)
        {
            List<SnapLogInfo> snapLogList = null;
            try
            {
                //url = "http://localhost:4335/ServerTool/SnapTools.ashx";
                snapLogList = Utils.GetWebClient<List<SnapLogInfo>>(Utils.CreateRequestUrl(url, "getSnapLog"));
            }
            catch (Exception e)
            {
                ViewData["OpInfo"] = ViewData["OpInfo"].AppendOpInfo(url + " " + e.Message.HtmlEncode());
            }
            return snapLogList == null ? new List<SnapLogInfo>() : snapLogList;
        }

        /// <summary>
        /// 清空(重置)指定url地址上的快照日志列表信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool ClearPageViews(string url)
        {
            try
            {
                //url = "http://localhost:4335/ServerTool/SnapTools.ashx";
                if (Utils.GetWebClient(Utils.CreateRequestUrl(url, "clearSnapLog")) == "OK")
                    return true;
            }
            catch (Exception e)
            {
                ViewData["OpInfo"] = ViewData["OpInfo"].AppendOpInfo(url + " " + e.Message.HtmlEncode());
            }
            return false;
        }

    }
}
