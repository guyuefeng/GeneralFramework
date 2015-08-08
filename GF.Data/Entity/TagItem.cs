namespace GF.Data.Entity
{
    /// <summary>
    /// 标签数据对象
    /// </summary>
    public class TagItem
    {
        //参数列表
        private int _id = 0;
        private string _key = string.Empty;
        private int _postCount = 0;

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
        /// 文章数量
        /// </summary>
        public int PostCount
        {
            get { return this._postCount; }
            set { this._postCount = value; }
        }
    }
}
