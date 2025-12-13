using BackendApi.Domain.Interfaces.Services;
using BackendApi.Infrastructure.DTO;
using BackendApi.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;


namespace BackendApi.Domain.Services
{
    public class CardNumberService : ICardNumberService
    {
        private readonly ClientContext _context;
        private readonly CardNumberGenerator _cardNumberGenerator = new CardNumberGenerator();

        public CardNumberService(ClientContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateUniqueCardNumberAsync()
        {
            string code;

            do             {
                code = _cardNumberGenerator.GenerateCardNumber();
            } 
            while(await _context.Clients.AnyAsync(c => c.CardNumber == code));
            return code;
            // Генерация уникального номера карты (пример реализации)

        }
    }
}
