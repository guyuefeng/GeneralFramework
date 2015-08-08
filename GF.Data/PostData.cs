using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GF.Data.Entity;


namespace GF.Data
{
    public class PostData
    {
        private _DbHelper conn;

        public PostData(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 增加一篇文章
        /// </summary>
        /// <param name="value">文章数据</param>
        /// <param name="mode">模式：A-文章，P-单页</param>
        /// <returns>返回新增文章的编号</returns>
        public int InsertPost(PostItem value, string mode)
        {
            int id = 0;
            _DbParameter[] pars ={
                                     new _DbParameter().Set("@CID", DbType.Int32, value.ColumnID),
                                     new _DbParameter().Set("@Tags", DbType.String, value.Tags),
                                     new _DbParameter().Set("@Local", DbType.String, value.Local),
                                     new _DbParameter().Set("@Title", DbType.String, value.Title),
                                     new _DbParameter().Set("@Explain", DbType.String, value.Explain),
                                     new _DbParameter().Set("@Content", DbType.String, value.Content),
                                     new _DbParameter().Set("@Author", DbType.String, value.Author),
                                     new _DbParameter().Set("@Publish", DbType.String, value.Publish.ToString("yyyy-MM-dd HH:mm:ss")),
                                     new _DbParameter().Set("@Password", DbType.String, value.Password),
                                     new _DbParameter().Set("@Fine", DbType.Int32, value.Fine ? 1 : 0),
                                     new _DbParameter().Set("@Vote", DbType.Int32, value.Vote),
                                     new _DbParameter().Set("@Reader", DbType.Int32, value.Reader),
                                     new _DbParameter().Set("@PostCount", DbType.Int32, value.PostCount),
                                     new _DbParameter().Set("@Show", DbType.Int32, value.Show ? 1 : 0),
                                     new _DbParameter().Set("@SwitchCMT", DbType.Int32, value.SwitchComment ? 1 : 0),
                                     new _DbParameter().Set("@SwitchTB", DbType.Int32, value.SwitchTrackback ? 1 : 0),
                                     new _DbParameter().Set("@AVCMT", DbType.Int32, value.AutoVerifyComment ? 1 : 0),
                                     new _DbParameter().Set("@AVTB", DbType.Int32, value.AutoVerifyTrackback ? 1 : 0),
                                     new _DbParameter().Set("@Mode", DbType.String, mode),
                                     new _DbParameter().Set("@Atts", DbType.String, value.Attachments)
                                 };
            conn.ExecuteNonQuery("INSERT INTO [Post] ([ColumnID], [Tags], [Local], [Title], [Explain], [Content], [Author], [Publish], [Password], [Fine], [Vote], [Reader], [PostCount], [Show], [SwitchCMT], [SwitchTB], [AutoVerifyCMT], [AutoVerifyTB], [Mode], [Attachments]) VALUES (@CID, @Tags, @Local, @Title, @Explain, @Content, @Author, @Publish, @Password, @Fine, @Vote, @Reader, @PostCount, @Show, @SwitchCMT, @SwitchTB, @AVCMT, @AVTB, @Mode, @Atts)", pars);
            conn.ExecuteNonQuery(string.Format("UPDATE [Column] SET [PostCount] = [PostCount] + 1 WHERE [ID] = {0}", value.ColumnID));
            id = Convert.ToInt32(conn.ExecuteNewField("[Post]", null, null));
            if (string.IsNullOrEmpty(value.Local))
            {
                conn.ExecuteNonQuery(string.Format("UPDATE [Post] SET [Local] = [ID] WHERE [ID] = {0}", id));
            }
            return id;
        }

        /// <summary>
        /// 更新一篇文章
        /// </summary>
        /// <param name="value">文章数据</param>
        /// <returns>返回被修改文章的编号</returns>
        public int UpdatePost(PostItem value)
        {
            int id = 0;
            _DbParameter[] pars ={
                                     new _DbParameter().Set("@CID", DbType.Int32, value.ColumnID),
                                     new _DbParameter().Set("@Tags", DbType.String, value.Tags),
                                     new _DbParameter().Set("@Local", DbType.String, value.Local),
                                     new _DbParameter().Set("@Title", DbType.String, value.Title),
                                     new _DbParameter().Set("@Explain", DbType.String, value.Explain),
                                     new _DbParameter().Set("@Content", DbType.String, value.Content),
                                     new _DbParameter().Set("@Author", DbType.String, value.Author),
                                     new _DbParameter().Set("@Publish", DbType.String, value.Publish.ToString("yyyy-MM-dd HH:mm:ss")),
                                     new _DbParameter().Set("@Password", DbType.String, value.Password),
                                     new _DbParameter().Set("@Fine", DbType.Int32, value.Fine ? 1 : 0),
                                     new _DbParameter().Set("@Vote", DbType.Int32, value.Vote),
                                     new _DbParameter().Set("@Reader", DbType.Int32, value.Reader),
                                     new _DbParameter().Set("@PostCount", DbType.Int32, value.PostCount),
                                     new _DbParameter().Set("@Show", DbType.Int32, value.Show ? 1 : 0),
                                     new _DbParameter().Set("@SwitchCMT", DbType.Int32, value.SwitchComment ? 1 : 0),
                                     new _DbParameter().Set("@SwitchTB", DbType.Int32, value.SwitchTrackback ? 1 : 0),
                                     new _DbParameter().Set("@AVCMT", DbType.Int32, value.AutoVerifyComment ? 1 : 0),
                                     new _DbParameter().Set("@AVTB", DbType.Int32, value.AutoVerifyTrackback ? 1 : 0),
                                     new _DbParameter().Set("@Atts", DbType.String, value.Attachments),
                                     new _DbParameter().Set("@ID", DbType.Int32, value.ID)
                                 };
            conn.ExecuteNonQuery("UPDATE [Post] SET [ColumnID] = @CID, [Tags] = @Tags, [Local] = @Local, [Title] = @Title, [Explain] = @Explain, [Content] = @Content, [Author] = @Author, [Publish] = @Publish, [Password] = @Password, [Fine] = @Fine, [Vote] = @Vote, [Reader] = @Reader, [PostCount] = @PostCount, [Show] = @Show, [SwitchCMT] = @SwitchCMT, [SwitchTB] = @SwitchTB, [AutoVerifyCMT] = @AVCMT, [AutoVerifyTB] = @AVTB, [Attachments] = @Atts WHERE [ID] = @ID", pars);
            id = value.ID;
            if (string.IsNullOrEmpty(value.Local))
            {
                conn.ExecuteNonQuery(string.Format("UPDATE [Post] SET [Local] = [ID] WHERE [ID] = {0}", id));
            }
            return id;
        }

        /// <summary>
        /// 重新统计文章下的评论数量
        /// </summary>
        /// <returns>返回处理的条数</returns>
        public int CountPost()
        {
            int result = 0;
            using (IDataReader reader = conn.ExecuteReader("SELECT [ID] FROM [Post]"))
            {
                conn.BeginTransaction();
                try
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        int count = Convert.ToInt32(conn.ExecuteScalar(string.Format("SELECT COUNT([ID]) FROM [Comment] WHERE [PostID] = {0}", id)));
                        conn.ExecuteNonQuery(string.Format("UPDATE [Post] SET [PostCount] = {1} WHERE [ID] = {0}", id, count));
                        result++;
                    }
                    conn.Commit();
                }
                catch (Exception err) { conn.Rollback(); throw err; }
            }
            return result;//51(aspx)
        }

