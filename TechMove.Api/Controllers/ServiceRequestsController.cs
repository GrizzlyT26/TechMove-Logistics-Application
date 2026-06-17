using Microsoft.AspNetCore.Mvc;
using TechMove_Logistics_Application.Api.Dtos;
using TechMove_Logistics_Application.Api.Repositories;
using TechMove_Logistics_Application.Models;
namespace TechMove_Logistics_Application.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public sealed class ServiceRequestsController : ControllerBase
{
    private readonly IServiceRequestRepository _serviceRequests;
    public ServiceRequestsController(IServiceRequestRepository serviceRequests) => _serviceRequests = serviceRequests;
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetServiceRequests() => Ok(await _serviceRequests.GetAllAsync());
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceRequest>> GetServiceRequest(int id) { var serviceRequest = await _serviceRequests.GetByIdAsync(id); return serviceRequest is null ? NotFound() : Ok(serviceRequest); }
    [HttpPost]
    public async Task<ActionResult<ServiceRequest>> CreateServiceRequest(ServiceRequestRequest request) { var serviceRequest = new ServiceRequest { ContractId = request.ContractId, Description = request.Description, CostUSD = request.CostUSD, CostZAR = request.CostZAR, Status = request.Status }; await _serviceRequests.AddAsync(serviceRequest); return CreatedAtAction(nameof(GetServiceRequest), new { id = serviceRequest.Id }, serviceRequest); }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateServiceRequest(int id, ServiceRequestRequest request) => await _serviceRequests.UpdateAsync(new ServiceRequest { Id = id, ContractId = request.ContractId, Description = request.Description, CostUSD = request.CostUSD, CostZAR = request.CostZAR, Status = request.Status }) ? NoContent() : NotFound();
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteServiceRequest(int id) => await _serviceRequests.DeleteAsync(id) ? NoContent() : NotFound();
}

