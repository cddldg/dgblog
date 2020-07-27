using DG.Abp.WeChat.Infrastructure;
using DG.Abp.WeChat.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tencent;
using Volo.Abp.AspNetCore.Mvc;

namespace DG.Abp.WeChat.MP.HttpApi.Controllers
{
    [ControllerName("WeChat")]
    [Route("/WeChat")]
    public class WeChatController : AbpController
    {
        private readonly CheckSignature _checkSignature;
        private readonly WeChatOptions _options;

        public WeChatController(WeChatOptions options, CheckSignature checkSignature)
        {
            _checkSignature = checkSignature;
            _options = options;
        }

        [HttpGet]
        public virtual Task<string> Get(Verifying v)
        {
            if (_checkSignature.Validate(_options, v))
            {
                return Task.FromResult(v.EchoStr);
            }

            return Task.FromResult("非法参数。");
        }

        [HttpPost]
        public virtual async Task<string> Post(Verifying v)
        {
            //Request starting HTTP / 1.0 POST http://wx.dldg.ink/wechat?signature=bcf8b95a2836ba01387bbb540bddecf3b2d38a8b&timestamp=1595569623&nonce=549071656&openid=oxemPtyk8yvP418ZQlIa-YV7yqRM&encrypt_type=aes&msg_signature=1f84d72efad67ea86b634e2b2f0e36def2cb64c1 text/xml 534

            if (!_checkSignature.Validate(_options, v))
            {
                return await Task.FromResult("非法参数。");
            }
            Request.EnableBuffering();
            using var reader = new StreamReader(Request.Body, encoding: Encoding.UTF8);
            var body = await reader.ReadToEndAsync();
            var biz = new WXBizMsgCrypt(_options.Token, _options.EncodingAesKey, _options.AppId);
            var handler = new MessageHandler(biz);
            handler.ExecuteRequest(v, body);
            Console.WriteLine(body);

            return "TT";
        }
    }
}