using DG.Abp.WeChat.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Modularity;

namespace DG.Abp.WeChat.MP.HttpApi
{
    [DependsOn(typeof(DGAbpWeChatInfrastructureModule))]
    public class DGAbpWeChatHttpApiModule : AbpModule
    {
    }
}