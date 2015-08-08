using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using GF.Core;
using GF.Logic.UI;
using GF.Data;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 后台默认页面类
    /// </summary>
    public class AdminWelcome
    {
        /// <summary>
        /// 写出主要代码
        /// </summary>
        public string OutCode()
        {
            SiteDat SiteDat = new SiteDat();
            HttpRequest request = HttpContext.Current.Request;
            StringBuilder sr = new StringBuilder();
            if (SiteFun.Query("reset") == "yes")
            {
                HttpRuntime.UnloadAppDomain();
                sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgRebootApplicationSucc")));
            }
            //快捷入口
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTd(HtmlUI.Link(HtmlUI.Image("Common/Admin/Icons/Basic.png", null) + "<br/>" + SiteDat.GetLan("AdmMenuBase"), "?act=basic&amp;mode=base")));
            sr.Append(HtmlUI.CreateTd(HtmlUI.Link(HtmlUI.Image("Common/Admin/Icons/ArticleAdd.png", null) + "<br/>" + SiteDat.GetLan("AddArt"), "?act=post&amp;mode=post&amp;m=A")));
            sr.Append(HtmlUI.CreateTd(HtmlUI.Link(HtmlUI.Image("Common/Admin/Icons/Article.png", null) + "<br/>" + SiteDat.GetLan("ArtList"), "?act=post&amp;mode=list&amp;m=A")));
            sr.Append(HtmlUI.CreateTd(HtmlUI.Link(HtmlUI.Image("Common/Admin/Icons/FellowAdd.png", null) + "<br/>" + SiteDat.GetLan("AddLink"), "?act=fellow&amp;mode=post")));
            sr.Append(HtmlUI.CreateTd(HtmlUI.Link(HtmlUI.Image("Common/Admin/Icons/Fellow.png", null) + "<br/>" + SiteDat.GetLan("LinkList"), "?act=fellow&amp;mode=list")));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTd(HtmlUI.Link(HtmlUI.Image("Common/Admin/Icons/Comment.png", null) + "<br/>" + SiteDat.GetLan("CmtList"), "?act=comment&amp;mode=list")));
            sr.Append(HtmlUI.CreateTd(HtmlUI.Link(HtmlUI.Image("Common/Admin/Icons/DbBackup.png", null) + "<br/>" + SiteDat.GetLan("DbBackup"), "?act=db&amp;mode=backup")));
            sr.Append(HtmlUI.CreateTd(HtmlUI.Link(HtmlUI.Image("Common/Admin/Icons/Sql.png", null) + "<br/>" + SiteDat.GetLan("ExeSql"), "?act=db&amp;mode=sql")));
            sr.Append(HtmlUI.CreateTd(HtmlUI.Link(HtmlUI.Image("Common/Admin/Icons/Cache.png", null) + "<br/>" + SiteDat.GetLan("CacheList"), "?act=info&amp;mode=cache")));
            sr.Append(HtmlUI.CreateTd(HtmlUI.Link(HtmlUI.Image("Common/Admin/Icons/File.png", null) + "<br/>" + SiteDat.GetLan("AttachList"), "?act=attach&amp;mode=list")));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTd(HtmlUI.Link(HtmlUI.Image("Common/Admin/Icons/Page.png", null) + "<br/>" + SiteDat.GetLan("AdmMenuPGMgr"), "?act=post&amp;mode=list&amp;m=P")));
            sr.Append(HtmlUI.CreateTd(HtmlUI.Link(HtmlUI.Image("Common/Admin/Icons/Tag.png", null) + "<br/>" + SiteDat.GetLan("AdmMenuTagMgr"), "?act=tag&amp;mode=list")));
            sr.Append(HtmlUI.CreateTd(HtmlUI.Link(HtmlUI.Image("Common/Admin/Icons/Master.png", null) + "<br/>" + SiteDat.GetLan("AdmMenuMgrControl"), "?act=user&amp;mode=list")));
            sr.Append(HtmlUI.CreateTd(HtmlUI.Link(HtmlUI.Image("Common/Admin/Icons/Upgrade.png", null) + "<br/>" + SiteDat.GetLan("AdmMenuChkUP"), "?act=upgrade&amp;mode=info")));
            sr.Append(HtmlUI.CreateTd(HtmlUI.Link(HtmlUI.Image("Common/Admin/Icons/Business.png", null) + "<br/>" + SiteDat.GetLan("AdmMenuOffi"), null)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            //系统信息
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart("onCenter"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("CurrVer")) + HtmlUI.CreateTd(SiteCfg.SystemVersionFull));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(HtmlUI.Link(SiteDat.GetLan("RebootApplication"), "?reset=yes")));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("SvrIP")) + HtmlUI.CreateTd(request.ServerVariables["LOCAL_ADDR"]));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("SvrName")) + HtmlUI.CreateTd(request.ServerVariables["SERVER_NAME"]));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("SvrSoft")) + HtmlUI.CreateTd(request.ServerVariables["SERVER_SOFTWARE"]));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("SvrPort")) + HtmlUI.CreateTd(request.ServerVariables["SERVER_PORT"]));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("SvrApplPath")) + HtmlUI.CreateTd(SiteCfg.Router));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("SvrFilePath")) + HtmlUI.CreateTd(request.ServerVariables["PATH_TRANSLATED"]));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("SvrHost")) + HtmlUI.CreateTd(request.ServerVariables["HTTP_HOST"]));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("SvrLang")) + HtmlUI.CreateTd(request.ServerVariables["HTTP_ACCEPT_LANGUAGE"]));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("SvrCLR")) + HtmlUI.CreateTd(".NET CLR v" + Environment.Version.Major + "." + Environment.Version.Minor + "." + Environment.Version.Build + "." + Environment.Version.Revision));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("SvrOS")) + HtmlUI.CreateTd(Environment.OSVersion.ToString()));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("SvrCpuType")) + HtmlUI.CreateTd(Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER")));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("SvrCpuNum")) + HtmlUI.CreateTd(Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS")));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Memory")) + HtmlUI.CreateTd(SiteFun.FormatLength(Process.GetCurrentProcess().PeakWorkingSet64)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            //最新评论
            //sr.Append(new AdminComment().List(20));
            return sr.ToString();
        }

        /// <summary>
        /// 授权中心
        /// </summary>
        public string Business()
        {
            StringBuilder sr = new StringBuilder();
            /*
            string key = new SettingData(conn).GetSetting().Parameter.Key;
            if (Regex.IsMatch(key, @"^[A-Z0-9]{10}$"))
            {
                try
                {
                    SiteFun fun = new SiteFun();
                    int page = fun.ToInt(fun.Query("page"));
                    string domain = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
                    if (domain != null) { domain = domain.ToUpper(); }
                    SiteXml xml = new SiteXml();
                    xml.LoadOfUri(string.Format("http://127.0.0.1/Business.xml?domain={0}&amp;key={1}&amp;page={2}", domain, key, page));
                    //官网信息
                    sr.Append(AdminUI.AdminBoxStart(true));
                    sr.Append(HtmlUI.TableStart("onCenter"));
                    sr.Append(HtmlUI.TrStart());
                    sr.Append(HtmlUI.CreateTh("授权域名") + HtmlUI.CreateTd(xml.GetString("/response/info/domain")));
                    sr.Append(HtmlUI.TrFinal());
                    sr.Append(HtmlUI.TrStart());
                    sr.Append(HtmlUI.CreateTh("授权密匙") + HtmlUI.CreateTd(xml.GetString("/response/info/key")));
                    sr.Append(HtmlUI.TrFinal());
                    sr.Append(HtmlUI.TrStart());
                    sr.Append(HtmlUI.CreateTh("授权期限") + HtmlUI.CreateTd(string.Format("{0} - {1}", xml.GetDate("/response/info/beginDate"), xml.GetDate("/response/info/endDate"))));
                    sr.Append(HtmlUI.TrFinal());
                    sr.Append(HtmlUI.TableFinal());
                    sr.Append(AdminUI.AdminBoxFinal());
                    if (xml.GetInt("/response/info/ratify") == 1)
                    {
                        int rows = xml.GetInt("/response/data/rows");
                        int pageSize = xml.GetInt("/response/data/pageSize");
                        int currPage = xml.GetInt("/response/data/currPage");
                        foreach (XmlNode node in xml.SelecttNodes("/response/data/workOrders/item"))
                        {
                            XmlNode contentNode = node.SelectSingleNode("content");
                            if (contentNode != null)
                            {
                                sr.Append(AdminUI.AdminBoxStart(true));
                                sr.Append(HtmlUI.TableStart("onCenter"));
                                sr.Append(HtmlUI.TrStart());
                                sr.Append(HtmlUI.CreateTd(contentNode.InnerText));
                                sr.Append(HtmlUI.TrFinal());
                                sr.Append(HtmlUI.TableFinal());
                                sr.Append(AdminUI.AdminBoxFinal());
                                XmlNode replyNode = node.SelectSingleNode("reply");
                                if (replyNode != null)
                                {
                                    if (!string.IsNullOrEmpty(replyNode.InnerText))
                                    {
                                        sr.Append(AdminUI.AdminBoxStart());
                                        sr.Append(HtmlUI.TableStart("onCenter"));
                                        sr.Append(HtmlUI.TrStart());
                                        sr.Append(HtmlUI.CreateTd(replyNode.InnerText));
                                        sr.Append(HtmlUI.TrFinal());
                                        sr.Append(HtmlUI.TableFinal());
                                        sr.Append(AdminUI.AdminBoxFinal());
                                    }
                                }
                            }
                        }
                        //商业用户通知和分页
                        sr.Append(AdminUI.AdminBoxStart());
                        sr.Append(HtmlUI.TableStart());
                        string msg = xml.GetString("/response/info/msg");
                        if (!string.IsNullOrEmpty(msg))
                        {
                            sr.Append(HtmlUI.TrStart());
                            sr.Append(HtmlUI.CreateTd(msg));
                            sr.Append(HtmlUI.TrFinal());
                        }
                        sr.Append(HtmlUI.TrStart());
                        sr.Append(HtmlUI.CreateTd(new SitePages().Make(rows, currPage, pageSize, "?act=info&amp;mode=business&amp;page={0}")));
                        sr.Append(HtmlUI.TrFinal());
                        sr.Append(HtmlUI.TableFinal());
                        sr.Append(AdminUI.AdminBoxFinal());
                    }
                    else
                    {
                        sr.Append(AdminUI.AdminBoxStart(true));
                        sr.Append(HtmlUI.TableStart());
                        sr.Append(HtmlUI.TrStart());
                        sr.Append(HtmlUI.CreateTd(AdminUI.ErrorBox("您的商业帐户受限，请与官网联系。")));
                        sr.Append(HtmlUI.TrFinal());
                        sr.Append(HtmlUI.TableFinal());
                        sr.Append(AdminUI.AdminBoxFinal());
                    }
                }
                catch (Exception err)
                {
                    sr.Append(AdminUI.AdminBoxStart(true));
                    sr.Append(HtmlUI.TableStart());
                    sr.Append(HtmlUI.TrStart());
                    sr.Append(HtmlUI.CreateTd(AdminUI.ErrorBox("授权中心正在维护，请稍后访问。【" + err.Message + "】")));
                    sr.Append(HtmlUI.TrFinal());
                    sr.Append(HtmlUI.TableFinal());
                    sr.Append(AdminUI.AdminBoxFinal());
                }
            }
            else
            {
                sr.Append(AdminUI.AdminBoxStart(true));
                sr.Append(HtmlUI.TableStart());
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTd(AdminUI.ErrorBox("授权密匙段不存在或不正确。")));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.TableFinal());
                sr.Append(AdminUI.AdminBoxFinal());
            }
            */
            return sr.ToString();
        }
    }
}
