using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace DG.Blog.Domain.Zhihu.Repositories
{
    public interface IZhQuestionRepository : IRepository<ZhQuestion, int>
    {
        void GetMonitorQuestions();

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="zhQuestions"></param>
        /// <returns></returns>
        Task BulkInsertAsync(IEnumerable<ZhQuestion> zhQuestions);
    }
}