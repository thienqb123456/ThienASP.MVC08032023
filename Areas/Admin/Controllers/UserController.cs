using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThienASPMVC08032023.Areas.Admin.Models.User;
using ThienASPMVC08032023.Models;
using X.PagedList;

namespace ThienASPMVC08032023.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,manager")]
    [Area("Admin")]
    [Route("/User/{Action}")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager; 
        private readonly ILogger<UserController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        [TempData]
        public string StatusMessage { get; set; }



        // GET: UserController
        [Route("/User/")]
        public ActionResult Index(int? currentPage, int? pageSize)
        {
            List<AppUser> users = _userManager.Users.ToList();

            if (currentPage == null)
            {
                currentPage = 1;
            }
             
            if (pageSize == null)
            {
                pageSize = 10;
            }

            
                
            return View(users.ToPagedList((int)currentPage, (int)pageSize));
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        // GET: UserController/SetRoleUser
        public async Task<ActionResult> SetRoleUser(string userId)
        {
            var SetRoleUserModel = new SetRoleUserModel();



            if (string.IsNullOrEmpty(userId))
            {
                return NotFound(" not found userId ");
            }
            SetRoleUserModel.User = await _userManager.FindByIdAsync(userId);

            SetRoleUserModel.RolesUser = await _userManager.GetRolesAsync(SetRoleUserModel.User);
            
            List<IdentityRole> roles = _roleManager.Roles.ToList();
            IEnumerable<string> allRolesName = roles.Select(r => r.Name).ToList();

            ViewBag.allRolesName = new SelectList(allRolesName);
            return View(SetRoleUserModel);
        }


        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetRoleUser(string userId, [Bind("RolesUser,User")]SetRoleUserModel SetRoleUserModel)
        {
            try
            {
                List<IdentityRole> roles = _roleManager.Roles.ToList();
                IEnumerable<string> allRolesName = roles.Select(roles => roles.Name);

                ViewBag.allRolesName = new SelectList(allRolesName);


                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound("not found userId");
                }

                SetRoleUserModel.User = await _userManager.FindByIdAsync(userId);
                if (SetRoleUserModel.User == null)
                {
                    return NotFound($"not found user, id = {userId}");
                }

                IList<string> currentRolesUser = await _userManager.GetRolesAsync(SetRoleUserModel.User);

                IEnumerable<string> deleteRolesUser = currentRolesUser.Where(role => !SetRoleUserModel.RolesUser.Contains(role));

                IEnumerable<string> AddRolesUser = SetRoleUserModel.RolesUser.Where(role => !currentRolesUser.Contains(role));

                var resultDeleteRolesUser = await _userManager.RemoveFromRolesAsync(SetRoleUserModel.User, deleteRolesUser);

                if (!resultDeleteRolesUser.Succeeded)
                {
                    return NotFound("Delete Roles Fail");
                }

                var resultAddRolesUser = await _userManager.AddToRolesAsync(SetRoleUserModel.User, AddRolesUser);

                if (!resultAddRolesUser.Succeeded)
                {
                    return NotFound("Add Roles Fail");

                }

                StatusMessage = $" Added roles for user: {SetRoleUserModel.User.UserName}";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public async Task<ActionResult> Delete(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("Not found userId");
            }

            var user = await _userManager.FindByIdAsync(userId);   

            if (user == null)
            {
                return NotFound("not found user");
            }

            return View(user);
        }

        // POST: UserController/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound("Not Found userId");
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("Not Found User");
                }

                var deleteResult = await _userManager.DeleteAsync(user);

                if (deleteResult.Succeeded)
                {
                    StatusMessage = $"Deleted user : {user.UserName} successfully!";

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
