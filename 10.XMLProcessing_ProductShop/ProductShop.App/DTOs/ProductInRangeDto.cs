namespace ProductShop.App.DTOs
{
    using System.Xml.Serialization;

    [XmlType("product")]
    public class ProductInRangeDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }

        [XmlAttribute("byuerName")]
        public string BuyerName { get; set; }
    }
}