using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Reflection;


namespace GF.Core
{
    public class SiteExpand
    {
        /// <summary>
        /// 析构过程
        /// </summary>
        public SiteExpand()
        {
            /*if (!_initExpand)
            {
                if (Directory.Exists(Path.Combine(SiteCfg.Router, this._binDir)))
                {
                    Directory.Delete(Path.Combine(SiteCfg.Router, this._binDir), true);
                }
                Directory.CreateDirectory(Path.Combine(SiteCfg.Router, this._binDir));
                foreach (string file in Directory.GetFiles(Path.Combine(SiteCfg.Router, "Common/Expand")))
                {
                    string fileBaseName = new FileInfo(file).Name;
                    File.Copy(file, Path.Combine(SiteCfg.Router, string.Format("{0}/{1}", this._binDir, fileBaseName)));
                }
                _initExpand = true;
            }*/
        }

        /// <summary>
        /// 取得类型列表
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <param name="fileFormat">文件名筛选</param>
        /// <param name="interfaceName">接口完整名</param>
        /// <returns>类型对象列表</returns>
        public ArrayList GetTypes(string dirPath, string fileFormat, string interfaceName)
        {
            if (string.IsNullOrEmpty(fileFormat)) { fileFormat = "GF.Expand.*.dll"; }
            ArrayList result = new ArrayList();
            foreach (string file in Directory.GetFiles(dirPath, fileFormat))
            {
                Assembly asm = Assembly.LoadFrom(file);
                foreach (Type type in asm.GetExportedTypes())
                {
                    if (type.GetInterface(interfaceName) != null)
                    {
                        result.Add(Activator.CreateInstance(type));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 取得类型列表
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <param name="interfaceName">接口完整名</param>
        /// <returns>类型对象列表</returns>
        public ArrayList GetTypes(string dirPath, string interfaceName)
        {
            return GetTypes(dirPath, null, interfaceName);
        }

        /// <summary>
        /// 取得类型列表
        /// </summary>
        /// <param name="interfaceName">接口完整名</param>
        /// <returns>类型对象列表</returns>
        public ArrayList GetTypes(string interfaceName)
        {
            return GetTypes(System.Web.HttpContext.Current.Server.MapPath("/bin"), null, interfaceName);
        }

    }
}
