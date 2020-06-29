﻿using DG.Blog.Application.Contracts.Blog;
using DG.Blog.Domain.Blog;
using DG.Blog.ToolKits.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DG.Blog.Application.Blog.Impl
{
    public partial class BlogService
    {
        /// <summary>
        /// 查询友链列表
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<FriendLinkDto>>> QueryFriendLinksAsync()
        {
            return await _blogCacheService.QueryFriendLinksAsync(async () =>
            {
                var result = new ServiceResult<IEnumerable<FriendLinkDto>>();

                var friendLinks = await _friendLinksRepository.GetListAsync();
                var list = ObjectMapper.Map<IEnumerable<FriendLink>, IEnumerable<FriendLinkDto>>(friendLinks);

                result.IsSuccess(list);
                return result;
            });
        }

        /// <summary>
        /// 查询友链列表admin
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<QueryFriendLinkForAdminDto>>> QueryAdminFriendLinksAsync()
        {
            var result = new ServiceResult<IEnumerable<QueryFriendLinkForAdminDto>>();

            var friendLinks = await _friendLinksRepository.GetListAsync();
            var list = friendLinks.Select(p => new QueryFriendLinkForAdminDto { Id = p.Id, Title = p.Title, LinkUrl = p.LinkUrl });

            result.IsSuccess(list);
            return result;
        }
    }
}