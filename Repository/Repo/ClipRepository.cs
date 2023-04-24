using Microsoft.EntityFrameworkCore;
using ThienASPMVC08032023.Database;
using ThienASPMVC08032023.Models;
using ThienASPMVC08032023.Repository.InterfaceRepo;

namespace ThienASPMVC08032023.Repository.Repo
{
    public class ClipRepository : RepositoryBase<Clip>, IClipRepository
    {
        public ClipRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Clip>> GetAllClipsAsync()
        {
            return await FindAll().Include(cl => cl.AuthorUser)
                                  .Include(cl => cl.Category)
                                  .Include(cl=> cl.MainComments)
                                  .OrderByDescending(cl => cl.TimeCreated)
                                  .ToListAsync();
        }

        public async Task<Clip> GetClipByIdAsync(int clipId)
        {
            var clip = await FindByCondition(cl => cl.Id == clipId)
                                            .Include(cl => cl.AuthorUser!)
                                            .Include(cl => cl.Category)
                                            .Include(cl => cl.MainComments)
                                            .FirstOrDefaultAsync();
            return clip!;
        }

        public void CreateClip(Clip clip)
        {
            Create(clip);
        }

        public void UpdateClip(Clip clip)
        {
            Update(clip);
        }

        public void DeleteClip(Clip clip)
        {
            Delete(clip);
        }

    }
}
