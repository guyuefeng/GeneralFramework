using System.Collections;
using System.Text;
using GF.Core;
using GF.Logic.UI;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 缓存管理类
    /// </summary>
    public class AdminCache
    {
        /// <summary>
        /// 取得系统缓存列表窗体
        /// </summary>
        /// <returns>返回系统缓存列表代码</returns>
        public string List()
        {
            StringBuilder sr = new StringBuilder();
            if (SiteFun.IsPost)
            {
                bool allRemove = SiteFun.ToInt(SiteFun.Post("removeAll")) == 0 ? false : true;
                if (allRemove) { SiteDat.ClearDat(); }
                else
                {
                    string key = SiteFun.Post("key");
                    SiteDat.RemoveDat(key);
                }
                sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgClearCacheSucc")));
            }
            //全部清除
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.FormStart());
            sr.Append(HtmlUI.TableStart("onCenter"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("ClearCache")));
            sr.Append(HtmlUI.CreateTd(HtmlUI.HiddenInput("removeAll", 1) + HtmlUI.SubmitButton(SiteDat.GetLan("BtnClear"))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(HtmlUI.FormFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            //缓存列表
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("CacheKey")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("CacheType")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Operate")));
            sr.Append(HtmlUI.TrFinal());
            int i = 1;
            IDictionaryEnumerator cacheEnum = SiteDat.GetCacheEnumerator();
            while (cacheEnum.MoveNext())
            {
                i++;
                sr.Append(HtmlUI.FormStart());
                sr.Append(HtmlUI.TrStart(i % 2 == 0 ? " cRow" : null));
                sr.Append(HtmlUI.CreateTd(cacheEnum.Key));
                sr.Append(HtmlUI.CreateTd(SiteDat.GetDat(cacheEnum.Key.ToString()).GetType().Name));
                sr.Append(HtmlUI.CreateTd(HtmlUI.HiddenInput("key", cacheEnum.Key) + HtmlUI.SubmitButton(SiteDat.GetLan("BtnDel"))));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.FormFinal());
            }
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            return sr.ToString();
        }
    }
}
