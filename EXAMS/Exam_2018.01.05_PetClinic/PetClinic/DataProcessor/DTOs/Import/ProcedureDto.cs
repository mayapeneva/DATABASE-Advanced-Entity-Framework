namespace PetClinic.DataProcessor.DTOs.Import
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Procedure")]
    public class ProcedureDto
    {
        public string Animal { get; set; }

        [Required]
        public string Vet { get; set; }

        [XmlArray("AnimalAids")]
        public List<AnimalAidDto> AnimalAids { get; set; }

        [Required]
        public string DateTime { get; set; }
    }
}