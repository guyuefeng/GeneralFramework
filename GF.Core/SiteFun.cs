using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Collections;


namespace GF.Core
{
    /// <summary>
    /// 全局操作静态类
    /// </summary>
    public class SiteFun
    {
        /// <summary>
        /// 是否有POST的键值
        /// </summary>
        public static bool IsPost
        {
            get { return HttpContext.Current.Request.Form.Count > 0; }
        }

        /// <summary>
        /// 将明文加密为系统字符
        /// </summary>
        /// <param name="str">即将加密的明文字符</param>
        public static string Encryption(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
                str = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1");
            }
            return str;
        }

        /// <summary>
        /// 取得Gravatar的MD5
        /// </summary>
        /// <param name="email">邮件地址</param>
        /// <returns>返回Gravatar用MD5值</returns>
        public static string GravatarID(string email)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(email))
            {
                result = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(email, "MD5").ToLower();
            }
            return result;
        }


        /// <summary>
        /// 将字符串进行HTML格式化
        /// </summary>
        /// <param name="str">字符串</param>
        public static string HtmlEncode(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = HttpContext.Current.Server.HtmlEncode(str);
            }
            return str;
        }


        /// <summary>
        /// 格式化文件大小
        /// </summary>
        /// <param name="len">大小字节</param>
        /// <returns>返回大小文本串</returns>
        public static string FormatLength(long len)
        {
            if (len > 0x40000000) { return string.Format("{0} GB", ToInt(len / 0x40000000)); }
            if (len > 0x00100000) { return string.Format("{0} MB", ToInt(len / 0x00100000)); }
            if (len > 0x00000400) { return string.Format("{0} KB", ToInt(len / 0x00000400)); }
            else { return string.Format("{0} byte", ToInt(len)); }
        }


        /// <summary>
        /// 格式化地址
        /// </summary>
        /// <param name="url">原始地址</param>
        /// <returns>返回包含协议的地址</returns>
        public static string FormatUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                if (!Regex.IsMatch(url, @"^\w{3,6}:\/\/.*$", RegexOptions.IgnoreCase)) { url = "http://" + url; }
            }
            return url;
        }


        /// <summary>
        /// 将字符串进行XML格式化
        /// </summary>
        /// <param name="str">字符串</param>
        public static string CDATA(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("<![CDATA[", "&lt;![CDATA[").Replace("]]>", "]]&gt;");
            }
            return "<![CDATA[" + str + "]]>";
        }


        /// <summary>
        /// 清除CDATA标记
        /// </summary>
        /// <param name="str">要处理的字符</param>
        public static string ClearCDATA(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("<![CDATA[", string.Empty).Replace("]]>", string.Empty);
            }
            return str;
        }


        /// <summary>
        /// 清除HTML
        /// </summary>
        /// <param name="html">原始代码</param>
        /// <returns>返回纯文本</returns>
        public static string ClearHtml(string html)
        {
            if (!string.IsNullOrEmpty(html))
            {
                html = Regex.Replace(html, @"<[^>]*>", string.Empty);
            }
            return html;
        }

        /// <summary>
        /// URL解密
        /// </summary>
        /// <param name="strVal">解密字串</param>
        public static string UrlDecode(string strVal)
        {
            if (!string.IsNullOrEmpty(strVal))
            {
                strVal = HttpUtility.UrlDecode(strVal, Encoding.UTF8);
            }
            return strVal;
        }


        /// <summary>
        /// URL加密
        /// </summary>
        /// <param name="strVal">加密字串</param>
        public static string UrlEncode(string strVal)
        {
            if (!string.IsNullOrEmpty(strVal))
            {
                strVal = HttpUtility.UrlEncode(strVal, Encoding.UTF8);
            }
            return strVal;
        }

        /// <summary>
        /// 取得GET参数值
        /// </summary>
        /// <param name="key">关键字</param>
        public static string Query(string key)
        {
            string val = HttpContext.Current.Request.QueryString[key];
            string result = string.IsNullOrEmpty(val) ? string.Empty : val.ToString().Trim();
            return result;
        }


        /// <summary>
        /// 取得POST参数值
        /// </summary>
        /// <param name="key">关键字</param>
        public static string Post(string key)
        {
            string val = HttpContext.Current.Request.Form[key];
            string result = string.IsNullOrEmpty(val) ? string.Empty : val.ToString().Trim();
            return result;
        }


        /// <summary>
        /// 转换为数字（错误兼容）
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        public static int ToInt(object obj)
        {
            int result = int.TryParse(obj.ToString(), out result) ? result : 0;
            return result;
        }

        /// <summary>
        /// 转换为日期（错误兼容）
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        public static DateTime ToDate(object obj)
        {
            DateTime result = DateTime.TryParse(obj.ToString(), out result) ? result : DateTime.Now;
            return result;
        }

        /// <summary>
        /// 转换为布尔类型（错误兼容）
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        public static bool ToBool(object obj)
        {
            bool result = bool.TryParse(obj.ToString(), out result) ? result : false;
            return result;
        }

        /// <summary>
        /// 判断是否是合法的永久文件名
        /// </summary>
        /// <param name="local">文件名</param>
        /// <returns>返回真假型数据</returns>
        public static bool IsLocal(string local)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(local))
            {
                result = Regex.IsMatch(local, @"^[^\d]+[\w\-]*");
            }
            return result;
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="len">截取的长度</param>
        /// <returns>返回截取后的字符串</returns>
        public static string StrCut(string str, int len)
        {
            if (!string.IsNullOrEmpty(str) && len > 0)
            {
                int l = str.Length;
                int clen = 0;
                while (clen < len && clen < l)
                {
                    //每次遇到一个中文，则将该长度减一   
                    if ((int)str[clen] > 128) { len--; }
                    clen++;
                }
                if (clen < l && clen > 2) { str = str.Substring(0, clen - 2) + ".."; }
            }
            return str;
        }

        /// <summary>
        /// 处理原本格式并进行HTML编码
        /// </summary>
        /// <param name="text">原始文本</param>
        /// <returns>返回处理后的代码</returns>
        public static string Pre(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = HtmlEncode(text).Replace("\n", "<br/>");
            }
            return text;
        }

        /// <summary>
        /// 标签配对格式化
        /// </summary>
        /// <param name="html">要配对的内容</param>
        /// <returns>不缺失的HTML内容</returns>
        public static string HtmlMatch(string html)
        {
            string result = html;
            if (result.Length > 0)
            {
                MatchCollection matchs = Regex.Matches(html, @"<([^\s>\/]+)(|\s[^>]+)>", RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
                ArrayList tabs = new ArrayList();
                foreach (Match match in matchs)
                {
                    string tab = match.Groups[1].Value.ToLower();
                    if (tab.Length > 0 && !tabs.Contains(tab))
                    {
                        if (tab != "hr" && tab != "br" && tab != "img")
                        {
                            tabs.Add(tab);
                        }
                    }
                }
                //string[] tabs = { "P", "TABLE", "", "", "", "", "", "UL", "OL", "DIV", "H1", "H2", "H3", "H4", "H5", "H6" };
                foreach (string tab in tabs)
                {
                    int tabNum = Regex.Matches(html, string.Format(@"<{0}(|\s[^>]+)>", tab), RegexOptions.IgnoreCase).Count;
                    int endTabNum = Regex.Matches(html, string.Format(@"<\/{0}>", tab), RegexOptions.IgnoreCase).Count;
                    if (tabNum > 0)
                    {
                        int outNum = tabNum - endTabNum;
                        if (outNum > 0)
                        {
                            for (int i = 0; i < outNum; i++)
                            {
                                result += string.Format("</{0}>", tab);
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 取客户端IP
        /// </summary>
        public static string GetGuestIP
        {
            /*
             <div></d></div>
             */
            get
            {
                string sIP = string.Empty;
                try
                {
                    HttpRequest oHR = HttpContext.Current.Request;
                    if (oHR.ServerVariables["HTTP_VIA"] != null) { sIP = oHR.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim(); }
                    else { sIP = oHR.UserHostAddress; }
                }
                catch { }
                return sIP;
            }
        }

    }
}
