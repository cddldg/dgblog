using DG.Blog.Domain.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace DG.Blog.Application.Contracts.Zhihu
{
    public class ZhQuestionDto
    {
        public long QuestionId { get; set; }

        /// <summary>
        /// 问题标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 关注人数
        /// </summary>
        public int FollowerCount { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 好问题数
        /// </summary>
        public int GoodQuestionCount { get; set; }

        /// <summary>
        /// 回答个数
        /// </summary>
        public int AnswerTotal { get; set; }

        /*******************/

        /// <summary>
        /// 关注人数
        /// </summary>
        public int FollowerDiff { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int ViewDiff { get; set; }

        /// <summary>
        /// 好问题数
        /// </summary>
        public int GoodQuestionDiff { get; set; }

        /// <summary>
        /// 回答个数
        /// </summary>
        public int AnswerDiff { get; set; }

        /// <summary>
        /// 问题创建时间
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        /// 问题最后更新时间
        /// </summary>
        public DateTime? UpdatedTime { get; set; }

        /// <summary>
        /// 第一条数据
        /// </summary>
        public bool FistTime { get; set; }

        /// <summary>
        /// 监控订阅用户数量
        /// </summary>
        public int Subscribes { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public QuestionStatus Status { get; set; }

        /// <summary>
        /// 创建监听时间
        /// </summary>
        public DateTime? CreateMonitorTime { get; set; }

        /// <summary>
        /// 监听更新时间
        /// </summary>
        public DateTime? MonitorUpdateTime { get; set; }
    }
}