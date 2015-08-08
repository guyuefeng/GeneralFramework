using System;

namespace GF.Data.Entity
{
    /// <summary>
    /// 评论数据对象
    /// </summary>
    public class CommentItem
    {
        //参数列表
        private int _id = 0;
        private int _postId = 0;
        private string _author = string.Empty;
        private string _title = string.Empty;
        private string _mail = string.Empty;
        private string _url = string.Empty;
        private string _content = string.Empty;
        private string _reply = string.Empty;
        private bool _isTb = false;
        private DateTime _publish = DateTime.Now;
        private bool _verify = false;

        /// <summary>
        /// 编号
        /// </summary>
        public int ID
        {
            get { return this._id; }
            set { this._id = value; }
        }

        /// <summary>
        /// 内容编号
        /// </summary>
        public int PostID
        {
            get { return this._postId; }
            set { this._postId = value; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return this._title; }
            set { this._title = value; }
        }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            get { return this._content; }
            set { this._content = value; }
        }

        /// <summary>
        /// 回复
        /// </summary>
        public string Reply
        {
            get { return this._reply; }
            set { this._reply = value; }
        }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author
        {
            get { return this._author; }
            set { this._author = value; }
        }

        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Mail
        {
            get { return this._mail; }
            set { this._mail = value; }
        }

        /// <summary>
        /// 个人网站地址
        /// </summary>
        public string URL
        {
            get { return this._url; }
            set { this._url = value; }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Publish
        {
            get { return this._publish; }
            set { this._publish = value; }
        }

        /// <summary>
        /// 是否是引用通告
        /// </summary>
        public bool Trackback
        {
            get { return this._isTb; }
            set { this._isTb = value; }
        }

        /// <summary>
        /// 是否审核
        /// </summary>
        public bool Verify
        {
            get { return this._verify; }
            set { this._verify = value; }
        }
    }
}
