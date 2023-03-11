using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThienASPMVC08032023.Database;
using ThienASPMVC08032023.Models;

namespace ThienASPMVC08032023.Controllers
{
    public class ClipController : Controller
    {
        private readonly AppDbContext _dbcontext;

        public ClipController(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;

        }

        // GET: ClipController
        public async Task<IActionResult> Index()
        {
            if (_dbcontext.Clips == null)
            {
                return View(null);
            }
            var clips = await _dbcontext.Clips.ToListAsync();

            return View(clips);
        }

        // GET: ClipController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var clip = await _dbcontext.Clips.FirstOrDefaultAsync(c => c.Id == id);
            if (clip == null)
            {
                return NotFound();
            }

            return View(clip);
        }

        // GET: ClipController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: ClipController/Create h
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Clip clip)
        {
            if (!ModelState.IsValid)
            {
                return View(clip);
            }

            _dbcontext.Clips.Add(clip);
            await _dbcontext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: ClipController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            ;
                var clip = await _dbcontext.Clips.FirstOrDefaultAsync(c => c.Id == id);

            if (clip == null)
            {
                return NotFound();
            }
            
            return View(clip);
        }

        // POST: ClipController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Clip clip)
        {
            if (!ModelState.IsValid)
            {
                return View(clip);
                
            }
            _dbcontext.Clips.Update(clip);
            await _dbcontext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        // GET: ClipController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id ==0)
            {
                return NotFound();
            }
            var clip = await _dbcontext.Clips.FirstOrDefaultAsync(c => c.Id == id);
            if (clip == null)
            {
                return NotFound();
            }
            return View(clip);
        }

        // POST: ClipController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Clip clip)
        {

            if (clip == null)
            {
                return NotFound();
            }
                _dbcontext.Clips.Remove(clip);
                await _dbcontext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
        }
    }
}
