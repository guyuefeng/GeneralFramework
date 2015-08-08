using System.IO;
using System.Text;
using GF.Core;
using GF.Data;
using GF.Logic.UI;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 主题管理类
    /// </summary>
    public class AdminTheme
    {
        private _DbHelper conn;

        public AdminTheme(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        private void ClearCache()
        {
            SiteDat.RemoveDat(SiteCache.Setting);
        }

        /// <summary>
        /// 选择主题包
        /// </summary>
        public string Select()
        {
            StringBuilder sr = new StringBuilder();
            SettingData setData = new SettingData(conn);
            if (SiteFun.IsPost)
            {
                string postVal = SiteFun.Post("theme");
                if (!string.IsNullOrEmpty(postVal))
                {
                    setData.SetTheme(postVal);
                    sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgUpdateThemeSucc")));
                }
                ClearCache();
            }
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart());
            sr.Append(HtmlUI.TrStart());
            //取得系统模板名称并循环
            string theme = setData.GetTheme;
            int count = 0;
            foreach (string dir in Directory.GetDirectories(Path.Combine(SiteCfg.Router, "Common/Theme/")))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                string innerTable = HtmlUI.FormStart();
                innerTable += HtmlUI.TableStart();
                innerTable += HtmlUI.TrStart();
                innerTable += HtmlUI.CreateTd(HtmlUI.Image(string.Format("Common/Theme/{0}/Preview.png", dirInfo.Name), dirInfo.Name, null, 300, 180));
                innerTable += HtmlUI.TrFinal();
                innerTable += HtmlUI.TrStart();
                innerTable += HtmlUI.CreateTd(HtmlUI.HiddenInput("theme", dirInfo.Name) + HtmlUI.SubmitButton(string.Format(SiteDat.GetLan("BtnThemeSel"), dirInfo.Name), (dirInfo.Name == theme ? "red" : string.Empty)));
                innerTable += HtmlUI.TrFinal();
                innerTable += HtmlUI.TableFinal();
                innerTable += HtmlUI.FormFinal();
                //开始处理
                sr.Append(HtmlUI.CreateTd(innerTable));
                count++;
                if (count % 2 == 0)
                {
                    sr.Append(HtmlUI.TrFinal());
                    sr.Append(HtmlUI.TrStart());
                }
            }
            //循环结束
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            return sr.ToString();
        }
    }
}
