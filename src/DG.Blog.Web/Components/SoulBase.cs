using DG.Blog.Web.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DG.Blog.Web.Components
{
    public class SoulBase : BaseComp
    {
        protected ServiceResult<string> Sources;
        //protected ServiceResult<string> GirlImg;

        /// <summary>
        /// 初始化
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await Common.SetTitleAsync("🐔~~~剧毒鸡汤~~~🐔", "毒鸡汤");
            await FetchData();
        }

        /// <summary>
        /// 请求数据，渲染页面
        /// <returns></returns>
        protected async Task FetchData()
        {
            Sources = await Http.GetFromJsonAsync<ServiceResult<string>>($"/soul");
            //GirlImg = await Http.GetFromJsonAsync<ServiceResult<string>>($"/common/girl/imgurl");
        }
    }
}