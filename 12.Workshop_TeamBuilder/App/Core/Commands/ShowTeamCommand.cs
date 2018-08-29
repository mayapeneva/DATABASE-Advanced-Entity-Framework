namespace App.Core.Commands
{
    using Contracts;
    using Data;
    using System;
    using System.Linq;
    using System.Text;
    using Utilities;

    public class ShowTeamCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLenght(1, args);
            var teamName = args[0];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            var result = new StringBuilder();
            using (var context = new TeamBuilderContext())
            {
                var team = context.Teams.FirstOrDefault(t => t.Name == teamName);
                result.AppendLine($"{team.Name} {team.Acronym}");
                result.AppendLine("Members:");
                foreach (var member in team.Memebers)
                {
                    result.AppendLine($"--{member.User.UserName}");
                }
            }

            return result.ToString().Trim();
        }
    }
}