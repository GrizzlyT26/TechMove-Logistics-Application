using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechMove_Logistics_Application.Data;
using TechMove_Logistics_Application.Models;

namespace TechMove_Logistics_Application.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Gets a list of all service requests,
        // including related contract information
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ServiceRequests
                .Include(s => s.Contract);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Gets details of a specific service request
        // based on the provided ID,
        // including related contract information
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .Include(s => s.Contract)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // GET: Displays form to create a new service request
        public IActionResult Create()
        {
            ViewData["ContractId"] = new SelectList(
                _context.Contracts,
                "Id",
                "Id");

            return View();
        }

        // POST: Creates a new service request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,ContractId,Description,CostUSD,CostZAR,Status")]
            ServiceRequest serviceRequest)
        {
            // Find selected contract
            var contract = await _context.Contracts
                .FirstOrDefaultAsync(c => c.Id == serviceRequest.ContractId);

            // Validation: Contract must exist
            if (contract == null)
            {
                ModelState.AddModelError("",
                    "Contract not found.");
            }

            // Validation: Prevent inactive contracts
            else if (contract.Status == "Expired" ||
                     contract.Status == "On Hold")
            {
                ModelState.AddModelError("",
                    "Cannot create service request for inactive contract.");
            }

            // Save request if validation passes
            if (ModelState.IsValid)
            {
                // Fixed USD → ZAR exchange rate
                decimal exchangeRate = 18.50m;

                // Automatically calculate ZAR
                serviceRequest.CostZAR =
                    Math.Round(
                        serviceRequest.CostUSD * exchangeRate,
                        2);

                // Save request
                _context.Add(serviceRequest);

                await _context.SaveChangesAsync();

                // Success popup message
                TempData["SuccessMessage"] =
                    "Service Request created successfully!";

                return RedirectToAction(nameof(Index));
            }

            // Reload dropdown if validation fails
            ViewData["ContractId"] = new SelectList(
                _context.Contracts,
                "Id",
                "Id",
                serviceRequest.ContractId);

            return View(serviceRequest);
        }

        // GET: Displays form to edit a service request
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest =
                await _context.ServiceRequests.FindAsync(id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            ViewData["ContractId"] = new SelectList(
                _context.Contracts,
                "Id",
                "Id",
                serviceRequest.ContractId);

            return View(serviceRequest);
        }

        // POST: Updates an existing service request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,ContractId,Description,CostUSD,CostZAR,Status")]
            ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Fixed USD → ZAR exchange rate
                    decimal exchangeRate = 18.50m;

                    // Automatically recalculate ZAR
                    serviceRequest.CostZAR =
                        Math.Round(
                            serviceRequest.CostUSD * exchangeRate,
                            2);

                    // Update request
                    _context.Update(serviceRequest);

                    await _context.SaveChangesAsync();

                    // Success popup
                    TempData["SuccessMessage"] =
                        "Service Request updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceRequestExists(serviceRequest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ContractId"] = new SelectList(
                _context.Contracts,
                "Id",
                "Id",
                serviceRequest.ContractId);

            return View(serviceRequest);
        }

        // GET: Delete confirmation page
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .Include(s => s.Contract)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // POST: Deletes a service request
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequest =
                await _context.ServiceRequests.FindAsync(id);

            if (serviceRequest != null)
            {
                _context.ServiceRequests.Remove(serviceRequest);

                TempData["SuccessMessage"] =
                    "Service Request deleted successfully!";
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ServiceRequestExists(int id)
        {
            return _context.ServiceRequests
                .Any(e => e.Id == id);
        }
    }
}