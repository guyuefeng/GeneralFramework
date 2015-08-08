using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GF.Logic.Admin;
using GF.Core;
using GF.Data;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.IO;
using GF.Logic;

namespace GF.Web
{
    public partial class AdminCP : System.Web.UI.Page
    {
        private _AdminCookie _ac = new _AdminCookie();

        /// <summary>
        /// 获取标题
        /// </summary>
        /// <returns></returns>
        public string GetTitle()
        {
            return SiteDat.GetLan("AdmCpTitle");
        }

        /// <summary>
        /// 后台页面执行过程
        /// </summary>
        /// <param name="sender">系统对象</param>
        /// <param name="e">事件对象</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { Page.DataBind(); }
            using (_DbHelper conn = new _DbHelper(SiteCfg.Conn))
            {
                conn.Open();
                conn.BeginTransaction();
                try
                {
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    //菜单HTML设置
                    StringBuilder parentMenu = new StringBuilder();
                    parentMenu.AppendLine("<div class=\"item\" id=\"{$ID}\">");
                    parentMenu.AppendLine("\t<div class=\"top\">");
                    parentMenu.AppendLine("\t\t<a class=\"menuCap\" rel=\"{$Rel}\">{$Caption}</a><span></span>");
                    parentMenu.AppendLine("\t</div>");
                    parentMenu.AppendLine("\t<div class=\"content\">");
                    parentMenu.AppendLine("\t\t<ul>");
                    parentMenu.AppendLine("\t\t{$ItemList}");
                    parentMenu.AppendLine("\t\t</ul>");
                    parentMenu.AppendLine("\t</div>");
                    parentMenu.AppendLine("</div>");
                    string menuItem = "<li><a href=\"{$Link}\">{$Caption}</a>{$Icon}</li>";
                    //初始化后台基类
                    _AdminBase admBase = new _AdminBase(conn);
                    //初始化全局数据连接
                    admBase.InitConn();
                    menuDisplay.Text = new _AdminBase(conn).MenuCode(GetMenuXml(admBase.IsLogin), parentMenu.ToString(), menuItem);
                    //是否登录
                    if (admBase.IsLogin)
                    {
                        //是否是用户控件扩展
                        if (SiteFun.Query("act") == "extend")
                        {
                            try
                            {
                                string folder = SiteFun.Query("folder");
                                string key = SiteFun.Query("key");
                                if (!string.IsNullOrEmpty(folder)) { folder = folder.Replace("..", string.Empty); }
                                XmlDocument extendXml = new XmlDocument();
                                string configXmlPath = Path.Combine(SiteCfg.Router, string.Format("Common/Expand/_Admin/{0}/Config.xml", folder));
                                if (File.Exists(configXmlPath))
                                {
                                    extendXml.Load(configXmlPath);
                                    //用户控件的文件名
                                    string ucFile = string.Empty;
                                    foreach (XmlNode node in extendXml.SelectNodes("/root/item"))
                                    {
                                        if (node.Attributes["key"].Value == key)
                                        {
                                            ucFile = node.Attributes["file"].Value;
                                            if (!string.IsNullOrEmpty(ucFile)) { ucFile = ucFile.Replace("..", string.Empty); }
                                            break;
                                        }
                                    }
                                    string ucFilePath = string.Format("Common/Expand/_Admin/{0}/{1}", folder, ucFile);
                                    if (File.Exists(Path.Combine(SiteCfg.Router, ucFilePath)))
                                    {
                                        Control uc = new UserControl().LoadControl(ucFilePath);
                                        mainDisplay.Controls.Add(uc);
                                    }
                                    else { mainDisplay.Text = "No " + ucFilePath; }
                                }
                            }
                            catch (Exception er) { mainDisplay.Text = er.Message; }
                        }
                        else { mainDisplay.Text = getMainCode(conn); }
                    }
                    else { mainDisplay.Text = admBase.LoginWindow(); }
                    powered.Text = SiteCfg.Powered;
                    if (conn.ExecuteCount > 0) { conn.Commit(); }
                    watch.Stop();
                    debug.Text = watch.Elapsed.TotalSeconds.ToString("0.0000");
                    watch.Reset();
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

        /// <summary>
        /// 获取管理菜单的XML内容
        /// </summary>
        /// <param name="isLogin">是否登录状态</param>
        /// <returns>返回完整XML内容</returns>
        private string GetMenuXml(bool isLogin)
        {
            StringBuilder xml = new StringBuilder();
            xml.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            xml.AppendLine("<root>");
            int i = 0;
            if (isLogin)
            {
                i++;
                xml.AppendFormat("<parent name=\"{0}\" id=\"menu_{1}\" rel=\"{1}\">", SiteDat.GetLan("AdmMenuIdx"), i);
                xml.AppendFormat("<item name=\"{0}\" link=\"./\" icon=\"home.gif\"/>", SiteDat.GetLan("AdmMenuBackHome"));
                xml.AppendFormat("<item name=\"{0}\" link=\"?\" icon=\"system.gif\"/>", SiteDat.GetLan("AdmMenuWelcome"));
                xml.AppendLine("<item name=\"" + SiteDat.GetLan("AdmMenuExit") + "\" link=\"javascript:if(confirm('" + SiteDat.GetLan("MsgAdmExit") + "')){top.location.href='?act=exit';};\" icon=\"verify.gif\"/>");
                xml.AppendLine("</parent>");
                i++;
                xml.AppendFormat("<parent name=\"{0}\" id=\"menu_{1}\" rel=\"{1}\">", SiteDat.GetLan("AdmMenuBasic"), i);
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=basic&amp;mode=base\" icon=\"system.gif\"/>", SiteDat.GetLan("AdmMenuBase"));
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=basic&amp;mode=par\" icon=\"tool.gif\"/>", SiteDat.GetLan("AdmMenuPar"));
                xml.AppendLine("</parent>");
                i++;
                xml.AppendFormat("<parent name=\"{0}\" id=\"menu_{1}\" rel=\"{1}\">", SiteDat.GetLan("AdmMenuTheme"), i);
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=theme&amp;mode=select\" icon=\"dark.gif\"/>", SiteDat.GetLan("AdmMenuThemeSel"));
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=myTag&amp;mode=post\" icon=\"\"/>", SiteDat.GetLan("AdmMenuAddMT"));
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=myTag&amp;mode=list\" icon=\"\"/>", SiteDat.GetLan("AdmMenuMTMgr"));
                xml.AppendLine("</parent>");
                i++;
                xml.AppendFormat("<parent name=\"{0}\" id=\"menu_{1}\" rel=\"{1}\">", SiteDat.GetLan("AdmMenuDB"), i);
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=db&amp;mode=backup\" icon=\"save.gif\"/>", SiteDat.GetLan("AdmMenuDBBackup"));
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=db&amp;mode=sql\" icon=\"data.gif\"/>", SiteDat.GetLan("AdmMenuDBSQL"));
                xml.AppendLine("</parent>");
                i++;
                xml.AppendFormat("<parent name=\"{0}\" id=\"menu_{1}\" rel=\"{1}\">", SiteDat.GetLan("AdmMenuColumnMgr"), i);
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=column&amp;mode=post\" icon=\"add.gif\"/>", SiteDat.GetLan("AdmMenuColumnAdd"));
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=column&amp;mode=list\" icon=\"dark.gif\"/>", SiteDat.GetLan("AdmMenuColumnList"));
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=tag&amp;mode=list\" icon=\"lab.gif\"/>", SiteDat.GetLan("AdmMenuTagMgr"));
                xml.AppendLine("</parent>");
                i++;
                xml.AppendFormat("<parent name=\"{0}\" id=\"menu_{1}\" rel=\"{1}\">", SiteDat.GetLan("AdmMenuMain"), i);
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=post&amp;mode=post&amp;m=A\" icon=\"add.gif\"/>", SiteDat.GetLan("AdmMenuArtAdd"));
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=post&amp;mode=list&amp;m=A\" icon=\"dark.gif\"/>", SiteDat.GetLan("AdmMenuArtMgr"));
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=comment&amp;mode=list\" icon=\"comment.gif\"/>", SiteDat.GetLan("AdmMenuCMTMgr"));
                xml.AppendLine("</parent>");
                i++;
                xml.AppendFormat("<parent name=\"{0}\" id=\"menu_{1}\" rel=\"{1}\">", SiteDat.GetLan("AdmMenuPG"), i);
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=post&amp;mode=post&amp;m=P\" icon=\"add.gif\"/>", SiteDat.GetLan("AdmMenuPGAdd"));
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=post&amp;mode=list&amp;m=P\" icon=\"dark.gif\"/>", SiteDat.GetLan("AdmMenuPGMgr"));
                xml.AppendLine("</parent>");
                i++;
                xml.AppendFormat("<parent name=\"{0}\" id=\"menu_{1}\" rel=\"{1}\">", SiteDat.GetLan("AdmMenuMgr"), i);
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=user&amp;mode=post\" icon=\"add.gif\"/>", SiteDat.GetLan("AdmMenuMgrAdd"));
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=user&amp;mode=list\" icon=\"user.gif\"/>", SiteDat.GetLan("AdmMenuMgrControl"));
                xml.AppendLine("</parent>");
                i++;
                xml.AppendFormat("<parent name=\"{0}\" id=\"menu_{1}\" rel=\"{1}\">", SiteDat.GetLan("AdmMenuFlw"), i);
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=fellow&amp;mode=post\" icon=\"add.gif\"/>", SiteDat.GetLan("AdmMenuFlwAdd"));
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=fellow&amp;mode=list&amp;home=1\" icon=\"dark.gif\"/>", SiteDat.GetLan("AdmMenuFlwIdx"));
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=fellow&amp;mode=list\" icon=\"delete.gif\"/>", SiteDat.GetLan("AdmMenuFlwMgr"));
                xml.AppendLine("</parent>");
                i++;
                xml.AppendFormat("<parent name=\"{0}\" id=\"menu_{1}\" rel=\"{1}\">", SiteDat.GetLan("AdmMenuLog"), i);
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=info&amp;mode=cache\" icon=\"data.gif\"/>", SiteDat.GetLan("AdmMenuCache"));
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=info&amp;mode=tb\" icon=\"comment.gif\"/>", SiteDat.GetLan("AdmMenuTrackback"));
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=attach&amp;mode=list\" icon=\"attach.gif\"/>", SiteDat.GetLan("AttachList"));
                xml.AppendLine("</parent>");
                //一般扩展
                i++;
                xml.AppendFormat("<parent name=\"{0}\" id=\"menu_{1}\" rel=\"{1}\">", SiteDat.GetLan("AdmMenuExpand"), i);
                foreach (object type in new SiteExpand().GetTypes(typeof(IAdminExpand).FullName))
                {
                    IAdminExpand iae = ((IAdminExpand)type);
                    xml.AppendFormat("<item name=\"{0}\" link=\"?act=expand&amp;key={1}\" icon=\"\"/>", iae.Name, iae.Key);
                }
                //超级扩展
                foreach (string folder in Directory.GetDirectories(Path.Combine(SiteCfg.Router, "Common/Expand/_Admin")))
                {
                    DirectoryInfo dInfo = new DirectoryInfo(folder);
                    string xmlPath = Path.Combine(folder, "Config.xml");
                    if (File.Exists(xmlPath))
                    {
                        XmlDocument cfgXml = new XmlDocument();
                        cfgXml.Load(xmlPath);
                        foreach (XmlNode node in cfgXml.SelectNodes("/root/item"))
                        {
                            if (node.Attributes["menuShow"].Value == "yes")
                            {
                                xml.AppendFormat("<item name=\"{0}\" link=\"?act=extend&amp;folder={1}&amp;key={2}\" icon=\"\"/>", node.Attributes["name"].Value, dInfo.Name, node.Attributes["key"].Value);
                            }
                        }
                    }
                }
                xml.AppendLine("</parent>");
                i++;
                xml.AppendFormat("<parent name=\"{0}\" id=\"menu_{1}\" rel=\"{1}\">", SiteDat.GetLan("AdmMenuUpgrade"), i);
                xml.AppendFormat("<item name=\"{0}\" link=\"?act=upgrade&amp;mode=info\" icon=\"verify.gif\"/>", SiteDat.GetLan("AdmMenuChkUP"));
                xml.AppendLine("</parent>");
            }
            else
            {
                i++;
                xml.AppendFormat("<parent name=\"{0}\" id=\"menu_{1}\" rel=\"{1}\">", SiteDat.GetLan("AdmMenuIdx"), i);
                xml.AppendFormat("<item name=\"{0}\" link=\"?\" icon=\"\"/>", SiteDat.GetLan("AdmMenuWelcome"));
                xml.AppendFormat("<item name=\"{0}\" link=\"Readme.html\" icon=\"\"/>", SiteDat.GetLan("AdmMenuReadme"));
                xml.AppendFormat("<item name=\"{0}\" link=\"{1}page/download.aspx\" icon=\"\"/>", SiteDat.GetLan("AdmMenuUPLog"), SiteCfg.WebSite);
                xml.AppendLine("</parent>");
                i++;
                xml.AppendFormat("<parent name=\"{0}\" id=\"menu_{1}\" rel=\"{1}\">", SiteDat.GetLan("AdmMenuHelpOL"), i);
                xml.AppendFormat("<item name=\"{0}\" link=\"{1}\" icon=\"\"/>", SiteDat.GetLan("AdmMenuHelpCenter"), SiteCfg.WebSite);
                xml.AppendLine("</parent>");
                i++;
                xml.AppendFormat("<parent name=\"{0}\" id=\"menu_{1}\" rel=\"{1}\">", SiteDat.GetLan("AdmMenuOffi"), i);
                xml.AppendFormat("<item name=\"{0}\" link=\"{1}\" icon=\"\"/>", SiteDat.GetLan("AdmMenuOffiSite"), SiteCfg.WebSite);
                xml.AppendFormat("<item name=\"{0}\" link=\"mailto:master@isroc.com\" icon=\"\"/>", SiteDat.GetLan("AdmMenuOffiContact"));
                xml.AppendLine("</parent>");
            }
            xml.AppendLine("</root>");
            return xml.ToString();
        }

        /// <summary>
        /// 显示主要部分的代码
        /// </summary>
        private string getMainCode(_DbHelper conn)
        {
            string sr = string.Empty;
            switch (SiteFun.Query("act"))
            {
                case "exit":
                    {
                        _ac.Clear();
                        Response.Redirect(Request.FilePath);
                        break;
                    }
                case "basic":
                    {
                        switch (SiteFun.Query("mode"))
                        {
                            case "par": { sr += new AdminSetting(conn).Parameter(); break; }
                            default: { sr += new AdminSetting(conn).Basic(); break; }
                        }
                        break;
                    }
                case "theme":
                    {
                        switch (SiteFun.Query("mode"))
                        {
                            default: { sr += new AdminTheme(conn).Select(); break; }
                        }
                        break;
                    }
                case "myTag":
                    {
                        switch (SiteFun.Query("mode"))
                        {
                            case "post": { sr += new AdminMyTag(conn).Post(); break; }
                            default: { sr += new AdminMyTag(conn).List(); break; }
                        }
                        break;
                    }
                case "db":
                    {
                        switch (SiteFun.Query("mode"))
                        {
                            case "sql": { sr += new AdminDB(conn).SQL(); break; }
                            default: { sr += new AdminDB(conn).Backup(); break; }
                        }
                        break;
                    }
                case "column":
                    {
                        switch (SiteFun.Query("mode"))
                        {
                            case "post": { sr += new AdminColumn(conn).Post(); break; }
                            default: { sr += new AdminColumn(conn).List(); break; }
                        }
                        break;
                    }
                case "tag":
                    {
                        switch (SiteFun.Query("mode"))
                        {
                            default: { sr += new AdminTag(conn).List(); break; }
                        }
                        break;
                    }
                case "post":
                    {
                        string m = SiteFun.Query("m");
                        switch (SiteFun.Query("mode"))
                        {
                            case "post": { sr += new AdminPost(conn).Post(m); break; }
                            default: { sr += new AdminPost(conn).List(m); break; }
                        }
                        break;
                    }
                case "comment":
                    {
                        switch (SiteFun.Query("mode"))
                        {
                            case "post": { sr += new AdminComment(conn).Post(); break; }
                            default: { sr += new AdminComment(conn).List(20); break; }
                        }
                        break;
                    }
                case "fellow":
                    {
                        switch (SiteFun.Query("mode"))
                        {
                            case "post": { sr += new AdminFellow(conn).Post(); break; }
                            default: { sr += new AdminFellow(conn).List(); break; }
                        }
                        break;
                    }
                case "info":
                    {
                        switch (SiteFun.Query("mode"))
                        {
                            case "cache": { sr += new AdminCache().List(); break; }
                            case "tb": { sr += new AdminTrackback(conn).List(); break; }
                            default: { sr += new AdminWelcome().Business(); break; }
                        }
                        break;
                    }
                case "user":
                    {
                        switch (SiteFun.Query("mode"))
                        {
                            case "post": { sr += new AdminUser(conn).Post(); break; }
                            default: { sr += new AdminUser(conn).List(); break; }
                        }
                        break;
                    }
                case "attach":
                    {
                        switch (SiteFun.Query("mode"))
                        {
                            default: { sr += new AdminAttach(conn).List(); break; }
                        }
                        break;
                    }
                case "expand":
                    {
                        string key = SiteFun.Query("key");
                        sr += new AdminExpand().OutWrite(key);
                        break;
                    }
                case "upgrade":
                    {
                        switch (SiteFun.Query("mode"))
                        {
                            case "info": { sr += new AdminUpgrade().OutWrite(); break; }
                        }
                        break;
                    }
                default:
                    {
                        sr += new AdminWelcome().OutCode();
                        break;
                    }
            }
            return sr;
        }
    }
}
