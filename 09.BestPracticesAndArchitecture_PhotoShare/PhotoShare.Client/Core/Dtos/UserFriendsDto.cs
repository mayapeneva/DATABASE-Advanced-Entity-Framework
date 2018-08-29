namespace PhotoShare.Client.Core.Dtos
{
    using System.Collections.Generic;

    public class UserFriendsDto
    {
        public int Id { get; set; }

        public bool IsLogged { get; set; }

        public ICollection<FriendDto> Friends { get; set; }
    }
}