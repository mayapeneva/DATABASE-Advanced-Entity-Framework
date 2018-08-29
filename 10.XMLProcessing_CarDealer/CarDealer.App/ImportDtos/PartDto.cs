﻿namespace CarDealer.App.ImportDtos
{
    using System.Xml.Serialization;

    [XmlType("part")]
    public class PartDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }

        [XmlAttribute("quantity")]
        public int Qunatity { get; set; }
    }
}