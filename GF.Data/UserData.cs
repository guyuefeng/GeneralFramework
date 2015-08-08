using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GF.Data.Entity;

namespace GF.Data
{
    public class UserData
    {
        private _DbHelper conn;

        public UserData(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 添加一个管理员用户
        /// </summary>
        /// <param name="value">用户资料</param>
        public int InsertUser(UserItem value)
        {
            int id = 0;
            _DbParameter[] pars ={
                                    new _DbParameter().Set("@UID", DbType.String, value.UserID),
                                    new _DbParameter().Set("@PWD", DbType.String, value.Password),
                                    new _DbParameter().Set("@Name", DbType.String, value.Name),
                                    new _DbParameter().Set("@LastIP", DbType.String, value.LastIP),
                                    new _DbParameter().Set("@LastTime", DbType.String, value.LastTime.ToString("yyyy-MM-dd HH:mm:ss")),
                                    new _DbParameter().Set("@Locked", DbType.Int32, value.Locked ? 1 : 0)
                                };
            conn.ExecuteNonQuery("INSERT INTO [User] ([UserID], [Password], [Name], [LastIP], [LastTime], [Locked]) VALUES (@UID, @PWD, @Name, @LastIP, @LastTime, @Locked)", pars);
            id = Convert.ToInt32(conn.ExecuteNewField("[User]", null, null));
            return id;
        }

        /// <summary>
        /// 修改一个管理员用户
        /// </summary>
        /// <param name="value">用户资料</param>
        public int UpdateUser(UserItem value)
        {
            _DbParameter[] pars ={
                                    new _DbParameter().Set("@UID", DbType.String, value.UserID),
                                    new _DbParameter().Set("@PWD", DbType.String, value.Password),
                                    new _DbParameter().Set("@Name", DbType.String, value.Name),
                                    new _DbParameter().Set("@LastIP", DbType.String, value.LastIP),
                                    new _DbParameter().Set("@LastTime", DbType.String, value.LastTime.ToString("yyyy-MM-dd HH:mm:ss")),
                                    new _DbParameter().Set("@Locked", DbType.Int32, value.Locked ? 1 : 0),
                                    new _DbParameter().Set("@ID", DbType.Int32, value.ID)
                                };
            conn.ExecuteNonQuery("UPDATE [User] SET [UserID] = @UID, [Password] = @PWD, [Name] = @Name, [LastIP] = @LastIP, [LastTime] = @LastTime, [Locked] = @Locked WHERE [ID] = @ID", pars);
            return value.ID;
        }

        /// <summary>
        /// 更新用户的最后使用时间
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="ip">IP</param>
        /// <param name="time">时间</param>
        public int UpdateUserIpAndTime(int id, string ip, DateTime time)
        {
            _DbParameter[] pars ={
                                    new _DbParameter().Set("@IP", DbType.String, ip),
                                    new _DbParameter().Set("@DT", DbType.String, time.ToString("yyyy-MM-dd HH:mm:ss")),
                                    new _DbParameter().Set("@ID", DbType.Int32, id)
                                };
            conn.ExecuteNonQuery("UPDATE [User] SET [LastIP] = @IP, [LastTime] = @DT WHERE [ID] = @ID", pars);
            return id;
        }

        /// <summary>
        /// 更新用户锁定状态
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="locked">是否锁定</param>
        public int UpdateUserLocked(int id, bool locked)
        {
            _DbParameter[] pars ={
                                    new _DbParameter().Set("@Locked", DbType.Int32, locked ? 1 : 0),
                                    new _DbParameter().Set("@ID", DbType.Int32, id)
                                };
            conn.ExecuteNonQuery("UPDATE [User] SET [Locked] = @Locked WHERE [ID] = @ID", pars);
            return id;
        }

        /// <summary>
        /// 验证管理员信息
        /// </summary>
        /// <param name="userID">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="selAll">是否筛选所有用户</param>
        public UserItem CheckUser(string userID, string pwd, bool selAll)
        {
            UserItem item = new UserItem();
            _DbParameter[] pars ={
                                    new _DbParameter().Set("@UID", DbType.String, userID),
                                    new _DbParameter().Set("@PWD", DbType.String, pwd)
                                };
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[User]", "[ID], [UserID], [Password], [Name], [LastIP], [LastTime], [Locked]", "[UserID] = @UID AND [Password] = @PWD" + (selAll ? string.Empty : " AND [Locked] = 0"), null, null, 1, 1, pars, ref rows))
            {
                while (reader.Read())
                {
                    item.ID = reader.GetInt32(0);
                    item.UserID = reader.GetString(1);
                    item.Password = reader.GetString(2);
                    item.Name = reader.GetString(3);
                    item.LastIP = reader.GetString(4);
                    item.LastTime = reader.GetDateTime(5);
                    item.Locked = reader.GetInt32(6) == 0 ? false : true;
                }
            }
            return item;
        }

        /// <summary>
        /// 删除一个用户
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <returns>返回被删除的用户编号</returns>
        public int DeleteUser(int id)
        {
            conn.ExecuteNonQuery(string.Format("DELETE FROM [User] WHERE [ID] = {0}", id));
            return id;
        }

        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <param name="id">用户编号</param>
        public UserItem GetUser(int id)
        {
            UserItem item = new UserItem();
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[User]", "[ID], [UserID], [Password], [Name], [LastIP], [LastTime], [Locked]", string.Format("[ID] = {0}", id), null, null, 1, 1, ref rows))
            {
                while (reader.Read())
                {
                    item.ID = reader.GetInt32(0);
                    item.UserID = reader.GetString(1);
                    item.Password = reader.GetString(2);
                    item.Name = reader.GetString(3);
                    item.LastIP = reader.GetString(4);
                    item.LastTime = reader.GetDateTime(5);
                    item.Locked = reader.GetInt32(6) == 0 ? false : true;
                }
            }
            return item;
        }

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="userId">用户名</param>
        /// <param name="ourId">例外的ID，为0则筛选所有数据</param>
        /// <returns>返回是否存在的布尔型数据</returns>
        public bool ExistsUserID(string userId, int ourId)
        {
            bool result = false;
            _DbParameter[] pars ={
                                    new _DbParameter().Set("@UserID", DbType.String, userId),
                                    new _DbParameter().Set("@OurID", DbType.Int32, ourId)
                                };
            if (Convert.ToInt32(conn.ExecuteScalar(string.Format("SELECT COUNT([ID]) FROM [User] WHERE [UserID] = @UserID{0}", (ourId == 0 ? string.Empty : " AND [ID] <> @OurID")), pars)) > 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 选择用户
        /// </summary>
        /// <param name="intCurPage">当前页码</param>
        /// <param name="btePerPage">每页数量</param>
        /// <returns>返回页面数据列表</returns>
        public DataList<UserItem> SelectUser(int intCurPage, int btePerPage)
        {
            DataList<UserItem> list = new DataList<UserItem>();
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[User]", "[ID], [UserID], [Password], [Name], [LastIP], [LastTime], [Locked]", null, null, null, intCurPage, btePerPage, ref rows))
            {
                while (reader.Read())
                {
                    UserItem item = new UserItem();
                    item.ID = reader.GetInt32(0);
                    item.UserID = reader.GetString(1);
                    item.Password = reader.GetString(2);
                    item.Name = reader.GetString(3);
                    item.LastIP = reader.GetString(4);
                    item.LastTime = reader.GetDateTime(5);
                    item.Locked = reader.GetInt32(6) == 0 ? false : true;
                    list.Add(item);
                }
            }
            list.Number = rows;
            return list;
        }
    }
}
