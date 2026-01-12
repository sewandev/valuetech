using Microsoft.AspNetCore.Mvc;
using ValueTech.Api.Services;
using ValueTech.Api.Contracts.Responses;
using ValueTech.Api.Contracts.Requests;

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
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            string auditUser = Request.Headers.TryGetValue("X-Audit-User", out var userVal) ? userVal.ToString() : "Unknown";
            string auditIp = Request.Headers.TryGetValue("X-Audit-IP", out var ipVal) ? ipVal.ToString() : HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            await _service.DeleteAsync(id, auditUser, auditIp);
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateRegionRequest request)
        {
            string auditUser = Request.Headers.TryGetValue("X-Audit-User", out var userVal) ? userVal.ToString() : "Unknown";
            string auditIp = Request.Headers.TryGetValue("X-Audit-IP", out var ipVal) ? ipVal.ToString() : HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            var newId = await _service.CreateAsync(request, auditUser, auditIp);
            return CreatedAtAction(nameof(GetById), new { id = newId }, new { id = newId });
        }
    }
}
