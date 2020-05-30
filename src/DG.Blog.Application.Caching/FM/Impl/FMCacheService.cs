﻿using DG.Blog.Application.Contracts.FM;
using DG.Blog.ToolKits.Base;
using DG.Blog.ToolKits.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DG.Blog.Domain.Shared.DGBlogConsts;

namespace DG.Blog.Application.Caching.FM.Impl
{
    public class FMCacheService : CachingServiceBase, IFMCacheService
    {
        private const string KEY_GetChannels = "FM:GetChannels";
        private const string KEY_GetRandomFm = "FM:KEY_GetRandomFm";
        private const string KEY_GetGeyLyric = "FM:GetGeyLyric-{0}-{1}";

        /// <summary>
        /// 获取专辑分类
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<ChannelDto>>> GetChannelsAsync(Func<Task<ServiceResult<IEnumerable<ChannelDto>>>> factory)
        {
            return await Cache.GetOrAddAsync(KEY_GetChannels, factory, CacheStrategy.ONE_MINUTE);
        }

        /// <summary>
        /// 获取随机歌曲
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<FMDto>>> GetRandomFmAsync(Func<Task<ServiceResult<IEnumerable<FMDto>>>> factory)
        {
            return await Cache.GetOrAddAsync(KEY_GetRandomFm, factory, CacheStrategy.ONE_MINUTE);
        }

        /// <summary>
        /// 获取歌词
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="ssid"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public async Task<ServiceResult<string>> GetLyricAsync(string sid, string ssid, Func<Task<ServiceResult<string>>> factory)
        {
            return await Cache.GetOrAddAsync(KEY_GetGeyLyric.FormatWith(sid, ssid), factory, CacheStrategy.ONE_HOURS);
        }
    }
}