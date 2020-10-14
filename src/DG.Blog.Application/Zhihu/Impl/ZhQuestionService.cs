using DG.Blog.Domain.Zhihu.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DG.Blog.Application.Zhihu.Impl
{
    public class ZhQuestionService: ServiceBase, IZhQuestionService
    {
        private readonly IZhQuestionRepository _zhRepository;
        public ZhQuestionService(IZhQuestionRepository zhRepository)
        {
            _zhRepository = zhRepository;
        }
    }
}
