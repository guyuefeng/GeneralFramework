using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using GF.Core;
using GF.Data;
using GF.Data.Entity;
using GF.Logic.UI;

namespace GF.Logic.Admin
{
    /// <summary>
    /// 数据库管理类
    /// </summary>
    public class AdminDB
    {
        private _DbHelper conn;

        public AdminDB(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        public string SQL()
        {
            StringBuilder sr = new StringBuilder();
            string sql = SiteFun.Post("sql");
            bool reCount = SiteFun.ToInt(SiteFun.Post("reCount")) == 0 ? false : true;
            if (SiteFun.IsPost && reCount)
            {
                int count = new PostData(conn).CountPost();
                count += new ColumnData(conn).CountColumnPost();
                sr.Append(AdminUI.SuccessBox(string.Format(SiteDat.GetLan("MsgCountSucc"), count)));
            }
            //数量重计
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.FormStart());
            sr.Append(HtmlUI.TableStart("onCenter"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("ReCount")));
            sr.Append(HtmlUI.CreateTd(HtmlUI.HiddenInput("reCount", 1) + HtmlUI.SubmitButton(SiteDat.GetLan("BtnCount")) + SiteDat.GetLan("IntroCount")));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(HtmlUI.FormFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            //SQL执行
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.FormStart());
            sr.Append(HtmlUI.TableStart("onCenter"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh("SQL") + HtmlUI.CreateTd(HtmlUI.Textarea("sql", 20, 0, SiteFun.HtmlEncode(sql))));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(HtmlUI.SubmitButton() + HtmlUI.ResetButton()));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(HtmlUI.FormFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            //取得默认值
            if (SiteFun.IsPost && !string.IsNullOrEmpty(sql))
            {
                try
                {
                    DataSet ds = new DataBase().ExecSql(sql);
                    sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgExeSqlSucc")));
                    //显示执行结果
                    if (ds.Tables.Count > 0)
                    {
                        sr.Append(AdminUI.AdminBoxStart(true));
                        sr.Append(HtmlUI.FormStart());
                        sr.Append(HtmlUI.TableStart());
                        sr.Append(HtmlUI.TrStart());
                        for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                        {
                            sr.Append(HtmlUI.CreateTh(ds.Tables[0].Columns[i].ColumnName));
                        }
                        sr.Append(HtmlUI.TrFinal());
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            sr.Append(HtmlUI.TrStart());
                            for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                            {
                                sr.Append(HtmlUI.CreateTd(SiteFun.HtmlEncode(ds.Tables[0].Rows[i][j].ToString())));
                            }
                            sr.Append(HtmlUI.TrFinal());
                        }
                        sr.Append(HtmlUI.TableFinal());
                        sr.Append(HtmlUI.FormFinal());
                        sr.Append(AdminUI.AdminBoxFinal());
                    }
                }
                catch (Exception err) { sr.Append(AdminUI.ErrorBox(err.Message)); }
            }
            return sr.ToString();
        }

