using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.Core
{
    /// <summary>
    ///  sealed 修饰符时，此修饰符会阻止其他类从该类继承
    ///  缓存表对象
    /// </summary>
    public sealed class SiteCache
    {
        /// <summary>
        /// 配置表
        /// </summary>
        public const string Setting = "Setting";
        /// <summary>
        /// 导航
        /// </summary>
        public const string Navigation = "Navigation";

        /// <summary>
        /// 分类
        /// </summary>
        public const string Column = "Column";

        /// <summary>
        /// 随机文章
        /// </summary>
        public const string RandomPost = "RandomPost";

        /// <summary>
        /// 热门文章
        /// </summary>
        public const string HotPost = "HotPost";

        /// <summary>
        /// 推荐文章
        /// </summary>
        public const string FinePost = "FinePost";

        /// <summary>
        /// 最新评论
        /// </summary>
        public const string NewComment = "NewComment";

        /// <summary>
        /// 热门标签
        /// </summary>
        public const string HotTag = "HotTag";

        /// <summary>
        /// 友情链接
        /// </summary>
        public const string Fellow = "Fellow";

        /// <summary>
        /// 单页
        /// </summary>
        public const string Page = "Page";

        /// <summary>
        /// 自定义标签
        /// </summary>
        public const string MyTag = "MyTag";

        /// <summary>
        /// 文章列表
        /// </summary>
        public const string PostsListFormat = "Posts-{0}-{1}-{2}-{3}";

        /// <summary>
        /// 评论列表，{0}为内容编号，{1}为页数
        /// </summary>
        public const string CommentsListFormat = "Comments-{0}-{1}";

        /// <summary>
        /// 内容页，{0}为编号
        /// </summary>
        public const string PostFormat = "Post-{0}";

        /// <summary>
        /// 栏目数据，{0}为编号
        /// </summary>
        public const string ColumnFormat = "Column-{0}";

    }

}
