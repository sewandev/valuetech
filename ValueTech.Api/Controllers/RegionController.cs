using Microsoft.AspNetCore.Mvc;
using ValueTech.Api.Services;
using ValueTech.Api.Contracts.Responses;

namespace ValueTech.Api.Controllers
{
    [ApiController]
    [Route("api/region")]
    public class RegionController : ControllerBase
    {
        private readonly IRegionService _service;

        public RegionController(IRegionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegionResponse>>> GetAll()
        {
            var regions = await _service.GetAllAsync();
            return Ok(regions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RegionResponse>> GetById(int id)
        {
            if (id <= 0) return BadRequest("El ID de región debe ser positivo."); // Validación 2.2

            var region = await _service.GetByIdAsync(id);
            
            if (region == null) return NotFound();
            
            return Ok(region);
        }
    }
}
