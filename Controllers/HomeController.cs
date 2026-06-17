using Microsoft.AspNetCore.Mvc;
using TechMove_Logistics_Application.Services;
using TechMove_Logistics_Application.ViewModels;

namespace TechMove_Logistics_Application.Controllers;

public class HomeController : Controller
{
    private readonly TechMoveApiClient _apiClient;
    public HomeController(TechMoveApiClient apiClient) => _apiClient = apiClient;
    public async Task<IActionResult> Index() => View(await _apiClient.GetDashboardAsync() ?? new DashboardViewModel());
}