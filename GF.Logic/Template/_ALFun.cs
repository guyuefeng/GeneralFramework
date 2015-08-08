using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GF.Core;
using GF.Data.Entity;
using System.Web;
using GF.Data;
using System.IO;

namespace GF.Logic.Template
{
    public class _ALFun
    {
        private _DbHelper conn;

        public _ALFun(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 取得查询数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>查询数据</returns>
        public string Query(string key)
        {
            return SiteFun.Query(key);
        }

        /// <summary>
        /// 取得提交数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>提交数据</returns>
        public string Post(string key)
        {
            return SiteFun.Post(key);
        }

        /// <summary>
        /// 强制转换为数字
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>数字</returns>
        public int ToInt(string value)
        {
            return SiteFun.ToInt(value);
        }

        /// <summary>
        /// 日期时间格式化
        /// </summary>
        /// <param name="dt">时间字符串</param>
        /// <param name="format">格式化字符串</param>
        /// <returns>格式化后的日期</returns>
        public string DateTimeFormat(string dt, string format)
        {
            return SiteFun.ToDate(dt).ToString(format);
        }

        /// <summary>
        /// 把标签列表加上链接
        /// </summary>
        /// <param name="tags">标签列表</param>
        /// <param name="cutstr">分隔字符</param>
        /// <returns>返回已加链接的标签列表代码</returns>
        public string TagsLink(string tags, string cutstr)
        {
            if (string.IsNullOrEmpty(cutstr)) { cutstr = ", "; }
            StringBuilder sr = new StringBuilder();
            if (!string.IsNullOrEmpty(tags))
            {
                foreach (string vItem in tags.Split(','))
                {
                    sr.AppendFormat("<a href=\"{0}\">{1}</a>{2}", string.Format(SitePath.TagLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(vItem)), vItem, cutstr);
                }
            }
            if (sr.Length > cutstr.Length) { sr = sr.Remove(sr.Length - cutstr.Length, cutstr.Length); }
            return sr.ToString();
        }

        /// <summary>
        /// 把标签列表加上链接
        /// </summary>
        /// <param name="tags">标签列表</param>
        /// <returns>返回已加链接的标签列表代码</returns>
        public string TagsLink(string tags)
        {
            return TagsLink(tags, null);
        }

        /// <summary>
        /// 取得Gravatar的MD5
        /// </summary>
        /// <param name="email">邮件地址</param>
        /// <returns>返回Gravatar用MD5值</returns>
        public string GravatarID(string email)
        {
            return SiteFun.GravatarID(email);
        }

        /// <summary>
        /// 处理原本格式
        /// </summary>
        /// <param name="text">原始文本</param>
        /// <returns>返回处理后的代码</returns>
        public string Pre(string text)
        {
            return SiteFun.Pre(text);
        }

        /// <summary>
        /// 字符串截断
        /// </summary>
        /// <param name="txt">文本</param>
        /// <param name="len">长度</param>
        /// <returns>返回被截断的字符串</returns>
        public string StrCut(string txt, int len)
        {
            return SiteFun.StrCut(txt, len);
        }

        /// <summary>
        /// 清除HTML标签
        /// </summary>
        /// <param name="html">HTML代码</param>
        /// <returns>纯文本</returns>
        public string ClearHtml(string html)
        {
            return SiteFun.ClearHtml(html);
        }

        /// <summary>
        /// 取得用户会话
        /// </summary>
        /// <param name="key">关键字，包含usrName、usrMail和usrSite</param>
        /// <returns>返回字符串</returns>
        public string GetUserCookie(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[SiteCfg.Token + "_User"];
            if (cookie != null) { return SiteFun.UrlDecode(cookie.Values.Get(key)); }
            else { return string.Empty; }
        }

        /// <summary>
        /// 获取上一篇文章
        /// </summary>
        /// <param name="artId">当前文章编号</param>
        /// <param name="len">标题显示长度</param>
        /// <param name="text">替换文本</param>
        /// <returns>返回上一篇文章的文字链</returns>
        public string GetPrevArticle(int artId, int len, string text)
        {
            PostItem art = new PostData(conn).GetClosePost(artId, false, "A");
            if (art.ID > 0)
            {
                return string.Format("<a href=\"{0}\">{1}</a>", string.Format(SitePath.ArticleLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(art.Local)), (string.IsNullOrEmpty(text)) ? SiteFun.HtmlEncode(SiteFun.StrCut(art.Title, len)) : text);
            }
            else
            {
                return string.IsNullOrEmpty(text) ? "没有了" : text;
            }
        }

        /// <summary>
        /// 获取下一篇文章
        /// </summary>
        /// <param name="artId">当前文章编号</param>
        /// <param name="len">标题显示长度</param>
        /// <param name="text">替换文本</param>
        /// <returns>返回下一篇文章的文字链</returns>
        public string GetNextArticle(int artId, int len, string text)
        {
            PostItem art = new PostData(conn).GetClosePost(artId, true, "A");
            if (art.ID > 0)
            {
                return string.Format("<a href=\"{0}\">{1}</a>", string.Format(SitePath.ArticleLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(art.Local)), (string.IsNullOrEmpty(text)) ? SiteFun.HtmlEncode(SiteFun.StrCut(art.Title, len)) : text);
            }
            else
            {
                return string.IsNullOrEmpty(text) ? "没有了" : text;
            }
        }

        /// <summary>
        /// 获取相关文章列
        /// </summary>
        /// <param name="outId">例外编号</param>
        /// <param name="limit">数据条数</param>
        /// <param name="length">标题长度</param>
        /// <param name="tags">纯文本标签列表</param>
        /// <returns>返回相关文章列表</returns>
        public string RelatedArticle(int outId, int limit, int length, string tags)
        {
            DataList<PostItem> list = new PostData(conn).SelectRelatedPost(outId, limit, tags, "A", false);
            string html = string.Empty;
            foreach (PostItem vItem in list)
            {
                string title = length > 0 ? SiteFun.StrCut(vItem.Title, length) : vItem.Title;
                html += string.Format("<li><a href=\"{0}\" title=\"{1}\">{2}</a><span>{3}</span></li>\n", string.Format(SitePath.ArticleLinkFormat, SiteCfg.Path, vItem.Local), SiteFun.HtmlEncode(vItem.Title), title, vItem.Publish);
            }
            return html;
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="cid">分类编号</param>
        /// <param name="limit">数据条数</param>
        /// <param name="length">标题长度</param>
        /// <param name="orderMode">0-时间，1-随机，2-阅读数，3-投票</param>
        /// <returns>返回获取的文章列表</returns>
        public string GetArticleList(int cid, int limit, int length, int orderMode)
        {
            DataList<PostItem> list = new PostData(conn).SelectPost(cid, null, null, 1, limit, orderMode, "A", false);
            string html = string.Empty;
            foreach (PostItem vItem in list)
            {
                string title = length > 0 ? SiteFun.StrCut(vItem.Title, length) : vItem.Title;
                html += string.Format("<li><a href=\"{0}\" title=\"{1}\">{2}</a><span>{3}</span></li>\n", string.Format(SitePath.ArticleLinkFormat, SiteCfg.Path, vItem.Local), SiteFun.HtmlEncode(vItem.Title), title, vItem.Publish);
            }
            return html;
        }

        /// <summary>
        /// 获取缩略图
        /// </summary>
        /// <param name="postId">资料编号</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="limit">多少个图片</param>
        /// <param name="pf">后缀名，例如：“.png”</param>
        /// <returns>标准图片代码</returns>
        public string GetPostImage(int postId, int width, int height, int limit, string pf)
        {
            string defaPic = SiteCfg.Path + "Common/Images/Nopic.png";
            if (limit < 1) { limit = 1; }
            string imgFormat = "<img src=\"{0}\" width=\"{1}\" height=\"{2}\" border=\"0\"/>";
            string result = string.Empty;
            PostItem item = new PostItem();
            if (SiteDat.GetDat(string.Format(SiteCache.PostFormat, postId)) == null)
            {
                SiteDat.SetDat(string.Format(SiteCache.PostFormat, postId), new PostData(conn).GetPost(postId));
            }
            item = (PostItem)SiteDat.GetDat(string.Format(SiteCache.PostFormat, postId));
            DataList<AttachmentItem> atts = new AttachmentData(conn).SelectAttachment(item.Attachments);
            if (atts.Count > 0)
            {
                int count = 0;
                foreach (AttachmentItem att in atts)
                {
                    FileInfo fInfo = new FileInfo(Path.Combine(SiteCfg.Router, att.Path));
                    if (fInfo.Exists && !string.IsNullOrEmpty(fInfo.Extension))
                    {
                        if (string.IsNullOrEmpty(pf))
                        {
                            if (fInfo.Extension.ToUpper() == ".BMP" || fInfo.Extension.ToUpper() == ".GIF" || fInfo.Extension.ToUpper() == ".JPG" || fInfo.Extension.ToUpper() == ".PNG")
                            {
                                count++;
                                result += string.Format(imgFormat, SiteCfg.Path + att.Path, width, height);
                            }
                        }
                        else
                        {
                            if (fInfo.Extension.ToUpper() == pf.ToUpper())
                            {
                                count++;
                                result += string.Format(imgFormat, SiteCfg.Path + att.Path, width, height);
                            }
                        }
                    }
                    if (count >= limit) { break; }
                }
            }
            if (string.IsNullOrEmpty(result)) { result = string.Format(imgFormat, defaPic, width, height); }
            return result;
        }

        /// <summary>
        /// 获取缩略图
        /// </summary>
        /// <param name="postId">资料编号</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="limit">多少个图片</param>
        /// <returns>标准图片代码</returns>
        public string GetPostImage(int postId, int width, int height, int limit)
        {
            return GetPostImage(postId, width, height, limit, null);
        }

        /// <summary>
        /// 获取缩略图
        /// </summary>
        /// <param name="postId">资料编号</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>标准图片代码</returns>
        public string GetPostImage(int postId, int width, int height)
        {
            return GetPostImage(postId, width, height, 1, null);
        }

        /// <summary>
        /// 获取当前数据表的数据数量
        /// </summary>
        /// <param name="tbName">表名</param>
        /// <returns></returns>
        public int GetTableCount(string tbName)
        {
            return ToInt(new DataBase().ExeSqlScalar(string.Format("SELECT COUNT(*) FROM [{0}]", tbName)).ToString());
        }
    }
}
