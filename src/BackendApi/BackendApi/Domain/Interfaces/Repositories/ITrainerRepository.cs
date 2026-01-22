using BackendApi.Domain.Models;
namespace BackendApi.Domain.Interfaces.Repositories
{
    public interface ITrainerRepository
    {
        Task<List<Training>> GetTrainingsForNextWeekAsync(int trainerId);
        Task<Training?> GetTrainingWithClientsAsync(int trainingId);
        Task AddSpecialNotesAsync(int trainingId, string notes);
        Task AddTrainingWithClientsAsync(Training training);
    }
}
