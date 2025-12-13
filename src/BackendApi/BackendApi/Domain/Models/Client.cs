using System.ComponentModel.DataAnnotations;

namespace BackendApi.Domain.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime? AbonementExpireDate { get; set; }
        
        public int SessionsLeft { get; set; } = 0;
        public string? CardNumber { get; set; }
    }
}
