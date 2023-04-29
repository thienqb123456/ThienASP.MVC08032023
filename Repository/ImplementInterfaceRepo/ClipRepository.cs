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

        public async Task<IEnumerable<Clip>> GetAllClipsAsync(string searchString, string sortBy)
        {
            var clipsQr = FindAll().Include(cl => cl.AuthorUser)
                                  .Include(cl => cl.Category)
                                  .Include(cl => cl.MainComments)
                                  .OrderByDescending(cl => cl.TimeCreated)
                                  .AsQueryable();
            if(!string.IsNullOrEmpty(searchString))
            {
                clipsQr = clipsQr.Where(cl => cl.Name!.Contains(searchString)
                                  ||cl.Description!.Contains(searchString) 
                                  ||cl.Category!.Name!.Contains(searchString));
            }

            if(!string.IsNullOrEmpty(sortBy))
            {
                switch(sortBy)
                {
                    case "name":
                        clipsQr = clipsQr.OrderBy(cl => cl.Name);
                        break;
                    case "name_desc":
                        clipsQr = clipsQr.OrderByDescending(cl => cl.Name);
                        break;
                    case "timeCreated":
                        clipsQr = clipsQr.OrderBy(cl => cl.TimeCreated);
                        break;
                    default:
                        clipsQr = clipsQr.OrderByDescending(cl => cl.TimeCreated);
                        break;
                }
            }

            return await clipsQr.ToListAsync();
        }

        public async Task<Clip> GetClipByIdAsync(int clipId)
        {
            var clip = await FindByCondition(cl => cl.Id == clipId)
                                            .Include(cl => cl.AuthorUser!)
                                            .Include(cl => cl.Category)
                                            .Include(cl => cl.MainComments).ThenInclude(cmt=> cmt.User)
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
