﻿namespace PhotoShare.Client
{
    using AutoMapper;
    using Core;
    using Core.Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Services;
    using Services.Contracts;
    using System;
    using System.IO;

    public class StartUp
    {
        public static void Main()
        {
            var service = ConfigureServices();

            Engine engine = new Engine(service);
            engine.Run();
        }

        private static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            serviceCollection.AddDbContext<PhotoShareContext>(options =>
                options.UseSqlServer(SQLConfiguration.ConnectionString));

            serviceCollection.AddAutoMapper(cfg => cfg.AddProfile<PhotoShareProfile>());

            serviceCollection.AddTransient<ICommandInterpreter, CommandInterpreter>();
            serviceCollection.AddTransient<IDatabaseInitializerService, DatabaseInitializerService>();

            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<IAlbumService, AlbumService>();

            serviceCollection.AddTransient<ITownService, TownService>();
            serviceCollection.AddTransient<ITagService, TagService>();

            serviceCollection.AddTransient<IAlbumRoleService, AlbumRoleService>();
            serviceCollection.AddTransient<IAlbumTagService, AlbumTagService>();
            serviceCollection.AddTransient<IPictureService, PictureService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}