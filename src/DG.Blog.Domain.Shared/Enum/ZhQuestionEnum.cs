using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DG.Blog.Domain.Shared.Enum
{
    public enum QuestionStatus
    {
        [Description("删除")]
        Del = -1,

        [Description("正常")]
        Normal = 1,

        [Description("暂停")]
        Pause = 2,
    }
}