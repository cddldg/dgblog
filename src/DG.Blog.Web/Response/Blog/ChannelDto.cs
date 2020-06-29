﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DG.Blog.Web.Response.Blog
{
    public class ChannelDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 介绍
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// Banner
        /// </summary>
        public string Banner { get; set; }

        /// <summary>
        /// Cover
        /// </summary>
        public string Cover { get; set; }
    }
}