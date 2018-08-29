using AutoMapper;

namespace Instagraph.App
{
    using System.Linq;
    using DataProcessor.ExportDtos;
    using DataProcessor.ImportDtos;
    using Models;

    public class InstagraphProfile : Profile
    {
        public InstagraphProfile()
        {
            this.CreateMap<UserDto, User>()
            .ForMember(dto => dto.ProfilePicture, dest => dest.Ignore());

            this.CreateMap<PostDto, Post>()
                .ForMember(dto => dto.User, dest => dest.Ignore())
                .ForMember(dto => dto.Picture, dest => dest.Ignore());

            this.CreateMap<CommentDto, Comment>()
                .ForMember(dto => dto.User, dest => dest.Ignore())
                .ForMember(dto => dto.PostId, dest => dest.MapFrom(c => c.PostElement.PostId));

            this.CreateMap<Post, UncommentedPostDto>()
                .ForMember(dto => dto.User, dest => dest.MapFrom(p => p.User.Username))
                .ForMember(dto => dto.Picture, dest => dest.MapFrom(p => p.Picture.Path));

            this.CreateMap<User, PopularUserDto>()
                .ForMember(dto => dto.User, dest => dest.MapFrom(u => u.Username))
                .ForMember(dto => dto.Followers, dest => dest.MapFrom(u => u.Followers.Count));

            this.CreateMap<User, UserDataDto>()
                .ForMember(dto => dto.MostComments, dest => dest.MapFrom(u => u.Posts.Select(p => p.Comments.Count).OrderByDescending(p => p).FirstOrDefault()));
        }
    }
}