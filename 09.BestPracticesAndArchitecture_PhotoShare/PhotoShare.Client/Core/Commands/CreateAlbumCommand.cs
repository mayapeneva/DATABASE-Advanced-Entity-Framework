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

    public class CreateAlbumCommand : ICommand
    {
        private const string CommandName = "CreateAlbum";

        private readonly IAlbumService albumService;
        private readonly IAlbumRoleService albumRoleService;
        private readonly IUserService userService;
        private readonly ITagService tagService;

        public CreateAlbumCommand(IAlbumService albumService, IAlbumRoleService albumRoleService, IUserService userService, ITagService tagService)
        {
            this.albumService = albumService;
            this.albumRoleService = albumRoleService;
            this.userService = userService;
            this.tagService = tagService;
        }

        public string Execute(string[] data)
        {
            if (data.Length < 3)
            {
                throw new InvalidOperationException(string.Format(Messages.InvalidCommand, CommandName));
            }

            var username = data[0];
            var albumTitle = data[1];
            var colorName = data[2];
            var tags = new string[data.Length - 3];
            if (data.Length > 3)
            {
                tags = data.Skip(3).ToArray();
            }

            if (!this.userService.Exists(username))
            {
                throw new ArgumentException(string.Format(Messages.UserDoesNotExists, username));
            }

            if (this.albumService.Exists(albumTitle))
            {
                throw new ArgumentException(string.Format(Messages.AlbumAlreadyExists, albumTitle));
            }

            var user = this.userService.ByUsername<UserDto>(username);
            if (!user.IsLogged)
            {
                throw new InvalidOperationException(Messages.InvalidCredentials);
            }

            Color bgColor;
            var ifParsed = Color.TryParse(colorName, out bgColor);
            if (!ifParsed)
            {
                throw new ArgumentException(string.Format(Messages.ColorDoesNotExist, colorName));
            }

            for (int i = 0; i < tags.Length; i++)
            {
                tags[i] = tags[i].ValidateOrTransform();

                if (!this.tagService.Exists(tags[i]))
                {
                    throw new ArgumentException(Messages.InvalidTag);
                }
            }

            this.albumService.Create(user.Id, albumTitle, colorName, tags);

            return string.Format(Messages.SuccessfullAlbumCreation, albumTitle);
        }
    }
}