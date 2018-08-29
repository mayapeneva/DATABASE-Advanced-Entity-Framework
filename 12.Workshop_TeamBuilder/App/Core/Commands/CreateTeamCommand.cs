namespace App.Core.Commands
{
    using Contracts;
    using Data;
    using Models;
    using System;
    using Utilities;

    public class CreateTeamCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLenght(2, args);

            AuthenticationManager.Authorize();

            var name = args[0];
            var acronym = args[1];

            if (CommandHelper.IsTeamExisting(name))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamExists, name));
            }

            using (var context = new TeamBuilderContext())
            {
                var currentUser = AuthenticationManager.GetCurrentUser();
                var team = new Team
                {
                    Name = name,
                    Acronym = acronym,
                    CreatorId = currentUser.Id
                };

                if (!Check.IsValid(team))
                {
                    throw new ArgumentException(Constants.ErrorMessages.TeamNotValid);
                }

                context.Teams.Add(team);
                context.SaveChanges();
            }

            return string.Format(Constants.SuccessMessages.Team, name);
        }
    }
}