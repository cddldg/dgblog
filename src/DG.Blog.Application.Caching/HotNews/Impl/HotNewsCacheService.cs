using DG.Blog.Application.Contracts.HotNews;
using DG.Blog.ToolKits.Base;
using DG.Blog.ToolKits.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DG.Blog.Domain.Shared.DGBlogConsts;

namespace DG.Blog.Application.Caching.HotNews.Impl
{
    public class HotNewsCacheService : CachingServiceBase, IHotNewsCacheService
    {
        private const string KEY_GetHotNewsSource = "HotNews:GetHotNewsSource";
        private const string KEY_QueryHotNews = "HotNews:QueryHotNews-{0}";

        /// <summary>
        /// 获取每日热点来源列表
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<EnumResponse>>> GetHotNewsSourceAsync(Func<Task<ServiceResult<IEnumerable<EnumResponse>>>> factory)
        {
            return await Cache.GetOrAddAsync(KEY_GetHotNewsSource, factory, CacheStrategy.ONE_DAY);
        }

        /// <summary>
        /// 根据来源获取每日热点列表
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<HotNewsDto>>> QueryHotNewsAsync(int sourceId, Func<Task<ServiceResult<IEnumerable<HotNewsDto>>>> factory)
        {
            return await Cache.GetOrAddAsync(KEY_QueryHotNews.FormatWith(sourceId), factory, CacheStrategy.ONE_HOURS);
        }
    }
}