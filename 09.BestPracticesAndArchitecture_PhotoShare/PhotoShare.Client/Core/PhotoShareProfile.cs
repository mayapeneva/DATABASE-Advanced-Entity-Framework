namespace PhotoShare.Client.Core
{
    using AutoMapper;
    using Dtos;
    using Models;

    public class PhotoShareProfile : Profile
    {
        public PhotoShareProfile()
        {
            this.CreateMap<User, UserDto>().ReverseMap();

            this.CreateMap<Town, TownDto>().ReverseMap();

            this.CreateMap<Album, AlbumDto>().ReverseMap();

            this.CreateMap<AlbumRole, AlbumRoleDto>();

            this.CreateMap<Tag, TagDto>().ReverseMap();

            this.CreateMap<User, UserFriendsDto>()
                .ForMember(dto => dto.Friends,
                    opt => opt.MapFrom(u => u.FriendsAdded));

            this.CreateMap<Friendship, FriendDto>()
                .ForMember(dto => dto.Username,
                    opt => opt.MapFrom(f => f.Friend.Username));

            this.CreateMap<User, User>();
            this.CreateMap<Town, Town>();
            this.CreateMap<Album, Album>();
            this.CreateMap<Tag, Tag>();
            this.CreateMap<AlbumRole, AlbumRole>();
            this.CreateMap<Friendship, Friendship>();
        }
    }
}