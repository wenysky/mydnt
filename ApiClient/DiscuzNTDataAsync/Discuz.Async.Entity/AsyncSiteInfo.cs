using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Async.Entity
{
    [Serializable]
    public class AsyncSiteInfo
    {
        private string apikey = "";

        public string Apikey
        {
            get { return apikey; }
            set { apikey = value; }
        }

        private string security = "";

        public string Security
        {
            get { return security; }
            set { security = value; }
        }

        private string websiteUrl = "";

        public string WebsiteUrl
        {
            get { return websiteUrl; }
            set { websiteUrl = value; }
        }

        private string callbackUrl = "";

        public string CallbackUrl
        {
            get { return callbackUrl; }
            set { callbackUrl = value; }
        }

        private string asyncUrl = "";

        public string AsyncUrl
        {
            get { return asyncUrl; }
            set { asyncUrl = value; }
        }

        private string asyncList = "";

        public string AsyncList
        {
            get { return asyncList; }
            set { asyncList = value; }
        }
    }
}
