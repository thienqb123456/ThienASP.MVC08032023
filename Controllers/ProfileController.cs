
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThienASPMVC08032023.Database;
using ThienASPMVC08032023.Models;

namespace ThienASPMVC08032023.Controllers
{

    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context,ILogger<ProfileController> logger, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }




        // GET: ProfileController
        public async Task<ActionResult> Index(string userName)
        {
            if (userName == null) { return NotFound("Notfound userName"); }


            if (_context.Clips == null) { return NotFound("Not found any clip in DB"); }

            var user = await _context.Users.Include(u => u.Clips)
                                     .FirstOrDefaultAsync(u => u.UserName == userName);

            return View(user);
        }
        
    }
}
