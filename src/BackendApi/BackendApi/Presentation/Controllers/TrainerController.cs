using BackendApi.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerController : ControllerBase
    {
        private readonly ITrainerRepository _repository;
        public TrainerController(ITrainerRepository trainerRepository)
        {
            this._repository = trainerRepository ?? throw new ArgumentNullException(nameof(trainerRepository)); 
        }
        [HttpGet("next-week/{trainerId}")]
        public async Task<IActionResult> GetTrainingsForNextWeek(int trainerId)
        {
            var trainings = await _repository.GetTrainingsForNextWeekAsync(trainerId);
            return Ok(trainings);
        }
        [HttpGet("training-with-clients/{trainingId}")]
        public async Task<IActionResult> GetTrainingWithClients(int trainingId)
        {
            var training = await _repository.GetTrainingWithClientsAsync(trainingId);
            if (training == null)
            {
                return NotFound($"Training with ID '{trainingId}' not found.");
            }
            return Ok(training);
        }
        [HttpPost("add-special-notes/{trainingId}")]
        public async Task<IActionResult> AddSpecialNotes(int trainingId, [FromBody] string notes)
        {
            if (string.IsNullOrWhiteSpace(notes))
            {
                return BadRequest("Special notes cannot be empty.");
            }
            try
            {
                await _repository.AddSpecialNotesAsync(trainingId, notes);
                return Ok(new { Message = "Special notes added successfully." });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
