namespace CarDealer.App.Dtos
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    [JsonObject("car")]
    public class CarWithPartsDto
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        public virtual List<PartForCarDto> Parts { get; set; }
    }
}