using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.Core
{
    public class SitePath
    {
        private static string _cateLinkFormat = "{0}category/{1}.aspx";
        private static string _artLinkFormat = "{0}article/{1}.aspx";
        private static string _tagLinkFormat = "{0}tag/{1}.aspx";
        private static string _pageLinkFormat = "{0}page/{1}.aspx";
        private static string _searLinkFormat = "{0}search/{1}.aspx";
        private static string _pagerFormat = "{0}?cid={1}&tag={2}&key={3}&page={4}";

        /// <summary>
        /// 主题包路径格式
        /// </summary>
        public static string ThemePathFormat
        {
            get { return "Common/Theme/{0}/{1}.xslt"; }
        }

        /// <summary>
        /// 分类链接格式
        /// </summary>
        public static string CategoryLinkFormat
        {
            get { return _cateLinkFormat; }
            set { _cateLinkFormat = value; }
        }

        /// <summary>
        /// 文章链接格式
        /// </summary>
        public static string ArticleLinkFormat
        {
            get { return _artLinkFormat; }
            set { _artLinkFormat = value; }
        }

        /// <summary>
        /// 标签链接格式
        /// </summary>
        public static string TagLinkFormat
        {
            get { return _tagLinkFormat; }
            set { _tagLinkFormat = value; }
        }

        /// <summary>
        /// 独立页链接格式
        /// </summary>
        public static string PageLinkFormat
        {
            get { return _pageLinkFormat; }
            set { _pageLinkFormat = value; }
        }

        /// <summary>
        /// 搜索链接格式
        /// </summary>
        public static string SearchLinkFormat
        {
            get { return _searLinkFormat; }
            set { _searLinkFormat = value; }
        }

        /// <summary>
        /// 翻页链接格式
        /// </summary>
        public static string PagerFormat
        {
            get { return _pagerFormat; }
            set { _pagerFormat = value; }
        }
    }
}
