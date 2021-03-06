﻿namespace PhotoShare.Services
{
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseInitializerService : IDatabaseInitializerService
    {
        private readonly PhotoShareContext context;

        public DatabaseInitializerService(PhotoShareContext context)
        {
            this.context = context;
        }

        public void InitializeDatabase()
        {
            this.context.Database.Migrate();
        }
    }
}