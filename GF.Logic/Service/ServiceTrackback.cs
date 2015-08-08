using System.Text;
using System.Web;
using GF.Core;
using GF.Data;
using GF.Data.Entity;

namespace GF.Logic.Service
{
    /// <summary>
    /// 引用通告接收类
    /// </summary>
    public class ServiceTrackback
    {
        private _DbHelper conn;

        public ServiceTrackback(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 写出引用通告处理状态
        /// </summary>
        public void OutWrite()
        {
            //取值
            PostItem postItem = new PostData(conn).GetPost(SiteFun.ToInt(SiteFun.Query("id")));
            CommentItem value = new CommentItem();
            value.PostID = postItem.ID;
            value.Author = SiteFun.Post("blog_name");
            value.Content = SiteFun.Post("excerpt");
            value.Title = SiteFun.Post("title");
            value.Trackback = true;
            value.URL = SiteFun.FormatUrl(SiteFun.Post("url"));
            value.Verify = postItem.AutoVerifyTrackback;
            //错误状态和返回信息
            int error = 1;
            string msg = string.Empty;
            if (postItem.ID > 0 && postItem.Show)
            {
                if (postItem.SwitchTrackback)
                {
                    if (string.IsNullOrEmpty(value.Content)) { msg = SiteDat.GetLan("MsgNoContent"); }
                    else
                    {
                        if (value.Content.Length > 255) { value.Content = value.Content.Substring(0, 252) + "..."; }
                        if (string.IsNullOrEmpty(value.Author)) { msg = SiteDat.GetLan("MsgNoName"); }
                        else
                        {
                            bool haveFilter = false;
                            string[] filters = new SettingData(conn).GetSetting().Basic.Filter.Split(',');
                            foreach (string filter in filters)
                            {
                                if (value.Content.Contains(filter)) { haveFilter = true; }
                            }
                            if (haveFilter) { msg = SiteDat.GetLan("MsgFilterTxt"); }
                            else
                            {
                                new CommentData(conn).InsertComment(value);
                                msg = SiteDat.GetLan("MsgSaveSucc");
                                error = 0;
                            }
                        }
                    }
                }
                else { msg = SiteDat.GetLan("MsgArtAllowedSendTb"); }
            }
            else { msg = SiteDat.GetLan("MsgArtNotExists"); }
            //处理和返回
            StringBuilder xml = new StringBuilder();
            xml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
            xml.Append("<response>\n");
            xml.AppendFormat("\t<error>0</error>\n", error);
            xml.AppendFormat("\t<message>{0}</message>\n", SiteFun.CDATA(msg));
            xml.Append("</response>");
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.Write(xml);
        }
    }
}
