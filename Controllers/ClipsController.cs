using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThienASPMVC08032023.Database;
using ThienASPMVC08032023.Models;

namespace ThienASPMVC08032023.Controllers
{
    public class ClipsController : Controller
    {
        private readonly AppDbContext _context;

        public ClipsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Clips
        [Authorize]
        public async Task<IActionResult> Index(string searchString)
        {
            var clips = from c in _context.Clips
                        select c;


            if (!string.IsNullOrEmpty(searchString))
            {
                clips = clips.Where(c => c.Name!.Contains(searchString));
                TempData["success"] = $"results of search : {searchString}";
            }
            
             return View(await clips.ToListAsync());
        }

        // GET: Clips/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clips == null)
            {
                return NotFound();
            }

            var clip = await _context.Clips
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clip == null)
            {
                return NotFound();
            }

            return View(clip);
        }

        // GET: Clips/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Author,Description,Url,TimeCreated")] Clip clip)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clip);
                await _context.SaveChangesAsync();

                TempData["success"] = $"Created Clip name :  {clip.Name} Successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(clip);
        }

        // GET: Clips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clips == null)
            {
                return NotFound();
            }

            var clip = await _context.Clips.FindAsync(id);
            if (clip == null)
            {
                return NotFound();
            }
            return View(clip);
        }

        // POST: Clips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Author,Description,Url,TimeCreated")] Clip clip)
        {
            if (id != clip.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clip);
                    await _context.SaveChangesAsync();
                    TempData["success"] = $"Edited Clip name : {clip.Name} Successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClipExists(clip.Id))
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
            return View(clip);
        }

        // GET: Clips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clips == null)
            {
                return NotFound();
            }

            var clip = await _context.Clips
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clip == null)
            {
                return NotFound();
            }

            return View(clip);
        }

        // POST: Clips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clips == null)
            {
                return Problem("Entity set 'AppDbContext.Clips'  is null.");
            }
            var clip = await _context.Clips.FindAsync(id);
            if (clip != null)
            {
                _context.Clips.Remove(clip);
            }
            
            await _context.SaveChangesAsync();
            TempData["success"] = $"Deleted Clip {clip?.Name} Successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool ClipExists(int id)
        {
          return _context.Clips.Any(e => e.Id == id);
        }
    }
}
