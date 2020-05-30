﻿using DG.Blog.Application.Contracts.Blog;
using DG.Blog.ToolKits.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DG.Blog.Application.Blog
{
    public partial interface IBlogService
    {
        /// <summary>
        /// 获取分类名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<ServiceResult<string>> GetCategoryAsync(string name);

        /// <summary>
        /// 查询分类列表
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<QueryCategoryDto>>> QueryCategoriesAsync();
    }
}