using DG.Blog.Redis;
using DG.Blog.ToolKits.Extensions;
using DG.Blog.ToolKits.Helper;
using HtmlAgilityPack;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using static DG.Blog.Domain.Shared.DGBlogConsts;

namespace DG.Blog.BackgroundJobs.Jobs
{
    public class ProxyTestJob : ITransientDependency
    {
        private readonly DGBlogRedisContext _redis;

        public ProxyTestJob(DGBlogRedisContext redisContext)
        {
            _redis = redisContext;
        }

        /// <summary>
        /// 检测代理数据
        /// </summary>
        /// <returns></returns>
        public async Task RunAsync()
        {
            try
            {
                var tstr = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                LoggerHelper.Write($"检测代理数据 {tstr}");

                var count = await _redis.ZCountAsync();

                for (long i = 0; i < count; i += Proxy.BATCH_TEST_SIZE)
                {
                    var start = i;
                    var stop = new long[] { i + Proxy.BATCH_TEST_SIZE, count }.Min();
                    LoggerHelper.Write($"{tstr} 正在测试第 {start + 1}-{stop} 个代理,共 {count} 个代理");
                    var batch = await _redis.ZBatchGet(start, stop);
                    await TestOneAsync(batch);
                    Thread.Sleep(1000);
                }

                LoggerHelper.Write($"检测代理数据 本次检测{count}条数据，时间:{DateTime.Now:yyyy-MM-dd HH:mm:ss}.");
            }
            catch (Exception ex)
            {
                LoggerHelper.Write(ex, $"检测代理数据 异常: {DateTime.Now:yyyy -MM-dd HH:mm:ss}");
            }
        }

        private async Task TestOneAsync(string[] batch)
        {
            foreach (var item in batch)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Proxy.TEST_URL);
                    WebProxy wp = new WebProxy(item);
                    request.Proxy = wp;
                    request.Timeout = 3000;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (((int)response.StatusCode) == 200 || ((int)response.StatusCode) == 302)
                    {
                        await _redis.ZMaxAsync(item);
                    }
                    else
                    {
                        await _redis.ZDecreaseAsync(item);
                    }
                }
                catch (Exception)
                {
                    await _redis.ZDecreaseAsync(item);
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
                    Subject = "【定时任务】检测代理数据任务推送",
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