using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BackendApi.Domain.Models.Auth
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? PasswordHash { get; set; }
        public string Role { get; set; } = string.Empty;
        public Client? Client { get; set; }
        public Trainer? Trainer { get; set; }
    }
    
}
