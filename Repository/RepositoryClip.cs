using Microsoft.EntityFrameworkCore;
using ThienASPMVC08032023.Database;
using ThienASPMVC08032023.Models;

namespace ThienASPMVC08032023.Repository
{
    public class RepositoryClip : IRepositoryClip
    {
        private AppDbContext _context;

        public RepositoryClip(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Clip> GetClips()
        {
            var clips = from clip in _context.Clips
                        select clip;
            return clips;
        }

        public async Task<Clip> GetClipByIdAsync(int id)
        {
            var clip = await _context.Clips!.Include( c=> c.MainComments)
                                            .FirstOrDefaultAsync(c => c.Id == id);
            return clip!;
        }

        public void CreateClip(Clip clip)
        {
            _context.Add(clip);
        }
        public void UpdateClip(Clip clip)
        {
            _context.Update(clip);
        }
        public async Task DeleteClipAsync(int id)
        {
            var clip = await  _context.Clips!.FirstOrDefaultAsync(c => c.Id == id);
            _context.Clips!.Remove(clip!);
        }

        public async Task SaveAsync()
        {
             await _context.SaveChangesAsync();
        }

    
    }
}
