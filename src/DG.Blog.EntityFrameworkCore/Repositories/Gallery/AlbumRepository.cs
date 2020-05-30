using DG.Blog.Domain.Gallery;
using DG.Blog.Domain.Gallery.Repositories;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace DG.Blog.EntityFrameworkCore.Repositories.Gallery
{
    public class AlbumRepository : EfCoreRepository<DGBlogDbContext, Album, Guid>, IAlbumRepository
    {
        public AlbumRepository(IDbContextProvider<DGBlogDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}