namespace BackendApi.Infrastructure.DTO
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? AbonementExpireDate { get; set; }
        public int SessionsLeft { get; set; } = 0;
        public string? CardNumber { get; set; } = null!;
    }
}
