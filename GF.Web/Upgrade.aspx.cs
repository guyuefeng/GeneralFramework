using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using GF.Data;
using GF.Core;
using GF.Core.Compression;
using GF.Logic.Upgrade;

namespace GF.Web
{
    public partial class Upgrade : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string root = HttpContext.Current.Server.MapPath("/");
            if (SiteCfg.Token == SiteFun.Post("token"))
            {
                string ver = SiteFun.Post("ver");
                if (string.IsNullOrEmpty(ver)) { Response.Write("Version error..."); }
                else
                {
                    string offFilePath = Path.Combine(root, "app_offline.htm");
                    try
                    {
                        UpgradeCheck uc = new UpgradeCheck(ver);
                        //Common/Temp/Upgrade
                        string packDir = Path.Combine(SiteCfg.Router, "Common/Temp/Upgrade");
                        if (!Directory.Exists(packDir)) { Directory.CreateDirectory(packDir); }
                        //1.1.0.gzip
                        string packName = ver + ".gzip";
                        //Common/Temp/Upgrade/1.1.0.gzip
                        string packFile = Path.Combine(packDir, packName);
                        switch (SiteFun.Query("act"))
                        {
                            case "down":
                                {
                                    UpgradeDown down = new UpgradeDown(ver, uc.PackFile, packFile);
                                    down.Down();
                                    break;
                                }
                            case "inst":
                                {
                                    File.WriteAllText(offFilePath, string.Format("{0} v{1} to {2} upgrading...", SiteCfg.System, SiteCfg.Version, uc.Version));
                                    if (!string.IsNullOrEmpty(uc.SQL))
                                    {
                                        DataBase.Conn = SiteCfg.Conn;
                                        string[] sqls = uc.SQL.Split(new string[] { ";\n" }, StringSplitOptions.None);
                                        new DataBase().ExecSql(sqls);
                                    }
                                    //common/temp/upgrade/1.0.0
                                    string deDir = Path.Combine(packDir, ver);
                                    if (!Directory.Exists(deDir)) { Directory.CreateDirectory(deDir); }
                                    GZipResult result = new GZip().Decompress(packDir, deDir, packName);
                                    //复制文件
                                    foreach (string file in Directory.GetFiles(deDir))
                                    {
                                        FileInfo fi = new FileInfo(file);
                                        string copyPath = Path.Combine(SiteCfg.Router, fi.Name);
                                        if (file.EndsWith("Web.config", true, null)) { copyPath = Path.Combine(root, fi.Name); }
                                        File.Copy(file, copyPath, true);
                                    }
                                    SiteIO sio = new SiteIO();
                                    //复制文件夹
                                    foreach (string dir in Directory.GetDirectories(deDir))
                                    {
                                        DirectoryInfo di = new DirectoryInfo(dir);
                                        string copyPath = Path.Combine(SiteCfg.Router, di.Name);
                                        if (dir.EndsWith("\\bin", true, null)) { Path.Combine(root, di.Name); }
                                        sio.CopyFolder(dir, copyPath);
                                    }
                                    //移除文件或文件夹
                                    foreach (UpgradeFileEntity item in uc.GetRemoveList)
                                    {
                                        string fileName = Path.Combine(SiteCfg.Router, item.FilePath);
                                        if (item.FilePath.ToUpper() == "BIN" || item.FilePath.StartsWith("bin/", true, null) || item.FilePath.EndsWith("Web.config", true, null))
                                        {
                                            fileName = Path.Combine(root, item.FilePath);
                                        }
                                        FileInfo info = new FileInfo(fileName);
                                        if (info.Attributes == FileAttributes.Directory) { Directory.Delete(fileName, true); }
                                        else { File.Delete(fileName); }
                                    }
                                    //File.Delete(Path.Combine(root, offFileName));
                                    if (Directory.Exists(packDir)) { Directory.Delete(packDir, true); }
                                }
                                break;
                        }
                        Response.Write("1");
                    }
                    catch (Exception err) { Response.Write(err.Message); }
                    if (File.Exists(offFilePath)) { File.Delete(offFilePath); }
                }
            }
            else { Response.Write("Token error..."); }
        }
    }
}
