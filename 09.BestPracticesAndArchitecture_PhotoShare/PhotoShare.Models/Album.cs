namespace PhotoShare.Models
{
    using Models.Enums;
    using System.Collections.Generic;

    public class Album
    {
        public Album()
        {
            this.Pictures = new HashSet<Picture>();
            this.AlbumTags = new HashSet<AlbumTag>();
            this.AlbumRoles = new HashSet<AlbumRole>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public Color? BackgroundColor { get; set; }

        public bool IsPublic { get; set; }

        public virtual ICollection<AlbumRole> AlbumRoles { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }

        public virtual ICollection<AlbumTag> AlbumTags { get; set; }
    }
}