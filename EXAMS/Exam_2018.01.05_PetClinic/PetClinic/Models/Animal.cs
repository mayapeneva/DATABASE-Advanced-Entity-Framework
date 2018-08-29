namespace PetClinic.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Animal
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20), MinLength(3)]
        public string Name { get; set; }

        [Required]
        [StringLength(20), MinLength(3)]
        public string Type { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int Age { get; set; }

        public string PassportSerialNumber { get; set; }

        [Required]
        public Passport Passport { get; set; }

        public ICollection<Procedure> Procedures { get; set; } = new HashSet<Procedure>();
    }
}