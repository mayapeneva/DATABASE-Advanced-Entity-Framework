namespace PhotoShare.Client.Core.Commands
{
    using Constants;
    using Contracts;
    using Dtos;
    using Models.Enums;
    using Services.Contracts;
    using System;
    using System.Linq;

    public class UploadPictureCommand : ICommand
    {
        private const string CommandName = "UploadPicture";

        private readonly IPictureService pictureService;
        private readonly IAlbumService albumService;
        private readonly IAlbumRoleService albumRoleService;
        private readonly IUserService userService;

        public UploadPictureCommand(IPictureService pictureService, IAlbumService albumService, IAlbumRoleService albumRoleService, IUserService userService)
        {
            this.pictureService = pictureService;
            this.albumService = albumService;
            this.albumRoleService = albumRoleService;
            this.userService = userService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 3)
            {
                throw new InvalidOperationException(string.Format(Messages.InvalidCommand, CommandName));
            }

            string albumName = data[0];
            string pictureTitle = data[1];
            string path = data[2];

            var albumExists = this.albumService.Exists(albumName);

            if (!albumExists)
            {
                throw new ArgumentException(string.Format(Messages.AlbumDoesNotExists, albumName));
            }

            var userId = this.userService.GetLoggedInUserId();
            var album = this.albumService.ByName<AlbumDto>(albumName);
            var albumRoles = this.albumRoleService.ByAlbumId<AlbumRoleDto>(album.Id);
            var albumRole = albumRoles.Where(ar => ar.UserId == userId && ar.Role == Role.Owner);
            if (userId == 0 || !albumRole.Any())
            {
                throw new InvalidOperationException(Messages.InvalidCredentials);
            }

            var albumId = this.albumService.ByName<AlbumDto>(albumName).Id;

            var picture = this.pictureService.Create(albumId, pictureTitle, path);

            return string.Format(Messages.SuccessfullPictureUploading, pictureTitle, albumName);
        }
    }
}