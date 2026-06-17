using Microsoft.AspNetCore.Mvc;
using TechMove_Logistics_Application.Api.Dtos;
using TechMove_Logistics_Application.Api.Repositories;
using TechMove_Logistics_Application.Models;

namespace TechMove_Logistics_Application.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ClientsController : ControllerBase
{
    private readonly IClientRepository _clients;

    public ClientsController(IClientRepository clients)
    {
        _clients = clients;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetClients([FromQuery] string? searchString, [FromQuery] string? regionFilter)
    {
        return Ok(await _clients.GetAllAsync(searchString, regionFilter));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Client>> GetClient(int id)
    {
        var client = await _clients.GetByIdAsync(id);
        return client is null ? NotFound() : Ok(client);
    }

    [HttpPost]
    public async Task<ActionResult<Client>> CreateClient(ClientRequest request)
    {
        var client = new Client
        {
            Name = request.Name,
            ContactDetails = request.ContactDetails,
            Region = request.Region
        };

        await _clients.AddAsync(client);
        return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateClient(int id, ClientRequest request)
    {
        var updated = await _clients.UpdateAsync(new Client
        {
            Id = id,
            Name = request.Name,
            ContactDetails = request.ContactDetails,
            Region = request.Region
        });

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        return await _clients.DeleteAsync(id) ? NoContent() : NotFound();
    }
}