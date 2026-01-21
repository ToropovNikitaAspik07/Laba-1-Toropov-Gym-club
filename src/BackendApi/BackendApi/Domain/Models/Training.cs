using BackendApi.Infrastructure.DTO;

namespace BackendApi.Domain.Models
{
    public class Training
    {
        public int Id { get; set; }
        public int TrainerId { get; set; }

        public DateTime TrainingDateTime { get; set; }
        public string TrainerName { get; set; } = "";
        public string? SpecialNotes { get; set; }
        public Trainer? Trainer { get; set; }
        public List<Client>? Clients { get; set; }
    }
}
