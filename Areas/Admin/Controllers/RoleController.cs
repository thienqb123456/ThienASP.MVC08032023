using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThienASPMVC08032023.Database;
using ThienASPMVC08032023.Models;

namespace ThienASPMVC08032023.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("/Role/{action}")]
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
        public async Task<ActionResult> Create(IFormCollection collection,[Bind("Id,Name")] IdentityRole role )
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return NotFound();  
                }

                var result = await _roleManager.CreateAsync(role);
 
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleController/Edit/roleid=
        public async Task<ActionResult> Edit(string roleid)
        {
            var role = await _roleManager.FindByIdAsync(roleid);
            if  (role == null)
            {
                return NotFound("role not found");
            }
            return View(role);
        }

        // POST: RoleController/Edit/roleid=
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string roleid, [Bind("Name")]IdentityRole role)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    role = await _roleManager.FindByIdAsync(roleid);
                    await _roleManager.UpdateAsync(role);
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
            var role = await _roleManager.FindByIdAsync(roleid);
            return View(role);
        }

        // POST: RoleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string roleid, IdentityRole role )
        {
            try
            {
                role = await _roleManager.FindByIdAsync(roleid);
                if (role == null) 
                { 
                    return NotFound("role not found"); 
                }
                
                await _roleManager.DeleteAsync(role);
                

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
