namespace ProductShop.App.ExportDtos
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class ProductsUsersDto
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("products")]
        public ProductUsersDto[] Products { get; set; }
    }
}