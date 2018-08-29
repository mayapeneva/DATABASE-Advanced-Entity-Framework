namespace PhotoShare.Client.Core.Commands
{
    using Constants;
    using Contracts;
    using Dtos;
    using Services.Contracts;
    using System;

    public class DeleteUserCommand : ICommand
    {
        private const string CommandName = "DeleteUser";

        private readonly IUserService userService;

        public DeleteUserCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 1)
            {
                throw new InvalidOperationException(string.Format(Messages.InvalidCommand, CommandName));
            }

            string username = data[0];

            var userExists = this.userService.Exists(username);

            if (!userExists)
            {
                throw new ArgumentException(string.Format(Messages.UserDoesNotExists, username));
            }

            var user = this.userService.ByUsername<UserDto>(username);
            if (!user.IsLogged)
            {
                throw new InvalidOperationException(Messages.InvalidCredentials);
            }

            if (user.IsDeleted == true)
            {
                throw new ArgumentException(string.Format(Messages.UserAlreadyDeleted, username));
            }

            this.userService.Delete(username);

            return string.Format(Messages.SuccessfullUserDeletion, username);
        }
    }
}