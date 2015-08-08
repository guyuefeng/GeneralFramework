using GF.Core;
using GF.Data;

namespace GF.Logic.Service
{
    /// <summary>
    /// 处理基础类
    /// </summary>
    public class _ServiceBase
    {
        /// <summary>
        /// 初始化数据库连接
        /// </summary>
        public void InitConn()
        {
            DataBase.Conn = SiteCfg.Conn;
        }
    }
}