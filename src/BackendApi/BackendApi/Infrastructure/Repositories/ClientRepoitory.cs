using BackendApi.Domain.Interfaces.Repositories;
using BackendApi.Domain.Services;
using BackendApi.Infrastructure.DTO;
using BackendApi.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;
using Npgsql;


namespace BackendApi.Infrastructure.Repositories
{
    public class ClientRepoitory : IClientRepository
    {
        private readonly ClientContext _context;
        private readonly ISessionStatisticsRepository _sessionStatisticsRepository;
       
        

        public ClientRepoitory(ClientContext context,IConfiguration configuration, ISessionStatisticsRepository sessionStatisticsRepository)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            this._sessionStatisticsRepository = sessionStatisticsRepository ?? throw new ArgumentNullException(nameof(sessionStatisticsRepository));
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        
        public async Task<int> UseSessionAsync(string cardNumber) {
                if (string.IsNullOrEmpty(cardNumber))
                {
                    throw new ArgumentNullException(nameof(cardNumber));
                }
                
                await _sessionStatisticsRepository
                    .IncrementSessionsUsedAsync(DateTime.UtcNow);
                return await _context.Clients
                        .Where(c => c.CardNumber == cardNumber && c.SessionsLeft > 0)
                        .ExecuteUpdateAsync(c => c
                            .SetProperty(
                                client => client.SessionsLeft,
                                client => client.SessionsLeft - 1)
                        );
                    
                
        }
        public async Task<Client?> GetClientByCardNumberAsync(string cardNumber)
        {
            return await _context.Clients
                .FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
        }
        
        public async Task<int> UpdateSessionNumber(string cardNumber, int sessionsToAdd)
        {
            if(string.IsNullOrEmpty(cardNumber))
            {
                throw new ArgumentNullException(nameof(cardNumber));
            }
            
                return await _context.Clients
                    .Where(c => c.CardNumber == cardNumber)
                    .ExecuteUpdateAsync(c => c
                        .SetProperty(
                            client => client.SessionsLeft,
                            client => client.SessionsLeft + sessionsToAdd)
                    );
           
        }
        public async Task AddClientAsync(Client client)
        {
            if( client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            client.CardNumber = await new CardNumberService(_context).GenerateUniqueCardNumberAsync();
            client.AbonementExpireDate = DateTime.SpecifyKind((DateTime)client.AbonementExpireDate, DateTimeKind.Utc);
            _context.Clients.Add(client);
            await _sessionStatisticsRepository
                .IncrementClientsRegisteredAsync(DateTime.UtcNow);
            await _context.SaveChangesAsync();
        }
        public async Task RegisterClientToTrainingAsync(string cardNumber, int trainingId)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                throw new ArgumentNullException(nameof(cardNumber));

            await using var transaction = await _context.Database.BeginTransactionAsync();

            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.CardNumber == cardNumber);

            if (client == null)
                throw new InvalidOperationException("Client not found");

            if (client.SessionsLeft <= 0)
                throw new InvalidOperationException("No sessions left");

            var training = await _context.Trainings
                .Include(t => t.Clients)
                .FirstOrDefaultAsync(t => t.Id == trainingId);

            if (training == null)
                throw new InvalidOperationException("Training not found");

            if (training.Clients.Any(c => c.Id == client.Id))
                throw new InvalidOperationException("Client already registered for this training");

            // добавляем клиента в тренировку
            training.Clients.Add(client);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }



    }
}
