﻿using Microsoft.AspNetCore.Mvc;
using Senparc.NeuChar.MessageHandlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DG.BLog.WeChat
{
    /// <summary>
    /// 修复微信换行 bug
    /// </summary>
    public class FixWeixinBugWeixinResult : ContentResult
    {
        protected IMessageHandlerDocument _messageHandlerDocument;

        /// <summary>
        /// 这个类型只用于特殊阶段：目前IOS版本微信有换行的bug，\r\n会识别为2行
        /// </summary>
        public FixWeixinBugWeixinResult(IMessageHandlerDocument messageHandlerDocument)
        {
            _messageHandlerDocument = messageHandlerDocument;
        }

        public FixWeixinBugWeixinResult(string content)
        {
            base.Content = content;
        }

        public new string Content
        {
            get
            {
                if (base.Content != null)
                {
                    return base.Content;
                }

                if (_messageHandlerDocument != null)
                {
                    //var textResponseMessag = _messageHandlerDocument.TextResponseMessage;
                    if (_messageHandlerDocument.TextResponseMessage != null)
                    {
                        return _messageHandlerDocument.TextResponseMessage.Replace("\r\n", "\n");
                    }

                    //if (_messageHandlerDocument.TextResponseMessage.Equals(String.Empty))
                    //{
                    //    //无需响应，开发者返回了ResponseNoResponse
                    //    return null;
                    //}

                    //if (_messageHandlerDocument.ResponseDocument != null)
                    //{
                    //    //返回XML响应信息
                    //    return _messageHandlerDocument.TextResponseMessage.Replace("\r\n", "\n");
                    //}
                    //else
                    //{
                    //    //返回XML响应信息或用户指定的文本内容
                    //    return _messageHandlerDocument.TextResponseMessage;
                    //}
                }
                return null;
            }
            set { base.Content = value; }
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var content = this.Content;

            if (content == null)
            {
                //使用IMessageHandler输出
                if (_messageHandlerDocument == null)
                {
                    throw new Senparc.Weixin.Exceptions.WeixinException("执行WeixinResult时提供的MessageHandler不能为Null！", null);
                }
                var finalResponseDocument = _messageHandlerDocument.FinalResponseDocument;

                if (finalResponseDocument == null)
                {
                    //throw new Senparc.Weixin.MP.WeixinException("FinalResponseDocument不能为Null！", null);
                }
                else
                {
                    content = finalResponseDocument.ToString();
                }
            }

            context.HttpContext.Response.ContentType = "text/xml";
            content = (content ?? "").Replace("\r\n", "\n");

            var bytes = Encoding.UTF8.GetBytes(content);
            //context.HttpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            await context.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);

            // return base.ExecuteResultAsync(context);
        }
    }
}