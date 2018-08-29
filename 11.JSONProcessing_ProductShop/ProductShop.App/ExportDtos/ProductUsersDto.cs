namespace ProductShop.App.ExportDtos
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class ProductUsersDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}