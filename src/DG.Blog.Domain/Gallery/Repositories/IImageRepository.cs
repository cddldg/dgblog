using System;
using Volo.Abp.Domain.Repositories;

namespace DG.Blog.Domain.Gallery.Repositories
{
    public interface IImageRepository : IRepository<Image, Guid>
    {
    }
}