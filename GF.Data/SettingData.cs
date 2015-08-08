using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GF.Data.Entity;


namespace GF.Data
{
    public class SettingData
    {
        private _DbHelper conn;

        public SettingData(_DbHelper c)
        {
            conn = c;
        }

        /// <summary>
        /// 取得配置
        /// </summary>
        /// <returns>配置信息对象</returns>
        public SettingItem GetSetting()
        {
            SettingItem item = new SettingItem();
            using (IDataReader reader = conn.ExecuteReader("SELECT [Name], [URL], [ICP], [Language], [Intro], [Keywords], [Affiche], [Filter], [UploadExt], [Mail], [MailFrom], [MailHost], [MailPort], [MailUID], [MailPWD], [WatermarkSeat], [ParArticleNum], [ParCommentNum], [ParAppendFineArticleNum], [ParAppendRandomArticleNum], [ParAppendHotArticleNum], [ParAppendHotTagNum], [ParAppendNewCommentNum], [RssNum], [RssMode], [WatermarkPath], [Key] FROM [Setting]", 1))
            {
                while (reader.Read())
                {
                    //主干配置部分
                    item.Basic.Name = reader.GetString(0);
                    item.Basic.URL = reader.GetString(1);
                    item.Basic.ICP = reader.GetString(2);
                    item.Basic.Language = reader.GetString(3);
                    item.Basic.Intro = reader.GetString(4);
                    item.Basic.Keywords = reader.GetString(5);
                    item.Basic.Affiche = reader.GetString(6);
                    item.Basic.Filter = reader.GetString(7);
                    item.Basic.UploadExt = reader.GetString(8);
                    item.Basic.Mail = reader.GetString(9);
                    item.Basic.MailFrom = reader.GetString(10);
                    item.Basic.MailHost = reader.GetString(11);
                    item.Basic.MailPort = reader.GetInt32(12);
                    item.Basic.MailUserID = reader.GetString(13);
                    item.Basic.MailPassword = reader.GetString(14);
                    //参数部分
                    item.Parameter.WatermarkSeat = reader.GetInt32(15);
                    item.Parameter.ArticleNum = reader.GetInt32(16);
                    item.Parameter.CommentNum = reader.GetInt32(17);
                    item.Parameter.AppendFineArticleNum = reader.GetInt32(18);
                    item.Parameter.AppendRandomArticleNum = reader.GetInt32(19);
                    item.Parameter.AppendHotArticleNum = reader.GetInt32(20);
                    item.Parameter.AppendHotTagNum = reader.GetInt32(21);
                    item.Parameter.AppendNewCommentNum = reader.GetInt32(22);
                    item.Parameter.RssNum = reader.GetInt32(23);
                    item.Parameter.RssMode = reader.GetInt32(24);
                    item.Parameter.WatermarkPath = reader.GetString(25);
                    item.Parameter.Key = reader.GetString(26);
                }
            }
            return item;
        }

        /// <summary>
        /// 更新配置的基本信息
        /// </summary>
        /// <param name="value">基本配置</param>
        public void UpdateSettingBasic(SettingBasicItem value)
        {
            _DbParameter[] pars ={
                                    new _DbParameter().Set("@Name", DbType.String, value.Name),
                                    new _DbParameter().Set("@URL", DbType.String, value.URL),
                                    new _DbParameter().Set("@ICP", DbType.String, value.ICP),
                                    new _DbParameter().Set("@Language", DbType.String, value.Language),
                                    new _DbParameter().Set("@Intro", DbType.String, value.Intro),
                                    new _DbParameter().Set("@Keywords", DbType.String, value.Keywords),
                                    new _DbParameter().Set("@Affiche", DbType.String, value.Affiche),
                                    new _DbParameter().Set("@Filter", DbType.String, value.Filter),
                                    new _DbParameter().Set("@UploadExt", DbType.String, value.UploadExt),
                                    new _DbParameter().Set("@Mail", DbType.String, value.Mail),
                                    new _DbParameter().Set("@MailFrom", DbType.String, value.MailFrom),
                                    new _DbParameter().Set("@MailHost", DbType.String, value.MailHost),
                                    new _DbParameter().Set("@MailPort", DbType.Int32, value.MailPort),
                                    new _DbParameter().Set("@MailUserID", DbType.String, value.MailUserID),
                                    new _DbParameter().Set("@MailPassword", DbType.String, value.MailPassword)
                                };
            conn.ExecuteNonQuery("UPDATE [Setting] SET [Name] = @Name, [URL] = @URL, [ICP] = @ICP, [Language] = @Language, [Intro] = @Intro, [Keywords] = @Keywords, [Affiche] = @Affiche, [Filter] = @Filter, [UploadExt] = @UploadExt, [Mail] = @Mail, [MailFrom] = @MailFrom, [MailHost] = @MailHost, [MailPort] = @MailPort, [MailUID] = @MailUserID, [MailPWD] = @MailPassword", pars);
        }

