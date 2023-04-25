using ThienASPMVC08032023.Database;
using ThienASPMVC08032023.Repository.InterfaceRepo;

namespace ThienASPMVC08032023.Repository.Repo
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly AppDbContext _context;
        private readonly IClipRepository _clipRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ICommentRepository _commentRepo;

        public IClipRepository ClipRepo { get { return _clipRepo; } }

        public ICategoryRepository CategoryRepo { get { return _categoryRepo; } }

        public ICommentRepository CommentRepo { get { return _commentRepo; } }


        public RepositoryWrapper(AppDbContext context)
        {
            _context = context;
            _clipRepo = new ClipRepository(_context);
            _categoryRepo = new CategoryRepository(_context);
            _commentRepo = new CommentRepository(_context);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
