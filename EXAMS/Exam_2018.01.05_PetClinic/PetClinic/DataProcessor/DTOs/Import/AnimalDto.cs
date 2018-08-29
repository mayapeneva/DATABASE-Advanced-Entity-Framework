namespace PetClinic.DataProcessor.DTOs.Import
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AnimalDto
    {
        [Required]
        [StringLength(20), MinLength(3)]
        public string Name { get; set; }

        [Required]
        [StringLength(20), MinLength(3)]
        public string Type { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int Age { get; set; }

        [Required]
        public PassportDto Passport { get; set; }
    }
}