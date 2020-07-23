using DG.Blog.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Modularity;

namespace DG.BLog.WeChat
{
    [DependsOn(typeof(DGBlogDomainModule))]
    public class DGBlogWeChatModule : AbpModule
    {
    }
}