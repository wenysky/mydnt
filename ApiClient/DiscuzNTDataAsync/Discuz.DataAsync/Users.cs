using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

using Discuz.Async.Entity;
using Discuz.Toolkit;
using Discuz.Common;

namespace Discuz.Async.Control
{
    public class Users
    {
        /// <summary>
        /// 该队列用来缓存已同步用户数据，减少不必要的API访问操作，减轻API压力
        /// </summary>
        public static List<ManualRegLogInfo> ManualRegLogList;

        public static int ManualAsyncRegister(string asyncSiteId)
        {
            if (asyncSiteId == string.Empty)
                return -1;

            DiscuzSession ds = DiscuzSessionHelper.GetMainSiteSession();

            HttpContextSession authToken = new HttpContextSession("AuthToken");

            if (authToken.GetSession() == null || authToken.GetSession().ToString() == "")
                RedirectToGetAuthToken();

            try
            {
                ds.session_info = ds.GetSessionFromToken(authToken.GetSession().ToString());
            }
            catch
            {
                RedirectToGetAuthToken();
            }

            Me me = ds.GetLoggedInUser();

            if (me.UId < 0)
                return -1;

            ManualRegLogInfo RegisteredLog = null;
            if (ManualRegLogList != null)
            {
                Predicate<ManualRegLogInfo> matchParent = new Predicate<ManualRegLogInfo>(delegate(ManualRegLogInfo logInfo)
                {
                    return logInfo.UId == me.UId && logInfo.SiteId == asyncSiteId;
                });
                RegisteredLog = ManualRegLogList.Find(matchParent);
                if (RegisteredLog != null && RegisteredLog.SiteId != "" && RegisteredLog.UId > 0)
                    return -1;

                if (ManualRegLogList.Count > 200)//如果已同步数据队列超过了一定值，则清空重建队列
                    ManualRegLogList = new List<ManualRegLogInfo>();
            }
            else
                ManualRegLogList = new List<ManualRegLogInfo>();

            Discuz.Toolkit.User userInfo = ds.GetUserInfo(me.UId);

            AsyncSiteInfo asyncSiteInfo = AsyncSites.GetAsyncSiteInfo(asyncSiteId);
            DiscuzSession targetDs = DiscuzSessionHelper.GetAsyncSiteSession(asyncSiteInfo);

            int result = -1;
            try
            {
                result = targetDs.Register(userInfo.UserName, userInfo.Password, userInfo.Email, true);
                if (result > 0)//如果成功注册，则将相关信息加入到已成功列表当中
                {
                    ManualRegLogList.Add(new ManualRegLogInfo(asyncSiteId, me.UId));
                }
            }
            catch (DiscuzException discuzException)
            {
                if (discuzException.ErrorCode == 110)//如果返回信息提示用户已经存在，则表示该用户已经同步过
                {
                    ManualRegLogList.Add(new ManualRegLogInfo(asyncSiteId, me.UId));
                }
            }
            return result;
        }

        public static int AsyncRegister(int uId)
        {
            DiscuzSession ds = DiscuzSessionHelper.GetMainSiteSession();

            User userInfo = ds.GetUserInfo(uId);

            List<AsyncSiteInfo> asyncSiteList = AsyncSites.GetAsyncSiteList();

            int successCount = 0;

            foreach (AsyncSiteInfo info in asyncSiteList)
            {
                if (!new ProcessRegister(userInfo, info).Enqueue())
                    successCount++;
            }
            return successCount;
        }

        public static int AsyncUserPassword(string userName, string password)
        {
            DiscuzSession ds = DiscuzSessionHelper.GetMainSiteSession();

            User userInfo = new User();
            userInfo.UserName = userName;

            UserForEditing userForEditing = new UserForEditing();
            userForEditing.Password = password;

            List<AsyncSiteInfo> asyncSiteList = AsyncSites.GetAsyncSiteList();

            int successCount = 0;

            foreach (AsyncSiteInfo info in asyncSiteList)
            {
                if (!new ProcessUpdateUser(userInfo, info, userForEditing).Enqueue())
                    successCount++;
            }
            return successCount;
        }

        public static int AsyncUserProfile(int uId)
        {
            DiscuzSession ds = DiscuzSessionHelper.GetMainSiteSession();

            User userInfo = ds.GetUserInfo(uId);

            UserForEditing userForEditing = new UserForEditing();
            userForEditing.Bio = userInfo.Bio;
            userForEditing.Birthday = userInfo.Birthday;
            userForEditing.Email = userInfo.Email;
            userForEditing.Gender = userInfo.Gender.ToString();
            userForEditing.Icq = userInfo.Icq;
            userForEditing.IdCard = userInfo.IdCard;
            userForEditing.Location = userInfo.Location;
            userForEditing.Mobile = userInfo.Mobile;
            userForEditing.Msn = userInfo.Msn;
            userForEditing.NickName = userInfo.NickName;
            userForEditing.Phone = userInfo.Phone;
            userForEditing.Qq = userInfo.Qq;
            userForEditing.RealName = userInfo.RealName;
            userForEditing.Skype = userInfo.Skype;
            userForEditing.WebSite = userInfo.WebSite;
            userForEditing.Yahoo = userInfo.Yahoo;

            List<AsyncSiteInfo> asyncSiteList = AsyncSites.GetAsyncSiteList();

            int successCount = 0;

            foreach (AsyncSiteInfo info in asyncSiteList)
            {
                if (!new ProcessUpdateUser(userInfo, info, userForEditing).Enqueue())
                    successCount++;
            }
            return successCount;
        }



        private static void RedirectToGetAuthToken()
        {
            string nextString = "";

            HttpRequest request = HttpContext.Current.Request;

            foreach (string key in request.QueryString.AllKeys)
            {
                nextString += string.Format("{0}:{1}-", key, request.QueryString[key]);
            }
            foreach (string key in request.Form.AllKeys)
            {
                nextString += string.Format("{0}:{1}-", key, request.QueryString[key]);
            }
            HttpContext.Current.Response.Redirect("sessioncreator.aspx?next=" + nextString.TrimEnd('-'));
        }
    }
}
