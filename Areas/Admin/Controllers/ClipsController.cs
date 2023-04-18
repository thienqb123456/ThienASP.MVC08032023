using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThienASPMVC08032023.Models;
using ThienASPMVC08032023.Repository;
using X.PagedList;

namespace ThienASPMVC08032023.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    [Route("/{controller}/{action=Index}/{id:int?}")]
    public class ClipsController : Controller
    {
        private readonly IRepositoryClip _repo;
        private readonly ILogger<ClipsController> _logger;
        private readonly UserManager<AppUser> _userManager;

        [TempData]
        public string? StatusMessage { get; set; }

        public ClipsController(IRepositoryClip repo, ILogger<ClipsController> logger, UserManager<AppUser> userManager)
        {
            _repo = repo;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: Clips
        [HttpGet]
        public ActionResult Index(string sortOrder, int? currentPage, int? pageSize)
        {
            var qrClips = _repo.GetClips();
            //sort
            ViewData["SortbyName"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["SortbyTimeCreated"] = sortOrder == "TimeCreated" ? "TimeCreated_desc" : "TimeCreated";

            switch (sortOrder)
            {
                case "name_desc":
                    qrClips = qrClips.OrderByDescending(c => c.Name);
                    break;
                case "TimeCreated":
                    qrClips = qrClips.OrderBy(c => c.TimeCreated);
                    break;
                case "TimeCreated_desc":
                    qrClips = qrClips.OrderByDescending(c => c.TimeCreated);
                    break;
                default:
                    qrClips = qrClips.OrderBy(c => c.Name);
                    break;
            }
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
            var clip = await _repo.GetClipByIdAsync(id);
            if (clip == null) { return NotFound($"Not Found Clip has id ={id}"); }
            return View(clip);
        }

        // GET: Clips/Create
        public ActionResult Create() { return View(); }
      
        // POST: Clips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Name,Description,Url")] Clip clipVM)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                clipVM.AuthorUser = currentUser;
                clipVM.AuthorUsername = currentUser.UserName;
                _repo.CreateClip(clipVM);
                await _repo.SaveAsync();
                StatusMessage = $"Uploaded clip Name :  {clipVM.Name} Successfully!";
            }
            return RedirectToAction("Index");
        }

        // GET: Clips/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var clip = await _repo.GetClipByIdAsync(id);
            if (clip == null) { return NotFound($"Not Found Clip has id ={id}"); }
            return View(clip);
        }

        // POST: Clips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Name,Description,Url")] Clip clipVM)
        {
            if (ModelState.IsValid)
            {
                var newClip = await _repo.GetClipByIdAsync(clipVM.Id);
                newClip.Name = clipVM.Name;
                newClip.Description = clipVM.Description;
                newClip.Url = clipVM.Url;
                _repo.UpdateClip(newClip);
                await _repo.SaveAsync();
                StatusMessage = $"Edited Clip name : {clipVM.Name} Successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Clips/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var clip =await _repo.GetClipByIdAsync(id);
            if (clip == null) { return NotFound($"Not Found Clip has id ={id}"); }
            return View(clip);
        }

        // POST: Clips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteClipAsync(id); 
            await _repo.SaveAsync();
            StatusMessage = $"Deleted clip successfully!";
            return RedirectToAction(nameof(Index));
        }

    }
}
