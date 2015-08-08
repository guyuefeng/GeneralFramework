using System;
using System.IO;
using System.Web;
using GF.Core;
using GF.Data;
using GF.Data.Entity;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 后台上传处理类
    /// </summary>
    public class _AdminUpload
    {
        private _DbHelper conn;

        public _AdminUpload(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 保存所有文件
        /// </summary>
        /// <param name="files">文件对象</param>
        /// <param name="content">内容（如果有的话）</param>
        /// <param name="explain">阐述（如果有的话）</param>
        /// <param name="wmPoint">水印位置</param>
        /// <param name="wmPath">水印文件路径</param>
        /// <returns>附件列表</returns>
        public string SaveAs(HttpFileCollection files, ref string content, ref string explain, int wmPoint, string wmPath)
        {
            string result = string.Empty;
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i] != null && files[i].ContentLength > 0)
                {
                    FileInfo tempFileInfo = new FileInfo(files[i].FileName);
                    if (!string.IsNullOrEmpty(tempFileInfo.Extension))
                    {
                        //保存文件
                        string saveFolder = "Attach/" + DateTime.Now.ToString("yyyyMM") + "/";
                        string savePath = saveFolder + DateTime.Now.ToString("ddHHmmss") + "-" + tempFileInfo.Name;
                        if (!Directory.Exists(Path.Combine(SiteCfg.Router, saveFolder))) { Directory.CreateDirectory(Path.Combine(SiteCfg.Router, saveFolder)); }
                        if (wmPoint > 0 && File.Exists(wmPath) && (tempFileInfo.Extension.ToLower() == ".bmp" || tempFileInfo.Extension.ToLower() == ".gif" || tempFileInfo.Extension.ToLower() == ".jpg" || tempFileInfo.Extension.ToLower() == ".png"))
                        {
                            files[i].SaveAs(Path.Combine(SiteCfg.Router, "Common/Temp/" + tempFileInfo.Name));
                            new SiteWatermark().HandleImage(Path.Combine(SiteCfg.Router, "Common/Temp/" + tempFileInfo.Name), Path.Combine(SiteCfg.Router, savePath), wmPath, wmPoint, true);
                        }
                        else { files[i].SaveAs(Path.Combine(SiteCfg.Router, savePath)); }
                        //创建附件信息
                        AttachmentItem attVal = new AttachmentItem();
                        attVal.Name = tempFileInfo.Name;
                        attVal.Path = savePath;
                        attVal.Type = files[i].ContentType;
                        attVal.Size = files[i].ContentLength;
                        int tmpId = new AttachmentData(conn).InsertAttachment(attVal);
                        result += string.Format("{0},", tmpId);
                        //判断文件
                        string replaceTag = string.Format("[LocalUpload_{0}]", i);
                        string replaceImgHtml = string.Format("<span class=\"sysAttachImage\"><img src=\"{0}\"/></span>", SiteCfg.Path + savePath);
                        string replaceFileHtml = string.Format("<span class=\"sysAttachDownload\"><a href=\"{0}\">{1}</a></span>", SiteCfg.Path + savePath, SiteDat.GetLan("ClickDown"));
                        string replaceMusicHtml = string.Format("<span class=\"sysAttachMusic\"><embed type=\"application/x-shockwave-flash\" classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-4445535400000\" src=\"{0}Common/Editor/player.swf?soundFile={1}&amp;t=swf\" wmode=\"opaque\" quality=\"high\" menu=\"false\" play=\"true\" loop=\"true\" allowfullscreen=\"true\" height=\"26\" width=\"300\" /></span>", SiteCfg.Path, SiteFun.UrlEncode(SiteCfg.Path + savePath));
                        //格式化内容数据
                        if (tempFileInfo.Extension.ToUpper() == ".BMP" || tempFileInfo.Extension.ToUpper() == ".GIF" || tempFileInfo.Extension.ToUpper() == ".JPG" || tempFileInfo.Extension.ToUpper() == ".PNG")
                        {
                            if (!string.IsNullOrEmpty(content)) { content = content.Replace(replaceTag, replaceImgHtml); }
                            if (!string.IsNullOrEmpty(explain)) { explain = explain.Replace(replaceTag, replaceImgHtml); }
                        }
                        else if (tempFileInfo.Extension.ToUpper() == ".MP3")
                        {
                            if (!string.IsNullOrEmpty(content)) { content = content.Replace(replaceTag, replaceMusicHtml); }
                            if (!string.IsNullOrEmpty(explain)) { explain = explain.Replace(replaceTag, replaceMusicHtml); }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(content)) { content = content.Replace(replaceTag, replaceFileHtml); }
                            if (!string.IsNullOrEmpty(explain)) { explain = explain.Replace(replaceTag, replaceFileHtml); }
                        }
                    }
                }
            }
            if (result.Length > 0) { result = result.Remove(result.Length - 1); }
            return result;
        }
    }
}