using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Discuz.EntLib.ToolKit
{
    public abstract class ControllerBase : Controller
    {
        public virtual string GetControllerName()
        {
            return " - {0}".With(GetType().Name.Replace("Controller", ""));
        }


        public virtual void AppendTitle(string text)
        {
            ViewData["Title"] = "{0} - {1}".With("Discuz!NT读写分离与负载均衡工具包", text);
        }

        public virtual void AppendMetaDescription(string text)
        {
            ViewData["MetaDescription"] = text;
        }

        public string Message
        {
            get { return TempData["message"] as string; }
            set { TempData["message"] = value; }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Response.Clear();
            base.OnException(filterContext);
        }
    }
}
