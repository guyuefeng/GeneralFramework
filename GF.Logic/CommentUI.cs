using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GF.Data.Entity;
using GF.Core;
using GF.Data;

namespace GF.Logic
{
    public class CommentUI
    {
        private _DbHelper conn;

        public CommentUI(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 获取评论区域的代码
        /// </summary>
        /// <param name="artID">上属编号</param>
        /// <param name="page">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>局部HTML代码</returns>
        public string GetCommentList(int artID, int page, int pageSize)
        {
            DataList<CommentItem> cmtList = new DataList<CommentItem>();
            if (SiteDat.GetDat(string.Format(SiteCache.CommentsListFormat, artID, page)) == null)
            {
                SiteDat.SetDat(string.Format(SiteCache.CommentsListFormat, artID, page), new CommentData(conn).SelectComment(artID, page, pageSize, false));
            }
            cmtList = (DataList<CommentItem>)SiteDat.GetDat(string.Format(SiteCache.CommentsListFormat, artID, page));
            StringBuilder result = new StringBuilder();
            foreach (CommentItem vItem in cmtList)
            {
                result.AppendFormat("<a name=\"comment_{0}\"></a>\n", vItem.ID);
                result.AppendLine("<dl class=\"commentItem\">");
                result.AppendLine("\t<dt>");
                result.AppendFormat("\t\t<div class=\"commentFace\"><img src=\"http://www.gravatar.com/avatar.php?gravatar_id={0}\"/></div>\n", SiteFun.GravatarID(vItem.Mail));
                result.AppendFormat("\t\t<div class=\"commentName\">{0}</div>\n", SiteFun.HtmlEncode(vItem.Author));
                result.AppendLine("\t</dt>");
                result.AppendLine("\t<dd>");
                result.AppendFormat("\t\t<div class=\"commentContent\">{0}</div>\n", SiteFun.Pre(vItem.Content));
                result.AppendLine("\t\t<div class=\"commentInfo\">");
                result.AppendFormat("\t\t\t<a href=\"{1}\" title=\"{1}\" class=\"commentWebsite\" target=\"_blank\" rel=\"nofollow\">{0}</a>\n", SiteDat.GetLan("Website"), SiteFun.HtmlEncode(vItem.URL));
                //result.AppendFormat("\t\t\t<a href=\"mailto:{1}\" title=\"{1}\" class=\"commentMail\" target=\"_blank\" rel=\"nofollow\">{0}</a>\n", SiteDat.GetLan("Mail"), SiteFun.HtmlEncode(vItem.Mail));
                //result.AppendFormat("\t\t\t<a href=\"{1}\" class=\"commentRe\">{0}</a>\n", SiteDat.GetLan("Reply"), "");
                result.AppendFormat("\t\t\t<span class=\"commentTime\">{0}</span>\n", vItem.Publish);
                result.AppendLine("\t\t</div>");
                if (!string.IsNullOrEmpty(vItem.Reply))
                {
                    result.AppendFormat("\t\t<div class=\"commentReply\">{0}</div>\n", vItem.Reply);
                }
                result.AppendLine("\t</dd>");
                result.AppendLine("</dl>");
            }
            result.AppendFormat("<div class=\"pager\">{0}</div>\n", new SitePages().Make(cmtList.Number, page, pageSize, "javascript:getComments(" + artID.ToString() + ", {0});"));
            return result.ToString();
        }
    }
}
