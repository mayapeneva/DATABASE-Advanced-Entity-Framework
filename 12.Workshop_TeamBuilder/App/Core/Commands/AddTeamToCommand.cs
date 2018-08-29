namespace App.Core.Commands
{
    using Contracts;
    using Data;
    using Models;
    using System;
    using System.Linq;
    using Utilities;

    public class AddTeamToCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLenght(2, args);
            var eventName = args[0];
            var teamName = args[1];

            AuthenticationManager.Authorize();

            if (!CommandHelper.IsEventExisting(eventName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound, eventName));
            }

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            var currentUser = AuthenticationManager.GetCurrentUser();
            if (!CommandHelper.IsUserCreatorOfEvent(eventName, currentUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            if (CommandHelper.IsTeamAddedToEvent(eventName, teamName))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.CannotAddSameTeamTwice);
            }

            using (var context = new TeamBuilderContext())
            {
                var eventt = context.Events
                    .Where(e => e.Name == eventName)
                    .OrderByDescending(e => e.StartDate)
                    .FirstOrDefault();
                var team = context.Teams.FirstOrDefault(t => t.Name == teamName);

                var teamEvent = new TeamEvent
                {
                    Event = eventt,
                    Team = team
                };

                eventt.ParticipatingTeams.Add(teamEvent);
                context.SaveChanges();
            }

            return string.Format(Constants.SuccessMessages.AddTeamTo, teamName, eventName);
        }
    }
}