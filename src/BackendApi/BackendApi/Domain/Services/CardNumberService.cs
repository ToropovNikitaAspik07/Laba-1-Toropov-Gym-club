using BackendApi.Domain.Interfaces.Services;
using BackendApi.Domain.Interfaces.Providers;
using BackendApi.Infrastructure.DTO;
using BackendApi.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;


namespace BackendApi.Domain.Services
{
    public class CardNumberService : ICardNumberService
    {
        private readonly AppDbContext _context;
        private readonly ICardNumberGenerator _cardNumberGenerator;

        public CardNumberService(AppDbContext context, ICardNumberGenerator cardNumberGenerator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _cardNumberGenerator = cardNumberGenerator ?? throw new ArgumentNullException(nameof(cardNumberGenerator));
        }

        public async Task<string> GenerateUniqueCardNumberAsync()
        {
            string code;
            int attempts = 0;
            do {
                code = _cardNumberGenerator.GenerateCardNumber();
                attempts++;
                if (attempts > 100)
                {
                    throw new Exception("Unable to generate a unique card number after 100 attempts.");
                }
            } 
            while(await _context.Clients.AnyAsync(c => c.CardNumber == code));
            return code;
            

        }
    }
}
