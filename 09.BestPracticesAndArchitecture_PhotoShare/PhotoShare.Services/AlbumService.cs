namespace PhotoShare.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Models;
    using Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AlbumService : IAlbumService
    {
        private readonly PhotoShareContext context;

        public AlbumService(PhotoShareContext context)
        {
            this.context = context;
        }

        public TModel ById<TModel>(int id)
        {
            return this.By<TModel>(a => a.Id == id).SingleOrDefault();
        }

        public TModel ByName<TModel>(string name)
        {
            return this.By<TModel>(a => a.Name == name).SingleOrDefault();
        }

        public bool Exists(int id)
        {
            return this.ById<Album>(id) != null;
        }

        public bool Exists(string name)
        {
            return this.ByName<Album>(name) != null;
        }

        private IEnumerable<TModel> By<TModel>(Func<Album, bool> predicate) =>
            this.context.Albums
                .Where(predicate)
                .AsQueryable()
                .ProjectTo<TModel>();

        public Album Create(int userId, string albumTitle, string bgColor, string[] tags)
        {
            var color = Enum.Parse<Color>(bgColor, true);

            var album = new Album()
            {
                Name = albumTitle,
                BackgroundColor = color,
            };
            this.context.Albums.Add(album);

            var albumRole = new AlbumRole()
            {
                AlbumId = album.Id,
                UserId = userId,
                Role = Role.Owner
            };
            this.context.AlbumRoles.Add(albumRole);

            foreach (var tagName in tags)
            {
                var tag = this.context.Tags.FirstOrDefault(t => t.Name == tagName);
                var albumTag = new AlbumTag()
                {
                    AlbumId = album.Id,
                    TagId = tag.Id
                };
                this.context.AlbumTags.Add(albumTag);
            }

            this.context.SaveChanges();

            return album;
        }
    }
}