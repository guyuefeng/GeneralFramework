using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using GF.Core;
using GF.Data;
using GF.Data.Entity;
using GF.Logic.UI;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 文章管理类
    /// </summary>
    public class AdminPost
    {
        private _DbHelper conn;
        private _AdminCookie _ac = new _AdminCookie();

        public AdminPost(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        private void ClearCache()
        {
            /*SiteCache cache = new SiteCache();
            SiteDat.RemoveDat(cache.FineArticle);
            SiteDat.RemoveDat(cache.HotArticle);
            SiteDat.RemoveDat(cache.RandomArticle);*/
            SiteDat.ClearDat();
        }

        /// <summary>
        /// 提交文章窗体
        /// </summary>
        /// <param name="mode">模式：A-文章，P-单页</param>
        /// <returns>返回发布文章控件代码</returns>
        public string Post(string mode)
        {
            StringBuilder sr = new StringBuilder();
            if (mode == "A" || mode == "P")
            {
                bool isArticle = mode == "A";
                SettingItem setting = new SettingData(conn).GetSetting();
                string theme = new SettingData(conn).GetTheme;
                string tmpFilePath = Path.Combine(SiteCfg.Router, "Common/Temp/post_cache.tmp");
                int id = SiteFun.ToInt(SiteFun.Query("id"));
                PostData artData = new PostData(conn);
                ColumnData cData = new ColumnData(conn);
                TagData tagData = new TagData(conn);
                //取得默认值
                string[] myUserInfo = _ac.Get();
                UserItem myUser = new UserData(conn).CheckUser(myUserInfo[0], SiteFun.Encryption(myUserInfo[1]), true);
                PostItem old = artData.GetPost(id);
                ColumnItem oldColumn = new ColumnData(conn).GetColumn(old.ColumnID);
                if (old.ID == 0)
                {
                    //默认显示、可评论并自动审核，可发通告
                    old.Show = true;
                    old.SwitchComment = true;
                    old.AutoVerifyComment = true;
                    old.SwitchTrackback = true;
                }
                if (string.IsNullOrEmpty(old.Author)) { old.Author = myUser.Name; }
                if (SiteFun.IsPost)
                {
                    //处理文章数据
                    PostItem postVal = new PostItem();
                    string content = SiteFun.HtmlMatch(SiteFun.Post("content"));
                    string explain = content;
                    if (!string.IsNullOrEmpty(explain))
                    {
                        explain = Regex.Replace(explain, SiteCfg.PageBreakRegular, SiteCfg.PageBreakSymbol, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                        if (explain.IndexOf(SiteCfg.PageBreakSymbol) > 0)
                        {
                            explain = explain.Substring(0, explain.IndexOf(SiteCfg.PageBreakSymbol));
                            explain = SiteFun.HtmlMatch(explain);
                        }
                        explain = explain.Replace(SiteCfg.PageBreakSymbol, string.Empty);
                    }
                    postVal.ID = id;
                    postVal.ColumnID = SiteFun.ToInt(SiteFun.Post("cid"));
                    postVal.Title = SiteFun.Post("title");
                    postVal.Tags = SiteFun.Post("tags");
                    postVal.Local = SiteFun.Post("local");
                    postVal.Explain = explain;
                    postVal.Content = content;
                    postVal.Author = SiteFun.Post("author");
                    postVal.Publish = SiteFun.ToDate(SiteFun.Post("publish"));
                    postVal.Password = SiteFun.Post("pwd");
                    postVal.Vote = SiteFun.ToInt(SiteFun.Post("vote"));
                    postVal.Reader = SiteFun.ToInt(SiteFun.Post("reader"));
                    postVal.Fine = SiteFun.ToInt(SiteFun.Post("fine")) == 0 ? false : true;
                    postVal.Show = SiteFun.ToInt(SiteFun.Post("show")) == 0 ? false : true;
                    postVal.PostCount = old.PostCount;
                    postVal.SwitchComment = SiteFun.ToInt(SiteFun.Post("switchCmt")) == 0 ? false : true;
                    postVal.SwitchTrackback = SiteFun.ToInt(SiteFun.Post("switchTb")) == 0 ? false : true;
                    postVal.AutoVerifyComment = SiteFun.ToInt(SiteFun.Post("avcmt")) == 0 ? false : true;
                    postVal.AutoVerifyTrackback = SiteFun.ToInt(SiteFun.Post("avtb")) == 0 ? false : true;
                    postVal.Attachments = old.Attachments;
                    //分类
                    ColumnItem postColumn = new ColumnData(conn).GetColumn(postVal.ColumnID);
                    if (string.IsNullOrEmpty(postVal.Title)) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoTitle"))); }
                    else
                    {
                        if (!string.IsNullOrEmpty(postVal.Local)) { postVal.Local = postVal.Local.Replace(" ", "-"); }
                        if (!string.IsNullOrEmpty(postVal.Local) && (!SiteFun.IsLocal(postVal.Local) || artData.ExistsLocal(postVal.Local, postVal.ID))) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoLocalOrExists"))); }
                        else
                        {
                            //处理上传图片的内容循环替换
                            if (!string.IsNullOrEmpty(postVal.Content))
                            {
                                string[] tmpFilesHandle = { postVal.Content, postVal.Explain };
                                string atts = new _AdminUpload(conn).SaveAs(HttpContext.Current.Request.Files, ref tmpFilesHandle[0], ref tmpFilesHandle[1], SiteFun.ToInt(SiteFun.Post("ws")), Path.Combine(SiteCfg.Router, setting.Parameter.WatermarkPath));
                                postVal.Content = tmpFilesHandle[0];
                                postVal.Explain = tmpFilesHandle[1];
                                if (!string.IsNullOrEmpty(atts))
                                {
                                    if (string.IsNullOrEmpty(postVal.Attachments)) { postVal.Attachments = atts; }
                                    else { postVal.Attachments += "," + atts; }
                                }
                            }
                            //保存或新增文章数据处理
                            if (postVal.ID == 0)
                            {
                                artData.InsertPost(postVal, mode);
                                //标签处理
                                tagData.InsertTagList(postVal.Tags);
                                //分类统计
                                //cData.CountCategoryPost(postVal.Category.ID);
                                sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgArtInsertSucc"), "?act=post&mode=list&M=" + mode));
                            }
                            else
                            {
                                artData.UpdatePost(postVal);
                                //标签处理
                                tagData.InsertTagList(postVal.Tags);
                                //类统计
                                cData.CountColumnPost(oldColumn.ID);
                                cData.CountColumnPost(postColumn.ID);
                                //更新修改后的显示
                                sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgArtUpdateSucc"), "?act=post&mode=list&M=" + mode));
                            }
                            try { File.Delete(tmpFilePath); }
                            catch { }
                        }
                    }
                    //处理引用通告
                    TrackbackLogData tbld = new TrackbackLogData(conn);
                    SettingBasicItem sbi = setting.Basic;
                    string tbUrls = SiteFun.Post("tbUrls");
                    if (!string.IsNullOrEmpty(tbUrls))
                    {
                        foreach (string vItem in tbUrls.Split('\n'))
                        {
                            if (!string.IsNullOrEmpty(vItem))
                            {
                                string[] tbState = new SiteTrackback().SendTrackback(vItem, string.Format(SitePath.ArticleLinkFormat, sbi.URL + SiteCfg.Path, postVal.Local), sbi.Name, postVal.Title, SiteFun.ClearHtml(postVal.Explain)).Split('|');
                                TrackbackLogItem tbVal = new TrackbackLogItem();
                                tbVal.Error = SiteFun.ToInt(tbState[0]) == 0 ? false : true;
                                tbVal.Message = tbState[1];
                                tbVal.URL = vItem;
                                tbld.InsertTrackbackLog(tbVal);
                            }
                        }
                    }
                    ClearCache();
                    old = postVal;
                }
                //设置分类数据
                DataList<ColumnItem> cateList = cData.SelectColumn(-1, 0, true);
                ArrayList values = new ArrayList();
                ArrayList captions = new ArrayList();
                for (int i = 0; i < cateList.Count; i++)
                {
                    values.Add(cateList[i].ID);
                    captions.Add(cateList[i].Name);
                }
                sr.Append(AdminUI.AdminBoxStart(true));
                sr.Append(HtmlUI.FormStart(null, true));

                sr.Append(HtmlUI.TableStart("onCenter advancedBox"));
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Title")) + HtmlUI.CreateTd(HtmlUI.Input("title", 50, null, SiteFun.HtmlEncode(old.Title)) + (isArticle ? HtmlUI.CreateSelect("cid", values, captions, oldColumn.ID) : string.Empty)));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.TrStart());
                if (Regex.IsMatch(old.Local, @"^\d+$")) { old.Local = string.Empty; }
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Local")) + HtmlUI.CreateTd(string.Format((isArticle ? SitePath.ArticleLinkFormat : SitePath.PageLinkFormat), null, HtmlUI.Input("local", 30, null, SiteFun.HtmlEncode(old.Local)))));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Content")) + HtmlUI.CreateTd(AdminUI.Editor("content", SiteFun.HtmlEncode(old.Content))));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd((File.Exists(tmpFilePath) ? "<a id=\"reInsertContent\">" + SiteDat.GetLan("MsgClickArtReload") + "</a>" : string.Empty) + string.Format(SiteDat.GetLan("MsgArtAutoSaveState"), "<span id=\"autoSaveCountdown\">--</span>", "<a id=\"autoSaveButton\">--</a>", "<span id=\"autoSaveIs\"></span>")));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Attach") + ", <a id=\"addFilesA\">" + SiteDat.GetLan("Add") + "</a>") + HtmlUI.CreateTd(string.Format("<span id=\"filesMainBox\">{0}</span><a id=\"filesMainInsertA\">{1}</a><div id=\"filesAttBox\"></div>", HtmlUI.FileInput(null, "files"), SiteDat.GetLan("Insert"))));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.TableFinal());

                sr.Append(HtmlUI.TableStart("onCenter hidden advancedBox"));
                string attsHtml = string.Empty;
                DataList<AttachmentItem> attachments = new AttachmentData(conn).SelectAttachment(old.Attachments);
                foreach (AttachmentItem att in attachments)
                {
                    attsHtml += HtmlUI.Link(att.Name, att.Path, att.Type, true) + string.Format(", {0}, {1}, {2}<br/>", att.Publish, att.Size, att.Type);
                }
                if (attachments.Count > 0)
                {
                    sr.Append(HtmlUI.TrStart());
                    sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Attach")) + HtmlUI.CreateTd(attsHtml));
                    sr.Append(HtmlUI.TrFinal());
                }
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Tag")) + HtmlUI.CreateTd(HtmlUI.Input("tags", "tags", 50, null, SiteFun.HtmlEncode(old.Tags))));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(HtmlUI.Button("getInputTags", SiteDat.GetLan("BtnTagsNow")) + "<div id=\"getInputTagsDsp\"></div>"));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Author")) + HtmlUI.CreateTd(HtmlUI.Input("author", 10, null, SiteFun.HtmlEncode(old.Author))));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Time")) + HtmlUI.CreateTd(HtmlUI.Input("timeInput", "publish", 20, null, old.Publish) + HtmlUI.Button("updateTime", SiteDat.GetLan("BtnUpdateTime"))));
                sr.Append(HtmlUI.TrFinal());
                if (isArticle)
                {
                    sr.Append(HtmlUI.TrStart());
                    sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Pwd")) + HtmlUI.CreateTd(HtmlUI.Input("pwd", 30, null, old.Password)));
                    sr.Append(HtmlUI.TrFinal());
                }
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("VoteNum")) + HtmlUI.CreateTd(HtmlUI.Input("vote", 5, null, old.Vote)));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Reader")) + HtmlUI.CreateTd(HtmlUI.Input("reader", 5, null, old.Reader)));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Virtue")) + HtmlUI.CreateTd((isArticle ? HtmlUI.CheckBoxInput(SiteDat.GetLan("Top"), "fine", 1, old.Fine) : string.Empty) + HtmlUI.CheckBoxInput(SiteDat.GetLan("Show"), "show", 1, old.Show)));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.TrStart());
                ArrayList wmValues = new ArrayList();
                ArrayList caps = new ArrayList();
                for (int wmI = 0; wmI <= 9; wmI++) { wmValues.Add(wmI); }
                caps.Add("--" + SiteDat.GetLan("WmPosition") + "--");
                caps.Add(SiteDat.GetLan("PTopLeft"));
                caps.Add(SiteDat.GetLan("PTopCen"));
                caps.Add(SiteDat.GetLan("PTopRight"));
                caps.Add(SiteDat.GetLan("PMdlLeft"));
                caps.Add(SiteDat.GetLan("PMdlCen"));
                caps.Add(SiteDat.GetLan("PMdlRight"));
                caps.Add(SiteDat.GetLan("PBtmLeft"));
                caps.Add(SiteDat.GetLan("PBtmCen"));
                caps.Add(SiteDat.GetLan("PBtmRight"));
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Switch")) + HtmlUI.CreateTd(HtmlUI.CheckBoxInput(SiteDat.GetLan("InsertCmt"), "switchCmt", 1, old.SwitchComment) + HtmlUI.CheckBoxInput(SiteDat.GetLan("InsertTb"), "switchTb", 1, old.SwitchTrackback) + HtmlUI.CreateSelect("ws", wmValues, caps, setting.Parameter.WatermarkSeat)));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("AutoVerify")) + HtmlUI.CreateTd(HtmlUI.CheckBoxInput(SiteDat.GetLan("VerifyCmt"), "avcmt", 1, old.AutoVerifyComment) + HtmlUI.CheckBoxInput(SiteDat.GetLan("VerifyTb"), "avtb", 1, old.AutoVerifyTrackback)));
                sr.Append(HtmlUI.TrFinal());
                if (isArticle)
                {
                    sr.Append(HtmlUI.TrStart());
                    sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Trackback")) + HtmlUI.CreateTd(HtmlUI.Textarea("tbUrls", 5, 80, null)));
                    sr.Append(HtmlUI.TrFinal());
                }
                sr.Append(HtmlUI.TableFinal());

                sr.Append(HtmlUI.TableStart("onCenter"));
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(HtmlUI.SubmitButton() + HtmlUI.ResetButton() + HtmlUI.Button("advancedLink", SiteDat.GetLan("BtnAdvanced"))));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.TableFinal());

                sr.Append(HtmlUI.FormFinal());
                sr.Append(AdminUI.AdminBoxFinal());
            }
            return sr.ToString();
        }

        /// <summary>
        /// 取得文章列表窗体
        /// </summary>
        /// <param name="mode">模式：A-文章，P-单页</param>
        /// <returns>返回文章列表代码</returns>
        public string List(string mode)
        {
            StringBuilder sr = new StringBuilder();
            int cid = SiteFun.ToInt(SiteFun.Query("cid"));
            string key = SiteFun.Query("key");
            //下面三行是分页设置
            int page = SiteFun.ToInt(SiteFun.Query("page"));
            if (page < 1) { page = 1; }
            int pageSize = 20;
            PostData pData = new PostData(conn);
            DataList<PostItem> list = pData.SelectPost(cid, null, key, page, pageSize, 0, mode, true);
            if (list.Count > 0)
            {
                if (mode == "A" || mode == "P")
                {
                    bool isArticle = mode == "A";
                    if (SiteFun.IsPost)
                    {
                        int id = SiteFun.ToInt(SiteFun.Post("id"));
                        bool del = SiteFun.ToInt(SiteFun.Post("del")) == 0 ? false : true;
                        if (del)
                        {
                            PostItem tmpPost = pData.GetPost(id);
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
                            pData.DeletePost(id);
                            sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgArtDelSucc")));
                        }
                        else
                        {
                            bool fine = SiteFun.ToInt(SiteFun.Post("fine")) == 0 ? false : true;
                            bool show = SiteFun.ToInt(SiteFun.Post("show")) == 0 ? false : true;
                            pData.UpdatePostFineAndShow(id, fine, show);
                            sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgUpdateDat")));
                        }
                        ClearCache();
                    }
                    if (isArticle)
                    {
                        //设置分类数据
                        DataList<ColumnItem> cateList = new ColumnData(conn).SelectColumn(-1, 0, false);
                        ArrayList values = new ArrayList();
                        ArrayList captions = new ArrayList();
                        values.Add(0);
                        captions.Add("--" + SiteDat.GetLan("Category") + "--");
                        for (int j = 0; j < cateList.Count; j++)
                        {
                            values.Add(cateList[j].ID);
                            captions.Add(cateList[j].Name);
                        }
                        //搜索
                        sr.Append(AdminUI.AdminBoxStart(true));
                        sr.Append(HtmlUI.FormStart(false, null, false));
                        sr.Append(HtmlUI.TableStart("onCenter"));
                        sr.Append(HtmlUI.TrStart());
                        sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Search") + HtmlUI.HiddenInput("act", "post") + HtmlUI.HiddenInput("mode", "list") + HtmlUI.HiddenInput("m", mode)));
                        sr.Append(HtmlUI.CreateTd(HtmlUI.CreateSelect("cid", values, captions, cid) + HtmlUI.Input("key", 30, null, SiteFun.HtmlEncode(key)) + HtmlUI.SubmitButton(SiteDat.GetLan("Search"))));
                        sr.Append(HtmlUI.TrFinal());
                        sr.Append(HtmlUI.TableFinal());
                        sr.Append(HtmlUI.FormFinal());
                        sr.Append(AdminUI.AdminBoxFinal());
                    }

                    //正常列表
                    sr.Append(AdminUI.AdminBoxStart(true));
                    sr.Append(HtmlUI.TableStart());
                    sr.Append(HtmlUI.TrStart());
                    sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Title")));
                    sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Local")));
                    if (isArticle)
                    {
                        sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Category")));
                        sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Top")));
                    }
                    sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Show")));
                    sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Delete")));
                    sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Operate")));
                    sr.Append(HtmlUI.TrFinal());
                    int i = 1;
                    foreach (PostItem vItem in list)
                    {
                        i++;
                        sr.Append(HtmlUI.FormStart());
                        sr.Append(HtmlUI.TrStart(i % 2 == 0 ? " cRow" : null));
                        sr.Append(HtmlUI.CreateTd(HtmlUI.Link(SiteFun.HtmlEncode(SiteFun.StrCut(vItem.Title, 50)), string.Format("?act=post&amp;mode=post&amp;m={1}&amp;id={0}", vItem.ID, mode))));
                        sr.Append(HtmlUI.CreateTd(SiteFun.HtmlEncode(SiteFun.StrCut(vItem.Local, 30))));
                        if (isArticle)
                        {
                            sr.Append(HtmlUI.CreateTd(SiteFun.HtmlEncode(new ColumnData(conn).GetColumn(vItem.ColumnID).Name)));
                            sr.Append(HtmlUI.CreateTd(HtmlUI.CheckBoxInput("fine", 1, vItem.Fine)));
                        }
                        sr.Append(HtmlUI.CreateTd(HtmlUI.CheckBoxInput("show", 1, vItem.Show)));
                        sr.Append(HtmlUI.CreateTd(HtmlUI.CheckBoxInput("del", 1, false)));
                        sr.Append(HtmlUI.CreateTd(HtmlUI.HiddenInput("id", vItem.ID) + HtmlUI.SubmitButton(SiteDat.GetLan("BtnSave"))));
                        sr.Append(HtmlUI.TrFinal());
                        sr.Append(HtmlUI.FormFinal());
                    }
                    sr.Append(HtmlUI.TrStart());
                    sr.Append(HtmlUI.CreateTd(new SitePages().Make(list.Number, page, pageSize, "?act=post&amp;mode=list&amp;cid=" + cid + "&amp;key=" + SiteFun.UrlEncode(key) + "&amp;m=" + mode + "&amp;page={0}"), (isArticle ? 7 : 5), null));
                    sr.Append(HtmlUI.TrFinal());
                    sr.Append(HtmlUI.TableFinal());
                    sr.Append(AdminUI.AdminBoxFinal());
                }
            }
            else { sr.Append(Post(mode)); }
            return sr.ToString();
        }
    }
}
