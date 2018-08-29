﻿namespace PhotoShare.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TagService : ITagService
    {
        private readonly PhotoShareContext context;

        public TagService(PhotoShareContext context)
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
            return this.ById<Tag>(id) != null;
        }

        public bool Exists(string name)
        {
            return this.ByName<Tag>(name) != null;
        }

        private IEnumerable<TModel> By<TModel>(Func<Tag, bool> predicate) =>
            this.context.Tags
                .Where(predicate)
                .AsQueryable()
                .ProjectTo<TModel>();

        public Tag AddTag(string name)
        {
            var tag = new Tag()
            {
                Name = name
            };

            this.context.Tags.Add(tag);
            this.context.SaveChanges();

            return tag;
        }
    }
}