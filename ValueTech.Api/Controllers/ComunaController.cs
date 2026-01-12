using Microsoft.AspNetCore.Mvc;
using ValueTech.Api.Services;
using ValueTech.Api.Contracts.Responses;
using ValueTech.Api.Contracts.Requests;

namespace ValueTech.Api.Controllers
{
    [ApiController]
    [Route("api/region/{regionId}/comuna")]
    public class ComunaController : ControllerBase
    {
        private readonly IComunaService _service;

        public ComunaController(IComunaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComunaResponse>>> GetByRegion(int regionId)
        {
            var comunas = await _service.GetByRegionIdAsync(regionId);
            return Ok(comunas);
        }

        [HttpGet("{idComuna}")]
        public async Task<ActionResult<ComunaResponse>> GetById(int regionId, int idComuna)
        {
            var comuna = await _service.GetByIdAsync(idComuna);
            
            if (comuna == null) return NotFound();
            if (comuna.IdRegion != regionId) return BadRequest("La comuna no pertenece a la región especificada.");

            return Ok(comuna);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateComunaRequest request)
        {
            if (request.IdComuna != id)
            {
                return BadRequest("ID mismatch");
            }

            string auditUser = Request.Headers.TryGetValue("X-Audit-User", out var userVal) ? userVal.ToString() : "Unknown";
            string auditIp = Request.Headers.TryGetValue("X-Audit-IP", out var ipVal) ? ipVal.ToString() : HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            await _service.UpdateAsync(id, request, auditUser, auditIp);
            return NoContent();
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
        public async Task<ActionResult<int>> Create(int regionId, [FromBody] CreateComunaRequest request)
        {
            if (regionId != request.IdRegion) return BadRequest("Region ID mismatch");

            string auditUser = Request.Headers.TryGetValue("X-Audit-User", out var userVal) ? userVal.ToString() : "Unknown";
            string auditIp = Request.Headers.TryGetValue("X-Audit-IP", out var ipVal) ? ipVal.ToString() : HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            var newId = await _service.CreateAsync(request, auditUser, auditIp);
            return CreatedAtAction(nameof(GetById), new { regionId = regionId, idComuna = newId }, new { idComuna = newId });
        }
    }
}
