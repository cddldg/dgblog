using DG.Blog.Domain.Shared;
using DG.Blog.Redis;
using DG.Blog.ToolKits.Extensions;
using DG.Blog.ToolKits.Helper;
using HtmlAgilityPack;
using MimeKit;
using MimeKit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using static DG.Blog.Domain.Shared.DGBlogConsts;

namespace DG.Blog.BackgroundJobs.Jobs
{
    public class ProxysJob : ITransientDependency
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly DGBlogRedisContext _redis;

        public ProxysJob(IHttpClientFactory httpClient, DGBlogRedisContext redisContext)
        {
            _redis = redisContext;
            _httpClient = httpClient;
        }

        /// <summary>
        /// 代理数据抓取
        /// </summary>
        /// <returns></returns>
        public async Task RunAsync()
        {
            try
            {
                LoggerHelper.Write($"代理数据抓取 {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                var count = await _redis.ZCountAsync();
                if (count >= Proxy.POOL_UPPER_THRESHOLD)
                {
                    LoggerHelper.Write($"代理数据抓取 代理数已达到代理池数量界限{Proxy.POOL_UPPER_THRESHOLD} {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    return;
                }
                var pageUrls = new List<(string, int)>
                {
                    ("http://www.ip3366.net/free/?stype=1&page=",1),
                    ("http://www.kuaidaili.com/free/inha/",2),
                    ("https://www.xicidaili.com/nn/",3)
                };
                var proxyUrls = new List<(string, int)>();
                for (int i = 1; i < 5; i++)
                {
                    pageUrls.ForEach(p => proxyUrls.Add(($"{p.Item1}{i}", p.Item2)));
                }

                proxyUrls.Add(("http://www.66ip.cn/nmtq.php?getnum=100&isp=0&anonymoustype=2&start=&ports=&export=&ipaddress=&area=0&proxytype=0&api=66ip", 4));

                var web = new HtmlWeb();
                var list_task = new List<Task<(HtmlDocument, int)>>();

                proxyUrls.ForEach(item =>
                {
                    var task = Task.Run(async () =>
                    {
                        if (item.Item2 == 3)
                        {
                            using var client = _httpClient.CreateClient();

                            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8'");
                            client.DefaultRequestHeaders.Add("Cookie", "_free_proxy_session=BAh7B0kiD3Nlc3Npb25faWQGOgZFVEkiJWRjYzc5MmM1MTBiMDMzYTUzNTZjNzA4NjBhNWRjZjliBjsAVEkiEF9jc3JmX3Rva2VuBjsARkkiMUp6S2tXT3g5a0FCT01ndzlmWWZqRVJNek1WanRuUDBCbTJUN21GMTBKd3M9BjsARg%3D%3D--2a69429cb2115c6a0cc9a86e0ebe2800c0d471b3");
                            client.DefaultRequestHeaders.Add("Host", "www.xicidaili.com");
                            client.DefaultRequestHeaders.Add("Referer", "http://www.xicidaili.com/nn/3");
                            client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1'");
                            client.DefaultRequestHeaders.Add("User-Agent", ProxyHelper.GetUserAgent());

                            var httpResponse = await client.GetAsync(item.Item1);
                            var obj = await httpResponse.Content.ReadAsStringAsync();
                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(obj);
                            return (doc, item.Item2);
                        }
                        else
                        {
                            return (await web.LoadFromWebAsync(item.Item1), item.Item2);
                        }
                    });
                    list_task.Add(task);
                });
                Task.WaitAll(list_task.ToArray());
                var proxys = new List<string>();
                foreach (var list in list_task)
                {
                    var item = await list;
                    if (string.IsNullOrWhiteSpace(item.Item1.Text))
                        continue;
                    if (item.Item2 == 4)
                    {
                        await Get66IpsAsync(item, proxys);
                    }
                    else
                    {
                        var table = item.Item2 == 3 ? "table" : "tbody";
                        try
                        {
                            var nodes = item.Item1.DocumentNode.SelectNodes($"//{table}/tr").ToList();
                            nodes.ForEach(async x =>
                            {
                                var tds = x.SelectNodes("td");
                                if (tds != null)
                                {
                                    for (int i = 0; i < tds.Count; i++)
                                    {
                                        var proxyStr = "";
                                        switch (item.Item2)
                                        {
                                            case 1:
                                            case 2:
                                                proxyStr = $"{tds[0].InnerText.Trim()}:{tds[1].InnerText.Trim()}";
                                                break;

                                            case 3:
                                                proxyStr = $"{tds[1].InnerText.Trim()}:{tds[2].InnerText.Trim()}";
                                                break;

                                            default:
                                                break;
                                        }
                                        var exist = await _redis.ZExistsAsync(proxyStr);
                                        var isip = proxyStr.IsProxyIp();
                                        if (!exist && proxyStr.IsProxyIp())
                                            proxys.Add(proxyStr);
                                    }
                                }
                            });
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.Write(ex, $"代理数据抓取 {item.Item2} item={item.Item1.Text} 异常: {DateTime.Now:yyyy -MM-dd HH:mm:ss}");
                        }
                    }
                }
                if (proxys.Any())
                {
                    proxys.ForEach(p => _redis.ZAddAsync(p));
                }
                _ = EmailAsync(proxys.Count());

                LoggerHelper.Write($"代理数据抓取 本次抓取到{proxys.Count()}条数据，时间:{DateTime.Now:yyyy-MM-dd HH:mm:ss}.");
            }
            catch (Exception ex)
            {
                LoggerHelper.Write(ex, $"代理数据抓取 异常: {DateTime.Now:yyyy -MM-dd HH:mm:ss}");
            }
        }

        public async Task Get66IpsAsync((HtmlDocument, int) item, List<string> proxys)
        {
            string ingContent = item.Item1.Text;
            foreach (Match m in StringExtensions.ProxyIpRegex.Matches(ingContent))
            {
                try
                {
                    var proxyStr = m.Value;
                    var exist = await _redis.ZExistsAsync(proxyStr);
                    if (!exist && proxyStr.IsProxyIp())
                        proxys.Add(proxyStr);
                }
                catch (Exception ex)
                {
                    LoggerHelper.Write(ex, $"代理数据抓取 {item.Item2} item={item.Item1.Text} 异常: {DateTime.Now:yyyy -MM-dd HH:mm:ss}");
                }
            }
        }

        private async Task EmailAsync(int count)
        {
            try
            {
                var builder = new BodyBuilder();

                builder.HtmlBody = "本次抓取到{0}条数据，时间:{1}.".FormatWith(count, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                var message = new MimeMessage
                {
                    Subject = "【定时任务】代理数据抓取任务推送",
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