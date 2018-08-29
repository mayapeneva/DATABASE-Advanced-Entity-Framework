namespace ProductShop.App.ExportDtos
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class UsersProductsDto
    {
        [JsonProperty("usersCount")]
        public int UsersCount { get; set; }

        [JsonProperty("users")]
        public UserProductsDto[] Users { get; set; }
    }
}