using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ValueTech.Web.Services;
using ValueTech.Api.Contracts.Requests;

namespace ValueTech.Web.Controllers
{
    [Authorize]
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
        public IActionResult Create(int regionId)
        {
            var model = new ValueTech.Web.Models.ComunaFormViewModel
            {
                IdRegion = regionId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ValueTech.Web.Models.ComunaFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var request = new ValueTech.Api.Contracts.Requests.CreateComunaRequest
                {
                    IdRegion = model.IdRegion,
                    Nombre = model.Nombre,
                    Superficie = model.Superficie,
                    Poblacion = model.Poblacion,
                    Densidad = model.Densidad
                };

                await _apiClient.CreateComunaAsync(request);
                TempData["SuccessMessage"] = "Comuna creada correctamente.";
                return RedirectToAction("Index", new { regionId = model.IdRegion });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "No se pudo crear la comuna. " + ExtractErrorMessage(ex);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int regionId, int id)
        {
            var comuna = await _apiClient.GetComunaByIdAsync(regionId, id);
            if (comuna == null) return NotFound();

            var model = new ValueTech.Web.Models.ComunaFormViewModel
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
        public async Task<IActionResult> Edit(int regionId, ValueTech.Web.Models.ComunaFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                 // Custom error mapping if needed, or rely on asp-validation-summary
                return View(model);
            }

            try
            {
                var request = new ValueTech.Api.Contracts.Requests.UpdateComunaRequest
                {
                    IdComuna = model.IdComuna,
                    IdRegion = model.IdRegion,
                    Nombre = model.Nombre,
                    Superficie = model.Superficie,
                    Poblacion = model.Poblacion,
                    Densidad = model.Densidad
                };

                await _apiClient.UpdateComunaAsync(regionId, request);
                TempData["SuccessMessage"] = "El registro se ha guardado correctamente.";
                return RedirectToAction("Index", new { regionId = regionId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ExtractErrorMessage(ex);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComuna(int regionId, int id)
        {
            try
            {
                await _apiClient.DeleteComunaAsync(regionId, id);
                TempData["SuccessMessage"] = "Comuna eliminada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "No se pudo eliminar la comuna. " + ExtractErrorMessage(ex);
            }
            return RedirectToAction("Index", new { regionId = regionId });
        }

        private string ExtractErrorMessage(Exception ex)
        {
            var msg = ex.Message;
            try 
            {
                if(msg.Contains("Error:")) msg = msg.Split(new[] { " - " }, 2, StringSplitOptions.None).LastOrDefault() ?? msg;
                var json = System.Text.Json.JsonDocument.Parse(msg);
                if (json.RootElement.TryGetProperty("detail", out var detail)) msg = detail.GetString() ?? msg;
                if (json.RootElement.TryGetProperty("error", out var error)) msg = error.GetString() ?? msg;
            } catch { }
            return msg;
        }
    }
}
