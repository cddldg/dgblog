using DG.Blog.Web.Commons;
using DG.Blog.Web.Response.Base;
using DG.Blog.Web.Response.Blog;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DG.Blog.Web.Components
{
    public class BaseComp : ComponentBase
    {
        public static IEnumerable<ChannelDto> FMChannel;

        /// <summary>
        /// 获得/设置 IJSRuntime 实例
        /// </summary>
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected HttpClient Http { get; set; }

        [Inject]
        protected Common Common { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Common.SetTitleAsync();
        }
    }
}