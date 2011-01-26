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
            AppendTitle("���չ���");

            List<string> statisticUrl = new List<string>();
            statisticUrl.AddRange(LoadBalanceConfigs.GetConfig().SiteUrl.
                Replace("tools/", "tools/SnapTools.ashx").Split(','));
            if (statisticUrl.Count > 0)
                ViewData["StatisticUrl"] = statisticUrl;

            ViewData["ViewName"] = "Snap";
            return View();
        }


        /// <summary>
        /// ����Action
        /// </summary>
        /// <param name="opName">��������</param>
        /// <param name="moniterUrl">��ز���Ҫ���ӵ�վ��URL</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Snap(string opName, string moniterUrl)
        {
            switch(opName)
            {
                case "GetSnapState" : //��ȡ��������״̬
                    ViewData["DbSnapState"] = DbSnapHelper.TestDbSnapState(); break;

                case "GetSnapLogList"://��ȡ������־�б�
                    {
                        if (!string.IsNullOrEmpty(moniterUrl))
                            ViewData["SnapLogList"] = GetWebSiteSnapLogList(moniterUrl);
                      
                        break;
                    }

                case "ClearSnapLogList"://��տ�����־�б�
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
        /// ��ȡָ��URLվ��Ŀ�����־��Ϣ�б�
        /// </summary>
        /// <param name="moniterUrl">ָ�����ӵ�վ��url</param>
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
                            //������Ӧ���ձ����ʵĴ���
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
                        //����ʾ��Ϣ
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
        /// ��ȡָ��url��ַ�ϵĿ�����־�б���Ϣ
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
        /// ���(����)ָ��url��ַ�ϵĿ�����־�б���Ϣ
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
