using DG.Blog.Domain.Configurations;
using DG.Blog.ToolKits.Base;
using DG.Blog.WeChat;
using DG.BLog.WeChat;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DG.Blog.Application.WeChat.Impl
{
    public class WeChatService : ServiceBase, IWeChatService
    {
        /// <summary>
        /// 与微信公众账号后台的Token设置保持一致，区分大小写。
        /// </summary>
        public static readonly string Token = AppSettings.WeiXin.Token;

        /// <summary>
        /// 与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
        /// </summary>
        public static readonly string EncodingAESKey = AppSettings.WeiXin.EncodingAESKey;

        /// <summary>
        /// 与微信公众账号后台的AppId设置保持一致，区分大小写。
        /// </summary>
        public static readonly string AppId = AppSettings.WeiXin.AppId;

        public async Task<ServiceResult<string>> CheckSignatureAsync(PostModel postModel, string echostr)
        {
            var result = new ServiceResult<string>();
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                result.IsSuccess(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                result.IsFailed("非法参数。");
            }

            return await Task.FromResult(result);
        }

        public async Task<ServiceResult<ContentResult>> ReceiveMessageAsync(PostModel postModel, Stream inputStream)
        {
            var result = new ServiceResult<ContentResult> { Result = new ContentResult() };

            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                result.IsFailed("非法参数。");
                result.Result.Content = "非法参数。";
                return result;
            }

            #region 打包 PostModel 信息

            postModel.Token = Token;
            postModel.EncodingAESKey = EncodingAESKey;
            postModel.AppId = AppId;

            #endregion 打包 PostModel 信息

            //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制（实际最大限制 99999）
            //注意：如果使用分布式缓存，不建议此值设置过大，如果需要储存历史信息，请使用数据库储存
            var maxRecordCount = 10;

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new CustomMessageHandler(inputStream, postModel, maxRecordCount);

            #region 设置消息去重设置

            messageHandler.OmitRepeatedMessage = true;//默认已经是开启状态，此处仅作为演示，也可以设置为 false 在本次请求中停用此功能

            #endregion 设置消息去重设置

            try
            {
                var cancellationToken = new CancellationToken();//给异步方法使用
                await messageHandler.ExecuteAsync(cancellationToken);
                result.Result = new FixWeixinBugWeixinResult(messageHandler);
            }
            catch (Exception)
            {
                result.IsFailed("异常");
                result.Result.Content = "异常";
            }

            return await Task.FromResult(result);
        }
    }
}