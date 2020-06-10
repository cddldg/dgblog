using CSRedis;
using DG.Blog.Domain;
using DG.Blog.Domain.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Modularity;

namespace DG.Blog.Redis
{
    [DependsOn(typeof(DGBlogDomainModule))]
    public class DGBlogRedisModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            DefaultRedis.Initialization(new CSRedisClient(AppSettings.Redis.ConnectionString));
            context.Services.AddSingleton(new DGBlogRedisContext());
        }
    }
}