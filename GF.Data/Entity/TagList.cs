using System.Collections;

namespace GF.Data.Entity
{
    /// <summary>
    /// 标签列表
    /// </summary>
    public class TagList : CollectionBase
    {
        // Fields
        private int _number = 0;

        /// <summary>
        /// 向当前列表增加标签对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add(TagItem value)
        {
            return base.List.Add(value);
        }

        /// <summary>
        /// 向列表里增加标签对象
        /// </summary>
        /// <param name="list">列表字符串，以半角逗号区分</param>
        /// <returns>返回零</returns>
        public int AddString(string list)
        {
            if (!string.IsNullOrEmpty(list))
            {
                string[] strArray = list.Split(new char[] { ',', ' ' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(strArray[i]))
                    {
                        TagItem item = new TagItem();
                        item.Key = strArray[i].Trim();
                        base.List.Add(item);
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取标签列表的文本串
        /// </summary>
        public string GetString
        {
            get
            {
                string str = string.Empty;
                for (int i = 0; i < base.List.Count; i++)
                {
                    str = str + ((TagItem)base.List[i]).Key + ",";
                }
                if (!string.IsNullOrEmpty(str))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                return str;
            }
        }

        /// <summary>
        /// 获取某索引下的标签对象
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>标签对象</returns>
        public TagItem this[int index]
        {
            get
            {
                return (TagItem)base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        /// <summary>
        /// 目前列表的标签总数
        /// </summary>
        public int Number
        {
            get
            {
                return this._number;
            }
            set
            {
                this._number = value;
            }
        }
    }
}
