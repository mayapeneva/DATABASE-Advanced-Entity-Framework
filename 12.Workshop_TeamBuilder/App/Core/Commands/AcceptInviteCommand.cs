namespace App.Core.Commands
{
    using Contracts;
    using Data;
    using Models;
    using System;
    using System.Linq;
    using Utilities;

    public class AcceptInviteCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLenght(1, args);
            var teamName = args[0];

            AuthenticationManager.Authorize();

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            var currentUser = AuthenticationManager.GetCurrentUser();
            if (!CommandHelper.IsInvitationExistingAndIsActive(teamName, currentUser))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InviteNotFound, teamName));
            }

            using (var context = new TeamBuilderContext())
            {
                var invitation = context.Invitations.FirstOrDefault(i => i.Team.Name == teamName && i.InvitedUserId == currentUser.Id);
                var team = context.Teams.FirstOrDefault(t => t.Name == teamName);

                invitation.IsActive = false;
                context.Invitations.Update(invitation);

                var userTeam = new UserTeam
                {
                    Team = team,
                    User = currentUser
                };
                team.Memebers.Add(userTeam);
                context.SaveChanges();
            }

            return string.Format(Constants.SuccessMessages.Accept, currentUser.UserName, teamName);
        }
    }
}