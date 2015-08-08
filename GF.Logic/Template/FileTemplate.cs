using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace GF.Logic.Template
{
    /// <summary>
    /// 简单模板类
    /// </summary>
    public class FileTemplate
    {
        private string _code = string.Empty;
        private string _pf = @"{$";
        private string _sf = @"}";

        /// <summary>
        /// 取得当前代码
        /// </summary>
        public string Code
        {
            get { return this._code; }
        }

        /// <summary>
        /// 读取文件代码
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public void LoadFile(string filePath)
        {
            this._code = File.ReadAllText(filePath);
        }

        /// <summary>
        /// 设置代码
        /// </summary>
        /// <param name="code">代码</param>
        public void LoadCode(string code)
        {
            this._code = code;
        }

        /// <summary>
        /// 设置一个标签项
        /// </summary>
        /// <param name="tag">标签名</param>
        /// <param name="html">代码</param>
        public void SetTag(string tag, string html)
        {
            if (!string.IsNullOrEmpty(this._code))
            {
                this._code = this._code.Replace(string.Format("{0}{1}{2}", this._pf, tag, this._sf), html);
            }
        }

        /// <summary>
        /// 直接打印代码
        /// </summary>
        public void Print()
        {
            HttpContext.Current.Response.Write(this._code);
        }
    }
}
