using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace DG.Blog.Domain.Shared
{
    [DependsOn(typeof(AbpIdentityDomainSharedModule))]
    public class DGBlogDomainSharedModule : AbpModule
    {

    }
}