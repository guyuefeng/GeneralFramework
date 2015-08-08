using System.Web;
using GF.Core;
using GF.Data;
using GF.Data.Entity;
using GF.Logic.Web;

namespace GF.Logic.Service
{
    /// <summary>
    /// 评论处理类
    /// </summary>
    public class ServiceComment
    {
        private _WebCookie _wc = new _WebCookie();
        private _DbHelper conn;

        public ServiceComment(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 向浏览器打印代码
        /// </summary>
        public void OutWrite()
        {
            switch (SiteFun.Query("mode"))
            {
                case "post": { OutWritePost(); break; }
                default: { OutWriteList(); break; }
            }
        }

        /// <summary>
        /// 发表评论过程
        /// </summary>
        private void OutWritePost()
        {
            string msg = string.Empty;
            SettingItem setting = new SettingData(conn).GetSetting();
            //取值和设置
            PostItem postItem = new PostData(conn).GetPost(SiteFun.ToInt(SiteFun.Query("artId")));
            CommentItem postVal = new CommentItem();
            postVal.PostID = postItem.ID;
            postVal.Author = SiteFun.Post("author");
            postVal.Mail = SiteFun.Post("mail");
            postVal.URL = SiteFun.FormatUrl(SiteFun.Post("url"));
            postVal.Content = SiteFun.Post("content");
            postVal.Verify = postItem.AutoVerifyComment;
            //设置Cookie
            _wc.Set(postVal.Author, postVal.Mail, postVal.URL);
            //开始判断并发布
            if (postItem.ID > 0 && postItem.SwitchComment)
            {
                if (string.IsNullOrEmpty(postVal.Author)) { msg = SiteDat.GetLan("MsgNoNickName"); }
                else
                {
                    if (string.IsNullOrEmpty(postVal.Mail)) { msg = SiteDat.GetLan("MsgNoMail"); }
                    else
                    {
                        if (string.IsNullOrEmpty(postVal.Content)) { msg = SiteDat.GetLan("MsgNoContent"); }
                        else
                        {
                            if (postVal.Content.Length > 255) { msg = SiteDat.GetLan("MsgContLenMax"); }
                            else
                            {
                                bool haveFilter = false;
                                string[] filters = setting.Basic.Filter.Split(',');
                                foreach (string filter in filters)
                                {
                                    if (postVal.Content.Contains(filter)) { haveFilter = true; break; }
                                }
                                if (haveFilter) { msg = SiteDat.GetLan("MsgFilterTxt"); }
                                else
                                {
                                    new CommentData(conn).InsertComment(postVal);
                                    SiteDat.RemoveDat(SiteCache.NewComment);
                                    SiteDat.RemoveDat(SiteCache.RandomPost);
                                    SiteDat.ClearDat(string.Format("Comments-{0}-", postItem.ID));
                                }
                            }
                        }
                    }
                }
            }
            else { msg = SiteDat.GetLan("MsgArtNotExistsOrLocked"); }
            PrintList(msg, postItem.ID);
        }

        /// <summary>
        /// 打印评论列表
        /// </summary>
        private void OutWriteList()
        {
            int artID = SiteFun.ToInt(SiteFun.Query("artId"));
            PrintList(null, artID);
        }

        /// <summary>
        /// 取得评论列表
        /// </summary>
        /// <param name="msg">返回消息</param>
        /// <param name="artID">文章编号</param>
        private void PrintList(string msg, int artID)
        {
            //初始化引擎
            SettingItem setting = new SettingData(conn).GetSetting();
            int page = SiteFun.ToInt(SiteFun.Query("page"));
            int pageSize = setting.Parameter.CommentNum;
            string listHtml = new CommentUI(conn).GetCommentList(artID, page, pageSize);
            string xmlInner = string.Format("\t\t<html>{0}</html>", SiteFun.CDATA(listHtml));
            string xml = new _ServiceBaseXml().OutBaseXml(msg, xmlInner);
            //输出
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.Write(xml);
        }
    }
}
