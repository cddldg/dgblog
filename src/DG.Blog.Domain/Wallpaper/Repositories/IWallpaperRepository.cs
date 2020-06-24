using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace DG.Blog.Domain.Wallpaper.Repositories
{
    /// <summary>
    /// IWallpaperRepository
    /// </summary>
    public interface IWallpaperRepository : IRepository<Wallpaper, Guid>
    {
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="wallpapers"></param>
        /// <returns></returns>
        Task BulkInsertAsync(IEnumerable<Wallpaper> wallpapers);

        /// <summary>
        /// 随机取背景图片
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<Wallpaper> GetRandomAsync(int type);
    }
}