        /// <summary>
        /// 获取附近文章
        /// </summary>
        /// <param name="thisId">当前文章编号</param>
        /// <param name="desc">是否正常排序，否则倒序</param>
        /// <param name="mode">模式：A-文章,P-单页</param>
        /// <returns>返回附近文章数据</returns>
        public PostItem GetClosePost(int thisId, bool desc, string mode)
        {
            string[] pa = { ">", "ASC" };
            if (!desc) { pa[0] = "<"; pa[1] = "DESC"; }
            PostItem item = new PostItem();
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[Post]", "[ID], [ColumnID], [Tags], [Local], [Title], [Explain], [Content], [Author], [Publish], [Password], [Fine], [Vote], [Reader], [PostCount], [Show], [SwitchCMT], [SwitchTB], [AutoVerifyCMT], [AutoVerifyTB], [Attachments]", string.Format("[Mode] = '{2}' AND [ID] {0} {1}", pa[0], thisId, mode), "[Publish]", pa[1], 1, 1, ref rows))
            {
                while (reader.Read())
                {
                    item.ID = reader.GetInt32(0);
                    item.ColumnID = reader.GetInt32(1);
                    item.Tags = reader.GetString(2);
                    item.Local = reader.GetString(3);
                    item.Title = reader.GetString(4);
                    item.Explain = reader.GetString(5);
                    item.Content = reader.GetString(6);
                    item.Author = reader.GetString(7);
                    item.Publish = reader.GetDateTime(8);
                    item.Password = reader.GetString(9);
                    item.Fine = reader.GetInt32(10) == 0 ? false : true;
                    item.Vote = reader.GetInt32(11);
                    item.Reader = reader.GetInt32(12);
                    item.PostCount = reader.GetInt32(13);
                    item.Show = reader.GetInt32(14) == 0 ? false : true;
                    item.SwitchComment = reader.GetInt32(15) == 0 ? false : true;
                    item.SwitchTrackback = reader.GetInt32(16) == 0 ? false : true;
                    item.AutoVerifyComment = reader.GetInt32(17) == 0 ? false : true;
                    item.AutoVerifyTrackback = reader.GetInt32(18) == 0 ? false : true;
                    item.Attachments = reader.GetString(19);
                }
            }
            return item;
        }

