using DG.Blog.Redis;
using DG.Blog.ToolKits.Helper;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.IO;
using PuppeteerSharp;
using MimeKit;
using MimeKit.Utils;
using DG.Blog.ToolKits.Extensions;
using System.Net;
using System.Net.Http;
using DG.Blog.Domain.Wallpaper.Repositories;
using DG.Blog.Domain.Wallpaper;
using DG.Blog.Domain.Shared.Enum;

namespace DG.Blog.BackgroundJobs.Jobs
{
    public class BgImageJob : ITransientDependency
    {
        private readonly DGBlogRedisContext _redis;
        private readonly IHttpClientFactory _httpClient;
        private const string KEY_BgImageJob = "BgImageJob";
        private readonly IWallpaperRepository _wallpaperRepository;
        public BgImageJob(DGBlogRedisContext redisContext, IHttpClientFactory httpClient, IWallpaperRepository wallpapers)
        {
            _redis = redisContext;
            _httpClient = httpClient;
            _wallpaperRepository = wallpapers;
        }

        /// <summary>
        /// 背景图片数据抓取
        /// </summary>
        /// <returns></returns>
        public async Task RunAsync()
        {
            try
            {
                LoggerHelper.Write($"背景图片数据抓取 {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                var per = 80;
                var pageUrls = new List<string>
                {
                    $"https://api.pexels.com/v1/search?query=sexy&per_page={per}&page={new Random().Next(1, 100)}",
                    $"https://api.pexels.com/v1/search?query=beauty&per_page={per}&page={new Random().Next(1, 100)}",
                    $"https://api.pexels.com/v1/search?query=basketball&per_page={per}&page={new Random().Next(1, 100)}",
                    $"https://api.pexels.com/v1/search?query=china&per_page={per}&page={new Random().Next(1, 100)}",
                    $"https://api.pexels.com/v1/search?query=dog&per_page={per}&page={new Random().Next(1, 100)}",
                    $"https://api.pexels.com/v1/search?query=science&per_page={per}&page={new Random().Next(1, 100)}",
                    $"https://api.pexels.com/v1/search?query=green&per_page={per}&page={new Random().Next(1, 100)}",
                    $"https://api.pexels.com/v1/search?query=sport&per_page={per}&page={new Random().Next(1, 100)}",
                    $"https://api.pexels.com/v1/search?query=phone&per_page={per}&page={new Random().Next(1, 100)}",
                    $"https://api.pexels.com/v1/search?query=car&per_page={per}&page={new Random().Next(1, 100)}",
                    $"https://api.pexels.com/v1/search?query=lingerie&per_page={per}&page={new Random().Next(1, 100)}",
                    $"https://api.pexels.com/v1/search?query=pretty&per_page={per}&page={new Random().Next(1, 100)}",
                    $"https://api.pexels.com/v1/search?query=couple&per_page={per}&page={new Random().Next(1, 100)}",
                    

                };

                var list_task = new List<Task<HtmlDocument>>();

                var web = new HtmlWeb();

                pageUrls.ForEach(item =>
                {
                    var task = Task.Run(async () =>
                    {
                        using var client = _httpClient.CreateClient();

                        client.DefaultRequestHeaders.Add("Authorization", "563492ad6f917000010000011c4e23f0bba54da2b5fe830bf7121898");
                        var httpResponse = await client.GetAsync(item);
                        var obj = await httpResponse.Content.ReadAsStringAsync();
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(obj);
                        return doc;
                    });
                    list_task.Add(task);
                });
                Task.WaitAll(list_task.ToArray());

                var wallpapers = new List<Wallpaper>();

                foreach (var list in list_task)
                {
                    var item = await list;

                    try
                    {
                        var obj = JObject.Parse(item.ParsedText);
                        var nodes = obj["photos"];
                        foreach (var node in nodes)
                        {
                            wallpapers.Add(new Wallpaper
                            {
                                Url = node["src"]["large2x"].ToString(),
                                Title = node["photographer"].ToString(),
                                Type = (int)WallpaperEnum.BgImage,
                                CreateTime = DateTime.Now
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Write(ex, $"背景图片数据抓取 list_task异常：本次抓取异常 {item} ");
                    }
                }
                var urls = _wallpaperRepository.GetListAsync().Result.Select(x => x.Url);
                wallpapers = wallpapers.Where(x => !urls.Contains(x.Url)).ToList();
                if (wallpapers.Any())
                {
                    await _wallpaperRepository.BulkInsertAsync(wallpapers);
                }

                _ = SendingAsync(wallpapers.Count());

                LoggerHelper.Write($"背景图片数据抓取  本次抓取到{wallpapers.Count()}条数据，时间:{DateTime.Now:yyyy-MM-dd HH:mm:ss}.");
            }
            catch (Exception ex)
            {
                LoggerHelper.Write(ex, $"背景图片数据抓取 异常：HotNewsJob本次抓取异常, {DateTime.Now:yyyy -MM-dd HH:mm:ss}");
            }
        }

        public async Task SendingAsync(int count, string url = null, string path = null)
        {
            url ??= "http://dldg.ink";
            path ??= Path.Combine(Path.GetTempPath(), "DG.png");
            var isOk = await ScreensAsync(url, path);

            await EmailAsync(count, path, isOk);
        }

        private async Task<bool> ScreensAsync(string url, string path)
        {
            #region Puppeteer访问指定URL,保存为图片

            try
            {
                await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

                using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true,
                    Args = new string[] { "--no-sandbox" }
                });

                using var page = await browser.NewPageAsync();
                await page.SetViewportAsync(new ViewPortOptions
                {
                    Width = 1920,
                    Height = 1080
                });

                await page.GoToAsync(url, WaitUntilNavigation.Networkidle0);
                await page.ScreenshotAsync(path, new ScreenshotOptions
                {
                    FullPage = true,
                    Type = ScreenshotType.Png
                });
                LoggerHelper.Write($"{url} 截屏成功 {path}");
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Write(ex, $"{url} 截屏失败 {path}");
                return false;
            }

            #endregion Puppeteer访问指定URL,保存为图片
        }

        private async Task EmailAsync(int count, string path, bool isOk)
        {
            try
            {
                var builder = new BodyBuilder();
                if (isOk)
                {
                    var image = builder.LinkedResources.Add(path);
                    image.ContentId = MimeUtils.GenerateMessageId();
                    builder.HtmlBody = "本次抓取到{0}条数据，时间:{1}.<img src=\"cid:{2}\"/>".FormatWith(count, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), image.ContentId);
                }
                else
                    builder.HtmlBody = "本次抓取到{0}条数据，时间:{1}.".FormatWith(count, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                var message = new MimeMessage
                {
                    Subject = "【定时任务】背景图片数据抓取任务推送",
                    Body = builder.ToMessageBody()
                };
                await EmailHelper.SendAsync(message);
                LoggerHelper.Write($"邮件发送成功 {isOk} {count}");
            }
            catch (Exception ex)
            {
                LoggerHelper.Write(ex, $"邮件发送失败 {isOk} {count}");
            }
        }
    }
}