using System.Text;
using GF.Core;
using GF.Data;
using GF.Data.Entity;
using GF.Logic.UI;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 分类管理类
    /// </summary>
    public class AdminTag
    {
        private _DbHelper conn;

        public AdminTag(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        private void ClearCache()
        {
            SiteDat.RemoveDat(SiteCache.HotTag);
        }

        /// <summary>
        /// 系统标签操作
        /// </summary>
        /// <returns>标签列表及操作表单</returns>
        public string List()
        {
            StringBuilder sr = new StringBuilder();
            TagData tData = new TagData(conn);
            if (SiteFun.IsPost)
            {
                //取值
                TagItem postVal = new TagItem();
                postVal.ID = SiteFun.ToInt(SiteFun.Post("id"));
                postVal.Key = SiteFun.Post("key");
                //处理
                if (string.IsNullOrEmpty(postVal.Key)) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgNoKey"))); }
                else
                {
                    if (tData.ExistsKey(postVal.Key, postVal.ID)) { sr.Append(AdminUI.ErrorBox(SiteDat.GetLan("MsgTagExists"))); }
                    else
                    {
                        if (postVal.ID == 0) { tData.InsertTag(postVal); sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgInsertDat"))); }
                        else
                        {
                            if (SiteFun.ToInt(SiteFun.Post("del")) == 0) { tData.UpdateTag(postVal); sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgUpdateDat"))); }
                            else { tData.DeleteTag(postVal.ID); sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgDelDat"))); }
                        }
                    }
                }
                ClearCache();
            }
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Key")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Article")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Delete")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Operate")));
            sr.Append(HtmlUI.TrFinal());
            //下面三行是分页设置
            int page = SiteFun.ToInt(SiteFun.Query("page"));
            if (page < 1) { page = 1; }
            int pageSize = 20;
            TagList list = new TagData(conn).SelectTag(page, pageSize);
            int i = 1;
            foreach (TagItem vItem in list)
            {
                i++;
                sr.Append(HtmlUI.FormStart());
                sr.Append(HtmlUI.TrStart(i % 2 == 0 ? " cRow" : null));
                sr.Append(HtmlUI.CreateTd(HtmlUI.HiddenInput("id", vItem.ID) + HtmlUI.Input("key", 20, null, SiteFun.HtmlEncode(vItem.Key))));
                sr.Append(HtmlUI.CreateTd(vItem.PostCount));
                sr.Append(HtmlUI.CreateTd(HtmlUI.CheckBoxInput("del", 1, false)));
                sr.Append(HtmlUI.CreateTd(HtmlUI.SubmitButton(SiteDat.GetLan("BtnSave"))));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.FormFinal());
            }
            sr.Append(HtmlUI.FormStart());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTd(HtmlUI.Input("key", 20, null, null)));
            sr.Append(HtmlUI.CreateTd(0));
            sr.Append(HtmlUI.CreateTd());
            sr.Append(HtmlUI.CreateTd(HtmlUI.SubmitButton(SiteDat.GetLan("BtnInsert"))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.FormFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTd(new SitePages().Make(list.Number, page, pageSize, "?act=tag&amp;mode=list&amp;page={0}"), 4, null));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            return sr.ToString();
        }
    }
}
