using DG.Blog.Domain.Shared.Enum;
using DG.Blog.Domain.Zhihu;
using DG.Blog.Domain.Zhihu.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace DG.Blog.EntityFrameworkCore.Repositories.Zhihu
{
    public class ZhQuestionRepository : EfCoreRepository<DGBlogDbContext, ZhQuestion, int>, IZhQuestionRepository
    {
        public ZhQuestionRepository(IDbContextProvider<DGBlogDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<ZhQuestion>> GetFistListAsync()
        {
            //var query = from a in DbContext.Set<ZhQuestion>().Where(p => p.FistTime && p.Status == QuestionStatus.Normal && p.Subscribes > 0)
            //            from b in DbContext.Set<ZhQuestion>().Where(p => p.QuestionId == a.QuestionId && p.FistTime == false).OrderByDescending(p => p.Id).Take(1).DefaultIfEmpty()
            //            select new
            //            {
            //                a.QuestionId,
            //                a.Title,
            //                b.
            //            };

            return await DbContext.Set<ZhQuestion>().Where(p => p.FistTime && p.Status == QuestionStatus.Normal && p.Subscribes > 0).ToListAsync();
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="chickenSoups"></param>
        /// <returns></returns>
        public async Task BulkInsertAsync(IEnumerable<ZhQuestion> zhQuestions)
        {
            await DbContext.Set<ZhQuestion>().AddRangeAsync(zhQuestions);
            await DbContext.SaveChangesAsync();
        }
    }
}