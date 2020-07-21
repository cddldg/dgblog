using DG.Blog.Application;
using DG.Blog.Domain.Configurations;
using EasyAbp.Abp.WeChat.Official;
using EasyAbp.Abp.WeChat.Official.HttpApi;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace DG.Blog.HttpApi
{
    [DependsOn(
        typeof(AbpIdentityHttpApiModule),
        typeof(DGBlogApplicationModule),
        typeof(AbpWeChatOfficialHttpApiModule)
        )]
    public class DGBlogHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpWeChatOfficialOptions>(op =>
            {
                op.Token = AppSettings.WeiXin.Token;
                op.AppId = AppSettings.WeiXin.AppId;
                op.AppSecret = AppSettings.WeiXin.AppSecret;
                op.OAuthRedirectUrl = AppSettings.WeiXin.OAuthRedirectUrl;
            });
        }
    }
}