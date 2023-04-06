using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Diagnostics;
using ThienASPMVC08032023.Database;
using ThienASPMVC08032023.Models;
using ThienASPMVC08032023.ViewModel;

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

        public async Task<IActionResult> Index()
        {
            if (_context.Clips == null)
            {
                return NotFound("Not Found any Clip in DB");
            }
            var clips = await _context.Clips.ToListAsync();
            return View(clips);
        }

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
            } else
            {
                var subComment = new SubComment
                {
                    MainCommentId = commentInput.MainCommentId,
                    CommentMsg = commentInput.CommentMsg,
                    User = currentUser,
                    UserName = currentUser.UserName
                };
                
                _context.Update(subComment);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Detail", "Home", new {id = commentInput.ClipId});
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