using DG.Blog.Domain;
using DG.Blog.Domain.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace DG.Blog.Application.Caching
{
    [DependsOn(
        typeof(AbpCachingModule),
        typeof(DGBlogDomainModule)
        )]
    public class DGBlogApplicationCachingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddDistributedRedisCache(options =>
            {
                options.Configuration = AppSettings.Caching.RedisConnectionString;
            });
        }
    }
}