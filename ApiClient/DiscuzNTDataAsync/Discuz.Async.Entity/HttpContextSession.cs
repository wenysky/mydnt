using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Discuz.Async.Entity
{
    public class HttpContextSession
    {
        private string keyMame = "";

        public HttpContextSession(string key)
        {
            this.keyMame = key;
            if (HttpContext.Current.Session[key] == null)
            {
                HttpContext.Current.Session[key] = "";
            }
        }

        public object GetSession()
        {
            return HttpContext.Current.Session[keyMame];
        }

        public void SetSession(object obj)
        {
            HttpContext.Current.Session[keyMame] = obj;
        }
    }
}
