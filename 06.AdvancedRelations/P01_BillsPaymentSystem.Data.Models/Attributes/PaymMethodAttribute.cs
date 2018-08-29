namespace P01_BillsPaymentSystem.Data.Models.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property)]
    public class PaymMethodAttribute : ValidationAttribute
    {
        private string targetAttribute;

        public PaymMethodAttribute(string targetAttribute)
        {
            this.targetAttribute = targetAttribute;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var targetAttr = context.ObjectType.GetProperty(this.targetAttribute)
                .GetValue(context.ObjectInstance);

            if ((targetAttr == null && value != null)
                || (targetAttr != null && value == null))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("One of the two properties must be null!");
        }
    }
}