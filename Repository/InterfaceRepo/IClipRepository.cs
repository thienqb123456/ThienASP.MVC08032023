using ThienASPMVC08032023.Models;

namespace ThienASPMVC08032023.Repository.InterfaceRepo
{
    public interface IClipRepository
    {
        Task<IEnumerable<Clip>> GetAllClipsAsync(string searchString, string sortBy);
        Task<Clip> GetClipByIdAsync(int clipId);

        void CreateClip(Clip clip);
        void UpdateClip(Clip clip);
        void DeleteClip(Clip clip);
    }
}
