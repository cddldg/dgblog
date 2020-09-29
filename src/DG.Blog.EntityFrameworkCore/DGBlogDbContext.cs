using DG.Blog.Domain.Blog;
using DG.Blog.Domain.Gallery;
using DG.Blog.Domain.HotNews;
using DG.Blog.Domain.Signature;
using DG.Blog.Domain.Soul;
using DG.Blog.Domain.Wallpaper;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace DG.Blog.EntityFrameworkCore
{
    [ConnectionStringName("MySql")]
    public class DGBlogDbContext : AbpDbContext<DGBlogDbContext>
    {
        public DbSet<Post> Posts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        public DbSet<FriendLink> FriendLinks { get; set; }

        public DbSet<Signature> Signatures { get; set; }

        public DbSet<Wallpaper> Wallpapers { get; set; }

        public DbSet<HotNews> HotNews { get; set; }

        public DbSet<ChickenSoup> ChickenSoups { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Image> Images { get; set; }

        public DGBlogDbContext(DbContextOptions<DGBlogDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Configure();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}