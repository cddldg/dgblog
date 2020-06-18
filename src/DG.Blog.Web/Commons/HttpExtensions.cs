using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace DG.Blog.Web.Commons
{
    public static class HttpExtensions
    {
        public static string[] keywd = new string[]
        {
            "sexy", "beauty", "basketball", "china", "dog", "science", "green", "sport", "phone", "car"
        };

        public static async Task<string> GetImage<T>(this HttpClient Http)
        {
            var keyIndex = keywd[new Random().Next(0, 9)];
            var keyPage = new Random().Next(1, 1000);

            string url = $"https://api.pexels.com/v1/search?query={keyIndex}&per_page=1&page={keyPage}";
            Http.DefaultRequestHeaders.Add("Authorization", "563492ad6f917000010000011c4e23f0bba54da2b5fe830bf7121898");
            var image = await Http.GetFromJsonAsync<RootImage>(url);

            return image?.photos?.FirstOrDefault()?.src.large2x ?? "";
        }
    }

    //如果好用，请收藏地址，帮忙分享。
    public class Src
    {
        /// <summary>
        ///
        /// </summary>
        public string large2x { get; set; }
    }

    public class PhotosItem
    {
        /// <summary>
        ///
        /// </summary>
        public Src src { get; set; }
    }

    public class RootImage
    {
        /// <summary>
        ///
        /// </summary>
        public List<PhotosItem> photos { get; set; }
    }
}