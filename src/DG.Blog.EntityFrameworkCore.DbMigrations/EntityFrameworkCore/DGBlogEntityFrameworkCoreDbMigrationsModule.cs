using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace DG.Blog.EntityFrameworkCore.DbMigrations.EntityFrameworkCore
{
    [DependsOn(
        typeof(DGBlogFrameworkCoreModule)
        )]
    public class DGBlogEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<DGBlogMigrationsDbContext>();
        }
    }
}