using Microsoft.AspNetCore.Mvc;
using ValueTech.Web.Services;
using ValueTech.Api.Contracts.Requests;

namespace ValueTech.Web.Controllers
{
    public class ComunaController : Controller
    {
        private readonly IApiClient _apiClient;

        public ComunaController(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int regionId)
        {
            var comunas = await _apiClient.GetComunasByRegionAsync(regionId);
            ViewBag.RegionId = regionId;
            return View(comunas);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int regionId, int id)
        {
            var comuna = await _apiClient.GetComunaByIdAsync(regionId, id);
            if (comuna == null) return NotFound();

            var model = new UpdateComunaRequest
            {
                IdComuna = comuna.IdComuna,
                IdRegion = comuna.IdRegion,
                Nombre = comuna.Nombre,
                Superficie = comuna.Superficie,
                Poblacion = comuna.Poblacion,
                Densidad = comuna.Densidad
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int regionId, UpdateComunaRequest model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .Select(msg => msg.Contains("The value") && msg.Contains("is invalid") 
                                   ? "El tipo de dato ingresado no es válido." 
                                   : msg);
                
                TempData["ErrorMessage"] = "Datos inválidos: " + string.Join(" | ", errors);
                return View(model);
            }

            try
            {
                await _apiClient.UpdateComunaAsync(regionId, model);
                TempData["SuccessMessage"] = "El registro se ha guardado correctamente.";
                return RedirectToAction("Index", new { regionId = regionId });
            }
            catch (Exception ex)
            {
                var msg = ex.Message.Replace("Error actualizando comuna: ", "");
                TempData["ErrorMessage"] = msg;
                return View(model);
            }
        }
    }
}
