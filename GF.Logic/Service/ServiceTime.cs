using System;
using System.Web;

namespace GF.Logic.Service
{
    /// <summary>
    /// 获取时间日期类
    /// </summary>
    public class ServiceTime
    {
        /// <summary>
        /// 打印内容
        /// </summary>
        public void OutWrite()
        {
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.Write(new _ServiceBaseXml().OutBaseXml(null, string.Format("\t\t<result>{0}</result>", DateTime.Now)));
        }
    }
}
