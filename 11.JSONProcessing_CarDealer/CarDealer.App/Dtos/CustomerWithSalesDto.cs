namespace CarDealer.App.Dtos
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class CustomerWithSalesDto
    {
        [JsonProperty("fullName")]
        public string Name { get; set; }

        [JsonProperty("boughtCars")]
        public int BoughtCars { get; set; }

        [JsonProperty("spentMoney")]
        public decimal SpentMoney { get; set; }
    }
}