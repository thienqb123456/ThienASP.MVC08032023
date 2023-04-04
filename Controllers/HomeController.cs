using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ThienASPMVC08032023.Database;
using ThienASPMVC08032023.Models;

namespace ThienASPMVC08032023.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (_context.Clips == null)
            {
                return NotFound("Not Found any Clip in DB");
            }
            var clips = await _context.Clips.ToListAsync();
            return View(clips);
        }



        public async Task<ActionResult> Detail(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound("Not found id ");
            }
            if (_context.Clips == null)
            {
                return NotFound("Not found any clip in DB");
            }
            var clip = await _context.Clips.FirstOrDefaultAsync(c => c.Id == id);

            if (clip == null)
            {
                return NotFound($"Not found clip has id = {id}");
            }

            return View(clip);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}