using DG.Blog.Domain;
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
            context.Services.AddDistributedCSRedisCache();
        }
    }
}