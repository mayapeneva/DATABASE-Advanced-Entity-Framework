namespace PhotoShare.Client.Core.Commands
{
    using Constants;
    using Contracts;
    using Dtos;
    using Services.Contracts;
    using System;
    using System.Linq;
    using System.Text;

    public class PrintFriendsListCommand : ICommand
    {
        private const string CommandName = "PrintFriendsList";

        private readonly IUserService userService;

        public PrintFriendsListCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 1)
            {
                throw new InvalidOperationException(string.Format(Messages.InvalidCommand, CommandName));
            }

            var userName = data[0];
            if (!this.userService.Exists(userName))
            {
                throw new ArgumentException(string.Join(Messages.UserDoesNotExists, userName));
            }

            var user = this.userService.ByUsername<UserFriendsDto>(userName);
            if (!user.Friends.Any())
            {
                return Messages.UserDoesNotHaveFriends;
            }

            var result = new StringBuilder();
            result.AppendLine("Friends:");
            foreach (var friend in user.Friends.OrderBy(f => f.Username))
            {
                result.AppendLine($"-{friend.Username}");
            }

            return result.ToString().Trim();
        }
    }
}