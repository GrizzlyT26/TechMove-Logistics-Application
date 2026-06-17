using TechMove_Logistics_Application.Models;
namespace TechMove_Logistics_Application.Api.Repositories;
public interface IServiceRequestRepository
{
    Task<List<ServiceRequest>> GetAllAsync();
    Task<ServiceRequest?> GetByIdAsync(int id);
    Task<ServiceRequest> AddAsync(ServiceRequest serviceRequest);
    Task<bool> UpdateAsync(ServiceRequest serviceRequest);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

