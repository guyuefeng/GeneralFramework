using System;
using System.IO;
using System.Text;
using GF.Core;
using GF.Data;
using GF.Data.Entity;

namespace GF.Logic.Web
{
    /// <summary>
    /// 友情链接展示类
    /// </summary>
    public class WebFellows
    {
        private SettingItem _setting;
        private string _theme;
        private _DbHelper conn;

        public WebFellows(_DbHelper c)
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
        /// 向浏览器打印完整XML内容
        /// </summary>
        public string OutWrite()
        {
            //本页XML处理
            StringBuilder xml = new StringBuilder();
            xml.Append("\t\t<fellows>\n");
            DataList<FellowItem> fellows = new FellowData(conn).SelectFellow(1, int.MaxValue, false, false);
            foreach (FellowItem vItem in fellows)
            {
                xml.Append("\t\t\t<item>\n");
                xml.AppendFormat("\t\t\t\t<id>{0}</id>\n", vItem.ID);
                xml.AppendFormat("\t\t\t\t<name>{0}</name>\n", SiteFun.CDATA(vItem.Name));
                xml.AppendFormat("\t\t\t\t<url>{0}</url>\n", SiteFun.CDATA(vItem.URL));
                xml.AppendFormat("\t\t\t\t<logo>{0}</logo>\n", SiteFun.CDATA(vItem.Logo));
                xml.AppendFormat("\t\t\t\t<explain>{0}</explain>\n", SiteFun.CDATA(vItem.Explain));
                xml.AppendFormat("\t\t\t\t<style>{0}</style>\n", SiteFun.CDATA(vItem.Style));
                xml.Append("\t\t\t</item>\n");
            }
            xml.Append("\t\t</fellows>\n");
            //绑定XML并写出
            return new _WebBaseXml(conn).OutBaseXml(SiteDat.GetLan("Fellow"), xml.ToString());
        }
    }
}
