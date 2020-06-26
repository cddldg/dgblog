using DG.Blog.Web.Commons;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace DG.Blog.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var config = builder.Configuration.GetSection("BlogConfig").Get<BlogConfig>();

            //if (builder.HostEnvironment.IsProduction())

            builder.Services.AddTransient(sp => new HttpClient
            {
                BaseAddress = new Uri(config.BaseAddress)
            });
            builder.Services.AddSingleton(config);
            builder.Services.AddSingleton(typeof(Common));
            //���cors ���� ���ÿ�����
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("corshost", builder =>
                {
                    builder
                    .AllowAnyOrigin() //�����κ���Դ����������
                    //.WithOrigins(_config.CorsHosts)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                    //.AllowCredentials();//ָ������cookie
                });
            });

            await builder.Build().RunAsync();
        }
    }
}