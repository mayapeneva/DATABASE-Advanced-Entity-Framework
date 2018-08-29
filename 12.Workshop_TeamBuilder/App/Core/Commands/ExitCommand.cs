namespace App.Core.Commands
{
    using Contracts;
    using System;
    using Utilities;

    public class ExitCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLenght(0, args);

            Environment.Exit(0);

            return "Bye!";
        }
    }
}