using System.Text;
using GF.Core;
using GF.Logic.UI;
using GF.Data;
using GF.Data.Entity;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 用户管理类
    /// </summary>
    public class AdminUser
    {
        private _AdminCookie _ac = new _AdminCookie();
        private _DbHelper conn;

        public AdminUser(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 取得用户列表窗体
        /// </summary>
        /// <returns>返回用户列表代码</returns>
        public string List()
        {
            StringBuilder sr = new StringBuilder();
            UserData usrData = new UserData(conn);
            string myUserId = _ac.Get()[0];
            if (SiteFun.IsPost)
            {
                int id = SiteFun.ToInt(SiteFun.Post("id"));
                if (SiteFun.ToInt(SiteFun.Post("delete")) == 0)
                {
                    bool locked = SiteFun.ToInt(SiteFun.Post("locked")) == 0 ? false : true;
                    usrData.UpdateUserLocked(id, locked);
                }
                else { usrData.DeleteUser(id); }
                sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgUpdateDat")));
            }
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("UserId")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("LastIp")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("LastDt")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Locked")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Delete")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Operate")));
            sr.Append(HtmlUI.TrFinal());
            //下面三行是分页设置
            int page = SiteFun.ToInt(SiteFun.Query("page"));
            if (page < 1) { page = 1; }
            int pageSize = 20;
            DataList<UserItem> list = usrData.SelectUser(page, pageSize);
            int i = 1;
            foreach (UserItem vItem in list)
            {
                i++;
                sr.Append(HtmlUI.FormStart());
                sr.Append(HtmlUI.TrStart(i % 2 == 0 ? " cRow" : null));
                sr.Append(HtmlUI.CreateTd(HtmlUI.Link(SiteFun.HtmlEncode(vItem.UserID), string.Format("?act=user&amp;mode=post&amp;id={0}", vItem.ID))));
                sr.Append(HtmlUI.CreateTd(SiteFun.HtmlEncode(vItem.LastIP)));
                sr.Append(HtmlUI.CreateTd(vItem.LastTime));
                sr.Append(HtmlUI.CreateTd(myUserId.ToUpper() == vItem.UserID.ToUpper() ? "--" : HtmlUI.CheckBoxInput("locked", 1, vItem.Locked)));
                sr.Append(HtmlUI.CreateTd(myUserId.ToUpper() == vItem.UserID.ToUpper() ? "--" : HtmlUI.CheckBoxInput("delete", 1, false)));
                sr.Append(HtmlUI.CreateTd(HtmlUI.HiddenInput("id", vItem.ID) + HtmlUI.SubmitButton(SiteDat.GetLan("BtnSave"))));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.FormFinal());
            }
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTd(new SitePages().Make(list.Number, page, pageSize, "?act=user&amp;mode=list&amp;page={0}"), 6, null));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            return sr.ToString();
        }

        /// <summary>
        /// 提交用户数据
        /// </summary>
        /// <returns>返回发布用户代码</returns>
        public string Post()
        {
            StringBuilder sr = new StringBuilder();
            int id = SiteFun.ToInt(SiteFun.Query("id"));
            UserData usrData = new UserData(conn);
            //取得默认值
            UserItem old = usrData.GetUser(id);
            if (SiteFun.IsPost)
            {
                UserItem postVal = new UserItem();
                string myUserId = _ac.Get()[0];
                postVal.ID = id;
                postVal.UserID = SiteFun.Post("userId");
                postVal.Name = SiteFun.Post("name");
                postVal.Password = SiteFun.Encryption(SiteFun.Post("password"));
                postVal.LastIP = old.LastIP;
                postVal.LastTime = old.LastTime;
                if (myUserId.ToUpper() != old.UserID.ToUpper()) { postVal.Locked = SiteFun.ToInt(SiteFun.Post("locked")) == 0 ? false : true; }
                if (string.IsNullOrEmpty(postVal.Password)) { postVal.Password = old.Password; }
                if (string.IsNullOrEmpty(postVal.UserID) || usrData.ExistsUserID(postVal.UserID, postVal.ID)) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoUserIdOrExists"))); }
                else
                {
                    if (string.IsNullOrEmpty(postVal.Password)) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoPwd"))); }
                    else
                    {
                        if (string.IsNullOrEmpty(postVal.Name)) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoNickName"))); }
                        else
                        {
                            if (postVal.ID == 0) { usrData.InsertUser(postVal); sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgInsertDat"), "?act=user&mode=list")); }
                            else { usrData.UpdateUser(postVal); sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgUpdateDat"), "?act=user&mode=list")); }
                        }
                    }
                }
                old = postVal;
            }
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.FormStart());
            sr.Append(HtmlUI.TableStart("onCenter"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("UserId")) + HtmlUI.CreateTd(HtmlUI.Input("userId", 15, null, SiteFun.HtmlEncode(old.UserID))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Pwd")) + HtmlUI.CreateTd(HtmlUI.PasswordInput("password", 30, null, null)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("NickName")) + HtmlUI.CreateTd(HtmlUI.Input("name", 15, null, old.Name)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("LastIp")) + HtmlUI.CreateTd(SiteFun.HtmlEncode(old.LastIP)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("LastDt")) + HtmlUI.CreateTd(old.LastTime));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Virtue")) + HtmlUI.CreateTd(HtmlUI.CheckBoxInput(SiteDat.GetLan("Locked"), "locked", 1, old.Locked)));
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
