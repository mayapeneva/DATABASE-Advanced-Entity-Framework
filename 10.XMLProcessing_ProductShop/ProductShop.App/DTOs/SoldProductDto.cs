﻿namespace ProductShop.App.DTOs
{
    using System.Xml.Serialization;

    [XmlType("product")]
    public class SoldProductDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}