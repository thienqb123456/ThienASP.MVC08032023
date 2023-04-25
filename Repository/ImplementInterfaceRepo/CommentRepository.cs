using Microsoft.EntityFrameworkCore;
using ThienASPMVC08032023.Database;
using ThienASPMVC08032023.Models;
using ThienASPMVC08032023.Repository.InterfaceRepo;

namespace ThienASPMVC08032023.Repository.Repo
{
    public class CommentRepository : RepositoryBase<MainComment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<MainComment> GetCommentByIdAsync(int id)
        {
            var comment = await FindByCondition(cmt => cmt.Id == id)
                                                .Include(cmt=>cmt.Clip)
                                                .Include(cmt=>cmt.User)
                                                .FirstOrDefaultAsync();
            return comment!;
        }

        public void CreateComment(MainComment comment)
        {
            Create(comment);
        }
        public void UpdateComment(MainComment comment)
        {
            Update(comment);
        }
        public void DeleteComment(MainComment comment)
        {
            Delete(comment);
        }


    }
}
