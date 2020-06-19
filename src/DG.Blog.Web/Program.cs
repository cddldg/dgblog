using DG.Blog.Web.Commons;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DG.Blog.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            //var baseAddress = "http://localhost:9001";
            var baseAddress = "http://dldg.ink:9001";

            //if (builder.HostEnvironment.IsProduction())

            builder.Services.AddTransient(sp => new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            });

            builder.Services.AddSingleton(typeof(Common));

            await builder.Build().RunAsync();
        }
    }
}