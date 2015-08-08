using System;
using System.Web;
using GF.Core;

namespace GF.Logic.Web
{
    /// <summary>
    /// Cookie 会话基类
    /// </summary>
    public class _WebCookie
    {
        /// <summary>
        /// 取得用户Cookie，数组0为用户昵称，1为邮件地址，2为个人网站地址
        /// </summary>
        public string[] Get()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[SiteCfg.Token + "_User"];
            string[] strArray = new string[] { string.Empty, string.Empty, string.Empty };
            if (cookie != null)
            {
                strArray[0] = SiteFun.UrlDecode(cookie.Values.Get("usrName"));
                strArray[1] = SiteFun.UrlDecode(cookie.Values.Get("usrMail"));
                strArray[1] = SiteFun.UrlDecode(cookie.Values.Get("usrSite"));
            }
            return strArray;
        }

        /// <summary>
        /// 设置用户Cookie
        /// </summary>
        /// <param name="name">用户昵称</param>
        /// <param name="mail">邮件地址</param>
        /// <param name="url">个人网站地址</param>
        public void Set(string name, string mail, string url)
        {
            HttpCookie cookie = new HttpCookie(SiteCfg.Token + "_User");
            cookie.Path = SiteCfg.Path;
            //保存一年
            cookie.Expires = DateTime.Now.AddYears(1);
            cookie.Values.Add("usrName", SiteFun.UrlEncode(name));
            cookie.Values.Add("usrMail", SiteFun.UrlEncode(mail));
            cookie.Values.Add("usrSite", SiteFun.UrlEncode(url));
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        /// <summary>
        /// 清除用户会话
        /// </summary>
        public void Clear()
        {
            //Set(string.Empty, string.Empty);
            Set(null, null, null);
        }
    }
}
