using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Discuz.EntLib.ToolKit
{
    /// <summary>
    /// 扩展方法类
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 输出操作信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string OutputOpInfo(this object obj)
        {
            return obj == null ? "" :
                string.Format("<fieldset style=\"width:600px;\"><legend>操作结果</legend>{0}</fieldset>", obj.ToString());
        }

        /// <summary>
        /// 添加操作信息
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="opInfo"></param>
        /// <returns></returns>
        public static object AppendOpInfo(this object obj, string opInfo)
        {
            return (obj == null ? "" : obj.ToString()) +  opInfo + "<br />" ;
        }
      
    }
}
