using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace GF.Core
{
    public class SiteXml
    {
        // Fields
        private XmlDocument _xml = new XmlDocument();

        /// <summary>
        /// 载入文档
        /// </summary>
        /// <param name="oStm">流</param>
        public void LoadOfStream(Stream oStm)
        {
            _xml.Load(oStm);
        }

        /// <summary>
        /// 载入文档
        /// </summary>
        /// <param name="xml">XML内容</param>
        public void LoadOfXml(string xml)
        {
            _xml.LoadXml(xml);
        }

        /// <summary>
        /// 载入文档
        /// </summary>
        /// <param name="uri">XML文件的地址，可以是网络地址</param>
        public void LoadOfUri(string uri)
        {
            _xml.Load(uri);
        }

        /// <summary>
        /// 获取节点值
        /// </summary>
        /// <param name="sVal">节点路径</param>
        /// <returns>返回节点值</returns>
        private string GetNote(string sVal)
        {
            XmlNode node = _xml.SelectSingleNode(sVal);
            if (node != null) { return node.InnerText; }
            else { return string.Empty; }
        }

        /// <summary>
        /// 获取XML列表
        /// </summary>
        /// <param name="sVal"></param>
        /// <returns></returns>
        public XmlNodeList SelecttNodes(string sVal)
        {
            XmlNodeList list = _xml.SelectNodes(sVal);
            return list;
        }

        /// <summary>
        /// 获取布尔值数据
        /// </summary>
        /// <param name="sVal">节点路径</param>
        /// <returns>返回真假型数据</returns>
        public bool GetBool(string sVal)
        {
            return SiteFun.ToBool(this.GetNote(sVal));
        }

        /// <summary>
        /// 获取日期型数据
        /// </summary>
        /// <param name="sVal">节点路径</param>
        /// <returns>返回日期数据</returns>
        public DateTime GetDate(string sVal)
        {
            return SiteFun.ToDate(this.GetNote(sVal));
        }

        /// <summary>
        /// 获取整型数据
        /// </summary>
        /// <param name="sVal">节点路径</param>
        /// <returns>返回整型数据</returns>
        public int GetInt(string sVal)
        {
            return SiteFun.ToInt(this.GetNote(sVal));
        }

        /// <summary>
        /// 获取数据流
        /// </summary>
        /// <param name="sVal">节点路径</param>
        /// <returns>返回节点值</returns>
        public Stream GetStream(string sVal)
        {
            return new MemoryStream(Convert.FromBase64String(this.GetNote(sVal)));
        }

        /// <summary>
        /// 获取字符串数据
        /// </summary>
        /// <param name="sVal">节点路径</param>
        /// <returns>返回字符串数据</returns>
        public string GetString(string sVal)
        {
            return GetNote(sVal);
        }

        /// <summary>
        /// 获取当前XML内容
        /// </summary>
        public string GetXml
        {
            get { return _xml.OuterXml; }
        }
    }
}
