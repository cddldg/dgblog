using DG.Blog.Web.Response.Base;
using DG.Blog.Web.Response.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DG.Blog.Web.Components
{
    public class HotBase : BaseComp
    {
        protected int SourcesId = 1;

        /// <summary>
        /// 文章详情数据
        /// </summary>
        protected ServiceResult<IEnumerable<EnumResponse>> Sources;

        protected ServiceResult<IEnumerable<HotNewsDto>> HotNews;

        /// <summary>
        /// 初始化
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await Common.SetTitleAsync("🚀~~~每日热点~~~🚀");
            await FetchData();
        }

        /// <summary>
        /// 请求数据，渲染页面
        /// <returns></returns>
        protected async Task FetchData()
        {
            Sources = await Http.GetFromJsonAsync<ServiceResult<IEnumerable<EnumResponse>>>($"/hotnews/sources");
            HotNews = await Http.GetFromJsonAsync<ServiceResult<IEnumerable<HotNewsDto>>>($"hotnews?sourceId=1");
        }

        /// <summary>
        /// 请求数据，渲染页面
        /// <returns></returns>
        protected async Task FetchData(int sourceId)
        {
            HotNews = await Http.GetFromJsonAsync<ServiceResult<IEnumerable<HotNewsDto>>>($"hotnews?sourceId={sourceId}");
        }
    }
}