        /// <summary>
        /// 选择相关文章
        /// </summary>
        /// <param name="outId">例外文章编号</param>
        /// <param name="limit">取得行数</param>
        /// <param name="tags">标签列表</param>
        /// <param name="mode">模式：A-文章,P-单页</param>
        /// <param name="selAll">是否在所有数据里筛选</param>
        /// <returns>返回文章数据列表</returns>
        public DataList<PostItem> SelectRelatedPost(int outId, int limit, string tags, string mode, bool selAll)
        {
            if (!string.IsNullOrEmpty(tags)) { tags = DataBase.SqlEncode(tags); }
            DataList<PostItem> list = new DataList<PostItem>();
            string sqlWhere = string.Format("[Mode] = '{0}'", mode);
            sqlWhere += selAll ? string.Empty : " AND ([Show] <> 0)";
            if (!string.IsNullOrEmpty(tags))
            {
                sqlWhere += " AND (";
                string[] arrTag = tags.Split(',');
                bool noOne = false;
                foreach (string tag in arrTag)
                {
                    if (!string.IsNullOrEmpty(tag))
                    {
                        sqlWhere += noOne ? " OR" : string.Empty;
                        sqlWhere += string.Format(" ([Tags] = '{0}' OR [Tags] LIKE '{0},%' OR [Tags] LIKE '%,{0},%' OR [Tags] LIKE '%,{0}')", tag.Trim());
                        noOne = true;
                    }
                }
                sqlWhere += ")";
            }
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[Post]", "[ID], [ColumnID], [Tags], [Local], [Title], [Explain], [Content], [Author], [Publish], [Password], [Fine], [Vote], [Reader], [PostCount], [Show], [SwitchCMT], [SwitchTB], [AutoVerifyCMT], [AutoVerifyTB], [Attachments]", sqlWhere, "[ID]", "DESC", 1, limit, ref rows))
            {
                while (reader.Read())
                {
                    PostItem item = new PostItem();
                    item.ID = reader.GetInt32(0);
                    item.ColumnID = reader.GetInt32(1);
                    item.Tags = reader.GetString(2);
                    item.Local = reader.GetString(3);
                    item.Title = reader.GetString(4);
                    item.Explain = reader.GetString(5);
                    item.Content = reader.GetString(6);
                    item.Author = reader.GetString(7);
                    item.Publish = reader.GetDateTime(8);
                    item.Password = reader.GetString(9);
                    item.Fine = reader.GetInt32(10) == 0 ? false : true;
                    item.Vote = reader.GetInt32(11);
                    item.Reader = reader.GetInt32(12);
                    item.PostCount = reader.GetInt32(13);
                    item.Show = reader.GetInt32(14) == 0 ? false : true;
                    item.SwitchComment = reader.GetInt32(15) == 0 ? false : true;
                    item.SwitchTrackback = reader.GetInt32(16) == 0 ? false : true;
                    item.AutoVerifyComment = reader.GetInt32(17) == 0 ? false : true;
                    item.AutoVerifyTrackback = reader.GetInt32(18) == 0 ? false : true;
                    item.Attachments = reader.GetString(19);
                    list.Add(item);
                }
            }
            list.Number = rows;
            return list;
        }

