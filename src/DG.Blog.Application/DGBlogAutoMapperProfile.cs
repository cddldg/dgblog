using AutoMapper;
using DG.Blog.Application.Contracts.Blog;
using DG.Blog.Application.Contracts.Blog.Params;
using DG.Blog.Application.Contracts.Gallery;
using DG.Blog.Application.Contracts.HotNews;
using DG.Blog.Application.Contracts.Signature;
using DG.Blog.Application.Contracts.Wallpaper;
using DG.Blog.Domain.Blog;
using DG.Blog.Domain.Gallery;

namespace DG.Blog.Application
{
    /// <summary>
    /// AutoMapper实体映射配置文件
    /// </summary>
    public class DGBlogAutoMapperProfile : Profile
    {
        public DGBlogAutoMapperProfile()
        {
            CreateMap<FriendLink, FriendLinkDto>();

            CreateMap<Post, PostForAdminDto>().ForMember(x => x.Tags, opt => opt.Ignore());

            CreateMap<EditPostInput, Post>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<EditTagInput, Tag>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<EditCategoryInput, Category>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<EditFriendLinkInput, FriendLink>().ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Domain.Signature.Signature, SignatureDto>();

            CreateMap<Domain.Wallpaper.Wallpaper, WallpaperDto>();

            CreateMap<WallpaperDto, Domain.Wallpaper.Wallpaper>().ForMember(x => x.Id, opt => opt.Ignore())
                                                                 .ForMember(x => x.Type, opt => opt.Ignore())
                                                                 .ForMember(x => x.CreateTime, opt => opt.Ignore());

            CreateMap<Domain.HotNews.HotNews, HotNewsDto>();
            CreateMap<HotNewsDto, Domain.HotNews.HotNews>().ForMember(x => x.Id, opt => opt.Ignore())
                                                           .ForMember(x => x.SourceId, opt => opt.Ignore())
                                                           .ForMember(x => x.CreateTime, opt => opt.Ignore());

            CreateMap<Album, AlbumDto>().ForMember(x => x.Count, opt => opt.Ignore());

            CreateMap<Image, ImageDto>();

        }
    }
}