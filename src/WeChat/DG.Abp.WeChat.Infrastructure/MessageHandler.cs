using DG.Abp.WeChat.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Tencent;
using Volo.Abp.DependencyInjection;

namespace DG.Abp.WeChat.Infrastructure
{
    public class MessageHandler
    {
        private readonly WXBizMsgCrypt _wXBiz;

        public MessageHandler(WXBizMsgCrypt wXBiz)
        {
            _wXBiz = wXBiz;
        }

        public void ExecuteRequest(Verifying v, string body)
        {
            string sMsg = "";
            int ret = _wXBiz.DecryptMsg(v.Msg_Signature, v.Timestamp, v.Nonce, body, ref sMsg);
            if (ret != 0)
            {
                System.Console.WriteLine("ERR: Decrypt fail, ret: " + ret);
                return;
            }
            System.Console.WriteLine(sMsg);
        }
    }
}