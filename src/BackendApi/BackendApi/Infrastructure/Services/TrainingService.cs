using BackendApi.Domain.Interfaces.Repositories;
using BackendApi.Domain.Models;
using BackendApi.Infrastructure.DTO;

namespace BackendApi.Infrastructure.Services
{
    public class TrainingService
    {
        private readonly ITrainerRepository _trainerRepository;
        private readonly IClientRepository _clientRepository;

        public TrainingService(
            ITrainerRepository trainerRepository,
            IClientRepository clientRepository)
        {
            _trainerRepository = trainerRepository;
            _clientRepository = clientRepository;
        }

        public async Task<int> CreateTrainingAsync(CreateTrainingDto dto)
        {
            var clients = new List<Client>();

            if (dto.ClientIds != null && dto.ClientIds.Any())
            {
                clients = await _clientRepository.GetByIdsAsync(dto.ClientIds);
            }

            var training = new Training
            {
                TrainerId = dto.TrainerId,
                TrainerName = dto.TrainerName,
                TrainingDateTime = dto.TrainingDateTime,
                SpecialNotes = dto.SpecialNotes,
                Clients = clients
            };

            await _trainerRepository.AddTrainingWithClientsAsync(training);

            return training.Id;
        }
    }
}
