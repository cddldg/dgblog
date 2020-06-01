using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using DG.Blog.ToolKits.Helper;
using System;

namespace DG.Blog.HttpApi.Hosting
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                LoggerHelper.WriteToFile("Starting web host.");
                await Host.CreateDefaultBuilder(args)
                      .ConfigureWebHostDefaults(builder =>
                      {
                          builder.UseIISIntegration()
                                 .UseStartup<Startup>();
                      }).UseAutofac().Build().RunAsync();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteToFile("Host terminated unexpectedly!", ex);
            }
        }
    }
}