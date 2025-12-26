namespace BackendApi.Infrastructure.DTO
{
    public class RegisterToTrainingRequest
    {
        public string CardNumber { get; set; } = null!;
        public int TrainingId { get; set; }
    }
}
