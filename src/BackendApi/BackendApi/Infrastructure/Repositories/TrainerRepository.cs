using BackendApi.Domain.Interfaces.Repositories;
using BackendApi.Infrastructure.DTO;
using Microsoft.EntityFrameworkCore;
namespace BackendApi.Infrastructure.Repositories
{
    public class TrainerRepository : ITrainerRepository
    {
        private readonly TrainerContext _context;
        public TrainerRepository(TrainerContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<Training>> GetTrainingsForNextWeekAsync(int trainerId)
        {
            var from = DateTime.UtcNow.Date;
            var to = from.AddDays(7);
            
            return await _context.Set<Training>()
                .Include(t => t.Trainer)
                .Where(t =>
                    t.TrainerId == trainerId &&
                    t.TrainingDateTime >= from &&
                    t.TrainingDateTime < to)
                .OrderBy(t => t.TrainingDateTime)
                .AsNoTracking()
                .ToListAsync();
           
        }

        //Просмотр тренировки с клиентами
        public async Task<Training?> GetTrainingWithClientsAsync(int trainingId)
        {
            
                return await _context.Set<Training>()
                .Include(t => t.Trainer)
                .Include(t => t.Clients)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == trainingId);
            
        }

        //Добавление особых отметок (травмы, ограничения)
        public async Task AddSpecialNotesAsync(int trainingId, string notes)
        {
            if (string.IsNullOrWhiteSpace(notes))
                throw new ArgumentException("Special notes cannot be empty");

            var training = await _context.Set<Training>()
                .FirstOrDefaultAsync(t => t.Id == trainingId);

            if (training == null)
                throw new Exception("Training not found");

            training.SpecialNotes = notes;

            await _context.SaveChangesAsync();
        }

    }
}
