using BackendApi.Domain.Models.Auth;
using Microsoft.EntityFrameworkCore;


namespace BackendApi.Infrastructure.DTO
{
    public class UserContext : DbContext
    {
        private IConfiguration configuration;
        
        public UserContext(DbContextOptions<UserContext> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ClientDto> Clients { get; set; }
        public virtual DbSet<TrainerDto> Trainers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(this.configuration.GetConnectionString("Toropov"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("Users_pkey");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Email)
                    .HasColumnType("character varying")
                    .HasColumnName("email");
                entity.Property(e => e.PasswordHash)
                    .HasColumnType("character varying")
                    .HasColumnName("password_hash");
                entity.Property(e => e.Role).HasColumnName("role");
            });
            modelBuilder.Entity<User>()
            .HasOne(x => x.Client)
            .WithOne(x => x.User)
            .HasForeignKey<ClientDto>(x => x.UserId);

            modelBuilder.Entity<User>()
                .HasOne(x => x.Trainer)
                .WithOne(x => x.User)
                .HasForeignKey<TrainerDto>(x => x.UserId);
        }
    }
}
