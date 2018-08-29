namespace PhotoShare.Client.Core.Commands
{
    using Constants;
    using Contracts;
    using Dtos;
    using Services.Contracts;
    using System;
    using System.Linq;

    public class ModifyUserCommand : ICommand
    {
        private const string CommandName = "ModifyUser";

        private readonly IUserService userService;
        private readonly ITownService townService;

        public ModifyUserCommand(IUserService userService, ITownService townService)
        {
            this.userService = userService;
            this.townService = townService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 3)
            {
                throw new InvalidOperationException(string.Format(Messages.InvalidCommand, CommandName));
            }

            var username = data[0];
            var property = data[1];
            var newValue = data[2];

            if (!this.userService.Exists(username))
            {
                throw new ArgumentException(string.Format(Messages.UserDoesNotExists, username));
            }

            var user = this.userService.ByUsername<UserDto>(username);
            if (!user.IsLogged)
            {
                throw new InvalidOperationException(Messages.InvalidCredentials);
            }

            if (property == "Password")
            {
                if (newValue.Any(char.IsLower) && newValue.Any(char.IsDigit))
                {
                    this.userService.ChangePassword(user.Id, newValue);
                }
                else
                {
                    throw new ArgumentException(string.Format(Messages.ValueNotValidForPassword, newValue));
                }
            }
            else if (property == "BornTown")
            {
                if (this.townService.Exists(newValue))
                {
                    var town = this.townService.ByName<TownDto>(newValue);
                    this.userService.SetBornTown(user.Id, town.Id);
                }
                else
                {
                    throw new ArgumentException(string.Format(Messages.ValueNotValidForTown, newValue, newValue));
                }
            }
            else if (property == "CurrentTown")
            {
                if (this.townService.Exists(newValue))
                {
                    var town = this.townService.ByName<TownDto>(newValue);
                    this.userService.SetCurrentTown(user.Id, town.Id);
                }
                else
                {
                    throw new ArgumentException(string.Format(Messages.ValueNotValidForTown, newValue, newValue));
                }
            }
            else
            {
                throw new ArgumentException(string.Format(Messages.PropertyNotFound, property));
            }

            return string.Format(Messages.SuccessfullUserModification, username, property, newValue);
        }
    }
}