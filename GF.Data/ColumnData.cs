using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GF.Data.Entity;
using System.Data;

namespace GF.Data
{
    public class ColumnData
    {
        private _DbHelper conn;

        public ColumnData(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 添加一个分类
        /// </summary>
        /// <param name="value">分类数据</param>
        /// <returns>返回新增的分类的ID</returns>
        public int InsertColumn(ColumnItem value)
        {
            int id = 0;
            _DbParameter[] pars ={
                                    new _DbParameter().Set("@Name", DbType.String, value.Name),
                                    new _DbParameter().Set("@Local", DbType.String, value.Local),
                                    new _DbParameter().Set("@Intro", DbType.String, value.Intro),
                                    new _DbParameter().Set("@Sorting", DbType.Int32, value.Sorting),
                                    new _DbParameter().Set("@Show", DbType.Int32, value.Show ? 1 : 0),
                                    new _DbParameter().Set("@Nav", DbType.Int32, value.Nav ? 1 : 0),
                                    new _DbParameter().Set("@Jump", DbType.Int32, value.Jump ? 1 : 0),
                                    new _DbParameter().Set("@JumpUrl", DbType.String, value.JumpUrl),
                                    new _DbParameter().Set("@Target", DbType.String, value.Target),
                                    new _DbParameter().Set("@ListTemplate", DbType.String, value.ListTemplate),
                                    new _DbParameter().Set("@ViewTemplate", DbType.String, value.ViewTemplate),
                                    new _DbParameter().Set("@PageSize", DbType.Int32, value.PageSize)
                                };
            conn.ExecuteNonQuery("INSERT INTO [Column] ([Name], [Local], [Intro], [Sorting], [Show], [IsNav], [IsJump], [JumpUrl], [Target], [ListTpl], [ViewTpl], [PageSize]) VALUES (@Name, @Local, @Intro, @Sorting, @Show, @Nav, @Jump, @JumpUrl, @Target, @ListTemplate, @ViewTemplate, @PageSize)", pars);
            id = Convert.ToInt32(conn.ExecuteNewField("[Column]", null, null));
            if (string.IsNullOrEmpty(value.Local))
            {
                conn.ExecuteNonQuery(string.Format("UPDATE [Column] SET [Local] = [ID] WHERE [ID] = {0}", id));
            }
            return id;
        }

        /// <summary>
        /// 修改一个分类
        /// </summary>
        /// <param name="value">分类数据</param>
        /// <returns>返回被修改的分类的ID</returns>
        public int UpdateColumn(ColumnItem value)
        {
            int id = 0;
            _DbParameter[] pars ={
                                    new _DbParameter().Set("@Name", DbType.String, value.Name),
                                    new _DbParameter().Set("@Local", DbType.String, value.Local),
                                    new _DbParameter().Set("@Intro", DbType.String, value.Intro),
                                    new _DbParameter().Set("@Sorting", DbType.Int32, value.Sorting),
                                    new _DbParameter().Set("@Show", DbType.Int32, value.Show ? 1 : 0),
                                    new _DbParameter().Set("@Nav", DbType.Int32, value.Nav ? 1 : 0),
                                    new _DbParameter().Set("@Jump", DbType.Int32, value.Jump ? 1 : 0),
                                    new _DbParameter().Set("@JumpUrl", DbType.String, value.JumpUrl),
                                    new _DbParameter().Set("@Target", DbType.String, value.Target),
                                    new _DbParameter().Set("@ListTemplate", DbType.String, value.ListTemplate),
                                    new _DbParameter().Set("@ViewTemplate", DbType.String, value.ViewTemplate),
                                    new _DbParameter().Set("@PageSize", DbType.Int32, value.PageSize),
                                    new _DbParameter().Set("@ID", DbType.Int32, value.ID)
                                };
            conn.ExecuteNonQuery("UPDATE [Column] SET [Name] = @Name, [Local] = @Local, [Intro] = @Intro, [Sorting] = @Sorting, [Show] = @Show, [IsNav] = @Nav, [IsJump] = @Jump, [JumpUrl] = @JumpUrl, [Target] = @Target, [ListTpl] = @ListTemplate, [ViewTpl] = @ViewTemplate, [PageSize] = @PageSize WHERE [ID] = @ID", pars);
            id = value.ID;
            if (string.IsNullOrEmpty(value.Local))
            {
                conn.ExecuteNonQuery(string.Format("UPDATE [Column] SET [Local] = [ID] WHERE [ID] = {0}", id));
            }
            return id;
        }

        /// <summary>
        /// 修改一个分类
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="name">名称</param>
        /// <param name="sorting">排序</param>
        /// <param name="isnav">是否是导航</param>
        /// <param name="show">是否显示</param>
        /// <returns>被修改的分类编号</returns>
        public int UpdateColumnSome(int id, string name, int sorting, bool isnav, bool show)
        {
            _DbParameter[] pars ={
                                    new _DbParameter().Set("@Name", DbType.String, name),
                                    new _DbParameter().Set("@Sorting", DbType.Int32, sorting),
                                    new _DbParameter().Set("@Show", DbType.Int32, show ? 1 : 0),
                                    new _DbParameter().Set("@Nav", DbType.Int32, isnav ? 1 : 0),
                                    new _DbParameter().Set("@ID", DbType.Int32, id)
                                };
            conn.ExecuteNonQuery("UPDATE [Column] SET [Name] = @Name, [Sorting] = @Sorting, [Show] = @Show, [IsNav] = @Nav WHERE [ID] = @ID", pars);
            return id;
        }

        /// <summary>
        /// 删除一个分类
        /// </summary>
        /// <param name="id">分类编号</param>
        /// <returns>返回被删除的分类编号</returns>
        public int DeleteColumn(int id)
        {
            using (IDataReader reader = conn.ExecuteReader(string.Format("SELECT [ID] FROM [Post] WHERE [ColumnID] = {0}", id)))
            {
                while (reader.Read())
                {
                    int artId = reader.GetInt32(0);
                    conn.ExecuteNonQuery(string.Format("DELETE FROM [Comment] WHERE [PostID] = {0}", artId));
                }
            }
            conn.ExecuteNonQuery(string.Format("DELETE FROM [Post] WHERE [ColumnID] = {0}", id));
            conn.ExecuteNonQuery(string.Format("DELETE FROM [Column] WHERE [ID] = {0}", id));
            return id;
        }

        /// <summary>
        /// 重新统计分类下的文章数量
        /// </summary>
        /// <returns>返回处理的条数</returns>
        public int CountColumnPost()
        {
            int result = 0;
            using (IDataReader reader = conn.ExecuteReader("SELECT [ID] FROM [Column]"))
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int count = Convert.ToInt32(conn.ExecuteScalar(string.Format("SELECT COUNT([ID]) FROM [Post] WHERE [ColumnID] = {0}", id)));
                    conn.ExecuteNonQuery(string.Format("UPDATE [Column] SET [PostCount] = {1} WHERE [ID] = {0}", id, count));
                    result++;
                }
            }
            return result;
        }

