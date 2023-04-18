using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ThienASPMVC08032023.Database;
using ThienASPMVC08032023.Models;
using ThienASPMVC08032023.Repository;
using X.PagedList;

namespace ThienASPMVC08032023.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositoryClip _repo;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(IRepositoryClip repo, UserManager<AppUser> userManager, ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _repo = repo;
        }

        [TempData]
        public string? StatusMessage { get; set; }

        public async Task <ActionResult> Index(string searchString)
        {
            List<Clip> clips = new List<Clip>();
            var qr = _repo.GetClips();
            if (!string.IsNullOrEmpty(searchString))
            {
                clips = qr.Where(c => c.Name!.Contains(searchString)
                           || c.Description!.Contains(searchString)).ToList();
            } else
            {
                clips = await qr.ToListAsync();
            }

            return View(clips);
        }

        [HttpGet("/clip/{id}")]
        public async Task<ActionResult> Detail(int id)
        {
            var clip = await _repo.GetClipByIdAsync(id);
            if (clip == null) { return NotFound($"Not found clip has id = {id}"); }
            return View(clip);
        }

        [Authorize]
        [HttpGet("{action}")]
        public ActionResult Upload() { return View(); }
       
        [Authorize]
        [HttpPost("{action}")]
        public async Task<ActionResult> Upload([Bind("Id,Name,Description,Url")] Clip clipVM)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            clipVM.AuthorUser = currentUser;
            clipVM.AuthorUsername = currentUser.UserName;
            if (ModelState.IsValid)
            {
                _repo.CreateClip(clipVM);
                await _repo.SaveAsync();
                StatusMessage = $"Uploaded clip Name :  {clipVM.Name} Successfully!";
            }
            return RedirectToAction("Detail", "Home", new { id = clipVM.Id });
        }


        [Authorize]
        [HttpGet("/clip/{action}/{id}")]
        public async Task<ActionResult> Edit(int id)
        {
            var clip = await _repo.GetClipByIdAsync(id);
            if(clip == null) { return NotFound($"Not Found Clip has id ={id}"); }
            var currentUser = await _userManager.GetUserAsync(User);
            if (clip.AuthorUser != currentUser  ) return NotFound("U do not have permisson to access ");
            return View(clip);

        }

        [Authorize]
        [HttpPost("/clip/{action}/{id}")]
        public async Task<ActionResult> Edit([Bind("Id,Name,Description,Url")] Clip clipVM)
        {
            var newClip = await _repo.GetClipByIdAsync(clipVM.Id);
            if (ModelState.IsValid && newClip != clipVM)
            {
                newClip.Name = clipVM.Name;
                newClip.Description = clipVM.Description;
                newClip.Url = clipVM.Url;
                _repo.UpdateClip(newClip);
                await _repo.SaveAsync();
                StatusMessage = $"Updated Clip has name : {clipVM.Name} Successfully!";
            }
            return RedirectToAction("Detail", "Home", new { id = clipVM.Id });
        }


        [Authorize]
        [HttpGet("/clip/{action}/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var clip = await _repo.GetClipByIdAsync(id);
            if (clip == null) { return NotFound($"Not Found Clip has id ={id}"); }
            var currentUser = await _userManager.GetUserAsync(User);
            if (clip.AuthorUser != currentUser)
            {
                return NotFound("U do not have permisson to access ");
            }
            return View(clip);
        }

        [Authorize]
        [HttpPost("/clip/{action}/{Id}")]
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            var clip = await _repo.GetClipByIdAsync(id);
            if (clip == null) { return NotFound($"Not Found Clip has id ={id}"); }
            var currentUser = await _userManager.GetUserAsync(User);
            if (clip.AuthorUser != currentUser)
            {
                return NotFound("U do not have permisson to access ");
            } else
            {
                await _repo.DeleteClipAsync(id);
                await _repo.SaveAsync();
                StatusMessage = $"Deleted Clip {clip?.Name} Successfully!";
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}