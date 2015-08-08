using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GF.Data;
using GF.Logic.Template;
using GF.Logic.Web;
using GF.Core;
using System.IO;
using System.Text;
using GF.Data.Entity;
using GF.Logic;

namespace GF.Web
{
    public partial class _Default : System.Web.UI.Page
    {
        /// <summary>
        /// 显示内容
        /// </summary>
        /// <param name="sender">基类对象</param>
        /// <param name="e">事件对象</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            using (_DbHelper conn = new _DbHelper(SiteCfg.Conn))
            {
                conn.Open();
                conn.BeginTransaction();
                try
                {
                    XsltTemplate tpl = new XsltTemplate(conn);
                    //初始化全局页面类库
                    _WebBase wb = new _WebBase();
                    wb.InitConn();
                    //开始执行
                    switch (SiteFun.Query("act"))
                    {
                        case "page":
                            {
                                int id = SiteFun.ToInt(SiteFun.Query("id"));
                                string local = SiteFun.Query("local");
                                WebPage webPg = new WebPage(conn);
                                tpl.LoadXslt(Path.Combine(SiteCfg.Router, string.Format(SitePath.ThemePathFormat, webPg.OutTheme, "Page.View")));
                                tpl.BindXml(webPg.OutWrite(id, local));
                                break;
                            }
                        case "tags":
                            {
                                WebTags webTags = new WebTags(conn);
                                tpl.LoadXslt(Path.Combine(SiteCfg.Router, string.Format(SitePath.ThemePathFormat, webTags.OutTheme, "Tag.List")));
                                tpl.BindXml(webTags.OutWrite());
                                break;
                            }
                        case "fellows":
                            {
                                WebFellows webFlw = new WebFellows(conn);
                                tpl.LoadXslt(Path.Combine(SiteCfg.Router, string.Format(SitePath.ThemePathFormat, webFlw.OutTheme, "Fellow.List")));
                                tpl.BindXml(webFlw.OutWrite());
                                break;
                            }
                        case "expand":
                            {
                                foreach (object type in new SiteExpand().GetTypes(typeof(IWebExpand).FullName))
                                {
                                    IWebExpand iae = ((IWebExpand)type);
                                    if (iae.Key == SiteFun.Query("key"))
                                    {
                                        StringBuilder xml = new StringBuilder();
                                        xml.Append("\t\t<expand>\n");
                                        xml.AppendFormat("\t\t\t<css>{0}</css>\n", SiteFun.CDATA(iae.Css));
                                        xml.AppendFormat("\t\t\t<html>{0}</html>\n", SiteFun.CDATA(iae.OutHtml()));
                                        xml.Append("\t\t</expand>\n");
                                        tpl.LoadXslt(Path.Combine(SiteCfg.Router, string.Format(SitePath.ThemePathFormat, iae.OutTheme, "Expand.View")));
                                        tpl.BindXml(new _WebBaseXml(conn).OutBaseXml(iae.Name, xml.ToString()));
                                        break;
                                    }
                                }
                                break;
                            }
                        default:
                            {
                                WebArticle webArt = new WebArticle(conn);
                                switch (SiteFun.Query("mode"))
                                {
                                    case "view":
                                        {
                                            int id = SiteFun.ToInt(SiteFun.Query("id"));
                                            string local = SiteFun.Query("local");
                                            string pwd = SiteFun.Post("pwd");
                                            int page = SiteFun.ToInt(SiteFun.Query("page"));
                                            int pageSize = webArt.OutSetting.Parameter.CommentNum;
                                            string tplFile = "Article.View";
                                            PostItem postItem = new PostData(conn).GetPost(id);
                                            ColumnItem columnItem = new ColumnData(conn).GetColumn(postItem.ColumnID);
                                            if (postItem.ID == 0) { postItem = new PostData(conn).GetPost(local); }
                                            if (!string.IsNullOrEmpty(columnItem.ViewTemplate) && File.Exists(Path.Combine(SiteCfg.Router, string.Format(SitePath.ThemePathFormat, webArt.OutTheme, columnItem.ViewTemplate)))) { tplFile = columnItem.ViewTemplate; }
                                            tpl.LoadXslt(Path.Combine(SiteCfg.Router, string.Format(SitePath.ThemePathFormat, webArt.OutTheme, tplFile)));
                                            tpl.BindXml(webArt.OutWriteView(id, local, pwd, page, pageSize));
                                            break;
                                        }
                                    default:
                                        {
                                            int cid = SiteFun.ToInt(SiteFun.Query("cid"));
                                            string clocal = SiteFun.Query("clocal");
                                            string tag = SiteFun.Query("tag");
                                            string key = SiteFun.Query("key");
                                            int page = SiteFun.ToInt(SiteFun.Query("page"));
                                            int pageSize = webArt.OutSetting.Parameter.ArticleNum;
                                            string tplFile = "Article.List";
                                            //开始处理
                                            ColumnItem column = new ColumnData(conn).GetColumn(cid);
                                            if (column.ID == 0) { column = new ColumnData(conn).GetColumn(clocal); }
                                            if (column.ID == 0 && string.IsNullOrEmpty(tag) && string.IsNullOrEmpty(key) && page == 0 && File.Exists(Path.Combine(SiteCfg.Router, string.Format(SitePath.ThemePathFormat, webArt.OutTheme, "Index")))) { tplFile = "Index"; }
                                            else
                                            {
                                                if (column.ID > 0)
                                                {
                                                    if (!string.IsNullOrEmpty(column.ListTemplate) && File.Exists(Path.Combine(SiteCfg.Router, string.Format(SitePath.ThemePathFormat, webArt.OutTheme, column.ListTemplate)))) { tplFile = column.ListTemplate; }
                                                    if (column.PageSize > 0) { pageSize = column.PageSize; }
                                                }
                                            }
                                            tpl.LoadXslt(Path.Combine(SiteCfg.Router, string.Format(SitePath.ThemePathFormat, webArt.OutTheme, tplFile)));
                                            tpl.BindXml(webArt.OutWriteList(cid, clocal, tag, key, page, pageSize));
                                            break;
                                        }
                                }
                                break;
                            }
                    }
                    if (conn.ExecuteCount > 0) { conn.Commit(); }
                    tpl.Print();
                }
                catch (Exception err)
                {
                    if (conn.ExecuteCount > 0) { conn.Rollback(); }
                    IMyError exeError = new MyError();
                    foreach (IMyError myError in new SiteExpand().GetTypes(typeof(IMyError).FullName))
                    {
                        exeError = myError;
                        break;
                    }
                    exeError.PrintError(err);
                }
                conn.Close();
            }
        }
    }
}
