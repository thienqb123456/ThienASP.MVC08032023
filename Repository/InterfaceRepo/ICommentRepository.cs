using ThienASPMVC08032023.Models;

namespace ThienASPMVC08032023.Repository.InterfaceRepo
{
    public interface ICommentRepository : IRepositoryBase<MainComment>
    {
        Task<MainComment> GetCommentByIdAsync(int id);

        void CreateComment(MainComment comment);
        void UpdateComment(MainComment comment);

        void DeleteComment(MainComment comment);

    }
}
