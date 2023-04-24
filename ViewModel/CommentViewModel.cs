using System.ComponentModel.DataAnnotations;

namespace ThienASPMVC08032023.ViewModel
{
    public class CommentViewModel
    {
        [Required]
        public int ClipId { get; set; }

        [Required]
        public string? CommentMsg { get; set; }

        [Required]
        public int MainCommentId { get; set; }
    }
}
