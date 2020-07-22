using DG.Blog.Domain.Shared;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace DG.Blog.Domain
{
    [DependsOn(
        typeof(AbpIdentityDomainModule),
        typeof(DGBlogDomainSharedModule)
    )]
    public class DGBlogDomainModule : AbpModule
    {
    }
}