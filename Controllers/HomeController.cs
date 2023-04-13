using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Diagnostics;
using ThienASPMVC08032023.Database;
using ThienASPMVC08032023.Models;
using ThienASPMVC08032023.ViewModel;
using X.PagedList;

namespace ThienASPMVC08032023.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        
        public HomeController(UserManager<AppUser> userManager, ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string? StatusMessage { get; set; }

        
        public async Task <IActionResult> Index(string searchString)
        {
            if (_context.Clips == null)
            {
                return NotFound("Not Found any Clip in DB");
            }
            List<Clip> clips = new List<Clip>();

            var qr = from c in _context.Clips
                     orderby c.TimeCreated descending
                     select c;

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
            var clip = await _context.Clips.Include(c => c.MainComments)
                                            .FirstOrDefaultAsync(c => c.Id == id);

            if (clip == null)
            {
                return NotFound($"Not found clip has id = {id}");
            }

            return View(clip);
        }

        [Authorize]
        [HttpGet("{action}")]
        public ActionResult Upload()
        {
            return View();
        }

        [Authorize]
        [HttpPost("{action}")]
        public async Task<ActionResult> Upload([Bind("Id,Name,Description,Url,TimeCreated,AuthorId,AuthorUsername,AuthorUser")] Clip clipModel)
        {
            AppUser currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                clipModel.AuthorUser = currentUser;
                clipModel.AuthorUsername = currentUser.UserName;

                _context.Add(clipModel);
                await _context.SaveChangesAsync();
                StatusMessage = $"Uploaded clip Name :  {clipModel.Name} Successfully!";
            }

            return RedirectToAction("Detail", "Home", new { id = clipModel.Id });
        }


        [Authorize]
        [HttpGet("/clip/{action}/{id}")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) return NotFound("Not found clipId");
            if (_context.Clips == null) return NotFound("Not found any Clip in db");
            
            var clip = await _context.Clips.FirstOrDefaultAsync(c => c.Id == id);
            if (clip == null) return NotFound($"Not found clip has id = {id}"); 

            var currentUser = await _userManager.GetUserAsync(User);
            

            if (clip.AuthorUser != currentUser  ) return NotFound("U do not have permisson to access ");
            

            return View(clip);

        }

        [Authorize]
        [HttpPost("/clip/{action}/{id}")]
        public async Task<ActionResult> Edit([Bind("Id,Name,Description,Url")] Clip clipModel)
        {
            if (_context.Clips == null) return BadRequest("Not found any clip in db");
            var clip = await _context.Clips.SingleOrDefaultAsync(c => c.Id== clipModel.Id);
            if (clip == null) return NotFound($"Not found clip has id = {clipModel.Id}");

            clip.Name = clipModel.Name;
            clip.Description = clipModel.Description;
            clip.Url = clipModel.Url;
            clip.TimeCreated = clipModel.TimeCreated;

            _context.Update(clip);
            await _context.SaveChangesAsync();
            StatusMessage = $"Updated Clip has name : {clipModel.Name} Successfully!";
            return RedirectToAction("Detail", "Home", new { id = clipModel.Id });
        }


        [Authorize]
        [HttpGet("/clip/{action}/{id}")]
        public async Task<ActionResult> Delete(int? Id)
        {

            if (Id == null) { return NotFound("Not found clipId"); }
            if (_context.Clips == null)
            {
                return NotFound("Not found any Clip in db");
            }
            var clip = _context.Clips.FirstOrDefault(c => c.Id == Id);
            if (clip == null) { return NotFound($"Not found clip has id = {Id}"); }

            var currentUser = await _userManager.GetUserAsync(User);

            if (clip.AuthorUser != currentUser)
            {
                return NotFound("U do not have permisson to access ");
            }

            return View(clip);
        }

        [Authorize]
        [HttpPost("/clip/{action}/{Id}")]
        public async Task<ActionResult> DeleteConfirm(int? Id)
        {
            if (Id == null) { return NotFound("Not found clipId"); }
            if (_context.Clips == null)
            {
                return NotFound("Not found any Clip in db");
            }
            var clip = await _context.Clips.FirstOrDefaultAsync(c => c.Id == Id);
            if (clip == null) { return NotFound($"Not found clip has id = {Id}"); }

            var currentUser = await _userManager.GetUserAsync(User);

            if (clip.AuthorUser != currentUser)
            {
                return NotFound("U do not have permisson to access ");
            } else
            {
                _context.Clips.Remove(clip);
                await _context.SaveChangesAsync();
                StatusMessage = $"Deleted Clip {clip?.Name} Successfully!";
            }

            return RedirectToAction("Index");
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