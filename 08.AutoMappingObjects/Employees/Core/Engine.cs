namespace Employees
{
    using Contracts;
    using Employees.Services.Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;

    public class Engine : IEngine
    {
        private readonly IServiceProvider serviceProvider;

        public Engine(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Run()
        {
            var initializeDb = this.serviceProvider.GetService<IDBInitializerService>();
            initializeDb.InitializeDatabase();

            var interpreter = this.serviceProvider.GetService<ICommandInterpreter>();

            while (true)
            {
                var input = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var commandName = input[0];
                var arguments = input.Skip(1).ToArray();

                try
                {
                    var command = interpreter.Interpret(commandName);

                    Console.WriteLine(command.Execute(arguments));
                }
                catch (ArgumentException exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }
    }
}