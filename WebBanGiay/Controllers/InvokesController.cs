using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanGiay.Data;
using WebBanGiay.Models;

namespace WebBanGiay.Controllers
{
    public class InvokesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvokesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Invokes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Invoke.Include(i => i.Status).Include(i => i.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Invokes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Invoke == null)
            {
                return NotFound();
            }

            var invoke = await _context.Invoke
                .Include(i => i.Status)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.InvokeId == id);
            if (invoke == null)
            {
                return NotFound();
            }

            return View(invoke);
        }

        // GET: Invokes/Create
        public IActionResult Create()
        {
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusName");
            ViewData["Username"] = new SelectList(_context.User, "UserName", "UserName");
            return View();
        }

        // POST: Invokes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvokeId,CustomerName,Address,PhoneNumber,OrderDate,DeliveryDate,DaysToDeliver,InvokeTotalAmount,ShippingMethod,PaymentMethod,ShippingFee,StatusId,Username")] Invoke invoke)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoke);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusName", invoke.StatusId);
            ViewData["Username"] = new SelectList(_context.User, "UserName", "UserName", invoke.Username);
            return View(invoke);
        }

        // GET: Invokes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Invoke == null)
            {
                return NotFound();
            }

            var invoke = await _context.Invoke.FindAsync(id);
            if (invoke == null)
            {
                return NotFound();
            }
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusName", invoke.StatusId);
            ViewData["Username"] = new SelectList(_context.User, "UserName", "UserName", invoke.Username);
            return View(invoke);
        }

        // POST: Invokes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvokeId,CustomerName,Address,PhoneNumber,OrderDate,DeliveryDate,DaysToDeliver,InvokeTotalAmount,ShippingMethod,PaymentMethod,ShippingFee,StatusId,Username")] Invoke invoke)
        {
            if (id != invoke.InvokeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoke);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvokeExists(invoke.InvokeId))
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
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusName", invoke.StatusId);
            ViewData["Username"] = new SelectList(_context.User, "UserName", "UserName", invoke.Username);
            return View(invoke);
        }

        // GET: Invokes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Invoke == null)
            {
                return NotFound();
            }

            var invoke = await _context.Invoke
                .Include(i => i.Status)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.InvokeId == id);
            if (invoke == null)
            {
                return NotFound();
            }

            return View(invoke);
        }

        // POST: Invokes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Invoke == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Invoke'  is null.");
            }
            var invoke = await _context.Invoke.FindAsync(id);
            if (invoke != null)
            {
                _context.Invoke.Remove(invoke);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvokeExists(int id)
        {
          return (_context.Invoke?.Any(e => e.InvokeId == id)).GetValueOrDefault();
        }
    }
}