        /// <summary>
        /// 选择文章
        /// </summary>
        /// <param name="cid">分类ID,为0则选择全部数据</param>
        /// <param name="tag">标签字段</param>
        /// <param name="key">搜索关键字</param>
        /// <param name="intCurPage">当前页码</param>
        /// <param name="btePerPage">分页大小</param>
        /// <param name="orderMode">排序模式：0-按时间倒序，1-编号倒序，2-按阅读数倒序，3-按投票数，4-随机</param>
        /// <param name="mode">模式：A-文章,P-单页</param>
        /// <param name="selAll">是否在所有数据里筛选</param>
        /// <returns>返回文章数据列表</returns>
        public DataList<PostItem> SelectPost(int cid, string tag, string key, int intCurPage, int btePerPage, int orderMode, string mode, bool selAll)
        {
            string putOrder = orderMode == 4 ? string.Empty : "[Fine] DESC, ";
            if (!string.IsNullOrEmpty(tag)) { tag = DataBase.SqlEncode(tag); }
            if (!string.IsNullOrEmpty(key)) { key = DataBase.SqlEncode(key); }
            DataList<PostItem> list = new DataList<PostItem>();
            string[] orderPar = { "[Publish]", "DESC" };
            switch (orderMode)
            {
                case 1: { orderPar[0] = "[ID]"; orderPar[1] = "DESC"; break; }
                case 2: { orderPar[0] = "[Reader]"; orderPar[1] = "DESC"; break; }
                case 3: { orderPar[0] = "[Vote]"; orderPar[1] = "DESC"; break; }
                case 4: { orderPar[0] = (conn.DBMode == DbMode.MSSQL ? "NEWID()" : "RND([ID])"); orderPar[1] = string.Empty; break; }
            }
            string sqlWhere = string.IsNullOrEmpty(mode) ? "[ID] > 0" : string.Format("[Mode] = '{0}'", mode);
            sqlWhere += cid == 0 ? string.Empty : string.Format(" AND ([ColumnID] = {0})", cid);
            sqlWhere += selAll ? string.Empty : " AND ([Show] <> 0)";
            sqlWhere += string.IsNullOrEmpty(tag) ? string.Empty : string.Format(" AND ([Tags] = '{0}' OR [Tags] LIKE '{0},%' OR [Tags] LIKE '%,{0},%' OR [Tags] LIKE '%,{0}')", tag);
            sqlWhere += string.IsNullOrEmpty(key) ? string.Empty : string.Format(" AND ([Title] LIKE '%{0}%' OR [Content] LIKE '%{0}%')", key);
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[Post]", "[ID], [ColumnID], [Tags], [Local], [Title], [Explain], [Content], [Author], [Publish], [Password], [Fine], [Vote], [Reader], [PostCount], [Show], [SwitchCMT], [SwitchTB], [AutoVerifyCMT], [AutoVerifyTB], [Attachments]", sqlWhere, putOrder + orderPar[0], orderPar[1], intCurPage, btePerPage, ref rows))
            {
                while (reader.Read())
                {
                    PostItem item = new PostItem();
                    item.ID = reader.GetInt32(0);
                    item.ColumnID = reader.GetInt32(1);
                    item.Tags = reader.GetString(2);
                    item.Local = reader.GetString(3);
                    item.Title = reader.GetString(4);
                    item.Explain = reader.GetString(5);
                    item.Content = reader.GetString(6);
                    item.Author = reader.GetString(7);
                    item.Publish = reader.GetDateTime(8);
                    item.Password = reader.GetString(9);
                    item.Fine = reader.GetInt32(10) == 0 ? false : true;
                    item.Vote = reader.GetInt32(11);
                    item.Reader = reader.GetInt32(12);
                    item.PostCount = reader.GetInt32(13);
                    item.Show = reader.GetInt32(14) == 0 ? false : true;
                    item.SwitchComment = reader.GetInt32(15) == 0 ? false : true;
                    item.SwitchTrackback = reader.GetInt32(16) == 0 ? false : true;
                    item.AutoVerifyComment = reader.GetInt32(17) == 0 ? false : true;
                    item.AutoVerifyTrackback = reader.GetInt32(18) == 0 ? false : true;
                    item.Attachments = reader.GetString(19);
                    list.Add(item);
                }
            }
            list.Number = rows;
            return list;
        }

