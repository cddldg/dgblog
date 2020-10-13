using Hangfire;
using DG.Blog.BackgroundJobs.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;

namespace DG.Blog.BackgroundJobs
{
    public static class DGBlogBackgroundJobsExtensions
    {
        public static void UseWallpaperJob(this ApplicationInitializationContext context)
        {
            var job = context.ServiceProvider.GetService<WallpaperJob>();

            RecurringJob.AddOrUpdate("壁纸数据抓取", () => job.RunAsync(), CronType.Hour(1, 3));
        }

        public static void UseHotNewsJob(this ApplicationInitializationContext context)
        {
            var job = context.ServiceProvider.GetService<HotNewsJob>();

            RecurringJob.AddOrUpdate("每日热点抓取", () => job.RunAsync(), CronType.Minute(10));
        }

        public static void UseProxysJob(this ApplicationInitializationContext context)
        {
            var job = context.ServiceProvider.GetService<ProxysJob>();

            RecurringJob.AddOrUpdate("代理数据抓取", () => job.RunAsync(), CronType.Hour(20, 1));
        }

        public static void UseProxyTestJob(this ApplicationInitializationContext context)
        {
            var job = context.ServiceProvider.GetService<ProxyTestJob>();

            RecurringJob.AddOrUpdate("检测代理数据", () => job.RunAsync(), CronType.Hour(30, 1));
        }

        public static void UseBgImageJob(this ApplicationInitializationContext context)
        {
            var job = context.ServiceProvider.GetService<BgImageJob>();

            RecurringJob.AddOrUpdate("背景图片数据", () => job.RunAsync(), CronType.Day(1, 1));
        }

        public static void UseChickenSoupJob(this ApplicationInitializationContext context)
        {
            var job = context.ServiceProvider.GetService<ChickenSoupJob>();

            RecurringJob.AddOrUpdate("剧毒鸡汤数据", () => job.RunAsync(), CronType.Hour(40, 3));
        }
    }
}