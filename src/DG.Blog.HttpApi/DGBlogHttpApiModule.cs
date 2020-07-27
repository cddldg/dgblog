using DG.Blog.Application;
using DG.Blog.Domain.Configurations;
using DG.Blog.WeChat;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace DG.Blog.HttpApi
{
    [DependsOn(
        typeof(AbpIdentityHttpApiModule),
        typeof(DGBlogApplicationModule),
        typeof(DGBlogWeChatModule)
        )]
    public class DGBlogHttpApiModule : AbpModule
    {
    }
}