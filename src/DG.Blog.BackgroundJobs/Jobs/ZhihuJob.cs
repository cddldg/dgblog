using DG.Blog.Domain.Shared.Enum;
using DG.Blog.Domain.Zhihu;
using DG.Blog.Domain.Zhihu.Repositories;
using DG.Blog.Redis;
using DG.Blog.ToolKits.Extensions;
using DG.Blog.ToolKits.Helper;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
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
                var dbQList = await _zhRepository.GetFistListAsync();
                var zhQList = new List<ZhQuestion>();
                var web = new HtmlWeb();
                if (dbQList.Any())
                {
                    foreach (var quitem in dbQList)
                    {
                        try
                        {
                            var url = $"https://www.zhihu.com/question/{quitem.QuestionId}";
                            using var client = _httpClient.CreateClient();
                            client.DefaultRequestHeaders.Add("User-Agent", ProxyHelper.GetUserAgent());

                            var httpResponse = await client.GetAsync(url);
                            var qstr = await httpResponse.Content.ReadAsStringAsync();
                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(qstr);

                            if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                            {
                                quitem.Status = QuestionStatus.NotFound;
                                await _zhRepository.UpdateAsync(quitem);
                                continue;
                            }

                            if (doc.DocumentNode.InnerHtml.IndexOf("问题已关闭") >= 0)
                            {
                                quitem.Status = QuestionStatus.Closed;
                                await _zhRepository.UpdateAsync(quitem);
                                continue;
                            }
                            var limit = 5;//回答条数
                            var offset = 0;//开始页数从0开始
                            var sortBy = "default";//：{ default, created}，表示默认排序或者时间排序
                            var answersUrl = $"https://www.zhihu.com/api/v4/questions/{quitem.QuestionId}/answers?include=data[*].is_normal,admin_closed_comment,reward_info,is_collapsed,annotation_action,annotation_detail,collapse_reason,is_sticky,collapsed_by,suggest_edit,comment_count,can_comment,content,editable_content,voteup_count,reshipment_settings,comment_permission,created_time,updated_time,review_info,relevant_info,question.detail,excerpt,relationship.is_authorized,is_author,voting,is_thanked,is_nothelp,is_labeled,is_recognized;data[*].mark_infos[*].url;data[*].author.follower_count,badge[*].topics&limit={limit}&offset={offset}&platform=desktop&sort_by={sortBy}";

                            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                            var answersDoc = await web.LoadFromWebAsync(answersUrl, Encoding.UTF8);
                            var obj = JObject.Parse(answersDoc.DocumentNode.InnerText);
                            var datas = obj["data"];
                            if (datas != null && datas.Any())
                            {
                                var qinfo = datas.FirstOrDefault();
                                quitem.Created = qinfo["question"]["created"].ToString().TryToDateTime();
                                quitem.UpdatedTime = qinfo["question"]["updated_time"].ToString().TryToDateTime();

                                for (int i = 0; i < datas.Count(); i++)
                                {
                                    var answer = datas[i];
                                    var voteup = answer["voteup_count"].TryToInt();
                                    var comment = answer["comment_count"].TryToInt();
                                    var card = ParsingCard(answer["content"].ToString());
                                    switch (i)
                                    {
                                        case 0:
                                            quitem.Answer1VoteupCount = voteup;
                                            quitem.Answer1CommentCount = comment;
                                            quitem.Answer1CardCount = card;
                                            break;

                                        case 1:
                                            quitem.Answer2VoteupCount = voteup;
                                            quitem.Answer2CommentCount = comment;
                                            quitem.Answer2CardCount = card;
                                            break;

                                        case 2:
                                            quitem.Answer3VoteupCount = voteup;
                                            quitem.Answer3CommentCount = comment;
                                            quitem.Answer3CardCount = card;
                                            break;

                                        case 3:
                                            quitem.Answer4VoteupCount = voteup;
                                            quitem.Answer4CommentCount = comment;
                                            quitem.Answer4CardCount = card;
                                            break;

                                        case 4:
                                            quitem.Answer5VoteupCount = voteup;
                                            quitem.Answer5CommentCount = comment;
                                            quitem.Answer5CardCount = card;
                                            break;
                                    }
                                }
                            }

                            var nodes = doc.DocumentNode.SelectNodes("//strong[@class='NumberBoard-itemValue']").ToList();
                            var goodNode = doc.DocumentNode.SelectNodes("//button[@class='Button GoodQuestionAction-commonBtn Button--plain Button--withIcon Button--withLabel']").FirstOrDefault();
                            var answerNode = doc.DocumentNode.SelectNodes("//h4[@class='List-headerText']").FirstOrDefault();

                            var newFollowerCount = nodes[0].GetAttributeValue("title", "0").TryToInt();
                            var newViewCount = nodes[1].GetAttributeValue("title", "0").TryToInt();
                            var newGoodQuestionCount = goodNode.InnerText.RegexNumStr().TryToInt();
                            var newAnswerTotal = answerNode.InnerText.RegexNumStr().TryToInt();
                            var newMonitorUpdateTime = DateTime.Now;

                            if (quitem.Title.IsNullOrWhiteSpace())
                            {
                                var titleNode = doc.DocumentNode.SelectNodes("//h1[@class='QuestionHeader-title']").FirstOrDefault();
                                quitem.Title = titleNode.InnerText;
                            }
                            else
                            {
                                zhQList.Add(new ZhQuestion
                                {
                                    QuestionId = quitem.QuestionId,
                                    FollowerCount = newFollowerCount,
                                    ViewCount = newViewCount,
                                    GoodQuestionCount = newGoodQuestionCount,
                                    AnswerTotal = newAnswerTotal,
                                    FistTime = false,
                                    Status = quitem.Status,

                                    FollowerDiff = newFollowerCount - quitem.FollowerCount,
                                    ViewDiff = newViewCount - quitem.ViewCount,
                                    GoodQuestionDiff = newGoodQuestionCount - quitem.GoodQuestionCount,
                                    AnswerDiff = newAnswerTotal - quitem.AnswerTotal,

                                    MonitorUpdateTime = newMonitorUpdateTime
                                });
                            }

                            quitem.FollowerCount = newFollowerCount;
                            quitem.ViewCount = newViewCount;
                            quitem.GoodQuestionCount = newGoodQuestionCount;
                            quitem.AnswerTotal = newAnswerTotal;
                            quitem.MonitorUpdateTime = newMonitorUpdateTime;
                            await _zhRepository.UpdateAsync(quitem);
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.Write(ex, $"知乎问题数据抓取 {quitem.QuestionId}  异常: {DateTime.Now:yyyy -MM-dd HH:mm:ss}");
                        }
                    }
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

        /// <summary>
        /// 解析带货数量
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private int ParsingCard(string content)
        {
            var list = StringExtensions.ZhCardRegex.Matches(content);
            if (list != null && list.Any())
                return list.Count;
            return 0;
        }
    }
}