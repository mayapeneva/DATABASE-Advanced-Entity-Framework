namespace ProductShop.App.DTOs
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("sold-products")]
    public class Sold_ProductsDto
    {
        [XmlAttribute("count")]
        public int ProductsCount { get; set; }

        [XmlElement("product")]
        public List<Sold_ProductDto> Products { get; set; }
    }
}