using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DG.Blog.EntityFrameworkCore.DbMigrations.EntityFrameworkCore
{
    public class DGBlogMigrationsDbContextFactory : IDesignTimeDbContextFactory<DGBlogMigrationsDbContext>
    {
        public DGBlogMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<DGBlogMigrationsDbContext>()
                //.UseMySql(configuration.GetConnectionString("Default"));
                //.UseNpgsql(configuration.GetConnectionString("Sqlite"))
                .UseSqlite(configuration.GetConnectionString("Sqlite"))
                ;

            return new DGBlogMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}