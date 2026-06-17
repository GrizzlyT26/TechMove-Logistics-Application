using Microsoft.AspNetCore.Mvc;
using TechMove_Logistics_Application.Models;
using TechMove_Logistics_Application.Services;

namespace TechMove_Logistics_Application.Controllers;

public class ClientsController : Controller
{
    private readonly TechMoveApiClient _apiClient;
    public ClientsController(TechMoveApiClient apiClient) => _apiClient = apiClient;
    public async Task<IActionResult> Index(string searchString, string regionFilter) => View(await _apiClient.GetClientsAsync(searchString, regionFilter));
    public async Task<IActionResult> Details(int? id) { if (id is null) return NotFound(); var client = await _apiClient.GetClientAsync(id.Value); return client is null ? NotFound() : View(client); }
    public IActionResult Create() => View();
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,ContactDetails,Region")] Client client) { if (!ModelState.IsValid) return View(client); await _apiClient.CreateClientAsync(client); return RedirectToAction("Create", "Contracts"); }
    public async Task<IActionResult> Edit(int? id) { if (id is null) return NotFound(); var client = await _apiClient.GetClientAsync(id.Value); return client is null ? NotFound() : View(client); }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ContactDetails,Region")] Client client) { if (id != client.Id) return NotFound(); if (!ModelState.IsValid) return View(client); return await _apiClient.UpdateClientAsync(id, client) ? RedirectToAction(nameof(Index)) : NotFound(); }
    public async Task<IActionResult> Delete(int? id) { if (id is null) return NotFound(); var client = await _apiClient.GetClientAsync(id.Value); return client is null ? NotFound() : View(client); }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) { await _apiClient.DeleteClientAsync(id); return RedirectToAction(nameof(Index)); }
}