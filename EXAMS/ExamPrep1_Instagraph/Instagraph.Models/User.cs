namespace Instagraph.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int Id { get; set; }

        [MaxLength(30)]
        [Required]
        public string Username { get; set; }

        [MaxLength(20)]
        [Required]
        public string Password { get; set; }

        [Required]
        public int ProfilePictureId { get; set; }

        public virtual Picture ProfilePicture { get; set; }

        public virtual ICollection<UserFollower> Followers { get; set; } = new HashSet<UserFollower>();

        public virtual ICollection<UserFollower> UsersFollowing { get; set; } = new HashSet<UserFollower>();

        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}