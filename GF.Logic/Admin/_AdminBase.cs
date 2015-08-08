using System;
using System.Text;
using System.Web;
using System.Xml;
using GF.Core;
using GF.Data;
using GF.Data.Entity;
using GF.Logic.Template;
using GF.Logic.UI;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 后台管理基类
    /// </summary>
    public class _AdminBase
    {
        private _AdminCookie _ac = new _AdminCookie();
        private _DbHelper conn;

        public _AdminBase(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 初始化数据库连接
        /// </summary>
        public void InitConn()
        {
            DataBase.Conn = SiteCfg.Conn;
        }

        /// <summary>
        /// 是否已经登录
        /// </summary>
        public bool IsLogin
        {
            get
            {
                string[] adminCookies = _ac.Get();
                UserItem myUser = new UserItem();
                myUser = new UserData(conn).CheckUser(adminCookies[0], SiteFun.Encryption(adminCookies[1]), false);
                return myUser.ID > 0;
            }
        }

        /// <summary>
        /// 登录窗口
        /// </summary>
        /// <returns>登录代码</returns>
        public string LoginWindow()
        {
            StringBuilder sr = new StringBuilder();
            if (SiteFun.IsPost)
            {
                UserData userData = new UserData(conn);
                //取得输入的值
                string[] postVal = { SiteFun.Post("userID"), SiteFun.Post("pwd"), SiteFun.Post("vc") };
                UserItem chkUser = userData.CheckUser(postVal[0], SiteFun.Encryption(postVal[1]), false);
                if (!SiteDat.CheckVerifyCode(postVal[2])) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgVcErr"))); }
                else
                {
                    if (chkUser.ID == 0) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoFindAdm"))); }
                    else
                    {
                        userData.UpdateUserIpAndTime(chkUser.ID, SiteFun.GetGuestIP, DateTime.Now);
                        _ac.Set(postVal[0], postVal[1]);
                        HttpContext.Current.Response.Redirect(HttpContext.Current.Request.FilePath);
                    }
                }
            }
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.FormStart());
            sr.Append(HtmlUI.TableStart("autoSize onCenter"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("UserId")) + HtmlUI.CreateTd(HtmlUI.Input("userID", 20, "userName", null)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Pwd")) + HtmlUI.CreateTd(HtmlUI.PasswordInput("pwd", 20, "password", null)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("VCode")) + HtmlUI.CreateTd(HtmlUI.Input("vc", 4, null, null) + HtmlUI.Image("Service.aspx?act=verifyCode", SiteDat.GetLan("MsgClickReloadVc"), "javascript:this.src='Service.aspx?act=verifyCode&amp;random='+Math.random();")));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(HtmlUI.SubmitButton(SiteDat.GetLan("BtnLogin")) + HtmlUI.ResetButton()));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(HtmlUI.FormFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            return sr.ToString();
        }

        /// <summary>
        /// 取得菜单
        /// </summary>
        public string MenuCode(string xmlContent, string bigClassCode, string classCode)
        {
            StringBuilder sr = new StringBuilder();
            //取得循环元素
            string parentMenu = bigClassCode;
            string menuItem = classCode;
            //开始处理
            XmlDocument menuXml = new XmlDocument();
            menuXml.LoadXml(xmlContent);
            XmlNodeList parents = menuXml.SelectNodes("/root/parent");
            foreach (XmlNode node in parents)
            {
                string itemsCode = string.Empty;
                XmlNodeList items = node.SelectNodes("item");
                foreach (XmlNode itemNode in items)
                {
                    FileTemplate ift = new FileTemplate();
                    ift.LoadCode(menuItem);
                    ift.SetTag("Caption", itemNode.Attributes["name"].Value);
                    ift.SetTag("Link", itemNode.Attributes["link"].Value);
                    if (string.IsNullOrEmpty(itemNode.Attributes["icon"].Value)) { ift.SetTag("Icon", string.Empty); }
                    else { ift.SetTag("Icon", string.Format("<img src=\"Common/Images/Icons/{0}\" width=\"16\" height=\"16\"/>", itemNode.Attributes["icon"].Value)); }
                    itemsCode += ift.Code;
                }
                //全局处理
                FileTemplate ft = new FileTemplate();
                ft.LoadCode(parentMenu);
                ft.SetTag("Caption", node.Attributes["name"].Value);
                ft.SetTag("Rel", node.Attributes["rel"].Value);
                ft.SetTag("ID", node.Attributes["id"].Value);
                ft.SetTag("ItemList", itemsCode);
                sr.Append(ft.Code);
            }
            return sr.ToString();
        }
    }
}
