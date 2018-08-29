namespace PhotoShare.Client.Core.Commands
{
    using Constants;
    using Contracts;
    using Dtos;
    using Models.Enums;
    using Services.Contracts;
    using System;
    using System.Linq;

    public class ShareAlbumCommand : ICommand
    {
        private const string CommandName = "ShareAlbum";

        private readonly IAlbumService albumService;
        private readonly IUserService userService;
        private readonly IAlbumRoleService albumRoleService;

        public ShareAlbumCommand(IAlbumService albumService, IUserService userService, IAlbumRoleService albumRoleService)
        {
            this.albumService = albumService;
            this.userService = userService;
            this.albumRoleService = albumRoleService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 3)
            {
                throw new InvalidOperationException(string.Format(Messages.InvalidCommand, CommandName));
            }

            var albumId = int.Parse(data[0]);
            var username = data[1];
            var roleName = data[2];
            Role permission;
            var ifParsed = Role.TryParse(roleName, out permission);

            if (!this.albumService.Exists(albumId))
            {
                throw new ArgumentException(string.Join(Messages.AlbumDoesNotExists, albumId));
            }

            if (!this.userService.Exists(username))
            {
                throw new ArgumentException(string.Join(Messages.UserDoesNotExists, username));
            }

            if (!ifParsed)
            {
                throw new ArgumentException(Messages.PermissionNotValid);
            }

            var userId = this.userService.GetLoggedInUserId();
            var album = this.albumService.ById<AlbumDto>(albumId);
            var albumRoles = this.albumRoleService.ByAlbumId<AlbumRoleDto>(album.Id);
            var albumRole = albumRoles.Where(ar => ar.UserId == userId && ar.Role == Role.Owner);
            if (userId == 0 || !albumRole.Any())
            {
                throw new InvalidOperationException(Messages.InvalidCredentials);
            }

            var userToShareTo = this.userService.ByUsername<UserDto>(username);
            this.albumRoleService.PublishAlbumRole(albumId, userToShareTo.Id, roleName);

            return string.Format(Messages.SuccessfullAlbumSharing, username, album.Name, roleName);
        }
    }
}