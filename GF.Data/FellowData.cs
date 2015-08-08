using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GF.Data.Entity;


namespace GF.Data
{
    public class FellowData
    {
        private _DbHelper conn;

        public FellowData(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 添加一个链接
        /// </summary>
        /// <param name="value">链接资料</param>
        public int InsertFellow(FellowItem value)
        {
            int id = 0;
            _DbParameter[] pars ={
                                   new _DbParameter().Set("@Name", DbType.String, value.Name),
                                   new _DbParameter().Set("@URL", DbType.String, value.URL),
                                   new _DbParameter().Set("@Explain", DbType.String, value.Explain),
                                   new _DbParameter().Set("@Logo", DbType.String, value.Logo),
                                   new _DbParameter().Set("@Style", DbType.String, value.Style),
                                   new _DbParameter().Set("@Sorting", DbType.Int32, value.Sorting),
                                   new _DbParameter().Set("@Home", DbType.Int32, value.Home ? 1 : 0),
                                   new _DbParameter().Set("@Show", DbType.Int32, value.Show ? 1 : 0)
                               };
            conn.ExecuteNonQuery("INSERT INTO [Fellow] ([Name], [URL], [Explain], [Logo], [Style], [Sorting], [Home], [Show]) VALUES (@Name, @URL, @Explain, @Logo, @Style, @Sorting, @Home, @Show)", pars);
            id = Convert.ToInt32(conn.ExecuteNewField("[Fellow]", null, null));
            return id;
        }

        /// <summary>
        /// 修改一个链接
        /// </summary>
        /// <param name="value">链接资料</param>
        public int UpdateFellow(FellowItem value)
        {
            int id = 0;
            _DbParameter[] pars ={
                                   new _DbParameter().Set("@Name", DbType.String, value.Name),
                                   new _DbParameter().Set("@URL", DbType.String, value.URL),
                                   new _DbParameter().Set("@Explain", DbType.String, value.Explain),
                                   new _DbParameter().Set("@Logo", DbType.String, value.Logo),
                                   new _DbParameter().Set("@Style", DbType.String, value.Style),
                                   new _DbParameter().Set("@Sorting", DbType.Int32, value.Sorting),
                                   new _DbParameter().Set("@Home", DbType.Int32, value.Home ? 1 : 0),
                                   new _DbParameter().Set("@Show", DbType.Int32, value.Show ? 1 : 0),
                                   new _DbParameter().Set("@ID", DbType.Int32, value.ID)
                               };
            conn.ExecuteNonQuery("UPDATE [Fellow] SET [Name] = @Name, [URL] = @URL, [Explain] = @Explain, [Logo] = @Logo, [Style] = @Style, [Sorting] = @Sorting, [Home] = @Home, [Show] = @Show WHERE [ID] = @ID", pars);
            id = value.ID;
            return id;
        }

        /// <summary>
        /// 选择链接
        /// </summary>
        /// <param name="intCurPage">当前页码</param>
        /// <param name="btePerPage">分页大小</param>
        /// <param name="home">是否是首页链接</param>
        /// <param name="selAll">是否选择所有数据</param>
        /// <returns>返回页面数据列表</returns>
        public DataList<FellowItem> SelectFellow(int intCurPage, int btePerPage, bool home, bool selAll)
        {
            DataList<FellowItem> list = new DataList<FellowItem>();
            string sqlWhere = string.Empty;
            if (home) { sqlWhere += " AND [Home] <> 0"; }
            if (!selAll) { sqlWhere += " AND [Show] <> 0"; }
            if (!string.IsNullOrEmpty(sqlWhere)) { sqlWhere = "[ID] > 0" + sqlWhere; }
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[Fellow]", "[ID], [Name], [URL], [Explain], [Logo], [Style], [Sorting], [Home], [Show]", sqlWhere, "[Sorting]", "ASC", intCurPage, btePerPage, ref rows))
            {
                while (reader.Read())
                {
                    FellowItem item = new FellowItem();
                    item.ID = reader.GetInt32(0);
                    item.Name = reader.GetString(1);
                    item.URL = reader.GetString(2);
                    item.Explain = reader.GetString(3);
                    item.Logo = reader.GetString(4);
                    item.Style = reader.GetString(5);
                    item.Sorting = reader.GetInt32(6);
                    item.Home = reader.GetInt32(7) == 0 ? false : true;
                    item.Show = reader.GetInt32(8) == 0 ? false : true;
                    list.Add(item);
                }
            }
            list.Number = rows;
            return list;
        }

        /// <summary>
        /// 删除一个链接
        /// </summary>
        /// <param name="id">链接编号</param>
        /// <returns>返回被删除的链接编号</returns>
        public int DeleteFellow(int id)
        {
            conn.BeginTransaction();
            try
            {
                conn.ExecuteNonQuery(string.Format("DELETE FROM [Fellow] WHERE [ID] = {0}", id));
                conn.Commit();
            }
            catch (Exception err) { conn.Rollback(); throw err; }
            return id;
        }

        /// <summary>
        /// 更新用户锁定状态
        /// </summary>
        /// <param name="id">链接编号</param>
        /// <param name="name">链接名称</param>
        /// <param name="url">地址</param>
        /// <param name="home">是否是首页链接</param>
        /// <param name="show">是否显示</param>
        /// <param name="sorting">排序</param>
        public int UpdateFellowSome(int id, string name, string url, bool home, bool show, int sorting)
        {
            _DbParameter[] pars ={
                                   new _DbParameter().Set("@Name", DbType.String, name),
                                   new _DbParameter().Set("@URL", DbType.String, url),
                                   new _DbParameter().Set("@Home", DbType.Int32, home ? 1 : 0),
                                   new _DbParameter().Set("@Show", DbType.Int32, show ? 1 : 0),
                                   new _DbParameter().Set("@Sorting", DbType.Int32, sorting),
                                   new _DbParameter().Set("@ID", DbType.Int32, id)
                               };
            conn.ExecuteNonQuery("UPDATE [Fellow] SET [Name] = @Name, [URL] = @URL, [Home] = @Home, [Show] = @Show, [Sorting] = @Sorting WHERE [ID] = @ID", pars);
            return id;
        }

        /// <summary>
        /// 获取链接信息
        /// </summary>
        /// <param name="id">链接编号</param>
        public FellowItem GetFellow(int id)
        {
            FellowItem item = new FellowItem();
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[Fellow]", "[ID], [Name], [URL], [Explain], [Logo], [Style], [Sorting], [Home], [Show]", string.Format("[ID] = {0}", id), null, null, 1, 1, ref rows))
            {
                while (reader.Read())
                {
                    item.ID = reader.GetInt32(0);
                    item.Name = reader.GetString(1);
                    item.URL = reader.GetString(2);
                    item.Explain = reader.GetString(3);
                    item.Logo = reader.GetString(4);
                    item.Style = reader.GetString(5);
                    item.Sorting = reader.GetInt32(6);
                    item.Home = reader.GetInt32(7) == 0 ? false : true;
                    item.Show = reader.GetInt32(8) == 0 ? false : true;
                }
            }
            return item;
        }

        /// <summary>
        /// 检查链接地址是否存在
        /// </summary>
        /// <param name="url">链接地址</param>
        /// <param name="ourId">例外的ID，为0则筛选所有数据</param>
        /// <returns>返回是否存在的布尔型数据</returns>
        public bool ExistsFellowUrl(string url, int ourId)
        {
            bool result = false;
            _DbParameter[] pars ={
                                   new _DbParameter().Set("@URL", DbType.String, url),
                                   new _DbParameter().Set("@OurID", DbType.Int32, ourId)
                               };
            if (Convert.ToInt32(conn.ExecuteScalar(string.Format("SELECT COUNT([ID]) FROM [Fellow] WHERE [URL] = @URL{0}", (ourId == 0 ? string.Empty : " AND [ID] <> @OurID")), pars)) > 0)
            {
                result = true;
            }
            return result;
        }
    }
}
