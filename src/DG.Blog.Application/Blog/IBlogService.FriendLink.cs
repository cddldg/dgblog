using DG.Blog.Application.Contracts.Blog;
using DG.Blog.ToolKits.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DG.Blog.Application.Blog
{
    public partial interface IBlogService
    {
        /// <summary>
        /// 查询友链列表
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<FriendLinkDto>>> QueryFriendLinksAsync();

        /// <summary>
        /// 查询友链列表admin
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<QueryFriendLinkForAdminDto>>> QueryAdminFriendLinksAsync();
    }
}