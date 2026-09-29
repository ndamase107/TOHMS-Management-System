using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TOHMS.Services.Interfaces;

namespace TOHMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public HomeController(
            IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard =
                await _dashboardService.GetDashboardAsync();

            return View(dashboard);
        }
    }
}