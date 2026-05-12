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
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Gets a list of all clients
        public async Task<IActionResult> Index(
    string searchString,
    string regionFilter)
        {
            var clients = from c in _context.Clients
                          select c;

            // SEARCH BY CLIENT NAME
            if (!string.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(c =>
                    c.Name.Contains(searchString));
            }

            // FILTER BY REGION
            if (!string.IsNullOrEmpty(regionFilter))
            {
                clients = clients.Where(c =>
                    c.Region == regionFilter);
            }

            return View(await clients.ToListAsync());
        }

        // GET: Is responsible for displaying the details of a specific client based on the provided ID
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET:Is responsible for rendering the form to create a new client
        public IActionResult Create()
        {
            return View();
        }

        // POST: Is responsible for handling the form submission to create a new client.
        // It validates the input, adds the client to the database,
        // and redirects to the Contracts Create page.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,ContactDetails,Region")]
            Client client)
        {
            // This checks if the model state is valid,
            // which means that all required fields are filled out correctly.
            _context.Add(client);

            await _context.SaveChangesAsync();

            // This part redirects the user to the
            // Create action of the Contracts controller
            // after successfully creating a client.

            return RedirectToAction(
                "Create",
                "Contracts");
        }

        // GET: Cients and is responsible for rendering
        // the form to edit an existing client's
        // details based on the provided ID.

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: XChecks if the provided ID matches the client's ID,
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,ContactDetails,Region")]
            Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(client);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(client.Id))
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

        // GET: Responsible for rendering the confirmation
        // page to delete a client based on the provided ID.

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Deletes the client and all associated contracts
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client != null)
            {
                _context.Clients.Remove(client);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}