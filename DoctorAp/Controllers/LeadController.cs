using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoctorAp.Data;
using DoctorAp.Models;
using Microsoft.AspNetCore.Authorization;

namespace DoctorAp.Controllers
    
{
    [Authorize]
    public class LeadController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeadController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lead
        public async Task<IActionResult> Index()
        {
              return _context.BookingLead != null ? 
                          View(await _context.BookingLead.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.BookingLead'  is null.");
        }

        // GET: Lead/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BookingLead == null)
            {
                return NotFound();
            }

            var bookingLeadEntity = await _context.BookingLead
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookingLeadEntity == null)
            {
                return NotFound();
            }

            return View(bookingLeadEntity);
        }

        // GET: Lead/Create
        // GET: Lead/Create
        public IActionResult Create()
        {
            // Retrieve the already booked times from the database
            var bookedTimes = _context.BookingLead.Select(lead => lead.Time).ToList();

            // Create a list of available time slots from 8:00 AM to 3:00 PM
            var availableTimes = new List<TimeSpan>();
            var startTime = new TimeSpan(8, 0, 0);
            var endTime = new TimeSpan(15, 0, 0);
            var timeSlotDuration = new TimeSpan(0, 30, 0); // 30 minutes

            while (startTime <= endTime)
            {
                // Create a DateTime object for the current time slot
                var currentTimeSlot = DateTime.Today.Add(startTime);

                // Check if the current time slot is already booked
                if (!bookedTimes.Any(bookedTime => bookedTime.Equals(currentTimeSlot.TimeOfDay)))
                {
                    availableTimes.Add(currentTimeSlot.TimeOfDay);
                }
                startTime = startTime.Add(timeSlotDuration);
            }

            // Pass the available times to the view
            ViewData["AvailableTimes"] = new SelectList(availableTimes);

            return View();
        }
        // POST: Lead/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,Mobile,Email,Time")] BookingLeadEntity bookingLeadEntity)
        {
            if (ModelState.IsValid)
            {
                // Check if there is any existing lead with the same booking time
                bool isBookingTimeAvailable = await _context.BookingLead
                    .AnyAsync(b => b.Time == bookingLeadEntity.Time);

                if (isBookingTimeAvailable)
                {
                    ModelState.AddModelError("Time", "The selected booking time is not available. Please choose a different time.");
                    return View(bookingLeadEntity);
                }

                _context.Add(bookingLeadEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(bookingLeadEntity);
        }
        // GET: Lead/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BookingLead == null)
            {
                return NotFound();
            }

            var bookingLeadEntity = await _context.BookingLead.FindAsync(id);
            if (bookingLeadEntity == null)
            {
                return NotFound();
            }
            return View(bookingLeadEntity);
        }

        // POST: Lead/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,Mobile,Email,Time")] BookingLeadEntity bookingLeadEntity)
        {
            if (id != bookingLeadEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookingLeadEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingLeadEntityExists(bookingLeadEntity.Id))
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
            return View(bookingLeadEntity);
        }

        // GET: Lead/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BookingLead == null)
            {
                return NotFound();
            }

            var bookingLeadEntity = await _context.BookingLead
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookingLeadEntity == null)
            {
                return NotFound();
            }

            return View(bookingLeadEntity);
        }

        // POST: Lead/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BookingLead == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BookingLead'  is null.");
            }
            var bookingLeadEntity = await _context.BookingLead.FindAsync(id);
            if (bookingLeadEntity != null)
            {
                _context.BookingLead.Remove(bookingLeadEntity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingLeadEntityExists(int id)
        {
          return (_context.BookingLead?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
