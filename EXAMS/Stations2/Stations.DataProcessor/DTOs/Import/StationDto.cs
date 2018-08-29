namespace Stations.DataProcessor.DTOs.Import
{
    using System.ComponentModel.DataAnnotations;

    public class StationDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Town { get; set; }
    }
}