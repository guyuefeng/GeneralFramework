using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GF.Data;
using GF.Logic.Service;
using GF.Core;
using GF.Logic;

namespace GF.Web
{
    public partial class Service : System.Web.UI.Page
    {
        /// <summary>
        /// 显示内容
        /// </summary>
        /// <param name="sender">基类对象</param>
        /// <param name="e">事件对象</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            using (_DbHelper conn = new _DbHelper(SiteCfg.Conn))
            {
                conn.Open();
                conn.BeginTransaction();
                try
                {
                    _ServiceBase sb = new _ServiceBase();
                    sb.InitConn();
                    //开始执行
                    switch (SiteFun.Query("act"))
                    {
                        case "comment":
                            {
                                new ServiceComment(conn).OutWrite();
                                break;
                            }
                        case "passwordArticle":
                            {
                                new ServicePost(conn).OutWriteOfPasswordArticle();
                                break;
                            }
                        case "rss":
                            {
                                new ServiceRss(conn).OutWrite();
                                break;
                            }
                        case "autoSave":
                            {
                                new ServicePost(conn).OutAutoSavePost();
                                break;
                            }
                        case "rePostContent":
                            {
                                new ServicePost(conn).OutRePostContent();
                                break;
                            }
                        case "artVote":
                            {
                                new ServicePost(conn).OutWriteVote();
                                break;
                            }
                        case "matchTags":
                            {
                                new ServiceTag(conn).GetMatchTags();
                                break;
                            }
                        case "trackback":
                            {
                                new ServiceTrackback(conn).OutWrite();
                                break;
                            }
                        case "verifyCode":
                            {
                                new ServiceVerifyCode().Display();
                                break;
                            }
                        case "getTime":
                            {
                                new ServiceTime().OutWrite();
                                break;
                            }
                        case "expand":
                            {
                                foreach (object type in new SiteExpand().GetTypes(typeof(IServiceExpand).FullName))
                                {
                                    IServiceExpand iae = ((IServiceExpand)type);
                                    if (iae.Key == SiteFun.Query("key"))
                                    {
                                        iae.OutWrite();
                                        break;
                                    }
                                }
                                break;
                            }
                    }
                    if (conn.ExecuteCount > 0) { conn.Commit(); }
                }
                catch (Exception err)
                {
                    if (conn.ExecuteCount > 0) { conn.Rollback(); }
                    IMyError exeError = new MyError();
                    foreach (IMyError myError in new SiteExpand().GetTypes(typeof(IMyError).FullName))
                    {
                        exeError = myError;
                        break;
                    }
                    exeError.PrintError(err);
                }
                conn.Close();
            }
        }
    }
}
