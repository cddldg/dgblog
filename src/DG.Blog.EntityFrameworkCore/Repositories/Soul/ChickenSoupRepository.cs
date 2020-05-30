using DG.Blog.Domain.Shared;
using DG.Blog.Domain.Soul;
using DG.Blog.Domain.Soul.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using static DG.Blog.Domain.Shared.DGBlogDbConsts;

namespace DG.Blog.EntityFrameworkCore.Repositories.Soul
{
    public class ChickenSoupRepository : EfCoreRepository<DGBlogDbContext, ChickenSoup, Guid>, IChickenSoupRepository
    {
        public ChickenSoupRepository(IDbContextProvider<DGBlogDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 获取一条随机数据
        /// </summary>
        /// <returns></returns>
        public async Task<ChickenSoup> GetRandomAsync()
        {
            var sql = $"SELECT * FROM {DGBlogConsts.DbTablePrefix + DbTableName.ChickenSoups} ORDER BY RAND() LIMIT 1";
            return await DbContext.Set<ChickenSoup>().FromSqlRaw(sql).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="chickenSoups"></param>
        /// <returns></returns>
        public async Task BulkInsertAsync(IEnumerable<ChickenSoup> chickenSoups)
        {
            await DbContext.Set<ChickenSoup>().AddRangeAsync(chickenSoups);
            await DbContext.SaveChangesAsync();
        }
    }
}