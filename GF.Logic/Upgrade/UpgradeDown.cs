using System.IO;
using System.Net;
using GF.Core;

namespace GF.Logic.Upgrade
{
    /// <summary>
    /// 更新文件下载类
    /// </summary>
    public class UpgradeDown
    {
        private string _downPath;
        private string _savaPath;

        /// <summary>
        /// 析构函数
        /// </summary>
        /// <param name="ver">版本号</param>
        /// <param name="downPath">相对路径</param>
        /// <param name="savePath">保存位置</param>
        public UpgradeDown(string ver, string downPath, string savePath)
        {
            _downPath = string.Format("{0}_GF/{1}/{2}", SiteCfg.UpdateHost, ver, downPath);
            _savaPath = savePath;
        }

        /// <summary>
        /// 是否成功创建文件
        /// </summary>
        /// <returns>返回是否成功创建</returns>
        private bool CreatFile()
        {
            bool yes = false;
            try
            {
                //检查储存路径是否存在，如果存在就删除文件
                if (File.Exists(_savaPath)) { File.Delete(_savaPath); }
                //创建保存文件
                Stream str = File.Create(_savaPath);
                str.Close();
                yes = true;
            }
            catch { }
            return yes;
        }

        /// <summary>
        /// 取得要下载的文件总长度
        /// </summary>
        /// <returns>返回长度数值</returns>
        public long GetLengh
        {
            get
            {
                WebRequest request = WebRequest.Create(_downPath);
                WebResponse wrp = (WebResponse)request.GetResponse();
                return wrp.ContentLength;
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns>返回是否下载成功</returns>
        public void Down()
        {
            //如果创建保存文件成功则继续执行下载
            if (CreatFile())
            {
                WebClient client = new WebClient();
                Stream str = client.OpenRead(_downPath);
                byte[] mbyte = new byte[1024];
                int read = str.Read(mbyte, 0, 1024);
                Stream fs = new FileStream(_savaPath, FileMode.OpenOrCreate, FileAccess.Write);
                while (read > 0)
                {
                    fs.Write(mbyte, 0, read);
                    read = str.Read(mbyte, 0, 1024);
                }
                str.Close();
                fs.Close();
            }
        }
    }
}
