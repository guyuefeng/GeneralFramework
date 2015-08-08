using System.Text;
using GF.Core;
using GF.Data;
using GF.Data.Entity;
using GF.Logic.UI;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 自定义标签管理类
    /// </summary>
    public class AdminMyTag
    {
        private _DbHelper conn;

        public AdminMyTag(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        private void ClearCache()
        {
            SiteDat.RemoveDat(SiteCache.MyTag);
        }

        /// <summary>
        /// 取得页面列表窗体
        /// </summary>
        /// <returns>返回页面列表代码</returns>
        public string List()
        {
            StringBuilder sr = new StringBuilder();
            MyTagData mtData = new MyTagData(conn);
            if (SiteFun.IsPost)
            {
                int id = SiteFun.ToInt(SiteFun.Post("id"));
                int priority = SiteFun.ToInt(SiteFun.Post("priority"));
                bool show = SiteFun.ToInt(SiteFun.Post("show")) == 0 ? false : true;
                bool del = SiteFun.ToInt(SiteFun.Post("del")) == 0 ? false : true;
                if (del)
                {
                    mtData.DeleteMyTag(id);
                    sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgDelDat")));
                }
                else
                {
                    mtData.UpdateMyTagSome(id, priority, show);
                    sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgUpdateDat")));
                }
                ClearCache();
            }
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Key")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Intro")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("DateTime")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Show")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Priority")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Delete")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Operate")));
            sr.Append(HtmlUI.TrFinal());
            //下面三行是分页设置
            int page = SiteFun.ToInt(SiteFun.Query("page"));
            if (page < 1) { page = 1; }
            int pageSize = 20;
            DataList<MyTagItem> list = mtData.SelectMyTag(page, pageSize, true);
            int i = 1;
            foreach (MyTagItem vItem in list)
            {
                i++;
                sr.Append(HtmlUI.FormStart());
                sr.Append(HtmlUI.TrStart(i % 2 == 0 ? " cRow" : null));
                sr.Append(HtmlUI.CreateTd(HtmlUI.Link(SiteFun.HtmlEncode(vItem.Key), string.Format("?act=myTag&amp;mode=post&amp;id={0}", vItem.ID))));
                sr.Append(HtmlUI.CreateTd(SiteFun.HtmlEncode(vItem.Intro)));
                sr.Append(HtmlUI.CreateTd(vItem.Publish));
                sr.Append(HtmlUI.CreateTd(HtmlUI.CheckBoxInput("show", 1, vItem.Show)));
                sr.Append(HtmlUI.CreateTd(HtmlUI.Input("priority", 5, null, vItem.Priority)));
                sr.Append(HtmlUI.CreateTd(HtmlUI.CheckBoxInput("del", 1, false)));
                sr.Append(HtmlUI.CreateTd(HtmlUI.HiddenInput("id", vItem.ID) + HtmlUI.SubmitButton(SiteDat.GetLan("BtnSave"))));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.FormFinal());
            }
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTd(new SitePages().Make(list.Number, page, pageSize, "?act=myTag&amp;mode=list&amp;page={0}"), 7, null));
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
            MyTagData mtData = new MyTagData(conn);
            if (SiteFun.IsPost)
            {
                MyTagItem postVal = new MyTagItem();
                postVal.ID = id;
                postVal.Key = SiteFun.Post("key");
                postVal.Intro = SiteFun.Post("intro");
                postVal.Code = SiteFun.Post("code");
                postVal.Publish = SiteFun.ToDate(SiteFun.Post("publish"));
                postVal.Priority = SiteFun.ToInt(SiteFun.Post("priority"));
                postVal.Show = SiteFun.ToInt(SiteFun.Post("show")) == 0 ? false : true;
                if (string.IsNullOrEmpty(postVal.Key)) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoKey"))); }
                else
                {
                    if (postVal.ID == 0) { mtData.InsertMyTag(postVal); sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgInsertDat"), "?act=myTag&mode=list")); }
                    else { mtData.UpdateMyTag(postVal); sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgUpdateDat"), "?act=myTag&mode=list")); }
                }
                ClearCache();
            }
            //取得默认值
            MyTagItem old = mtData.GetMyTag(id);
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.FormStart());
            sr.Append(HtmlUI.TableStart("onCenter"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Key")) + HtmlUI.CreateTd(HtmlUI.Input("key", 20, null, SiteFun.HtmlEncode(old.Key))));
            sr.Append(HtmlUI.TrFinal());
            if (old.ID > 0)
            {
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(SiteFun.HtmlEncode(string.Format("<xsl:value-of select=\"/ui/myTag/{0}\" disable-output-escaping=\"yes\"/>", old.Key))));
                sr.Append(HtmlUI.TrFinal());
            }
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Intro")) + HtmlUI.CreateTd(HtmlUI.Input("intro", 50, null, SiteFun.HtmlEncode(old.Intro))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Content")) + HtmlUI.CreateTd(HtmlUI.Textarea("code", 15, 80, SiteFun.HtmlEncode(old.Code))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("DateTime")) + HtmlUI.CreateTd(HtmlUI.Input("publish", 20, null, old.Publish)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Priority")) + HtmlUI.CreateTd(HtmlUI.Input("priority", 5, null, old.Priority)));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Virtue")) + HtmlUI.CreateTd(HtmlUI.CheckBoxInput(SiteDat.GetLan("Show"), "show", 1, old.Show)));
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
