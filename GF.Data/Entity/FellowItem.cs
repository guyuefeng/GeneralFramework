using System;

namespace GF.Data.Entity
{
    /// <summary>
    /// 友情链接数据对象
    /// </summary>
    public class FellowItem
    {
        //参数列表
        private int _id = 0;
        private string _name = string.Empty;
        private string _explain = string.Empty;
        private string _url = string.Empty;
        private string _logo = string.Empty;
        private string _style = string.Empty;
        private int _sorting = 0;
        private bool _home = false;
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
        /// 网站名称
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        /// <summary>
        /// 网站介绍
        /// </summary>
        public string Explain
        {
            get { return this._explain; }
            set { this._explain = value; }
        }

        /// <summary>
        /// 网站地址
        /// </summary>
        public string URL
        {
            get { return this._url; }
            set { this._url = value; }
        }

        /// <summary>
        /// 标志地址
        /// </summary>
        public string Logo
        {
            get { return this._logo; }
            set { this._logo = value; }
        }

        /// <summary>
        /// 字体样式
        /// </summary>
        public string Style
        {
            get { return this._style; }
            set { this._style = value; }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sorting
        {
            get { return this._sorting; }
            set { this._sorting = value; }
        }

        /// <summary>
        /// 是否在首页显示
        /// </summary>
        public bool Home
        {
            get { return this._home; }
            set { this._home = value; }
        }

        /// <summary>
        /// 是否在前台显示
        /// </summary>
        public bool Show
        {
            get { return this._show; }
            set { this._show = value; }
        }
    }
}
