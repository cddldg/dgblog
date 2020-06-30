using DG.Blog.Web.Commons;
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
    public class PostBase : BaseComp
    {
        [Parameter]
        public int Year { get; set; }

        [Parameter]
        public int Month { get; set; }

        [Parameter]
        public int Day { get; set; }

        [Parameter]
        public string Name { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        protected string Url => $"/{Year}/{(Month >= 10 ? Month.ToString() : $"0{Month}")}/{(Day >= 10 ? Day.ToString() : $"0{Day}")}/{Name}/";

        /// <summary>
        /// 文章详情数据
        /// </summary>
        protected ServiceResult<PostDetailDto> Post;

        /// <summary>
        /// 初始化
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await FetchData(Url);
        }

        /// <summary>
        /// 请求数据，渲染页面
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected async Task FetchData(string url, bool isPostNav = false)
        {
            Post = await Http.GetFromJsonAsync<ServiceResult<PostDetailDto>>($"/blog/post?url={url}");
            if (Post.Success)
                await Common.SetTitleAsync(Post.Result.Title, string.Join(",", Post.Result.Tags.Select(p => p.TagName)));
            await Common.InvokeAsync("window.func.renderMarkdown");
        }
    }
}