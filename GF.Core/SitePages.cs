using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GF.Core
{
    public class SitePages
    {
        ///<summary>
        ///设置分页信息
        ///</summary>
        ///<param name="allCount">数据总数</param>
        ///<param name="thisPage">当前页码</param>
        ///<param name="pageSize">每页最大显示上限</param>
        ///<param name="pageLink">连接字符，{0} 为当前页匹配</param>
        public string Make(int allCount, int thisPage, int pageSize, string pageLink)
        {
            if (pageSize < 1) { pageSize = 1; }
            if (thisPage < 1) { thisPage = 1; }
            //开始
            string html = string.Empty;
            string htmlLeftPage = string.Empty;
            string htmlRightPage = string.Empty;
            if (pageLink == string.Empty) { pageLink = "?"; }
            //开始计算
            long pageCount = 0;
            if (allCount % pageSize == 0)
            {
                pageCount = allCount / pageSize;
            }
            else
            {
                pageCount = (allCount / pageSize) + 1;
            }
            for (int i = 3; i >= 1; i--)
            {
                if (thisPage - i >= 1)
                {
                    htmlLeftPage += string.Format("<a href=\"{0}\">{1}</a>", string.Format(pageLink, (thisPage - i)), thisPage - i);
                }
            }
            for (int j = 1; j <= 3; j++)
            {
                if (thisPage + j <= pageCount)
                {
                    htmlRightPage += string.Format("<a href=\"{0}\">{1}</a>", string.Format(pageLink, (thisPage + j)), thisPage + j);
                }
            }
            long iFPage = thisPage - 1;
            if (iFPage < 1) { iFPage = 1; }
            long iNPage = thisPage + 1;
            if (iNPage > pageCount) { iNPage = pageCount; }
            //最后处理
            if (iNPage < 1) { iNPage = 1; }
            if (pageCount < 1) { pageCount = 1; }
            //计算结束
            html = string.Format("<span>{0}/{1}, {2}</span><a href=\"{3}\">«</a><a href=\"{4}\">‹</a>{5}<b>{0}</b>{6}<a href=\"{7}\">›</a><a href=\"{8}\">»</a>", thisPage, pageCount, allCount, string.Format(pageLink, 1), string.Format(pageLink, iFPage), htmlLeftPage, htmlRightPage, string.Format(pageLink, iNPage), string.Format(pageLink, pageCount));
            return string.Format("<div class=\"sysPages\">{0}</div>", html);
        }
    }
}
