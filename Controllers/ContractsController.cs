using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TechMove_Logistics_Application.Data;
using TechMove_Logistics_Application.Models;

namespace TechMove_Logistics_Application.Controllers
{
    public class ContractsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ContractsController(
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Contracts
        public async Task<IActionResult> Index(
            string searchString,
            string statusFilter)
        {
            var contracts = _context.Contracts
                .Include(c => c.Client)
                .AsQueryable();

            // SEARCH FUNCTIONALITY
            if (!string.IsNullOrEmpty(searchString))
            {
                contracts = contracts.Where(c =>
                    c.Client.Name.Contains(searchString) ||
                    c.ServiceLevel.Contains(searchString) ||
                    c.Status.Contains(searchString));
            }

            // FILTER FUNCTIONALITY
            if (!string.IsNullOrEmpty(statusFilter))
            {
                contracts = contracts.Where(c =>
                    c.Status == statusFilter);
            }

            return View(await contracts.ToListAsync());
        }

        // GET: Contract Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: Create Contract
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(
                _context.Clients,
                "Id",
                "Name");

            return View();
        }

        // POST: Create Contract
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,ClientId,StartDate,EndDate,Status,ServiceLevel")]
            Contract contract,
            IFormFile agreementFile)
        {
            // FILE VALIDATION
            if (agreementFile != null)
            {
                var extension = Path.GetExtension(
                    agreementFile.FileName);

                // ALLOW ONLY PDF FILES
                if (extension.ToLower() != ".pdf")
                {
                    ModelState.AddModelError("",
                        "Only PDF files are allowed.");

                    ViewData["ClientId"] = new SelectList(
                        _context.Clients,
                        "Id",
                        "Name",
                        contract.ClientId);

                    return View(contract);
                }

                // CREATE AGREEMENTS FOLDER IF IT DOESN'T EXIST
                string uploadsFolder = Path.Combine(
                    _environment.WebRootPath,
                    "agreements");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // GENERATE UNIQUE FILE NAME
                string uniqueFileName =
                    Guid.NewGuid().ToString() + "_" +
                    agreementFile.FileName;

                string filePath = Path.Combine(
                    uploadsFolder,
                    uniqueFileName);

                // SAVE FILE
                using (var fileStream = new FileStream(
                    filePath,
                    FileMode.Create))
                {
                    await agreementFile.CopyToAsync(fileStream);
                }

                // SAVE FILE NAME TO DATABASE
                contract.AgreementFilePath = uniqueFileName;
            }

            if (ModelState.IsValid)
            {
                _context.Add(contract);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "Contract created successfully!";

                // REDIRECT TO SERVICE REQUEST PAGE
                return RedirectToAction(
                    "Create",
                    "ServiceRequests");
            }

            ViewData["ClientId"] = new SelectList(
                _context.Clients,
                "Id",
                "Name",
                contract.ClientId);

            return View(contract);
        }

        // GET: Edit Contract
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            ViewData["ClientId"] = new SelectList(
                _context.Clients,
                "Id",
                "Name",
                contract.ClientId);

            return View(contract);
        }

        // POST: Edit Contract
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,ClientId,StartDate,EndDate,Status,ServiceLevel,AgreementFilePath")]
            Contract contract)
        {
            if (id != contract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contract);

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] =
                        "Contract updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractExists(contract.Id))
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

            ViewData["ClientId"] = new SelectList(
                _context.Clients,
                "Id",
                "Name",
                contract.ClientId);

            return View(contract);
        }

        // GET: Delete Contract
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: Delete Contract
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract != null)
            {
                _context.Contracts.Remove(contract);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Contract deleted successfully!";

            return RedirectToAction(nameof(Index));
        }

        private bool ContractExists(int id)
        {
            return _context.Contracts.Any(e => e.Id == id);
        }
    }
}