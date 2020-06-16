using System.Collections.Generic;

namespace DG.Blog.Web.Response.Blog
{
    public class QueryPostForAdminDto
    {
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Posts
        /// </summary>
        public IEnumerable<PostBriefForAdminDto> Posts { get; set; }
    }
}