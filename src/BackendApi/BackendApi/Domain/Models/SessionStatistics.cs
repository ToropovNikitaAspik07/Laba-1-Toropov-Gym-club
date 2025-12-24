using System.ComponentModel.DataAnnotations;

namespace BackendApi.Domain.Models
{
    public class SessionStatistics
    {
        DateTime StatisticsDay{ get; set; }
        int SessionsUsed { get; set; } = 0;
        int clientsRegistered { get; set; } = 0;
    }
}
