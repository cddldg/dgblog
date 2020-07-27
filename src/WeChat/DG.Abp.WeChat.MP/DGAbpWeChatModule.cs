using DG.Abp.WeChat.Infrastructure.Models;
using DG.Abp.WeChat.MP.HttpApi;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Modularity;

namespace DG.Abp.WeChat.MP
{
    [DependsOn(typeof(DGAbpWeChatHttpApiModule))]
    public class DGAbpWeChatModule : AbpModule
    {
    }
}