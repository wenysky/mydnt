using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Async.Entity
{
    public class ManualRegLogInfo
    {
        private string siteId = "";

        public string SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        private long uId = -1;

        public long UId
        {
            get { return uId; }
            set { uId = value; }
        }

        public ManualRegLogInfo(string _siteId,long _uId)
        {
            this.siteId = _siteId;
            this.uId = _uId;
        }
    }
}
