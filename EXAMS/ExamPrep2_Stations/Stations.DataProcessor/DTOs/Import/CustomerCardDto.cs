using System.Xml.Serialization;

namespace Stations.DataProcessor.DTOs.Import
{
    using System.ComponentModel.DataAnnotations;

    [XmlType("Card")]
    public class CustomerCardDto
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [Range(0, 120)]
        public int Age { get; set; }

        public string CardType { get; set; } = "Normal";
    }
}