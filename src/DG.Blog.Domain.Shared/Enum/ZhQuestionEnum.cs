using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DG.Blog.Domain.Shared.Enum
{
    public enum QuestionStatus
    {
        /// <summary>
        /// 知乎已关闭问题
        /// </summary>
        [Description("关闭")]
        Closed = 0,

        [Description("正常")]
        Normal = 1,

        [Description("暂停")]
        Pause = 2,

        [Description("错误")]
        NotFound = 404,
    }
}