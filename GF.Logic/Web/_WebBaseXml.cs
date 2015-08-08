using System.Text;
using System.Web;
using GF.Core;
using GF.Data;
using GF.Data.Entity;

namespace GF.Logic.Web
{
    /// <summary>
    /// 前台数据提供基类
    /// </summary>
    public class _WebBaseXml
    {
        private _DbHelper conn;

        public _WebBaseXml(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 获取 XML 内容
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="thisXmlText">本页单独的 XML 内容</param>
        /// <returns>返回完整的 XML 内容</returns>
        public string OutBaseXml(string title, string thisXmlText)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
            xml.Append("<ui>\n");
            //配置数据
            xml.Append("\t<config>\n");
            xml.AppendFormat("\t\t<path>{0}</path>\n", SiteFun.CDATA(SiteCfg.Path));
            xml.AppendFormat("\t\t<system>{0}</system>\n", SiteFun.CDATA(SiteCfg.System));
            xml.AppendFormat("\t\t<version>{0}</version>\n", SiteFun.CDATA(SiteCfg.Version));
            xml.AppendFormat("\t\t<fullVersion>{0}</fullVersion>\n", SiteFun.CDATA(SiteCfg.SystemVersionFull));
            xml.AppendFormat("\t\t<powered>{0}</powered>\n", SiteFun.CDATA(SiteCfg.Powered));
            xml.Append("\t</config>\n");
            //设置数据
            SettingData setData = new SettingData(conn);
            if (SiteDat.GetDat(SiteCache.Setting) == null)
            {
                SiteDat.SetDat(SiteCache.Setting, setData.GetSetting());
            }
            SettingItem setting = (SettingItem)SiteDat.GetDat(SiteCache.Setting);
            string theme = setData.GetTheme;
            xml.Append("\t<setting>\n");
            xml.AppendFormat("\t\t<theme>{0}</theme>\n", SiteFun.CDATA(theme));
            xml.AppendFormat("\t\t<language>{0}</language>\n", SiteFun.CDATA(setting.Basic.Language));
            xml.AppendFormat("\t\t<name>{0}</name>\n", SiteFun.CDATA(setting.Basic.Name));
            xml.AppendFormat("\t\t<url>{0}</url>\n", SiteFun.CDATA(setting.Basic.URL));
            xml.AppendFormat("\t\t<intro>{0}</intro>\n", SiteFun.CDATA(setting.Basic.Intro));
            xml.AppendFormat("\t\t<keywords>{0}</keywords>\n", SiteFun.CDATA(setting.Basic.Keywords));
            xml.AppendFormat("\t\t<icp>{0}</icp>\n", SiteFun.CDATA(setting.Basic.ICP));
            xml.AppendFormat("\t\t<affiche>{0}</affiche>\n", SiteFun.CDATA(setting.Basic.Affiche));
            xml.Append("\t</setting>\n");
            //导航
            xml.Append("\t<navigations>\n");
            xml.Append("\t\t<item>\n");
            xml.AppendFormat("\t\t\t<link>{0}</link>\n", SiteFun.CDATA(SiteCfg.Path));
            xml.AppendFormat("\t\t\t<name>{0}</name>\n", SiteFun.CDATA(SiteDat.GetLan("HomePage")));
            xml.AppendFormat("\t\t\t<intro>{0}</intro>\n", SiteFun.CDATA(setting.Basic.Name));
            xml.AppendFormat("\t\t\t<target>{0}</target>\n", SiteFun.CDATA("_self"));
            xml.AppendFormat("\t\t\t<current>{0}</current>\n", string.IsNullOrEmpty(HttpContext.Current.Request.Url.Query));
            xml.Append("\t\t</item>\n");
            if (SiteDat.GetDat(SiteCache.Navigation) == null)
            {
                SiteDat.SetDat(SiteCache.Navigation, new ColumnData(conn).SelectColumn(1, -1, false));
            }
            foreach (ColumnItem vItem in (DataList<ColumnItem>)SiteDat.GetDat(SiteCache.Navigation))
            {
                string itemLink = string.Format(SitePath.CategoryLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(vItem.Local));
                xml.Append("\t\t<item>\n");
                xml.AppendFormat("\t\t\t<link>{0}</link>\n", SiteFun.CDATA(vItem.Jump ? vItem.JumpUrl : itemLink));
                xml.AppendFormat("\t\t\t<name>{0}</name>\n", SiteFun.CDATA(vItem.Name));
                xml.AppendFormat("\t\t\t<intro>{0}</intro>\n", SiteFun.CDATA(vItem.Intro));
                xml.AppendFormat("\t\t\t<target>{0}</target>\n", SiteFun.CDATA(vItem.Target));
                xml.AppendFormat("\t\t\t<current>{0}</current>\n", string.Format((SiteFun.ToInt(vItem.Local) > 0 ? "?act=defa&cid={0}" : "?act=defa&clocal={1}"), vItem.ID, SiteFun.UrlEncode(vItem.Local)).ToLower() == HttpContext.Current.Request.Url.Query.ToLower());
                xml.Append("\t\t</item>\n");
            }
            xml.Append("\t</navigations>\n");
            //分类
            xml.Append("\t<categorys>\n");
            if (SiteDat.GetDat(SiteCache.Column) == null)
            {
                SiteDat.SetDat(SiteCache.Column, new ColumnData(conn).SelectColumn(-1, 0, false));
            }
            foreach (ColumnItem vItem in (DataList<ColumnItem>)SiteDat.GetDat(SiteCache.Column))
            {
                xml.Append("\t\t<item>\n");
                xml.AppendFormat("\t\t\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.CategoryLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(vItem.Local))));
                xml.AppendFormat("\t\t\t<id>{0}</id>\n", vItem.ID);
                xml.AppendFormat("\t\t\t<name>{0}</name>\n", SiteFun.CDATA(vItem.Name));
                xml.AppendFormat("\t\t\t<intro>{0}</intro>\n", SiteFun.CDATA(vItem.Intro));
                xml.AppendFormat("\t\t\t<local>{0}</local>\n", SiteFun.CDATA(vItem.Local));
                xml.AppendFormat("\t\t\t<postCount>{0}</postCount>\n", vItem.PostCount);
                xml.Append("\t\t</item>\n");
            }
            xml.Append("\t</categorys>\n");
            //随机文章
            xml.Append("\t<randomArticles>\n");
            if (SiteDat.GetDat(SiteCache.RandomPost) == null)
            {
                SiteDat.SetDat(SiteCache.RandomPost, new PostData(conn).SelectPost(0, null, null, 1, setting.Parameter.AppendRandomArticleNum, 4, "A", false));
            }
            foreach (PostItem vItem in (DataList<PostItem>)SiteDat.GetDat(SiteCache.RandomPost))
            {
                ColumnItem columnItem = new ColumnItem();
                if (SiteDat.GetDat(string.Format(SiteCache.ColumnFormat, vItem.ColumnID)) == null)
                {
                    SiteDat.SetDat(string.Format(SiteCache.ColumnFormat, vItem.ColumnID), new ColumnData(conn).GetColumn(vItem.ColumnID));
                }
                columnItem = (ColumnItem)SiteDat.GetDat(string.Format(SiteCache.ColumnFormat, vItem.ColumnID));
                xml.Append("\t\t<item>\n");
                xml.AppendFormat("\t\t\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.ArticleLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(vItem.Local))));
                xml.AppendFormat("\t\t\t<id>{0}</id>\n", vItem.ID);
                xml.AppendFormat("\t\t\t<title>{0}</title>\n", SiteFun.CDATA(vItem.Title));
                xml.AppendFormat("\t\t\t<tags>{0}</tags>\n", SiteFun.CDATA(vItem.Tags));
                xml.AppendFormat("\t\t\t<local>{0}</local>\n", SiteFun.CDATA(vItem.Local));
                xml.AppendFormat("\t\t\t<author>{0}</author>\n", SiteFun.CDATA(vItem.Author));
                xml.AppendFormat("\t\t\t<postCount>{0}</postCount>\n", vItem.PostCount);
                xml.AppendFormat("\t\t\t<reader>{0}</reader>\n", vItem.Reader);
                xml.AppendFormat("\t\t\t<vote>{0}</vote>\n", vItem.Vote);
                xml.AppendFormat("\t\t\t<fine>{0}</fine>\n", vItem.Fine);
                xml.AppendFormat("\t\t\t<publish>{0}</publish>\n", vItem.Publish);
                xml.Append("\t\t\t<category>\n");
                xml.AppendFormat("\t\t\t\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.CategoryLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(columnItem.Local))));
                xml.AppendFormat("\t\t\t\t<id>{0}</id>\n", columnItem.ID);
                xml.AppendFormat("\t\t\t\t<local>{0}</local>\n", SiteFun.CDATA(columnItem.Local));
                xml.AppendFormat("\t\t\t\t<name>{0}</name>\n", SiteFun.CDATA(columnItem.Name));
                xml.AppendFormat("\t\t\t\t<intro>{0}</intro>\n", SiteFun.CDATA(columnItem.Intro));
                xml.AppendFormat("\t\t\t\t<postCount>{0}</postCount>\n", columnItem.PostCount);
                xml.Append("\t\t\t</category>\n");
                xml.Append("\t\t</item>\n");
            }
            xml.Append("\t</randomArticles>\n");
            //热门文章
            xml.Append("\t<hotArticles>\n");
            if (SiteDat.GetDat(SiteCache.HotPost) == null)
            {
                SiteDat.SetDat(SiteCache.HotPost, new PostData(conn).SelectPost(0, null, null, 1, setting.Parameter.AppendHotArticleNum, 2, "A", false));
            }
            foreach (PostItem vItem in (DataList<PostItem>)SiteDat.GetDat(SiteCache.HotPost))
            {
                ColumnItem columnItem = new ColumnItem();
                if (SiteDat.GetDat(string.Format(SiteCache.ColumnFormat, vItem.ColumnID)) == null)
                {
                    SiteDat.SetDat(string.Format(SiteCache.ColumnFormat, vItem.ColumnID), new ColumnData(conn).GetColumn(vItem.ColumnID));
                }
                columnItem = (ColumnItem)SiteDat.GetDat(string.Format(SiteCache.ColumnFormat, vItem.ColumnID));
                xml.Append("\t\t<item>\n");
                xml.AppendFormat("\t\t\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.ArticleLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(vItem.Local))));
                xml.AppendFormat("\t\t\t<id>{0}</id>\n", vItem.ID);
                xml.AppendFormat("\t\t\t<title>{0}</title>\n", SiteFun.CDATA(vItem.Title));
                xml.AppendFormat("\t\t\t<tags>{0}</tags>\n", SiteFun.CDATA(vItem.Tags));
                xml.AppendFormat("\t\t\t<local>{0}</local>\n", SiteFun.CDATA(vItem.Local));
                xml.AppendFormat("\t\t\t<author>{0}</author>\n", SiteFun.CDATA(vItem.Author));
                xml.AppendFormat("\t\t\t<postCount>{0}</postCount>\n", vItem.PostCount);
                xml.AppendFormat("\t\t\t<reader>{0}</reader>\n", vItem.Reader);
                xml.AppendFormat("\t\t\t<vote>{0}</vote>\n", vItem.Vote);
                xml.AppendFormat("\t\t\t<fine>{0}</fine>\n", vItem.Fine);
                xml.AppendFormat("\t\t\t<publish>{0}</publish>\n", vItem.Publish);
                xml.Append("\t\t\t<category>\n");
                xml.AppendFormat("\t\t\t\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.CategoryLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(columnItem.Local))));
                xml.AppendFormat("\t\t\t\t<id>{0}</id>\n", columnItem.ID);
                xml.AppendFormat("\t\t\t\t<local>{0}</local>\n", SiteFun.CDATA(columnItem.Local));
                xml.AppendFormat("\t\t\t\t<name>{0}</name>\n", SiteFun.CDATA(columnItem.Name));
                xml.AppendFormat("\t\t\t\t<intro>{0}</intro>\n", SiteFun.CDATA(columnItem.Intro));
                xml.AppendFormat("\t\t\t\t<postCount>{0}</postCount>\n", columnItem.PostCount);
                xml.Append("\t\t\t</category>\n");
                xml.Append("\t\t</item>\n");
            }
            xml.Append("\t</hotArticles>\n");
            //好文章
            xml.Append("\t<fineArticles>\n");
            if (SiteDat.GetDat(SiteCache.FinePost) == null)
            {
                SiteDat.SetDat(SiteCache.FinePost, new PostData(conn).SelectPost(0, null, null, 1, setting.Parameter.AppendFineArticleNum, 3, "A", false));
            }
            foreach (PostItem vItem in (DataList<PostItem>)SiteDat.GetDat(SiteCache.FinePost))
            {
                ColumnItem columnItem = new ColumnItem();
                if (SiteDat.GetDat(string.Format(SiteCache.ColumnFormat, vItem.ColumnID)) == null)
                {
                    SiteDat.SetDat(string.Format(SiteCache.ColumnFormat, vItem.ColumnID), new ColumnData(conn).GetColumn(vItem.ColumnID));
                }
                columnItem = (ColumnItem)SiteDat.GetDat(string.Format(SiteCache.ColumnFormat, vItem.ColumnID));
                xml.Append("\t\t<item>\n");
                xml.AppendFormat("\t\t\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.ArticleLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(vItem.Local))));
                xml.AppendFormat("\t\t\t<id>{0}</id>\n", vItem.ID);
                xml.AppendFormat("\t\t\t<title>{0}</title>\n", SiteFun.CDATA(vItem.Title));
                xml.AppendFormat("\t\t\t<tags>{0}</tags>\n", SiteFun.CDATA(vItem.Tags));
                xml.AppendFormat("\t\t\t<local>{0}</local>\n", SiteFun.CDATA(vItem.Local));
                xml.AppendFormat("\t\t\t<author>{0}</author>\n", SiteFun.CDATA(vItem.Author));
                xml.AppendFormat("\t\t\t<postCount>{0}</postCount>\n", vItem.PostCount);
                xml.AppendFormat("\t\t\t<reader>{0}</reader>\n", vItem.Reader);
                xml.AppendFormat("\t\t\t<vote>{0}</vote>\n", vItem.Vote);
                xml.AppendFormat("\t\t\t<fine>{0}</fine>\n", vItem.Fine);
                xml.AppendFormat("\t\t\t<publish>{0}</publish>\n", vItem.Publish);
                xml.Append("\t\t\t<category>\n");
                xml.AppendFormat("\t\t\t\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.CategoryLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(columnItem.Local))));
                xml.AppendFormat("\t\t\t\t<id>{0}</id>\n", columnItem.ID);
                xml.AppendFormat("\t\t\t\t<local>{0}</local>\n", SiteFun.CDATA(columnItem.Local));
                xml.AppendFormat("\t\t\t\t<name>{0}</name>\n", SiteFun.CDATA(columnItem.Name));
                xml.AppendFormat("\t\t\t\t<intro>{0}</intro>\n", SiteFun.CDATA(columnItem.Intro));
                xml.AppendFormat("\t\t\t\t<postCount>{0}</postCount>\n", columnItem.PostCount);
                xml.Append("\t\t\t</category>\n");
                xml.Append("\t\t</item>\n");
            }
            xml.Append("\t</fineArticles>\n");
            //最新评论
            xml.Append("\t<newComments>\n");
            if (SiteDat.GetDat(SiteCache.NewComment) == null)
            {
                SiteDat.SetDat(SiteCache.NewComment, new CommentData(conn).SelectComment(0, 1, setting.Parameter.AppendNewCommentNum, false));
            }
            foreach (CommentItem vItem in (DataList<CommentItem>)SiteDat.GetDat(SiteCache.NewComment))
            {
                PostItem postItem = new PostData(conn).GetPost(vItem.PostID);
                ColumnItem columnItem = new ColumnItem();
                if (SiteDat.GetDat(string.Format(SiteCache.ColumnFormat, postItem.ColumnID)) == null)
                {
                    SiteDat.SetDat(string.Format(SiteCache.ColumnFormat, postItem.ColumnID), new ColumnData(conn).GetColumn(postItem.ColumnID));
                }
                xml.Append("\t\t<item>\n");
                xml.AppendFormat("\t\t\t<id>{0}</id>\n", vItem.ID);
                xml.AppendFormat("\t\t\t<author>{0}</author>\n", SiteFun.CDATA(vItem.Author));
                xml.AppendFormat("\t\t\t<title>{0}</title>\n", SiteFun.CDATA(vItem.Title));
                xml.AppendFormat("\t\t\t<content>{0}</content>\n", SiteFun.CDATA(vItem.Content));
                xml.AppendFormat("\t\t\t<reply>{0}</reply>\n", SiteFun.CDATA(vItem.Reply));
                xml.AppendFormat("\t\t\t<mail>{0}</mail>\n", SiteFun.CDATA(vItem.Mail));
                xml.AppendFormat("\t\t\t<url>{0}</url>\n", SiteFun.CDATA(vItem.URL));
                xml.AppendFormat("\t\t\t<publish>{0}</publish>\n", vItem.Publish);
                xml.AppendFormat("\t\t\t<trackback>{0}</trackback>\n", vItem.Trackback);
                xml.Append("\t\t\t<article>\n");
                xml.AppendFormat("\t\t\t\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.ArticleLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(postItem.Local))));
                xml.AppendFormat("\t\t\t\t<id>{0}</id>\n", postItem.ID);
                xml.AppendFormat("\t\t\t\t<title>{0}</title>\n", SiteFun.CDATA(postItem.Title));
                xml.AppendFormat("\t\t\t\t<tags>{0}</tags>\n", SiteFun.CDATA(postItem.Tags));
                xml.AppendFormat("\t\t\t\t<local>{0}</local>\n", SiteFun.CDATA(postItem.Local));
                xml.AppendFormat("\t\t\t\t<author>{0}</author>\n", SiteFun.CDATA(postItem.Author));
                xml.AppendFormat("\t\t\t\t<postCount>{0}</postCount>\n", postItem.PostCount);
                xml.AppendFormat("\t\t\t\t<reader>{0}</reader>\n", postItem.Reader);
                xml.AppendFormat("\t\t\t\t<vote>{0}</vote>\n", postItem.Vote);
                xml.AppendFormat("\t\t\t\t<fine>{0}</fine>\n", postItem.Fine);
                xml.AppendFormat("\t\t\t\t<publish>{0}</publish>\n", postItem.Publish);
                xml.Append("\t\t\t\t<category>\n");
                xml.AppendFormat("\t\t\t\t\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.CategoryLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(columnItem.Local))));
                xml.AppendFormat("\t\t\t\t\t<id>{0}</id>\n", columnItem.ID);
                xml.AppendFormat("\t\t\t\t\t<local>{0}</local>\n", SiteFun.CDATA(columnItem.Local));
                xml.AppendFormat("\t\t\t\t\t<name>{0}</name>\n", SiteFun.CDATA(columnItem.Name));
                xml.AppendFormat("\t\t\t\t\t<intro>{0}</intro>\n", SiteFun.CDATA(columnItem.Intro));
                xml.AppendFormat("\t\t\t\t\t<postCount>{0}</postCount>\n", columnItem.PostCount);
                xml.Append("\t\t\t\t</category>\n");
                xml.Append("\t\t\t</article>\n");
                xml.Append("\t\t</item>\n");
            }
            xml.Append("\t</newComments>\n");
            //单页面
            xml.Append("\t<pages>\n");
            if (SiteDat.GetDat(SiteCache.Page) == null)
            {
                SiteDat.SetDat(SiteCache.Page, new PostData(conn).SelectPost(0, null, null, 1, 999, 0, "P", false));
            }
            foreach (PostItem vItem in (DataList<PostItem>)SiteDat.GetDat(SiteCache.Page))
            {
                xml.Append("\t\t<item>\n");
                xml.AppendFormat("\t\t\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.PageLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(vItem.Local))));
                xml.AppendFormat("\t\t\t<id>{0}</id>\n", vItem.ID);
                xml.AppendFormat("\t\t\t<title>{0}</title>\n", SiteFun.CDATA(vItem.Title));
                xml.AppendFormat("\t\t\t<local>{0}</local>\n", SiteFun.CDATA(vItem.Local));
                xml.AppendFormat("\t\t\t<publish>{0}</publish>\n", vItem.Publish);
                xml.Append("\t\t</item>\n");
            }
            xml.Append("\t</pages>\n");
            //热门标签
            xml.Append("\t<tags>\n");
            if (SiteDat.GetDat(SiteCache.HotTag) == null)
            {
                SiteDat.SetDat(SiteCache.HotTag, new TagData(conn).SelectTag(1, setting.Parameter.AppendHotTagNum, "PostCount", "DESC"));
            }
            foreach (TagItem vItem in (TagList)SiteDat.GetDat(SiteCache.HotTag))
            {
                xml.Append("\t\t<item>\n");
                xml.AppendFormat("\t\t\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.TagLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(vItem.Key))));
                xml.AppendFormat("\t\t\t<id>{0}</id>\n", vItem.ID);
                xml.AppendFormat("\t\t\t<key>{0}</key>\n", SiteFun.CDATA(vItem.Key));
                xml.AppendFormat("\t\t\t<postCount>{0}</postCount>\n", vItem.PostCount);
                xml.Append("\t\t</item>\n");
            }
            xml.Append("\t</tags>\n");
            //首页链接
            xml.Append("\t<fellows>\n");
            if (SiteDat.GetDat(SiteCache.Fellow) == null)
            {
                SiteDat.SetDat(SiteCache.Fellow, new FellowData(conn).SelectFellow(1, int.MaxValue, true, false));
            }
            DataList<FellowItem> fellows = (DataList<FellowItem>)SiteDat.GetDat(SiteCache.Fellow);
            foreach (FellowItem vItem in fellows)
            {
                xml.Append("\t\t<item>\n");
                xml.AppendFormat("\t\t\t<id>{0}</id>\n", vItem.ID);
                xml.AppendFormat("\t\t\t<name>{0}</name>\n", SiteFun.CDATA(vItem.Name));
                xml.AppendFormat("\t\t\t<url>{0}</url>\n", SiteFun.CDATA(vItem.URL));
                xml.AppendFormat("\t\t\t<logo>{0}</logo>\n", SiteFun.CDATA(vItem.Logo));
                xml.AppendFormat("\t\t\t<explain>{0}</explain>\n", SiteFun.CDATA(SiteFun.HtmlMatch(vItem.Explain)));
                xml.AppendFormat("\t\t\t<style>{0}</style>\n", SiteFun.CDATA(vItem.Style));
                xml.Append("\t\t</item>\n");
            }
            xml.Append("\t</fellows>\n");
            //自定义标签
            xml.Append("\t<myTag>\n");
            if (SiteDat.GetDat(SiteCache.MyTag) == null)
            {
                SiteDat.SetDat(SiteCache.MyTag, new MyTagData(conn).SelectMyTag(1, int.MaxValue, false));
            }
            foreach (MyTagItem vItem in (DataList<MyTagItem>)SiteDat.GetDat(SiteCache.MyTag))
            {
                xml.AppendFormat("\t\t<{0}>{1}</{0}>\n", vItem.Key, SiteFun.CDATA(vItem.Code));
            }
            xml.Append("\t</myTag>\n");
            //单页数据
            xml.Append("\t<this>\n");
            xml.AppendFormat("\t\t<title>{0}</title>\n", SiteFun.CDATA(title));
            xml.Append(thisXmlText);
            xml.Append("\t</this>\n");
            //扩展部分
            xml.Append("\t<expand>\n");
            foreach (object type in new SiteExpand().GetTypes(typeof(IWebExpandXml).FullName))
            {
                IWebExpandXml iae = ((IWebExpandXml)type);
                xml.Append(iae.OutXML());
            }
            xml.Append("\t</expand>\n");
            xml.Append("</ui>\n");
            return xml.ToString();
        }
    }
}
