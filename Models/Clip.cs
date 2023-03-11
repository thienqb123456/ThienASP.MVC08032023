using System.ComponentModel.DataAnnotations;

namespace ThienASPMVC08032023.Models
{
    public class Clip
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="bắt buộc nhập")]
        [Display(Name = "Clip title")]
        public string Name { get; set; }

        [Required(ErrorMessage = "bat buoc nhap")]
        [Display(Name = "Clip Author")]
        public string Author { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Url { get; set; }

        public DateTime TimeCreated { get; set; } = DateTime.Now;
    }
}
