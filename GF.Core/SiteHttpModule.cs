using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GF.Core
{
    public class SiteHttpModule : IHttpModule
    {

        #region IHttpModule 成员

        /// <summary>
        /// 开始获取重写数据
        /// </summary>
        /// <param name="sender">基础类对象</param>
        /// <param name="e">事件对象</param>
        private void Rewrite_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            string url = SiteDat.GetUrl(application.Context.Request.Path);
            if (!string.IsNullOrEmpty(url))
            {
                application.Context.RewritePath(url);
            }
        }

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(this.Rewrite_BeginRequest);
        }


        #endregion
    }
}
