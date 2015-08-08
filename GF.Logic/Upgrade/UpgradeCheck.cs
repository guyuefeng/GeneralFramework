using System.Collections.Generic;
using System.Xml;
using GF.Core;

namespace GF.Logic.Upgrade
{
    /// <summary>
    /// 更新检查类
    /// </summary>
    public class UpgradeCheck
    {
        private string _xmlUri = string.Empty;
        private string _newVer = string.Empty;
        private string _packFile = string.Empty;
        private List<UpgradeFileEntity> _list = new List<UpgradeFileEntity>();
        private List<UpgradeFileEntity> _remove = new List<UpgradeFileEntity>();
        private string _intro = string.Empty;
        private string _sql = string.Empty;

        /// <summary>
        /// 检查更新
        /// </summary>
        /// <param name="ver"></param>
        public UpgradeCheck(string ver)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //载入升级配置文件
            _xmlUri = string.Format("{0}_GF/{1}/Files.xml", SiteCfg.UpdateHost, ver);
            xmlDoc.Load(_xmlUri);
            //获取即将更新到某个版本号
            _newVer = xmlDoc.SelectSingleNode("/upgrade/version").InnerText;
            //重要！获取更新文件压缩包，GZIP类型
            if (xmlDoc.SelectNodes("/upgrade/file").Count > 0)
            {
                _packFile = xmlDoc.SelectSingleNode("/upgrade/file").InnerText;
            }
            //获取更新说明
            if (xmlDoc.SelectNodes("/upgrade/intro").Count > 0)
            {
                _intro = xmlDoc.SelectSingleNode("/upgrade/intro").InnerText;
            }
            //获取数据库升级语句
            if (xmlDoc.SelectNodes("/upgrade/sql").Count > 0)
            {
                _sql = xmlDoc.SelectSingleNode("/upgrade/sql").InnerText;
            }
            //获取需要更新的文件列表
            if (xmlDoc.SelectNodes("/upgrade/list").Count > 0 && xmlDoc.SelectNodes("/upgrade/list/item").Count > 0)
            {
                foreach (XmlNode node in xmlDoc.SelectNodes("/upgrade/list/item"))
                {
                    string fPath = node.Attributes["path"].Value;
                    if (!string.IsNullOrEmpty(fPath))
                    {
                        _list.Add(new UpgradeFileEntity() { FilePath = fPath });
                    }
                }
            }
            //获取要删除的文件列表
            if (xmlDoc.SelectNodes("/upgrade/remove").Count > 0 && xmlDoc.SelectNodes("/upgrade/remove/item").Count > 0)
            {
                foreach (XmlNode node in xmlDoc.SelectNodes("/upgrade/remove/item"))
                {
                    string fPath = node.Attributes["path"].Value;
                    if (!string.IsNullOrEmpty(fPath))
                    {
                        _remove.Add(new UpgradeFileEntity() { FilePath = fPath });
                    }
                }
            }
        }

        /// <summary>
        /// 即将更新的版本号
        /// </summary>
        public string Version
        {
            get { return _newVer; }
        }

        /// <summary>
        /// 更新说明
        /// </summary>
        public string Intro
        {
            get { return _intro; }
        }

        /// <summary>
        /// 要执行的SQL语句
        /// </summary>
        public string SQL
        {
            get { return _sql; }
        }

        /// <summary>
        /// 获取XML地址
        /// </summary>
        public string XmlUri { get { return _xmlUri; } }

        /// <summary>
        /// 压缩文件路径
        /// </summary>
        public string PackFile
        {
            get { return _packFile; }
        }

        /// <summary>
        /// 更新文件列表
        /// </summary>
        /// <returns>返回需要更新的文件列表</returns>
        public List<UpgradeFileEntity> GetFileList
        {
            get { return _list; }
        }

        /// <summary>
        /// 移除文件列表
        /// </summary>
        /// <returns>返回要移除的文件列表</returns>
        public List<UpgradeFileEntity> GetRemoveList
        {
            get { return _remove; }
        }
    }
}
