using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.PostgreSql;
using DG.Blog.Domain.Configurations;
using DG.Blog.Domain.Shared;
using Volo.Abp;
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.Modularity;
using DG.Blog.Redis;

namespace DG.Blog.BackgroundJobs
{
    [DependsOn(
        typeof(AbpBackgroundJobsHangfireModule),
        typeof(DGBlogRedisModule)
        )]
    public class DGBlogBackgroundJobsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHangfire(config =>
            {
                config.UseStorage(
                    new PostgreSqlStorage(AppSettings.ConnectionStrings,
                    new PostgreSqlStorageOptions
                    {
                        SchemaName = DGBlogConsts.DbTablePrefix + "Hangfire"
                    }));
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseHangfireServer();
            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new[]
                {
                    new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                    {
                        RequireSsl = false,
                        SslRedirect = false,
                        LoginCaseSensitive = true,
                        Users = new []
                        {
                            new BasicAuthAuthorizationUser
                            {
                                Login = AppSettings.Hangfire.Login,
                                PasswordClear =  AppSettings.Hangfire.Password
                            }
                        }
                    })
                },
                DashboardTitle = "任务调度中心"
            });

            // 壁纸数据抓取
            context.UseWallpaperJob();

            // 每日热点数据抓取
            context.UseHotNewsJob();

            // 代理数据抓取
            context.UseProxysJob();

            // 检测代理数据
            context.UseProxyTestJob();
        }
    }
}