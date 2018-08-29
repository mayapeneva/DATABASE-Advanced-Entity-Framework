namespace Stations.DataProcessor.DTOs.Import
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TrainSeatDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Abbreviation { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        public int Quantity { get; set; }
    }
}