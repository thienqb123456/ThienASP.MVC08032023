using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThienASPMVC08032023.Database;
using ThienASPMVC08032023.Models;
using ThienASPMVC08032023.ViewModel;

namespace ThienASPMVC08032023.Controllers
{
    public class CommentController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public CommentController(UserManager<AppUser> userManager, AppDbContext context, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [Authorize]
        public async Task<ActionResult> Create(CommentViewModel commentInput)
        {
            AppUser currentUser = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                RedirectToAction("Detail", "Home", new { id = commentInput.ClipId });
            }

            if (_context.Clips == null) { return NotFound("Notfound any clip in DB"); }

            var clip = await _context.Clips.FirstOrDefaultAsync(c => c.Id == commentInput.ClipId);

            if (clip == null) { return NotFound($"Not found clip has Id = {commentInput.ClipId}"); }
            
            if (commentInput.MainCommentId == 0)
            {
                clip.MainComments = clip.MainComments ?? new List<MainComment>();
                clip.MainComments.Add(new MainComment
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

            return RedirectToAction("Detail", "Home", new { id = commentInput.ClipId });
        }


        // GET: CommentController1/Edit/5
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if(id == 0 || id == null) { return NotFound("Not found mainComment id"); }
            if(_context.MainComments == null) { return NotFound("Not found any main comment in db"); }
            var mainComment =await _context.MainComments.FirstOrDefaultAsync(c => c.Id == id);
            if(mainComment == null)
            {
                return NotFound($"Not Found comment has id = {id}");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != mainComment.User )
            {
                return NotFound("U do not allow to edit this comment");
            }

            return View(mainComment);
        }

        //POST: CommentController1/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MainComment mainComment)
        {
            if (!ModelState.IsValid)
            {
                RedirectToAction("Detail", "Home", new { id = mainComment.ClipId });
            }

            _context.Update(mainComment);

            await _context.SaveChangesAsync();

            return RedirectToAction("Detail", "Home", new { id = mainComment.ClipId });

        }

        // GET: CommentController1/Delete/5
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == 0 || id == null) { return NotFound("Not found mainComment id"); }
            if (_context.MainComments == null) { return NotFound("Not found any main comment in db"); }
            var mainComment = await _context.MainComments.FirstOrDefaultAsync(c => c.Id == id);
            if(mainComment == null) { return NotFound($"Not found comment has id = {id}"); }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != mainComment.User)
            {
                return NotFound("U do not allow to Delete this comment");
            }
            return View(mainComment);
        }

        // POST: CommentController1/Delete/5
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> DeleteConfirm(int? id)
        {
            if (id == 0 || id == null) { return NotFound("Not found mainComment id"); }
            if (_context.MainComments == null) { return NotFound("Not found any main comment in db"); }
            var mainComment = await _context.MainComments.FirstOrDefaultAsync(c => c.Id == id);
            if(mainComment == null) { return NotFound($"Not found comment has id = {id}"); }
            _context.MainComments.Remove(mainComment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Detail", "Home", new { id = mainComment.ClipId });
        }
    }
}
