using System;
using System.Web.UI;
using Discuz.Async.Control;
using Discuz.Async.Entity;
using Discuz.Common;
using Discuz.Toolkit;

namespace Discuz.Async.Web
{
    public class asyncserver : Page
    {
        public MainSiteInfo mainSiteInfo = new MainSiteInfo();

        public string asyncSiteId = DNTRequest.GetString("siteid");

        public string asyncType = DNTRequest.GetString("action");

        public asyncserver()
        {
            this.Load += new EventHandler(AsyncServer_Load);
        }

        void AsyncServer_Load(object sender, EventArgs e)
        {
            if (asyncType == "userasync")
            {
                ManualAsyncRegister(); return;
            }

            System.Collections.Generic.Dictionary<string, string> dic = DiscuzSessionHelper.GetMainSiteSession().GetQueryString();

            asyncType = dic.ContainsKey("action") ? dic["action"] : "";
            switch (asyncType)
            {
                case "updatepwd":
                    Response.Write(Users.AsyncUserPassword(dic["user_name"], dic["password"]) + " bbs userinfo async success!");
                    break;
                case "updateprofile":
                    Response.Write(Users.AsyncUserProfile(Utils.StrToInt(dic["uid"], 0)) + " bbs userinfo async success!");
                    break;
                case "register":
                    Response.Write(Users.AsyncRegister(Utils.StrToInt(dic["uid"], 0)) + " bbs register success!");
                    break;
            }
        }

        private void ManualAsyncRegister()
        {
            int result = Users.ManualAsyncRegister(asyncSiteId);
            if (result > 0)
                Response.Write("async OK!");
        }
    }
}
