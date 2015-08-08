namespace GF.Logic.Admin
{
    public interface IAdminExpand
    {
        /// <summary>
        /// 插件自编号（建议以“公司.部门.产品.功能”命名，例如：“GF.EXPAND.ARTICLE.CONTROL”）
        /// </summary>
        string Key { get; }

        /// <summary>
        /// 扩展名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 写出内容
        /// </summary>
        /// <returns>返回管理区代码</returns>
        string OutWrite();
    }
}
