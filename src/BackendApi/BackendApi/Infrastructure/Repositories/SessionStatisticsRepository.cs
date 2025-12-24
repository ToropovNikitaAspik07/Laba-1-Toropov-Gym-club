using BackendApi.Domain.Interfaces.Repositories;
using BackendApi.Infrastructure.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;

namespace BackendApi.Infrastructure.Repositories
{
    public class SessionStatisticsRepository : ISessionStatisticsRepository
    {
        private readonly SessionStatisticsContext _context;
        private readonly string? _connectionString;
        private readonly IConfiguration _configuration;
       

        public SessionStatisticsRepository(SessionStatisticsContext context, IConfiguration configuration)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this._connectionString = configuration.GetConnectionString("Toropov") ?? throw new ArgumentNullException("Connection string 'Toropov' not found.");
            
        }

        public async Task ExecuteCommand(
            string sqlCommand,
            Dictionary<string, object> parameters)
        {
            if (string.IsNullOrWhiteSpace(sqlCommand))
                throw new ArgumentException("SQL command is empty");
           
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new NpgsqlCommand(sqlCommand, connection);

                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }

                await command.ExecuteNonQueryAsync();
           

        }

        public async Task IncrementSessionsUsedAsync(DateTime day)
        {
            if (day == DateTime.MinValue)
                throw new ArgumentException("Invalid date value");
            day = day.Date;
            var sql = """
        INSERT INTO "SessionStatistics"
            ("StatisticsDay", "SessionsUsed", "clientsRegistered")
        VALUES
            (@day, 1, 0)
        ON CONFLICT ("StatisticsDay")
        DO UPDATE
        SET "SessionsUsed" = "SessionStatistics"."SessionsUsed" + 1;
        """;
            try
            {
                await ExecuteCommand(
                     sql,
                     new Dictionary<string, object>
                     {
                        { "@day", day.Date }
            });
        }
            catch (Exception ex)
            {
                throw new Exception($"Failed to increment SessionsUsed for day {day:yyyy-MM-dd}", ex);
            }
            
        }

        public async Task IncrementClientsRegisteredAsync(DateTime day)
        {
            day = day.Date;
            if (day == default)
                throw new ArgumentException("Invalid date value");
            
            var stat = await _context.SessionStatistics
                .FirstOrDefaultAsync(x => x.StatisticsDay == day);

            if (stat == null)
            {
                stat = new SessionStatistics
                {
                    StatisticsDay = day,
                    SessionsUsed = 0,
                    clientsRegistered = 1
                };

                _context.SessionStatistics.Add(stat);
            }
            else
            {
                stat.clientsRegistered++;
            }

            await _context.SaveChangesAsync();
            


        }

        public async Task<List<SessionStatistics>> GetByPeriodAsync(DateTime from, DateTime to)
        {
            
                return await _context.SessionStatistics
                                .Where(x => x.StatisticsDay >= from.Date && x.StatisticsDay <= to.Date)
                                .OrderBy(x => x.StatisticsDay)
                                .ToListAsync();
            
        }
    }
}
