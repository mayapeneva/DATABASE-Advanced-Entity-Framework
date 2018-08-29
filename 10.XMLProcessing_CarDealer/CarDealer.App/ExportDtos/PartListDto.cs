namespace CarDealer.App.ExportDtos
{
    using System.Xml.Serialization;

    [XmlType("part")]
    public class PartListDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }
    }
}