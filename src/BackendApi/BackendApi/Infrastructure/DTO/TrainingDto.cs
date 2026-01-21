using BackendApi.Domain.Models;

namespace BackendApi.Infrastructure.DTO
{
    public class TrainingDto
    {
        public int Id { get; set; }

        public DateTime TrainingDateTime { get; set; }
        public string? SpecialNotes { get; set; }
        public int TrainerId { get; set; }
        public virtual TrainerDto? Trainer { get; set; }
        public virtual List<Client>? Clients { get; set; }
    }
}
