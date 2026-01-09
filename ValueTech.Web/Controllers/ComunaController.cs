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
            ViewBag.RegionId = regionId; // Para volver o saber contexto
            return View(comunas);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int regionId, int id)
        {
            var comuna = await _apiClient.GetComunaByIdAsync(regionId, id);
            if (comuna == null) return NotFound();

            // Mapeamos Response -> Request para usarlo de ViewModel (Simplificación válida para prueba)
            // O idealmente crear un ComunaViewModel separado. Usaremos el Request DTO para editar.
            var model = new CreateComunaRequest
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
        public async Task<IActionResult> Edit(int regionId, CreateComunaRequest model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                await _apiClient.UpdateComunaAsync(regionId, model);
                return RedirectToAction("Index", new { regionId = regionId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error actualizando comuna: " + ex.Message);
                return View(model);
            }
        }
    }
}
