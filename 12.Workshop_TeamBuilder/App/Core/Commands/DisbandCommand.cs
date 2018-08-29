namespace App.Core.Commands
{
    using Contracts;
    using Data;
    using System;
    using System.Linq;
    using Utilities;

    public class DisbandCommand : ICommand

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
            if (!CommandHelper.IsUserCreatorOfTeam(teamName, currentUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            using (var context = new TeamBuilderContext())
            {
                var team = context.Teams.FirstOrDefault(t => t.Name == teamName);
                context.Teams.Remove(team);
                context.SaveChanges();
            }

            return string.Format(Constants.SuccessMessages.Disband, teamName);
        }
    }
}