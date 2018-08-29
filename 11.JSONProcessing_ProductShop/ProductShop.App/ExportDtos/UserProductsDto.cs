namespace ProductShop.App.ExportDtos
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class UserProductsDto
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("soldProducts")]
        public ProductsUsersDto SoldProducts { get; set; }
    }
}