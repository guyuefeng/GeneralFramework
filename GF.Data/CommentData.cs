using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GF.Data.Entity;

namespace GF.Data
{
    public class CommentData
    {
        private _DbHelper conn;

        public CommentData(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 插入新的评论数据
        /// </summary>
        /// <param name="value">评论数据</param>
        /// <returns>返回新增评论的编号</returns>
        public int InsertComment(CommentItem value)
        {
            int id = 0;
            _DbParameter[] pars ={
                                   new _DbParameter().Set("@PostID", DbType.Int32, value.PostID),
                                   new _DbParameter().Set("@Author", DbType.String, value.Author),
                                   new _DbParameter().Set("@Title", DbType.String, value.Title),
                                   new _DbParameter().Set("@Mail", DbType.String, value.Mail),
                                   new _DbParameter().Set("@URL", DbType.String, value.URL),
                                   new _DbParameter().Set("@Content", DbType.String, value.Content),
                                   new _DbParameter().Set("@Reply", DbType.String, value.Reply),
                                   new _DbParameter().Set("@IsTB", DbType.Int32, value.Trackback ? 1 : 0),
                                   new _DbParameter().Set("@Publish", DbType.String, value.Publish.ToString("yyyy-MM-dd HH:mm:ss")),
                                   new _DbParameter().Set("@Verify", DbType.Int32, value.Verify ? 1 : 0)
                               };
            conn.ExecuteNonQuery("INSERT INTO [Comment] ([PostID], [Author], [Title], [Mail], [URL], [Content], [Reply], [IsTB], [Publish], [Verify]) VALUES (@PostID, @Author, @Title, @Mail, @URL, @Content, @Reply, @IsTB, @Publish, @Verify)", pars);
            conn.ExecuteNonQuery("UPDATE [Post] SET [PostCount] = [PostCount] + 1 WHERE [ID] = @PostID", pars);
            id = Convert.ToInt32(conn.ExecuteNewField("[Comment]", null, null));
            return id;
        }

        /// <summary>
        /// 修改一个评论
        /// </summary>
        /// <param name="value">评论数据</param>
        /// <returns>返回被修改的评论编号</returns>
        public int UpdateComment(CommentItem value)
        {
            int id = 0;
            _DbParameter[] pars ={
                                   new _DbParameter().Set("@PostID", DbType.Int32, value.PostID),
                                   new _DbParameter().Set("@Author", DbType.String, value.Author),
                                   new _DbParameter().Set("@Title", DbType.String, value.Title),
                                   new _DbParameter().Set("@Mail", DbType.String, value.Mail),
                                   new _DbParameter().Set("@URL", DbType.String, value.URL),
                                   new _DbParameter().Set("@Content", DbType.String, value.Content),
                                   new _DbParameter().Set("@Reply", DbType.String, value.Reply),
                                   new _DbParameter().Set("@IsTB", DbType.Int32, value.Trackback ? 1 : 0),
                                   new _DbParameter().Set("@Publish", DbType.String, value.Publish.ToString("yyyy-MM-dd HH:mm:ss")),
                                   new _DbParameter().Set("@Verify", DbType.Int32, value.Verify ? 1 : 0),
                                   new _DbParameter().Set("@ID", DbType.Int32, value.ID)
                               };
            conn.ExecuteNonQuery("UPDATE [Comment] SET [PostID] = @PostID, [Author] = @Author, [Title] = @Title, [Mail] = @Mail, [URL] = @URL, [Content] = @Content, [Reply] = @Reply, [IsTB] = @IsTB, [Publish] = @Publish, [Verify] = @Verify WHERE [ID] = @ID", pars);
            id = value.ID;
            return id;
        }

        /// <summary>
        /// 删除一个评论
        /// </summary>
        /// <param name="id">评论编号</param>
        /// <returns>返回被删除的评论编号</returns>
        public int DeleteComment(int id)
        {
            int artId = 0;
            using (IDataReader reader = conn.ExecuteReader(string.Format("SELECT [PostID] FROM [Comment] WHERE [ID] = {0}", id)))
            {
                if (reader.Read()) { artId = reader.GetInt32(0); }
            }
            conn.ExecuteNonQuery(string.Format("DELETE FROM [Comment] WHERE [ID] = {0}", id));
            if (artId > 0)
            {
                conn.ExecuteNonQuery(string.Format("UPDATE [Post] SET [PostCount] = [PostCount] - 1 WHERE [ID] = {0}", artId));
            }
            return id;
        }

        /// <summary>
        /// 筛选评论
        /// </summary>
        /// <param name="artId">大于零则筛选相关文章下的评论</param>
        /// <param name="intCurPage">当前页</param>
        /// <param name="btePerPage">每页总数</param>
        /// <param name="selAll">是否筛选所有数据</param>
        /// <returns>返回评论数据列表</returns>
        public DataList<CommentItem> SelectComment(int artId, int intCurPage, int btePerPage, bool selAll)
        {
            DataList<CommentItem> list = new DataList<CommentItem>();
            string where = string.Empty;
            if (artId > 0) { where += string.Format(" AND [PostID] = {0}", artId); }
            if (!selAll) { where += " AND [Verify] <> 0"; }
            if (!string.IsNullOrEmpty(where)) { where = "[ID] > 0" + where; }
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[Comment]", "[ID], [PostID], [Author], [Title], [Mail], [URL], [Content], [Reply], [IsTB], [Publish], [Verify]", where, "[Publish]", "DESC", intCurPage, btePerPage, ref rows))
            {
                while (reader.Read())
                {
                    CommentItem item = new CommentItem();
                    item.ID = reader.GetInt32(0);
                    item.PostID = reader.GetInt32(1);
                    item.Author = reader.GetString(2);
                    item.Title = reader.GetString(3);
                    item.Mail = reader.GetString(4);
                    item.URL = reader.GetString(5);
                    item.Content = reader.GetString(6);
                    item.Reply = reader.GetString(7);
                    item.Trackback = reader.GetInt32(8) == 0 ? false : true;
                    item.Publish = reader.GetDateTime(9);
                    item.Verify = reader.GetInt32(10) == 0 ? false : true;
                    list.Add(item);
                }
            }
            list.Number = rows;
            return list;
        }

        /// <summary>
        /// 获取评论
        /// </summary>
        /// <param name="cmtId">评论编号</param>
        /// <returns>返回评论数据</returns>
        public CommentItem GetComment(int cmtId)
        {
            CommentItem item = new CommentItem();
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[Comment]", "[ID], [PostID], [Author], [Title], [Mail], [URL], [Content], [Reply], [IsTB], [Publish], [Verify]", string.Format("[ID] = {0}", cmtId), null, null, 1, 1, ref rows))
            {
                while (reader.Read())
                {
                    item.ID = reader.GetInt32(0);
                    item.PostID = reader.GetInt32(1);
                    item.Author = reader.GetString(2);
                    item.Title = reader.GetString(3);
                    item.Mail = reader.GetString(4);
                    item.URL = reader.GetString(5);
                    item.Content = reader.GetString(6);
                    item.Reply = reader.GetString(7);
                    item.Trackback = reader.GetInt32(8) == 0 ? false : true;
                    item.Publish = reader.GetDateTime(9);
                    item.Verify = reader.GetInt32(10) == 0 ? false : true;
                }
            }
            return item;
        }

        /// <summary>
        /// 设置评论审核状态
        /// </summary>
        /// <param name="id">评论编号</param>
        /// <param name="verify">是否审核</param>
        /// <returns>返回被修改评论的编号</returns>
        public int UpdateCommentVerify(int id, bool verify)
        {
            _DbParameter[] pars ={
                                     new _DbParameter().Set("@Verify", DbType.Int32, verify ? 1 : 0),
                                     new _DbParameter().Set("@ID", DbType.Int32, id)
                                 };
            conn.ExecuteNonQuery("UPDATE [Comment] SET [Verify] = @Verify WHERE [ID] = @ID", pars);
            return id;
        }
    }
}
