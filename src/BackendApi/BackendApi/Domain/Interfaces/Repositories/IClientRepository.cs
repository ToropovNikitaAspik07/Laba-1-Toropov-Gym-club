using BackendApi.Domain.Models;
namespace BackendApi.Domain.Interfaces.Repositories
{
    public interface IClientRepository
    {
        Task AddClientAsync(Client client);
        Task<Client?> GetClientByCardNumberAsync(string cardNumber);
        Task<int> UseSessionAsync(string cardNumber);
        Task RegisterClientToTrainingAsync(string cardNumber, int trainingId);
        Task<List<Client>> GetByIdsAsync(List<Guid> ids);
    }
}
