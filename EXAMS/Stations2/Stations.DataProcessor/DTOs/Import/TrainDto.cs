namespace Stations.DataProcessor.DTOs.Import
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TrainDto
    {
        [Required]
        [MaxLength(10)]
        public string TrainNumber { get; set; }

        public string Type { get; set; } = "HighSpeed";

        public List<TrainSeatDto> Seats { get; set; } = new List<TrainSeatDto>();
    }
}