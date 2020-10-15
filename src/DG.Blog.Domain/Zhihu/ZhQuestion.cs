using DG.Blog.Domain.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace DG.Blog.Domain.Zhihu
{
    /// <summary>
    /// 问题
    /// </summary>
    public class ZhQuestion : Entity<int>
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

        /*******************/

        #region 回答信息

        /*********1**********/

        /// <summary>
        /// 1答点赞数
        /// </summary>
        public int Answer1VoteupCount { get; set; }

        /// <summary>
        /// 1答评论数
        /// </summary>
        public int Answer1CommentCount { get; set; }

        /// <summary>
        /// 1答带货数
        /// </summary>
        public int Answer1CardCount { get; set; }

        /*********2**********/

        /// <summary>
        /// 2答点赞数
        /// </summary>
        public int Answer2VoteupCount { get; set; }

        /// <summary>
        /// 2答评论数
        /// </summary>
        public int Answer2CommentCount { get; set; }

        /// <summary>
        /// 2答带货数
        /// </summary>
        public int Answer2CardCount { get; set; }

        /*********3**********/

        /// <summary>
        /// 3答点赞数
        /// </summary>
        public int Answer3VoteupCount { get; set; }

        /// <summary>
        /// 3答评论数
        /// </summary>
        public int Answer3CommentCount { get; set; }

        /// <summary>
        /// 3答带货数
        /// </summary>
        public int Answer3CardCount { get; set; }

        /*********4**********/

        /// <summary>
        /// 4答点赞数
        /// </summary>
        public int Answer4VoteupCount { get; set; }

        /// <summary>
        /// 4答评论数
        /// </summary>
        public int Answer4CommentCount { get; set; }

        /// <summary>
        /// 4答带货数
        /// </summary>
        public int Answer4CardCount { get; set; }

        /*********5**********/

        /// <summary>
        /// 5答点赞数
        /// </summary>
        public int Answer5VoteupCount { get; set; }

        /// <summary>
        /// 5答评论数
        /// </summary>
        public int Answer5CommentCount { get; set; }

        /// <summary>
        /// 5答带货数
        /// </summary>
        public int Answer5CardCount { get; set; }

        #endregion 回答信息

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