using System.Text;
using System.Web;
using GF.Core;
using GF.Data;
using GF.Data.Entity;
using GF.Logic.Admin;

namespace GF.Logic.Service
{
    /// <summary>
    /// 获取相关标签类
    /// </summary>
    public class ServiceTag
    {
        private _AdminCookie _ac = new _AdminCookie();
        private _DbHelper conn;

        public ServiceTag(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 获取匹配的关键字列表
        /// </summary>
        /// <returns>返回匹配成功的关键字列表</returns>
        public void GetMatchTags()
        {
            StringBuilder result = new StringBuilder();
            string[] usrInfo = _ac.Get();
            if (new UserData(conn).CheckUser(usrInfo[0], SiteFun.Encryption(usrInfo[1]), false).ID > 0)
            {
                TagList list = new TagData(conn).SelectTag(1, 200, "[PostCount]", "DESC");
                foreach (TagItem vItem in list)
                {
                    result.AppendFormat("<a rel=\"tags\">{0}</a>\r\n", vItem.Key);
                }
            }
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.Write(new _ServiceBaseXml().OutBaseXml(null, string.Format("\t\t<result>{0}</result>", SiteFun.CDATA(result.ToString()))));
        }
    }
}
