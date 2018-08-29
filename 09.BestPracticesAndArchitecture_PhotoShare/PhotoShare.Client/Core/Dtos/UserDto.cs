namespace PhotoShare.Client.Core.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Password { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsLogged { get; set; }
    }
}