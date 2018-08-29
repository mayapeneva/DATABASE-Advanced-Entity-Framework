namespace PhotoShare.Client.Core.Commands
{
    using Constants;
    using Contracts;

    public class ExitCommand : ICommand
    {
        public string Execute(string[] data)
        {
            return Messages.Goodbye;
        }
    }
}