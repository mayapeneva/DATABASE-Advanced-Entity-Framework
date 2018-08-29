namespace ProductShop.App.DTOs
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("user")]
    public class UserProductsDto
    {
        [XmlAttribute("first-name")]
        public string FirstName { get; set; }

        [XmlAttribute("last-name")]
        public string LastName { get; set; }

        [XmlAttribute("age")]
        public string Age { get; set; }

        [XmlElement("sold-products")]
        public Sold_ProductsDto SoldProducts { get; set; }
    }
}