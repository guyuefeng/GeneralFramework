using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GF.Data.Entity;


namespace GF.Data
{
    public class MyTagData
    {
        private _DbHelper conn;

        public MyTagData(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 添加一个自定义标签
        /// </summary>
        /// <param name="value">自定义标签数据</param>
        /// <returns>返回新增的自定义标签的ID</returns>
        public int InsertMyTag(MyTagItem value)
        {
            int id = 0;
            _DbParameter[] pars ={
                                   new _DbParameter().Set("@Key", DbType.String, value.Key),
                                   new _DbParameter().Set("@Intro", DbType.String, value.Intro),
                                   new _DbParameter().Set("@Code", DbType.String, value.Code),
                                   new _DbParameter().Set("@Publish", DbType.String, value.Publish.ToString("yyyy-MM-dd HH:mm:ss")),
                                   new _DbParameter().Set("@Priority", DbType.Int32, value.Priority),
                                   new _DbParameter().Set("@Show", DbType.Int32, value.Show ? 1 : 0)
                               };
            conn.ExecuteNonQuery("INSERT INTO [MyTag] ([Key], [Intro], [Code], [Publish], [Priority], [Show]) VALUES (@Key, @Intro, @Code, @Publish, @Priority, @Show)", pars);
            id = Convert.ToInt32(conn.ExecuteNewField("[MyTag]", null, null));
            return id;
        }

        /// <summary>
        /// 修改一个自定义标签
        /// </summary>
        /// <param name="value">自定义标签数据</param>
        /// <returns>返回被修改的自定义标签编号</returns>
        public int UpdateMyTag(MyTagItem value)
        {
            int id = 0;
            _DbParameter[] pars ={
                                   new _DbParameter().Set("@Key", DbType.String, value.Key),
                                   new _DbParameter().Set("@Intro", DbType.String, value.Intro),
                                   new _DbParameter().Set("@Code", DbType.String, value.Code),
                                   new _DbParameter().Set("@Publish", DbType.String, value.Publish.ToString("yyyy-MM-dd HH:mm:ss")),
                                   new _DbParameter().Set("@Priority", DbType.Int32, value.Priority),
                                   new _DbParameter().Set("@Show", DbType.Int32, value.Show ? 1 : 0),
                                   new _DbParameter().Set("@ID", DbType.Int32, value.ID)
                               };
            conn.ExecuteNonQuery("UPDATE [MyTag] SET [Key] = @Key, [Intro] = @Intro, [Code] = @Code, [Publish] = @Publish, [Priority] = @Priority, [Show] = @Show WHERE [ID] = @ID", pars);
            id = value.ID;
            return id;
        }

        /// <summary>
        /// 修改自定义标签优先级
        /// </summary>
        /// <param name="id">自定义标签编号</param>
        /// <param name="priority">优先级</param>
        /// <param name="show">是否显示</param>
        /// <returns>返回被修改的自定义标签编号</returns>
        public int UpdateMyTagSome(int id, int priority, bool show)
        {
            _DbParameter[] pars ={
                                   new _DbParameter().Set("@Priority", DbType.Int32, priority),
                                   new _DbParameter().Set("@Show", DbType.Int32, show ? 1 : 0),
                                   new _DbParameter().Set("@ID", DbType.Int32, id)
                               };
            conn.ExecuteNonQuery("UPDATE [MyTag] SET [Priority] = @Priority, [Show] = @Show WHERE [ID] = @ID", pars);
            return id;
        }

        /// <summary>
        /// 删除一个自定义标签
        /// </summary>
        /// <param name="id">标签编号</param>
        /// <returns>返回被删除的标签编号</returns>
        public int DeleteMyTag(int id)
        {
            conn.ExecuteNonQuery(string.Format("DELETE FROM [MyTag] WHERE [ID] = {0}", id));
            return id;
        }

        /// <summary>
        /// 筛选自定义标签
        /// </summary>
        /// <param name="intCurPage">当前页码</param>
        /// <param name="btePerPage">分页大小</param>
        /// <param name="selAll">是否在所有数据里筛选</param>
        /// <returns>返回自定义标签数据列表</returns>
        public DataList<MyTagItem> SelectMyTag(int intCurPage, int btePerPage, bool selAll)
        {
            DataList<MyTagItem> list = new DataList<MyTagItem>();
            string sqlWhere = selAll ? string.Empty : "[Show] <> 0";
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[MyTag]", "[ID], [Key], [Intro], [Code], [Publish], [Priority], [Show]", sqlWhere, "[Priority]", "ASC", intCurPage, btePerPage, ref rows))
            {
                while (reader.Read())
                {
                    MyTagItem item = new MyTagItem();
                    item.ID = reader.GetInt32(0);
                    item.Key = reader.GetString(1);
                    item.Intro = reader.GetString(2);
                    item.Code = reader.GetString(3);
                    item.Publish = reader.GetDateTime(4);
                    item.Priority = reader.GetInt32(5);
                    item.Show = reader.GetInt32(6) == 0 ? false : true;
                    list.Add(item);
                }
            }
            list.Number = rows;
            return list;
        }

        /// <summary>
        /// 取得自定义标签
        /// </summary>
        /// <param name="id">自定义标签编号</param>
        /// <returns>返回自定义标签数据</returns>
        public MyTagItem GetMyTag(int id)
        {
            MyTagItem item = new MyTagItem();
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[MyTag]", "[ID], [Key], [Intro], [Code], [Publish], [Priority], [Show]", string.Format("[ID] = {0}", id), null, null, 1, 1, ref rows))
            {
                while (reader.Read())
                {
                    item.ID = reader.GetInt32(0);
                    item.Key = reader.GetString(1);
                    item.Intro = reader.GetString(2);
                    item.Code = reader.GetString(3);
                    item.Publish = reader.GetDateTime(4);
                    item.Priority = reader.GetInt32(5);
                    item.Show = reader.GetInt32(6) == 0 ? false : true;
                }
            }
            return item;
        }
    }
}
