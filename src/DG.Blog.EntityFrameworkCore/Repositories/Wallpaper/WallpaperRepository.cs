using DG.Blog.Domain.Shared;
using DG.Blog.Domain.Wallpaper.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using static DG.Blog.Domain.Shared.DGBlogDbConsts;

namespace DG.Blog.EntityFrameworkCore.Repositories.Wallpaper
{
    public class WallpaperRepository : EfCoreRepository<DGBlogDbContext, Domain.Wallpaper.Wallpaper, Guid>, IWallpaperRepository
    {
        public WallpaperRepository(IDbContextProvider<DGBlogDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="wallpapers"></param>
        /// <returns></returns>
        public async Task BulkInsertAsync(IEnumerable<Domain.Wallpaper.Wallpaper> wallpapers)
        {
            await DbContext.Set<Domain.Wallpaper.Wallpaper>().AddRangeAsync(wallpapers);
            await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 获取一条随机数据Pgsql
        /// </summary>
        /// <returns></returns>
        public async Task<Domain.Wallpaper.Wallpaper> GetRandomAsync(int type)
        {
            var sql = $"SELECT * FROM public.\"{ DGBlogConsts.DbTablePrefix + DbTableName.Wallpapers}\" tablesample system(1) where \"Type\"={type} limit 1";
            
            int count = 0;
            while (count < 5)
            {
                var random = await DbContext.Set<Domain.Wallpaper.Wallpaper>().FromSqlRaw(sql).FirstOrDefaultAsync();
                if (random != null)
                    return random;
                count++;
            }
            return null;
        }
    }
}