using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Async.Entity
{
    [Serializable]
    public class AsyncServiceConfigInfo
    {
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string passport;

        public string Passport
        {
            get { return passport; }
            set { passport = value; }
        }
    }
}
