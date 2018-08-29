namespace CarDealer.App.ExportDtos
{
    using System.Xml.Serialization;

    [XmlType("supplier")]
    public class SupplierLocalDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("parts-count")]
        public int PartsCount { get; set; }
    }
}