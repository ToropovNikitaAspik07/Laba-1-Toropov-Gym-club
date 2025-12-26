using BackendApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Infrastructure.DTO
{
    public partial class ClientContext : DbContext
    {
        private IConfiguration configuration;
        public ClientContext(DbContextOptions<ClientContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public virtual DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Training> Trainings { get; set; } = null!;


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
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}
