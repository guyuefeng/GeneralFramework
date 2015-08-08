using System.Collections;
using System.IO;
using System.Text;
using GF.Core;
using GF.Data;
using GF.Data.Entity;
using GF.Logic.UI;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 系统配置类
    /// </summary>
    public class AdminSetting
    {
        private _DbHelper conn;

        public AdminSetting(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        private void ClearCache()
        {
            SiteDat.RemoveDat(SiteCache.Setting);
        }

        /// <summary>
        /// 参数设置
        /// </summary>
        public string Parameter()
        {
            StringBuilder sr = new StringBuilder();
            SettingData setData = new SettingData(conn);
            //取得默认值
            if (SiteFun.IsPost)
            {
                SettingParameterItem postVal = new SettingParameterItem();
                postVal.WatermarkSeat = SiteFun.ToInt(SiteFun.Post("ws"));
                postVal.WatermarkPath = SiteFun.Post("wmPath");
                postVal.ArticleNum = SiteFun.ToInt(SiteFun.Post("artNum"));
                postVal.CommentNum = SiteFun.ToInt(SiteFun.Post("cmtNum"));
                postVal.AppendFineArticleNum = SiteFun.ToInt(SiteFun.Post("appFAN"));
                postVal.AppendRandomArticleNum = SiteFun.ToInt(SiteFun.Post("appRAN"));
                postVal.AppendHotArticleNum = SiteFun.ToInt(SiteFun.Post("appHAN"));
                postVal.AppendHotTagNum = SiteFun.ToInt(SiteFun.Post("appHTN"));
                postVal.AppendNewCommentNum = SiteFun.ToInt(SiteFun.Post("appNCN"));
                postVal.RssNum = SiteFun.ToInt(SiteFun.Post("rssNum"));
                postVal.RssMode = SiteFun.ToInt(SiteFun.Post("rssMode"));
                postVal.Key = SiteFun.Post("key");
                setData.UpdateSettingParameter(postVal);
                sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgUpdateDat")));
                ClearCache();
            }
            SettingItem old = setData.GetSetting();
            sr.Append(HtmlUI.FormStart());
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart("onCenter"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("RssNum")) + HtmlUI.CreateTd(HtmlUI.Input("rssNum", 5, null, old.Parameter.RssNum)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("RssMode")) + HtmlUI.CreateTd(HtmlUI.RadioInput(SiteDat.GetLan("ShowExplain"), "rssMode", 0, old.Parameter.RssMode == 0) + HtmlUI.RadioInput(SiteDat.GetLan("ShowContent"), "rssMode", 1, old.Parameter.RssMode == 1)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart("onCenter"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("ArtNum")) + HtmlUI.CreateTd(HtmlUI.Input("artNum", 5, null, old.Parameter.ArticleNum)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("CmtNum")) + HtmlUI.CreateTd(HtmlUI.Input("cmtNum", 5, null, old.Parameter.CommentNum)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart("onCenter"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("TopArtNum")) + HtmlUI.CreateTd(HtmlUI.Input("appFAN", 5, null, old.Parameter.AppendFineArticleNum)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart()); 
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("RandomArtNum")) + HtmlUI.CreateTd(HtmlUI.Input("appRAN", 5, null, old.Parameter.AppendRandomArticleNum)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("HotArtNum")) + HtmlUI.CreateTd(HtmlUI.Input("appHAN", 5, null, old.Parameter.AppendHotArticleNum)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart()); 
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("HotTagNum")) + HtmlUI.CreateTd(HtmlUI.Input("appHTN", 5, null, old.Parameter.AppendHotTagNum)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("NewCmtNum")) + HtmlUI.CreateTd(HtmlUI.Input("appNCN", 5, null, old.Parameter.AppendNewCommentNum)));
            sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd());
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            //水印设置
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart("onCenter"));
            sr.Append(HtmlUI.TrStart());
            ArrayList values = new ArrayList();
            for (int wmI = 0; wmI <= 9; wmI++)
            {
                values.Add(wmI);
            }
            ArrayList caps = new ArrayList();
            caps.Add(SiteDat.GetLan("Close"));
            caps.Add(SiteDat.GetLan("PTopLeft"));
            caps.Add(SiteDat.GetLan("PTopCen"));
            caps.Add(SiteDat.GetLan("PTopRight"));
            caps.Add(SiteDat.GetLan("PMdlLeft"));
            caps.Add(SiteDat.GetLan("PMdlCen"));
            caps.Add(SiteDat.GetLan("PMdlRight"));
            caps.Add(SiteDat.GetLan("PBtmLeft"));
            caps.Add(SiteDat.GetLan("PBtmCen"));
            caps.Add(SiteDat.GetLan("PBtmRight"));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("WmPosition")));
            sr.Append(HtmlUI.CreateTd(HtmlUI.Input("wmPath", 50, null, SiteFun.HtmlEncode(old.Parameter.WatermarkPath)) + HtmlUI.CreateSelect("ws", values, caps, old.Parameter.WatermarkSeat)));
            sr.Append(HtmlUI.TrFinal());
            if (File.Exists(Path.Combine(SiteCfg.Router, old.Parameter.WatermarkPath)))
            {
                sr.Append(HtmlUI.CreateTh());
                sr.Append(HtmlUI.CreateTd(HtmlUI.Image(SiteCfg.Path + old.Parameter.WatermarkPath, old.Parameter.WatermarkPath)));
                sr.Append(HtmlUI.TrFinal());
            }
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            //水印
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart("onCenter"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("BusinessKey")) + HtmlUI.CreateTd(HtmlUI.Input("key", 15, null, SiteFun.HtmlEncode(old.Parameter.Key))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(HtmlUI.SubmitButton() + HtmlUI.ResetButton()));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            sr.Append(HtmlUI.FormFinal());
            return sr.ToString();
        }

        /// <summary>
        /// 基础设置
        /// </summary>
        public string Basic()
        {
            StringBuilder sr = new StringBuilder();
            SettingData setData = new SettingData(conn);
            //取得默认值
            if (SiteFun.IsPost)
            {
                SettingBasicItem postVal = new SettingBasicItem();
                postVal.Name = SiteFun.Post("caption");
                postVal.URL = SiteFun.Post("url");
                if (!string.IsNullOrEmpty(postVal.URL))
                {
                    if (!postVal.URL.EndsWith("/")) { postVal.URL += "/"; }
                }
                postVal.ICP = SiteFun.Post("icp");
                //postVal.Language = SiteFun.Post("language");
                postVal.Intro = SiteFun.Post("intro");
                postVal.Keywords = SiteFun.Post("keywords");
                postVal.Affiche = SiteFun.Post("affiche");
                postVal.Filter = SiteFun.Post("filter");
                postVal.UploadExt = SiteFun.Post("uploadExt");
                postVal.Mail = SiteFun.Post("mail");
                postVal.MailFrom = SiteFun.Post("mailFrom");
                postVal.MailHost = SiteFun.Post("mailHost");
                postVal.MailPort = SiteFun.ToInt(SiteFun.Post("mailPort"));
                postVal.MailUserID = SiteFun.Post("mailUid");
                postVal.MailPassword = SiteFun.Post("mailPwd");
                setData.UpdateSettingBasic(postVal);
                sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgUpdateDat")));
                ClearCache();
            }
            SettingItem old = setData.GetSetting();
            sr.Append(HtmlUI.FormStart());
            sr.Append(AdminUI.AdminBoxStart(true));

            sr.Append(HtmlUI.TableStart("onCenter advancedBox"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("SiteCap")) + HtmlUI.CreateTd(HtmlUI.Input("caption", 20, null, SiteFun.HtmlEncode(old.Basic.Name))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("URL")) + HtmlUI.CreateTd(HtmlUI.Input("url", 50, null, SiteFun.HtmlEncode(old.Basic.URL))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Icp")) + HtmlUI.CreateTd(HtmlUI.Input("icp", 30, null, SiteFun.HtmlEncode(old.Basic.ICP))));
            sr.Append(HtmlUI.TrFinal());
            /*sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Lang")) + HtmlUI.CreateTd(HtmlUI.Input("language", 10, null, SiteFun.HtmlEncode(old.Basic.Language))));
            sr.Append(HtmlUI.TrFinal());*/
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Intro")) + HtmlUI.CreateTd(HtmlUI.Input("intro", 50, null, SiteFun.HtmlEncode(old.Basic.Intro))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Keywords")) + HtmlUI.CreateTd(HtmlUI.Input("keywords", 50, null, SiteFun.HtmlEncode(old.Basic.Keywords))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Affiche")) + HtmlUI.CreateTd(AdminUI.Editor("affiche", SiteFun.HtmlEncode(old.Basic.Affiche))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Mail")) + HtmlUI.CreateTd(HtmlUI.Input("mail", 30, null, SiteFun.HtmlEncode(old.Basic.Mail))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.TableFinal());
            //安全相关
            sr.Append(HtmlUI.TableStart("onCenter hidden advancedBox"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("IllChar")) + HtmlUI.CreateTd(HtmlUI.Textarea("filter", 10, 80, SiteFun.HtmlEncode(old.Basic.Filter))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("SafeSuffix")) + HtmlUI.CreateTd(HtmlUI.Input("uploadExt", 60, null, SiteFun.HtmlEncode(old.Basic.UploadExt))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            //邮件服务器
            sr.Append(HtmlUI.TableStart("onCenter hidden advancedBox"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("MailFrom")) + HtmlUI.CreateTd(HtmlUI.Input("mailFrom", 30, null, SiteFun.HtmlEncode(old.Basic.MailFrom))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("MailSvr")) + HtmlUI.CreateTd(HtmlUI.Input("mailHost", 30, null, SiteFun.HtmlEncode(old.Basic.MailHost))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("MailSvrPort")) + HtmlUI.CreateTd(HtmlUI.Input("mailPort", 5, null, old.Basic.MailPort)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("MailUserId")) + HtmlUI.CreateTd(HtmlUI.Input("mailUid", 20, null, SiteFun.HtmlEncode(old.Basic.MailUserID))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("MailPwd")) + HtmlUI.CreateTd(HtmlUI.PasswordInput("mailPwd", 20, null, SiteFun.HtmlEncode(old.Basic.MailPassword))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());

            sr.Append(HtmlUI.TableStart("onCenter"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(HtmlUI.SubmitButton() + HtmlUI.ResetButton() + HtmlUI.Button("advancedLink", SiteDat.GetLan("BtnAdvanced"))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());

            sr.Append(AdminUI.AdminBoxFinal());
            sr.Append(HtmlUI.FormFinal());
            return sr.ToString();
        }
    }
}
