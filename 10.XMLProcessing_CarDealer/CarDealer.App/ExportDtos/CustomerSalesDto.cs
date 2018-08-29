namespace CarDealer.App.ExportDtos
{
    using System.Xml.Serialization;

    [XmlType("customer")]
    public class CustomerSalesDto
    {
        [XmlAttribute("full-name")]
        public string Name { get; set; }

        [XmlAttribute("bought-cars")]
        public int BoughtCarsCount { get; set; }

        [XmlAttribute("spent-money")]
        public decimal SpentMoney { get; set; }
    }
}