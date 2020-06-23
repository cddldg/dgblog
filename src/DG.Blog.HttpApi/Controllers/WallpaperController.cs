﻿using DG.Blog.Application.Contracts.Wallpaper;
using DG.Blog.Application.Contracts.Wallpaper.Params;
using DG.Blog.Application.Wallpaper;
using DG.Blog.ToolKits.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using static DG.Blog.Domain.Shared.DGBlogConsts;

namespace DG.Blog.HttpApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = Grouping.GroupName_v3)]
    public class WallpaperController : AbpController
    {
        private readonly IWallpaperService _wallpaperService;

        public WallpaperController(IWallpaperService wallpaperService)
        {
            _wallpaperService = wallpaperService;
        }

        /// <summary>
        /// 获取所有壁纸类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("types")]
        public async Task<ServiceResult<IEnumerable<EnumResponse>>> GetWallpaperTypesAsync()
        {
            return await _wallpaperService.GetWallpaperTypesAsync();
        }
        /// <summary>
        /// 获取随机壁纸
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("random")]
        public async Task<ServiceResult<string>> GetRandomWallpaperAsync([Required] int type)
        {
            return await _wallpaperService.GetRandomWallpaperAsync(type);
        }
        /// <summary>
        /// 分页查询壁纸
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ServiceResult<PagedList<WallpaperDto>>> QueryWallpapersAsync([FromQuery] QueryWallpapersInput input)
        {
            return await _wallpaperService.QueryWallpapersAsync(input);
        }

        /// <summary>
        /// 批量插入壁纸
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ServiceResult<string>> BulkInsertWallpaperAsync([FromBody] BulkInsertWallpaperInput input)
        {
            return await _wallpaperService.BulkInsertWallpaperAsync(input);
        }
    }
}