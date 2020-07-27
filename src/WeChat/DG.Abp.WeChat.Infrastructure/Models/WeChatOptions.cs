using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace DG.Abp.WeChat.Infrastructure.Models
{
    public class WeChatOptions : ITransientDependency
    {
        /// <summary>
        /// 配置文件的根节点
        /// </summary>
        private static readonly IConfigurationRoot _config;

        /// <summary>
        /// Constructor
        /// </summary>
        static WeChatOptions()
        {
            // 加载appsettings.json，并构建IConfigurationRoot
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.json", true, true);
            _config = builder.Build();
        }

        /// <summary>
        /// 消息加密的 Token。
        /// </summary>
        public string Token => _config["WeChatOptions:Token"];

        /// <summary>
        /// 微信公众号的 AppId。
        /// </summary>
        public string AppId => _config["WeChatOptions:AppId"];

        /// <summary>
        /// 微信公众号的 API Secret。
        /// </summary>
        public string AppSecret => _config["WeChatOptions:AppSecret"];

        public string EncodingAesKey => _config["WeChatOptions:EncodingAesKey"];

        /// <summary>
        /// 微信网页授权成功后，重定向的 URL。
        /// </summary>
        public string OAuthRedirectUrl => _config["WeChatOptions:OAuthRedirectUrl"];
    }
}