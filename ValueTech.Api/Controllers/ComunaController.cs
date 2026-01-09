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

        [HttpPost]
        public async Task<IActionResult> Update(int regionId, [FromBody] CreateComunaRequest request)
        {
            if (request.IdRegion != regionId)
            {
                return BadRequest($"El ID de región en la URL ({regionId}) no coincide con el cuerpo ({request.IdRegion}).");
            }
            
            Console.WriteLine($"[API Update] RegionId: {regionId}, Request RegionId: {request.IdRegion}, ComunaId: {request.IdComuna}");

            await _service.UpdateAsync(request.IdComuna, request);

            return Ok();
        }
    }
}
