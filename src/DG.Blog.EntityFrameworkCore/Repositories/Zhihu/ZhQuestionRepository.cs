using DG.Blog.Domain.Zhihu;
using DG.Blog.Domain.Zhihu.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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