        /// <summary>
        /// 选择文章
        /// </summary>
        /// <param name="cid">分类ID,为0则选择全部数据</param>
        /// <param name="tag">标签字段</param>
        /// <param name="key">搜索关键字</param>
        /// <param name="intCurPage">当前页码</param>
        /// <param name="btePerPage">分页大小</param>
        /// <param name="orderMode">排序模式：0-按时间倒序，1-编号倒序，2-按阅读数倒序，3-按投票数，4-随机</param>
        /// <param name="mode">模式：A-文章,P-单页</param>
        /// <returns>返回文章数据列表</returns>
        public DataList<PostItem> SelectPost(int cid, string tag, string key, int intCurPage, int btePerPage, int orderMode, string mode)
        {
            return SelectPost(cid, tag, key, intCurPage, btePerPage, orderMode, mode, true);
        }

        /// <summary>
        /// 选择文章
        /// </summary>
        /// <param name="cid">分类ID,为0则选择全部数据</param>
        /// <param name="tag">标签字段</param>
        /// <param name="key">搜索关键字</param>
        /// <param name="intCurPage">当前页码</param>
        /// <param name="btePerPage">分页大小</param>
        /// <param name="mode">模式：A-文章,P-单页</param>
        /// <returns>返回文章数据列表</returns>
        public DataList<PostItem> SelectPost(int cid, string tag, string key, int intCurPage, int btePerPage, string mode)
        {
            return SelectPost(cid, tag, key, intCurPage, btePerPage, 0, mode, true);
        }

        /// <summary>
        /// 选择文章
        /// </summary>
        /// <param name="cid">分类ID,为0则选择全部数据</param>
        /// <param name="intCurPage">当前页码</param>
        /// <param name="btePerPage">分页大小</param>
        /// <param name="mode">模式：A-文章,P-单页</param>
        /// <returns>返回文章数据列表</returns>
        public DataList<PostItem> SelectPost(int cid, int intCurPage, int btePerPage, string mode)
        {
            return SelectPost(cid, null, null, intCurPage, btePerPage, 0, mode, true);
        }

        /// <summary>
        /// 选择文章
        /// </summary>
        /// <param name="intCurPage">当前页码</param>
        /// <param name="btePerPage">分页大小</param>
        /// <param name="mode">模式：A-文章,P-单页</param>
        /// <returns>返回文章数据列表</returns>
        public DataList<PostItem> SelectPost(int intCurPage, int btePerPage, string mode)
        {
            return SelectPost(0, null, null, intCurPage, btePerPage, 0, mode, true);
        }

