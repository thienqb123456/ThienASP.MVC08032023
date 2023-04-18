using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThienASPMVC08032023.Database;
using ThienASPMVC08032023.Models;

namespace ThienASPMVC08032023.Areas.Admin.Controllers
{
    [Authorize(Roles ="admin")]
    [Area("Admin")]
    [Route("/Role/{action=Index}")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RoleController> _logger;
        private readonly AppDbContext _context;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, ILogger<RoleController> logger, AppDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        [TempData]
        public string? StatusMessage { get; set; }


        // GET: RoleController
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        // GET: RoleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Name")] IdentityRole role )
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return NotFound();  
                }

                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    StatusMessage = $"Created a role named : {role.Name} successfully ";

                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: RoleController/Delete/5
        public async Task<ActionResult> Delete(string roleid)
        {
            if (string.IsNullOrEmpty(roleid))
            {
                return NotFound("Not Found roleid");
            }
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("not found role");
            }
            return View(role);
        }

        // POST: RoleController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string roleid)
        {
            try
            {
                if (string.IsNullOrEmpty(roleid))
                {
                    return NotFound("not found roleid");
                }

                var role = await _roleManager.FindByIdAsync(roleid);
                if (role == null) 
                { 
                    return NotFound("role not found"); 
                }
                
                var deleteResult = await _roleManager.DeleteAsync(role);
                if (deleteResult.Succeeded)
                {
                    StatusMessage = $"Deleted role named : {role.Name} successfully ";
                }
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
