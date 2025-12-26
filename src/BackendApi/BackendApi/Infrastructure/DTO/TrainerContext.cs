using Microsoft.EntityFrameworkCore;

namespace BackendApi.Infrastructure.DTO
{
    public partial class TrainerContext : DbContext
    {
        private IConfiguration configuration;
        public TrainerContext(DbContextOptions<TrainerContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public virtual DbSet<Trainer> Trainers { get; set; } = null!;
        public virtual DbSet<Training> Trainings { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(this.configuration.GetConnectionString("Toropov"));
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trainer>(entity =>
            {
                entity.ToTable("Trainer");
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("Name");
                entity.Property(e => e.Phone)
                    .HasMaxLength(16)
                    .HasColumnName("Phone");
                entity.Property(e => e.Email).HasColumnName("Email");
                entity.Property(e => e.Description).HasColumnName("Description");
                entity.Property(e => e.IsActive).HasColumnName("IsActive");
                entity.HasMany(e => e.Trainings)
                    .WithOne(e => e.Trainer)
                    .HasForeignKey(e => e.TrainerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Training_Trainer");
            });
            modelBuilder.Entity<Training>(entity =>
            {
                entity.ToTable("Training");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.TrainingDateTime)
                    .IsRequired();

                entity.Property(e => e.SpecialNotes);

                entity.HasIndex(e => e.TrainerId);
            });

            OnModelCreatingPartial(modelBuilder);

        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        

    }
}
