using System.Text;
using System.Web;
using GF.Core;
using GF.Data;
using GF.Data.Entity;

namespace GF.Logic.Service
{
    /// <summary>
    /// RSS 处理类
    /// </summary>
    public class ServiceRss
    {
        private _DbHelper conn;

        public ServiceRss(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 写出引用通告处理状态
        /// </summary>
        public void OutWrite()
        {
            //取值
            int cid = SiteFun.ToInt(SiteFun.Query("cid"));
            SettingItem set = new SettingData(conn).GetSetting();
            //处理和返回
            StringBuilder xml = new StringBuilder();
            xml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
            xml.Append("<rss version=\"2.0\">\n");
            xml.Append("<channel>\n");
            xml.AppendFormat("<title>{0}</title>\n", SiteFun.CDATA(set.Basic.Name));
            xml.AppendFormat("<link>{0}</link>\n", SiteFun.CDATA(set.Basic.URL));
            xml.AppendFormat("<description>{0}</description>\n", SiteFun.CDATA(set.Basic.Intro));
            xml.AppendFormat("<language>{0}</language>\n", SiteFun.CDATA(set.Basic.Language));
            xml.AppendFormat("<copyright>{0}</copyright>\n", SiteFun.CDATA("Copyright " + set.Basic.Name));
            xml.AppendFormat("<webMaster>{0}</webMaster>\n", SiteFun.CDATA(set.Basic.Mail));
            xml.AppendFormat("<generator>{0}</generator>\n", SiteFun.CDATA(SiteCfg.SystemVersionFull));
            xml.Append("<image>\n");
            xml.AppendFormat("\t<title>{0}</title>\n", SiteFun.CDATA(set.Basic.Name));
            xml.AppendFormat("\t<url>{0}</url>\n", SiteFun.CDATA(set.Basic.URL + "Common/Images/Logo.png"));
            xml.AppendFormat("\t<link>{0}</link>\n", SiteFun.CDATA(set.Basic.URL));
            xml.AppendFormat("\t<description>{0}</description>\n", SiteFun.CDATA(set.Basic.Intro));
            xml.Append("</image>\n");
            foreach (PostItem vItem in new PostData(conn).SelectPost(cid, null, null, 1, set.Parameter.RssNum, 0, "A", false))
            {
                xml.Append("<item>\n");
                xml.AppendFormat("\t<link>{0}</link>\n", SiteFun.CDATA(string.Format(SitePath.ArticleLinkFormat, set.Basic.URL, vItem.Local)));
                xml.AppendFormat("\t<title>{0}</title>\n", SiteFun.CDATA(vItem.Title));
                xml.AppendFormat("\t<author>{0}</author>\n", SiteFun.CDATA(vItem.Author));
                xml.AppendFormat("\t<category>{0}</category>\n", SiteFun.CDATA(new ColumnData(conn).GetColumn(vItem.ColumnID).Name));
                xml.AppendFormat("\t<pubDate>{0}</pubDate>\n", vItem.Publish);
                xml.AppendFormat("\t<guid>{0}</guid>\n", SiteFun.CDATA(string.Format(SitePath.ArticleLinkFormat, set.Basic.URL, vItem.Local)));
                xml.AppendFormat("\t<description>{0}</description>\n", SiteFun.CDATA(string.IsNullOrEmpty(vItem.Password) ? (set.Parameter.RssMode == 0 ? vItem.Explain : vItem.Content) : SiteDat.GetLan("MsgEncContent")));
                xml.Append("</item>\n");
            }
            xml.Append("</channel>\n");
            xml.Append("</rss>");
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.Write(xml);
        }
    }
}
