using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.Logic
{
    public interface IMyError
    {
        /// <summary>
        /// 打印错误内容
        /// </summary>
        /// <param name="err">错误对象</param>
        void PrintError(Exception err);
    }
}
