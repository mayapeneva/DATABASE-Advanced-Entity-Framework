namespace PhotoShare.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Models;
    using Models.Enums;
    using System;
    using System.Linq;

    public class AlbumRoleService : IAlbumRoleService
    {
        private readonly PhotoShareContext context;

        public AlbumRoleService(PhotoShareContext context)
        {
            this.context = context;
        }

        public TModel[] ByAlbumId<TModel>(int albumId)
        {
            return this.context.AlbumRoles.Where(ar => ar.AlbumId == albumId)
                .AsQueryable()
                .ProjectTo<TModel>().ToArray();
        }

        public AlbumRole PublishAlbumRole(int albumId, int userId, string role)
        {
            var roleAsEnum = Enum.Parse<Role>(role);

            var albumRole = new AlbumRole()
            {
                AlbumId = albumId,
                UserId = userId,
                Role = roleAsEnum
            };

            this.context.AlbumRoles.Add(albumRole);

            this.context.SaveChanges();

            return albumRole;
        }
    }
}