namespace P01_BillsPaymentSystem.Data.Models.Models
{
    using Attributes;
    using Enums;
    using System.ComponentModel.DataAnnotations;

    public class PaymentMethod
    {
        public int Id { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }

        [PaymMethod(nameof(CreditCardId))]
        public int? BankAccountId { get; set; }

        public BankAccount BankAccount { get; set; }

        public int? CreditCardId { get; set; }
        public CreditCard CreditCard { get; set; }
    }
}