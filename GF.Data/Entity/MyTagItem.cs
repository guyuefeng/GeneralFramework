using System;

namespace GF.Data.Entity
{
    /// <summary>
    /// 自定义标签数据对象
    /// </summary>
    public class MyTagItem
    {
        //参数列表
        private int _id = 0;
        private string _key = string.Empty;
        private string _intro = string.Empty;
        private string _code = string.Empty;
        private DateTime _publish = DateTime.Now;
        private int _priority = 0;
        private bool _show = false;

        /// <summary>
        /// 编号
        /// </summary>
        public int ID
        {
            get { return this._id; }
            set { this._id = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Key
        {
            get { return this._key; }
            set { this._key = value; }
        }

        /// <summary>
        /// 介绍说明
        /// </summary>
        public string Intro
        {
            get { return this._intro; }
            set { this._intro = value; }
        }

        /// <summary>
        /// 代码内容
        /// </summary>
        public string Code
        {
            get { return this._code; }
            set { this._code = value; }
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
        /// 优先权
        /// </summary>
        public int Priority
        {
            get { return this._priority; }
            set { this._priority = value; }
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Show
        {
            get { return this._show; }
            set { this._show = value; }
        }
    }
}
