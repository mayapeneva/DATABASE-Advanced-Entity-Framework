namespace PhotoShare.Services.Contracts
{
    using Models;

    public interface IAlbumRoleService
    {
        TModel[] ByAlbumId<TModel>(int albumId);

        AlbumRole PublishAlbumRole(int albumId, int userId, string role);
    }
}