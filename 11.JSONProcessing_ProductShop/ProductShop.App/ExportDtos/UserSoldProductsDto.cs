namespace ProductShop.App.ExportDtos
{
    using Newtonsoft.Json;

    public class UserSoldProductsDto
    {
        [JsonProperty("firstname")]
        public string FirstName { get; set; }

        [JsonProperty("lastname")]
        public string LastName { get; set; }

        [JsonProperty("soldProducts")]
        public ProductSoldDto[] ProductsSold { get; set; }
    }
}