namespace PetClinic.DataProcessor.DTOs.Import
{
    using System.ComponentModel.DataAnnotations;

    public class PassportDto
    {
        [RegularExpression(@"^([a-zA-Z]{7}[0-9]{3})$")]
        public string SerialNumber { get; set; }

        [Required]
        [RegularExpression(@"^(\+359\d{9})|(0\d{9})$")]
        public string OwnerPhoneNumber { get; set; }

        [Required]
        [StringLength(30), MinLength(3)]
        public string OwnerName { get; set; }

        [Required]
        public string RegistrationDate { get; set; }
    }
}