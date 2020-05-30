﻿using DG.Blog.Domain.Configurations;
using DG.Blog.Swagger.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;

namespace DG.Blog.Swagger
{
    public static class DGBlogSwaggerExtensions
    {
        /// <summary>
        /// 当前API版本，从appsettings.json获取
        /// </summary>
        private static readonly string version = $"v{AppSettings.ApiVersion}";

        /// <summary>
        /// Swagger描述信息
        /// </summary>
        private static readonly string description = @"<b>Blog</b>：<a target=""_blank"" href=""http://dldg.ink"">http://dldg.ink</a> <b>GitHub</b>：<a target=""_blank"" href=""https://github.com/cddldg"">https://github.com/cddldg</a> <b>Hangfire</b>：<a target=""_blank"" href=""/hangfire"">任务调度中心</a> <code>Powered by .NET Core 3.1 on Linux</code>";

        /// <summary>
        /// Swagger分组信息，将进行遍历使用
        /// </summary>
        private static readonly List<SwaggerApiInfo> ApiInfos = new List<SwaggerApiInfo>()
        {
            new SwaggerApiInfo
            {
                UrlPrefix = "v1",
                Name = "博客前台接口",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "黄师傅 - 博客前台接口",
                    Description = description
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = "v2",
                Name = "博客后台接口",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "黄师傅 - 博客后台接口",
                    Description = description
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = "v3",
                Name = "通用公共接口",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "黄师傅 - 通用公共接口",
                    Description = description
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = "v4",
                Name = "JWT授权接口",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "黄师傅 - JWT授权接口",
                    Description = description
                }
            }
        };

        /// <summary>
        /// AddSwagger
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                // 遍历并应用Swagger分组信息
                ApiInfos.ForEach(x =>
                {
                    options.SwaggerDoc(x.UrlPrefix, x.OpenApiInfo);
                });

                // API注释所需XML文件
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "DG.Blog.HttpApi.xml"));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "DG.Blog.Domain.xml"));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "DG.Blog.Application.Contracts.xml"));

                #region 小绿锁，JWT身份认证配置

                var security = new OpenApiSecurityScheme
                {
                    Description = "JWT模式授权，请输入 Bearer {Token} 进行身份验证",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };
                options.AddSecurityDefinition("oauth2", security);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement { { security, new List<string>() } });
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();

                #endregion 小绿锁，JWT身份认证配置

                // 应用Controller的API文档描述信息
                options.DocumentFilter<SwaggerDocumentFilter>();
            });
        }

        /// <summary>
        /// UseSwaggerUI
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwaggerUI(options =>
            {
                // 遍历分组信息，生成Json
                ApiInfos.ForEach(x =>
                {
                    options.SwaggerEndpoint($"/swagger/{x.UrlPrefix}/swagger.json", x.Name);
                });
                // 模型的默认扩展深度，设置为 -1 完全隐藏模型
                options.DefaultModelsExpandDepth(-1);
                // API文档仅展开标记
                options.DocExpansion(DocExpansion.List);
                // API前缀设置为空
                options.RoutePrefix = string.Empty;
                // API页面Title
                options.DocumentTitle = "接口文档 - 黄师傅";
            });
        }

        internal class SwaggerApiInfo
        {
            /// <summary>
            /// URL前缀
            /// </summary>
            public string UrlPrefix { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// <see cref="Microsoft.OpenApi.Models.OpenApiInfo"/>
            /// </summary>
            public OpenApiInfo OpenApiInfo { get; set; }
        }
    }
}