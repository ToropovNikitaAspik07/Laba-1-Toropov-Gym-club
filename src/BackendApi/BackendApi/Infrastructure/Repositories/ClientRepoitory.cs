using BackendApi.Domain.Interfaces.Repositories;
using BackendApi.Domain.Services;
using BackendApi.Infrastructure.DTO;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;

namespace BackendApi.Infrastructure.Repositories
{
    public class ClientRepoitory : IClientRepository
    {
        private readonly string? connectionString;
        private readonly ClientContext _context;
        const string selectCommand = "SELECT \"Id\", \"Name\", \"AbonementExpireDate\", \"SessionsLeft\" FROM \"Client\" WHERE \"Id\" = @Id";
        const string insertCommand = "INSERT INTO \"Client\"(\"Name\",\"AbonementExpireDate\",\"SessionsLeft\", \"CardNumber\" VALUES ({0}, {1}, {2}, {3}, {4}))";

        public ClientRepoitory(ClientContext context,IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            this.connectionString = configuration.GetConnectionString("Toropov");
        }

        
        private async Task<IEnumerable<Client>> GetDataByQueryAsync(string query)
        {
            var result = new List<Client>();
            using (var connection = new Npgsql.NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new Npgsql.NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var client = new Client
                            {
                                Id = reader.GetGuid(0),
                                Name = reader.GetString(1),
                                AbonementExpireDate = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                                SessionsLeft = reader.GetInt32(3)
                            };
                            result.Add(client);
                        }
                    }
                }
            }
            return result;
        }
        public async Task<Client?> GetClientByIdAsync(Guid id)
        {
            return (await GetDataByQueryAsync($"{selectCommand.Replace("@Id", $"'{id}'")}")).SingleOrDefault();
        }

        public async Task ExecuteCommand(string sqlCommand)
        {
            using (var connection = new NpgsqlConnection(this.connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sqlCommand;

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<Client?> GetClientByCardNumberAsync(string cardNumber)
        {
            return await _context.Clients
                .FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
        }
        
        public async Task AddClientAsync(Client client)
        {
            if( client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            client.CardNumber = await new CardNumberService(_context).GenerateUniqueCardNumberAsync();

            await this.ExecuteCommand(string.Format(insertCommand,
                client.Name,
                client.AbonementExpireDate,
                client.SessionsLeft,
                client.CardNumber
                ));
        }
        
        Task<Domain.Models.Client?> IClientRepository.GetClientByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        
    }
}
