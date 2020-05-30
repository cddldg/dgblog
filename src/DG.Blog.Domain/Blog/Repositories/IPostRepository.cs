using Volo.Abp.Domain.Repositories;

namespace DG.Blog.Domain.Blog.Repositories
{
    /// <summary>
    /// IPostRepository
    /// </summary>
    public interface IPostRepository : IRepository<Post, int>
    {
        
    }
}