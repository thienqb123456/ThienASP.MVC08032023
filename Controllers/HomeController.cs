using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using ThienASPMVC08032023.Database;
using ThienASPMVC08032023.Models;
using ThienASPMVC08032023.Repository.InterfaceRepo;

namespace ThienASPMVC08032023.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(IRepositoryWrapper repo, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _repo = repo;
        }

        [TempData]
        public string? StatusMessage { get; set; }

        public async Task<ActionResult> Index(string searchString, string sortBy, int? cateId)
        {
            var clips = await _repo.ClipRepo.GetAllClipsAsync(searchString, sortBy);
            if (cateId != null)
            {
                clips = clips.Where(c => c.CategoryId == cateId).ToList();
            }
            var categories = await _repo.CategoryRepo.GetAllCategoriesAsync();
            ViewBag.categories = categories;
            return View(clips);
        }

        [HttpGet("/clip/{id}")]
        public async Task<ActionResult> Detail(int id)
        {
            var clip = await _repo.ClipRepo.GetClipByIdAsync(id);
            if (clip == null) { return NotFound($"Not found clip has id = {id}"); }
            return View(clip);
        }

        [Authorize]
        [HttpGet("{action}")]
        public async Task<ActionResult> Upload() 
        { 
            var categories = await  _repo.CategoryRepo.GetAllCategoriesAsync();
            ViewBag.ListCategories = new SelectList(categories, "Id", "Name");
            return View(); 
        }
       
        [Authorize]
        [HttpPost("{action}")]
        public async Task<ActionResult> Upload([Bind("Id,Name,Description,Url,CategoryId")] Clip clipVM)
        {
            var currentUser = await _userManager.GetUserAsync(this.User);
            clipVM.AuthorUser = currentUser;
            if (ModelState.IsValid)
            {
                _repo.ClipRepo.CreateClip(clipVM);
                await _repo.SaveAsync();
                StatusMessage = $"Uploaded clip Name :  {clipVM.Name} Successfully!";
            }
            return RedirectToAction("Detail", "Home", new { id = clipVM.Id });
        }

        [Authorize]
        [HttpGet("/clip/{action}/{id}")]
        public async Task<ActionResult> Edit(int id)
        {
            var categories = await _repo.CategoryRepo.GetAllCategoriesAsync();
            ViewBag.ListCategories = new SelectList(categories, "Id", "Name");
            var clip = await _repo.ClipRepo.GetClipByIdAsync(id);
            if(clip == null) { return NotFound($"Not Found Clip has id ={id}"); }
            var currentUser = await _userManager.GetUserAsync(this.User);
            if (clip.AuthorUser!.Id != currentUser.Id ) return NotFound("U do not have permisson to access ");
            return View(clip);
        }

        [Authorize]
        [HttpPost("/clip/{action}/{id}")]
        public async Task<ActionResult> Edit(int id,[Bind("Id,Name,Description,Url,CategoryId")] Clip clipVM)
        {
            if (id != clipVM.Id) { return NotFound($" id {id} != clipVM.Id {clipVM.Id}"); }
            var newClip = await _repo.ClipRepo.GetClipByIdAsync(id);
            if (ModelState.IsValid && newClip != clipVM)
            {
                newClip.Name = clipVM.Name;
                newClip.Description = clipVM.Description;
                newClip.Url = clipVM.Url;
                newClip.CategoryId = clipVM.CategoryId;
                _repo.ClipRepo.UpdateClip(newClip);
                await _repo.SaveAsync();
                StatusMessage = $"Updated Clip has name : {clipVM.Name} Successfully!";
            }
            return RedirectToAction("Detail", "Home", new { id = clipVM.Id });
        }

        [Authorize]
        [HttpGet("/clip/{action}/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var clip = await _repo.ClipRepo.GetClipByIdAsync(id);
            if (clip == null) { return NotFound($"Not Found Clip has id ={id}"); }
            var currentUser = await _userManager.GetUserAsync(this.User);
            if (clip.AuthorUser!.Id != currentUser.Id)
            {
                return NotFound("accessdenied");
            }
            return View(clip);
        }

        [Authorize]
        [HttpPost("/clip/{action}/{id}")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var clip = await _repo.ClipRepo.GetClipByIdAsync(id);
            if (clip == null) { return NotFound($"Not Found Clip has id ={id}"); }
            var currentUser = await _userManager.GetUserAsync(User);
            if (clip.AuthorUser!.Id == currentUser.Id)
            {
                _repo.ClipRepo.DeleteClip(clip);
                await _repo.SaveAsync();
                StatusMessage = $"Deleted Clip {clip?.Name} Successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("/AccessDenied/");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}