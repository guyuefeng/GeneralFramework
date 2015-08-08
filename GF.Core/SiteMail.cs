using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace GF.Core
{
    public class SiteMail
    {
        /// <summary>
        /// 发件人地址
        /// </summary>
        public string From = string.Empty;

        /// <summary>
        /// 收件人地址
        /// </summary>
        public string To = string.Empty;

        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject = string.Empty;

        /// <summary>
        /// 邮件正文
        /// </summary>
        public string Body = string.Empty;

        /// <summary>
        /// 邮件主机地址
        /// </summary>
        public string Host = string.Empty;

        /// <summary>
        /// 邮件主机端口
        /// </summary>
        public int Port = 25;

        /// <summary>
        /// 登录帐号
        /// </summary>
        public string UserName = string.Empty;

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password = string.Empty;

        ///<summary>
        ///发送邮件
        ///</summary>
        ///<param name="from">发件人地址</param>
        ///<param name="to">收件人地址</param>
        ///<param name="subject">邮件主题</param>
        ///<param name="body">正文</param>
        ///<param name="host">主机地址</param>
        ///<param name="port">端口</param>
        ///<param name="userName">登录帐户</param>
        ///<param name="password">密码</param>
        ///<param name="filePath">邮件的附件</param>
        public void Send(string from, string to, string subject, string body, string host, int port, string userName, string password, string filePath)
        {
            using (MailMessage oMail = new MailMessage(from, to, subject, body))
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    oMail.Attachments.Add(new Attachment(filePath));
                }
                oMail.IsBodyHtml = true;
                SmtpClient oSC = new SmtpClient(host, port);//5^1aspx
                oSC.Credentials = new NetworkCredential(userName, password);
                oSC.Send(oMail);
            }
        }

        /// <summary>
        /// 发送带附件的邮件
        /// </summary>
        ///<param name="filePath">邮件的附件</param>
        public void Send(string filePath)
        {
            Send(this.From, this.To, this.Subject, this.Body, this.Host, this.Port, this.UserName, this.Password, filePath);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        public void Send()
        {
            Send(null);
        }
    }
}
