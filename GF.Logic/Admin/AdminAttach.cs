using System;
using System.IO;
using System.Text;
using GF.Core;
using GF.Data;
using GF.Data.Entity;
using GF.Logic.UI;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 附件管理类
    /// </summary>
    public class AdminAttach
    {
        private _DbHelper conn;

        public AdminAttach(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 附件列表
        /// </summary>
        /// <returns>附件列表区域</returns>
        public string List()
        {
            StringBuilder sr = new StringBuilder();
            AttachmentData attData = new AttachmentData(conn);
            if (SiteFun.IsPost)
            {
                int id = SiteFun.ToInt(SiteFun.Post("id"));
                try
                {
                    AttachmentItem delAttInfo = attData.GetAttachment(id);
                    if (File.Exists(Path.Combine(SiteCfg.Router, delAttInfo.Path)))
                    {
                        File.Delete(Path.Combine(SiteCfg.Router, delAttInfo.Path));
                    }
                    attData.DeleteAttachment(id);
                    sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgDelFile")));
                }
                catch (Exception err) { sr.Append(AdminUI.ErrorBox(err.Message)); }
            }
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("FileName")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("FileType")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Size")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("DateTime")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Operate")));
            sr.Append(HtmlUI.TrFinal());
            //下面三行是分页设置
            int page = SiteFun.ToInt(SiteFun.Query("page"));
            if (page < 1) { page = 1; }
            int pageSize = 20;
            DataList<AttachmentItem> list = attData.SelectAttachment(page, pageSize);
            int i = 1;
            foreach (AttachmentItem vItem in list)
            {
                i++;
                FileInfo fileInfo = new FileInfo(Path.Combine(SiteCfg.Router, vItem.Path));
                sr.Append(HtmlUI.FormStart());
                sr.Append(HtmlUI.TrStart(i % 2 == 0 ? " cRow" : null));
                if (fileInfo.Exists)
                {
                    sr.Append(HtmlUI.CreateTd(HtmlUI.Image("Common/Images/FileType/" + (fileInfo.Length > 1 ? fileInfo.Extension.Substring(1) + ".png" : string.Empty), SiteFun.HtmlEncode(vItem.Type)) + HtmlUI.Link(SiteFun.HtmlEncode(vItem.Name), SiteCfg.Path + vItem.Path, vItem.Path, true)));
                }
                else { sr.Append(HtmlUI.CreateTd("--")); }
                sr.Append(HtmlUI.CreateTd(SiteFun.HtmlEncode(vItem.Type)));
                sr.Append(HtmlUI.CreateTd(SiteFun.FormatLength(vItem.Size)));
                sr.Append(HtmlUI.CreateTd(vItem.Publish));
                sr.Append(HtmlUI.CreateTd(HtmlUI.HiddenInput("id", vItem.ID) + HtmlUI.SubmitButton(SiteDat.GetLan("BtnDel"))));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.FormFinal());
            }
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTd(new SitePages().Make(list.Number, page, pageSize, "?act=attach&amp;mode=list&amp;page={0}"), 5, null));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            return sr.ToString();
        }
    }
}
