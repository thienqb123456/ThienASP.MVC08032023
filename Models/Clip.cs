
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThienASPMVC08032023.Models
{
    public class Clip
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Clip title")]
        public string? Name { get; set; }

        
        public string? AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public AppUser? AuthorUser { get; set; }

        public string? AuthorUsername { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Url { get; set; }

        public List<Comment>? Comments { get; set; }

        public DateTime TimeCreated { get; set; } = DateTime.Now;   
    }
}
