namespace Models
{
    using Enums;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string UserName { get; set; }

        [MaxLength(25)]
        public string FirstName { get; set; }

        [MaxLength(25)]
        public string LastName { get; set; }

        [StringLength(30, MinimumLength = 6)]
        [RegularExpression(@"(.*[A-Z].*[0-9].*)|(.*[0-9].*[A-Z].*)")]
        public string Password { get; set; }

        public Gender Gender { get; set; }

        [Range(0, int.MaxValue)]
        public int Age { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<UserTeam> UserTeams { get; set; } = new HashSet<UserTeam>();

        public virtual ICollection<UserTeam> CreatedUserTeams { get; set; } = new HashSet<UserTeam>();

        public virtual ICollection<Event> CreatedEvents { get; set; } = new HashSet<Event>();

        public virtual ICollection<Invitation> ReceivedInvitations { get; set; } = new HashSet<Invitation>();
    }
}