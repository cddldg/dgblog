﻿using DG.Blog.ToolKits.Base;
using DG.Blog.ToolKits.Extensions;
using System;
using System.Threading.Tasks;
using static DG.Blog.Domain.Shared.DGBlogConsts;

namespace DG.Blog.Application.Caching.Authorize.Impl
{
    public class AuthorizeCacheService : CachingServiceBase, IAuthorizeCacheService
    {
        private const string KEY_GetLoginAddress = "Authorize:GetLoginAddress";

        private const string KEY_GetAccessToken = "Authorize:GetAccessToken-{0}";

        private const string KEY_GenerateToken = "Authorize:GenerateToken-{0}";

        /// <summary>
        /// 获取登录地址(GitHub)
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public async Task<ServiceResult<string>> GetLoginAddressAsync(Func<Task<ServiceResult<string>>> factory)
        {
            return await Cache.GetOrAddAsync(KEY_GetLoginAddress, factory, CacheStrategy.NEVER);
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="code"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public async Task<ServiceResult<string>> GetAccessTokenAsync(string code, Func<Task<ServiceResult<string>>> factory)
        {
            return await Cache.GetOrAddAsync(KEY_GetAccessToken.FormatWith(code), factory, CacheStrategy.FIVE_MINUTES);
        }

        /// <summary>
        /// 登录成功，生成Token
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public async Task<ServiceResult<string>> GenerateTokenAsync(string access_token, Func<Task<ServiceResult<string>>> factory)
        {
            return await Cache.GetOrAddAsync(KEY_GenerateToken.FormatWith(access_token), factory, CacheStrategy.ONE_HOURS);
        }
    }
}