using GF.Core;
using GF.Data;

namespace GF.Logic.Web
{
    public class _WebBase
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