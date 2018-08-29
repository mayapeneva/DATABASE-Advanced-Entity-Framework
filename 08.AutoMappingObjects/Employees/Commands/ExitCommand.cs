namespace Employees.Commands
{
    using Contracts;
    using System;

    public class ExitCommand : ICommand
    {
        public string Execute(string[] arguments)
        {
            Environment.Exit(0);
            return null;
        }
    }
}