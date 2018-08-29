namespace Instagraph.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection.Metadata;

    public class Picture
    {
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        public string Path { get; set; }

        [Required]
        public decimal Size { get; set; }

        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();

        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}