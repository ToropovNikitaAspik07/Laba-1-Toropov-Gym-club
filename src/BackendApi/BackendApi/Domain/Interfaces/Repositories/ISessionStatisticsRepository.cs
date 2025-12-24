using BackendApi.Infrastructure.DTO;
namespace BackendApi.Domain.Interfaces.Repositories
{
    public interface ISessionStatisticsRepository
    {
        Task IncrementSessionsUsedAsync(DateTime day);
        Task IncrementClientsRegisteredAsync(DateTime day);

        Task<List<SessionStatistics>> GetByPeriodAsync(DateTime from, DateTime to);
    }
}
