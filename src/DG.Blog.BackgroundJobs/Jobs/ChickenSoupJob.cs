using DG.Blog.Domain.Soul;
using DG.Blog.Domain.Soul.Repositories;
using DG.Blog.ToolKits.Helper;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace DG.Blog.BackgroundJobs.Jobs
{
    public class ChickenSoupJob : ITransientDependency
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IChickenSoupRepository _chickenSoupsRepository;

        public ChickenSoupJob(IHttpClientFactory httpClient, IChickenSoupRepository chickenSoups)
        {
            _httpClient = httpClient;
            _chickenSoupsRepository = chickenSoups;
        }

        /// <summary>
        /// 剧毒鸡汤数据抓取
        /// </summary>
        /// <returns></returns>
        public async Task RunAsync()
        {
            try
            {
                LoggerHelper.Write($"剧毒鸡汤数据抓取 {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                var url = "https://www.nihaowua.com/home.html";
                var list_task = new List<Task<HtmlDocument>>();
                var web = new HtmlWeb();
                for (int i = 0; i < 100; i++)
                {
                    var task = Task.Run(async () =>
                    {
                        using var client = _httpClient.CreateClient();
                        client.DefaultRequestHeaders.Add("User-Agent", ProxyHelper.GetUserAgent());

                        var httpResponse = await client.GetAsync(url);
                        var obj = await httpResponse.Content.ReadAsStringAsync();
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(obj);
                        return doc;
                    });
                    list_task.Add(task);
                    Thread.Sleep(100);
                }
                Task.WaitAll(list_task.ToArray());
                var texts = new List<string>();
                foreach (var list in list_task)
                {
                    var item = await list;
                    if (string.IsNullOrWhiteSpace(item.Text))
                        continue;
                    try
                    {
                        var nodes = item.DocumentNode.SelectNodes("//section/div/*/text()").ToList();
                        nodes.ForEach(x =>
                        {
                            texts.Add(x.InnerText);
                        });
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Write(ex, $"剧毒鸡汤数据抓取 {item}  异常: {DateTime.Now:yyyy -MM-dd HH:mm:ss}");
                    }
                }
                if (texts.Any())
                {
                    var contents = _chickenSoupsRepository.GetListAsync().Result?.Select(x => x.Content);
                    texts = contents == null ? texts : texts.Where(x => !contents.Contains(x)).ToList();
                    if (texts.Any())
                    {
                        var chickenSoups = texts.Select(p => new ChickenSoup { Content = p });
                        await _chickenSoupsRepository.BulkInsertAsync(chickenSoups);
                    }
                }

                LoggerHelper.Write($"剧毒鸡汤数据抓取 本次抓取到{texts.Count()}条数据，时间:{DateTime.Now:yyyy-MM-dd HH:mm:ss}.");
            }
            catch (Exception ex)
            {
                LoggerHelper.Write(ex, $"剧毒鸡汤数据抓取 异常: {DateTime.Now:yyyy -MM-dd HH:mm:ss}");
            }
        }
    }
}