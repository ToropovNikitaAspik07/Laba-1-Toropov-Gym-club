namespace BackendApi.Domain.Interfaces.Services
{
    public interface ICardNumberService
    {
        Task<string> GenerateUniqueCardNumberAsync();
    }
}
