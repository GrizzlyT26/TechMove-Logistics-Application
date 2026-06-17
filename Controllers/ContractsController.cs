using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechMove_Logistics_Application.Models;
using TechMove_Logistics_Application.Services;

namespace TechMove_Logistics_Application.Controllers;

public class ContractsController : Controller
{
    private readonly TechMoveApiClient _apiClient;
    private readonly IWebHostEnvironment _environment;

    public ContractsController(TechMoveApiClient apiClient, IWebHostEnvironment environment)
    {
        _apiClient = apiClient;
        _environment = environment;
    }

    public async Task<IActionResult> Index(string searchString, string statusFilter)
    {
        return View(await _apiClient.GetContractsAsync(searchString, statusFilter));
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        var contract = await _apiClient.GetContractAsync(id.Value);
        return contract is null ? NotFound() : View(contract);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateClientsAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,ClientId,StartDate,EndDate,Status,ServiceLevel")] Contract contract, IFormFile agreementFile)
    {
        if (agreementFile != null)
        {
            var extension = Path.GetExtension(agreementFile.FileName);
            if (extension.ToLowerInvariant() != ".pdf")
            {
                ModelState.AddModelError("", "Only PDF files are allowed.");
                await PopulateClientsAsync(contract.ClientId);
                return View(contract);
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "agreements");
            Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = Guid.NewGuid() + "_" + agreementFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await agreementFile.CopyToAsync(fileStream);
            contract.AgreementFilePath = uniqueFileName;
        }

        if (!ModelState.IsValid)
        {
            await PopulateClientsAsync(contract.ClientId);
            return View(contract);
        }

        await _apiClient.CreateContractAsync(contract);
        TempData["SuccessMessage"] = "Contract created successfully!";
        return RedirectToAction("Create", "ServiceRequests");
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var contract = await _apiClient.GetContractAsync(id.Value);
        if (contract is null) return NotFound();
        await PopulateClientsAsync(contract.ClientId);
        return View(contract);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,StartDate,EndDate,Status,ServiceLevel,AgreementFilePath")] Contract contract)
    {
        if (id != contract.Id) return NotFound();
        if (!ModelState.IsValid)
        {
            await PopulateClientsAsync(contract.ClientId);
            return View(contract);
        }

        if (!await _apiClient.UpdateContractAsync(id, contract)) return NotFound();
        TempData["SuccessMessage"] = "Contract updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var contract = await _apiClient.GetContractAsync(id.Value);
        return contract is null ? NotFound() : View(contract);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _apiClient.DeleteContractAsync(id);
        TempData["SuccessMessage"] = "Contract deleted successfully!";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateClientsAsync(int? selectedClientId = null)
    {
        var clients = await _apiClient.GetClientsAsync();
        ViewData["ClientId"] = new SelectList(clients, "Id", "Name", selectedClientId);
    }
}

