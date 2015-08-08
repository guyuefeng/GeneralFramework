using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace GF.Logic.UI
{
    public static class HtmlUI
    {
        /// <summary>
        /// 表格开始
        /// </summary>
        /// <param name="classList">样式列表</param>
        public static string TableStart(string classList)
        {
            classList = string.IsNullOrEmpty(classList) ? string.Empty : string.Format(" class=\"{0}\"", classList);
            return string.Format("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"{0}>\n", classList);
        }

        /// <summary>
        /// 表格开始
        /// </summary>
        public static string TableStart()
        {
            return TableStart(null);
        }

        /// <summary>
        /// 表格结束
        /// </summary>
        public static string TableFinal()
        {
            return "</table>\n";
        }

        /// <summary>
        /// 一行开始
        /// </summary>
        /// <param name="classList">样式名</param>
        public static string TrStart(string classList)
        {
            return string.Format("<tr{0}>\n", (string.IsNullOrEmpty(classList) ? string.Empty : string.Format(" class=\"{0}\"", classList)));
        }

        /// <summary>
        /// 一行开始
        /// </summary>
        public static string TrStart()
        {
            return TrStart(null);
        }

        /// <summary>
        /// 一行结束
        /// </summary>
        public static string TrFinal()
        {
            return "</tr>\n";
        }

        /// <summary>
        /// 创建一组th标签
        /// </summary>
        /// <param name="content">内容</param>
        public static string CreateTh(string content)
        {
            return string.Format("<th>{0}</th>\n", (content == null ? "&nbsp;" : content));
        }

        /// <summary>
        /// 创建一组th标签
        /// </summary>
        public static string CreateTh()
        {
            return CreateTh(null);
        }

        /// <summary>
        /// 创建一组td标签
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="colspan">宽容数字</param>
        /// <param name="classList">类列表</param>
        public static string CreateTd(object content, int colspan, string classList)
        {
            return string.Format("<td{1}{2}>{0}</td>\n", (content == null ? "&nbsp;" : content), (colspan > 0 ? string.Format(" colspan=\"{0}\"", colspan) : string.Empty), (string.IsNullOrEmpty(classList) ? string.Empty : string.Format(" class=\"{0}\"", classList)));
        }

        /// <summary>
        /// 创建一组td标签
        /// </summary>
        /// <param name="content">内容</param>
        public static string CreateTd(object content)
        {
            return CreateTd(content, 0, null);
        }

        /// <summary>
        /// 创建一组td标签
        /// </summary>
        public static string CreateTd()
        {
            return CreateTd(null, 0, null);
        }

        /// <summary>
        /// 创建一般链接
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="link">链接地址</param>
        /// <param name="title">阐述</param>
        /// <param name="newWindow">新窗口打开</param>
        public static string Link(string text, string link, string title, bool newWindow)
        {
            return string.Format("<a href=\"{0}\" title=\"{1}\"{3}>{2}</a>", link, title, text, newWindow ? " target=\"_blank\"" : string.Empty);
        }

        /// <summary>
        /// 创建一般链接
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="link">链接地址</param>
        /// <param name="title">阐述</param>
        public static string Link(string text, string link, string title)
        {
            return Link(text, link, title, false);
        }

        /// <summary>
        /// 创建一般链接
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="link">链接地址</param>
        public static string Link(string text, string link)
        {
            return Link(text, link, null, false);
        }

        /// <summary>
        /// 创建删除链接
        /// </summary>
        /// <param name="link">链接地址</param>
        public static string DeleteLink(string link)
        {
            return string.Format("<a href=\"{0}\" onclick=\"javascript:return confirm('您确定要删除此数据吗？');\">删除</a>", link);
        }

        /// <summary>
        /// 获取文本输入框
        /// </summary>
        /// <param name="id">唯一编号</param>
        /// <param name="name">控件名</param>
        /// <param name="rows">行数</param>
        /// <param name="cols">列数</param>
        /// <param name="val">值</param>
        public static string Textarea(string id, string name, int rows, int cols, object val)
        {
            if (!string.IsNullOrEmpty(id)) { id = string.Format(" id=\"{0}\"", id); };
            string sRows = null;
            if (rows > 0) { sRows = string.Format(" rows=\"{0}\"", rows); };
            string sCols = null;
            if (cols > 0) { sCols = string.Format(" cols=\"{0}\"", cols); };
            return string.Format("<textarea{0} name=\"{1}\"{2}{3}>{4}</textarea>", id, name, sRows, sCols, val);
        }

        /// <summary>
        /// 获取文本输入框
        /// </summary>
        /// <param name="name">控件名</param>
        /// <param name="rows">行数</param>
        /// <param name="cols">列数</param>
        /// <param name="val">值</param>
        public static string Textarea(string name, int rows, int cols, object val)
        {
            return Textarea(null, name, rows, cols, val);
        }

        /// <summary>
        /// 获取文本输入框
        /// </summary>
        /// <param name="name">控件名</param>
        /// <param name="val">值</param>
        public static string Textarea(string name, object val)
        {
            return Textarea(null, name, 5, 0, val);
        }

        /// <summary>
        /// 创建一个按钮
        /// </summary>
        /// <param name="id">唯一编号</param>
        /// <param name="classList">样式列</param>
        /// <param name="text">按钮上的文字</param>
        public static string Button(string id, string classList, string text)
        {
            return string.Format("<button type=\"button\"{0}{2}><img src=\"Common/Images/Icons/ok.gif\"/>{1}</button>", (string.IsNullOrEmpty(id) ? null : " id=\"" + id + "\""), text, (string.IsNullOrEmpty(classList) ? null : " class=\"" + classList + "\""));
        }

        /// <summary>
        /// 创建一个按钮
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="text">按钮上的文字</param>
        public static string Button(string id, string text)
        {
            return Button(id, null, text);
        }

        /// <summary>
        /// 创建一个按钮
        /// </summary>
        /// <param name="text">按钮上的文字</param>
        public static string Button(string text)
        {
            return Button(null, null, text);
        }

        /// <summary>
        /// 创建一个重置按钮
        /// </summary>
        /// <param name="text">文字</param>
        public static string ResetButton(string text)
        {
            return string.Format("<button type=\"reset\"><img src=\"Common/Images/Icons/warning.gif\"/>{0}</button>", text);
        }

        /// <summary>
        /// 创建一个重置按钮
        /// </summary>
        public static string ResetButton()
        {
            return ResetButton("重置(R)");
        }

        /// <summary>
        /// 创建一个提交按钮
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="classList">类列表</param>
        public static string SubmitButton(string text, string classList)
        {
            return string.Format("<button type=\"submit\" class=\"{1}\"><img src=\"Common/Images/Icons/save.gif\"/>{0}</button>", text, (string.IsNullOrEmpty(classList) ? string.Empty : " " + classList));
        }

        /// <summary>
        /// 创建一个提交按钮
        /// </summary>
        /// <param name="text">文字</param>
        public static string SubmitButton(string text)
        {
            return SubmitButton(text, null);
        }

        /// <summary>
        /// 创建一个提交按钮
        /// </summary>
        public static string SubmitButton()
        {
            return SubmitButton("提交(S)", null);
        }

        /// <summary>
        /// 文件上传控件
        /// </summary>
        /// <param name="id">唯一标识</param>
        /// <param name="name">名称</param>
        public static string FileInput(string id, string name)
        {
            return string.Format("<input type=\"file\"{0} name=\"{1}\"/>", (string.IsNullOrEmpty(id) ? null : " id=\"" + id + "\""), name);
        }

        /// <summary>
        /// 创建下拉框
        /// </summary>
        /// <param name="name">控件名</param>
        /// <param name="value">值列表</param>
        /// <param name="cap">显示列表</param>
        /// <param name="defaVal">默认值</param>
        /// <returns>返回下拉框控件</returns>
        public static string CreateSelect(string name, ArrayList value, ArrayList cap, object defaVal)
        {
            string sr = string.Format("<select name=\"{0}\">", name);
            if (value.Count == cap.Count)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    string selected = string.Empty;
                    if (defaVal != null && value[i] != null)
                    {
                        if (!string.IsNullOrEmpty(defaVal.ToString()) && !string.IsNullOrEmpty(value[i].ToString()))
                        {
                            selected = (defaVal.ToString() == value[i].ToString() ? " selected=\"selected\"" : string.Empty);
                        }
                    }
                    sr += string.Format("<option value=\"{0}\"{2}>{1}</option>", value[i], cap[i], selected);
                }
            }
            else { sr += string.Format("<option>{0}</option>", "Array number error..."); }
            sr += "</select>";
            return sr;
        }

        /// <summary>
        /// 创建图像
        /// </summary>
        /// <param name="src">图像地址</param>
        /// <param name="alt">说明</param>
        /// <param name="onclick">脚本</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>返回图片对象代码</returns>
        public static string Image(string src, string alt, string onclick, int width, int height)
        {
            if (!string.IsNullOrEmpty(alt)) { alt = string.Format(" alt=\"{0}\"", alt); }
            if (!string.IsNullOrEmpty(onclick)) { onclick = string.Format(" onclick=\"{0}\"", onclick); }
            string sWidth = string.Empty;
            if (width > 0) { sWidth = string.Format(" width=\"{0}\"", width); }
            string sHeight = string.Empty;
            if (height > 0) { sHeight = string.Format(" height=\"{0}\"", height); }
            return string.Format("<img src=\"{0}\"{1}{2}{3}{4}/>", src, alt, onclick, sWidth, sHeight);
        }

        /// <summary>
        /// 创建图像
        /// </summary>
        /// <param name="src">图像地址</param>
        /// <param name="alt">说明</param>
        /// <param name="onclick">脚本</param>
        /// <returns>返回图片对象代码</returns>
        public static string Image(string src, string alt, string onclick)
        {
            return Image(src, alt, onclick, 0, 0);
        }

        /// <summary>
        /// 创建图像
        /// </summary>
        /// <param name="src">图像地址</param>
        /// <param name="alt">说明</param>
        /// <returns>返回图片对象代码</returns>
        public static string Image(string src, string alt)
        {
            return Image(src, alt, null, 0, 0);
        }

        /// <summary>
        /// 创建一般输入框
        /// </summary>
        /// <param name="id">独立编号</param>
        /// <param name="name">控件名</param>
        /// <param name="len">字符长度上限</param>
        /// <param name="classList">类名称列表，为空则默认</param>
        /// <param name="value">默认值</param>
        public static string Input(string id, string name, int len, string classList, object value)
        {
            return string.Format("<input type=\"text\" name=\"{3}\"{4} value=\"{0}\" size=\"{1}\" maxlength=\"255\" class=\"input{2}\"/>", value, len, (string.IsNullOrEmpty(classList) ? string.Empty : " " + classList), name, (string.IsNullOrEmpty(id) ? string.Empty : string.Format(" id=\"{0}\"", id)));
        }

        /// <summary>
        /// 创建一般输入框
        /// </summary>
        /// <param name="name">控件名</param>
        /// <param name="len">字符长度上限</param>
        /// <param name="classList">类名称列表，为空则默认</param>
        /// <param name="value">默认值</param>
        public static string Input(string name, int len, string classList, object value)
        {
            return Input(null, name, len, classList, value);
        }

        /// <summary>
        /// 创建隐藏表单项
        /// </summary>
        /// <param name="id">唯一编号</param>
        /// <param name="name">控件名</param>
        /// <param name="val">值</param>
        /// <returns>返回隐藏输入框</returns>
        public static string HiddenInput(string id, string name, object val)
        {
            return string.Format("<input type=\"hidden\" name=\"{0}\" value=\"{1}\" id=\"{2}\"/>", name, val, id);
        }

        /// <summary>
        /// 创建隐藏表单项
        /// </summary>
        /// <param name="name">控件名</param>
        /// <param name="val">值</param>
        /// <returns>返回隐藏输入框</returns>
        public static string HiddenInput(string name, object val)
        {
            return string.Format("<input type=\"hidden\" name=\"{0}\" value=\"{1}\"/>", name, val);
        }

        /// <summary>
        /// 创建密码输入框
        /// </summary>
        /// <param name="id">独立编号</param>
        /// <param name="name">控件名</param>
        /// <param name="len">字符长度上限</param>
        /// <param name="classList">类名称列表，为空则默认</param>
        /// <param name="value">默认值</param>
        public static string PasswordInput(string id, string name, int len, string classList, string value)
        {
            return string.Format("<input type=\"password\" name=\"{3}\"{4} value=\"{0}\" size=\"{1}\" maxlength=\"255\" class=\"input{2}\"/>", value, len, (string.IsNullOrEmpty(classList) ? string.Empty : " " + classList), name, (string.IsNullOrEmpty(id) ? string.Empty : string.Format(" id=\"{0}\"", id)));
        }

        /// <summary>
        /// 创建密码输入框
        /// </summary>
        /// <param name="name">控件名</param>
        /// <param name="len">字符长度上限</param>
        /// <param name="classList">类名称列表，为空则默认</param>
        /// <param name="value">默认值</param>
        public static string PasswordInput(string name, int len, string classList, string value)
        {
            return PasswordInput(null, name, len, classList, value);
        }

        /// <summary>
        /// 选择文件控件
        /// </summary>
        /// <param name="name">控件名</param>
        public static string FileInput(string name)
        {
            return string.Format("<input type=\"file\" name=\"{0}\"/>", name);
        }

        /// <summary>
        /// 创建选择框
        /// </summary>
        /// <param name="txt">文字</param>
        /// <param name="name">控件名</param>
        /// <param name="val">值</param>
        /// <param name="sel">是否选中</param>
        public static string CheckBoxInput(string txt, string name, object val, bool sel)
        {
            return string.Format("<input type=\"checkbox\" id=\"__sysCheckBox_{1}_{2}\" name=\"{1}\" value=\"{2}\"{3}/>{0}", string.Format("<label for=\"__sysCheckBox_{1}_{2}\">{0}</label>", txt, name, val), name, val, (sel ? " checked=\"checked\"" : string.Empty));
        }

        /// <summary>
        /// 创建单选框
        /// </summary>
        /// <param name="txt">文字</param>
        /// <param name="name">控件名</param>
        /// <param name="val">值</param>
        /// <param name="sel">是否选中</param>
        public static string RadioInput(string txt, string name, object val, bool sel)
        {
            return string.Format("<input type=\"radio\" id=\"__sysRadio_{1}_{2}\" name=\"{1}\" value=\"{2}\"{3}/>{0}", string.Format("<label for=\"__sysRadio_{1}_{2}\">{0}</label>", txt, name, val), name, val, (sel ? " checked=\"checked\"" : string.Empty));
        }

        /// <summary>
        /// 创建选择框
        /// </summary>
        /// <param name="name">控件名</param>
        /// <param name="val">值</param>
        /// <param name="sel">是否选中</param>
        public static string CheckBoxInput(string name, object val, bool sel)
        {
            return CheckBoxInput(null, name, val, sel);
        }

        /// <summary>
        /// 创建表单开始
        /// </summary>
        /// <param name="isPost">是否以POST传值</param>
        /// <param name="act">提交地址，为空则提交到本页</param>
        /// <param name="isUpload">是否包含上传控件</param>
        public static string FormStart(bool isPost, string act, bool isUpload)
        {
            return string.Format("<form method=\"{0}\"{1}{2}>\n", (isPost ? "post" : "get"), (string.IsNullOrEmpty(act) ? string.Empty : string.Format(" action=\"{0}\"", act)), (isUpload ? " enctype=\"multipart/form-data\"" : string.Empty));
        }

        /// <summary>
        /// 以POST发送数据表单开始
        /// </summary>
        /// <param name="act">提交地址</param>
        public static string FormStart(string act)
        {
            return FormStart(true, act, false);
        }

        /// <summary>
        /// 以POST发送数据表单开始
        /// </summary>
        /// <param name="act">提交地址</param>
        /// <param name="isUpload">是否包含上传控件</param>
        public static string FormStart(string act, bool isUpload)
        {
            return FormStart(true, act, isUpload);
        }

        /// <summary>
        /// 以POST发送数据到本页开始
        /// </summary>
        public static string FormStart()
        {
            return FormStart(true, null, false);
        }

        /// <summary>
        /// 表单结束
        /// </summary>
        public static string FormFinal()
        {
            return "</form>\n";
        }
    }
}
