﻿using DG.Blog.Application.Contracts.Blog;
using DG.Blog.ToolKits.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DG.Blog.Application.Blog
{
    public partial interface IBlogService
    {
        /// <summary>
        /// 获取标签名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<ServiceResult<string>> GetTagAsync(string name);

        /// <summary>
        /// 查询标签列表
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<QueryTagDto>>> QueryTagsAsync();
    }
}