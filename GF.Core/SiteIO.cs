using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace GF.Core
{
    public class SiteIO
    {
        /// <summary>
        /// 将整个文件夹复制到目标文件夹中。
        /// </summary>
        /// <param name="srcPath">源文件夹</param>
        /// <param name="aimPath">目标文件夹</param>
        public void CopyFolder(string srcPath, string aimPath)
        {
            // 检查目标目录是否以目录分割字符结束如果不是则添加之
            if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar) { aimPath += Path.DirectorySeparatorChar; }
            // 判断目标目录是否存在如果不存在则新建之
            if (!Directory.Exists(aimPath)) { Directory.CreateDirectory(aimPath); }
            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
            // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
            // string[] fileList = Directory.GetFiles(srcPath);
            string[] fileList = Directory.GetFileSystemEntries(srcPath);
            // 遍历所有的文件和目录
            foreach (string file in fileList)
            {
                // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                if (Directory.Exists(file)) { CopyFolder(file, aimPath + Path.GetFileName(file)); }
                // 否则直接Copy文件
                else { File.Copy(file, aimPath + Path.GetFileName(file), true); }
            }
        }

        /// <summary>
        /// 获取某个目录的大小
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns>大小字节</returns>
        public long GetDirectorySize(string path)
        {
            long result = 0;
            if (Directory.Exists(path))
            {
                foreach (string dir in Directory.GetDirectories(path))
                {
                    result += GetDirectorySize(dir);
                }
                foreach (string file in Directory.GetFiles(path))
                {
                    result += new FileInfo(file).Length;
                }
            }
            return result;
        }

        /// <summary>
        /// 远程下载
        /// </summary>
        /// <param name="uri">远程地址</param>
        /// <param name="savePath">保存地址</param>
        /// <returns>是否成功</returns>
        public bool RemoteDownload(string uri, string savePath)
        {
            bool result = false;
            try
            {
                //检查储存路径是否存在，如果存在就删除文件
                if (File.Exists(savePath)) { File.Delete(savePath); }
                //创建保存文件
                Stream saveFile = File.Create(savePath);
                saveFile.Close();
                //开始下载
                WebClient client = new WebClient();
                Stream remoteFile = client.OpenRead(uri);
                byte[] mbyte = new byte[1024];
                int read = remoteFile.Read(mbyte, 0, 1024);
                Stream fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write);
                while (read > 0)
                {
                    fs.Write(mbyte, 0, read);
                    read = remoteFile.Read(mbyte, 0, 1024);
                }
                remoteFile.Close();
                fs.Close();
                result = true;
            }
            catch { }
            return result;
        }
    }
}
