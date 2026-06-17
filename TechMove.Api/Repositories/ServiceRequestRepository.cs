using Microsoft.EntityFrameworkCore;
using TechMove_Logistics_Application.Data;
using TechMove_Logistics_Application.Models;
namespace TechMove_Logistics_Application.Api.Repositories;
public sealed class ServiceRequestRepository : IServiceRequestRepository
{
    private readonly ApplicationDbContext _context;
    public ServiceRequestRepository(ApplicationDbContext context) => _context = context;
    public Task<List<ServiceRequest>> GetAllAsync() => _context.ServiceRequests.Include(s => s.Contract).ToListAsync();
    public Task<ServiceRequest?> GetByIdAsync(int id) => _context.ServiceRequests.Include(s => s.Contract).FirstOrDefaultAsync(s => s.Id == id);
    public async Task<ServiceRequest> AddAsync(ServiceRequest serviceRequest) { serviceRequest.CostZAR = Math.Round(serviceRequest.CostUSD * 18.50m, 2); _context.ServiceRequests.Add(serviceRequest); await _context.SaveChangesAsync(); return serviceRequest; }
    public async Task<bool> UpdateAsync(ServiceRequest serviceRequest) { if (!await ExistsAsync(serviceRequest.Id)) return false; serviceRequest.CostZAR = Math.Round(serviceRequest.CostUSD * 18.50m, 2); _context.ServiceRequests.Update(serviceRequest); await _context.SaveChangesAsync(); return true; }
    public async Task<bool> DeleteAsync(int id) { var serviceRequest = await _context.ServiceRequests.FindAsync(id); if (serviceRequest is null) return false; _context.ServiceRequests.Remove(serviceRequest); await _context.SaveChangesAsync(); return true; }
    public Task<bool> ExistsAsync(int id) => _context.ServiceRequests.AnyAsync(s => s.Id == id);
}

