namespace GF.Logic.Web
{
    /// <summary>
    /// 前台页面扩展
    /// </summary>
    public interface IWebExpand
    {
        /// <summary>
        /// 关键字，节点路径为：Default.aspx?act=expand&amp;key=关键字
        /// </summary>
        string Key { get; }

        /// <summary>
        /// 扩展名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 样式表地址
        /// </summary>
        string Css { get; }

        /// <summary>
        /// 导出当前文件夹名称（利用 SettingData 进行导出）
        /// </summary>
        string OutTheme { get; }

        /// <summary>
        /// 模板文件名（例如“Expand.MyBook”，不含后缀）
        /// </summary>
        string TemplateFile { get; }

        /// <summary>
        /// 扩展页面的显示HTML
        /// </summary>
        /// <returns>HTML的部分内容</returns>
        string OutHtml();
    }
}
