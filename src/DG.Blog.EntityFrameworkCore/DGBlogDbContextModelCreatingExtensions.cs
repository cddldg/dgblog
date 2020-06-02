using DG.Blog.Domain.Blog;
using DG.Blog.Domain.Gallery;
using DG.Blog.Domain.HotNews;
using DG.Blog.Domain.Shared;
using DG.Blog.Domain.Signature;
using DG.Blog.Domain.Soul;
using DG.Blog.Domain.Wallpaper;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using static DG.Blog.Domain.Shared.DGBlogDbConsts;

namespace DG.Blog.EntityFrameworkCore
{
    public static class DGBlogDbContextModelCreatingExtensions
    {
        public static void Configure(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
            //builder.HasDefaultSchema(DGBlogConsts.DbTablePrefix + "Blog");
            builder.Entity<Post>(b =>
            {
                b.ToTable(DGBlogConsts.DbTablePrefix + DbTableName.Posts);
                b.HasKey(x => x.Id);
                b.Property(x => x.Title).HasMaxLength(200).IsRequired();
                b.Property(x => x.Author).HasMaxLength(10);
                b.Property(x => x.Url).HasMaxLength(100).IsRequired();
                //b.Property(x => x.Html).HasColumnType("longtext").IsRequired();
                //b.Property(x => x.Markdown).HasColumnType("longtext").IsRequired();
                //b.Property(x => x.CategoryId).HasColumnType("int");
                //b.Property(x => x.CreationTime).HasColumnType("datetime");
            });

            builder.Entity<Category>(b =>
            {
                b.ToTable(DGBlogConsts.DbTablePrefix + DbTableName.Categories);
                b.HasKey(x => x.Id);
                b.Property(x => x.CategoryName).HasMaxLength(50).IsRequired();
                b.Property(x => x.DisplayName).HasMaxLength(50).IsRequired();
            });

            builder.Entity<Tag>(b =>
            {
                b.ToTable(DGBlogConsts.DbTablePrefix + DbTableName.Tags);
                b.HasKey(x => x.Id);
                b.Property(x => x.TagName).HasMaxLength(50).IsRequired();
                b.Property(x => x.DisplayName).HasMaxLength(50).IsRequired();
            });

            builder.Entity<PostTag>(b =>
            {
                b.ToTable(DGBlogConsts.DbTablePrefix + DbTableName.PostTags);
                b.HasKey(x => x.Id);
                //b.Property(x => x.PostId).HasColumnType("int").IsRequired();
                // b.Property(x => x.TagId).HasColumnType("int").IsRequired();
            });

            builder.Entity<FriendLink>(b =>
            {
                b.ToTable(DGBlogConsts.DbTablePrefix + DbTableName.Friendlinks);
                b.HasKey(x => x.Id);
                b.Property(x => x.Title).HasMaxLength(20).IsRequired();
                b.Property(x => x.LinkUrl).HasMaxLength(100).IsRequired();
            });

            builder.Entity<Signature>(b =>
            {
                b.ToTable(DGBlogConsts.DbTablePrefix + DbTableName.Signatures);
                b.HasKey(x => x.Id);
                b.Property(x => x.Id).ValueGeneratedOnAdd();
                b.Property(x => x.Name).HasMaxLength(20).IsRequired();
                b.Property(x => x.Type).HasMaxLength(20).IsRequired();
                b.Property(x => x.Url).HasMaxLength(100).IsRequired();
                b.Property(x => x.Ip).HasMaxLength(50).IsRequired();
                //b.Property(x => x.CreateTime).HasColumnType("datetime");
            });

            builder.Entity<Wallpaper>(b =>
            {
                b.ToTable(DGBlogConsts.DbTablePrefix + DbTableName.Wallpapers);
                b.HasKey(x => x.Id);
                b.Property(x => x.Id).ValueGeneratedOnAdd();
                b.Property(x => x.Url).HasMaxLength(200).IsRequired();
                b.Property(x => x.Title).HasMaxLength(100).IsRequired();
                //b.Property(x => x.Type).HasColumnType("int").IsRequired();
                //b.Property(x => x.CreateTime).HasColumnType("datetime").IsRequired();
            });

            builder.Entity<HotNews>(b =>
            {
                b.ToTable(DGBlogConsts.DbTablePrefix + DbTableName.HotNews);
                b.HasKey(x => x.Id);
                b.Property(x => x.Id).ValueGeneratedOnAdd();
                b.Property(x => x.Title).HasMaxLength(200).IsRequired();
                b.Property(x => x.Url).HasMaxLength(250).IsRequired();
                //b.Property(x => x.SourceId).HasColumnType("int").IsRequired();
                //b.Property(x => x.CreateTime).HasColumnType("datetime").IsRequired();
            });

            builder.Entity<ChickenSoup>(b =>
            {
                b.ToTable(DGBlogConsts.DbTablePrefix + DbTableName.ChickenSoups);
                b.HasKey(x => x.Id);
                b.Property(x => x.Id).ValueGeneratedOnAdd();
                b.Property(x => x.Content).HasMaxLength(200).IsRequired();
            });

            builder.Entity<Album>(b =>
            {
                b.ToTable(DGBlogConsts.DbTablePrefix + DbTableName.Albums);
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).HasMaxLength(100).IsRequired();
                b.Property(x => x.ImgUrl).HasMaxLength(200).IsRequired();
                //b.Property(x => x.IsPublic).HasColumnType("bool").IsRequired();
                b.Property(x => x.Password).HasMaxLength(20).IsRequired();
                //b.Property(x => x.CreateTime).HasColumnType("datetime").IsRequired();
            });

            builder.Entity<Image>(b =>
            {
                b.ToTable(DGBlogConsts.DbTablePrefix + DbTableName.Images);
                b.HasKey(x => x.Id);
                //b.Property(x => x.AlbumId).HasColumnType("char(36)").IsRequired();
                b.Property(x => x.ImgUrl).HasMaxLength(200).IsRequired();
                //b.Property(x => x.Width).HasColumnType("int").IsRequired();
                //b.Property(x => x.Height).HasColumnType("int").IsRequired();
                //b.Property(x => x.CreateTime).HasColumnType("datetime").IsRequired();
            });
        }
    }
}