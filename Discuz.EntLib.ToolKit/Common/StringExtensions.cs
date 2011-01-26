using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Discuz.EntLib.ToolKit
{
    public static class StringExtensions
    {
        public static string TrimWithElipsis(this string text, int length)
        {
            if (text.Length <= length) return text;
            return text.Substring(0, length) + "...";
        }

        /// <summary>
        /// replacement for String.Format
        /// </summary>
        public static string With(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static string HtmlEncode(this string str)
        {
            return Discuz.Common.Utils.HtmlEncode(str);
        }

        public static string HtmlDecode(this string str)
        {
            return Discuz.Common.Utils.HtmlDecode(str);
        }
    }
}
