using Microsoft.AspNetCore.Mvc;
using TechMove_Logistics_Application.Api.Dtos;
using TechMove_Logistics_Application.Api.Repositories;
using TechMove_Logistics_Application.Models;
namespace TechMove_Logistics_Application.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public sealed class ContractsController : ControllerBase
{
    private readonly IContractRepository _contracts;
    public ContractsController(IContractRepository contracts) => _contracts = contracts;
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contract>>> GetContracts([FromQuery] string? searchString, [FromQuery] string? statusFilter, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate) => Ok(await _contracts.GetAllAsync(searchString, statusFilter, startDate, endDate));
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Contract>> GetContract(int id) { var contract = await _contracts.GetByIdAsync(id); return contract is null ? NotFound() : Ok(contract); }
    [HttpPost]
    public async Task<ActionResult<Contract>> CreateContract(ContractRequest request) { var contract = new Contract { ClientId = request.ClientId, StartDate = request.StartDate, EndDate = request.EndDate, Status = request.Status, ServiceLevel = request.ServiceLevel, AgreementFilePath = request.AgreementFilePath }; await _contracts.AddAsync(contract); return CreatedAtAction(nameof(GetContract), new { id = contract.Id }, contract); }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateContract(int id, ContractRequest request) => await _contracts.UpdateAsync(new Contract { Id = id, ClientId = request.ClientId, StartDate = request.StartDate, EndDate = request.EndDate, Status = request.Status, ServiceLevel = request.ServiceLevel, AgreementFilePath = request.AgreementFilePath }) ? NoContent() : NotFound();
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, ContractStatusUpdateRequest request) { if (string.IsNullOrWhiteSpace(request.Status)) return BadRequest("Status is required."); return await _contracts.UpdateStatusAsync(id, request.Status) ? NoContent() : NotFound(); }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteContract(int id) => await _contracts.DeleteAsync(id) ? NoContent() : NotFound();
}

