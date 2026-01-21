namespace BackendApi.Infrastructure.DTO
{
    public class CreateTrainingDto
    {
        public int TrainerId { get; set; }
        public DateTime TrainingDateTime { get; set; }
        public string TrainerName { get; set; } = "";
        public string? SpecialNotes { get; set; }
        public List<Guid>? ClientIds { get; set; }
    }
}
