using TechMove_Logistics_Application.Models;
namespace TechMove_Logistics_Application.Api.Repositories;
public interface IContractRepository
{
    Task<List<Contract>> GetAllAsync(string? searchString, string? statusFilter, DateTime? startDate, DateTime? endDate);
    Task<Contract?> GetByIdAsync(int id);
    Task<Contract> AddAsync(Contract contract);
    Task<bool> UpdateAsync(Contract contract);
    Task<bool> UpdateStatusAsync(int id, string status);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

