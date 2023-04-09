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

        public async Task<ActionResult> Detail(int? Id)
        {
            if (Id == 0 || Id == null)
            {
                return NotFound("Not found id ");
            }
            if (_context.Clips == null)
            {
                return NotFound("Not found any clip in DB");
            }
            var clip = await _context.Clips.Include(c => c.MainComments)
                                            .FirstOrDefaultAsync(c => c.Id == Id);

            if (clip == null)
            {
                return NotFound($"Not found clip has id = {Id}");
            }

            return View(clip);
        }

        [Authorize]
        public ActionResult Upload()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Upload([Bind("Id,Name,Description,Url,TimeCreated,AuthorId,AuthorUsername")] Clip clip)
        {
            if (ModelState.IsValid)
            {
                AppUser currentUser = await _userManager.GetUserAsync(User);
                clip.AuthorUser = currentUser;
                clip.AuthorUsername = currentUser.UserName;

                _context.Add(clip);
                await _context.SaveChangesAsync();

                StatusMessage = $"Uploaded clip Name :  {clip.Name} Successfully!";
                return RedirectToAction(nameof(Index));
            }

            _context.Add(clip);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<ActionResult> Comment(CommentViewModel commentInput)
        {
            AppUser currentUser = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                RedirectToAction("Detail", "Home", new { id = commentInput });
            }

            if (_context.Clips == null) { return NotFound("Notfound any clip in DB"); }

            var clip = await _context.Clips.FirstOrDefaultAsync(c=> c.Id == commentInput.ClipId);
            
            if (clip == null) { return NotFound($"Not found clip has Id = {commentInput.ClipId}"); }

            if (commentInput.MainCommentId == 0)
            {
                clip.MainComments = clip.MainComments ?? new List<MainComment>();
                clip.MainComments.Add( new MainComment
                {
                    CommentMsg = commentInput.CommentMsg,
                    User = currentUser,
                    UserName = currentUser.UserName,
                    ClipId = clip.Id

                });
                _context.Update(clip);
            } 
            //else
            //{
            //    var subComment = new SubComment
            //    {
            //        MainCommentId = commentInput.MainCommentId,
            //        CommentMsg = commentInput.CommentMsg,
            //        User = currentUser,
            //        UserName = currentUser.UserName
            //    };
                
            //    _context.Update(subComment);
            //}

            await _context.SaveChangesAsync();

            return RedirectToAction("Detail", "Home", new {id = commentInput.ClipId});
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Edit(int? Id)
        {
            if (Id == null) { return NotFound("Not found clipId"); }
            if (_context.Clips == null) {return NotFound("Not found any Clip in db");
            }
            var clip = await _context.Clips.FirstOrDefaultAsync(c => c.Id == Id);
            if (clip == null) { return NotFound($"Not found clip has id = {Id}"); }

            var currentUser = await _userManager.GetUserAsync(User);
            

            if (clip.AuthorUser != currentUser  )
            {
                return NotFound("U do not have permisson to access ");
            }
            

            return View(clip);

        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Edit([Bind("Id,Name,Description,Url,TimeCreated,AuthorId,AuthorUsername")] Clip clipModel)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                clipModel.AuthorUser = currentUser;
                clipModel.AuthorUsername = currentUser.UserName;

                _context.Update(clipModel);
                await _context.SaveChangesAsync();
                StatusMessage = $"Updated Clip has name : {clipModel.Name} Successfully!";
            }

            return RedirectToAction("Detail", "Home", new { id = clipModel.Id });
        }


        [Authorize]
        [HttpGet]
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
        [HttpPost]
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