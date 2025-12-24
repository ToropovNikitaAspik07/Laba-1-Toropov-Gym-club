namespace BackendApi.Domain.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string? Phone { get; set; }
        public string? Email { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }
        public List<Training> Trainings { get; set; } = new();

    }
}
