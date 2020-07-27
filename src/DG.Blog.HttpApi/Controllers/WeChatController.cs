using DG.Blog.Application.Contracts.Wallpaper;
using DG.Blog.Application.Contracts.Wallpaper.Params;
using DG.Blog.Application.Wallpaper;
using DG.Blog.Domain.Configurations;
using DG.Blog.ToolKits.Base;
using DG.Blog.ToolKits.Helper;
using DG.Blog.WeChat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senparc.NeuChar;
using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageContexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp.AspNetCore.Mvc;
using static DG.Blog.Domain.Shared.DGBlogConsts;

namespace DG.Blog.HttpApi.Controllers
{
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = Grouping.GroupName_v5)]
    public class WeChatController : AbpController
    {
        [HttpGet]
        public virtual Task<string> Get(PostModel postModel, string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, AppSettings.WeChatOptions.Token))
            {
                return Task.FromResult(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Task.FromResult("非法参数。");
            }
        }

        //[HttpPost]
        //public virtual async Task<ActionResult> Post(PostModel postModel)
        //{
        //    if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, AppSettings.WeChatOptions.Token))
        //    {
        //        return new WeixinResult("参数错误！");
        //    }

        //    postModel.Token = AppSettings.WeChatOptions.Token;
        //    postModel.EncodingAESKey = AppSettings.WeChatOptions.EncodingAesKey; //根据自己后台的设置保持一致
        //    postModel.AppId = AppSettings.WeChatOptions.AppId; //根据自己后台的设置保持一致

        //    var cancellationToken = new CancellationToken();//给异步方法使用
        //    Request.EnableBuffering();
        //    using var reader = new StreamReader(Request.Body, encoding: Encoding.UTF8);

        //    var body = await reader.ReadToEndAsync();
        //    var agentDoc = XDocument.Parse(body);
        //    Request.Body.Position = 0;

        //    var messageHandler = new CustomMessageHandler(agentDoc, postModel, 10);

        //    #region 设置消息去重设置

        //    // messageHandler.OmitRepeatedMessage = true;//默认已经是开启状态，此处仅作为演示，也可以设置为 false 在本次请求中停用此功能

        //    #endregion 设置消息去重设置

        //    try
        //    {
        //        //messageHandler.SaveRequestMessageLog();//记录 Request 日志（可选）
        //        await messageHandler.ExecuteAsync(cancellationToken); //执行微信处理过程（关键）
        //        //messageHandler.SaveResponseMessageLog();//记录 Response 日志（可选）
        //        return new FixWeixinBugWeixinResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
        //    }
        //    catch (Exception ex)
        //    {
        //        #region 异常处理

        //        //WeixinTrace.Log("MessageHandler错误：{0}", ex.Message);
        //        LoggerHelper.Write(ex, $"MessageHandler错误：{ex.Message }");
        //        return Content("");

        //        #endregion 异常处理
        //    }
        //}

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML
        /// PS：此方法为常规switch判断方法，从v0.3.3版本起，此Demo不再更新
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> OldPostAsync(string signature, string timestamp, string nonce, string echostr)
        {
            LocationService locationService = new LocationService();
            EventService eventService = new EventService();

            if (!CheckSignature.Check(signature, timestamp, nonce, AppSettings.WeChatOptions.Token))
            {
                return Content("参数错误！");
            }
            XDocument requestDoc = null;
            try
            {
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body, encoding: Encoding.UTF8);
                var body = await reader.ReadToEndAsync();
                //var cancellationToken = new CancellationToken();//给异步方法使用
                requestDoc = XDocument.Parse(body);
                LoggerHelper.Write($"OldPost 收到消息：{requestDoc}");
                var requestMessage = RequestMessageFactory.GetRequestEntity(new DefaultMpMessageContext(), requestDoc);

                //var requestMessage = RequestMessageFactory.GetRequestEntity(Request.InputStream);

                ResponseMessageBase responseMessage = null;
                switch (requestMessage.MsgType)
                {
                    case RequestMsgType.Text://文字
                        {
                            //TODO:交给Service处理具体信息，参考/Service/EventSercice.cs 及 /Service/LocationSercice.cs
                            var strongRequestMessage = requestMessage as RequestMessageText;
                            var strongresponseMessage =
                                ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
                            strongresponseMessage.Content = $"文字消息:{strongRequestMessage.Content}";
                            responseMessage = strongresponseMessage;
                            break;
                        }
                    case RequestMsgType.Location://位置
                        {
                            responseMessage = locationService.GetResponseMessage(requestMessage as RequestMessageLocation);
                            break;
                        }
                    case RequestMsgType.Image://图片
                        {
                            //TODO:交给Service处理具体信息
                            var strongRequestMessage = requestMessage as RequestMessageImage;
                            var strongresponseMessage =
                                ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
                            strongresponseMessage.Articles.Add(new Article()
                            {
                                Title = "您刚才发送了图片信息",
                                Description = "您发送的图片将会显示在边上",
                                PicUrl = strongRequestMessage.PicUrl,
                                Url = "https://dldg.ink/"
                            });
                            strongresponseMessage.Articles.Add(new Article()
                            {
                                Title = "第二条",
                                Description = "第二条带连接的内容",
                                PicUrl = strongRequestMessage.PicUrl,
                                Url = "https://dldg.ink/"
                            });
                            responseMessage = strongresponseMessage;
                            break;
                        }
                    case RequestMsgType.Voice://语音
                        {
                            //TODO:交给Service处理具体信息
                            var strongRequestMessage = requestMessage as RequestMessageVoice;
                            var strongresponseMessage =
                               ResponseMessageBase.CreateFromRequestMessage<ResponseMessageMusic>(requestMessage);

                            strongresponseMessage.Music.MusicUrl = "https://s-bj-1636-dldg.oss.dogecdn.com/glgs-zdsjjt.mp3";
                            responseMessage = strongresponseMessage;
                            break;
                        }
                    case RequestMsgType.Event://事件
                        {
                            responseMessage = eventService.GetResponseMessage(requestMessage as RequestMessageEventBase);
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                var responseDoc = Senparc.NeuChar.Helpers.EntityHelper.ConvertEntityToXml(responseMessage);
                LoggerHelper.Write($"OldPost 发出消息：{responseDoc}");
                return Content(responseDoc.ToString());
                //如果不需要记录responseDoc，只需要：
                //return Content(responseMessage.ConvertEntityToXmlString());
            }
            catch (Exception ex)
            {
                LoggerHelper.Write(ex, $"OldPost 异常{ex.Message}");
                return Content("");
            }
        }

        //[HttpPost]
        //public ActionResult OriginalPost(string signature, string timestamp, string nonce, string echostr)
        //{
        //    //消息安全验证代码开始
        //    //...
        //    //消息安全验证代码结束

        //    string requestXmlString = null;//请求XML字符串
        //    using (var sr = new StreamReader(HttpContext.Request.Body))
        //    {
        //        requestXmlString = sr.ReadToEnd();
        //    }

        //    //XML消息格式正确性验证代码开始
        //    //...
        //    //XML消息格式正确性验证代码结束

        //    /* XML消息范例
        //    <xml>
        //        <ToUserName><![CDATA[gh_a96a4a619366]]></ToUserName>
        //        <FromUserName><![CDATA[olPjZjsXuQPJoV0HlruZkNzKc91E]]></FromUserName>
        //        <CreateTime>{{0}}</CreateTime>
        //        <MsgType><![CDATA[text]]></MsgType>
        //        <Content><![CDATA[{0}]]></Content>
        //        <MsgId>5832509444155992350</MsgId>
        //    </xml>
        //    */

        //    XDocument xmlDocument = XDocument.Parse(requestXmlString);//XML消息对象
        //    var xmlRoot = xmlDocument.Root;
        //    var msgType = xmlRoot.Element("MsgType").Value;//消息类型
        //    var toUserName = xmlRoot.Element("ToUserName").Value;
        //    var fromUserName = xmlRoot.Element("FromUserName").Value;
        //    var createTime = xmlRoot.Element("CreateTime").Value;
        //    var msgId = xmlRoot.Element("MsgId").Value;

        //    //根据MsgId去重开始
        //    //...
        //    //根据MsgId去重结束

        //    string responseXml = null;//响应消息XML
        //    var responseTime = (SystemTime.Now.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000 - 8 * 60 * 60;

        //    switch (msgType)
        //    {
        //        case "text":
        //            //处理文本消息
        //            var content = xmlRoot.Element("Content").Value;//文本内容
        //            if (content == "你好")
        //            {
        //                var title = "Title";
        //                var description = "Description";
        //                var picUrl = "PicUrl";
        //                var url = "Url";
        //                var articleCount = 1;
        //                responseXml = @"<xml>
        //                <ToUserName><![CDATA[" + fromUserName + @"]]></ToUserName>
        //                <FromUserName><![CDATA[" + toUserName + @"]]></FromUserName>
        //                <CreateTime>" + responseTime + @"</CreateTime>
        //                <MsgType><![CDATA[news]]></MsgType>
        //                <ArticleCount>" + articleCount + @"</ArticleCount>
        //                <Articles>
        //                <item>
        //                <Title><![CDATA[" + title + @"]]></Title>
        //                <Description><![CDATA[" + description + @"]]></Description>
        //                <PicUrl><![CDATA[" + picUrl + @"]]></PicUrl>
        //                <Url><![CDATA[" + url + @"]]></Url>
        //                </item>
        //                </Articles>
        //                </xml> ";
        //            }
        //            else if (content == "Senparc")
        //            {
        //                //相似处理逻辑
        //            }
        //            else
        //            {
        //                //...
        //            }
        //            break;

        //        case "image":
        //            //处理图片消息
        //            //...
        //            break;

        //        case "event":
        //            //这里会有更加复杂的事件类型处理
        //            //...
        //            break;
        //        //更多其他请求消息类型的判断...
        //        default:
        //            //其他未知类型
        //            break;
        //    }

        //    return Content(responseXml, "text/xml");//返回结果
        //}
    }
}