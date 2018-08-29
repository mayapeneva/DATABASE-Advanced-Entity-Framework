namespace P01_BillsPaymentSystem.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models.Enums;
    using Models.Models;
    using System;

    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.Property(pm => pm.PaymentType)
                .HasConversion(pt => pt.ToString(),
                    pt => (PaymentType)Enum.Parse(typeof(PaymentType), pt));
        }
    }
}