using BackendApi.Domain.Models;
using BackendApi.Domain.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Infrastructure.DTO
{
    public partial class AppDbContext : DbContext
    {
        private IConfiguration configuration;
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public virtual DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Training> Trainings { get; set; } = null!;
        public virtual DbSet<Trainer> Trainers { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<SessionStatistics> SessionStatistics { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(this.configuration.GetConnectionString("Toropov"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client");
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("Name");
                entity.Property(e => e.AbonementExpireDate).HasColumnName("AbonementExpireDate");
                entity.Property(e => e.SessionsLeft).HasColumnName("SessionsLeft");
                entity.Property(e => e.CardNumber)
                    .HasMaxLength(50)
                    .HasColumnName("CardNumber");
            });
            modelBuilder.Entity<SessionStatistics>(entity =>
            {
                entity.ToTable("SessionStatistics");
                entity.HasKey(e => e.Id).HasName("Id");
                entity.Property(e => e.StatisticsDay).HasColumnName("StatisticsDay");
                entity.Property(e => e.SessionsUsed).HasColumnName("SessionsUsed");
                entity.Property(e => e.clientsRegistered).HasColumnName("clientsRegistered");
            });
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
                    .HasConstraintName("TrainerId");
            });
            modelBuilder.Entity<Training>(entity =>
            {
                entity.ToTable("Training");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.TrainingDateTime)
                    .IsRequired();

                entity.Property(e => e.SpecialNotes);

                entity.HasIndex(e => e.TrainerId);

                entity.HasMany(e => e.Clients)
                    .WithMany(c => c.Trainings)
                    .UsingEntity<Dictionary<string, object>>(
                        "ClientTraining",
                        j => j
                            .HasOne<Client>()
                            .WithMany()
                            .HasForeignKey("ClientId")
                            .HasPrincipalKey(c => c.Id)
                            .OnDelete(DeleteBehavior.Cascade),
                        j => j
                            .HasOne<Training>()
                            .WithMany()
                            .HasForeignKey("TrainingId")
                            .HasPrincipalKey(t => t.Id)
                            .OnDelete(DeleteBehavior.Cascade),
                        j =>
                        {
                            j.ToTable("ClientTraining");
                            j.HasKey("ClientId", "TrainingId");
                        });
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(e => e.Id).HasName("Id");
                entity.Property(e => e.Email)
                    .HasColumnName("Email");
                entity.Property(e => e.PasswordHash)
                    .HasColumnName("PasswordHash");
                entity.Property(e => e.Role).HasColumnName("Role");
            });
            modelBuilder.Entity<User>()
            .HasOne(x => x.Client)
            .WithOne(x => x.User)
            .HasForeignKey<Client>(x => x.UserId);

            modelBuilder.Entity<User>()
                .HasOne(x => x.Trainer)
                .WithOne(x => x.User)
                .HasForeignKey<Trainer>(x => x.UserId);
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}
