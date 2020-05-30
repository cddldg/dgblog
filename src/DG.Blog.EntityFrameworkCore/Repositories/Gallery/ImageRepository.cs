using DG.Blog.Domain.Gallery;
using DG.Blog.Domain.Gallery.Repositories;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace DG.Blog.EntityFrameworkCore.Repositories.Gallery
{
    public class ImageRepository : EfCoreRepository<DGBlogDbContext, Image, Guid>, IImageRepository
    {
        public ImageRepository(IDbContextProvider<DGBlogDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}