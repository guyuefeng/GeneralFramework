using System.Collections.Generic;

namespace GF.Data.Entity
{
    /// <summary>
    /// 数据列表公共类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataList<T> : List<T>
    {
        private int _number = 0;

        /// <summary>
        /// 当前执行任务的总数（包括不可见数量）
        /// </summary>
        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }

        /// <summary>
        /// 把对象列表添加进本实例
        /// </summary>
        /// <param name="list">对象列表</param>
        /// <returns>添加的数量</returns>
        public int Append(DataList<T> list)
        {
            int result = 0;
            foreach (T item in list) { Add(item); result++; }
            return result;
        }
    }
}
