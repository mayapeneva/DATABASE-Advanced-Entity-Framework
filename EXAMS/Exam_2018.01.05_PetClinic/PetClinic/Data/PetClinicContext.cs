namespace PetClinic.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class PetClinicContext : DbContext
    {
        public PetClinicContext()
        {
        }

        public PetClinicContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Passport> Passports { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<AnimalAid> AnimalAids { get; set; }
        public DbSet<ProcedureAnimalAid> ProceduresAnimalAids { get; set; }
        public DbSet<Vet> Vets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AnimalAid>()
                .HasAlternateKey(aa => aa.Name);

            builder.Entity<ProcedureAnimalAid>()
                .HasKey(paa => new { paa.AnimalAidId, paa.ProcedureId });

            builder.Entity<ProcedureAnimalAid>()
                .HasOne(paa => paa.AnimalAid)
                .WithMany(aa => aa.AnimalAidProcedures)
                .HasForeignKey(aa => aa.AnimalAidId);

            builder.Entity<ProcedureAnimalAid>()
                .HasOne(paa => paa.Procedure)
                .WithMany(p => p.ProcedureAnimalAids)
                .HasForeignKey(p => p.ProcedureId);

            builder.Entity<Passport>()
                .HasOne(a => a.Animal)
                .WithOne(p => p.Passport)
                .HasForeignKey<Animal>(p => p.PassportSerialNumber);
        }
    }
}