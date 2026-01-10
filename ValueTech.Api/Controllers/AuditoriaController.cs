using Microsoft.AspNetCore.Mvc;
using ValueTech.Data.Interfaces;

namespace ValueTech.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriaController : ControllerBase
    {
        private readonly IAuditoriaRepository _repository;

        public AuditoriaController(IAuditoriaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _repository.GetAllAsync();
            return Ok(logs);
        }
    }
}
