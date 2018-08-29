namespace PhotoShare.Client.Core.Commands
{
    using Constants;
    using Contracts;
    using Services.Contracts;
    using System;
    using Utilities;

    public class AddTagCommand : ICommand
    {
        private const string CommandName = "AddTag";

        private readonly ITagService tagService;
        private readonly IUserService userService;

        public AddTagCommand(ITagService tagService, IUserService userService)
        {
            this.tagService = tagService;
            this.userService = userService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 1)
            {
                throw new InvalidOperationException(string.Format(Messages.InvalidCommand, CommandName));
            }

            var tagName = data[0];
            if (this.tagService.Exists(tagName))
            {
                throw new ArgumentException(string.Format(Messages.TagAlreadyExists, tagName));
            }

            if (!this.userService.AnyUserLoggedIn())
            {
                throw new InvalidOperationException(Messages.InvalidCredentials);
            }

            tagName = tagName.ValidateOrTransform();

            var tag = this.tagService.AddTag(tagName);

            return string.Format(Messages.SuccessfullTagAdding, tagName);
        }
    }
}