namespace GF.Data.Entity
{
    /// <summary>
    /// 配置参数对象
    /// </summary>
    public class SettingParameterItem
    {
        private int _watermark = 0;
        private int _articleNum = 0;
        private int _commentNum = 0;
        private int _appendFineArticleNum = 0;
        private int _appendRandomArticleNum = 0;
        private int _appendHotArticleNum = 0;
        private int _appendHotTagNum = 0;
        private int _appendNewCommentNum = 0;
        private int _rssNum = 0;
        private int _rssMode = 0;
        private string _wmPath = string.Empty;
        private string _key = string.Empty;

        /// <summary>
        /// 水印位置
        /// </summary>
        public int WatermarkSeat
        {
            get { return _watermark; }
            set { _watermark = value; }
        }

        /// <summary>
        /// 水印相对位置
        /// </summary>
        public string WatermarkPath
        {
            get { return _wmPath; }
            set { _wmPath = value; }
        }

        /// <summary>
        /// 每页文章数量
        /// </summary>
        public int ArticleNum
        {
            get { return _articleNum; }
            set { _articleNum = value; }
        }

        /// <summary>
        /// 每页评论数量
        /// </summary>
        public int CommentNum
        {
            get { return _commentNum; }
            set { _commentNum = value; }
        }

        /// <summary>
        /// 附加精品文章数量
        /// </summary>
        public int AppendFineArticleNum
        {
            get { return _appendFineArticleNum; }
            set { _appendFineArticleNum = value; }
        }

        /// <summary>
        /// 附加随机文章数量
        /// </summary>
        public int AppendRandomArticleNum
        {
            get { return _appendRandomArticleNum; }
            set { _appendRandomArticleNum = value; }
        }

        /// <summary>
        /// 附加热门文章数量
        /// </summary>
        public int AppendHotArticleNum
        {
            get { return _appendHotArticleNum; }
            set { _appendHotArticleNum = value; }
        }

        /// <summary>
        /// 附加热门标签数量
        /// </summary>
        public int AppendHotTagNum
        {
            get { return _appendHotTagNum; }
            set { _appendHotTagNum = value; }
        }

        /// <summary>
        /// 附加最新评论数量
        /// </summary>
        public int AppendNewCommentNum
        {
            get { return _appendNewCommentNum; }
            set { _appendNewCommentNum = value; }
        }

        /// <summary>
        /// RSS显示数量
        /// </summary>
        public int RssNum
        {
            get { return _rssNum; }
            set { _rssNum = value; }
        }

        /// <summary>
        /// RSS显示模式
        /// </summary>
        public int RssMode
        {
            get { return _rssMode; }
            set { _rssMode = value; }
        }

        /// <summary>
        /// 授权密匙
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }
    }
}
