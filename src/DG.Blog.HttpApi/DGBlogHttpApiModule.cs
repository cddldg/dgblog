using DG.Blog.Application;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace DG.Blog.HttpApi
{
    [DependsOn(
        typeof(AbpIdentityHttpApiModule),
        typeof(DGBlogApplicationModule)
        )]
    public class DGBlogHttpApiModule : AbpModule
    {

    }
}