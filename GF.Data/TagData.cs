using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GF.Data.Entity;

namespace GF.Data
{
    public class TagData
    {
        private _DbHelper conn;

        public TagData(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 添加一个标签
        /// </summary>
        /// <param name="value">标签数据</param>
        /// <returns>返回新增的标签的ID</returns>
        public int InsertTag(TagItem value)
        {
            int id = 0;
            _DbParameter[] pars ={
                                     new _DbParameter().Set("@Key", DbType.String, value.Key),
                                     new _DbParameter().Set("@PC", DbType.Int32, value.PostCount)
                                 };
            conn.ExecuteNonQuery("INSERT INTO [Tag] ([Key], [PostCount]) VALUES (@Key, @PC)", pars);
            id = Convert.ToInt32(conn.ExecuteNewField("[Tag]", null, null));
            return id;
        }

        /// <summary>
        /// 修改一个标签
        /// </summary>
        /// <param name="value">标签数据</param>
        /// <returns>返回被修改的标签的ID</returns>
        public int UpdateTag(TagItem value)
        {
            int id = 0;
            _DbParameter[] pars ={
                                     new _DbParameter().Set("@Key", DbType.String, value.Key),
                                     new _DbParameter().Set("@ID", DbType.Int32, value.ID)
                                 };
            conn.ExecuteNonQuery("UPDATE [Tag] SET [Key] = @Key WHERE [ID] = @ID", pars);
            id = value.ID;
            return id;
        }

        /// <summary>
        /// 删除一个标签
        /// </summary>
        /// <param name="id">标签编号</param>
        /// <returns>返回被删除的标签ID</returns>
        public int DeleteTag(int id)
        {
            conn.ExecuteNonQuery(string.Format("DELETE FROM [Tag] WHERE [ID] = {0}", id));
            return id;
        }

        /// <summary>
        /// 计算标签下的数据量
        /// </summary>
        /// <param name="key">标签名称</param>
        public void CountTagPost(string key)
        {
            _DbParameter[] pars ={
                                     new _DbParameter().Set("@Key", DbType.String, key),
                                     new _DbParameter().Set("@KeyLeft", DbType.String, key + ",%"),
                                     new _DbParameter().Set("@KeyCenter", DbType.String, "%," + key + ",%"),
                                     new _DbParameter().Set("@KeyRight", DbType.String, "%," + key)
                                 };
            int count = Convert.ToInt32(conn.ExecuteScalar("SELECT COUNT([ID]) FROM [Post] WHERE [Tags] = @Key OR [Tags] LIKE @KeyLeft OR [Tags] LIKE @KeyCenter OR [Tags] LIKE @KeyRight", pars));
            conn.ExecuteNonQuery(string.Format("UPDATE [Tag] SET [PostCount] = {0} WHERE [Key] = @Key", count), pars);
        }

        /// <summary>
        /// 检查标签是否存在
        /// </summary>
        /// <param name="key">标签名</param>
        /// <param name="ourId">例外的ID，为0则筛选所有数据</param>
        /// <returns>返回是否存在的布尔型数据</returns>
        public bool ExistsKey(string key, int ourId)
        {
            bool result = false;
            _DbParameter[] pars ={
                                     new _DbParameter().Set("@Key", DbType.String, key),
                                     new _DbParameter().Set("@OurID", DbType.Int32, ourId)
                                 };
            if (Convert.ToInt32(conn.ExecuteScalar(string.Format("SELECT COUNT([ID]) FROM [Tag] WHERE [Key] = @Key{0};", (ourId == 0 ? string.Empty : " AND [ID] <> @OurID")), pars)) > 0)
            {
                result = true;
            }//5$1$a$s$p$x
            return result;
        }

        /// <summary>
        /// 筛选标签
        /// </summary>
        /// <param name="intCurPage">当前页码</param>
        /// <param name="btePerPage">分页大小</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderMode">排序模式，ASC, DESC</param>
        /// <returns>返回标签数据列表</returns>
        public TagList SelectTag(int intCurPage, int btePerPage, string orderField, string orderMode)
        {
            if (string.IsNullOrEmpty(orderField)) { orderField = "[Key]"; }
            if (string.IsNullOrEmpty(orderMode)) { orderMode = "ASC"; }
            TagList list = new TagList();
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[Tag]", "[ID], [Key], [PostCount]", null, orderField, orderMode, intCurPage, btePerPage, ref rows))
            {
                while (reader.Read())
                {
                    TagItem item = new TagItem();
                    item.ID = reader.GetInt32(0);
                    item.Key = reader.GetString(1);
                    item.PostCount = reader.GetInt32(2);
                    list.Add(item);
                }
            }
            list.Number = rows;
            return list;
        }

        /// <summary>
        /// 筛选标签
        /// </summary>
        /// <param name="intCurPage">当前页码</param>
        /// <param name="btePerPage">分页大小</param>
        /// <returns>返回标签数据列表</returns>
        public TagList SelectTag(int intCurPage, int btePerPage)
        {
            return SelectTag(intCurPage, btePerPage, null, null);
        }

        /// <summary>
        /// 把一个标签列插入到数据库里（存在的标签则更新，否则新增）
        /// </summary>
        /// <param name="list">标签列表</param>
        /// <returns>返回成功插入的数量</returns>
        public int InsertTagList(string list)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(list))
            {
                string[] arrList = list.Split(',', ' ');
                for (int i = 0; i < arrList.Length; i++)
                {
                    if (!string.IsNullOrEmpty(arrList[i])) { arrList[i] = arrList[i].Trim(); }
                    if (ExistsKey(arrList[i], 0)) { CountTagPost(arrList[i]); }
                    else
                    {
                        TagItem val = new TagItem();
                        val.Key = arrList[i];
                        val.PostCount = 1;
                        InsertTag(val);
                    }
                    count++;
                }
            }
            return count;
        }
    }
}
