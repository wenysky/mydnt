using System;
using System.Text;
using System.Web;

using Discuz.Async.Entity;
using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Toolkit;

namespace Discuz.Async.Control
{
    public class AsyncSites
    {
        private static object lockHelper = new object();

        private static List<AsyncSiteInfo> asyncSiteList;

        static AsyncSites()
        {
            if (asyncSiteList == null)
            {
                asyncSiteList = Deserialize(HttpContext.Current.Server.MapPath("config/asyncsite.config"));
            }
        }


        /// <summary>
        /// 序列化配置信息为XML
        /// </summary>
        /// <param name="configinfo">配置信息</param>
        /// <param name="configFilePath">配置文件完整路径</param>
        public static List<AsyncSiteInfo> Serialiaze(List<AsyncSiteInfo> siteList, string configFilePath)
        {
            lock (lockHelper)
            {
                SerializationHelper.Save(siteList, configFilePath);
            }
            asyncSiteList = null;
            return siteList;
        }

        public static List<AsyncSiteInfo> Deserialize(string configFilePath)
        {
            return (List<AsyncSiteInfo>)SerializationHelper.Load(typeof(List<AsyncSiteInfo>), configFilePath);
        }

        public static AsyncSiteInfo GetAsyncSiteInfo(string apikey)
        {
            Predicate<AsyncSiteInfo> matchParent = new Predicate<AsyncSiteInfo>(delegate(AsyncSiteInfo asyncSiteInfo)
            {
                return asyncSiteInfo.Apikey == apikey;
            });
            AsyncSiteInfo siteInfo = null;
            if (asyncSiteList != null)
            {
                siteInfo = asyncSiteList.Find(matchParent);
            }
            return siteInfo;
        }

        public static List<AsyncSiteInfo> GetAsyncSiteList()
        {
            return asyncSiteList;
        }
    }

    public class ProcessUpdateUser
    {
        protected AsyncSiteInfo _siteInfo;

        protected User _userInfo;

        protected UserForEditing _userForEditing;

        protected bool _error = false;

        public ProcessUpdateUser(User userInfo, AsyncSiteInfo asyncSiteInfo, UserForEditing userForEditing)
        {
            this._userInfo = userInfo;
            this._siteInfo = asyncSiteInfo;
            this._userForEditing = userForEditing;
        }

        public bool Enqueue()
        {
            ManagedThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(Process));
            return _error;
        }

        private void Process(object state)
        {
            try
            {
                DiscuzSession targetDs = DiscuzSessionHelper.GetAsyncSiteSession(_siteInfo);

                int userId = targetDs.GetUserID(_userInfo.UserName);
                _error = targetDs.SetUserInfo(userId, _userForEditing);
            }
            catch
            {
                _error = true;
                //do something......
            }
        }
    }

    public class ProcessRegister
    {
        protected AsyncSiteInfo _siteInfo;

        protected User _userInfo;

        protected bool _error = false;

        public ProcessRegister(User userInfo, AsyncSiteInfo asyncSiteInfo)
        {
            this._siteInfo = asyncSiteInfo;
            this._userInfo = userInfo;
        }

        public bool Enqueue()
        {
            ManagedThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(Process));
            return _error;
        }

        private void Process(object state)
        {
            try
            {
                DiscuzSession targetDs = DiscuzSessionHelper.GetAsyncSiteSession(_siteInfo);
                _error = targetDs.Register(_userInfo.UserName, _userInfo.Password, _userInfo.Email, true) <= 0;
            }
            catch
            {
                _error = true;
                //do something......
            }
        }
    }
}
