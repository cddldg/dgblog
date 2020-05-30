using DG.Blog.Application.Caching;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace DG.Blog.Application
{
    [DependsOn(
        typeof(AbpIdentityApplicationModule),
        typeof(AbpAutoMapperModule),
        typeof(DGBlogApplicationCachingModule)
        )]
    public class DGBlogApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<DGBlogApplicationModule>(validate: true);
                options.AddProfile<DGBlogAutoMapperProfile>(validate: true);
            });
        }
    }
}