using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSampleHelper;

namespace Discuz.Toolkit.WebSample
{
    public partial class asyncreceive : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DiscuzSession ds = DiscuzSessionHelper.GetSession();
            Dictionary<string, string> dic = ds.GetQueryString();
            if (dic.ContainsKey("action"))
            {
                AsyncHelper.WriteAsyncLog(dic["action"], GetActionParams(dic));
            }
        }

        private string GetActionParams(Dictionary<string, string> dic)
        {
            string result = "";
            foreach (KeyValuePair<string, string> kv in dic)
            {
                if (kv.Key == "action")
                    continue;
                result += kv.Key + "=" + kv.Value + ";";
            }
            return result;
        }
    }
}
