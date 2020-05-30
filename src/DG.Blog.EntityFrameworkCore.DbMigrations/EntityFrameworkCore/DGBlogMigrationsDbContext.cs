using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace DG.Blog.EntityFrameworkCore.DbMigrations.EntityFrameworkCore
{
    public class DGBlogMigrationsDbContext : AbpDbContext<DGBlogMigrationsDbContext>
    {
        public DGBlogMigrationsDbContext(DbContextOptions<DGBlogMigrationsDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Configure();
        }
    }
}