        //51aspx
        /// <summary>
        /// 选择文章
        /// </summary>
        /// <param name="id">文章编号</param>
        /// <returns>返回文章数据</returns>
        public PostItem GetPost(int id)
        {
            PostItem item = new PostItem();
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[Post]", "[ID], [ColumnID], [Tags], [Local], [Title], [Explain], [Content], [Author], [Publish], [Password], [Fine], [Vote], [Reader], [PostCount], [Show], [SwitchCMT], [SwitchTB], [AutoVerifyCMT], [AutoVerifyTB], [Attachments]", string.Format("[ID] = {0}", id), "[ID]", "DESC", 1, 1, ref rows))
            {
                if (reader.Read())
                {
                    item.ID = reader.GetInt32(0);
                    item.ColumnID = reader.GetInt32(1);
                    item.Tags = reader.GetString(2);
                    item.Local = reader.GetString(3);
                    item.Title = reader.GetString(4);
                    item.Explain = reader.GetString(5);
                    item.Content = reader.GetString(6);
                    item.Author = reader.GetString(7);
                    item.Publish = reader.GetDateTime(8);
                    item.Password = reader.GetString(9);
                    item.Fine = reader.GetInt32(10) == 0 ? false : true;
                    item.Vote = reader.GetInt32(11);
                    item.Reader = reader.GetInt32(12);
                    item.PostCount = reader.GetInt32(13);
                    item.Show = reader.GetInt32(14) == 0 ? false : true;
                    item.SwitchComment = reader.GetInt32(15) == 0 ? false : true;
                    item.SwitchTrackback = reader.GetInt32(16) == 0 ? false : true;
                    item.AutoVerifyComment = reader.GetInt32(17) == 0 ? false : true;
                    item.AutoVerifyTrackback = reader.GetInt32(18) == 0 ? false : true;
                    item.Attachments = reader.GetString(19);
                }
            }
            return item;
        }

        /// <summary>
        /// 选择文章
        /// </summary>
        /// <param name="local">文章永久链接</param>
        /// <returns>返回文章数据</returns>
        public PostItem GetPost(string local)
        {
            _DbParameter[] pars ={
                                     new _DbParameter().Set("@Local", DbType.String, local)
                                 };
            PostItem item = new PostItem();
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[Post]", "[ID], [ColumnID], [Tags], [Local], [Title], [Explain], [Content], [Author], [Publish], [Password], [Fine], [Vote], [Reader], [PostCount], [Show], [SwitchCMT], [SwitchTB], [AutoVerifyCMT], [AutoVerifyTB], [Attachments]", "[Local] = @Local", "[ID]", "DESC", 1, 1, pars, ref rows))
            {
                if (reader.Read())
                {
                    item.ID = reader.GetInt32(0);
                    item.ColumnID = reader.GetInt32(1);
                    item.Tags = reader.GetString(2);
                    item.Local = reader.GetString(3);
                    item.Title = reader.GetString(4);
                    item.Explain = reader.GetString(5);
                    item.Content = reader.GetString(6);
                    item.Author = reader.GetString(7);
                    item.Publish = reader.GetDateTime(8);
                    item.Password = reader.GetString(9);
                    item.Fine = reader.GetInt32(10) == 0 ? false : true;
                    item.Vote = reader.GetInt32(11);
                    item.Reader = reader.GetInt32(12);
                    item.PostCount = reader.GetInt32(13);
                    item.Show = reader.GetInt32(14) == 0 ? false : true;
                    item.SwitchComment = reader.GetInt32(15) == 0 ? false : true;
                    item.SwitchTrackback = reader.GetInt32(16) == 0 ? false : true;
                    item.AutoVerifyComment = reader.GetInt32(17) == 0 ? false : true;
                    item.AutoVerifyTrackback = reader.GetInt32(18) == 0 ? false : true;
                    item.Attachments = reader.GetString(19);
                }
            }
            return item;
        }

        /// <summary>
        /// 删除一篇文章
        /// </summary>
        /// <param name="id">文章编号</param>
        /// <returns>返回被删除的文章编号</returns>
        public int DeletePost(int id)
        {
            _DbParameter[] pars ={
                                     new _DbParameter().Set("@ID", DbType.Int32, id)
                                 };
            int cId = 0;
            using (IDataReader reader = conn.ExecuteReader("SELECT [ColumnID] FROM [Post] WHERE [ID] = @ID", pars))
            {
                if (reader.Read()) { cId = reader.GetInt32(0); }
            }
            conn.ExecuteNonQuery("DELETE FROM [Comment] WHERE [PostID] = @ID", pars);
            conn.ExecuteNonQuery("DELETE FROM [Post] WHERE [ID] = @ID", pars);
            if (cId > 0)
            {
                conn.ExecuteNonQuery(string.Format("UPDATE [Column] SET [PostCount] = [PostCount] - 1 WHERE [ID] = {0}", cId));
            }
            return id;
        }

