namespace BackendApi.Infrastructure.DTO
{
    public class SessionStatisticsDto
    {
        public DateTime StatisticsDay { get; set; }
        public int SessionsUsed { get; set; } = 0;
        public int clientsRegistered { get; set; } = 0;
    }
}
