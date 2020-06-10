using DG.Blog.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace DG.Blog.ToolKits.Helper
{
    public static class ProxyHelper
    {
        public static string GetUserAgent()
        {
            return DGBlogConsts.Proxy.User_Agent_List[new Random().Next(DGBlogConsts.Proxy.User_Agent_List.Length)];
        }
    }
}