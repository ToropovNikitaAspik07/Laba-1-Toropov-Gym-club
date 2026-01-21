using BackendApi.Domain.Models.Auth;
using System.ComponentModel.DataAnnotations;

namespace BackendApi.Domain.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? AbonementExpireDate { get; set; }
        
        public int SessionsLeft { get; set; } = 0;
        public string? CardNumber { get; set; }
        public User User { get; set; } = null!;
        public Guid UserId { get; set; }
        public List<Training>? Trainings { get; set; }
    }
}
