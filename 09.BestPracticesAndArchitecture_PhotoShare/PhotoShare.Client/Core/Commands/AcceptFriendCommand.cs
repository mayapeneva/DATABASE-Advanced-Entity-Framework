namespace PhotoShare.Client.Core.Commands
{
    using Constants;
    using Contracts;
    using Dtos;
    using Services.Contracts;
    using System;
    using System.Linq;

    public class AcceptFriendCommand : ICommand
    {
        private const string CommandName = "AcceptFriend";

        private readonly IUserService userService;

        public AcceptFriendCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 2)
            {
                throw new InvalidOperationException(string.Format(Messages.InvalidCommand, CommandName));
            }

            var userName1 = data[0];
            var userName2 = data[1];

            for (int i = 0; i < data.Length; i++)
            {
                if (!this.userService.Exists(data[i]))
                {
                    throw new ArgumentException(string.Format(Messages.UserDoesNotExists, data[i]));
                }
            }

            var user1 = this.userService.ByUsername<UserFriendsDto>(userName1);
            var user2 = this.userService.ByUsername<UserFriendsDto>(userName2);

            var requestFromUser = user1.Friends.Any(u => u.Username == userName2);
            var requestFromFriend = user2.Friends.Any(u => u.Username == userName1);
            if (requestFromUser && requestFromFriend)
            {
                throw new InvalidOperationException(string.Format(Messages.UsersAreAlreadyFriends, userName1, userName2));
            }

            if (requestFromUser && !requestFromFriend)
            {
                throw new InvalidOperationException(Messages.UserAlreadySentRequest);
            }

            this.userService.AcceptFriend(user1.Id, user2.Id);

            return string.Format(Messages.SuccessfullFriendAccepting, userName1, userName2);
        }
    }
}