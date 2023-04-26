using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using ThienASPMVC08032023.Models;
using ThienASPMVC08032023.Repository.InterfaceRepo;
using X.PagedList;

namespace ThienASPMVC08032023.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    [Route("/{controller}/{action=Index}/{id:int?}")]
    public class ClipsController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        private readonly ILogger<ClipsController> _logger;
        private readonly UserManager<AppUser> _userManager;

        [TempData]
        public string? StatusMessage { get; set; }

        public ClipsController(IRepositoryWrapper repo, ILogger<ClipsController> logger, UserManager<AppUser> userManager)
        {
            _repo = repo;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: Clips
        [HttpGet]
        public async Task<ActionResult> Index(string searchString,string sortBy, int? currentPage, int? pageSize)
        {
            var qrClips = await _repo.ClipRepo.GetAllClipsAsync(searchString, sortBy);

            ViewData["SortByName"] = sortBy == "name" ? "name_desc" : "name";
            ViewData["SortByTimeCreated"] = sortBy == "timeCreated" ? "timeCreated_desc" : "timeCreated";

            // paging
            if (currentPage == null)
            {
                currentPage = 1;
            }
            if (pageSize == null)
            {
                pageSize = 5;
            }
            return View(qrClips.ToPagedList((int)currentPage, (int)pageSize));
        }

        // GET: Clips/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var clip = await _repo.ClipRepo.GetClipByIdAsync(id);
            if (clip == null) { return NotFound($"Not Found Clip has id ={id}"); }
            return View(clip);
        }

        // GET: Clips/Create
        public async Task<ActionResult> Create()
        {
            var categories = await _repo.CategoryRepo.GetAllCategoriesAsync();
            ViewBag.listCategories = new SelectList(categories, "Id", "Name");
            return View();
        }
      
        // POST: Clips/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Name,Description,Url,CategoryId")] Clip clipVM)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                clipVM.AuthorUser = currentUser;
                _repo.ClipRepo.CreateClip (clipVM);
                await _repo.SaveAsync();
                StatusMessage = $"Uploaded clip Name :  {clipVM.Name} Successfully!";
            }
            return RedirectToAction("Index");
        }

        // GET: Clips/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var categories = await _repo.CategoryRepo.GetAllCategoriesAsync();
            ViewBag.listCategories = new SelectList(categories, "Id", "Name");
            var clip = await _repo.ClipRepo.GetClipByIdAsync(id);
            if (clip == null) { return NotFound($"Not Found Clip has id ={id}"); }
            return View(clip);
        }

        // POST: Clips/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id,[Bind("Id,Name,Description,Url,CategoryId")] Clip clipVM)
        {
            if(id != clipVM.Id) { return NotFound($"wrong id, {id} != {clipVM.Id}"); }
            if (ModelState.IsValid)
            {
                var newClip = await _repo.ClipRepo.GetClipByIdAsync(id);
                newClip.Name = clipVM.Name;
                newClip.Description = clipVM.Description;
                newClip.Url = clipVM.Url;
                newClip.CategoryId = clipVM.CategoryId;
                _repo.ClipRepo.UpdateClip(newClip);
                await _repo.SaveAsync();
                StatusMessage = $"Edited Clip name : {clipVM.Name} Successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Clips/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var clip =await _repo.ClipRepo.GetClipByIdAsync(id);
            if (clip == null) { return NotFound($"Not Found Clip has id ={id}"); }
            return View(clip);
        }

        // POST: Clips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var clip = await _repo.ClipRepo.GetClipByIdAsync(id);
            _repo.ClipRepo.DeleteClip(clip);
            await _repo.SaveAsync();
            StatusMessage = $"Deleted clip successfully!";
            return RedirectToAction(nameof(Index));
        }

    }
}
