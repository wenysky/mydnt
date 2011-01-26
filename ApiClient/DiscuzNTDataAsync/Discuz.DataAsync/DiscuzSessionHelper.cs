using System;
using System.Collections.Generic;
using System.Text;

using Discuz.Toolkit;
using Discuz.Async.Entity;
using System.Web;

namespace Discuz.Async.Control
{
    public class DiscuzSessionHelper
    {
        private static MainSiteInfo mainSiteInfo = MainSites.GetMainSiteInfo();

        private static DiscuzSession mainSiteDs;

        public static DiscuzSession GetMainSiteSession()
        {
            if (mainSiteDs == null)
            {
                mainSiteDs = new DiscuzSession(mainSiteInfo.Apikey, mainSiteInfo.Security, mainSiteInfo.WebsiteUrl);
            }
            return mainSiteDs;
        }

        public static DiscuzSession GetAsyncSiteSession(AsyncSiteInfo asyncSiteInfo)
        {
            return new DiscuzSession(asyncSiteInfo.Apikey, asyncSiteInfo.Security, asyncSiteInfo.WebsiteUrl);
        }
    }
}