        /// <summary>
        /// 备份和恢复
        /// </summary>
        public string Backup()
        {
            StringBuilder sr = new StringBuilder();
            string backupPath = Path.Combine(SiteCfg.Router, "Common/Data/Backup/");
            if (SiteFun.IsPost)
            {
                try
                {
                    bool dbSel = SiteFun.ToInt(SiteFun.Post("dbSel")) > 0;
                    bool attachSel = SiteFun.ToInt(SiteFun.Post("attachSel")) > 0;
                    bool delBackup = SiteFun.ToInt(SiteFun.Post("del")) > 0;
                    bool sendMail = SiteFun.ToInt(SiteFun.Post("sendMail")) > 0;
                    string fileName = SiteFun.Post("fileName");
                    //开始备份数据
                    if (dbSel || attachSel)
                    {
                        if (dbSel)
                        {
                            string dbFile = HttpContext.Current.Server.MapPath(SiteCfg.DBPath);
                            string tmpSavFileName = string.Format("{1}-{0}.bak", Path.GetFileNameWithoutExtension(dbFile), DateTime.Now.ToString("yyyyMMdd"));
                            File.Copy(dbFile, Path.Combine(backupPath, tmpSavFileName), true);
                        }
                        //开始备份附件
                        if (attachSel)
                        {
                            //Folder.CopyFolder(Path.Combine(SiteCfg.Router, @"Attach\"), Path.Combine(rootFolder, @"Attach\"));
                        }
                        sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgBackupSucc")));
                    }
                    else
                    {
                        if (sendMail)
                        {
                            SettingBasicItem basic = new SettingData(conn).GetSetting().Basic;
                            SiteMail mail = new SiteMail();
                            string dt = DateTime.Now.ToString("yyyy-MM-dd");
                            mail.To = basic.Mail;
                            mail.Subject = string.Format("{0} DB BAK, {1}", basic.Name, dt);
                            mail.From = basic.MailFrom;
                            mail.Body = "Time: " + dt;
                            mail.Host = basic.MailHost;
                            mail.Port = basic.MailPort;
                            mail.UserName = basic.MailUserID;
                            mail.Password = basic.MailPassword;
                            mail.Send(Path.Combine(backupPath, fileName));
                            sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgBackupSucc")));
                        }
                        if (delBackup)
                        {
                            File.Delete(Path.Combine(backupPath, fileName));
                            sr.Append(AdminUI.SuccessBox(SiteDat.GetLan("MsgDelBackupSucc")));
                        }
                    }
                }
                catch (Exception err) { sr.Append(AdminUI.ErrorBox(err.Message)); }
            }
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.FormStart());
            sr.Append(HtmlUI.TableStart("onCenter"));
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Backup")) + HtmlUI.CreateTd(HtmlUI.CheckBoxInput(SiteDat.GetLan("Database"), "dbSel", 1, false)));
            /*sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Backup")) + HtmlUI.CreateTd(HtmlUI.CheckBoxInput(SiteDat.GetLan("Database"), "dbSel", 1, false) + HtmlUI.CheckBoxInput(SiteDat.GetLan("Attach"), "attachSel", 1, false)));*/
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh() + HtmlUI.CreateTd(HtmlUI.SubmitButton() + HtmlUI.ResetButton()));
            sr.Append(HtmlUI.TrFinal());
            sr.Append(HtmlUI.TableFinal());
            sr.Append(HtmlUI.FormFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            //列表
            sr.Append(AdminUI.AdminBoxStart(true));
            sr.Append(HtmlUI.TableStart());
            sr.Append(HtmlUI.TrStart());
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("FileName")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("DateTime")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Size")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Delete")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("MailNotice")));
            sr.Append(HtmlUI.CreateTh(SiteDat.GetLan("Operate")));
            sr.Append(HtmlUI.TrFinal());
            //循环
            int i = 1;
            foreach (string file in Directory.GetFiles(backupPath))
            {
                i++;
                FileInfo fileInfo = new FileInfo(file);
                sr.Append(HtmlUI.FormStart());
                sr.Append(HtmlUI.TrStart(i % 2 == 0 ? " cRow" : null));
                sr.Append(HtmlUI.CreateTd(fileInfo.Name));
                sr.Append(HtmlUI.CreateTd(fileInfo.CreationTime));
                sr.Append(HtmlUI.CreateTd(SiteFun.ToInt(fileInfo.Length / 1024) + " Kb"));
                sr.Append(HtmlUI.CreateTd(HtmlUI.CheckBoxInput("del", 1, false)));
                sr.Append(HtmlUI.CreateTd(HtmlUI.CheckBoxInput("sendMail", 1, false)));
                sr.Append(HtmlUI.CreateTd(HtmlUI.HiddenInput("fileName", SiteFun.HtmlEncode(fileInfo.Name)) + HtmlUI.SubmitButton()));
                sr.Append(HtmlUI.TrFinal());
                sr.Append(HtmlUI.FormFinal());
            }
            //循环结束
            sr.Append(HtmlUI.TableFinal());
            sr.Append(AdminUI.AdminBoxFinal());
            return sr.ToString();
        }
    }
}
