namespace PhotoShare.Client.Core.Commands
{
    using Constants;
    using Contracts;
    using Dtos;
    using Models.Enums;
    using Services.Contracts;
    using System;
    using System.Linq;
    using Utilities;

    public class AddTagToCommand : ICommand
    {
        private const string CommandName = "AddTagTo";

        private readonly IAlbumTagService albumTagService;
        private readonly IAlbumService albumService;
        private readonly IAlbumRoleService albumRoleService;
        private readonly ITagService tagService;
        private readonly IUserService userService;

        public AddTagToCommand(IAlbumTagService albumTagService, IAlbumService albumService, IAlbumRoleService albumRoleService, ITagService tagService, IUserService userService)
        {
            this.albumTagService = albumTagService;
            this.albumService = albumService;
            this.albumRoleService = albumRoleService;
            this.tagService = tagService;
            this.userService = userService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 2)
            {
                throw new InvalidOperationException(string.Format(Messages.InvalidCommand, CommandName));
            }

            var albumName = data[0];
            var tagName = data[1];
            tagName = tagName.ValidateOrTransform();

            if (!this.albumService.Exists(albumName) || !this.tagService.Exists(tagName))
            {
                throw new ArgumentException(string.Format(Messages.AlbumOrTagDoesNotExists));
            }

            var userId = this.userService.GetLoggedInUserId();
            var album = this.albumService.ByName<AlbumDto>(albumName);
            var albumRoles = this.albumRoleService.ByAlbumId<AlbumRoleDto>(album.Id);
            var albumRole = albumRoles.Where(ar => ar.UserId == userId && ar.Role == Role.Owner);
            if (userId == 0 || !albumRole.Any())
            {
                throw new InvalidOperationException(Messages.InvalidCredentials);
            }

            var tag = this.tagService.ByName<TagDto>(tagName);
            this.albumTagService.AddTagTo(album.Id, tag.Id);

            return string.Format(Messages.SuccesсfullTagAddingTo, tagName, albumName);
        }
    }
}