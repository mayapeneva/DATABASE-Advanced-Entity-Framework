namespace PhotoShare.Client.Core.Commands
{
    using Constants;
    using Contracts;
    using Services.Contracts;
    using System;

    public class LogoutCommand : ICommand
    {
        private const string CommandName = "LogoutUser";

        private readonly IUserService userService;

        public LogoutCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 0)
            {
                throw new InvalidOperationException(string.Format(Messages.InvalidCommand, CommandName));
            }

            if (!this.userService.AnyUserLoggedIn())
            {
                throw new ArgumentException(Messages.NoUserLoggedIn);
            }

            var username = this.userService.LogOut();

            return string.Format(Messages.SuccessfullLogout, username);
        }
    }
}