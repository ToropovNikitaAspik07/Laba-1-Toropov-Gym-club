using System.ComponentModel.DataAnnotations;

namespace BackendApi.Domain.Models
{
    public class SessionStatistics
    {
        public Guid Id { get; set; }
        public DateTime StatisticsDay{ get; set; }
        public int SessionsUsed { get; set; } = 0;
        public int clientsRegistered { get; set; } = 0;
    }
}
