using TechMove_Logistics_Application.Models;

namespace TechMove_Logistics_Application.Api.Repositories;

public interface IClientRepository
{
    Task<List<Client>> GetAllAsync(string? searchString, string? regionFilter);
    Task<Client?> GetByIdAsync(int id);
    Task<Client> AddAsync(Client client);
    Task<bool> UpdateAsync(Client client);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}