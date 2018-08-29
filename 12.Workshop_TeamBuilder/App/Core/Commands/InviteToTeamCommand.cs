namespace App.Core.Commands
{
    using Contracts;
    using Data;
    using Models;
    using System;
    using System.Linq;
    using Utilities;

    public class InviteToTeamCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLenght(2, args);

            AuthenticationManager.Authorize();

            var teamName = args[0];
            var newMemberUserName = args[1];

            if (!CommandHelper.IsUserExisting(newMemberUserName)
                || CommandHelper.IsUserDeleted(newMemberUserName)
                || !CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(Constants.ErrorMessages.TeamOrUserNotExist);
            }

            var currentUser = AuthenticationManager.GetCurrentUser();
            if (!CommandHelper.IsUserCreatorOfTeam(teamName, currentUser)
                || !CommandHelper.IsMemberOfTeam(teamName, currentUser.UserName)
                || CommandHelper.IsMemberOfTeam(teamName, newMemberUserName))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            var newUser = CommandHelper.GetUser(newMemberUserName);
            if (CommandHelper.IsInvitationExisting(teamName, newUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.InviteIsAlreadySent);
            }

            using (var context = new TeamBuilderContext())
            {
                var team = context.Teams.FirstOrDefault(t => t.Name == teamName);

                var invitation = new Invitation
                {
                    Team = team,
                    InvitedUser = newUser
                };

                context.Invitations.Add(invitation);
                context.SaveChanges();
            }

            return string.Format(Constants.SuccessMessages.Invite, teamName, newMemberUserName);
        }
    }
}