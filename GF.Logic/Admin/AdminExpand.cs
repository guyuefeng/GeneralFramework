using System.Collections;
using GF.Core;

namespace GF.Logic.Admin
{
    public class AdminExpand
    {
        /// <summary>
        /// 按自编号写出扩展管理区内容
        /// </summary>
        /// <param name="key">自编号</param>
        /// <returns>管理区内容</returns>
        public string OutWrite(string key)
        {
            string result = string.Empty;
            foreach (object type in new SiteExpand().GetTypes(typeof(IAdminExpand).FullName))
            {
                IAdminExpand iae = ((IAdminExpand)type);
                if (iae.Key == key) { return iae.OutWrite(); }
            }
            return result;
        }
    }
}
