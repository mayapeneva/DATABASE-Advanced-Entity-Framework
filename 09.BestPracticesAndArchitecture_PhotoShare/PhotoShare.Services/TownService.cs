namespace PhotoShare.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TownService : ITownService
    {
        private readonly PhotoShareContext context;

        public TownService(PhotoShareContext context)
        {
            this.context = context;
        }

        public TModel ById<TModel>(int id)
        {
            return this.By<TModel>(t => t.Id == id).SingleOrDefault();
        }

        public TModel ByName<TModel>(string name)
        {
            return this.By<TModel>(t => t.Name == name).SingleOrDefault();
        }

        public bool Exists(int id)
        {
            return this.ById<Town>(id) != null;
        }

        public bool Exists(string name)
        {
            return this.ByName<Town>(name) != null;
        }

        private IEnumerable<TModel> By<TModel>(Func<Town, bool> predicate) =>
            this.context.Towns
                .Where(predicate)
                .AsQueryable()
                .ProjectTo<TModel>();

        public Town Add(string townName, string countryName)
        {
            var town = new Town()
            {
                Country = countryName,
                Name = townName
            };

            this.context.Towns.Add(town);
            this.context.SaveChanges();

            return town;
        }
    }
}