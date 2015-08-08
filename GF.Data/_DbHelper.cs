using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace GF.Data
{
    public enum DbMode
    {
        /// <summary>
        /// Microsoft Office Access 中小型个人级数据库（默认）
        /// </summary>
        Access,
        /// <summary>
        /// Microsoft SQL Server 中大型企业级数据库（需强制设置）
        /// </summary>
        MSSQL
    }

    public class _DbHelper:IDisposable 
    {
        private IDbConnection _conn;
        private IDbCommand _cmd;
        private IDbTransaction _tan;
        private bool _isTan = false;
        private string _connStr;
        private DbMode _dbMode = DbMode.Access;
        private int _exeCount = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conn">连接字符串</param>
        public _DbHelper(string conn)
        {
            _connStr = conn;
            if (ConfigurationManager.AppSettings["DBMode"].ToUpper() == "MSSQL") { _dbMode = DbMode.MSSQL; }
            else { _dbMode = DbMode.Access; }
        }

        /// <summary>
        /// 当前数据库引擎
        /// </summary>
        public DbMode DBMode
        {
            get { return _dbMode; }
        }

        /// <summary>
        /// 打开数据库
        /// </summary>
        public void Open()
        {
            if (DBMode == DbMode.MSSQL) { _conn = new SqlConnection(_connStr); }
            else { _conn = new OleDbConnection(_connStr); }
            _conn.Open();
            _cmd = _conn.CreateCommand();
        }

        /// <summary>
        /// 获取语句执行次数
        /// </summary>
        public int ExecuteCount { get { return _exeCount; } }

        /// <summary>
        /// 关闭数据库
        /// </summary>
        public void Close()
        {
            _cmd.Dispose();
            _conn.Close();
        }

        /// <summary>
        /// 开始事务操作
        /// </summary>
        public void BeginTransaction()
        {
            _tan = _conn.BeginTransaction();
            _cmd.Transaction = _tan;
            _isTan = true;
        }

        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="sql">语句</param>
        /// <param name="pars">参数</param>
        /// <returns>影响的行数</returns>
        public int ExecuteNonQuery(string sql, _DbParameter[] pars)
        {
            _cmd.Parameters.Clear();
            if (pars != null)
            {
                foreach (_DbParameter item in pars)
                {
                    IDataParameter par = _cmd.CreateParameter();
                    par.ParameterName = item.Name;
                    par.DbType = item.Type;
                    par.Value = item.Value;
                    _cmd.Parameters.Add(par);
                }
            }
            _cmd.CommandText = sql;
            _exeCount++;
            return _cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="sql">语句</param>
        /// <returns>影响的行数</returns>
        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, null);
        }

        /// <summary>
        /// 执行数据阅读
        /// </summary>
        /// <param name="sql">语句</param>
        /// <param name="pars">参数</param>
        /// <param name="limit">数量</param>
        /// <returns>阅读对象</returns>
        public IDataReader ExecuteReader(string sql, _DbParameter[] pars, int limit)
        {
            IDataReader reader;
            using (IDbCommand cmd = _conn.CreateCommand())
            {
                if (_isTan) { cmd.Transaction = _tan; }
                if (pars != null)
                {
                    foreach (_DbParameter item in pars)
                    {
                        IDataParameter par = _cmd.CreateParameter();
                        par.ParameterName = item.Name;
                        par.DbType = item.Type;
                        par.Value = item.Value;
                        cmd.Parameters.Add(par);
                    }
                }
                if (!string.IsNullOrEmpty(sql) && limit > 0) { sql = sql.ToUpper().Replace("SELECT ", string.Format("SELECT TOP {0} ", limit)); }
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
            }
            _exeCount++;
            return reader;
        }

        /// <summary>
        /// 执行数据阅读
        /// </summary>
        /// <param name="sql">语句</param>
        /// <param name="limit">数量</param>
        /// <returns>阅读对象</returns>
        public IDataReader ExecuteReader(string sql, int limit)
        {
            return ExecuteReader(sql, null, limit);
        }

        /// <summary>
        /// 执行数据阅读
        /// </summary>
        /// <param name="sql">语句</param>
        /// <param name="pars">参数</param>
        /// <returns>阅读对象</returns>
        public IDataReader ExecuteReader(string sql, _DbParameter[] pars)
        {
            return ExecuteReader(sql, pars, 0);
        }

        /// <summary>
        /// 执行数据阅读
        /// </summary>
        /// <param name="sql">语句</param>
        /// <returns>阅读对象</returns>
        public IDataReader ExecuteReader(string sql)
        {
            return ExecuteReader(sql, null, 0);
        }

        /// <summary>
        /// 执行语句并返回第一行第一列的数据
        /// </summary>
        /// <param name="sql">语句</param>
        /// <param name="pars">参数</param>
        /// <returns>第一行第一列的数据</returns>
        public object ExecuteScalar(string sql, _DbParameter[] pars)
        {
            _cmd.Parameters.Clear();
            if (pars != null)
            {
                foreach (_DbParameter item in pars)
                {
                    IDataParameter par = _cmd.CreateParameter();
                    par.ParameterName = item.Name;
                    par.DbType = item.Type;
                    par.Value = item.Value;
                    _cmd.Parameters.Add(par);
                }
            }
            _cmd.CommandText = sql;
            _exeCount++;
            return _cmd.ExecuteScalar();
        }

        /// <summary>
        /// 执行语句并返回第一行第一列的数据
        /// </summary>
        /// <param name="sql">语句</param>
        /// <returns>第一行第一列的数据</returns>
        public object ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql, null);
        }

        /// <summary>
        /// 配合分页使用的数据阅读
        /// </summary>
        /// <param name="tb">数据表名</param>
        /// <param name="fieldList">字段列表</param>
        /// <param name="where">条件，不含 WHERE 关键字</param>
        /// <param name="orderBy">排序针对字段</param>
        /// <param name="orderMode">排序模式：ASC 或 DESC</param>
        /// <param name="pageThis">当前分页，大于零</param>
        /// <param name="pageSize">分页大小，大于零</param>
        /// <param name="pars">参数列表</param>
        /// <param name="rows">当前语句的处理行数</param>
        /// <returns>数据阅读器</returns>
        public IDataReader ExecutePager(string tb, string fieldList, string where, string orderBy, string orderMode, int pageThis, int pageSize, _DbParameter[] pars, ref int rows)
        {
            if (string.IsNullOrEmpty(orderBy)) { orderBy = "ID"; }
            if (string.IsNullOrEmpty(orderMode)) { orderMode = "DESC"; }
            string orderModeBack = "ASC";
            if (!string.IsNullOrEmpty(orderMode) && orderMode.ToUpper() == "ASC") { orderModeBack = "DESC"; }
            _cmd.Parameters.Clear();
            if (pars != null)
            {
                foreach (_DbParameter item in pars)
                {
                    IDataParameter par = _cmd.CreateParameter();
                    par.ParameterName = item.Name;
                    par.DbType = item.Type;
                    par.Value = item.Value;
                    _cmd.Parameters.Add(par);
                }
            }
            if (pageThis < 1) { pageThis = 1; }
            if (!string.IsNullOrEmpty(where))
            {
                where = string.Format(" WHERE {0}", where);
            }
            _cmd.CommandText = string.Format("SELECT COUNT([ID]) FROM {0}{1}", tb, where);
            rows = Convert.ToInt32(_cmd.ExecuteScalar());
            //构建SQL语句
            string sql = "SELECT * FROM (";
            sql += "SELECT TOP {6} * FROM (";
            sql += "SELECT TOP {7} {5} FROM {0}{1} ORDER BY {2} {3}, [ID] {3}";
            sql += ") {0} ORDER BY {2} {4}, [ID] {3}";
            sql += ") {0} ORDER BY {2} {3}, [ID] {3}";
            _cmd.CommandText = string.Format(sql, tb, where, orderBy, orderMode, orderModeBack, fieldList, pageSize, pageThis * pageSize);
            _exeCount++;
            return _cmd.ExecuteReader();
        }

        /// <summary>
        /// 配合分页使用的数据阅读
        /// </summary>
        /// <param name="tb">数据表名</param>
        /// <param name="fieldList">字段列表</param>
        /// <param name="where">条件，不含 WHERE 关键字</param>
        /// <param name="orderBy">排序针对字段</param>
        /// <param name="orderMode">排序模式：ASC 或 DESC</param>
        /// <param name="pageThis">当前分页，大于零</param>
        /// <param name="pageSize">分页大小，大于零</param>
        /// <param name="rows">当前语句的处理行数</param>
        /// <returns>数据阅读器</returns>
        public IDataReader ExecutePager(string tb, string fieldList, string where, string orderBy, string orderMode, int pageThis, int pageSize, ref int rows)
        {
            return ExecutePager(tb, fieldList, where, orderBy, orderMode, pageThis, pageSize, null, ref rows);
        }

        /// <summary>
        /// 执行语句返回数据集
        /// </summary>
        /// <param name="sql">语句</param>
        /// <returns>数据集</returns>
        public DataSet ExecuteDataSet(string sql)
        {
            DataSet result = new DataSet();
            _cmd.CommandText = sql;
            if (DBMode == DbMode.MSSQL)
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = (SqlCommand)_cmd;
                    da.Fill(result);
                }
            }
            else
            {
                using (OleDbDataAdapter da = new OleDbDataAdapter())
                {
                    da.SelectCommand = (OleDbCommand)_cmd;
                    da.Fill(result);
                }
            }
            _exeCount++;
            return result;
        }

        /// <summary>
        /// 获取最后添加的数据字段
        /// </summary>
        /// <param name="tb">表名</param>
        /// <param name="field">字段</param>
        /// <param name="orderBy">以什么字段作为索引</param>
        /// <returns>返回的最新数据的字段值</returns>
        public object ExecuteNewField(string tb, string field, string orderBy)
        {
            if (string.IsNullOrEmpty(field)) { field = "ID"; }
            if (string.IsNullOrEmpty(orderBy)) { orderBy = "ID"; }
            _cmd.CommandText = string.Format("SELECT TOP 1 {1} FROM {0} ORDER BY {2} DESC", tb, field, orderBy);
            _exeCount++;
            return _cmd.ExecuteScalar();
        }

        /// <summary>
        /// 获取最后添加的数据字段
        /// </summary>
        /// <param name="tb">表名</param>
        /// <returns>返回的最新数据的字段值</returns>
        public object ExecuteNewField(string tb)
        {
            return ExecuteNewField(tb, null, null);
        }

        /// <summary>
        /// 提交数据库事务
        /// </summary>
        public void Commit()
        {
            _tan.Commit();
        }

        /// <summary>
        /// 从挂起状态回滚事务
        /// </summary>
        public void Rollback() { _tan.Rollback(); }

        /// <summary>
        /// 释放数据库资源
        /// </summary>
        public void Dispose()
        {
            if (_conn.State == ConnectionState.Open) { Close(); }
            _conn.Dispose();
        }

    }
}
