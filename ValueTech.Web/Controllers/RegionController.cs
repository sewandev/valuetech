using Microsoft.AspNetCore.Mvc;
using ValueTech.Web.Services;

namespace ValueTech.Web.Controllers
{
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
    }
}
