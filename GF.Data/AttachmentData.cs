using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GF.Data.Entity;
using System.Data;

namespace GF.Data
{
    public class AttachmentData
    {
        private _DbHelper conn;

        public AttachmentData(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 添加一个附件记录
        /// </summary>
        /// <param name="value">附件记录数据</param>
        /// <returns>返回新增的附件记录编号</returns>
        public int InsertAttachment(AttachmentItem value)
        {
            int id = 0;
            _DbParameter[] pars ={
                                    new _DbParameter().Set("@Name", DbType.String, value.Name),
                                    new _DbParameter().Set("@Path", DbType.String, value.Path),
                                    new _DbParameter().Set("@Type", DbType.String, value.Type),
                                    new _DbParameter().Set("@Size", DbType.Int64, value.Size),
                                    new _DbParameter().Set("@Publish", DbType.String, value.Publish.ToString("yyyy-MM-dd HH:mm:ss"))
                                };
            conn.ExecuteNonQuery("INSERT INTO [Attach] ([Name], [Path], [Type], [Size], [Publish]) VALUES (@Name, @Path, @Type, @Size, @Publish)", pars);
            id = Convert.ToInt32(conn.ExecuteNewField("[Attach]", null, null));
            return id;
        }

        /// <summary>
        /// 删除一个附件记录
        /// </summary>
        /// <param name="id">记录编号</param>
        /// <returns>返回被删除的记录编号</returns>
        public int DeleteAttachment(int id)
        {
            conn.ExecuteNonQuery(string.Format("DELETE FROM [Attach] WHERE [ID] = {0}", id));
            return id;
        }

        /// <summary>
        /// 删除附件记录
        /// </summary>
        /// <param name="ids">编号列表（例如：0,1,79,80）</param>
        /// <returns>执行结果</returns>
        public int DeleteAttachment(string ids)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(ids))
            {
                ids = ids.Replace(" ", string.Empty);
                string[] arrIds = ids.Split(',');
                if (arrIds.Length > 0)
                {
                    string trueIds = "0";
                    foreach (string id in arrIds)
                    {
                        if (Regex.IsMatch(id, @"^\d+$")) { trueIds += string.Format("{0},", id); }
                    }
                    if (trueIds.Length > 1) { trueIds = trueIds.Substring(0, trueIds.Length - 1); }
                    result = conn.ExecuteNonQuery(string.Format("DELETE FROM [Attach] WHERE [ID] IN ({0})", trueIds));
                }
            }
            return result;
        }

        /// <summary>
        /// 筛选附件记录
        /// </summary>
        /// <param name="intCurPage">当前页码</param>
        /// <param name="btePerPage">分页大小</param>
        /// <returns>返回附件记录记录数据列表</returns>
        public DataList<AttachmentItem> SelectAttachment(int intCurPage, int btePerPage)
        {
            DataList<AttachmentItem> list = new DataList<AttachmentItem>();
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[Attach]", "[ID], [Name], [Path], [Type], [Size], [Publish]", null, "[ID]", "DESC", intCurPage, btePerPage, ref rows))
            {
                while (reader.Read())
                {
                    AttachmentItem item = new AttachmentItem();
                    item.ID = reader.GetInt32(0);
                    item.Name = reader.GetString(1);
                    item.Path = reader.GetString(2);
                    item.Type = reader.GetString(3);
                    item.Size = reader.GetInt32(4);
                    item.Publish = reader.GetDateTime(5);
                    list.Add(item);
                }
            }
            list.Number = rows;
            return list;
        }

        /// <summary>
        /// 筛选附件记录
        /// </summary>
        /// <param name="ids">编号列表（例如：0,1,79,80）</param>
        /// <returns>返回附件记录记录数据列表</returns>
        public DataList<AttachmentItem> SelectAttachment(string ids)
        {
            DataList<AttachmentItem> list = new DataList<AttachmentItem>();
            if (!string.IsNullOrEmpty(ids))
            {
                ids = ids.Replace(" ", string.Empty);
                string[] arrIds = ids.Split(',');
                if (arrIds.Length > 0)
                {
                    string trueIds = "0";
                    foreach (string id in arrIds)
                    {
                        if (Regex.IsMatch(id, @"^\d+$")) { trueIds += string.Format("{0},", id); }
                    }
                    if (trueIds.Length > 1) { trueIds = trueIds.Substring(0, trueIds.Length - 1); }
                    using (IDataReader reader = conn.ExecuteReader(string.Format("SELECT [ID], [Name], [Path], [Type], [Size], [Publish] FROM [Attach] WHERE [ID] IN ({0})", trueIds)))
                    {
                        while (reader.Read())
                        {
                            AttachmentItem item = new AttachmentItem();
                            item.ID = reader.GetInt32(0);
                            item.Name = reader.GetString(1);
                            item.Path = reader.GetString(2);
                            item.Type = reader.GetString(3);
                            item.Size = reader.GetInt32(4);
                            item.Publish = reader.GetDateTime(5);
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取附件记录
        /// </summary>
        /// <param name="id">附件编号</param>
        /// <returns>返回附件记录记录数据</returns>
        public AttachmentItem GetAttachment(int id)
        {
            AttachmentItem item = new AttachmentItem();
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[Attach]", "[ID], [Name], [Path], [Type], [Size], [Publish]", string.Format("[ID] = {0}", id), null, null, 1, 1, ref rows))
            {
                while (reader.Read())
                {
                    item.ID = reader.GetInt32(0);
                    item.Name = reader.GetString(1);
                    item.Path = reader.GetString(2);
                    item.Type = reader.GetString(3);
                    item.Size = reader.GetInt32(4);
                    item.Publish = reader.GetDateTime(5);
                }
            }
            return item;
        }
    }
}
