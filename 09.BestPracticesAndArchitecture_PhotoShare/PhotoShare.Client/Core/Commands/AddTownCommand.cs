namespace PhotoShare.Client.Core.Commands
{
    using Constants;
    using Contracts;
    using Services.Contracts;
    using System;

    public class AddTownCommand : ICommand
    {
        private const string CommandName = "AddTown";

        private readonly ITownService townService;
        private readonly IUserService userService;

        public AddTownCommand(ITownService townService, IUserService userService)
        {
            this.townService = townService;
            this.userService = userService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 2)
            {
                throw new InvalidOperationException(string.Format(Messages.InvalidCommand, CommandName));
            }

            string townName = data[0];
            string country = data[1];

            if (this.townService.Exists(townName))
            {
                throw new ArgumentException(string.Format(Messages.TownAlreadyExists, townName));
            }

            if (!this.userService.AnyUserLoggedIn())
            {
                throw new InvalidOperationException(Messages.InvalidCredentials);
            }

            var town = this.townService.Add(townName, country);

            return string.Format(Messages.SuccessfullTownAdding, townName);
        }
    }
}