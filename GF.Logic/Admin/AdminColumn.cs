using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using GF.Core;
using GF.Data;
using GF.Data.Entity;
using GF.Logic.UI;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 分类管理类
    /// </summary>
    public class AdminColumn
    {
        private _DbHelper conn;

        public AdminColumn(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        private void ClearCache()
        {
            SiteDat.RemoveDat(SiteCache.Navigation);
            SiteDat.RemoveDat(SiteCache.Column);
        }

        /// <summary>
        /// 提交分类数据
        /// </summary>
        /// <returns>返回发布分类代码</returns>
        public string Post()
        {
            StringBuilder sr = new StringBuilder();
            int id = SiteFun.ToInt(SiteFun.Query("id"));
            string theme = new SettingData(conn).GetTheme;
            ColumnData cateData = new ColumnData(conn);
            //取得默认值
            ColumnItem old = cateData.GetColumn(id);
            if (old.ID == 0)
            {
                //默认显示
                old.Show = true;
            }
            if (SiteFun.IsPost)
            {
                ColumnItem postVal = new ColumnItem();
                postVal.ID = id;
                postVal.Name = SiteFun.Post("name");
                postVal.Local = SiteFun.Post("local");
                postVal.Target = SiteFun.Post("target");
                postVal.ListTemplate = SiteFun.Post("listTpl");
                postVal.ViewTemplate = SiteFun.Post("viewTpl");
                postVal.PageSize = SiteFun.ToInt(SiteFun.Post("pageSize"));
                postVal.Jump = SiteFun.ToInt(SiteFun.Post("jump")) == 0 ? false : true;
                postVal.JumpUrl = SiteFun.Post("jumpUrl");
                postVal.Intro = SiteFun.Post("intro");
                postVal.Sorting = SiteFun.ToInt(SiteFun.Post("sorting"));
                postVal.Nav = SiteFun.ToInt(SiteFun.Post("nav")) == 0 ? false : true;
                postVal.Show = SiteFun.ToInt(SiteFun.Post("show")) == 0 ? false : true;
                if (string.IsNullOrEmpty(postVal.Name)) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoName"))); }
                else
                {
                    if (!string.IsNullOrEmpty(postVal.Local)) { postVal.Local = postVal.Local.Replace(" ", "-"); }
                    if (!string.IsNullOrEmpty(postVal.Local) && (!SiteFun.IsLocal(postVal.Local) || cateData.ExistsLocal(postVal.Local, postVal.ID))) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoLocalOrExists"))); }
                    else
                    {
                        if (postVal.ID == 0)
                        {
                            cateData.InsertColumn(postVal);
                            sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgInsertDat"), "?act=column&mode=list"));
                        }
                        else
                        {
                            cateData.UpdateColumn(postVal);
                            sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgUpdateDat"), "?act=column&mode=list"));
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
            if (Regex.IsMatch(old.Local, @"^\d+$")) { old.Local = string.Empty; }
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Local")) + HtmlUI.CreateTd(string.Format(SitePath.CategoryLinkFormat, null, HtmlUI.Input("local", 15, null, SiteFun.HtmlEncode(old.Local)))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(HtmlUI.CheckBoxInput(SiteDat.GetLan("Jump"), "jump", 1, old.Jump) + HtmlUI.Input("jumpUrl", 50, null, SiteFun.HtmlEncode(old.JumpUrl))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Intro")) + HtmlUI.CreateTd(HtmlUI.Input("intro", 50, null, SiteFun.HtmlEncode(old.Intro))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Sorting")) + HtmlUI.CreateTd(HtmlUI.Input("sorting", 5, null, old.Sorting)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Target")) + HtmlUI.CreateTd(HtmlUI.Input("target", 10, null, SiteFun.HtmlEncode(old.Target))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("ListTpl")) + HtmlUI.CreateTd(string.Format(SitePath.ThemePathFormat, theme, HtmlUI.Input("listTpl", 15, null, SiteFun.HtmlEncode(old.ListTemplate)))));
            sr.Append(HtmlUI.TrFinal());
            if (!string.IsNullOrEmpty(old.ListTemplate) && !File.Exists(Path.Combine(SiteCfg.Router, string.Format(SitePath.ThemePathFormat, theme, old.ListTemplate))))
            {
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(string.Format(SiteDat.GetLan("MsgFileNotExistsFormat"), old.ListTemplate)));
                sr.Append(HtmlUI.TrFinal());
            }
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("ViewTpl")) + HtmlUI.CreateTd(string.Format(SitePath.ThemePathFormat, theme, HtmlUI.Input("viewTpl", 15, null, SiteFun.HtmlEncode(old.ViewTemplate)))));
            sr.Append(HtmlUI.TrFinal());
            if (!string.IsNullOrEmpty(old.ViewTemplate) && !File.Exists(Path.Combine(SiteCfg.Router, string.Format(SitePath.ThemePathFormat, theme, old.ViewTemplate))))
            {
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(string.Format(SiteDat.GetLan("MsgFileNotExistsFormat"), old.ViewTemplate)));
                sr.Append(HtmlUI.TrFinal());
            }
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("PageSize")) + HtmlUI.CreateTd(HtmlUI.Input("pageSize", 5, null, old.PageSize)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Virtue")) + HtmlUI.CreateTd(HtmlUI.CheckBoxInput(SiteDat.GetLan("Nav"), "nav", 1, old.Nav) + HtmlUI.CheckBoxInput(SiteDat.GetLan("Show"), "show", 1, old.Show)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(HtmlUI.SubmitButton() + HtmlUI.ResetButton()));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(HtmlUI.FormFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            return sr.ToString();
        }

        /// <summary>
        /// 系统文章分类操作
        /// </summary>
        /// <returns>分类列表及操作表单</returns>
        public string List()
        {
            StringBuilder sr = new StringBuilder();
            DataList<ColumnItem> list = new ColumnData(conn).SelectColumn(-1, -1, true);
            if (list.Count > 0)
            {
                ColumnData cData = new ColumnData(conn);
                if (SiteFun.IsPost)
                {
                    //取值
                    ColumnItem postVal = new ColumnItem();
                    postVal.ID = SiteFun.ToInt(SiteFun.Post("id"));
                    postVal.Name = SiteFun.Post("name");
                    postVal.Sorting = SiteFun.ToInt(SiteFun.Post("sorting"));
                    postVal.Show = SiteFun.ToInt(SiteFun.Post("show")) == 0 ? false : true;
                    postVal.Nav = SiteFun.ToInt(SiteFun.Post("nav")) == 0 ? false : true;
                    //处理
                    if (string.IsNullOrEmpty(postVal.Name)) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoName"))); }
                    else
                    {
                        if (postVal.ID > 0)
                        {
                            if (SiteFun.ToInt(SiteFun.Post("del")) == 0) { cData.UpdateColumnSome(postVal.ID, postVal.Name, postVal.Sorting, postVal.Nav, postVal.Show); sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgUpdateDat"))); }
                            else
                            {
                                PostData pData = new PostData(conn);
                                foreach (PostItem tmpPost in pData.SelectPost(postVal.ID, 1, int.MaxValue, null))
                                {
                                    //删除附件
                                    if (!string.IsNullOrEmpty(tmpPost.Attachments))
                                    {
                                        AttachmentData attData = new AttachmentData(conn);
                                        foreach (AttachmentItem delAtt in attData.SelectAttachment(tmpPost.Attachments))
                                        {
                                            File.Delete(Path.Combine(SiteCfg.Router, delAtt.Path));
                                        }
                                        attData.DeleteAttachment(tmpPost.Attachments);
                                    }
                                }
                                cData.DeleteColumn(postVal.ID);
                                sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgDelDat")));
                            }
                        }
                    }
                    ClearCache();
                }
                sr.Append(AdminUI.AdminBoxStart(true));
                sr.Append(HtmlUI.TableStart());
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Name")));
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Article")));
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Sorting")));
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Nav")));
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Show")));
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Delete")));
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Operate")));
                sr.Append(HtmlUI.TrFinal());
                int i = 1;
                foreach (ColumnItem vItem in list)
                {
                    i++;
                    sr.Append(HtmlUI.FormStart());
                    sr.Append(HtmlUI.TrStart(i % 2 == 0 ? " cRow" : null));
                    sr.Append(HtmlUI.CreateTd(HtmlUI.HiddenInput("id", vItem.ID) + HtmlUI.Input("name", 10, null, SiteFun.HtmlEncode(vItem.Name)) + HtmlUI.Link(SiteDat.GetLan("Edit"), string.Format("?act=column&amp;mode=post&amp;id={0}", vItem.ID))));
                    sr.Append(HtmlUI.CreateTd(vItem.PostCount));
                    sr.Append(HtmlUI.CreateTd(HtmlUI.Input("sorting", 5, null, vItem.Sorting)));
                    sr.Append(HtmlUI.CreateTd(HtmlUI.CheckBoxInput("nav", 1, vItem.Nav)));
                    sr.Append(HtmlUI.CreateTd(HtmlUI.CheckBoxInput("show", 1, vItem.Show)));
                    sr.Append(HtmlUI.CreateTd(HtmlUI.CheckBoxInput("del", 1, false)));
                    sr.Append(HtmlUI.CreateTd(HtmlUI.SubmitButton(SiteDat.GetLan("BtnSave"))));
                    sr.Append(HtmlUI.TrFinal());
                    sr.Append(HtmlUI.FormFinal());
                }
                sr.Append(HtmlUI.TableFinal());
                sr.Append(AdminUI.AdminBoxFinal());
            }
            else { sr.Append(Post()); }
            return sr.ToString();
        }
    }
}
