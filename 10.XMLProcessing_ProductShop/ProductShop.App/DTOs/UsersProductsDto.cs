namespace ProductShop.App.DTOs
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("users")]
    public class UsersProductsDto
    {
        [XmlAttribute("count")]
        public int UsersCount { get; set; }

        [XmlElement("user")]
        public List<UserProductsDto> Users { get; set; }
    }
}