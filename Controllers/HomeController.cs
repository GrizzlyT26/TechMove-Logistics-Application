using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMove_Logistics_Application.Data;
using TechMove_Logistics_Application.ViewModels;

namespace TechMove_Logistics_Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            DashboardViewModel dashboard = new DashboardViewModel
            {
                TotalClients = await _context.Clients.CountAsync(),

                ActiveContracts = await _context.Contracts
                    .CountAsync(c => c.Status == "Active"),

                ExpiredContracts = await _context.Contracts
                    .CountAsync(c => c.Status == "Expired"),

                TotalServiceRequests = await _context.ServiceRequests
                    .CountAsync()
            };

            return View(dashboard);
        }
    }
}