namespace Employees
{
    using Contracts;
    using System;
    using System.Linq;
    using System.Reflection;

    public class CommandInterpreter : ICommandInterpreter
    {
        public const string CommandSufix = "Command";
        private readonly IServiceProvider serviceProvider;

        public CommandInterpreter(IServiceProvider service)
        {
            this.serviceProvider = service;
        }

        public ICommand Interpret(string command)
        {
            var commandName = command + CommandSufix;

            var classtype = Assembly.GetCallingAssembly().GetTypes().FirstOrDefault(t => t.Name == commandName);
            var ctor = classtype.GetConstructors().First();
            var ctorParams = ctor.GetParameters().Select(p => p.ParameterType).ToArray();
            var service = ctorParams.Select(this.serviceProvider.GetService).ToArray();

            return (ICommand)ctor.Invoke(service);
        }
    }
}