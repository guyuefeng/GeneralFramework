using System.Collections;

namespace GF.Data.Entity
{
    /// <summary>
    /// 基本配置数据对象
    /// </summary>
    public class SettingBasicItem
    {
        //参数列表
        private string _name = string.Empty;
        private string _url = string.Empty;
        private string _icp = string.Empty;
        private string _language = string.Empty;
        private string _intro = string.Empty;
        private string _keywords = string.Empty;
        private string _affiche = string.Empty;
        private string _filter = string.Empty;
        private string _uploadExt = string.Empty;
        private string _mail = string.Empty;
        private string _mailFrom = string.Empty;
        private string _mailHost = string.Empty;
        private int _mailPort = 0;
        private string _mailUid = string.Empty;
        private string _mailPwd = string.Empty;

        /// <summary>
        /// 获取或设置站点名称
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        /// <summary>
        /// 获取或设置站点地址
        /// </summary>
        public string URL
        {
            get { return this._url; }
            set { this._url = value; }
        }

        /// <summary>
        /// 获取或设置站点备案号
        /// </summary>
        public string ICP
        {
            get { return this._icp; }
            set { this._icp = value; }
        }

        /// <summary>
        /// 获取或设置语言包目录名
        /// </summary>
        public string Language
        {
            get { return this._language; }
            set { this._language = value; }
        }

        /// <summary>
        /// 获取或设置站点介绍
        /// </summary>
        public string Intro
        {
            get { return this._intro; }
            set { this._intro = value; }
        }

        /// <summary>
        /// 获取或设置站点关键字
        /// </summary>
        public string Keywords
        {
            get { return this._keywords; }
            set { this._keywords = value; }
        }

        /// <summary>
        /// 获取或设置站点公告
        /// </summary>
        public string Affiche
        {
            get { return this._affiche; }
            set { this._affiche = value; }
        }

        /// <summary>
        /// 获取或设置站点过滤字符
        /// </summary>
        public string Filter
        {
            get { return this._filter; }
            set { this._filter = value; }
        }

        /// <summary>
        /// 获取或设置上传合法文件后缀
        /// </summary>
        public string UploadExt
        {
            get { return this._uploadExt; }
            set { this._uploadExt = value; }
        }

        /// <summary>
        /// 获取或设置管理员邮件地址
        /// </summary>
        public string Mail
        {
            get { return this._mail; }
            set { this._mail = value; }
        }

        /// <summary>
        /// 获取或设置邮件发送服务显示地址
        /// </summary>
        public string MailFrom
        {
            get { return this._mailFrom; }
            set { this._mailFrom = value; }
        }

        /// <summary>
        /// 获取或设置邮件发送服务器主机
        /// </summary>
        public string MailHost
        {
            get { return this._mailHost; }
            set { this._mailHost = value; }
        }

        /// <summary>
        /// 获取或设置邮件发送服务器端口
        /// </summary>
        public int MailPort
        {
            get { return this._mailPort; }
            set { this._mailPort = value; }
        }

        /// <summary>
        /// 获取或设置邮件发送服务用户名
        /// </summary>
        public string MailUserID
        {
            get { return this._mailUid; }
            set { this._mailUid = value; }
        }

        /// <summary>
        /// 获取或设置邮件发送服务密码
        /// </summary>
        public string MailPassword
        {
            get { return this._mailPwd; }
            set { this._mailPwd = value; }
        }
    }
}
