using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechMove_Logistics_Application.Models;
using TechMove_Logistics_Application.Services;

namespace TechMove_Logistics_Application.Controllers;

public class ServiceRequestsController : Controller
{
    private readonly TechMoveApiClient _apiClient;

    public ServiceRequestsController(TechMoveApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _apiClient.GetServiceRequestsAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        var serviceRequest = await _apiClient.GetServiceRequestAsync(id.Value);
        return serviceRequest is null ? NotFound() : View(serviceRequest);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateContractsAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,ContractId,Description,CostUSD,CostZAR,Status")] ServiceRequest serviceRequest)
    {
        var contract = await _apiClient.GetContractAsync(serviceRequest.ContractId);
        if (contract is null)
        {
            ModelState.AddModelError("", "Contract not found.");
        }
        else if (contract.Status == "Expired" || contract.Status == "On Hold")
        {
            ModelState.AddModelError("", "Cannot create service request for inactive contract.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateContractsAsync(serviceRequest.ContractId);
            return View(serviceRequest);
        }

        serviceRequest.CostZAR = Math.Round(serviceRequest.CostUSD * 18.50m, 2);
        await _apiClient.CreateServiceRequestAsync(serviceRequest);
        TempData["SuccessMessage"] = "Service Request created successfully!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var serviceRequest = await _apiClient.GetServiceRequestAsync(id.Value);
        if (serviceRequest is null) return NotFound();
        await PopulateContractsAsync(serviceRequest.ContractId);
        return View(serviceRequest);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,ContractId,Description,CostUSD,CostZAR,Status")] ServiceRequest serviceRequest)
    {
        if (id != serviceRequest.Id) return NotFound();
        if (!ModelState.IsValid)
        {
            await PopulateContractsAsync(serviceRequest.ContractId);
            return View(serviceRequest);
        }

        serviceRequest.CostZAR = Math.Round(serviceRequest.CostUSD * 18.50m, 2);
        if (!await _apiClient.UpdateServiceRequestAsync(id, serviceRequest)) return NotFound();
        TempData["SuccessMessage"] = "Service Request updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var serviceRequest = await _apiClient.GetServiceRequestAsync(id.Value);
        return serviceRequest is null ? NotFound() : View(serviceRequest);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _apiClient.DeleteServiceRequestAsync(id);
        TempData["SuccessMessage"] = "Service Request deleted successfully!";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateContractsAsync(int? selectedContractId = null)
    {
        var contracts = await _apiClient.GetContractsAsync();
        ViewData["ContractId"] = new SelectList(contracts, "Id", "Id", selectedContractId);
    }
}

