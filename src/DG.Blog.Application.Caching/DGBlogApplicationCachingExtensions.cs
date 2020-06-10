using CSRedis;
using DG.Blog.Domain.Configurations;
using DG.Blog.ToolKits.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DG.Blog.Application.Caching
{
    public static class DGBlogApplicationCachingExtensions
    {
        /// <summary>
        /// IDistributedCache替换为CSRedis
        /// </summary>
        /// <param name="service"></param>
        public static void AddDistributedCSRedisCache(this IServiceCollection service)
        {
            RedisHelper.Initialization(new CSRedisClient(AppSettings.Caching.RedisConnectionString));
            service.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));
        }

        /// <summary>
        /// 获取或添加缓存
        /// </summary>
        /// <typeparam name="TCacheItem"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static async Task<TCacheItem> GetOrAddAsync<TCacheItem>(this IDistributedCache cache, string key, Func<Task<TCacheItem>> factory, int minutes)
        {
            TCacheItem cacheItem;

            var result = await cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(result))
            {
                cacheItem = await factory.Invoke();

                var options = new DistributedCacheEntryOptions();
                if (minutes != -1)
                {
                    options.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(minutes);
                }

                await cache.SetStringAsync(key, cacheItem.ToJson(), options);
            }
            else
            {
                cacheItem = result.FromJson<TCacheItem>();
            }

            return cacheItem;
        }
    }
}