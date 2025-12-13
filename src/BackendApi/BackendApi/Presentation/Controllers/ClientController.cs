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
        private readonly ICardNumberService cardNumberService;
        private readonly ClientContext _context;

        public ClientController(ClientContext context,IClientRepository repository, ICardNumberService cardNumberService)
        { 
            this.clientRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.cardNumberService = cardNumberService ?? throw new ArgumentNullException(nameof(cardNumberService));
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /*[HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(Guid id)
        {
            var client = await clientRepository.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }*/
        [HttpGet("bycard/{cardNumber}")]
        public async Task<IActionResult> GetClientByCardNumber(string cardNumber)
        {
            var client = await clientRepository.GetClientByCardNumberAsync(cardNumber);
            if (client == null)
                return NotFound($"Client with CardNumber '{cardNumber}' not found.");

            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] Client client)
        {
            client.CardNumber = await cardNumberService.GenerateUniqueCardNumberAsync();
            client.AbonementExpireDate = DateTime.SpecifyKind((DateTime)client.AbonementExpireDate, DateTimeKind.Utc);
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return Ok(client);
        }
        //private readonly ILogger<ClientController> _ClientLogger;

    }
}
