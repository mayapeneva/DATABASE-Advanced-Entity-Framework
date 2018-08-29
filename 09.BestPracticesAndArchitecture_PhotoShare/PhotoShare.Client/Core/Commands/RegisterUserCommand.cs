namespace PhotoShare.Client.Core.Commands
{
    using Constants;
    using Contracts;
    using Dtos;
    using Services.Contracts;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class RegisterUserCommand : ICommand
    {
        private const string CommandName = "RegisterUser";

        private readonly IUserService userService;

        public RegisterUserCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 4)
            {
                throw new InvalidOperationException(string.Format(Messages.InvalidCommand, CommandName));
            }

            var username = data[0];
            var password = data[1];
            var password2 = data[2];
            var email = data[3];

            if (this.userService.Exists(username))
            {
                throw new InvalidOperationException(string.Format(Messages.UsernameTaken, username));
            }

            if (password != password2)
            {
                throw new ArgumentException(string.Format(Messages.PasswordsDontMatch));
            }

            if (this.userService.AnyUserLoggedIn())
            {
                throw new InvalidOperationException(Messages.InvalidCredentials);
            }

            var registerDto = new RegisterUserDto()
            {
                Username = username,
                Password = password,
                Email = email,
            };
            if (!this.IsValid(registerDto))
            {
                throw new ArgumentException(Messages.InvalidDetails);
            }

            this.userService.Register(username, password, email);

            return string.Format(Messages.SuccessfullUserRegistration, username);
        }

        private bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}