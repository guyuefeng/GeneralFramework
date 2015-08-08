using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GF.Data.Entity;

namespace GF.Data
{
    public class TrackbackLogData
    {
        private _DbHelper conn;

        public TrackbackLogData(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 添加一个发送信息
        /// </summary>
        /// <param name="value">引用通告数据</param>
        /// <returns>返回新增的引用通告编号</returns>
        public int InsertTrackbackLog(TrackbackLogItem value)
        {
            int id = 0;
            _DbParameter[] pars ={
                                    new _DbParameter().Set("@Error", DbType.Int32, value.Error ? 1 : 0),
                                    new _DbParameter().Set("@Msg", DbType.String, value.Message),
                                    new _DbParameter().Set("@URL", DbType.String, value.URL),
                                    new _DbParameter().Set("@Publish", DbType.String, value.Publish.ToString("yyyy-MM-dd HH:mm:ss"))
                                };
            conn.ExecuteNonQuery("INSERT INTO [TrackbackLog] ([Error], [Message], [URL], [Publish]) VALUES (@Error, @Msg, @URL, @Publish)", pars);
            id = Convert.ToInt32(conn.ExecuteNewField("[TrackbackLog]", null, null));
            return id;
        }

        /// <summary>
        /// 删除一个引用通告记录
        /// </summary>
        /// <param name="id">记录编号</param>
        /// <returns>返回被删除的记录编号</returns>
        public int DeleteTrackbackLog(int id)
        {
            conn.ExecuteNonQuery(string.Format("DELETE FROM [TrackbackLog] WHERE [ID] = {0}", id));
            return id;
        }

        /// <summary>
        /// 清空引用通告记录
        /// </summary>
        /// <returns>返回影响的总行数</returns>
        public int ClearTrackbackLog()
        {
            int rows = 0;
            rows = conn.ExecuteNonQuery("DELETE FROM [TrackbackLog]");
            return rows;
        }

        /// <summary>
        /// 筛选引用通告记录
        /// </summary>
        /// <param name="intCurPage">当前页码</param>
        /// <param name="btePerPage">分页大小</param>
        /// <returns>返回引用通告记录数据列表</returns>
        public DataList<TrackbackLogItem> SelectTrackbackLog(int intCurPage, int btePerPage)
        {
            DataList<TrackbackLogItem> list = new DataList<TrackbackLogItem>();
            int rows = 0;
            using (IDataReader reader = conn.ExecutePager("[TrackbackLog]", "[ID], [Error], [Message], [URL], [Publish]", null, "[Publish]", "DESC", intCurPage, btePerPage, ref rows))
            {
                while (reader.Read())
                {
                    TrackbackLogItem item = new TrackbackLogItem();
                    item.ID = reader.GetInt32(0);
                    item.Error = reader.GetInt32(1) == 0 ? false : true;
                    item.Message = reader.GetString(2);
                    item.URL = reader.GetString(3);
                    item.Publish = reader.GetDateTime(4);
                    list.Add(item);
                }
            }
            list.Number = rows;
            return list;
        }
    }
}
