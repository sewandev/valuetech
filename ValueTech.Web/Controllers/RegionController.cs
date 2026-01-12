using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ValueTech.Web.Services;
using ValueTech.Api.Contracts.Requests;

namespace ValueTech.Web.Controllers
{
    [Authorize]
    public class RegionController : Controller
    {
        private readonly IApiClient _apiClient;

        public RegionController(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var regions = await _apiClient.GetRegionsAsync();
            return View(regions);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ValueTech.Web.Models.RegionFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ValueTech.Web.Models.RegionFormViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                 var request = new CreateRegionRequest { Nombre = model.Nombre };
                 await _apiClient.CreateRegionAsync(request);
                 TempData["SuccessMessage"] = "Región creada correctamente.";
                 return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error creando región: " + ExtractErrorMessage(ex);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRegion(int id)
        {
            try
            {
                await _apiClient.DeleteRegionAsync(id);
                TempData["SuccessMessage"] = "Región eliminada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "No se pudo eliminar la región. " + ExtractErrorMessage(ex);
            }
            return RedirectToAction(nameof(Index));
        }

        private string ExtractErrorMessage(Exception ex)
        {
            var msg = ex.Message;
            try 
            {
                if(msg.Contains("Error:")) msg = msg.Split(new[] { " - " }, 2, StringSplitOptions.None).LastOrDefault() ?? msg;
                var json = System.Text.Json.JsonDocument.Parse(msg);
                if (json.RootElement.TryGetProperty("detail", out var detail)) msg = detail.GetString() ?? msg;
            } catch { }
            return msg;
        }
    }
}
