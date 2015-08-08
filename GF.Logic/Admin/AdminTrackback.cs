using GF.Core;
using GF.Logic.UI;
using GF.Data;
using GF.Data.Entity;
using System.Text;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 引用通告管理类
    /// </summary>
    public class AdminTrackback
    {
        private _DbHelper conn;

        public AdminTrackback(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 取得已发送引用通告列表窗体
        /// </summary>
        /// <returns>返回引用通告列表代码</returns>
        public string List()
        {
            StringBuilder sr = new StringBuilder();
            TrackbackLogData tbData = new TrackbackLogData(conn);
            if (SiteFun.IsPost)
            {
                int id = SiteFun.ToInt(SiteFun.Post("id"));
                if (id > 0) { tbData.DeleteTrackbackLog(id); sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgDelDat"))); }
            }
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("URL")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Success")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Result")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("DateTime")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Operate")));
            sr.Append(HtmlUI.TrFinal());
            //下面三行是分页设置
            int page = SiteFun.ToInt(SiteFun.Query("page"));
            if (page < 1) { page = 1; }
            int pageSize = 20;
            DataList<TrackbackLogItem> list = tbData.SelectTrackbackLog(page, pageSize);
            int i = 1;
            foreach (TrackbackLogItem vItem in list)
            {
                i++;
                sr.Append(HtmlUI.FormStart());
                sr.Append(HtmlUI.TrStart(i % 2 == 0 ? " cRow" : null));
                sr.Append(HtmlUI.CreateTd(HtmlUI.Link(SiteFun.HtmlEncode(SiteFun.StrCut(vItem.URL, 50)), vItem.URL)));
                sr.Append(HtmlUI.CreateTd(vItem.Error ? SiteDat.GetLan("No") : SiteDat.GetLan("Yes")));
                sr.Append(HtmlUI.CreateTd(SiteFun.HtmlEncode(vItem.Message)));
                sr.Append(HtmlUI.CreateTd(vItem.Publish));
                sr.Append(HtmlUI.CreateTd(HtmlUI.HiddenInput("id", vItem.ID) + HtmlUI.SubmitButton(SiteDat.GetLan("BtnDel"))));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.FormFinal());
            }
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTd(new SitePages().Make(list.Number, page, pageSize, "?act=info&amp;mode=tb&amp;page={0}"), 5, null));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            return sr.ToString();
        }
    }
}
