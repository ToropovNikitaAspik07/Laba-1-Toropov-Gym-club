using BackendApi.Domain.Models.Auth;

namespace BackendApi.Infrastructure.DTO
{
    public class ClientDto
    {
        public string Name { get; set; } = null!;
        public DateTime? AbonementExpireDate { get; set; }
        public int SessionsLeft { get; set; } = 0;
        public string? CardNumber { get; set; } = null!;
        public User User { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}
