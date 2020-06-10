using DG.Blog.BackgroundJobs;
using DG.Blog.Domain.Configurations;
using DG.Blog.EntityFrameworkCore;
using DG.Blog.HttpApi.Hosting.Middleware;
using DG.Blog.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace DG.Blog.HttpApi.Hosting
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule),
        typeof(DGBlogHttpApiModule),
        typeof(DGBlogSwaggerModule),
        typeof(DGBlogFrameworkCoreModule),
        typeof(DGBlogBackgroundJobsModule)
        )]
    public class DGBlogHttpApiHostingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 路由配置
            context.Services.AddRouting(options =>
            {
                // 设置URL为小写
                options.LowercaseUrls = true;
                // 在生成的URL后面添加斜杠
                options.AppendTrailingSlash = true;
            });
            //添加cors 服务 配置跨域处理
            context.Services.AddCors(options =>
            {
                options.AddPolicy("corshost", builder =>
                {
                    builder
                    .AllowAnyOrigin() //允许任何来源的主机访问
                    //.WithOrigins(_config.CorsHosts)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                    //.AllowCredentials();//指定处理cookie
                });
            });
            // 身份验证之JWT
            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           // 是否验证颁发者
                           ValidateIssuer = true,
                           // 是否验证访问群体
                           ValidateAudience = true,
                           // 是否验证生存期
                           ValidateLifetime = true,
                           // 验证Token的时间偏移量
                           ClockSkew = TimeSpan.FromSeconds(30),
                           // 是否验证安全密钥
                           ValidateIssuerSigningKey = true,
                           // 访问群体
                           ValidAudience = AppSettings.JWT.Domain,
                           // 颁发者
                           ValidIssuer = AppSettings.JWT.Domain,
                           // 安全密钥
                           IssuerSigningKey = new SymmetricSecurityKey(AppSettings.JWT.SecurityKey.GetBytes())
                       };
                   });

            // 认证授权
            context.Services.AddAuthorization();

            // Http请求
            context.Services.AddHttpClient();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            // 环境变量，开发环境
            if (env.IsDevelopment())
            {
                // 生成异常页面
                app.UseDeveloperExceptionPage();
            }

            // 使用HSTS的中间件，该中间件添加了严格传输安全头
            app.UseHsts();

            // 转发将标头代理到当前请求，配合 Nginx 使用，获取用户真实IP
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // 路由
            app.UseRouting();

            // 跨域
            app.UseCors("corshost");
            // 异常处理中间件
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            // 身份验证
            app.UseAuthentication();

            // 认证授权
            app.UseAuthorization();

            // HTTP => HTTPS
            app.UseHttpsRedirection();

            // 路由映射
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule),

        typeof(DGBlogBackgroundJobsModule)
        )]
    public class DGBlogHttpApiHostingModule2 : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 路由配置
            context.Services.AddRouting(options =>
            {
                // 设置URL为小写
                options.LowercaseUrls = true;
                // 在生成的URL后面添加斜杠
                options.AppendTrailingSlash = true;
            });
            //添加cors 服务 配置跨域处理
            context.Services.AddCors(options =>
            {
                options.AddPolicy("corshost", builder =>
                {
                    builder
                    .AllowAnyOrigin() //允许任何来源的主机访问
                    //.WithOrigins(_config.CorsHosts)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                    //.AllowCredentials();//指定处理cookie
                });
            });
            // 身份验证之JWT
            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           // 是否验证颁发者
                           ValidateIssuer = true,
                           // 是否验证访问群体
                           ValidateAudience = true,
                           // 是否验证生存期
                           ValidateLifetime = true,
                           // 验证Token的时间偏移量
                           ClockSkew = TimeSpan.FromSeconds(30),
                           // 是否验证安全密钥
                           ValidateIssuerSigningKey = true,
                           // 访问群体
                           ValidAudience = AppSettings.JWT.Domain,
                           // 颁发者
                           ValidIssuer = AppSettings.JWT.Domain,
                           // 安全密钥
                           IssuerSigningKey = new SymmetricSecurityKey(AppSettings.JWT.SecurityKey.GetBytes())
                       };
                   });

            // 认证授权
            context.Services.AddAuthorization();

            // Http请求
            context.Services.AddHttpClient();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            // 环境变量，开发环境
            if (env.IsDevelopment())
            {
                // 生成异常页面
                app.UseDeveloperExceptionPage();
            }

            // 使用HSTS的中间件，该中间件添加了严格传输安全头
            app.UseHsts();

            // 转发将标头代理到当前请求，配合 Nginx 使用，获取用户真实IP
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // 路由
            app.UseRouting();

            // 跨域
            app.UseCors("corshost");
            // 异常处理中间件
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            // 身份验证
            app.UseAuthentication();

            // 认证授权
            app.UseAuthorization();

            // HTTP => HTTPS
            app.UseHttpsRedirection();

            // 路由映射
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}