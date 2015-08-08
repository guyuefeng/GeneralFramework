using System;
using System.Web;
using GF.Core;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 管理员 Cookie 会话基类
    /// </summary>
    public class _AdminCookie
    {
        /// <summary>
        /// 取得管理员Cookie，数组0为帐号，1为密码明文
        /// </summary>
        public string[] Get()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[SiteCfg.Token + "_Admin"];
            string[] strArray = new string[] { string.Empty, string.Empty };
            if (cookie != null)
            {
                strArray[0] = SiteFun.UrlDecode(cookie.Values.Get("UserName"));
                strArray[1] = SiteFun.UrlDecode(cookie.Values.Get("Password"));
            }
            return strArray;
        }

        /// <summary>
        /// 设置管理员Cookie
        /// </summary>
        /// <param name="userName">帐号</param>
        /// <param name="password">密码明文</param>
        public void Set(string userName, string password)
        {
            HttpCookie cookie = new HttpCookie(SiteCfg.Token + "_Admin");
            cookie.Path = SiteCfg.Path;
            //保存一个月
            cookie.Expires = DateTime.Now.AddMonths(1);
            cookie.Values.Add("UserName", SiteFun.UrlEncode(userName));
            cookie.Values.Add("Password", SiteFun.UrlEncode(password));
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        /// <summary>
        /// 清除管理员会话
        /// </summary>
        public void Clear()
        {
            //Set(string.Empty, string.Empty);
            Set(null, null);
        }
    }
}
