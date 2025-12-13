using BackendApi.Domain.Interfaces.Providers;

namespace BackendApi.Infrastructure.Providers
{
    public class CardNumberGenerator : ICardNumberGenerator
    {
        private static  readonly Random random = new Random();
        public string GenerateCardNumber()
        {
            
            var cardNumber = string.Empty;
            for (int i = 0; i < 16; i++)
            {
                cardNumber += random.Next(0, 10).ToString();
            }
            return cardNumber;
        }
    }
}
