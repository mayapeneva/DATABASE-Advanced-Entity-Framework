namespace App.Core.Commands
{
    using Contracts;
    using Data;
    using Models;
    using Models.Enums;
    using System;
    using Utilities;

    public class RegisterUserCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLenght(7, args);

            var username = args[0];
            var password = args[1];
            var repeatedPassword = args[2];
            if (password != repeatedPassword)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.PasswordDoesNotMatch);
            }
            var firstName = args[3];
            var lastName = args[4];
            var age = int.Parse(args[5]);
            var ifParsed = Enum.TryParse<Gender>(args[6], false, out Gender gender);
            if (!ifParsed)
            {
                throw new ArgumentException(Constants.ErrorMessages.GenderNotValid);
            };

            if (CommandHelper.IsUserExisting(username))
            {
                var user = CommandHelper.GetUser(username);
                if (user.IsDeleted)
                {
                    using (var context = new TeamBuilderContext())
                    {
                        user.IsDeleted = false;
                        context.Users.Update(user);
                        context.SaveChanges();
                        return string.Format(Constants.SuccessMessages.Reregister, username);
                    }
                }

                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.UsernameIsTaken, username));
            }

            if (AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LogoutFirst);
            }

            using (var context = new TeamBuilderContext())
            {
                var user = new User
                {
                    UserName = username,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age,
                    Gender = gender
                };

                if (!Check.IsValid(user))
                {
                    throw new ArgumentException(Constants.ErrorMessages.UserNotValid);
                }

                context.Users.Add(user);
                context.SaveChanges();
            }

            return string.Format(Constants.SuccessMessages.Register, username);
        }
    }
}