using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMove_Logistics_Application.Data;
using TechMove_Logistics_Application.ViewModels;
namespace TechMove_Logistics_Application.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public sealed class DashboardController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public DashboardController(ApplicationDbContext context) => _context = context;
    [HttpGet]
    public async Task<ActionResult<DashboardViewModel>> GetDashboard() => Ok(new DashboardViewModel { TotalClients = await _context.Clients.CountAsync(), ActiveContracts = await _context.Contracts.CountAsync(c => c.Status == "Active"), ExpiredContracts = await _context.Contracts.CountAsync(c => c.Status == "Expired"), TotalServiceRequests = await _context.ServiceRequests.CountAsync() });
}

