using System;
using System.IO;
using System.Text;
using GF.Core;
using GF.Data;
using GF.Data.Entity;

namespace GF.Logic.Web
{
    /// <summary>
    /// 标签展示类
    /// </summary>
    public class WebTags
    {
        private SettingItem _setting;
        private string _theme;
        private _DbHelper conn;

        public WebTags(_DbHelper c)
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
        /// 向浏览器打印代码
        /// </summary>
        public string OutWrite()
        {
            //本页XML处理
            StringBuilder xml = new StringBuilder();
            xml.Append("\t\t<tags>\n");
            TagList tags = new TagData(conn).SelectTag(1, int.MaxValue);
            foreach (TagItem vItem in tags)
            {
                xml.Append("\t\t\t<item>\n");
                xml.AppendFormat("\t\t\t\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.TagLinkFormat, SiteCfg.Path, SiteFun.UrlEncode(vItem.Key))));
                xml.AppendFormat("\t\t\t\t<id>{0}</id>\n", vItem.ID);
                xml.AppendFormat("\t\t\t\t<key>{0}</key>\n", SiteFun.CDATA(vItem.Key));
                xml.AppendFormat("\t\t\t\t<postCount>{0}</postCount>\n", vItem.PostCount);
                xml.Append("\t\t\t</item>\n");
            }
            xml.Append("\t\t</tags>\n");
            //绑定XML并写出
            return new _WebBaseXml(conn).OutBaseXml(SiteDat.GetLan("Tag"), xml.ToString());
        }
    }
}
