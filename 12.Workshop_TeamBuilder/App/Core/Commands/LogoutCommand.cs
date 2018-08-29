namespace App.Core.Commands
{
    using Contracts;
    using Utilities;

    public class LogoutCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLenght(0, args);

            AuthenticationManager.Authorize();

            var currentUser = AuthenticationManager.GetCurrentUser();

            AuthenticationManager.Logout();

            return string.Format(Constants.SuccessMessages.Logout, currentUser.UserName);
        }
    }
}