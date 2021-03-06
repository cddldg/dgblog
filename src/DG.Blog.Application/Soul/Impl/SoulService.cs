﻿using DG.Blog.Domain.Soul;
using DG.Blog.Domain.Soul.Repositories;
using DG.Blog.ToolKits.Base;
using DG.Blog.ToolKits.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using static DG.Blog.Domain.Shared.DGBlogConsts;

namespace DG.Blog.Application.Soul.Impl
{
    public class SoulService : ServiceBase, ISoulService
    {
        private readonly IChickenSoupRepository _chickenSoupRepository;

        public SoulService(IChickenSoupRepository chickenSoupRepository)
        {
            _chickenSoupRepository = chickenSoupRepository;
        }

        /// <summary>
        /// 获取鸡汤文本
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<string>> GetRandomChickenSoupAsync()
        {
            var result = new ServiceResult<string>();
            var list = await _chickenSoupRepository.GetListAsync();
            var random = list?.Randomize(RandomHelper.GetRandom(1, 10))?.FirstOrDefault();
            if (random == null)
            {
                result.IsFailed(ResponseText.DATA_IS_NONE);
                return result;
            }
            result.IsSuccess(random.Content);
            return result;
        }

        /// <summary>
        /// 批量插入鸡汤
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<ServiceResult<string>> BulkInsertChickenSoupAsync(IEnumerable<string> list)
        {
            var result = new ServiceResult<string>();

            if (!list.Any())
            {
                result.IsFailed(ResponseText.DATA_IS_NONE);
                return result;
            }

            var chickenSoups = list.Select(x => new ChickenSoup { Content = x });
            await _chickenSoupRepository.BulkInsertAsync(chickenSoups);

            result.IsSuccess(ResponseText.INSERT_SUCCESS);
            return result;
        }
    }
}