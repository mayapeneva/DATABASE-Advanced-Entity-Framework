namespace Stations.DataProcessor.DTOs.Export
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("Card")]
    public class CardExportDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        public TicketExportDto[] Tickets { get; set; }
    }
}