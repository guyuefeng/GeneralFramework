using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.Logic.Template
{
    public interface ITemplateFun
    {
        /// <summary>
        /// 引入关键字，为 expand:引入关键字
        /// </summary>
        string URI { get; }
    }
}
