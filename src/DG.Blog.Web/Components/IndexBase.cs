using DG.Blog.Web.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DG.Blog.Web.Components
{
    public class IndexBase : BaseComp
    {
        protected string CurrentTheme = "Dark";

        /// <summary>
        /// 是否隐藏
        /// </summary>
        protected bool IsHidden = true;

        /// <summary>
        /// 二维码CSS
        /// </summary>
        protected string QrCodeCssClass => IsHidden ? "hidden" : null;

        /// <summary>
        /// 鼠标移入移出操作
        /// </summary>
        protected void Hover() => IsHidden = !IsHidden;

        protected string BgImage = "";//$"/images/index/{new Random().Next(1, 9)}.jpg";

        /// <summary>
        /// 初始化
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await Common.SetTitleAsync();
            await FetchData();
        }

        /// <summary>
        /// 请求数据，渲染页面
        /// <returns></returns>
        protected async Task FetchData()
        {
            BgImage = await Http.GetImage<object>();
            BgImage = string.IsNullOrWhiteSpace(BgImage) ? $"/images/index/{new Random().Next(1, 9)}.jpg" : BgImage;
        }
    }
}