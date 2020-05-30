using DG.Blog.Application.Contracts.Gallery;
using DG.Blog.Application.Contracts.Gallery.Params;
using DG.Blog.Application.Gallery;
using DG.Blog.ToolKits.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using static DG.Blog.Domain.Shared.DGBlogConsts;

namespace DG.Blog.HttpApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = Grouping.GroupName_v3)]
    public class GalleryController : AbpController
    {
        private readonly IGalleryService _galleryService;

        public GalleryController(IGalleryService galleryService)
        {
            _galleryService = galleryService;
        }

        /// <summary>
        /// 查询图集列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ServiceResult<IEnumerable<AlbumDto>>> QueryAlbumsAsync()
        {
            return await _galleryService.QueryAlbumsAsync();
        }

        /// <summary>
        /// 根据图集参数查询图片列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("images")]
        public async Task<ServiceResult<IEnumerable<ImageDto>>> QueryImagesAsync([FromQuery] QueryImagesInput input)
        {
            return await _galleryService.QueryImagesAsync(input);
        }
    }
}