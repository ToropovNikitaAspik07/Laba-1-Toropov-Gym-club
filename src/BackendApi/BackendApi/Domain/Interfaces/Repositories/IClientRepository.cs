using BackendApi.Infrastructure.DTO;
namespace BackendApi.Domain.Interfaces.Repositories
{
    public interface IClientRepository
    {
        //Task<IEnumerable<Models.Client>> GetAllClientsAsync();
        Task<Models.Client?> GetClientByIdAsync(Guid id);
        //Task AddClientAsync(Models.Client client);
        //Task UpdateClientAsync(Models.Client client);
        //Task DeleteClientAsync(Guid id);
        Task<Client?> GetClientByCardNumberAsync(string cardNumber);
    }
}
