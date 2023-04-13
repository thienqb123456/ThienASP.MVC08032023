using System.ComponentModel.DataAnnotations;

namespace ThienASPMVC08032023.ViewModel
{
    public class ClipViewModel
    {

        [Required]
        public string Name { get; set; }
      
        [Required]
        public string Description { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
