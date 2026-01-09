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
            var regions = await _service.GetAllAsync();
            var region = regions.FirstOrDefault(r => r.IdRegion == id);
            
            if (region == null) return NotFound();
            
            return Ok(region);
        }
    }
}
