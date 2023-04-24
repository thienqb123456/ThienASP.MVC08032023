namespace ThienASPMVC08032023.Repository.InterfaceRepo
{
    public interface IRepositoryWrapper
    {
        IClipRepository ClipRepo { get; }

        ICategoryRepository CategoryRepo { get; }

        ICommentRepository CommentRepo { get; }

        Task SaveAsync();
    }
}
