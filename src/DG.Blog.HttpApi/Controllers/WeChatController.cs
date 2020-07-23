using DG.Blog.Application.WeChat;
using DG.Blog.Domain.Configurations;
using DG.Blog.WeChat;
using DG.BLog.WeChat;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Senparc.CO2NET.HttpUtility;
using static DG.Blog.Domain.Shared.DGBlogConsts;
using Senparc.CO2NET.AspNet.HttpUtility;

namespace DG.Blog.HttpApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = Grouping.GroupName_v5)]
    public class WeChatController : AbpController
    {
        private readonly IWeChatService _weChatService;

        public WeChatController(IWeChatService weChatService)
        {
            _weChatService = weChatService;
        }

        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：https://wx.dldg.ink/wechat
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> Get(PostModel postModel, string echostr)
        {
            var result = await _weChatService.CheckSignatureAsync(postModel, echostr);
            return Content(result.Result);
        }

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Post(PostModel postModel)
        {
            var result = await _weChatService.ReceiveMessageAsync(postModel, Request.GetRequestMemoryStream());
            return result.Result;
        }
    }
}