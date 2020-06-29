using DG.Blog.Application.Caching.Wallpaper;
using DG.Blog.Application.Contracts.Wallpaper;
using DG.Blog.Application.Contracts.Wallpaper.Params;
using DG.Blog.Domain.Shared.Enum;
using DG.Blog.Domain.Wallpaper.Repositories;
using DG.Blog.ToolKits.Base;
using DG.Blog.ToolKits.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using static DG.Blog.Domain.Shared.DGBlogConsts;

namespace DG.Blog.Application.Wallpaper.Impl
{
    public class WallpaperService : ServiceBase, IWallpaperService
    {
        private readonly IWallpaperCacheService _wallpaperCacheService;
        private readonly IWallpaperRepository _wallpaperRepository;

        public WallpaperService(IWallpaperCacheService wallpaperCacheService,
                                IWallpaperRepository wallpaperRepository)
        {
            _wallpaperCacheService = wallpaperCacheService;
            _wallpaperRepository = wallpaperRepository;
        }

        /// <summary>
        /// 获取所有壁纸类型
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<EnumResponse>>> GetWallpaperTypesAsync()
        {
            return await _wallpaperCacheService.GetWallpaperTypesAsync(async () =>
            {
                var result = new ServiceResult<IEnumerable<EnumResponse>>();

                var types = typeof(WallpaperEnum).TryToList().Where(p => p.Value > 0);
                result.IsSuccess(types);

                return await Task.FromResult(result);
            });
        }

        /// <summary>
        /// 分页查询壁纸
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult<PagedList<WallpaperDto>>> QueryWallpapersAsync(QueryWallpapersInput input)
        {
            return await _wallpaperCacheService.QueryWallpapersAsync(input, async () =>
            {
                var result = new ServiceResult<PagedList<WallpaperDto>>();

                var query = _wallpaperRepository.WhereIf(input.Type != -1, x => x.Type == input.Type)
                                                .WhereIf(input.Type == -1, x => x.Type > (int)WallpaperEnum.BgImage)
                                                .WhereIf(input.Keywords.IsNotNullOrEmpty(), x => x.Title.Contains(input.Keywords))
                                                .OrderByDescending(x => x.CreateTime);
                var count = query.Count();
                var wallpapers = query.PageByIndex(input.Page, input.Limit);

                var list = ObjectMapper.Map<IEnumerable<Domain.Wallpaper.Wallpaper>, List<WallpaperDto>>(wallpapers);

                result.IsSuccess(new PagedList<WallpaperDto>(count, list));
                return await Task.FromResult(result);
            });
        }

        /// <summary>
        /// 批量插入壁纸
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult<string>> BulkInsertWallpaperAsync(BulkInsertWallpaperInput input)
        {
            var result = new ServiceResult<string>();

            if (!input.Wallpapers.Any())
            {
                result.IsFailed(ResponseText.DATA_IS_NONE);
                return result;
            }

            var urls = _wallpaperRepository.Where(x => x.Type == (int)input.Type).Select(x => x.Url).ToList();

            var wallpapers = ObjectMapper.Map<IEnumerable<WallpaperDto>, IEnumerable<Domain.Wallpaper.Wallpaper>>(input.Wallpapers)
                .Where(x => !urls.Contains(x.Url));
            foreach (var item in wallpapers)
            {
                item.Type = (int)input.Type;
                item.CreateTime = item.Url.Split("/").Last().Split("_").First().TryToDateTime();
            }

            await _wallpaperRepository.BulkInsertAsync(wallpapers);

            result.IsSuccess(ResponseText.INSERT_SUCCESS);
            return result;
        }

        public async Task<ServiceResult<string>> GetRandomWallpaperAsync(int type)
        {
            var result = new ServiceResult<string>();
            //var random = await _wallpaperRepository.GetRandomAsync(type);
            var list=await QueryWallpapersAsync(new QueryWallpapersInput { Type = (int)WallpaperEnum.BgImage, Page = 1, Limit = 100000 });
            var random = list?.Result?.Item?.Randomize(RandomHelper.GetRandom(1, 10))?.FirstOrDefault();
            if (random == null)
            {
                result.IsFailed(ResponseText.DATA_IS_NONE);
                return result;
            }
            result.IsSuccess(random.Url);
            return result;
        }
    }
}