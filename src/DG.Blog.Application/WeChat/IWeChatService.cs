using DG.Blog.ToolKits.Base;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MP.Entities.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DG.Blog.Application.WeChat
{
    public interface IWeChatService
    {
        Task<ServiceResult<string>> CheckSignatureAsync(PostModel postModel, string echostr);

        Task<ServiceResult<ContentResult>> ReceiveMessageAsync(PostModel postModel, Stream inputStream);
    }
}