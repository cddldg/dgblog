using DG.Blog.Web.Response.Base;
using DG.Blog.Web.Response.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DG.Blog.Web.Components
{
    public class WallpaperBase : BaseComp
    {
        protected int TypeId = -1;

        /// <summary>
        /// 类型
        /// </summary>
        protected ServiceResult<IEnumerable<EnumResponse>> Types;

        /// <summary>
        /// 数据
        /// </summary>
        protected ServiceResult<PagedList<WallpaperDto>> Sources;

        /// <summary>
        /// 初始化
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await Common.SetTitleAsync("📱~~~手机壁纸~~~📱");
            await FetchData();
        }

        /// <summary>
        /// 请求数据，渲染页面
        /// <returns></returns>
        protected async Task FetchData()
        {
            Sources = await Http.GetFromJsonAsync<ServiceResult<PagedList<WallpaperDto>>>("/wallpaper?Type=-1");

            Types = await Http.GetFromJsonAsync<ServiceResult<IEnumerable<EnumResponse>>>($"/wallpaper/types");
        }

        /// <summary>
        /// 请求数据，渲染页面
        /// <returns></returns>
        protected async Task FetchData(int type, int page = 1)
        {
            Sources = await Http.GetFromJsonAsync<ServiceResult<PagedList<WallpaperDto>>>($"/wallpaper?Type={type}&Page={page}");
        }
    }
}