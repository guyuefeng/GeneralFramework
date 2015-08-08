using System.Text;
using GF.Core;
using GF.Data;
using GF.Data.Entity;

namespace GF.Logic.Web
{
    /// <summary>
    /// 文章展示类
    /// </summary>
    public class WebArticle
    {
        private SettingItem _setting;
        private string _theme;
        private _DbHelper conn;

        public WebArticle(_DbHelper c)
        {
            conn = c;
            SettingData setData = new SettingData(conn);
            this._setting = setData.GetSetting();
            this._theme = setData.GetTheme;
        }

        /// <summary>
        /// 处理密码文章的显示
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <param name="content">内容</param>
        /// <param name="local">文章永久文件名</param>
        /// <param name="show">是否忽略密码直接显示内容</param>
        /// <returns>返回密码输入控件或是内容</returns>
        private string CheckPasswordContent(string pwd, string content, string local, bool show)
        {
            string result = content;
            if (!show && pwd.Length > 0)
            {
                result = string.Format("<div class=\"sysArticlePasswordCheckBox\"><form method=\"post\" action=\"{0}\"><input type=\"password\" name=\"pwd\"/><button type=\"submit\">{1}</button></form></div>", string.Format(SitePath.ArticleLinkFormat, SiteCfg.Path, local), SiteDat.GetLan("BtnChkPwd"));
            }
            return result;
        }

        /// <summary>
        /// 获取网站配置
        /// </summary>
        public SettingItem OutSetting
        {
            get
            {
                return this._setting;
            }
        }

        /// <summary>
        /// 获取网站主题
        /// </summary>
        public string OutTheme
        {
            get
            {
                return this._theme;
            }
        }

