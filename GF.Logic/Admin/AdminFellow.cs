using System.IO;
using System.Text;
using GF.Core;
using GF.Data;
using GF.Data.Entity;
using GF.Logic.UI;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 友情链接管路类
    /// </summary>
    public class AdminFellow
    {
        private _DbHelper conn;

        public AdminFellow(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        private void ClearCache()
        {
            SiteDat.RemoveDat(SiteCache.Fellow);
        }

        /// <summary>
        /// 取得链接列表窗体
        /// </summary>
        /// <returns>返回链接列表代码</returns>
        public string List()
        {

            StringBuilder sr = new StringBuilder();
            int selHome = SiteFun.ToInt(SiteFun.Query("home"));
            FellowData flwData = new FellowData(conn);
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart("onCenter"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("SelMode")));
            sr.Append(HtmlUI.CreateTd(HtmlUI.Link(SiteDat.GetLan("IdxLink"), "?act=fellow&amp;mode=list&amp;home=1") + ", " + HtmlUI.Link(SiteDat.GetLan("NormalLink"), "?act=fellow&amp;mode=list")));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            if (SiteFun.IsPost)
            {
                int id = SiteFun.ToInt(SiteFun.Post("id"));
                bool del = SiteFun.ToInt(SiteFun.Post("del")) == 0 ? false : true;
                if (del)
                {
                    FellowItem delLink = new FellowData(conn).GetFellow(id);
                    if (delLink.ID > 0)
                    {
                        string deleteLogoFile = Path.Combine(SiteCfg.Router, string.Format("Attach/FellowLogo/{0}.gif", delLink.ID));
                        if (File.Exists(deleteLogoFile)) { File.Delete(deleteLogoFile); }
                        flwData.DeleteFellow(id);
                        sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgDelDat")));
                    }
                }
                else
                {
                    string name = SiteFun.Post("name");
                    string url = SiteFun.Post("url");
                    bool home = SiteFun.ToInt(SiteFun.Post("home")) == 0 ? false : true;
                    bool show = SiteFun.ToInt(SiteFun.Post("show")) == 0 ? false : true;
                    int sorting = SiteFun.ToInt(SiteFun.Post("sorting"));
                    if (string.IsNullOrEmpty(name)) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoName"))); }
                    else
                    {
                        if (string.IsNullOrEmpty(url) || flwData.ExistsFellowUrl(url, id)) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoUrlOrExists"))); }
                        else
                        {
                            flwData.UpdateFellowSome(id, name, url, home, show, sorting);
                            sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgUpdateDat")));
                        }
                    }
                }
                ClearCache();
            }
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Name")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("URL")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Sorting")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Delete")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("IdxLink")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Show")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Operate")));
            sr.Append(HtmlUI.TrFinal());
            //下面三行是分页设置
            int page = SiteFun.ToInt(SiteFun.Query("page"));
            if (page < 1) { page = 1; }
            int pageSize = 20;
            DataList<FellowItem> list = flwData.SelectFellow(page, pageSize, selHome == 0 ? false : true, true);
            int i = 1;
            foreach (FellowItem vItem in list)
            {
                i++;
                sr.Append(HtmlUI.FormStart());
                sr.Append(HtmlUI.TrStart(i % 2 == 0 ? " cRow" : null));
                sr.Append(HtmlUI.CreateTd(HtmlUI.Input("name", 15, null, SiteFun.HtmlEncode(vItem.Name)) + HtmlUI.Link(SiteDat.GetLan("Edit"), string.Format("?act=fellow&amp;mode=post&amp;id={0}", vItem.ID))));
                sr.Append(HtmlUI.CreateTd(HtmlUI.Input("url", 30, null, SiteFun.HtmlEncode(vItem.URL))));
                sr.Append(HtmlUI.CreateTd(HtmlUI.Input("sorting", 5, null, vItem.Sorting)));
                sr.Append(HtmlUI.CreateTd(HtmlUI.CheckBoxInput("del", 1, false)));
                sr.Append(HtmlUI.CreateTd(HtmlUI.CheckBoxInput("home", 1, vItem.Home)));
                sr.Append(HtmlUI.CreateTd(HtmlUI.CheckBoxInput("show", 1, vItem.Show)));
                sr.Append(HtmlUI.CreateTd(HtmlUI.HiddenInput("id", vItem.ID) + HtmlUI.SubmitButton(SiteDat.GetLan("BtnSave"))));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.FormFinal());
            }
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTd(new SitePages().Make(list.Number, page, pageSize, "?act=fellow&amp;mode=list&amp;home=" + selHome + "&amp;page={0}"), 7, null));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            return sr.ToString();
        }

        /// <summary>
        /// 提交链接数据
        /// </summary>
        /// <returns>返回发布链接代码</returns>
        public string Post()
        {
            StringBuilder sr = new StringBuilder();
            int id = SiteFun.ToInt(SiteFun.Query("id"));
            FellowData flwData = new FellowData(conn);
            //取得默认值
            FellowItem old = flwData.GetFellow(id);
            if (SiteFun.IsPost)
            {
                FellowItem postVal = new FellowItem();
                postVal.ID = id;
                postVal.Name = SiteFun.Post("name");
                postVal.URL = SiteFun.Post("url");
                postVal.Logo = SiteFun.Post("logo");
                postVal.Explain = SiteFun.Post("explain");
                postVal.Style = SiteFun.Post("style");
                postVal.Sorting = SiteFun.ToInt(SiteFun.Post("sorting"));
                postVal.Home = SiteFun.ToInt(SiteFun.Post("home")) == 0 ? false : true;
                postVal.Show = SiteFun.ToInt(SiteFun.Post("show")) == 0 ? false : true;
                if (string.IsNullOrEmpty(postVal.URL) || flwData.ExistsFellowUrl(postVal.URL, postVal.ID)) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoUrlOrExists"))); }
                else
                {
                    if (string.IsNullOrEmpty(postVal.Name)) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoName"))); }
                    else
                    {
                        if (postVal.ID == 0)
                        {
                            postVal.ID = flwData.InsertFellow(postVal);
                            sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgInsertDat"), string.Format("?act=fellow&mode=list&home={0}", postVal.Home ? 1 : 0)));
                        }
                        else
                        {
                            postVal.ID = flwData.UpdateFellow(postVal);
                            sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgUpdateDat"), string.Format("?act=fellow&mode=list&home={0}", postVal.Home ? 1 : 0)));
                        }
                        if (!string.IsNullOrEmpty(postVal.Logo) && SiteFun.ToInt(SiteFun.Post("down")) == 1)
                        {
                            string saveLogoDir = Path.Combine(SiteCfg.Router, "Attach/FellowLogo");
                            if (!Directory.Exists(saveLogoDir)) { Directory.CreateDirectory(saveLogoDir); }
                            //LOGO的后缀还需要处理
                            string saveLogoFile = Path.Combine(saveLogoDir, postVal.ID + ".gif");
                            if (new SiteIO().RemoteDownload(postVal.Logo, saveLogoFile))
                            {
                                postVal.Logo = saveLogoFile.Replace(SiteCfg.Router, SiteCfg.Path).Replace("\\", "/");
                                flwData.UpdateFellow(postVal);
                            }
                        }
                    }
                }
                ClearCache();
                old = postVal;
            }
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.FormStart());
            sr.Append(HtmlUI.TableStart("onCenter"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Name")) + HtmlUI.CreateTd(HtmlUI.Input("name", 20, null, SiteFun.HtmlEncode(old.Name))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("URL")) + HtmlUI.CreateTd(HtmlUI.Input("url", 50, null, SiteFun.HtmlEncode(old.URL))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Logo")) + HtmlUI.CreateTd(HtmlUI.Input("logo", 50, null, SiteFun.HtmlEncode(old.Logo)) + HtmlUI.CheckBoxInput(SiteDat.GetLan("AreYouDown"), "down", 1, false)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Intro")) + HtmlUI.CreateTd(HtmlUI.Input("explain", 50, null, SiteFun.HtmlEncode(old.Explain))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Style")) + HtmlUI.CreateTd(HtmlUI.Input("style", 50, null, SiteFun.HtmlEncode(old.Style))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Sorting")) + HtmlUI.CreateTd(HtmlUI.Input("sorting", 5, null, old.Sorting)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Virtue")) + HtmlUI.CreateTd(HtmlUI.CheckBoxInput(SiteDat.GetLan("IdxLink"), "home", 1, old.Home) + HtmlUI.CheckBoxInput(SiteDat.GetLan("Show"), "show", 1, old.Show)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(HtmlUI.SubmitButton() + HtmlUI.ResetButton()));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(HtmlUI.FormFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            return sr.ToString();
        }
    }
}
