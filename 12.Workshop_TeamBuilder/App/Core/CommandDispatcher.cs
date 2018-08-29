namespace App.Core
{
    using Commands;
    using System;
    using System.Linq;
    using System.Reflection;
    using Contracts;
    using Utilities;

    public class CommandDispatcher
    {
        public const string Sufix = "Command";

        public string Dispatch(string input)
        {
            var inputArgs = input.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var commandName = inputArgs.Length > 0 ? inputArgs[0] + Sufix : string.Empty;
            var args = inputArgs.Skip(1).ToArray();

            var commandType = Assembly.GetCallingAssembly().GetTypes().FirstOrDefault(t => t.Name == commandName);
            if (commandType == null)
            {
                throw new NotSupportedException(string.Format(Constants.ErrorMessages.CommandNotValid, commandName));
            }

            var command = (ICommand)Activator.CreateInstance(commandType);
            return command.Execute(args);

            //var result = string.Empty;
            //switch (commandName)
            //{
            //    case "RegisterUser":
            //        var register = new RegisterUserCommand();
            //        result = register.Execute(args);
            //        break;

            //    case "Login":
            //        var login = new LoginCommand();
            //        result = login.Execute(args);
            //        break;

            //    case "Logout":
            //        var logout = new LogoutCommand();
            //        result = logout.Execute(args);
            //        break;

            //    case "DeleteUser":
            //        var delete = new DeleteUserCommand();
            //        result = delete.Execute(args);
            //        break;

            //    case "CreateEvent":
            //        var createEvent = new CreateEventCommand();
            //        result = createEvent.Execute(args);
            //        break;

            //    case "CreateTeam":
            //        var createTeam = new CreateTeamCommand();
            //        result = createTeam.Execute(args);
            //        break;

            //    case "InviteToTeam":
            //        var invite = new InviteToTeamCommand();
            //        result = invite.Execute(args);
            //        break;

            //    case "AcceptInvite":
            //        var acceptInvite = new AcceptInviteCommand();
            //        result = acceptInvite.Execute(args);
            //        break;

            //    case "DeclineInvite":
            //        var declineInvite = new DeclineInviteCommand();
            //        result = declineInvite.Execute(args);
            //        break;

            //    case "KickMember":
            //        var kick = new KickMemberCommand();
            //        result = kick.Execute(args);
            //        break;

            //    case "Disband":
            //        var disband = new DisbandCommand();
            //        result = disband.Execute(args);
            //        break;

            //    case "AddTeamTo":
            //        var addTeam = new AddTeamToCommand();
            //        result = addTeam.Execute(args);
            //        break;

            //    case "ShowEvent":
            //        var showEvent = new ShowEventCommand();
            //        result = showEvent.Execute(args);
            //        break;

            //    case "ShowTeam":
            //        var showTeam = new ShowTeamCommand();
            //        result = showTeam.Execute(args);
            //        break;

            //    case "Exit":
            //        var exit = new ExitCommand();
            //        exit.Execute(args);
            //        break;

            //    default:
            //        throw new NotSupportedException(string.Format(Constants.ErrorMessages.CommandNotValid, commandName));
            //}

            //return result;
        }
    }
}