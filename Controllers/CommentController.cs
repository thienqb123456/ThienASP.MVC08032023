using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThienASPMVC08032023.Models;
using ThienASPMVC08032023.Repository.InterfaceRepo;
using ThienASPMVC08032023.ViewModel;

namespace ThienASPMVC08032023.Controllers
{
    public class CommentController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRepositoryWrapper _repo;

        public CommentController(IRepositoryWrapper repo,UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _repo = repo;
        }

        [Authorize]
        public async Task<ActionResult> Create(CommentViewModel commentInput)
        {
            AppUser currentUser = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                RedirectToAction("Detail", "Home", new { id = commentInput.ClipId });
            }
            var clip = await _repo.ClipRepo.GetClipByIdAsync(commentInput.ClipId);

            if (clip == null) { return NotFound($"Not found clip has Id = {commentInput.ClipId}"); }
            
            if (commentInput.MainCommentId == 0)
            {
                clip.MainComments = clip.MainComments ?? new List<MainComment>();
                clip.MainComments.Add(new MainComment
                {
                    CommentMsg = commentInput.CommentMsg,
                    User = currentUser,
                    ClipId = clip.Id
                }); ;
                _repo.ClipRepo.UpdateClip(clip);
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

            await _repo.SaveAsync();

            return RedirectToAction("Detail", "Home", new { id = commentInput.ClipId });
        }


        // GET: CommentController1/Edit/5
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var mainComment = await _repo.CommentRepo.GetCommentByIdAsync(id);
            if(mainComment == null){ return NotFound($"Not Found comment has id = {id}");}
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
            _repo.CommentRepo.Update(mainComment);
            await _repo.SaveAsync();
            return RedirectToAction("Detail", "Home", new { id = mainComment.ClipId });
        }

        // GET: CommentController1/Delete/5
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var mainComment = await _repo.CommentRepo.GetCommentByIdAsync(id);
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
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            var mainComment = await _repo.CommentRepo.GetCommentByIdAsync(id);
            if(mainComment == null) { return NotFound($"Not found comment has id = {id}"); }
            _repo.CommentRepo.DeleteComment(mainComment);
            await _repo.SaveAsync();
            return RedirectToAction("Detail", "Home", new { id = mainComment.ClipId });
        }
    }
}
