using DG.Blog.Domain.Blog;
using DG.Blog.Domain.Blog.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
namespace DG.Blog.EntityFrameworkCore.Repositories.Blog
{
    /// <summary>
    /// PostTagRepository
    /// </summary>
    public class CategoryRepository : EfCoreRepository<DGBlogDbContext, Category, int>, ICategoryRepository
    {
        public CategoryRepository(IDbContextProvider<DGBlogDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}