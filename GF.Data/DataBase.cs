using System;
using System.Text;
using System.Web;
using System.Data;
using System.Collections.Generic;

namespace GF.Data
{
    public class DataBase
    {
        private static string _conn = string.Empty;

        /// <summary>
        /// 获取或设置连接信息
        /// </summary>
        public static string Conn
        {
            get { return _conn; }
            set
            {
                _conn = value;
                initConn();
            }
        }

        public static string SqlEncode(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("'", "&#39;");
                str = HttpContext.Current.Server.HtmlEncode(str);
            }
            return str;
        }

        /// <summary>
        /// 设置连接时的初始化工作
        /// </summary>
        private static void initConn()
        {
            using (_DbHelper conn = new _DbHelper(_conn))
            {
                conn.Open();
                conn.BeginTransaction();
                try
                {
                    if (Convert.ToInt32(conn.ExecuteScalar("SELECT COUNT([ID]) FROM [Setting]")) == 0)
                    {
                        Dictionary<string, object> vals = new Dictionary<string, object>();
                        vals.Add("Name", "'我的网站'");
                        vals.Add("URL", "'http://www.GF.com/'");
                        vals.Add("ICP", "'备案号'");
                        vals.Add("Language", "'zh-CN'");
                        vals.Add("Theme", "'Amarketer-blue'");
                        vals.Add("Intro", "'我的个人网站'");
                        vals.Add("Keywords", "''");
                        vals.Add("Affiche", "'无'");
                        vals.Add("Filter", "'你妈,我操,日你,贱人,打飞机,干你'");
                        vals.Add("UploadExt", "'.ZIP,.RAR,.PNG,.JPG,.GIF'");
                        vals.Add("Mail", "''");
                        vals.Add("MailFrom", "''");
                        vals.Add("MailHost", "''");
                        vals.Add("MailPort", 0);
                        vals.Add("MailUID", "''");
                        vals.Add("MailPWD", "''");
                        vals.Add("WatermarkSeat", 0);
                        vals.Add("ParArticleNum", 8);
                        vals.Add("ParCommentNum", 10);
                        vals.Add("ParAppendFineArticleNum", 5);
                        vals.Add("ParAppendRandomArticleNum", 5);
                        vals.Add("ParAppendHotArticleNum", 5);
                        vals.Add("ParAppendHotTagNum", 10);
                        vals.Add("ParAppendNewCommentNum", 5);
                        vals.Add("RssNum", 50);
                        vals.Add("RssMode", 0);
                        vals.Add("WatermarkPath", "'Common/Images/Watermark.png'");
                        vals.Add("Key", "''");
                        string fields = string.Empty;
                        string values = string.Empty;
                        foreach (KeyValuePair<string, object> pair in vals)
                        {
                            fields = fields + string.Format("[{0}],", pair.Key);
                            values = values + string.Format("{0},", pair.Value);
                        }
                        fields = fields.Substring(0, fields.Length - 1);
                        values = values.Substring(0, values.Length - 1);
                        conn.ExecuteNonQuery(string.Format("INSERT INTO [Setting] ({0}) VALUES ({1})", fields, values));
                    }
                    conn.Commit();
                }
                catch (Exception err) { conn.Rollback(); throw err; }
                conn.Close();
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL</param>
        public DataSet ExecSql(string sql)
        {
            DataSet result = new DataSet();
            using (_DbHelper conn = new _DbHelper(_conn))
            {
                conn.Open();
                try { result = conn.ExecuteDataSet(sql); }
                catch (Exception err) { throw err; }
                finally { conn.Close(); }
            }
            return result;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sqls">SQL</param>
        public int ExecSql(string[] sqls)
        {
            int result = 0;
            using (_DbHelper conn = new _DbHelper(_conn))
            {
                conn.Open();
                conn.BeginTransaction();
                try
                {
                    foreach (string sql in sqls)
                    {
                        result += conn.ExecuteNonQuery(sql);
                    }
                    conn.Commit();
                }
                catch (Exception err) { conn.Rollback(); throw err; }
                finally { conn.Close(); }
            }
            return result;
        }

        /// <summary>
        /// 执行SQL并返回第一行第一列的数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExeSqlScalar(string sql)
        {
            object result = new object();
            using (_DbHelper conn = new _DbHelper(_conn))
            {
                conn.Open();
                result = conn.ExecuteScalar(sql);
                conn.Close();
            }
            return result;
        }
    }
}
