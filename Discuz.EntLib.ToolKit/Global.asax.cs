using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Discuz.EntLib.ToolKit
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "1" }  // Parameter defaults
            );

            //routes.Add(new Route("PageSample/Pager/{Page}", new RouteValueDictionary { { "Page", 1 } }, new MvcRouteHandler()));

            //routes.MapRoute(
            //    "PageSample",
            //    "Pager/{action}/{pageid}",
            //    new { controller = "PageSample", action = "Pager", pageid = "1" },// controller:文件夹  action:文件名称
            //    new { pageid = @"\d{1}" });//此处可不发分大小写
            //sample: http://localhost:4335/Pager/Pager/2

            //http://localhost:4335/archive/2008-05-07
            //routes.MapRoute(
            //    "BlogArchive",
            //    "Archive/{Date}",
            //    new { controller = "blog", action = "archive" },// controller:文件夹  action:文件名称
            //    new { date = @"\d{4}-\d{2}-\d{2}" });//此处可不发分大小写


        }

        protected void Application_Start()
        {
            //System.Web.Mvc.ViewEngines.Engines.Add(new ViewEngines.StringTemplateViewEngine());

            RegisterRoutes(RouteTable.Routes);
            //http://files.cnblogs.com/zhangziqiu/RouteDebug-Binary.zip
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }
    }
}