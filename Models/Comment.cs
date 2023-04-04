using System.ComponentModel.DataAnnotations;

namespace ThienASPMVC08032023.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? CommentMsg { get; set; }

        public DateTime DateTime { get; set; }
    }
}
