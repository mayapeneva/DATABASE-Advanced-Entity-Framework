namespace PhotoShare.Client.Core.Commands
{
    using Constants;
    using Contracts;
    using Dtos;
    using Services.Contracts;
    using System;

    public class LoginCommand : ICommand
    {
        private const string CommandName = "LoginUser";

        private readonly IUserService userService;

        public LoginCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 2)
            {
                throw new InvalidOperationException(string.Format(Messages.InvalidCommand, CommandName));
            }

            var username = data[0];
            var password = data[1];

            var user = this.userService.ByUsername<UserDto>(username);
            if (!this.userService.Exists(username) || user.Password != password)
            {
                throw new ArgumentException(Messages.UserDoesNotExistOrPassDontMatch);
            }

            if (this.userService.AnyUserLoggedIn())
            {
                throw new InvalidOperationException(Messages.InvalidCredentials);
            }

            if (this.userService.AnyUserLoggedIn() == true)
            {
                throw new ArgumentException(Messages.UserAlreadyLoggedIn);
            }

            this.userService.LogIn(username);

            return string.Format(Messages.SuccessfullLogin, username);
        }
    }
}