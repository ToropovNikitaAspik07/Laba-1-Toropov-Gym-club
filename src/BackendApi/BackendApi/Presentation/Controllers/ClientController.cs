using BackendApi.Domain.Interfaces.Repositories;
using BackendApi.Domain.Interfaces.Services;
using BackendApi.Infrastructure.DTO;
using BackendApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : Controller
    {
        private readonly IClientRepository clientRepository;
        
        

        public ClientController(IClientRepository repository)
        { 
            this.clientRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("bycard/{cardNumber}")]
        public async Task<IActionResult> GetClientByCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
                return BadRequest("CardNumber cannot be null or empty.");
            var client = await clientRepository.GetClientByCardNumberAsync(cardNumber);
            if (client == null)
                return NotFound($"Client with CardNumber '{cardNumber}' not found.");

            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] Client client)
        {
            if (client == null)
                return BadRequest("Client cannot be null.");
            await clientRepository.AddClientAsync(client);
            return Ok(client);
        }
        [HttpPost("use-session/{cardNumber}")]
        public async Task<IActionResult> UseSession(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
                return BadRequest("CardNumber cannot be null or empty.");
            var updated = await clientRepository.UseSessionAsync(cardNumber);
            if (updated == 0)
                return NotFound($"No sessions left or client with CardNumber '{cardNumber}' not found.");
            return Ok(new { Message = "Session used successfully.", Updated = updated });
        }
        [HttpPost("add-sessions")]
        public async Task<IActionResult> AddSessions([FromQuery] string cardNumber, [FromQuery] int sessionsToAdd)
        {
            if (string.IsNullOrEmpty(cardNumber))
                return BadRequest("CardNumber cannot be null or empty.");
            if (sessionsToAdd <= 0)
                return BadRequest("Sessions to add must be greater than zero.");
            var client = await clientRepository.GetClientByCardNumberAsync(cardNumber);
            if (client == null)
                return NotFound($"Client with CardNumber '{cardNumber}' not found.");
            client.SessionsLeft += sessionsToAdd;
            await clientRepository.AddClientAsync(client); 
            return Ok(new { Message = "Sessions added successfully.", TotalSessions = client.SessionsLeft });
        }
        [HttpPost("register-to-training")]
        public async Task<IActionResult> RegisterToTraining([FromBody] RegisterToTrainingRequest request)
        {
            if (request == null)
                return BadRequest("Request body is required");

            if (string.IsNullOrWhiteSpace(request.CardNumber))
                return BadRequest("CardNumber is required");

            if (request.TrainingId <= 0)
                return BadRequest("Invalid TrainingId");

            try
            {
                await clientRepository.RegisterClientToTrainingAsync(
                    request.CardNumber,
                    request.TrainingId);

                return Ok(new
                {
                    message = "Client successfully registered to training"
                });
            }
            catch (InvalidOperationException ex)
            {
                // бизнес-ошибки (нет сессий, не найдено и т.д.)
                return Conflict(new { error = ex.Message });
            }
        }

    }
}
