using System.ComponentModel.DataAnnotations;

namespace ThienASPMVC08032023.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        public List<Clip>? Clips { get; set; }
    }
}
