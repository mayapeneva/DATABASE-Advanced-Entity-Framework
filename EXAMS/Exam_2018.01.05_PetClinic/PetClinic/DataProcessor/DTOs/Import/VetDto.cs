﻿namespace PetClinic.DataProcessor.DTOs.Import
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Vet")]
    public class VetDto
    {
        [Required]
        [StringLength(40), MinLength(3)]
        public string Name { get; set; }

        [Required]
        [StringLength(50), MinLength(3)]
        public string Profession { get; set; }

        [Required]
        [Range(22, 65)]
        public int Age { get; set; }

        [Required]
        [RegularExpression(@"^(\+359\d{9})|(0\d{9})$")]
        public string PhoneNumber { get; set; }
    }
}