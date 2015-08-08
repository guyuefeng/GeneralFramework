using System;

namespace GF.Data.Entity
{
    /// <summary>
    /// 用户数据对象
    /// </summary>
    public class UserItem
    {
        //参数列表
        private int _id = 0;
        private string _uid = string.Empty;
        private string _pwd = string.Empty;
        private string _name = string.Empty;
        private string _lastIP = string.Empty;
        private DateTime _lastDT = DateTime.Now;
        private bool _locked = false;

        /// <summary>
        /// 管理员自动编号
        /// </summary>
        public int ID
        {
            get { return this._id; }
            set { this._id = value; }
        }

        /// <summary>
        /// 管理员登录帐号
        /// </summary>
        public string UserID
        {
            get { return this._uid; }
            set { this._uid = value; }
        }

        /// <summary>
        /// 管理员密码明文
        /// </summary>
        public string Password
        {
            get { return this._pwd; }
            set { this._pwd = value; }
        }

        /// <summary>
        /// 管理员称呼
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        /// <summary>
        /// 最后使用IP
        /// </summary>
        public string LastIP
        {
            get { return this._lastIP; }
            set { this._lastIP = value; }
        }

        /// <summary>
        /// 最后使用时间
        /// </summary>
        public DateTime LastTime
        {
            get { return this._lastDT; }
            set { this._lastDT = value; }
        }

        /// <summary>
        /// 是否是锁定状态
        /// </summary>
        public bool Locked
        {
            get { return this._locked; }
            set { this._locked = value; }
        }
    }
}
