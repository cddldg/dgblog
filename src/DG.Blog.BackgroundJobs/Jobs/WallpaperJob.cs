﻿using HtmlAgilityPack;
using DG.Blog.Application.Contracts.Wallpaper;
using DG.Blog.Domain.Configurations;
using DG.Blog.Domain.Shared.Enum;
using DG.Blog.Domain.Wallpaper;
using DG.Blog.Domain.Wallpaper.Repositories;
using DG.Blog.ToolKits.Extensions;
using DG.Blog.ToolKits.Helper;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using System.Net;
using DG.Blog.Redis;
using System.Text;

namespace DG.Blog.BackgroundJobs.Jobs
{
    public class WallpaperJob : ITransientDependency
    {
        private readonly IWallpaperRepository _wallpaperRepository;
        private readonly DGBlogRedisContext _redis;

        public WallpaperJob(IWallpaperRepository wallpaperRepository, DGBlogRedisContext redisContext)
        {
            _wallpaperRepository = wallpaperRepository;
            _redis = redisContext;
        }

        /// <summary>
        /// 壁纸数据抓取
        /// </summary>
        /// <returns></returns>
        public async Task RunAsync()
        {
            try
            {
                LoggerHelper.Write($"壁纸数据抓取 {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                var wallpaperUrls = new List<WallpaperJobItem<string>>
            {
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_1_1.html", Type = WallpaperEnum.Beauty },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_58_1.html", Type = WallpaperEnum.Sportsman },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_66_1.html", Type = WallpaperEnum.CuteBaby },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_4_1.html", Type = WallpaperEnum.Emotion },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_3_1.html", Type = WallpaperEnum.Landscape },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_9_1.html", Type = WallpaperEnum.Animal },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_13_1.html", Type = WallpaperEnum.Plant },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_64_1.html", Type = WallpaperEnum.Food },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_11_1.html", Type = WallpaperEnum.Movie },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_5_1.html", Type = WallpaperEnum.Anime },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_34_1.html", Type = WallpaperEnum.HandPainted },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_65_1.html", Type = WallpaperEnum.Text },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_2_1.html",  Type = WallpaperEnum.Creative },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_10_1.html", Type = WallpaperEnum.Car },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_14_1.html", Type = WallpaperEnum.PhysicalEducation },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_63_1.html", Type = WallpaperEnum.Other },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_17_1.html", Type = WallpaperEnum.Military },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_15_1.html", Type = WallpaperEnum.Festival },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_12_1.html", Type = WallpaperEnum.Game },
                new WallpaperJobItem<string> { Result = "https://www.i4.cn/wper_4_19_7_1.html", Type = WallpaperEnum.Apple }
            };

                var web = new HtmlWeb();

                var list_task = new List<Task<WallpaperJobItem<HtmlDocument>>>();
                //var proxy = await _redis.ZRandomAsync();
                //var hasProxy = !string.IsNullOrWhiteSpace(proxy);
                wallpaperUrls.ForEach(item =>
                {
                    var task = Task.Run(async () =>
                    {
                        var obj = new HtmlDocument();
                        //if (hasProxy)
                        //{
                        //    WebClient wc = new WebClient();
                        //    wc.Proxy = new WebProxy(proxy);
                        //    obj.LoadHtml(await wc.DownloadStringTaskAsync(item.Result));
                        //}
                        //else
                        obj = await web.LoadFromWebAsync(item.Result);
                        return new WallpaperJobItem<HtmlDocument>
                        {
                            Result = obj,
                            Type = item.Type
                        };
                    });
                    list_task.Add(task);
                });
                Task.WaitAll(list_task.ToArray());

                var wallpapers = new List<Wallpaper>();

                foreach (var list in list_task)
                {
                    var item = await list;

                    var imgs = item.Result.DocumentNode.SelectNodes("//article[@id='wper']/div[@class='jbox']/div[@class='kbox']/div/a/img[1]").ToList();
                    imgs.ForEach(x =>
                    {
                        wallpapers.Add(new Wallpaper
                        {
                            Url = x.GetAttributeValue("data-big", ""),
                            Title = x.GetAttributeValue("title", ""),
                            Type = (int)item.Type,
                            CreateTime = x.Attributes["data-big"].Value.Split("/").Last().Split("_").First().TryToDateTime()
                        });
                    });
                }

                var urls = _wallpaperRepository.GetListAsync().Result.Select(x => x.Url);
                wallpapers = wallpapers.Where(x => !urls.Contains(x.Url)).ToList();
                if (wallpapers.Any())
                {
                    await _wallpaperRepository.BulkInsertAsync(wallpapers);
                    _ = EmailAsync(wallpapers.Count());
                }

                LoggerHelper.Write($"壁纸数据抓取 hasProxy= 本次抓取到{wallpapers.Count()}条数据，时间:{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            }
            catch (Exception ex)
            {
                LoggerHelper.Write(ex, $"壁纸数据抓取 异常：WallpaperJob本次抓取异常, {DateTime.Now:yyyy -MM-dd HH:mm:ss}");
            }
        }

        private async Task EmailAsync(int count)
        {
            try
            {
                var builder = new BodyBuilder
                {
                    HtmlBody = "本次抓取到{0}条数据，时间:{1}.".FormatWith(count, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                };

                var message = new MimeMessage
                {
                    Subject = "【定时任务】壁纸数据抓取数据抓取任务推送",
                    Body = builder.ToMessageBody()
                };
                await EmailHelper.SendAsync(message);
                LoggerHelper.Write($"邮件发送成功 {count}");
            }
            catch (Exception ex)
            {
                LoggerHelper.Write(ex, $"邮件发送失败 {count}");
            }
        }
    }
}