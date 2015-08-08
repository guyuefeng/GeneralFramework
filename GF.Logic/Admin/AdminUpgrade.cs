using System;
using System.Text;
using GF.Core;
using GF.Logic.UI;
using GF.Logic.Upgrade;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 更新检查类
    /// </summary>
    public class AdminUpgrade
    {
        /// <summary>
        /// 数据状态信息
        /// </summary>
        public string OutWrite()
        {
            StringBuilder sr = new StringBuilder();
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart("onCenter"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("CurrVer")) + HtmlUI.CreateTd(SiteCfg.SystemVersionFull));
            sr.Append(HtmlUI.TrFinal());
            try
            {
                UpgradeCheck uc = new UpgradeCheck(SiteCfg.Version);
                sr.Append(HtmlUI.TrStart());
                if (uc.Version == SiteCfg.SystemVersionFull) { sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(SiteDat.GetLan("MsgIsLatestVer"))); }
                else
                {
                    sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("ToVer")) + HtmlUI.CreateTd(uc.Version));
                }
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(uc.XmlUri));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.TrFinal());
                if (!string.IsNullOrEmpty(uc.Intro))
                {
                    sr.Append(HtmlUI.TrStart());
                    sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(uc.Intro));
                    sr.Append(HtmlUI.TrFinal());
                }
                sr.Append(HtmlUI.TableFinal());
                sr.Append(AdminUI.AdminBoxFinal());
                sr.Append(AdminUI.AdminBoxStart());
                sr.Append(HtmlUI.TableStart("onCenter"));
                int i = 0;
                foreach (UpgradeFileEntity vItem in uc.GetFileList)
                {
                    i++;
                    sr.Append(HtmlUI.TrStart());
                    sr.Append(HtmlUI.CreateTh(i == 1 ? SiteDat.GetLan("UpgradeList") : string.Empty) + HtmlUI.CreateTd(vItem.FilePath));
                    sr.Append(HtmlUI.TrFinal());
                }
                i = 0;
                foreach (UpgradeFileEntity vItem in uc.GetRemoveList)
                {
                    i++;
                    sr.Append(HtmlUI.TrStart());
                    sr.Append(HtmlUI.CreateTh(i == 1 ? SiteDat.GetLan("RemoveList") : string.Empty) + HtmlUI.CreateTd(vItem.FilePath));
                    sr.Append(HtmlUI.TrFinal());
                }
                if (uc.Version != SiteCfg.SystemVersionFull)
                {
                    string upPackUri = uc.PackFile;
                    if (!upPackUri.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase)) { upPackUri = string.Format("{0}_GF/{1}/{2}", SiteCfg.UpdateHost, SiteCfg.Version, uc.PackFile); }
                    sr.Append(HtmlUI.TrStart());
                    sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("ManualUpdate")) + HtmlUI.CreateTd(HtmlUI.Link(SiteDat.GetLan("GetUpgradePack"), upPackUri, null, true)));
                    sr.Append(HtmlUI.TrFinal());
                    sr.Append(HtmlUI.TrStart());
                    sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("ChkToken")) + HtmlUI.CreateTd(HtmlUI.Input("upgradeToken", "upgradeToken", 20, null, null)));
                    sr.Append(HtmlUI.TrFinal());
                    sr.Append(HtmlUI.TrStart());
                    sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(HtmlUI.HiddenInput("upgradeVer", "upgradeVer", SiteCfg.Version) + HtmlUI.Button("upgradeBegin", SiteDat.GetLan("BtnUpgrade"))));
                    sr.Append(HtmlUI.TrFinal());
                    sr.Append(HtmlUI.TrStart());
                    sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd("<div class=\"preBox\"><div id=\"preDemo\" class=\"preNum\" style=\"width:0%;\"></div></div><div id=\"upgradeState\"></div>"));
                    sr.Append(HtmlUI.TrFinal());
                }
            }
            catch (Exception err)
            {
                sr.Append(HtmlUI.TrStart());
                sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("GetUpgradeError")) + HtmlUI.CreateTd(err.Message));
                sr.Append(HtmlUI.TrFinal());
            }
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            return sr.ToString();
        }
    }
}
