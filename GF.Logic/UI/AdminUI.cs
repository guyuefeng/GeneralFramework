using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GF.Core;

namespace GF.Logic.UI
{
    public static class AdminUI
    {
        /// <summary>
        /// 显示布局头部
        /// </summary>
        /// <param name="showTop">是否显示头元素</param>
        public static string AdminBoxStart(bool showTop)
        {
            return string.Format("<div class=\"box\">\n{0}<div class=\"content\">\n", (showTop ? "<div class=\"top\"></div>\n" : string.Empty));
        }

        /// <summary>
        /// 显示布局头部
        /// </summary>
        public static string AdminBoxStart()
        {
            return AdminBoxStart(false);
        }

        /// <summary>
        /// 显示布局底部
        /// </summary>
        public static string AdminBoxFinal()
        {
            return "</div>\n</div>\n";
        }

        /// <summary>
        /// 提示错误信息
        /// </summary>
        /// <param name="msg">提示文本</param>
        public static string ErrorBox(string msg)
        {
            string sr = string.Empty;
            sr += AdminBoxStart();
            sr += HtmlUI.TableStart("error");
            sr += HtmlUI.TrStart();
            sr += HtmlUI.CreateTd(msg);
            sr += HtmlUI.TrFinal();
            sr += HtmlUI.TableFinal();
            sr += AdminBoxFinal();
            return sr;
        }

        /// <summary>
        /// 提示成功信息
        /// </summary>
        /// <param name="msg">提示文本</param>
        /// <param name="url">跳转地址</param>
        public static string SuccessBox(string msg, string url)
        {
            string jump = string.IsNullOrEmpty(url) ? string.Empty : "<script type=\"text/javascript\">setTimeout(function(){self.location.href=\"" + url + "\"}, 2000);</script>";
            string sr = string.Empty;
            sr += AdminBoxStart();
            sr += HtmlUI.TableStart("success");
            sr += HtmlUI.TrStart();
            sr += HtmlUI.CreateTd(msg + jump);
            sr += HtmlUI.TrFinal();
            sr += HtmlUI.TableFinal();
            sr += AdminBoxFinal();
            return sr;
        }

        /// <summary>
        /// 提示成功信息
        /// </summary>
        /// <param name="msg">提示文本</param>
        public static string SuccessBox(string msg)
        {
            return SuccessBox(msg, null);
        }

        /// <summary>
        /// 创建编辑器
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <param name="value">默认值</param>
        public static string Editor(string name, string value)
        {
            //return "<textarea name=\"" + name + "\" style=\"display:none;\">" + value + "</textarea><iframe id=\"" + name + "___Frame\" src=\"Common/Editor/editor/fckeditor.html?InstanceName=" + name + "&amp;Toolbar=Default\" width=\"100%\" height=\"300\" frameborder=\"0\" scrolling=\"no\"></iframe>";
            StringBuilder result = new StringBuilder();
            //result.AppendFormat("<textarea name=\"{0}\" class=\"{0}_editor\" style=\"width:100%;height:300px;\">{1}</textarea>\n", name, value);
            //result.AppendLine("<script type=\"text/javascript\">var " + name + "_editor = $('." + name + "_editor').xheditor({plugins:myPlugin, tools:'Pagebreak,Cut,Copy,Paste,Pastetext,Blocktag,Fontface,FontSize,Bold,Italic,Underline,Strikethrough,FontColor,BackColor,Removeformat,Align,List,Outdent,Indent,Link,Unlink,Img,Flash,Media,Insertmusic,Table,Insertcode,Source,Fullscreen,About'});</script>");
            result.AppendFormat("<textarea name=\"{0}\" style=\"display:none;\">{1}</textarea><script type=\"text/javascript\">CKEDITOR.replace(\"{0}\");</script>", name, value);
            result.AppendLine("<div class=\"emotBox\">");
            result.AppendLine(HtmlUI.Button(null, "showEmotsBox", SiteDat.GetLan("BtnShowEmots")));
            result.AppendLine("<div class=\"emots\">");
            foreach (string file in Directory.GetFiles(Path.Combine(SiteCfg.Router, "Common/Images/Emot")))
            {
                FileInfo fInfo = new FileInfo(file);
                result.AppendFormat("<a class=\"emotLink\" rel=\"{0}\"><img src=\"{1}Common/Images/Emot/{2}\" width=\"24\" height=\"24\"/></a>\r\n", name, SiteCfg.Path, fInfo.Name);
            }
            result.AppendLine("</div>");
            result.AppendLine("</div>");
            return result.ToString();
        }
    }
}
