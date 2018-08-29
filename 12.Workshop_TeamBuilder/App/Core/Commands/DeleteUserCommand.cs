namespace App.Core.Commands
{
    using Contracts;
    using Data;
    using Utilities;

    public class DeleteUserCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLenght(0, args);

            AuthenticationManager.Authorize();

            var currentUser = AuthenticationManager.GetCurrentUser();
            using (var context = new TeamBuilderContext())
            {
                currentUser.IsDeleted = true;
                context.Users.Update(currentUser);
                context.SaveChanges();

                AuthenticationManager.Logout(); ;
            }

            return string.Format(Constants.SuccessMessages.Delete, currentUser.UserName);
        }
    }
}