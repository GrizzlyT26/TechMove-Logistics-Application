using Microsoft.EntityFrameworkCore;
using TechMove_Logistics_Application.Data;
using TechMove_Logistics_Application.Models;
namespace TechMove_Logistics_Application.Api.Repositories;
public sealed class ClientRepository : IClientRepository
{
    private readonly ApplicationDbContext _context;
    public ClientRepository(ApplicationDbContext context) => _context = context;
    public Task<List<Client>> GetAllAsync(string? searchString, string? regionFilter)
    {
        var clients = _context.Clients.AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchString)) clients = clients.Where(c => c.Name.Contains(searchString));
        if (!string.IsNullOrWhiteSpace(regionFilter)) clients = clients.Where(c => c.Region == regionFilter);
        return clients.ToListAsync();
    }
    public Task<Client?> GetByIdAsync(int id) => _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
    public async Task<Client> AddAsync(Client client) { _context.Clients.Add(client); await _context.SaveChangesAsync(); return client; }
    public async Task<bool> UpdateAsync(Client client) { if (!await ExistsAsync(client.Id)) return false; _context.Clients.Update(client); await _context.SaveChangesAsync(); return true; }
    public async Task<bool> DeleteAsync(int id) { var client = await _context.Clients.FindAsync(id); if (client is null) return false; _context.Clients.Remove(client); await _context.SaveChangesAsync(); return true; }
    public Task<bool> ExistsAsync(int id) => _context.Clients.AnyAsync(c => c.Id == id);
}

