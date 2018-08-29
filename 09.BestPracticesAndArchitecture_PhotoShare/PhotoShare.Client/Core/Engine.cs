namespace PhotoShare.Client.Core
{
    using Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using Services.Contracts;
    using System;
    using System.Data.SqlClient;

    public class Engine : IEngine
    {
        private readonly IServiceProvider serviceProvider;

        public Engine(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Run()
        {
            var initializeService = this.serviceProvider.GetService<IDatabaseInitializerService>();
            initializeService.InitializeDatabase();

            var commandInterpreter = this.serviceProvider.GetService<ICommandInterpreter>();

            while (true)
            {
                try
                {
                    string[] input = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    string result = commandInterpreter.Read(input);
                    Console.WriteLine(result);
                    if (result == "Good Bye!")
                    {
                        Environment.Exit(0);
                    }
                }
                catch (Exception exception)
                    when (exception is SqlException
                          || exception is ArgumentException
                          || exception is InvalidOperationException)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }
    }
}