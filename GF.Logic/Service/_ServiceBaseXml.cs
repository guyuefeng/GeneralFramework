using System.Text;
using GF.Core;

namespace GF.Logic.Service
{
    /// <summary>
    /// 异步处理基类
    /// </summary>
    public class _ServiceBaseXml
    {
        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="msg">消息文本</param>
        /// <param name="xmlStr">本次 XML 内容</param>
        /// <returns>完整的 XML 内容</returns>
        public string OutBaseXml(string msg, string xmlStr)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
            xml.Append("<xml>\n");
            xml.AppendFormat("\t<msg>{0}</msg>\n", SiteFun.CDATA(msg));
            xml.Append("\t<this>\n");
            xml.Append(xmlStr);
            xml.Append("\t</this>\n");
            xml.Append("</xml>\n");
            return xml.ToString();
        }
    }
}