        /// <summary>
        ///  获取文章列表页完整XML内容
        /// </summary>
        /// <param name="cateId">分类编号</param>
        /// <param name="cateLocal">分类标签名</param>
        /// <param name="tag">搜索标签名</param>
        /// <param name="key">搜索关键字</param>
        /// <param name="page">当前页</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>完整XML内容</returns>
        public string OutWriteList(int cateId, string cateLocal, string tag, string key, int page, int pageSize)
        {
            SettingItem setting = this.OutSetting;
            ColumnItem cateItem = new ColumnItem();
            if (cateId > 0) { cateItem = new ColumnData(conn).GetColumn(cateId); }
            else { cateItem = new ColumnData(conn).GetColumn(cateLocal); }
            string listCacheName = string.Format(SiteCache.PostsListFormat, page, cateItem.ID, tag, key);
            if (SiteDat.GetDat(listCacheName) == null)
            {
                SiteDat.SetDat(listCacheName, new PostData(conn).SelectPost(cateItem.ID, tag, key, page, pageSize, 0, "A", false));
            }
            DataList<PostItem> artList = (DataList<PostItem>)SiteDat.GetDat(listCacheName);
            //本页XML处理
            StringBuilder xml = new StringBuilder();
            xml.AppendFormat("\t\t<tagName>{0}</tagName>\n", SiteFun.CDATA(tag));
            xml.AppendFormat("\t\t<searchKey>{0}</searchKey>\n", SiteFun.CDATA(key));
            xml.AppendFormat("\t\t<categoryName>{0}</categoryName>\n", SiteFun.CDATA(cateItem.Name));
            xml.Append("\t\t<articles>\n");
            //取得文章列表数据
            string pagerLink = string.Format(SitePath.PagerFormat, SiteCfg.Path, cateItem.ID, SiteFun.UrlEncode(tag), SiteFun.UrlEncode(key), "{0}");
            xml.AppendFormat("\t\t\t<pages>{0}</pages>\n", SiteFun.CDATA(new SitePages().Make(artList.Number, page, pageSize, pagerLink)));
            foreach (PostItem vItem in artList)
            {
                ColumnItem columnItem = new ColumnItem();
                if (SiteDat.GetDat(string.Format(SiteCache.ColumnFormat, vItem.ColumnID)) == null)
                {
                    SiteDat.SetDat(string.Format(SiteCache.ColumnFormat, vItem.ColumnID), new ColumnData(conn).GetColumn(vItem.ColumnID));
                }
                columnItem = (ColumnItem)SiteDat.GetDat(string.Format(SiteCache.ColumnFormat, vItem.ColumnID));
                xml.Append("\t\t\t<item>\n");
                xml.AppendFormat("\t\t\t\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.ArticleLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(vItem.Local))));
                xml.AppendFormat("\t\t\t\t<id>{0}</id>\n", vItem.ID);
                xml.AppendFormat("\t\t\t\t<title>{0}</title>\n", SiteFun.CDATA(vItem.Title));
                xml.AppendFormat("\t\t\t\t<local>{0}</local>\n", SiteFun.CDATA(vItem.Local));
                xml.AppendFormat("\t\t\t\t<tags>{0}</tags>\n", SiteFun.CDATA(vItem.Tags));
                xml.AppendFormat("\t\t\t\t<explain>{0}</explain>\n", SiteFun.CDATA(CheckPasswordContent(vItem.Password, vItem.Explain, vItem.Local, false)));
                xml.AppendFormat("\t\t\t\t<content>{0}</content>\n", SiteFun.CDATA(CheckPasswordContent(vItem.Password, vItem.Content, vItem.Local, false)));
                xml.AppendFormat("\t\t\t\t<publish>{0}</publish>\n", vItem.Publish);
                xml.AppendFormat("\t\t\t\t<author>{0}</author>\n", SiteFun.CDATA(vItem.Author));
                xml.AppendFormat("\t\t\t\t<postCount>{0}</postCount>\n", vItem.PostCount);
                xml.AppendFormat("\t\t\t\t<reader>{0}</reader>\n", vItem.Reader);
                xml.AppendFormat("\t\t\t\t<vote>{0}</vote>\n", vItem.Vote);
                xml.AppendFormat("\t\t\t\t<fine>{0}</fine>\n", vItem.Fine);
                xml.Append("\t\t\t\t<category>\n");
                xml.AppendFormat("\t\t\t\t\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.CategoryLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(columnItem.Local))));
                xml.AppendFormat("\t\t\t\t\t<id>{0}</id>\n", columnItem.ID);
                xml.AppendFormat("\t\t\t\t\t<local>{0}</local>\n", SiteFun.CDATA(columnItem.Local));
                xml.AppendFormat("\t\t\t\t\t<name>{0}</name>\n", SiteFun.CDATA(columnItem.Name));
                xml.AppendFormat("\t\t\t\t\t<intro>{0}</intro>\n", SiteFun.CDATA(columnItem.Intro));
                xml.AppendFormat("\t\t\t\t\t<postCount>{0}</postCount>\n", columnItem.PostCount);
                xml.Append("\t\t\t\t</category>\n");
                xml.Append("\t\t\t</item>\n");
            }
            xml.Append("\t\t</articles>\n");
            //绑定XML并写出
            string title = string.Format("{0} - {1}", setting.Basic.Name, setting.Basic.Intro);
            if (cateItem.ID > 0) { title = string.Format("{0}: {1} - {2}", SiteDat.GetLan("Category"), cateItem.Name, setting.Basic.Name); }
            if (!string.IsNullOrEmpty(tag)) { title = string.Format("{0}: {1} - {2}", SiteDat.GetLan("Tag"), tag, setting.Basic.Name); }
            if (!string.IsNullOrEmpty(key)) { title = string.Format("{0}: {1} - {2}", SiteDat.GetLan("Search"), key, setting.Basic.Name); }
            return new _WebBaseXml(conn).OutBaseXml(title, xml.ToString());
        }

        /// <summary>
        /// 获取文章内容页完整XML内容
        /// </summary>
        /// <param name="id">文章编号</param>
        /// <param name="local">文章标签</param>
        /// <param name="pwd">文章密码</param>
        /// <param name="page">当前页（评论用）</param>
        /// <param name="pageSize">分页大小（评论用）</param>
        /// <returns>完整XML内容</returns>
        public string OutWriteView(int id, string local, string pwd, int page, int pageSize)
        {
            SettingItem setting = this.OutSetting;
            PostData artData = new PostData(conn);
            SiteDat SiteDat = new SiteDat();
            //取得文章列表数据
            PostItem art = new PostItem();
            if (id > 0)
            {
                if (SiteDat.GetDat(string.Format(SiteCache.PostFormat, id)) == null)
                {
                    SiteDat.SetDat(string.Format(SiteCache.PostFormat, id), artData.GetPost(id));
                }
                art = (PostItem)SiteDat.GetDat(string.Format(SiteCache.PostFormat, id));
            }
            else
            {
                if (SiteDat.GetDat(string.Format(SiteCache.PostFormat, local)) == null)
                {
                    SiteDat.SetDat(string.Format(SiteCache.PostFormat, local), artData.GetPost(local));
                }
                art = (PostItem)SiteDat.GetDat(string.Format(SiteCache.PostFormat, local));
            }
            //取得栏目数据
            ColumnItem columnItem = new ColumnItem();
            if (SiteDat.GetDat(string.Format(SiteCache.ColumnFormat, art.ColumnID)) == null)
            {
                SiteDat.SetDat(string.Format(SiteCache.ColumnFormat, art.ColumnID), new ColumnData(conn).GetColumn(art.ColumnID));
            }
            columnItem = (ColumnItem)SiteDat.GetDat(string.Format(SiteCache.ColumnFormat, art.ColumnID));

            artData.AddPostReader(art.ID);
            //设置模板引擎
            //设置密码文章显示状态
            bool viewPwdArt = pwd == art.Password;
            //本页XML处理
            StringBuilder xml = new StringBuilder();
            xml.Append("\t\t<article>\n");
            xml.AppendFormat("\t\t\t\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.ArticleLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(art.Local))));
            xml.AppendFormat("\t\t\t\t<id>{0}</id>\n", art.ID);
            xml.AppendFormat("\t\t\t\t<title>{0}</title>\n", SiteFun.CDATA(art.Title));
            xml.AppendFormat("\t\t\t\t<local>{0}</local>\n", SiteFun.CDATA(art.Local));
            xml.AppendFormat("\t\t\t\t<tags>{0}</tags>\n", SiteFun.CDATA(art.Tags));
            xml.Append("\t\t\t\t<category>\n");
            xml.AppendFormat("\t\t\t\t\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.CategoryLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(columnItem.Local))));
            xml.AppendFormat("\t\t\t\t\t<id>{0}</id>\n", columnItem.ID);
            xml.AppendFormat("\t\t\t\t\t<local>{0}</local>\n", SiteFun.CDATA(columnItem.Local));
            xml.AppendFormat("\t\t\t\t\t<name>{0}</name>\n", SiteFun.CDATA(columnItem.Name));
            xml.AppendFormat("\t\t\t\t\t<intro>{0}</intro>\n", SiteFun.CDATA(columnItem.Intro));
            xml.AppendFormat("\t\t\t\t\t<postCount>{0}</postCount>\n", columnItem.PostCount);
            xml.Append("\t\t\t\t</category>\n");
            xml.AppendFormat("\t\t\t\t<explain>{0}</explain>\n", SiteFun.CDATA(CheckPasswordContent(art.Password, art.Explain, art.Local, viewPwdArt)));
            xml.AppendFormat("\t\t\t\t<content>{0}</content>\n", SiteFun.CDATA(CheckPasswordContent(art.Password, art.Content, art.Local, viewPwdArt)));
            xml.AppendFormat("\t\t\t\t<publish>{0}</publish>\n", art.Publish);
            xml.AppendFormat("\t\t\t\t<author>{0}</author>\n", SiteFun.CDATA(art.Author));
            xml.AppendFormat("\t\t\t\t<postCount>{0}</postCount>\n", art.PostCount);
            xml.AppendFormat("\t\t\t\t<reader>{0}</reader>\n", art.Reader);
            xml.AppendFormat("\t\t\t\t<vote>{0}</vote>\n", art.Vote);
            xml.Append("\t\t</article>\n");
            xml.Append("\t\t<comments>\n");
            xml.AppendFormat("\t\t\t<html>{0}</html>\n", SiteFun.CDATA(new CommentUI(conn).GetCommentList(art.ID, page, pageSize)));
            xml.Append("\t\t</comments>\n");
            //绑定XML并写出
            string title = string.Empty;
            if (art.ID > 0) { title = string.Format("{0} - {1} - {2}", art.Title, columnItem.Name, setting.Basic.Name); }
            return new _WebBaseXml(conn).OutBaseXml(title, xml.ToString());
        }
    }
}
