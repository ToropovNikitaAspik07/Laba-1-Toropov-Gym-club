using BackendApi.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionStatisticsController : ControllerBase
    {


        private readonly ISessionStatisticsRepository _repository;

        public SessionStatisticsController(ISessionStatisticsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("day")]
        public async Task<IActionResult> GetDayStatistics(DateTime date)
        {
            var result = await _repository.GetByPeriodAsync(date, date);
            return Ok(result.FirstOrDefault());
        }
        [HttpGet("week")]
        public async Task<IActionResult> GetWeekStatistics()
        {
            var to = DateTime.UtcNow.Date;
            var from = to.AddDays(-6);
            if (from > to)
            {
                return BadRequest("Invalid date range.");
            }
            var result = await _repository.GetByPeriodAsync(from, to);
            return Ok(result);
        }
        [HttpGet("month")]
        public async Task<IActionResult> GetMonthStatistics()
        {
            var to = DateTime.UtcNow.Date;
            var from = new DateTime(to.Year, to.Month, 1);
            if (from > to)
            {
                return BadRequest("Invalid date range.");
            }
            var result = await _repository.GetByPeriodAsync(from, to);
            return Ok(result);
        }
    }




}


