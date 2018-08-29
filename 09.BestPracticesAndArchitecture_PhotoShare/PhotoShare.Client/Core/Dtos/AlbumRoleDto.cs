namespace PhotoShare.Client.Core.Dtos
{
    using Models.Enums;

    public class AlbumRoleDto
    {
        public int AlbumId { get; set; }

        public int UserId { get; set; }

        public Role Role { get; set; }
    }
}