using ThienASPMVC08032023.Models;

namespace ThienASPMVC08032023.Repository
{
    public interface IRepositoryClip
    {
        IQueryable<Clip> GetClips();
        Task<Clip> GetClipByIdAsync(int id);

        void CreateClip(Clip clip);

        void UpdateClip(Clip clip);

        Task DeleteClipAsync(int id);

        Task SaveAsync();
    }
}
