using DG.Blog.Domain.Shared;
using EasyAbp.Abp.WeChat.Official;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace DG.Blog.Domain
{
    [DependsOn(
        typeof(AbpIdentityDomainModule),
        typeof(DGBlogDomainSharedModule),
        typeof(AbpWeChatOfficialModule)
    )]
    public class DGBlogDomainModule : AbpModule
    {
    }
}