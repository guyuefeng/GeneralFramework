using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GF.Core;
using GF.Logic.Template;
using System.IO;

namespace GF.Logic
{
    public class MyError:IMyError 
    {
        private string Pre(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("\n", "<br/>").Replace("  ", "　");
            }
            return text;
        }

        #region IMyError 成员

        public void PrintError(Exception err)
        {
            if (string.IsNullOrEmpty(err.HelpLink)) { err.HelpLink = SiteCfg.WebSite; }
            FileTemplate tpl = new FileTemplate();
            tpl.LoadFile(Path.Combine(SiteCfg.Router, "Common/Theme/Error.htm"));
            tpl.SetTag("HelpLink", Pre(err.HelpLink));
            tpl.SetTag("Message", Pre(err.Message));
            tpl.SetTag("Source", Pre(err.Source));
            tpl.SetTag("StackTrace", Pre(err.StackTrace));
            tpl.Print();
        }

        #endregion
    }
}
