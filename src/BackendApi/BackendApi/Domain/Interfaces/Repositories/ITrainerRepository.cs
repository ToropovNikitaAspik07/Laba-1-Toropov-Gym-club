using BackendApi.Infrastructure.DTO;
namespace BackendApi.Domain.Interfaces.Repositories
{
    public interface ITrainerRepository
    {
        Task<List<Training>> GetTrainingsForNextWeekAsync(int trainerId);
        Task<Training?> GetTrainingWithClientsAsync(int trainingId);
        Task AddSpecialNotesAsync(int trainingId, string notes);
    }
}