        /// <summary>
        /// 统计分类下的文章数
        /// </summary>
        /// <param name="id">分类编号</param>
        public void CountColumnPost(int id)
        {
            int count = Convert.ToInt32(conn.ExecuteScalar(string.Format("SELECT COUNT([ID]) FROM [Post] WHERE [ColumnID] = {0}", id)));
            conn.ExecuteNonQuery(string.Format("UPDATE [Column] SET [PostCount] = {1} WHERE [ID] = {0}", id, count));
        }

        /// <summary>
        /// 检查永久链接是否存在
        /// </summary>
        /// <param name="local">永久链接地址</param>
        /// <param name="ourId">例外的ID，为0则筛选所有数据</param>
        /// <returns>返回是否存在的布尔型数据</returns>
        public bool ExistsLocal(string local, int ourId)
        {
            bool result = false;
            _DbParameter[] pars ={
                                    new _DbParameter().Set("@Local", DbType.String, local),
                                    new _DbParameter().Set("@OurID", DbType.Int32, ourId)
                                };
            if (Convert.ToInt32(conn.ExecuteScalar(string.Format("SELECT COUNT([ID]) FROM [Column] WHERE [Local] = @Local{0};", (ourId == 0 ? string.Empty : " AND [ID] <> @OurID")), pars)) > 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 筛选分类
        /// </summary>
        /// <param name="nav">是否是导航，-1：默认，0：不是，1：是</param>
        /// <param name="jump">是否是外链,-1：默认，0：不是，1：是</param>
        /// <param name="selAll">是否筛选所有数据</param>
        /// <returns>返回分类数据列表</returns>
        public DataList<ColumnItem> SelectColumn(int nav, int jump, bool selAll)
        {
            DataList<ColumnItem> list = new DataList<ColumnItem>();
            string where = selAll ? string.Empty : " AND [Show] <> 0";
            switch (nav)
            {
                case 0: { where += " AND [IsNav] = 0"; break; }
                case 1: { where += " AND [IsNav] <> 0"; break; }
            }
            switch (jump)
            {
                case 0: { where += " AND [IsJump] = 0"; break; }
                case 1: { where += " AND [IsJump] <> 0"; break; }
            }
            list.Number = Convert.ToInt32(conn.ExecuteScalar(string.Format("SELECT COUNT([ID]) FROM [Column] WHERE [ID] > 0{0};", where)));
            using (IDataReader reader = conn.ExecuteReader(string.Format("SELECT [ID], [Name], [Local], [Intro], [Sorting], [PostCount], [Show], [IsNav], [IsJump], [JumpUrl], [Target], [ListTpl], [ViewTpl], [PageSize] FROM [Column] WHERE [ID] > 0{0} ORDER BY [Sorting] ASC, [ID] ASC", where)))
            {
                while (reader.Read())
                {
                    ColumnItem item = new ColumnItem();
                    item.ID = reader.GetInt32(0);
                    item.Name = reader.GetString(1);
                    item.Local = reader.GetString(2);
                    item.Intro = reader.GetString(3);
                    item.Sorting = reader.GetInt32(4);
                    item.PostCount = reader.GetInt32(5);
                    item.Show = reader.GetInt32(6) == 0 ? false : true;
                    item.Nav = reader.GetInt32(7) == 0 ? false : true;
                    item.Jump = reader.GetInt32(8) == 0 ? false : true;
                    item.JumpUrl = reader.GetString(9);
                    item.Target = reader.GetString(10);
                    item.ListTemplate = reader.GetString(11);
                    item.ViewTemplate = reader.GetString(12);
                    item.PageSize = reader.GetInt32(13);
                    list.Add(item);
                }
            }
            return list;
        }

        /// <summary>
        /// 取得分类
        /// </summary>
        /// <param name="id">分类编号</param>
        /// <returns>返回分类数据</returns>
        public ColumnItem GetColumn(int id)
        {
            ColumnItem item = new ColumnItem();
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[Column]", "[ID], [Name], [Local], [Intro], [Sorting], [PostCount], [Show], [IsNav], [IsJump], [JumpUrl], [Target], [ListTpl], [ViewTpl], [PageSize]", string.Format("[ID] = {0}", id), null, null, 1, 1, ref rows))
            {
                while (reader.Read())
                {
                    item.ID = reader.GetInt32(0);
                    item.Name = reader.GetString(1);
                    item.Local = reader.GetString(2);
                    item.Intro = reader.GetString(3);
                    item.Sorting = reader.GetInt32(4);
                    item.PostCount = reader.GetInt32(5);
                    item.Show = reader.GetInt32(6) == 0 ? false : true;
                    item.Nav = reader.GetInt32(7) == 0 ? false : true;
                    item.Jump = reader.GetInt32(8) == 0 ? false : true;
                    item.JumpUrl = reader.GetString(9);
                    item.Target = reader.GetString(10);
                    item.ListTemplate = reader.GetString(11);
                    item.ViewTemplate = reader.GetString(12);
                    item.PageSize = reader.GetInt32(13);
                }
            }
            return item;
        }

        /// <summary>
        /// 取得分类
        /// </summary>
        /// <param name="local">永久地址</param>
        /// <returns>返回分类数据</returns>
        public ColumnItem GetColumn(string local)
        {
            _DbParameter[] pars ={
                                     new _DbParameter().Set("@Local", DbType.String, local)
                                 };
            ColumnItem item = new ColumnItem();
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[Column]", "[ID], [Name], [Local], [Intro], [Sorting], [PostCount], [Show], [IsNav], [IsJump], [JumpUrl], [Target], [ListTpl], [ViewTpl], [PageSize]", "[Local] = @Local", null, null, 1, 1, pars, ref rows))
            {
                while (reader.Read())
                {
                    item.ID = reader.GetInt32(0);
                    item.Name = reader.GetString(1);
                    item.Local = reader.GetString(2);
                    item.Intro = reader.GetString(3);
                    item.Sorting = reader.GetInt32(4);
                    item.PostCount = reader.GetInt32(5);
                    item.Show = reader.GetInt32(6) == 0 ? false : true;
                    item.Nav = reader.GetInt32(7) == 0 ? false : true;
                    item.Jump = reader.GetInt32(8) == 0 ? false : true;
                    item.JumpUrl = reader.GetString(9);
                    item.Target = reader.GetString(10);
                    item.ListTemplate = reader.GetString(11);
                    item.ViewTemplate = reader.GetString(12);
                    item.PageSize = reader.GetInt32(13);
                }
            }
            return item;
        }

        /// <summary>
        /// 根据分类名取得分类
        /// </summary>
        /// <param name="name">分类名称</param>
        /// <returns>返回分类数据</returns>
        public ColumnItem GetColumnForName(string name)
        {
            _DbParameter[] pars ={
                                     new _DbParameter().Set("@Name", DbType.String, name)
                                 };
            ColumnItem item = new ColumnItem();
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[Column]", "[ID], [Name], [Local], [Intro], [Sorting], [PostCount], [Show], [IsNav], [IsJump], [JumpUrl], [Target], [ListTpl], [ViewTpl], [PageSize]", "[Name] = @Name", null, null, 1, 1, pars, ref rows))
            {
                while (reader.Read())
                {
                    item.ID = reader.GetInt32(0);
                    item.Name = reader.GetString(1);
                    item.Local = reader.GetString(2);
                    item.Intro = reader.GetString(3);
                    item.Sorting = reader.GetInt32(4);
                    item.PostCount = reader.GetInt32(5);
                    item.Show = reader.GetInt32(6) == 0 ? false : true;
                    item.Nav = reader.GetInt32(7) == 0 ? false : true;
                    item.Jump = reader.GetInt32(8) == 0 ? false : true;
                    item.JumpUrl = reader.GetString(9);
                    item.Target = reader.GetString(10);
                    item.ListTemplate = reader.GetString(11);
                    item.ViewTemplate = reader.GetString(12);
                    item.PageSize = reader.GetInt32(13);
                }
            }
            return item;
        }
    }
}
