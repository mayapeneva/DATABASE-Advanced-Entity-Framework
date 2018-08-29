﻿namespace CarDealer.App.ExportDtos
{
    using System.Xml.Serialization;

    [XmlType("sale")]
    public class SaleWithDiscountDto
    {
        [XmlElement("car")]
        public CarSalesDto Car { get; set; }

        [XmlElement("customer-name")]
        public string CustomerName { get; set; }

        [XmlElement("discount")]
        public int Discount { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("price-with-discount")]
        public decimal PriceWithDiscount { get; set; }
    }
}