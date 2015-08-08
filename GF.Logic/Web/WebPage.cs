using System;
using System.IO;
using System.Text;
using GF.Core;
using GF.Data;
using GF.Data.Entity;

namespace GF.Logic.Web
{
    /// <summary>
    /// 独立页面展示类
    /// </summary>
    public class WebPage
    {
        private SettingItem _setting;
        private string _theme;
        private _DbHelper conn;

        public WebPage(_DbHelper c)
        {
            conn = c;
            SettingData setData = new SettingData(conn);
            this._setting = setData.GetSetting();
            this._theme = setData.GetTheme;
        }

        /// <summary>
        /// 获取网站配置
        /// </summary>
        public SettingItem OutSetting
        {
            get
            {
                return this._setting;
            }
        }

        /// <summary>
        /// 获取网站主题
        /// </summary>
        public string OutTheme
        {
            get
            {
                return this._theme;
            }
        }
        /// <summary>
        /// 向浏览器完整XML内容
        /// </summary>
        public string OutWrite(int id, string local)
        {
            SettingItem setting = this._setting;
            //获取页面数据
            PostItem pg = new PostItem();
            if (id > 0)
            {
                if (SiteDat.GetDat(string.Format(SiteCache.PostFormat, id)) == null)
                {
                    SiteDat.SetDat(string.Format(SiteCache.PostFormat, id), new PostData(conn).GetPost(id));
                }
                pg = (PostItem)SiteDat.GetDat(string.Format(SiteCache.PostFormat, id));
            }
            else
            {
                if (SiteDat.GetDat(string.Format(SiteCache.PostFormat, local)) == null)
                {
                    SiteDat.SetDat(string.Format(SiteCache.PostFormat, local), new PostData(conn).GetPost(local));
                }
                pg = (PostItem)SiteDat.GetDat(string.Format(SiteCache.PostFormat, local));
            }
            //本页XML处理
            StringBuilder xml = new StringBuilder();
            xml.Append("\t\t<page>\n");
            xml.AppendFormat("\t\t\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.PageLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(pg.Local))));
            xml.AppendFormat("\t\t\t<id>{0}</id>\n", pg.ID);
            xml.AppendFormat("\t\t\t<local>{0}</local>\n", SiteFun.CDATA(pg.Local));
            xml.AppendFormat("\t\t\t<title>{0}</title>\n", SiteFun.CDATA(pg.Title));
            xml.AppendFormat("\t\t\t<content>{0}</content>\n", SiteFun.CDATA(pg.Content));
            xml.AppendFormat("\t\t\t<publish>{0}</publish>\n", pg.Publish);
            xml.Append("\t\t</page>\n");
            //绑定XML并写出
            string title = string.Empty;
            if (pg.ID > 0) { title = string.Format("{0} - {1}", pg.Title, setting.Basic.Name); }
            return new _WebBaseXml(conn).OutBaseXml(title, xml.ToString());
        }
    }
}
