namespace Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Team
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        [MaxLength(32)]
        public string Description { get; set; }

        [StringLength(3, MinimumLength = 3)]
        public string Acronym { get; set; }

        public int CreatorId { get; set; }
        public virtual User Creator { get; set; }

        public virtual ICollection<UserTeam> Memebers { get; set; } = new HashSet<UserTeam>();

        public virtual ICollection<TeamEvent> Events { get; set; } = new HashSet<TeamEvent>();

        public virtual ICollection<Invitation> Invitations { get; set; } = new HashSet<Invitation>();
    }
}