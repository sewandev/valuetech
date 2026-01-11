using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ValueTech.Web.Services;

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
                TempData["ErrorMessage"] = "No se pudo eliminar la región. " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
