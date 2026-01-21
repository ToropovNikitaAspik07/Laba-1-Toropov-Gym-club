using System.ComponentModel.DataAnnotations;

namespace BackendApi.Infrastructure.DTO
{
    public class Register
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
