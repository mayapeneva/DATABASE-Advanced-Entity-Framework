namespace Stations.DataProcessor.DTOs.Import
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("card")]
    public class CardTicketDto
    {
        [Required]
        [XmlAttribute("Name")]
        public string Name { get; set; }
    }
}