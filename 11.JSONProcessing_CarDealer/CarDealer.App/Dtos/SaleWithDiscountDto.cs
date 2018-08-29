namespace CarDealer.App.Dtos
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class SaleWithDiscountDto
    {
        [JsonProperty("car")]
        public CarSaleDto Car { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        public int Discount { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("priceWithDiscount")]
        public decimal PriceWithDiscount { get; set; }
    }
}