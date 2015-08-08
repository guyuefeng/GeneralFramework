using System.Data;

namespace GF.Data
{
    public class _DbParameter
    {
        private string _name;
        private DbType _type;
        private object _val;

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        public DbType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get { return _val; }
            set { _val = value; }
        }

        /// <summary>
        /// 直接设置并返回一个参数对象
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="type">参数类型</param>
        /// <param name="value">值</param>
        /// <returns>参数对象</returns>
        public _DbParameter Set(string name, DbType type, object value)
        {
            _DbParameter par = new _DbParameter();
            par.Name = name;
            par.Type = type;
            par.Value = value;
            return par;
        }
    }
}
