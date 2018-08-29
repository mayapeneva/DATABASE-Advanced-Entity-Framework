namespace FastFood.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;

    public class ItemDto
    {
        [Required]
        [StringLength(30), MinLength(3)]
        public string Name { get; set; }

        [Required]
        [StringLength(30), MinLength(3)]
        public string Category { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }
    }
}