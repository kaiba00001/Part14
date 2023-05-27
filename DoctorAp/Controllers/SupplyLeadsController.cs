using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoctorAp.Data;
using DoctorAp.Models;

namespace DoctorAp.Controllers
{
    public class SupplyLeadsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplyLeadsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SupplyLeads
        public async Task<IActionResult> Index()
        {
              return _context.SupplyLead != null ? 
                          View(await _context.SupplyLead.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.SupplyLead'  is null.");
        }

        // GET: SupplyLeads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SupplyLead == null)
            {
                return NotFound();
            }

            var supplyLead = await _context.SupplyLead
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplyLead == null)
            {
                return NotFound();
            }

            return View(supplyLead);
        }

        // GET: SupplyLeads/Create
        public IActionResult Create()
        {
            var items = new List<SelectListItem>
    {
        new SelectListItem { Value = "Concerta", Text = "Concerta" },
        new SelectListItem { Value = "Advil", Text = "Advil" },
        new SelectListItem { Value = "Amoxicillin", Text = "Amoxicillin" },
        new SelectListItem { Value = "Aspirin", Text = "Aspirin" },
        new SelectListItem { Value = "Benadryl", Text = "Benadryl" },
        new SelectListItem { Value = "Ciprofloxacin", Text = "Ciprofloxacin" }
        // Add more items as needed
    };

            ViewBag.Items = items;

            return View();
        }

        // POST: SupplyLeads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Item,Quantity,CostPerItem")] SupplyLead supplyLead)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplyLead);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplyLead);
        }

        // GET: SupplyLeads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SupplyLead == null)
            {
                return NotFound();
            }

            var supplyLead = await _context.SupplyLead.FindAsync(id);
            if (supplyLead == null)
            {
                return NotFound();
            }
            return View(supplyLead);
        }

        // POST: SupplyLeads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Item,Quantity, CostPerItem")] SupplyLead supplyLead)
        {
            if (id != supplyLead.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplyLead);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplyLeadExists(supplyLead.Id))
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
            return View(supplyLead);
        }

        // GET: SupplyLeads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SupplyLead == null)
            {
                return NotFound();
            }

            var supplyLead = await _context.SupplyLead
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplyLead == null)
            {
                return NotFound();
            }

            return View(supplyLead);
        }

        // POST: SupplyLeads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SupplyLead == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SupplyLead'  is null.");
            }
            var supplyLead = await _context.SupplyLead.FindAsync(id);
            if (supplyLead != null)
            {
                _context.SupplyLead.Remove(supplyLead);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplyLeadExists(int id)
        {
          return (_context.SupplyLead?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
