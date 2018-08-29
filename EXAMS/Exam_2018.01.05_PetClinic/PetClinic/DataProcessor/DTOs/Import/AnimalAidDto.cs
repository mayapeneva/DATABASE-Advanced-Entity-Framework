namespace PetClinic.DataProcessor.DTOs.Import
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("AnimalAid")]
    public class AnimalAidDto
    {
        [Required]
        public string Name { get; set; }
    }
}