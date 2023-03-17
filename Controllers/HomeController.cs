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
        private readonly AppDbContext _dbcontext;

        public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbcontext = dbContext;
        }

        public IList<Clip>? Clips { get; set; }


        public async Task<IActionResult> Index()
        {
            if (_dbcontext.Clips == null)
            {
                return View(null);
            }
            Clips = await _dbcontext.Clips.ToListAsync();
            return View(Clips);
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