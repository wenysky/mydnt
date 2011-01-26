using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Discuz.Common;
using Discuz.Async.Entity;
using Discuz.Async.Control;
using Discuz.Common.Generic;

namespace Discuz.Async.Web
{
    public partial class websitemanage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!DNTRequest.IsPost())
                return;

            List<AsyncSiteInfo> asyncSiteList = new List<AsyncSiteInfo>();

            try
            {
                asyncSiteList = AsyncSites.Deserialize(Server.MapPath("config/asyncsite.config"));
            }
            catch { }

            if (DNTRequest.GetString("sitetype") == "main")
            {
                MainSiteInfo mainSiteInfo = new MainSiteInfo();
                mainSiteInfo.Apikey = DNTRequest.GetString("apikey");
                mainSiteInfo.AsyncList = DNTRequest.GetString("asyncdatalist");
                mainSiteInfo.AsyncUrl = DNTRequest.GetString("asyncurl");
                mainSiteInfo.CallbackUrl = DNTRequest.GetString("callbackurl");
                mainSiteInfo.Security = DNTRequest.GetString("security");
                mainSiteInfo.WebsiteUrl = DNTRequest.GetString("websiteurl");

                MainSites.Serialiaze(mainSiteInfo, Server.MapPath("config/mainsite.config"));
            }
            else
            {
                AsyncSiteInfo asyncSiteInfo = new AsyncSiteInfo();
                asyncSiteInfo.Apikey = DNTRequest.GetString("apikey");
                asyncSiteInfo.AsyncList = DNTRequest.GetString("asyncdatalist");
                asyncSiteInfo.AsyncUrl = DNTRequest.GetString("asyncurl");
                asyncSiteInfo.CallbackUrl = DNTRequest.GetString("callbackurl");
                asyncSiteInfo.Security = DNTRequest.GetString("security");
                asyncSiteInfo.WebsiteUrl = DNTRequest.GetString("websiteurl");

                asyncSiteList.Add(asyncSiteInfo);

                AsyncSites.Serialiaze(asyncSiteList, Server.MapPath("config/asyncsite.config"));
            }
        }
    }
}
