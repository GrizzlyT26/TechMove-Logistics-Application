using Microsoft.EntityFrameworkCore;
using TechMove_Logistics_Application.Data;
using TechMove_Logistics_Application.Models;
namespace TechMove_Logistics_Application.Api.Repositories;
public sealed class ContractRepository : IContractRepository
{
    private readonly ApplicationDbContext _context;
    public ContractRepository(ApplicationDbContext context) => _context = context;
    public Task<List<Contract>> GetAllAsync(string? searchString, string? statusFilter, DateTime? startDate, DateTime? endDate)
    {
        var contracts = _context.Contracts.Include(c => c.Client).AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchString)) contracts = contracts.Where(c => c.ServiceLevel.Contains(searchString) || c.Status.Contains(searchString) || (c.Client != null && c.Client.Name.Contains(searchString)));
        if (!string.IsNullOrWhiteSpace(statusFilter)) contracts = contracts.Where(c => c.Status == statusFilter);
        if (startDate.HasValue) contracts = contracts.Where(c => c.StartDate >= startDate.Value);
        if (endDate.HasValue) contracts = contracts.Where(c => c.EndDate <= endDate.Value);
        return contracts.ToListAsync();
    }
    public Task<Contract?> GetByIdAsync(int id) => _context.Contracts.Include(c => c.Client).FirstOrDefaultAsync(c => c.Id == id);
    public async Task<Contract> AddAsync(Contract contract) { _context.Contracts.Add(contract); await _context.SaveChangesAsync(); return contract; }
    public async Task<bool> UpdateAsync(Contract contract) { if (!await ExistsAsync(contract.Id)) return false; _context.Contracts.Update(contract); await _context.SaveChangesAsync(); return true; }
    public async Task<bool> UpdateStatusAsync(int id, string status) { var contract = await _context.Contracts.FindAsync(id); if (contract is null) return false; contract.Status = status; await _context.SaveChangesAsync(); return true; }
    public async Task<bool> DeleteAsync(int id) { var contract = await _context.Contracts.FindAsync(id); if (contract is null) return false; _context.Contracts.Remove(contract); await _context.SaveChangesAsync(); return true; }
    public Task<bool> ExistsAsync(int id) => _context.Contracts.AnyAsync(c => c.Id == id);
}

