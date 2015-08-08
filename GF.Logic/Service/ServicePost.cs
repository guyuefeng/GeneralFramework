using System;
using System.IO;
using System.Text;
using System.Web;
using GF.Core;
using GF.Data;
using GF.Data.Entity;
using GF.Logic.Admin;

namespace GF.Logic.Service
{
    /// <summary>
    /// 文章异步处理类
    /// </summary>
    public class ServicePost
    {
        private _AdminCookie _ac = new _AdminCookie();
        private _DbHelper conn;

        public ServicePost(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 取得密码文章内容
        /// </summary>
        public void OutWriteOfPasswordArticle()
        {
            //初始化引擎
            string msg = string.Empty;
            int error = 1;
            string cont = string.Empty;
            int artId = SiteFun.ToInt(SiteFun.Post("artId"));
            //0-阐述,1-内容
            int result = SiteFun.ToInt(SiteFun.Post("isContent"));
            PostItem art = new PostData(conn).GetPost(artId);
            if (art.ID > 0 && art.Show)
            {
                if (art.Password == SiteFun.Post("pwd"))
                {
                    error = 0;
                    cont = result == 0 ? art.Explain : art.Content;
                }
                else { msg = SiteDat.GetLan("MsgPwdErr"); }
            }
            else { msg = SiteDat.GetLan("MsgArtNotExists"); }
            StringBuilder xml = new StringBuilder();
            xml.AppendFormat("\t\t<error>{0}</error>\n", error);
            xml.AppendFormat("\t\t<result>{0}</result>\n", SiteFun.CDATA(cont));
            xml.Append(new _ServiceBaseXml().OutBaseXml(msg, xml.ToString()));
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.Write(xml);
        }

        /// <summary>
        /// 投票
        /// </summary>
        public void OutWriteVote()
        {
            int artId = SiteFun.ToInt(SiteFun.Post("artId"));
            HttpContext.Current.Response.Write(new PostData(conn).AddPostVote(artId));
        }

        /// <summary>
        /// 取得未保存文章内容
        /// </summary>
        public void OutRePostContent()
        {
            //开始
            string content = string.Empty;
            string[] usrInfo = _ac.Get();
            if (new UserData(conn).CheckUser(usrInfo[0], SiteFun.Encryption(usrInfo[1]), false).ID > 0)
            {
                try
                {
                    string filePath = Path.Combine(SiteCfg.Router, "Common/Temp/post_cache.tmp");
                    content = File.ReadAllText(filePath);
                    //File.Delete(filePath);
                }
                catch { }
            }
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.Write(new _ServiceBaseXml().OutBaseXml(null, string.Format("\t\t<result>{0}</result>", SiteFun.CDATA(content))));
        }

        /// <summary>
        /// 自动保存内容
        /// </summary>
        public void OutAutoSavePost()
        {
            //开始
            string msg = string.Empty;
            string[] usrInfo = _ac.Get();
            if (new UserData(conn).CheckUser(usrInfo[0], SiteFun.Encryption(usrInfo[1]), false).ID == 0) { msg = SiteDat.GetLan("MsgNotAnAdm"); }
            else
            {
                string content = SiteFun.Post("content");
                try
                {
                    File.WriteAllText(Path.Combine(SiteCfg.Router, "Common/Temp/post_cache.tmp"), content);
                    msg = SiteDat.GetLan("MsgSaveSucc");
                }
                catch (Exception err) { msg = err.Message; }
            }
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.Write(new _ServiceBaseXml().OutBaseXml(msg, null));
        }
    }
}
