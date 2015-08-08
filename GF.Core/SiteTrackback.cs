using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;

namespace GF.Core
{
    public class SiteTrackback
    {
        /// <summary>
        /// 向指定的 URL 发送 Trackback Ping,并根据服务器端返回的信息,提示用户处理情况
        /// </summary>
        /// <param name="RemoteUrl">引用地址</param>
        /// <param name="MyBlogURL">访问地址</param>
        /// <param name="MyBlogName">站点名称,要进行URL编码</param>
        /// <param name="MyBlogTitle">当前文章标题,要进行URL编码</param>
        /// <param name="MyBlogExcerpt">当前文章摘要,要进行URL编码</param>
        /// <returns>返回结果:"状态|说明",状态：0-成功,1-失败</returns>
        public string SendTrackback(string RemoteUrl, string MyBlogURL, string MyBlogName, string MyBlogTitle, string MyBlogExcerpt)
        {
            string sEndReturn = string.Empty;
            try
            {
                string strPostInfo = "title=" + MyBlogTitle;
                strPostInfo += "&url=" + MyBlogURL;
                strPostInfo += "&excerpt=" + MyBlogExcerpt;
                strPostInfo += "&blog_name=" + MyBlogName;
                CookieContainer oCC = new CookieContainer();
                byte[] strs = Encoding.Default.GetBytes(strPostInfo);
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(RemoteUrl);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = strs.Length;
                Stream newStream = myRequest.GetRequestStream();
                //发送数据
                newStream.Write(strs, 0, strs.Length);
                newStream.Close();
                myRequest.CookieContainer = oCC;
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                oCC.Add(myResponse.Cookies);
                Stream oStream = myResponse.GetResponseStream();
                string sReturn = new StreamReader(oStream).ReadToEnd();
                XmlDocument oXmlDoc = new XmlDocument();
                oXmlDoc.LoadXml(sReturn);
                sEndReturn = string.Format("{0}|{1}", oXmlDoc.GetElementsByTagName("error").Item(0).InnerText, oXmlDoc.GetElementsByTagName("message").Item(0).InnerText);
            }
            catch (Exception err) { sEndReturn = "1|" + err.Message; }
            return sEndReturn;
        }
    }
}
