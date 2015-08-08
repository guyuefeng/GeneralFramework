using System;

namespace GF.Data.Entity
{
    /// <summary>
    /// 引用通告数据对象
    /// </summary>
    public class TrackbackLogItem
    {
        private int _id = 0;
        private bool _error = false;
        private string _msg = string.Empty;
        private string _url = string.Empty;
        private DateTime _publish = DateTime.Now;

        /// <summary>
        /// 编号
        /// </summary>
        public int ID
        {
            get { return this._id; }
            set { this._id = value; }
        }

        /// <summary>
        /// 是否出错
        /// </summary>
        public bool Error
        {
            get { return this._error; }
            set { this._error = value; }
        }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message
        {
            get { return this._msg; }
            set { this._msg = value; }
        }

        /// <summary>
        /// 发送地址
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
    }
}
