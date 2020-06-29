﻿using DG.Blog.Application.Contracts;
using DG.Blog.Application.Contracts.Blog;
using DG.Blog.Application.Contracts.Blog.Params;
using DG.Blog.Domain.Blog;
using DG.Blog.ToolKits.Base;
using DG.Blog.ToolKits.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DG.Blog.Domain.Shared.DGBlogConsts;

namespace DG.Blog.Application.Blog.Impl
{
    public partial class BlogService
    {
        /// <summary>
        /// 获取文章详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<PostForAdminDto>> GetPostForAdminAsync(int id)
        {
            var result = new ServiceResult<PostForAdminDto>();

            var post = await _postRepository.GetAsync(id);

            var tags = from post_tags in await _postTagRepository.GetListAsync()
                       join tag in await _tagRepository.GetListAsync()
                       on post_tags.TagId equals tag.Id
                       where post_tags.PostId.Equals(post.Id)
                       select tag.TagName;

            var detail = ObjectMapper.Map<Post, PostForAdminDto>(post);
            detail.Tags = tags;
            detail.Url = post.Url.Split("/").Where(x => !string.IsNullOrEmpty(x)).Last();

            result.IsSuccess(detail);
            return result;
        }

        /// <summary>
        /// 分页查询文章列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult<PagedList<QueryPostForAdminDto>>> QueryPostsForAdminAsync(PagingInput input)
        {
            var result = new ServiceResult<PagedList<QueryPostForAdminDto>>();

            var count = await _postRepository.GetCountAsync();

            var list = _postRepository.OrderByDescending(x => x.CreationTime)
                                      .PageByIndex(input.Page, input.Limit)
                                      .Select(x => new PostBriefForAdminDto
                                      {
                                          Id = x.Id,
                                          Title = x.Title,
                                          Url = x.Url,
                                          Year = x.CreationTime.Year,
                                          CreationTime = x.CreationTime.TryToDateTime()
                                      })
                                      .GroupBy(x => x.Year)
                                      .Select(x => new QueryPostForAdminDto
                                      {
                                          Year = x.Key,
                                          Posts = x.ToList()
                                      }).ToList();

            result.IsSuccess(new PagedList<QueryPostForAdminDto>(count.TryToInt(), list));
            return result;
        }

        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult> InsertPostAsync(EditPostInput input)
        {
            var result = new ServiceResult();

            var post = ObjectMapper.Map<EditPostInput, Post>(input);
            post.Url = $"{post.CreationTime.ToString(" yyyy MM dd ").Replace(" ", "/")}{post.Url}/";
            await _postRepository.InsertAsync(post);

            var tags = await _tagRepository.GetListAsync();

            var newTags = input.Tags
                               .Where(item => !tags.Any(x => x.TagName.Equals(item)))
                               .Select(item => new Tag
                               {
                                   TagName = item,
                                   DisplayName = item
                               });
            await _tagRepository.BulkInsertAsync(newTags);

            var postTags = input.Tags.Select(item => new PostTag
            {
                PostId = post.Id,
                TagId = _tagRepository.FirstOrDefault(x => x.TagName == item).Id
            });
            await _postTagRepository.BulkInsertAsync(postTags);

            result.IsSuccess(ResponseText.INSERT_SUCCESS);
            return result;
        }

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult> UpdatePostAsync(int id, EditPostInput input)
        {
            var result = new ServiceResult();

            var post = await _postRepository.GetAsync(id);
            post.Title = input.Title;
            post.Author = input.Author;
            post.Url = $"{input.CreationTime.ToString(" yyyy MM dd ").Replace(" ", "/")}{input.Url}/";
            post.Html = input.Html;
            post.Markdown = input.Markdown;
            post.CreationTime = input.CreationTime;
            post.CategoryId = input.CategoryId;

            await _postRepository.UpdateAsync(post);

            var tags = await _tagRepository.GetListAsync();

            var oldPostTags = from post_tags in await _postTagRepository.GetListAsync()
                              join tag in await _tagRepository.GetListAsync()
                              on post_tags.TagId equals tag.Id
                              where post_tags.PostId.Equals(post.Id)
                              select new
                              {
                                  post_tags.Id,
                                  tag.TagName
                              };

            var removedIds = oldPostTags.Where(item => !input.Tags.Any(x => x == item.TagName) &&
                                                       tags.Any(t => t.TagName == item.TagName))
                                        .Select(item => item.Id);
            await _postTagRepository.DeleteAsync(x => removedIds.Contains(x.Id));

            var newTags = input.Tags
                               .Where(item => !tags.Any(x => x.TagName == item))
                               .Select(item => new Tag
                               {
                                   TagName = item,
                                   DisplayName = item
                               });
            await _tagRepository.BulkInsertAsync(newTags);

            var postTags = input.Tags
                                .Where(item => !oldPostTags.Any(x => x.TagName == item))
                                .Select(item => new PostTag
                                {
                                    PostId = id,
                                    TagId = _tagRepository.FirstOrDefault(x => x.TagName == item).Id
                                });
            await _postTagRepository.BulkInsertAsync(postTags);

            result.IsSuccess(ResponseText.UPDATE_SUCCESS);
            return result;
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult> DeletePostAsync(int id)
        {
            var result = new ServiceResult();

            var post = await _postRepository.GetAsync(id);
            if (null == post)
            {
                result.IsFailed(ResponseText.WHAT_NOT_EXIST.FormatWith("Id", id));
                return result;
            }

            await _postRepository.DeleteAsync(id);
            await _postTagRepository.DeleteAsync(x => x.PostId == id);

            result.IsSuccess(ResponseText.DELETE_SUCCESS);
            return result;
        }

        /// <summary>
        /// 查询标签列表
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<QueryTagForAdminDto>>> QueryTagsForAdminAsync()
        {
            var result = new ServiceResult<IEnumerable<QueryTagForAdminDto>>();

            var post_tags = await _postTagRepository.GetListAsync();

            var tags = _tagRepository.GetListAsync().Result.Select(x => new QueryTagForAdminDto
            {
                Id = x.Id,
                TagName = x.TagName,
                DisplayName = x.DisplayName,
                Count = post_tags.Count(p => p.TagId == x.Id)
            });

            result.IsSuccess(tags);
            return result;
        }

        /// <summary>
        /// 新增标签
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ServiceResult> InsertTagAsync(EditTagInput input)
        {
            var result = new ServiceResult();

            var tag = ObjectMapper.Map<EditTagInput, Tag>(input);
            await _tagRepository.InsertAsync(tag);

            result.IsSuccess(ResponseText.INSERT_SUCCESS);
            return result;
        }

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ServiceResult> UpdateTagAsync(int id, EditTagInput input)
        {
            var result = new ServiceResult();

            var tag = await _tagRepository.GetAsync(id);
            tag.TagName = input.TagName;
            tag.DisplayName = input.DisplayName;

            await _tagRepository.UpdateAsync(tag);

            result.IsSuccess(ResponseText.UPDATE_SUCCESS);
            return result;
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult> DeleteTagAsync(int id)
        {
            var result = new ServiceResult();

            var tag = await _tagRepository.GetAsync(id);
            if (null == tag)
            {
                result.IsFailed(ResponseText.WHAT_NOT_EXIST.FormatWith("Id", id));
                return result;
            }

            await _tagRepository.DeleteAsync(id);
            await _postTagRepository.DeleteAsync(x => x.TagId == id);

            result.IsSuccess(ResponseText.DELETE_SUCCESS);
            return result;
        }

        /// <summary>
        /// 查询分类列表
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<QueryCategoryForAdminDto>>> QueryCategoriesForAdminAsync()
        {
            var result = new ServiceResult<IEnumerable<QueryCategoryForAdminDto>>();

            var posts = await _postRepository.GetListAsync();

            var categories = _categoryRepository.GetListAsync().Result.Select(x => new QueryCategoryForAdminDto
            {
                Id = x.Id,
                CategoryName = x.CategoryName,
                DisplayName = x.DisplayName,
                Count = posts.Count(p => p.CategoryId == x.Id)
            });

            result.IsSuccess(categories);
            return result;
        }

        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult> InsertCategoryAsync(EditCategoryInput input)
        {
            var result = new ServiceResult();

            var category = ObjectMapper.Map<EditCategoryInput, Category>(input);
            await _categoryRepository.InsertAsync(category);

            result.IsSuccess(ResponseText.INSERT_SUCCESS);
            return result;
        }

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult> UpdateCategoryAsync(int id, EditCategoryInput input)
        {
            var result = new ServiceResult();

            var category = await _categoryRepository.GetAsync(id);
            category.CategoryName = input.CategoryName;
            category.DisplayName = input.DisplayName;

            await _categoryRepository.UpdateAsync(category);

            result.IsSuccess(ResponseText.UPDATE_SUCCESS);
            return result;
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult> DeleteCategoryAsync(int id)
        {
            var result = new ServiceResult();

            var category = await _categoryRepository.GetAsync(id);
            if (null == category)
            {
                result.IsFailed(ResponseText.WHAT_NOT_EXIST.FormatWith("Id", id));
                return result;
            }

            await _categoryRepository.DeleteAsync(id);

            result.IsSuccess(ResponseText.DELETE_SUCCESS);
            return result;
        }

        /// <summary>
        /// 新增友链
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult> InsertFriendLinkAsync(EditFriendLinkInput input)
        {
            var result = new ServiceResult();

            var friendLink = ObjectMapper.Map<EditFriendLinkInput, FriendLink>(input);
            await _friendLinksRepository.InsertAsync(friendLink);

            result.IsSuccess(ResponseText.INSERT_SUCCESS);
            return result;
        }

        /// <summary>
        /// 删除友链
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult> DeleteFriendLinkAsync(int id)
        {
            var result = new ServiceResult();

            var category = await _friendLinksRepository.GetAsync(id);
            if (null == category)
            {
                result.IsFailed(ResponseText.WHAT_NOT_EXIST.FormatWith("Id", id));
                return result;
            }

            await _friendLinksRepository.DeleteAsync(id);

            result.IsSuccess(ResponseText.DELETE_SUCCESS);
            return result;
        }

        /// <summary>
        /// 更新友链
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult> UpdateFriendLinkAsync(int id, EditFriendLinkInput input)
        {
            var result = new ServiceResult();

            var category = await _friendLinksRepository.GetAsync(id);
            category.Title = input.Title;
            category.LinkUrl = input.LinkUrl;

            await _friendLinksRepository.UpdateAsync(category);

            result.IsSuccess(ResponseText.UPDATE_SUCCESS);
            return result;
        }
    }
}