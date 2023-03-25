using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using VacationManager.Models;
using VacationManager.Repositories;

namespace VacationManager.Controllers
{
    /// <summary>
    /// Class TimeOffsController.
    /// Implements the <see cref="Controller" />
    /// </summary>
    /// <seealso cref="Controller" />
    public class TimeOffsController : Controller
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly VacationManagerDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOffsController"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public TimeOffsController(VacationManagerDbContext context)
        {
            _context = context;
        }

        // GET: TimeOffs
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>IActionResult.</returns>
        public async Task<IActionResult> Index()
        {
            ViewData["Teams"] = _context.Teams;
            ViewData["Users"] = _context.Users;
            return View(await _context.TimeOffs.ToListAsync());
        }

        // GET: TimeOffs/Details/5
        /// <summary>
        /// Detailses the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IActionResult.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeOff = await _context.TimeOffs
                .FirstOrDefaultAsync(m => m.TimeOffId == id);
            if (timeOff == null)
            {
                return NotFound();
            }

            return View(timeOff);
        }

        // GET: TimeOffs/Create
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>IActionResult.</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: TimeOffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Creates the specified time off.
        /// </summary>
        /// <param name="timeOff">The time off.</param>
        /// <returns>IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TimeOffId,From,To,HalfDay,Type,Approved,RequestingUser,CreatedOn")] TimeOff timeOff)
        {
            if (ModelState.IsValid)
            {
                timeOff.CreatedOn = DateTime.Now;
                _context.Add(timeOff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(timeOff);
        }

        // GET: TimeOffs/Edit/5
        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IActionResult.</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeOff = await _context.TimeOffs.FindAsync(id);
            if (timeOff == null)
            {
                return NotFound();
            }
            return View(timeOff);
        }

        // POST: TimeOffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="timeOff">The time off.</param>
        /// <returns>IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TimeOffId,From,To,CreatedOn,HalfDay,Type,Approved,RequestingUser")] TimeOff timeOff)
        {
            if (id != timeOff.TimeOffId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeOff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeOffExists(timeOff.TimeOffId))
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
            return View(timeOff);
        }

        // GET: TimeOffs/Delete/5
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IActionResult.</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeOff = await _context.TimeOffs
                .FirstOrDefaultAsync(m => m.TimeOffId == id);
            if (timeOff == null)
            {
                return NotFound();
            }

            return View(timeOff);
        }

        // POST: TimeOffs/Delete/5
        /// <summary>
        /// Deletes the confirmed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IActionResult.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timeOff = await _context.TimeOffs.FindAsync(id);
            _context.TimeOffs.Remove(timeOff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Times the off exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool TimeOffExists(int id)
        {
            return _context.TimeOffs.Any(e => e.TimeOffId == id);
        }
    }
}
