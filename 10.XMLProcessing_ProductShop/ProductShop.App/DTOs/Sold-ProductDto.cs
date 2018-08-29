namespace ProductShop.App.DTOs
{
    using System.Xml.Serialization;

    [XmlType("product")]
    public class Sold_ProductDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }
    }
}