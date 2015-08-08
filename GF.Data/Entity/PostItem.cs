using System;

namespace GF.Data.Entity
{
    /// <summary>
    /// 文章数据对象
    /// </summary>
    public class PostItem
    {
        //参数列表
        private int _id = 0;
        private int _columnId = 0;
        private string _tags = string.Empty;
        private string _local = string.Empty;
        private string _title = string.Empty;
        private string _explain = string.Empty;
        private string _content = string.Empty;
        private string _author = string.Empty;
        private DateTime _publish = DateTime.Now;
        private string _pwd = string.Empty;
        private bool _fine = false;
        private int _vote = 0;
        private int _reader = 0;
        private int _postCount = 0;
        private bool _show = false;
        private bool _switchCmt = false;
        private bool _switchTb = false;
        private bool _avCmt = false;
        private bool _avTb = false;
        private string _atts = string.Empty;

        /// <summary>
        /// 编号
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 分类编号
        /// </summary>
        public int ColumnID
        {
            get { return _columnId; }
            set { _columnId = value; }
        }

        /// <summary>
        /// 标签（例：日记,游戏）
        /// </summary>
        public string Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }

        /// <summary>
        /// 永久链接文件名
        /// </summary>
        public string Local
        {
            get { return _local; }
            set { _local = value; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// 阐述
        /// </summary>
        public string Explain
        {
            get { return _explain; }
            set { _explain = value; }
        }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Publish
        {
            get { return _publish; }
            set { _publish = value; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return _pwd; }
            set { _pwd = value; }
        }

        /// <summary>
        /// 是否是好文章
        /// </summary>
        public bool Fine
        {
            get { return _fine; }
            set { _fine = value; }
        }

        /// <summary>
        /// 投票数量
        /// </summary>
        public int Vote
        {
            get { return _vote; }
            set { _vote = value; }
        }

        /// <summary>
        /// 阅读次数
        /// </summary>
        public int Reader
        {
            get { return _reader; }
            set { _reader = value; }
        }

        /// <summary>
        /// 评论数量
        /// </summary>
        public int PostCount
        {
            get { return _postCount; }
            set { _postCount = value; }
        }

        /// <summary>
        /// 是否在前台显示
        /// </summary>
        public bool Show
        {
            get { return _show; }
            set { _show = value; }
        }

        /// <summary>
        /// 是否允许发表评论
        /// </summary>
        public bool SwitchComment
        {
            get { return _switchCmt; }
            set { _switchCmt = value; }
        }

        /// <summary>
        /// 是否允许发表引用通告
        /// </summary>
        public bool SwitchTrackback
        {
            get { return _switchTb; }
            set { _switchTb = value; }
        }

        /// <summary>
        /// 是否自动审核评论
        /// </summary>
        public bool AutoVerifyComment
        {
            get { return _avCmt; }
            set { _avCmt = value; }
        }

        /// <summary>
        /// 是否自动审核引用通告
        /// </summary>
        public bool AutoVerifyTrackback
        {
            get { return _avTb; }
            set { _avTb = value; }
        }

        /// <summary>
        /// 附件列表（例：108,668）
        /// </summary>
        public string Attachments
        {
            get { return _atts; }
            set { _atts = value; }
        }
    }
}
