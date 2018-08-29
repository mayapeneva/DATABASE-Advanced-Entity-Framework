namespace App.Core.Commands
{
    using Contracts;
    using Data;
    using System;
    using System.Linq;
    using System.Text;
    using Utilities;

    public class ShowEventCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLenght(1, args);
            var eventName = args[0];

            if (!CommandHelper.IsEventExisting(eventName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound, eventName));
            }

            var result = new StringBuilder();
            using (var context = new TeamBuilderContext())
            {
                var eventt = context.Events
                    .Where(e => e.Name == eventName)
                    .OrderByDescending(e => e.StartDate)
                    .FirstOrDefault();

                result.AppendLine($"{eventt.Name} {eventt.StartDate} {eventt.EndDate}");
                result.AppendLine(eventt.Description);
                result.AppendLine("Teams:");
                foreach (var team in eventt.ParticipatingTeams)
                {
                    result.AppendLine($"-{team.Team.Name}");
                }
            }

            return result.ToString().Trim();
        }
    }
}