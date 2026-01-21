using BackendApi.Domain.Models.Auth;
using BackendApi.Infrastructure.DTO;
using BackendApi.Infrastructure.Repositories;
using BackendApi.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Presentation.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository userRepository;
        private readonly JwtTokenService jwtTokenService;

        public AuthController(UserRepository userRepository, JwtTokenService jwtTokenService)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("Email is required");

            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Password is required");
            var existing = await userRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
                return BadRequest("User already exists");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                PasswordHash = PasswordHasher.Hash(dto.Password),
                Role = "Client"
            };

            await userRepository.AddAsync(user);

            return Ok();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("Email is required");

            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Password is required");

            var user = await userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var passwordHash = PasswordHasher.Hash(dto.Password);
            if (!string.Equals(user.PasswordHash, passwordHash, StringComparison.Ordinal))
                return Unauthorized("Invalid credentials");

            var token = jwtTokenService.GenerateToken(user);
            return Ok(token);
        }
    }
}
