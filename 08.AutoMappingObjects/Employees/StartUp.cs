namespace Employees
{
    using AutoMapper;
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Services;
    using Services.Contracts;
    using System;

    public class StartUp
    {
        public static void Main()
        {
            var service = ConfigureService();

            var engine = new Engine(service);
            engine.Run();
        }

        private static IServiceProvider ConfigureService()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<EmployeesContext>(opts => opts.UseSqlServer(Configuration.ConnectionString));

            serviceCollection.AddAutoMapper(conf => conf.AddProfile<EmployeesProfile>());

            serviceCollection.AddTransient<IDBInitializerService, DBInitializerService>();
            serviceCollection.AddTransient<ICommandInterpreter, CommandInterpreter>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}