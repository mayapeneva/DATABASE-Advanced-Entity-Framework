namespace Stations.DataProcessor.DTOs.Export
{
    using System.Xml.Serialization;

    [XmlType("Ticket")]
    public class TicketExportDto
    {
        public string OriginStation { get; set; }

        public string DestinationStation { get; set; }

        public string DepartureTime { get; set; }
    }
}