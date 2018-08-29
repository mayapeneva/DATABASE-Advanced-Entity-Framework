namespace P03_SalesDatabase.Configurations
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(80)
                .IsUnicode();

            builder.HasMany(st => st.Sales)
                .WithOne(s => s.Store)
                .HasForeignKey(s => s.StoreId);
        }
    }
}