using System;
using System.Text;
using GF.Core;
using GF.Data;
using GF.Data.Entity;
using GF.Logic.UI;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 评论管路类
    /// </summary>
    public class AdminComment
    {
        private _DbHelper conn;

        public AdminComment(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        private void ClearCache()
        {
            SiteDat.RemoveDat(SiteCache.NewComment);
            SiteDat.ClearDat("Comments-");
        }

        /// <summary>
        /// 取得页面列表窗体
        /// </summary>
        /// <returns>返回页面列表代码</returns>
        public string List(int pageSize)
        {
            StringBuilder sr = new StringBuilder();
            CommentData cmtData = new CommentData(conn);
            if (SiteFun.IsPost)
            {
                int id = SiteFun.ToInt(SiteFun.Post("id"));
                bool verify = SiteFun.ToInt(SiteFun.Post("verify")) == 0 ? false : true;
                bool del = SiteFun.ToInt(SiteFun.Post("del")) == 0 ? false : true;
                if (del)
                {
                    cmtData.DeleteComment(id);
                    sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgDelDat")));
                }
                else
                {
                    cmtData.UpdateCommentVerify(id, verify);
                    sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgUpdateDat")));
                }
                ClearCache();
            }
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Author")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Content")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Article")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("DateTime")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Verify")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Delete")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Operate")));
            sr.Append(HtmlUI.TrFinal());
            //下面三行是分页设置
            int page = SiteFun.ToInt(SiteFun.Query("page"));
            if (page < 1) { page = 1; }
            DataList<CommentItem> list = cmtData.SelectComment(0, page, pageSize, true);
            int i = 1;
            foreach (CommentItem vItem in list)
            {
                PostItem postItem = new PostData(conn).GetPost(vItem.PostID);
                i++;
                sr.Append(HtmlUI.FormStart());
                sr.Append(HtmlUI.TrStart(i % 2 == 0 ? " cRow" : null));
                sr.Append(HtmlUI.CreateTd(SiteFun.HtmlEncode(SiteFun.StrCut(vItem.Author, 10))));
                sr.Append(HtmlUI.CreateTd(HtmlUI.Link(SiteFun.HtmlEncode(SiteFun.StrCut(vItem.Content, 50)), string.Format("?act=comment&amp;mode=post&amp;id={0}", vItem.ID))));
                sr.Append(HtmlUI.CreateTd(HtmlUI.Link(SiteFun.HtmlEncode(SiteFun.StrCut(postItem.Title, 30)), string.Format("?act=post&amp;mode=post&amp;m=A&amp;id={0}", postItem.ID))));
                sr.Append(HtmlUI.CreateTd(vItem.Publish));
                sr.Append(HtmlUI.CreateTd(HtmlUI.CheckBoxInput("verify", 1, vItem.Verify)));
                sr.Append(HtmlUI.CreateTd(HtmlUI.CheckBoxInput("del", 1, false)));
                sr.Append(HtmlUI.CreateTd(HtmlUI.HiddenInput("id", vItem.ID) + HtmlUI.SubmitButton(SiteDat.GetLan("BtnSave"))));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.FormFinal());
            }
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTd(new SitePages().Make(list.Number, page, pageSize, "?act=comment&amp;mode=list&amp;page={0}"), 7, null));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            return sr.ToString();
        }

        /// <summary>
        /// 提交页面窗体
        /// </summary>
        /// <returns>返回发布页面控件代码</returns>
        public string Post()
        {
            StringBuilder sr = new StringBuilder();
            int id = SiteFun.ToInt(SiteFun.Query("id"));
            CommentData cmtData = new CommentData(conn);
            CommentItem old = cmtData.GetComment(id);
            PostItem postItem = new PostData(conn).GetPost(old.PostID);
            if (SiteFun.IsPost)
            {
                bool sendMail = false;
                if (SiteFun.ToInt(SiteFun.Post("sendMail")) != 0) { sendMail = true; }
                CommentItem postVal = new CommentItem();
                postVal.ID = id;
                postVal.PostID = postItem.ID;
                postVal.Author = old.Author;
                postVal.Mail = old.Mail;
                postVal.URL = old.URL;
                postVal.Title = old.Title;
                postVal.Content = old.Content;
                postVal.Reply = SiteFun.Post("reply");
                postVal.Trackback = SiteFun.ToInt(SiteFun.Post("isTb")) == 0 ? false : true;
                postVal.Publish = old.Publish;
                postVal.Verify = SiteFun.ToInt(SiteFun.Post("verify")) == 0 ? false : true;
                if (string.IsNullOrEmpty(postVal.Author)) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoAuthor"))); }
                else
                {
                    if (string.IsNullOrEmpty(postVal.Mail)) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoMail"))); }
                    else
                    {
                        if (string.IsNullOrEmpty(postVal.Content)) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoContent"))); }
                        else
                        {
                            try
                            {
                                if (sendMail)
                                {
                                    SettingBasicItem basic = new SettingData(conn).GetSetting().Basic;
                                    SiteMail mail = new SiteMail();
                                    mail.To = old.Mail;
                                    mail.Subject = string.Format(SiteDat.GetLan("MailTitle"), basic.Name);
                                    mail.From = basic.MailFrom;
                                    mail.Body = string.Format("<p><b>{0}: </b>{1}</p><p><b>{2}: </b>{3}</p><p><b>{4}: </b>{5}</p>", SiteDat.GetLan("Content"), SiteFun.ClearHtml(old.Content), SiteDat.GetLan("Reply"), SiteFun.ClearHtml(postVal.Reply), SiteDat.GetLan("URL"), string.Format(SitePath.ArticleLinkFormat, basic.URL, postItem.Local));
                                    mail.Host = basic.MailHost;
                                    mail.Port = basic.MailPort;
                                    mail.UserName = basic.MailUserID;
                                    mail.Password = basic.MailPassword;
                                    mail.Send();
                                }
                                sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgSaveSucc"), "?act=comment&mode=list"));
                            }
                            catch (Exception err) { sr.Append(AdminUI.ErrorBox(err.Message)); }
                            old = cmtData.GetComment(cmtData.UpdateComment(postVal));
                        }
                    }
                }
                ClearCache();
            }
            //取得默认值
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.FormStart());
            sr.Append(HtmlUI.TableStart("onCenter"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Author")) + HtmlUI.CreateTd(SiteFun.HtmlEncode(old.Author)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Mail")) + HtmlUI.CreateTd(SiteFun.HtmlEncode(old.Mail)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Website")) + HtmlUI.CreateTd(HtmlUI.Link(SiteFun.HtmlEncode(old.URL), old.URL, null, true)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Article")) + HtmlUI.CreateTd(SiteFun.HtmlEncode(postItem.Title)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Content")) + HtmlUI.CreateTd(SiteFun.HtmlEncode(old.Content)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Reply")) + HtmlUI.CreateTd(AdminUI.Editor("reply", SiteFun.HtmlEncode(old.Reply))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("DateTime")) + HtmlUI.CreateTd(old.Publish));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Virtue")) + HtmlUI.CreateTd(HtmlUI.CheckBoxInput(SiteDat.GetLan("Trackback"), "isTb", 1, old.Trackback) + HtmlUI.CheckBoxInput(SiteDat.GetLan("Verify"), "verify", 1, old.Verify) + HtmlUI.CheckBoxInput(SiteDat.GetLan("MailNotice"), "sendMail", 1, false)));
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
