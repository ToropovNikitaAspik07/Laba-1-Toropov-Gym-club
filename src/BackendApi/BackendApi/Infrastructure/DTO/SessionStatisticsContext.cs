using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BackendApi.Infrastructure.DTO
{
    public partial class SessionStatisticsContext : DbContext
    {
        private IConfiguration configuration;
        public SessionStatisticsContext(DbContextOptions<SessionStatisticsContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public virtual DbSet<SessionStatisticsDto> SessionStatistics { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(this.configuration.GetConnectionString("Toropov"));
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SessionStatisticsDto>(entity =>
            {
                entity.ToTable("SessionStatistics");
                entity.Property(e => e.StatisticsDay).HasColumnName("StatisticsDay");
                entity.Property(e => e.SessionsUsed).HasColumnName("SessionsUsed");
                entity.Property(e => e.clientsRegistered).HasColumnName("clientsRegistered");
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
