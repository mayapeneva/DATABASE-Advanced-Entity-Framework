namespace App.Core.Commands
{
    using Contracts;
    using Data;
    using Models;
    using System;
    using System.Linq;
    using Utilities;

    public class LoginCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLenght(2, args);

            var username = args[0];
            var password = args[1];

            if (AuthenticationManager.IsAuthenticated())
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LogoutFirst);
            }

            var user = this.GetUserByCredentials(username, password);
            if (user == null || user.IsDeleted)
            {
                throw new ArgumentException(Constants.ErrorMessages.UserOrPasswordIsInvalid);
            }

            AuthenticationManager.Login(user);

            return string.Format(Constants.SuccessMessages.Login, user.UserName);
        }

        private User GetUserByCredentials(string username, string password)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
            }
        }
    }
}