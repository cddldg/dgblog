using DG.Blog.Domain.Shared.Enum;
using DG.Blog.Domain.Zhihu;
using DG.Blog.Domain.Zhihu.Repositories;
using DG.Blog.Redis;
using DG.Blog.ToolKits.Extensions;
using DG.Blog.ToolKits.Helper;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace DG.Blog.BackgroundJobs.Jobs
{
    /// <summary>
    /// 知乎问题
    /// </summary>
    public class ZhihuJob : ITransientDependency
    {
        private readonly IHttpClientFactory _httpClient;

        private readonly IZhQuestionRepository _zhRepository;
        private readonly DGBlogRedisContext _redis;

        public ZhihuJob(IHttpClientFactory httpClient, IZhQuestionRepository zhRepository, DGBlogRedisContext redisContext)
        {
            _httpClient = httpClient;
            _zhRepository = zhRepository;
            _redis = redisContext;
        }

        public async Task RunAsync()
        {
            try
            {
                LoggerHelper.Write($"知乎问题数据抓取 {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                var dbQList = _zhRepository.Where(p => p.FistTime && p.Status == QuestionStatus.Normal && p.Subscribes > 0).ToList();
                var zhQList = new List<ZhQuestion>();
                if (dbQList.Any())
                {
                    dbQList.ForEach(async quitem =>
                    {
                        try
                        {
                            var url = $"https://www.zhihu.com/question/{quitem.QuestionId}";
                            using var client = _httpClient.CreateClient();
                            client.DefaultRequestHeaders.Add("User-Agent", ProxyHelper.GetUserAgent());

                            var httpResponse = await client.GetAsync(url);
                            var obj = await httpResponse.Content.ReadAsStringAsync();
                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(obj);

                            var nodes = doc.DocumentNode.SelectNodes("//strong[@class='NumberBoard-itemValue']").ToList();
                            var goodNode = doc.DocumentNode.SelectNodes("//button[@class='Button GoodQuestionAction-commonBtn Button--plain Button--withIcon Button--withLabel']").FirstOrDefault();
                            var answerNode = doc.DocumentNode.SelectNodes("//h4[@class='List-headerText']").FirstOrDefault();

                            if (quitem.Title.IsNullOrWhiteSpace())
                            {
                                var titleNode = doc.DocumentNode.SelectNodes("//h1[@class='QuestionHeader-title']").FirstOrDefault();
                                quitem.Title = titleNode.InnerText;
                            }
                            else
                                quitem.FistTime = false;

                            quitem.FollowerCount = nodes[0].GetAttributeValue("title", "0").TryToInt();
                            quitem.ViewCount = nodes[1].GetAttributeValue("title", "0").TryToInt();
                            quitem.GoodQuestionCount = goodNode.InnerText.RegexNumStr().TryToInt();
                            quitem.AnswerTotal = answerNode.InnerText.RegexNumStr().TryToInt();
                            quitem.MonitorUpdateTime = DateTime.Now;
                            zhQList.Add(quitem);
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.Write(ex, $"知乎问题数据抓取 {quitem.QuestionId}  异常: {DateTime.Now:yyyy -MM-dd HH:mm:ss}");
                        }
                    });
                }
                else
                {
                    LoggerHelper.Write($"知乎问题数据抓取 还没有添加任何监控问题: {DateTime.Now:yyyy -MM-dd HH:mm:ss}");
                }

                if (zhQList.Any())
                {
                    await _zhRepository.BulkInsertAsync(zhQList);
                }

                LoggerHelper.Write($"知乎问题数据抓取 本次抓取到{zhQList.Count()}条数据，时间:{DateTime.Now:yyyy-MM-dd HH:mm:ss}.");
            }
            catch (Exception ex)
            {
                LoggerHelper.Write(ex, $"知乎问题数据抓取 异常：ZhihuJob本次抓取异常, {DateTime.Now:yyyy -MM-dd HH:mm:ss}");
            }
        }
    }
}