        /// <summary>
        /// 增加文章投票
        /// </summary>
        /// <param name="id">文章编号</param>
        /// <returns>返回统计数量</returns>
        public int AddPostVote(int id)
        {
            int voteCount = 0;
            conn.ExecuteNonQuery(string.Format("UPDATE [Post] SET [Vote] = [Vote] + 1 WHERE [ID] = {0}", id));
            voteCount = Convert.ToInt32(conn.ExecuteScalar(string.Format("SELECT [Vote] FROM [Post] WHERE [ID] = {0}", id)));
            return voteCount;
        }

        /// <summary>
        /// 增加文章阅读数
        /// </summary>
        /// <param name="id">文章编号</param>
        /// <returns>返回被更新的文章编号</returns>
        public int AddPostReader(int id)
        {
            conn.ExecuteNonQuery(string.Format("UPDATE [Post] SET [Reader] = [Reader] + 1 WHERE [ID] = {0}", id));
            return id;
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
                                   new _DbParameter().Set("@local", DbType.String, local),
                                   new _DbParameter().Set("@OurID", DbType.Int32, ourId)
                               };
            if (Convert.ToInt32(conn.ExecuteScalar(string.Format("SELECT COUNT([ID]) FROM [Post] WHERE [Local] = @Local{0};", (ourId == 0 ? string.Empty : " AND [ID] <> @OurID")), pars)) > 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 检查文章是否存在
        /// </summary>
        /// <param name="id">文章编号</param>
        /// <returns>返回是否存在的布尔型数据</returns>
        public bool ExistsPost(int id)
        {
            bool result = false;
            if (Convert.ToInt32(conn.ExecuteScalar(string.Format("SELECT COUNT([ID]) FROM [Post] WHERE [ID] = {0}", id))) > 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 更新文章的推荐、显示状态
        /// </summary>
        /// <param name="id">文章编号</param>
        /// <param name="fine">是否推荐</param>
        /// <param name="show">是否显示</param>
        public void UpdatePostFineAndShow(int id, bool fine, bool show)
        {
            _DbParameter[] pars ={
                                    new _DbParameter().Set("@Fine", DbType.Int32, fine ? 1 : 0),
                                    new _DbParameter().Set("@Show", DbType.Int32, show ? 1 : 0),
                                    new _DbParameter().Set("@ID", DbType.Int32, id)
                                };
            conn.ExecuteNonQuery("UPDATE [Post] SET [Fine] = @Fine, [Show] = @Show WHERE [ID] = @ID", pars);
        }

        /// <summary>
        /// 更新文章的推荐、显示状态
        /// </summary>
        /// <param name="id">文章编号</param>
        /// <param name="show">是否显示</param>
        public void UpdatePostShow(int id, bool show)
        {
            _DbParameter[] pars ={
                                    new _DbParameter().Set("@Show", DbType.Int32, show ? 1 : 0),
                                    new _DbParameter().Set("@ID", DbType.Int32, id)
                                };
            conn.ExecuteNonQuery("UPDATE [Post] SET [Show] = @Show WHERE [ID] = @ID", pars);
        }

        /// <summary>
        /// 移动文章分类
        /// </summary>
        /// <param name="oldCid">原本分类编号</param>
        /// <param name="newCid">新的分类编号</param>
        /// <returns>返回移动数量</returns>
        public int MovePost(int oldCid, int newCid)
        {
            int result = 0;
            result += conn.ExecuteNonQuery(string.Format("UPDATE [Post] SET [ColumnID] = {1} WHERE [ColumnID] = {0}", oldCid, newCid));
            int count = Convert.ToInt32(conn.ExecuteScalar(string.Format("SELECT COUNT([ID]) FROM [Post] WHERE [ColumnID] = {0}", oldCid)));
            conn.ExecuteNonQuery(string.Format("UPDATE [Column] SET [PostCount] = {1} WHERE [ID] = {0}", oldCid, count));
            count = Convert.ToInt32(conn.ExecuteScalar(string.Format("SELECT COUNT([ID]) FROM [Post] WHERE [ColumnID] = {0}", newCid)));
            conn.ExecuteNonQuery(string.Format("UPDATE [Column] SET [PostCount] = {1} WHERE [ID] = {0}", newCid, count));
            return result;
        }
    }
}