        /// <summary>
        /// 更新参数
        /// </summary>
        /// <param name="value">参数</param>
        public void UpdateSettingParameter(SettingParameterItem value)
        {
            _DbParameter[] pars ={
                                    new _DbParameter().Set("@WatermarkSeat", DbType.Int32, value.WatermarkSeat),
                                    new _DbParameter().Set("@ArticleNum", DbType.Int32, value.ArticleNum),
                                    new _DbParameter().Set("@CommentNum", DbType.Int32, value.CommentNum),
                                    new _DbParameter().Set("@AppendFineArticleNum", DbType.Int32, value.AppendFineArticleNum),
                                    new _DbParameter().Set("@AppendRandomArticleNum", DbType.Int32, value.AppendRandomArticleNum),
                                    new _DbParameter().Set("@AppendHotArticleNum", DbType.Int32, value.AppendHotArticleNum),
                                    new _DbParameter().Set("@AppendHotTagNum", DbType.Int32, value.AppendHotTagNum),
                                    new _DbParameter().Set("@AppendNewCommentNum", DbType.Int32, value.AppendNewCommentNum),
                                    new _DbParameter().Set("@RssNum", DbType.Int32, value.RssNum),
                                    new _DbParameter().Set("@RssMode", DbType.Int32, value.RssMode),
                                    new _DbParameter().Set("@WMPath", DbType.String, value.WatermarkPath),
                                    new _DbParameter().Set("@Key", DbType.String, value.Key)
                                };
            conn.ExecuteNonQuery("UPDATE [Setting] SET [WatermarkSeat] = @WatermarkSeat, [ParArticleNum] = @ArticleNum, [ParCommentNum] = @CommentNum, [ParAppendFineArticleNum] = @AppendFineArticleNum, [ParAppendRandomArticleNum] = @AppendRandomArticleNum, [ParAppendHotArticleNum] = @AppendHotArticleNum, [ParAppendHotTagNum] = @AppendHotTagNum, [ParAppendNewCommentNum] = @AppendNewCommentNum, [RssNum] = @RssNum, [RssMode] = @RssMode, [WatermarkPath] = @WMPath, [Key] = @Key", pars);
        }

        /// <summary>
        /// 取得主题包目录
        /// </summary>
        public string GetTheme
        {
            get
            {
                string theme = string.Empty;
                theme = Convert.ToString(conn.ExecuteScalar("SELECT [Theme] FROM [Setting]"));
                return theme;
            }
        }

        /// <summary>
        /// 设置主题包目录
        /// </summary>
        /// <param name="theme">主题包目录名</param>
        /// <returns>主题包目录名</returns>
        public string SetTheme(string theme)
        {
            _DbParameter[] pars ={
                                    new _DbParameter().Set("@Theme", DbType.String, theme)
                                };
            conn.ExecuteNonQuery("UPDATE [Setting] SET [Theme] = @Theme", pars);
            return theme;
        }

    }
}
