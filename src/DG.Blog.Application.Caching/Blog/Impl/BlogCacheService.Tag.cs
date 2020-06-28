﻿using DG.Blog.Application.Contracts.Blog;
using DG.Blog.ToolKits.Base;
using DG.Blog.ToolKits.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DG.Blog.Domain.Shared.DGBlogConsts;

namespace DG.Blog.Application.Caching.Blog.Impl
{
    public partial class BlogCacheService
    {
        private const string KEY_GetTag = "Blog:Tag:GetTag-{0}";

        private const string KEY_QueryTags = "Blog:Tag:QueryTags";

        /// <summary>
        /// 获取标签名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public async Task<ServiceResult<string>> GetTagAsync(string name, Func<Task<ServiceResult<string>>> factory)
        {
            return await Cache.GetOrAddAsync(KEY_GetTag.FormatWith(name), factory, CacheStrategy.ONE_HOURS);
        }

        /// <summary>
        /// 查询标签列表
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<QueryTagDto>>> QueryTagsAsync(Func<Task<ServiceResult<IEnumerable<QueryTagDto>>>> factory)
        {
            return await Cache.GetOrAddAsync(KEY_QueryTags, factory, CacheStrategy.ONE_HOURS);
        }
    }
}