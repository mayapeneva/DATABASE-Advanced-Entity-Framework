﻿namespace PhotoShare.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PictureService : IPictureService
    {
        private readonly PhotoShareContext context;

        public PictureService(PhotoShareContext context)
        {
            this.context = context;
        }

        public TModel ById<TModel>(int id)
                => this.By<TModel>(a => a.Id == id).SingleOrDefault();

        public TModel ByTitle<TModel>(string name)
            => this.By<TModel>(a => a.Title == name).SingleOrDefault();

        public bool Exists(int id)
            => this.ById<Picture>(id) != null;

        public bool Exists(string name)
           => this.ByTitle<Picture>(name) != null;

        private IEnumerable<TModel> By<TModel>(Func<Picture, bool> predicate) =>
            this.context.Pictures
                .Where(predicate)
                .AsQueryable()
                .ProjectTo<TModel>();

        public Picture Create(int albumId, string pictureTitle, string pictureFilePath)
        {
            var picture = new Picture()
            {
                Title = pictureTitle,
                Path = pictureFilePath,
                AlbumId = albumId
            };

            this.context.Pictures.Add(picture);

            this.context.SaveChanges();

            return picture;
        }
    }
}