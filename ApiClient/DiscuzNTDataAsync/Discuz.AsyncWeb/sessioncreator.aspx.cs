using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Discuz.Toolkit;
using Discuz.Async.Control;
using Discuz.Common;
using Discuz.Async.Entity;

namespace Discuz.Async.Web
{
    public class sessioncreator : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DiscuzSession ds = DiscuzSessionHelper.GetMainSiteSession();

            HttpContextSession authToken = new HttpContextSession("AuthToken");

            int errorcode = 0;
            string next = DNTRequest.GetString("next");

            if (authToken.GetSession() == null || DNTRequest.GetString("auth_token") != string.Empty)
                authToken.SetSession(DNTRequest.GetString("auth_token"));

            if (ValidateAuthToken(ds, ref errorcode))
                RedirectPage(next);
            else
                GetAuthToken(next, ds);
        }

        //获取AuthToken
        public void GetAuthToken(string n, DiscuzSession ds)
        {
            Response.Redirect(ds.CreateToken().ToString() + "&next=" + n);
        }

        //成功更新AuthToken后进行页面转向
        public void RedirectPage(string n)
        {
            //switch (n)
            //{
            //    //case "default": Response.Redirect("default.aspx"); break;
            //    //case "usermanage": Response.Redirect("usermanage.aspx"); break;
            //    //case "topicmanage": Response.Redirect("topicsmanage.aspx"); break;
            //    //case "msgop": Response.Redirect("messageop.aspx"); break;
            //    case "asyncserver": Response.Redirect("asyncserver.aspx?siteid=" + n); break;
            //    default: break;
            //}
            Response.Redirect("asyncserver.aspx?" + n.Replace(":","=").Replace("-","&"));
        }

        //验证当前的AuthToken是否可用
        public bool ValidateAuthToken(DiscuzSession ds, ref int Errorcode)
        {
            try
            {
                ds.session_info = ds.GetSessionFromToken(new HttpContextSession("AuthToken").GetSession().ToString());
                Errorcode = 0;
                return true;
            }
            catch (DiscuzException d)
            {
                Errorcode = d.ErrorCode;
                return false;
            }
            catch (NullReferenceException)
            {
                Errorcode = 0;
                return false;
            }
        }
    }
}
