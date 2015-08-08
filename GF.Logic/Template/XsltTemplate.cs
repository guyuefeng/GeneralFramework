using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using GF.Core;
using System.IO;
using System.Diagnostics;
using System.Xml.Xsl;
using GF.Data;

namespace GF.Logic.Template
{
    public class XsltTemplate
    {
        private string _code;
        private string _path;
        private string _xml;
        private XsltArgumentList _argList;
        Stopwatch watch = new Stopwatch();
        private _DbHelper conn;

        /// <summary>
        /// 析构函数
        /// </summary>
        public XsltTemplate(_DbHelper c)
        {
            watch.Start();
            conn = c;
            _argList = new XsltArgumentList();
            _argList.AddExtensionObject("roclog:function", new _ALFun(conn));
            _argList.AddExtensionObject("sys:fun", new _ALFun(conn));
            //扩展部分
            foreach (object type in new SiteExpand().GetTypes(typeof(ITemplateFun).FullName))
            {
                ITemplateFun iae = ((ITemplateFun)type);
                _argList.AddExtensionObject(string.Format("expand:{0}", iae.URI), iae);
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        /// <param name="path">模板文件地址</param>
        public void LoadXslt(string path)
        {
            this._path = path;
        }

        /// <summary>
        /// 从文本XML绑定数据
        /// </summary>
        /// <param name="xmlStr">XML内容</param>
        public void BindXml(string xmlStr)
        {
            _xml = xmlStr;
            XslTransform xsl = new XslTransform();
            //XslCompiledTransform xsl = new XslCompiledTransform(false);
            xsl.Load(_path);
            //处理部分
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);
            StringBuilder strBuilder = new StringBuilder();
            xsl.Transform(xmlDoc, _argList, new StringWriter(strBuilder));
            _code = strBuilder.ToString();
        }

        /// <summary>
        /// 强制设置输出类型
        /// </summary>
        public void SetContentTypeHtml()
        {
            HttpContext.Current.Response.ContentType = "text/html";
        }

        /// <summary>
        /// 强制设置输出类型
        /// </summary>
        public void SetContentTypeXml()
        {
            HttpContext.Current.Response.ContentType = "text/xml";
        }

        /// <summary>
        /// 取得当前代码
        /// </summary>
        public string Code
        {
            get { return this._code; }
        }

        /// <summary>
        /// 取得当前XML代码
        /// </summary>
        public string XML
        {
            get { return this._xml; }
        }

        /// <summary>
        /// 直接向浏览器打印当前代码（已包含转换输出类型）
        /// </summary>
        /// <param name="isXml">是否是XML文件</param>
        public void Print(bool isXml)
        {
            if (isXml) { SetContentTypeXml(); } else { SetContentTypeHtml(); }
            if (SiteFun.Query("outxml") == "yes")
            {
                SetContentTypeXml();
                HttpContext.Current.Response.Write(_xml);
            }
            else
            {
                if (!string.IsNullOrEmpty(_code))
                {
                    string exeTime = string.Format("Processed in {0} second(s), {1} queries", watch.Elapsed.TotalSeconds.ToString("0.0000"), conn.ExecuteCount);
                    _code = _code.Replace("<roclog.debug />", exeTime);
                    _code = _code.Replace("<sys.debug />", exeTime);
                }
                HttpContext.Current.Response.Write(_code);
            }
        }

        /// <summary>
        /// 直接向浏览器打印当前代码（已包含转换输出类型）
        /// </summary>
        public void Print()
        {
            Print(false);
        }
    }
}
