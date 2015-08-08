using System.Configuration;
using System.Web;

namespace GF.Core
{
    /// <summary>
    /// 全局配置
    /// </summary>
   public sealed class SiteCfg
    {
        /// <summary>
        /// 获取系统名称
        /// </summary>
       public static string System = "GeneralFramework";

        /// <summary>
        /// 获取系统版本
        /// </summary>
        public static string Version = "1.0.0";

        /// <summary>
        /// 官方网站
        /// </summary>
        public static string WebSite = "http://www.5ianzhuo.com/";

        /// <summary>
        /// 更新源
        /// </summary>
        public static string UpdateHost = "http://update.5ianzhuo.com/";

        /// <summary>
        /// 截断匹配正则
        /// </summary>
        public static string PageBreakRegular = @"<div\s+style=""page-break-after:\s*always;?\s*"">((.|\n)+?|^<\/div>)<\/div>";

        /// <summary>
        /// 截断检查符号
        /// </summary>
        public static string PageBreakSymbol = "[__GF_TEMP_PAGEBREAK_CHARS]";

        /// <summary>
        /// 获取系统版本完整纯字符串
        /// </summary>
        public static string SystemVersionFull
        {
            get { return string.Format("{0} v{1}", System, Version); }
        }

        /// <summary>
        /// 获取提供版权代码
        /// </summary>
        public static string Powered
        {
            get { return string.Format("Powered by <a href=\"{2}\" rel=\"external\"><strong>{0}</strong></a> <em>v{1}</em>", System, Version, WebSite); }
        }

        /// <summary>
        /// 获取安装根目录相对路径
        /// </summary>
        public static string Path
        {
            get { return ConfigurationManager.AppSettings["Path"].ToString(); }
        }

        /// <summary>
        /// 获取安装根目录绝对路径
        /// </summary>
        public static string Router
        {
            get { return HttpContext.Current.Server.MapPath(Path); }
        }

        /// <summary>
        /// 数据库相对路径
        /// </summary>
        public static string DBPath
        {
            get { return ConfigurationManager.AppSettings["DBPath"].ToString(); }
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        public static string Conn
        {
            get
            {
                string path = HttpContext.Current.Server.MapPath(DBPath);
                return ConfigurationManager.AppSettings["DBConn"].Replace("{Path}", path);
            }
        }

        /// <summary>
        /// 获取会话标记
        /// </summary>
        public static string Token
        {
            get { return ConfigurationManager.AppSettings["Token"].ToString(); }
        }

    }


}
