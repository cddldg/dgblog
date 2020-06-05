using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using DG.Blog.ToolKits.Helper;
using System;
using Microsoft.Extensions.DependencyInjection;
using TencentCloud.Ckafka.V20190819.Models;
using Microsoft.Extensions.Configuration;

namespace DG.Blog.HttpApi.Hosting
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                LoggerHelper.Write("Starting web host.");
                await Host.CreateDefaultBuilder(args)
                      .ConfigureAppConfiguration((context, config) =>
                      {
                          config.AddJsonFile("hosting.json");
                      })
                      .ConfigureWebHostDefaults(builder =>
                      {
                          builder.UseIISIntegration()
                                 .UseStartup<Startup>();
                      }).UseAutofac().Build().RunAsync();
            }
            catch (Exception ex)
            {
                LoggerHelper.Write(ex, "Host terminated unexpectedly!");
            }
        }
    }
}