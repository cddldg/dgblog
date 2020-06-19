using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DG.Blog.Web.Response.Blog
{
    public class HotNewsDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; set; }
    }
}