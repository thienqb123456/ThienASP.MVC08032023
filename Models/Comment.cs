using System.ComponentModel.DataAnnotations;

namespace ThienASPMVC08032023.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? CommentMsg { get; set; }

        public string? UserName { get; set; }


        [Required]
        public AppUser? User { get; set; }

        public DateTime TimeCreated { get; set; } = DateTime.Now;
    }
}
