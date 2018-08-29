namespace App.Core.Commands
{
    using Contracts;
    using Data;
    using System;
    using System.Linq;
    using Utilities;

    public class KickMemberCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLenght(2, args);
            var teamName = args[0];
            var userName = args[1];

            AuthenticationManager.Authorize();

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            if (!CommandHelper.IsUserExisting(userName)
                || CommandHelper.IsUserDeleted(userName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UserNotFound, userName));
            }

            if (!CommandHelper.IsMemberOfTeam(teamName, userName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.NotPartOfTeam, userName, teamName));
            }

            var currentUser = AuthenticationManager.GetCurrentUser();
            if (!CommandHelper.IsUserCreatorOfTeam(teamName, currentUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            if (userName == currentUser.UserName)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.CommandNotAllowed);
            }

            using (var context = new TeamBuilderContext())
            {
                var team = context.Teams.FirstOrDefault(t => t.Name == teamName);
                var user = context.Users.FirstOrDefault(u => u.UserName == userName);

                var membership = team.Memebers.FirstOrDefault(m => m.TeamId == team.Id && m.UserId == user.Id);
                team.Memebers.Remove(membership);
                context.SaveChanges();
            }

            return string.Format(Constants.SuccessMessages.Kick, userName, teamName);
        }
    }
}