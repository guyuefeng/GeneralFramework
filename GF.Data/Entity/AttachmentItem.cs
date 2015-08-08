using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.Data.Entity
{
    /// <summary>
    /// 附件数据对象
    /// </summary>
    public class AttachmentItem
    {
        private int _id = 0;
        private string _name = string.Empty;
        private string _path = string.Empty;
        private string _type = string.Empty;
        private int _size = 0;
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
        /// 原始文件名
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        /// <summary>
        /// 文件地址
        /// </summary>
        public string Path
        {
            get { return this._path; }
            set { this._path = value; }
        }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string Type
        {
            get { return this._type; }
            set { this._type = value; }
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int Size
        {
            get { return this._size; }
            set { this._size = value; }
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
