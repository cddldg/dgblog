﻿using System;
using Volo.Abp.Domain.Entities;

namespace DG.Blog.Domain.Soul
{
    public class ChickenSoup : Entity<Guid>
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}