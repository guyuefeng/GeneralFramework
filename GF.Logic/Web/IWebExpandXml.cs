namespace GF.Logic.Web
{
    /// <summary>
    /// 前台页面附加XML
    /// </summary>
    public interface IWebExpandXml
    {
        /// <summary>
        /// 关键字，节点路径为：/ui/expand/关键字
        /// </summary>
        string Key { get; }

        /// <summary>
        /// 扩展部分XML内容
        /// </summary>
        /// <returns>扩展部分的XML内容</returns>
        string OutXML();
    }
}
