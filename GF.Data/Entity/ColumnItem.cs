using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.Data.Entity
{
 /// <summary>
    /// 分类数据对象
    /// </summary>
    public class ColumnItem
    {
        //参数列表
        private int _id = 0;
        private string _local = string.Empty;
        private string _name = string.Empty;
        private string _intro = string.Empty;
        private string _target = string.Empty;
        private int _postCount = 0;
        private int _sorting = 0;
        private bool _show = false;
        private bool _nav = false;
        private bool _jump = false;
        private string _jumpUrl = string.Empty;
        private string _listTpl = string.Empty;
        private string _viewTpl = string.Empty;
        private int _pageSize = 0;

        /// <summary>
        /// 编号
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
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
        /// 分类名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro
        {
            get { return _intro; }
            set { _intro = value; }
        }

        /// <summary>
        /// 打开方式
        /// </summary>
        public string Target
        {
            get { return _target; }
            set { _target = value; }
        }

        /// <summary>
        /// 文章数量
        /// </summary>
        public int PostCount
        {
            get { return _postCount; }
            set { _postCount = value; }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sorting
        {
            get { return _sorting; }
            set { _sorting = value; }
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
        /// 是否显示在主导航
        /// </summary>
        public bool Nav
        {
            get { return _nav; }
            set { _nav = value; }
        }

        /// <summary>
        /// 是否是跳转链接
        /// </summary>
        public bool Jump
        {
            get { return _jump; }
            set { _jump = value; }
        }

        /// <summary>
        /// 跳转链接（设置为跳转链接时有效）
        /// </summary>
        public string JumpUrl
        {
            get { return _jumpUrl; }
            set { _jumpUrl = value; }
        }

        /// <summary>
        /// 列表模板文件名
        /// </summary>
        public string ListTemplate
        {
            get { return _listTpl; }
            set { _listTpl = value; }
        }

        /// <summary>
        /// 展示模板文件名
        /// </summary>
        public string ViewTemplate
        {
            get { return _viewTpl; }
            set { _viewTpl = value; }
        }

        /// <summary>
        /// 列表显示数量
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
    }
}
