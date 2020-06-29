using DG.Blog.Web.Commons;
using DG.Blog.Web.Response.Base;
using DG.Blog.Web.Response.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
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
            BgImage = (await Http.GetFromJsonAsync<ServiceResult<string>>($"/wallpaper/random?type=0")).Result;
            BgImage = string.IsNullOrWhiteSpace(BgImage) ? $"/images/index/{new Random().Next(1, 9)}.jpg" : BgImage;
            FMChannel = (await Http.GetFromJsonAsync<ServiceResult<IEnumerable<ChannelDto>>>($"/fm/channels")).Result;

            var count = 0;
            while (count < 10)
            {
                count += await AddSongsAsync();
            }
        }

        private async Task<int> AddSongsAsync()
        {
            var count = 0;
            var songs = (await Http.GetFromJsonAsync<ServiceResult<IEnumerable<FMDto>>>($"/fm/random")).Result;
            if (songs != null && songs.Any())
            {
                count = songs.Count();
                var ap = songs.Select(p => new
                {
                    name = p.AlbumTitle,
                    artist = p.Artist,
                    url = p.Url,
                    lrc = p.Lyric,
                    cover = p.Picture
                }).ToArray();
                await Common.AddAplayerAsync(ap);
            }
            return count;
        }
    }
}