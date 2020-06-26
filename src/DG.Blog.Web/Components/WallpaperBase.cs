using DG.Blog.Web.Response.Base;
using DG.Blog.Web.Response.Blog;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DG.Blog.Web.Components
{
    public class WallpaperBase : BaseComp
    {
        [Parameter]
        public int? TypeId { get; set; }
        [Parameter]
        public int? Page { get; set; }
        protected int Limit = 50;


        /// <summary>
        /// 总页码
        /// </summary>
        protected int TotalPage;

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
            TypeId ??= -1;
            Page ??= 1;
            await Common.SetTitleAsync("📱~~~手机壁纸~~~📱");
            await FetchData();
        }


        /// <summary>
        /// 请求数据，渲染页面
        /// <returns></returns>
        protected async Task FetchData()
        {
            Types ??= await Http.GetFromJsonAsync<ServiceResult<IEnumerable<EnumResponse>>>($"/wallpaper/types");
            Sources = await Http.GetFromJsonAsync<ServiceResult<PagedList<WallpaperDto>>>($"/wallpaper?Type={TypeId}&Page={Page}&Limit={Limit}");
            CaPage();
        }
        /// <summary>
        /// 请求数据，渲染页面
        /// <returns></returns>
        protected async Task FetchData(int type)
        {
            TypeId = type;
            Page = 1;
            Sources = await Http.GetFromJsonAsync<ServiceResult<PagedList<WallpaperDto>>>($"/wallpaper?Type={TypeId}&Page={Page}&Limit={Limit}");
            CaPage();
        }
        /// <summary>
        /// 点击页码重新渲染数据
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        protected async Task RenderPage(int rPage)
        {
            if(rPage != Page&&rPage<=TotalPage&&rPage>0)
            {
                Page = rPage;
                Types ??= await Http.GetFromJsonAsync<ServiceResult<IEnumerable<EnumResponse>>>($"/wallpaper/types");
                Sources = await Http.GetFromJsonAsync<ServiceResult<PagedList<WallpaperDto>>>($"/wallpaper?Type={TypeId}&Page={Page}&Limit={Limit}");
                CaPage();
            }
        }
        private void CaPage()
        {
            Page = Page.HasValue ? Page : 1;
            // 计算总页码
            TotalPage = (int)Math.Ceiling((Sources.Result.Total / (double)Limit));
        }
    }
}