using System;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Script.Serialization; 

using Discuz.Common.Generic;
using Discuz.Config;

namespace Discuz.EntLib.ToolKit.Controllers
{
    /// <summary>
    /// ���ؾ�����ͼ�ؼ���
    /// </summary>
    public class LoadBalanceController : ControllerBase
    {
   
        public ActionResult Index()
        {
            return View();
        }

        #region ���ܼ���
        /// <summary>
        /// ���ܼ���
        /// </summary>
        /// <returns></returns>
        public ActionResult PerfMoniter()
        {
            AppendTitle("���ؼ�Ⱥ�����¸�������PV");

            List<string> moniterUrl = new List<string>();
            moniterUrl.AddRange(LoadBalanceConfigs.GetConfig().SiteUrl.
                Replace("http","'http").
                Replace("tools/", "tools/ComputerPerfMoniter.ashx'").Split(','));
            ViewData["MoniterUrl"] = moniterUrl.ToArray();
            return View();
        }
        #endregion

        #region PV Action
        /// <summary>
        /// PV Action
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        public ActionResult PageViews()
        {
            AppendTitle("���ܼ�����");

            List<string> statisticUrl = new List<string>();
            statisticUrl.AddRange(LoadBalanceConfigs.GetConfig().SiteUrl.
                Replace("tools/", "tools/LBStatistic.ashx").Split(','));
            if (statisticUrl.Count > 0)
                ViewData["StatisticUrl"] = statisticUrl;

            ViewData["ViewName"] = "PageViews";
            return View();
        }

        /// <summary>
        /// PV Action
        /// </summary>
        /// <param name="moniterUrl">ָ����url(������)</param>
        /// <param name="opName">��������</opName>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PageViews(string moniterUrl, string opName)
        {
            if (!string.IsNullOrEmpty(moniterUrl))
            {
                if (opName == "ClearPV")
                    ClearPV(moniterUrl);
                else
                    GetPageView(moniterUrl);
            }
            ViewData["MoniterUrl"] = moniterUrl;
            return PageViews();
        }

        /// <summary>
        /// ��ȡָ��url(������)��PV
        /// </summary>
        /// <param name="moniterUrl">ָ���ķ�����url�б�</param>
        /// <returns></returns>
        private void GetPageView(string moniterUrl)
        {
            List<WebSitePageViewInfo> pageViewList = new List<WebSitePageViewInfo>();
            foreach (string url in moniterUrl.Split(','))
            {
                if (!string.IsNullOrEmpty(url))
                {
                    List<PageViewInfo> pageViewInfoList = GetServerPageViews(url);
                    int pageViewSum = pageViewInfoList.Count > 2 ? pageViewInfoList.Sum(page => page.Views) : 0;

                    pageViewList.Add(new WebSitePageViewInfo()
                    {
                        WebSiteName = url,
                        ViewSum = pageViewSum < 0 ? 0 : pageViewSum,
                        PageViewInfoList = pageViewInfoList
                    });
                }
            }
            ViewData["pageViewList"] = pageViewList;
        }


        /// <summary>
        /// ���(����)ָ��url��PV��Ϣ
        /// </summary>
        /// <param name="moniterUrl">ָ����url�б�</param>
        /// <returns></returns>
        private ActionResult ClearPV(string moniterUrl)
        {
            foreach (string url in moniterUrl.Split(','))
            {
                if (!string.IsNullOrEmpty(url))
                {
                    ClearPageViews(url);
                }
            }
            return PageViews();
        }

        /// <summary>
        /// ��ȡָ��url(������)��PV
        /// </summary>
        /// <param name="moniterUrl">ָ���ķ�����url</param>
        /// <returns></returns>
        public List<PageViewInfo> GetServerPageViews(string url)
        {
            List<PageViewInfo> pageViewList = new List<PageViewInfo>();
            try
            {
                //url = "http://localhost:4335/ServerTool/LBStatistic.ashx";
                pageViewList = Utils.GetWebClient<List<PageViewInfo>>(Utils.CreateRequestUrl(url, "pageViewStatistic"));
            }
            catch (Exception e)
            {
                ViewData["OpInfo"] = ViewData["OpInfo"].AppendOpInfo(url + " " + e.Message.HtmlEncode());
            }
            return pageViewList;
        }

        /// <summary>
        /// ���(����)ָ��url��PV��Ϣ
        /// </summary>
        /// <param name="url">ָ����url</param>
        /// <returns></returns>
        public bool ClearPageViews(string url)
        {
            try
            {
                //url = "http://localhost:4335/ServerTool/LBStatistic.ashx";
                if (Utils.GetWebClient(Utils.CreateRequestUrl(url, "clearPageView")) == "OK")
                    return true;
            }
            catch (Exception e)
            {
                ViewData["OpInfo"] = ViewData["OpInfo"].AppendOpInfo(url + " " + e.Message.HtmlEncode());
            }
            return false;
        }
        #endregion
    }
}
