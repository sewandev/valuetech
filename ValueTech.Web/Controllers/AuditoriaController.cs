using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ValueTech.Web.Services;
using ValueTech.Data.Models; // Or use a DTO if strictly separated, reusing Model for speed

namespace ValueTech.Web.Controllers
{
    [Authorize]
    public class AuditoriaController : Controller
    {
        private readonly IApiClient _apiClient;

        public AuditoriaController(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await _apiClient.GetAuditLogsAsync();
            return View(logs);
        }
    }
}
